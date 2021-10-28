using Microsoft.AspNetCore.Mvc;
using StudyBuddy.BusinessLogic;
using StudyBuddy.Model;

namespace StudyBuddy.Api.Controllers
{
    public class LoggingController : Controller
    {
        private readonly IBackend backend;

        public LoggingController(IBackend backend)
        {
            this.backend = backend;
        }

        [Route("/Loggig/")]
        [HttpGet]
        public IActionResult GetAll([FromQuery] LogMessageFilter filter)
        {
            if (filter == null)
                filter = new LogMessageFilter();

            return Json(backend.Logging.All(filter));
        }

        [Route("/Logging/")]
        [HttpPost]
        public IActionResult Insert([FromBody] LogMessage obj)
        {
            backend.Logging.Log(obj);
            return Json(new { Status = "ok" });
        }
    }
}