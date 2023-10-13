namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRateDetailTable1 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.RateDetails", new[] { "PolicyTerm" });
            DropIndex("dbo.RateDetails", new[] { "PolicyTermRemain" });
            AlterColumn("dbo.RateDetails", "PolicyTerm", c => c.Double());
            AlterColumn("dbo.RateDetails", "PolicyTermRemain", c => c.Double());
            CreateIndex("dbo.RateDetails", "PolicyTerm");
            CreateIndex("dbo.RateDetails", "PolicyTermRemain");
        }
        
        public override void Down()
        {
            DropIndex("dbo.RateDetails", new[] { "PolicyTermRemain" });
            DropIndex("dbo.RateDetails", new[] { "PolicyTerm" });
            AlterColumn("dbo.RateDetails", "PolicyTermRemain", c => c.Int());
            AlterColumn("dbo.RateDetails", "PolicyTerm", c => c.Int());
            CreateIndex("dbo.RateDetails", "PolicyTermRemain");
            CreateIndex("dbo.RateDetails", "PolicyTerm");
        }
    }
}
