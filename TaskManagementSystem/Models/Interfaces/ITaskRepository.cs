using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagementSystem.Models.DatabaseModels;

namespace TaskManagementSystem.Models.Interfaces
{
    public interface ITaskRepository
    {
        Task<List<Tasks>> GetAllTasks();
        Task<List<Tasks>> GetAllTasks(int userId);
        Task<List<Tasks>> GetAllTasks(string username);
        Task<List<TaskStatuses>> GetAllTaskStatuses();
        Task<List<TaskPriorities>> GetAllTaskPriorities();
        Task<List<Users>> GetAllUsersExceptSelf();
        Task<Tasks> GetTask(int taskId);
        Task<int> GetUserID(string username);
        Task CreateTask(Tasks task);
        Task UpdateTask(Tasks task);
        Task DeleteTask(Tasks task);
        Task<TaskLogs> GetTaskLogsObject(Tasks task, byte actionID);
    }
}
