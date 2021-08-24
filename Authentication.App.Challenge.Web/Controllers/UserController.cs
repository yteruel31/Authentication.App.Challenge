using System;
using System.Threading.Tasks;
using Authentication.App.Challenge.Repositories.Database.Dtos;
using Authentication.App.Challenge.Services.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.App.Challenge.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/users/{id:int}")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;

        public UserController(IUserService userService, IAuthService authService)
        {
            _userService = userService;
            _authService = authService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int id, [FromHeader] AuthenticationHeader header)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            
            AuthService.Payload payload = _authService.Decode(header.Authorization[JwtBearerDefaults.AuthenticationScheme.Length..].Trim());

            if (id != payload.Id)
            {
                return Unauthorized();
            }
            
            try
            {
                return Ok(await _userService.Get(id));
            }
            catch (Exception e)
            {
                return StatusCode(500, new {exception = e});
            }
        }
        
        [HttpPatch]
        public async Task<IActionResult> Put(int id, [FromHeader] AuthenticationHeader header, [FromBody] JsonPatchDocument<UserDto> patchUser)
        {
            AuthService.Payload payload = _authService.Decode(header.Authorization[JwtBearerDefaults.AuthenticationScheme.Length..].Trim());

            if (id != payload.Id)
            {
                return Unauthorized();
            }
            
            try
            {
                UserDto userToUpdate = await _userService.GetDto(id);
                
                patchUser.ApplyTo(userToUpdate);
                
                return Ok(await _userService.Update(userToUpdate));
            }
            catch (Exception e)
            {
                return StatusCode(500, new {exception = e});
            }
        }
    }
}