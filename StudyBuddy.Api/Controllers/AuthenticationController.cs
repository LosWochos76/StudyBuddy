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
            return Json(new LoginResult { Status = 0 });
        }

        [Route("/Login/ResetPassword/")]
        [HttpPost]
        public IActionResult ResetPassword([FromBody] ResetPasswordData data)
        {
            return Json(backend.UserService.ResetPassword(data));
        }

        [Route("/Login/SendVerificationMail")]
        [HttpPost]
        public IActionResult SendVerificationMail([FromBody] string email)
        {
            backend.AuthenticationService.SendMail(email, false);
            return Json(new LoginResult { Status = 0 }); 
        }

        [Route("/Login/VerifyEmail")]
        [HttpPost]
        public IActionResult VerifyEmail([FromBody] VerifyEmailData data)
        {
            return Json(backend.UserService.VerifyEmail(data));
        }

        [Route("/Login/EnableAccount")]
        [HttpPost]
        public IActionResult EnableAccount([FromBody] UserCredentials uc)
        {
            User user = backend.Repository.Users.ByEmailAllAccounts(uc.EMail);
            LoginResult response = backend.AuthenticationService.Login(uc);
            if (response.Status != 7)
                return Json(response);
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