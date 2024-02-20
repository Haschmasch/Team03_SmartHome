using MainUnit.Models;

namespace MainUnit.Services.Interfaces
{
    public interface IRoomService
    {
        public void AddRoom(Room room);
        public void RemoveRoom(int Id);
        public void UpdateRoom(Room room);
        public Room GetRoom(int Id);
        public void AddThermostat(int roomId, int thermostatId);
        public void SetRoomTemperature(int roomId, double temperature);
    }
}
