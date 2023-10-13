namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRiDataWarehouseHistoryTable7 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RiDataWarehouseHistories", "TotalDirectRetroNoClaimBonus", c => c.Double());
            AddColumn("dbo.RiDataWarehouseHistories", "TotalDirectRetroDatabaseCommission", c => c.Double());
            AddColumn("dbo.RiDataWarehouseHistories", "RetroPremiumSpread1", c => c.Double());
            AddColumn("dbo.RiDataWarehouseHistories", "RetroPremiumSpread2", c => c.Double());
            AddColumn("dbo.RiDataWarehouseHistories", "RetroPremiumSpread3", c => c.Double());
            AddColumn("dbo.RiDataWarehouseHistories", "RetroNoClaimBonus1", c => c.Double());
            AddColumn("dbo.RiDataWarehouseHistories", "RetroNoClaimBonus2", c => c.Double());
            AddColumn("dbo.RiDataWarehouseHistories", "RetroNoClaimBonus3", c => c.Double());
            AddColumn("dbo.RiDataWarehouseHistories", "RetroDatabaseCommission1", c => c.Double());
            AddColumn("dbo.RiDataWarehouseHistories", "RetroDatabaseCommission2", c => c.Double());
            AddColumn("dbo.RiDataWarehouseHistories", "RetroDatabaseCommission3", c => c.Double());
            AddColumn("dbo.RiDataWarehouseHistories", "ConflictType", c => c.Int(nullable: false));
            CreateIndex("dbo.RiDataWarehouseHistories", "ConflictType");
        }
        
        public override void Down()
        {
            DropIndex("dbo.RiDataWarehouseHistories", new[] { "ConflictType" });
            DropColumn("dbo.RiDataWarehouseHistories", "ConflictType");
            DropColumn("dbo.RiDataWarehouseHistories", "RetroDatabaseCommission3");
            DropColumn("dbo.RiDataWarehouseHistories", "RetroDatabaseCommission2");
            DropColumn("dbo.RiDataWarehouseHistories", "RetroDatabaseCommission1");
            DropColumn("dbo.RiDataWarehouseHistories", "RetroNoClaimBonus3");
            DropColumn("dbo.RiDataWarehouseHistories", "RetroNoClaimBonus2");
            DropColumn("dbo.RiDataWarehouseHistories", "RetroNoClaimBonus1");
            DropColumn("dbo.RiDataWarehouseHistories", "RetroPremiumSpread3");
            DropColumn("dbo.RiDataWarehouseHistories", "RetroPremiumSpread2");
            DropColumn("dbo.RiDataWarehouseHistories", "RetroPremiumSpread1");
            DropColumn("dbo.RiDataWarehouseHistories", "TotalDirectRetroDatabaseCommission");
            DropColumn("dbo.RiDataWarehouseHistories", "TotalDirectRetroNoClaimBonus");
        }
    }
}
