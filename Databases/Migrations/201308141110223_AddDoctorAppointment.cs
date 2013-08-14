namespace FnR.Databases.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDoctorAppointment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DoctorAppointmentStatus", "ExpectedWaitTime", c => c.Int(nullable: false));
            AddColumn("dbo.DoctorAppointmentStatus", "ExpectedDurationOfDelay", c => c.Int(nullable: false));
            DropColumn("dbo.DoctorAppointmentStatus", "Name");
            DropColumn("dbo.DoctorAppointmentStatus", "Description");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DoctorAppointmentStatus", "Description", c => c.String());
            AddColumn("dbo.DoctorAppointmentStatus", "Name", c => c.String());
            DropColumn("dbo.DoctorAppointmentStatus", "ExpectedDurationOfDelay");
            DropColumn("dbo.DoctorAppointmentStatus", "ExpectedWaitTime");
        }
    }
}
