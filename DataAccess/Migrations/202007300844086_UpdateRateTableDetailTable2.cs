namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRateTableDetailTable2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RateTableDetails", "CedingTreatyCode", c => c.String(maxLength: 30));
            AddColumn("dbo.RateTableDetails", "CedingPlanCode2", c => c.String(maxLength: 10));
            AddColumn("dbo.RateTableDetails", "CedingBenefitTypeCode", c => c.String(maxLength: 30));
            AddColumn("dbo.RateTableDetails", "CedingBenefitRiskCode", c => c.String(maxLength: 10));
            AddColumn("dbo.RateTableDetails", "GroupPolicyNumber", c => c.String(maxLength: 30));
            CreateIndex("dbo.RateTableDetails", "CedingTreatyCode");
            CreateIndex("dbo.RateTableDetails", "CedingPlanCode2");
            CreateIndex("dbo.RateTableDetails", "CedingBenefitTypeCode");
            CreateIndex("dbo.RateTableDetails", "CedingBenefitRiskCode");
            CreateIndex("dbo.RateTableDetails", "GroupPolicyNumber");
        }
        
        public override void Down()
        {
            DropIndex("dbo.RateTableDetails", new[] { "GroupPolicyNumber" });
            DropIndex("dbo.RateTableDetails", new[] { "CedingBenefitRiskCode" });
            DropIndex("dbo.RateTableDetails", new[] { "CedingBenefitTypeCode" });
            DropIndex("dbo.RateTableDetails", new[] { "CedingPlanCode2" });
            DropIndex("dbo.RateTableDetails", new[] { "CedingTreatyCode" });
            DropColumn("dbo.RateTableDetails", "GroupPolicyNumber");
            DropColumn("dbo.RateTableDetails", "CedingBenefitRiskCode");
            DropColumn("dbo.RateTableDetails", "CedingBenefitTypeCode");
            DropColumn("dbo.RateTableDetails", "CedingPlanCode2");
            DropColumn("dbo.RateTableDetails", "CedingTreatyCode");
        }
    }
}
