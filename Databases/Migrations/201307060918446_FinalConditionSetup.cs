namespace FnR.Databases.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FinalConditionSetup : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Conditions", "Pet_Id", c => c.Int());
            AddForeignKey("dbo.Conditions", "Pet_Id", "dbo.Pets", "Id");
            CreateIndex("dbo.Conditions", "Pet_Id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Conditions", new[] { "Pet_Id" });
            DropForeignKey("dbo.Conditions", "Pet_Id", "dbo.Pets");
            DropColumn("dbo.Conditions", "Pet_Id");
        }
    }
}
