using System.Drawing.Imaging;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using StudyBuddy.BusinessLogic;
using StudyBuddy.Model;
using StudyBuddy.Persistence;

namespace StudyBuddy.Api
{
    public class ChallengeController : Controller
    {
        private readonly IRepository repository;

        public ChallengeController(IRepository repository)
        {
            this.repository = repository;
        }

        [Route("/Challenge/")]
        [HttpGet]
        public IActionResult Get()
        {
            var service = new ChallengeService(repository, HttpContext.Items["user"] as User);
            return Json(service.All());
        }

        [Route("/Challenge/ForToday/")]
        [HttpGet]
        public IActionResult ForToday([FromBody] string search_string)
        {
            var service = new ChallengeService(repository, HttpContext.Items["user"] as User);
            return Json(service.ForToday(search_string));
        }

        [Route("/Challenge/{id}")]
        [HttpGet]
        public IActionResult GetById(int id)
        {
            var service = new ChallengeService(repository, HttpContext.Items["user"] as User);
            return Json(service.GetById(id));
        }

        [Route("/Challenge/ByText/{text}")]
        [HttpGet]
        public IActionResult GetByText(string text)
        {
            var service = new ChallengeService(repository, HttpContext.Items["user"] as User);
            return Json(service.GetByText(text));
        }

        [Route("/Challenge/{id}")]
        [HttpPut]
        public IActionResult Update([FromBody] Challenge obj)
        {
            var service = new ChallengeService(repository, HttpContext.Items["user"] as User);
            return Json(service.Update(obj));
        }

        [Route("/Challenge/OfBadge/{id}")]
        [HttpGet]
        public IActionResult OfBadge(int id)
        {
            var service = new ChallengeService(repository, HttpContext.Items["user"] as User);
            return Json(service.OfBadge(id));
        }

        [Route("/Challenge/NotOfBadge/{id}")]
        [HttpGet]
        public IActionResult NotOfBadge(int id)
        {
            var service = new ChallengeService(repository, HttpContext.Items["user"] as User);
            return Json(service.NotOfBadge(id));
        }

        [Route("/Challenge/")]
        [HttpPost]
        public IActionResult Insert([FromBody] Challenge obj)
        {
            var service = new ChallengeService(repository, HttpContext.Items["user"] as User);
            return Json(service.Insert(obj));
        }

        [Route("/Challenge/{id}")]
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var service = new ChallengeService(repository, HttpContext.Items["user"] as User);
            service.Delete(id);
            return Json(new {Status = "ok"});
        }

        [Route("/Challenge/CreateSeries/")]
        [HttpPost]
        public IActionResult CreateSeries([FromBody] CreateSeriesParameter param)
        {
            var service = new ChallengeService(repository, HttpContext.Items["user"] as User);
            service.CreateSeries(param);
            return Json(new {Status = "ok"});
        }

        [Route("/Challenge/{id}/QrCode/")]
        [HttpGet]
        public IActionResult GetQrCode(int id)
        {
            var service = new ChallengeService(repository, HttpContext.Items["user"] as User);
            var code = service.GetQrCode(id);

            var ms = new MemoryStream();
            code.Save(ms, ImageFormat.Png);
            ms.Position = 0;
            return File(ms, "image/png");
        }

        [Route("/Challenge/AcceptFromQrCode/")]
        [HttpPost]
        public IActionResult AcceptFromQrCode([FromBody] QrCodeParameter parameter)
        {
            var service = new ChallengeService(repository, HttpContext.Items["user"] as User);
            service.AcceptFromQrCode(parameter.Payload);
            return Json(new {Status = "ok"});
        }

        [Route("/Challenge/{challenge_id}/RemoveAcceptance/{user_id}")]
        [HttpPut]
        public IActionResult RemoveAcceptance(int challenge_id, int user_id)
        {
            var service = new ChallengeService(repository, HttpContext.Items["user"] as User);
            service.RemoveAcceptance(challenge_id, user_id);
            return Json(new {Status = "ok"});
        }
    }
}