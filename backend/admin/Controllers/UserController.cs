using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using StudyBuddy.Model;
using StudyBuddy.Persistence;

namespace StudyBuddy.Admin.Controllers
{
    [Admin]
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

        private UserViewModel PrepareModel(User obj)
        {
            var user = UserViewModel.FromUser(obj);
            user.AllPrograms = new SelectList(repository.StudyPrograms.All(), "ID", "FullName");
            return user;
        }

        private UserViewModel PrepareModel(UserViewModel obj)
        {
            obj.AllPrograms = new SelectList(repository.StudyPrograms.All(), "ID", "FullName");
            return obj;
        }

        public IActionResult Edit(int id) 
        {
            var obj = repository.Users.ById(id);
            if (obj == null)
                return Redirect("/User");

            return View("Edit", PrepareModel(obj));
        }

        public IActionResult New()
        {
            return View("Edit", PrepareModel(new User()));
        }

        public IActionResult Save([FromForm] UserViewModel obj)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", PrepareModel(obj));
            }
            
            if (!obj.PasswordIsOk)
            {
                ModelState.AddModelError("", "Password and password repeat must be equal!");
                return View("Edit", PrepareModel(obj));
            }

            var user_with_nickname = repository.Users.FindByNickname(obj.Nickname);
            if (user_with_nickname != null && user_with_nickname.ID != obj.ID)
            {
                ModelState.AddModelError("Nickname", "Nickname is already in use! Please select a different nickname!");
                return View("Edit", PrepareModel(obj));
            }

            var user = UserViewModel.ToUser(obj);
            this.repository.Users.Save(user);
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            // ToDo: Check, if user is required somewhere

            this.repository.Users.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
