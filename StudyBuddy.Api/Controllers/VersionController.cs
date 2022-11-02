using System;
using Microsoft.AspNetCore.Mvc;

namespace StudyBuddy.Api.Controllers
{
    public class VersionController : Controller
    {
        [Route("/ApiVersion/")]
        [HttpGet]
        public IActionResult GetApiVersion()
        {
            var version = new Version(1, 11, 0, 0);
            return Json(version);
        }
    }
}