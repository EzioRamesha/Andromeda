namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateClaimRegisterHistoryTable2 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.ClaimRegisterHistories", new[] { "RefferalCaseIndicator" });
            AddColumn("dbo.ClaimRegisterHistories", "IsReferralCase", c => c.Boolean(nullable: false));
            CreateIndex("dbo.ClaimRegisterHistories", "IsReferralCase");
            DropColumn("dbo.ClaimRegisterHistories", "RefferalCaseIndicator");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ClaimRegisterHistories", "RefferalCaseIndicator", c => c.Boolean(nullable: false));
            DropIndex("dbo.ClaimRegisterHistories", new[] { "IsReferralCase" });
            DropColumn("dbo.ClaimRegisterHistories", "IsReferralCase");
            CreateIndex("dbo.ClaimRegisterHistories", "RefferalCaseIndicator");
        }
    }
}
