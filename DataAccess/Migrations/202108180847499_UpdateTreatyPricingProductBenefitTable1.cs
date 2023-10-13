namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTreatyPricingProductBenefitTable1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TreatyPricingProductBenefits", "ProfitMargin", c => c.String(maxLength: 128));
            AddColumn("dbo.TreatyPricingProductBenefits", "ExpenseMargin", c => c.String(maxLength: 128));
            AddColumn("dbo.TreatyPricingProductBenefits", "CommissionMargin", c => c.String(maxLength: 128));
            AddColumn("dbo.TreatyPricingProductBenefits", "GroupProfitCommissionLoading", c => c.String(maxLength: 128));
            AddColumn("dbo.TreatyPricingProductBenefits", "TakafulLoading", c => c.String(maxLength: 128));
            CreateIndex("dbo.TreatyPricingProductBenefits", "ProfitMargin");
            CreateIndex("dbo.TreatyPricingProductBenefits", "ExpenseMargin");
            CreateIndex("dbo.TreatyPricingProductBenefits", "CommissionMargin");
            CreateIndex("dbo.TreatyPricingProductBenefits", "GroupProfitCommissionLoading");
            CreateIndex("dbo.TreatyPricingProductBenefits", "TakafulLoading");
        }
        
        public override void Down()
        {
            DropIndex("dbo.TreatyPricingProductBenefits", new[] { "TakafulLoading" });
            DropIndex("dbo.TreatyPricingProductBenefits", new[] { "GroupProfitCommissionLoading" });
            DropIndex("dbo.TreatyPricingProductBenefits", new[] { "CommissionMargin" });
            DropIndex("dbo.TreatyPricingProductBenefits", new[] { "ExpenseMargin" });
            DropIndex("dbo.TreatyPricingProductBenefits", new[] { "ProfitMargin" });
            DropColumn("dbo.TreatyPricingProductBenefits", "TakafulLoading");
            DropColumn("dbo.TreatyPricingProductBenefits", "GroupProfitCommissionLoading");
            DropColumn("dbo.TreatyPricingProductBenefits", "CommissionMargin");
            DropColumn("dbo.TreatyPricingProductBenefits", "ExpenseMargin");
            DropColumn("dbo.TreatyPricingProductBenefits", "ProfitMargin");
        }
    }
}
