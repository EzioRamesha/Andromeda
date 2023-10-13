namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTreatyPricingGroupReferralVersionTable1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TreatyPricingGroupReferralVersions", "GroupReferralPersonInChargeId", "dbo.PickListDetails");
            AddForeignKey("dbo.TreatyPricingGroupReferralVersions", "GroupReferralPersonInChargeId", "dbo.Users", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TreatyPricingGroupReferralVersions", "GroupReferralPersonInChargeId", "dbo.Users");
            AddForeignKey("dbo.TreatyPricingGroupReferralVersions", "GroupReferralPersonInChargeId", "dbo.PickListDetails", "Id");
        }
    }
}
