namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRiDataBatchTable2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RiDataBatches", "TotalProcessWarehouseFailedStatus", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RiDataBatches", "TotalProcessWarehouseFailedStatus");
        }
    }
}
