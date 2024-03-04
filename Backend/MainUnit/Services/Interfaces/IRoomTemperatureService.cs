using MainUnit.Models.RoomTemperature;

namespace MainUnit.Services.Interfaces
{
    public interface IRoomTemperatureService
    {
        public IList<RoomTemperatureEntry> GetTemperatureEntries(DateTime start, DateTime end);
        public IList<RoomTemperatureEntry> GetTemperatureEntriesByRoom(string roomId, DateTime start, DateTime end);
        public IList<RoomTemperatureEntry> GetTemperatureEntriesByThermostat(string thermostatId, DateTime start, DateTime end);
        public IList<RoomTemperatureEntry> GetTemperatureEntriesByRoomAndThermostat(string thermostatId, string roomId, DateTime start, DateTime end);
    }
}
