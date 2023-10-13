namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRiDataTable13 : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.RiData", new[] { "TreatyCode", "RiskPeriodMonth", "RiskPeriodYear", "TransactionTypeCode", "PolicyNumber", "CedingBasicPlanCode", "CedingBenefitTypeCode", "CedingBenefitRiskCode", "InsuredName", "NetPremium" }, name: "IX_Duplicate");
        }
        
        public override void Down()
        {
            DropIndex("dbo.RiData", "IX_Duplicate");
        }
    }
}
