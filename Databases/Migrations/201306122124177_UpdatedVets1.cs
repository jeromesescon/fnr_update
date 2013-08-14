namespace FnR.Databases.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedVets1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Vets", "Name", c => c.String(nullable: false));
            DropColumn("dbo.Vets", "VetName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Vets", "VetName", c => c.String(nullable: false));
            DropColumn("dbo.Vets", "Name");
        }
    }
}
