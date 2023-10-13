namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTreatyBenefitCodeMappingTable1 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.TreatyBenefitCodeMappings", new[] { "CedingPlanCode" });
            DropIndex("dbo.TreatyBenefitCodeMappings", new[] { "CedingBenefitTypeCode" });
            DropIndex("dbo.TreatyBenefitCodeMappings", new[] { "CedingBenefitRiskCode" });
            DropIndex("dbo.TreatyBenefitCodeMappings", new[] { "CedingTreatyCode" });
            DropIndex("dbo.TreatyBenefitCodeMappings", new[] { "CampaignCode" });
            AlterColumn("dbo.TreatyBenefitCodeMappings", "CedingPlanCode", c => c.String(nullable: false, storeType: "ntext"));
            AlterColumn("dbo.TreatyBenefitCodeMappings", "CedingBenefitTypeCode", c => c.String(nullable: false, storeType: "ntext"));
            AlterColumn("dbo.TreatyBenefitCodeMappings", "CedingBenefitRiskCode", c => c.String(storeType: "ntext"));
            AlterColumn("dbo.TreatyBenefitCodeMappings", "CedingTreatyCode", c => c.String(storeType: "ntext"));
            AlterColumn("dbo.TreatyBenefitCodeMappings", "CampaignCode", c => c.String(storeType: "ntext"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TreatyBenefitCodeMappings", "CampaignCode", c => c.String(maxLength: 255));
            AlterColumn("dbo.TreatyBenefitCodeMappings", "CedingTreatyCode", c => c.String(maxLength: 255));
            AlterColumn("dbo.TreatyBenefitCodeMappings", "CedingBenefitRiskCode", c => c.String(maxLength: 255));
            AlterColumn("dbo.TreatyBenefitCodeMappings", "CedingBenefitTypeCode", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.TreatyBenefitCodeMappings", "CedingPlanCode", c => c.String(nullable: false, maxLength: 255));
            CreateIndex("dbo.TreatyBenefitCodeMappings", "CampaignCode");
            CreateIndex("dbo.TreatyBenefitCodeMappings", "CedingTreatyCode");
            CreateIndex("dbo.TreatyBenefitCodeMappings", "CedingBenefitRiskCode");
            CreateIndex("dbo.TreatyBenefitCodeMappings", "CedingBenefitTypeCode");
            CreateIndex("dbo.TreatyBenefitCodeMappings", "CedingPlanCode");
        }
    }
}
