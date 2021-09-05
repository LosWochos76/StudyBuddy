using System.Linq;
using StudyBuddy.Model;
using Microsoft.AspNetCore.Mvc;
using StudyBuddy.Persistence;

namespace StudyBuddy.Services
{
    public class ChallengeController : Controller
    {
        private IRepository repository;

        public ChallengeController(IRepository repository)
        {
            this.repository = repository;
        }

        [Route("/Challenge/")]
        [HttpGet]
        [IsLoggedIn]
        public IActionResult Get()
        {
            var user = HttpContext.Items["user"] as User;

            if (user.IsAdmin)   
                return Json(repository.Challenges.All());
            else
                return Json(repository.Challenges.OfOwner(user.ID));
        }

        [Route("/Challenge/{id}")]
        [HttpGet]
        [IsLoggedIn]
        public IActionResult GetById(int id)
        {
            return Json(repository.Challenges.ById(id));
        }

        [Route("/Challenge/ByText/{text}")]
        [HttpGet]
        [IsLoggedIn]
        public IActionResult GetByText(string text)
        {
            var user = HttpContext.Items["user"] as User;

            if (user.IsAdmin)
                return Json(repository.Challenges.ByText(text));
            else
                return Json(repository.Challenges.OfOwnerByText(user.ID, text));
        }

        [Route("/Challenge/{id}")]
        [HttpPut]
        [IsLoggedIn]
        public IActionResult Update([FromBody] Challenge obj)
        {
            var user = HttpContext.Items["user"] as User;
            if (!user.IsAdmin && obj.OwnerID != user.ID)
                return Json(new { Error = "Unauthorized" });

            repository.Challenges.Update(obj);
            return Json(obj);
        }

        [Route("/Challenge/")]
        [HttpPost]
        [IsLoggedIn]
        public IActionResult Insert([FromBody] Challenge obj)
        {
            repository.Challenges.Insert(obj);
            return Json(obj);
        }

        [Route("/Challenge/{id}")]
        [HttpDelete]
        [IsLoggedIn]
        public IActionResult Delete(int id)
        {
            var obj = repository.Challenges.ById(id);
            var user = HttpContext.Items["user"] as User;
            if (!user.IsAdmin && obj != null && obj.OwnerID != user.ID)
                return Json(new { Error = "Unauthorized" });

            repository.Challenges.Delete(id);
            return Json(new { Status = "ok" });
        }

        [Route("/Challenge/CreateSeries/")]
        [HttpPost]
        [IsLoggedIn]
        public IActionResult CreateSeries([FromBody] CreateSeriesParameter param)
        {
            var obj = repository.Challenges.ById(param.ChallengeId);
            var user = HttpContext.Items["user"] as User;
            if (!user.IsAdmin && obj != null && obj.OwnerID != user.ID)
                return Json(new { Error = "Unauthorized" });

            repository.Challenges.CreateSeries(param.ChallengeId, param.DaysAdd, param.Count);
            return Json(new { Status = "ok" });
        }
    }
}