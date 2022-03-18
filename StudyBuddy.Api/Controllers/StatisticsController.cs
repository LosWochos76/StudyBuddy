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

        [Route("/Statistics/{user_id}")]
        [HttpGet]
        public IActionResult GetUserStatistics(int user_id)
        {
            return Json(backend.StatisticsService.GetUserStatistics(user_id));
        }

        [Route("/Statistics/FriendsRanks/{user_id}")]
        [HttpGet]
        public IActionResult GetFriendsRanks(int user_id)
        {
            return Json(backend.StatisticsService.GetFriendsRanks(user_id));
        }

        [Route("/Score/{user_id}")]
        [HttpGet]
        public IActionResult GetScore(int user_id)
        {
            return Json(backend.StatisticsService.GetScore(user_id));
        }
    }
}