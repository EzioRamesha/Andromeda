namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatePerLifeAggregationDetailDataTable2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PerLifeAggregationDetailData", "Remarks", c => c.String(storeType: "ntext"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PerLifeAggregationDetailData", "Remarks");
        }
    }
}
