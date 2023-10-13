namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRetroSummaryTable1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RetroSummaries", "RiskQuarter", c => c.String(maxLength: 10));
            CreateIndex("dbo.RetroSummaries", "RiskQuarter");
        }
        
        public override void Down()
        {
            DropIndex("dbo.RetroSummaries", new[] { "RiskQuarter" });
            DropColumn("dbo.RetroSummaries", "RiskQuarter");
        }
    }
}
