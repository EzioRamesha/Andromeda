namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class UpdateAccountCodeMappingTable4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AccountCodeMappings", "RetroRegisterFieldPickListDetailId", c => c.Int());
            CreateIndex("dbo.AccountCodeMappings", "RetroRegisterFieldPickListDetailId");
            AddForeignKey("dbo.AccountCodeMappings", "RetroRegisterFieldPickListDetailId", "dbo.PickListDetails", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.AccountCodeMappings", "RetroRegisterFieldPickListDetailId", "dbo.PickListDetails");
            DropIndex("dbo.AccountCodeMappings", new[] { "RetroRegisterFieldPickListDetailId" });
            DropColumn("dbo.AccountCodeMappings", "RetroRegisterFieldPickListDetailId");
        }
    }
}
