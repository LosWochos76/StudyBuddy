using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StudyBuddy.Model;
using StudyBuddy.Persistence;

namespace StudyBuddy.Admin.Controllers
{
    public class SettingsController : Controller
    {
        private readonly ILogger<UserController> logger;
        private IRepository repository;

        public SettingsController(ILogger<UserController> logger, IRepository repository)
        {
            this.logger = logger;
            this.repository = repository;
        }

        public IActionResult Index()
        {
            var user = HttpContext.Items["user"] as User;
            return Redirect("/User/Edit/" + user.ID);
        }
    }
}