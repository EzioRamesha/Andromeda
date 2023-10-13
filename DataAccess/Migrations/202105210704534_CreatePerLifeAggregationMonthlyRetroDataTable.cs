namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class CreatePerLifeAggregationMonthlyRetroDataTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PerLifeAggregationMonthlyRetroData",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    PerLifeAggregationDetailDataMonthlyId = c.Int(nullable: false),
                    RetroParty = c.String(maxLength: 50),
                    RetroAmount = c.Double(),
                    RetroGrossPremium = c.Double(),
                    RetroNetPremium = c.Double(),
                    RetroDiscount = c.Double(),
                    RetroGst = c.Double(),
                    CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    UpdatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatedById = c.Int(nullable: false),
                    UpdatedById = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedById)
                .ForeignKey("dbo.PerLifeAggregationDetailDataMonthly", t => t.PerLifeAggregationDetailDataMonthlyId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.PerLifeAggregationDetailDataMonthlyId)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdatedById);

        }

        public override void Down()
        {
            DropForeignKey("dbo.PerLifeAggregationMonthlyRetroData", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.PerLifeAggregationMonthlyRetroData", "PerLifeAggregationDetailDataMonthlyId", "dbo.PerLifeAggregationDetailDataMonthly");
            DropForeignKey("dbo.PerLifeAggregationMonthlyRetroData", "CreatedById", "dbo.Users");
            DropIndex("dbo.PerLifeAggregationMonthlyRetroData", new[] { "UpdatedById" });
            DropIndex("dbo.PerLifeAggregationMonthlyRetroData", new[] { "CreatedById" });
            DropIndex("dbo.PerLifeAggregationMonthlyRetroData", new[] { "PerLifeAggregationDetailDataMonthlyId" });
            DropTable("dbo.PerLifeAggregationMonthlyRetroData");
        }
    }
}
