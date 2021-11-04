using Microsoft.AspNetCore.Mvc;
using StudyBuddy.BusinessLogic;
using StudyBuddy.Model;

namespace StudyBuddy.Api
{
    public class GameBadgeController : Controller
    {
        private readonly IBackend backend;

        public GameBadgeController(IBackend backend)
        {
            this.backend = backend;
        }

        [Route("/GameBadge/")]
        [HttpGet]
        public IActionResult Get([FromQuery] GameBadgeFilter filter)
        {
            return Json(backend.GameBadgeService.All(filter));
        }

        [Route("/GameBadge/{id}")]
        [HttpGet]
        public IActionResult GetById(int id)
        {
            return Json(backend.GameBadgeService.GetById(id));
        }

        [Route("/GameBadge/{id}")]
        [HttpPut]
        public IActionResult Update([FromBody] GameBadge obj)
        {
            return Json(backend.GameBadgeService.Update(obj));
        }

        [Route("/GameBadge/")]
        [HttpPost]
        public IActionResult Insert([FromBody] GameBadge obj)
        {
            return Json(backend.GameBadgeService.Insert(obj));
        }

        [Route("/GameBadge/{id}")]
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            backend.GameBadgeService.Delete(id);
            return Json(new {Status = "ok"});
        }

        [Route("/GameBadge/{badge_id}/UserSuccess/{user_id}")]
        [HttpGet]
        public IActionResult GetSuccessRate(int badge_id, int user_id)
        {
            return Json(backend.GameBadgeService.GetSuccessRate(badge_id, user_id));
        }

        [Route("/GameBadge/{badge_id}/User/")]
        [HttpGet]
        public IActionResult GetAllUsersHavingBadge(int badge_id)
        {
            return Json(backend.UserService.GetAllUsersHavingBadge(badge_id));
        }

        [Route("/GameBadge/{badge_id}/Challenge")]
        [HttpGet]
        public IActionResult GetChallengesOfBadge(int badge_id)
        {
            return Json(backend.ChallengeService.GetChallengesOfBadge(badge_id));
        }
    }
}