namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTreatyPricingQuotationWorkflowVersionQuotationChecklistTable1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TreatyPricingQuotationWorkflows", "CEOPending", c => c.Int(nullable: false));
            AlterColumn("dbo.TreatyPricingQuotationWorkflows", "PricingPending", c => c.Int(nullable: false));
            AlterColumn("dbo.TreatyPricingQuotationWorkflows", "UnderwritingPending", c => c.Int(nullable: false));
            AlterColumn("dbo.TreatyPricingQuotationWorkflows", "HealthPending", c => c.Int(nullable: false));
            AlterColumn("dbo.TreatyPricingQuotationWorkflows", "ClaimsPending", c => c.Int(nullable: false));
            AlterColumn("dbo.TreatyPricingQuotationWorkflows", "BDPending", c => c.Int(nullable: false));
            AlterColumn("dbo.TreatyPricingQuotationWorkflows", "TGPending", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TreatyPricingQuotationWorkflows", "TGPending", c => c.Boolean(nullable: false));
            AlterColumn("dbo.TreatyPricingQuotationWorkflows", "BDPending", c => c.Boolean(nullable: false));
            AlterColumn("dbo.TreatyPricingQuotationWorkflows", "ClaimsPending", c => c.Boolean(nullable: false));
            AlterColumn("dbo.TreatyPricingQuotationWorkflows", "HealthPending", c => c.Boolean(nullable: false));
            AlterColumn("dbo.TreatyPricingQuotationWorkflows", "UnderwritingPending", c => c.Boolean(nullable: false));
            AlterColumn("dbo.TreatyPricingQuotationWorkflows", "PricingPending", c => c.Boolean(nullable: false));
            AlterColumn("dbo.TreatyPricingQuotationWorkflows", "CEOPending", c => c.Boolean(nullable: false));
        }
    }
}
