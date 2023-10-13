namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateInvoiceRegisterTable6 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.InvoiceRegister", "ValuationMode", c => c.String(maxLength: 3));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.InvoiceRegister", "ValuationMode", c => c.Int());
        }
    }
}
