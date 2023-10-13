namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class UpdateLargeDiscountTable1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.LargeDiscounts", "EffectiveStartDate", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.LargeDiscounts", "EffectiveEndDate", c => c.DateTime(precision: 7, storeType: "datetime2"));
        }

        public override void Down()
        {
            AlterColumn("dbo.LargeDiscounts", "EffectiveEndDate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.LargeDiscounts", "EffectiveStartDate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
        }
    }
}
