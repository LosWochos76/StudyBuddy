using Microsoft.AspNetCore.Mvc;
using StudyBuddy.BusinessLogic;
using StudyBuddy.Model;

namespace StudyBuddy.Api.Controllers
{
    public class BusinessEventController : Controller
    {
        private readonly IBackend backend;

        public BusinessEventController(IBackend backend)
        {
            this.backend = backend;
        }

        [Route("/BusinessEvent/")]
        [HttpGet]
        public IActionResult Get([FromQuery] BusinessEventFilter filter)
        {
            return Json(backend.BusinessEventService.All(filter));
        }

        [Route("/BusinessEvent/{id}")]
        [HttpGet]
        public IActionResult GetById(int id)
        {
            return Json(backend.BusinessEventService.GetById(id));
        }

        [Route("/BusinessEvent/{id}")]
        [HttpPut]
        public IActionResult Update([FromBody] BusinessEvent obj)
        {
            return Json(backend.BusinessEventService.Update(obj));
        }

        [Route("/BusinessEvent/")]
        [HttpPost]
        public IActionResult Insert([FromBody] BusinessEvent obj)
        {
            return Json(backend.BusinessEventService.Insert(obj));
        }

        [Route("/BusinessEvent/{id}")]
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            backend.BusinessEventService.Delete(id);
            return Json(new { Status = "ok" });
        }
    }
}