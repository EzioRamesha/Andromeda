using BusinessObject.InvoiceRegisters;
using DataAccess.Entities;
using DataAccess.Entities.InvoiceRegisters;
using DataAccess.Entities.SoaDatas;
using PagedList;
using Shared;
using System;
using System.Linq.Expressions;

namespace WebApp.Models
{
    public class InvoiceRegisterViewModel
    {
        //Search results
        public int? SearchResults1Page { get; set; }
        public int? SearchResults2Page { get; set; }
        public IPagedList<InvoiceRegisterListingViewModel> InvoiceRegisters { get; set; }
        public IPagedList<InvoiceRegisterBatchListingViewModel> InvoiceRegisterBatches { get; set; }
        public int? ActiveTab { get; set; }
    }

    public class InvoiceRegisterListingViewModel
    {
        public int Id { get; set; }

        public int InvoiceRegisterBatchId { get; set; }

        public string InvoiceReference { get; set; }

        public string InvoiceNo { get; set; }

        public DateTime? InvoiceDate { get; set; }

        public DateTime? StatementReceivedDate { get; set; }

        public string ClientName { get; set; }

        public string RiskQuarter { get; set; }

        public string AccountsFor { get; set; }

        public double? TotalPaid { get; set; }

        public double? Year1st { get; set; }

        public double? Renewal { get; set; }

        public double? Gross1st { get; set; }

        public double? GrossRenewal { get; set; }

        public int? PreparedById { get; set; }

        public string PreparedByName { get; set; }

        public string InvoiceDateStr { get; set; }

        public string StatementReceivedDateStr { get; set; }

        public string TotalPaidStr { get; set; }

        public string Year1stStr { get; set; }

        public string RenewalStr { get; set; }

        public string Gross1stStr { get; set; }

        public string GrossRenewalStr { get; set; }

        public int? CedantId { get; set; }

        public int? TreatyCodeId { get; set; }

        public int? InvoiceType { get; set; }

        public int? Status { get; set; }

        public string CurrencyCode { get; set; }

        public int? SoaDataBatchId { get; set; }

        public int ReportingType { get; set; }

        public virtual Cedant Cedant { get; set; }

        public virtual TreatyCode TreatyCode { get; set; }

        public virtual SoaDataBatch SoaDataBatch { get; set; }

        public static Expression<Func<InvoiceRegister, InvoiceRegisterListingViewModel>> Expression()
        {
            return entity => new InvoiceRegisterListingViewModel
            {
                Id = entity.Id,
                InvoiceRegisterBatchId = entity.InvoiceRegisterBatchId,
                InvoiceReference = entity.InvoiceReference,
                InvoiceNo = entity.InvoiceNumber,
                InvoiceDate = entity.InvoiceDate,
                StatementReceivedDate = entity.StatementReceivedDate,    
                InvoiceType = entity.InvoiceType,
                CedantId = entity.CedantId,
                TreatyCodeId = entity.TreatyCodeId,
                SoaDataBatchId = entity.SoaDataBatchId,
                Status = entity.SoaDataBatch.InvoiceStatus,
                CurrencyCode = entity.CurrencyCode,
                ReportingType = entity.ReportingType,

                ClientName = entity.Cedant.Name,
                RiskQuarter = entity.RiskQuarter,
                AccountsFor = entity.AccountDescription,

                TotalPaid = entity.TotalPaid,
                Year1st = entity.Year1st,
                Renewal = entity.Renewal,
                Gross1st = entity.Gross1st,
                GrossRenewal = entity.GrossRenewal,
                PreparedById = entity.CreatedById,
                PreparedByName = entity.CreatedBy.FullName,

                Cedant = entity.Cedant,
                TreatyCode = entity.TreatyCode,
                SoaDataBatch = entity.SoaDataBatch,
            };
        }
    }

    public class InvoiceRegisterBatchListingViewModel
    {
        public int Id { get; set; }

        public int? BatchNo { get; set; }

        public DateTime? BatchDate { get; set; }

        public int? TotalInvoice { get; set; }

        public int? PersonInChargeId { get; set; }

