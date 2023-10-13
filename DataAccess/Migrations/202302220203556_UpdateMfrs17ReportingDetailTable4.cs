namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateMfrs17ReportingDetailTable4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Mfrs17ReportingDetails", "CedingPlanCode", c => c.String(maxLength: 30));
            CreateIndex("dbo.Mfrs17ReportingDetails", "CedingPlanCode");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Mfrs17ReportingDetails", new[] { "CedingPlanCode" });
            DropColumn("dbo.Mfrs17ReportingDetails", "CedingPlanCode");
        }
    }
}
