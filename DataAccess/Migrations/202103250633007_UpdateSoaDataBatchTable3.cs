namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateSoaDataBatchTable3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SoaDataBatches", "IsProfitCommissionData", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SoaDataBatches", "IsProfitCommissionData");
        }
    }
}
