using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using StudyBuddy.Model;
using StudyBuddy.Persistence;

namespace StudyBuddy.Admin.Controllers
{
    [Admin]
    public class StudyProgramController : Controller
    {
        private IStringLocalizer<SharedResources> localizer;
        private IRepository repository;

        public StudyProgramController(IStringLocalizer<SharedResources> localizer, IRepository repository)
        {
            this.localizer = localizer;
            this.repository = repository;
        }

        public IActionResult Index()
        {
            var objects = this.repository.StudyPrograms.All();
            return View(objects);
        }

        public IActionResult Edit(int id)
        {
            var obj = this.repository.StudyPrograms.ById(id);
            if (obj == null)
                return RedirectToAction("Index");
            else
                return View("Edit", obj);
        }

        public IActionResult New()
        {
            var obj = new StudyProgram();
            return View("Edit", obj);
        }

        public IActionResult Save([FromForm] StudyProgram obj)
        {
            if (!ModelState.IsValid)
                return View("Edit", obj);

            // ensure that the acronym is unique
            var other = repository.StudyPrograms.ByAcronym(obj.Acronym);
            if (other != null && other.ID != obj.ID) 
            {
                ModelState.AddModelError("Acronym", localizer["Acronym is already in use! Please select a different acronym!"]);
                return View("Edit", obj);
            }
            
            this.repository.StudyPrograms.Save(obj);
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            this.repository.StudyPrograms.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
