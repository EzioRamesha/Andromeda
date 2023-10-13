namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTreatyPricingQuotationWorkflowVersionTable2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TreatyPricingQuotationWorkflowVersions", "ExpenseMargin", c => c.Double());
            CreateIndex("dbo.TreatyPricingQuotationWorkflowVersions", "ExpenseMargin");
        }
        
        public override void Down()
        {
            DropIndex("dbo.TreatyPricingQuotationWorkflowVersions", new[] { "ExpenseMargin" });
            DropColumn("dbo.TreatyPricingQuotationWorkflowVersions", "ExpenseMargin");
        }
    }
}
