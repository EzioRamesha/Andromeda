namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateInvoiceRegisterTable7 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InvoiceRegister", "ReportingType", c => c.Int(nullable: false));
            AlterColumn("dbo.InvoiceRegister", "PaymentAmount", c => c.String(maxLength: 255));
            CreateIndex("dbo.InvoiceRegister", "ReportingType");
        }
        
        public override void Down()
        {
            DropIndex("dbo.InvoiceRegister", new[] { "ReportingType" });
            AlterColumn("dbo.InvoiceRegister", "PaymentAmount", c => c.Double());
            DropColumn("dbo.InvoiceRegister", "ReportingType");
        }
    }
}
