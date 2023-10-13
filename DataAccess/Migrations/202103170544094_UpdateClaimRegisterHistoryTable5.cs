namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateClaimRegisterHistoryTable5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ClaimRegisterHistories", "SoaDataBatchId", c => c.Int());
            CreateIndex("dbo.ClaimRegisterHistories", "SoaDataBatchId");
            AddForeignKey("dbo.ClaimRegisterHistories", "SoaDataBatchId", "dbo.SoaDataBatches", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ClaimRegisterHistories", "SoaDataBatchId", "dbo.SoaDataBatches");
            DropIndex("dbo.ClaimRegisterHistories", new[] { "SoaDataBatchId" });
            DropColumn("dbo.ClaimRegisterHistories", "SoaDataBatchId");
        }
    }
}
