namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateFacMasterListingDetailTable2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FacMasterListingDetails", "CedingBenefitTypeCode", c => c.String(maxLength: 10));
            CreateIndex("dbo.FacMasterListingDetails", "CedingBenefitTypeCode");
        }
        
        public override void Down()
        {
            DropIndex("dbo.FacMasterListingDetails", new[] { "CedingBenefitTypeCode" });
            DropColumn("dbo.FacMasterListingDetails", "CedingBenefitTypeCode");
        }
    }
}
