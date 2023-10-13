namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTreatyPricingPerLifeRetroVersionsTable1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TreatyPricingPerLifeRetroVersions", "JumboLimitCurrencyCodePickListDetailId", c => c.Int());
            CreateIndex("dbo.TreatyPricingPerLifeRetroVersions", "JumboLimitCurrencyCodePickListDetailId");
            AddForeignKey("dbo.TreatyPricingPerLifeRetroVersions", "JumboLimitCurrencyCodePickListDetailId", "dbo.PickListDetails", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TreatyPricingPerLifeRetroVersions", "JumboLimitCurrencyCodePickListDetailId", "dbo.PickListDetails");
            DropIndex("dbo.TreatyPricingPerLifeRetroVersions", new[] { "JumboLimitCurrencyCodePickListDetailId" });
            DropColumn("dbo.TreatyPricingPerLifeRetroVersions", "JumboLimitCurrencyCodePickListDetailId");
        }
    }
}
