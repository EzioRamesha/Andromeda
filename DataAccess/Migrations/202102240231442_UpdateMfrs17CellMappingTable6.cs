namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateMfrs17CellMappingTable6 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Mfrs17CellMappings", "LoaCode", c => c.String(maxLength: 20));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Mfrs17CellMappings", "LoaCode", c => c.String(maxLength: 10));
        }
    }
}
