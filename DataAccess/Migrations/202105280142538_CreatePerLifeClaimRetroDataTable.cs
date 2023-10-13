namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class CreatePerLifeClaimRetroDataTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PerLifeClaimRetroData",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    PerLifeClaimDataId = c.Int(nullable: false),
                    MlreShare = c.Double(),
                    RetroClaimRecoveryAmount = c.Double(),
                    LateInterest = c.Double(),
                    ExGratia = c.Double(),
                    RetroRecoveryId = c.Int(nullable: false),
                    RetroTreatyId = c.Int(nullable: false),
                    RetroRatio = c.Double(),
                    Aar = c.Double(),
                    ComputedRetroRecoveryAmount = c.Double(),
                    ComputedRetroLateInterest = c.Double(),
                    ComputedRetroExGratia = c.Double(),
                    ReportedSoaQuarter = c.String(maxLength: 30),
                    RetroRecoveryAmount = c.Double(),
                    RetroLateInterest = c.Double(),
                    RetroExGratia = c.Double(),
                    ComputedClaimCategory = c.Int(nullable: false),
                    ClaimCategory = c.Int(nullable: false),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.PerLifeClaims", t => t.PerLifeClaimDataId)
                .ForeignKey("dbo.RetroTreaties", t => t.RetroTreatyId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.PerLifeClaimDataId)
                .Index(t => t.RetroRecoveryId)
                .Index(t => t.RetroTreatyId)
                .Index(t => t.ReportedSoaQuarter)
                .Index(t => t.ComputedClaimCategory)
                .Index(t => t.ClaimCategory)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

        }

        public override void Down()
        {
            DropForeignKey("dbo.PerLifeClaimRetroData", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.PerLifeClaimRetroData", "RetroTreatyId", "dbo.RetroTreaties");
            DropForeignKey("dbo.PerLifeClaimRetroData", "PerLifeClaimDataId", "dbo.PerLifeClaims");
            DropForeignKey("dbo.PerLifeClaimRetroData", "CreatedById", "dbo.Users");
            DropIndex("dbo.PerLifeClaimRetroData", new[] { "UpdatedById" });
            DropIndex("dbo.PerLifeClaimRetroData", new[] { "CreatedById" });
            DropIndex("dbo.PerLifeClaimRetroData", new[] { "ClaimCategory" });
            DropIndex("dbo.PerLifeClaimRetroData", new[] { "ComputedClaimCategory" });
            DropIndex("dbo.PerLifeClaimRetroData", new[] { "ReportedSoaQuarter" });
            DropIndex("dbo.PerLifeClaimRetroData", new[] { "RetroTreatyId" });
            DropIndex("dbo.PerLifeClaimRetroData", new[] { "RetroRecoveryId" });
            DropIndex("dbo.PerLifeClaimRetroData", new[] { "PerLifeClaimDataId" });
            DropTable("dbo.PerLifeClaimRetroData");
        }
    }
}
