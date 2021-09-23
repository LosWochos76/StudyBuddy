using Microsoft.AspNetCore.Mvc;
using StudyBuddy.Persistence;
using StudyBuddy.BusinessLogic;
using StudyBuddy.Model;

namespace StudyBuddy.Api
{
    public class AuthenticationController : Controller
    {
        private IRepository repository;

        public AuthenticationController(IRepository repository)
        {
            this.repository = repository;
        }

        [Route("/Login/")]
        [HttpPost]
        public IActionResult Login([FromBody] UserCredentials uc)
        {
            var service = new AuthenticationService(repository, HttpContext.Items["user"] as User);
            return Json(service.Login(uc));
        }

        [Route("/Login/SendPasswortResetMail/")]
        [HttpPost]
        public IActionResult SendPasswortResetMail([FromBody] string email)
        {
            var service = new AuthenticationService(repository, HttpContext.Items["user"] as User);
            service.SendPasswortResetMail(email);
            return Json(new { Status = "ok" });
        }
    }
}