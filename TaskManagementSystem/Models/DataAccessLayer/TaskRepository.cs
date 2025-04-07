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
            return await _dbContext.Tasks
                      .Where(x => DbFunctions.TruncateTime(x.DateTimeCreated) >= fromDate.Date
                       && DbFunctions.TruncateTime(x.DateTimeCreated) <= toDate.Date
                       && (statusID == 0 || x.TaskStatusID == statusID)
                       && (userID == 0 || x.UserID == userID)
                       )
                       .CountAsync();
        }

        public async Task<List<Tasks>> GetAllTasks(DateTime fromDate, DateTime toDate, int statusID, int userID)
        {
            var response = await (from t in _dbContext.Tasks
                                  join u in _dbContext.Users on t.UserID equals u.ID
                                  join ts in _dbContext.TaskStatuses on t.TaskStatusID equals ts.ID
                                  join tp in _dbContext.TaskPriorities on t.TaskPriorityID equals tp.ID
                                  where DbFunctions.TruncateTime(t.DateTimeCreated) >= DbFunctions.TruncateTime(fromDate)
                                     && DbFunctions.TruncateTime(t.DateTimeCreated) <= DbFunctions.TruncateTime(toDate)
                                     && (statusID == 0 || t.TaskStatusID == statusID)
                                     && (userID == 0 || t.UserID == userID)
                                  select new
                                  {
                                      TaskEntity = t,
                                      UserEntity = u,
                                      TaskStatusEntity = ts,
                                      TaskPriorityEntity = tp
                                  }).OrderByDescending(x=>x.TaskEntity.ID).ToListAsync();

            return response.Select(x => new Tasks
            {
                ID = x.TaskEntity.ID,
                Title = x.TaskEntity.Title,
                Description = x.TaskEntity.Description,
                DateTimeCreated = x.TaskEntity.DateTimeCreated,
                DueDate = x.TaskEntity.DueDate,
                TaskPriorityID = x.TaskEntity.TaskPriorityID,

                TaskPriority = new TaskPriorities
                {
                    PriorityName = x.TaskPriorityEntity.PriorityName
                },

                TaskStatusID = x.TaskEntity.TaskStatusID,
                TaskStatus = new TaskStatuses
                {
                    StatusName = x.TaskStatusEntity.StatusName
                },

                UserID = x.TaskEntity.UserID,
                User = new Users
                {
                    Username = x.UserEntity.Username
                }
            }).ToList();
        }



        public async Task<List<Tasks>> GetAllTasks(int userId)
        {
            return await _dbContext.Tasks.Where(x => x.UserID == userId).OrderByDescending(x => x.ID).ToListAsync();
        }


        public async Task<List<Tasks>> GetAllTasks(string username, DateTime fromDate, DateTime toDate, int statusID)
        {
            var _user = await _dbContext.Users.SingleOrDefaultAsync(s => s.Username == username);
            if (_user == null) return new List<Tasks>();

            return await _dbContext.Tasks.Where(x => x.UserID == _user.ID
                     && DbFunctions.TruncateTime(x.DateTimeCreated) >= fromDate.Date
                     && DbFunctions.TruncateTime(x.DateTimeCreated) <= toDate.Date
                     && (statusID == 0 || x.TaskStatusID == statusID)
                     )
                 .OrderByDescending(x => x.ID)
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