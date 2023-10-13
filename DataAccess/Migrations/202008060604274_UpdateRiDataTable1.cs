namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRiDataTable1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RiData", "IgnoreFinalise", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RiData", "IgnoreFinalise");
        }
    }
}
