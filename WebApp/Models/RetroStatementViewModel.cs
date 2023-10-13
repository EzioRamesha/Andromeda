﻿using BusinessObject;
using DataAccess.Entities;
using Services;
using Shared;
using Shared.Forms.Attributes;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace WebApp.Models
{
    public class RetroStatementViewModel
    {
        public int Id { get; set; }

        [Required, DisplayName("Direct Retro")]
        public int DirectRetroId { get; set; }

        public DirectRetroBo DirectRetroBo { get; set; }

        public DirectRetro DirectRetro { get; set; }

        [Required, DisplayName("Retro Party")]
        public int RetroPartyId { get; set; }

        public RetroPartyBo RetroPartyBo { get; set; }

        public RetroParty RetroParty { get; set; }

        public int Status { get; set; }

        [StringLength(128), DisplayName("MLRe Ref")]
        public string MlreRef { get; set; }

        [StringLength(255), DisplayName("Ceding Company")]
        public string CedingCompany { get; set; }

        [StringLength(35), DisplayName("Treaty Code")]
        public string TreatyCode { get; set; }

        [StringLength(128), DisplayName("Treaty No")]
        public string TreatyNo { get; set; }

        [StringLength(20), DisplayName("Schedule")]
        public string Schedule { get; set; }

        [StringLength(128), DisplayName("Treaty Type")]
        public string TreatyType { get; set; }

        [StringLength(128), DisplayName("From MLRe To")]
        public string FromMlreTo { get; set; }

        [StringLength(128), DisplayName("Accounts for")]
        public string AccountsFor { get; set; }

        [DisplayName("Date Report Completed")]
        public DateTime? DateReportCompleted { get; set; }

        [DisplayName("Date Send to Retro")]
        public DateTime? DateSendToRetro { get; set; }

        // Data Set 1
        [ValidateQuarter]
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

        public double? NoClaimBonus { get; set; }

        public double? GstPayable { get; set; }

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
        [ValidateQuarter]
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

        public double? NoClaimBonus2 { get; set; }

        public double? GstPayable2 { get; set; }

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
        [ValidateQuarter]
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

        public double? NoClaimBonus3 { get; set; }

        public double? GstPayable3 { get; set; }

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

        // DateTime Str
        [DisplayName("Date Report Completed")]
        public string DateReportCompletedStr { get; set; }

        [DisplayName("Date Send to Retro")]
        public string DateSendToRetroStr { get; set; }

        // Double Str
        // Data Set 1
        [ValidateDouble]
        public string ReserveCededBeginStr { get; set; }

        [ValidateDouble]
        public string ReserveCededEndStr { get; set; }

        [ValidateDouble]
        public string RiskChargeCededBeginStr { get; set; }

        [ValidateDouble]
        public string RiskChargeCededEndStr { get; set; }

        [ValidateDouble]
        public string AverageReserveCededStr { get; set; }

        // Reinsurance Premium
        [ValidateDouble]
        public string RiPremiumNBStr { get; set; }

        [ValidateDouble]
        public string RiPremiumRNStr { get; set; }

        [ValidateDouble]
        public string RiPremiumALTStr { get; set; }

        [ValidateDouble]
        public string QuarterlyRiskPremiumStr { get; set; }

        [ValidateDouble]
        public string RetrocessionMarketingFeeStr { get; set; }

        // Reinsurance Discount
        [ValidateDouble]
        public string RiDiscountNBStr { get; set; }

        [ValidateDouble]
        public string RiDiscountRNStr { get; set; }

        [ValidateDouble]
        public string RiDiscountALTStr { get; set; }

        [ValidateDouble]
        public string AgreedDatabaseCommStr { get; set; }

        [ValidateDouble]
        public string NoClaimBonusStr { get; set; }

        [ValidateDouble]
        public string GstPayableStr { get; set; }

        [ValidateDouble]
        public string ClaimsStr { get; set; }

        [ValidateDouble]
        public string ProfitCommStr { get; set; }

        [ValidateDouble]
        public string SurrenderValueStr { get; set; }

        [ValidateDouble]
        public string PaymentToTheReinsurerStr { get; set; }

        // Total Sum Reinsured
        [ValidateDouble]
        public string TotalSumReinsuredNBStr { get; set; }

        [ValidateDouble]
        public string TotalSumReinsuredRNStr { get; set; }

        [ValidateDouble]
        public string TotalSumReinsuredALTStr { get; set; }

        // Data Set 2
        [ValidateDouble]
        public string ReserveCededBegin2Str { get; set; }

        [ValidateDouble]
        public string ReserveCededEnd2Str { get; set; }

        [ValidateDouble]
        public string RiskChargeCededBegin2Str { get; set; }

        [ValidateDouble]
        public string RiskChargeCededEnd2Str { get; set; }

        [ValidateDouble]
        public string AverageReserveCeded2Str { get; set; }

        // Reinsurance Premium
        [ValidateDouble]
        public string RiPremiumNB2Str { get; set; }

        [ValidateDouble]
        public string RiPremiumRN2Str { get; set; }

        [ValidateDouble]
        public string RiPremiumALT2Str { get; set; }

        [ValidateDouble]
        public string QuarterlyRiskPremium2Str { get; set; }

        [ValidateDouble]
        public string RetrocessionMarketingFee2Str { get; set; }

        // Reinsurance Discount
        [ValidateDouble]
        public string RiDiscountNB2Str { get; set; }

        [ValidateDouble]
        public string RiDiscountRN2Str { get; set; }

        [ValidateDouble]
        public string RiDiscountALT2Str { get; set; }

        [ValidateDouble]
        public string AgreedDatabaseComm2Str { get; set; }

        [ValidateDouble]
        public string NoClaimBonus2Str { get; set; }

        [ValidateDouble]
        public string GstPayable2Str { get; set; }

        [ValidateDouble]
        public string Claims2Str { get; set; }

        [ValidateDouble]
        public string ProfitComm2Str { get; set; }

        [ValidateDouble]
        public string SurrenderValue2Str { get; set; }

        [ValidateDouble]
        public string PaymentToTheReinsurer2Str { get; set; }

        // Total Sum Reinsured
        [ValidateDouble]
        public string TotalSumReinsuredNB2Str { get; set; }

        [ValidateDouble]
        public string TotalSumReinsuredRN2Str { get; set; }

        [ValidateDouble]
        public string TotalSumReinsuredALT2Str { get; set; }

        // Data Set 3
        [ValidateDouble]
        public string ReserveCededBegin3Str { get; set; }

        [ValidateDouble]
        public string ReserveCededEnd3Str { get; set; }

        [ValidateDouble]
        public string RiskChargeCededBegin3Str { get; set; }

        [ValidateDouble]
        public string RiskChargeCededEnd3Str { get; set; }

        [ValidateDouble]
        public string AverageReserveCeded3Str { get; set; }

        // Reinsurance Premium
        [ValidateDouble]
        public string RiPremiumNB3Str { get; set; }

        [ValidateDouble]
        public string RiPremiumRN3Str { get; set; }

        [ValidateDouble]
        public string RiPremiumALT3Str { get; set; }

        [ValidateDouble]
        public string QuarterlyRiskPremium3Str { get; set; }

        [ValidateDouble]
        public string RetrocessionMarketingFee3Str { get; set; }

        // Reinsurance Discount
        [ValidateDouble]
        public string RiDiscountNB3Str { get; set; }

        [ValidateDouble]
        public string RiDiscountRN3Str { get; set; }

        [ValidateDouble]
        public string RiDiscountALT3Str { get; set; }

        [ValidateDouble]
        public string AgreedDatabaseComm3Str { get; set; }

        [ValidateDouble]
        public string NoClaimBonus3Str { get; set; }

        [ValidateDouble]
        public string GstPayable3Str { get; set; }

        [ValidateDouble]
        public string Claims3Str { get; set; }

        [ValidateDouble]
        public string ProfitComm3Str { get; set; }

        [ValidateDouble]
        public string SurrenderValue3Str { get; set; }

        [ValidateDouble]
        public string PaymentToTheReinsurer3Str { get; set; }

        // Total Sum Reinsured
        [ValidateDouble]
        public string TotalSumReinsuredNB3Str { get; set; }

        [ValidateDouble]
        public string TotalSumReinsuredRN3Str { get; set; }

        [ValidateDouble]
        public string TotalSumReinsuredALT3Str { get; set; }

        public RetroStatementViewModel()
        {
            Set();
        }

        public RetroStatementViewModel(RetroStatementBo retroStatementBo)
        {
            Set(retroStatementBo);
        }

        public void Set(RetroStatementBo bo = null)
        {
            if (bo != null)
            {
                Id = bo.Id;
                DirectRetroId = bo.DirectRetroId;
                RetroPartyId = bo.RetroPartyId;
                Status = bo.Status;
                MlreRef = bo.MlreRef;
                CedingCompany = bo.CedingCompany;
                TreatyCode = bo.TreatyCode;
                TreatyNo = bo.TreatyNo;
                Schedule = bo.Schedule;
                TreatyType = bo.TreatyType;
                FromMlreTo = bo.FromMlreTo;
                AccountsFor = bo.AccountsFor;
                DateReportCompleted = bo.DateReportCompleted;
                DateSendToRetro = bo.DateSendToRetro;

                // Data Set 1
                AccountingPeriod = bo.AccountingPeriod;
                ReserveCededBegin = bo.ReserveCededBegin;
                ReserveCededEnd = bo.ReserveCededEnd;
                RiskChargeCededBegin = bo.RiskChargeCededBegin;
                RiskChargeCededEnd = bo.RiskChargeCededEnd;
                AverageReserveCeded = bo.AverageReserveCeded;
                RiPremiumNB = bo.RiPremiumNB;
                RiPremiumRN = bo.RiPremiumRN;
                RiPremiumALT = bo.RiPremiumALT;
                QuarterlyRiskPremium = bo.QuarterlyRiskPremium;
                RetrocessionMarketingFee = bo.RetrocessionMarketingFee;
                RiDiscountNB = bo.RiDiscountNB;
                RiDiscountRN = bo.RiDiscountRN;
                RiDiscountALT = bo.RiDiscountALT;
                AgreedDatabaseComm = bo.AgreedDatabaseComm;
                NoClaimBonus = bo.NoClaimBonus;
                GstPayable = bo.GstPayable;
                Claims = bo.Claims;
                ProfitComm = bo.ProfitComm;
                SurrenderValue = bo.SurrenderValue;
                PaymentToTheReinsurer = bo.PaymentToTheReinsurer;
                TotalNoOfPolicyNB = bo.TotalNoOfPolicyNB;
                TotalNoOfPolicyRN = bo.TotalNoOfPolicyRN;
                TotalNoOfPolicyALT = bo.TotalNoOfPolicyALT;
                TotalSumReinsuredNB = bo.TotalSumReinsuredNB;
                TotalSumReinsuredRN = bo.TotalSumReinsuredRN;
                TotalSumReinsuredALT = bo.TotalSumReinsuredALT;

                // Data Set 2
                AccountingPeriod2 = bo.AccountingPeriod2;
                ReserveCededBegin2 = bo.ReserveCededBegin2;
                ReserveCededEnd2 = bo.ReserveCededEnd2;
                RiskChargeCededBegin2 = bo.RiskChargeCededBegin2;
                RiskChargeCededEnd2 = bo.RiskChargeCededEnd2;
                AverageReserveCeded2 = bo.AverageReserveCeded2;
                RiPremiumNB2 = bo.RiPremiumNB2;
                RiPremiumRN2 = bo.RiPremiumRN2;
                RiPremiumALT2 = bo.RiPremiumALT2;
                QuarterlyRiskPremium2 = bo.QuarterlyRiskPremium2;
                RetrocessionMarketingFee2 = bo.RetrocessionMarketingFee2;
                RiDiscountNB2 = bo.RiDiscountNB2;
                RiDiscountRN2 = bo.RiDiscountRN2;
                RiDiscountALT2 = bo.RiDiscountALT2;
                AgreedDatabaseComm2 = bo.AgreedDatabaseComm2;
                GstPayable2 = bo.GstPayable2;
                NoClaimBonus2 = bo.NoClaimBonus2;
                Claims2 = bo.Claims2;
                ProfitComm2 = bo.ProfitComm2;
                SurrenderValue2 = bo.SurrenderValue2;
                PaymentToTheReinsurer2 = bo.PaymentToTheReinsurer2;
                TotalNoOfPolicyNB2 = bo.TotalNoOfPolicyNB2;
                TotalNoOfPolicyRN2 = bo.TotalNoOfPolicyRN2;
                TotalNoOfPolicyALT2 = bo.TotalNoOfPolicyALT2;
                TotalSumReinsuredNB2 = bo.TotalSumReinsuredNB2;
                TotalSumReinsuredRN2 = bo.TotalSumReinsuredRN2;
                TotalSumReinsuredALT2 = bo.TotalSumReinsuredALT2;

                // Data Set 3
                AccountingPeriod3 = bo.AccountingPeriod3;
                ReserveCededBegin3 = bo.ReserveCededBegin3;
                ReserveCededEnd3 = bo.ReserveCededEnd3;
                RiskChargeCededBegin3 = bo.RiskChargeCededBegin3;
                RiskChargeCededEnd3 = bo.RiskChargeCededEnd3;
                AverageReserveCeded3 = bo.AverageReserveCeded3;
                RiPremiumNB3 = bo.RiPremiumNB3;
                RiPremiumRN3 = bo.RiPremiumRN3;
                RiPremiumALT3 = bo.RiPremiumALT3;
                QuarterlyRiskPremium3 = bo.QuarterlyRiskPremium3;
                RetrocessionMarketingFee3 = bo.RetrocessionMarketingFee3;
                RiDiscountNB3 = bo.RiDiscountNB3;
                RiDiscountRN3 = bo.RiDiscountRN3;
                RiDiscountALT3 = bo.RiDiscountALT3;
                AgreedDatabaseComm3 = bo.AgreedDatabaseComm3;
                GstPayable3 = bo.GstPayable3;
                NoClaimBonus3 = bo.NoClaimBonus3;
                Claims3 = bo.Claims3;
                ProfitComm3 = bo.ProfitComm3;
                SurrenderValue3 = bo.SurrenderValue3;
                PaymentToTheReinsurer3 = bo.PaymentToTheReinsurer3;
                TotalNoOfPolicyNB3 = bo.TotalNoOfPolicyNB3;
                TotalNoOfPolicyRN3 = bo.TotalNoOfPolicyRN3;
                TotalNoOfPolicyALT3 = bo.TotalNoOfPolicyALT3;
                TotalSumReinsuredNB3 = bo.TotalSumReinsuredNB3;
                TotalSumReinsuredRN3 = bo.TotalSumReinsuredRN3;
                TotalSumReinsuredALT3 = bo.TotalSumReinsuredALT3;

                // Datetime Str
                DateReportCompletedStr = bo.DateReportCompleted?.ToString(Util.GetDateFormat());
                DateSendToRetroStr = bo.DateSendToRetro?.ToString(Util.GetDateFormat());

                // Double Str
                // Data Set 1
                ReserveCededBeginStr = Util.DoubleToString(bo.ReserveCededBegin, 2);
                ReserveCededEndStr = Util.DoubleToString(bo.ReserveCededEnd, 2);
                RiskChargeCededBeginStr = Util.DoubleToString(bo.RiskChargeCededBegin, 2);
                RiskChargeCededEndStr = Util.DoubleToString(bo.RiskChargeCededEnd, 2);
                AverageReserveCededStr = Util.DoubleToString(bo.AverageReserveCeded, 2);
                RiPremiumNBStr = Util.DoubleToString(bo.RiPremiumNB, 2);
                RiPremiumRNStr = Util.DoubleToString(bo.RiPremiumRN, 2);
                RiPremiumALTStr = Util.DoubleToString(bo.RiPremiumALT, 2);
                QuarterlyRiskPremiumStr = Util.DoubleToString(bo.QuarterlyRiskPremium, 2);
                RetrocessionMarketingFeeStr = Util.DoubleToString(bo.RetrocessionMarketingFee, 2);
                RiDiscountNBStr = Util.DoubleToString(bo.RiDiscountNB, 2);
                RiDiscountRNStr = Util.DoubleToString(bo.RiDiscountRN, 2);
                RiDiscountALTStr = Util.DoubleToString(bo.RiDiscountALT, 2);
                AgreedDatabaseCommStr = Util.DoubleToString(bo.AgreedDatabaseComm, 2);
                GstPayableStr = Util.DoubleToString(bo.GstPayable, 2);
                NoClaimBonusStr = Util.DoubleToString(bo.NoClaimBonus, 2);
                ClaimsStr = Util.DoubleToString(bo.Claims, 2);
                ProfitCommStr = Util.DoubleToString(bo.ProfitComm, 2);
                SurrenderValueStr = Util.DoubleToString(bo.SurrenderValue, 2);
                PaymentToTheReinsurerStr = Util.DoubleToString(bo.PaymentToTheReinsurer, 2);
                TotalSumReinsuredNBStr = Util.DoubleToString(bo.TotalSumReinsuredNB, 2);
                TotalSumReinsuredRNStr = Util.DoubleToString(bo.TotalSumReinsuredRN, 2);
                TotalSumReinsuredALTStr = Util.DoubleToString(bo.TotalSumReinsuredALT, 2);

                // Data Set 2
                ReserveCededBegin2Str = Util.DoubleToString(bo.ReserveCededBegin2, 2);
                ReserveCededEnd2Str = Util.DoubleToString(bo.ReserveCededEnd2, 2);
                RiskChargeCededBegin2Str = Util.DoubleToString(bo.RiskChargeCededBegin2, 2);
                RiskChargeCededEnd2Str = Util.DoubleToString(bo.RiskChargeCededEnd2, 2);
                AverageReserveCeded2Str = Util.DoubleToString(bo.AverageReserveCeded2, 2);
                RiPremiumNB2Str = Util.DoubleToString(bo.RiPremiumNB2, 2);
                RiPremiumRN2Str = Util.DoubleToString(bo.RiPremiumRN2, 2);
                RiPremiumALT2Str = Util.DoubleToString(bo.RiPremiumALT2, 2);
                QuarterlyRiskPremium2Str = Util.DoubleToString(bo.QuarterlyRiskPremium2, 2);
                RetrocessionMarketingFee2Str = Util.DoubleToString(bo.RetrocessionMarketingFee2, 2);
                RiDiscountNB2Str = Util.DoubleToString(bo.RiDiscountNB2, 2);
                RiDiscountRN2Str = Util.DoubleToString(bo.RiDiscountRN2, 2);
                RiDiscountALT2Str = Util.DoubleToString(bo.RiDiscountALT2, 2);
                AgreedDatabaseComm2Str = Util.DoubleToString(bo.AgreedDatabaseComm2, 2);
                GstPayable2Str = Util.DoubleToString(bo.GstPayable2, 2);
                NoClaimBonus2Str = Util.DoubleToString(bo.NoClaimBonus2, 2);
                Claims2Str = Util.DoubleToString(bo.Claims2, 2);
                ProfitComm2Str = Util.DoubleToString(bo.ProfitComm2, 2);
                SurrenderValue2Str = Util.DoubleToString(bo.SurrenderValue2, 2);
                PaymentToTheReinsurer2Str = Util.DoubleToString(bo.PaymentToTheReinsurer2, 2);
                TotalSumReinsuredNB2Str = Util.DoubleToString(bo.TotalSumReinsuredNB2, 2);
                TotalSumReinsuredRN2Str = Util.DoubleToString(bo.TotalSumReinsuredRN2, 2);
                TotalSumReinsuredALT2Str = Util.DoubleToString(bo.TotalSumReinsuredALT2, 2);

                // Data Set 2
                ReserveCededBegin3Str = Util.DoubleToString(bo.ReserveCededBegin3, 2);
                ReserveCededEnd3Str = Util.DoubleToString(bo.ReserveCededEnd3, 2);
                RiskChargeCededBegin3Str = Util.DoubleToString(bo.RiskChargeCededBegin3, 2);
                RiskChargeCededEnd3Str = Util.DoubleToString(bo.RiskChargeCededEnd3, 2);
                AverageReserveCeded3Str = Util.DoubleToString(bo.AverageReserveCeded3, 2);
                RiPremiumNB3Str = Util.DoubleToString(bo.RiPremiumNB3, 2);
                RiPremiumRN3Str = Util.DoubleToString(bo.RiPremiumRN3, 2);
                RiPremiumALT3Str = Util.DoubleToString(bo.RiPremiumALT3, 2);
                QuarterlyRiskPremium3Str = Util.DoubleToString(bo.QuarterlyRiskPremium3, 2);
                RetrocessionMarketingFee3Str = Util.DoubleToString(bo.RetrocessionMarketingFee3, 2);
                RiDiscountNB3Str = Util.DoubleToString(bo.RiDiscountNB3, 2);
                RiDiscountRN3Str = Util.DoubleToString(bo.RiDiscountRN3, 2);
                RiDiscountALT3Str = Util.DoubleToString(bo.RiDiscountALT3, 2);
                AgreedDatabaseComm3Str = Util.DoubleToString(bo.AgreedDatabaseComm3, 2);
                GstPayable3Str = Util.DoubleToString(bo.GstPayable3, 2);
                NoClaimBonus3Str = Util.DoubleToString(bo.NoClaimBonus3, 2);
                Claims3Str = Util.DoubleToString(bo.Claims3, 2);
                ProfitComm3Str = Util.DoubleToString(bo.ProfitComm3, 2);
                SurrenderValue3Str = Util.DoubleToString(bo.SurrenderValue3, 2);
                PaymentToTheReinsurer3Str = Util.DoubleToString(bo.PaymentToTheReinsurer3, 2);
                TotalSumReinsuredNB3Str = Util.DoubleToString(bo.TotalSumReinsuredNB3, 2);
                TotalSumReinsuredRN3Str = Util.DoubleToString(bo.TotalSumReinsuredRN3, 2);
                TotalSumReinsuredALT3Str = Util.DoubleToString(bo.TotalSumReinsuredALT3, 2);
            }
        }

        public RetroStatementBo FormBo(int createdById, int updatedById)
        {
            var bo = new RetroStatementBo
            {
                Id = Id,
                DirectRetroId = DirectRetroId,
                DirectRetroBo = DirectRetroService.Find(DirectRetroId),
                Status = Status,
                RetroPartyId = RetroPartyId,
                RetroPartyBo = RetroPartyService.Find(RetroPartyId),
                MlreRef = MlreRef,
                CedingCompany = CedingCompany,
                TreatyCode = TreatyCode,
                TreatyNo = TreatyNo,
                Schedule = Schedule,
                TreatyType = TreatyType,
                FromMlreTo = FromMlreTo,
                AccountsFor = AccountsFor,

                DateReportCompleted = Util.GetParseDateTime(DateReportCompletedStr),
                DateSendToRetro = Util.GetParseDateTime(DateSendToRetroStr),

                // Data Set 1
                AccountingPeriod = AccountingPeriod,
                ReserveCededBegin = Util.StringToDouble(ReserveCededBeginStr, true, 2),
                ReserveCededEnd = Util.StringToDouble(ReserveCededEndStr, true, 2),
                RiskChargeCededBegin = Util.StringToDouble(RiskChargeCededBeginStr, true, 2),
                RiskChargeCededEnd = Util.StringToDouble(RiskChargeCededEndStr, true, 2),
                AverageReserveCeded = Util.StringToDouble(AverageReserveCededStr, true, 2),
                RiPremiumNB = Util.StringToDouble(RiPremiumNBStr, true, 2),
                RiPremiumRN = Util.StringToDouble(RiPremiumRNStr, true, 2),
                RiPremiumALT = Util.StringToDouble(RiPremiumALTStr, true, 2),
                QuarterlyRiskPremium = Util.StringToDouble(QuarterlyRiskPremiumStr, true, 2),
                RetrocessionMarketingFee = Util.StringToDouble(RetrocessionMarketingFeeStr, true, 2),
                RiDiscountNB = Util.StringToDouble(RiDiscountNBStr, true, 2),
                RiDiscountRN = Util.StringToDouble(RiDiscountRNStr, true, 2),
                RiDiscountALT = Util.StringToDouble(RiDiscountALTStr, true, 2),
                GstPayable = Util.StringToDouble(GstPayableStr, true, 2),
                AgreedDatabaseComm = Util.StringToDouble(AgreedDatabaseCommStr, true, 2),
                NoClaimBonus = Util.StringToDouble(NoClaimBonusStr, true, 2),
                Claims = Util.StringToDouble(ClaimsStr, true, 2),
                ProfitComm = Util.StringToDouble(ProfitCommStr, true, 2),
                SurrenderValue = Util.StringToDouble(SurrenderValueStr, true, 2),
                PaymentToTheReinsurer = Util.StringToDouble(PaymentToTheReinsurerStr, true, 2),
                TotalNoOfPolicyNB = TotalNoOfPolicyNB,
                TotalNoOfPolicyRN = TotalNoOfPolicyRN,
                TotalNoOfPolicyALT = TotalNoOfPolicyALT,
                TotalSumReinsuredNB = Util.StringToDouble(TotalSumReinsuredNBStr, true, 2),
                TotalSumReinsuredRN = Util.StringToDouble(TotalSumReinsuredRNStr, true, 2),
                TotalSumReinsuredALT = Util.StringToDouble(TotalSumReinsuredALTStr, true, 2),

                // Data Set 2
                AccountingPeriod2 = AccountingPeriod2,
                ReserveCededBegin2 = Util.StringToDouble(ReserveCededBegin2Str, true, 2),
                ReserveCededEnd2 = Util.StringToDouble(ReserveCededEnd2Str, true, 2),
                RiskChargeCededBegin2 = Util.StringToDouble(RiskChargeCededBegin2Str, true, 2),
                RiskChargeCededEnd2 = Util.StringToDouble(RiskChargeCededEnd2Str, true, 2),
                AverageReserveCeded2 = Util.StringToDouble(AverageReserveCeded2Str, true, 2),
                RiPremiumNB2 = Util.StringToDouble(RiPremiumNB2Str, true, 2),
                RiPremiumRN2 = Util.StringToDouble(RiPremiumRN2Str, true, 2),
                RiPremiumALT2 = Util.StringToDouble(RiPremiumALT2Str, true, 2),
                QuarterlyRiskPremium2 = Util.StringToDouble(QuarterlyRiskPremium2Str, true, 2),
                RetrocessionMarketingFee2 = Util.StringToDouble(RetrocessionMarketingFee2Str, true, 2),
                RiDiscountNB2 = Util.StringToDouble(RiDiscountNB2Str, true, 2),
                RiDiscountRN2 = Util.StringToDouble(RiDiscountRN2Str, true, 2),
                RiDiscountALT2 = Util.StringToDouble(RiDiscountALT2Str, true, 2),
                GstPayable2 = Util.StringToDouble(GstPayable2Str, true, 2),
                AgreedDatabaseComm2 = Util.StringToDouble(AgreedDatabaseComm2Str, true, 2),
                NoClaimBonus2 = Util.StringToDouble(NoClaimBonus2Str, true, 2),
                Claims2 = Util.StringToDouble(Claims2Str, true, 2),
                ProfitComm2 = Util.StringToDouble(ProfitComm2Str, true, 2),
                SurrenderValue2 = Util.StringToDouble(SurrenderValue2Str, true, 2),
                PaymentToTheReinsurer2 = Util.StringToDouble(PaymentToTheReinsurer2Str, true, 2),
                TotalNoOfPolicyNB2 = TotalNoOfPolicyNB2,
                TotalNoOfPolicyRN2 = TotalNoOfPolicyRN2,
                TotalNoOfPolicyALT2 = TotalNoOfPolicyALT2,
                TotalSumReinsuredNB2 = Util.StringToDouble(TotalSumReinsuredNB2Str, true, 2),
                TotalSumReinsuredRN2 = Util.StringToDouble(TotalSumReinsuredRN2Str, true, 2),
                TotalSumReinsuredALT2 = Util.StringToDouble(TotalSumReinsuredALT2Str, true, 2),

                // Data Set 3
                AccountingPeriod3 = AccountingPeriod3,
                ReserveCededBegin3 = Util.StringToDouble(ReserveCededBegin3Str, true, 2),
                ReserveCededEnd3 = Util.StringToDouble(ReserveCededEnd3Str, true, 2),
                RiskChargeCededBegin3 = Util.StringToDouble(RiskChargeCededBegin3Str, true, 2),
                RiskChargeCededEnd3 = Util.StringToDouble(RiskChargeCededEnd3Str, true, 2),
                AverageReserveCeded3 = Util.StringToDouble(AverageReserveCeded3Str, true, 2),
                RiPremiumNB3 = Util.StringToDouble(RiPremiumNB3Str, true, 2),
                RiPremiumRN3 = Util.StringToDouble(RiPremiumRN3Str, true, 2),
                RiPremiumALT3 = Util.StringToDouble(RiPremiumALT3Str, true, 2),
                QuarterlyRiskPremium3 = Util.StringToDouble(QuarterlyRiskPremium3Str, true, 2),
                RetrocessionMarketingFee3 = Util.StringToDouble(RetrocessionMarketingFee3Str, true, 2),
                RiDiscountNB3 = Util.StringToDouble(RiDiscountNB3Str, true, 2),
                RiDiscountRN3 = Util.StringToDouble(RiDiscountRN3Str, true, 2),
                RiDiscountALT3 = Util.StringToDouble(RiDiscountALT3Str, true, 2),
                GstPayable3 = Util.StringToDouble(GstPayable3Str, true, 2),
                AgreedDatabaseComm3 = Util.StringToDouble(AgreedDatabaseComm3Str, true, 2),
                NoClaimBonus3 = Util.StringToDouble(NoClaimBonus3Str, true, 2),
                Claims3 = Util.StringToDouble(Claims3Str, true, 2),
                ProfitComm3 = Util.StringToDouble(ProfitComm3Str, true, 2),
                SurrenderValue3 = Util.StringToDouble(SurrenderValue3Str, true, 2),
                PaymentToTheReinsurer3 = Util.StringToDouble(PaymentToTheReinsurer3Str, true, 2),
                TotalNoOfPolicyNB3 = TotalNoOfPolicyNB3,
                TotalNoOfPolicyRN3 = TotalNoOfPolicyRN3,
                TotalNoOfPolicyALT3 = TotalNoOfPolicyALT3,
                TotalSumReinsuredNB3 = Util.StringToDouble(TotalSumReinsuredNB3Str, true, 2),
                TotalSumReinsuredRN3 = Util.StringToDouble(TotalSumReinsuredRN3Str, true, 2),
                TotalSumReinsuredALT3 = Util.StringToDouble(TotalSumReinsuredALT3Str, true, 2),

                CreatedById = createdById,
                UpdatedById = updatedById,
            };

            return bo;
        }

        public static Expression<Func<RetroStatement, RetroStatementViewModel>> Expression()
        {
            return entity => new RetroStatementViewModel
            {
                Id = entity.Id,
                DirectRetroId = entity.DirectRetroId,
                DirectRetro = entity.DirectRetro,
                RetroPartyId = entity.RetroPartyId,
                RetroParty = entity.RetroParty,
                MlreRef = entity.MlreRef,
                CedingCompany = entity.CedingCompany,
                TreatyCode = entity.TreatyCode,
                TreatyNo = entity.TreatyNo,
                Schedule = entity.Schedule,
                TreatyType = entity.TreatyType,
                FromMlreTo = entity.FromMlreTo,
                AccountsFor = entity.AccountsFor,
                DateReportCompleted = entity.DateReportCompleted,
                DateSendToRetro = entity.DateSendToRetro,

                // Data Set 1
                AccountingPeriod = entity.AccountingPeriod,
                ReserveCededBegin = entity.ReserveCededBegin,
                ReserveCededEnd = entity.ReserveCededEnd,
                RiskChargeCededBegin = entity.RiskChargeCededBegin,
                RiskChargeCededEnd = entity.RiskChargeCededEnd,
                AverageReserveCeded = entity.AverageReserveCeded,
                RiPremiumNB = entity.RiPremiumNB,
                RiPremiumRN = entity.RiPremiumRN,
                RiPremiumALT = entity.RiPremiumALT,
                QuarterlyRiskPremium = entity.QuarterlyRiskPremium,
                RetrocessionMarketingFee = entity.RetrocessionMarketingFee,
                RiDiscountNB = entity.RiDiscountNB,
                RiDiscountRN = entity.RiDiscountRN,
                RiDiscountALT = entity.RiDiscountALT,
                AgreedDatabaseComm = entity.AgreedDatabaseComm,
                GstPayable = entity.GstPayable,
                NoClaimBonus = entity.NoClaimBonus,
                Claims = entity.Claims,
                ProfitComm = entity.ProfitComm,
                SurrenderValue = entity.SurrenderValue,
                PaymentToTheReinsurer = entity.PaymentToTheReinsurer,
                TotalNoOfPolicyNB = entity.TotalNoOfPolicyNB,
                TotalNoOfPolicyRN = entity.TotalNoOfPolicyRN,
                TotalNoOfPolicyALT = entity.TotalNoOfPolicyALT,
                TotalSumReinsuredNB = entity.TotalSumReinsuredNB,
                TotalSumReinsuredRN = entity.TotalSumReinsuredRN,
                TotalSumReinsuredALT = entity.TotalSumReinsuredALT,

                // Data Set 2
                AccountingPeriod2 = entity.AccountingPeriod2,
                ReserveCededBegin2 = entity.ReserveCededBegin2,
                ReserveCededEnd2 = entity.ReserveCededEnd2,
                RiskChargeCededBegin2 = entity.RiskChargeCededBegin2,
                RiskChargeCededEnd2 = entity.RiskChargeCededEnd2,
                AverageReserveCeded2 = entity.AverageReserveCeded2,
                RiPremiumNB2 = entity.RiPremiumNB2,
                RiPremiumRN2 = entity.RiPremiumRN2,
                RiPremiumALT2 = entity.RiPremiumALT2,
                QuarterlyRiskPremium2 = entity.QuarterlyRiskPremium2,
                RetrocessionMarketingFee2 = entity.RetrocessionMarketingFee2,
                RiDiscountNB2 = entity.RiDiscountNB2,
                RiDiscountRN2 = entity.RiDiscountRN2,
                RiDiscountALT2 = entity.RiDiscountALT2,
                AgreedDatabaseComm2 = entity.AgreedDatabaseComm2,
                GstPayable2 = entity.GstPayable2,
                NoClaimBonus2 = entity.NoClaimBonus2,
                Claims2 = entity.Claims2,
                ProfitComm2 = entity.ProfitComm2,
                SurrenderValue2 = entity.SurrenderValue2,
                PaymentToTheReinsurer2 = entity.PaymentToTheReinsurer2,
                TotalNoOfPolicyNB2 = entity.TotalNoOfPolicyNB2,
                TotalNoOfPolicyRN2 = entity.TotalNoOfPolicyRN2,
                TotalNoOfPolicyALT2 = entity.TotalNoOfPolicyALT2,
                TotalSumReinsuredNB2 = entity.TotalSumReinsuredNB2,
                TotalSumReinsuredRN2 = entity.TotalSumReinsuredRN2,
                TotalSumReinsuredALT2 = entity.TotalSumReinsuredALT2,

                // Data Set 3
                AccountingPeriod3 = entity.AccountingPeriod3,
                ReserveCededBegin3 = entity.ReserveCededBegin3,
                ReserveCededEnd3 = entity.ReserveCededEnd3,
                RiskChargeCededBegin3 = entity.RiskChargeCededBegin3,
                RiskChargeCededEnd3 = entity.RiskChargeCededEnd3,
                AverageReserveCeded3 = entity.AverageReserveCeded3,
                RiPremiumNB3 = entity.RiPremiumNB3,
                RiPremiumRN3 = entity.RiPremiumRN3,
                RiPremiumALT3 = entity.RiPremiumALT3,
                QuarterlyRiskPremium3 = entity.QuarterlyRiskPremium3,
                RetrocessionMarketingFee3 = entity.RetrocessionMarketingFee3,
                RiDiscountNB3 = entity.RiDiscountNB3,
                RiDiscountRN3 = entity.RiDiscountRN3,
                RiDiscountALT3 = entity.RiDiscountALT3,
                AgreedDatabaseComm3 = entity.AgreedDatabaseComm3,
                GstPayable3 = entity.GstPayable3,
                NoClaimBonus3 = entity.NoClaimBonus3,
                Claims3 = entity.Claims3,
                ProfitComm3 = entity.ProfitComm3,
                SurrenderValue3 = entity.SurrenderValue3,
                PaymentToTheReinsurer3 = entity.PaymentToTheReinsurer3,
                TotalNoOfPolicyNB3 = entity.TotalNoOfPolicyNB3,
                TotalNoOfPolicyRN3 = entity.TotalNoOfPolicyRN3,
                TotalNoOfPolicyALT3 = entity.TotalNoOfPolicyALT3,
                TotalSumReinsuredNB3 = entity.TotalSumReinsuredNB3,
                TotalSumReinsuredRN3 = entity.TotalSumReinsuredRN3,
                TotalSumReinsuredALT3 = entity.TotalSumReinsuredALT3,
            };
        }
    }
}