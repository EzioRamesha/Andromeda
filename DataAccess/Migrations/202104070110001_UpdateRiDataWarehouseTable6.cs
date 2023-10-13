namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRiDataWarehouseTable6 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.RiDataWarehouse", new[] { "RetroShare1" });
            DropIndex("dbo.RiDataWarehouse", new[] { "RetroShare2" });
            DropIndex("dbo.RiDataWarehouse", new[] { "RetroShare3" });
            DropIndex("dbo.RiDataWarehouse", new[] { "RetroAar1" });
            DropIndex("dbo.RiDataWarehouse", new[] { "RetroAar2" });
            DropIndex("dbo.RiDataWarehouse", new[] { "RetroAar3" });
            DropIndex("dbo.RiDataWarehouse", new[] { "RetroReinsurancePremium1" });
            DropIndex("dbo.RiDataWarehouse", new[] { "RetroReinsurancePremium2" });
            DropIndex("dbo.RiDataWarehouse", new[] { "RetroReinsurancePremium3" });
            DropIndex("dbo.RiDataWarehouse", new[] { "RetroDiscount1" });
            DropIndex("dbo.RiDataWarehouse", new[] { "RetroDiscount2" });
            DropIndex("dbo.RiDataWarehouse", new[] { "RetroDiscount3" });
            DropIndex("dbo.RiDataWarehouse", new[] { "RetroNetPremium1" });
            DropIndex("dbo.RiDataWarehouse", new[] { "RetroNetPremium2" });
            DropIndex("dbo.RiDataWarehouse", new[] { "RetroNetPremium3" });
            DropIndex("dbo.RiDataWarehouse", new[] { "AarShare2" });
            DropIndex("dbo.RiDataWarehouse", new[] { "AarCap2" });
            DropIndex("dbo.RiDataWarehouse", new[] { "WakalahFeePercentage" });
            DropIndex("dbo.RiDataWarehouse", new[] { "TreatyNumber" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.RiDataWarehouse", "TreatyNumber");
            CreateIndex("dbo.RiDataWarehouse", "WakalahFeePercentage");
            CreateIndex("dbo.RiDataWarehouse", "AarCap2");
            CreateIndex("dbo.RiDataWarehouse", "AarShare2");
            CreateIndex("dbo.RiDataWarehouse", "RetroNetPremium3");
            CreateIndex("dbo.RiDataWarehouse", "RetroNetPremium2");
            CreateIndex("dbo.RiDataWarehouse", "RetroNetPremium1");
            CreateIndex("dbo.RiDataWarehouse", "RetroDiscount3");
            CreateIndex("dbo.RiDataWarehouse", "RetroDiscount2");
            CreateIndex("dbo.RiDataWarehouse", "RetroDiscount1");
            CreateIndex("dbo.RiDataWarehouse", "RetroReinsurancePremium3");
            CreateIndex("dbo.RiDataWarehouse", "RetroReinsurancePremium2");
            CreateIndex("dbo.RiDataWarehouse", "RetroReinsurancePremium1");
            CreateIndex("dbo.RiDataWarehouse", "RetroAar3");
            CreateIndex("dbo.RiDataWarehouse", "RetroAar2");
            CreateIndex("dbo.RiDataWarehouse", "RetroAar1");
            CreateIndex("dbo.RiDataWarehouse", "RetroShare3");
            CreateIndex("dbo.RiDataWarehouse", "RetroShare2");
            CreateIndex("dbo.RiDataWarehouse", "RetroShare1");
        }
    }
}
