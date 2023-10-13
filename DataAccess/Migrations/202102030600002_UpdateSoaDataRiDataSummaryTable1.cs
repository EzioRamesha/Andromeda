namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateSoaDataRiDataSummaryTable1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SoaDataRiDataSummaries", "DatabaseCommission", c => c.Double());
            AddColumn("dbo.SoaDataRiDataSummaries", "BrokerageFee", c => c.Double());
            AddColumn("dbo.SoaDataRiDataSummaries", "ServiceFee", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SoaDataRiDataSummaries", "ServiceFee");
            DropColumn("dbo.SoaDataRiDataSummaries", "BrokerageFee");
            DropColumn("dbo.SoaDataRiDataSummaries", "DatabaseCommission");
        }
    }
}
