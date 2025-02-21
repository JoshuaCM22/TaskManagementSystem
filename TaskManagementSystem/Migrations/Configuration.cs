namespace TaskManagementSystem.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using TaskManagementSystem.Models.DatabaseModels;

    internal sealed class Configuration : DbMigrationsConfiguration<TaskManagementSystem.Context.DBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(TaskManagementSystem.Context.DBContext context)
        {
            var roles = new List<Roles>
            {
                new Roles { RoleName = "Admin" },
                new Roles { RoleName = "Regular User" },
            };

            foreach (var role in roles)
            {
                if (!context.Roles.Any(r => r.RoleName == role.RoleName))
                {
                    context.Roles.Add(role);
                }
            }
            context.SaveChanges();


            var users = new List<Users>
             {
            new Users { Username = "admin", Password = "admin", RoleID = context.Roles.FirstOrDefault(r => r.RoleName == "Admin")?.RoleID ?? 1 },
            new Users { Username = "JCM", Password = "JCM", RoleID = context.Roles.FirstOrDefault(r => r.RoleName == "User")?.RoleID ?? 2 }
             };

            foreach (var user in users)
            {
                if (!context.Users.Any(u => u.Username == user.Username))
                {
                    context.Users.Add(user);
                }
            }

            context.SaveChanges();

            var taskStatuses = new List<TaskStatuses>
             {
              new TaskStatuses { StatusName = "Pending"},
              new TaskStatuses { StatusName = "In Progress" },
              new TaskStatuses { StatusName = "Completed" },
              new TaskStatuses { StatusName = "On Hold" },
              new TaskStatuses { StatusName = "Canceled" }
             };

            foreach (var taskStatus in taskStatuses)
            {
                if (!context.TaskStatuses.Any(u => u.StatusName == taskStatus.StatusName))
                {
                    context.TaskStatuses.Add(taskStatus);
                }
            }

            context.SaveChanges();

            var taskPriorities = new List<TaskPriorities>
             {
              new TaskPriorities { PriorityName = "Low"},
              new TaskPriorities { PriorityName = "Medium" },
              new TaskPriorities { PriorityName = "High" }
             };

            foreach (var taskPriority in taskPriorities)
            {
                if (!context.TaskPriorities.Any(u => u.PriorityName == taskPriority.PriorityName))
                {
                    context.TaskPriorities.Add(taskPriority);
                }
            }

            context.SaveChanges();


            var actions = new List<Actions>
             {
              new Actions { Description = "Insert"},
              new Actions { Description = "Update" },
              new Actions { Description = "Delete" },
             };

            foreach (var action in actions)
            {
                if (!context.Actions.Any(u => u.Description == action.Description))
                {
                    context.Actions.Add(action);
                }
            }

            context.SaveChanges();
        }
    }
}
