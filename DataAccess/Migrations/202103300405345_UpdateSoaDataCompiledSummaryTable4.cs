namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateSoaDataCompiledSummaryTable4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SoaDataCompiledSummaries", "Frequency", c => c.String(maxLength: 3));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SoaDataCompiledSummaries", "Frequency");
        }
    }
}
