using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using StudyBuddy.Model;
using StudyBuddy.Persistence;

namespace StudyBuddy.Admin.Controllers
{
    public class GameBadgeController : Controller
    {
        private IRepository repository;

        public GameBadgeController(IRepository repository)
        {
            this.repository = repository;
        }

        public IActionResult Index()
        {
            var objects = repository.GameBadges.All();
            return View(objects);
        }

        public IActionResult New()
        {
            var user = HttpContext.Items["user"] as User;
            var obj = new GameBadge();
            obj.OwnerID = user.ID;
            ViewBag.Coverages = CreateSelectListForCoverages();

            return View("Edit", obj);
        }

        public IActionResult Edit(int id)
        {
            var obj = repository.GameBadges.ById(id);
            if (obj == null)
                return RedirectToAction("Index");

            ViewBag.Coverages = CreateSelectListForCoverages();
            return View("Edit", obj);
        }

        private SelectList CreateSelectListForCoverages()
        {
            var objects = new List<Object>();
            for (int i=1; i<=10; i++)
            {
                var o = new { name = i*10 + "%", value = (double)i/10 };
                objects.Add(o);
            }
            return new SelectList(objects, "value", "name");
        }

        public IActionResult Save([FromForm] GameBadge obj)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Coverages = CreateSelectListForCoverages();
                return View("Edit", obj);
            }

            this.repository.GameBadges.Save(obj);
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            repository.GameBadges.Delete(id);
            return RedirectToAction("Index");
        }

        public IActionResult ManageChallenges(int id)
        {
            var obj = repository.GameBadges.ById(id);
            if (obj == null)
                return RedirectToAction("Index");
            
            ViewBag.Challenges = repository.Challenges.OfBadge(id);
            ViewBag.OtherChallenges = repository.Challenges.NotOfBadge(id);

            return View(obj);
        }

        public IActionResult AddChallenge(int badgeID, int challengeID)
        {
            if (badgeID != 0 && challengeID != 0)
                repository.GameBadges.AddChallenge(badgeID, challengeID);

            var obj = repository.GameBadges.ById(badgeID);
            ViewBag.Challenges = repository.Challenges.OfBadge(badgeID);
            ViewBag.OtherChallenges = repository.Challenges.NotOfBadge(badgeID);

            return View("ManageChallenges", obj);
        }

        public IActionResult RemoveChallenge(int badgeID, int challengeID)
        {
            if (badgeID != 0 && challengeID != 0)
                repository.GameBadges.RemoveChallenge(badgeID, challengeID);

            var obj = repository.GameBadges.ById(badgeID);
            ViewBag.Challenges = repository.Challenges.OfBadge(badgeID);
            ViewBag.OtherChallenges = repository.Challenges.NotOfBadge(badgeID);

            return View("ManageChallenges", obj);
        }
    }
}
