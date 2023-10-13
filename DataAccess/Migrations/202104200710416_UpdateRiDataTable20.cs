namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRiDataTable20 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.RiData", new[] { "IsConflict" });
            AddColumn("dbo.RiData", "TotalDirectRetroNoClaimBonus", c => c.Double());
            AddColumn("dbo.RiData", "TotalDirectRetroDatabaseCommission", c => c.Double());
            AddColumn("dbo.RiData", "RetroPremiumSpread1", c => c.Double());
            AddColumn("dbo.RiData", "RetroPremiumSpread2", c => c.Double());
            AddColumn("dbo.RiData", "RetroPremiumSpread3", c => c.Double());
            AddColumn("dbo.RiData", "RetroNoClaimBonus1", c => c.Double());
            AddColumn("dbo.RiData", "RetroNoClaimBonus2", c => c.Double());
            AddColumn("dbo.RiData", "RetroNoClaimBonus3", c => c.Double());
            AddColumn("dbo.RiData", "RetroDatabaseCommission1", c => c.Double());
            AddColumn("dbo.RiData", "RetroDatabaseCommission2", c => c.Double());
            AddColumn("dbo.RiData", "RetroDatabaseCommission3", c => c.Double());
            AddColumn("dbo.RiData", "ConflictType", c => c.Int(nullable: false));
            CreateIndex("dbo.RiData", "ConflictType");
            DropColumn("dbo.RiData", "IsConflict");
        }
        
        public override void Down()
        {
            AddColumn("dbo.RiData", "IsConflict", c => c.Boolean(nullable: false));
            DropIndex("dbo.RiData", new[] { "ConflictType" });
            DropColumn("dbo.RiData", "ConflictType");
            DropColumn("dbo.RiData", "RetroDatabaseCommission3");
            DropColumn("dbo.RiData", "RetroDatabaseCommission2");
            DropColumn("dbo.RiData", "RetroDatabaseCommission1");
            DropColumn("dbo.RiData", "RetroNoClaimBonus3");
            DropColumn("dbo.RiData", "RetroNoClaimBonus2");
            DropColumn("dbo.RiData", "RetroNoClaimBonus1");
            DropColumn("dbo.RiData", "RetroPremiumSpread3");
            DropColumn("dbo.RiData", "RetroPremiumSpread2");
            DropColumn("dbo.RiData", "RetroPremiumSpread1");
            DropColumn("dbo.RiData", "TotalDirectRetroDatabaseCommission");
            DropColumn("dbo.RiData", "TotalDirectRetroNoClaimBonus");
            CreateIndex("dbo.RiData", "IsConflict");
        }
    }
}
