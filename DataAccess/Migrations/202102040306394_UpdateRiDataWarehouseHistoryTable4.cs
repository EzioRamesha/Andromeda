namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRiDataWarehouseHistoryTable4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RiDataWarehouseHistories", "TreatyNumber", c => c.String(maxLength: 128));
            CreateIndex("dbo.RiDataWarehouseHistories", "TreatyNumber");
        }
        
        public override void Down()
        {
            DropIndex("dbo.RiDataWarehouseHistories", new[] { "TreatyNumber" });
            DropColumn("dbo.RiDataWarehouseHistories", "TreatyNumber");
        }
    }
}
