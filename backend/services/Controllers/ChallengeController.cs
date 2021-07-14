using System.Linq;
using StudyBuddy.Model;
using Microsoft.AspNetCore.Mvc;
using StudyBuddy.Persistence;

namespace StudyBuddy.Services
{
    public class ChallengeController : Controller
    {
        private User current_user;
        private IRepository repository;

        public ChallengeController(IRepository repository)
        {
            this.repository = repository;
            this.current_user = HttpContext.Items["user"] as User;
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
        public IActionResult Get(int id)
        {
            var obj = repository.Challenges.ById(id);
            return Json(obj);
        }
    }
}