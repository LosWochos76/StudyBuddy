using StudyBuddy.Model;
using Microsoft.AspNetCore.Mvc;
using StudyBuddy.Persistence;
using System;
using StudyBuddy.BusinessLogic;

namespace StudyBuddy.Api
{
    public class TermController : Controller
    {
        private TermService service;

        public TermController(IRepository repository)
        {
            this.service = new TermService(repository);
        }

        [Route("/Term/")]
        [HttpGet]
        public IActionResult Get()
        {
            return Json(service.All());
        }

        [Route("/Term/{id}")]
        [HttpGet]
        public IActionResult GetById(int id)
        {
            return Json(service.GetById(id));
        }

        [Route("/Term/{id}")]
        [HttpPut]
        [IsAdmin]
        public IActionResult Update([FromBody] Term obj)
        {
            return Json(service.Update(obj));
        }

        [Route("/Term/")]
        [HttpPost]
        [IsAdmin]
        public IActionResult Insert([FromBody] Term obj)
        {
            return Json(service.Insert(obj));
        }

        [Route("/Term/{id}")]
        [HttpDelete]
        [IsAdmin]
        public IActionResult Delete(int id)
        {
            service.Delete(id);
            return Json(new { Status = "ok" });
        }

        [Route("/Term/ByDate/{date}")]
        [HttpGet]
        public IActionResult Delete(DateTime date)
        {
            return Json(service.ByDate(date));
        }
    }
}