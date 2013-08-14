namespace FnR.Databases.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialDatabaseState : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Breeds",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        IdealWeight = c.Double(nullable: false),
                        PetTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PetTypes", t => t.PetTypeId, cascadeDelete: true)
                .Index(t => t.PetTypeId);

            CreateTable(
                "dbo.PetTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Pets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        BreedId = c.Int(nullable: false),
                        Birthday = c.DateTime(nullable: false),
                        UserId = c.Int(nullable: false),
                        Weight = c.Double(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Breeds", t => t.BreedId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.BreedId)
                .Index(t => t.UserId);

            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        Address = c.String(),
                        PhoneNumber = c.String(),
                        Username = c.String(nullable: false),
                        Password = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Subscriptions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(),
                        PetId = c.Int(),
                        ProductId = c.Int(),
                        VetId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId)
                .ForeignKey("dbo.Pets", t => t.PetId)
                .ForeignKey("dbo.Products", t => t.ProductId)
                .ForeignKey("dbo.Vets", t => t.VetId)
                .Index(t => t.UserId)
                .Index(t => t.PetId)
                .Index(t => t.ProductId)
                .Index(t => t.VetId);

            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        Cost = c.Double(nullable: false),
                        Price = c.Double(nullable: false),
                        Amount = c.Int(nullable: false),
                        PetTypeId = c.Int(nullable: false),
                        LowerWeightLimit = c.Double(nullable: false),
                        HeigherWeightLimit = c.Double(nullable: false),
                        Vet_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PetTypes", t => t.PetTypeId, cascadeDelete: true)
                .ForeignKey("dbo.Vets", t => t.Vet_Id)
                .Index(t => t.PetTypeId)
                .Index(t => t.Vet_Id);

            CreateTable(
                "dbo.Vets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Products", new[] { "Vet_Id" });
            DropIndex("dbo.Products", new[] { "PetTypeId" });
            DropIndex("dbo.Subscriptions", new[] { "VetId" });
            DropIndex("dbo.Subscriptions", new[] { "ProductId" });
            DropIndex("dbo.Subscriptions", new[] { "PetId" });
            DropIndex("dbo.Subscriptions", new[] { "UserId" });
            DropIndex("dbo.Pets", new[] { "UserId" });
            DropIndex("dbo.Pets", new[] { "BreedId" });
            DropIndex("dbo.Breeds", new[] { "PetTypeId" });
            DropForeignKey("dbo.Products", "Vet_Id", "dbo.Vets");
            DropForeignKey("dbo.Products", "PetTypeId", "dbo.PetTypes");
            DropForeignKey("dbo.Subscriptions", "VetId", "dbo.Vets");
            DropForeignKey("dbo.Subscriptions", "ProductId", "dbo.Products");
            DropForeignKey("dbo.Subscriptions", "PetId", "dbo.Pets");
            DropForeignKey("dbo.Subscriptions", "UserId", "dbo.Users");
            DropForeignKey("dbo.Pets", "UserId", "dbo.Users");
            DropForeignKey("dbo.Pets", "BreedId", "dbo.Breeds");
            DropForeignKey("dbo.Breeds", "PetTypeId", "dbo.PetTypes");
            DropTable("dbo.Vets");
            DropTable("dbo.Products");
            DropTable("dbo.Subscriptions");
            DropTable("dbo.Users");
            DropTable("dbo.Pets");
            DropTable("dbo.PetTypes");
            DropTable("dbo.Breeds");
        }
    }
}
