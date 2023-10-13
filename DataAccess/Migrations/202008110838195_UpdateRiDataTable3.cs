namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRiDataTable3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.RiDataBatches", "TreatyCodeId", "dbo.TreatyCodes");
            DropIndex("dbo.RiDataBatches", new[] { "TreatyCodeId" });
            AddColumn("dbo.RiDataBatches", "TreatyId", c => c.Int());
            CreateIndex("dbo.RiDataBatches", "TreatyId");
            AddForeignKey("dbo.RiDataBatches", "TreatyId", "dbo.Treaties", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RiDataBatches", "TreatyId", "dbo.Treaties");
            DropIndex("dbo.RiDataBatches", new[] { "TreatyId" });
            DropColumn("dbo.RiDataBatches", "TreatyId");
            CreateIndex("dbo.RiDataBatches", "TreatyCodeId");
            AddForeignKey("dbo.RiDataBatches", "TreatyCodeId", "dbo.TreatyCodes", "Id");
        }
    }
}
