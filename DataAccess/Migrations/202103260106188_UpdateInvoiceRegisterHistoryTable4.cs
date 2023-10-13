namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateInvoiceRegisterHistoryTable4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InvoiceRegisterHistories", "TPD", c => c.Double());
            AddColumn("dbo.InvoiceRegisterHistories", "CI", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.InvoiceRegisterHistories", "CI");
            DropColumn("dbo.InvoiceRegisterHistories", "TPD");
        }
    }
}
