namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTreatyPricingProductBenefitTable3 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.TreatyPricingProductBenefits", new[] { "MinimumSumAssured" });
            DropIndex("dbo.TreatyPricingProductBenefits", new[] { "MaximumSumAssured" });
            DropIndex("dbo.TreatyPricingProductBenefits", new[] { "BenefitPayout" });
            DropIndex("dbo.TreatyPricingProductBenefits", new[] { "CedantRetention" });
            DropIndex("dbo.TreatyPricingProductBenefits", new[] { "ReinsuranceShare" });
            AlterColumn("dbo.TreatyPricingProductBenefits", "MinimumSumAssured", c => c.String(maxLength: 256));
            AlterColumn("dbo.TreatyPricingProductBenefits", "MaximumSumAssured", c => c.String(maxLength: 256));
            AlterColumn("dbo.TreatyPricingProductBenefits", "BenefitPayout", c => c.String(maxLength: 256));
            AlterColumn("dbo.TreatyPricingProductBenefits", "CedantRetention", c => c.String(maxLength: 256));
            AlterColumn("dbo.TreatyPricingProductBenefits", "ReinsuranceShare", c => c.String(maxLength: 256));
            CreateIndex("dbo.TreatyPricingProductBenefits", "MinimumSumAssured");
            CreateIndex("dbo.TreatyPricingProductBenefits", "MaximumSumAssured");
            CreateIndex("dbo.TreatyPricingProductBenefits", "BenefitPayout");
            CreateIndex("dbo.TreatyPricingProductBenefits", "CedantRetention");
            CreateIndex("dbo.TreatyPricingProductBenefits", "ReinsuranceShare");
        }
        
        public override void Down()
        {
            DropIndex("dbo.TreatyPricingProductBenefits", new[] { "ReinsuranceShare" });
            DropIndex("dbo.TreatyPricingProductBenefits", new[] { "CedantRetention" });
            DropIndex("dbo.TreatyPricingProductBenefits", new[] { "BenefitPayout" });
            DropIndex("dbo.TreatyPricingProductBenefits", new[] { "MaximumSumAssured" });
            DropIndex("dbo.TreatyPricingProductBenefits", new[] { "MinimumSumAssured" });
            AlterColumn("dbo.TreatyPricingProductBenefits", "ReinsuranceShare", c => c.String(maxLength: 128));
            AlterColumn("dbo.TreatyPricingProductBenefits", "CedantRetention", c => c.String(maxLength: 128));
            AlterColumn("dbo.TreatyPricingProductBenefits", "BenefitPayout", c => c.String(maxLength: 128));
            AlterColumn("dbo.TreatyPricingProductBenefits", "MaximumSumAssured", c => c.String(maxLength: 128));
            AlterColumn("dbo.TreatyPricingProductBenefits", "MinimumSumAssured", c => c.String(maxLength: 128));
            CreateIndex("dbo.TreatyPricingProductBenefits", "ReinsuranceShare");
            CreateIndex("dbo.TreatyPricingProductBenefits", "CedantRetention");
            CreateIndex("dbo.TreatyPricingProductBenefits", "BenefitPayout");
            CreateIndex("dbo.TreatyPricingProductBenefits", "MaximumSumAssured");
            CreateIndex("dbo.TreatyPricingProductBenefits", "MinimumSumAssured");
        }
    }
}
