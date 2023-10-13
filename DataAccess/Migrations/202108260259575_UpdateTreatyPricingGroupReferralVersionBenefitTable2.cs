namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTreatyPricingGroupReferralVersionBenefitTable2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TreatyPricingGroupReferralVersionBenefits", "TabarruLoading", c => c.String(maxLength: 128));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TreatyPricingGroupReferralVersionBenefits", "TabarruLoading");
        }
    }
}
