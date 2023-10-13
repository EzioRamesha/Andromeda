namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateSoaDataRiDataSummaryTable3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SoaDataRiDataSummaries", "ContractCode", c => c.String(maxLength: 35));
            AddColumn("dbo.SoaDataRiDataSummaries", "AnnualCohort", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SoaDataRiDataSummaries", "AnnualCohort");
            DropColumn("dbo.SoaDataRiDataSummaries", "ContractCode");
        }
    }
}
