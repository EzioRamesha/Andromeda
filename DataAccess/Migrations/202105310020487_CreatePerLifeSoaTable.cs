namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class CreatePerLifeSoaTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PerLifeSoa",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    RetroPartyId = c.Int(nullable: false),
                    RetroTreatyId = c.Int(nullable: false),
                    Status = c.Int(nullable: false),
                    SoaQuarter = c.String(maxLength: 30),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.RetroParties", t => t.RetroPartyId)
                .ForeignKey("dbo.RetroTreaties", t => t.RetroTreatyId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.RetroPartyId)
                .Index(t => t.RetroTreatyId)
                .Index(t => t.Status)
                .Index(t => t.SoaQuarter)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

            CreateTable(
                "dbo.PerLifeSoaData",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    PerLifeSoaId = c.Int(nullable: false),
                    PerLifeAggregationDetailDataId = c.Int(nullable: false),
                    PerLifeClaimDataId = c.Int(nullable: false),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.PerLifeAggregationDetailData", t => t.PerLifeAggregationDetailDataId)
                .ForeignKey("dbo.PerLifeClaimData", t => t.PerLifeClaimDataId)
                .ForeignKey("dbo.PerLifeSoa", t => t.PerLifeSoaId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.PerLifeSoaId)
                .Index(t => t.PerLifeAggregationDetailDataId)
                .Index(t => t.PerLifeClaimDataId)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

        }

        public override void Down()
        {
            DropForeignKey("dbo.PerLifeSoaData", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.PerLifeSoaData", "PerLifeSoaId", "dbo.PerLifeSoa");
            DropForeignKey("dbo.PerLifeSoaData", "PerLifeClaimDataId", "dbo.PerLifeClaimData");
            DropForeignKey("dbo.PerLifeSoaData", "PerLifeAggregationDetailDataId", "dbo.PerLifeAggregationDetailData");
            DropForeignKey("dbo.PerLifeSoaData", "CreatedById", "dbo.Users");
            DropForeignKey("dbo.PerLifeSoa", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.PerLifeSoa", "RetroTreatyId", "dbo.RetroTreaties");
            DropForeignKey("dbo.PerLifeSoa", "RetroPartyId", "dbo.RetroParties");
            DropForeignKey("dbo.PerLifeSoa", "CreatedById", "dbo.Users");
            DropIndex("dbo.PerLifeSoaData", new[] { "UpdatedById" });
            DropIndex("dbo.PerLifeSoaData", new[] { "CreatedById" });
            DropIndex("dbo.PerLifeSoaData", new[] { "PerLifeClaimDataId" });
            DropIndex("dbo.PerLifeSoaData", new[] { "PerLifeAggregationDetailDataId" });
            DropIndex("dbo.PerLifeSoaData", new[] { "PerLifeSoaId" });
            DropIndex("dbo.PerLifeSoa", new[] { "UpdatedById" });
            DropIndex("dbo.PerLifeSoa", new[] { "CreatedById" });
            DropIndex("dbo.PerLifeSoa", new[] { "SoaQuarter" });
            DropIndex("dbo.PerLifeSoa", new[] { "Status" });
            DropIndex("dbo.PerLifeSoa", new[] { "RetroTreatyId" });
            DropIndex("dbo.PerLifeSoa", new[] { "RetroPartyId" });
            DropTable("dbo.PerLifeSoaData");
            DropTable("dbo.PerLifeSoa");
        }
    }
}
