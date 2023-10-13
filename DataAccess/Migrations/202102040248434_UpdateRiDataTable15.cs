namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRiDataTable15 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RiData", "TreatyNumber", c => c.String(maxLength: 128));
            CreateIndex("dbo.RiData", "TreatyNumber");
        }
        
        public override void Down()
        {
            DropIndex("dbo.RiData", new[] { "TreatyNumber" });
            DropColumn("dbo.RiData", "TreatyNumber");
        }
    }
}
