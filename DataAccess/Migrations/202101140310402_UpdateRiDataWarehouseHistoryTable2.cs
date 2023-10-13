namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRiDataWarehouseHistoryTable2 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.RiDataWarehouseHistories", new[] { "EndingPolicyStatus" });
            AlterColumn("dbo.RiDataWarehouseHistories", "EndingPolicyStatus", c => c.Int());
            CreateIndex("dbo.RiDataWarehouseHistories", "EndingPolicyStatus");
        }
        
        public override void Down()
        {
            DropIndex("dbo.RiDataWarehouseHistories", new[] { "EndingPolicyStatus" });
            AlterColumn("dbo.RiDataWarehouseHistories", "EndingPolicyStatus", c => c.Int(nullable: false));
            CreateIndex("dbo.RiDataWarehouseHistories", "EndingPolicyStatus");
        }
    }
}
