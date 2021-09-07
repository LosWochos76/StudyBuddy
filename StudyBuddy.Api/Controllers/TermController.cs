using System.Linq;
using StudyBuddy.Model;
using Microsoft.AspNetCore.Mvc;
using StudyBuddy.Persistence;
using System;

namespace StudyBuddy.Api
{
    public class TermController : Controller
    {
        private IRepository repository;

        public TermController(IRepository repository)
        {
            this.repository = repository;
        }

        [Route("/Term/")]
        [HttpGet]
        public IActionResult Get()
        {
            var objects = repository.Terms.All();
            return Json(objects);
        }

        [Route("/Term/{id}")]
        [HttpGet]
        public IActionResult GetById(int id)
        {
            var obj = repository.Terms.ById(id);
            return Json(obj);
        }

        [Route("/Term/{id}")]
        [HttpPut]
        [IsAdmin]
        public IActionResult Update([FromBody] Term obj)
        {
            repository.Terms.Update(obj);
            return Json(obj);
        }

        [Route("/Term/")]
        [HttpPost]
        [IsAdmin]
        public IActionResult Insert([FromBody] Term obj)
        {
            repository.Terms.Insert(obj);
            return Json(obj);
        }

        [Route("/Term/{id}")]
        [HttpDelete]
        [IsAdmin]
        public IActionResult Delete(int id)
        {
            repository.Terms.Delete(id);
            return Json(new { Status = "ok" });
        }

        [Route("/Term/ByDate/{date}")]
        [HttpGet]
        public IActionResult Delete(DateTime date)
        {
            return Json(repository.Terms.ByDate(date));
        }
    }
}