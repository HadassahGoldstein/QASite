using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using QASite.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace QASite.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IConfiguration _configuration;
        public AccountController(IConfiguration configuration)
        {
            _configuration = configuration;

        }
        public IActionResult Signup()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Signup(User user,string password)
        {
            var connectionString = _configuration.GetConnectionString("ConStr");
            var repo = new QARepository(connectionString);
            repo.AddUser(user, password);
            return Redirect("/Account/Login");
        }
        public IActionResult Login()
        {
            ViewBag.Message = TempData["message"];
            return View();
        }
        [HttpPost]
        public IActionResult Login(string email,string password)
        {
            var connectionString = _configuration.GetConnectionString("ConStr");
            var repo = new QARepository(connectionString);
            var user = repo.Login(email, password);
            if (!user)
            {
                TempData["message"] = "Invalid email/password combo";
                return Redirect("/Account/Login");
            }
            var claims = new List<Claim>
            {
                new Claim("user",email)
            };
            HttpContext.SignInAsync(new ClaimsPrincipal(
                new ClaimsIdentity(claims, "cookies", "user", "role"))).Wait();
            return Redirect("/Home/NewQuestion");
        }
        [Authorize]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync().Wait();
            return Redirect("/");
        }

    }
}
