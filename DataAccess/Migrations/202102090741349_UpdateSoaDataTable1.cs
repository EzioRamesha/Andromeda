namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateSoaDataTable1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SoaData", "RiskPremium", c => c.Double());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SoaData", "RiskPremium", c => c.Int());
        }
    }
}
