namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class UpdateTreatyBenefitCodeMappingTable4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TreatyBenefitCodeMappings", "EffectiveDate", c => c.DateTime(precision: 7, storeType: "datetime2"));
            CreateIndex("dbo.TreatyBenefitCodeMappings", "ReinsEffDatePolStartDate");
            CreateIndex("dbo.TreatyBenefitCodeMappings", "ReinsEffDatePolEndDate");
            CreateIndex("dbo.TreatyBenefitCodeMappings", "ReportingStartDate");
            CreateIndex("dbo.TreatyBenefitCodeMappings", "ReportingEndDate");
            CreateIndex("dbo.TreatyBenefitCodeMappings", "EffectiveDate");
        }

        public override void Down()
        {
            DropIndex("dbo.TreatyBenefitCodeMappings", new[] { "EffectiveDate" });
            DropIndex("dbo.TreatyBenefitCodeMappings", new[] { "ReportingEndDate" });
            DropIndex("dbo.TreatyBenefitCodeMappings", new[] { "ReportingStartDate" });
            DropIndex("dbo.TreatyBenefitCodeMappings", new[] { "ReinsEffDatePolEndDate" });
            DropIndex("dbo.TreatyBenefitCodeMappings", new[] { "ReinsEffDatePolStartDate" });
            DropColumn("dbo.TreatyBenefitCodeMappings", "EffectiveDate");
        }
    }
}
