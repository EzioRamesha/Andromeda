namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateMfrs17CellMappingTable4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Mfrs17CellMappings", "RateTable", c => c.String(maxLength: 50));
            CreateIndex("dbo.Mfrs17CellMappings", "RateTable");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Mfrs17CellMappings", new[] { "RateTable" });
            DropColumn("dbo.Mfrs17CellMappings", "RateTable");
        }
    }
}
