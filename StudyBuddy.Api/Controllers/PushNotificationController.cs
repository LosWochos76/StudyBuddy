using Microsoft.AspNetCore.Mvc;
using StudyBuddy.BusinessLogic;
using StudyBuddy.Model;
using StudyBuddy.Persistence;

namespace StudyBuddy.Api
{
    public class PushNotificationController : Controller
    {
        public PushNotificationService pushNotificationService;

        public PushNotificationController(IRepository repository)
        {
            pushNotificationService = new PushNotificationService(repository);
        }
        
        
        [Route("/Notification/")]
        [HttpPost]
        [IsAdmin]
        public IActionResult BroadcastMessage([FromBody] PushNotificationBroadcastDto obj)
        {
            pushNotificationService.BroadcastMessage(obj.title, obj.body);
            return Json(true);
        }
    }
}