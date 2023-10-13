namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTreatyCodeTable4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TreatyCodes", "LineOfBusinessPickListDetailId", c => c.Int());
            CreateIndex("dbo.TreatyCodes", "LineOfBusinessPickListDetailId");
            AddForeignKey("dbo.TreatyCodes", "LineOfBusinessPickListDetailId", "dbo.PickListDetails", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TreatyCodes", "LineOfBusinessPickListDetailId", "dbo.PickListDetails");
            DropIndex("dbo.TreatyCodes", new[] { "LineOfBusinessPickListDetailId" });
            DropColumn("dbo.TreatyCodes", "LineOfBusinessPickListDetailId");
        }
    }
}
