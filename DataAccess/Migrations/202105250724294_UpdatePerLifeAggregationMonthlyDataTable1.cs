namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatePerLifeAggregationMonthlyDataTable1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PerLifeAggregationMonthlyData", "Errors", c => c.String(storeType: "ntext"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PerLifeAggregationMonthlyData", "Errors");
        }
    }
}
