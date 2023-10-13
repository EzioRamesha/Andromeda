namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateSoaDataRiDataSummaryTable2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SoaDataRiDataSummaries", "Type", c => c.Int(nullable: false));
            CreateIndex("dbo.SoaDataRiDataSummaries", "Type");
        }
        
        public override void Down()
        {
            DropIndex("dbo.SoaDataRiDataSummaries", new[] { "Type" });
            DropColumn("dbo.SoaDataRiDataSummaries", "Type");
        }
    }
}
