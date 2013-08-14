namespace FnR.Databases.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ScheduleReorganization : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Schedules", "VetId", "dbo.Vets");
            DropIndex("dbo.Schedules", new[] { "VetId" });
            AddColumn("dbo.Schedules", "DoctorId", c => c.Int(nullable: true));
            AddForeignKey("dbo.Schedules", "DoctorId", "dbo.Doctors", "Id", cascadeDelete: true);
            CreateIndex("dbo.Schedules", "DoctorId");
            DropColumn("dbo.Schedules", "VetId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Schedules", "VetId", c => c.Int(nullable: false));
            DropIndex("dbo.Schedules", new[] { "DoctorId" });
            DropForeignKey("dbo.Schedules", "DoctorId", "dbo.Doctors");
            DropColumn("dbo.Schedules", "DoctorId");
            CreateIndex("dbo.Schedules", "VetId");
            AddForeignKey("dbo.Schedules", "VetId", "dbo.Vets", "Id", cascadeDelete: true);
        }
    }
}
