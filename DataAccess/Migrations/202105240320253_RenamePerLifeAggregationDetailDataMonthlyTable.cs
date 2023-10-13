namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenamePerLifeAggregationDetailDataMonthlyTable : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.PerLifeAggregationDetailDataMonthly", newName: "PerLifeAggregationMonthlyData");
            RenameColumn(table: "dbo.PerLifeAggregationMonthlyRetroData", name: "PerLifeAggregationDetailDataMonthlyId", newName: "PerLifeAggregationMonthlyDataId");
        }
        
        public override void Down()
        {
            RenameColumn(table: "dbo.PerLifeAggregationMonthlyRetroData", name: "PerLifeAggregationMonthlyDataId", newName: "PerLifeAggregationDetailDataMonthlyId");
            RenameTable(name: "dbo.PerLifeAggregationMonthlyData", newName: "PerLifeAggregationDetailDataMonthly");
        }
    }
}
