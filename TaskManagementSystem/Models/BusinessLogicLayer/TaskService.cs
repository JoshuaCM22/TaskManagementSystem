using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using TaskManagementSystem.Models.DatabaseModels;
using TaskManagementSystem.Models.Interfaces;
using TaskManagementSystem.Models.ViewModels;

namespace TaskManagementSystem.Models.BusinessLogicLayer
{
    public class TaskService : ITaskService
    {
        public readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<DashboardViewModel> GetDashboardData(DateTime fromDate, DateTime toDate, int userID)
        {
            var dashboardViewModel = new DashboardViewModel();
            dashboardViewModel.AllTasks = await _taskRepository.GetTasksCount(fromDate, toDate, 0, userID);
            dashboardViewModel.PendingTasks = await _taskRepository.GetTasksCount(fromDate, toDate, 1, userID);
            dashboardViewModel.InProgressTasks = await _taskRepository.GetTasksCount(fromDate, toDate, 2, userID);
            dashboardViewModel.CompletedTasks = await _taskRepository.GetTasksCount(fromDate, toDate, 3, userID);
            dashboardViewModel.OnHoldTasks = await _taskRepository.GetTasksCount(fromDate, toDate, 4, userID);
            dashboardViewModel.CanceledTasks = await _taskRepository.GetTasksCount(fromDate, toDate, 5, userID);
            return dashboardViewModel;
        }

        public async Task<List<TaskViewModel>> GetAllTasks(DateTime fromDate, DateTime toDate, int statusID, int userID)
        {
            var tasks = await _taskRepository.GetAllTasks(fromDate, toDate, statusID, userID);


            return tasks.Select(task => new TaskViewModel()
            {
                TaskId = task.ID,
                Title = task.Title,
                Description = task.Description,
                DateTimeCreated = task.DateTimeCreated,
                DueDate = task.DueDate,
                TaskPriorityID = task.TaskPriorityID,
                TaskPriorityName = task.TaskPriority?.PriorityName,
                TaskStatusID = task.TaskStatusID,
                TaskStatusName = task.TaskStatus?.StatusName,
                UserID = task.UserID,
                UserName = task.User.Username
            }).ToList();
        }


        public async Task<IEnumerable<SelectListItem>> GetAllTaskPriorities()
        {
            var priorities = await _taskRepository.GetAllTaskPriorities();

            return priorities.Select(x => new SelectListItem
            {
                Value = x.ID.ToString(),
                Text = x.PriorityName
            });
        }


        public async Task<IEnumerable<SelectListItem>> GetAllTaskStatuses(bool isInsertAll = false)
        {
            var status = await _taskRepository.GetAllTaskStatuses();

            // Convert to List to allow modifications
            var statusList = status
                .OrderBy(x => x.ID)
                .Select(x => new SelectListItem
                {
                    Value = x.ID.ToString(),
                    Text = x.StatusName
                }).ToList();

            if (isInsertAll) statusList.Insert(0, new SelectListItem { Value = "0", Text = "All" });

            return statusList; // Return as IEnumerable<SelectListItem>
        }

        public async Task<IEnumerable<SelectListItem>> GetAllUsers()
        {
            var users = await _taskRepository.GetAllUsers();

            var userList = users.Select(x => new SelectListItem
            {
                Value = x.ID.ToString(),
                Text = x.Username
            }).ToList();

            userList.Insert(0, new SelectListItem { Value = "0", Text = "All" });

            return userList;
        }

        public async Task<IEnumerable<SelectListItem>> GetAllUsersExceptSelf()
        {
            var users = await _taskRepository.GetAllUsersExceptSelf();

            return users.Select(x => new SelectListItem
            {
                Value = x.ID.ToString(),
                Text = x.Username
            });
        }


        public async Task<List<TaskViewModel>> GetAllTasksByUsername(string username, DateTime fromDate, DateTime toDate, int statusID)
        {
            var tasks = await _taskRepository.GetAllTasks(username, fromDate, toDate, statusID);

            return tasks.Select(x => new TaskViewModel()
            {
                TaskId = x.ID,
                Title = x.Title,
                Description = x.Description,
                DateTimeCreated = x.DateTimeCreated,
                DueDate = x.DueDate,
                TaskPriorityID = x.TaskPriorityID,
                TaskPriorityName = x.TaskPriority?.PriorityName,
                TaskStatusID = x.TaskStatusID,
                TaskStatusName = x.TaskStatus?.StatusName,
                UserID = x.UserID
            }).ToList();
        }



        public async Task<TaskViewModel> GetTaskById(int taskId)
        {
            Tasks _task = await _taskRepository.GetTask(taskId);
            if (_task != null)
            {
                TaskViewModel _taskViewModel = new TaskViewModel()
                {
                    TaskId = _task.ID,
                    Title = _task.Title,
                    Description = _task.Description,
                    DateTimeCreated = _task.DateTimeCreated,
                    DueDate = _task.DueDate,
                    TaskPriorityID = _task.TaskPriorityID,
                    TaskPriorityName = _task.TaskPriority?.PriorityName,
                    TaskStatusID = _task.TaskStatusID,
                    TaskStatusName = _task.TaskStatus?.StatusName,
                    UserID = _task.UserID,
                    UserName = _task.User.Username
                };
                return _taskViewModel;
            }
            return null;
        }


        public async Task CreateTask(TaskViewModel taskViewModel)
        {
            if (taskViewModel.UserName != null && taskViewModel.UserID == 0) taskViewModel.UserID = await _taskRepository.GetUserID(taskViewModel.UserName);

            Tasks task = new Tasks()
            {
                Title = taskViewModel.Title,
                Description = taskViewModel.Description,
                DueDate = taskViewModel.DueDate,
                TaskPriorityID = taskViewModel.TaskPriorityID,
                TaskStatusID = taskViewModel.TaskStatusID,
                UserID = taskViewModel.UserID,
            };

            await _taskRepository.CreateTask(task);
        }


        public async Task UpdateTask(TaskViewModel taskViewModel)
        {
            Tasks _task = new Tasks()
            {
                ID = taskViewModel.TaskId,
                Title = taskViewModel.Title,
                Description = taskViewModel.Description,
                DueDate = taskViewModel.DueDate,
                TaskPriorityID = taskViewModel.TaskPriorityID,
                TaskStatusID = taskViewModel.TaskStatusID,
                UserID = taskViewModel.UserID
            };
            await _taskRepository.UpdateTask(_task);
        }


        public async Task DeleteTask(TaskViewModel taskViewModel)
        {
            Tasks _task = new Tasks()
            {
                ID = taskViewModel.TaskId,
                Title = taskViewModel.Title,
                Description = taskViewModel.Description,
                DueDate = taskViewModel.DueDate,
                TaskPriorityID = taskViewModel.TaskPriorityID,
                TaskStatusID = taskViewModel.TaskStatusID,
                UserID = taskViewModel.UserID
            };
            await _taskRepository.DeleteTask(_task);
        }


        public async Task<int> GetUserID(string username)
        {
           return await _taskRepository.GetUserID(username);
        }

    }
}