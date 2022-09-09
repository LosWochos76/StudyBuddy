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
        public IActionResult Insert([FromBody] Comment insert)
        {
            return Json(backend.CommentService.Insert(insert));
        }

        [HttpGet]
        public IActionResult GetAll([FromQuery] CommentFilter filter)
        {
            return Json(backend.CommentService.GetAll(filter));
        }
    }
}