using System;
using System.Web.Mvc;
using System.Web.Security;
using TaskManagementSystem.Models.Interfaces;
using TaskManagementSystem.Models.ViewModels;

namespace TaskManagementSystem.Controllers
{
    [RoutePrefix("account")]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [Route("register")]
        [HttpGet]
        public ActionResult Register()
        {
            if (!User.Identity.IsAuthenticated)
            {
                var viewModels = new RegisterViewModel();
                return View(viewModels);
            }
            return RedirectToAction("task-list", "task");
        }

        [Route("register")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel viewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _accountService.Register(viewModel);
                    TempData["successMessage"] = "Successfully Created";
                    return RedirectToAction("login", "account");
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "An error has occured in Register(). Error Message: " + ex.Message;
            }

            return RedirectToAction("register", "account");
        }

        [Route("login")]
        [HttpGet]
        public ActionResult Login()
        {
            if (!User.Identity.IsAuthenticated)
            {
                var viewModels = new LoginViewModel();
                return View(viewModels);
            }
            return RedirectToAction("task-list", "task");
        }

        [Route("login")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel viewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (_accountService.Login(viewModel))
                    {
                        FormsAuthentication.SetAuthCookie(viewModel.Username, true);
                        if (_accountService.IsAdmin(viewModel.Username)) return RedirectToAction("task-list", "admin");
                        return RedirectToAction("task-list", "task");
                    }
                    TempData["errorMessage"] = "Invalid Username and/or Password";
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "An error has occured in Login(). Error Message: " + ex.Message;
            }


            return RedirectToAction("login", "account");
        }

        [Route("logout")]
        [HttpGet]
        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("login");
        }
    }
}