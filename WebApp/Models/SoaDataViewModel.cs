using BusinessObject.SoaDatas;
using DataAccess.Entities.SoaDatas;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace WebApp.Models
{
    public class SoaDataViewModel { }

    public class SoaDataReinsuranceViewModel
    {
        public int Id { get; set; }

        public string CompanyName { get; set; }

        public string BusinessCode { get; set; }

        public string TreatyId { get; set; }

        public string TreatyCode { get; set; }

        public string TreatyMode { get; set; }

        public string TreatyType { get; set; }

        public string PlanBlock { get; set; }

        public int? RiskMonth { get; set; }

        public string SoaQuarter { get; set; }

        public string RiskQuarter { get; set; }

        public double? NbPremium { get; set; }

        public double? RnPremium { get; set; }

        public double? AltPremium { get; set; }

        public double? GrossPremium { get; set; }

        public double? TotalDiscount { get; set; }

        public double? RiskPremium { get; set; }

        public double? NoClaimBonus { get; set; }

        public double? Levy { get; set; }

        public double? Claim { get; set; }

        public double? ProfitComm { get; set; }

        public double? SurrenderValue { get; set; }

        public double? Gst { get; set; }

        public double? ModcoReserveIncome { get; set; }

        public double? RiDeposit { get; set; }

        public double? DatabaseCommission { get; set; }

        public double? AdministrationContribution { get; set; }

        public double? ShareOfRiCommissionFromCompulsoryCession { get; set; }

        public double? RecaptureFee { get; set; }

        public double? CreditCardCharges { get; set; }

        public double? BrokerageFee { get; set; }

        public double? TotalCommission { get; set; }

        public double? NetTotalAmount { get; set; }

        public DateTime? SoaReceivedDate { get; set; }

        public DateTime? BordereauxReceivedDate { get; set; }

        public string StatementStatus { get; set; }

        public string Remarks1 { get; set; }

        public string CurrencyCode { get; set; }

        public double? CurrencyRate { get; set; }

        public string SoaStatus { get; set; }

        public DateTime? ConfirmationDate { get; set; }

        public int? MappingStatus { get; set; }

        public string Error { get; set; }

        public static Expression<Func<SoaData, SoaDataReinsuranceViewModel>> Expression()
        {
            return entity => new SoaDataReinsuranceViewModel
            {
                Id = entity.Id,

                CompanyName = entity.CompanyName,
                BusinessCode = entity.BusinessCode,
                TreatyId = entity.TreatyId,
                TreatyCode = entity.TreatyCode,
                TreatyMode = entity.TreatyMode,
                TreatyType = entity.TreatyType,
                PlanBlock = entity.PlanBlock,
                RiskMonth = entity.RiskMonth,
                SoaQuarter = entity.SoaQuarter,
                RiskQuarter = entity.RiskQuarter,
                NbPremium = entity.NbPremium,
                RnPremium = entity.RnPremium,
                AltPremium = entity.AltPremium,
                GrossPremium = entity.GrossPremium,
                TotalDiscount = entity.TotalDiscount,
                RiskPremium = entity.RiskPremium,
                NoClaimBonus = entity.NoClaimBonus,
                Levy = entity.Levy,
                Claim = entity.Claim,
                ProfitComm = entity.ProfitComm,
                SurrenderValue = entity.SurrenderValue,
                Gst = entity.Gst,
                ModcoReserveIncome = entity.ModcoReserveIncome,
                RiDeposit = entity.RiDeposit,
                DatabaseCommission = entity.DatabaseCommission,
                AdministrationContribution = entity.AdministrationContribution,
                ShareOfRiCommissionFromCompulsoryCession = entity.ShareOfRiCommissionFromCompulsoryCession,
                RecaptureFee = entity.RecaptureFee,
                CreditCardCharges = entity.CreditCardCharges,
                BrokerageFee = entity.BrokerageFee,
                TotalCommission = entity.TotalCommission,
                NetTotalAmount = entity.NetTotalAmount,
                SoaReceivedDate = entity.SoaReceivedDate,
                BordereauxReceivedDate = entity.BordereauxReceivedDate,
                StatementStatus = entity.StatementStatus,
                Remarks1 = entity.Remarks1,
                CurrencyCode = entity.CurrencyCode,
                CurrencyRate = entity.CurrencyRate,
                SoaStatus = entity.SoaStatus,
                ConfirmationDate = entity.ConfirmationDate,
                MappingStatus = entity.MappingStatus,
                Error = entity.Errors,
            };
        }
    }

    public class SoaDataRetakafulViewModel
    {
        public int Id { get; set; }

        public string CompanyName { get; set; }

        public string BusinessCode { get; set; }

        public string TreatyId { get; set; }

        public string TreatyCode { get; set; }

        public string TreatyMode { get; set; }

        public string TreatyType { get; set; }

        public string PlanBlock { get; set; }

        public int? RiskMonth { get; set; }

        public string SoaQuarter { get; set; }

        public string RiskQuarter { get; set; }

        public double? NbPremium { get; set; }

        public double? RnPremium { get; set; }

        public double? AltPremium { get; set; }

        public double? GrossPremium { get; set; }

        public double? TotalDiscount { get; set; }

        public double? RiskPremium { get; set; }

        public double? NoClaimBonus { get; set; }

        public double? Levy { get; set; }

        public double? Claim { get; set; }

        public double? ProfitComm { get; set; }

        public double? SurrenderValue { get; set; }

        public double? Gst { get; set; }

        public double? ModcoReserveIncome { get; set; }

        public double? RiDeposit { get; set; }

        public double? DatabaseCommission { get; set; }

        public double? AdministrationContribution { get; set; }

        public double? ShareOfRiCommissionFromCompulsoryCession { get; set; }

        public double? RecaptureFee { get; set; }

        public double? CreditCardCharges { get; set; }

        public double? BrokerageFee { get; set; }

        public double? TotalCommission { get; set; }

        public double? NetTotalAmount { get; set; }

        public DateTime? SoaReceivedDate { get; set; }

        public DateTime? BordereauxReceivedDate { get; set; }

        public string StatementStatus { get; set; }

        public string Remarks1 { get; set; }

        public string CurrencyCode { get; set; }

        public double? CurrencyRate { get; set; }

        public string SoaStatus { get; set; }

        public DateTime? ConfirmationDate { get; set; }

        public int? MappingStatus { get; set; }

        public string Error { get; set; }

        public static Expression<Func<SoaData, SoaDataRetakafulViewModel>> Expression()
        {
            return entity => new SoaDataRetakafulViewModel
            {
                Id = entity.Id,

                CompanyName = entity.CompanyName,
                BusinessCode = entity.BusinessCode,
                TreatyId = entity.TreatyId,
                TreatyCode = entity.TreatyCode,
                TreatyMode = entity.TreatyMode,
                TreatyType = entity.TreatyType,
                PlanBlock = entity.PlanBlock,
                RiskMonth = entity.RiskMonth,
                SoaQuarter = entity.SoaQuarter,
                RiskQuarter = entity.RiskQuarter,
                NbPremium = entity.NbPremium,
                RnPremium = entity.RnPremium,
                AltPremium = entity.AltPremium,
                GrossPremium = entity.GrossPremium,
                TotalDiscount = entity.TotalDiscount,
                RiskPremium = entity.RiskPremium,
                NoClaimBonus = entity.NoClaimBonus,
                Levy = entity.Levy,
                Claim = entity.Claim,
                ProfitComm = entity.ProfitComm,
                SurrenderValue = entity.SurrenderValue,
                Gst = entity.Gst,
                ModcoReserveIncome = entity.ModcoReserveIncome,
                RiDeposit = entity.RiDeposit,
                DatabaseCommission = entity.DatabaseCommission,
                AdministrationContribution = entity.AdministrationContribution,
                ShareOfRiCommissionFromCompulsoryCession = entity.ShareOfRiCommissionFromCompulsoryCession,
                RecaptureFee = entity.RecaptureFee,
                CreditCardCharges = entity.CreditCardCharges,
                BrokerageFee = entity.BrokerageFee,
                TotalCommission = entity.TotalCommission,
                NetTotalAmount = entity.NetTotalAmount,
                SoaReceivedDate = entity.SoaReceivedDate,
                BordereauxReceivedDate = entity.BordereauxReceivedDate,
                StatementStatus = entity.StatementStatus,
                Remarks1 = entity.Remarks1,
                CurrencyCode = entity.CurrencyCode,
                CurrencyRate = entity.CurrencyRate,
                SoaStatus = entity.SoaStatus,
                ConfirmationDate = entity.ConfirmationDate,
                MappingStatus = entity.MappingStatus,
                Error = entity.Errors,
            };
        }
    }

    public class SoaDataExcelViewModel
    {
        public string CurrencyCode { get; set; }

        public double? CurrencyRate { get; set; }

        public string TreatyCode { get; set; }

        public int? RiskMonth { get; set; }

        public string SoaQuarter { get; set; }

        public string RiskQuarter { get; set; }

        public double? NBPremium { get; set; }

        public double? RNPremium { get; set; }

        public double? ALTPremium { get; set; }

        public double? GrossPremium { get; set; }

        public double? TotalDiscount { get; set; }

        public int? RiskPremium { get; set; }

        public double? NoClaimBonus { get; set; }

        public double? Levy { get; set; }

        public double? Claim { get; set; }

        public double? ProfitComm { get; set; }

        public double? SurrenderValue { get; set; }

        public double? GST { get; set; }

        public double? MODCOReserveIncome { get; set; }

        public double? RiDeposit { get; set; }

        public double? DatabaseCommission { get; set; }

        public double? AdministrationContribution { get; set; }

        public double? ShareOfRiCommissionFromCompulsoryCession { get; set; }

        public double? RecaptureFee { get; set; }

        public double? CreditCardCharges { get; set; }

        public double? BrokerageFee { get; set; }

        public double? NetTotalAmount { get; set; }

        public DateTime? SoaReceivedDate { get; set; }

        public DateTime? BordereauxReceivedDate { get; set; }

        public int? StatementStatus { get; set; }

        public string Remarks1 { get; set; }
    }

    public class SoaDataValidationListingViewModel
    {
        public IList<SoaDataValidationViewModel> SoaDatas { get; set; }

        public IList<SoaDataValidationViewModel> RiSummarys { get; set; }

        public IList<SoaDataValidationViewModel> Differences { get; set; }
    }

    public class SoaDataValidationViewModel
    {
        public int Id { get; set; }
        public string BusinessCode { get; set; }
        public string TreatyId { get; set; }
        public string TreatyCode { get; set; }
        public int? RiskMonth { get; set; }
        public string SoaQuarter { get; set; }
        public string RiskQuarter { get; set; }
        public double? NbPremium { get; set; }
        public double? RnPremium { get; set; }
        public double? AltPremium { get; set; }
        public double? GrossPremium { get; set; }
        public double? TotalDiscount { get; set; }
        public double? NoClaimBonus { get; set; }
        public double? Claim { get; set; }
        public double? SurrenderValue { get; set; }
        public double? Gst { get; set; }
        public double? NetTotalAmount { get; set; }

        public string CurrencyCode { get; set; }
        public double? CurrencyRate { get; set; }

        public static Expression<Func<SoaDataRiDataSummaryBo, SoaDataValidationViewModel>> ExpressionRi()
        {
            return entity => new SoaDataValidationViewModel
            {
                Id = entity.Id,
                BusinessCode = entity.BusinessCode,
                TreatyId = entity.TreatyId,
                TreatyCode = entity.TreatyCode,
                RiskMonth = entity.RiskMonth,
                SoaQuarter = entity.SoaQuarter,
                RiskQuarter = entity.RiskQuarter,
                NbPremium = entity.NbPremium,
                RnPremium = entity.RnPremium,
                AltPremium = entity.AltPremium,
                GrossPremium = entity.GrossPremium,
                TotalDiscount = entity.TotalDiscount,
                NoClaimBonus = entity.NoClaimBonus,
                Claim = entity.Claim,
                SurrenderValue = entity.SurrenderValue,
                Gst = entity.Gst,
                NetTotalAmount = entity.NetTotalAmount,
                CurrencyCode = entity.CurrencyCode,
                CurrencyRate = entity.CurrencyRate,
            };
        }
    }

    public class SoaDataPostValidationListingViewModel
    {
        public IList<SoaDataPostValidationViewModel> MLReCheckings { get; set; }

        public IList<SoaDataPostValidationViewModel> CedantAmounts { get; set; }

        public IList<SoaDataPostValidationViewModel> DiscrepancyChecks { get; set; }

        public IList<SoaDataPostValidationDifferencesViewModel> Differences { get; set; }

        public IList<SoaDataPostValidationDiscrepancyViewModel> Discrepancies { get; set; }
    }

    public class SoaDataPostValidationViewModel
    {
        public int Id { get; set; }
        public int Type { get; set; }
        public string TreatyCode { get; set; }
        public string SoaQuarter { get; set; }
        public string RiskQuarter { get; set; }
        public int? RiskMonth { get; set; }
        public double? NbPremium { get; set; }
        public double? RnPremium { get; set; }
        public double? AltPremium { get; set; }
        public double? GrossPremium { get; set; }
        public double? NbDiscount { get; set; }
        public double? RnDiscount { get; set; }
        public double? AltDiscount { get; set; }
        public double? TotalDiscount { get; set; }
        public double? NoClaimBonus { get; set; }
        public double? SurrenderValue { get; set; }
        public double? NetTotalAmount { get; set; }
        public int? NbCession { get; set; }
        public int? RnCession { get; set; }
        public int? AltCession { get; set; }
        public double? NbSar { get; set; }
        public double? RnSar { get; set; }
        public double? AltSar { get; set; }
        public double? DTH { get; set; }
        public double? TPD { get; set; }
        public double? CI { get; set; }
        public double? PA { get; set; }
        public double? HS { get; set; }

        public string CurrencyCode { get; set; }
        public double? CurrencyRate { get; set; }

        public static Expression<Func<SoaDataPostValidationBo, SoaDataPostValidationViewModel>> Expression()
        {
            return entity => new SoaDataPostValidationViewModel
            {
                Id = entity.Id,
                Type = entity.Type,
                TreatyCode = entity.TreatyCode,
                SoaQuarter = entity.SoaQuarter,
                RiskQuarter = entity.RiskQuarter,
                RiskMonth = entity.RiskMonth,
                NbPremium = entity.NbPremium,
                RnPremium = entity.RnPremium,
                AltPremium = entity.AltPremium,
                GrossPremium = entity.GrossPremium,
                NbDiscount = entity.NbDiscount,
                RnDiscount = entity.RnDiscount,
                AltDiscount = entity.AltDiscount,
                TotalDiscount = entity.TotalDiscount,
                NoClaimBonus = entity.NoClaimBonus,
                SurrenderValue = entity.SurrenderValue,
                NetTotalAmount = entity.NetTotalAmount,
                NbCession = entity.NbCession,
                RnCession = entity.RnCession,
                AltCession = entity.AltCession,
                NbSar = entity.NbSar,
                RnSar = entity.RnSar,
                AltSar = entity.AltSar,
                DTH = entity.DTH,
                TPD = entity.TPD,
                CI = entity.CI,
                PA = entity.PA,
                HS = entity.HS,
                CurrencyCode = entity.CurrencyCode,
                CurrencyRate = entity.CurrencyRate,
            };
        }
    }

    public class SoaDataPostValidationDifferencesViewModel
    {
        public int Id { get; set; }
        public int Type { get; set; }
        public string TreatyCode { get; set; }
        public string SoaQuarter { get; set; }
        public double? GrossPremium { get; set; }
        public double? DifferenceNetTotalAmount { get; set; }
        public double? DifferencePercetage { get; set; }
        public string Remark { get; set; }
        public string Check { get; set; }

        public static Expression<Func<SoaDataPostValidationDifferenceBo, SoaDataPostValidationDifferencesViewModel>> Expression()
        {
            return entity => new SoaDataPostValidationDifferencesViewModel
            {
                Id = entity.Id,
                Type = entity.Type,
                TreatyCode = entity.TreatyCode,
                SoaQuarter = entity.SoaQuarter,
                GrossPremium = entity.GrossPremium,
                DifferenceNetTotalAmount = entity.DifferenceNetTotalAmount,
                DifferencePercetage = entity.DifferencePercetage,
                Remark = entity.Remark,
                Check = entity.Check,
            };
        }
    }

    public class SoaDataPostValidationDiscrepancyViewModel
    {
        public int Id { get; set; }
        public int Type { get; set; }
        public string TreatyCode { get; set; }
        public string CedingPlanCode { get; set; }
        public double? CedantAmount { get; set; }
        public double? MlreChecking { get; set; }
        public double? Discrepancy { get; set; }

        public static Expression<Func<SoaDataDiscrepancyBo, SoaDataPostValidationDiscrepancyViewModel>> Expression()
        {
            return entity => new SoaDataPostValidationDiscrepancyViewModel
            {
                Id = entity.Id,
                Type = entity.Type,
                TreatyCode = entity.TreatyCode,
                CedingPlanCode = entity.CedingPlanCode,
                CedantAmount = entity.CedantAmount,
                MlreChecking = entity.MlreChecking,
                Discrepancy = entity.Discrepancy,
            };
        }
    }
}