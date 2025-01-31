using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using TaskManagementSystem.Context;
using TaskManagementSystem.Models.DatabaseModels;
using TaskManagementSystem.Models.Interfaces;

namespace TaskManagementSystem.Models.DataAccessLayer
{
    public class TaskRepository : ITaskRepository, IDisposable
    {
        private readonly DBContext _dbContext;
        public TaskRepository(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Tasks> GetAllTasks()
        {
            var tasks = _dbContext.Tasks.ToList();
            var users = _dbContext.Users.ToList();
            var taskStatuses = _dbContext.TaskStatuses.ToList();

            return tasks
                    // First join: Tasks with Users
                    .Join(users,
                          t => t.UserID, // Key from Tasks
                          u => u.ID,     // Key from Users
                          (t, u) => new { Task = t, User = u })
                    // Second join: The result of the first join with TaskStatuses
                    .Join(taskStatuses,
                          t => t.Task.TaskStatusID, // Key from Tasks
                          ts => ts.ID,               // Key from TaskStatuses
                          (t, ts) => new { t.Task, t.User, TaskStatus = ts })
                    // Project the final result
                    .Select(x => new Tasks
                    {
                        ID = x.Task.ID,
                        Title = x.Task.Title,
                        Description = x.Task.Description,
                        DueDate = x.Task.DueDate,
                        TaskStatusID = x.Task.TaskStatusID,
                        TaskStatus = new TaskStatuses
                        {
                            StatusName = x.TaskStatus.StatusName,
                        },          
                        UserID = x.Task.UserID,
                        User = new Users
                        {
                            Username = x.User.Username
                        }
                    })
                    .ToList();
        }


        public List<Tasks> GetAllTasks(int userId)
        {
            return _dbContext.Tasks.Where(x => x.UserID == userId).ToList();
        }


        public List<Tasks> GetAllTasks(string username)
        {
            var _user = _dbContext.Users.ToList().SingleOrDefault(s => s.Username == username);
            if (_user == null) return new List<Tasks>();
            return _dbContext.Tasks.Where(x => x.UserID == _user.ID).ToList();
        }


        public List<TaskStatuses> GetAllTaskStatuses()
        {
            return _dbContext.TaskStatuses.ToList();
        }

        public Tasks GetTask(int taskId)
        {
            return _dbContext.Tasks.SingleOrDefault(x => x.ID == taskId);
        }


        public int CreateTask(Tasks task)
        {
            task.UserID = task.UserID = _dbContext.Users.SingleOrDefault(x => x.Username == task.User.Username).ID;
            task.User = null;
            _dbContext.Tasks.Add(task);
            return _dbContext.SaveChanges();
        }

        public int UpdateTask(Tasks task)
        {
            _dbContext.Entry(task).State = EntityState.Modified;
            return _dbContext.SaveChanges();
        }

        public int DeleteTask(int taskId)
        {
            var _task = GetTask(taskId);
            _dbContext.Entry(_task).State = EntityState.Deleted;
            return _dbContext.SaveChanges();
        }

        // Dispose this context when this repository is no longer needed. To avoid increased memory usage.
        public void Dispose()
        {
            _dbContext.Dispose();
        }

    }
}