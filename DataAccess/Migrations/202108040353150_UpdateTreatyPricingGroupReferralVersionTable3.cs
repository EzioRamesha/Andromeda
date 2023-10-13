namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTreatyPricingGroupReferralVersionTable3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TreatyPricingGroupReferralVersions", "UnderwritingMethodPickListDetailId", "dbo.PickListDetails");
            DropIndex("dbo.TreatyPricingGroupReferralVersions", new[] { "UnderwritingMethodPickListDetailId" });
            AddColumn("dbo.TreatyPricingGroupReferralVersions", "UnderwritingMethod", c => c.String(storeType: "ntext"));
            DropColumn("dbo.TreatyPricingGroupReferralVersions", "UnderwritingMethodPickListDetailId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TreatyPricingGroupReferralVersions", "UnderwritingMethodPickListDetailId", c => c.Int());
            DropColumn("dbo.TreatyPricingGroupReferralVersions", "UnderwritingMethod");
            CreateIndex("dbo.TreatyPricingGroupReferralVersions", "UnderwritingMethodPickListDetailId");
            AddForeignKey("dbo.TreatyPricingGroupReferralVersions", "UnderwritingMethodPickListDetailId", "dbo.PickListDetails", "Id");
        }
    }
}
