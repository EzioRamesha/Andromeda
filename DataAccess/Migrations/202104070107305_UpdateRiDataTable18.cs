namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRiDataTable18 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.RiData", new[] { "RetroShare1" });
            DropIndex("dbo.RiData", new[] { "RetroShare2" });
            DropIndex("dbo.RiData", new[] { "RetroShare3" });
            DropIndex("dbo.RiData", new[] { "RetroAar1" });
            DropIndex("dbo.RiData", new[] { "RetroAar2" });
            DropIndex("dbo.RiData", new[] { "RetroAar3" });
            DropIndex("dbo.RiData", new[] { "RetroReinsurancePremium1" });
            DropIndex("dbo.RiData", new[] { "RetroReinsurancePremium2" });
            DropIndex("dbo.RiData", new[] { "RetroReinsurancePremium3" });
            DropIndex("dbo.RiData", new[] { "RetroDiscount1" });
            DropIndex("dbo.RiData", new[] { "RetroDiscount2" });
            DropIndex("dbo.RiData", new[] { "RetroDiscount3" });
            DropIndex("dbo.RiData", new[] { "RetroNetPremium1" });
            DropIndex("dbo.RiData", new[] { "RetroNetPremium2" });
            DropIndex("dbo.RiData", new[] { "RetroNetPremium3" });
            DropIndex("dbo.RiData", new[] { "AarShare2" });
            DropIndex("dbo.RiData", new[] { "AarCap2" });
            DropIndex("dbo.RiData", new[] { "WakalahFeePercentage" });
            DropIndex("dbo.RiData", new[] { "TreatyNumber" });
            CreateIndex("dbo.RiData", "IgnoreFinalise");
        }
        
        public override void Down()
        {
            DropIndex("dbo.RiData", new[] { "IgnoreFinalise" });
            CreateIndex("dbo.RiData", "TreatyNumber");
            CreateIndex("dbo.RiData", "WakalahFeePercentage");
            CreateIndex("dbo.RiData", "AarCap2");
            CreateIndex("dbo.RiData", "AarShare2");
            CreateIndex("dbo.RiData", "RetroNetPremium3");
            CreateIndex("dbo.RiData", "RetroNetPremium2");
            CreateIndex("dbo.RiData", "RetroNetPremium1");
            CreateIndex("dbo.RiData", "RetroDiscount3");
            CreateIndex("dbo.RiData", "RetroDiscount2");
            CreateIndex("dbo.RiData", "RetroDiscount1");
            CreateIndex("dbo.RiData", "RetroReinsurancePremium3");
            CreateIndex("dbo.RiData", "RetroReinsurancePremium2");
            CreateIndex("dbo.RiData", "RetroReinsurancePremium1");
            CreateIndex("dbo.RiData", "RetroAar3");
            CreateIndex("dbo.RiData", "RetroAar2");
            CreateIndex("dbo.RiData", "RetroAar1");
            CreateIndex("dbo.RiData", "RetroShare3");
            CreateIndex("dbo.RiData", "RetroShare2");
            CreateIndex("dbo.RiData", "RetroShare1");
        }
    }
}
