namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRateTableDetailTable1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RateTableDetails", "TreatyCode", c => c.String(maxLength: 35));
            AddColumn("dbo.RateTableDetails", "CedingPlanCode", c => c.String(maxLength: 30));
            CreateIndex("dbo.RateTableDetails", "TreatyCode");
            CreateIndex("dbo.RateTableDetails", "CedingPlanCode");
        }
        
        public override void Down()
        {
            DropIndex("dbo.RateTableDetails", new[] { "CedingPlanCode" });
            DropIndex("dbo.RateTableDetails", new[] { "TreatyCode" });
            DropColumn("dbo.RateTableDetails", "CedingPlanCode");
            DropColumn("dbo.RateTableDetails", "TreatyCode");
        }
    }
}
