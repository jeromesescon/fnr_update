namespace FnR.Databases.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PetToConditionRelation : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Conditions", "Pet_Id", "dbo.Pets");
            DropIndex("dbo.Conditions", new[] { "Pet_Id" });
            CreateTable(
                "dbo.ConditionPets",
                c => new
                    {
                        Condition_Id = c.Int(nullable: false),
                        Pet_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Condition_Id, t.Pet_Id })
                .ForeignKey("dbo.Conditions", t => t.Condition_Id, cascadeDelete: true)
                .ForeignKey("dbo.Pets", t => t.Pet_Id, cascadeDelete: true)
                .Index(t => t.Condition_Id)
                .Index(t => t.Pet_Id);
            
            DropColumn("dbo.Conditions", "Pet_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Conditions", "Pet_Id", c => c.Int());
            DropIndex("dbo.ConditionPets", new[] { "Pet_Id" });
            DropIndex("dbo.ConditionPets", new[] { "Condition_Id" });
            DropForeignKey("dbo.ConditionPets", "Pet_Id", "dbo.Pets");
            DropForeignKey("dbo.ConditionPets", "Condition_Id", "dbo.Conditions");
            DropTable("dbo.ConditionPets");
            CreateIndex("dbo.Conditions", "Pet_Id");
            AddForeignKey("dbo.Conditions", "Pet_Id", "dbo.Pets", "Id");
        }
    }
}
