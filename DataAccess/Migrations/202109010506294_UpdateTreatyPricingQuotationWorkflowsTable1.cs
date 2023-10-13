namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTreatyPricingQuotationWorkflowsTable1 : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.TreatyPricingQuotationWorkflows", "BDPersonInChargeId");
            CreateIndex("dbo.TreatyPricingQuotationWorkflows", "PersonInChargeId");
            AddForeignKey("dbo.TreatyPricingQuotationWorkflows", "BDPersonInChargeId", "dbo.Users", "Id");
            AddForeignKey("dbo.TreatyPricingQuotationWorkflows", "PersonInChargeId", "dbo.Users", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TreatyPricingQuotationWorkflows", "PersonInChargeId", "dbo.Users");
            DropForeignKey("dbo.TreatyPricingQuotationWorkflows", "BDPersonInChargeId", "dbo.Users");
            DropIndex("dbo.TreatyPricingQuotationWorkflows", new[] { "PersonInChargeId" });
            DropIndex("dbo.TreatyPricingQuotationWorkflows", new[] { "BDPersonInChargeId" });
        }
    }
}
