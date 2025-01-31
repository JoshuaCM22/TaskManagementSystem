using System.Collections.Generic;
using System.Web.Mvc;
using TaskManagementSystem.Models.Interfaces;
using TaskManagementSystem.Models.ViewModels;

namespace TaskManagementSystem.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminController : Controller
    {
        private readonly ITaskService _taskService;
        public AdminController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        public ActionResult TaskList()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                List<TaskViewModel> taskList = _taskService.GetAllTasks();
                return View(taskList);
            }
        }


        [HttpGet]
        public ActionResult Delete(int? taskId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                if (taskId != 0 && taskId != null)
                {
                    TaskViewModel _task = _taskService.GetTaskById(int.Parse(taskId.ToString()));
                    ViewBag.StatusList = _taskService.GetAllTaskStatuses();
                    return View(_task);
                }
                else
                {
                    return RedirectToAction("TaskList");
                }
            }
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete_Post(TaskViewModel task)
        {

            if (task.TaskId != 0)
            {
                bool isUpdated = _taskService.DeleteTask(task.TaskId);
                if (isUpdated)
                {
                    return RedirectToAction("TaskList");
                }
                else
                {
                    return View(task);
                }
            }
            else
            {
                return View(task);
            }
        }
    }
}