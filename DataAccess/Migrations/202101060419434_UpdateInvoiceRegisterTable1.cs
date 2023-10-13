namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateInvoiceRegisterTable1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InvoiceRegister", "DTH", c => c.Double());
            AddColumn("dbo.InvoiceRegister", "TPA", c => c.Double());
            AddColumn("dbo.InvoiceRegister", "TPS", c => c.Double());
            AddColumn("dbo.InvoiceRegister", "PPD", c => c.Double());
            AddColumn("dbo.InvoiceRegister", "CCA", c => c.Double());
            AddColumn("dbo.InvoiceRegister", "CCS", c => c.Double());
            AddColumn("dbo.InvoiceRegister", "PA", c => c.Double());
            AddColumn("dbo.InvoiceRegister", "HS", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.InvoiceRegister", "HS");
            DropColumn("dbo.InvoiceRegister", "PA");
            DropColumn("dbo.InvoiceRegister", "CCS");
            DropColumn("dbo.InvoiceRegister", "CCA");
            DropColumn("dbo.InvoiceRegister", "PPD");
            DropColumn("dbo.InvoiceRegister", "TPS");
            DropColumn("dbo.InvoiceRegister", "TPA");
            DropColumn("dbo.InvoiceRegister", "DTH");
        }
    }
}