        public string PersonInChargeName { get; set; }

        public int? Status { get; set; }

        public string BatchDateStr { get; set; }

        public static Expression<Func<InvoiceRegisterBatch, InvoiceRegisterBatchListingViewModel>> Expression()
        {
            return entity => new InvoiceRegisterBatchListingViewModel
            {
                Id = entity.Id,
                BatchNo = entity.BatchNo,
                BatchDate = entity.BatchDate,
                TotalInvoice = entity.TotalInvoice,
                PersonInChargeId = entity.CreatedById,
                PersonInChargeName = entity.CreatedBy.FullName,
                Status = entity.Status,
            };
        }
    }

    public class InvoiceRegisterDetailViewModel
    {
        public int Id { get; set; }
        public int Status { get; set; } = 0;

        public int InvoiceBatchNo { get; set; }
        public string InvoiceBatchDate { get; set; }

        public string InvoiceType { get; set; }
        public string InvoiceReference { get; set; }
        public string InvoiceNumber { get; set; }
        public string InvoiceDate { get; set; }
        public string StatementReceivedDate { get; set; }

        public string ClientName { get; set; }
        public string PartyCode { get; set; }
        public string RiskQuarter { get; set; }
        public string TreatyId { get; set; }
        public string TreatyCode { get; set; }
        public string TreatyType { get; set; }
        public string Lob { get; set; }
        public string AccountDescription { get; set; }
        public string TotalPaid { get; set; }

        public string PaymentReference { get; set; }
        public string PaymentAmount { get; set; }
        public string PaymentReceivedDate { get; set; }

        public string Year1st { get; set; }
        public string Renewal { get; set; }
        public string Gross1st { get; set; }
        public string GrossRenewal { get; set; }
        public string AltPremium { get; set; }
        public string Discount1st { get; set; }
        public string DiscountRen { get; set; }
        public string DiscountAlt { get; set; }

        public string RiskPremium { get; set; }
        public string Claim { get; set; }
        public string ProfitComm { get; set; }
        public string Levy { get; set; }
        public string SurrenderValue { get; set; }
        public string Gst { get; set; }
        public string ModcoReserveIncome { get; set; }
        public string ReinsDeposit { get; set; }
        public string NoClaimBonus { get; set; }
        public string DatabaseCommission { get; set; }
        public string AdministrationContribution { get; set; }
        public string ShareOfRiCommissionFromCompulsoryCession { get; set; }
        public string RecaptureFee { get; set; }
        public string CreditCardCharges { get; set; }
        public string BrokerageFee { get; set; }

        public int? NbCession { get; set; }
        public string NbSumReins { get; set; }
        public int? RnCession { get; set; }
        public string RnSumReins { get; set; }
        public int? AltCession { get; set; }
        public string AltSumReins { get; set; }

        public string Frequency { get; set; }
        public string PlanName { get; set; }

        public string ValuationGross1st { get; set; }
        public string ValuationGrossRen { get; set; }
        public string ValuationDiscount1st { get; set; }
        public string ValuationDiscountRen { get; set; }
        public string ValuationCom1st { get; set; }
        public string ValuationComRen { get; set; }
        public string ValuationClaims { get; set; }
        public string ValuationProfitCom { get; set; }
        public string ValuationRiskPremium { get; set; }

