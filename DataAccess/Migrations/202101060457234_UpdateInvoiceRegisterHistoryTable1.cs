namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateInvoiceRegisterHistoryTable1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InvoiceRegisterHistories", "DTH", c => c.Double());
            AddColumn("dbo.InvoiceRegisterHistories", "TPA", c => c.Double());
            AddColumn("dbo.InvoiceRegisterHistories", "TPS", c => c.Double());
            AddColumn("dbo.InvoiceRegisterHistories", "PPD", c => c.Double());
            AddColumn("dbo.InvoiceRegisterHistories", "CCA", c => c.Double());
            AddColumn("dbo.InvoiceRegisterHistories", "CCS", c => c.Double());
            AddColumn("dbo.InvoiceRegisterHistories", "PA", c => c.Double());
            AddColumn("dbo.InvoiceRegisterHistories", "HS", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.InvoiceRegisterHistories", "HS");
            DropColumn("dbo.InvoiceRegisterHistories", "PA");
            DropColumn("dbo.InvoiceRegisterHistories", "CCS");
            DropColumn("dbo.InvoiceRegisterHistories", "CCA");
            DropColumn("dbo.InvoiceRegisterHistories", "PPD");
            DropColumn("dbo.InvoiceRegisterHistories", "TPS");
            DropColumn("dbo.InvoiceRegisterHistories", "TPA");
            DropColumn("dbo.InvoiceRegisterHistories", "DTH");
        }
    }
}
