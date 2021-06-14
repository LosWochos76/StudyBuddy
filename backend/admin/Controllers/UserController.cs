using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StudyBuddy.Model;
using StudyBuddy.Persistence;

namespace StudyBuddy.Admin.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> logger;
        private IRepository repository;

        public UserController(ILogger<UserController> logger, IRepository repository)
        {
            this.logger = logger;
            this.repository = repository;
        }

        public IActionResult Index()
        {
            var objects = this.repository.Users.All();
            return View(objects);
        }

        public IActionResult Edit(int id) 
        {
            var obj = repository.Users.ById(id);
            if (obj == null)
                return Redirect("/User");
            
            return View("Edit", obj);
        }

        public IActionResult New()
        {
            var obj = new User();
            return View("Edit", obj);
        }
    }
}
