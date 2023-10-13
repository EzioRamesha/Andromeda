namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRiDataFilesTable1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.RiDataFiles", "TreatyCodeId", "dbo.TreatyCodes");
            DropIndex("dbo.RiDataFiles", new[] { "TreatyCodeId" });
            AddColumn("dbo.RiDataFiles", "TreatyId", c => c.Int());
            CreateIndex("dbo.RiDataFiles", "TreatyId");
            AddForeignKey("dbo.RiDataFiles", "TreatyId", "dbo.Treaties", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RiDataFiles", "TreatyId", "dbo.Treaties");
            DropIndex("dbo.RiDataFiles", new[] { "TreatyId" });
            DropColumn("dbo.RiDataFiles", "TreatyId");
            CreateIndex("dbo.RiDataFiles", "TreatyCodeId");
            AddForeignKey("dbo.RiDataFiles", "TreatyCodeId", "dbo.TreatyCodes", "Id");
        }
    }
}
