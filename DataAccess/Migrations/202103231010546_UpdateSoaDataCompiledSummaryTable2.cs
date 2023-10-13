namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateSoaDataCompiledSummaryTable2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SoaDataCompiledSummaries", "CurrencyCode", c => c.String(maxLength: 3));
            AddColumn("dbo.SoaDataCompiledSummaries", "CurrencyRate", c => c.Double());
            AddColumn("dbo.SoaDataCompiledSummaries", "ContractCode", c => c.String(maxLength: 35));
            AddColumn("dbo.SoaDataCompiledSummaries", "AnnualCohort", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SoaDataCompiledSummaries", "AnnualCohort");
            DropColumn("dbo.SoaDataCompiledSummaries", "ContractCode");
            DropColumn("dbo.SoaDataCompiledSummaries", "CurrencyRate");
            DropColumn("dbo.SoaDataCompiledSummaries", "CurrencyCode");
        }
    }
}
