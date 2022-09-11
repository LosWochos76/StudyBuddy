using Microsoft.AspNetCore.Mvc;
using StudyBuddy.BusinessLogic;
using StudyBuddy.Model;

namespace StudyBuddy.Api.Controllers
{
    [Route("NotificationUserMetadata/")]
    public class NotificationUserMetadataController : Controller
    {
        private readonly IBackend backend;

        public NotificationUserMetadataController(IBackend backend)
        {
            this.backend = backend;
        }

        [HttpPost]
        public IActionResult Upsert([FromBody] NotificationUserMetadata obj)
        {
            backend.NotificationUserMetadataService.Upsert(obj);
            return Ok();
        }

        [HttpGet]
        public IActionResult GetAllLikes()
        {
            return Ok(backend.NotificationUserMetadataService.GetAll());
        }
    }
}