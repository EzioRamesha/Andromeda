namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateMfrs17ReportingTable1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Mfrs17Reportings", "GenerateType", c => c.Int());
            AddColumn("dbo.Mfrs17Reportings", "GeneratePercentage", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Mfrs17Reportings", "GeneratePercentage");
            DropColumn("dbo.Mfrs17Reportings", "GenerateType");
        }
    }
}
