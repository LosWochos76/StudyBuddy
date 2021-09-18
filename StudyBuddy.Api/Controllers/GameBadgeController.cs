using System.Linq;
using StudyBuddy.Model;
using Microsoft.AspNetCore.Mvc;
using StudyBuddy.Persistence;
using StudyBuddy.BusinessLogic;

namespace StudyBuddy.Api
{
    public class GameBadgeController : Controller
    {
        private GameBadgeService service;

        public GameBadgeController(IRepository repository)
        {
            this.service = new GameBadgeService(repository);
        }

        [Route("/GameBadge/")]
        [HttpGet]
        [IsLoggedIn]
        public IActionResult Get()
        {
            var current_user = HttpContext.Items["user"] as User;
            return Json(service.All(current_user));
        }

        [Route("/GameBadge/{id}")]
        [HttpGet]
        [IsLoggedIn]
        public IActionResult GetById(int id)
        {
            return Json(service.GetById(id));
        }

        [Route("/GameBadge/{id}")]
        [HttpPut]
        [IsLoggedIn]
        public IActionResult Update([FromBody] GameBadge obj)
        {
            var current_user = HttpContext.Items["user"] as User;
            return Json(service.Update(current_user, obj));
        }

        [Route("/GameBadge/")]
        [HttpPost]
        [IsLoggedIn]
        public IActionResult Insert([FromBody] GameBadge obj)
        {
            return Json(service.Insert(obj));
        }

        [Route("/GameBadge/{id}")]
        [HttpDelete]
        [IsLoggedIn]
        public IActionResult Delete(int id)
        {
            var current_user = HttpContext.Items["user"] as User;
            service.Delete(current_user, id);
            return Json(new { Status = "ok" });
        }

        [Route("/GameBadge/Challenges/")]
        [HttpPost]
        [IsLoggedIn]
        public IActionResult SetChallenges([FromBody] GameBadgeChallenge[] challenges)
        {
            service.SetChallenges(challenges);
            return Json(new { Status = "ok" });
        }
    }
}