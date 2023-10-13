namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatePerLifeAggregationMonthlyRetroData1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PerLifeAggregationMonthlyRetroData", "MlreShare", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PerLifeAggregationMonthlyRetroData", "MlreShare");
        }
    }
}
