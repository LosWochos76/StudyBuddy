using StudyBuddy.Model;
using Microsoft.AspNetCore.Mvc;
using StudyBuddy.Persistence;

namespace StudyBuddy.Api
{
    public class TeamController : Controller
    {
        private IRepository repository;

        public TeamController(IRepository repository)
        {
            this.repository = repository;
        }

        [Route("/Team/")]
        [HttpGet]
        [IsLoggedIn]
        public IActionResult Get()
        {
            var objects = repository.Teams.All();
            return Json(objects);
        }

        [Route("/Team/Count")]
        [HttpGet]
        [IsLoggedIn]
        public IActionResult GetCount()
        {
            return Json(repository.Teams.Count());
        }

        [Route("/Team/{id}")]
        [HttpGet]
        [IsLoggedIn]
        public IActionResult GetById(int id)
        {
            var obj = repository.Teams.ById(id);
            return Json(obj);
        }

        [Route("/Team/{id}")]
        [HttpPut]
        [IsLoggedIn]
        public IActionResult Update([FromBody] Team obj)
        {
            var user = HttpContext.Items["user"] as User;
            if (user == null || (!user.IsAdmin && user.ID != obj.OwnerID))
                return Json(new { Error = "Unauthorized" });

            repository.Teams.Update(obj);
            return Json(obj);
        }

        [Route("/Team/")]
        [HttpPost]
        [IsLoggedIn]
        public IActionResult Insert([FromBody] Team obj)
        {
            repository.Teams.Insert(obj);
            return Json(obj);
        }

        [Route("/Team/{id}")]
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var team = repository.Teams.ById(id);
            var user = HttpContext.Items["user"] as User;
            if (user == null || (!user.IsAdmin && user.ID != team.OwnerID))
                return Json(new { Error = "Unauthorized" });

            repository.Teams.Delete(id);
            repository.Teams.DeleteMembers(id);
            return Json(new { Status = "ok" });
        }

        [Route("/Team/Members/{id}")]
        [HttpGet]
        public IActionResult GetMembers(int id)
        {
            var team = repository.Teams.ById(id);
            var user = HttpContext.Items["user"] as User;
            if (user == null || (!user.IsAdmin && user.ID != team.OwnerID))
                return Json(new { Error = "Unauthorized" });

            var members = repository.Teams.GetMembers(id);
            return Json(members);
        }

        [Route("/Team/Members/")]
        [HttpPost]
        [IsLoggedIn]
        public IActionResult SetMembers([FromBody] TeamMember[] members)
        {
            if (members.Length == 0)
                return Json(new { Status = "ok" });

            // ToDo: Braucht man hier noch weitere Rechte?
            repository.Teams.DeleteMembers(members[0].TeamId);
            repository.Teams.AddMembers(members);
            return Json(new { Status = "ok" });
        }

        [Route("/Team/TeamIdByName/{name}")]
        [HttpGet]
        [IsLoggedIn]
        public IActionResult TeamIdByName(string name)
        {
            var obj = repository.Teams.ByName(name);
            if (obj == null)
                return Json(new { Error = "User not found!" });
            else
                return Json(new { Id = obj.ID });
        }

        [Route("/Team/TeamsInWhichUserIsMember/{user_id}")]
        [HttpGet]
        [IsLoggedIn]
        public IActionResult TeamsInWhichUserIsMember(int user_id)
        {
            var user = HttpContext.Items["user"] as User;
            if (user == null || (!user.IsAdmin && user.ID != user_id))
                return Json(new { Error = "Unauthorized" });

            return Json(repository.Teams.TeamsInWhichUserIsMember(user_id));
        }
    }
}