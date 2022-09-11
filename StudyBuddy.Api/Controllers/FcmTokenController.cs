using Microsoft.AspNetCore.Mvc;
using StudyBuddy.BusinessLogic;
using StudyBuddy.Model;

namespace StudyBuddy.Api
{
    public class FcmTokenController : Controller
    {
        private readonly IBackend backend;

        public FcmTokenController(IBackend backend)
        {
            this.backend = backend;
        }

        [Route("/FcmToken/")]
        [HttpGet]
        public IActionResult Get([FromQuery] FcmTokenFilter filter)
        {
            if (filter == null)
                filter = new FcmTokenFilter();

            return Json(backend.FcmTokenService.GetAll(filter));
        }

        [Route("/FcmToken/")]
        [HttpPost]
        public IActionResult Insert([FromBody] FcmToken obj)
        {
            return Json(backend.FcmTokenService.Save(obj));
        }

        [Route("/FcmToken/")]
        [HttpDelete]
        public IActionResult DeleteOldTokens()
        {
            backend.FcmTokenService.DeleteOldTokens();
            return Json(new { Status = "ok" });
        }
    }
}