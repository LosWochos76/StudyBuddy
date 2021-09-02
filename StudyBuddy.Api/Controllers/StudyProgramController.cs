using System.Linq;
using StudyBuddy.Model;
using Microsoft.AspNetCore.Mvc;
using StudyBuddy.Persistence;

namespace StudyBuddy.Services
{
    public class StudyProgramController : Controller
    {
        private IRepository repository;

        public StudyProgramController(IRepository repository)
        {
            this.repository = repository;
        }

        [Route("/StudyProgram/")]
        [HttpGet]
        public IActionResult Get()
        {
            var objects = repository.StudyPrograms.All();
            return Json(objects);
        }

        [Route("/StudyProgram/{id}")]
        [HttpGet]
        public IActionResult GetById(int id)
        {
            var obj = repository.StudyPrograms.ById(id);
            return Json(obj);
        }

        [Route("/StudyProgram/{id}")]
        [HttpPut]
        [IsAdmin]
        public IActionResult Update([FromBody] StudyProgram obj)
        {
            repository.StudyPrograms.Update(obj);
            return Json(obj);
        }

        [Route("/StudyProgram/")]
        [HttpPost]
        [IsAdmin]
        public IActionResult Insert([FromBody] StudyProgram obj)
        {
            repository.StudyPrograms.Insert(obj);
            return Json(obj);
        }

        [Route("/StudyProgram/{id}")]
        [HttpDelete]
        [IsAdmin]
        public IActionResult Delete(int id)
        {
            repository.StudyPrograms.Delete(id);
            return Json(new { Status = "ok" });
        }
    }
}