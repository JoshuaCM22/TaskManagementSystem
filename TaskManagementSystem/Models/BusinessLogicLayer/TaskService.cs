using System;
using System.Collections.Generic;
using System.Linq;
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

        public List<TaskViewModel> GetAllTasks()
        {
            return _taskRepository.GetAllTasks().Select(tasks => new TaskViewModel()
            {
                TaskId = tasks.ID,
                Title = tasks.Title,
                Description = tasks.Description,
                DueDate = tasks.DueDate,
                TaskPriorityID = tasks.TaskPriorityID,
                TaskPriorityName = tasks.TaskPriority?.PriorityName,
                TaskStatusID = tasks.TaskStatusID,
                TaskStatusName = tasks.TaskStatus?.StatusName,
                UserID = tasks.UserID,
                UserName = tasks.User.Username
            }).ToList();
        }


        public IEnumerable<SelectListItem> GetAllTaskPriorities()
        {
            var prioritySelectList = _taskRepository.GetAllTaskPriorities().Select(x => new SelectListItem
            {
                Value = x.ID.ToString(),
                Text = x.PriorityName
            });

            return prioritySelectList;
        }



        public IEnumerable<SelectListItem> GetAllTaskStatuses()
        {
            var statusSelectList = _taskRepository.GetAllTaskStatuses().Select(x => new SelectListItem
            {
                Value = x.ID.ToString(),
                Text = x.StatusName
            });

            return statusSelectList;
        }

        public IEnumerable<SelectListItem> GetAllUsersExceptSelf()
        {
            var statusSelectList = _taskRepository.GetAllUsersExceptSelf().Select(x => new SelectListItem
            {
                Value = x.ID.ToString(),
                Text = x.Username
            });

            return statusSelectList;
        }



        public List<TaskViewModel> GetAllTasksByUsername(string username)
        {
            return _taskRepository.GetAllTasks(username)
                                  .Select(x => new TaskViewModel()
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



        public TaskViewModel GetTaskById(int taskId)
        {
            Tasks _task = _taskRepository.GetTask(taskId);
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


        public void CreateTask(TaskViewModel taskViewModel)
        {
            if (taskViewModel.UserName != null && taskViewModel.UserID == 0) taskViewModel.UserID = _taskRepository.GetUserID(taskViewModel.UserName);

            Tasks task = new Tasks()
            {
                Title = taskViewModel.Title,
                Description = taskViewModel.Description,
                DueDate = taskViewModel.DueDate,
                TaskPriorityID = taskViewModel.TaskPriorityID,
                TaskStatusID = taskViewModel.TaskStatusID,
                UserID = taskViewModel.UserID,
            };
            _taskRepository.CreateTask(task);
        }


        public void UpdateTask(TaskViewModel taskViewModel)
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
            _taskRepository.UpdateTask(_task);
        }


        public void DeleteTask(int taskId)
        {
            _taskRepository.DeleteTask(taskId);
        }

    }
}