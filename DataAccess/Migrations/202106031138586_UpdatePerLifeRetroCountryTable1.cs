namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatePerLifeRetroCountryTable1 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.PerLifeRetroCountries", new[] { "Country" });
            AddColumn("dbo.PerLifeRetroCountries", "TerritoryOfIssueCodePickListDetailId", c => c.Int());
            AlterColumn("dbo.PerLifeRetroCountries", "Country", c => c.String(maxLength: 50));
            CreateIndex("dbo.PerLifeRetroCountries", "TerritoryOfIssueCodePickListDetailId");
            CreateIndex("dbo.PerLifeRetroCountries", "Country");
            AddForeignKey("dbo.PerLifeRetroCountries", "TerritoryOfIssueCodePickListDetailId", "dbo.PickListDetails", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PerLifeRetroCountries", "TerritoryOfIssueCodePickListDetailId", "dbo.PickListDetails");
            DropIndex("dbo.PerLifeRetroCountries", new[] { "Country" });
            DropIndex("dbo.PerLifeRetroCountries", new[] { "TerritoryOfIssueCodePickListDetailId" });
            AlterColumn("dbo.PerLifeRetroCountries", "Country", c => c.String(nullable: false, maxLength: 50));
            DropColumn("dbo.PerLifeRetroCountries", "TerritoryOfIssueCodePickListDetailId");
            CreateIndex("dbo.PerLifeRetroCountries", "Country");
        }
    }
}
