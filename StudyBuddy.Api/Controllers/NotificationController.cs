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
        
        [Route("/Notification/Feed")]
        [HttpGet]
        public IActionResult NotificationFeed([FromQuery] NotificationFilter filter)
        {
            logger.LogInformation("NotificationController.NotificationFeed");
            var notifications = backend.NotificationService.GetNotificationsForFriends(filter);
            return Json(notifications);
        }

        [Route("/Notification/")]
        [HttpGet]
        public IActionResult GetAll([FromQuery] NotificationFilter filter)
        {
            logger.LogInformation("NotificationController.GetAll");
            return Json(backend.NotificationService.GetAll(filter));
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

        [Route("/Notification/{id}/Likers")]
        [HttpGet]
        public IActionResult GetAllLikersForNotification(int id)
        {
            logger.LogInformation("NotificationController.GetAllLikersForNotification");
            return Json(backend.UserService.GetAllLikersForNotification(id));
        }
    }
}