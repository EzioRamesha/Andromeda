namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRiDataWarehouseHistoryTable6 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.RiDataWarehouseHistories", new[] { "RetroShare1" });
            DropIndex("dbo.RiDataWarehouseHistories", new[] { "RetroShare2" });
            DropIndex("dbo.RiDataWarehouseHistories", new[] { "RetroShare3" });
            DropIndex("dbo.RiDataWarehouseHistories", new[] { "RetroAar1" });
            DropIndex("dbo.RiDataWarehouseHistories", new[] { "RetroAar2" });
            DropIndex("dbo.RiDataWarehouseHistories", new[] { "RetroAar3" });
            DropIndex("dbo.RiDataWarehouseHistories", new[] { "RetroReinsurancePremium1" });
            DropIndex("dbo.RiDataWarehouseHistories", new[] { "RetroReinsurancePremium2" });
            DropIndex("dbo.RiDataWarehouseHistories", new[] { "RetroReinsurancePremium3" });
            DropIndex("dbo.RiDataWarehouseHistories", new[] { "RetroDiscount1" });
            DropIndex("dbo.RiDataWarehouseHistories", new[] { "RetroDiscount2" });
            DropIndex("dbo.RiDataWarehouseHistories", new[] { "RetroDiscount3" });
            DropIndex("dbo.RiDataWarehouseHistories", new[] { "RetroNetPremium1" });
            DropIndex("dbo.RiDataWarehouseHistories", new[] { "RetroNetPremium2" });
            DropIndex("dbo.RiDataWarehouseHistories", new[] { "RetroNetPremium3" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.RiDataWarehouseHistories", "RetroNetPremium3");
            CreateIndex("dbo.RiDataWarehouseHistories", "RetroNetPremium2");
            CreateIndex("dbo.RiDataWarehouseHistories", "RetroNetPremium1");
            CreateIndex("dbo.RiDataWarehouseHistories", "RetroDiscount3");
            CreateIndex("dbo.RiDataWarehouseHistories", "RetroDiscount2");
            CreateIndex("dbo.RiDataWarehouseHistories", "RetroDiscount1");
            CreateIndex("dbo.RiDataWarehouseHistories", "RetroReinsurancePremium3");
            CreateIndex("dbo.RiDataWarehouseHistories", "RetroReinsurancePremium2");
            CreateIndex("dbo.RiDataWarehouseHistories", "RetroReinsurancePremium1");
            CreateIndex("dbo.RiDataWarehouseHistories", "RetroAar3");
            CreateIndex("dbo.RiDataWarehouseHistories", "RetroAar2");
            CreateIndex("dbo.RiDataWarehouseHistories", "RetroAar1");
            CreateIndex("dbo.RiDataWarehouseHistories", "RetroShare3");
            CreateIndex("dbo.RiDataWarehouseHistories", "RetroShare2");
            CreateIndex("dbo.RiDataWarehouseHistories", "RetroShare1");
        }
    }
}
