namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTreatyPricingPerLifeRetroVersionBenefitTable2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TreatyPricingPerLifeRetroVersionBenefits", "RetrocessionnaireShare", c => c.String(maxLength: 128));
            AlterColumn("dbo.TreatyPricingPerLifeRetroVersionBenefits", "MaxExpiryAge", c => c.String(maxLength: 128));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TreatyPricingPerLifeRetroVersionBenefits", "MaxExpiryAge", c => c.Int());
            AlterColumn("dbo.TreatyPricingPerLifeRetroVersionBenefits", "RetrocessionnaireShare", c => c.Double());
        }
    }
}
