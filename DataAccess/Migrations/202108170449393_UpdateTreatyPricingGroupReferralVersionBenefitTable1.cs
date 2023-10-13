namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTreatyPricingGroupReferralVersionBenefitTable1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TreatyPricingGroupReferralVersionBenefits", "GroupProfitCommissionId", "dbo.TreatyPricingProfitCommissions");
            DropIndex("dbo.TreatyPricingGroupReferralVersionBenefits", new[] { "GroupProfitCommissionId" });
            AddColumn("dbo.TreatyPricingGroupReferralVersionBenefits", "GroupProfitCommission", c => c.String(storeType: "ntext"));
            AlterColumn("dbo.TreatyPricingGroupReferralVersionBenefits", "GroupFreeCoverLimitNonCI", c => c.String(storeType: "ntext"));
            AlterColumn("dbo.TreatyPricingGroupReferralVersionBenefits", "GroupFreeCoverLimitCI", c => c.String(storeType: "ntext"));
            CreateIndex("dbo.TreatyPricingGroupReferralVersionBenefits", "GroupFreeCoverLimitAgeNonCI");
            CreateIndex("dbo.TreatyPricingGroupReferralVersionBenefits", "GroupFreeCoverLimitAgeCI");
            DropColumn("dbo.TreatyPricingGroupReferralVersionBenefits", "GroupProfitCommissionId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TreatyPricingGroupReferralVersionBenefits", "GroupProfitCommissionId", c => c.Int());
            DropIndex("dbo.TreatyPricingGroupReferralVersionBenefits", new[] { "GroupFreeCoverLimitAgeCI" });
            DropIndex("dbo.TreatyPricingGroupReferralVersionBenefits", new[] { "GroupFreeCoverLimitAgeNonCI" });
            AlterColumn("dbo.TreatyPricingGroupReferralVersionBenefits", "GroupFreeCoverLimitCI", c => c.String(maxLength: 128));
            AlterColumn("dbo.TreatyPricingGroupReferralVersionBenefits", "GroupFreeCoverLimitNonCI", c => c.String(maxLength: 128));
            DropColumn("dbo.TreatyPricingGroupReferralVersionBenefits", "GroupProfitCommission");
            CreateIndex("dbo.TreatyPricingGroupReferralVersionBenefits", "GroupProfitCommissionId");
            AddForeignKey("dbo.TreatyPricingGroupReferralVersionBenefits", "GroupProfitCommissionId", "dbo.TreatyPricingProfitCommissions", "Id");
        }
    }
}
