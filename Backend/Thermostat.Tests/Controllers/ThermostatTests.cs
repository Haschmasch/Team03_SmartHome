using Microsoft.AspNetCore.Mvc;
using Moq;
using Thermostat.Services.Interfaces;

namespace Thermostat.Tests.Controllers
{
    public class ThermostatTests
    {
        private readonly Mock<IThermostatService> _authThermostatService;

        public ThermostatTests()
        {
            _authThermostatService = new Mock<IThermostatService>();
        }

        [Test]
        public void SetTemperature_ValidTemperature_ShouldReturnTrue()
        {
            _authThermostatService.Setup(item => item.UpdateTemperature(It.IsAny<float>()))
                .Returns(new LanguageExt.Common.Result<bool>(true));
            var thermostatController = new Thermostat.Controllers.Thermostat(_authThermostatService.Object);

            var response = thermostatController.SetTemperature(42);
            Assert.That(response, Is.InstanceOf<OkObjectResult>());
        }
    }
}
