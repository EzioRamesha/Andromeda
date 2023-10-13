namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateSoaDataCompiledSummaryHistoryTable1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SoaDataCompiledSummaryHistories", "Frequency", c => c.String(maxLength: 3));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SoaDataCompiledSummaryHistories", "Frequency");
        }
    }
}
