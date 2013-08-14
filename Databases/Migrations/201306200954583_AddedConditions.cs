namespace FnR.Databases.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedConditions : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Conditions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        VetId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Vets", t => t.VetId, cascadeDelete: true)
                .Index(t => t.VetId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Conditions", new[] { "VetId" });
            DropForeignKey("dbo.Conditions", "VetId", "dbo.Vets");
            DropTable("dbo.Conditions");
        }
    }
}
