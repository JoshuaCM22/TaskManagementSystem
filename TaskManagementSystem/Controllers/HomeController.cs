using System.Web.Mvc;

namespace TaskManagementSystem.Controllers
{
    [RoutePrefix("")]
    public class HomeController : Controller
    {
        [Route("")]
        [HttpGet]
        public ActionResult Index()
        {
            return RedirectToAction("your-task-list", "task");
        }

        [HttpGet]
        [Route("page-not-found")]
        public ActionResult PageNotFound()
        {
            return View();
        }
    }
}