namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class CreatePerLifeAggregationDetailDataMonthlyTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PerLifeAggregationDetailDataMonthly",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    PerLifeAggregationDetailDataId = c.Int(nullable: false),
                    RiskYear = c.Int(nullable: false),
                    RiskMonth = c.Int(nullable: false),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.PerLifeAggregationDetailData", t => t.PerLifeAggregationDetailDataId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.PerLifeAggregationDetailDataId)
                .Index(t => t.RiskYear)
                .Index(t => t.RiskMonth)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

        }

        public override void Down()
        {
            DropForeignKey("dbo.PerLifeAggregationDetailDataMonthly", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.PerLifeAggregationDetailDataMonthly", "PerLifeAggregationDetailDataId", "dbo.PerLifeAggregationDetailData");
            DropForeignKey("dbo.PerLifeAggregationDetailDataMonthly", "CreatedById", "dbo.Users");
            DropIndex("dbo.PerLifeAggregationDetailDataMonthly", new[] { "UpdatedById" });
            DropIndex("dbo.PerLifeAggregationDetailDataMonthly", new[] { "CreatedById" });
            DropIndex("dbo.PerLifeAggregationDetailDataMonthly", new[] { "RiskMonth" });
            DropIndex("dbo.PerLifeAggregationDetailDataMonthly", new[] { "RiskYear" });
            DropIndex("dbo.PerLifeAggregationDetailDataMonthly", new[] { "PerLifeAggregationDetailDataId" });
            DropTable("dbo.PerLifeAggregationDetailDataMonthly");
        }
    }
}
