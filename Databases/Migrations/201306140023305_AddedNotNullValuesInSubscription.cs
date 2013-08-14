namespace FnR.Databases.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedNotNullValuesInSubscription : DbMigration
    {
        public override void Up()
        {
            //DropForeignKey("dbo.Subscriptions", "UserId", "dbo.Users");
            //DropForeignKey("dbo.Subscriptions", "PetId", "dbo.Pets");
            //DropForeignKey("dbo.Subscriptions", "ProductId", "dbo.Products");
            //DropForeignKey("dbo.Subscriptions", "VetId", "dbo.Vets");
            //DropIndex("dbo.Subscriptions", new[] { "UserId" });
            //DropIndex("dbo.Subscriptions", new[] { "PetId" });
            //DropIndex("dbo.Subscriptions", new[] { "ProductId" });
            //DropIndex("dbo.Subscriptions", new[] { "VetId" });
            AlterColumn("dbo.Subscriptions", "UserId", c => c.Int(nullable: false));
            AlterColumn("dbo.Subscriptions", "PetId", c => c.Int(nullable: false));
            AlterColumn("dbo.Subscriptions", "ProductId", c => c.Int(nullable: false));
            AlterColumn("dbo.Subscriptions", "VetId", c => c.Int(nullable: false));
            //AddForeignKey("dbo.Subscriptions", "UserId", "dbo.Users", "Id", cascadeDelete: true);
            //AddForeignKey("dbo.Subscriptions", "PetId", "dbo.Pets", "Id", cascadeDelete: true);
            //AddForeignKey("dbo.Subscriptions", "ProductId", "dbo.Products", "Id", cascadeDelete: true);
            //AddForeignKey("dbo.Subscriptions", "VetId", "dbo.Vets", "Id", cascadeDelete: true);
            //CreateIndex("dbo.Subscriptions", "UserId");
            //CreateIndex("dbo.Subscriptions", "PetId");
            //CreateIndex("dbo.Subscriptions", "ProductId");
            //CreateIndex("dbo.Subscriptions", "VetId");
        }
        
        public override void Down()
        {
            //DropIndex("dbo.Subscriptions", new[] { "VetId" });
            //DropIndex("dbo.Subscriptions", new[] { "ProductId" });
            //DropIndex("dbo.Subscriptions", new[] { "PetId" });
            //DropIndex("dbo.Subscriptions", new[] { "UserId" });
            //DropForeignKey("dbo.Subscriptions", "VetId", "dbo.Vets");
            //DropForeignKey("dbo.Subscriptions", "ProductId", "dbo.Products");
            //DropForeignKey("dbo.Subscriptions", "PetId", "dbo.Pets");
            //DropForeignKey("dbo.Subscriptions", "UserId", "dbo.Users");
            AlterColumn("dbo.Subscriptions", "VetId", c => c.Int());
            AlterColumn("dbo.Subscriptions", "ProductId", c => c.Int());
            AlterColumn("dbo.Subscriptions", "PetId", c => c.Int());
            AlterColumn("dbo.Subscriptions", "UserId", c => c.Int());
            //CreateIndex("dbo.Subscriptions", "VetId");
            //CreateIndex("dbo.Subscriptions", "ProductId");
            //CreateIndex("dbo.Subscriptions", "PetId");
            //CreateIndex("dbo.Subscriptions", "UserId");
            //AddForeignKey("dbo.Subscriptions", "VetId", "dbo.Vets", "Id");
            //AddForeignKey("dbo.Subscriptions", "ProductId", "dbo.Products", "Id");
            //AddForeignKey("dbo.Subscriptions", "PetId", "dbo.Pets", "Id");
            //AddForeignKey("dbo.Subscriptions", "UserId", "dbo.Users", "Id");
        }
    }
}
