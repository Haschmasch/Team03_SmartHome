using MainUnit.Controllers;
using MainUnit.Models.Exceptions;
using MainUnit.Models.Room;
using MainUnit.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace MainUnit.Tests.Controllers
{
    [TestFixture]
    public class RoomsTests
    {
        private readonly Mock<IRoomService> _roomService;
        private readonly Mock<ILogger<Rooms>> _logger;

        public RoomsTests()
        {
            _logger = new Mock<ILogger<Rooms>>();
            _roomService = new Mock<IRoomService>();
        }

        [Test]
        public void Get_InvalidLimit_ShouldReturnBadRequest()
        {
            _roomService.Setup(item => item.GetRooms(It.IsAny<int>(), It.IsAny<int>()));
            var roomsController = new Rooms(_roomService.Object, _logger.Object);

            var response = roomsController.Get(0, 0);

            Assert.That(response.Result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public void Get_InvalidSkip_ShouldReturnBadRequest()
        {
            _roomService.Setup(item => item.GetRooms(It.IsAny<int>(), It.IsAny<int>()));
            var roomsController = new Rooms(_roomService.Object, _logger.Object);

            var response = roomsController.Get(-1, 0);

            Assert.That(response.Result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public void Get_NoValidRooms_ShouldReturnNotFound()
        {
            _roomService.Setup(item => item.GetRooms(It.IsAny<int>(), It.IsAny<int>())).Returns(new List<Room>());
            var roomsController = new Rooms(_roomService.Object, _logger.Object);

            var response = roomsController.Get(0, 10);

            Assert.That(response.Result, Is.InstanceOf<NotFoundObjectResult>());
        }

        [Test]
        public void Get_Valid_ShouldReturnRooms()
        {
            _roomService.Setup(item => item.GetRooms(It.IsAny<int>(), It.IsAny<int>())).Returns(new List<Room>() { new Room() });
            var roomsController = new Rooms(_roomService.Object, _logger.Object);

            var response = roomsController.Get(0, 10);

            Assert.That(response.Result, Is.InstanceOf<OkObjectResult>());
            OkObjectResult createdAtAction = (OkObjectResult)response.Result;
            Assert.That(((List<Room>)createdAtAction.Value!).Count, Is.EqualTo(1));
        }

        [Test]
        public void Get_InvalidId_ShouldReturnNotFound()
        {
            _roomService.Setup(item => item.GetRoom(It.IsAny<string>()))
                .Throws(new InvalidIdException());
            var roomsController = new Rooms(_roomService.Object, _logger.Object);

            var response = roomsController.Get("123");

            Assert.That(response.Result, Is.InstanceOf<NotFoundObjectResult>());
        }

        [Test]
        public void Get_ValidId_ShouldReturnRoom()
        {
            _roomService.Setup(item => item.GetRoom(It.IsAny<string>()))
                .Returns(new Room());
            var roomsController = new Rooms(_roomService.Object, _logger.Object);

            var response = roomsController.Get("123");

            Assert.That(response.Result, Is.InstanceOf<OkObjectResult>());
            OkObjectResult createdAtAction = (OkObjectResult)response.Result;
            Assert.That(createdAtAction.Value!, Is.InstanceOf<Room>());
        }

        [Test]
        public void Post_ValidRoom_ShouldReturnRoom()
        {
            var room = new Room();
            _roomService.Setup(item => item.AddRoom(It.IsAny<BaseRoom>()))
                .Returns(room);
            var roomsController = new Rooms(_roomService.Object, _logger.Object);

            var response = roomsController.Post(room);

            Assert.That(response, Is.InstanceOf<CreatedAtActionResult>());
            CreatedAtActionResult createdAtAction = (CreatedAtActionResult)response;
            Assert.That(room, Is.EqualTo(createdAtAction.Value!));
        }

        [Test]
        public void Put_InvalidRoom_ShouldReturnNotFound()
        {
            var room = new Room();
            _roomService.Setup(item => item.UpdateRoom(It.IsAny<BaseRoom>()))
                .Throws(new InvalidIdException());
            var roomsController = new Rooms(_roomService.Object, _logger.Object);

            var response = roomsController.Put(room);

            Assert.That(response.Result, Is.InstanceOf<NotFoundObjectResult>());
        }

        [Test]
        public void Put_ValidRoom_ShouldReturnUpdatedRoom()
        {
            var room = new Room();
            _roomService.Setup(item => item.UpdateRoom(It.IsAny<BaseRoom>()))
                .Returns(room);
            var roomsController = new Rooms(_roomService.Object, _logger.Object);

            var response = roomsController.Put(room);

            Assert.That(response.Result, Is.InstanceOf<OkObjectResult>());
            OkObjectResult createdAtAction = (OkObjectResult)response.Result;
            Assert.That(room, Is.EqualTo(createdAtAction.Value!));
        }

        [Test]
        public void AddThermostat_InvalidId_ShouldReturnNotFound()
        {
            _roomService.Setup(item => item.AddThermostat(It.IsAny<string>(), It.IsAny<string>()))
                .Throws(new InvalidIdException());
            var roomsController = new Rooms(_roomService.Object, _logger.Object);

            var response = roomsController.AddThermostat("123", "12345");

            Assert.That(response.Result, Is.InstanceOf<NotFoundObjectResult>());
        }

        [Test]
        public void AddThermostat_ValidId_ShouldReturnRoom()
        {
            var room = new Room();
            _roomService.Setup(item => item.AddThermostat(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(room);
            var roomsController = new Rooms(_roomService.Object, _logger.Object);

            var response = roomsController.AddThermostat("123", "12345");

            Assert.That(response.Result, Is.InstanceOf<OkObjectResult>());
            OkObjectResult okObjectResult = (OkObjectResult)response.Result;
            Assert.That(room, Is.EqualTo(okObjectResult.Value!));
        }

        [Test]
        public void RemoveThermostat_InvalidId_ShouldReturnNotFound()
        {
            var room = new Room();
            _roomService.Setup(item => item.RemoveRoom(It.IsAny<string>()))
                .Throws(new RoomNotFoundException());
            var roomsController = new Rooms(_roomService.Object, _logger.Object);

            var response = roomsController.RemoveThermostat("123", "12");

            Assert.That(response.Result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public void RemoveThermostat_ValidId_ShouldReturnNoContent()
        {
            _roomService.Setup(item => item.RemoveRoom(It.IsAny<string>()));
            var roomsController = new Rooms(_roomService.Object, _logger.Object);

            var response = roomsController.RemoveThermostat("123", "12");

            Assert.That(response.Result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public void UpdateTemperature_InvalidTemperature_ShouldReturnNotFound()
        {
            _roomService.Setup(item => item.SetRoomTemperature(It.IsAny<string>(), It.IsAny<float>()))
                .Throws(new InvalidIdException());
            var roomsController = new Rooms(_roomService.Object, _logger.Object);

            var response = roomsController.UpdateTemperature("123", 50);

            Assert.That(response.Result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public void UpdateTemperature_InvalidId_ShouldReturnNotFound()
        {
            _roomService.Setup(item => item.SetRoomTemperature(It.IsAny<string>(), It.IsAny<float>()))
                .Throws(new InvalidIdException());
            var roomsController = new Rooms(_roomService.Object, _logger.Object);

            var response = roomsController.UpdateTemperature("123", 21);

            Assert.That(response.Result, Is.InstanceOf<NotFoundObjectResult>());
        }

        [Test]
        public void UpdateTemperature_ValidId_ShouldReturnOk()
        {
            var room = new Room();
            _roomService.Setup(item => item.SetRoomTemperature(It.IsAny<string>(), It.IsAny<float>()))
                .Returns(room);
            var roomsController = new Rooms(_roomService.Object, _logger.Object);

            var response = roomsController.UpdateTemperature("123", 21);

            Assert.That(response.Result, Is.InstanceOf<OkObjectResult>());
            OkObjectResult createdAtAction = (OkObjectResult)response.Result;
            Assert.That(room, Is.EqualTo(createdAtAction.Value!));
        }
    }
}
