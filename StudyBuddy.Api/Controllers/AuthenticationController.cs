using Microsoft.AspNetCore.Mvc;
using StudyBuddy.Persistence;
using StudyBuddy.BusinessLogic;

namespace StudyBuddy.Api
{
    public class AuthenticationController : Controller
    {
        private AuthenticationService service;

        public AuthenticationController(IRepository repository)
        {
            this.service = new AuthenticationService(repository);
        }

        [Route("/Login/")]
        [HttpPost]
        public IActionResult Index([FromBody] UserCredentials uc)
        {
            return Json(service.Login(uc));
        }

        [Route("/Login/SendPasswortResetMail/")]
        [HttpPost]
        public IActionResult SendPasswortResetMail([FromBody] string email)
        {
            service.SendPasswortResetMail(email);
            return Json(new { Status = "ok" });
        }
    }
}