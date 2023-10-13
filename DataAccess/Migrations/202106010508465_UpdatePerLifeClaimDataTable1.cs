namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatePerLifeClaimDataTable1 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.PerLifeClaimData", new[] { "PerLifeAggregationDetailDataId" });
            DropIndex("dbo.PerLifeClaimData", new[] { "ClaimCategory" });
            DropIndex("dbo.PerLifeClaimData", new[] { "ClaimRecoveryStatus" });
            DropIndex("dbo.PerLifeClaimData", new[] { "ClaimRecoveryDecision" });
            DropIndex("dbo.PerLifeClaimData", new[] { "MovementType" });
            DropIndex("dbo.PerLifeClaimData", new[] { "RetroOutputId" });
            DropIndex("dbo.PerLifeClaimData", new[] { "RetainPoolId" });
            DropIndex("dbo.PerLifeClaimData", new[] { "NoOfRetroTreaty" });
            DropIndex("dbo.PerLifeClaimData", new[] { "RetroRecoveryId" });
            AlterColumn("dbo.PerLifeClaimData", "PerLifeAggregationDetailDataId", c => c.Int());
            AlterColumn("dbo.PerLifeClaimData", "ClaimCategory", c => c.Int());
            AlterColumn("dbo.PerLifeClaimData", "ClaimRecoveryStatus", c => c.Int());
            AlterColumn("dbo.PerLifeClaimData", "ClaimRecoveryDecision", c => c.Int());
            AlterColumn("dbo.PerLifeClaimData", "MovementType", c => c.Int());
            AlterColumn("dbo.PerLifeClaimData", "RetroOutputId", c => c.Int());
            AlterColumn("dbo.PerLifeClaimData", "RetainPoolId", c => c.Int());
            AlterColumn("dbo.PerLifeClaimData", "NoOfRetroTreaty", c => c.Int());
            AlterColumn("dbo.PerLifeClaimData", "RetroRecoveryId", c => c.Int());
            CreateIndex("dbo.PerLifeClaimData", "PerLifeAggregationDetailDataId");
            CreateIndex("dbo.PerLifeClaimData", "ClaimCategory");
            CreateIndex("dbo.PerLifeClaimData", "ClaimRecoveryStatus");
            CreateIndex("dbo.PerLifeClaimData", "ClaimRecoveryDecision");
            CreateIndex("dbo.PerLifeClaimData", "MovementType");
            CreateIndex("dbo.PerLifeClaimData", "RetroOutputId");
            CreateIndex("dbo.PerLifeClaimData", "RetainPoolId");
            CreateIndex("dbo.PerLifeClaimData", "NoOfRetroTreaty");
            CreateIndex("dbo.PerLifeClaimData", "RetroRecoveryId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.PerLifeClaimData", new[] { "RetroRecoveryId" });
            DropIndex("dbo.PerLifeClaimData", new[] { "NoOfRetroTreaty" });
            DropIndex("dbo.PerLifeClaimData", new[] { "RetainPoolId" });
            DropIndex("dbo.PerLifeClaimData", new[] { "RetroOutputId" });
            DropIndex("dbo.PerLifeClaimData", new[] { "MovementType" });
            DropIndex("dbo.PerLifeClaimData", new[] { "ClaimRecoveryDecision" });
            DropIndex("dbo.PerLifeClaimData", new[] { "ClaimRecoveryStatus" });
            DropIndex("dbo.PerLifeClaimData", new[] { "ClaimCategory" });
            DropIndex("dbo.PerLifeClaimData", new[] { "PerLifeAggregationDetailDataId" });
            AlterColumn("dbo.PerLifeClaimData", "RetroRecoveryId", c => c.Int(nullable: false));
            AlterColumn("dbo.PerLifeClaimData", "NoOfRetroTreaty", c => c.Int(nullable: false));
            AlterColumn("dbo.PerLifeClaimData", "RetainPoolId", c => c.Int(nullable: false));
            AlterColumn("dbo.PerLifeClaimData", "RetroOutputId", c => c.Int(nullable: false));
            AlterColumn("dbo.PerLifeClaimData", "MovementType", c => c.Int(nullable: false));
            AlterColumn("dbo.PerLifeClaimData", "ClaimRecoveryDecision", c => c.Int(nullable: false));
            AlterColumn("dbo.PerLifeClaimData", "ClaimRecoveryStatus", c => c.Int(nullable: false));
            AlterColumn("dbo.PerLifeClaimData", "ClaimCategory", c => c.Int(nullable: false));
            AlterColumn("dbo.PerLifeClaimData", "PerLifeAggregationDetailDataId", c => c.Int(nullable: false));
            CreateIndex("dbo.PerLifeClaimData", "RetroRecoveryId");
            CreateIndex("dbo.PerLifeClaimData", "NoOfRetroTreaty");
            CreateIndex("dbo.PerLifeClaimData", "RetainPoolId");
            CreateIndex("dbo.PerLifeClaimData", "RetroOutputId");
            CreateIndex("dbo.PerLifeClaimData", "MovementType");
            CreateIndex("dbo.PerLifeClaimData", "ClaimRecoveryDecision");
            CreateIndex("dbo.PerLifeClaimData", "ClaimRecoveryStatus");
            CreateIndex("dbo.PerLifeClaimData", "ClaimCategory");
            CreateIndex("dbo.PerLifeClaimData", "PerLifeAggregationDetailDataId");
        }
    }
}
