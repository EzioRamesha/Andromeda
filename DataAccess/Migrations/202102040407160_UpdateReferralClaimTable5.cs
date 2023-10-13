namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateReferralClaimTable5 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ReferralClaims", "RetroReviewedById", "dbo.Users");
            DropIndex("dbo.ReferralClaims", new[] { "RetroReviewedById" });
            AddColumn("dbo.ReferralClaims", "RetroReviewedBy", c => c.String(maxLength: 255));
            DropColumn("dbo.ReferralClaims", "RetroReviewedById");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ReferralClaims", "RetroReviewedById", c => c.Int());
            DropColumn("dbo.ReferralClaims", "RetroReviewedBy");
            CreateIndex("dbo.ReferralClaims", "RetroReviewedById");
            AddForeignKey("dbo.ReferralClaims", "RetroReviewedById", "dbo.Users", "Id");
        }
    }
}
