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
        public IActionResult All([FromQuery] GameBadgeFilter filter)
        {
            return Json(backend.GameBadgeService.All(filter));
        }

        [Route("/User/{user_id}/GameBadge/")]
        [HttpGet]
        public IActionResult GetBadgesOfUser(int user_id, [FromQuery] GameBadgeFilter filter)
        {
            if (filter == null)
                filter = new GameBadgeFilter();

            filter.OnlyReceived = true;
            filter.CurrentUserId = user_id;

            var result = backend.GameBadgeService.All(filter);
            return Json(result);
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

        [Route("/GameBadge/{badge_id}/Challenge")]
        [HttpGet]
        public IActionResult GetChallengesOfBadge(int badge_id)
        {
            return Json(backend.ChallengeService.GetChallengesOfBadge(badge_id));
        }

        [Route("/User/{user_id}/GameBadge/{badge_id}")]
        [HttpPost]
        public IActionResult AddBadgeToUser(int user_id, int badge_id)
        {
            backend.GameBadgeService.AddBadgeToUser(user_id, badge_id);
            return Json(new { Status = "ok" });
        }

        [Route("/User/{user_id}/GameBadge/{badge_id}")]
        [HttpDelete]
        public IActionResult RemoveBadgeFromUser(int user_id, int badge_id)
        {
            backend.GameBadgeService.RemoveBadgeFromUser(user_id, badge_id);
            return Json(new { Status = "ok" });
        }

        [Route("/Challenge/{challenge_id}/Badge")]
        [HttpGet]
        public IActionResult GetBadgesForChallenge(int challenge_id)
        {
            return Json(backend.GameBadgeService.GetBadgesForChallenge(challenge_id));
        }
    }
}