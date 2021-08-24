using System;
using System.Threading.Tasks;
using Authentication.App.Challenge.Repositories.Database.Dtos;
using Authentication.App.Challenge.Services.Users;
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

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            if (id == 0)
            {
                return BadRequest();
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
        public async Task<IActionResult> Put(int id, [FromBody] JsonPatchDocument<UserDto> patchUser)
        {
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