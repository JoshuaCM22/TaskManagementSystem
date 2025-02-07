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
            return RedirectToAction("TaskList", "Task");
        }

        [HttpGet]
        [Route("page-not-found")]
        public ActionResult PageNotFound()
        {
            return View();
        }
    }
}