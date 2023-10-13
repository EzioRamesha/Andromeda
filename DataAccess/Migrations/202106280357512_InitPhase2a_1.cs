namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitPhase2a_1 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.TreatyPricingAdvantagePrograms", new[] { "Code" });
            DropIndex("dbo.TreatyPricingCampaigns", new[] { "Code" });
            DropIndex("dbo.TreatyPricingProducts", new[] { "Code" });
            DropIndex("dbo.TreatyPricingFinancialTables", new[] { "FinancialTableId" });
            DropIndex("dbo.TreatyPricingMedicalTables", new[] { "MedicalTableId" });
            DropIndex("dbo.TreatyPricingUwLimits", new[] { "LimitId" });
            AlterColumn("dbo.TreatyPricingAdvantagePrograms", "Code", c => c.String(nullable: false, maxLength: 60));
            AlterColumn("dbo.TreatyPricingCampaigns", "Code", c => c.String(nullable: false, maxLength: 60));
            AlterColumn("dbo.TreatyPricingProducts", "Code", c => c.String(nullable: false, maxLength: 60));
            AlterColumn("dbo.TreatyPricingFinancialTables", "FinancialTableId", c => c.String(maxLength: 60));
            AlterColumn("dbo.TreatyPricingMedicalTables", "MedicalTableId", c => c.String(maxLength: 60));
            AlterColumn("dbo.TreatyPricingUwLimits", "LimitId", c => c.String(maxLength: 60));
            CreateIndex("dbo.TreatyPricingAdvantagePrograms", "Code");
            CreateIndex("dbo.TreatyPricingCampaigns", "Code");
            CreateIndex("dbo.TreatyPricingProducts", "Code");
            CreateIndex("dbo.TreatyPricingFinancialTables", "FinancialTableId");
            CreateIndex("dbo.TreatyPricingMedicalTables", "MedicalTableId");
            CreateIndex("dbo.TreatyPricingUwLimits", "LimitId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.TreatyPricingUwLimits", new[] { "LimitId" });
            DropIndex("dbo.TreatyPricingMedicalTables", new[] { "MedicalTableId" });
            DropIndex("dbo.TreatyPricingFinancialTables", new[] { "FinancialTableId" });
            DropIndex("dbo.TreatyPricingProducts", new[] { "Code" });
            DropIndex("dbo.TreatyPricingCampaigns", new[] { "Code" });
            DropIndex("dbo.TreatyPricingAdvantagePrograms", new[] { "Code" });
            AlterColumn("dbo.TreatyPricingUwLimits", "LimitId", c => c.String(maxLength: 30));
            AlterColumn("dbo.TreatyPricingMedicalTables", "MedicalTableId", c => c.String(maxLength: 30));
            AlterColumn("dbo.TreatyPricingFinancialTables", "FinancialTableId", c => c.String(maxLength: 30));
            AlterColumn("dbo.TreatyPricingProducts", "Code", c => c.String(nullable: false, maxLength: 30));
            AlterColumn("dbo.TreatyPricingCampaigns", "Code", c => c.String(nullable: false, maxLength: 30));
            AlterColumn("dbo.TreatyPricingAdvantagePrograms", "Code", c => c.String(nullable: false, maxLength: 30));
            CreateIndex("dbo.TreatyPricingUwLimits", "LimitId");
            CreateIndex("dbo.TreatyPricingMedicalTables", "MedicalTableId");
            CreateIndex("dbo.TreatyPricingFinancialTables", "FinancialTableId");
            CreateIndex("dbo.TreatyPricingProducts", "Code");
            CreateIndex("dbo.TreatyPricingCampaigns", "Code");
            CreateIndex("dbo.TreatyPricingAdvantagePrograms", "Code");
        }
    }
}
