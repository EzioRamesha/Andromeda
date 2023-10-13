namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRetroRegisterHistoryTable2 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.RetroRegisterHistories", new[] { "RetroRegisterBatchId" });
            AddColumn("dbo.RetroRegisterHistories", "Type", c => c.Int(nullable: false));
            AlterColumn("dbo.RetroRegisterHistories", "RetroRegisterBatchId", c => c.Int());
            CreateIndex("dbo.RetroRegisterHistories", "RetroRegisterBatchId");
            CreateIndex("dbo.RetroRegisterHistories", "Type");
        }
        
        public override void Down()
        {
            DropIndex("dbo.RetroRegisterHistories", new[] { "Type" });
            DropIndex("dbo.RetroRegisterHistories", new[] { "RetroRegisterBatchId" });
            AlterColumn("dbo.RetroRegisterHistories", "RetroRegisterBatchId", c => c.Int(nullable: false));
            DropColumn("dbo.RetroRegisterHistories", "Type");
            CreateIndex("dbo.RetroRegisterHistories", "RetroRegisterBatchId");
        }
    }
}
