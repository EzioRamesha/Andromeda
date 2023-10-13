namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTreatyPricingGroupReferralHipsTables1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TreatyPricingGroupReferralHipsTables", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TreatyPricingGroupReferralHipsTables", "Description");
        }
    }
}
