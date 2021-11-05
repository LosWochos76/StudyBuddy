﻿using Microsoft.AspNetCore.Mvc;
using StudyBuddy.BusinessLogic;
using StudyBuddy.Model;

namespace StudyBuddy.Api
{
    public class UserController : Controller
    {
        private readonly IBackend backend;

        public UserController(IBackend backend)
        {
            this.backend = backend;
        }

        [Route("/User/")]
        [HttpGet]
        public IActionResult GetAll([FromQuery] UserFilter filter)
        {
            return Json(backend.UserService.GetAll(filter));
        }

        [Route("/User/Count")]
        [HttpGet]
        public IActionResult GetCount()
        {
            return Json(backend.UserService.GetCount());
        }

        [Route("/User/{id}")]
        [HttpGet]
        public IActionResult GetById(int id)
        {
            return Json(backend.UserService.GetById(id));
        }

        [Route("/User/{id}")]
        [HttpPut]
        public IActionResult Update([FromBody] User obj)
        {
            return Json(backend.UserService.Update(obj));
        }

        [Route("/User/")]
        [HttpPost]
        public IActionResult Insert([FromBody] User obj)
        {
            return Json(backend.UserService.Insert(obj));
        }

        [Route("/User/{id}")]
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            backend.UserService.Delete(id);
            return Json(new {Status = "ok"});
        }

        [Route("/User/UserIdByNickname/{nickname}")]
        [HttpGet]
        public IActionResult GetUserIdByNickname(string nickname)
        {
            return Json(new {Id = backend.UserService.GetUserIdByNickname(nickname)});
        }

        [Route("/User/UserIdByEmail/{email}")]
        [HttpGet]
        public IActionResult GetUserIdByEmail(string email)
        {
            return Json(new {Id = backend.UserService.GetUserIdByEmail(email)});
        }

        [Route("/User/{id}/Friends")]
        [HttpGet]
        public IActionResult GetAllFriends(int id, [FromQuery] FriendFilter filter)
        {
            if (filter == null)
                filter = new FriendFilter() { UserId = id };
            else
                filter.UserId = id;

            return Json(backend.UserService.GetAllFriends(filter));
        }

        [Route("/User/{id}/NotFriends")]
        [HttpGet]
        public IActionResult GetAllNotFriends(int id, [FromQuery] FriendFilter filter)
        {
            if (filter == null)
                filter = new FriendFilter() { UserId = id };
            else
                filter.UserId = id;

            return Json(backend.UserService.GetAllNotFriends(filter));
        }

        [Route("/User/{user_id}/Friend/{friend_id}")]
        [HttpPost]
        public IActionResult AddFriend(int user_id, int friend_id)
        {
            backend.UserService.AddFriend(user_id, friend_id);
            return Json(new {Status = "ok"});
        }

        [Route("/User/{user_id}/Friend/{friend_id}")]
        [HttpDelete]
        public IActionResult RemoveFriend(int user_id, int friend_id)
        {
            backend.UserService.RemoveFriend(user_id, friend_id);
            return Json(new {Status = "ok"});
        }

        [Route("/User/Friend/{id}")]
        [HttpDelete]
        public IActionResult RemoveFriends(int id)
        {
            backend.UserService.RemoveFriends(id);
            return Json(new {Status = "ok"});
        }

        [Route("/User/Friends/")]
        [HttpPost]
        public IActionResult SetFriends([FromBody] MultipleFriendsParameter parameter)
        {
            backend.UserService.SetFriends(parameter);
            return Json(new {Status = "ok"});
        }

        [Route("/User/{user_a_id}/CountOfCommonFriends/{user_b_id}")]
        [HttpGet]
        public IActionResult GetCountOfCommonFriends(int user_a_id, int user_b_id)
        {
            var result = backend.UserService.GetCountOfCommonFriends(user_a_id, user_b_id);
            return Json(result);
        }

        [Route("/User/{user_id}/GameBadge/")]
        [HttpGet]
        public IActionResult GetBadgesOfUser(int user_id)
        {
            var result = backend.GameBadgeService.GetBadgesOfUser(user_id);
            return Json(result);
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
    }
}