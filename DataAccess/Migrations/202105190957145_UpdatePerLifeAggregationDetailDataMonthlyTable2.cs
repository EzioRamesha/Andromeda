namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatePerLifeAggregationDetailDataMonthlyTable2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PerLifeAggregationDetailDataMonthly", "SumOfAar", c => c.Double());
            AddColumn("dbo.PerLifeAggregationDetailDataMonthly", "SumOfNetPremium", c => c.Double());
            AddColumn("dbo.PerLifeAggregationDetailDataMonthly", "RetentionLimit", c => c.Double());
            AddColumn("dbo.PerLifeAggregationDetailDataMonthly", "DistributedRetentionLimit", c => c.Double());
            AddColumn("dbo.PerLifeAggregationDetailDataMonthly", "RetroAmount", c => c.Double());
            AddColumn("dbo.PerLifeAggregationDetailDataMonthly", "DistributedRetroAmount", c => c.Double());
            AddColumn("dbo.PerLifeAggregationDetailDataMonthly", "AccumulativeRetainAmount", c => c.Double());
            AddColumn("dbo.PerLifeAggregationDetailDataMonthly", "RetroGrossPremium", c => c.Double());
            AddColumn("dbo.PerLifeAggregationDetailDataMonthly", "RetroNetPremium", c => c.Double());
            AddColumn("dbo.PerLifeAggregationDetailDataMonthly", "RetroDiscount", c => c.Double());
            AddColumn("dbo.PerLifeAggregationDetailDataMonthly", "RetroIndicator", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PerLifeAggregationDetailDataMonthly", "RetroIndicator");
            DropColumn("dbo.PerLifeAggregationDetailDataMonthly", "RetroDiscount");
            DropColumn("dbo.PerLifeAggregationDetailDataMonthly", "RetroNetPremium");
            DropColumn("dbo.PerLifeAggregationDetailDataMonthly", "RetroGrossPremium");
            DropColumn("dbo.PerLifeAggregationDetailDataMonthly", "AccumulativeRetainAmount");
            DropColumn("dbo.PerLifeAggregationDetailDataMonthly", "DistributedRetroAmount");
            DropColumn("dbo.PerLifeAggregationDetailDataMonthly", "RetroAmount");
            DropColumn("dbo.PerLifeAggregationDetailDataMonthly", "DistributedRetentionLimit");
            DropColumn("dbo.PerLifeAggregationDetailDataMonthly", "RetentionLimit");
            DropColumn("dbo.PerLifeAggregationDetailDataMonthly", "SumOfNetPremium");
            DropColumn("dbo.PerLifeAggregationDetailDataMonthly", "SumOfAar");
        }
    }
}
