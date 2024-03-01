using MainUnit.Models.Thermostat;

namespace MainUnit.Services.Interfaces
{
    public interface IThermostatService
    {
        public ThermostatWithURL AddThermostat(string url);
        public Thermostat GetThermostat(string id);
        public Thermostat GetThermostatByURL(string name);
        public IList<Thermostat> GetThermostats(int skip, int limit);
    }
}
