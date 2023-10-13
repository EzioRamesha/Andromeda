namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTreatyPricingGroupReferralVersionTable7 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TreatyPricingGroupReferralVersions", "QuotationTAT", c => c.Int());
            AddColumn("dbo.TreatyPricingGroupReferralVersions", "InternalTAT", c => c.Int());
            AddColumn("dbo.TreatyPricingGroupReferralVersions", "QuotationValidityDate", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AddColumn("dbo.TreatyPricingGroupReferralVersions", "QuotationValidityDay", c => c.String());
            AddColumn("dbo.TreatyPricingGroupReferralVersions", "FirstQuotationSentWeek", c => c.Int());
            AddColumn("dbo.TreatyPricingGroupReferralVersions", "FirstQuotationSentMonth", c => c.Int());
            AddColumn("dbo.TreatyPricingGroupReferralVersions", "FirstQuotationSentQuarter", c => c.String(maxLength: 10));
            AddColumn("dbo.TreatyPricingGroupReferralVersions", "FirstQuotationSentYear", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TreatyPricingGroupReferralVersions", "FirstQuotationSentYear");
            DropColumn("dbo.TreatyPricingGroupReferralVersions", "FirstQuotationSentQuarter");
            DropColumn("dbo.TreatyPricingGroupReferralVersions", "FirstQuotationSentMonth");
            DropColumn("dbo.TreatyPricingGroupReferralVersions", "FirstQuotationSentWeek");
            DropColumn("dbo.TreatyPricingGroupReferralVersions", "QuotationValidityDay");
            DropColumn("dbo.TreatyPricingGroupReferralVersions", "QuotationValidityDate");
            DropColumn("dbo.TreatyPricingGroupReferralVersions", "InternalTAT");
            DropColumn("dbo.TreatyPricingGroupReferralVersions", "QuotationTAT");
        }
    }
}
