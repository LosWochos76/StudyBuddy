using Microsoft.AspNetCore.Mvc;
using StudyBuddy.BusinessLogic;
using StudyBuddy.Model;
using StudyBuddy.Persistence;

namespace StudyBuddy.Api
{
    public class RequestController : Controller
    {
        private readonly IRepository repository;

        public RequestController(IRepository repository)
        {
            this.repository = repository;
        }

        [Route("/Request/")]
        [HttpGet]
        [IsAdmin]
        public IActionResult Get()
        {
            var service = new RequestService(repository, HttpContext.Items["user"] as User);
            return Json(service.All());
        }

        [Route("/Request/ForRecipient/{user_id}")]
        [HttpGet]
        [IsLoggedIn]
        public IActionResult ForRecipient(int user_id)
        {
            var service = new RequestService(repository, HttpContext.Items["user"] as User);
            return Json(service.ForRecipient(user_id));
        }

        [Route("/Request/OfSender/{user_id}")]
        [HttpGet]
        [IsLoggedIn]
        public IActionResult OfSender(int user_id)
        {
            var service = new RequestService(repository, HttpContext.Items["user"] as User);
            return Json(service.OfSender(user_id));
        }

        [Route("/Request/{id}")]
        [HttpGet]
        [IsLoggedIn]
        public IActionResult GetById(int id)
        {
            var service = new RequestService(repository, HttpContext.Items["user"] as User);
            return Json(service.GetById(id));
        }

        [Route("/Request/{id}")]
        [HttpDelete]
        [IsAdmin]
        public IActionResult Delete(int id)
        {
            var service = new RequestService(repository, HttpContext.Items["user"] as User);
            service.Delete(id);
            return Json(new {Status = "Ok"});
        }

        [Route("/Request/")]
        [HttpPost]
        [IsLoggedIn]
        public IActionResult Insert([FromBody] Request obj)
        {
            var service = new RequestService(repository, HttpContext.Items["user"] as User);
            return Json(service.Insert(obj));
        }

        [Route("/Request/Accept/{id}")]
        [HttpPost]
        [IsLoggedIn]
        public IActionResult Accept(int id)
        {
            var service = new RequestService(repository, HttpContext.Items["user"] as User);
            service.Accept(id);
            return Json(new {Status = "Ok"});
        }

        [Route("/Request/Deny/{id}")]
        [HttpPost]
        [IsLoggedIn]
        public IActionResult Deny(int id)
        {
            var service = new RequestService(repository, HttpContext.Items["user"] as User);
            service.Deny(id);
            return Json(new { Status = "Ok" });
        }
    }
}