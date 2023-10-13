namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatePerLifeAggregationDetailDataTable3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PerLifeAggregationDetailData", "ExceptionErrorType", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PerLifeAggregationDetailData", "ExceptionErrorType");
        }
    }
}
