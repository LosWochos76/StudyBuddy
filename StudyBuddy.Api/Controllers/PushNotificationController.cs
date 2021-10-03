using Microsoft.AspNetCore.Mvc;
using StudyBuddy.BusinessLogic;
using StudyBuddy.Model;
using StudyBuddy.Persistence;

namespace StudyBuddy.Api
{
    public class PushNotificationController : Controller
    {
        public IRepository repository;

        public PushNotificationController(IRepository repository)
        {
            this.repository = repository;
        }

        [Route("/Notification/")]
        [HttpPost]
        [IsAdmin]
        public IActionResult BroadcastMessage([FromBody] PushNotificationBroadcastDto obj)
        {
            var service = new PushNotificationService(repository, HttpContext.Items["user"] as User);
            service.BroadcastMessage(obj.title, obj.body);
            return Json(true);
        }
    }
}