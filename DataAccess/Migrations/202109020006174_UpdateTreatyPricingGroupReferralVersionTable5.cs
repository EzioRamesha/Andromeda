namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTreatyPricingGroupReferralVersionTable5 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TreatyPricingGroupReferralVersions", "GroupSize", c => c.Double());
            DropColumn("dbo.TreatyPricingGroupReferralVersions", "DeclinedRisk");
            DropColumn("dbo.TreatyPricingGroupReferralVersions", "ReferredRisk");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TreatyPricingGroupReferralVersions", "ReferredRisk", c => c.String(maxLength: 128));
            AddColumn("dbo.TreatyPricingGroupReferralVersions", "DeclinedRisk", c => c.String(maxLength: 128));
            AlterColumn("dbo.TreatyPricingGroupReferralVersions", "GroupSize", c => c.String());
        }
    }
}
