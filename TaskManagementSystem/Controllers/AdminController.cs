using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        [Route("all-task-list")]
        [HttpGet]
        public async Task<ActionResult> AllTaskList()
        {
            if (!User.Identity.IsAuthenticated) return RedirectToAction("login", "account");

            List<TaskViewModel> taskList = null;

            try
            {
                taskList = await _taskService.GetAllTasks();
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "An error has occured in AllTaskList(). Error Message: " + ex.Message;
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
            ViewBag.PriorityList = await _taskService.GetAllTaskPriorities();
            ViewBag.UserList = await _taskService.GetAllUsersExceptSelf();
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
                    task.TaskStatusID = 1;
                    await _taskService.CreateTask(task);
                    TempData["successMessage"] = "Successfully Created";
                    return RedirectToAction("all-task-list", "admin");
                }

                ViewBag.PriorityList = await _taskService.GetAllTaskPriorities();
                ViewBag.UserList = await _taskService.GetAllUsersExceptSelf();
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "An error has occured in CreateTask(). Error Message: " + ex.Message;
            }

            return View(task);
        }



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


            return RedirectToAction("all-task-list", "admin");
        }

        [Route("delete/{taskId}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(TaskViewModel taskViewModel)
        {
            try
            {
                await _taskService.DeleteTask(taskViewModel);
                TempData["successMessage"] = "Successfully Deleted";
                return RedirectToAction("all-task-list", "admin");
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "An error has occured in Delete(). Error Message: " + ex.Message;
            }

            return View();
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
                    ViewBag.UserList = await _taskService.GetAllUsersExceptSelf();
                    return View(_task);
                }
                catch (Exception ex)
                {
                    TempData["errorMessage"] = "An error has occured in Edit(). Error Message: " + ex.Message;
                }
            }

            return RedirectToAction("all-task-list", "admin");
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
                    return RedirectToAction("all-task-list", "admin");
                }
                catch (Exception ex)
                {
                    TempData["errorMessage"] = "An error has occured in Edit(). Error Message: " + ex.Message;
                }

                return View(task);
            }

            ViewBag.PriorityList = await _taskService.GetAllTaskPriorities();
            ViewBag.StatusList = await _taskService.GetAllTaskStatuses();
            ViewBag.UserList = await _taskService.GetAllUsersExceptSelf();

            return View(task);

        }
    }
}