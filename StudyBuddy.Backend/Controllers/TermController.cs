using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using StudyBuddy.Model;
using StudyBuddy.Persistence;

namespace StudyBuddy.Admin.Controllers
{
    [Admin]
    public class TermController : Controller
    {
        IStringLocalizer<SharedResources> localizer;
        private IRepository repository;

        public TermController(IStringLocalizer<SharedResources> localizer, IRepository repository)
        {
            this.localizer = localizer;
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

            if (obj.End <= obj.Start)
            {
                ModelState.AddModelError(string.Empty, localizer["The end of the semester must be at least one day after the beginning!"]);
                return View("Edit", obj);
            }
            
            var other1 = repository.Terms.ByDate(obj.Start);
            var other2 = repository.Terms.ByDate(obj.End);
            if ((other1 != null && other1.ID != obj.ID) || (other2 != null && other2.ID != obj.ID))
            {
                ModelState.AddModelError(string.Empty, localizer["The period of the semester cannot overlap with another semester!"]);
                return View("Edit", obj);
            }

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
