using Shared;
using System;
using System.IO;

namespace BusinessObject
{
    public class RetroStatementBo
    {
        public int Id { get; set; }

        public int DirectRetroId { get; set; }

        public DirectRetroBo DirectRetroBo { get; set; }

        public int RetroPartyId { get; set; }

        public RetroPartyBo RetroPartyBo { get; set; }

        public int Status { get; set; }

        public string StatusName { get; set; }

        public string MlreRef { get; set; }

        public string CedingCompany { get; set; }

        public string TreatyCode { get; set; }

        public string TreatyNo { get; set; }

        public string Schedule { get; set; }

        public string TreatyType { get; set; }

        public string FromMlreTo { get; set; }

        public string AccountsFor { get; set; }

        public DateTime? DateReportCompleted { get; set; }

        public DateTime? DateSendToRetro { get; set; }

        // Data Set 1
        public string AccountingPeriod { get; set; }

        public double? ReserveCededBegin { get; set; }

        public double? ReserveCededEnd { get; set; }

        public double? RiskChargeCededBegin { get; set; }

        public double? RiskChargeCededEnd { get; set; }

        public double? AverageReserveCeded { get; set; }

        // Reinsurance Premium
        public double? RiPremiumNB { get; set; }

        public double? RiPremiumRN { get; set; }

        public double? RiPremiumALT { get; set; }

        public double? QuarterlyRiskPremium { get; set; }

        public double? RetrocessionMarketingFee { get; set; }

        // Reinsurance Discount
        public double? RiDiscountNB { get; set; }

        public double? RiDiscountRN { get; set; }

        public double? RiDiscountALT { get; set; }

        public double? AgreedDatabaseComm { get; set; }

        public double? GstPayable { get; set; }

        public double? NoClaimBonus { get; set; }

        public double? Claims { get; set; }

        public double? ProfitComm { get; set; }

        public double? SurrenderValue { get; set; }

        public double? PaymentToTheReinsurer { get; set; }

        // Total No of Policies
        public int? TotalNoOfPolicyNB { get; set; }

        public int? TotalNoOfPolicyRN { get; set; }

        public int? TotalNoOfPolicyALT { get; set; }

        // Total Sum Reinsured
        public double? TotalSumReinsuredNB { get; set; }

        public double? TotalSumReinsuredRN { get; set; }

        public double? TotalSumReinsuredALT { get; set; }

        // Data Set 2
        public string AccountingPeriod2 { get; set; }

        public double? ReserveCededBegin2 { get; set; }

        public double? ReserveCededEnd2 { get; set; }

        public double? RiskChargeCededBegin2 { get; set; }

        public double? RiskChargeCededEnd2 { get; set; }

        public double? AverageReserveCeded2 { get; set; }

        // Reinsurance Premium
        public double? RiPremiumNB2 { get; set; }

        public double? RiPremiumRN2 { get; set; }

        public double? RiPremiumALT2 { get; set; }

        public double? QuarterlyRiskPremium2 { get; set; }

        public double? RetrocessionMarketingFee2 { get; set; }

        // Reinsurance Discount
        public double? RiDiscountNB2 { get; set; }

        public double? RiDiscountRN2 { get; set; }

        public double? RiDiscountALT2 { get; set; }

        public double? AgreedDatabaseComm2 { get; set; }

        public double? GstPayable2 { get; set; }

        public double? NoClaimBonus2 { get; set; }

        public double? Claims2 { get; set; }

        public double? ProfitComm2 { get; set; }

        public double? SurrenderValue2 { get; set; }

        public double? PaymentToTheReinsurer2 { get; set; }

        // Total No of Policies
        public int? TotalNoOfPolicyNB2 { get; set; }

        public int? TotalNoOfPolicyRN2 { get; set; }

        public int? TotalNoOfPolicyALT2 { get; set; }

        // Total Sum Reinsured
        public double? TotalSumReinsuredNB2 { get; set; }

        public double? TotalSumReinsuredRN2 { get; set; }

        public double? TotalSumReinsuredALT2 { get; set; }

        // Data Set 3
        public string AccountingPeriod3 { get; set; }

        public double? ReserveCededBegin3 { get; set; }

        public double? ReserveCededEnd3 { get; set; }

        public double? RiskChargeCededBegin3 { get; set; }

