using System;
using System.Collections.Generic;
using System.Web.Mvc;
using TaskManagementSystem.Models.Interfaces;
using TaskManagementSystem.Models.ViewModels;

namespace TaskManagementSystem.Controllers
{
    [Authorize(Roles ="Admin, User")]
    public class TaskController : Controller
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
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
                List<TaskViewModel> taskList = _taskService.GetAllTasksByUsername(User.Identity.Name);                
                return View(taskList);
            }
        }

        [HttpGet]
        public ActionResult CreateTask()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {

                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateTask(TaskViewModel task)
        {
            if (ModelState.IsValid)
            {
                task.UserName = User.Identity.Name;
                task.TaskStatusID = 1;
                task.Date = DateTime.Now;
                bool isVerify = _taskService.CreateTask(task);
                if (isVerify)
                    return RedirectToAction("TaskList");
                task.ErrorMessage = "Task Creation Failed";
            }

            return View(task);
        }

        [HttpGet]
        public ActionResult Edit(int? taskId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                if (taskId != 0 && taskId!=null)
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
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit_Post(TaskViewModel task)
        {
            if(ModelState.IsValid)
            {
               bool isUpdated= _taskService.UpdateTask(task);
                if(isUpdated)
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