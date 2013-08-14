namespace FnR.Databases.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddVetUsernamePassword : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Vets", "Username", c => c.String());
            AddColumn("dbo.Vets", "Password", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Vets", "Password");
            DropColumn("dbo.Vets", "Username");
        }
    }
}
