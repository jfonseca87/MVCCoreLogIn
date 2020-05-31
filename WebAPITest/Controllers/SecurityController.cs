using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPITest.Filters;
using WebAPITest.Models;
using WebAPITest.Services;

namespace WebAPITest.Controllers
{   
    [ApiController]
    public class SecurityController : ControllerBase
    {
        private readonly AunthenticateMethod1 method1;
        private readonly AuthenticateMethod2 method2;

        public SecurityController(AunthenticateMethod1 _method1, AuthenticateMethod2 _method2)
        {
            method1 = _method1;
            method2 = _method2;
        }

        [Route("api/v1/authenticate")]
        [HttpPost]
        public IActionResult Authenticate(Usuario usuario) 
        {
            string result = method1.Auth(usuario);

            if (result.Equals("NotAuthorized"))
            {
                return Unauthorized();
            }

            return Ok(result);
        }

        [Route("api/v1/authenticate2")]
        [HttpPost]
        public IActionResult Authenticate2(Usuario usuario)
        {
            string result = method2.Auth(usuario);

            if (result.Equals("NotAuthorized"))
            {
                return Unauthorized();
            }

            return Ok(result);
        }

        [CustomAuth()]
        [Route("api/service1")]
        [HttpGet]
        public IActionResult Service1()
        {
            return Ok();
        }

        [CustomAuth2]
        [Route("api/service2")]
        [HttpGet]
        public IActionResult Service2()
        {
            return Ok();
        }

        [CustomAuth(Roles = "Administrador")]
        [Route("api/service3")]
        [HttpGet]
        public IActionResult Service3()
        {
            return Ok();
        }

        [CustomAuth2(Roles = "Administrador")]
        [Route("api/service4")]
        [HttpGet]
        public IActionResult Service4()
        {
            return Ok();
        }

        [CustomAuth2]
        [Route("api/refreshToken")]
        [HttpGet]
        public IActionResult RefreshToken(string token)
        {
            return Ok(method2.RefreshToken(token));
        }
    }
}
