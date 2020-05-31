using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPITest2.Models.DTOs;
using WebAPITest2.Services;

namespace WebAPITest2.Controllers
{
    [Authorize]
    [ApiController]
    public class SecurityController : ControllerBase
    {
        private readonly Method1 method1;
        private readonly Method2 method2;

        public SecurityController(Method1 _method1, Method2 _method2)
        {
            method1 = _method1;
            method2 = _method2;
        }

        [AllowAnonymous]
        [Route("api/v1/authenticate")]
        [HttpPost]
        public IActionResult Authenticate(UserDTO user)
        {
            var result = method1.Authtenticate(user.NomUser, user.Password);

            if (result.Equals("NotAuthorized"))
            {
                return Unauthorized();
            }

            return Ok(result);
        }

        [AllowAnonymous]
        [Route("api/v2/authenticate")]
        [HttpPost]
        public IActionResult AuthenticateV2(UserDTO user)
        {
            var result = method2.Authtenticate(user.NomUser, user.Password);

            if (result.Equals("NotAuthorized"))
            {
                return Unauthorized();
            }

            return Ok(result);
        }
    }
}
