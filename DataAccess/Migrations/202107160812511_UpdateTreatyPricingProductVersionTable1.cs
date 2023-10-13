namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTreatyPricingProductVersionTable1 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.TreatyPricingProductVersions", new[] { "ResidenceCountry" });
            AddColumn("dbo.TreatyPricingProductVersions", "TerritoryOfIssueCodePickListDetailId", c => c.Int());
            CreateIndex("dbo.TreatyPricingProductVersions", "TerritoryOfIssueCodePickListDetailId");
            AddForeignKey("dbo.TreatyPricingProductVersions", "TerritoryOfIssueCodePickListDetailId", "dbo.PickListDetails", "Id");
            DropColumn("dbo.TreatyPricingProductVersions", "ResidenceCountry");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TreatyPricingProductVersions", "ResidenceCountry", c => c.String(maxLength: 128));
            DropForeignKey("dbo.TreatyPricingProductVersions", "TerritoryOfIssueCodePickListDetailId", "dbo.PickListDetails");
            DropIndex("dbo.TreatyPricingProductVersions", new[] { "TerritoryOfIssueCodePickListDetailId" });
            DropColumn("dbo.TreatyPricingProductVersions", "TerritoryOfIssueCodePickListDetailId");
            CreateIndex("dbo.TreatyPricingProductVersions", "ResidenceCountry");
        }
    }
}
