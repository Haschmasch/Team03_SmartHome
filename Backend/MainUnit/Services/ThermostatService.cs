using MainUnit.Models;
using MainUnit.Models.Settings;
using MainUnit.Services.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

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
            _thermostatCollection.InsertOne(thermostat);
        }

        public Thermostat GetThermostat(int id)
        {
            var result = _thermostatCollection.Find(t => t.Id == id);

            if(result != null && result.Any())
            {
                return result.FirstOrDefault();
            }
            return null;
        }
    }
}
