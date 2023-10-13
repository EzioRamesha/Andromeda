namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDirectRetroProvisioningTransactionTable1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DirectRetroProvisioningTransactions", "TreatyCode", c => c.String(maxLength: 35));
            AddColumn("dbo.DirectRetroProvisioningTransactions", "TreatyType", c => c.String(maxLength: 35));
            AddColumn("dbo.DirectRetroProvisioningTransactions", "ClaimCode", c => c.String(maxLength: 30));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DirectRetroProvisioningTransactions", "ClaimCode");
            DropColumn("dbo.DirectRetroProvisioningTransactions", "TreatyType");
            DropColumn("dbo.DirectRetroProvisioningTransactions", "TreatyCode");
        }
    }
}
