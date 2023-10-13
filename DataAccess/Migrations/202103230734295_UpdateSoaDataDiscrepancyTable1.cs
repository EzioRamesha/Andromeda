namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateSoaDataDiscrepancyTable1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SoaDataDiscrepancies", "CurrencyCode", c => c.String(maxLength: 3));
            AddColumn("dbo.SoaDataDiscrepancies", "CurrencyRate", c => c.Double());
            CreateIndex("dbo.SoaDataDiscrepancies", "CurrencyCode");
        }
        
        public override void Down()
        {
            DropIndex("dbo.SoaDataDiscrepancies", new[] { "CurrencyCode" });
            DropColumn("dbo.SoaDataDiscrepancies", "CurrencyRate");
            DropColumn("dbo.SoaDataDiscrepancies", "CurrencyCode");
        }
    }
}
