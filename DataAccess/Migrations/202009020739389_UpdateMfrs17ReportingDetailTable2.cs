namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateMfrs17ReportingDetailTable2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Mfrs17ReportingDetails", "Mfrs17TreatyCode", c => c.String(maxLength: 25));
            CreateIndex("dbo.Mfrs17ReportingDetails", "Mfrs17TreatyCode");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Mfrs17ReportingDetails", new[] { "Mfrs17TreatyCode" });
            DropColumn("dbo.Mfrs17ReportingDetails", "Mfrs17TreatyCode");
        }
    }
}
