namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRiDataTable16 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.RiData", "LoaCode", c => c.String(maxLength: 20));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.RiData", "LoaCode", c => c.String(maxLength: 10));
        }
    }
}
