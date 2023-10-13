namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateInvoiceRegisterHistoryTable7 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.InvoiceRegisterHistories", new[] { "InvoiceReference" });
            AlterColumn("dbo.InvoiceRegisterHistories", "InvoiceReference", c => c.String(maxLength: 30));
            CreateIndex("dbo.InvoiceRegisterHistories", "InvoiceReference");
        }
        
        public override void Down()
        {
            DropIndex("dbo.InvoiceRegisterHistories", new[] { "InvoiceReference" });
            AlterColumn("dbo.InvoiceRegisterHistories", "InvoiceReference", c => c.String(nullable: false, maxLength: 30));
            CreateIndex("dbo.InvoiceRegisterHistories", "InvoiceReference");
        }
    }
}
