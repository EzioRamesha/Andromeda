namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTreatyPricingPerLifeRetroVersionBenefitTable1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TreatyPricingPerLifeRetroVersionBenefits", "MlreRetention", c => c.String());
            AlterColumn("dbo.TreatyPricingPerLifeRetroVersionBenefits", "RateTablePercentage", c => c.String());
            AlterColumn("dbo.TreatyPricingPerLifeRetroVersionBenefits", "ClaimApprovalLimit", c => c.String());
            AlterColumn("dbo.TreatyPricingPerLifeRetroVersionBenefits", "AutoBindingLimit", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TreatyPricingPerLifeRetroVersionBenefits", "AutoBindingLimit", c => c.Double());
            AlterColumn("dbo.TreatyPricingPerLifeRetroVersionBenefits", "ClaimApprovalLimit", c => c.Double());
            AlterColumn("dbo.TreatyPricingPerLifeRetroVersionBenefits", "RateTablePercentage", c => c.Double());
            AlterColumn("dbo.TreatyPricingPerLifeRetroVersionBenefits", "MlreRetention", c => c.Double());
        }
    }
}
