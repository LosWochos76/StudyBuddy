using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StudyBuddy.Model;
using StudyBuddy.Persistence;

namespace StudyBuddy.Admin.Controllers
{
    [Admin]
    public class TermController : Controller
    {
        private readonly ILogger<UserController> logger;
        private IRepository repository;

        public TermController(ILogger<UserController> logger, IRepository repository)
        {
            this.logger = logger;
            this.repository = repository;
        }

        public IActionResult Index()
        {
            var objects = repository.Terms.All();
            return View(objects);
        }

        public IActionResult New()
        {
            var obj = new Term();
            return View("Edit", obj);
        }

        public IActionResult Edit(int id)
        {
            var obj = repository.Terms.ById(id);
            if (obj == null)
                return RedirectToAction("Index");
            
            return View("Edit", obj);
        }

        public IActionResult Save([FromForm] Term obj)
        {
            if (!ModelState.IsValid)
                return View("Edit", obj);
            
            this.repository.Terms.Save(obj);
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            this.repository.Terms.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
