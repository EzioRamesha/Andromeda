namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateAccountCodeMappingTable1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AccountCodeMappings", "Type", c => c.Int());
            AddColumn("dbo.AccountCodeMappings", "TreatyCodeId", c => c.Int());
            CreateIndex("dbo.AccountCodeMappings", "Type");
            CreateIndex("dbo.AccountCodeMappings", "TreatyCodeId");
            AddForeignKey("dbo.AccountCodeMappings", "TreatyCodeId", "dbo.TreatyCodes", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AccountCodeMappings", "TreatyCodeId", "dbo.TreatyCodes");
            DropIndex("dbo.AccountCodeMappings", new[] { "TreatyCodeId" });
            DropIndex("dbo.AccountCodeMappings", new[] { "Type" });
            DropColumn("dbo.AccountCodeMappings", "TreatyCodeId");
            DropColumn("dbo.AccountCodeMappings", "Type");
        }
    }
}
