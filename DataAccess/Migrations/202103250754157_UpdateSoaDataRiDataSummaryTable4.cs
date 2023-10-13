namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateSoaDataRiDataSummaryTable4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SoaDataRiDataSummaries", "TPD", c => c.Double());
            AddColumn("dbo.SoaDataRiDataSummaries", "CI", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SoaDataRiDataSummaries", "CI");
            DropColumn("dbo.SoaDataRiDataSummaries", "TPD");
        }
    }
}
