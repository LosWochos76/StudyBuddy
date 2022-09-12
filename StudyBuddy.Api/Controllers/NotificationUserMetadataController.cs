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
        public IActionResult Upsert([FromBody] NotificationUserMetadata obj)
        {
            backend.NotificationUserMetadataService.Upsert(obj);
            return Ok();
        }

        [HttpGet]
        public IActionResult GetAll([FromQuery] NotificationUserMetadataFilter filter)
        {
            return Ok(backend.NotificationUserMetadataService.GetAll(filter));
        }
        
        [Route("{id}")]
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            backend.NotificationUserMetadataService.Delete(id);
            return Accepted();
        }
    }
}