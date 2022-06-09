using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudyBuddy.BusinessLogic;
using StudyBuddy.BusinessLogic.Parameters;

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

        [Route("/Login2/")]
        [HttpPost]
        public IActionResult Login2([FromBody] UserCredentials uc)
        {
            var user = backend.Repository.Users.ByEmail(uc.EMail);
            if (user == null)
                return NotFound("User not found!");
            if (!user.EmailConfirmed)
                return Unauthorized("Email not confirmed");
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
            backend.AuthenticationService.SendMail(email, true);
            return Json(new {Status = "ok"});
        }

        [Route("/Login/ResetPassword/")]
        [HttpPost]
        public IActionResult ResetPassword([FromBody] ResetPasswordData data)
        {
            var user = backend.Repository.Users.ByEmail(data.Email);
            if (user == null)
                return Json(new { Status = "User not found" });
            if (backend.AuthenticationService.CheckPasswordResetToken(data.Token, user.PasswordHash))
            {
                user.Password = data.Password;
                backend.UserService.ResetPassword(user);
                return Json(new { Status = "ok" });
            }

            return Json(new { Status = "Invalid request" });
        }

        [Route("/Login/SendVerificationMail")]
        [HttpPost]
        public IActionResult SendVerificationMail([FromBody] string email)
        {
            backend.AuthenticationService.SendMail(email, false);
            return Json(new { Status = "ok" });
        }

        [Route("/Login/VerifyEmail")]
        [HttpPost]
        public IActionResult VerifyEmail([FromBody] VerifyEmailData data)
        {
            var user = backend.Repository.Users.ByEmail(data.Email);
            if (user == null)
                return NotFound(new { Id = data.Email, error = "User not found." });
            if (backend.AuthenticationService.CheckPasswordResetToken(data.Token, user.PasswordHash))
            {
                user.EmailConfirmed = true;
                backend.UserService.VerifyEmail(user);
                return Ok();
            }
            return Json(new { status = "Invalid request" });
        }
    }
}