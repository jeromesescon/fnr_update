namespace FnR.Databases.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TempRelationshipFix : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Schedules", "DoctorId", "dbo.Doctors");
            DropIndex("dbo.Schedules", new[] { "DoctorId" });
            AlterColumn("dbo.Schedules", "DoctorId", c => c.Int());
            AddForeignKey("dbo.Schedules", "DoctorId", "dbo.Doctors", "Id");
            CreateIndex("dbo.Schedules", "DoctorId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Schedules", new[] { "DoctorId" });
            DropForeignKey("dbo.Schedules", "DoctorId", "dbo.Doctors");
            AlterColumn("dbo.Schedules", "DoctorId", c => c.Int(nullable: false));
            CreateIndex("dbo.Schedules", "DoctorId");
            AddForeignKey("dbo.Schedules", "DoctorId", "dbo.Doctors", "Id", cascadeDelete: true);
        }
    }
}
