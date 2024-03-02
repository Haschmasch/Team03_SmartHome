using Microsoft.AspNetCore.Mvc;
using Thermostat.Extensions;
using Thermostat.Services.Interfaces;

namespace Thermostat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Thermostat(IThermostatService _thermostatService) : Controller
    {
        // POST: api/Thermostat
        [HttpPost]
        public IActionResult SetTemperature(float temperature)
        {
            var result = _thermostatService.UpdateTemperature(temperature);

            return result.CheckForErrors();
        }
    }
}
