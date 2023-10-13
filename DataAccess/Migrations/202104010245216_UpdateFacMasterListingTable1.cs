namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateFacMasterListingTable1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.FacMasterListings", "BenefitId", "dbo.Benefits");
            DropIndex("dbo.FacMasterListings", new[] { "BenefitId" });
            AddColumn("dbo.FacMasterListings", "BenefitCode", c => c.String(storeType: "ntext"));
            DropColumn("dbo.FacMasterListings", "BenefitId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.FacMasterListings", "BenefitId", c => c.Int());
            DropColumn("dbo.FacMasterListings", "BenefitCode");
            CreateIndex("dbo.FacMasterListings", "BenefitId");
            AddForeignKey("dbo.FacMasterListings", "BenefitId", "dbo.Benefits", "Id");
        }
    }
}
