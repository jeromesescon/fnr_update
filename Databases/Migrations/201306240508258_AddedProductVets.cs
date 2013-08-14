namespace FnR.Databases.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedProductVets : DbMigration
    {
        public override void Up()
        {
            //DropForeignKey("dbo.Products", "Vet_Id", "dbo.Vets");
            ////DropIndex("dbo.Products", new[] { "Vet_Id" });
            //CreateTable(
            //    "dbo.VetProducts",
            //    c => new
            //        {
            //            Vet_Id = c.Int(nullable: false),
            //            Product_Id = c.Int(nullable: false),
            //        })
            //    .PrimaryKey(t => new { t.Vet_Id, t.Product_Id })
            //    .ForeignKey("dbo.Vets", t => t.Vet_Id, cascadeDelete: true)
            //    .ForeignKey("dbo.Products", t => t.Product_Id, cascadeDelete: true)
            //    .Index(t => t.Vet_Id)
            //    .Index(t => t.Product_Id);
            
            //DropColumn("dbo.Products", "Vet_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "Vet_Id", c => c.Int());
            //DropIndex("dbo.VetProducts", new[] { "Product_Id" });
            //DropIndex("dbo.VetProducts", new[] { "Vet_Id" });
            //DropForeignKey("dbo.VetProducts", "Product_Id", "dbo.Products");
            //DropForeignKey("dbo.VetProducts", "Vet_Id", "dbo.Vets");
            //DropTable("dbo.VetProducts");
            //CreateIndex("dbo.Products", "Vet_Id");
            AddForeignKey("dbo.Products", "Vet_Id", "dbo.Vets", "Id");
        }
    }
}
