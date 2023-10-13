namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateSoaDataCompiledSummaryTable3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SoaDataCompiledSummaries", "TPD", c => c.Double());
            AddColumn("dbo.SoaDataCompiledSummaries", "CI", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SoaDataCompiledSummaries", "CI");
            DropColumn("dbo.SoaDataCompiledSummaries", "TPD");
        }
    }
}
