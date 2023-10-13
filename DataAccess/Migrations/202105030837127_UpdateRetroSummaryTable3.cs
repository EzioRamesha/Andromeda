namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRetroSummaryTable3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RetroSummaries", "ReportingType", c => c.Int(nullable: false));
            AddColumn("dbo.RetroSummaries", "Mfrs17AnnualCohort", c => c.Int());
            AddColumn("dbo.RetroSummaries", "Mfrs17ContractCode", c => c.String(maxLength: 25));
            CreateIndex("dbo.RetroSummaries", "ReportingType");
            CreateIndex("dbo.RetroSummaries", "Mfrs17AnnualCohort");
            CreateIndex("dbo.RetroSummaries", "Mfrs17ContractCode");
        }
        
        public override void Down()
        {
            DropIndex("dbo.RetroSummaries", new[] { "Mfrs17ContractCode" });
            DropIndex("dbo.RetroSummaries", new[] { "Mfrs17AnnualCohort" });
            DropIndex("dbo.RetroSummaries", new[] { "ReportingType" });
            DropColumn("dbo.RetroSummaries", "Mfrs17ContractCode");
            DropColumn("dbo.RetroSummaries", "Mfrs17AnnualCohort");
            DropColumn("dbo.RetroSummaries", "ReportingType");
        }
    }
}
