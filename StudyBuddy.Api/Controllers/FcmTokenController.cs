using Microsoft.AspNetCore.Mvc;
using StudyBuddy.BusinessLogic;
using StudyBuddy.Model;
using StudyBuddy.Persistence;

namespace StudyBuddy.Api
{
    public class FcmTokenController : Controller
    {
        private readonly FcmTokenService service;

        public FcmTokenController(IRepository repository)
        {
            service = new FcmTokenService(repository);
        }

        [Route("/FcmToken/")]
        [HttpGet]
        [IsAdmin]
        public IActionResult Get()
        {
            var current_user = HttpContext.Items["user"] as User;
            return Json(service.All(current_user));
        }


        [Route("/FcmToken/")]
        [HttpPost]
        [IsLoggedIn]
        public IActionResult Insert([FromBody] FcmTokenSaveDto obj)
        {
            var token = obj.ToFcmToken();
            var current_user = HttpContext.Items["user"] as User;
            token.UserID = current_user.ID;
            
            service.Save(token);

            return null;
        }
    }
}