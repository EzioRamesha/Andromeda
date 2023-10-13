namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTreatyPricingTreatyWorkflowTable2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.TreatyPricingTreatyWorkflows", "OrionGroup");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TreatyPricingTreatyWorkflows", "OrionGroup", c => c.Double());
        }
    }
}
