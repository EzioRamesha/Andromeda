namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRiDataBatchTable3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RiDataBatches", "TotalConflict", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RiDataBatches", "TotalConflict");
        }
    }
}
