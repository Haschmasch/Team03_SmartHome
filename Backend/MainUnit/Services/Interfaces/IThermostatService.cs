using MainUnit.Models.Thermostat;

namespace MainUnit.Services.Interfaces
{
    public interface IThermostatService
    {
        public ThermostatWithURL AddThermostat(ThermostatWithURL thermostat);
        public Thermostat GetThermostat(string id);
        public IList<Thermostat> GetThermostats(int skip, int limit);
    }
}
