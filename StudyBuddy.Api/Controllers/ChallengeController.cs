using System.Linq;
using StudyBuddy.Model;
using Microsoft.AspNetCore.Mvc;
using StudyBuddy.Persistence;

namespace StudyBuddy.Services
{
    public class ChallengeController : Controller
    {
        private IRepository repository;

        public ChallengeController(IRepository repository)
        {
            this.repository = repository;
        }

        [Route("/Challenge/")]
        [HttpGet]
        public IActionResult Get()
        {
            var objects = repository.Challenges.All();
            return Json(objects);
        }

        [Route("/Challenge/{id}")]
        [HttpGet]
        public IActionResult GetById(int id)
        {
            var current_user = HttpContext.Items["user"] as User;
            var obj = repository.Challenges.ById(id);
            return Json(obj);
        }

        [Route("/Challenge")]
        [HttpGet]
        public IActionResult GetAll(int id)
        {
            var current_user = HttpContext.Items["user"] as User;
            var obj = repository.Challenges.AllForUser(current_user.ID);
            return Json(obj);
        }
    }
}