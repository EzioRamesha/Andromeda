namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRiDataTable4 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.RiData", new[] { "CedingBenefitRiskCode" });
            AlterColumn("dbo.RiData", "CedingBenefitRiskCode", c => c.String(maxLength: 50));
            AlterColumn("dbo.RiData", "LoadingType", c => c.String(maxLength: 15));
            CreateIndex("dbo.RiData", "CedingBenefitRiskCode");
        }
        
        public override void Down()
        {
            DropIndex("dbo.RiData", new[] { "CedingBenefitRiskCode" });
            AlterColumn("dbo.RiData", "LoadingType", c => c.String(maxLength: 10));
            AlterColumn("dbo.RiData", "CedingBenefitRiskCode", c => c.String(maxLength: 10));
            CreateIndex("dbo.RiData", "CedingBenefitRiskCode");
        }
    }
}
