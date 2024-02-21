using MainUnit.Models;

namespace MainUnit.Services.Interfaces
{
    public interface IRoomService
    {
        public Room AddRoom(Room room);
        public void RemoveRoom(int id);
        List<Room> GetRooms(int skip, int limit);
        public Room UpdateRoom(Room room);
        public Room GetRoom(int id);
        public Room AddThermostat(int roomId, int thermostatId);
        public Room SetRoomTemperature(int roomId, float temperature);
    }
}
