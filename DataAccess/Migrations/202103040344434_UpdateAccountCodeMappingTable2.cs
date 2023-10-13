namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateAccountCodeMappingTable2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AccountCodeMappings", "ClaimCodeId", "dbo.ClaimCodes");
            DropForeignKey("dbo.AccountCodeMappings", "TreatyTypePickListDetailId", "dbo.PickListDetails");
            DropIndex("dbo.AccountCodeMappings", new[] { "TreatyTypePickListDetailId" });
            DropIndex("dbo.AccountCodeMappings", new[] { "ClaimCodeId" });
            AddColumn("dbo.AccountCodeMappings", "TreatyType", c => c.String(storeType: "ntext"));
            AddColumn("dbo.AccountCodeMappings", "ClaimCode", c => c.String(storeType: "ntext"));
            DropColumn("dbo.AccountCodeMappings", "TreatyTypePickListDetailId");
            DropColumn("dbo.AccountCodeMappings", "ClaimCodeId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AccountCodeMappings", "ClaimCodeId", c => c.Int());
            AddColumn("dbo.AccountCodeMappings", "TreatyTypePickListDetailId", c => c.Int(nullable: false));
            DropColumn("dbo.AccountCodeMappings", "ClaimCode");
            DropColumn("dbo.AccountCodeMappings", "TreatyType");
            CreateIndex("dbo.AccountCodeMappings", "ClaimCodeId");
            CreateIndex("dbo.AccountCodeMappings", "TreatyTypePickListDetailId");
            AddForeignKey("dbo.AccountCodeMappings", "TreatyTypePickListDetailId", "dbo.PickListDetails", "Id");
            AddForeignKey("dbo.AccountCodeMappings", "ClaimCodeId", "dbo.ClaimCodes", "Id");
        }
    }
}
