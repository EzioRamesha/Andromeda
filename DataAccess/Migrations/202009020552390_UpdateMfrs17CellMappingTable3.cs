namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateMfrs17CellMappingTable3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Mfrs17CellMappings", "FundCodePickListDetailId", "dbo.PickListDetails");
            DropIndex("dbo.Mfrs17CellMappings", new[] { "FundCodePickListDetailId" });
            AddColumn("dbo.Mfrs17CellMappings", "Mfrs17TreatyCode", c => c.String(nullable: false, maxLength: 25));
            CreateIndex("dbo.Mfrs17CellMappings", "Mfrs17TreatyCode");
            DropColumn("dbo.Mfrs17CellMappings", "FundCodePickListDetailId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Mfrs17CellMappings", "FundCodePickListDetailId", c => c.Int());
            DropIndex("dbo.Mfrs17CellMappings", new[] { "Mfrs17TreatyCode" });
            DropColumn("dbo.Mfrs17CellMappings", "Mfrs17TreatyCode");
            CreateIndex("dbo.Mfrs17CellMappings", "FundCodePickListDetailId");
            AddForeignKey("dbo.Mfrs17CellMappings", "FundCodePickListDetailId", "dbo.PickListDetails", "Id");
        }
    }
}
