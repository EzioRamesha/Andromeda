namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateSoaDataCompiledSummaryHistoryTable2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SoaDataCompiledSummaryHistories", "ReportingType", c => c.Int(nullable: false));
            CreateIndex("dbo.SoaDataCompiledSummaryHistories", "ReportingType");
        }
        
        public override void Down()
        {
            DropIndex("dbo.SoaDataCompiledSummaryHistories", new[] { "ReportingType" });
            DropColumn("dbo.SoaDataCompiledSummaryHistories", "ReportingType");
        }
    }
}
