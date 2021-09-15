using StudyBuddy.Model;
using Microsoft.AspNetCore.Mvc;
using StudyBuddy.Persistence;
using StudyBuddy.BusinessLogic;

namespace StudyBuddy.Api
{
    public class UserController : Controller
    {
        private UserService service;

        public UserController(IRepository repository)
        {
            this.service = new UserService(repository);
        }

        [Route("/User/")]
        [HttpGet]
        [IsAdmin]
        public IActionResult Get()
        {
            return Json(service.All());
        }

        [Route("/User/Count")]
        [HttpGet]
        [IsAdmin]
        public IActionResult GetCount()
        {
            return Json(service.GetCount());
        }

        [Route("/User/{id}")]
        [HttpGet]
        [IsLoggedIn]
        public IActionResult GetById(int id)
        {
            var current_user = HttpContext.Items["user"] as User;
            return Json(service.GetById(current_user, id));
        }

        [Route("/User/{id}")]
        [HttpPut]
        [IsLoggedIn]
        public IActionResult Update([FromBody] User obj)
        {
            var current_user = HttpContext.Items["user"] as User;
            return Json(service.Update(current_user, obj));
        }

        [Route("/User/")]
        [HttpPost]
        public IActionResult Insert([FromBody] User obj)
        {
            return Json(service.Insert(obj));
        }

        [Route("/User/{id}")]
        [HttpDelete]
        [IsLoggedIn]
        public IActionResult Delete(int id)
        {
            var user = HttpContext.Items["user"] as User;
            service.Delete(user, id);
            return Json(new { Status = "ok" });
        }

        [Route("/User/UserIdByNickname/{nickname}")]
        [HttpGet]
        public IActionResult UserIdByNickname(string nickname)
        {
            return Json(new { Id = service.UserIdByNickname(nickname) });
        }

        [Route("/User/UserIdByEmail/{email}")]
        [HttpGet]
        public IActionResult UserIdByEmail(string email)
        {
            return Json(new { Id = service.UserIdByEmail(email) });
        }

        [Route("/User/Friends/{id}")]
        [HttpGet]
        [IsLoggedIn]
        public IActionResult AllFriends(int id)
        {
            var current_user = HttpContext.Items["user"] as User;
            return Json(service.AllFriends(current_user, id));
        }

        [Route("/User/Friend/")]
        [HttpPost]
        [IsAdmin]
        public IActionResult AddFriend([FromBody] SingleFriendParameter parameter)
        {
            service.AddFriend(parameter);
            return Json(new { Status = "ok" });
        }

        [Route("/User/Friend/")]
        [HttpDelete]
        [IsLoggedIn]
        public IActionResult RemoveFriend([FromBody] SingleFriendParameter parameter)
        {
            var current_user = HttpContext.Items["user"] as User;
            service.RemoveFriend(current_user, parameter);
            return Json(new { Status = "ok" });
        }

        [Route("/User/Friend/{id}")]
        [HttpDelete]
        [IsLoggedIn]
        public IActionResult RemoveFriends(int id)
        {
            var current_user = HttpContext.Items["user"] as User;
            service.RemoveFriends(current_user, id);
            return Json(new { Status = "ok" });
        }

        [Route("/User/Friends/")]
        [HttpPost]
        [IsAdmin]
        public IActionResult SetFriends([FromBody] MultipleFriendsParameter parameter)
        {
            var current_user = HttpContext.Items["user"] as User;
            service.SetFriends(current_user, parameter);
            return Json(new { Status = "ok" });
        }
    }
}