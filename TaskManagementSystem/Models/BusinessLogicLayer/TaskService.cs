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
            if (fromDate == DateTime.MinValue) throw new ArgumentException("The fromDate cannot use the default date time value.");
            else if (toDate == DateTime.MinValue) throw new ArgumentException("The toDate cannot use the default date time value.");
            else if (userID < 0) throw new ArgumentException("The userID cannot be less than zero.");

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
            if (fromDate == DateTime.MinValue) throw new ArgumentException("The fromDate cannot use the default date time value.");
            else if (toDate == DateTime.MinValue) throw new ArgumentException("The toDate cannot use the default date time value.");
            else if (statusID < 0) throw new ArgumentException("The statusID cannot be less than zero.");
            else if (userID < 0) throw new ArgumentException("The userID cannot be less than zero.");

            var tasks = await _taskRepository.GetAllTasks(fromDate, toDate, statusID, userID);

            return tasks.Select(task => new TaskViewModel()
            {
                TaskId = task.ID,
                Title = task.Title.Trim(),
                Description = task.Description.Trim(),
                DateTimeCreated = task.DateTimeCreated,
                DueDate = task.DueDate,
                TaskPriorityID = task.TaskPriorityID,
                TaskPriorityName = task.TaskPriority.PriorityName.Trim(),
                TaskStatusID = task.TaskStatusID,
                TaskStatusName = task.TaskStatus.StatusName.Trim(),
                UserID = task.UserID,
                UserName = task.User.Username.Trim()
            }).ToList();
        }


        public async Task<IEnumerable<SelectListItem>> GetAllTaskPriorities()
        {
            var priorities = await _taskRepository.GetAllTaskPriorities();

            return priorities.Select(x => new SelectListItem
            {
                Value = x.ID.ToString(),
                Text = x.PriorityName.Trim()
            });
        }


        public async Task<IEnumerable<SelectListItem>> GetAllTaskStatuses(bool isInsertAll = false)
        {
            var status = await _taskRepository.GetAllTaskStatuses();


            var statusList = status
                .OrderBy(x => x.ID)
                .Select(x => new SelectListItem
                {
                    Value = x.ID.ToString(),
                    Text = x.StatusName.Trim()
                }).ToList();

            if (isInsertAll) statusList.Insert(0, new SelectListItem { Value = "0", Text = "All" });

            return statusList; 
        }

        public async Task<IEnumerable<SelectListItem>> GetAllUsers()
        {
            var users = await _taskRepository.GetAllUsers();

            var userList = users.Select(x => new SelectListItem
            {
                Value = x.ID.ToString(),
                Text = x.Username.Trim()
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
                Text = x.Username.Trim()
            });
        }


        public async Task<List<TaskViewModel>> GetAllTasksByUsername(string username, DateTime fromDate, DateTime toDate, int statusID)
        {
            if (string.IsNullOrEmpty(username)) throw new ArgumentException("The username cannot be null or empty string.");
            else if (fromDate == DateTime.MinValue) throw new ArgumentException("The fromDate cannot use the default date time value.");
            else if (toDate == DateTime.MinValue) throw new ArgumentException("The toDate cannot use the default date time value.");
            else if (statusID < 0) throw new ArgumentException("The statusID cannot be less than zero.");

            var tasks = await _taskRepository.GetAllTasks(username, fromDate, toDate, statusID);

            return tasks.Select(x => new TaskViewModel()
            {
                TaskId = x.ID,
                Title = x.Title.Trim(),
                Description = x.Description.Trim(),
                DateTimeCreated = x.DateTimeCreated,
                DueDate = x.DueDate,
                TaskPriorityID = x.TaskPriorityID,
                TaskPriorityName = x.TaskPriority.PriorityName.Trim(),
                TaskStatusID = x.TaskStatusID,
                TaskStatusName = x.TaskStatus.StatusName.Trim(),
                UserID = x.UserID
            }).ToList();
        }



        public async Task<TaskViewModel> GetTaskById(int taskId)
        {
            if (taskId <= 0) throw new ArgumentException("The taskId cannot be less than or equal to zero.");

            Tasks _task = await _taskRepository.GetTask(taskId);
            if (_task != null)
            {
                TaskViewModel _taskViewModel = new TaskViewModel()
                {
                    TaskId = _task.ID,
                    Title = _task.Title.Trim(),
                    Description = _task.Description.Trim(),
                    DateTimeCreated = _task.DateTimeCreated,
                    DueDate = _task.DueDate,
                    TaskPriorityID = _task.TaskPriorityID,
                    TaskPriorityName = _task.TaskPriority.PriorityName.Trim(),
                    TaskStatusID = _task.TaskStatusID,
                    TaskStatusName = _task.TaskStatus.StatusName.Trim(),
                    UserID = _task.UserID,
                    UserName = _task.User.Username.Trim()
                };
                return _taskViewModel;
            }
            return null;
        }


        public async Task CreateTask(TaskViewModel taskViewModel)
        {
            if (String.IsNullOrEmpty(taskViewModel.Title)) throw new ArgumentException("The Title cannot be null or empty string.");
            else if (String.IsNullOrEmpty(taskViewModel.Description)) throw new ArgumentException("The Description cannot be null or empty string.");
            else if (taskViewModel.DueDate == DateTime.MinValue) throw new ArgumentException("The DueDate cannot use the default date time value.");
            else if (taskViewModel.TaskPriorityID <= 0) throw new ArgumentException("The TaskPriorityID cannot be less than or equal to zero.");
            else if (taskViewModel.TaskStatusID <= 0) throw new ArgumentException("The TaskStatusID cannot be less than or equal to zero.");
            else if (String.IsNullOrEmpty(taskViewModel.UserName) && taskViewModel.UserID == 0) throw new ArgumentException("The username cannot be null or empty string.");

            if (taskViewModel.UserName != null && taskViewModel.UserID == 0) taskViewModel.UserID = await _taskRepository.GetUserID(taskViewModel.UserName);

            Tasks task = new Tasks()
            {
                Title = taskViewModel.Title.Trim(),
                Description = taskViewModel.Description.Trim(),
                DueDate = taskViewModel.DueDate,
                TaskPriorityID = taskViewModel.TaskPriorityID,
                TaskStatusID = taskViewModel.TaskStatusID,
                UserID = taskViewModel.UserID,
            };

            await _taskRepository.CreateTask(task);
        }


        public async Task UpdateTask(TaskViewModel taskViewModel)
        {
            if (taskViewModel.TaskId <= 0) throw new ArgumentException("The TaskId cannot be less than or equal to zero.");
            else if (String.IsNullOrEmpty(taskViewModel.Title)) throw new ArgumentException("The Title cannot be null or empty string.");
            else if (String.IsNullOrEmpty(taskViewModel.Description)) throw new ArgumentException("The Description cannot be null or empty string.");
            else if (taskViewModel.DueDate == DateTime.MinValue) throw new ArgumentException("The DueDate cannot use the default date time value.");
            else if (taskViewModel.TaskPriorityID <= 0) throw new ArgumentException("The TaskPriorityID cannot be less than or equal to zero.");
            else if (taskViewModel.TaskStatusID <= 0) throw new ArgumentException("The TaskStatusID cannot be less than or equal to zero.");
            else if (taskViewModel.UserID <= 0) throw new ArgumentException("The UserID cannot be less than or equal to zero.");

            Tasks _task = new Tasks()
            {
                ID = taskViewModel.TaskId,
                Title = taskViewModel.Title.Trim(),
                Description = taskViewModel.Description.Trim(),
                DueDate = taskViewModel.DueDate,
                TaskPriorityID = taskViewModel.TaskPriorityID,
                TaskStatusID = taskViewModel.TaskStatusID,
                UserID = taskViewModel.UserID
            };
            await _taskRepository.UpdateTask(_task);
        }


        public async Task DeleteTask(TaskViewModel taskViewModel)
        {
            if (taskViewModel.TaskId <= 0) throw new ArgumentException("The TaskId cannot be less than or equal to zero.");
            else if (String.IsNullOrEmpty(taskViewModel.Title)) throw new ArgumentException("The Title cannot be null or empty string.");
            else if (String.IsNullOrEmpty(taskViewModel.Description)) throw new ArgumentException("The Description cannot be null or empty string.");
            else if (taskViewModel.DueDate == DateTime.MinValue) throw new ArgumentException("The DueDate cannot use the default date time value.");
            else if (taskViewModel.TaskPriorityID <= 0) throw new ArgumentException("The TaskPriorityID cannot be less than or equal to zero.");
            else if (taskViewModel.TaskStatusID <= 0) throw new ArgumentException("The TaskStatusID cannot be less than or equal to zero.");
            else if (taskViewModel.UserID <= 0) throw new ArgumentException("The UserID cannot be less than or equal to zero.");

            Tasks _task = new Tasks()
            {
                ID = taskViewModel.TaskId,
                Title = taskViewModel.Title.Trim(),
                Description = taskViewModel.Description.Trim(),
                DueDate = taskViewModel.DueDate,
                TaskPriorityID = taskViewModel.TaskPriorityID,
                TaskStatusID = taskViewModel.TaskStatusID,
                UserID = taskViewModel.UserID
            };
            await _taskRepository.DeleteTask(_task);
        }


        public async Task<int> GetUserID(string username)
        {
            if (string.IsNullOrEmpty(username)) throw new ArgumentException("The username cannot be null or empty string.");
            return await _taskRepository.GetUserID(username);
        }

    }
}