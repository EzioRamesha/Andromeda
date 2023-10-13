using BusinessObject;
using DataAccess.Entities;
using PagedList;
using Services;
using Shared;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace WebApp.Models
{
    public class RetroRegisterViewModel
    {
        //Search criteria
        public RetroRegisterListingViewModel Retro { get; set; }

        public RetroRegisterBatchListingViewModel Batch { get; set; }

        //Search results
        public int? SearchResults1Page { get; set; }
        public int? SearchResults2Page { get; set; }
        public IPagedList<RetroRegisterListingViewModel> RetroRegisters { get; set; }
        public IPagedList<RetroRegisterBatchListingViewModel> RetroRegisterBatches { get; set; }
        public int? ActiveTab { get; set; }
    }

    public class RetroRegisterListingViewModel
    {
        public int Id { get; set; }

        public int? RetroRegisterBatchId { get; set; }

        public int? Type { get; set; }

        public string InvoiceNo { get; set; }

        public DateTime? InvoiceDate { get; set; }

        public DateTime? ReportCompletedDate { get; set; }

        public DateTime? RetroConfirmationDate { get; set; }

        public string ClientName { get; set; }

        public string RetroPartyName { get; set; }

        public string RiskQuarter { get; set; }

        public string AccountsFor { get; set; }

        public string TreatyType { get; set; }

        public double? TotalPaid { get; set; }

        public double? Year1st { get; set; }

        public double? Renewal { get; set; }

        public double? Gross1st { get; set; }

        public double? GrossRenewal { get; set; }

        public int? PreparedById { get; set; }

        public string PreparedByName { get; set; }

        public string InvoiceDateStr { get; set; }

        public string ReportCompletedDateStr { get; set; }

        public string TotalPaidStr { get; set; }

        public string Year1stStr { get; set; }

        public string RenewalStr { get; set; }

        public string Gross1stStr { get; set; }

        public string GrossRenewalStr { get; set; }

        public int? CedantId { get; set; }

        public int? TreatyCodeId { get; set; }

        public int? RetoPartyId { get; set; }

        public int ReportingType { get; set; }

        public int? Status { get; set; }

        public int? DirectRetroId { get; set; }

        public virtual Cedant Cedant { get; set; }

        public virtual TreatyCode TreatyCode { get; set; }

        public virtual DirectRetro DirectRetro { get; set; }

        public static Expression<Func<RetroRegister, RetroRegisterListingViewModel>> Expression()
        {
            return entity => new RetroRegisterListingViewModel
            {
                Id = entity.Id,
                Type = entity.Type,
                RetroRegisterBatchId = entity.RetroRegisterBatchId,
                InvoiceNo = entity.RetroStatementNo,
                InvoiceDate = entity.RetroStatementDate,
                ReportCompletedDate = entity.ReportCompletedDate,
                RetroConfirmationDate = entity.RetroConfirmationDate,
                CedantId = entity.CedantId,
                ClientName = entity.Cedant.Name,
                TreatyCodeId = entity.TreatyCodeId,
                RetoPartyId = entity.RetroPartyId,
                RetroPartyName = entity.RetroParty.Name,
                RiskQuarter = entity.RiskQuarter,
                AccountsFor = entity.AccountFor,
                TreatyType= entity.TreatyType,
                TotalPaid = entity.NetTotalAmount,
                Year1st = entity.Year1st,
                Renewal = entity.Renewal,
                Gross1st = entity.Gross1st,
                GrossRenewal = entity.GrossRen,
                PreparedById = entity.PreparedById,
                PreparedByName = entity.PreparedBy.FullName,
                ReportingType = entity.ReportingType,
                Status = entity.Status,
                DirectRetroId = entity.DirectRetroId,

                Cedant = entity.Cedant,
                TreatyCode = entity.TreatyCode,
                DirectRetro = entity.DirectRetro,
            };
        }
    }

    public class RetroRegisterBatchListingViewModel
    {
        public int Id { get; set; }

        public int? BatchNo { get; set; }

        public DateTime? BatchDate { get; set; }

        public int BatchType { get; set; }

        public int? TotalInvoice { get; set; }

        public int? PersonInChargeId { get; set; }

        public string PersonInChargeName { get; set; }

        public int? Status { get; set; }

        public string BatchDateStr { get; set; }

        public static Expression<Func<RetroRegisterBatch, RetroRegisterBatchListingViewModel>> Expression()
        {
            return entity => new RetroRegisterBatchListingViewModel
            {
                Id = entity.Id,
                BatchNo = entity.BatchNo,
                BatchDate = entity.BatchDate,
                BatchType = entity.Type,
                TotalInvoice = entity.TotalInvoice,
                PersonInChargeId = entity.CreatedById,
                PersonInChargeName = entity.CreatedBy.FullName,
                Status = entity.Status,
            };
        }
    }

    public class RetroRegisterDetailViewModel
    {
        public int Id { get; set; }

        [Required, Display(Name = "Retro Type")]
        public int Type { get; set; }

        public int? Status { get; set; }

        public int RetroBatchNo { get; set; }
        public string RetroBatchDateStr { get; set; }

        [Required, Display(Name = "Invoice Type")]
        public int RetroStatementType { get; set; }
        [Display(Name = "MLRe Ref")]
        public string RetroStatementNo { get; set; }
        [Required, Display(Name = "Invoice Date")]
        public string RetroStatementDateStr { get; set; }
        [Display(Name = "Date Report Completed")]
        public string ReportCompletedDateStr { get; set; }
        [Display(Name = "Date Send to Retro")]
        public string SendToRetroDateStr { get; set; }

        [Display(Name = "Retro Party")]
        public int? RetroPartyId { get; set; }
        [Display(Name = "Risk Quarter")]
        public string RiskQuarter { get; set; }
        [Display(Name = "Ceding Company")]
        public int? CedantId { get; set; }
        [Display(Name = "Treaty Code")]
        public int? TreatyCodeId { get; set; }
        [Display(Name = "Treaty No.")]
        public string TreatyNumber { get; set; }
        public string Schedule { get; set; }
        [Display(Name = "Treaty Type")]
        public string TreatyType { get; set; }
        [Display(Name = "LOB")]
        public string Lob { get; set; }
        [Display(Name = "Account for")]
        public string AccountFor { get; set; }

        [Display(Name = "1st Year")]
        public string Year1stStr { get; set; }
        [Display(Name = "Renewal")]
        public string RenewalStr { get; set; }
        [Display(Name = "Gross - 1st")]
        public string Gross1stStr { get; set; }
        [Display(Name = "Gross - Renewal")]
        public string GrossRenewalStr { get; set; }
        [Display(Name = "Alt - Premium")]
        public string AltPremiumStr { get; set; }
        [Display(Name = "Discount - 1st")]
        public string Discount1stStr { get; set; }
        [Display(Name = "Discount - Renewal")]
        public string DiscountRenewalStr { get; set; }
        [Display(Name = "Discount - Alt")]
        public string DiscountAltStr { get; set; }
        [Display(Name = "Reserve Ceded (Beginning)")]
        public string ReserveCededBeginStr { get; set; }
        [Display(Name = "Reserve Ceded (End)")]
        public string ReserveCededEndStr { get; set; }
        [Display(Name = "Risk Charge Ceded (Beginning)")]
        public string RiskChargeCededBeginStr { get; set; }
        [Display(Name = "Risk Charge Ceded (End)")]
        public string RiskChargeCededEndStr { get; set; }
        [Display(Name = "Average Reserve Ceded")]
        public string AverageReserveCededStr { get; set; }
        [Display(Name = "Risk Premium")]
        public string RiskPremiumStr { get; set; }
        [Display(Name = "Claim")]
        public string ClaimsStr { get; set; }
        [Display(Name = "Profit Commission")]
        public string ProfitCommissionStr { get; set; }
        [Display(Name = "Surrender Value")]
        public string SurrenderValStr { get; set; }
        [Display(Name = "GST")]
        public string GstPayableStr { get; set; }
        [Display(Name = "No Claim Bonus")]
        public string NoClaimBonusStr { get; set; }
        [Display(Name = "Retrocession Marketing Fee")]
        public string RetrocessionMarketingFeeStr { get; set; }
        [Display(Name = "Agreed Database Commission")]
        public string AgreedDbCommissionStr { get; set; }
        [Display(Name = "Net Total Amount")]
        public string NetTotalAmountStr { get; set; }

        [Display(Name = "# of Cession (NB)")]
        public int? NbCession { get; set; }
        [Display(Name = "Sum Reins (NB)")]
        public string NbSumReins { get; set; }
        [Display(Name = "# of Cession (RN)")]
        public int? RnCession { get; set; }
        [Display(Name = "Sum Reins (RN)")]
        public string RnSumReins { get; set; }
        [Display(Name = "# of Cession (ALT)")]
        public int? AltCession { get; set; }
        [Display(Name = "Sum Reins (ALT)")]
        public string AltSumReins { get; set; }

        [Display(Name = "Frequency")]
        public string Frequency { get; set; }
        [Display(Name = "Original SOA Quarter")]
        public string OriginalSoaQuarter { get; set; }
        [Display(Name = "Retro Confirmation Date")]
        public string RetroConfirmationDateStr { get; set; }

        [Display(Name = "Valuation Gross - 1st")]
        public string ValuationGross1stStr { get; set; }
        [Display(Name = "Valuation Gross - Renewal")]
        public string ValuationGrossRenStr { get; set; }
        [Display(Name = "Valuation Discount - 1st")]
        public string ValuationDiscount1stStr { get; set; }
        [Display(Name = "Valuation Discount - Renewal")]
        public string ValuationDiscountRenStr { get; set; }
        [Display(Name = "Valuation Com - 1st")]
        public string ValuationCom1stStr { get; set; }
        [Display(Name = "Valuation Com - Renewal")]
        public string ValuationComRenStr { get; set; }

        public string Remark { get; set; }

        [Display(Name = "Prepared By")]
        public int? PreparedById { get; set; }

        [Display(Name = "Retro Name")]
        public string RetroName { get; set; }

        [Display(Name = "Party Code")]
        public string PartyCode { get; set; }

        public int? DirectRetroId { get; set; }
        public DirectRetroBo DirectRetroBo { get; set; }

        public bool IsGenerateIFRS4 { get; set; } = false;
        public bool IsGenerateIFRS17 { get; set; } = false;

        public RetroRegisterDetailViewModel()
        {
            Set();
        }

        public RetroRegisterDetailViewModel(RetroRegisterBo retroRegisterBo)
        {
            Set(retroRegisterBo);
        }

        public void Set(RetroRegisterBo retroRegisterBo = null)
        {
            if (retroRegisterBo != null)
            {
                Id = retroRegisterBo.Id;
                if (retroRegisterBo.RetroRegisterBatchBo != null)
                {
                    RetroBatchNo = retroRegisterBo.RetroRegisterBatchBo.BatchNo;
                    RetroBatchDateStr = retroRegisterBo.RetroRegisterBatchBo.BatchDate.ToString(Util.GetDateFormat());
                }
                Id = retroRegisterBo.Id;
                Type = retroRegisterBo.Type;
                Status = retroRegisterBo.Status;

                RetroStatementType = retroRegisterBo.RetroStatementType;
                RetroStatementNo = retroRegisterBo.RetroStatementNo;
                RetroStatementDateStr = retroRegisterBo.RetroStatementDate?.ToString(Util.GetDateFormat());
                ReportCompletedDateStr = retroRegisterBo.ReportCompletedDate?.ToString(Util.GetDateFormat());
                SendToRetroDateStr = retroRegisterBo.SendToRetroDate?.ToString(Util.GetDateFormat());

                RetroPartyId = retroRegisterBo.RetroPartyId;
                CedantId = retroRegisterBo.CedantId;                
                if (retroRegisterBo.TreatyCodeBo != null)
                {
                    TreatyCodeId = retroRegisterBo.TreatyCodeId;
                    if (retroRegisterBo.TreatyCodeBo.LineOfBusinessPickListDetailBo != null)
                        Lob = retroRegisterBo.TreatyCodeBo.LineOfBusinessPickListDetailBo.Code;
                }
                if (retroRegisterBo.RetroPartyBo != null)
                {
                    RetroName = retroRegisterBo.RetroPartyBo.Name;
                    PartyCode = retroRegisterBo.RetroPartyBo.Party;
                }
                TreatyType = retroRegisterBo.TreatyType;
                RiskQuarter = retroRegisterBo.RiskQuarter;
                AccountFor = retroRegisterBo.AccountFor;
                TreatyNumber = retroRegisterBo.TreatyNumber;
                Schedule = retroRegisterBo.Schedule;
                Lob = retroRegisterBo.LOB;

                Year1stStr = Util.DoubleToString(retroRegisterBo.Year1st);
                RenewalStr = Util.DoubleToString(retroRegisterBo.Renewal);
                Gross1stStr = Util.DoubleToString(retroRegisterBo.Gross1st);
                GrossRenewalStr = Util.DoubleToString(retroRegisterBo.GrossRen);
                AltPremiumStr = Util.DoubleToString(retroRegisterBo.AltPremium);
                Discount1stStr = Util.DoubleToString(retroRegisterBo.Discount1st);
                DiscountRenewalStr = Util.DoubleToString(retroRegisterBo.DiscountRen);
                DiscountAltStr = Util.DoubleToString(retroRegisterBo.DiscountAlt);
                ReserveCededBeginStr = Util.DoubleToString(retroRegisterBo.ReserveCededBegin);
                ReserveCededEndStr = Util.DoubleToString(retroRegisterBo.ReserveCededEnd);
                RiskChargeCededBeginStr = Util.DoubleToString(retroRegisterBo.RiskChargeCededBegin);
                RiskChargeCededEndStr = Util.DoubleToString(retroRegisterBo.RiskChargeCededEnd);
                AverageReserveCededStr = Util.DoubleToString(retroRegisterBo.AverageReserveCeded);
                RiskPremiumStr = Util.DoubleToString(retroRegisterBo.RiskPremium);
                ClaimsStr = Util.DoubleToString(retroRegisterBo.Claims);
                ProfitCommissionStr = Util.DoubleToString(retroRegisterBo.ProfitCommission);
                SurrenderValStr = Util.DoubleToString(retroRegisterBo.SurrenderVal);
                GstPayableStr = Util.DoubleToString(retroRegisterBo.GstPayable);
                NoClaimBonusStr = Util.DoubleToString(retroRegisterBo.NoClaimBonus);
                RetrocessionMarketingFeeStr = Util.DoubleToString(retroRegisterBo.RetrocessionMarketingFee);
                AgreedDbCommissionStr = Util.DoubleToString(retroRegisterBo.AgreedDBCommission);
                NetTotalAmountStr = Util.DoubleToString(retroRegisterBo.NetTotalAmount);

                NbCession = retroRegisterBo.NbCession;
                NbSumReins = Util.DoubleToString(retroRegisterBo.NbSumReins);
                RnCession = retroRegisterBo.RnCession;
                RnSumReins = Util.DoubleToString(retroRegisterBo.RnSumReins);
                AltCession = retroRegisterBo.AltCession;
                AltSumReins = Util.DoubleToString(retroRegisterBo.AltSumReins);

                Frequency = retroRegisterBo.Frequency;
                OriginalSoaQuarter = retroRegisterBo.OriginalSoaQuarter;
                RetroConfirmationDateStr = retroRegisterBo.RetroConfirmationDate?.ToString(Util.GetDateFormat());

                ValuationGross1stStr = Util.DoubleToString(retroRegisterBo.ValuationGross1st);
                ValuationGrossRenStr = Util.DoubleToString(retroRegisterBo.ValuationGrossRen);
                ValuationDiscount1stStr = Util.DoubleToString(retroRegisterBo.ValuationDiscount1st);
                ValuationDiscountRenStr = Util.DoubleToString(retroRegisterBo.ValuationDiscountRen);
                ValuationCom1stStr = Util.DoubleToString(retroRegisterBo.ValuationCom1st);
                ValuationComRenStr = Util.DoubleToString(retroRegisterBo.ValuationComRen);

                Remark = retroRegisterBo.Remark;
                PreparedById = retroRegisterBo.PreparedById;

                DirectRetroId = retroRegisterBo.DirectRetroId;
                DirectRetroBo = retroRegisterBo.DirectRetroBo;
            }
        }

        public void Get(ref RetroRegisterBo retroRegisterBo)
        {
            retroRegisterBo.Id = Id;
            retroRegisterBo.Type = Type;
            retroRegisterBo.Status = Status;
            //retroRegisterBo.RetroRegisterBatchId = RetroRegisterBatchId;
            retroRegisterBo.RetroStatementType = RetroStatementType;
            retroRegisterBo.RetroStatementNo = RetroStatementNo;
            retroRegisterBo.RetroStatementDate = Util.GetParseDateTime(RetroStatementDateStr);
            retroRegisterBo.ReportCompletedDate = Util.GetParseDateTime(ReportCompletedDateStr);
            retroRegisterBo.SendToRetroDate = Util.GetParseDateTime(SendToRetroDateStr);

            retroRegisterBo.RetroPartyId = RetroPartyId;
            retroRegisterBo.RiskQuarter = RiskQuarter;
            retroRegisterBo.CedantId = CedantId;
            retroRegisterBo.TreatyCodeId = TreatyCodeId;
            retroRegisterBo.TreatyNumber = TreatyNumber;
            retroRegisterBo.Schedule = Schedule;
            retroRegisterBo.TreatyType = TreatyType;
            retroRegisterBo.LOB = Lob;
            retroRegisterBo.AccountFor = AccountFor;

            retroRegisterBo.Year1st = Util.StringToDouble(Year1stStr);
            retroRegisterBo.Renewal = Util.StringToDouble(RenewalStr);
            retroRegisterBo.ReserveCededBegin = Util.StringToDouble(ReserveCededBeginStr);
            retroRegisterBo.ReserveCededEnd = Util.StringToDouble(ReserveCededEndStr);
            retroRegisterBo.RiskChargeCededBegin = Util.StringToDouble(RiskChargeCededBeginStr);
            retroRegisterBo.RiskChargeCededEnd = Util.StringToDouble(RiskChargeCededEndStr);
            retroRegisterBo.AverageReserveCeded = Util.StringToDouble(AverageReserveCededStr);
            retroRegisterBo.Gross1st = Util.StringToDouble(Gross1stStr);
            retroRegisterBo.GrossRen = Util.StringToDouble(GrossRenewalStr);
            retroRegisterBo.AltPremium = Util.StringToDouble(AltPremiumStr);
            retroRegisterBo.Discount1st = Util.StringToDouble(Discount1stStr);
            retroRegisterBo.DiscountRen = Util.StringToDouble(DiscountRenewalStr);
            retroRegisterBo.DiscountAlt = Util.StringToDouble(DiscountAltStr);
            retroRegisterBo.RiskPremium = Util.StringToDouble(RiskPremiumStr);
            retroRegisterBo.Claims = Util.StringToDouble(ClaimsStr);
            retroRegisterBo.ProfitCommission = Util.StringToDouble(ProfitCommissionStr);
            retroRegisterBo.SurrenderVal = Util.StringToDouble(SurrenderValStr);
            retroRegisterBo.GstPayable = Util.StringToDouble(GstPayableStr);
            retroRegisterBo.NoClaimBonus = Util.StringToDouble(NoClaimBonusStr);
            retroRegisterBo.RetrocessionMarketingFee = Util.StringToDouble(RetrocessionMarketingFeeStr);
            retroRegisterBo.AgreedDBCommission = Util.StringToDouble(AgreedDbCommissionStr);
            retroRegisterBo.NetTotalAmount = Util.StringToDouble(NetTotalAmountStr);

            retroRegisterBo.NbCession = NbCession;
            retroRegisterBo.NbSumReins = Util.StringToDouble(NbSumReins);
            retroRegisterBo.RnCession = RnCession;
            retroRegisterBo.RnSumReins = Util.StringToDouble(RnSumReins);
            retroRegisterBo.AltCession = AltCession;
            retroRegisterBo.AltSumReins = Util.StringToDouble(AltSumReins);

            retroRegisterBo.Remark = Remark;
            retroRegisterBo.Frequency = Frequency;
            retroRegisterBo.PreparedById = PreparedById;
            retroRegisterBo.OriginalSoaQuarter = OriginalSoaQuarter;
            retroRegisterBo.RetroConfirmationDate = Util.GetParseDateTime(RetroConfirmationDateStr);

            retroRegisterBo.ValuationGross1st = Util.StringToDouble(ValuationGross1stStr);
            retroRegisterBo.ValuationGrossRen = Util.StringToDouble(ValuationGrossRenStr);
            retroRegisterBo.ValuationDiscount1st = Util.StringToDouble(ValuationDiscount1stStr);
            retroRegisterBo.ValuationDiscountRen = Util.StringToDouble(ValuationDiscountRenStr);
            retroRegisterBo.ValuationCom1st = Util.StringToDouble(ValuationCom1stStr);
            retroRegisterBo.ValuationComRen = Util.StringToDouble(ValuationComRenStr);

            retroRegisterBo.GetYear1st();
            retroRegisterBo.GetRenewal();
            retroRegisterBo.GetValuationGross1st();
            retroRegisterBo.GetValuationGrossRen();
            retroRegisterBo.GetValuationDiscount1st();
            retroRegisterBo.GetValuationDiscountRen();
            retroRegisterBo.GetValuationCom1st();
            retroRegisterBo.GetValuationComRen();
            retroRegisterBo.GetNetTotalAmount();
        }
    }
}