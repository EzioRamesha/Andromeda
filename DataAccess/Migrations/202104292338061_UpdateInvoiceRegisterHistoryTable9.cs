namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateInvoiceRegisterHistoryTable9 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InvoiceRegisterHistories", "ReportingType", c => c.Int(nullable: false));
            AlterColumn("dbo.InvoiceRegisterHistories", "PaymentAmount", c => c.String(maxLength: 255));
            CreateIndex("dbo.InvoiceRegisterHistories", "ReportingType");
        }
        
        public override void Down()
        {
            DropIndex("dbo.InvoiceRegisterHistories", new[] { "ReportingType" });
            AlterColumn("dbo.InvoiceRegisterHistories", "PaymentAmount", c => c.Double());
            DropColumn("dbo.InvoiceRegisterHistories", "ReportingType");
        }
    }
}
