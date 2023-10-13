namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRateTableTable2 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.RateTables", new[] { "TreatyCode" });
            DropIndex("dbo.RateTables", new[] { "CedingPlanCode" });
            DropIndex("dbo.RateTables", new[] { "CedingTreatyCode" });
            DropIndex("dbo.RateTables", new[] { "CedingPlanCode2" });
            DropIndex("dbo.RateTables", new[] { "CedingBenefitTypeCode" });
            DropIndex("dbo.RateTables", new[] { "CedingBenefitRiskCode" });
            DropIndex("dbo.RateTables", new[] { "GroupPolicyNumber" });
            AlterColumn("dbo.RateTables", "TreatyCode", c => c.String(nullable: false, storeType: "ntext"));
            AlterColumn("dbo.RateTables", "CedingPlanCode", c => c.String(storeType: "ntext"));
            AlterColumn("dbo.RateTables", "CedingTreatyCode", c => c.String(storeType: "ntext"));
            AlterColumn("dbo.RateTables", "CedingPlanCode2", c => c.String(storeType: "ntext"));
            AlterColumn("dbo.RateTables", "CedingBenefitTypeCode", c => c.String(storeType: "ntext"));
            AlterColumn("dbo.RateTables", "CedingBenefitRiskCode", c => c.String(storeType: "ntext"));
            AlterColumn("dbo.RateTables", "GroupPolicyNumber", c => c.String(storeType: "ntext"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.RateTables", "GroupPolicyNumber", c => c.String(maxLength: 255));
            AlterColumn("dbo.RateTables", "CedingBenefitRiskCode", c => c.String(maxLength: 255));
            AlterColumn("dbo.RateTables", "CedingBenefitTypeCode", c => c.String(maxLength: 255));
            AlterColumn("dbo.RateTables", "CedingPlanCode2", c => c.String(maxLength: 255));
            AlterColumn("dbo.RateTables", "CedingTreatyCode", c => c.String(maxLength: 255));
            AlterColumn("dbo.RateTables", "CedingPlanCode", c => c.String(maxLength: 255));
            AlterColumn("dbo.RateTables", "TreatyCode", c => c.String(nullable: false, maxLength: 255));
            CreateIndex("dbo.RateTables", "GroupPolicyNumber");
            CreateIndex("dbo.RateTables", "CedingBenefitRiskCode");
            CreateIndex("dbo.RateTables", "CedingBenefitTypeCode");
            CreateIndex("dbo.RateTables", "CedingPlanCode2");
            CreateIndex("dbo.RateTables", "CedingTreatyCode");
            CreateIndex("dbo.RateTables", "CedingPlanCode");
            CreateIndex("dbo.RateTables", "TreatyCode");
        }
    }
}
