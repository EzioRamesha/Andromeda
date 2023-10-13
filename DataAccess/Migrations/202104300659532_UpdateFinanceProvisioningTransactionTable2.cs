namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateFinanceProvisioningTransactionTable2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FinanceProvisioningTransactions", "SortIndex", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.FinanceProvisioningTransactions", "SortIndex");
        }
    }
}
