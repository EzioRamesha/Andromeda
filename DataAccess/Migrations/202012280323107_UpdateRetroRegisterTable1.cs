namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRetroRegisterTable1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.RetroRegister", "SoaDataBatchId", "dbo.SoaDataBatches");
            DropIndex("dbo.RetroRegister", new[] { "SoaDataBatchId" });
            AddColumn("dbo.RetroRegister", "DirectRetroId", c => c.Int());
            CreateIndex("dbo.RetroRegister", "DirectRetroId");
            AddForeignKey("dbo.RetroRegister", "DirectRetroId", "dbo.DirectRetro", "Id");
            DropColumn("dbo.RetroRegister", "SoaDataBatchId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.RetroRegister", "SoaDataBatchId", c => c.Int());
            DropForeignKey("dbo.RetroRegister", "DirectRetroId", "dbo.DirectRetro");
            DropIndex("dbo.RetroRegister", new[] { "DirectRetroId" });
            DropColumn("dbo.RetroRegister", "DirectRetroId");
            CreateIndex("dbo.RetroRegister", "SoaDataBatchId");
            AddForeignKey("dbo.RetroRegister", "SoaDataBatchId", "dbo.SoaDataBatches", "Id");
        }
    }
}
