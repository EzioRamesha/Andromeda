namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateMfrs17CellMappingDetailTable2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Mfrs17CellMappingDetails", "TreatyCode", c => c.String(maxLength: 35));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Mfrs17CellMappingDetails", "TreatyCode");
        }
    }
}
