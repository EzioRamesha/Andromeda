namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTreatyPricingQuotationWorkflowVersionTable1 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.TreatyPricingQuotationWorkflowVersions", new[] { "PendingOnDate" });
            AddColumn("dbo.TreatyPricingQuotationWorkflowVersions", "PendingOn", c => c.String(maxLength: 255));
            CreateIndex("dbo.TreatyPricingQuotationWorkflowVersions", "PendingOn");
            DropColumn("dbo.TreatyPricingQuotationWorkflowVersions", "PendingOnDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TreatyPricingQuotationWorkflowVersions", "PendingOnDate", c => c.DateTime(precision: 7, storeType: "datetime2"));
            DropIndex("dbo.TreatyPricingQuotationWorkflowVersions", new[] { "PendingOn" });
            DropColumn("dbo.TreatyPricingQuotationWorkflowVersions", "PendingOn");
            CreateIndex("dbo.TreatyPricingQuotationWorkflowVersions", "PendingOnDate");
        }
    }
}
