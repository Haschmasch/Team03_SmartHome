using MainUnit.Models.Exceptions;
using MainUnit.Models.RoomTemperature;
using MainUnit.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MainUnit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomTemperature : ControllerBase
    {
        private const string dateValidationErrorText = "The start date must smaller or equal the end date";

        private readonly IRoomTemperatureService _roomTemperatureService;
        private readonly ILogger _logger;

        public RoomTemperature(IRoomTemperatureService roomTemperatureService, ILogger<RoomTemperature> logger)
        {
            _roomTemperatureService = roomTemperatureService;
            this._logger = logger;
        }

        //There are 3 enpoints here, bacause filtering by both thermostat and room 
        //GET: api/RoomTemperature?start=2012-12-31T22:00:00.000Z&end=2030-12-31T22:00:00.000Z
        //or: GET: api/RoomTemperature?roomId=507f1f77bcf86cd799439011&start=2012-12-31T22:00:00.000Z&end=2030-12-31T22:00:00.000Z
        //or: GET: api/RoomTemperature?thermostatId=507f1f77bcf86cd799439011&start=2012-12-31T22:00:00.000Z&end=2030-12-31T22:00:00.000Z
        //or: GET: api/RoomTemperature?roomId=507f1f77bcf86cd799439011&thermostatId=507f1f77bcf86cd799439011&start=2012-12-31T22:00:00.000Z&end=2030-12-31T22:00:00.000Z
        [HttpGet]
        public ActionResult<IList<RoomTemperatureEntry>> Get(string? roomId, string? thermostatId, DateTime start, DateTime end)
        {
            if (end < start)
                return BadRequest(dateValidationErrorText);

            IList<RoomTemperatureEntry> entries;
            try
            {
                if(roomId != null && thermostatId != null)
                {
                    entries = _roomTemperatureService.GetTemperatureEntriesByRoomAndThermostat(thermostatId, roomId, start, end);
                }
                else if (roomId != null)
                {
                    entries = _roomTemperatureService.GetTemperatureEntriesByRoom(roomId, start, end);
                }
                else if (thermostatId != null)
                {
                    entries = _roomTemperatureService.GetTemperatureEntriesByThermostat(thermostatId, start, end);
                }
                else
                {
                    entries = _roomTemperatureService.GetTemperatureEntries(start, end);
                }
            }
            catch (InvalidIdException ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }

            if (entries.Count == 0)
            {
                string message = $"No entrys found for query start:'{start}' end:'{end}'.";
                _logger.LogError(message);
                return NotFound(message);
            }

            return Ok(entries);
        }
    }
}
