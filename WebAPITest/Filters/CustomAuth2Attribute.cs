using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebAPITest.Services;

namespace WebAPITest.Filters
{
    public class CustomAuth2Attribute : ActionFilterAttribute, IAuthorizationFilter
    {
        public CustomAuth2Attribute()
        {
        }

        public string Roles { get; set; }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var request = context.HttpContext.Request;

            if (!request.Headers.ContainsKey("Token"))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            AuthenticateMethod2 method2 = new AuthenticateMethod2();
            string token = request.Headers["Token"];

            if (!method2.ValidateToken(token))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            if (!string.IsNullOrEmpty(Roles))
            {
                string rolToken = method2.GetRol(token);

                if (!ValidateRol(rolToken))
                {
                    context.Result = new UnauthorizedResult();
                }
            }
        }

        private bool ValidateRol(string rol)
        {
            string[] arrayRoles = Roles.Split(',');

            return arrayRoles.Contains(rol.Trim());
        }
    }
}
