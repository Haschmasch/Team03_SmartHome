using MainUnit.Controllers;
using MainUnit.Models.Exceptions;
using MainUnit.Models.RoomTemperature;
using MainUnit.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace MainUnit.Tests.Controllers
{
    [TestFixture]
    public class RoomTemperatureTests
    {
        private readonly Mock<IRoomTemperatureService> _roomTemperatureService;
        private readonly Mock<ILogger<RoomTemperature>> _logger;

        public RoomTemperatureTests()
        {
            _logger = new Mock<ILogger<RoomTemperature>>();
            _roomTemperatureService = new Mock<IRoomTemperatureService>();
        }

        [Test]
        public void GetByDate_StartSmallerEnd_ShouldReturnBadRequest()
        {
            _roomTemperatureService.Setup(item => item.GetTemperatureEntries(It.IsAny<DateTime>(), It.IsAny<DateTime>()));
            var roomTemperatureController = new RoomTemperature(_roomTemperatureService.Object, _logger.Object);

            var response = roomTemperatureController.GetByDate(DateTime.Now, DateTime.MinValue);

            Assert.That(response.Result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public void GetByDate_InvalidStartToEnd_ShouldReturnNotFound()
        {
            _roomTemperatureService.Setup(item => item.GetTemperatureEntries(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(new List<RoomTemperatureEntry>());
            var roomTemperatureController = new RoomTemperature(_roomTemperatureService.Object, _logger.Object);

            var response = roomTemperatureController.GetByDate(DateTime.Now, DateTime.MaxValue);


            Assert.That(response.Result, Is.InstanceOf<NotFoundObjectResult>());
        }

        [Test]
        public void GetByDate_InvalidRoomId_ShouldReturnNotFound()
        {
            _roomTemperatureService.Setup(item => item.GetTemperatureEntriesByRoom(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Throws(new InvalidIdException());
            var roomTemperatureController = new RoomTemperature(_roomTemperatureService.Object, _logger.Object);

            var response = roomTemperatureController.GetByRoomAndDate("123", DateTime.Now, DateTime.MaxValue);

            Assert.That(response.Result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public void GetByDate_InvalidThermostatId_ShouldReturnNotFound()
        {
            _roomTemperatureService.Setup(item => item.GetTemperatureEntriesByThermostat(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Throws(new InvalidIdException());
            var roomTemperatureController = new RoomTemperature(_roomTemperatureService.Object, _logger.Object);

            var response = roomTemperatureController.GetByThermostatAndDate("123", DateTime.Now, DateTime.MaxValue);

            Assert.That(response.Result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public void GetByDate_ValidStandToEnd_ShouldReturnEntries()
        {
            _roomTemperatureService.Setup(item => item.GetTemperatureEntries(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(new List<RoomTemperatureEntry>() { new RoomTemperatureEntry() });
            var roomTemperatureController = new RoomTemperature(_roomTemperatureService.Object, _logger.Object);

            var response = roomTemperatureController.GetByDate(DateTime.Now, DateTime.MaxValue);

            Assert.That(response.Result, Is.InstanceOf<OkObjectResult>());
            OkObjectResult createdAtAction = (OkObjectResult)response.Result;
            Assert.That(((List<RoomTemperatureEntry>)createdAtAction.Value!).Count!, Is.EqualTo(1));
        }
    }
}
