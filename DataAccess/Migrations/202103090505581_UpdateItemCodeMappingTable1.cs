namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateItemCodeMappingTable1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ItemCodeMappings", "StandardSoaDataOutputId", "dbo.StandardSoaDataOutputs");
            DropForeignKey("dbo.ItemCodeMappings", "TreatyTypePickListDetailId", "dbo.PickListDetails");
            DropIndex("dbo.ItemCodeMappings", new[] { "TreatyTypePickListDetailId" });
            DropIndex("dbo.ItemCodeMappings", new[] { "StandardSoaDataOutputId" });
            AddColumn("dbo.ItemCodeMappings", "TreatyType", c => c.String(storeType: "ntext"));
            AddColumn("dbo.ItemCodeMappings", "TreatyCode", c => c.String(storeType: "ntext"));
            DropColumn("dbo.ItemCodeMappings", "TreatyTypePickListDetailId");
            DropColumn("dbo.ItemCodeMappings", "StandardSoaDataOutputId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ItemCodeMappings", "StandardSoaDataOutputId", c => c.Int());
            AddColumn("dbo.ItemCodeMappings", "TreatyTypePickListDetailId", c => c.Int(nullable: false));
            DropColumn("dbo.ItemCodeMappings", "TreatyCode");
            DropColumn("dbo.ItemCodeMappings", "TreatyType");
            CreateIndex("dbo.ItemCodeMappings", "StandardSoaDataOutputId");
            CreateIndex("dbo.ItemCodeMappings", "TreatyTypePickListDetailId");
            AddForeignKey("dbo.ItemCodeMappings", "TreatyTypePickListDetailId", "dbo.PickListDetails", "Id");
            AddForeignKey("dbo.ItemCodeMappings", "StandardSoaDataOutputId", "dbo.StandardSoaDataOutputs", "Id");
        }
    }
}
