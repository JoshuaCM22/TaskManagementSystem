namespace TaskManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDateTimeCreatedToTasks : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tasks", "DateTimeCreated", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tasks", "DateTimeCreated");
        }
    }
}
