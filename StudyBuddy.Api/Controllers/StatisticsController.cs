using Microsoft.AspNetCore.Mvc;
using StudyBuddy.BusinessLogic;
using StudyBuddy.Model;
using System.Globalization;
using System.Text;

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

        [Route("/Statistics/GetGameObjectStatistics")]
        [HttpGet]
        public FileResult GetGameObjectStatistics([FromQuery]bool orderAscending)
        {
            StringBuilder stringBuilder = new();

            IEnumerable<Challenge> challengeStatistics = backend.StatisticsService.GetChallengeStatistic(orderAscending);
            stringBuilder.AppendLine("ChallengeID,Points,Category,DateCreatedChallenge");
            foreach (Challenge challengeObj in challengeStatistics)
            {
                stringBuilder.AppendLine($"{challengeObj.ID},{challengeObj.Points},{challengeObj.Category},{challengeObj.Created}");
            }
            stringBuilder.Append(System.Environment.NewLine);

            IEnumerable<GameBadge> badgeStatistics = backend.StatisticsService.GetBadgeStatistics(orderAscending);
            stringBuilder.AppendLine("BadgeID,CreatorID,Created");
            foreach (GameBadge badgeObj in badgeStatistics)
            {
                stringBuilder.AppendLine($"{badgeObj.ID},{badgeObj.OwnerID},{badgeObj.Created}");
            }
            stringBuilder.Append(System.Environment.NewLine);

            IEnumerable<User> userStatistics = backend.StatisticsService.GetUsersWithDateCreated(orderAscending);
            stringBuilder.AppendLine("UserID,DateCreatedUser");
            foreach (User userObj in userStatistics)
            {
                stringBuilder.AppendLine($"{userObj.ID},{userObj.Created}");
            }

            return File(Encoding.UTF8.GetBytes(stringBuilder.ToString()), "text/csv", "GameObjectStatistics.csv");
        }

        [Route("/Statistics/GetItemCompletionStatistics")]
        [HttpGet]
        public FileResult GetItemCompletionStatistics([FromQuery] bool orderAscending)
        {
            StringBuilder stringBuilder = new();

            IEnumerable<GameObjectStatistics> challengeCompletionStatistics = backend.StatisticsService.GetChallengeCompletionStatistics(orderAscending);
            stringBuilder.AppendLine("UserID,ChallengeID,DateCompleted");
            foreach (GameObjectStatistics obj in challengeCompletionStatistics)
            {
                stringBuilder.AppendLine($"{obj.UserID},{obj.ItemID},{obj.DateCompleted}");
            }
            stringBuilder.Append(System.Environment.NewLine);

            IEnumerable<GameObjectStatistics> badgeCompletionStatistics = backend.StatisticsService.GetBadgeCompletionStatistics(orderAscending);
            stringBuilder.AppendLine("UserID,BadgeID,DateCompleted");
            foreach (GameObjectStatistics obj in badgeCompletionStatistics)
            {
                stringBuilder.AppendLine($"{obj.UserID},{obj.ItemID},{obj.DateCompleted}");
            }

            return File(Encoding.UTF8.GetBytes(stringBuilder.ToString()), "text/csv", "ItemCompletionStatistics.csv");
        }

    }
}