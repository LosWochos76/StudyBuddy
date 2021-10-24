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
        public IActionResult Get()
        {
            return Json(backend.GameBadgeService.All());
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

        [Route("/GameBadge/Challenges/")]
        [HttpPost]
        public IActionResult SetChallenges([FromBody] GameBadgeChallenge[] challenges)
        {
            backend.GameBadgeService.SetChallenges(challenges);
            return Json(new {Status = "ok"});
        }
    }
}