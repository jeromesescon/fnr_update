namespace FnR.Databases.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedEventSchedule : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Events", "Schedule_Id", "dbo.Schedules");
            DropIndex("dbo.Events", new[] { "Schedule_Id" });
            AddColumn("dbo.Events", "ScheduleId", c => c.Int(nullable: false));
            AddForeignKey("dbo.Events", "ScheduleId", "dbo.Schedules", "Id", cascadeDelete: true);
            CreateIndex("dbo.Events", "ScheduleId");
            DropColumn("dbo.Events", "Schedule_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Events", "Schedule_Id", c => c.Int());
            DropIndex("dbo.Events", new[] { "ScheduleId" });
            DropForeignKey("dbo.Events", "ScheduleId", "dbo.Schedules");
            DropColumn("dbo.Events", "ScheduleId");
            CreateIndex("dbo.Events", "Schedule_Id");
            AddForeignKey("dbo.Events", "Schedule_Id", "dbo.Schedules", "Id");
        }
    }
}
