namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateMfrs17CellMappingTable1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Mfrs17CellMappings", "TreatyCodeId", "dbo.TreatyCodes");
            DropIndex("dbo.Mfrs17CellMappings", new[] { "TreatyCodeId" });
            DropIndex("dbo.Mfrs17CellMappings", new[] { "CedingPlanCode" });
            DropIndex("dbo.Mfrs17CellMappings", new[] { "BenefitCode" });
            AddColumn("dbo.Mfrs17CellMappings", "TreatyCode", c => c.String(nullable: false, storeType: "ntext"));
            AlterColumn("dbo.Mfrs17CellMappings", "TreatyCodeId", c => c.Int());
            AlterColumn("dbo.Mfrs17CellMappings", "CedingPlanCode", c => c.String(storeType: "ntext"));
            AlterColumn("dbo.Mfrs17CellMappings", "BenefitCode", c => c.String(storeType: "ntext"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Mfrs17CellMappings", "BenefitCode", c => c.String(maxLength: 255));
            AlterColumn("dbo.Mfrs17CellMappings", "CedingPlanCode", c => c.String(maxLength: 255));
            AlterColumn("dbo.Mfrs17CellMappings", "TreatyCodeId", c => c.Int(nullable: false));
            DropColumn("dbo.Mfrs17CellMappings", "TreatyCode");
            CreateIndex("dbo.Mfrs17CellMappings", "BenefitCode");
            CreateIndex("dbo.Mfrs17CellMappings", "CedingPlanCode");
            CreateIndex("dbo.Mfrs17CellMappings", "TreatyCodeId");
            AddForeignKey("dbo.Mfrs17CellMappings", "TreatyCodeId", "dbo.TreatyCodes", "Id");
        }
    }
}
