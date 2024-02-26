﻿using MainUnit.Models.Exceptions;
using MainUnit.Models.Room;
using MainUnit.Models.RoomTemperature;
using MainUnit.Models.Thermostat;
using MainUnit.Services;
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
        public ActionResult<IList<RoomTemperatureEntry>> GetByDate(DateTime start, DateTime end)
        {
            return GetEntries(null!, null!, start, end);
        }

        //GET: api/RoomTemperature?roomId=507f1f77bcf86cd799439011&start=2012-12-31T22:00:00.000Z&end=2030-12-31T22:00:00.000Z
        [HttpGet("ByRoom")]
        public ActionResult<IList<RoomTemperatureEntry>> GetByRoomAndDate(string roomId, DateTime start, DateTime end)
        {
            return GetEntries(roomId, null!, start, end);
        }

        //GET: api/RoomTemperature?thermostatId=507f1f77bcf86cd799439011&start=2012-12-31T22:00:00.000Z&end=2030-12-31T22:00:00.000Z
        [HttpGet("ByThermostat")]
        public ActionResult<IList<RoomTemperatureEntry>> GetByThermostatAndDate(string thermostatId, DateTime start, DateTime end)
        {
            return GetEntries(null!, thermostatId, start, end);
        }

        private ActionResult<IList<RoomTemperatureEntry>> GetEntries(string roomId, string thermostatId, DateTime start, DateTime end)
        {

            if (end < start)
                return BadRequest(dateValidationErrorText);

            IList<RoomTemperatureEntry> entries;
            try
            {
                if (roomId != null)
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
                return BadRequest(ex.Message);
            }

            if (entries.Count == 0)
                return NotFound($"No entrys found for query start:'{start}' end:'{end}'.");

            return Ok(entries);
        }
    }
}
