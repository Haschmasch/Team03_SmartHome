using MainUnit.Models;
using MainUnit.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MainUnit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomTemperature : ControllerBase
    {
        private const string dateValidationErrorText = "The start date must smaller or equal the end date";

        private readonly IRoomTemperatureService _roomTemperatureService;

        public RoomTemperature(IRoomTemperatureService roomTemperatureService)
        {
            _roomTemperatureService = roomTemperatureService;
        }

        //GET: api/RoomTemperature?start=2012-12-31T22:00:00.000Z&end=2030-12-31T22:00:00.000Z
        [HttpGet]
        public ActionResult<List<RoomTemperatureEntry>> GetByDate(DateTime start, DateTime end)
        {
            if (ValidateDate(start,end))
                return BadRequest(dateValidationErrorText);

            var products = _roomTemperatureService.GetTemperatureEntries(start, end);

            return Ok(products);
        }

        //GET: api/RoomTemperature?roomId=1&start=2012-12-31T22:00:00.000Z&end=2030-12-31T22:00:00.000Z
        [HttpGet]
        public ActionResult<List<RoomTemperatureEntry>> GetByRoomAndDate(int roomId, DateTime start, DateTime end)
        {
            if (ValidateDate(start, end))
                return BadRequest(dateValidationErrorText);

            var products = _roomTemperatureService.GetTemperatureEntriesByRoom(roomId, start, end);

            return Ok(products);
        }

        //GET: api/RoomTemperature?thermostatId=1&start=2012-12-31T22:00:00.000Z&end=2030-12-31T22:00:00.000Z
        [HttpGet]
        public ActionResult<List<RoomTemperatureEntry>> GetByThermostatAndDate(int thermostatId, DateTime start, DateTime end)
        {
            if (ValidateDate(start, end))
                return BadRequest(dateValidationErrorText);

            var products = _roomTemperatureService.GetTemperatureEntriesByThermostat(thermostatId, start, end);

            return Ok(products);
        }

        private bool ValidateDate(DateTime start, DateTime end)
        {
            return end < start;
        }
    }
}
