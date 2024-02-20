namespace MainUnit.Models
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public List<Thermostat> Thermostats { get; set; }
        public double Temperature { get; set; }

        public Room(int id, string name, string? description, double temperature)
        {
            Id = id;
            Name = name;
            Description = description;
            Thermostats = new List<Thermostat>();
            Temperature = temperature;
        }
    }
}
