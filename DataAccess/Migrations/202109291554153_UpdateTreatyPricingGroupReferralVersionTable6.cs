namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTreatyPricingGroupReferralVersionTable6 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TreatyPricingGroupReferralVersions", "ChecklistPendingUnderwriting", c => c.Boolean(nullable: false));
            AddColumn("dbo.TreatyPricingGroupReferralVersions", "ChecklistPendingHealth", c => c.Boolean(nullable: false));
            AddColumn("dbo.TreatyPricingGroupReferralVersions", "ChecklistPendingClaims", c => c.Boolean(nullable: false));
            AddColumn("dbo.TreatyPricingGroupReferralVersions", "ChecklistPendingBD", c => c.Boolean(nullable: false));
            AddColumn("dbo.TreatyPricingGroupReferralVersions", "ChecklistPendingCR", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TreatyPricingGroupReferralVersions", "ChecklistPendingCR");
            DropColumn("dbo.TreatyPricingGroupReferralVersions", "ChecklistPendingBD");
            DropColumn("dbo.TreatyPricingGroupReferralVersions", "ChecklistPendingClaims");
            DropColumn("dbo.TreatyPricingGroupReferralVersions", "ChecklistPendingHealth");
            DropColumn("dbo.TreatyPricingGroupReferralVersions", "ChecklistPendingUnderwriting");
        }
    }
}
