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
                Date = tasks.DueDate,
                TaskStatusID = tasks.TaskStatusID,
                TaskStatusName = tasks.TaskStatus.StatusName,
                UserID = tasks.UserID,
                UserName = tasks.User.Username
            }).ToList();
        }

        public IEnumerable<SelectListItem> GetAllTaskStatuses()
        {
            var statusSelectList = _taskRepository.GetAllTaskStatuses().Select(ts => new SelectListItem
            {
                Value = ts.ID.ToString(),
                Text = ts.StatusName
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
                                      Date = x.DueDate,
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
                    Date = _task.DueDate,
                    TaskStatusID = _task.TaskStatusID,
                    TaskStatusName = _task.TaskStatus?.StatusName,
                    UserID = _task.UserID
                };
                return _taskViewModel;
            }
            else return null;
        }


        public bool CreateTask(TaskViewModel taskViewModel)
        {
            Tasks task = new Tasks()
            {
                Title = taskViewModel.Title,
                Description = taskViewModel.Description,
                DueDate = taskViewModel.Date,
                TaskStatusID = taskViewModel.TaskStatusID,
                User = new Users() { Username = taskViewModel.UserName },
            };
            int i = _taskRepository.CreateTask(task);
            if (i > 0) return true;
            return false;
        }


        public bool UpdateTask(TaskViewModel taskViewModel)
        {
            Tasks _task = new Tasks()
            {
                ID = taskViewModel.TaskId,
                Title = taskViewModel.Title,
                Description = taskViewModel.Description,
                DueDate = DateTime.Now,
                TaskStatusID = taskViewModel.TaskStatusID,
                UserID = taskViewModel.UserID
            };
            int i = _taskRepository.UpdateTask(_task);
            if (i > 0) return true;
            return false;
        }


        public bool DeleteTask(int taskId)
        {
            int i = _taskRepository.DeleteTask(taskId);
            if (i > 0) return true;
            return false;
        }

    }
}