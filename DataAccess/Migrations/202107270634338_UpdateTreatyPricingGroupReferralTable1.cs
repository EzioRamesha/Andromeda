namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTreatyPricingGroupReferralTable1 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.TreatyPricingGroupReferrals", "FirstRequestDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TreatyPricingGroupReferrals", "FirstRequestDate", c => c.DateTime(precision: 7, storeType: "datetime2"));
        }
    }
}
