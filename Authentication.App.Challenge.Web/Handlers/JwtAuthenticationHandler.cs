using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Authentication.App.Challenge.Repositories.Database.Redis;
using Authentication.App.Challenge.Services.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Authentication.App.Challenge.Web.Handlers
{
    public class JwtAuthenticationOptions : AuthenticationSchemeOptions
    {
    }

    public class JwtAuthenticationHandler : AuthenticationHandler<JwtAuthenticationOptions>
    {
        private readonly IAuthService _authService;
        private readonly IRedisRepository _redisRepository;

        public JwtAuthenticationHandler(IOptionsMonitor<JwtAuthenticationOptions> options, ILoggerFactory logger,
            UrlEncoder encoder, ISystemClock clock, IAuthService authService, IRedisRepository redisRepository) : base(options, logger, encoder, clock)
        {
            _authService = authService;
            _redisRepository = redisRepository;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return AuthenticateResult.Fail("Unauthorized");

            string authorizationHeader = Request.Headers["Authorization"];
            if (string.IsNullOrEmpty(authorizationHeader))
            {
                return AuthenticateResult.NoResult();
            }

            if (!authorizationHeader.StartsWith(JwtBearerDefaults.AuthenticationScheme,
                StringComparison.OrdinalIgnoreCase))
            {
                return AuthenticateResult.Fail("Unauthorized");
            }

            string token = authorizationHeader.Substring(JwtBearerDefaults.AuthenticationScheme.Length).Trim();

            if (string.IsNullOrEmpty(token))
            {
                return AuthenticateResult.Fail("Unauthorized");
            }

            try
            {
                return ValidateToken(token);
            }
            catch (Exception ex)
            {
                return AuthenticateResult.Fail(ex.Message);
            }
        }

        private AuthenticateResult ValidateToken(string token)
        {
            AuthService.Payload payload = _authService.Decode(token);
            
            if (payload == null || _redisRepository.Database.StringGet(token).HasValue)
            {
                return AuthenticateResult.Fail("Unauthorized");
            }

            List<Claim> claims = new List<Claim>
            {
                new(ClaimTypes.Sid, payload.Id.ToString()),
                new(ClaimTypes.Email, payload.Email),
                new(ClaimTypes.Expiration, payload.Exp.ToString())
            };
            
            ClaimsIdentity identity = new(claims, JwtBearerDefaults.AuthenticationScheme);
            GenericPrincipal principal = new(identity, null);
            AuthenticationTicket ticket = new(principal, JwtBearerDefaults.AuthenticationScheme);
            
            return AuthenticateResult.Success(ticket);
        }
    }
}