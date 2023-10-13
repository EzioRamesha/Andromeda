namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRiDataWarehouseTable2 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.RiDataWarehouse", new[] { "EndingPolicyStatus" });
            AlterColumn("dbo.RiDataWarehouse", "EndingPolicyStatus", c => c.Int());
            CreateIndex("dbo.RiDataWarehouse", "EndingPolicyStatus");
        }
        
        public override void Down()
        {
            DropIndex("dbo.RiDataWarehouse", new[] { "EndingPolicyStatus" });
            AlterColumn("dbo.RiDataWarehouse", "EndingPolicyStatus", c => c.Int(nullable: false));
            CreateIndex("dbo.RiDataWarehouse", "EndingPolicyStatus");
        }
    }
}