        public double? RiskChargeCededEnd3 { get; set; }

        public double? AverageReserveCeded3 { get; set; }

        // Reinsurance Premium
        public double? RiPremiumNB3 { get; set; }

        public double? RiPremiumRN3 { get; set; }

        public double? RiPremiumALT3 { get; set; }

        public double? QuarterlyRiskPremium3 { get; set; }

        public double? RetrocessionMarketingFee3 { get; set; }

        // Reinsurance Discount
        public double? RiDiscountNB3 { get; set; }

        public double? RiDiscountRN3 { get; set; }

        public double? RiDiscountALT3 { get; set; }

        public double? AgreedDatabaseComm3 { get; set; }

        public double? GstPayable3 { get; set; }

        public double? NoClaimBonus3 { get; set; }

        public double? Claims3 { get; set; }

        public double? ProfitComm3 { get; set; }

        public double? SurrenderValue3 { get; set; }

        public double? PaymentToTheReinsurer3 { get; set; }

        // Total No of Policies
        public int? TotalNoOfPolicyNB3 { get; set; }

        public int? TotalNoOfPolicyRN3 { get; set; }

        public int? TotalNoOfPolicyALT3 { get; set; }

        // Total Sum Reinsured
        public double? TotalSumReinsuredNB3 { get; set; }

        public double? TotalSumReinsuredRN3 { get; set; }

