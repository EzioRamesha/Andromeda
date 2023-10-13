namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateGroupDiscountTable1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.GroupDiscounts", "EffectiveStartDate", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.GroupDiscounts", "EffectiveEndDate", c => c.DateTime(precision: 7, storeType: "datetime2"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.GroupDiscounts", "EffectiveEndDate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.GroupDiscounts", "EffectiveStartDate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
        }
    }
}
