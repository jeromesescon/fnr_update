namespace FnR.Databases.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UnsibscribeEway : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Subscriptions", "RebillID", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Subscriptions", "RebillID");
        }
    }
}
