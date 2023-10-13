namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class UpdatePerLifeAggregationDetailDataTable1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PerLifeAggregationDetailData", "ProceedStatus", c => c.Int(nullable: false));
            CreateIndex("dbo.PerLifeAggregationDetailData", "ProceedStatus");
        }

        public override void Down()
        {
            DropIndex("dbo.PerLifeAggregationDetailData", new[] { "ProceedStatus" });
            DropColumn("dbo.PerLifeAggregationDetailData", "ProceedStatus");
        }
    }
}
