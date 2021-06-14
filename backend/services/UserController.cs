using System.Linq;
using Microsoft.AspNetCore.Mvc;
using StudyBuddy.Persistence;

namespace StudyBuddy.Services
{
    public class UserController : Controller
    {
        private IRepository repository;

        public UserController(IRepository repository)
        {
            this.repository = repository;
        }

        [Route("/User/")]
        [HttpGet]
        public IActionResult Get()
        {
            var objects = repository.Users.All();
            return Json(objects);
        }

        [Route("/User/{id}")]
        [HttpGet]
        public IActionResult Get(int id)
        {
            var obj = repository.Users.ById(id);
            return Json(obj);
        }
    }
}