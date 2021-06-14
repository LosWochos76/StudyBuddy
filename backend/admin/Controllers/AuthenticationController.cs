using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StudyBuddy.Persistence;
using Microsoft.AspNetCore.Http;

namespace StudyBuddy.Admin.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly ILogger<AuthenticationController> logger;
        private readonly IRepository repository;

        public AuthenticationController(ILogger<AuthenticationController> logger, IRepository repository)
        {
            this.logger = logger;
            this.repository = repository;
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult CheckLogin(LoginModel login)
        {
            if (!ModelState.IsValid)
                return View("Login", login);
    
            var user = repository.Users.FindByEmailAndPassword(login.EMail, login.Password);
            if (user == null)
            {
                ModelState.AddModelError("", "User not found");
                return View("Login", login);
            }

            HttpContext.Session.SetInt32("user_id", user.ID);
            return Redirect("/Challenge/Index");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("user_id");
            return Redirect("/Authentication/Login");
        }
    }
}
