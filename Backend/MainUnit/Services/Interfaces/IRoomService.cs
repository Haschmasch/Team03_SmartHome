using MainUnit.Models.Room;

namespace MainUnit.Services.Interfaces
{
    public interface IRoomService
    {
        public Room AddRoom(BaseRoom room);
        public void RemoveRoom(string id);
        List<Room> GetRooms(int skip, int limit);
        public Room UpdateRoom(BaseRoom room);
        public Room GetRoom(string id);
        public Room AddThermostat(string roomId, string thermostatId);
        public void RemoveThermostat(string roomId, string thermostatId);
        public Room SetRoomTemperature(string roomId, float temperature);
    }
}
