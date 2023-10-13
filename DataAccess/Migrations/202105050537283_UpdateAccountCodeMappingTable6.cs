namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateAccountCodeMappingTable6 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AccountCodeMappingDetails", "InvoiceField", c => c.String(maxLength: 50));
            AddColumn("dbo.AccountCodeMappings", "InvoiceField", c => c.String(storeType: "ntext"));
            CreateIndex("dbo.AccountCodeMappingDetails", "InvoiceField");
        }
        
        public override void Down()
        {
            DropIndex("dbo.AccountCodeMappingDetails", new[] { "InvoiceField" });
            DropColumn("dbo.AccountCodeMappings", "InvoiceField");
            DropColumn("dbo.AccountCodeMappingDetails", "InvoiceField");
        }
    }
}
