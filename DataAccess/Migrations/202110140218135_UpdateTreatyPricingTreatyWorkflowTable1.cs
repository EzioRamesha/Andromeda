namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTreatyPricingTreatyWorkflowTable1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TreatyPricingTreatyWorkflows", "BusinessTypePickListDetailId", "dbo.PickListDetails");
            DropIndex("dbo.TreatyPricingTreatyWorkflows", new[] { "BusinessTypePickListDetailId" });
            AddColumn("dbo.TreatyPricingTreatyWorkflows", "TypeOfBusiness", c => c.String());
            AddColumn("dbo.TreatyPricingTreatyWorkflows", "CountryOrigin", c => c.String());
            DropColumn("dbo.TreatyPricingTreatyWorkflows", "BusinessTypePickListDetailId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TreatyPricingTreatyWorkflows", "BusinessTypePickListDetailId", c => c.Int());
            DropColumn("dbo.TreatyPricingTreatyWorkflows", "CountryOrigin");
            DropColumn("dbo.TreatyPricingTreatyWorkflows", "TypeOfBusiness");
            CreateIndex("dbo.TreatyPricingTreatyWorkflows", "BusinessTypePickListDetailId");
            AddForeignKey("dbo.TreatyPricingTreatyWorkflows", "BusinessTypePickListDetailId", "dbo.PickListDetails", "Id");
        }
    }
}
