namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTreatyBenefitCodeMappingDetailTable1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TreatyBenefitCodeMappingDetails", "CedingPlanCode", c => c.String(maxLength: 30));
            AddColumn("dbo.TreatyBenefitCodeMappingDetails", "CedingBenefitTypeCode", c => c.String(maxLength: 32));
            AddColumn("dbo.TreatyBenefitCodeMappingDetails", "CedingBenefitRiskCode", c => c.String(maxLength: 10));
            AddColumn("dbo.TreatyBenefitCodeMappingDetails", "CedingTreatyCode", c => c.String(maxLength: 30));
            AddColumn("dbo.TreatyBenefitCodeMappingDetails", "CampaignCode", c => c.String(maxLength: 10));
            CreateIndex("dbo.TreatyBenefitCodeMappingDetails", "CedingPlanCode");
            CreateIndex("dbo.TreatyBenefitCodeMappingDetails", "CedingBenefitTypeCode");
            CreateIndex("dbo.TreatyBenefitCodeMappingDetails", "CedingBenefitRiskCode");
            CreateIndex("dbo.TreatyBenefitCodeMappingDetails", "CedingTreatyCode");
            CreateIndex("dbo.TreatyBenefitCodeMappingDetails", "CampaignCode");
        }
        
        public override void Down()
        {
            DropIndex("dbo.TreatyBenefitCodeMappingDetails", new[] { "CampaignCode" });
            DropIndex("dbo.TreatyBenefitCodeMappingDetails", new[] { "CedingTreatyCode" });
            DropIndex("dbo.TreatyBenefitCodeMappingDetails", new[] { "CedingBenefitRiskCode" });
            DropIndex("dbo.TreatyBenefitCodeMappingDetails", new[] { "CedingBenefitTypeCode" });
            DropIndex("dbo.TreatyBenefitCodeMappingDetails", new[] { "CedingPlanCode" });
            DropColumn("dbo.TreatyBenefitCodeMappingDetails", "CampaignCode");
            DropColumn("dbo.TreatyBenefitCodeMappingDetails", "CedingTreatyCode");
            DropColumn("dbo.TreatyBenefitCodeMappingDetails", "CedingBenefitRiskCode");
            DropColumn("dbo.TreatyBenefitCodeMappingDetails", "CedingBenefitTypeCode");
            DropColumn("dbo.TreatyBenefitCodeMappingDetails", "CedingPlanCode");
        }
    }
}
