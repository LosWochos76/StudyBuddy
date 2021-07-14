using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StudyBuddy.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Localization;
using System.Threading;
using System.Globalization;

namespace StudyBuddy.Admin.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IRepository repository;
        private readonly IStringLocalizer<SharedResources> localizer;

        public AuthenticationController(IStringLocalizer<SharedResources> localizer, IRepository repository)
        {
            this.localizer = localizer;
            this.repository = repository;
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult CheckLogin(LoginViewModel login)
        {
            if (!ModelState.IsValid)
                return View("Login", login);
    
            var user = repository.Users.ByEmailAndPassword(login.EMail, login.Password);
            if (user == null || user.Role == Model.Role.Student)
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
