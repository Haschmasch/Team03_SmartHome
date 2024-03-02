using MainUnit.Controllers;
using MainUnit.Models.Exceptions;
using MainUnit.Models.Thermostat;
using MainUnit.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace MainUnit.Tests.Controllers
{
    [TestFixture]
    public class ThermostatsTests
    {
        private readonly Mock<IThermostatService> _thermostatService;
        private readonly Mock<ILogger<Thermostats>> _logger;

        public ThermostatsTests()
        {
            _logger = new Mock<ILogger<Thermostats>>();
            _thermostatService = new Mock<IThermostatService>();
        }

        [Test]
        public void Get_InvalidLimit_ShouldReturnBadRequest()
        {
            _thermostatService.Setup(item => item.GetThermostats(It.IsAny<int>(), It.IsAny<int>()));
            var thermostatsController = new Thermostats(_thermostatService.Object, _logger.Object);

            var response = thermostatsController.Get(0, 0);

            Assert.That(response.Result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public void Get_InvalidSkip_ShouldReturnBadRequest()
        {
            _thermostatService.Setup(item => item.GetThermostats(It.IsAny<int>(), It.IsAny<int>()));
            var thermostatsController = new Thermostats(_thermostatService.Object, _logger.Object);

            var response = thermostatsController.Get(-1, 0);

            Assert.That(response.Result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public void Get_NoValidRooms_ShouldReturnNotFound()
        {
            _thermostatService.Setup(item => item.GetThermostats(It.IsAny<int>(), It.IsAny<int>())).Returns(new List<Thermostat>());
            var thermostatsController = new Thermostats(_thermostatService.Object, _logger.Object);

            var response = thermostatsController.Get(0, 10);

            Assert.That(response.Result, Is.InstanceOf<NotFoundObjectResult>());
        }

        [Test]
        public void Get_Valid_ShouldReturnThermostats()
        {
            _thermostatService.Setup(item => item.GetThermostats(It.IsAny<int>(), It.IsAny<int>())).Returns(new List<Thermostat>() { new Thermostat() });
            var thermostatsController = new Thermostats(_thermostatService.Object, _logger.Object);

            var response = thermostatsController.Get(0, 10);

            Assert.That(response.Result, Is.InstanceOf<OkObjectResult>());
            OkObjectResult createdAtAction = (OkObjectResult)response.Result;
            Assert.That(((List<Thermostat>)createdAtAction.Value!).Count, Is.EqualTo(1));
        }


        [Test]
        public void GetById_InvalidId_ShouldReturnNotFound()
        {
            _thermostatService.Setup(item => item.GetThermostat(It.IsAny<string>())).Throws(new ThermostatNotFoundException());
            var thermostatsController = new Thermostats(_thermostatService.Object, _logger.Object);

            var response = thermostatsController.Get("123");

            Assert.That(response.Result, Is.InstanceOf<NotFoundObjectResult>());
        }

        [Test]
        public void GetById_ValidId_ShouldReturnThermostat()
        {
            var thermostat = new Thermostat();
            _thermostatService.Setup(item => item.GetThermostat(It.IsAny<string>())).Returns(thermostat);
            var thermostatsController = new Thermostats(_thermostatService.Object, _logger.Object);

            var response = thermostatsController.Get("123");

            Assert.That(response.Result, Is.InstanceOf<OkObjectResult>());
            OkObjectResult createdAtAction = (OkObjectResult)response.Result;
            Assert.That(createdAtAction.Value!, Is.EqualTo(thermostat));
        }

        [Test]
        public void Register_InvalidURL_ShouldReturnBadRequest()
        {
            _thermostatService.Setup(item => item.AddThermostat(It.IsAny<string>())).Throws(new UriFormatException());
            var thermostatsController = new Thermostats(_thermostatService.Object, _logger.Object);

            var response = thermostatsController.Register("123");

            Assert.That(response.Result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public void Register_ExisitingURL_ShouldReturnThermostat()
        {
            var thermostat = new ThermostatWithURL();
            _thermostatService.Setup(item => item.AddThermostat(It.IsAny<string>())).Throws(new ThermostatExistsException());
            _thermostatService.Setup(item => item.GetThermostatByURL(It.IsAny<string>())).Returns(thermostat);
            var thermostatsController = new Thermostats(_thermostatService.Object, _logger.Object);

            var response = thermostatsController.Register("123");

            Assert.That(response.Result, Is.InstanceOf<CreatedAtActionResult>());
            CreatedAtActionResult createdAtAction = (CreatedAtActionResult)response.Result;
            Assert.That(createdAtAction.Value!, Is.EqualTo(thermostat));
        }

        [Test]
        public void Register_NewURL_ShouldReturnThermostat()
        {
            var thermostat = new ThermostatWithURL();
            _thermostatService.Setup(item => item.AddThermostat(It.IsAny<string>())).Returns(thermostat);
            var thermostatsController = new Thermostats(_thermostatService.Object, _logger.Object);

            var response = thermostatsController.Register("123");

            Assert.That(response.Result, Is.InstanceOf<CreatedAtActionResult>());
            CreatedAtActionResult createdAtAction = (CreatedAtActionResult)response.Result;
            Assert.That(createdAtAction.Value!, Is.EqualTo(thermostat));
        }
    }
}
