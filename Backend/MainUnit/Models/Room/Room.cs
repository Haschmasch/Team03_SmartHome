namespace MainUnit.Models.Room
{
    public class Room : BaseRoom
    {
        public List<string> ThermostatIds { get; set; } = [];

        public float Temperature { get; set; } = 21;

        public Room()
        {
        }

        public Room(BaseRoom room)
        {
            Description = room.Description;
            Name = room.Name;
        }
    }
}
