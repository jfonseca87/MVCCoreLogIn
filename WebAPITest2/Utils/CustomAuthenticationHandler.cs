using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using WebAPITest2.Models.DTOs;

namespace WebAPITest2.Utils
{
    public class CustomAuthenticationHandler : AuthenticationHandler<CustomAuthenticationOptions>
    {
        public CustomAuthenticationHandler(
            IOptionsMonitor<CustomAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock
        ) : base(options, logger, encoder, clock)
        {
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Token"))
            {
                return AuthenticateResult.Fail("Unauthorized");
            }

            string token = Request.Headers["Token"];
            if (string.IsNullOrEmpty(token))
            {
                return AuthenticateResult.Fail("Authorization");
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
            TokenGenerator tokenGenerator = new TokenGenerator();

            var jsonUser = tokenGenerator.Decrypt(Global.Key, token);
            var user = JsonConvert.DeserializeObject<UserDTO>(jsonUser);

            TimeSpan timeElapsed = DateTime.UtcNow - user.FechaGeneracion;

            if (timeElapsed.TotalMinutes > 5)
            {
                return AuthenticateResult.Fail("Unauthorized");
            }

            var claims = new List<Claim>
                {
                    new Claim("Id", user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.NomUser),
                    new Claim(ClaimTypes.Role, user.NomRol),
                };

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new System.Security.Principal.GenericPrincipal(identity, null);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            return AuthenticateResult.Success(ticket);
        }
    }
}
