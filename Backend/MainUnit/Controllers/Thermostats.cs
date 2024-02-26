using MainUnit.Models.Exceptions;
using MainUnit.Models.Thermostat;
using MainUnit.Services;
using MainUnit.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MainUnit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Thermostats : ControllerBase
    {
        private readonly IThermostatService _thermostatService;

        public Thermostats(IThermostatService thermostatService)
        {
            _thermostatService = thermostatService;
        }

        // GET: api/Thermostats?skip=0&limit=5
        [HttpGet]
        public ActionResult<List<Thermostat>> Get(int skip, int limit)
        {
            if (limit <= 0)
            {
                return BadRequest("The value of limit cannot be smaller than 1");
            }
            else if (skip < 0)
            {
                return BadRequest("The value of skip cannot be smaller than 0");
            }

            var thermostats = _thermostatService.GetThermostats(skip, limit);
            if (thermostats.Count == 0)
                return NotFound($"No Thermostats found for skip: {skip} and limit: {limit}");

            return Ok(thermostats);
        }

        // GET: api/Thermostats/507f1f77bcf86cd799439011
        [HttpGet("{id}")]
        public ActionResult<Thermostat> Get(string id)
        {
            try
            {
                var thermostat = _thermostatService.GetThermostat(id);
                return Ok(thermostat);
            }
            catch (ThermostatNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // POST: api/Thermostats
        [HttpPost]
        public ActionResult<Thermostat> Post([FromBody] ThermostatWithURL thermostatWithURL)
        {
            try
            {
                var result = _thermostatService.AddThermostat(thermostatWithURL);
                return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
            }
            catch (ThermostatNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            
        }
    }
}
