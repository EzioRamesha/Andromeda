namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateItemCodeTable1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ItemCodes", "BusinessOriginPickListDetailId", c => c.Int());
            CreateIndex("dbo.ItemCodes", "BusinessOriginPickListDetailId");
            AddForeignKey("dbo.ItemCodes", "BusinessOriginPickListDetailId", "dbo.PickListDetails", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ItemCodes", "BusinessOriginPickListDetailId", "dbo.PickListDetails");
            DropIndex("dbo.ItemCodes", new[] { "BusinessOriginPickListDetailId" });
            DropColumn("dbo.ItemCodes", "BusinessOriginPickListDetailId");
        }
    }
}
