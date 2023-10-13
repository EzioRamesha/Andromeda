namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRiDataWarehouseTable1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RiDataWarehouse", "WakalahFeePercentage", c => c.Double());
            AlterColumn("dbo.RiDataWarehouse", "PolicyTerm", c => c.Double());
            AlterColumn("dbo.RiDataWarehouse", "PolicyTermRemain", c => c.Double());
            CreateIndex("dbo.RiDataWarehouse", "WakalahFeePercentage");
        }
        
        public override void Down()
        {
            DropIndex("dbo.RiDataWarehouse", new[] { "WakalahFeePercentage" });
            AlterColumn("dbo.RiDataWarehouse", "PolicyTermRemain", c => c.Int());
            AlterColumn("dbo.RiDataWarehouse", "PolicyTerm", c => c.Int());
            DropColumn("dbo.RiDataWarehouse", "WakalahFeePercentage");
        }
    }
}
