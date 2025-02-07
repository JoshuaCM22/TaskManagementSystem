namespace TaskManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTasksLogsWithData : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Actions",
                c => new
                {
                    ID = c.Byte(nullable: false, identity: true),
                    Description = c.String(),
                })
                .PrimaryKey(t => t.ID);

            CreateTable(
                "dbo.TaskLogs",
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    Title = c.String(),
                    Description = c.String(),
                    DueDate = c.DateTime(nullable: false),
                    TaskPriorityID = c.Byte(nullable: false),
                    TaskStatusID = c.Byte(nullable: false),
                    UserID = c.Int(nullable: false),
                    ActionID = c.Byte(nullable: false),
                    DateTimeCreated = c.DateTime(nullable: false),
                    CreatedBy = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Actions", t => t.ActionID, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.CreatedBy, cascadeDelete: false)  // 🔥 Set to false
                .ForeignKey("dbo.TaskPriorities", t => t.TaskPriorityID, cascadeDelete: true)
                .ForeignKey("dbo.TaskStatuses", t => t.TaskStatusID, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: false)  // 🔥 Set to false
                .Index(t => t.TaskPriorityID)
                .Index(t => t.TaskStatusID)
                .Index(t => t.UserID)
                .Index(t => t.ActionID)
                .Index(t => t.CreatedBy);
        }


        public override void Down()
        {
            DropForeignKey("dbo.TaskLogs", "UserID", "dbo.Users");
            DropForeignKey("dbo.TaskLogs", "TaskStatusID", "dbo.TaskStatuses");
            DropForeignKey("dbo.TaskLogs", "TaskPriorityID", "dbo.TaskPriorities");
            DropForeignKey("dbo.TaskLogs", "CreatedBy", "dbo.Users");
            DropForeignKey("dbo.TaskLogs", "ActionID", "dbo.Actions");
            DropIndex("dbo.TaskLogs", new[] { "CreatedBy" });
            DropIndex("dbo.TaskLogs", new[] { "ActionID" });
            DropIndex("dbo.TaskLogs", new[] { "UserID" });
            DropIndex("dbo.TaskLogs", new[] { "TaskStatusID" });
            DropIndex("dbo.TaskLogs", new[] { "TaskPriorityID" });
            DropTable("dbo.TaskLogs");
            DropTable("dbo.Actions");
        }
    }
}
