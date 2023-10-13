namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateInvoiceRegisterHistoryTable2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InvoiceRegisterHistories", "Gst", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.InvoiceRegisterHistories", "Gst");
        }
    }
}
