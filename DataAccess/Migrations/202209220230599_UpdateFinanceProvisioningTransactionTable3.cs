namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateFinanceProvisioningTransactionTable3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FinanceProvisioningTransactions", "LastTransactionDate", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AddColumn("dbo.FinanceProvisioningTransactions", "LastTransactionQuarter", c => c.String(maxLength: 30));
            AddColumn("dbo.FinanceProvisioningTransactions", "DateOfEvent", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AddColumn("dbo.FinanceProvisioningTransactions", "RiskQuarter", c => c.String(maxLength: 30));
            AddColumn("dbo.FinanceProvisioningTransactions", "RiskPeriodYear", c => c.Int());
            AddColumn("dbo.FinanceProvisioningTransactions", "RiskPeriodMonth", c => c.Int());
            AddColumn("dbo.FinanceProvisioningTransactions", "MlreBenefitCode", c => c.String(maxLength: 30));
            AddColumn("dbo.FinanceProvisioningTransactions", "FundsAccountingTypeCode", c => c.String(maxLength: 30));
            AddColumn("dbo.FinanceProvisioningTransactions", "RetroParty1", c => c.String(maxLength: 128));
            AddColumn("dbo.FinanceProvisioningTransactions", "RetroParty2", c => c.String(maxLength: 128));
            AddColumn("dbo.FinanceProvisioningTransactions", "RetroParty3", c => c.String(maxLength: 128));
            AddColumn("dbo.FinanceProvisioningTransactions", "RetroRecovery1", c => c.Double());
            AddColumn("dbo.FinanceProvisioningTransactions", "RetroRecovery2", c => c.Double());
            AddColumn("dbo.FinanceProvisioningTransactions", "RetroRecovery3", c => c.Double());
            AddColumn("dbo.FinanceProvisioningTransactions", "ReinsBasisCode", c => c.String());
            AddColumn("dbo.FinanceProvisioningTransactions", "ReinsEffDatePol", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AddColumn("dbo.FinanceProvisioningTransactions", "Mfrs17AnnualCohort", c => c.Int());
            AddColumn("dbo.FinanceProvisioningTransactions", "Mfrs17ContractCode", c => c.String(maxLength: 30));
        }
        
        public override void Down()
        {
            DropColumn("dbo.FinanceProvisioningTransactions", "Mfrs17ContractCode");
            DropColumn("dbo.FinanceProvisioningTransactions", "Mfrs17AnnualCohort");
            DropColumn("dbo.FinanceProvisioningTransactions", "ReinsEffDatePol");
            DropColumn("dbo.FinanceProvisioningTransactions", "ReinsBasisCode");
            DropColumn("dbo.FinanceProvisioningTransactions", "RetroRecovery3");
            DropColumn("dbo.FinanceProvisioningTransactions", "RetroRecovery2");
            DropColumn("dbo.FinanceProvisioningTransactions", "RetroRecovery1");
            DropColumn("dbo.FinanceProvisioningTransactions", "RetroParty3");
            DropColumn("dbo.FinanceProvisioningTransactions", "RetroParty2");
            DropColumn("dbo.FinanceProvisioningTransactions", "RetroParty1");
            DropColumn("dbo.FinanceProvisioningTransactions", "FundsAccountingTypeCode");
            DropColumn("dbo.FinanceProvisioningTransactions", "MlreBenefitCode");
            DropColumn("dbo.FinanceProvisioningTransactions", "RiskPeriodMonth");
            DropColumn("dbo.FinanceProvisioningTransactions", "RiskPeriodYear");
            DropColumn("dbo.FinanceProvisioningTransactions", "RiskQuarter");
            DropColumn("dbo.FinanceProvisioningTransactions", "DateOfEvent");
            DropColumn("dbo.FinanceProvisioningTransactions", "LastTransactionQuarter");
            DropColumn("dbo.FinanceProvisioningTransactions", "LastTransactionDate");
        }
    }
}
