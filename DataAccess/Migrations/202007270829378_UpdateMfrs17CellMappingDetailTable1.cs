namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateMfrs17CellMappingDetailTable1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Mfrs17CellMappingDetails", "CedingPlanCode", c => c.String(maxLength: 30));
            AddColumn("dbo.Mfrs17CellMappingDetails", "BenefitCode", c => c.String(maxLength: 10));
            CreateIndex("dbo.Mfrs17CellMappingDetails", "CedingPlanCode");
            CreateIndex("dbo.Mfrs17CellMappingDetails", "BenefitCode");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Mfrs17CellMappingDetails", new[] { "BenefitCode" });
            DropIndex("dbo.Mfrs17CellMappingDetails", new[] { "CedingPlanCode" });
            DropColumn("dbo.Mfrs17CellMappingDetails", "BenefitCode");
            DropColumn("dbo.Mfrs17CellMappingDetails", "CedingPlanCode");
        }
    }
}
