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
        Tasks GetTask(int taskId);
        int CreateTask(Tasks task);
        int UpdateTask(Tasks task);
        int DeleteTask(int taskId);
    }
}
