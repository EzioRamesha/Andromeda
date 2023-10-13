namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateReferralClaimTable2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ReferralClaims", "DocRespondedAt", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AddColumn("dbo.ReferralClaims", "DocDelayReasonId", c => c.Int());
            CreateIndex("dbo.ReferralClaims", "DocRespondedAt");
            CreateIndex("dbo.ReferralClaims", "DocDelayReasonId");
            AddForeignKey("dbo.ReferralClaims", "DocDelayReasonId", "dbo.ClaimReasons", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ReferralClaims", "DocDelayReasonId", "dbo.ClaimReasons");
            DropIndex("dbo.ReferralClaims", new[] { "DocDelayReasonId" });
            DropIndex("dbo.ReferralClaims", new[] { "DocRespondedAt" });
            DropColumn("dbo.ReferralClaims", "DocDelayReasonId");
            DropColumn("dbo.ReferralClaims", "DocRespondedAt");
        }
    }
}
