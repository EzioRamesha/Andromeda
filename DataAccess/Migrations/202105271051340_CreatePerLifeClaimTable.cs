namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class CreatePerLifeClaimTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PerLifeClaimData",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    PerLifeClaimId = c.Int(nullable: false),
                    ClaimRegisterHistoryId = c.Long(nullable: false),
                    PerLifeAggregationDetailDataId = c.Int(nullable: false),
                    IsException = c.Boolean(nullable: false),
                    ClaimCategory = c.Int(nullable: false),
                    IsExcludePerformClaimRecovery = c.Boolean(nullable: false),
                    ClaimRecoveryStatus = c.Int(nullable: false),
                    ClaimRecoveryDecision = c.Int(nullable: false),
                    MovementType = c.Int(nullable: false),
                    PerLifeRetro = c.String(maxLength: 255),
                    RetroOutputId = c.Int(nullable: false),
                    RetainPoolId = c.Int(nullable: false),
                    NoOfRetroTreaty = c.Int(nullable: false),
                    RetroRecoveryId = c.Int(nullable: false),
                    LateInterestShareFlag = c.String(maxLength: 255),
                    ExGratiaShareFlag = c.String(maxLength: 255),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ClaimRegisterHistories", t => t.ClaimRegisterHistoryId)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.PerLifeAggregationDetailData", t => t.PerLifeAggregationDetailDataId)
                .ForeignKey("dbo.PerLifeClaims", t => t.PerLifeClaimId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.PerLifeClaimId)
                .Index(t => t.ClaimRegisterHistoryId)
                .Index(t => t.PerLifeAggregationDetailDataId)
                .Index(t => t.IsException)
                .Index(t => t.ClaimCategory)
                .Index(t => t.IsExcludePerformClaimRecovery)
                .Index(t => t.ClaimRecoveryStatus)
                .Index(t => t.ClaimRecoveryDecision)
                .Index(t => t.MovementType)
                .Index(t => t.PerLifeRetro)
                .Index(t => t.RetroOutputId)
                .Index(t => t.RetainPoolId)
                .Index(t => t.NoOfRetroTreaty)
                .Index(t => t.RetroRecoveryId)
                .Index(t => t.LateInterestShareFlag)
                .Index(t => t.ExGratiaShareFlag)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.PerLifeClaims",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    CutOffId = c.Int(nullable: false),
                    FundsAccountingTypePickListDetailId = c.Int(nullable: false),
                    Status = c.Int(nullable: false),
                    SoaQuarter = c.String(maxLength: 30),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.CutOff", t => t.CutOffId)
                .ForeignKey("dbo.PickListDetails", t => t.FundsAccountingTypePickListDetailId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.CutOffId)
                .Index(t => t.FundsAccountingTypePickListDetailId)
                .Index(t => t.Status)
                .Index(t => t.SoaQuarter)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

        }

        public override void Down()
        {
            DropForeignKey("dbo.PerLifeClaimData", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.PerLifeClaimData", "PerLifeClaimId", "dbo.PerLifeClaims");
            DropForeignKey("dbo.PerLifeClaims", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.PerLifeClaims", "FundsAccountingTypePickListDetailId", "dbo.PickListDetails");
            DropForeignKey("dbo.PerLifeClaims", "CutOffId", "dbo.CutOff");
            DropForeignKey("dbo.PerLifeClaims", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.PerLifeClaimData", "PerLifeAggregationDetailDataId", "dbo.PerLifeAggregationDetailData");
            DropForeignKey("dbo.PerLifeClaimData", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.PerLifeClaimData", "ClaimRegisterHistoryId", "dbo.ClaimRegisterHistories");
            DropIndex("dbo.PerLifeClaims", new[] { "UpdatedById" });
            DropIndex("dbo.PerLifeClaims", new[] { "CreatedById" });
            DropIndex("dbo.PerLifeClaims", new[] { "SoaQuarter" });
            DropIndex("dbo.PerLifeClaims", new[] { "Status" });
            DropIndex("dbo.PerLifeClaims", new[] { "FundsAccountingTypePickListDetailId" });
            DropIndex("dbo.PerLifeClaims", new[] { "CutOffId" });
            DropIndex("dbo.PerLifeClaimData", new[] { "UpdatedById" });
            DropIndex("dbo.PerLifeClaimData", new[] { "CreatedById" });
            DropIndex("dbo.PerLifeClaimData", new[] { "ExGratiaShareFlag" });
            DropIndex("dbo.PerLifeClaimData", new[] { "LateInterestShareFlag" });
            DropIndex("dbo.PerLifeClaimData", new[] { "RetroRecoveryId" });
            DropIndex("dbo.PerLifeClaimData", new[] { "NoOfRetroTreaty" });
            DropIndex("dbo.PerLifeClaimData", new[] { "RetainPoolId" });
            DropIndex("dbo.PerLifeClaimData", new[] { "RetroOutputId" });
            DropIndex("dbo.PerLifeClaimData", new[] { "PerLifeRetro" });
            DropIndex("dbo.PerLifeClaimData", new[] { "MovementType" });
            DropIndex("dbo.PerLifeClaimData", new[] { "ClaimRecoveryDecision" });
            DropIndex("dbo.PerLifeClaimData", new[] { "ClaimRecoveryStatus" });
            DropIndex("dbo.PerLifeClaimData", new[] { "IsExcludePerformClaimRecovery" });
            DropIndex("dbo.PerLifeClaimData", new[] { "ClaimCategory" });
            DropIndex("dbo.PerLifeClaimData", new[] { "IsException" });
            DropIndex("dbo.PerLifeClaimData", new[] { "PerLifeAggregationDetailDataId" });
            DropIndex("dbo.PerLifeClaimData", new[] { "ClaimRegisterHistoryId" });
            DropIndex("dbo.PerLifeClaimData", new[] { "PerLifeClaimId" });
            DropTable("dbo.PerLifeClaims");
            DropTable("dbo.PerLifeClaimData");
        }
    }
}
