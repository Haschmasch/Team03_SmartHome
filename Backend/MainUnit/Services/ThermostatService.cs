using MainUnit.Models.Exceptions;
using MainUnit.Models.Settings;
using MainUnit.Models.Thermostat;
using MainUnit.Services.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace MainUnit.Services
{
    public class ThermostatService : IThermostatService
    {
        private readonly IMongoCollection<ThermostatWithURL> _thermostatCollection;


        public ThermostatService(IOptions<MongoDbSettings> settings)
        {
            var mongoClient = new MongoClient(
                settings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                settings.Value.DatabaseName);

            _thermostatCollection = mongoDatabase.GetCollection<ThermostatWithURL>(
                settings.Value.ThermostatCollectionName);
        }

        public void AddThermostat(ThermostatWithURL thermostat)
        {
            var result = _thermostatCollection.Find(t => t.Id == thermostat.Id);
            if(result == null || !result.Any()) 
            {
                _thermostatCollection.InsertOne(thermostat);
            }
            throw new ThermostatExistsException($"Thermostat with id: {thermostat.Id} already exists");
        }

        public List<Thermostat> GetThermostats(int skip, int limit)
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
    }
}
