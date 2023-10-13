namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTreatyPricingFinancialTableUploadLegendsTable1 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.TreatyPricingFinancialTableUploadLegends", new[] { "Description" });
            DropIndex("dbo.TreatyPricingMedicalTableUploadLegends", new[] { "Description" });
            AlterColumn("dbo.TreatyPricingFinancialTableUploadLegends", "Description", c => c.String());
            AlterColumn("dbo.TreatyPricingMedicalTableUploadLegends", "Description", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TreatyPricingMedicalTableUploadLegends", "Description", c => c.String(maxLength: 255));
            AlterColumn("dbo.TreatyPricingFinancialTableUploadLegends", "Description", c => c.String(maxLength: 255));
            CreateIndex("dbo.TreatyPricingMedicalTableUploadLegends", "Description");
            CreateIndex("dbo.TreatyPricingFinancialTableUploadLegends", "Description");
        }
    }
}
