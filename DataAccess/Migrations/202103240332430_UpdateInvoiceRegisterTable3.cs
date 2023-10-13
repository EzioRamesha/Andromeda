namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateInvoiceRegisterTable3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InvoiceRegister", "ContractCode", c => c.String(maxLength: 35));
            AddColumn("dbo.InvoiceRegister", "AnnualCohort", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.InvoiceRegister", "AnnualCohort");
            DropColumn("dbo.InvoiceRegister", "ContractCode");
        }
    }
}
