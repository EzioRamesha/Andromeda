namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRiDataTable2 : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.RiData", "TreatyCode");
            CreateIndex("dbo.RiData", "RiskPeriodMonth");
            CreateIndex("dbo.RiData", "RiskPeriodYear");
            CreateIndex("dbo.RiData", "TransactionTypeCode");
            CreateIndex("dbo.RiData", "PolicyNumber");
            CreateIndex("dbo.RiData", "CedingPlanCode");
            CreateIndex("dbo.RiData", "CedingBenefitTypeCode");
            CreateIndex("dbo.RiData", "CedingBenefitRiskCode");
            CreateIndex("dbo.RiData", "InsuredName");
        }
        
        public override void Down()
        {
            DropIndex("dbo.RiData", new[] { "InsuredName" });
            DropIndex("dbo.RiData", new[] { "CedingBenefitRiskCode" });
            DropIndex("dbo.RiData", new[] { "CedingBenefitTypeCode" });
            DropIndex("dbo.RiData", new[] { "CedingPlanCode" });
            DropIndex("dbo.RiData", new[] { "PolicyNumber" });
            DropIndex("dbo.RiData", new[] { "TransactionTypeCode" });
            DropIndex("dbo.RiData", new[] { "RiskPeriodYear" });
            DropIndex("dbo.RiData", new[] { "RiskPeriodMonth" });
            DropIndex("dbo.RiData", new[] { "TreatyCode" });
        }
    }
}
