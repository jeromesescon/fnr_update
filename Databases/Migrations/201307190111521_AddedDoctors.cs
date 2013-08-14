namespace FnR.Databases.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedDoctors : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Doctors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        VetId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Vets", t => t.VetId, cascadeDelete: true)
                .Index(t => t.VetId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Doctors", new[] { "VetId" });
            DropForeignKey("dbo.Doctors", "VetId", "dbo.Vets");
            DropTable("dbo.Doctors");
        }
    }
}
