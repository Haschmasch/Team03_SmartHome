using MainUnit.Models;

namespace MainUnit.Services.Interfaces
{
    public interface IThermostatService
    {
        public void AddThermostat(ThermostatWithURL thermostat);
        public Thermostat GetThermostat(int id);
    }
}
