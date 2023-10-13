namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateClaimRegisterHistoryTable9 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.ClaimRegisterHistories", new[] { "TempS1" });
            AddColumn("dbo.ClaimRegisterHistories", "ReferralRiDataId", c => c.Int());
            AddColumn("dbo.ClaimRegisterHistories", "ReferralClaimId", c => c.Int());
            AlterColumn("dbo.ClaimRegisterHistories", "TempS1", c => c.String(maxLength: 150));
            CreateIndex("dbo.ClaimRegisterHistories", "ReferralRiDataId");
            CreateIndex("dbo.ClaimRegisterHistories", "ReferralClaimId");
            CreateIndex("dbo.ClaimRegisterHistories", "TempS1");
            AddForeignKey("dbo.ClaimRegisterHistories", "ReferralClaimId", "dbo.ReferralClaims", "Id");
            AddForeignKey("dbo.ClaimRegisterHistories", "ReferralRiDataId", "dbo.ReferralRiData", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ClaimRegisterHistories", "ReferralRiDataId", "dbo.ReferralRiData");
            DropForeignKey("dbo.ClaimRegisterHistories", "ReferralClaimId", "dbo.ReferralClaims");
            DropIndex("dbo.ClaimRegisterHistories", new[] { "TempS1" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "ReferralClaimId" });
            DropIndex("dbo.ClaimRegisterHistories", new[] { "ReferralRiDataId" });
            AlterColumn("dbo.ClaimRegisterHistories", "TempS1", c => c.String(maxLength: 50));
            DropColumn("dbo.ClaimRegisterHistories", "ReferralClaimId");
            DropColumn("dbo.ClaimRegisterHistories", "ReferralRiDataId");
            CreateIndex("dbo.ClaimRegisterHistories", "TempS1");
        }
    }
}
