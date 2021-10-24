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
        public IActionResult Get()
        {
            return Json(backend.FcmTokenService.All());
        }

        [Route("/FcmToken/")]
        [HttpPost]
        public IActionResult Insert([FromBody] FcmTokenSaveDto obj)
        {
            return Json(backend.FcmTokenService.Save(obj));
        }
    }
}