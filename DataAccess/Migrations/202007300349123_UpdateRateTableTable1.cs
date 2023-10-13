namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class UpdateRateTableTable1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RateTables", "CedingTreatyCode", c => c.String(maxLength: 255));
            AddColumn("dbo.RateTables", "CedingPlanCode2", c => c.String(maxLength: 255));
            AddColumn("dbo.RateTables", "CedingBenefitTypeCode", c => c.String(maxLength: 255));
            AddColumn("dbo.RateTables", "CedingBenefitRiskCode", c => c.String(maxLength: 255));
            AddColumn("dbo.RateTables", "GroupPolicyNumber", c => c.String(maxLength: 255));
            AddColumn("dbo.RateTables", "ReinsBasisCodePickListDetailId", c => c.Int());
            AddColumn("dbo.RateTables", "AarFrom", c => c.Int());
            AddColumn("dbo.RateTables", "AarTo", c => c.Int());
            CreateIndex("dbo.RateTables", "CedingTreatyCode");
            CreateIndex("dbo.RateTables", "CedingPlanCode2");
            CreateIndex("dbo.RateTables", "CedingBenefitTypeCode");
            CreateIndex("dbo.RateTables", "CedingBenefitRiskCode");
            CreateIndex("dbo.RateTables", "GroupPolicyNumber");
            CreateIndex("dbo.RateTables", "ReinsBasisCodePickListDetailId");
            CreateIndex("dbo.RateTables", "AarFrom");
            CreateIndex("dbo.RateTables", "AarTo");
            AddForeignKey("dbo.RateTables", "ReinsBasisCodePickListDetailId", "dbo.PickListDetails", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.RateTables", "ReinsBasisCodePickListDetailId", "dbo.PickListDetails");
            DropIndex("dbo.RateTables", new[] { "AarTo" });
            DropIndex("dbo.RateTables", new[] { "AarFrom" });
            DropIndex("dbo.RateTables", new[] { "ReinsBasisCodePickListDetailId" });
            DropIndex("dbo.RateTables", new[] { "GroupPolicyNumber" });
            DropIndex("dbo.RateTables", new[] { "CedingBenefitRiskCode" });
            DropIndex("dbo.RateTables", new[] { "CedingBenefitTypeCode" });
            DropIndex("dbo.RateTables", new[] { "CedingPlanCode2" });
            DropIndex("dbo.RateTables", new[] { "CedingTreatyCode" });
            DropColumn("dbo.RateTables", "AarTo");
            DropColumn("dbo.RateTables", "AarFrom");
            DropColumn("dbo.RateTables", "ReinsBasisCodePickListDetailId");
            DropColumn("dbo.RateTables", "GroupPolicyNumber");
            DropColumn("dbo.RateTables", "CedingBenefitRiskCode");
            DropColumn("dbo.RateTables", "CedingBenefitTypeCode");
            DropColumn("dbo.RateTables", "CedingPlanCode2");
            DropColumn("dbo.RateTables", "CedingTreatyCode");
        }
    }
}
