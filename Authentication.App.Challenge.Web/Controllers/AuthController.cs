using System.Threading.Tasks;
using Authentication.App.Challenge.Models.Auth;
using Authentication.App.Challenge.Services.Users;
using Authentication.App.Challenge.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.App.Challenge.Web.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCredentials credentials)
        {
            if (string.IsNullOrEmpty(credentials.Email) || string.IsNullOrEmpty(credentials.Password))
            {
                return BadRequest();
            }
            
            ResultBase result = await _authService.Login(credentials.Email, credentials.Password);

            if (result is ResultContent<Error> error)
            {
                return Unauthorized(error);
            }
            
            return Ok(result);
        }
        
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterCredentials credentials)
        {
            if (string.IsNullOrEmpty(credentials.Email) || string.IsNullOrEmpty(credentials.Password))
            {
                return BadRequest();
            }
            
            ResultBase result = await _authService.Register(credentials.Email, credentials.Password);

            if (result is ResultContent<Error> error)
            {
                return Unauthorized(error);
            }
            
            return Ok(result);
        }
    }
}