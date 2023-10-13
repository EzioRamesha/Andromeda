namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTreatyPricingPerLifeRetroVersionsTable2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TreatyPricingPerLifeRetroVersions", "TerminationPeriod", c => c.String(maxLength: 128));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TreatyPricingPerLifeRetroVersions", "TerminationPeriod", c => c.Int());
        }
    }
}
