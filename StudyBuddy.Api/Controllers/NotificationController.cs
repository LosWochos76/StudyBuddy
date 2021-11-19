using Microsoft.AspNetCore.Mvc;
using StudyBuddy.BusinessLogic;

namespace StudyBuddy.Api.Controllers
{
    public class NotificationController : Controller
    {
        private readonly IBackend backend;

        public NotificationController(IBackend backend)
        {
            this.backend = backend;
        }
        
        [Route("/Notification/")]
        [HttpGet]
        public IActionResult GetAllNotifications([FromBody] CreateNotificationParameter obj)
        {
            var userId = backend.CurrentUser.ID;
            var notifications = backend.NotificationService.GetNotificationFromUser(userId);
            return Json(notifications);
        }
        
        [Route("/Notification/Feed")]
        [HttpGet]
        public IActionResult NotificationFeed([FromBody] CreateNotificationParameter obj)
        {
            var userId = backend.CurrentUser.ID;
            var notifications = backend.NotificationService.GetNotificationFeedForUser(userId);
            return Json(notifications);
        }
    }
}