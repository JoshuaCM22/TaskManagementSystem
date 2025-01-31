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
        bool CreateTask(TaskViewModel taskViewModel);
        bool UpdateTask(TaskViewModel taskViewModel);
        bool DeleteTask(int taskId);
        IEnumerable<SelectListItem> GetAllTaskStatuses();
    }
}
