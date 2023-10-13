namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTreatyPricingProductVersionTable3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TreatyPricingProductVersions", "OccupationalClassification", c => c.String(maxLength: 128));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TreatyPricingProductVersions", "OccupationalClassification");
        }
    }
}
