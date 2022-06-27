using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudyBuddy.BusinessLogic;
using StudyBuddy.BusinessLogic.Parameters;
using StudyBuddy.Model;

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
            var result = backend.AuthenticationService.Login(uc);

            if (result.Status == 3)
                return NotFound("User not found!");

            if (result.Status == 1)
                return Unauthorized("Email not confirmed");

            return Json(result);

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
            var user = backend.Repository.Users.ByEmailActiveAccounts(data.Email);
            if (user == null)
                return NotFound("User not found");
            if (backend.AuthenticationService.CheckPasswordResetToken(data.Token, user.PasswordHash))
            {
                user.Password = data.Password;
                User updatedUser = backend.UserService.ResetPassword(user);
                return Ok(updatedUser);
            }

            return BadRequest("Invalid request");
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
            var user = backend.Repository.Users.ByEmailActiveAccounts(data.Email);
            if (user == null)
                return NotFound(new { Id = data.Email, error = "User not found." });
            if (backend.AuthenticationService.CheckPasswordResetToken(data.Token, user.PasswordHash))
            {
                user.EmailConfirmed = true;
                backend.UserService.VerifyEmail(user);
                return Ok(user);
            }
            return Json(new { status = "Invalid request" });
        }

        [Route("/Login/EnableAccount")]
        [HttpPost]
        public IActionResult EnableAccount([FromBody] UserCredentials uc)
        {
            User user = backend.Repository.Users.ByEmailAllAccounts(uc.EMail);
            LoginResult response = backend.AuthenticationService.Login(uc);
            if (response.Status != 7)
                return Json(response);
            user.AccountActive = true;
            User updatedUser = backend.UserService.EnableAccount(user);
            if (updatedUser == null)
            {
                response.User = null;
                response.Token = null;
                response.Status = 6;
                return Json(response);
            }
            response.User = updatedUser;
            return Json(response);
        }
    }
}