using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
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
            var taskPriorities = _dbContext.TaskPriorities.ToList();

            return tasks
                // First join: Tasks with Users
                .Join(users,
                      task => task.UserID, // Alias: task
                      user => user.ID,     // Alias: user
                      (task, user) => new { TaskEntity = task, UserEntity = user }) // Aliases: TaskEntity, UserEntity
                                                                                    // Second join: The result of the first join with TaskStatuses
                .Join(taskStatuses,
                      t => t.TaskEntity.TaskStatusID, // Alias: t (previous join result)
                      status => status.ID,            // Alias: status
                      (t, status) => new { t.TaskEntity, t.UserEntity, TaskStatusEntity = status }) // Alias: TaskStatusEntity
                                                                                                    // Third join: The result of the first join with TaskPriorities
                .Join(taskPriorities,
                      t => t.TaskEntity.TaskPriorityID, // Alias: t (previous join result)
                      priority => priority.ID,          // Alias: priority
                      (t, priority) => new { t.TaskEntity, t.UserEntity, t.TaskStatusEntity, TaskPriorityEntity = priority }) // Alias: TaskPriorityEntity
                                                                                                                              // Project the final result
                .Select(x => new Tasks
                {
                    ID = x.TaskEntity.ID,
                    Title = x.TaskEntity.Title,
                    Description = x.TaskEntity.Description,
                    DueDate = x.TaskEntity.DueDate,
                    TaskPriorityID = x.TaskEntity.TaskPriorityID, // ✅ Fixed incorrect mapping

                    TaskPriority = new TaskPriorities
                    {
                        PriorityName = x.TaskPriorityEntity.PriorityName, // Alias applied
                    },

                    TaskStatusID = x.TaskEntity.TaskStatusID,
                    TaskStatus = new TaskStatuses
                    {
                        StatusName = x.TaskStatusEntity.StatusName, // Alias applied
                    },

                    UserID = x.TaskEntity.UserID,
                    User = new Users
                    {
                        Username = x.UserEntity.Username // Alias applied
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

        public List<TaskPriorities> GetAllTaskPriorities()
        {
            return _dbContext.TaskPriorities.ToList();
        }

        public List<Users> GetAllUsersExceptSelf()
        {
            return _dbContext.Users.Where(u => u.Username != HttpContext.Current.User.Identity.Name).ToList();
        }

        public Tasks GetTask(int taskId)
        {
            return _dbContext.Tasks.SingleOrDefault(x => x.ID == taskId);
        }


        public int GetUserID(string username)
        {
            return (_dbContext.Users.SingleOrDefault(x => x.Username == username)).ID;
        }


        public void CreateTask(Tasks task)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    _dbContext.Tasks.Add(task);
                    if (_dbContext.SaveChanges() <= 0) throw new Exception("No rows affected.");

                    _dbContext.TaskLogs.Add(GetTaskLogsObject(task, 1)); // 1 = Insert
                    if (_dbContext.SaveChanges() <= 0) throw new Exception("No rows affected.");

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }

        public void UpdateTask(Tasks task)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    _dbContext.Entry(task).State = EntityState.Modified;
                    if (_dbContext.SaveChanges() <= 0) throw new Exception("No rows affected.");

                    _dbContext.TaskLogs.Add(GetTaskLogsObject(task, 2)); // 2 = Update
                    if (_dbContext.SaveChanges() <= 0) throw new Exception("No rows affected.");

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }

        }

        public void DeleteTask(int taskId)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var task = GetTask(taskId);
                    _dbContext.Entry(task).State = EntityState.Deleted;
                    if (_dbContext.SaveChanges() <= 0) throw new Exception("No rows affected.");

                    _dbContext.TaskLogs.Add(GetTaskLogsObject(task, 3)); // 3 = Delete
                    if (_dbContext.SaveChanges() <= 0) throw new Exception("No rows affected.");

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }

        public TaskLogs GetTaskLogsObject(Tasks task, byte actionID)
        {
            var taskLogs = new TaskLogs();
            taskLogs.Title = task.Title;
            taskLogs.Description = task.Description;
            taskLogs.DueDate = task.DueDate;
            taskLogs.TaskPriorityID = task.TaskPriorityID;
            taskLogs.TaskStatusID = task.TaskStatusID;
            taskLogs.UserID = task.UserID;
            taskLogs.ActionID = actionID;
            taskLogs.DateTimeCreated = DateTime.Now;
            taskLogs.CreatedBy = _dbContext.Users.SingleOrDefault(x => x.Username == HttpContext.Current.User.Identity.Name).ID;
            return taskLogs;
        }


        // Dispose this context when this repository is no longer needed. To avoid increased memory usage.
        public void Dispose()
        {
            _dbContext.Dispose();
        }

    }
}