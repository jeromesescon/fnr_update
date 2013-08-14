namespace FnR.Databases.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserSchedules : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Users", "Schedule_Id", "dbo.Schedules");
            DropIndex("dbo.Users", new[] { "Schedule_Id" });
            CreateTable(
                "dbo.ScheduleUsers",
                c => new
                    {
                        Schedule_Id = c.Int(nullable: false),
                        User_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Schedule_Id, t.User_Id })
                .ForeignKey("dbo.Schedules", t => t.Schedule_Id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.Schedule_Id)
                .Index(t => t.User_Id);
            
            AddColumn("dbo.Schedules", "VetId", c => c.Int(nullable: false));
            AddForeignKey("dbo.Schedules", "VetId", "dbo.Vets", "Id", cascadeDelete: true);
            CreateIndex("dbo.Schedules", "VetId");
            DropColumn("dbo.Users", "Schedule_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "Schedule_Id", c => c.Int());
            DropIndex("dbo.ScheduleUsers", new[] { "User_Id" });
            DropIndex("dbo.ScheduleUsers", new[] { "Schedule_Id" });
            DropIndex("dbo.Schedules", new[] { "VetId" });
            DropForeignKey("dbo.ScheduleUsers", "User_Id", "dbo.Users");
            DropForeignKey("dbo.ScheduleUsers", "Schedule_Id", "dbo.Schedules");
            DropForeignKey("dbo.Schedules", "VetId", "dbo.Vets");
            DropColumn("dbo.Schedules", "VetId");
            DropTable("dbo.ScheduleUsers");
            CreateIndex("dbo.Users", "Schedule_Id");
            AddForeignKey("dbo.Users", "Schedule_Id", "dbo.Schedules", "Id");
        }
    }
}
