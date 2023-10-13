namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTreatyPricingGroupReferralVersionTable4 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TreatyPricingGroupReferralVersions", "GrossRiskPremium", c => c.Double());
            AlterColumn("dbo.TreatyPricingGroupReferralVersions", "ReinsurancePremium", c => c.Double());
            AlterColumn("dbo.TreatyPricingGroupReferralVersions", "GrossRiskPremiumGTL", c => c.Double());
            AlterColumn("dbo.TreatyPricingGroupReferralVersions", "ReinsurancePremiumGTL", c => c.Double());
            AlterColumn("dbo.TreatyPricingGroupReferralVersions", "GrossRiskPremiumGHS", c => c.Double());
            AlterColumn("dbo.TreatyPricingGroupReferralVersions", "ReinsurancePremiumGHS", c => c.Double());
            AlterColumn("dbo.TreatyPricingGroupReferralVersions", "AverageSumAssured", c => c.Double());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TreatyPricingGroupReferralVersions", "AverageSumAssured", c => c.String());
            AlterColumn("dbo.TreatyPricingGroupReferralVersions", "ReinsurancePremiumGHS", c => c.String());
            AlterColumn("dbo.TreatyPricingGroupReferralVersions", "GrossRiskPremiumGHS", c => c.String());
            AlterColumn("dbo.TreatyPricingGroupReferralVersions", "ReinsurancePremiumGTL", c => c.String());
            AlterColumn("dbo.TreatyPricingGroupReferralVersions", "GrossRiskPremiumGTL", c => c.String());
            AlterColumn("dbo.TreatyPricingGroupReferralVersions", "ReinsurancePremium", c => c.String());
            AlterColumn("dbo.TreatyPricingGroupReferralVersions", "GrossRiskPremium", c => c.String());
        }
    }
}
