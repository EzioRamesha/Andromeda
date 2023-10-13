namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTreatyPricingClaimApprovalLimitVersionTable1 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.TreatyPricingClaimApprovalLimitVersions", new[] { "Amount" });
            AlterColumn("dbo.TreatyPricingClaimApprovalLimitVersions", "Amount", c => c.String(maxLength: 255));
            CreateIndex("dbo.TreatyPricingClaimApprovalLimitVersions", "Amount");
        }
        
        public override void Down()
        {
            DropIndex("dbo.TreatyPricingClaimApprovalLimitVersions", new[] { "Amount" });
            AlterColumn("dbo.TreatyPricingClaimApprovalLimitVersions", "Amount", c => c.String(maxLength: 50));
            CreateIndex("dbo.TreatyPricingClaimApprovalLimitVersions", "Amount");
        }
    }
}
