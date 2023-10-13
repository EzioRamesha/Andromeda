namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRiDataWarehouseTable7 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RiDataWarehouse", "TotalDirectRetroNoClaimBonus", c => c.Double());
            AddColumn("dbo.RiDataWarehouse", "TotalDirectRetroDatabaseCommission", c => c.Double());
            AddColumn("dbo.RiDataWarehouse", "RetroPremiumSpread1", c => c.Double());
            AddColumn("dbo.RiDataWarehouse", "RetroPremiumSpread2", c => c.Double());
            AddColumn("dbo.RiDataWarehouse", "RetroPremiumSpread3", c => c.Double());
            AddColumn("dbo.RiDataWarehouse", "RetroNoClaimBonus1", c => c.Double());
            AddColumn("dbo.RiDataWarehouse", "RetroNoClaimBonus2", c => c.Double());
            AddColumn("dbo.RiDataWarehouse", "RetroNoClaimBonus3", c => c.Double());
            AddColumn("dbo.RiDataWarehouse", "RetroDatabaseCommission1", c => c.Double());
            AddColumn("dbo.RiDataWarehouse", "RetroDatabaseCommission2", c => c.Double());
            AddColumn("dbo.RiDataWarehouse", "RetroDatabaseCommission3", c => c.Double());
            AddColumn("dbo.RiDataWarehouse", "ConflictType", c => c.Int(nullable: false));
            CreateIndex("dbo.RiDataWarehouse", "ConflictType");
        }
        
        public override void Down()
        {
            DropIndex("dbo.RiDataWarehouse", new[] { "ConflictType" });
            DropColumn("dbo.RiDataWarehouse", "ConflictType");
            DropColumn("dbo.RiDataWarehouse", "RetroDatabaseCommission3");
            DropColumn("dbo.RiDataWarehouse", "RetroDatabaseCommission2");
            DropColumn("dbo.RiDataWarehouse", "RetroDatabaseCommission1");
            DropColumn("dbo.RiDataWarehouse", "RetroNoClaimBonus3");
            DropColumn("dbo.RiDataWarehouse", "RetroNoClaimBonus2");
            DropColumn("dbo.RiDataWarehouse", "RetroNoClaimBonus1");
            DropColumn("dbo.RiDataWarehouse", "RetroPremiumSpread3");
            DropColumn("dbo.RiDataWarehouse", "RetroPremiumSpread2");
            DropColumn("dbo.RiDataWarehouse", "RetroPremiumSpread1");
            DropColumn("dbo.RiDataWarehouse", "TotalDirectRetroDatabaseCommission");
            DropColumn("dbo.RiDataWarehouse", "TotalDirectRetroNoClaimBonus");
        }
    }
}
