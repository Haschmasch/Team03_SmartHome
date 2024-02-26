using MainUnit.Models.Exceptions;
using MainUnit.Models.Room;
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
        public ActionResult<IList<Room>> Get(int skip, int limit)
        {
            if (limit <= 0)
            {
               return BadRequest("The value of limit cannot be smaller than 1");
            }
            else if(skip < 0)
            {
                return BadRequest("The value of skip cannot be smaller than 0");
            }
            var rooms = _roomService.GetRooms(skip, limit);
            if (rooms.Count == 0)
                return NotFound($"No rooms found for skip: {skip} and limit: {limit}");

            return Ok(rooms);
        }

        // GET: api/Rooms/507f1f77bcf86cd799439011
        [HttpGet("{id}")]
        public ActionResult<Room> Get(string id)
        {
            try
            {
                var room = _roomService.GetRoom(id);
                return Ok(room);
            }
            catch (Exception ex) when (ex is RoomNotFoundException||
            ex is ThermostatNotFoundException ||
            ex is InvalidIdException ||
            ex is ThermostatExistsException ||
            ex is RoomExistsException) 
            {
                return NotFound(ex.Message);
            }
        }

        // POST api/Rooms
        [HttpPost]
        public ActionResult Post([FromBody] BaseRoom room)
        {
            try
            {
                var result = _roomService.AddRoom(room);
                return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
            }
            catch (Exception ex) when (ex is RoomNotFoundException ||
                ex is ThermostatNotFoundException ||
                ex is InvalidIdException ||
                ex is ThermostatExistsException ||
                ex is RoomExistsException)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/Rooms
        [HttpPut]
        public ActionResult<Room> Put([FromBody] BaseRoom room)
        {
            try
            {
                var result = _roomService.UpdateRoom(room);
                return Ok(result);
            }
            catch (Exception ex) when (ex is RoomNotFoundException ||
                ex is ThermostatNotFoundException ||
                ex is InvalidIdException ||
                ex is ThermostatExistsException ||
                ex is RoomExistsException)
            {
                return NotFound(ex.Message);
            }
        }

        // DELETE api/Rooms/507f1f77bcf86cd799439011
        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {
            try
            {
                _roomService.RemoveRoom(id);
                return NoContent();
            }
            catch (Exception ex) when (ex is RoomNotFoundException ||
                ex is ThermostatNotFoundException ||
                ex is InvalidIdException ||
                ex is ThermostatExistsException ||
                ex is RoomExistsException)
            {
                return NotFound(ex.Message);
            }

        }

        // PUT api/Rooms/507f1f77bcf86cd799439011/Thermostats
        [HttpPut("{id}/Thermostats")]
        public ActionResult<Room> AddThermostat(string id, [FromBody] string thermostatId)
        {
            try
            {
                var result = _roomService.AddThermostat(id, thermostatId);
                return Ok(result);
            }
            catch (Exception ex) when (ex is RoomNotFoundException ||
                ex is ThermostatNotFoundException ||
                ex is InvalidIdException ||
                ex is ThermostatExistsException ||
                ex is RoomExistsException)
            {
                return NotFound(ex.Message);
            }
        }

        // PUT api/Rooms/507f1f77bcf86cd799439011/Thermostats
        [HttpDelete("{id}/Thermostats")]
        public ActionResult<Room> RemoveThermostat(string id, [FromBody] string thermostatId)
        {
            try
            {
                _roomService.RemoveThermostat(id, thermostatId);
                return NoContent();
            }
            catch (Exception ex) when (ex is RoomNotFoundException ||
                ex is ThermostatNotFoundException ||
                ex is InvalidIdException ||
                ex is ThermostatExistsException ||
                ex is RoomExistsException)
            {
                return NotFound(ex.Message);
            }
        }

        // PUT api/Rooms/507f1f77bcf86cd799439011/Temperature
        [HttpPut("{id}/Temperature")]
        public ActionResult<Room> UpdateTemperature(string id, [FromBody] float temperature)
        {
            try
            {
                var result = _roomService.SetRoomTemperature(id, temperature);
                return Ok(result);
            }
            catch (Exception ex) when (ex is RoomNotFoundException ||
                ex is ThermostatNotFoundException ||
                ex is InvalidIdException ||
                ex is ThermostatExistsException ||
                ex is RoomExistsException)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
