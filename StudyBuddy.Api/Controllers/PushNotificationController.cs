using Microsoft.AspNetCore.Mvc;
using StudyBuddy.BusinessLogic;

namespace StudyBuddy.Api
{
    public class PushNotificationController : Controller
    {
        private readonly IBackend backend;

        public PushNotificationController(IBackend backend)
        {
            this.backend = backend;
        }

        [Route("/Notification/")]
        [HttpPost]
        public IActionResult BroadcastMessage([FromBody] PushNotificationBroadcastDto obj)
        {
            backend.PushNotificationService.BroadcastMessage(obj);
            return Json(true);
        }
    }
}