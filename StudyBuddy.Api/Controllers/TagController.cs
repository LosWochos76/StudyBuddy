using StudyBuddy.Model;
using Microsoft.AspNetCore.Mvc;
using StudyBuddy.Persistence;
using System;
using StudyBuddy.BusinessLogic;

namespace StudyBuddy.Api
{
    public class TagController : Controller
    {
        private IRepository repository;

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
            return Json(new { Status = "ok" });
        }

        [Route("/Tag/CreateOrFind/")]
        [HttpPost]
        public IActionResult CreateOrFind([FromBody] string tags)
        {
            var service = new TagService(repository, HttpContext.Items["user"] as User);
            return Json(service.CreateOrFindMultiple(tags));
        }
    }
}