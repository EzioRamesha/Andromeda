namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateItemCodeMappingTable2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ItemCodeMappings", "BusinessOriginPickListDetailId", c => c.Int());
            CreateIndex("dbo.ItemCodeMappings", "BusinessOriginPickListDetailId");
            AddForeignKey("dbo.ItemCodeMappings", "BusinessOriginPickListDetailId", "dbo.PickListDetails", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ItemCodeMappings", "BusinessOriginPickListDetailId", "dbo.PickListDetails");
            DropIndex("dbo.ItemCodeMappings", new[] { "BusinessOriginPickListDetailId" });
            DropColumn("dbo.ItemCodeMappings", "BusinessOriginPickListDetailId");
        }
    }
}
