namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRiDataTable11 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.RiData", new[] { "PolicyNumber" });
            AlterColumn("dbo.RiData", "PolicyNumber", c => c.String(maxLength: 150));
            AlterColumn("dbo.RiData", "PolicyNumberOld", c => c.String(maxLength: 150));
            CreateIndex("dbo.RiData", "PolicyNumber");
        }
        
        public override void Down()
        {
            DropIndex("dbo.RiData", new[] { "PolicyNumber" });
            AlterColumn("dbo.RiData", "PolicyNumberOld", c => c.String(maxLength: 30));
            AlterColumn("dbo.RiData", "PolicyNumber", c => c.String(maxLength: 30));
            CreateIndex("dbo.RiData", "PolicyNumber");
        }
    }
}
