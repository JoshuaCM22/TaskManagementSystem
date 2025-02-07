using System.Collections.Generic;
using TaskManagementSystem.Models.DatabaseModels;

namespace TaskManagementSystem.Models.Interfaces
{
    public interface ITaskRepository
    {
        List<Tasks> GetAllTasks();
        List<Tasks> GetAllTasks(int userId);
        List<Tasks> GetAllTasks(string username);
        List<TaskStatuses> GetAllTaskStatuses();
        List<TaskPriorities> GetAllTaskPriorities();
        List<Users> GetAllUsersExceptSelf();
        Tasks GetTask(int taskId);
        int GetUserID(string username);
        void CreateTask(Tasks task);
        void UpdateTask(Tasks task);
        void DeleteTask(int taskId);
        TaskLogs GetTaskLogsObject(Tasks task, byte actionID);
    }
}
