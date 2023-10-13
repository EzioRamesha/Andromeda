namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRiDataConfigTable1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.RiDataConfigs", "TreatyCodeId", "dbo.TreatyCodes");
            DropIndex("dbo.RiDataConfigs", new[] { "TreatyCodeId" });
            AddColumn("dbo.RiDataConfigs", "TreatyId", c => c.Int());
            CreateIndex("dbo.RiDataConfigs", "TreatyId");
            AddForeignKey("dbo.RiDataConfigs", "TreatyId", "dbo.Treaties", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RiDataConfigs", "TreatyId", "dbo.Treaties");
            DropIndex("dbo.RiDataConfigs", new[] { "TreatyId" });
            DropColumn("dbo.RiDataConfigs", "TreatyId");
            CreateIndex("dbo.RiDataConfigs", "TreatyCodeId");
            AddForeignKey("dbo.RiDataConfigs", "TreatyCodeId", "dbo.TreatyCodes", "Id");
        }
    }
}
