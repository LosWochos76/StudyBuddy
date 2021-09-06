using System.Linq;
using StudyBuddy.Model;
using Microsoft.AspNetCore.Mvc;
using StudyBuddy.Persistence;

namespace StudyBuddy.Services
{
    public class GameBadgeController : Controller
    {
        private IRepository repository;

        public GameBadgeController(IRepository repository)
        {
            this.repository = repository;
        }

        [Route("/GameBadge/")]
        [HttpGet]
        [IsLoggedIn]
        public IActionResult Get()
        {
            var user = HttpContext.Items["user"] as User;

            if (user.IsAdmin)
                return Json(repository.GameBadges.All());
            else
                return Json(repository.GameBadges.OfOwner(user.ID));
        }

        [Route("/GameBadge/{id}")]
        [HttpGet]
        [IsLoggedIn]
        public IActionResult GetById(int id)
        {
            return Json(repository.GameBadges.ById(id));
        }

        [Route("/GameBadge/{id}")]
        [HttpPut]
        [IsLoggedIn]
        public IActionResult Update([FromBody] GameBadge obj)
        {
            if (obj == null)
                return Json(new { Error = "Object invalid!" });

            var user = HttpContext.Items["user"] as User;
            if (!user.IsAdmin && obj.OwnerID != user.ID)
                return Json(new { Error = "Unauthorized" });

            repository.GameBadges.Update(obj);
            return Json(obj);
        }

        [Route("/GameBadge/")]
        [HttpPost]
        [IsLoggedIn]
        public IActionResult Insert([FromBody] GameBadge obj)
        {
            repository.GameBadges.Insert(obj);
            return Json(obj);
        }

        [Route("/GameBadge/{id}")]
        [HttpDelete]
        [IsLoggedIn]
        public IActionResult Delete(int id)
        {
            var obj = repository.GameBadges.ById(id);
            var user = HttpContext.Items["user"] as User;
            if (!user.IsAdmin && obj != null && obj.OwnerID != user.ID)
                return Json(new { Error = "Unauthorized" });

            repository.GameBadges.Delete(id);
            return Json(new { Status = "ok" });
        }

        /// <summary>
        /// Add a list of Challenges to an existing GameBadge.
        /// </summary>
        /// <param name="challenges">Tuples of GameBadeId and ChallengeId.</param>
        /// <returns>Ok</returns>
        [Route("/GameBadge/Challenges/")]
        [HttpPost]
        [IsLoggedIn]
        public IActionResult SetChallenges([FromBody] GameBadgeChallenge[] challenges)
        {
            if (challenges.Length == 0)
                return Json(new { Status = "ok" });

            repository.GameBadges.DeleteChallenges(challenges[0].GameBadgeId);
            repository.GameBadges.AddChallenges(challenges);
            return Json(new { Status = "ok" });
        }
    }
}