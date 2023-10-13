namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateMfrs17ReportingDetailTable1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Mfrs17ReportingDetails", "TreatyCodeId", "dbo.TreatyCodes");
            DropIndex("dbo.Mfrs17ReportingDetails", new[] { "TreatyCodeId" });
            AddColumn("dbo.Mfrs17ReportingDetails", "TreatyCode", c => c.String(maxLength: 30));
            AddColumn("dbo.Mfrs17ReportingDetails", "RiskQuarter", c => c.String(maxLength: 64));
            AlterColumn("dbo.Mfrs17ReportingDetails", "TreatyCodeId", c => c.Int());
            CreateIndex("dbo.Mfrs17ReportingDetails", "TreatyCode");
            CreateIndex("dbo.Mfrs17ReportingDetails", "RiskQuarter");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Mfrs17ReportingDetails", new[] { "RiskQuarter" });
            DropIndex("dbo.Mfrs17ReportingDetails", new[] { "TreatyCode" });
            AlterColumn("dbo.Mfrs17ReportingDetails", "TreatyCodeId", c => c.Int(nullable: false));
            DropColumn("dbo.Mfrs17ReportingDetails", "RiskQuarter");
            DropColumn("dbo.Mfrs17ReportingDetails", "TreatyCode");
            CreateIndex("dbo.Mfrs17ReportingDetails", "TreatyCodeId");
            AddForeignKey("dbo.Mfrs17ReportingDetails", "TreatyCodeId", "dbo.TreatyCodes", "Id");
        }
    }
}
