namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTreatyPricingGroupReferralVersionTable2 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.TreatyPricingGroupReferralVersions", new[] { "IsCompulsoryOrVoluntary" });
            AddColumn("dbo.TreatyPricingGroupReferralVersions", "ChecklistRemark", c => c.String());
            AlterColumn("dbo.TreatyPricingGroupReferralVersions", "IsCompulsoryOrVoluntary", c => c.Int(nullable: false));
            CreateIndex("dbo.TreatyPricingGroupReferralVersions", "IsCompulsoryOrVoluntary");
        }
        
        public override void Down()
        {
            DropIndex("dbo.TreatyPricingGroupReferralVersions", new[] { "IsCompulsoryOrVoluntary" });
            AlterColumn("dbo.TreatyPricingGroupReferralVersions", "IsCompulsoryOrVoluntary", c => c.Boolean(nullable: false));
            DropColumn("dbo.TreatyPricingGroupReferralVersions", "ChecklistRemark");
            CreateIndex("dbo.TreatyPricingGroupReferralVersions", "IsCompulsoryOrVoluntary");
        }
    }
}
