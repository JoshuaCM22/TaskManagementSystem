using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagementSystem.Models.DatabaseModels;

namespace TaskManagementSystem.Models.Interfaces
{
    public interface ITaskRepository
    {
        Task<int> GetTasksCount(DateTime fromDate, DateTime toDate, int statusID, int userID);
        Task<List<Tasks>> GetAllTasks(DateTime fromDate, DateTime toDate, int statusID, int userID);
        Task<List<Tasks>> GetAllTasks(int userId);
        Task<List<Tasks>> GetAllTasks(string username, DateTime fromDate, DateTime toDate, int statusID);
        Task<List<TaskStatuses>> GetAllTaskStatuses();
        Task<List<TaskPriorities>> GetAllTaskPriorities();
        Task<List<Users>> GetAllUsersExceptSelf();
        Task<List<Users>> GetAllUsers();
        Task<Tasks> GetTask(int taskId);
        Task<int> GetUserID(string username);
        Task CreateTask(Tasks task);
        Task UpdateTask(Tasks task);
        Task DeleteTask(Tasks task);
        Task<TaskLogs> GetTaskLogsObject(Tasks task, byte actionID);
    }
}
