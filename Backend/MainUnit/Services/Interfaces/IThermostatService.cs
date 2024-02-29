using MainUnit.Models.Thermostat;

namespace MainUnit.Services.Interfaces
{
    public interface IThermostatService
    {
        public Thermostat AddThermostat(Thermostat thermostat);
        public Thermostat GetThermostat(string id);
        public Thermostat GetThermostatByName(string name);
        public IList<Thermostat> GetThermostats(int skip, int limit);
    }
}
