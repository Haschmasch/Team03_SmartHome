namespace MainUnit.Models
{
    public class Thermostat
    {
        public int Id { get; set; }
        public double Temperature {  get; set; }
        public string URL { get; set; }


        public Thermostat(int id, double temperature, string URL)
        {
            Id = id;
            Temperature = temperature;
            this.URL = URL;
        }
    }
}
