namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTreatyPricingMedicalTableUploadCellsTable1 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.TreatyPricingFinancialTableUploads", new[] { "Code" });
            DropIndex("dbo.TreatyPricingMedicalTableUploadCells", new[] { "Code" });
            AlterColumn("dbo.TreatyPricingFinancialTableUploads", "Code", c => c.String(maxLength: 255));
            AlterColumn("dbo.TreatyPricingMedicalTableUploadCells", "Code", c => c.String(maxLength: 255));
            CreateIndex("dbo.TreatyPricingFinancialTableUploads", "Code");
            CreateIndex("dbo.TreatyPricingMedicalTableUploadCells", "Code");
        }
        
        public override void Down()
        {
            DropIndex("dbo.TreatyPricingMedicalTableUploadCells", new[] { "Code" });
            DropIndex("dbo.TreatyPricingFinancialTableUploads", new[] { "Code" });
            AlterColumn("dbo.TreatyPricingMedicalTableUploadCells", "Code", c => c.String(maxLength: 30));
            AlterColumn("dbo.TreatyPricingFinancialTableUploads", "Code", c => c.String(maxLength: 30));
            CreateIndex("dbo.TreatyPricingMedicalTableUploadCells", "Code");
            CreateIndex("dbo.TreatyPricingFinancialTableUploads", "Code");
        }
    }
}
