namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTreatyPricingGroupReferralGtlTables1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TreatyPricingGroupReferralGtlTables", "BenefitId", "dbo.Benefits");
            DropForeignKey("dbo.TreatyPricingGroupReferralGtlTables", "DesignationId", "dbo.Designations");
            DropIndex("dbo.TreatyPricingGroupReferralGtlTables", new[] { "BenefitId" });
            DropIndex("dbo.TreatyPricingGroupReferralGtlTables", new[] { "DesignationId" });
            AddColumn("dbo.TreatyPricingGroupReferralGtlTables", "BenefitCode", c => c.String(maxLength: 128));
            AddColumn("dbo.TreatyPricingGroupReferralGtlTables", "Designation", c => c.String(maxLength: 128));
            DropColumn("dbo.TreatyPricingGroupReferralGtlTables", "BenefitId");
            DropColumn("dbo.TreatyPricingGroupReferralGtlTables", "DesignationId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TreatyPricingGroupReferralGtlTables", "DesignationId", c => c.Int());
            AddColumn("dbo.TreatyPricingGroupReferralGtlTables", "BenefitId", c => c.Int());
            DropColumn("dbo.TreatyPricingGroupReferralGtlTables", "Designation");
            DropColumn("dbo.TreatyPricingGroupReferralGtlTables", "BenefitCode");
            CreateIndex("dbo.TreatyPricingGroupReferralGtlTables", "DesignationId");
            CreateIndex("dbo.TreatyPricingGroupReferralGtlTables", "BenefitId");
            AddForeignKey("dbo.TreatyPricingGroupReferralGtlTables", "DesignationId", "dbo.Designations", "Id");
            AddForeignKey("dbo.TreatyPricingGroupReferralGtlTables", "BenefitId", "dbo.Benefits", "Id");
        }
    }
}
