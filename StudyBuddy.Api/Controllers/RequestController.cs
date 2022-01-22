using Microsoft.AspNetCore.Mvc;
using StudyBuddy.BusinessLogic;
using StudyBuddy.Model;
using StudyBuddy.Model.Filter;

namespace StudyBuddy.Api
{
    public class RequestController : Controller
    {
        private readonly IBackend backend;

        public RequestController(IBackend backend)
        {
            this.backend = backend;
        }

        [Route("/Request/")]
        [HttpGet]
        public IActionResult Get([FromQuery] RequestFilter filter)
        {
            return Json(backend.RequestService.All(filter));
        }

        [Route("/Request/{id}")]
        [HttpGet]
        public IActionResult GetById(int id)
        {
            return Json(backend.RequestService.GetById(id));
        }

        [Route("/Request/{id}")]
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            backend.RequestService.Delete(id);
            return Json(new {Status = "Ok"});
        }

        [Route("/Request/")]
        [HttpPost]
        public IActionResult Insert([FromBody] Request obj)
        {
            return Json(backend.RequestService.Insert(obj));
        }

        [Route("/Request/Accept/{id}")]
        [HttpPost]
        public IActionResult Accept(int id)
        {
            backend.RequestService.Accept(id);
            return Json(new {Status = "Ok"});
        }

        [Route("/Request/Deny/{id}")]
        [HttpPost]
        public IActionResult Deny(int id)
        {
            backend.RequestService.Deny(id);
            return Json(new { Status = "Ok" });
        }
    }
}