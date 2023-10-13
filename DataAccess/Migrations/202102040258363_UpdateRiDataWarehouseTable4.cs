namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRiDataWarehouseTable4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RiDataWarehouse", "TreatyNumber", c => c.String(maxLength: 128));
            CreateIndex("dbo.RiDataWarehouse", "TreatyNumber");
        }
        
        public override void Down()
        {
            DropIndex("dbo.RiDataWarehouse", new[] { "TreatyNumber" });
            DropColumn("dbo.RiDataWarehouse", "TreatyNumber");
        }
    }
}
