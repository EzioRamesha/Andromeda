namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class UpdateTreatyBenefitCodeMappingTable3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TreatyBenefitCodeMappings", "OriSumAssuredFrom", c => c.Double());
            AddColumn("dbo.TreatyBenefitCodeMappings", "OriSumAssuredTo", c => c.Double());
            CreateIndex("dbo.TreatyBenefitCodeMappings", "OriSumAssuredFrom");
            CreateIndex("dbo.TreatyBenefitCodeMappings", "OriSumAssuredTo");
        }

        public override void Down()
        {
            DropIndex("dbo.TreatyBenefitCodeMappings", new[] { "OriSumAssuredTo" });
            DropIndex("dbo.TreatyBenefitCodeMappings", new[] { "OriSumAssuredFrom" });
            DropColumn("dbo.TreatyBenefitCodeMappings", "OriSumAssuredTo");
            DropColumn("dbo.TreatyBenefitCodeMappings", "OriSumAssuredFrom");
        }
    }
}
