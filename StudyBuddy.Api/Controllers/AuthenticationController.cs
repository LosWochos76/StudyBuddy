using Microsoft.AspNetCore.Mvc;
using StudyBuddy.BusinessLogic;

namespace StudyBuddy.Api
{
    public class AuthenticationController : Controller
    {
        private readonly IBackend backend;

        public AuthenticationController(IBackend backend)
        {
            this.backend = backend;
        }

        [Route("/Login/")]
        [HttpPost]
        public IActionResult Login([FromBody] UserCredentials uc)
        {
            return Json(backend.AuthenticationService.Login(uc));
        }

        [Route("/Login/")]
        [HttpPut]
        public IActionResult CheckToken([FromBody] string token)
        {
            return Json(backend.AuthenticationService.CheckToken(token));
        }

        [Route("/Login/SendPasswortResetMail/")]
        [HttpPost]
        public IActionResult SendPasswortResetMail([FromBody] string email)
        {
            backend.AuthenticationService.SendPasswortResetMail(email);
            return Json(new {Status = "ok"});
        }
    }
}