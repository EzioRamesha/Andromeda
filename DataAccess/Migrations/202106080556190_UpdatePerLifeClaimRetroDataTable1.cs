namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatePerLifeClaimRetroDataTable1 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.PerLifeClaimRetroData", new[] { "RetroRecoveryId" });
            DropIndex("dbo.PerLifeClaimRetroData", new[] { "RetroTreatyId" });
            AlterColumn("dbo.PerLifeClaimRetroData", "RetroRecoveryId", c => c.Int());
            AlterColumn("dbo.PerLifeClaimRetroData", "RetroTreatyId", c => c.Int());
            CreateIndex("dbo.PerLifeClaimRetroData", "RetroRecoveryId");
            CreateIndex("dbo.PerLifeClaimRetroData", "RetroTreatyId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.PerLifeClaimRetroData", new[] { "RetroTreatyId" });
            DropIndex("dbo.PerLifeClaimRetroData", new[] { "RetroRecoveryId" });
            AlterColumn("dbo.PerLifeClaimRetroData", "RetroTreatyId", c => c.Int(nullable: false));
            AlterColumn("dbo.PerLifeClaimRetroData", "RetroRecoveryId", c => c.Int(nullable: false));
            CreateIndex("dbo.PerLifeClaimRetroData", "RetroTreatyId");
            CreateIndex("dbo.PerLifeClaimRetroData", "RetroRecoveryId");
        }
    }
}
