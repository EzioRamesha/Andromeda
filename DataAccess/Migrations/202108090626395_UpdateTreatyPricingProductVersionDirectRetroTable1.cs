namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTreatyPricingProductVersionDirectRetroTable1 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.TreatyPricingProductBenefitDirectRetros", new[] { "MlreRetention" });
            DropIndex("dbo.TreatyPricingProductBenefitDirectRetros", new[] { "RetrocessionShare" });
            DropIndex("dbo.TreatyPricingProductBenefitDirectRetros", new[] { "RetrocessionRateTable" });
            DropIndex("dbo.TreatyPricingProductBenefitDirectRetros", new[] { "NewBusinessRateGuarantee" });
            DropIndex("dbo.TreatyPricingProductBenefitDirectRetros", new[] { "RenewalBusinessRateGuarantee" });
            DropIndex("dbo.TreatyPricingProductBenefitDirectRetros", new[] { "AdditionalDiscount" });
            AlterColumn("dbo.TreatyPricingProductBenefitDirectRetros", "MlreRetention", c => c.String(maxLength: 512));
            AlterColumn("dbo.TreatyPricingProductBenefitDirectRetros", "RetrocessionShare", c => c.String(maxLength: 512));
            AlterColumn("dbo.TreatyPricingProductBenefitDirectRetros", "RetrocessionRateTable", c => c.String(maxLength: 256));
            AlterColumn("dbo.TreatyPricingProductBenefitDirectRetros", "NewBusinessRateGuarantee", c => c.String(maxLength: 256));
            AlterColumn("dbo.TreatyPricingProductBenefitDirectRetros", "RenewalBusinessRateGuarantee", c => c.String(maxLength: 256));
            AlterColumn("dbo.TreatyPricingProductBenefitDirectRetros", "AdditionalDiscount", c => c.String(maxLength: 256));
            CreateIndex("dbo.TreatyPricingProductBenefitDirectRetros", "MlreRetention");
            CreateIndex("dbo.TreatyPricingProductBenefitDirectRetros", "RetrocessionShare");
            CreateIndex("dbo.TreatyPricingProductBenefitDirectRetros", "RetrocessionRateTable");
            CreateIndex("dbo.TreatyPricingProductBenefitDirectRetros", "NewBusinessRateGuarantee");
            CreateIndex("dbo.TreatyPricingProductBenefitDirectRetros", "RenewalBusinessRateGuarantee");
            CreateIndex("dbo.TreatyPricingProductBenefitDirectRetros", "AdditionalDiscount");
        }
        
        public override void Down()
        {
            DropIndex("dbo.TreatyPricingProductBenefitDirectRetros", new[] { "AdditionalDiscount" });
            DropIndex("dbo.TreatyPricingProductBenefitDirectRetros", new[] { "RenewalBusinessRateGuarantee" });
            DropIndex("dbo.TreatyPricingProductBenefitDirectRetros", new[] { "NewBusinessRateGuarantee" });
            DropIndex("dbo.TreatyPricingProductBenefitDirectRetros", new[] { "RetrocessionRateTable" });
            DropIndex("dbo.TreatyPricingProductBenefitDirectRetros", new[] { "RetrocessionShare" });
            DropIndex("dbo.TreatyPricingProductBenefitDirectRetros", new[] { "MlreRetention" });
            AlterColumn("dbo.TreatyPricingProductBenefitDirectRetros", "AdditionalDiscount", c => c.String(maxLength: 128));
            AlterColumn("dbo.TreatyPricingProductBenefitDirectRetros", "RenewalBusinessRateGuarantee", c => c.String(maxLength: 128));
            AlterColumn("dbo.TreatyPricingProductBenefitDirectRetros", "NewBusinessRateGuarantee", c => c.String(maxLength: 128));
            AlterColumn("dbo.TreatyPricingProductBenefitDirectRetros", "RetrocessionRateTable", c => c.String(maxLength: 128));
            AlterColumn("dbo.TreatyPricingProductBenefitDirectRetros", "RetrocessionShare", c => c.String(maxLength: 128));
            AlterColumn("dbo.TreatyPricingProductBenefitDirectRetros", "MlreRetention", c => c.String(maxLength: 128));
            CreateIndex("dbo.TreatyPricingProductBenefitDirectRetros", "AdditionalDiscount");
            CreateIndex("dbo.TreatyPricingProductBenefitDirectRetros", "RenewalBusinessRateGuarantee");
            CreateIndex("dbo.TreatyPricingProductBenefitDirectRetros", "NewBusinessRateGuarantee");
            CreateIndex("dbo.TreatyPricingProductBenefitDirectRetros", "RetrocessionRateTable");
            CreateIndex("dbo.TreatyPricingProductBenefitDirectRetros", "RetrocessionShare");
            CreateIndex("dbo.TreatyPricingProductBenefitDirectRetros", "MlreRetention");
        }
    }
}
