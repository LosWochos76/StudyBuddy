using Microsoft.AspNetCore.Mvc;
using StudyBuddy.BusinessLogic;
using StudyBuddy.Model;
using StudyBuddy.Model.Filter;

namespace StudyBuddy.Api.Controllers
{
    [Route("Comment/")]
    public class CommentController : Controller
    {
        private readonly IBackend backend;

        public CommentController(IBackend backend)
        {
            this.backend = backend;
        }

        [HttpPost]
        public IActionResult CreateComment([FromBody] CommentInsert insert)
        {
            backend.CommentService.CreateComment(insert);

            // backend.PushNotificationService.SendUserCommentNotification(insert.);

            return Ok();
        }

        [HttpGet]
        public IActionResult GetAllLikes([FromQuery] CommentFilter filter)
        {
            var likes = backend.CommentService.GetAll(filter);
            return Ok(likes);
        }
    }
}