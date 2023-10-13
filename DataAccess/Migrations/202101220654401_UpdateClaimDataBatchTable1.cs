namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateClaimDataBatchTable1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ClaimDataBatches", "ClaimTransactionTypePickListDetailId", c => c.Int());
            CreateIndex("dbo.ClaimDataBatches", "ClaimTransactionTypePickListDetailId");
            AddForeignKey("dbo.ClaimDataBatches", "ClaimTransactionTypePickListDetailId", "dbo.PickListDetails", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ClaimDataBatches", "ClaimTransactionTypePickListDetailId", "dbo.PickListDetails");
            DropIndex("dbo.ClaimDataBatches", new[] { "ClaimTransactionTypePickListDetailId" });
            DropColumn("dbo.ClaimDataBatches", "ClaimTransactionTypePickListDetailId");
        }
    }
}
