using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVCLoginApp.Models;

namespace MVCLoginApp.Controllers
{
    [Authorize()]
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(User user)
        {
            var appClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, "Bob"),
                new Claim(ClaimTypes.Email, "Bob@fmail.com"),
                new Claim(ClaimTypes.DateOfBirth, "11/11/2000"),
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim(ClaimTypes.Role, "AdminTwo")
            };

            var appIdentity = new ClaimsIdentity(appClaims, "LoginApp");
            var userPrincipal = new ClaimsPrincipal(new[] { appIdentity });

            HttpContext.SignInAsync(userPrincipal);

            return RedirectToAction("Main");
        }

        public IActionResult LogOut()
        {
            HttpContext.SignOutAsync();
            
            return RedirectToAction("Main");
        }

        
        public IActionResult Main()
        {
            var asd = HttpContext.User;

            return View();
        }
    }
}
