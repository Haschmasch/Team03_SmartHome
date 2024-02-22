using MainUnit.Models.RoomTemperature;

namespace MainUnit.Services.Interfaces
{
    public interface IRoomTemperatureService
    {
        public List<RoomTemperatureEntry> GetTemperatureEntries(DateTime start, DateTime end);
        public List<RoomTemperatureEntry> GetTemperatureEntriesByRoom(string roomId, DateTime start, DateTime end);
        public List<RoomTemperatureEntry> GetTemperatureEntriesByThermostat(string thermostatId, DateTime start, DateTime end);
    }
}