        public double? TotalSumReinsuredALT3 { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? UpdatedById { get; set; }

        // String for Double
        // Data Set 1
        public string ReserveCededBeginStr { get; set; }

        public string ReserveCededEndStr { get; set; }

        public string RiskChargeCededBeginStr { get; set; }

        public string RiskChargeCededEndStr { get; set; }

        public string AverageReserveCededStr { get; set; }

        public string RiPremiumNBStr { get; set; }

        public string RiPremiumRNStr { get; set; }

        public string RiPremiumALTStr { get; set; }

        public string QuarterlyRiskPremiumStr { get; set; }

        public string RetrocessionMarketingFeeStr { get; set; }

        public string RiDiscountNBStr { get; set; }

        public string RiDiscountRNStr { get; set; }

        public string RiDiscountALTStr { get; set; }

        public string AgreedDatabaseCommStr { get; set; }

        public string GstPayableStr { get; set; }

        public string NoClaimBonusStr { get; set; }

        public string ClaimsStr { get; set; }

        public string ProfitCommStr { get; set; }

        public string SurrenderValueStr { get; set; }

        public string PaymentToTheReinsurerStr { get; set; }

        public string TotalSumReinsuredNBStr { get; set; }

        public string TotalSumReinsuredRNStr { get; set; }

        public string TotalSumReinsuredALTStr { get; set; }

        // Data Set 2
        public string ReserveCededBegin2Str { get; set; }

        public string ReserveCededEnd2Str { get; set; }

        public string RiskChargeCededBegin2Str { get; set; }

        public string RiskChargeCededEnd2Str { get; set; }

        public string AverageReserveCeded2Str { get; set; }

        public string RiPremiumNB2Str { get; set; }

        public string RiPremiumRN2Str { get; set; }

        public string RiPremiumALT2Str { get; set; }

        public string QuarterlyRiskPremium2Str { get; set; }

        public string RetrocessionMarketingFee2Str { get; set; }

        public string RiDiscountNB2Str { get; set; }

        public string RiDiscountRN2Str { get; set; }

        public string RiDiscountALT2Str { get; set; }

        public string AgreedDatabaseComm2Str { get; set; }

        public string GstPayable2Str { get; set; }

        public string NoClaimBonus2Str { get; set; }

        public string Claims2Str { get; set; }

        public string ProfitComm2Str { get; set; }

        public string SurrenderValue2Str { get; set; }

        public string PaymentToTheReinsurer2Str { get; set; }

        public string TotalSumReinsuredNB2Str { get; set; }

        public string TotalSumReinsuredRN2Str { get; set; }

        public string TotalSumReinsuredALT2Str { get; set; }

        // Data Set 3
        public string ReserveCededBegin3Str { get; set; }

        public string ReserveCededEnd3Str { get; set; }

        public string RiskChargeCededBegin3Str { get; set; }

        public string RiskChargeCededEnd3Str { get; set; }

        public string AverageReserveCeded3Str { get; set; }

        public string RiPremiumNB3Str { get; set; }

        public string RiPremiumRN3Str { get; set; }

        public string RiPremiumALT3Str { get; set; }

        public string QuarterlyRiskPremium3Str { get; set; }

        public string RetrocessionMarketingFee3Str { get; set; }

        public string RiDiscountNB3Str { get; set; }

        public string RiDiscountRN3Str { get; set; }

        public string RiDiscountALT3Str { get; set; }

        public string AgreedDatabaseComm3Str { get; set; }

        public string GstPayable3Str { get; set; }

        public string NoClaimBonus3Str { get; set; }

        public string Claims3Str { get; set; }

        public string ProfitComm3Str { get; set; }

        public string SurrenderValue3Str { get; set; }

        public string PaymentToTheReinsurer3Str { get; set; }

        public string TotalSumReinsuredNB3Str { get; set; }

        public string TotalSumReinsuredRN3Str { get; set; }

        public string TotalSumReinsuredALT3Str { get; set; }

        public const int StatusPending = 1;
        public const int StatusFinalised = 2;

        public static string GetStatusName(int key)
        {
            switch (key)
            {
                case StatusPending:
                    return "Pending";
                case StatusFinalised:
                    return "Finalised";
                default:
                    return "";
            }
        }

        public static string GetStatusClass(int key)
        {
            switch (key)
            {
                case StatusPending:
                    return "status-pending-badge";
                case StatusFinalised:
                    return "status-success-badge";
                default:
                    return "";
            }
        }

        public string GetLocalDirectory()
        {
            return Util.GetRetroStatementPath();
        }

        public void CalculatePaymentToReinsuerer(bool isSwissRe = false)
        {
            // Reset
            PaymentToTheReinsurer = null;
            PaymentToTheReinsurer2 = null;
            PaymentToTheReinsurer3 = null;

            // Calculate Payment to the Reinsurer
            // Data Set 1
            double riPremiumNB = Util.RoundNullableValue(RiPremiumNB, 2) ?? 0;
            double riPremiumRN = Util.RoundNullableValue(RiPremiumRN, 2) ?? 0;
            double riPremiumALT = Util.RoundNullableValue(RiPremiumALT, 2) ?? 0;
            double quarterlyRiskPremium = Util.RoundNullableValue(QuarterlyRiskPremium, 2) ?? 0;
            double retrocessionMarketingFee = Util.RoundNullableValue(RetrocessionMarketingFee, 2) ?? 0;
            double riDiscountNB = Util.RoundNullableValue(RiDiscountNB, 2) ?? 0;
            double riDiscountRN = Util.RoundNullableValue(RiDiscountRN, 2) ?? 0;
            double riDiscountALT = Util.RoundNullableValue(RiDiscountALT, 2) ?? 0;
            double agreedDatabaseComm = Util.RoundNullableValue(AgreedDatabaseComm, 2) ?? 0;
            double gstPayable = Util.RoundNullableValue(GstPayable, 2) ?? 0;
            double noClaimBonus = Util.RoundNullableValue(NoClaimBonus, 2) ?? 0;
            double claims = Util.RoundNullableValue(Claims, 2) ?? 0;
            double profitComm = Util.RoundNullableValue(ProfitComm, 2) ?? 0;
            double surrenderValue = Util.RoundNullableValue(SurrenderValue, 2) ?? 0;

            // Data Set 2
            double riPremiumNB2 = Util.RoundNullableValue(RiPremiumNB2, 2) ?? 0;
            double riPremiumRN2 = Util.RoundNullableValue(RiPremiumRN2, 2) ?? 0;
            double riPremiumALT2 = Util.RoundNullableValue(RiPremiumALT2, 2) ?? 0;
            double quarterlyRiskPremium2 = Util.RoundNullableValue(QuarterlyRiskPremium2, 2) ?? 0;
            double retrocessionMarketingFee2 = Util.RoundNullableValue(RetrocessionMarketingFee2, 2) ?? 0;
            double riDiscountNB2 = Util.RoundNullableValue(RiDiscountNB2, 2) ?? 0;
            double riDiscountRN2 = Util.RoundNullableValue(RiDiscountRN2, 2) ?? 0;
            double riDiscountALT2 = Util.RoundNullableValue(RiDiscountALT2, 2) ?? 0;
            double agreedDatabaseComm2 = Util.RoundNullableValue(AgreedDatabaseComm2, 2) ?? 0;
            double gstPayable2 = Util.RoundNullableValue(GstPayable2, 2) ?? 0;
            double noClaimBonus2 = Util.RoundNullableValue(NoClaimBonus2, 2) ?? 0;
            double claims2 = Util.RoundNullableValue(Claims2, 2) ?? 0;
            double profitComm2 = Util.RoundNullableValue(ProfitComm2, 2) ?? 0;
            double surrenderValue2 = Util.RoundNullableValue(SurrenderValue2, 2) ?? 0;

            // Data Set 3
            double riPremiumNB3 = Util.RoundNullableValue(RiPremiumNB3, 2) ?? 0;
            double riPremiumRN3 = Util.RoundNullableValue(RiPremiumRN3, 2) ?? 0;
            double riPremiumALT3 = Util.RoundNullableValue(RiPremiumALT3, 2) ?? 0;
            double quarterlyRiskPremium3 = Util.RoundNullableValue(QuarterlyRiskPremium3, 2) ?? 0;
            double retrocessionMarketingFee3 = Util.RoundNullableValue(RetrocessionMarketingFee3, 2) ?? 0;
            double riDiscountNB3 = Util.RoundNullableValue(RiDiscountNB3, 2) ?? 0;
            double riDiscountRN3 = Util.RoundNullableValue(RiDiscountRN3, 2) ?? 0;
            double riDiscountALT3 = Util.RoundNullableValue(RiDiscountALT3, 2) ?? 0;
            double agreedDatabaseComm3 = Util.RoundNullableValue(AgreedDatabaseComm3, 2) ?? 0;
            double gstPayable3 = Util.RoundNullableValue(GstPayable3, 2) ?? 0;
            double noClaimBonus3 = Util.RoundNullableValue(NoClaimBonus3, 2) ?? 0;
            double claims3 = Util.RoundNullableValue(Claims3, 2) ?? 0;
            double profitComm3 = Util.RoundNullableValue(ProfitComm3, 2) ?? 0;
            double surrenderValue3 = Util.RoundNullableValue(SurrenderValue3, 2) ?? 0;

            if (isSwissRe)
            {
                PaymentToTheReinsurer = riPremiumNB + riPremiumRN + riPremiumALT +
                    retrocessionMarketingFee -
                    riDiscountNB - riDiscountRN - riDiscountALT -
                    agreedDatabaseComm -
                    noClaimBonus -
                    claims -
                    surrenderValue;

                PaymentToTheReinsurer2 = riPremiumNB2 + riPremiumRN2 + riPremiumALT2 +
                    retrocessionMarketingFee2 -
                    riDiscountNB2 - riDiscountRN2 - riDiscountALT2 -
                    agreedDatabaseComm2 -
                    noClaimBonus2 -
                    claims2 -
                    surrenderValue2;

                PaymentToTheReinsurer3 = riPremiumNB3 + riPremiumRN3 + riPremiumALT3 +
                    retrocessionMarketingFee3 -
                    riDiscountNB3 - riDiscountRN3 - riDiscountALT3 -
                    agreedDatabaseComm3 -
                    noClaimBonus3 -
                    claims3 -
                    surrenderValue3;
            }
            else
            {
                PaymentToTheReinsurer = riPremiumNB + riPremiumRN + riPremiumALT +
                    quarterlyRiskPremium -
                    riDiscountNB - riDiscountRN - riDiscountALT +
                    gstPayable -
                    claims -
                    profitComm -
                    surrenderValue;

                PaymentToTheReinsurer2 = riPremiumNB2 + riPremiumRN2 + riPremiumALT2 +
                    quarterlyRiskPremium2 -
                    riDiscountNB2 - riDiscountRN2 - riDiscountALT2 +
                    gstPayable2 -
                    claims2 -
                    profitComm2 -
                    surrenderValue2;

                PaymentToTheReinsurer3 = riPremiumNB3 + riPremiumRN3 + riPremiumALT3 +
                    quarterlyRiskPremium3 -
                    riDiscountNB3 - riDiscountRN3 - riDiscountALT3 +
                    gstPayable3 -
                    claims3 -
                    profitComm3 -
                    surrenderValue3;
            }
        }
    }
}
