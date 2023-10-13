namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateMfrs17ReportingDetailTable3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Mfrs17ReportingDetails", "GenerateStatus", c => c.Int());
            CreateIndex("dbo.Mfrs17ReportingDetails", "GenerateStatus");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Mfrs17ReportingDetails", new[] { "GenerateStatus" });
            DropColumn("dbo.Mfrs17ReportingDetails", "GenerateStatus");
        }
    }
}
