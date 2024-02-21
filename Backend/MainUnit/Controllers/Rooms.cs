﻿using MainUnit.Models;
using MainUnit.Models.Exceptions;
using MainUnit.Services;
using MainUnit.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MainUnit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Rooms : ControllerBase
    {
        private readonly IRoomService _roomService;

        public Rooms(IRoomService thermostatService)
        {
            _roomService = thermostatService;
        }

        // GET: api/Rooms?skip=0&limit=5
        [HttpGet]
        public ActionResult<List<Room>> Get(int skip, int limit)
        {
            var rooms = _roomService.GetRooms(skip, limit);
            if (rooms.Count == 0)
                return NotFound($"No rooms found for skip: {skip} and limit: {limit}");

            return Ok(rooms);
        }

        // GET: api/Rooms/5
        [HttpGet("{id}")]
        public ActionResult<Room> Get(int id)
        {
            try
            {
                var room = _roomService.GetRoom(id);
                return Ok(room);
            }
            catch (RoomNotFoundException ex) 
            {
                return NotFound(ex.Message);
            }
        }

        // POST api/Rooms
        [HttpPost]
        public ActionResult Post([FromBody] Room room)
        {
            try
            {
                var result = _roomService.AddRoom(room);
                return CreatedAtAction(nameof(Get), new { id = room.Id }, room);
            }
            catch (RoomExistsException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/Rooms
        [HttpPut]
        public ActionResult<Room> Put([FromBody] Room room)
        {
            try
            {
                var result = _roomService.UpdateRoom(room);
                return Ok(result);
            }
            catch (RoomNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // DELETE api/Rooms/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                _roomService.RemoveRoom(id);
                return NoContent();
            }
            catch (RoomNotFoundException ex)
            {
                return NotFound(ex.Message);
            }

        }

        // PUT api/Rooms/5/Thermostats
        [HttpPut("{id}/Thermostats")]
        public ActionResult<Room> AddThermostat(int id, [FromBody] int thermostatId)
        {
            try
            {
                var result = _roomService.AddThermostat(id, thermostatId);
                return Ok(result);
            }
            catch (Exception ex) when (ex is RoomNotFoundException || ex is ThermostatNotFoundException)
            {
                return NotFound(ex.Message);
            }
        }

        // PUT api/Rooms/5/Temperature
        [HttpPut("{id}/Temperature")]
        public ActionResult<Room> UpdateTemperature(int id, [FromBody] float temperature)
        {
            try
            {
                var result = _roomService.SetRoomTemperature(id, temperature);
                return Ok(result);
            }
            catch (Exception ex) when (ex is RoomNotFoundException)
            {
                return NotFound(ex.Message);
            }
        }
    }
}