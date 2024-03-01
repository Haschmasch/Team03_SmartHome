using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LanguageExt;
using LanguageExt.Common;
using MainUnit.Extensions;
using MainUnit.Helper;
using MainUnit.Models.Auth;
using MainUnit.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace MainUnit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService, ILogger<AuthController> logger) : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserLoginTransferObject model)
        {
            var result = await authService.RegisterUser(model.Username, Cryptography.ComputeSha512Hash(model.Password));

            return result.CheckForErrors();
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginTransferObject model)
        {
            //Hardcoded because this will not change.
            if (model.Username == "svc_thermostat" && model.Password == "E9;$t,hD:%_2-{!ksm46vz")
            {
                var token = GenerateJwtToken();
                return Ok(new { token });
            }

            var userResult = await authService.GetUserByName(model.Username);

            if (userResult != null)
            {
                if (userResult.Password == Cryptography.ComputeSha512Hash(model.Password))
                {
                    var token = GenerateJwtToken();
                    return Ok(new { token });
                }
            }

            logger.LogWarning("Invalid credentials.");
            return Unauthorized("Invalid credentials.");
        }

        private string GenerateJwtToken()
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("LQKeZVE_x+-v{4zsnrPMwt76AJ#3,=R'<\"%W5h;g?yYF>!}@)c");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, "admin")
                    // Add more claims if needed
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
