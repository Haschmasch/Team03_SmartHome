using MainUnit.Models;
using MainUnit.Models.Exceptions;
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
            var thermostats = _thermostatService.GetThermostats(skip, limit);
            if (thermostats.Count == 0)
                return NotFound($"No Thermostats found for skip: {skip} and limit: {limit}");

            return Ok(thermostats);
        }

        // GET: api/Thermostats/5
        [HttpGet("{id}")]
        public ActionResult<Thermostat> Get(int id)
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
                _thermostatService.AddThermostat(thermostatWithURL);
                return CreatedAtAction(nameof(Get), new { id = thermostatWithURL.Id }, thermostatWithURL);
            }
            catch (ThermostatNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            
        }
    }
}
