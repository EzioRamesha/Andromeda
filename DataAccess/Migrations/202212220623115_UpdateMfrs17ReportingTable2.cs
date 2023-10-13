namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateMfrs17ReportingTable2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Mfrs17Reportings", "IsResume", c => c.Boolean());
            CreateIndex("dbo.Mfrs17Reportings", "IsResume");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Mfrs17Reportings", new[] { "IsResume" });
            DropColumn("dbo.Mfrs17Reportings", "IsResume");
        }
    }
}
