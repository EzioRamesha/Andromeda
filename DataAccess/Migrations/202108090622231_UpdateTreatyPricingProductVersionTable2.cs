namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTreatyPricingProductVersionTable2 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.TreatyPricingProductVersions", new[] { "WaitingPeriod" });
            DropIndex("dbo.TreatyPricingProductVersions", new[] { "RecaptureClause" });
            AlterColumn("dbo.TreatyPricingProductVersions", "WaitingPeriod", c => c.String(maxLength: 256));
            AlterColumn("dbo.TreatyPricingProductVersions", "RecaptureClause", c => c.String(maxLength: 256));
            CreateIndex("dbo.TreatyPricingProductVersions", "WaitingPeriod");
            CreateIndex("dbo.TreatyPricingProductVersions", "RecaptureClause");
        }
        
        public override void Down()
        {
            DropIndex("dbo.TreatyPricingProductVersions", new[] { "RecaptureClause" });
            DropIndex("dbo.TreatyPricingProductVersions", new[] { "WaitingPeriod" });
            AlterColumn("dbo.TreatyPricingProductVersions", "RecaptureClause", c => c.String(maxLength: 128));
            AlterColumn("dbo.TreatyPricingProductVersions", "WaitingPeriod", c => c.String(maxLength: 128));
            CreateIndex("dbo.TreatyPricingProductVersions", "RecaptureClause");
            CreateIndex("dbo.TreatyPricingProductVersions", "WaitingPeriod");
        }
    }
}
