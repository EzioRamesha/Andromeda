namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateInvoiceRegisterHistoryTable3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InvoiceRegisterHistories", "ContractCode", c => c.String(maxLength: 35));
            AddColumn("dbo.InvoiceRegisterHistories", "AnnualCohort", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.InvoiceRegisterHistories", "AnnualCohort");
            DropColumn("dbo.InvoiceRegisterHistories", "ContractCode");
        }
    }
}
