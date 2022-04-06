using Microsoft.AspNetCore.Mvc;
using StudyBuddy.BusinessLogic;
using StudyBuddy.Model;
using StudyBuddy.Model.Filter;

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
        public IActionResult UpsertLike([FromBody] NotificationUserMetadataUpsert like)
        {
            backend.NotificationUserMetadataService.Upsert(like);
            return Ok();
        }

        [HttpGet]
        public IActionResult GetAllLikes([FromQuery] NotificationUserMetadataFilter filter)
        {
            var likes = backend.NotificationUserMetadataService.GetAll(filter);
            return Ok(likes);
        }
    }
}