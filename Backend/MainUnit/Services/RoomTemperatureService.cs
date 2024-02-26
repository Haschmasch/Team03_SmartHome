using MainUnit.Models.Exceptions;
using MainUnit.Models.RoomTemperature;
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

        public IList<RoomTemperatureEntry> GetTemperatureEntries(DateTime start, DateTime end)
        {
            return _roomTemperatureEntries.Find(t => t.Timestamp >= start && t.Timestamp <= end).ToList();
        }

        public IList<RoomTemperatureEntry> GetTemperatureEntriesByRoom(string roomId, DateTime start, DateTime end)
        {
            try
            {
                return _roomTemperatureEntries.Find(t => t.Timestamp >= start && t.Timestamp <= end && t.Metadata.Id == roomId).ToList();
            }
            catch (FormatException ex)
            {
                throw new InvalidIdException($"No room found for id:'{roomId}'", ex);
            }
        }

        public IList<RoomTemperatureEntry> GetTemperatureEntriesByThermostat(string thermostatId, DateTime start, DateTime end)
        {
            try
            {
                return _roomTemperatureEntries.Find(t => t.Timestamp >= start && t.Timestamp <= end && t.Metadata.ThermostatIds.Contains(thermostatId)).ToList();
            }
            catch (FormatException ex)
            {
                throw new InvalidIdException($"No thermostat found for id:'{thermostatId}'", ex);
            }
        }
    }
}
