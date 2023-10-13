namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRiDataWarehouseHistoryTable1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RiDataWarehouseHistories", "WakalahFeePercentage", c => c.Double());
            AlterColumn("dbo.RiDataWarehouseHistories", "PolicyTerm", c => c.Double());
            AlterColumn("dbo.RiDataWarehouseHistories", "PolicyTermRemain", c => c.Double());
            CreateIndex("dbo.RiDataWarehouseHistories", "WakalahFeePercentage");
        }
        
        public override void Down()
        {
            DropIndex("dbo.RiDataWarehouseHistories", new[] { "WakalahFeePercentage" });
            AlterColumn("dbo.RiDataWarehouseHistories", "PolicyTermRemain", c => c.Int());
            AlterColumn("dbo.RiDataWarehouseHistories", "PolicyTerm", c => c.Int());
            DropColumn("dbo.RiDataWarehouseHistories", "WakalahFeePercentage");
        }
    }
}
