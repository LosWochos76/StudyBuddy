using Microsoft.AspNetCore.Mvc;
using StudyBuddy.BusinessLogic;
using StudyBuddy.Model;
using StudyBuddy.Persistence;

namespace StudyBuddy.Api
{
    public class UserController : Controller
    {
        private readonly IRepository repository;

        public UserController(IRepository repository)
        {
            this.repository = repository;
        }

        [Route("/User/")]
        [HttpGet]
        public IActionResult GetAll()
        {
            var service = new UserService(repository, HttpContext.Items["user"] as User);
            return Json(service.GetAll());
        }

        [Route("/User/Count")]
        [HttpGet]
        public IActionResult GetCount()
        {
            var service = new UserService(repository, HttpContext.Items["user"] as User);
            return Json(service.GetCount());
        }

        [Route("/User/{id}")]
        [HttpGet]
        public IActionResult GetById(int id)
        {
            var service = new UserService(repository, HttpContext.Items["user"] as User);
            return Json(service.GetById(id));
        }

        [Route("/User/{id}")]
        [HttpPut]
        public IActionResult Update([FromBody] User obj)
        {
            var service = new UserService(repository, HttpContext.Items["user"] as User);
            return Json(service.Update(obj));
        }

        [Route("/User/")]
        [HttpPost]
        public IActionResult Insert([FromBody] User obj)
        {
            var service = new UserService(repository, HttpContext.Items["user"] as User);
            return Json(service.Insert(obj));
        }

        [Route("/User/{id}")]
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var service = new UserService(repository, HttpContext.Items["user"] as User);
            service.Delete(id);
            return Json(new {Status = "ok"});
        }

        [Route("/User/UserIdByNickname/{nickname}")]
        [HttpGet]
        public IActionResult GetUserIdByNickname(string nickname)
        {
            var service = new UserService(repository, HttpContext.Items["user"] as User);
            return Json(new {Id = service.GetUserIdByNickname(nickname)});
        }

        [Route("/User/UserIdByEmail/{email}")]
        [HttpGet]
        public IActionResult GetUserIdByEmail(string email)
        {
            var service = new UserService(repository, HttpContext.Items["user"] as User);
            return Json(new {Id = service.GetUserIdByEmail(email)});
        }

        [Route("/User/Friends/{id}")]
        [HttpGet]
        public IActionResult GetAllFriends(int id)
        {
            var service = new UserService(repository, HttpContext.Items["user"] as User);
            return Json(service.GetAllFriends(id));
        }

        [Route("/User/Friend/")]
        [HttpPost]
        public IActionResult AddFriend([FromBody] SingleFriendParameter parameter)
        {
            var service = new UserService(repository, HttpContext.Items["user"] as User);
            service.AddFriend(parameter);
            return Json(new {Status = "ok"});
        }

        [Route("/User/Friend/")]
        [HttpDelete]
        public IActionResult RemoveFriend([FromBody] SingleFriendParameter parameter)
        {
            var service = new UserService(repository, HttpContext.Items["user"] as User);
            service.RemoveFriend(parameter);
            return Json(new {Status = "ok"});
        }

        [Route("/User/Friend/{id}")]
        [HttpDelete]
        public IActionResult RemoveFriends(int id)
        {
            var service = new UserService(repository, HttpContext.Items["user"] as User);
            service.RemoveFriends(id);
            return Json(new {Status = "ok"});
        }

        [Route("/User/Friends/")]
        [HttpPost]
        public IActionResult SetFriends([FromBody] MultipleFriendsParameter parameter)
        {
            var service = new UserService(repository, HttpContext.Items["user"] as User);
            service.SetFriends(parameter);
            return Json(new {Status = "ok"});
        }

        [Route("/User/ThatAcceptedChallenge/{challenge_id}")]
        [HttpGet]
        public IActionResult ThatAcceptedChallenge(int challenge_id)
        {
            var service = new UserService(repository, HttpContext.Items["user"] as User);
            var result = service.GetAllUsersThatAcceptedChallenge(challenge_id);
            return Json(result);
        }
    }
}