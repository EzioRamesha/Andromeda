namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateClaimRegisterTable4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ClaimRegister", "SoaDataBatchId", c => c.Int());
            CreateIndex("dbo.ClaimRegister", "SoaDataBatchId");
            AddForeignKey("dbo.ClaimRegister", "SoaDataBatchId", "dbo.SoaDataBatches", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ClaimRegister", "SoaDataBatchId", "dbo.SoaDataBatches");
            DropIndex("dbo.ClaimRegister", new[] { "SoaDataBatchId" });
            DropColumn("dbo.ClaimRegister", "SoaDataBatchId");
        }
    }
}
