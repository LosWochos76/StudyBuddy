using System.Linq;
using StudyBuddy.Model;
using Microsoft.AspNetCore.Mvc;
using StudyBuddy.Persistence;
using StudyBuddy.BusinessLogic;

namespace StudyBuddy.Api
{
    public class StudyProgramController : Controller
    {
        private StudyProgramService service;

        public StudyProgramController(IRepository repository)
        {
            this.service = new StudyProgramService(repository);
        }

        [Route("/StudyProgram/")]
        [HttpGet]
        public IActionResult Get()
        {
            return Json(service.All());
        }

        [Route("/StudyProgram/{id}")]
        [HttpGet]
        public IActionResult GetById(int id)
        {
            return Json(service.GetById(id));
        }

        [Route("/StudyProgram/{id}")]
        [HttpPut]
        [IsAdmin]
        public IActionResult Update([FromBody] StudyProgram obj)
        {
            return Json(service.Update(obj));
        }

        [Route("/StudyProgram/")]
        [HttpPost]
        [IsAdmin]
        public IActionResult Insert([FromBody] StudyProgram obj)
        {
            return Json(service.Insert(obj));
        }

        [Route("/StudyProgram/{id}")]
        [HttpDelete]
        [IsAdmin]
        public IActionResult Delete(int id)
        {
            service.Delete(id);
            return Json(new { Status = "ok" });
        }
    }
}