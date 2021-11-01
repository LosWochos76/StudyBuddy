using Microsoft.AspNetCore.Mvc;
using StudyBuddy.BusinessLogic;
using StudyBuddy.Model;

namespace StudyBuddy.Api
{
    public class TagController : Controller
    {
        private readonly IBackend backend;

        public TagController(IBackend backend)
        {
            this.backend = backend;
        }

        [Route("/Tag/")]
        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(backend.TagService.GetAll());
        }

        [Route("/Tag/{id}")]
        [HttpGet]
        public IActionResult GetById(int id)
        {
            return Json(backend.TagService.GetById(id));
        }

        [Route("/Tag/{id}")]
        [HttpPut]
        public IActionResult Update([FromBody] Tag obj)
        {
            return Json(backend.TagService.Update(obj));
        }

        [Route("/Tag/")]
        [HttpPost]
        public IActionResult Insert([FromBody] Tag obj)
        {
            return Json(backend.TagService.Insert(obj));
        }

        [Route("/Tag/{id}")]
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            backend.TagService.Delete(id);
            return Json(new {Status = "ok"});
        }

        [Route("/Tag/CreateOrFind/")]
        [HttpPost]
        public IActionResult CreateOrFind([FromBody] string tags)
        {
            return Json(backend.TagService.CreateOrFindMultiple(tags));
        }
    }
}