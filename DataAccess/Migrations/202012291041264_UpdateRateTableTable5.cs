namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRateTableTable5 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.RateTables", new[] { "PolicyTermFrom" });
            DropIndex("dbo.RateTables", new[] { "PolicyTermTo" });
            AlterColumn("dbo.RateTables", "PolicyTermFrom", c => c.Double());
            AlterColumn("dbo.RateTables", "PolicyTermTo", c => c.Double());
            CreateIndex("dbo.RateTables", "PolicyTermFrom");
            CreateIndex("dbo.RateTables", "PolicyTermTo");
        }
        
        public override void Down()
        {
            DropIndex("dbo.RateTables", new[] { "PolicyTermTo" });
            DropIndex("dbo.RateTables", new[] { "PolicyTermFrom" });
            AlterColumn("dbo.RateTables", "PolicyTermTo", c => c.Int());
            AlterColumn("dbo.RateTables", "PolicyTermFrom", c => c.Int());
            CreateIndex("dbo.RateTables", "PolicyTermTo");
            CreateIndex("dbo.RateTables", "PolicyTermFrom");
        }
    }
}
