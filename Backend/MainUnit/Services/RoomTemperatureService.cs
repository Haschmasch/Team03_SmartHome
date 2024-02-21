using MainUnit.Models;
using MainUnit.Models.Settings;
using MainUnit.Services.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace MainUnit.Services
{
    public class RoomTemperatureService : IRoomTemperatureService
    {
        private readonly IMongoCollection<RoomTemperatureEntry> _roomTemperatureEntries;

        public RoomTemperatureService(IOptions<MongoDbSettings> settings)
        {
            var mongoClient = new MongoClient(
                settings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                settings.Value.DatabaseName);

            _roomTemperatureEntries = mongoDatabase.GetCollection<RoomTemperatureEntry>(
                settings.Value.RoomTemperatureCollectionName);
        }

        public List<RoomTemperatureEntry> GetTemperatureEntries(DateTime start, DateTime end)
        {
            return _roomTemperatureEntries.Find(t => t.Timestamp >= start && t.Timestamp <= end).ToList();
        }

        public List<RoomTemperatureEntry> GetTemperatureEntriesByRoom(int roomId, DateTime start, DateTime end)
        {
            return _roomTemperatureEntries.Find(t => t.Timestamp >= start && t.Timestamp <= end && t.Metadata.Id == roomId).ToList();
        }

        public List<RoomTemperatureEntry> GetTemperatureEntriesByThermostat(int thermostatId, DateTime start, DateTime end)
        {
            return _roomTemperatureEntries.Find(t => t.Timestamp >= start && t.Timestamp <= end && t.Metadata.ThermostatIds.Contains(thermostatId)).ToList();
        }
    }
}
