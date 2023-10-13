namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateSoaDataRiDataSummaryTable5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SoaDataRiDataSummaries", "Frequency", c => c.String(maxLength: 3));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SoaDataRiDataSummaries", "Frequency");
        }
    }
}
