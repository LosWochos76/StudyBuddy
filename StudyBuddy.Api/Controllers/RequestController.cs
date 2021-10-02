using Microsoft.AspNetCore.Mvc;
using StudyBuddy.BusinessLogic;
using StudyBuddy.Model;
using StudyBuddy.Persistence;

namespace StudyBuddy.Api
{
    public class RequestController : Controller
    {
        private readonly RequestService service;

        public RequestController(IRepository repository)
        {
            service = new RequestService(repository);
        }

        [Route("/Request/")]
        [HttpGet]
        [IsAdmin]
        public IActionResult Get()
        {
            return Json(service.All());
        }

        [Route("/Request/ForRecipient/{id}")]
        [HttpGet]
        [IsLoggedIn]
        public IActionResult ForRecipient(int id)
        {
            var current_user = HttpContext.Items["user"] as User;
            return Json(service.ForRecipient(current_user, id));
        }

        [Route("/Request/{id}")]
        [HttpGet]
        [IsLoggedIn]
        public IActionResult GetById(int id)
        {
            return Json(service.GetById(id));
        }

        [Route("/Request/{id}")]
        [HttpDelete]
        [IsAdmin]
        public IActionResult Delete(int id)
        {
            service.Delete(id);
            return Json(new {Status = "Ok"});
        }

        [Route("/Request/")]
        [HttpPost]
        [IsLoggedIn]
        public IActionResult Insert([FromBody] Request obj)
        {
            var current_user = HttpContext.Items["user"] as User;
            return Json(service.Insert(current_user, obj));
        }

        [Route("/Request/Accept/{id}")]
        [HttpPost]
        [IsLoggedIn]
        public IActionResult Accept(int id)
        {
            var current_user = HttpContext.Items["user"] as User;
            service.Accept(current_user, id);
            return Json(new {Status = "Ok"});
        }
    }
}