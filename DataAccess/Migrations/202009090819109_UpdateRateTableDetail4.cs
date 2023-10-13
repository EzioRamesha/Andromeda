namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRateTableDetail4 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.RateTableDetails", new[] { "CedingPlanCode2" });
            AlterColumn("dbo.RateTableDetails", "CedingPlanCode2", c => c.String(maxLength: 30));
            CreateIndex("dbo.RateTableDetails", "CedingPlanCode2");
        }
        
        public override void Down()
        {
            DropIndex("dbo.RateTableDetails", new[] { "CedingPlanCode2" });
            AlterColumn("dbo.RateTableDetails", "CedingPlanCode2", c => c.String(maxLength: 10));
            CreateIndex("dbo.RateTableDetails", "CedingPlanCode2");
        }
    }
}
