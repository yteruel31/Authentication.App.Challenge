using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Authentication.App.Challenge.Repositories.Database;
using Authentication.App.Challenge.Repositories.Database.Dtos;
using Authentication.App.Challenge.Utils;
using JWT.Algorithms;
using JWT.Builder;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Authentication.App.Challenge.Services.Users
{
    public class AuthService : IAuthService
    {
        private readonly CoreContext _coreContext;

        public AuthService(CoreContext coreContext)
        {
            _coreContext = coreContext;
        }

        public async Task<string> Login(string email, string password)
        {
            UserDto user = await _coreContext.Users.FirstOrDefaultAsync(u => u.Email == email);
            bool isPasswordMatched = EncryptHelper.VerifyPassword(password, user.Salt, user.Password);
            
            if (isPasswordMatched)
            {
                return Encode(new Dictionary<string, object>()
                {
                    {"id", user.Id},
                    {"email", user.Email},
                    {"exp", DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds()}
                });
            }

            return string.Empty;
        }

        public async Task<string> Register(string email, string password)
        {
            EncryptHelper.HashSalt hashSalt = EncryptHelper.EncryptPassword(password);
            
            var result = await _coreContext.Users.AddAsync(new UserDto
            {
                Email = email,
                Password = hashSalt.Hash,
                Salt = hashSalt.Salt,
                CreatedAt = DateTime.Now
            });
            await _coreContext.SaveChangesAsync();
            
            return Encode(new Dictionary<string, object>()
            {
                {"id", result.Entity.Id},
                {"email", result.Entity.Email},
                {"exp", DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds()}
            });
        }

        private string Encode(Dictionary<string, object> payload)
        {
            return JwtBuilder.Create()
                .WithAlgorithm(new HMACSHA256Algorithm())
                .WithSecret(Environment.GetEnvironmentVariable("SECRET"))
                .AddClaims(payload)
                .Encode();
        }
        
        public Payload Decode(string token)
        {
            return JsonConvert.DeserializeObject<Payload>(JwtBuilder.Create()
                .WithAlgorithm(new HMACSHA256Algorithm())
                .WithSecret(Environment.GetEnvironmentVariable("SECRET"))
                .MustVerifySignature()
                .Decode(token));
        }
        
        public class Payload
        {
            [JsonProperty("id")]
            public int Id { get; set; }

            [JsonProperty("email")]
            public string Email { get; set; }

            [JsonProperty("exp")]
            public int Exp { get; set; }
        }
    }

    public interface IAuthService
    {
        Task<string> Login(string email, string password);
        
        Task<string> Register(string email, string password);
        
        AuthService.Payload Decode(string token);
    }
}