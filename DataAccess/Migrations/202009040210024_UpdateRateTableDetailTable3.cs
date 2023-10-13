namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRateTableDetailTable3 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.RateTableDetails", new[] { "CedingBenefitRiskCode" });
            AlterColumn("dbo.RateTableDetails", "CedingBenefitRiskCode", c => c.String(maxLength: 50));
            CreateIndex("dbo.RateTableDetails", "CedingBenefitRiskCode");
        }
        
        public override void Down()
        {
            DropIndex("dbo.RateTableDetails", new[] { "CedingBenefitRiskCode" });
            AlterColumn("dbo.RateTableDetails", "CedingBenefitRiskCode", c => c.String(maxLength: 10));
            CreateIndex("dbo.RateTableDetails", "CedingBenefitRiskCode");
        }
    }
}
