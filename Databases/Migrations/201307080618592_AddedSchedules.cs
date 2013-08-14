namespace FnR.Databases.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedSchedules : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Schedules",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        StartDateTime = c.DateTime(nullable: false),
                        EndDateTime = c.DateTime(nullable: false),
                        Schedule_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Schedules", t => t.Schedule_Id)
                .Index(t => t.Schedule_Id);
            
            AddColumn("dbo.Users", "Schedule_Id", c => c.Int());
            AddColumn("dbo.Conditions", "ScheduleId", c => c.Int());
            AddForeignKey("dbo.Users", "Schedule_Id", "dbo.Schedules", "Id");
            AddForeignKey("dbo.Conditions", "ScheduleId", "dbo.Schedules", "Id");
            CreateIndex("dbo.Users", "Schedule_Id");
            CreateIndex("dbo.Conditions", "ScheduleId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Events", new[] { "Schedule_Id" });
            DropIndex("dbo.Conditions", new[] { "ScheduleId" });
            DropIndex("dbo.Users", new[] { "Schedule_Id" });
            DropForeignKey("dbo.Events", "Schedule_Id", "dbo.Schedules");
            DropForeignKey("dbo.Conditions", "ScheduleId", "dbo.Schedules");
            DropForeignKey("dbo.Users", "Schedule_Id", "dbo.Schedules");
            DropColumn("dbo.Conditions", "ScheduleId");
            DropColumn("dbo.Users", "Schedule_Id");
            DropTable("dbo.Events");
            DropTable("dbo.Schedules");
        }
    }
}
