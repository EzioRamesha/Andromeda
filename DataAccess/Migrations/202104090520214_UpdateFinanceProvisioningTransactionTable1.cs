namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateFinanceProvisioningTransactionTable1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FinanceProvisioningTransactions", "TreatyCode", c => c.String(maxLength: 35));
            AddColumn("dbo.FinanceProvisioningTransactions", "TreatyType", c => c.String(maxLength: 35));
            AddColumn("dbo.FinanceProvisioningTransactions", "ClaimCode", c => c.String(maxLength: 30));
        }
        
        public override void Down()
        {
            DropColumn("dbo.FinanceProvisioningTransactions", "ClaimCode");
            DropColumn("dbo.FinanceProvisioningTransactions", "TreatyType");
            DropColumn("dbo.FinanceProvisioningTransactions", "TreatyCode");
        }
    }
}
