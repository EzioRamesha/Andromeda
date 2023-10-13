namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRiDataTable17 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.RiData", "EffectiveDate", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.RiData", "OfferLetterSentDate", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.RiData", "RiskPeriodStartDate", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.RiData", "RiskPeriodEndDate", c => c.DateTime(precision: 7, storeType: "datetime2"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.RiData", "RiskPeriodEndDate", c => c.DateTime());
            AlterColumn("dbo.RiData", "RiskPeriodStartDate", c => c.DateTime());
            AlterColumn("dbo.RiData", "OfferLetterSentDate", c => c.DateTime());
            AlterColumn("dbo.RiData", "EffectiveDate", c => c.DateTime());
        }
    }
}
