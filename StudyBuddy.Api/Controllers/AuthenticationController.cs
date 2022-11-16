using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StudyBuddy.BusinessLogic;
using StudyBuddy.BusinessLogic.Parameters;
using StudyBuddy.Model;
using StudyBuddy.Model.Enum;

namespace StudyBuddy.Api
{
    public class AuthenticationController : Controller
    {
        private readonly IBackend backend;
        private readonly ILogger logger;

        public AuthenticationController(IBackend backend, ILogger<AuthenticationController> logger)
        {
            this.backend = backend;
            this.logger = logger;
            logger.LogInformation("Creating AuthenticationController");
        }

        [Route("/Login/")]
        [HttpPost]
        public IActionResult Login([FromBody] UserCredentials uc)
        {
            logger.LogInformation("AuthenticationController.Login");
            return Json(backend.AuthenticationService.Login(uc));
        }

        [Route("/Login/")]
        [HttpPut]
        public IActionResult CheckToken([FromBody] string token)
        {
            logger.LogInformation("AuthenticationController.CheckToken");
            return Json(backend.AuthenticationService.CheckToken(token));
        }

        [Route("/Login/SendPasswortResetMail/")]
        [HttpPost]
        public IActionResult SendPasswortResetMail([FromBody] string email)
        {
            logger.LogInformation("AuthenticationController.SendPasswortResetMail");
            //backend.AuthenticationService.SendMail(email, true);
            backend.AuthenticationService.SendPasswordResetMail(email);
            return Json(new LoginResult() { Status = LoginStatus.Success });
        }

        [Route("/Login/ResetPassword/")]
        [HttpPost]
        public IActionResult ResetPassword([FromBody] ResetPasswordData data)
        {
            logger.LogInformation("AuthenticationController.ResetPassword");
            return Json(backend.UserService.ResetPassword(data));
        }

        [Route("/Login/SendVerificationMail")]
        [HttpPost]
        public IActionResult SendVerificationMail([FromBody] string email)
        {
            logger.LogInformation("AuthenticationController.SendVerificationMail");
            //backend.AuthenticationService.SendMail(email, false);
            backend.AuthenticationService.SendVerificationMail(email);
            return Json(new LoginResult() { Status = LoginStatus.Success }); 
        }

        [Route("/Login/VerifyEmail")]
        [HttpPost]
        public IActionResult VerifyEmail([FromBody] VerifyEmailData data)
        {
            logger.LogInformation("AuthenticationController.VerifyEmail");
            return Json(backend.UserService.VerifyEmail(data));
        }

        [Route("/Login/EnableAccount")]
        [HttpPost]
        public IActionResult EnableAccount([FromBody] UserCredentials uc)
        {
            logger.LogInformation("AuthenticationController.EnableAccount");

            User user = backend.Repository.Users.ByEmailAllAccounts(uc.EMail);
            LoginResult response = backend.AuthenticationService.Login(uc);
            if (response.Status != LoginStatus.AccountDisabled)
                return Json(response);

            User updatedUser = backend.UserService.EnableAccount(user);
            if (updatedUser == null)
            {
                response.User = null;
                response.Token = null;
                response.Status = LoginStatus.UndocumentedError;
                return Json(response);
            }

            response.User = updatedUser;
            return Json(response);
        }
    }
}