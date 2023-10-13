namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateInvoiceRegisterTable2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InvoiceRegister", "Gst", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.InvoiceRegister", "Gst");
        }
    }
}
