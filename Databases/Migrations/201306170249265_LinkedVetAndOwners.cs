namespace FnR.Databases.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LinkedVetAndOwners : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "VetId", c => c.Int());
            AddForeignKey("dbo.Users", "VetId", "dbo.Vets", "Id");
            CreateIndex("dbo.Users", "VetId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Users", new[] { "VetId" });
            DropForeignKey("dbo.Users", "VetId", "dbo.Vets");
            DropColumn("dbo.Users", "VetId");
        }
    }
}
