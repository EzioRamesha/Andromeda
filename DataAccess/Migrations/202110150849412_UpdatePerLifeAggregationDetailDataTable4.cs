namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatePerLifeAggregationDetailDataTable4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PerLifeAggregationDetailData", "IsToAggregate", c => c.Boolean(nullable: false));
            CreateIndex("dbo.PerLifeAggregationDetailData", "IsToAggregate");
        }
        
        public override void Down()
        {
            DropIndex("dbo.PerLifeAggregationDetailData", new[] { "IsToAggregate" });
            DropColumn("dbo.PerLifeAggregationDetailData", "IsToAggregate");
        }
    }
}
