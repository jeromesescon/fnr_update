namespace FnR.Databases.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedBillingIds : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "TokenCustomerID", c => c.String());
            AddColumn("dbo.Users", "RebillCustomerID", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "RebillCustomerID");
            DropColumn("dbo.Users", "TokenCustomerID");
        }
    }
}
