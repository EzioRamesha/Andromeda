namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateMfrs17CellMappingTable2 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Mfrs17CellMappings", new[] { "CellName" });
            AddColumn("dbo.Mfrs17CellMappings", "FundCodePickListDetailId", c => c.Int());
            AlterColumn("dbo.Mfrs17CellMappings", "CellName", c => c.String(nullable: false, maxLength: 50));
            CreateIndex("dbo.Mfrs17CellMappings", "CellName");
            CreateIndex("dbo.Mfrs17CellMappings", "FundCodePickListDetailId");
            AddForeignKey("dbo.Mfrs17CellMappings", "FundCodePickListDetailId", "dbo.PickListDetails", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Mfrs17CellMappings", "FundCodePickListDetailId", "dbo.PickListDetails");
            DropIndex("dbo.Mfrs17CellMappings", new[] { "FundCodePickListDetailId" });
            DropIndex("dbo.Mfrs17CellMappings", new[] { "CellName" });
            AlterColumn("dbo.Mfrs17CellMappings", "CellName", c => c.String(nullable: false, maxLength: 64));
            DropColumn("dbo.Mfrs17CellMappings", "FundCodePickListDetailId");
            CreateIndex("dbo.Mfrs17CellMappings", "CellName");
        }
    }
}
