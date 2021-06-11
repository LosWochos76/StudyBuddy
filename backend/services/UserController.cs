using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace StudyBuddy.Services
{
    public class UserController : Controller
    {
        private StudyBuddyContext context;

        public UserController(StudyBuddyContext context)
        {
            this.context = context;
        }

        [Route("/User/")]
        [HttpGet]
        public IActionResult Get()
        {
            var objects = context.Users.ToList();
            return Json(objects);
        }

        [Route("/User/{id}")]
        [HttpGet]
        public IActionResult Get(int id)
        {
            var obj = context.Users.Find(id);
            return Json(obj);
        }
    }
}