namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatePerLifeAggregationMonthlyDataTable2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PerLifeAggregationMonthlyData", "RetroRatio", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PerLifeAggregationMonthlyData", "RetroRatio");
        }
    }
}
