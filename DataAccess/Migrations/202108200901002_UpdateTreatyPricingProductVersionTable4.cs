namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTreatyPricingProductVersionTable4 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TreatyPricingProductVersions", "OccupationalClassification", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TreatyPricingProductVersions", "OccupationalClassification", c => c.String(maxLength: 128));
        }
    }
}
