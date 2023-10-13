namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRetroStatementTable2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RetroStatements", "RetrocessionMarketingFee", c => c.Double());
            AddColumn("dbo.RetroStatements", "AgreedDatabaseComm", c => c.Double());
            AddColumn("dbo.RetroStatements", "NoClaimBonus", c => c.Double());
            AddColumn("dbo.RetroStatements", "AccountingPeriod2", c => c.String(maxLength: 10));
            AddColumn("dbo.RetroStatements", "ReserveCededBegin2", c => c.Double());
            AddColumn("dbo.RetroStatements", "ReserveCededEnd2", c => c.Double());
            AddColumn("dbo.RetroStatements", "RiskChargeCededBegin2", c => c.Double());
            AddColumn("dbo.RetroStatements", "RiskChargeCededEnd2", c => c.Double());
            AddColumn("dbo.RetroStatements", "AverageReserveCeded2", c => c.Double());
            AddColumn("dbo.RetroStatements", "RiPremiumNB2", c => c.Double());
            AddColumn("dbo.RetroStatements", "RiPremiumRN2", c => c.Double());
            AddColumn("dbo.RetroStatements", "RiPremiumALT2", c => c.Double());
            AddColumn("dbo.RetroStatements", "QuarterlyRiskPremium2", c => c.Double());
            AddColumn("dbo.RetroStatements", "RetrocessionMarketingFee2", c => c.Double());
            AddColumn("dbo.RetroStatements", "RiDiscountNB2", c => c.Double());
            AddColumn("dbo.RetroStatements", "RiDiscountRN2", c => c.Double());
            AddColumn("dbo.RetroStatements", "RiDiscountALT2", c => c.Double());
            AddColumn("dbo.RetroStatements", "AgreedDatabaseComm2", c => c.Double());
            AddColumn("dbo.RetroStatements", "GstPayable2", c => c.Double());
            AddColumn("dbo.RetroStatements", "NoClaimBonus2", c => c.Double());
            AddColumn("dbo.RetroStatements", "Claims2", c => c.Double());
            AddColumn("dbo.RetroStatements", "ProfitComm2", c => c.Double());
            AddColumn("dbo.RetroStatements", "SurrenderValue2", c => c.Double());
            AddColumn("dbo.RetroStatements", "PaymentToTheReinsurer2", c => c.Double());
            AddColumn("dbo.RetroStatements", "TotalNoOfPolicyNB2", c => c.Int());
            AddColumn("dbo.RetroStatements", "TotalNoOfPolicyRN2", c => c.Int());
            AddColumn("dbo.RetroStatements", "TotalNoOfPolicyALT2", c => c.Int());
            AddColumn("dbo.RetroStatements", "TotalSumReinsuredNB2", c => c.Double());
            AddColumn("dbo.RetroStatements", "TotalSumReinsuredRN2", c => c.Double());
            AddColumn("dbo.RetroStatements", "TotalSumReinsuredALT2", c => c.Double());
            AddColumn("dbo.RetroStatements", "AccountingPeriod3", c => c.String(maxLength: 10));
            AddColumn("dbo.RetroStatements", "ReserveCededBegin3", c => c.Double());
            AddColumn("dbo.RetroStatements", "ReserveCededEnd3", c => c.Double());
            AddColumn("dbo.RetroStatements", "RiskChargeCededBegin3", c => c.Double());
            AddColumn("dbo.RetroStatements", "RiskChargeCededEnd3", c => c.Double());
            AddColumn("dbo.RetroStatements", "AverageReserveCeded3", c => c.Double());
            AddColumn("dbo.RetroStatements", "RiPremiumNB3", c => c.Double());
            AddColumn("dbo.RetroStatements", "RiPremiumRN3", c => c.Double());
            AddColumn("dbo.RetroStatements", "RiPremiumALT3", c => c.Double());
            AddColumn("dbo.RetroStatements", "QuarterlyRiskPremium3", c => c.Double());
            AddColumn("dbo.RetroStatements", "RetrocessionMarketingFee3", c => c.Double());
            AddColumn("dbo.RetroStatements", "RiDiscountNB3", c => c.Double());
            AddColumn("dbo.RetroStatements", "RiDiscountRN3", c => c.Double());
            AddColumn("dbo.RetroStatements", "RiDiscountALT3", c => c.Double());
            AddColumn("dbo.RetroStatements", "AgreedDatabaseComm3", c => c.Double());
            AddColumn("dbo.RetroStatements", "GstPayable3", c => c.Double());
            AddColumn("dbo.RetroStatements", "NoClaimBonus3", c => c.Double());
            AddColumn("dbo.RetroStatements", "Claims3", c => c.Double());
            AddColumn("dbo.RetroStatements", "ProfitComm3", c => c.Double());
            AddColumn("dbo.RetroStatements", "SurrenderValue3", c => c.Double());
            AddColumn("dbo.RetroStatements", "PaymentToTheReinsurer3", c => c.Double());
            AddColumn("dbo.RetroStatements", "TotalNoOfPolicyNB3", c => c.Int());
            AddColumn("dbo.RetroStatements", "TotalNoOfPolicyRN3", c => c.Int());
            AddColumn("dbo.RetroStatements", "TotalNoOfPolicyALT3", c => c.Int());
            AddColumn("dbo.RetroStatements", "TotalSumReinsuredNB3", c => c.Double());
            AddColumn("dbo.RetroStatements", "TotalSumReinsuredRN3", c => c.Double());
            AddColumn("dbo.RetroStatements", "TotalSumReinsuredALT3", c => c.Double());
            DropColumn("dbo.RetroStatements", "RiskQuarter");
        }
        
        public override void Down()
        {
            AddColumn("dbo.RetroStatements", "RiskQuarter", c => c.String(nullable: false, maxLength: 10));
            DropColumn("dbo.RetroStatements", "TotalSumReinsuredALT3");
            DropColumn("dbo.RetroStatements", "TotalSumReinsuredRN3");
            DropColumn("dbo.RetroStatements", "TotalSumReinsuredNB3");
            DropColumn("dbo.RetroStatements", "TotalNoOfPolicyALT3");
            DropColumn("dbo.RetroStatements", "TotalNoOfPolicyRN3");
            DropColumn("dbo.RetroStatements", "TotalNoOfPolicyNB3");
            DropColumn("dbo.RetroStatements", "PaymentToTheReinsurer3");
            DropColumn("dbo.RetroStatements", "SurrenderValue3");
            DropColumn("dbo.RetroStatements", "ProfitComm3");
            DropColumn("dbo.RetroStatements", "Claims3");
            DropColumn("dbo.RetroStatements", "NoClaimBonus3");
            DropColumn("dbo.RetroStatements", "GstPayable3");
            DropColumn("dbo.RetroStatements", "AgreedDatabaseComm3");
            DropColumn("dbo.RetroStatements", "RiDiscountALT3");
            DropColumn("dbo.RetroStatements", "RiDiscountRN3");
            DropColumn("dbo.RetroStatements", "RiDiscountNB3");
            DropColumn("dbo.RetroStatements", "RetrocessionMarketingFee3");
            DropColumn("dbo.RetroStatements", "QuarterlyRiskPremium3");
            DropColumn("dbo.RetroStatements", "RiPremiumALT3");
            DropColumn("dbo.RetroStatements", "RiPremiumRN3");
            DropColumn("dbo.RetroStatements", "RiPremiumNB3");
            DropColumn("dbo.RetroStatements", "AverageReserveCeded3");
            DropColumn("dbo.RetroStatements", "RiskChargeCededEnd3");
            DropColumn("dbo.RetroStatements", "RiskChargeCededBegin3");
            DropColumn("dbo.RetroStatements", "ReserveCededEnd3");
            DropColumn("dbo.RetroStatements", "ReserveCededBegin3");
            DropColumn("dbo.RetroStatements", "AccountingPeriod3");
            DropColumn("dbo.RetroStatements", "TotalSumReinsuredALT2");
            DropColumn("dbo.RetroStatements", "TotalSumReinsuredRN2");
            DropColumn("dbo.RetroStatements", "TotalSumReinsuredNB2");
            DropColumn("dbo.RetroStatements", "TotalNoOfPolicyALT2");
            DropColumn("dbo.RetroStatements", "TotalNoOfPolicyRN2");
            DropColumn("dbo.RetroStatements", "TotalNoOfPolicyNB2");
            DropColumn("dbo.RetroStatements", "PaymentToTheReinsurer2");
            DropColumn("dbo.RetroStatements", "SurrenderValue2");
            DropColumn("dbo.RetroStatements", "ProfitComm2");
            DropColumn("dbo.RetroStatements", "Claims2");
            DropColumn("dbo.RetroStatements", "NoClaimBonus2");
            DropColumn("dbo.RetroStatements", "GstPayable2");
            DropColumn("dbo.RetroStatements", "AgreedDatabaseComm2");
            DropColumn("dbo.RetroStatements", "RiDiscountALT2");
            DropColumn("dbo.RetroStatements", "RiDiscountRN2");
            DropColumn("dbo.RetroStatements", "RiDiscountNB2");
            DropColumn("dbo.RetroStatements", "RetrocessionMarketingFee2");
            DropColumn("dbo.RetroStatements", "QuarterlyRiskPremium2");
            DropColumn("dbo.RetroStatements", "RiPremiumALT2");
            DropColumn("dbo.RetroStatements", "RiPremiumRN2");
            DropColumn("dbo.RetroStatements", "RiPremiumNB2");
            DropColumn("dbo.RetroStatements", "AverageReserveCeded2");
            DropColumn("dbo.RetroStatements", "RiskChargeCededEnd2");
            DropColumn("dbo.RetroStatements", "RiskChargeCededBegin2");
            DropColumn("dbo.RetroStatements", "ReserveCededEnd2");
            DropColumn("dbo.RetroStatements", "ReserveCededBegin2");
            DropColumn("dbo.RetroStatements", "AccountingPeriod2");
            DropColumn("dbo.RetroStatements", "NoClaimBonus");
            DropColumn("dbo.RetroStatements", "AgreedDatabaseComm");
            DropColumn("dbo.RetroStatements", "RetrocessionMarketingFee");
        }
    }
}
