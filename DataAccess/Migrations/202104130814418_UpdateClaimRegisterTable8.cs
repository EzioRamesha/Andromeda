namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateClaimRegisterTable8 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ClaimRegister", "ReferralClaimId", c => c.Int());
            CreateIndex("dbo.ClaimRegister", "ReferralClaimId");
            AddForeignKey("dbo.ClaimRegister", "ReferralClaimId", "dbo.ReferralClaims", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ClaimRegister", "ReferralClaimId", "dbo.ReferralClaims");
            DropIndex("dbo.ClaimRegister", new[] { "ReferralClaimId" });
            DropColumn("dbo.ClaimRegister", "ReferralClaimId");
        }
    }
}
