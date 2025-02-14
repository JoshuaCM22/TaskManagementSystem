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

        public async Task<List<TaskViewModel>> GetAllTasks()
        {
            var tasks = await _taskRepository.GetAllTasks();


            return tasks.Select(task => new TaskViewModel()
            {
                TaskId = task.ID,
                Title = task.Title,
                Description = task.Description,
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


        public async Task<IEnumerable<SelectListItem>> GetAllTaskStatuses()
        {
            var status = await _taskRepository.GetAllTaskStatuses();

            return status.Select(x => new SelectListItem
            {
                Value = x.ID.ToString(),
                Text = x.StatusName
            });
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


        public async Task<List<TaskViewModel>> GetAllTasksByUsername(string username)
        {
            var tasks = await _taskRepository.GetAllTasks(username);

            return tasks.Select(x => new TaskViewModel()
            {
                TaskId = x.ID,
                Title = x.Title,
                Description = x.Description,
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

    }
}