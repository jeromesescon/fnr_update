namespace FnR.Databases.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedStatusOnDelivery : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Subscriptions", "Sent", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Subscriptions", "Sent");
        }
    }
}
