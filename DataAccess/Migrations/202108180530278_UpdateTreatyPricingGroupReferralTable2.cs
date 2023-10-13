namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTreatyPricingGroupReferralTable2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.TreatyPricingGroupReferrals", "CoverageType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TreatyPricingGroupReferrals", "CoverageType", c => c.Int());
        }
    }
}
