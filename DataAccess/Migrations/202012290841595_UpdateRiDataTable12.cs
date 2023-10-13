namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRiDataTable12 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RiData", "WakalahFeePercentage", c => c.Double());
            AlterColumn("dbo.RiData", "PolicyTerm", c => c.Double());
            AlterColumn("dbo.RiData", "PolicyTermRemain", c => c.Double());
            CreateIndex("dbo.RiData", "AarShare2");
            CreateIndex("dbo.RiData", "AarCap2");
            CreateIndex("dbo.RiData", "WakalahFeePercentage");
        }
        
        public override void Down()
        {
            DropIndex("dbo.RiData", new[] { "WakalahFeePercentage" });
            DropIndex("dbo.RiData", new[] { "AarCap2" });
            DropIndex("dbo.RiData", new[] { "AarShare2" });
            AlterColumn("dbo.RiData", "PolicyTermRemain", c => c.Int());
            AlterColumn("dbo.RiData", "PolicyTerm", c => c.Int());
            DropColumn("dbo.RiData", "WakalahFeePercentage");
        }
    }
}
