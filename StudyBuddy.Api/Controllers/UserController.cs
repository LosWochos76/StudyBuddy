using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StudyBuddy.BusinessLogic;
using StudyBuddy.Model;

namespace StudyBuddy.Api
{
    public class UserController : Controller
    {
        private readonly IBackend backend;
        private readonly ILogger logger;

        public UserController(IBackend backend, ILogger<UserController> logger)
        {
            this.backend = backend;
            this.logger = logger;
            logger.LogInformation("Creating UserController");
        }

        [Route("/User/")]
        [HttpGet]
        public IActionResult GetAll([FromQuery] UserFilter filter)
        {
            logger.LogInformation("UserController.GetAll");
            return Json(backend.UserService.GetAll(filter));
        }

        [Route("/User/{user_id}")]
        [HttpGet]
        public IActionResult GetById(int user_id)
        {
            logger.LogInformation("UserController.GetById");
            return Json(backend.UserService.GetById(user_id));
        }

        [Route("/User/{user_id}")]
        [HttpPut]
        public IActionResult Update([FromBody] User obj)
        {
            logger.LogInformation("UserController.Update");
            return Json(backend.UserService.Update(obj));
        }

        [Route("/User/")]
        [HttpPost]
        public IActionResult Insert([FromBody] User obj)
        {
            logger.LogInformation("UserController.Insert");
            return Json(backend.UserService.Insert(obj));
        }

        [Route("/User/{user_id}")]
        [HttpDelete]
        public IActionResult Delete(int user_id)
        {
            logger.LogInformation("UserController.Delete");
            backend.UserService.Delete(user_id);
            return Json(new {Status = "ok"});
        }

        [Route("/User/UserIdByNickname/{nickname}")]
        [HttpGet]
        public IActionResult GetUserIdByNickname(string nickname)
        {
            logger.LogInformation("UserController.GetUserIdByNickname");
            return Json(new {Id = backend.UserService.GetUserIdByNickname(nickname)});
        }

        [Route("/User/UserIdByEmail/{email}")]
        [HttpGet]
        public IActionResult GetUserIdByEmail(string email)
        {
            logger.LogInformation("UserController.GetUserIdByEmail");
            return Json(new {Id = backend.UserService.GetUserIdByEmail(email)});
        }

        [Route("/User/{user_id}/Friends")]
        [HttpGet]
        public IActionResult GetAllFriends(int user_id, [FromQuery] FriendFilter filter)
        {
            logger.LogInformation("UserController.GetAllFriends");

            if (filter == null)
                filter = new FriendFilter() { UserId = user_id };
            else
                filter.UserId = user_id;

            return Json(backend.UserService.GetAllFriends(filter));
        }

        [Route("/User/{user_id}/NotFriends")]
        [HttpGet]
        public IActionResult GetAllNotFriends(int user_id, [FromQuery] FriendFilter filter)
        {
            logger.LogInformation("UserController.GetAllNotFriends");

            if (filter == null)
                filter = new FriendFilter() { UserId = user_id };
            else
                filter.UserId = user_id;

            return Json(backend.UserService.GetAllNotFriends(filter));
        }

        [Route("/User/{user_id}/Friend/{friend_id}")]
        [HttpPost]
        public IActionResult AddFriend(int user_id, int friend_id)
        {
            logger.LogInformation("UserController.AddFriend");

            backend.UserService.AddFriend(user_id, friend_id);
            return Json(new {Status = "ok"});
        }

        [Route("/User/{user_id}/Friend/{friend_id}")]
        [HttpDelete]
        public IActionResult RemoveFriend(int user_id, int friend_id)
        {
            logger.LogInformation("UserController.RemoveFriend");

            backend.UserService.RemoveFriend(user_id, friend_id);
            return Json(new {Status = "ok"});
        }

        [Route("/User/Friend/{id}")]
        [HttpDelete]
        public IActionResult RemoveFriends(int id)
        {
            logger.LogInformation("UserController.RemoveFriends");

            backend.UserService.RemoveFriends(id);
            return Json(new {Status = "ok"});
        }

        [Route("/User/Friends/")]
        [HttpPost]
        public IActionResult SetFriends([FromBody] MultipleFriendsParameter parameter)
        {
            logger.LogInformation("UserController.SetFriend");

            backend.UserService.SetFriends(parameter);
            return Json(new {Status = "ok"});
        }

        [Route("/User/{user_a_id}/CountOfCommonFriends/{user_b_id}")]
        [HttpGet]
        public IActionResult GetCountOfCommonFriends(int user_a_id, int user_b_id)
        {
            logger.LogInformation("UserController.GetCountOfCommonFriends");

            var result = backend.UserService.GetCountOfCommonFriends(user_a_id, user_b_id);
            return Json(result);
        }

        [Route("/Challenge/{challenge_id}/User")]
        [HttpGet]
        public IActionResult GetAllUsersThatAcceptedChallenge(int challenge_id)
        {
            logger.LogInformation("UserController.GetAllUsersThatAcceptedChallenge");

            var result = backend.UserService.GetAllUsersThatAcceptedChallenge(challenge_id);
            return Json(result);
        }

        [Route("/GameBadge/{badge_id}/User/")]
        [HttpGet]
        public IActionResult GetAllUsersHavingBadge(int badge_id)
        {
            logger.LogInformation("UserController.GetAllUsersHavingBadge");

            return Json(backend.UserService.GetAllUsersHavingBadge(badge_id));
        }

        [Route("/User/SendMail/{user_id}")]
        [HttpPost]
        public IActionResult SendMailToUser([FromBody] MailDto mail)
        {
            logger.LogInformation("UserController.SenMailToUser");
            backend.UserService.SendMailToUser(mail);
            return Json(new { Status = "ok" });
        }
    }
}