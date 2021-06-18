using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StudyBuddy.Model;
using StudyBuddy.Persistence;

namespace StudyBuddy.Admin.Controllers
{
    [Admin]
    public class TeamController : Controller
    {
        private readonly ILogger<UserController> logger;
        private IRepository repository;

        public TeamController(ILogger<UserController> logger, IRepository repository)
        {
            this.logger = logger;
            this.repository = repository;
        }

        public IActionResult Index()
        {
            var objects = repository.Teams.All();
            return View(objects);
        }

        public IActionResult New()
        {
            var obj = new Team();
            return View("Edit", obj);
        }

        public IActionResult Edit(int id)
        {
            var obj = repository.Teams.ById(id);
            if (obj == null)
                return RedirectToAction("Index");
            
            return View("Edit", obj);
        }

        public IActionResult Save([FromForm] Team obj)
        {
            if (!ModelState.IsValid)
                return View("Edit", obj);
            
            this.repository.Teams.Save(obj);
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            this.repository.Teams.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
