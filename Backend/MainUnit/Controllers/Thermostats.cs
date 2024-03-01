using MainUnit.Models.Exceptions;
using MainUnit.Models.Thermostat;
using MainUnit.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System.Net.Http;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MainUnit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Thermostats : ControllerBase
    {
        private readonly IThermostatService _thermostatService;
        private readonly ILogger _logger;


        public Thermostats(IThermostatService thermostatService, ILogger<Thermostats> logger)
        {
            _thermostatService = thermostatService;
            this._logger = logger;
        }

        // GET: api/Thermostats?skip=0&limit=5
        [HttpGet]
        public ActionResult<IList<Thermostat>> Get(int skip, int limit)
        {
            string message;
            if (limit <= 0)
            {
                message = "The value of limit cannot be smaller than 1";
                _logger.LogError(message);
                return BadRequest(message);
            }
            else if (skip < 0)
            {
                message = "The value of skip cannot be smaller than 0";
                _logger.LogError(message);
                return BadRequest(message);
            }

            var thermostats = _thermostatService.GetThermostats(skip, limit);
            if (thermostats.Count == 0)
            {
                message = $"No Thermostats found for skip: {skip} and limit: {limit}";
                _logger.LogError(message);
                return NotFound(message);
            }

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
                _logger.LogError(ex.Message);
                return NotFound(ex.Message);
            }
        }

        // POST: api/Thermostats?url=http://thermostat1:8080
        [HttpPost]
        public ActionResult<ThermostatWithURL> Register(string url)
        {
            try
            {
                var thermostat = _thermostatService.AddThermostat(url);
                return CreatedAtAction(nameof(Get), new { id = thermostat.Id }, thermostat);
            }
            catch(ThermostatExistsException ex)
            {
                //This exception always gets thrown when a thermostat tires to register itself upon start, but it already did in a previous startup.
                var thermostat = _thermostatService.GetThermostat(url);
                _logger.LogInformation("Thermostat was already registered. Message:\n" + ex.Message);
                return CreatedAtAction(nameof(Get), new { id = thermostat.Id }, thermostat);
            }
            catch (UriFormatException ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
