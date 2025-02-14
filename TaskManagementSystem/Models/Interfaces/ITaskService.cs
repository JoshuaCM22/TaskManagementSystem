using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using TaskManagementSystem.Models.ViewModels;

namespace TaskManagementSystem.Models.Interfaces
{
    public interface ITaskService
    {
        Task<List<TaskViewModel>> GetAllTasks();
        Task<List<TaskViewModel>> GetAllTasksByUsername(string username);
        Task<TaskViewModel> GetTaskById(int taskId);
        Task CreateTask(TaskViewModel taskViewModel);
        Task UpdateTask(TaskViewModel taskViewModel);
        Task DeleteTask(TaskViewModel taskViewModel);
        Task<IEnumerable<SelectListItem>> GetAllTaskPriorities();
        Task<IEnumerable<SelectListItem>> GetAllTaskStatuses();
        Task<IEnumerable<SelectListItem>> GetAllUsersExceptSelf();
    }
}
