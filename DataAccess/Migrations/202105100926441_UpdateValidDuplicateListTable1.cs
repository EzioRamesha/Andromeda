namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateValidDuplicateListTable1 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.ValidDuplicationLists", new[] { "InsuredName" });
            DropIndex("dbo.ValidDuplicationLists", new[] { "CedingBenefitRiskCode" });
            DropIndex("dbo.ValidDuplicationLists", new[] { "CedingBenefitTypeCode" });
            AlterColumn("dbo.ValidDuplicationLists", "InsuredName", c => c.String(maxLength: 255));
            AlterColumn("dbo.ValidDuplicationLists", "CedingBenefitRiskCode", c => c.String(maxLength: 100));
            AlterColumn("dbo.ValidDuplicationLists", "CedingBenefitTypeCode", c => c.String(maxLength: 100));
            CreateIndex("dbo.ValidDuplicationLists", "InsuredName");
            CreateIndex("dbo.ValidDuplicationLists", "CedingBenefitRiskCode");
            CreateIndex("dbo.ValidDuplicationLists", "CedingBenefitTypeCode");
        }
        
        public override void Down()
        {
            DropIndex("dbo.ValidDuplicationLists", new[] { "CedingBenefitTypeCode" });
            DropIndex("dbo.ValidDuplicationLists", new[] { "CedingBenefitRiskCode" });
            DropIndex("dbo.ValidDuplicationLists", new[] { "InsuredName" });
            AlterColumn("dbo.ValidDuplicationLists", "CedingBenefitTypeCode", c => c.String(maxLength: 30));
            AlterColumn("dbo.ValidDuplicationLists", "CedingBenefitRiskCode", c => c.String(maxLength: 30));
            AlterColumn("dbo.ValidDuplicationLists", "InsuredName", c => c.String(maxLength: 100));
            CreateIndex("dbo.ValidDuplicationLists", "CedingBenefitTypeCode");
            CreateIndex("dbo.ValidDuplicationLists", "CedingBenefitRiskCode");
            CreateIndex("dbo.ValidDuplicationLists", "InsuredName");
        }
    }
}
