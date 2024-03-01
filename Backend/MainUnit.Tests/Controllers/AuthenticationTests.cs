using MainUnit.Controllers;
using MainUnit.Helper;
using MainUnit.Models.Auth;
using MainUnit.Models.Exceptions;
using MainUnit.Services;
using MainUnit.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainUnit.Tests.Controllers
{
    [TestFixture]
    public class AuthenticationTests
    {
        private readonly Mock<IAuthService> _authService;
        private readonly Mock<ILogger<AuthController>> _logger;

        public AuthenticationTests()
        {
            _logger = new Mock<ILogger<AuthController>>();
            _authService = new Mock<IAuthService>();
        }

        [Test]
        public void Register_ValidModel_ShouldReturnTrue()
        {
            _authService.Setup(item => item.RegisterUser(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(new LanguageExt.Common.Result<bool>(true)));
            var authController = new AuthController(_authService.Object, _logger.Object);

            var response = authController.Register(new UserLoginTransferObject() { Username = "123", Password = "456" });
            var result = response.GetAwaiter().GetResult();
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public void Login_InvalidUsername_ShouldReturnUnauthorized()
        {
            _authService.Setup(item => item.GetUserByName(It.IsAny<string>())).Returns(Task.FromResult(null as UserLogin));
            var authController = new AuthController(_authService.Object, _logger.Object);

            var response = authController.Login(new UserLoginTransferObject() { Username = "123", Password = "456" });
            var result = response.GetAwaiter().GetResult();
            Assert.That(result, Is.InstanceOf<UnauthorizedObjectResult>());
        }

        [Test]
        public void Login_InvalidPassword_ShouldReturnUnauthorized()
        {
            _authService.Setup(item => item.GetUserByName(It.IsAny<string>())).Returns(Task.FromResult(new UserLogin("123", "456")));
            var authController = new AuthController(_authService.Object, _logger.Object);

            var response = authController.Login(new UserLoginTransferObject() { Username = "123", Password = "456789"});
            var result = response.GetAwaiter().GetResult();
            Assert.That(result, Is.InstanceOf<UnauthorizedObjectResult>());
        }

        [Test]
        public void Login_ValidCredentials_ShouldReturnOk()
        {
            _authService.Setup(item => item.GetUserByName(It.IsAny<string>())).Returns(Task.FromResult(new UserLogin("123", Cryptography.ComputeSha512Hash("456"))));
            var authController = new AuthController(_authService.Object, _logger.Object);

            var response = authController.Login(new UserLoginTransferObject() { Username = "123", Password = "456" });
            var result = response.GetAwaiter().GetResult();

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }
    }
}
