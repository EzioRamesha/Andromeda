namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatePerLifeAggregationDetailDataMonthlyTable1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PerLifeAggregationDetailDataMonthly", "Aar", c => c.Double(nullable: false));
            AddColumn("dbo.PerLifeAggregationDetailDataMonthly", "NetPremium", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PerLifeAggregationDetailDataMonthly", "NetPremium");
            DropColumn("dbo.PerLifeAggregationDetailDataMonthly", "Aar");
        }
    }
}
