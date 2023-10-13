namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTreatyBenefitCodeMappingDetailTable2 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.TreatyBenefitCodeMappingDetails", new[] { "CedingBenefitRiskCode" });
            AlterColumn("dbo.TreatyBenefitCodeMappingDetails", "CedingBenefitRiskCode", c => c.String(maxLength: 50));
            CreateIndex("dbo.TreatyBenefitCodeMappingDetails", "CedingBenefitRiskCode");
        }
        
        public override void Down()
        {
            DropIndex("dbo.TreatyBenefitCodeMappingDetails", new[] { "CedingBenefitRiskCode" });
            AlterColumn("dbo.TreatyBenefitCodeMappingDetails", "CedingBenefitRiskCode", c => c.String(maxLength: 10));
            CreateIndex("dbo.TreatyBenefitCodeMappingDetails", "CedingBenefitRiskCode");
        }
    }
}
