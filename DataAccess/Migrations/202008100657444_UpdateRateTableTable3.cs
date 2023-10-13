namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRateTableTable3 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.RateTables", new[] { "RateTableCode" });
            AlterColumn("dbo.RateTables", "RateTableCode", c => c.String(nullable: false, maxLength: 50));
            CreateIndex("dbo.RateTables", "RateTableCode");
        }
        
        public override void Down()
        {
            DropIndex("dbo.RateTables", new[] { "RateTableCode" });
            AlterColumn("dbo.RateTables", "RateTableCode", c => c.String(nullable: false, maxLength: 64));
            CreateIndex("dbo.RateTables", "RateTableCode");
        }
    }
}
