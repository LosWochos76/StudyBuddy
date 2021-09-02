using System.Linq;
using StudyBuddy.Model;
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
        [IsAdmin]
        public IActionResult Get()
        {
            var objects = repository.Users.All();
            return Json(objects);
        }

        [Route("/User/Count")]
        [HttpGet]
        [IsAdmin]
        public IActionResult GetCount()
        {
            return Json(repository.Users.Count());
        }

        [Route("/User/{id}")]
        [HttpGet]
        public IActionResult GetById(int id)
        {
            var user = HttpContext.Items["user"] as User;
            if (user == null || (!user.IsAdmin && user.ID != id))
                return Json(new { Error = "Unauthorized" });

            var obj = repository.Users.ById(id);
            return Json(obj);
        }

        [Route("/User/{id}")]
        [HttpPut]
        public IActionResult Update([FromBody] User obj)
        {
            var user = HttpContext.Items["user"] as User;
            if (user == null || (!user.IsAdmin && user.ID != obj.ID))
                return Json(new { Error = "Unauthorized" });

            repository.Users.Update(obj);
            return Json(obj);
        }

        [Route("/User/")]
        [HttpPost]
        public IActionResult Insert([FromBody] User obj)
        {
            repository.Users.Insert(obj);
            return Json(obj);
        }

        [Route("/User/{id}")]
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var user = HttpContext.Items["user"] as User;
            if (user == null || (!user.IsAdmin && user.ID != id))
                return Json(new { Error = "Unauthorized" });

            var teams = repository.Teams.TeamsInWhichUserIsMember(id);
            if (teams.Any())
                return Json(new { Error = "User is member of teams!" });

            repository.Users.Delete(id);
            return Json(new { Status = "ok" });
        }

        [Route("/User/UserIdByNickname/{nickname}")]
        [HttpGet]
        public IActionResult UserIdByNickname(string nickname)
        {
            var obj = repository.Users.ByNickname(nickname);
            if (obj == null)
                return Json(new { Error = "User not found!" });
            else
                return Json(new { Id = obj.ID });
        }

        [Route("/User/UserIdByEmail/{email}")]
        [HttpGet]
        public IActionResult UserIdByEmail(string email)
        {
            var obj = repository.Users.ByEmail(email);
            if (obj == null)
                return Json(new { Error = "User not found!" });
            else
                return Json(new { Id = obj.ID });
        }
    }
}