namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateInvoiceRegisterHistoryTable6 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.InvoiceRegisterHistories", "ValuationMode", c => c.String(maxLength: 3));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.InvoiceRegisterHistories", "ValuationMode", c => c.Int());
        }
    }
}
