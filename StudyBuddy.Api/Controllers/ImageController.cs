using Microsoft.AspNetCore.Mvc;
using StudyBuddy.BusinessLogic;
using StudyBuddy.Model;

namespace StudyBuddy.Api.Controllers
{
    public class ImageController : Controller
    {
        private readonly IBackend backend;

        public ImageController(IBackend backend)
        {
            this.backend = backend;
        }

        [Route("/Image/{image_id}")]
        [HttpGet]
        public IActionResult Get(int image_id)
        {
            return Json(backend.ImageService.GetById(image_id));
        }

        [Route("/Image/")]
        [HttpGet]
        public IActionResult All([FromQuery] ImageFilter filter)
        {
            return Json(backend.ImageService.All(filter));
        }

        [Route("/Image/{id}")]
        [HttpPut]
        public IActionResult Update([FromBody] PersistentImage obj)
        {
            return Json(backend.ImageService.Update(obj));
        }

        [Route("/Image/")]
        [HttpPost]
        public IActionResult Insert([FromBody] PersistentImage obj)
        {
            return Json(backend.ImageService.Insert(obj));
        }

        [Route("/Image/{id}")]
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            backend.ImageService.Delete(id);
            return Json(new { Status = "ok" });
        }

        [Route("/Image/OfUser/{user_id}")]
        [HttpGet]
        public IActionResult OfUser(int user_id)
        {
            return Json(backend.ImageService.OfUser(user_id));
        }
    }
}