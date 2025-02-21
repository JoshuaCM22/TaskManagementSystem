using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using TaskManagementSystem.Models.ViewModels;

namespace TaskManagementSystem.Models.Interfaces
{
    public interface ITaskService
    {
        Task<DashboardViewModel> GetDashboardData(DateTime fromDate, DateTime toDate, int userID);
        Task<List<TaskViewModel>> GetAllTasks(DateTime fromDate, DateTime toDate, int statusID, int userID);
        Task<List<TaskViewModel>> GetAllTasksByUsername(string username, DateTime fromDate, DateTime toDate, int statusID);
        Task<TaskViewModel> GetTaskById(int taskId);
        Task CreateTask(TaskViewModel taskViewModel);
        Task UpdateTask(TaskViewModel taskViewModel);
        Task DeleteTask(TaskViewModel taskViewModel);
        Task<IEnumerable<SelectListItem>> GetAllTaskPriorities();
        Task<IEnumerable<SelectListItem>> GetAllTaskStatuses(bool isInsertAll = false);
        Task<IEnumerable<SelectListItem>> GetAllUsersExceptSelf();
        Task<IEnumerable<SelectListItem>> GetAllUsers();
        Task<int> GetUserID(string username);
    }
}
