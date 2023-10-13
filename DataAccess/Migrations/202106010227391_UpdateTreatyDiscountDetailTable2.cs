namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTreatyDiscountDetailTable2 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.TreatyDiscountTableDetails", new[] { "AARFrom" });
            DropIndex("dbo.TreatyDiscountTableDetails", new[] { "AARTo" });
            AlterColumn("dbo.TreatyDiscountTableDetails", "AARFrom", c => c.Double());
            AlterColumn("dbo.TreatyDiscountTableDetails", "AARTo", c => c.Double());
            CreateIndex("dbo.TreatyDiscountTableDetails", "AARFrom");
            CreateIndex("dbo.TreatyDiscountTableDetails", "AARTo");
        }
        
        public override void Down()
        {
            DropIndex("dbo.TreatyDiscountTableDetails", new[] { "AARTo" });
            DropIndex("dbo.TreatyDiscountTableDetails", new[] { "AARFrom" });
            AlterColumn("dbo.TreatyDiscountTableDetails", "AARTo", c => c.Int());
            AlterColumn("dbo.TreatyDiscountTableDetails", "AARFrom", c => c.Int());
            CreateIndex("dbo.TreatyDiscountTableDetails", "AARTo");
            CreateIndex("dbo.TreatyDiscountTableDetails", "AARFrom");
        }
    }
}
