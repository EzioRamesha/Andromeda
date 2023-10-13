namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatePremiumSpreadTableDetailTable1 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.PremiumSpreadTableDetails", new[] { "AgeFrom" });
            DropIndex("dbo.PremiumSpreadTableDetails", new[] { "AgeTo" });
            AlterColumn("dbo.PremiumSpreadTableDetails", "CedingPlanCode", c => c.String(nullable: false, storeType: "ntext"));
            AlterColumn("dbo.PremiumSpreadTableDetails", "AgeFrom", c => c.Int());
            AlterColumn("dbo.PremiumSpreadTableDetails", "AgeTo", c => c.Int());
            CreateIndex("dbo.PremiumSpreadTableDetails", "AgeFrom");
            CreateIndex("dbo.PremiumSpreadTableDetails", "AgeTo");
        }
        
        public override void Down()
        {
            DropIndex("dbo.PremiumSpreadTableDetails", new[] { "AgeTo" });
            DropIndex("dbo.PremiumSpreadTableDetails", new[] { "AgeFrom" });
            AlterColumn("dbo.PremiumSpreadTableDetails", "AgeTo", c => c.Int(nullable: false));
            AlterColumn("dbo.PremiumSpreadTableDetails", "AgeFrom", c => c.Int(nullable: false));
            AlterColumn("dbo.PremiumSpreadTableDetails", "CedingPlanCode", c => c.String(nullable: false, maxLength: 30));
            CreateIndex("dbo.PremiumSpreadTableDetails", "AgeTo");
            CreateIndex("dbo.PremiumSpreadTableDetails", "AgeFrom");
        }
    }
}
