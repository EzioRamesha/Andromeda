namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTreatyDiscountTableDetailTable1 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.TreatyDiscountTableDetails", new[] { "AgeFrom" });
            DropIndex("dbo.TreatyDiscountTableDetails", new[] { "AgeTo" });
            AlterColumn("dbo.TreatyDiscountTableDetails", "CedingPlanCode", c => c.String(nullable: false, storeType: "ntext"));
            AlterColumn("dbo.TreatyDiscountTableDetails", "AgeFrom", c => c.Int());
            AlterColumn("dbo.TreatyDiscountTableDetails", "AgeTo", c => c.Int());
            CreateIndex("dbo.TreatyDiscountTableDetails", "AgeFrom");
            CreateIndex("dbo.TreatyDiscountTableDetails", "AgeTo");
        }
        
        public override void Down()
        {
            DropIndex("dbo.TreatyDiscountTableDetails", new[] { "AgeTo" });
            DropIndex("dbo.TreatyDiscountTableDetails", new[] { "AgeFrom" });
            AlterColumn("dbo.TreatyDiscountTableDetails", "AgeTo", c => c.Int(nullable: false));
            AlterColumn("dbo.TreatyDiscountTableDetails", "AgeFrom", c => c.Int(nullable: false));
            AlterColumn("dbo.TreatyDiscountTableDetails", "CedingPlanCode", c => c.String(nullable: false, maxLength: 30));
            CreateIndex("dbo.TreatyDiscountTableDetails", "AgeTo");
            CreateIndex("dbo.TreatyDiscountTableDetails", "AgeFrom");
        }
    }
}
