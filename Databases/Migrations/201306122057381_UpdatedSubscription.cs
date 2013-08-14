namespace FnR.Databases.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedSubscription : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Subscriptions", "DateSubscribed", c => c.DateTime(nullable: false));
            AddColumn("dbo.Subscriptions", "NextDeliveryDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Subscriptions", "NextDeliveryDate");
            DropColumn("dbo.Subscriptions", "DateSubscribed");
        }
    }
}
