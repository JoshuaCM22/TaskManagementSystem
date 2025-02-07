namespace TaskManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        RoleID = c.Byte(nullable: false, identity: true),
                        RoleName = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.RoleID)
                .Index(t => t.RoleName, unique: true)
                .Index(t => t.RoleName, unique: true, name: "UK_RoleName");
            
            CreateTable(
                "dbo.Tasks",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        DueDate = c.DateTime(nullable: false),
                        TaskPriorityID = c.Byte(nullable: false),
                        TaskStatusID = c.Byte(nullable: false),
                        UserID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.TaskPriorities", t => t.TaskPriorityID, cascadeDelete: true)
                .ForeignKey("dbo.TaskStatuses", t => t.TaskStatusID, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: true)
                .Index(t => t.TaskPriorityID)
                .Index(t => t.TaskStatusID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.TaskPriorities",
                c => new
                    {
                        ID = c.Byte(nullable: false, identity: true),
                        PriorityName = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.ID)
                .Index(t => t.PriorityName, unique: true);
            
            CreateTable(
                "dbo.TaskStatuses",
                c => new
                    {
                        ID = c.Byte(nullable: false, identity: true),
                        StatusName = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.ID)
                .Index(t => t.StatusName, unique: true);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Username = c.String(nullable: false, maxLength: 50),
                        Password = c.String(),
                        RoleID = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Roles", t => t.RoleID, cascadeDelete: true)
                .Index(t => t.Username, unique: true)
                .Index(t => t.Username, unique: true, name: "UK_Username")
                .Index(t => t.RoleID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tasks", "UserID", "dbo.Users");
            DropForeignKey("dbo.Users", "RoleID", "dbo.Roles");
            DropForeignKey("dbo.Tasks", "TaskStatusID", "dbo.TaskStatuses");
            DropForeignKey("dbo.Tasks", "TaskPriorityID", "dbo.TaskPriorities");
            DropIndex("dbo.Users", new[] { "RoleID" });
            DropIndex("dbo.Users", "UK_Username");
            DropIndex("dbo.Users", new[] { "Username" });
            DropIndex("dbo.TaskStatuses", new[] { "StatusName" });
            DropIndex("dbo.TaskPriorities", new[] { "PriorityName" });
            DropIndex("dbo.Tasks", new[] { "UserID" });
            DropIndex("dbo.Tasks", new[] { "TaskStatusID" });
            DropIndex("dbo.Tasks", new[] { "TaskPriorityID" });
            DropIndex("dbo.Roles", "UK_RoleName");
            DropIndex("dbo.Roles", new[] { "RoleName" });
            DropTable("dbo.Users");
            DropTable("dbo.TaskStatuses");
            DropTable("dbo.TaskPriorities");
            DropTable("dbo.Tasks");
            DropTable("dbo.Roles");
        }
    }
}
