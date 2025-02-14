using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using TaskManagementSystem.Models.Interfaces;
using TaskManagementSystem.Models.ViewModels;

namespace TaskManagementSystem.Controllers
{
    [RoutePrefix("task")]
    [Authorize(Roles = "Admin, Regular User")]
    public class TaskController : Controller
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [Route("your-task-list")]
        [HttpGet]
        public async Task<ActionResult> YourTaskList()
        {
            if (!User.Identity.IsAuthenticated) return RedirectToAction("login", "account");

            List<TaskViewModel> taskList = null;
            try
            {
                taskList = await _taskService.GetAllTasksByUsername(User.Identity.Name);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "An error has occured in YourTaskList(). Error Message: " + ex.Message;
            }

            return View(taskList);
        }

        [Route("create-new")]
        [HttpGet]
        public async Task<ActionResult> CreateNew()
        {
            if (!User.Identity.IsAuthenticated) return RedirectToAction("login", "account");

            var viewModel = new TaskViewModel();
            viewModel.TaskPriorityID = 1;

            try
            {
                ViewBag.PriorityList = await _taskService.GetAllTaskPriorities();
                ViewBag.UserList = await _taskService.GetAllUsersExceptSelf();
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "An error has occured in CreateNew(). Error Message: " + ex.Message;
            }
            return View(viewModel);
        }

        [Route("create-new")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateNew(TaskViewModel task)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    task.UserName = User.Identity.Name;
                    task.TaskStatusID = 1;
                    await _taskService.CreateTask(task);
                    TempData["successMessage"] = "Successfully Created";
                    return RedirectToAction("your-task-list", "task");
                }

                ViewBag.PriorityList = await _taskService.GetAllTaskPriorities();
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "An error has occured in CreateNew(). Error Message: " + ex.Message;
            }

            return View(task);
        }

        [Route("edit/{taskId?}")]
        [HttpGet]
        public async Task<ActionResult> Edit(int taskId)
        {
            if (!User.Identity.IsAuthenticated) return RedirectToAction("login", "account");

            if (taskId != 0)
            {
                try
                {
                    TaskViewModel _task = await _taskService.GetTaskById(int.Parse(taskId.ToString()));
                    ViewBag.PriorityList = await _taskService.GetAllTaskPriorities();
                    ViewBag.StatusList = await _taskService.GetAllTaskStatuses();
                    return View(_task);
                }
                catch (Exception ex)
                {
                    TempData["errorMessage"] = "An error has occured in Edit(). Error Message: " + ex.Message;
                }
            }

            return RedirectToAction("your-task-list", "task");
        }

        [Route("edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(TaskViewModel task)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _taskService.UpdateTask(task);
                    TempData["successMessage"] = "Successfully Saved";
                    return RedirectToAction("your-task-list", "task");
                }
                catch (Exception ex)
                {
                    TempData["errorMessage"] = "An error has occured in Edit(). Error Message: " + ex.Message;
                }

                return View(task);
            }

            ViewBag.PriorityList = await _taskService.GetAllTaskPriorities();
            ViewBag.StatusList = await _taskService.GetAllTaskStatuses();

            return View(task);
        }

        [Authorize(Roles = "Admin")]
        [Route("delete/{taskId}")]
        [HttpGet]
        public async Task<ActionResult> Delete(int taskId)
        {
            if (!User.Identity.IsAuthenticated) return RedirectToAction("login", "account");

            if (taskId != 0)
            {
                TaskViewModel viewModel = null;
                try
                {
                    viewModel = await _taskService.GetTaskById(int.Parse(taskId.ToString()));
                    ViewBag.StatusList = await _taskService.GetAllTaskStatuses();
                }
                catch (Exception ex)
                {
                    TempData["errorMessage"] = "An error has occured in Delete(). Error Message: " + ex.Message;
                }
                return View(viewModel);
            }


            return RedirectToAction("your-task-list", "task");
        }

        [Authorize(Roles = "Admin")]
        [Route("delete/{taskId}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(TaskViewModel taskViewModel)
        {
            try
            {
                await _taskService.DeleteTask(taskViewModel);
                TempData["successMessage"] = "Successfully Deleted";
                return RedirectToAction("your-task-list", "task");
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "An error has occured in PostDelete(). Error Message: " + ex.Message;
            }

            return View();
        }

    }
}