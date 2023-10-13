namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRetroRegisterHistoryTable1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.RetroRegisterHistories", "SoaDataBatchId", "dbo.SoaDataBatches");
            DropIndex("dbo.RetroRegisterHistories", new[] { "SoaDataBatchId" });
            AddColumn("dbo.RetroRegisterHistories", "DirectRetroId", c => c.Int());
            CreateIndex("dbo.RetroRegisterHistories", "DirectRetroId");
            AddForeignKey("dbo.RetroRegisterHistories", "DirectRetroId", "dbo.DirectRetro", "Id");
            DropColumn("dbo.RetroRegisterHistories", "SoaDataBatchId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.RetroRegisterHistories", "SoaDataBatchId", c => c.Int());
            DropForeignKey("dbo.RetroRegisterHistories", "DirectRetroId", "dbo.DirectRetro");
            DropIndex("dbo.RetroRegisterHistories", new[] { "DirectRetroId" });
            DropColumn("dbo.RetroRegisterHistories", "DirectRetroId");
            CreateIndex("dbo.RetroRegisterHistories", "SoaDataBatchId");
            AddForeignKey("dbo.RetroRegisterHistories", "SoaDataBatchId", "dbo.SoaDataBatches", "Id");
        }
    }
}
