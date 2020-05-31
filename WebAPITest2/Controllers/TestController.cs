using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPITest2.Controllers
{
    [Authorize]
    [ApiController]
    public class TestController : ControllerBase
    {
        [Route("api/test1")]
        [HttpGet]
        public IActionResult Test1()
        {
            return Ok("Test 1");
        }

        [Authorize(Roles = "Administrador")]
        [Route("api/test2")]
        [HttpGet]
        public IActionResult Test2()
        {
            return Ok(new { Message = "Test 2" });
        }
    }
}
