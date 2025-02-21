using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<int> GetTasksCount(DateTime fromDate, DateTime toDate, int statusID, int userID)
        {
            var tasks = await _dbContext.Tasks
                  .Where(x => DbFunctions.TruncateTime(x.DateTimeCreated) >= fromDate.Date
                   && DbFunctions.TruncateTime(x.DateTimeCreated) <= toDate.Date)
                  .ToListAsync();
            if (statusID > 0) tasks = tasks.Where(x => x.TaskStatusID == statusID).ToList();
            if (userID > 0) tasks = tasks.Where(x => x.UserID == userID).ToList();
            return tasks.Count();
        }

        public async Task<List<Tasks>> GetAllTasks(DateTime fromDate, DateTime toDate, int statusID, int userID)
        {
            var tasks = await _dbContext.Tasks
                    .Where(x => DbFunctions.TruncateTime(x.DateTimeCreated) >= fromDate.Date
                     && DbFunctions.TruncateTime(x.DateTimeCreated) <= toDate.Date)
                    .ToListAsync();

            if (statusID > 0) tasks = tasks.Where(x => x.TaskStatusID == statusID).ToList();

            var users = await _dbContext.Users.ToListAsync();

            if (userID > 0) users = users.Where(x => x.ID == userID).ToList();

            var taskStatuses = await _dbContext.TaskStatuses.ToListAsync();
            var taskPriorities = await _dbContext.TaskPriorities.ToListAsync();

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
                    DateTimeCreated = x.TaskEntity.DateTimeCreated,
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


        public async Task<List<Tasks>> GetAllTasks(int userId)
        {
            return await _dbContext.Tasks.Where(x => x.UserID == userId).ToListAsync();
        }


        public async Task<List<Tasks>> GetAllTasks(string username, DateTime fromDate, DateTime toDate, int statusID)
        {
            var _user = await _dbContext.Users.SingleOrDefaultAsync(s => s.Username == username);
            if (_user == null) return new List<Tasks>();

            if (statusID > 0)
            {
                return await _dbContext.Tasks.Where(x => x.UserID == _user.ID
                         && DbFunctions.TruncateTime(x.DateTimeCreated) >= fromDate.Date
                         && DbFunctions.TruncateTime(x.DateTimeCreated) <= toDate.Date
                         && x.TaskStatusID == statusID
                         )
                     .ToListAsync();
            }

            return await _dbContext.Tasks
                      .Where(x => x.UserID == _user.ID
                       && DbFunctions.TruncateTime(x.DateTimeCreated) >= fromDate.Date
                       && DbFunctions.TruncateTime(x.DateTimeCreated) <= toDate.Date
                       )
                   .ToListAsync();
        }



        public async Task<List<TaskStatuses>> GetAllTaskStatuses()
        {
            return await _dbContext.TaskStatuses.ToListAsync();
        }

        public async Task<List<TaskPriorities>> GetAllTaskPriorities()
        {
            return await _dbContext.TaskPriorities.ToListAsync();
        }

        public async Task<List<Users>> GetAllUsersExceptSelf()
        {
            return await _dbContext.Users.Where(u => u.Username != HttpContext.Current.User.Identity.Name).ToListAsync();
        }

        public async Task<List<Users>> GetAllUsers()
        {
            return await _dbContext.Users.ToListAsync();
        }


        public async Task<Tasks> GetTask(int taskId)
        {
            return await _dbContext.Tasks.SingleOrDefaultAsync(x => x.ID == taskId);
        }


        public async Task<int> GetUserID(string username)
        {
            return (await _dbContext.Users.SingleOrDefaultAsync(x => x.Username == username)).ID;
        }


        public async Task CreateTask(Tasks task)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    _dbContext.Tasks.Add(task);
                    if (await _dbContext.SaveChangesAsync() <= 0) throw new Exception("No rows affected.");

                    _dbContext.TaskLogs.Add(await GetTaskLogsObject(task, 1)); // 1 = Insert
                    if (await _dbContext.SaveChangesAsync() <= 0) throw new Exception("No rows affected.");

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }

        public async Task UpdateTask(Tasks task)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    _dbContext.Entry(task).State = EntityState.Modified;
                    if (await _dbContext.SaveChangesAsync() <= 0) throw new Exception("No rows affected.");

                    _dbContext.TaskLogs.Add(await GetTaskLogsObject(task, 2)); // 2 = Update
                    if (await _dbContext.SaveChangesAsync() <= 0) throw new Exception("No rows affected.");

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }

        }

        public async Task DeleteTask(Tasks task)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    _dbContext.Entry(task).State = EntityState.Deleted;
                    if (await _dbContext.SaveChangesAsync() <= 0) throw new Exception("No rows affected.");

                    _dbContext.TaskLogs.Add(await GetTaskLogsObject(task, 3)); // 3 = Delete
                    if (await _dbContext.SaveChangesAsync() <= 0) throw new Exception("No rows affected.");

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }

        public async Task<TaskLogs> GetTaskLogsObject(Tasks task, byte actionID)
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
            taskLogs.CreatedBy = (await _dbContext.Users.SingleOrDefaultAsync(x => x.Username == HttpContext.Current.User.Identity.Name)).ID;
            return taskLogs;
        }


        // Dispose this context when this repository is no longer needed. To avoid increased memory usage.
        public void Dispose()
        {
            _dbContext.Dispose();
        }

    }
}