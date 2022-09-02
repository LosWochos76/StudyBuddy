using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StudyBuddy.BusinessLogic;
using StudyBuddy.Model;

namespace StudyBuddy.Api.Controllers
{
    public class NotificationController : Controller
    {
        private readonly IBackend backend;
        private readonly ILogger logger;

        public NotificationController(IBackend backend, ILogger<ChallengeController> logger)
        {
            this.backend = backend;
            this.logger = logger;
        }
        
        [Route("/Notification/")]
        [HttpGet]
        public IActionResult GetAllForUser()
        {
            logger.LogInformation("NotificationController.GetAllForUser");
            var userId = backend.CurrentUser.ID;
            var notifications = backend.NotificationService.GetNotificationFromUser(userId);
            return Json(notifications);
        }
        
        [Route("/Notification/Feed")]
        [HttpGet]
        public IActionResult NotificationFeed([FromQuery] NotificationFilter filter)
        {
            logger.LogInformation("NotificationController.NotificationFeed");
            var userId = backend.CurrentUser.ID;
            var notifications = backend.NotificationService.GetNotificationFeedForUser(userId, filter);
            return Json(notifications);
        }

        [Route("/v2/Notification/")]
        [HttpGet]
        public IActionResult GetAll([FromQuery] NotificationFilter filter)
        {
            logger.LogInformation("NotificationController.GetAll");
            var notifications = backend.NotificationService.GetAll(filter);
            return Json(notifications);
        }

        [Route("/Notification/{id}")]
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            logger.LogInformation("NotificationController.Delete");
            backend.NotificationService.Delete(id);
            return Json(new { Status = "ok" });
        }

        [Route("/Notification/{id}")]
        [HttpGet]
        public IActionResult ById(int id)
        {
            logger.LogInformation("NotificationController.ById");
            return Json(backend.NotificationService.ById(id));
        }
    }
}