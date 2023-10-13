namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateAccountCodeMappingTable5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AccountCodeMappings", "ReportType", c => c.Int(nullable: false));
            AddColumn("dbo.AccountCodeMappings", "ModifiedContractCodeId", c => c.Int());
            CreateIndex("dbo.AccountCodeMappings", "ReportType");
            CreateIndex("dbo.AccountCodeMappings", "ModifiedContractCodeId");
            AddForeignKey("dbo.AccountCodeMappings", "ModifiedContractCodeId", "dbo.Mfrs17ContractCodes", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AccountCodeMappings", "ModifiedContractCodeId", "dbo.Mfrs17ContractCodes");
            DropIndex("dbo.AccountCodeMappings", new[] { "ModifiedContractCodeId" });
            DropIndex("dbo.AccountCodeMappings", new[] { "ReportType" });
            DropColumn("dbo.AccountCodeMappings", "ModifiedContractCodeId");
            DropColumn("dbo.AccountCodeMappings", "ReportType");
        }
    }
}
