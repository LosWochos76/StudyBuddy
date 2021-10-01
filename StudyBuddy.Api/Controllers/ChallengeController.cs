using StudyBuddy.Model;
using Microsoft.AspNetCore.Mvc;
using StudyBuddy.Persistence;
using StudyBuddy.BusinessLogic;
using System.IO;
using System;

namespace StudyBuddy.Api
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
        public IActionResult Get()
        {
            var service = new ChallengeService(repository, HttpContext.Items["user"] as User);
            return Json(service.All());
        }

        [Route("/Challenge/ForToday/")]
        [HttpGet]
        public IActionResult ForToday([FromBody] string tag_string)
        {
            var service = new ChallengeService(repository, HttpContext.Items["user"] as User);
            return Json(service.ForToday(tag_string));
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
            return Json(new { Status = "ok" });
        }

        [Route("/Challenge/CreateSeries/")]
        [HttpPost]
        public IActionResult CreateSeries([FromBody] CreateSeriesParameter param)
        {
            var service = new ChallengeService(repository, HttpContext.Items["user"] as User);
            service.CreateSeries(param);
            return Json(new { Status = "ok" });
        }

        [Route("/Challenge/GetQrCode/{id}")]
        [HttpGet]
        public IActionResult GetQrCode(int id)
        {
            var service = new ChallengeService(repository, HttpContext.Items["user"] as User);
            var code = service.GetQrCode(id);

            MemoryStream ms = new MemoryStream();
            code.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            ms.Position = 0;
            return File(ms, "image/png");
        }

        [Route("/Challenge/AcceptFromQrCode/")]
        [HttpPost]
        public IActionResult AcceptFromQrCode([FromBody] string payload)
        {
            var service = new ChallengeService(repository, HttpContext.Items["user"] as User);
            service.AcceptFromQrCode(payload);
            return Json(new { Status = "ok" });
        }
    }
}