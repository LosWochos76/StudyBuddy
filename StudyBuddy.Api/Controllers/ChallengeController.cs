using System.Drawing.Imaging;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using StudyBuddy.BusinessLogic;
using StudyBuddy.Model;

namespace StudyBuddy.Api
{
    public class ChallengeController : Controller
    {
        private readonly IBackend backend;

        public ChallengeController(IBackend backend)
        {
            this.backend = backend;
        }

        [Route("/Challenge/")]
        [HttpGet]
        public IActionResult Get([FromQuery] ChallengeFilter filter)
        {
            return Json(backend.ChallengeService.All(filter));
        }

        [Route("/Challenge/{id}")]
        [HttpGet]
        public IActionResult GetById(int id)
        {
            return Json(backend.ChallengeService.GetById(id));
        }

        [Route("/Challenge/{id}")]
        [HttpPut]
        public IActionResult Update([FromBody] Challenge obj)
        {
            return Json(backend.ChallengeService.Update(obj));
        }

        [Route("/Challenge/")]
        [HttpPost]
        public IActionResult Insert([FromBody] Challenge obj)
        {
            return Json(backend.ChallengeService.Insert(obj));
        }

        [Route("/Challenge/{id}")]
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            backend.ChallengeService.Delete(id);
            return Json(new {Status = "ok"});
        }

        [Route("/Challenge/CreateSeries/")]
        [HttpPost]
        public IActionResult CreateSeries([FromBody] CreateSeriesParameter param)
        {
            backend.ChallengeService.CreateSeries(param);
            return Json(new {Status = "ok"});
        }

        [Route("/Challenge/{id}/QrCode/")]
        [HttpGet]
        public IActionResult GetQrCode(int id)
        {
            var code = backend.ChallengeService.GetQrCode(id);

            var ms = new MemoryStream();
            code.Save(ms, ImageFormat.Png);
            ms.Position = 0;
            return File(ms, "image/png");
        }

        [Route("/Challenge/AcceptFromQrCode/")]
        [HttpPost]
        public IActionResult AcceptFromQrCode([FromBody] string parameter)
        {
            backend.ChallengeService.AcceptFromQrCode(parameter);
            return Json(new {Status = "ok"});
        }

        [Route("/Challenge/{challenge_id}/User/{user_id}")]
        [HttpDelete]
        public IActionResult RemoveAcceptance(int challenge_id, int user_id)
        {
            backend.ChallengeService.RemoveAcceptance(challenge_id, user_id);
            return Json(new {Status = "ok"});
        }

        [Route("/Challenge/{challenge_id}/Badge")]
        [HttpGet]
        public IActionResult GetBadgesForChallenge(int challenge_id)
        {
            return Json(backend.GameBadgeService.GetBadgesForChallenge(challenge_id));
        }

        [Route("/Challenge/{challenge_id}/User")]
        [HttpGet]
        public IActionResult GetAllUsersThatAcceptedChallenge(int challenge_id)
        {
            var result = backend.UserService.GetAllUsersThatAcceptedChallenge(challenge_id);
            return Json(result);
        }
    }
}