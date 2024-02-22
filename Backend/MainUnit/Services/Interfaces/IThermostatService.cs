using MainUnit.Models.Thermostat;

namespace MainUnit.Services.Interfaces
{
    public interface IThermostatService
    {
        public void AddThermostat(ThermostatWithURL thermostat);
        public Thermostat GetThermostat(string id);
        public List<Thermostat> GetThermostats(int skip, int limit);
    }
}
