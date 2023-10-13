namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRateTableTable4 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.RateTables", new[] { "PolicyAmountFrom" });
            DropIndex("dbo.RateTables", new[] { "PolicyAmountTo" });
            DropIndex("dbo.RateTables", new[] { "AarFrom" });
            DropIndex("dbo.RateTables", new[] { "AarTo" });
            AlterColumn("dbo.RateTables", "PolicyAmountFrom", c => c.Double());
            AlterColumn("dbo.RateTables", "PolicyAmountTo", c => c.Double());
            AlterColumn("dbo.RateTables", "AarFrom", c => c.Double());
            AlterColumn("dbo.RateTables", "AarTo", c => c.Double());
            CreateIndex("dbo.RateTables", "PolicyAmountFrom");
            CreateIndex("dbo.RateTables", "PolicyAmountTo");
            CreateIndex("dbo.RateTables", "AarFrom");
            CreateIndex("dbo.RateTables", "AarTo");
        }
        
        public override void Down()
        {
            DropIndex("dbo.RateTables", new[] { "AarTo" });
            DropIndex("dbo.RateTables", new[] { "AarFrom" });
            DropIndex("dbo.RateTables", new[] { "PolicyAmountTo" });
            DropIndex("dbo.RateTables", new[] { "PolicyAmountFrom" });
            AlterColumn("dbo.RateTables", "AarTo", c => c.Int());
            AlterColumn("dbo.RateTables", "AarFrom", c => c.Int());
            AlterColumn("dbo.RateTables", "PolicyAmountTo", c => c.Int());
            AlterColumn("dbo.RateTables", "PolicyAmountFrom", c => c.Int());
            CreateIndex("dbo.RateTables", "AarTo");
            CreateIndex("dbo.RateTables", "AarFrom");
            CreateIndex("dbo.RateTables", "PolicyAmountTo");
            CreateIndex("dbo.RateTables", "PolicyAmountFrom");
        }
    }
}
