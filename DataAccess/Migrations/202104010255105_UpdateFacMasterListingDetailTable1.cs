namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class UpdateFacMasterListingDetailTable1 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.FacMasterListingDetails", "IX_FacMasterListingMapping");
            AddColumn("dbo.FacMasterListingDetails", "BenefitCode", c => c.String(maxLength: 10));
            CreateIndex("dbo.FacMasterListingDetails", new[] { "PolicyNumber", "BenefitCode", "FacMasterListingId" }, name: "IX_FacMasterListingMapping");
            CreateIndex("dbo.FacMasterListingDetails", "BenefitCode");
        }

        public override void Down()
        {
            DropIndex("dbo.FacMasterListingDetails", new[] { "BenefitCode" });
            DropIndex("dbo.FacMasterListingDetails", "IX_FacMasterListingMapping");
            DropColumn("dbo.FacMasterListingDetails", "BenefitCode");
            CreateIndex("dbo.FacMasterListingDetails", new[] { "PolicyNumber", "FacMasterListingId" }, name: "IX_FacMasterListingMapping");
        }
    }
}
