namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateInvoiceRegisterTable4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InvoiceRegister", "TPD", c => c.Double());
            AddColumn("dbo.InvoiceRegister", "CI", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.InvoiceRegister", "CI");
            DropColumn("dbo.InvoiceRegister", "TPD");
        }
    }
}
