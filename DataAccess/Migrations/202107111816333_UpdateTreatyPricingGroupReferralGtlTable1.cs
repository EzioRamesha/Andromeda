namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTreatyPricingGroupReferralGtlTable1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TreatyPricingGroupReferralGtlTables", "GtlBenefitCategoryId", "dbo.GtlBenefitCategories");
            DropIndex("dbo.TreatyPricingGroupReferralGtlTables", new[] { "GtlBenefitCategoryId" });
            AddColumn("dbo.TreatyPricingGroupReferralGtlTables", "BenefitId", c => c.Int());
            CreateIndex("dbo.TreatyPricingGroupReferralGtlTables", "BenefitId");
            AddForeignKey("dbo.TreatyPricingGroupReferralGtlTables", "BenefitId", "dbo.Benefits", "Id");
            DropColumn("dbo.TreatyPricingGroupReferralGtlTables", "GtlBenefitCategoryId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TreatyPricingGroupReferralGtlTables", "GtlBenefitCategoryId", c => c.Int());
            DropForeignKey("dbo.TreatyPricingGroupReferralGtlTables", "BenefitId", "dbo.Benefits");
            DropIndex("dbo.TreatyPricingGroupReferralGtlTables", new[] { "BenefitId" });
            DropColumn("dbo.TreatyPricingGroupReferralGtlTables", "BenefitId");
            CreateIndex("dbo.TreatyPricingGroupReferralGtlTables", "GtlBenefitCategoryId");
            AddForeignKey("dbo.TreatyPricingGroupReferralGtlTables", "GtlBenefitCategoryId", "dbo.GtlBenefitCategories", "Id");
        }
    }
}
