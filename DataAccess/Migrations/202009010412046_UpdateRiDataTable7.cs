namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRiDataTable7 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.RiData", "FundCode", c => c.String(maxLength: 25));
            AlterColumn("dbo.RiData", "Mfrs17CellName", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.RiData", "Mfrs17CellName", c => c.String(maxLength: 30));
            AlterColumn("dbo.RiData", "FundCode", c => c.String(maxLength: 5));
        }
    }
}
