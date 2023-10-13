namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRiDataTable19 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.RiData", new[] { "RiDataBatchId" });
            AddColumn("dbo.RiData", "IsConflict", c => c.Boolean(nullable: false));
            AlterColumn("dbo.RiData", "RiDataBatchId", c => c.Int());
            CreateIndex("dbo.RiData", "RiDataBatchId");
            CreateIndex("dbo.RiData", "IsConflict");
        }
        
        public override void Down()
        {
            DropIndex("dbo.RiData", new[] { "IsConflict" });
            DropIndex("dbo.RiData", new[] { "RiDataBatchId" });
            AlterColumn("dbo.RiData", "RiDataBatchId", c => c.Int(nullable: false));
            DropColumn("dbo.RiData", "IsConflict");
            CreateIndex("dbo.RiData", "RiDataBatchId");
        }
    }
}
