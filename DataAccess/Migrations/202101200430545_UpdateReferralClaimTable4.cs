namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateReferralClaimTable4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ReferralClaims", "ClaimsDecision", c => c.Int());
            AddColumn("dbo.ReferralClaims", "ClaimsDecisionDate", c => c.DateTime(precision: 7, storeType: "datetime2"));
            DropColumn("dbo.ReferralClaims", "DecisionStatus");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ReferralClaims", "DecisionStatus", c => c.Int());
            DropColumn("dbo.ReferralClaims", "ClaimsDecisionDate");
            DropColumn("dbo.ReferralClaims", "ClaimsDecision");
        }
    }
}
