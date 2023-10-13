namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateSoaDataCompiledSummaryTable1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SoaDataCompiledSummaries", "Gst", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SoaDataCompiledSummaries", "Gst");
        }
    }
}
