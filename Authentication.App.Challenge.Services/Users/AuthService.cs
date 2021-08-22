using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Authentication.App.Challenge.Repositories.Database;
using Authentication.App.Challenge.Repositories.Database.Dtos;
using Authentication.App.Challenge.Repositories.Database.Redis;
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
        private readonly IRedisRepository _redisRepository;

        public AuthService(CoreContext coreContext, 
            IRedisRepository redisRepository)
        {
            _coreContext = coreContext;
            _redisRepository = redisRepository;
        }

        public async Task<ResultBase> Login(string email, string password)
        {
            UserDto user = await _coreContext.Users.FirstOrDefaultAsync(u => u.Email == email);
            bool isPasswordMatched = EncryptHelper.VerifyPassword(password, user.Salt, user.Password);

            if (!isPasswordMatched)
            {
                return new ResultContent<Error>
                {
                    Result = new Error
                    {
                        Message = "Password not matched"
                    }
                };
            }
            
            long expire = DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds();
            string token = Encode(new Dictionary<string, object>
            {
                {"id", user.Id},
                {"email", user.Email},
                {"exp", expire}
            });

            if (_redisRepository.Database.SetContains(token, user.Id))
            {
                return new ResultContent<Error>
                {
                    Result = new Error
                    {
                        Message = "Token is blacklisted"
                    }
                };
            }

            return new ResultContent<AuthTokenSuccess>
            {
                Result = new AuthTokenSuccess
                {
                    Token = token,
                    Expire = expire
                }
            };
        }

        public async Task<ResultBase> Register(string email, string password)
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

            long expire = DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds();
            string token = Encode(new Dictionary<string, object>
            {
                { "id", result.Entity.Id },
                { "email", result.Entity.Email },
                { "exp", expire }
            });

            if (_redisRepository.Database.SetContains(token, result.Entity.Id))
            {
                return new ResultContent<Error>
                {
                    Result = new Error
                    {
                        Message = "Token is blacklisted"
                    }
                };
            }

            return new ResultContent<AuthTokenSuccess>
            {
                Result = new AuthTokenSuccess
                {
                    Token = token,
                    Expire = expire
                }
            };
        }
        
        public async Task<ResultBase> Logout(string token)
        {
            Payload payload = Decode(token);
            TimeSpan timeSpan = (DateTimeOffset.FromUnixTimeSeconds(payload.Exp) - DateTime.UtcNow).Duration();
            
            if (await _redisRepository.Database.StringSetAsync(token, payload.Id, timeSpan))
            {
                return new ResultContent<Info>
                {
                    Result = new Info
                    {
                        Message = "Logout successfully"
                    }
                };
            }
            
            return new ResultContent<Error>
            {
                Result = new Error
                {
                    Message = ""
                }
            };
        }

        public Payload Decode(string token)
        {
            return JsonConvert.DeserializeObject<Payload>(JwtBuilder.Create()
                .WithAlgorithm(new HMACSHA256Algorithm())
                .WithSecret(Environment.GetEnvironmentVariable("SECRET"))
                .MustVerifySignature()
                .Decode(token));
        }
        
        private string Encode(Dictionary<string, object> payload)
        {
            return JwtBuilder.Create()
                .WithAlgorithm(new HMACSHA256Algorithm())
                .WithSecret(Environment.GetEnvironmentVariable("SECRET"))
                .AddClaims(payload)
                .Encode();
        }

        public class Payload
        {
            [JsonProperty("id")]
            public int Id { get; set; }

            [JsonProperty("email")]
            public string Email { get; set; }

            [JsonProperty("exp")]
            public long Exp { get; set; }
        }
    }

    public class AuthTokenSuccess : ResultContentBase
    {
        public string Token { get; set; }
        
        public long Expire { get; set; }
    }

    public interface IAuthService
    {
        Task<ResultBase> Login(string email, string password);
        
        Task<ResultBase> Register(string email, string password);
        
        Task<ResultBase> Logout(string token);
        
        AuthService.Payload Decode(string token);
    }
}