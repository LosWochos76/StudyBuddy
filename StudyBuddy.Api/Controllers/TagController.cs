using Microsoft.AspNetCore.Mvc;
using StudyBuddy.BusinessLogic;
using StudyBuddy.Model;
using StudyBuddy.Persistence;

namespace StudyBuddy.Api
{
    public class TagController : Controller
    {
        private readonly IRepository repository;

        public TagController(IRepository repository)
        {
            this.repository = repository;
        }

        [Route("/Tag/")]
        [HttpGet]
        public IActionResult GetAll()
        {
            var service = new TagService(repository, HttpContext.Items["user"] as User);
            return Json(service.GetAll());
        }

        [Route("/Tag/{id}")]
        [HttpGet]
        public IActionResult GetById(int id)
        {
            var service = new TagService(repository, HttpContext.Items["user"] as User);
            return Json(service.GetById(id));
        }

        [Route("/Tag/{id}")]
        [HttpPut]
        [IsAdmin]
        public IActionResult Update([FromBody] Tag obj)
        {
            var service = new TagService(repository, HttpContext.Items["user"] as User);
            return Json(service.Update(obj));
        }

        [Route("/Tag/")]
        [HttpPost]
        public IActionResult Insert([FromBody] Tag obj)
        {
            var service = new TagService(repository, HttpContext.Items["user"] as User);
            return Json(service.Insert(obj));
        }

        [Route("/Tag/{id}")]
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var service = new TagService(repository, HttpContext.Items["user"] as User);
            service.Delete(id);
            return Json(new {Status = "ok"});
        }

        [Route("/Tag/CreateOrFind/")]
        [HttpPost]
        public IActionResult CreateOrFind([FromBody] string tags)
        {
            var service = new TagService(repository, HttpContext.Items["user"] as User);
            return Json(service.CreateOrFindMultiple(tags));
        }

        [Route("/Tag/OfChallenge/{challenge_id}")]
        [HttpGet]
        public IActionResult OfChallenge(int challenge_id)
        {
            var service = new TagService(repository, HttpContext.Items["user"] as User);
            return Json(service.OfChallenge(challenge_id));
        }

        [Route("/Tag/SetForChallenge/")]
        [HttpPost]
        public IActionResult SetForChallenge([FromBody] SetTagsParameter parameter)
        {
            var service = new TagService(repository, HttpContext.Items["user"] as User);
            return Json(service.SetForChallenge(parameter));
        }
    }
}