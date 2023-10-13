namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTreatyPricingProductBenefitTable2 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.TreatyPricingProductBenefits", new[] { "TakafulLoading" });
            RenameColumn("dbo.TreatyPricingProductBenefits", "TakafulLoading", "TabarruLoading");
            CreateIndex("dbo.TreatyPricingProductBenefits", "TabarruLoading");
        }
        
        public override void Down()
        {
            DropIndex("dbo.TreatyPricingProductBenefits", new[] { "TabarruLoading" });
            RenameColumn("dbo.TreatyPricingProductBenefits", "TabarruLoading", "TakafulLoading");
            CreateIndex("dbo.TreatyPricingProductBenefits", "TakafulLoading");
        }
    }
}
