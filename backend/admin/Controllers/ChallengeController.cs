using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StudyBuddy.Persistence;

namespace StudyBuddy.Admin.Controllers
{
    public class ChallengeController : Controller
    {
        private readonly ILogger<UserController> logger;
        private IRepository repository;

        public ChallengeController(ILogger<UserController> logger, IRepository repository)
        {
            this.logger = logger;
            this.repository = repository;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
