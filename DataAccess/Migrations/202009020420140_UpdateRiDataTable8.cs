namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRiDataTable8 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.RiData", "Mfrs17TreatyCode", c => c.String(maxLength: 25));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.RiData", "Mfrs17TreatyCode", c => c.String(maxLength: 10));
        }
    }
}
