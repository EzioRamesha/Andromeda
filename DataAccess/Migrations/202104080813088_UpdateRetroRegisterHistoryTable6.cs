namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRetroRegisterHistoryTable6 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.RetroRegisterHistories", "DirectRetroId", "dbo.DirectRetro");
            DropForeignKey("dbo.RetroRegisterHistories", "RetroRegisterId", "dbo.RetroRegister");
            DropForeignKey("dbo.RetroRegisterHistories", "RetroRegisterBatchId", "dbo.RetroRegisterBatches");
            DropIndex("dbo.RetroRegisterHistories", new[] { "RetroRegisterBatchId" });
            DropIndex("dbo.RetroRegisterHistories", new[] { "DirectRetroId" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.RetroRegisterHistories", "DirectRetroId");
            CreateIndex("dbo.RetroRegisterHistories", "RetroRegisterBatchId");
            AddForeignKey("dbo.RetroRegisterHistories", "RetroRegisterBatchId", "dbo.RetroRegisterBatches", "Id");
            AddForeignKey("dbo.RetroRegisterHistories", "RetroRegisterId", "dbo.RetroRegister", "Id");
            AddForeignKey("dbo.RetroRegisterHistories", "DirectRetroId", "dbo.DirectRetro", "Id");
        }
    }
}
