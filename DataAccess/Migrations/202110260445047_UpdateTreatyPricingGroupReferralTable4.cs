namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTreatyPricingGroupReferralTable4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TreatyPricingGroupReferrals", "AverageScore", c => c.Double());
            DropColumn("dbo.TreatyPricingGroupReferrals", "QuotationTAT");
            DropColumn("dbo.TreatyPricingGroupReferrals", "InternalTAT");
            DropColumn("dbo.TreatyPricingGroupReferrals", "QuotationValidityDate");
            DropColumn("dbo.TreatyPricingGroupReferrals", "QuotationValidityDay");
            DropColumn("dbo.TreatyPricingGroupReferrals", "FirstQuotationSentWeek");
            DropColumn("dbo.TreatyPricingGroupReferrals", "FirstQuotationSentMonth");
            DropColumn("dbo.TreatyPricingGroupReferrals", "FirstQuotationSentQuarter");
            DropColumn("dbo.TreatyPricingGroupReferrals", "FirstQuotationSentYear");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TreatyPricingGroupReferrals", "FirstQuotationSentYear", c => c.Int());
            AddColumn("dbo.TreatyPricingGroupReferrals", "FirstQuotationSentQuarter", c => c.String(maxLength: 10));
            AddColumn("dbo.TreatyPricingGroupReferrals", "FirstQuotationSentMonth", c => c.Int());
            AddColumn("dbo.TreatyPricingGroupReferrals", "FirstQuotationSentWeek", c => c.Int());
            AddColumn("dbo.TreatyPricingGroupReferrals", "QuotationValidityDay", c => c.String());
            AddColumn("dbo.TreatyPricingGroupReferrals", "QuotationValidityDate", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AddColumn("dbo.TreatyPricingGroupReferrals", "InternalTAT", c => c.Int());
            AddColumn("dbo.TreatyPricingGroupReferrals", "QuotationTAT", c => c.Int());
            DropColumn("dbo.TreatyPricingGroupReferrals", "AverageScore");
        }
    }
}
