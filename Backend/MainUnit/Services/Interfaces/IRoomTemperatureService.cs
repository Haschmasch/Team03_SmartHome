using MainUnit.Models;

namespace MainUnit.Services.Interfaces
{
    public interface IRoomTemperatureService
    {
        public List<RoomTemperatureEntry> GetTemperatureEntries(DateTime start, DateTime end);
        public List<RoomTemperatureEntry> GetTemperatureEntriesByRoom(int roomId, DateTime start, DateTime end);
        public List<RoomTemperatureEntry> GetTemperatureEntriesByThermostat(int thermostatId, DateTime start, DateTime end);
    }
}
