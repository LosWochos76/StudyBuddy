using Microsoft.AspNetCore.Mvc;
using StudyBuddy.BusinessLogic;
using StudyBuddy.Model;

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
        public IActionResult Get()
        {
            return Json(backend.RequestService.All());
        }

        [Route("/Request/ForRecipient/{user_id}")]
        [HttpGet]
        public IActionResult ForRecipient(int user_id)
        {
            return Json(backend.RequestService.ForRecipient(user_id));
        }

        [Route("/Request/OfSender/{user_id}")]
        [HttpGet]
        public IActionResult OfSender(int user_id)
        {
            return Json(backend.RequestService.OfSender(user_id));
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