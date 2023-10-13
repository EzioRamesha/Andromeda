namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateInvoiceRegisterHistoryTable5 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.InvoiceRegisterHistories", "Frequency", c => c.String(maxLength: 3));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.InvoiceRegisterHistories", "Frequency", c => c.Int());
        }
    }
}
