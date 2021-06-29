using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StudyBuddy.Model;
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
            var objects = repository.Challenges.All();
            return View(objects);
        }

        public IActionResult New()
        {
            var user = HttpContext.Items["user"] as User;
            var challenge = new Challenge();
            challenge.OwnerID = user.ID;

            var view_model = ChallengeViewModel.FromChallenge(challenge);
            return View("Edit", view_model);
        }

        public IActionResult Edit(int id)
        {
            var challenge = repository.Challenges.ById(id);
            var view_model = ChallengeViewModel.FromChallenge(challenge);
            return View("Edit", view_model);
        }

        public IActionResult Save([FromForm] ChallengeViewModel obj)
        {
            if (!ModelState.IsValid)
                return View("Edit", obj);

            var challenge = ChallengeViewModel.ToChallenge(obj);
            this.repository.Challenges.Save(challenge);
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            repository.Challenges.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
