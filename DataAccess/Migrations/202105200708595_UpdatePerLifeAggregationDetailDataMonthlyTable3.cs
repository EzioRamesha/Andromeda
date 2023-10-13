namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatePerLifeAggregationDetailDataMonthlyTable3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PerLifeAggregationDetailDataMonthly", "UniqueKeyPerLife", c => c.String());
            AddColumn("dbo.PerLifeAggregationDetailDataMonthly", "RetroPremFreq", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PerLifeAggregationDetailDataMonthly", "RetroPremFreq");
            DropColumn("dbo.PerLifeAggregationDetailDataMonthly", "UniqueKeyPerLife");
        }
    }
}