        public string Remark { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencyRate { get; set; }
        public string SoaQuarter { get; set; }

        public string ReasonOfAdjustment1 { get; set; }
        public string ReasonOfAdjustment2 { get; set; }
        public string InvoiceNumber1 { get; set; }
        public string InvoiceDate1 { get; set; }
        public string Amount1 { get; set; }
        public string InvoiceNumber2 { get; set; }
        public string InvoiceDate2 { get; set; }
        public string Amount2 { get; set; }

        public string PreparedBy { get; set; }

        public string DTH { get; set; }
        public string TPD { get; set; }
        public string CI { get; set; }
        public string PA { get; set; }
        public string HS { get; set; }
        

        //public string ContractCode { get; set; }
        //public int? AnnualCohort { get; set; }

        public InvoiceRegisterDetailViewModel(InvoiceRegisterBo invoiceRegisterBo)
        {
            Set(invoiceRegisterBo);
        }

        public void Set(InvoiceRegisterBo invoiceRegisterBo) 
        {
            if (invoiceRegisterBo.SoaDataBatchId.HasValue)
                Status = invoiceRegisterBo.SoaDataBatchBo.InvoiceStatus;
            InvoiceBatchNo = invoiceRegisterBo.InvoiceRegisterBatchBo.BatchNo;
            InvoiceBatchDate = invoiceRegisterBo.InvoiceRegisterBatchBo.BatchDate.ToString(Util.GetDateFormat());

            InvoiceType = InvoiceRegisterBo.GetInvoiceTypeName(invoiceRegisterBo.InvoiceType);
            InvoiceReference = invoiceRegisterBo.InvoiceReference;
            InvoiceNumber = invoiceRegisterBo.InvoiceNumber;
            InvoiceDate = invoiceRegisterBo.InvoiceDate.ToString(Util.GetDateFormat());
            StatementReceivedDate = invoiceRegisterBo.StatementReceivedDate?.ToString(Util.GetDateFormat());

            if (invoiceRegisterBo.CedantBo != null)
            {
                ClientName = invoiceRegisterBo.CedantBo.Name;
                PartyCode = invoiceRegisterBo.CedantBo.PartyCode;
            }
            if (invoiceRegisterBo.TreatyCodeBo != null)
            {
                TreatyId = invoiceRegisterBo.TreatyCodeBo.TreatyBo.TreatyIdCode;
                TreatyCode = invoiceRegisterBo.TreatyCodeBo.Code;
                if (invoiceRegisterBo.TreatyCodeBo.TreatyTypePickListDetailBo != null)
                    TreatyType = invoiceRegisterBo.TreatyCodeBo.TreatyTypePickListDetailBo.Code;
                if (invoiceRegisterBo.TreatyCodeBo.LineOfBusinessPickListDetailBo != null)
                    Lob = invoiceRegisterBo.TreatyCodeBo.LineOfBusinessPickListDetailBo.Code;
            }
            RiskQuarter = invoiceRegisterBo.RiskQuarter;
            AccountDescription = invoiceRegisterBo.AccountDescription;
            TotalPaid = Util.DoubleToString(invoiceRegisterBo.TotalPaid);

            PaymentReference = invoiceRegisterBo.PaymentReference;
            PaymentAmount = invoiceRegisterBo.PaymentAmount;
            PaymentReceivedDate = invoiceRegisterBo.PaymentReceivedDate?.ToString(Util.GetDateFormat());

            Year1st = Util.DoubleToString(invoiceRegisterBo.Year1st);
            Renewal = Util.DoubleToString(invoiceRegisterBo.Renewal);
            Gross1st = Util.DoubleToString(invoiceRegisterBo.Gross1st);
            GrossRenewal = Util.DoubleToString(invoiceRegisterBo.GrossRenewal);
            AltPremium = Util.DoubleToString(invoiceRegisterBo.AltPremium);
            Discount1st = Util.DoubleToString(invoiceRegisterBo.Discount1st);
            DiscountRen = Util.DoubleToString(invoiceRegisterBo.DiscountRen);
            DiscountAlt = Util.DoubleToString(invoiceRegisterBo.DiscountAlt);

            RiskPremium = Util.DoubleToString(invoiceRegisterBo.RiskPremium);
            NoClaimBonus = Util.DoubleToString(invoiceRegisterBo.NoClaimBonus);
            Levy = Util.DoubleToString(invoiceRegisterBo.Levy);
            Claim = Util.DoubleToString(invoiceRegisterBo.Claim);
            ProfitComm = Util.DoubleToString(invoiceRegisterBo.ProfitComm);
            SurrenderValue = Util.DoubleToString(invoiceRegisterBo.SurrenderValue);
            Gst = Util.DoubleToString(invoiceRegisterBo.Gst);
            ModcoReserveIncome = Util.DoubleToString(invoiceRegisterBo.ModcoReserveIncome);
            ReinsDeposit = Util.DoubleToString(invoiceRegisterBo.ReinsDeposit);
            DatabaseCommission = Util.DoubleToString(invoiceRegisterBo.DatabaseCommission);
            AdministrationContribution = Util.DoubleToString(invoiceRegisterBo.AdministrationContribution);
            ShareOfRiCommissionFromCompulsoryCession = Util.DoubleToString(invoiceRegisterBo.ShareOfRiCommissionFromCompulsoryCession);
            RecaptureFee = Util.DoubleToString(invoiceRegisterBo.RecaptureFee);
            CreditCardCharges = Util.DoubleToString(invoiceRegisterBo.CreditCardCharges);
            BrokerageFee = Util.DoubleToString(invoiceRegisterBo.BrokerageFee);

            NbCession = invoiceRegisterBo.NBCession;
            NbSumReins = Util.DoubleToString(invoiceRegisterBo.NBSumReins);
            RnCession = invoiceRegisterBo.RNCession;
            RnSumReins = Util.DoubleToString(invoiceRegisterBo.RNSumReins);
            AltCession = invoiceRegisterBo.AltCession;
            AltSumReins = Util.DoubleToString(invoiceRegisterBo.AltSumReins);

            Frequency = invoiceRegisterBo.Frequency;            
            PlanName = invoiceRegisterBo.PlanName;

            ValuationGross1st = Util.DoubleToString(invoiceRegisterBo.ValuationGross1st);
            ValuationGrossRen = Util.DoubleToString(invoiceRegisterBo.ValuationGrossRen);
            ValuationDiscount1st = Util.DoubleToString(invoiceRegisterBo.ValuationDiscount1st);
            ValuationDiscountRen = Util.DoubleToString(invoiceRegisterBo.ValuationDiscountRen);
            ValuationCom1st = Util.DoubleToString(invoiceRegisterBo.ValuationCom1st);
            ValuationComRen = Util.DoubleToString(invoiceRegisterBo.ValuationComRen);
            ValuationClaims = Util.DoubleToString(invoiceRegisterBo.ValuationClaims);
            ValuationProfitCom = Util.DoubleToString(invoiceRegisterBo.ValuationProfitCom);
            ValuationRiskPremium = Util.DoubleToString(invoiceRegisterBo.ValuationRiskPremium);

            Remark = invoiceRegisterBo.Remark;
            CurrencyCode = invoiceRegisterBo.CurrencyCode;
            CurrencyRate = Util.DoubleToString(invoiceRegisterBo.CurrencyRate);
            SoaQuarter = invoiceRegisterBo.SoaQuarter;

            ReasonOfAdjustment1 = invoiceRegisterBo.ReasonOfAdjustment1;
            ReasonOfAdjustment2 = invoiceRegisterBo.ReasonOfAdjustment2;
            InvoiceNumber1 = invoiceRegisterBo.InvoiceNumber1;
            InvoiceDate1 = invoiceRegisterBo.InvoiceDate1?.ToString(Util.GetDateFormat());
            Amount1 = Util.DoubleToString(invoiceRegisterBo.Amount1);
            InvoiceNumber2 = invoiceRegisterBo.InvoiceNumber2;
            InvoiceDate2 = invoiceRegisterBo.InvoiceDate2?.ToString(Util.GetDateFormat());
            Amount2 = Util.DoubleToString(invoiceRegisterBo.Amount2);

            PreparedBy = invoiceRegisterBo.PreparedByBo.FullName;
            //ContractCode = invoiceRegisterBo.ContractCode;
            //AnnualCohort = invoiceRegisterBo.AnnualCohort;

            DTH = Util.DoubleToString(invoiceRegisterBo.DTH);
            TPD = Util.DoubleToString(invoiceRegisterBo.TPD);
            CI = Util.DoubleToString(invoiceRegisterBo.CI);
            PA = Util.DoubleToString(invoiceRegisterBo.PA);
            HS = Util.DoubleToString(invoiceRegisterBo.HS);
        }
    }
}