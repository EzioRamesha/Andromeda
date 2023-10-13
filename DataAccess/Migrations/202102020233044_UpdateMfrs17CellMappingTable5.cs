namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateMfrs17CellMappingTable5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Mfrs17CellMappings", "ProfitCommPickListDetailId", c => c.Int());
            CreateIndex("dbo.Mfrs17CellMappings", "ProfitCommPickListDetailId");
            AddForeignKey("dbo.Mfrs17CellMappings", "ProfitCommPickListDetailId", "dbo.PickListDetails", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Mfrs17CellMappings", "ProfitCommPickListDetailId", "dbo.PickListDetails");
            DropIndex("dbo.Mfrs17CellMappings", new[] { "ProfitCommPickListDetailId" });
            DropColumn("dbo.Mfrs17CellMappings", "ProfitCommPickListDetailId");
        }
    }
}
