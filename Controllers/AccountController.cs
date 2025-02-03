using System.Web.Mvc;
using System.Web.Security;
using TaskManagementSystem.Models.Interfaces;
using TaskManagementSystem.Models.ViewModels;

namespace TaskManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public ActionResult Register()
        {
            if (!User.Identity.IsAuthenticated)
            {
                var viewModels = new RegisterViewModel();
                return View(viewModels);
            }
            return RedirectToAction("TaskList", "Task");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel _register)
        {
            ModelState.Remove("Email");
            if (ModelState.IsValid)
            {
                _register.RoleId = 1;
                bool isAccountCreated = _accountService.Register(_register);
                if (isAccountCreated) return RedirectToAction("Login", "Account");
            }
            return View(_register);
        }

        [HttpGet]
        public ActionResult Login()
        {
            if (!User.Identity.IsAuthenticated)
            {
                var viewModels = new LoginViewModel();
                return View(viewModels);
            }
            return RedirectToAction("TaskList", "Task");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel _login)
        {
            if (ModelState.IsValid)
            {
                bool isVerify = _accountService.Login(_login);
                if (isVerify)
                {
                    FormsAuthentication.SetAuthCookie(_login.Username, true);
                    if (_login.Username != "admin" && _login.Username != "Admin") return RedirectToAction("TaskList", "Task");
                    return RedirectToAction("TaskList", "Admin");
                }
                ModelState.AddModelError("", "invalid Username or Password");
            }

            return View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Login");
        }
    }
}