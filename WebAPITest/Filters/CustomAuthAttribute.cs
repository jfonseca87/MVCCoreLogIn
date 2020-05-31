using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebAPITest.Services;

namespace WebAPITest.Filters
{
    public class CustomAuthAttribute : ActionFilterAttribute, IAuthorizationFilter
    {
        public CustomAuthAttribute()
        {
        }

        public string Roles { get; set; }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var request = context.HttpContext.Request;

            if (request.Path.HasValue && request.Path.Value.Contains("authenticate"))
            {
                return;
            }

            if (!request.Headers.ContainsKey("Token"))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            AunthenticateMethod1 method1 = new AunthenticateMethod1();
            string token = request.Headers["Token"];

            if (!method1.ValidateToken(token))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            if (!string.IsNullOrEmpty(Roles))
            {
                string rolDb = method1.GetRol(token);

                if (!ValidateRoles(rolDb)) 
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }
            }

            method1.RefreshTokenTime(token);
        }

        private bool ValidateRoles(string rolUser)
        {
            string[] arrayRoles = Roles.Split(',');

            return arrayRoles.Contains(rolUser);
        }
    }
}
