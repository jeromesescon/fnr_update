namespace FnR.Databases.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedVets : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Vets", "VetName", c => c.String(nullable: false));
            DropColumn("dbo.Vets", "Name");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Vets", "Name", c => c.String(nullable: false));
            DropColumn("dbo.Vets", "VetName");
        }
    }
}
