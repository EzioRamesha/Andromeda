namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateFacMasterListingTable2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FacMasterListings", "CedingBenefitTypeCode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.FacMasterListings", "CedingBenefitTypeCode");
        }
    }
}
