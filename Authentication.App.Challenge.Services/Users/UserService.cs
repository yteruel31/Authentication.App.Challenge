using System;
using System.Threading.Tasks;
using Authentication.App.Challenge.Models;
using Authentication.App.Challenge.Repositories.Database;
using Authentication.App.Challenge.Repositories.Database.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Authentication.App.Challenge.Services.Users
{
    public class UserService : IUserService
    {
        private readonly CoreContext _coreContext;

        public UserService(CoreContext coreContext)
        {
            _coreContext = coreContext;
        }

        public async Task<User> Get(int id)
        {
            UserDto dto = await _coreContext.Users.SingleOrDefaultAsync(p => p.Id == id);

            return new User
            {
                Id = dto.Id,
                Email = dto.Email,
                Name = dto.Name,
                Phone = dto.Phone,
                Bio = dto.Bio
            };
        }
        
        public Task<UserDto> GetDto(int id)
        {
            return _coreContext.Users.SingleOrDefaultAsync(p => p.Id == id);
        }
        
        public async Task<int> Update(UserDto userDto)
        {
            userDto.UpdatedAt = DateTime.Now;
            _coreContext.Entry(await GetDto(userDto.Id))
                .CurrentValues
                .SetValues(userDto);
            
            return await _coreContext.SaveChangesAsync();
        }
    }

    public interface IUserService
    {
        Task<User> Get(int id);

        Task<UserDto> GetDto(int id);

        Task<int> Update(UserDto userDto);
    }
}