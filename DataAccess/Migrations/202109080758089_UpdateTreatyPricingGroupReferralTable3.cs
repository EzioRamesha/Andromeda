namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTreatyPricingGroupReferralTable3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TreatyPricingGroupReferrals", "PolicyNumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TreatyPricingGroupReferrals", "PolicyNumber");
        }
    }
}
