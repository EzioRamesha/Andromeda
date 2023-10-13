namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class UpdateTreatyBenefitCodeMappingTable2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TreatyBenefitCodeMappings", "UnderwriterRatingFrom", c => c.Double());
            AddColumn("dbo.TreatyBenefitCodeMappings", "UnderwriterRatingTo", c => c.Double());
            AddColumn("dbo.TreatyBenefitCodeMappings", "RiShare2", c => c.Double());
            AddColumn("dbo.TreatyBenefitCodeMappings", "RiShareCap2", c => c.Double());
            CreateIndex("dbo.TreatyBenefitCodeMappings", "UnderwriterRatingFrom");
            CreateIndex("dbo.TreatyBenefitCodeMappings", "UnderwriterRatingTo");
            CreateIndex("dbo.TreatyBenefitCodeMappings", "RiShare2");
            CreateIndex("dbo.TreatyBenefitCodeMappings", "RiShareCap2");
        }

        public override void Down()
        {
            DropIndex("dbo.TreatyBenefitCodeMappings", new[] { "RiShareCap2" });
            DropIndex("dbo.TreatyBenefitCodeMappings", new[] { "RiShare2" });
            DropIndex("dbo.TreatyBenefitCodeMappings", new[] { "UnderwriterRatingTo" });
            DropIndex("dbo.TreatyBenefitCodeMappings", new[] { "UnderwriterRatingFrom" });
            DropColumn("dbo.TreatyBenefitCodeMappings", "RiShareCap2");
            DropColumn("dbo.TreatyBenefitCodeMappings", "RiShare2");
            DropColumn("dbo.TreatyBenefitCodeMappings", "UnderwriterRatingTo");
            DropColumn("dbo.TreatyBenefitCodeMappings", "UnderwriterRatingFrom");
        }
    }
}
