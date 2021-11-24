using Microsoft.AspNetCore.Mvc;
using StudyBuddy.BusinessLogic;

namespace StudyBuddy.Api.Controllers
{
    public class StatisticsController : Controller
    {
        private readonly IBackend backend;

        public StatisticsController(IBackend backend)
        {
            this.backend = backend;
        }

        [Route("/Statistics/AcceptedChallengesCount/{user_id}")]
        [HttpGet]
        public IActionResult GetAcceptedChallenges(int user_id)
        {
            return Json(backend.StatisticsService.GetAcceptedChallengesCount(user_id));
        }
    }
}
