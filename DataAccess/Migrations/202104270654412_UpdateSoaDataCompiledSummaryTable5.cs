namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateSoaDataCompiledSummaryTable5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SoaDataCompiledSummaries", "ReportingType", c => c.Int(nullable: false));
            CreateIndex("dbo.SoaDataCompiledSummaries", "ReportingType");
        }
        
        public override void Down()
        {
            DropIndex("dbo.SoaDataCompiledSummaries", new[] { "ReportingType" });
            DropColumn("dbo.SoaDataCompiledSummaries", "ReportingType");
        }
    }
}
