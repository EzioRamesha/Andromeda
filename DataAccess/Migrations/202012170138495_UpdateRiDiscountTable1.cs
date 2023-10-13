namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class UpdateRiDiscountTable1 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.RiDiscounts", new[] { "DurationFrom" });
            DropIndex("dbo.RiDiscounts", new[] { "DurationTo" });
            AlterColumn("dbo.RiDiscounts", "EffectiveStartDate", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.RiDiscounts", "EffectiveEndDate", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.RiDiscounts", "DurationFrom", c => c.Int());
            AlterColumn("dbo.RiDiscounts", "DurationTo", c => c.Int());
            CreateIndex("dbo.RiDiscounts", "DurationFrom");
            CreateIndex("dbo.RiDiscounts", "DurationTo");
        }

        public override void Down()
        {
            DropIndex("dbo.RiDiscounts", new[] { "DurationTo" });
            DropIndex("dbo.RiDiscounts", new[] { "DurationFrom" });
            AlterColumn("dbo.RiDiscounts", "DurationTo", c => c.Int(nullable: false));
            AlterColumn("dbo.RiDiscounts", "DurationFrom", c => c.Int(nullable: false));
            AlterColumn("dbo.RiDiscounts", "EffectiveEndDate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.RiDiscounts", "EffectiveStartDate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            CreateIndex("dbo.RiDiscounts", "DurationTo");
            CreateIndex("dbo.RiDiscounts", "DurationFrom");
        }
    }
}
