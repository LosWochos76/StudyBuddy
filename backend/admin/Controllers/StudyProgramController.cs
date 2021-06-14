using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StudyBuddy.Persistence;

namespace StudyBuddy.Admin.Controllers
{
    public class StudyProgramController : Controller
    {
        private readonly ILogger<UserController> logger;
        private IRepository repository;

        public StudyProgramController(ILogger<UserController> logger, IRepository repository)
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
