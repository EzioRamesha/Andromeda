namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateReferralRiDataTable1 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.ReferralRiData", new[] { "PolicyTerm" });
            DropIndex("dbo.ReferralRiData", new[] { "PolicyTermRemain" });
            AddColumn("dbo.ReferralRiData", "WakalahFeePercentage", c => c.Double());
            AlterColumn("dbo.ReferralRiData", "PolicyTerm", c => c.Double());
            AlterColumn("dbo.ReferralRiData", "PolicyTermRemain", c => c.Double());
            CreateIndex("dbo.ReferralRiData", "PolicyTerm");
            CreateIndex("dbo.ReferralRiData", "PolicyTermRemain");
            CreateIndex("dbo.ReferralRiData", "WakalahFeePercentage");
        }
        
        public override void Down()
        {
            DropIndex("dbo.ReferralRiData", new[] { "WakalahFeePercentage" });
            DropIndex("dbo.ReferralRiData", new[] { "PolicyTermRemain" });
            DropIndex("dbo.ReferralRiData", new[] { "PolicyTerm" });
            AlterColumn("dbo.ReferralRiData", "PolicyTermRemain", c => c.Int());
            AlterColumn("dbo.ReferralRiData", "PolicyTerm", c => c.Int());
            DropColumn("dbo.ReferralRiData", "WakalahFeePercentage");
            CreateIndex("dbo.ReferralRiData", "PolicyTermRemain");
            CreateIndex("dbo.ReferralRiData", "PolicyTerm");
        }
    }
}
