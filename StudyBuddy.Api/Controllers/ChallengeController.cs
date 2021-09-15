using StudyBuddy.Model;
using Microsoft.AspNetCore.Mvc;
using StudyBuddy.Persistence;
using StudyBuddy.BusinessLogic;

namespace StudyBuddy.Api
{
    public class ChallengeController : Controller
    {
        private ChallengeService service;

        public ChallengeController(IRepository repository)
        {
            this.service = new ChallengeService(repository);
        }

        [Route("/Challenge/")]
        [HttpGet]
        [IsLoggedIn]
        public IActionResult Get()
        {
            var current_user = HttpContext.Items["user"] as User;
            return Json(service.All(current_user));
        }

        [Route("/Challenge/{id}")]
        [HttpGet]
        [IsLoggedIn]
        public IActionResult GetById(int id)
        {
            return Json(service.GetById(id));
        }

        [Route("/Challenge/ByText/{text}")]
        [HttpGet]
        [IsLoggedIn]
        public IActionResult GetByText(string text)
        {
            var current_user = HttpContext.Items["user"] as User;
            return Json(service.GetByText(current_user, text));
        }

        [Route("/Challenge/{id}")]
        [HttpPut]
        [IsLoggedIn]
        public IActionResult Update([FromBody] Challenge obj)
        {
            var current_user = HttpContext.Items["user"] as User;
            return Json(service.Update(current_user, obj));
        }

        [Route("/Challenge/OfBadge/{id}")]
        [HttpGet]
        [IsLoggedIn]
        public IActionResult OfBadge(int id)
        {
            return Json(service.OfBadge(id));
        }

        [Route("/Challenge/NotOfBadge/{id}")]
        [HttpGet]
        [IsLoggedIn]
        public IActionResult NotOfBadge(int id)
        {
            return Json(service.NotOfBadge(id));
        }

        [Route("/Challenge/")]
        [HttpPost]
        [IsLoggedIn]
        public IActionResult Insert([FromBody] Challenge obj)
        {
            return Json(service.Insert(obj));
        }

        [Route("/Challenge/{id}")]
        [HttpDelete]
        [IsLoggedIn]
        public IActionResult Delete(int id)
        {
            var current_user = HttpContext.Items["user"] as User;
            service.Delete(current_user, id);
            return Json(new { Status = "ok" });
        }

        [Route("/Challenge/CreateSeries/")]
        [HttpPost]
        [IsLoggedIn]
        public IActionResult CreateSeries([FromBody] CreateSeriesParameter param)
        {
            var current_user = HttpContext.Items["user"] as User;
            service.CreateSeries(current_user, param);
            return Json(new { Status = "ok" });
        }
    }
}