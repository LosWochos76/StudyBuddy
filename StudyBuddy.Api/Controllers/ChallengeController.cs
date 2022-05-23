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


        [Route("/User/{user_id}/AcceptedChallenges/")]
        [HttpGet]
        public IActionResult AcceptedChallenged(int user_id, [FromQuery] ChallengeFilter filter)
        {
            if (filter == null)
                filter = new ChallengeFilter();

            filter.CurrentUserId = user_id;
            filter.OnlyAccepted = true;
            return Json(backend.ChallengeService.All(filter));
        }

        [Route("/User/{user_id}/UnacceptedChallenges/")]
        [HttpGet]
        public IActionResult UnacepptedChallenges(int user_id, [FromQuery] ChallengeFilter filter)
        {
            if (filter == null)
                filter = new ChallengeFilter();

            filter.CurrentUserId = user_id;
            filter.OnlyAccepted = true;
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
            var ms = backend.ChallengeService.GetQrCode(id);
            ms.Position = 0;
            return File(ms, "image/png");
        }

        [Route("/Challenge/AcceptFromQrCode/")]
        [HttpPost]
        public IActionResult AcceptFromQrCode([FromBody] string parameter)
        {
            return Json(backend.ChallengeService.AcceptFromQrCode(parameter));
        }

        [Route("/Challenge/{challenge_id}/AcceptWithAddendum/")]
        [HttpPost]
        public IActionResult AcceptWithAddendum(int challenge_id, [FromBody] string prove_addendum)
        {
            return Json(backend.ChallengeService.AcceptWithAddendum(challenge_id, prove_addendum));
        }

        [Route("/Challenge/{challenge_id}/Accept/")]
        [HttpPost]
        public IActionResult Accept(int challenge_id)
        {
            backend.ChallengeService.Accept(challenge_id);
            return Json(new { Status = "ok" });
        }

        [Route("/Challenge/{challenge_id}/User/{user_id}")]
        [HttpDelete]
        public IActionResult RemoveAcceptance(int challenge_id, int user_id)
        {
            backend.ChallengeService.RemoveAcceptance(challenge_id, user_id);
            return Json(new {Status = "ok"});
        }

        [Route("/Challenge/{challenge_id}/User/{user_id}")]
        [HttpPost]
        public IActionResult AddAcceptance(int challenge_id, int user_id)
        {
            backend.ChallengeService.AddAcceptance(challenge_id, user_id);
            return Json(new { Status = "ok" });
        }
    }
}