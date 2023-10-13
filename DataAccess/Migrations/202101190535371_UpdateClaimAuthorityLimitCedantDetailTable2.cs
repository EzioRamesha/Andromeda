namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateClaimAuthorityLimitCedantDetailTable2 : DbMigration
    {
        public override void Up()
        {
            RenameColumn("dbo.ClaimAuthorityLimitCedantDetails", "FundAccountingCode", "FundsAccountingTypePickListDetailId");
            CreateIndex("dbo.ClaimAuthorityLimitCedantDetails", "FundsAccountingTypePickListDetailId");
            AddForeignKey("dbo.ClaimAuthorityLimitCedantDetails", "FundsAccountingTypePickListDetailId", "dbo.PickListDetails", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ClaimAuthorityLimitCedantDetails", "FundsAccountingTypePickListDetailId", "dbo.PickListDetails");
            DropIndex("dbo.ClaimAuthorityLimitCedantDetails", new[] { "FundsAccountingTypePickListDetailId" });
            RenameColumn("dbo.ClaimAuthorityLimitCedantDetails", "FundsAccountingTypePickListDetailId", "FundAccountingCode");
        }
    }
}
