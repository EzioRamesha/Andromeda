namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRiDataWarehouseTable5 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.RiDataWarehouse", "LoaCode", c => c.String(maxLength: 20));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.RiDataWarehouse", "LoaCode", c => c.String(maxLength: 10));
        }
    }
}
