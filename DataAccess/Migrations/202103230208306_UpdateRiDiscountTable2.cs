namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRiDiscountTable2 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.RiDiscounts", new[] { "DurationFrom" });
            DropIndex("dbo.RiDiscounts", new[] { "DurationTo" });
            AlterColumn("dbo.RiDiscounts", "DurationFrom", c => c.Double());
            AlterColumn("dbo.RiDiscounts", "DurationTo", c => c.Double());
            CreateIndex("dbo.RiDiscounts", "DurationFrom");
            CreateIndex("dbo.RiDiscounts", "DurationTo");
        }
        
        public override void Down()
        {
            DropIndex("dbo.RiDiscounts", new[] { "DurationTo" });
            DropIndex("dbo.RiDiscounts", new[] { "DurationFrom" });
            AlterColumn("dbo.RiDiscounts", "DurationTo", c => c.Int());
            AlterColumn("dbo.RiDiscounts", "DurationFrom", c => c.Int());
            CreateIndex("dbo.RiDiscounts", "DurationTo");
            CreateIndex("dbo.RiDiscounts", "DurationFrom");
        }
    }
}
