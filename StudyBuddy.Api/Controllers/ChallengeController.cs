using System.Drawing.Imaging;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StudyBuddy.BusinessLogic;
using StudyBuddy.Model;

namespace StudyBuddy.Api
{
    public class ChallengeController : Controller
    {
        private readonly IBackend backend;
        private readonly ILogger logger;

        public ChallengeController(IBackend backend, ILogger<ChallengeController> logger)
        {
            this.backend = backend;
            this.logger = logger;
            logger.LogInformation("Creating ChallengeController");
        }

        [Route("/Challenge/")]
        [HttpGet]
        public IActionResult Get([FromQuery] ChallengeFilter filter)
        {
            logger.LogInformation("ChallengeController.Get");
            return Json(backend.ChallengeService.All(filter));
        }


        [Route("/User/{user_id}/AcceptedChallenges/")]
        [HttpGet]
        public IActionResult AcceptedChallenged(int user_id, [FromQuery] ChallengeFilter filter)
        {
            logger.LogInformation("ChallengeController.AcceptedChallenged");

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
            logger.LogInformation("ChallengeController.UnacepptedChallenges");

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
            logger.LogInformation("ChallengeController.GetById");
            return Json(backend.ChallengeService.GetById(id));
        }

        [Route("/Challenge/{id}")]
        [HttpPut]
        public IActionResult Update([FromBody] Challenge obj)
        {
            logger.LogInformation("ChallengeController.Update");
            return Json(backend.ChallengeService.Update(obj));
        }

        [Route("/Challenge/")]
        [HttpPost]
        public IActionResult Insert([FromBody] Challenge obj)
        {
            logger.LogInformation("ChallengeController.Insert");
            return Json(backend.ChallengeService.Insert(obj));
        }

        [Route("/Challenge/{id}")]
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            logger.LogInformation("ChallengeController.Delete");
            backend.ChallengeService.Delete(id);
            return Json(new {Status = "ok"});
        }

        [Route("/Challenge/CreateSeries/")]
        [HttpPost]
        public IActionResult CreateSeries([FromBody] CreateSeriesParameter param)
        {
            logger.LogInformation("ChallengeController.CreateSeries");
            backend.ChallengeService.CreateSeries(param);
            return Json(new {Status = "ok"});
        }

        [Route("/Challenge/{id}/QrCode/")]
        [HttpGet]
        public IActionResult GetQrCode(int id)
        {
            logger.LogInformation("ChallengeController.GetQrCode");
            var ms = backend.ChallengeService.GetQrCode(id);
            ms.Position = 0;
            return File(ms, "image/png");
        }

        [Route("/Challenge/AcceptFromQrCode/")]
        [HttpPost]
        public IActionResult AcceptFromQrCode([FromBody] string parameter)
        {
            logger.LogInformation("ChallengeController.AcceptFromQrCode");
            return Json(backend.ChallengeService.AcceptFromQrCode(parameter));
        }

        [Route("/Challenge/{challenge_id}/AcceptWithAddendum/")]
        [HttpPost]
        public IActionResult AcceptWithAddendum(int challenge_id, [FromBody] string prove_addendum)
        {
            logger.LogInformation("ChallengeController.AcceptWithAddendum");
            return Json(backend.ChallengeService.AcceptWithAddendum(challenge_id, prove_addendum));
        }

        [Route("/Challenge/{challenge_id}/Accept/")]
        [HttpPost]
        public IActionResult Accept(int challenge_id)
        {
            logger.LogInformation("ChallengeController.Accept");
            backend.ChallengeService.Accept(challenge_id);
            return Json(new { Status = "ok" });
        }

        [Route("/Challenge/{challenge_id}/User/{user_id}")]
        [HttpDelete]
        public IActionResult RemoveAcceptance(int challenge_id, int user_id)
        {
            logger.LogInformation("ChallengeController.RemoveAcceptance");
            backend.ChallengeService.RemoveAcceptance(challenge_id, user_id);
            return Json(new {Status = "ok"});
        }

        [Route("/Challenge/{challenge_id}/User/{user_id}")]
        [HttpPost]
        public IActionResult AddAcceptance(int challenge_id, int user_id)
        {
            logger.LogInformation("ChallengeController.AddAcceptance");
            backend.ChallengeService.AddAcceptance(challenge_id, user_id);
            return Json(new { Status = "ok" });
        }
    }
}