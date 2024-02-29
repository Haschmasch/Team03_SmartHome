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

        public Thermostat AddThermostat(Thermostat thermostat)
        {
            thermostat.Id = ObjectId.GenerateNewId(Convert.ToInt32(thermostat.Id)).ToString();

            _thermostatCollection.InsertOne(thermostat);
            return _thermostatCollection.Find(t => t.Id == thermostat.Id).FirstOrDefault();
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

            if(result != null && result.Any())
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
    }
}
