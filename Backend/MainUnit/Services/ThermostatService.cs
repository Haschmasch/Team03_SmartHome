using MainUnit.Models.Exceptions;
using MainUnit.Models.Settings;
using MainUnit.Models.Thermostat;
using MainUnit.Services.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace MainUnit.Services
{
    public class ThermostatService : IThermostatService
    {
        private readonly IMongoCollection<Thermostat> _thermostatCollection;


        public ThermostatService(IOptions<MongoDbSettings> settings)
        {
            var mongoClient = new MongoClient(
                settings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                settings.Value.DatabaseName);

            _thermostatCollection = mongoDatabase.GetCollection<Thermostat>(
                settings.Value.ThermostatCollectionName);
        }

        public ThermostatWithURL AddThermostat(ThermostatWithURL thermostat)
        {
            thermostat.Id = ObjectId.GenerateNewId(Convert.ToInt32(thermostat.Id)).ToString();
            
            if (!ValidateUrl(thermostat.URL))
                throw new UriFormatException($"A invalid URL was provided. URL:'{thermostat.URL}'.");

            var existingThermostat = _thermostatCollection.Find(t => t.URL == thermostat.URL).FirstOrDefault();
            if (existingThermostat != null)
                throw new ThermostatExistsException($"The thermostat with the URL:'{thermostat.URL}' already exists.");

            _thermostatCollection.InsertOne(thermostat);
            return thermostat;
        }

        public IList<Thermostat> GetThermostats(int skip, int limit)
        {
            var result = _thermostatCollection.AsQueryable();
            result = result.Skip(skip).Take(limit);

            return new List<Thermostat>(result.ToList());
        }

        public Thermostat GetThermostat(string id)
        {
            var result = _thermostatCollection.Find(t => t.Id == id);

            if (result != null && result.Any())
            {
                return result.FirstOrDefault();
            }
            throw new ThermostatNotFoundException($"Thermostat with id: {id} not found");
        }

        public Thermostat GetThermostatByName(string name)
        {
            var result = _thermostatCollection.Find(t => t.Name == name);

            if(result != null && result.Any())
            {
                return result.FirstOrDefault();
            }
            throw new ThermostatNotFoundException($"Thermostat with id: {name} not found");
        }

        private bool ValidateUrl(string url)
        {
            if (Uri.TryCreate(url, UriKind.Absolute, out Uri validatedUri))
            {
                return (validatedUri.Scheme == Uri.UriSchemeHttp || validatedUri.Scheme == Uri.UriSchemeHttps);
            }
            return false;
        }
    }
}
