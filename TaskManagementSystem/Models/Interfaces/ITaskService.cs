using System.Collections.Generic;
using System.Web.Mvc;
using TaskManagementSystem.Models.ViewModels;

namespace TaskManagementSystem.Models.Interfaces
{
    public interface ITaskService
    {
        List<TaskViewModel> GetAllTasks();
        List<TaskViewModel> GetAllTasksByUsername(string username);
        TaskViewModel GetTaskById(int taskId);
        void CreateTask(TaskViewModel taskViewModel);
        void UpdateTask(TaskViewModel taskViewModel);
        void DeleteTask(int taskId);
        IEnumerable<SelectListItem> GetAllTaskPriorities();
        IEnumerable<SelectListItem> GetAllTaskStatuses();
        IEnumerable<SelectListItem> GetAllUsersExceptSelf();   
    }
}
