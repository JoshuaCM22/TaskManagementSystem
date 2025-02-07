using System;
using System.Collections.Generic;
using System.Web.Mvc;
using TaskManagementSystem.Models.Interfaces;
using TaskManagementSystem.Models.ViewModels;

namespace TaskManagementSystem.Controllers
{
    [RoutePrefix("admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ITaskService _taskService;
        public AdminController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [Route("task-list")]
        [HttpGet]
        public ActionResult TaskList()
        {
            if (!User.Identity.IsAuthenticated) return RedirectToAction("login", "account");

            List<TaskViewModel> taskList = null;

            try
            {
                taskList = _taskService.GetAllTasks();
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "An error has occured in TaskList(). Error Message: " + ex.Message;
            }

            return View(taskList);
        }

        [Route("create-task")]
        [HttpGet]
        public ActionResult CreateTask()
        {
            if (!User.Identity.IsAuthenticated) return RedirectToAction("login", "account");

            var viewModel = new TaskViewModel();
            viewModel.TaskPriorityID = 1;
            ViewBag.PriorityList = _taskService.GetAllTaskPriorities();
            ViewBag.UserList = _taskService.GetAllUsersExceptSelf();
            return View(viewModel);
        }

        [Route("create-task")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateTask(TaskViewModel task)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    task.TaskStatusID = 1;
                    _taskService.CreateTask(task);
                    TempData["successMessage"] = "Successfully Created";
                    return RedirectToAction("task-list");
                }

                ViewBag.PriorityList = _taskService.GetAllTaskPriorities();
                ViewBag.UserList = _taskService.GetAllUsersExceptSelf();
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "An error has occured in CreateTask(). Error Message: " + ex.Message;
            }

            return View(task);
        }



        [Route("delete/{taskId?}")]  // Use a more specific route
        [HttpGet]
        public ActionResult Delete(int? taskId)
        {
            if (!User.Identity.IsAuthenticated) return RedirectToAction("login", "account");

            if (taskId != 0 && taskId != null)
            {
                TaskViewModel viewModel = null;
                try
                {
                    viewModel = _taskService.GetTaskById(int.Parse(taskId.ToString()));
                    ViewBag.StatusList = _taskService.GetAllTaskStatuses();
                }
                catch (Exception ex)
                {
                    TempData["errorMessage"] = "An error has occured in Delete(). Error Message: " + ex.Message;
                }
                return View(viewModel);
            }


            return RedirectToAction("task-list", "admin");
        }

        [Route("delete")]
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(TaskViewModel task)
        {

            if (task.TaskId != 0)
            {
                try
                {
                    _taskService.DeleteTask(task.TaskId);
                    TempData["successMessage"] = "Successfully Deleted";
                    return RedirectToAction("task-list", "admin");
                }
                catch (Exception ex)
                {
                    TempData["errorMessage"] = "An error has occured in Delete(). Error Message: " + ex.Message;
                }

                return View(task);
            }


            return View(task);

        }


        [Route("edit/{taskId?}")] 
        [HttpGet]
        public ActionResult Edit(int? taskId)
        {
            if (!User.Identity.IsAuthenticated) return RedirectToAction("login", "account");

            if (taskId != 0 && taskId != null)
            {
                try
                {
                    TaskViewModel _task = _taskService.GetTaskById(int.Parse(taskId.ToString()));
                    ViewBag.PriorityList = _taskService.GetAllTaskPriorities();
                    ViewBag.StatusList = _taskService.GetAllTaskStatuses();
                    ViewBag.UserList = _taskService.GetAllUsersExceptSelf();
                    return View(_task);
                }
                catch (Exception ex)
                {
                    TempData["errorMessage"] = "An error has occured in Edit(). Error Message: " + ex.Message;
                }
            }

            return RedirectToAction("task-list");
        }

        [Route("edit/{taskId?}")] 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TaskViewModel task)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _taskService.UpdateTask(task);
                    TempData["successMessage"] = "Successfully Saved";
                    return RedirectToAction("task-list", "admin");
                }
                catch (Exception ex)
                {
                    TempData["errorMessage"] = "An error has occured in Edit(). Error Message: " + ex.Message;
                }

                return View(task);
            }

            ViewBag.PriorityList = _taskService.GetAllTaskPriorities();
            ViewBag.StatusList = _taskService.GetAllTaskStatuses();

            return View(task);

        }
    }
}