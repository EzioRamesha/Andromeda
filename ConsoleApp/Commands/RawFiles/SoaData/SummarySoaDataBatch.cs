using BusinessObject;
using BusinessObject.Claims;
using BusinessObject.Identity;
using BusinessObject.RiDatas;
using BusinessObject.SoaDatas;
using ConsoleApp.Commands.RawFiles.SoaData.Query;
using DataAccess.Entities.Identity;
using DataAccess.EntityFramework;
using Services;
using Services.Claims;
using Services.Identity;
using Services.RiDatas;
using Services.SoaDatas;
using Shared;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp.Commands.RawFiles.SoaData
{
    public class SummarySoaDataBatch : Command
    {
        public bool Test { get; set; }

        public CacheService CacheService { get; set; }
        public SoaDataBatchBo SoaDataBatchBo { get; set; }
        public List<SoaDataRiDataSummaryBo> SoaDataRiDataSummaryBos { get; set; }
        public List<SoaDataPostValidationBo> SoaDataPostValidationBos { get; set; }
        public List<SoaDataPostValidationDifferenceBo> SoaDataPostValidationDifferenceBos { get; set; }
        public List<SoaDataBo> SoaDataBos { get; set; }
        public List<SoaDataCompiledSummaryBo> SoaDataCompiledSummaryBos { get; set; }
        public List<SoaDataDiscrepancyBo> SoaDataDiscrepancyBos { get; set; }

        public int TreatyId { get; set; }
        public TreatyBo TreatyBo { get; set; }
        public int RiDataBatchId { get; set; }
        public RiDataBatchBo RiDataBatchBo { get; set; }
        public int ClaimDataBatchId { get; set; }
        public ClaimDataBatchBo ClaimDataBatchBo { get; set; }
        public StatusHistoryBo UpdatingStatusHistoryBo { get; set; }
        public ModuleBo ModuleBo { get; set; }
        public bool RiDataAutoCreate { get; set; }
        public bool ClaimDataAutoCreate { get; set; }

        public SummarySoaDataBatch()
        {
            Title = "SummarySoaDataBatch";
            Description = "To summary SOA Data Batch Data Status";
            Options = new string[] {
                "--t|test : Test process data",
            };

            TreatyId = 4;
            RiDataBatchId = 34;
            ClaimDataBatchId = 0;
        }

        public override void Initial()
        {
            base.Initial();

            Test = IsOption("test");
            Log = !Test;

            CacheService = new CacheService();
        }

        public override void Run()
        {
            #region Checking for jobs with status 'Data Updating'
            var failedBos = SoaDataBatchService.GetProcessingFailedByHours(true);
            SoaDataBatchBo failedBo;

            if (failedBos.Count > 0)
            {
                PrintStarting();
                PrintMessage("Failing SummarySoaDataBatch stucked");
                foreach (SoaDataBatchBo eachBo in failedBos)
                {
                    PrintMessage("Failing Id: " + eachBo.Id);
                    PrintMessage();

                    eachBo.DataUpdateStatus = SoaDataBatchBo.DataUpdateStatusDataUpdateFailed;

                    failedBo = eachBo;

                    SoaDataBatchService.Update(ref failedBo);
                }
            }
            #endregion

            try
            {
                if (CutOffService.IsCutOffProcessing())
                {
                    Log = false;
                    PrintMessage(MessageBag.ProcessCannotRunDueToCutOff, true, false);
                    return;
                }
                if (SoaDataBatchService.CountByDataUpdateStatus(SoaDataBatchBo.DataUpdateStatusSubmitForDataUpdate) == 0)
                {
                    Log = false;
                    PrintMessage(MessageBag.NoBatchPendingProcess);
                    return;
                }
                PrintStarting();

                ModuleBo = ModuleService.FindByController(ModuleBo.ModuleController.SoaData.ToString());

                while (LoadSoaDataBatch() != null)
                {
                    CacheService.Load();
                    CacheService.LoadForSoaData(SoaDataBatchBo.CedantId);

                    if (GetProcessCount("Batch") > 0)
                        PrintProcessCount();
                    SetProcessCount("Batch");

                    //if (RiDataBatchBo == null)
                    //{
                    //    SetProcessCount("RI Data Batch Not Linked");
                    //    if (Test)
                    //        break;
                    //    continue;
                    //}

                    DeleteSoaDataRiDataSummary();
                    DeleteSoaDataPostValidation();
                    DeleteSoaDataPostValidationDifferences();
                    DeleteSoaDataDiscrepancy();
                    DeleteSoaDataCompiledSummary();
                    UpdateBatchDataUpdateStatus(SoaDataBatchBo.DataUpdateStatusDataUpdating, MessageBag.SummarySoaDataBatchDataUpdating, true);

                    if (RiDataAutoCreate)
                    {
                        if (SoaDataFileService.GetBySoaDataBatchId(SoaDataBatchBo.Id).Count == 0)
                        {
                            // Auto Created SOA: to generate latest amount for SOA Data which will be populate from RI Data
                            DeleteSoaData();
                            CreateSoaDataByRiData();
                        }
                    }

                    if (ClaimDataAutoCreate)
                    {
                        if (SoaDataFileService.GetBySoaDataBatchId(SoaDataBatchBo.Id).Count == 0)
                        {
                            // Auto Created SOA: to generate latest amount for SOA Data which will be populate from Claim Data
                            DeleteSoaData();
                            if (GetRiDataBatchId() == 0)
                                CreateSoaDataByClaimData();
                            else
                                CreateSoaDataByRiData();
                        }
                    }

                    CreateRiSummaryValidation();
                    CreateRiSummaryValidationIfrs17();
                    CreateRiSummaryValidationByCellNameIfrs17();
                    CreatePostValidation();
                    CreateDiscrepancy();
                    CreateCompiledSummary();
                    CreateCompiledSummaryIFRS17();
                    CreateCompiledSummaryByCellNameIFRS17();

                    Save();
                    UpdateBatchDataUpdateStatus(SoaDataBatchBo.DataUpdateStatusDataUpdateComplete, MessageBag.SummarySoaDataBatchDataUpdateCompleted);

                    if (Test)
                        break;
                }
            }
            catch (Exception e)
            {
                PrintError(e.Message);
            }

            PrintEnding();
        }

        public void CreateRiSummaryValidation()
        {
            bool updateClaim = true;
            SoaDataRiDataSummaryBos = new List<SoaDataRiDataSummaryBo> { };
            using (var db = new AppDbContext(false))
            {
                var riDataGroups = QueryRiDataGroupBy(db);
                var claimDataGroups = QueryRiDataSummaryClaimDataGroupBy(db);

                var soaDataGroups = db.SoaData
                    .Where(q => q.SoaDataBatchId == SoaDataBatchBo.Id)
                    .GroupBy(q => new { q.BusinessCode, q.TreatyCode, q.RiskMonth, q.RiskQuarter, q.CurrencyCode, q.CurrencyRate })
                    .Select(q => new SoaDataRiDataSummaryBo
                    {
                        BusinessCode = q.Key.BusinessCode,
                        TreatyCode = q.Key.TreatyCode,
                        RiskMonth = q.Key.RiskMonth,
                        RiskQuarter = q.Key.RiskQuarter,

                        CurrencyCode = q.Key.CurrencyCode,
                        CurrencyRate = q.Key.CurrencyRate,

                        TotalDiscount = q.Sum(d => d.TotalDiscount),
                        GrossPremium = q.Sum(d => d.GrossPremium),
                        NetTotalAmount = q.Sum(d => d.NetTotalAmount),

                        NbPremium = q.Sum(d => d.NbPremium),
                        RnPremium = q.Sum(d => d.RnPremium),
                        AltPremium = q.Sum(d => d.AltPremium),

                        NoClaimBonus = q.Sum(d => d.NoClaimBonus),
                        Claim = q.Sum(d => d.Claim),
                        SurrenderValue = q.Sum(d => d.SurrenderValue),
                        DatabaseCommission = q.Sum(d => d.DatabaseCommission),
                        BrokerageFee = q.Sum(d => d.BrokerageFee),
                        Gst = q.Sum(d => d.Gst),
                    })
                    .OrderBy(q => q.TreatyCode)
                    .ToList();

                if (!soaDataGroups.IsNullOrEmpty())
                {
                    foreach (var soaDataGroup in soaDataGroups)
                    {
                        soaDataGroup.Type = SoaDataRiDataSummaryBo.TypeSoaData;
                        soaDataGroup.SoaQuarter = FormatQuarter(SoaDataBatchBo.Quarter);
                        SoaDataRiDataSummaryBos.Add(soaDataGroup);
                    }
                }

                if (!riDataGroups.IsNullOrEmpty())
                {
                    var nbs = QueryRiDataSumByTransactionType(db, PickListDetailBo.TransactionTypeCodeNewBusiness);
                    var rns = QueryRiDataSumByTransactionType(db, PickListDetailBo.TransactionTypeCodeRenewal);
                    var als = QueryRiDataSumByTransactionType(db, PickListDetailBo.TransactionTypeCodeAlteration);

                    var nbCountsMonthly = QueryCountRiDataPostValidationTransactionType(db, PickListDetailBo.TransactionTypeCodeNewBusiness);
                    var nbCounts = QueryCountRiDataPostValidationTransactionType(db, PickListDetailBo.TransactionTypeCodeNewBusiness, false);

                    var rnCountsMonthly = QueryCountRiDataPostValidationTransactionType(db, PickListDetailBo.TransactionTypeCodeRenewal);
                    var rnCounts = QueryCountRiDataPostValidationTransactionType(db, PickListDetailBo.TransactionTypeCodeRenewal, false);

                    var alCountsMonthly = QueryCountRiDataPostValidationTransactionType(db, PickListDetailBo.TransactionTypeCodeAlteration);
                    var alCounts = QueryCountRiDataPostValidationTransactionType(db, PickListDetailBo.TransactionTypeCodeAlteration, false);

                    var dths = QueryRiDataPostValidationBenefitCode(db, BenefitBo.CodeDTH);
                    var tpas = QueryRiDataPostValidationBenefitCode(db, BenefitBo.CodeTPA);
                    var tpss = QueryRiDataPostValidationBenefitCode(db, BenefitBo.CodeTPS);
                    var ppds = QueryRiDataPostValidationBenefitCode(db, BenefitBo.CodePPD);
                    var ccas = QueryRiDataPostValidationBenefitCode(db, BenefitBo.CodeCCA);
                    var ccss = QueryRiDataPostValidationBenefitCode(db, BenefitBo.CodeCCS);
                    var pas = QueryRiDataPostValidationBenefitCode(db, BenefitBo.CodePA);
                    var hss = QueryRiDataPostValidationBenefitCode(db, BenefitBo.CodeHS);
                    var tpds = QueryRiDataPostValidationBenefitCode(db, BenefitBo.CodeTPD);
                    var cis = QueryRiDataPostValidationBenefitCode(db, BenefitBo.CodeCI);

                    foreach (var riDataGroup in riDataGroups)
                    {
                        var businessOriginCode = CacheService.BusinessOrigins.Where(q => q.Id == TreatyBo?.BusinessOriginPickListDetailId).Select(q => q.Code).FirstOrDefault();
                        var treatyCode = riDataGroup.TreatyCode;
                        var riskPeriodMonth = riDataGroup.RiskPeriodMonth;
                        var riskPeriodYear = riDataGroup.RiskPeriodYear;
                        var currencyCode = riDataGroup.CurrencyCode;
                        var summary = new SoaDataRiDataSummaryBo
                        {
                            Type = SoaDataRiDataSummaryBo.TypeRiDataSummaryIfrs4,
                            BusinessCode = businessOriginCode,
                            TreatyCode = riDataGroup.TreatyCode,
                            RiskMonth = riDataGroup.RiskPeriodMonth.GetValueOrDefault(),
                            TotalDiscount = riDataGroup.TransactionDiscount.GetValueOrDefault(),
                            RiskQuarter = GetQuarterInfo(riDataGroup.RiskPeriodMonth, riDataGroup.RiskPeriodYear),
                            CurrencyCode = riDataGroup.CurrencyCode,
                            CurrencyRate = SoaDataBatchBo.CurrencyRate,
                            Frequency = riDataGroup.PremiumFrequencyCode,

                            SoaQuarter = FormatQuarter(SoaDataBatchBo.Quarter),
                        };
                        if (string.IsNullOrEmpty(summary.CurrencyCode)) summary.CurrencyCode = SoaDataBatchBo.CurrencyCodePickListDetailBo?.Code;

                        var nb = nbs?.Where(q => q.TreatyCode == treatyCode && q.RiskPeriodMonth == riskPeriodMonth && q.RiskPeriodYear == riskPeriodYear && q.CurrencyCode == currencyCode && q.PremiumFrequencyCode == summary.Frequency).FirstOrDefault();
                        if (nb != null)
                        {
                            summary.NbPremium = nb.TransactionPremium.GetValueOrDefault();
                            summary.NbDiscount = nb.TransactionDiscount.GetValueOrDefault();
                            summary.NbSar = nb.Aar;
                        }

                        var rn = rns?.Where(q => q.TreatyCode == treatyCode && q.RiskPeriodMonth == riskPeriodMonth && q.RiskPeriodYear == riskPeriodYear && q.CurrencyCode == currencyCode && q.PremiumFrequencyCode == summary.Frequency).FirstOrDefault();
                        if (rn != null)
                        {
                            summary.RnPremium = rn.TransactionPremium.GetValueOrDefault();
                            summary.RnDiscount = rn.TransactionDiscount.GetValueOrDefault();
                            summary.RnSar = rn.Aar;
                        }

                        var al = als?.Where(q => q.TreatyCode == treatyCode && q.RiskPeriodMonth == riskPeriodMonth && q.RiskPeriodYear == riskPeriodYear && q.CurrencyCode == currencyCode && q.PremiumFrequencyCode == summary.Frequency).FirstOrDefault();
                        if (al != null)
                        {
                            summary.AltPremium = al.TransactionPremium.GetValueOrDefault();
                            summary.AltDiscount = al.TransactionDiscount.GetValueOrDefault();
                            summary.AltSar = al.Aar;
                        }

                        summary.GetGrossPremium();

                        summary.NoClaimBonus = riDataGroup.NoClaimBonus.GetValueOrDefault();
                        summary.DatabaseCommission = riDataGroup.DatabaseCommission.GetValueOrDefault();
                        summary.BrokerageFee = riDataGroup.BrokerageFee.GetValueOrDefault();
                        summary.ServiceFee = riDataGroup.ServiceFee.GetValueOrDefault();
                        summary.Gst = riDataGroup.GstAmount.GetValueOrDefault();
                        summary.SurrenderValue = riDataGroup.SurrenderValue.GetValueOrDefault();

                        if (summary.RiskQuarter == FormatQuarter(SoaDataBatchBo.Quarter))
                        {
                            var claimDataGroup = claimDataGroups?.Where(q => q.TreatyCode == treatyCode && q.SoaQuarter == SoaDataBatchBo.Quarter).FirstOrDefault();
                            if (claimDataGroup != null && updateClaim)
                            {
                                if (summary.CurrencyCode == PickListDetailBo.CurrencyCodeMyr)
                                    summary.Claim = claimDataGroup.ClaimRecoveryAmt.GetValueOrDefault();
                                else
                                    summary.Claim = claimDataGroup.ForeignClaimRecoveryAmt.GetValueOrDefault();
                                updateClaim = false;
                            }
                        }
                        summary.GetNetTotalAmount();

                        var nbCountMonthly = nbCountsMonthly?.Where(q => q.TreatyCode == treatyCode && q.RiskPeriodMonth == riskPeriodMonth && q.RiskPeriodYear == riskPeriodYear && q.CurrencyCode == currencyCode && q.PremiumFrequencyCode == summary.Frequency).FirstOrDefault();
                        var nbCount = nbCounts?.Where(q => q.TreatyCode == treatyCode && q.RiskPeriodMonth == riskPeriodMonth && q.RiskPeriodYear == riskPeriodYear && q.CurrencyCode == currencyCode && q.PremiumFrequencyCode == summary.Frequency).FirstOrDefault();
                        int? nbCession = 0;
                        if (summary.Frequency == PickListDetailBo.PremiumFrequencyCodeMonthly)
                        {
                            if (nbCountMonthly != null)
                                nbCession += nbCountMonthly.Total;
                        }
                        else
                        {
                            if (nbCount != null)
                                nbCession += nbCount.Total;
                        }                        

                        var rnCountMonthly = rnCountsMonthly?.Where(q => q.TreatyCode == treatyCode && q.RiskPeriodMonth == riskPeriodMonth && q.RiskPeriodYear == riskPeriodYear && q.CurrencyCode == currencyCode && q.PremiumFrequencyCode == summary.Frequency).FirstOrDefault();
                        var rnCount = rnCounts?.Where(q => q.TreatyCode == treatyCode && q.RiskPeriodMonth == riskPeriodMonth && q.RiskPeriodYear == riskPeriodYear && q.CurrencyCode == currencyCode && q.PremiumFrequencyCode == summary.Frequency).FirstOrDefault();
                        int? rnCession = 0;
                        if (summary.Frequency == PickListDetailBo.PremiumFrequencyCodeMonthly)
                        {
                            if (rnCountMonthly != null)
                                rnCession += rnCountMonthly.Total;
                        }
                        else
                        {
                            if (rnCount != null)
                                rnCession += rnCount.Total;
                        }                        

                        var alCountMonthly = alCountsMonthly?.Where(q => q.TreatyCode == treatyCode && q.RiskPeriodMonth == riskPeriodMonth && q.RiskPeriodYear == riskPeriodYear && q.CurrencyCode == currencyCode && q.PremiumFrequencyCode == summary.Frequency).FirstOrDefault();
                        var alCount = alCounts?.Where(q => q.TreatyCode == treatyCode && q.RiskPeriodMonth == riskPeriodMonth && q.RiskPeriodYear == riskPeriodYear && q.CurrencyCode == currencyCode && q.PremiumFrequencyCode == summary.Frequency).FirstOrDefault();
                        int? alCession = 0;
                        if (summary.Frequency == PickListDetailBo.PremiumFrequencyCodeMonthly)
                        {
                            if (alCountMonthly != null)
                                alCession += alCountMonthly.Total;
                        }
                        else
                        {
                            if (alCount != null)
                                alCession += alCount.Total;
                        }

                        summary.NbCession = nbCession.GetValueOrDefault();
                        summary.RnCession = rnCession.GetValueOrDefault();
                        summary.AltCession = alCession.GetValueOrDefault();


                        var dth = dths?.Where(q => q.TreatyCode == treatyCode && q.RiskPeriodMonth == riskPeriodMonth && q.RiskPeriodYear == riskPeriodYear && q.CurrencyCode == currencyCode && q.PremiumFrequencyCode == summary.Frequency).FirstOrDefault();
                        if (dth != null)
                            summary.DTH = dth.TransactionPremium.GetValueOrDefault();

                        var tpa = tpas?.Where(q => q.TreatyCode == treatyCode && q.RiskPeriodMonth == riskPeriodMonth && q.RiskPeriodYear == riskPeriodYear && q.CurrencyCode == currencyCode && q.PremiumFrequencyCode == summary.Frequency).FirstOrDefault();
                        if (tpa != null)
                            summary.TPA = tpa.TransactionPremium.GetValueOrDefault();

                        var tps = tpss?.Where(q => q.TreatyCode == treatyCode && q.RiskPeriodMonth == riskPeriodMonth && q.RiskPeriodYear == riskPeriodYear && q.CurrencyCode == currencyCode && q.PremiumFrequencyCode == summary.Frequency).FirstOrDefault();
                        if (tps != null)
                            summary.TPS = tps.TransactionPremium.GetValueOrDefault();

                        var ppd = ppds?.Where(q => q.TreatyCode == treatyCode && q.RiskPeriodMonth == riskPeriodMonth && q.RiskPeriodYear == riskPeriodYear && q.CurrencyCode == currencyCode && q.PremiumFrequencyCode == summary.Frequency).FirstOrDefault();
                        if (ppd != null)
                            summary.PPD = ppd.TransactionPremium.GetValueOrDefault();

                        var cca = ccas?.Where(q => q.TreatyCode == treatyCode && q.RiskPeriodMonth == riskPeriodMonth && q.RiskPeriodYear == riskPeriodYear && q.CurrencyCode == currencyCode && q.PremiumFrequencyCode == summary.Frequency).FirstOrDefault();
                        if (cca != null)
                            summary.CCA = cca.TransactionPremium.GetValueOrDefault();

                        var ccs = ccss?.Where(q => q.TreatyCode == treatyCode && q.RiskPeriodMonth == riskPeriodMonth && q.RiskPeriodYear == riskPeriodYear && q.CurrencyCode == currencyCode && q.PremiumFrequencyCode == summary.Frequency).FirstOrDefault();
                        if (ccs != null)
                            summary.CCS = ccs.TransactionPremium.GetValueOrDefault();

                        var pa = pas?.Where(q => q.TreatyCode == treatyCode && q.RiskPeriodMonth == riskPeriodMonth && q.RiskPeriodYear == riskPeriodYear && q.CurrencyCode == currencyCode && q.PremiumFrequencyCode == summary.Frequency).FirstOrDefault();
                        if (pa != null)
                            summary.PA = pa.TransactionPremium.GetValueOrDefault();

                        var hs = hss?.Where(q => q.TreatyCode == treatyCode && q.RiskPeriodMonth == riskPeriodMonth && q.RiskPeriodYear == riskPeriodYear && q.CurrencyCode == currencyCode && q.PremiumFrequencyCode == summary.Frequency).FirstOrDefault();
                        if (hs != null)
                            summary.HS = hs.TransactionPremium.GetValueOrDefault();

                        var tpd = tpds?.Where(q => q.TreatyCode == treatyCode && q.RiskPeriodMonth == riskPeriodMonth && q.RiskPeriodYear == riskPeriodYear && q.CurrencyCode == currencyCode && q.PremiumFrequencyCode == summary.Frequency).FirstOrDefault();
                        if (tpd != null)
                            summary.TPD = tpd.TransactionPremium.GetValueOrDefault();

                        var ci = cis?.Where(q => q.TreatyCode == treatyCode && q.RiskPeriodMonth == riskPeriodMonth && q.RiskPeriodYear == riskPeriodYear && q.CurrencyCode == currencyCode && q.PremiumFrequencyCode == summary.Frequency).FirstOrDefault();
                        if (ci != null)
                            summary.CI = ci.TransactionPremium.GetValueOrDefault();

                        SoaDataRiDataSummaryBos.Add(summary);
                        //PrintSoaDataRiDataSummary(summary);

                        if (!string.IsNullOrEmpty(summary.CurrencyCode) && summary.CurrencyCode != PickListDetailBo.CurrencyCodeMyr)
                        {
                            var summaryMYR = new SoaDataRiDataSummaryBo
                            { 
                                Type = SoaDataRiDataSummaryBo.TypeRiDataSummaryIfrs4,
                                BusinessCode = businessOriginCode,
                                TreatyCode = riDataGroup.TreatyCode,
                                RiskMonth = riDataGroup.RiskPeriodMonth.GetValueOrDefault(),
                                TotalDiscount = riDataGroup.TransactionDiscount.GetValueOrDefault(),
                                RiskQuarter = GetQuarterInfo(riDataGroup.RiskPeriodMonth, riDataGroup.RiskPeriodYear),
                                CurrencyCode = PickListDetailBo.CurrencyCodeMyr,
                                CurrencyRate = SoaDataBatchBo.CurrencyRate,
                                Frequency = riDataGroup.PremiumFrequencyCode,
                                SoaQuarter = FormatQuarter(SoaDataBatchBo.Quarter),
                            };
                            summaryMYR.NbCession = summary.NbCession;
                            summaryMYR.RnCession = summary.RnCession;
                            summaryMYR.AltCession = summary.AltCession;

                            summaryMYR.NbPremium = summary.NbPremium * summaryMYR.CurrencyRate;
                            summaryMYR.RnPremium = summary.RnPremium * summaryMYR.CurrencyRate;
                            summaryMYR.AltPremium = summary.AltPremium * summaryMYR.CurrencyRate;

                            summaryMYR.NbDiscount = summary.NbDiscount * summaryMYR.CurrencyRate;
                            summaryMYR.RnDiscount = summary.RnDiscount * summaryMYR.CurrencyRate;
                            summaryMYR.AltDiscount = summary.AltDiscount * summaryMYR.CurrencyRate;

                            summaryMYR.NbSar = summary.NbSar * summaryMYR.CurrencyRate;
                            summaryMYR.RnSar = summary.RnSar * summaryMYR.CurrencyRate;
                            summaryMYR.AltSar = summary.AltSar * summaryMYR.CurrencyRate;

                            summaryMYR.TotalDiscount = summary.TotalDiscount * summaryMYR.CurrencyRate;
                            summaryMYR.NoClaimBonus = summary.NoClaimBonus * summaryMYR.CurrencyRate;
                            summaryMYR.SurrenderValue = summary.SurrenderValue * summaryMYR.CurrencyRate;
                            summaryMYR.DatabaseCommission = summary.DatabaseCommission * summaryMYR.CurrencyRate;
                            summaryMYR.BrokerageFee = summary.BrokerageFee * summaryMYR.CurrencyRate;
                            summaryMYR.ServiceFee = summary.ServiceFee * summaryMYR.CurrencyRate;
                            summaryMYR.Gst = summary.Gst * summaryMYR.CurrencyRate;
                            summaryMYR.Claim = summary.Claim * summaryMYR.CurrencyRate;

                            summaryMYR.DTH = summary.DTH * summaryMYR.CurrencyRate;
                            summaryMYR.TPA = summary.TPA * summaryMYR.CurrencyRate;
                            summaryMYR.TPS = summary.TPS * summaryMYR.CurrencyRate;
                            summaryMYR.PPD = summary.PPD * summaryMYR.CurrencyRate;
                            summaryMYR.CCA = summary.CCA * summaryMYR.CurrencyRate;
                            summaryMYR.CCS = summary.CCS * summaryMYR.CurrencyRate;
                            summaryMYR.PA = summary.PA * summaryMYR.CurrencyRate;
                            summaryMYR.HS = summary.HS * summaryMYR.CurrencyRate;
                            summaryMYR.TPD = summary.TPD * summaryMYR.CurrencyRate;
                            summaryMYR.CI = summary.CI * summaryMYR.CurrencyRate;

                            summaryMYR.GetGrossPremium();
                            summaryMYR.GetNetTotalAmount();

                            SoaDataRiDataSummaryBos.Add(summaryMYR);
                        }

                        //Added update for UpdatedAt for fail checking
                        var boToUpdate = SoaDataBatchBo;
                        SoaDataBatchService.Update(ref boToUpdate);
                    }
                }
                else
                {
                    // Only when Soa Data auto create from Claim Data
                    if (SoaDataBatchBo.IsClaimDataAutoCreate == true && !soaDataGroups.IsNullOrEmpty())
                    {
                        var claimRegisterGroups = QueryClaimDataGroupBy(db);
                        foreach (var soaDataGroup in soaDataGroups)
                        {
                            var summary = new SoaDataRiDataSummaryBo
                            {
                                Type = SoaDataRiDataSummaryBo.TypeRiDataSummaryIfrs4,
                                BusinessCode = soaDataGroup.BusinessCode,
                                TreatyCode = soaDataGroup.TreatyCode,
                                RiskQuarter = soaDataGroup.RiskQuarter,
                                SoaQuarter = FormatQuarter(SoaDataBatchBo.Quarter),
                                CurrencyCode = SoaDataBatchBo.CurrencyCodePickListDetailBo?.Code,
                                CurrencyRate = SoaDataBatchBo.CurrencyRate,
                            };

                            if (summary.RiskQuarter == FormatQuarter(SoaDataBatchBo.Quarter))
                            {
                                var claimDataGroup = claimRegisterGroups?.Where(q => q.TreatyCode == summary.TreatyCode && q.SoaQuarter == SoaDataBatchBo.Quarter).FirstOrDefault();
                                if (claimDataGroup != null && updateClaim)
                                {
                                    if (summary.CurrencyCode == PickListDetailBo.CurrencyCodeMyr)
                                        summary.Claim = claimDataGroup.ClaimRecoveryAmt.GetValueOrDefault();
                                    else
                                        summary.Claim = claimDataGroup.ForeignClaimRecoveryAmt.GetValueOrDefault();
                                    updateClaim = false;
                                }
                            }

                            SoaDataRiDataSummaryBos.Add(summary);

                            if (!string.IsNullOrEmpty(summary.CurrencyCode) && summary.CurrencyCode != PickListDetailBo.CurrencyCodeMyr)
                            {
                                var summaryMYR = new SoaDataRiDataSummaryBo
                                {
                                    Type = SoaDataRiDataSummaryBo.TypeRiDataSummaryIfrs4,
                                    BusinessCode = soaDataGroup.BusinessCode,
                                    TreatyCode = soaDataGroup.TreatyCode,
                                    RiskQuarter = FormatQuarter(soaDataGroup.RiskQuarter),
                                    SoaQuarter = FormatQuarter(SoaDataBatchBo.Quarter),
                                    CurrencyCode = SoaDataBatchBo.CurrencyCodePickListDetailBo?.Code,
                                    CurrencyRate = SoaDataBatchBo.CurrencyRate,
                                };
                                summaryMYR.Claim = summary.Claim * summaryMYR.CurrencyRate;
                                summaryMYR.GetGrossPremium();
                                summaryMYR.GetNetTotalAmount();

                                SoaDataRiDataSummaryBos.Add(summaryMYR);
                            }

                            //Added update for UpdatedAt for fail checking
                            var boToUpdate = SoaDataBatchBo;
                            SoaDataBatchService.Update(ref boToUpdate);
                        }
                    }
                }
            }

            #region GROUPS WITHOUT FREQUENCY CODE
            var RiSummaries = SoaDataRiDataSummaryBos
                .Where(q => q.Type == SoaDataRiDataSummaryBo.TypeRiDataSummaryIfrs4)
                .GroupBy(q => new { q.BusinessCode, q.TreatyCode, q.RiskMonth, q.RiskQuarter, q.CurrencyCode, q.CurrencyRate, q.SoaQuarter })
                .Select(q => new SoaDataRiDataSummaryBo
                {
                    BusinessCode = q.Key.BusinessCode,
                    TreatyCode = q.Key.TreatyCode,
                    RiskMonth = q.Key.RiskMonth,
                    RiskQuarter = q.Key.RiskQuarter,
                    SoaQuarter = q.Key.SoaQuarter,
                    CurrencyCode = q.Key.CurrencyCode,
                    CurrencyRate = q.Key.CurrencyRate,

                    TotalDiscount = q.Sum(d => d.TotalDiscount),

                    NbPremium = q.Sum(d => d.NbPremium),
                    RnPremium = q.Sum(d => d.RnPremium),
                    AltPremium = q.Sum(d => d.AltPremium),

                    NbDiscount = q.Sum(d => d.NbDiscount),
                    RnDiscount = q.Sum(d => d.RnDiscount),
                    AltDiscount = q.Sum(d => d.AltDiscount),

                    NbSar = q.Sum(d => d.NbSar),
                    RnSar = q.Sum(d => d.RnSar),
                    AltSar = q.Sum(d => d.AltSar),

                    NbCession = q.Sum(d => d.NbCession),
                    RnCession = q.Sum(d => d.RnCession),
                    AltCession = q.Sum(d => d.AltCession),

                    NoClaimBonus = q.Sum(d => d.NoClaimBonus),
                    Claim = q.Sum(d => d.Claim),
                    SurrenderValue = q.Sum(d => d.SurrenderValue),
                    DatabaseCommission = q.Sum(d => d.DatabaseCommission),
                    BrokerageFee = q.Sum(d => d.BrokerageFee),
                    Gst = q.Sum(d => d.Gst),
                    ServiceFee = q.Sum(d => d.ServiceFee),

                    DTH = q.Sum(d => d.DTH),
                    TPA = q.Sum(d => d.TPA),
                    TPS = q.Sum(d => d.TPS),
                    PPD = q.Sum(d => d.PPD),
                    CCA = q.Sum(d => d.CCA),
                    CCS = q.Sum(d => d.CCS),
                    PA = q.Sum(d => d.PA),
                    HS = q.Sum(d => d.HS),
                    TPD = q.Sum(d => d.TPD),
                    CI = q.Sum(d => d.CI)
                })
                .ToList();

            if (!RiSummaries.IsNullOrEmpty())
            {
                foreach (var riSummary in RiSummaries)
                {
                    riSummary.Type = SoaDataRiDataSummaryBo.TypeRiDataSummary;
                    riSummary.GetGrossPremium();
                    riSummary.GetNetTotalAmount();
                    SoaDataRiDataSummaryBos.Add(riSummary);
                }
            }
            #endregion

            #region DIFFERENCES
            var validationSoaData = SoaDataRiDataSummaryBos.Where(q => q.Type == SoaDataRiDataSummaryBo.TypeSoaData).ToList();
            var validationRiSummaries = SoaDataRiDataSummaryBos.Where(q => q.Type == SoaDataRiDataSummaryBo.TypeRiDataSummaryIfrs4).ToList();
            CreateValidationDifferences(validationSoaData, validationRiSummaries);
            #endregion
        }

        public void CreateValidationDifferences(IList<SoaDataRiDataSummaryBo> SoaData, IList<SoaDataRiDataSummaryBo> RiSummaries)
        {
            SoaData = SoaData
                .GroupBy(q => new { q.TreatyId, q.SoaQuarter, q.BusinessCode, q.CurrencyCode, q.CurrencyRate })
                .Select(q => new SoaDataRiDataSummaryBo
                {
                    TreatyId = q.Key.TreatyId,
                    SoaQuarter = q.Key.SoaQuarter,
                    BusinessCode = q.Key.BusinessCode,
                    CurrencyCode = q.Key.CurrencyCode,
                    CurrencyRate = q.Key.CurrencyRate,

                    NbPremium = q.Sum(d => d.NbPremium),
                    RnPremium = q.Sum(d => d.RnPremium),
                    AltPremium = q.Sum(d => d.AltPremium),
                    GrossPremium = q.Sum(d => d.GrossPremium),
                    TotalDiscount = q.Sum(d => d.TotalDiscount),
                    NoClaimBonus = q.Sum(d => d.NoClaimBonus),
                    Claim = q.Sum(d => d.Claim),
                    SurrenderValue = q.Sum(d => d.SurrenderValue),
                    Gst = q.Sum(d => d.Gst),
                    NetTotalAmount = q.Sum(d => d.NetTotalAmount),
                })
                .ToList();

            RiSummaries = RiSummaries
                .GroupBy(q => new { q.TreatyId, q.SoaQuarter, q.BusinessCode, q.CurrencyCode, q.CurrencyRate })
                .Select(q => new SoaDataRiDataSummaryBo
                {
                    TreatyId = q.Key.TreatyId,
                    SoaQuarter = q.Key.SoaQuarter,
                    BusinessCode = q.Key.BusinessCode,
                    CurrencyCode = q.Key.CurrencyCode,
                    CurrencyRate = q.Key.CurrencyRate,

                    NbPremium = q.Sum(d => d.NbPremium),
                    RnPremium = q.Sum(d => d.RnPremium),
                    AltPremium = q.Sum(d => d.AltPremium),
                    GrossPremium = q.Sum(d => d.GrossPremium),
                    TotalDiscount = q.Sum(d => d.TotalDiscount),
                    NoClaimBonus = q.Sum(d => d.NoClaimBonus),
                    Claim = q.Sum(d => d.Claim),
                    SurrenderValue = q.Sum(d => d.SurrenderValue),
                    Gst = q.Sum(d => d.Gst),
                    NetTotalAmount = q.Sum(d => d.NetTotalAmount),
                })
                .ToList();

            foreach (var sd in SoaData)
            {
                var rss = RiSummaries.Where(r => r.TreatyId == sd.TreatyId && r.SoaQuarter == sd.SoaQuarter && r.BusinessCode == sd.BusinessCode && r.CurrencyCode == sd.CurrencyCode && r.CurrencyRate == sd.CurrencyRate);
                if (rss.Count() > 0)
                {
                    foreach (var rs in rss)
                    {
                        var differencess = new SoaDataRiDataSummaryBo
                        {
                            Type = SoaDataRiDataSummaryBo.TypeDifferences,
                            BusinessCode = rs.BusinessCode,
                            TreatyCode = rs.TreatyCode,
                            RiskMonth = rs.RiskMonth,
                            RiskQuarter = rs.RiskQuarter,
                            SoaQuarter = rs.SoaQuarter,

                            CurrencyCode = rs.CurrencyCode,
                            CurrencyRate = rs.CurrencyRate,

                            NbPremium = (sd.NbPremium.GetValueOrDefault() - rs.NbPremium.GetValueOrDefault()),
                            RnPremium = (sd.RnPremium.GetValueOrDefault() - rs.RnPremium.GetValueOrDefault()),
                            AltPremium = (sd.AltPremium.GetValueOrDefault() - rs.AltPremium.GetValueOrDefault()),

                            TotalDiscount = (sd.TotalDiscount.GetValueOrDefault() - rs.TotalDiscount.GetValueOrDefault()),
                            NoClaimBonus = (sd.NoClaimBonus.GetValueOrDefault() - rs.NoClaimBonus.GetValueOrDefault()),
                            Claim = (sd.Claim.GetValueOrDefault() - rs.Claim.GetValueOrDefault()),
                            SurrenderValue = (sd.SurrenderValue.GetValueOrDefault() - rs.SurrenderValue.GetValueOrDefault()),
                            Gst = (sd.Gst.GetValueOrDefault() - rs.Gst.GetValueOrDefault()),
                        };
                        differencess.GetGrossPremium();
                        differencess.GetNetTotalAmount();

                        SoaDataRiDataSummaryBos.Add(differencess);
                    }
                }
                else
                {
                    sd.Type = SoaDataRiDataSummaryBo.TypeDifferences;
                    SoaDataRiDataSummaryBos.Add(sd);
                }

                //Added update for UpdatedAt for fail checking
                var boToUpdate = SoaDataBatchBo;
                SoaDataBatchService.Update(ref boToUpdate);
            }
        }

        public void CreatePostValidation()
        {
            SoaDataPostValidationBos = new List<SoaDataPostValidationBo> { };
            SoaDataPostValidationDifferenceBos = new List<SoaDataPostValidationDifferenceBo> { };

            var TempSoaDataPostValidationBos = new List<SoaDataPostValidationBo> { };
            using (var db = new AppDbContext(false))
            {
                var riDataGroups = QueryRiDataGroupBy(db, byFrequencyCode: false);

                var nbs = QueryRiDataPostValidationTransactionType(db, PickListDetailBo.TransactionTypeCodeNewBusiness);
                var rns = QueryRiDataPostValidationTransactionType(db, PickListDetailBo.TransactionTypeCodeRenewal);
                var als = QueryRiDataPostValidationTransactionType(db, PickListDetailBo.TransactionTypeCodeAlteration);

                var dths = QueryRiDataPostValidationBenefitCode(db, BenefitBo.CodeDTH, byFrequencyCode: false);
                var tpas = QueryRiDataPostValidationBenefitCode(db, BenefitBo.CodeTPA, byFrequencyCode: false);
                var tpss = QueryRiDataPostValidationBenefitCode(db, BenefitBo.CodeTPS, byFrequencyCode: false);
                var ppds = QueryRiDataPostValidationBenefitCode(db, BenefitBo.CodePPD, byFrequencyCode: false);
                var ccas = QueryRiDataPostValidationBenefitCode(db, BenefitBo.CodeCCA, byFrequencyCode: false);
                var ccss = QueryRiDataPostValidationBenefitCode(db, BenefitBo.CodeCCS, byFrequencyCode: false);
                var pas = QueryRiDataPostValidationBenefitCode(db, BenefitBo.CodePA, byFrequencyCode: false);
                var hss = QueryRiDataPostValidationBenefitCode(db, BenefitBo.CodeHS, byFrequencyCode: false);
                var tpds = QueryRiDataPostValidationBenefitCode(db, BenefitBo.CodeTPD, byFrequencyCode: false);
                var cis = QueryRiDataPostValidationBenefitCode(db, BenefitBo.CodeCI, byFrequencyCode: false);

                if (riDataGroups.IsNullOrEmpty())
                    return;

                foreach (var riDataGroup in riDataGroups)
                {
                    var businessOriginCode = CacheService.BusinessOrigins.Where(q => q.Id == TreatyBo?.BusinessOriginPickListDetailId).Select(q => q.Code).FirstOrDefault();
                    var treatyCode = riDataGroup.TreatyCode;
                    var riskPeriodMonth = riDataGroup.RiskPeriodMonth;
                    var riskPeriodYear = riDataGroup.RiskPeriodYear;
                    var currencyCode = riDataGroup.CurrencyCode;

                    var nb = nbs?.Where(q => q.TreatyCode == treatyCode && q.RiskPeriodMonth == riskPeriodMonth && q.RiskPeriodYear == riskPeriodYear && q.CurrencyCode == currencyCode).FirstOrDefault();
                    var rn = rns?.Where(q => q.TreatyCode == treatyCode && q.RiskPeriodMonth == riskPeriodMonth && q.RiskPeriodYear == riskPeriodYear && q.CurrencyCode == currencyCode).FirstOrDefault();
                    var al = als?.Where(q => q.TreatyCode == treatyCode && q.RiskPeriodMonth == riskPeriodMonth && q.RiskPeriodYear == riskPeriodYear && q.CurrencyCode == currencyCode).FirstOrDefault();

                    var dth = dths?.Where(q => q.TreatyCode == treatyCode && q.RiskPeriodMonth == riskPeriodMonth && q.RiskPeriodYear == riskPeriodYear && q.CurrencyCode == currencyCode).FirstOrDefault();
                    var tpa = tpas?.Where(q => q.TreatyCode == treatyCode && q.RiskPeriodMonth == riskPeriodMonth && q.RiskPeriodYear == riskPeriodYear && q.CurrencyCode == currencyCode).FirstOrDefault();
                    var tps = tpss?.Where(q => q.TreatyCode == treatyCode && q.RiskPeriodMonth == riskPeriodMonth && q.RiskPeriodYear == riskPeriodYear && q.CurrencyCode == currencyCode).FirstOrDefault();
                    var ppd = ppds?.Where(q => q.TreatyCode == treatyCode && q.RiskPeriodMonth == riskPeriodMonth && q.RiskPeriodYear == riskPeriodYear && q.CurrencyCode == currencyCode).FirstOrDefault();
                    var cca = ccas?.Where(q => q.TreatyCode == treatyCode && q.RiskPeriodMonth == riskPeriodMonth && q.RiskPeriodYear == riskPeriodYear && q.CurrencyCode == currencyCode).FirstOrDefault();
                    var ccs = ccss?.Where(q => q.TreatyCode == treatyCode && q.RiskPeriodMonth == riskPeriodMonth && q.RiskPeriodYear == riskPeriodYear && q.CurrencyCode == currencyCode).FirstOrDefault();
                    var pa = pas?.Where(q => q.TreatyCode == treatyCode && q.RiskPeriodMonth == riskPeriodMonth && q.RiskPeriodYear == riskPeriodYear && q.CurrencyCode == currencyCode).FirstOrDefault();
                    var hs = hss?.Where(q => q.TreatyCode == treatyCode && q.RiskPeriodMonth == riskPeriodMonth && q.RiskPeriodYear == riskPeriodYear && q.CurrencyCode == currencyCode).FirstOrDefault();
                    var tpd = tpds?.Where(q => q.TreatyCode == treatyCode && q.RiskPeriodMonth == riskPeriodMonth && q.RiskPeriodYear == riskPeriodYear && q.CurrencyCode == currencyCode).FirstOrDefault();
                    var ci = cis?.Where(q => q.TreatyCode == treatyCode && q.RiskPeriodMonth == riskPeriodMonth && q.RiskPeriodYear == riskPeriodYear && q.CurrencyCode == currencyCode).FirstOrDefault();

                    if (businessOriginCode != PickListDetailBo.BusinessOriginCodeServiceFee)
                    {
                        var mlreShareMlreChecking = new SoaDataPostValidationBo
                        {
                            Type = SoaDataPostValidationBo.TypeMlreShareMlreChecking,
                            BusinessCode = businessOriginCode,
                            TreatyCode = riDataGroup.TreatyCode,
                            RiskMonth = riDataGroup.RiskPeriodMonth.GetValueOrDefault(),
                            RiskQuarter = GetQuarterInfo(riDataGroup.RiskPeriodMonth, riDataGroup.RiskPeriodYear),

                            NoClaimBonus = riDataGroup.NoClaimBonus.GetValueOrDefault(),
                            SurrenderValue = riDataGroup.SurrenderValue.GetValueOrDefault(),
                            Gst = riDataGroup.GstAmount.GetValueOrDefault(),

                            CurrencyCode = riDataGroup.CurrencyCode,
                            CurrencyRate = SoaDataBatchBo.CurrencyRate,
                        };
                        var mlreShareCedantAmount = new SoaDataPostValidationBo
                        {
                            Type = SoaDataPostValidationBo.TypeMlreShareCedantAmount,
                            BusinessCode = businessOriginCode,
                            TreatyCode = riDataGroup.TreatyCode,
                            RiskMonth = riDataGroup.RiskPeriodMonth.GetValueOrDefault(),
                            RiskQuarter = GetQuarterInfo(riDataGroup.RiskPeriodMonth, riDataGroup.RiskPeriodYear),

                            NoClaimBonus = riDataGroup.NoClaimBonus.GetValueOrDefault(),
                            SurrenderValue = riDataGroup.SurrenderValue.GetValueOrDefault(),
                            Gst = riDataGroup.GstAmount.GetValueOrDefault(),

                            CurrencyCode = riDataGroup.CurrencyCode,
                            CurrencyRate = SoaDataBatchBo.CurrencyRate,
                        };
                        var layer1ShareMlreChecking = new SoaDataPostValidationBo
                        {
                            Type = SoaDataPostValidationBo.TypeLayer1ShareMlreChecking,
                            BusinessCode = businessOriginCode,
                            TreatyCode = riDataGroup.TreatyCode,
                            RiskMonth = riDataGroup.RiskPeriodMonth.GetValueOrDefault(),
                            RiskQuarter = GetQuarterInfo(riDataGroup.RiskPeriodMonth, riDataGroup.RiskPeriodYear),

                            NoClaimBonus = riDataGroup.NoClaimBonus.GetValueOrDefault(),
                            SurrenderValue = riDataGroup.SurrenderValue.GetValueOrDefault(),
                            Gst = riDataGroup.GstAmount.GetValueOrDefault(),

                            CurrencyCode = riDataGroup.CurrencyCode,
                            CurrencyRate = SoaDataBatchBo.CurrencyRate,
                        };
                        var layer1ShareCedantAmount = new SoaDataPostValidationBo
                        {
                            Type = SoaDataPostValidationBo.TypeLayer1ShareCedantAmount,
                            BusinessCode = businessOriginCode,
                            TreatyCode = riDataGroup.TreatyCode,
                            RiskMonth = riDataGroup.RiskPeriodMonth.GetValueOrDefault(),
                            RiskQuarter = GetQuarterInfo(riDataGroup.RiskPeriodMonth, riDataGroup.RiskPeriodYear),

                            NoClaimBonus = riDataGroup.NoClaimBonus.GetValueOrDefault(),
                            SurrenderValue = riDataGroup.SurrenderValue.GetValueOrDefault(),
                            Gst = riDataGroup.GstAmount.GetValueOrDefault(),

                            CurrencyCode = riDataGroup.CurrencyCode,
                            CurrencyRate = SoaDataBatchBo.CurrencyRate,
                        };

                        if (nb != null)
                        {
                            mlreShareMlreChecking.NbPremium = nb.MlreGrossPremium.GetValueOrDefault();
                            mlreShareCedantAmount.NbPremium = nb.TransactionPremium.GetValueOrDefault();
                            layer1ShareMlreChecking.NbPremium = nb.MlreGrossPremium.GetValueOrDefault() * 2;
                            layer1ShareCedantAmount.NbPremium = nb.Layer1GrossPremium.GetValueOrDefault();

                            mlreShareMlreChecking.NbDiscount = nb.MlreTotalDiscount.GetValueOrDefault();
                            mlreShareCedantAmount.NbDiscount = nb.TransactionDiscount.GetValueOrDefault();
                            layer1ShareMlreChecking.NbDiscount = nb.MlreTotalDiscount.GetValueOrDefault() * 2;
                            layer1ShareCedantAmount.NbDiscount = nb.Layer1TotalDiscount.GetValueOrDefault();

                            mlreShareMlreChecking.NbSar = nb.Aar;
                            mlreShareCedantAmount.NbSar = nb.Aar;
                            layer1ShareMlreChecking.NbSar = nb.Aar;
                            layer1ShareCedantAmount.NbSar = nb.Aar;

                            mlreShareMlreChecking.NbCession = nb.Total;
                            mlreShareCedantAmount.NbCession = nb.Total;
                            layer1ShareMlreChecking.NbCession = nb.Total;
                            layer1ShareCedantAmount.NbCession = nb.Total;
                        }
                        if (rn != null)
                        {
                            mlreShareMlreChecking.RnPremium = rn.MlreGrossPremium.GetValueOrDefault();
                            mlreShareCedantAmount.RnPremium = rn.TransactionPremium.GetValueOrDefault();
                            layer1ShareMlreChecking.RnPremium = rn.MlreGrossPremium.GetValueOrDefault() * 2;
                            layer1ShareCedantAmount.RnPremium = rn.Layer1GrossPremium.GetValueOrDefault();

                            mlreShareMlreChecking.RnDiscount = rn.MlreTotalDiscount.GetValueOrDefault();
                            mlreShareCedantAmount.RnDiscount = rn.TransactionDiscount.GetValueOrDefault();
                            layer1ShareMlreChecking.RnDiscount = rn.MlreTotalDiscount.GetValueOrDefault() * 2;
                            layer1ShareCedantAmount.RnDiscount = rn.Layer1TotalDiscount.GetValueOrDefault();

                            mlreShareMlreChecking.RnSar = rn.Aar;
                            mlreShareCedantAmount.RnSar = rn.Aar;
                            layer1ShareMlreChecking.RnSar = rn.Aar;
                            layer1ShareCedantAmount.RnSar = rn.Aar;

                            mlreShareMlreChecking.RnCession = rn.Total;
                            mlreShareCedantAmount.RnCession = rn.Total;
                            layer1ShareMlreChecking.RnCession = rn.Total;
                            layer1ShareCedantAmount.RnCession = rn.Total;
                        }
                        if (al != null)
                        {
                            mlreShareMlreChecking.AltPremium = al.MlreGrossPremium.GetValueOrDefault();
                            mlreShareCedantAmount.AltPremium = al.TransactionPremium.GetValueOrDefault();
                            layer1ShareMlreChecking.AltPremium = al.MlreGrossPremium.GetValueOrDefault() * 2;
                            layer1ShareCedantAmount.AltPremium = al.Layer1GrossPremium.GetValueOrDefault();

                            mlreShareMlreChecking.AltDiscount = al.MlreTotalDiscount.GetValueOrDefault();
                            mlreShareCedantAmount.AltDiscount = al.TransactionDiscount.GetValueOrDefault();
                            layer1ShareMlreChecking.AltDiscount = al.MlreTotalDiscount.GetValueOrDefault() * 2;
                            layer1ShareCedantAmount.AltDiscount = al.Layer1TotalDiscount.GetValueOrDefault();

                            mlreShareMlreChecking.AltSar = al.Aar;
                            mlreShareCedantAmount.AltSar = al.Aar;
                            layer1ShareMlreChecking.AltSar = al.Aar;
                            layer1ShareCedantAmount.AltSar = al.Aar;

                            mlreShareMlreChecking.AltCession = al.Total;
                            mlreShareCedantAmount.AltCession = al.Total;
                            layer1ShareMlreChecking.AltCession = al.Total;
                            layer1ShareCedantAmount.AltCession = al.Total;
                        }

                        mlreShareMlreChecking.GetGrossPremium();
                        mlreShareCedantAmount.GetGrossPremium();
                        layer1ShareMlreChecking.GetGrossPremium();
                        layer1ShareCedantAmount.GetGrossPremium();

                        mlreShareMlreChecking.GetTotalDiscount();
                        mlreShareCedantAmount.GetTotalDiscount();
                        layer1ShareMlreChecking.GetTotalDiscount();
                        layer1ShareCedantAmount.GetTotalDiscount();

                        mlreShareMlreChecking.GetNetTotalAmount();
                        mlreShareCedantAmount.GetNetTotalAmount();
                        layer1ShareMlreChecking.GetNetTotalAmount();
                        layer1ShareCedantAmount.GetNetTotalAmount();

                        if (dth != null)
                        {
                            mlreShareMlreChecking.DTH = dth.MlreGrossPremium.GetValueOrDefault();
                            mlreShareCedantAmount.DTH = dth.TransactionPremium.GetValueOrDefault();
                            layer1ShareMlreChecking.DTH = dth.MlreGrossPremium.GetValueOrDefault();
                            layer1ShareCedantAmount.DTH = dth.TransactionPremium.GetValueOrDefault();
                        }

                        if (tpa != null)
                        {
                            mlreShareMlreChecking.TPA = tpa.MlreGrossPremium.GetValueOrDefault();
                            mlreShareCedantAmount.TPA = tpa.TransactionPremium.GetValueOrDefault();
                            layer1ShareMlreChecking.TPA = tpa.MlreGrossPremium.GetValueOrDefault();
                            layer1ShareCedantAmount.TPA = tpa.TransactionPremium.GetValueOrDefault();
                        }

                        if (tps != null)
                        {
                            mlreShareMlreChecking.TPS = tps.MlreGrossPremium.GetValueOrDefault();
                            mlreShareCedantAmount.TPS = tps.TransactionPremium.GetValueOrDefault();
                            layer1ShareMlreChecking.TPS = tps.MlreGrossPremium.GetValueOrDefault();
                            layer1ShareCedantAmount.TPS = tps.TransactionPremium.GetValueOrDefault();
                        }

                        if (ppd != null)
                        {
                            mlreShareMlreChecking.PPD = ppd.MlreGrossPremium.GetValueOrDefault();
                            mlreShareCedantAmount.PPD = ppd.TransactionPremium.GetValueOrDefault();
                            layer1ShareMlreChecking.PPD = ppd.MlreGrossPremium.GetValueOrDefault();
                            layer1ShareCedantAmount.PPD = ppd.TransactionPremium.GetValueOrDefault();
                        }

                        if (cca != null)
                        {
                            mlreShareMlreChecking.CCA = cca.MlreGrossPremium.GetValueOrDefault();
                            mlreShareCedantAmount.CCA = cca.TransactionPremium.GetValueOrDefault();
                            layer1ShareMlreChecking.CCA = cca.MlreGrossPremium.GetValueOrDefault();
                            layer1ShareCedantAmount.CCA = cca.TransactionPremium.GetValueOrDefault();
                        }

                        if (ccs != null)
                        {
                            mlreShareMlreChecking.CCS = ccs.MlreGrossPremium.GetValueOrDefault();
                            mlreShareCedantAmount.CCS = ccs.TransactionPremium.GetValueOrDefault();
                            layer1ShareMlreChecking.CCS = ccs.MlreGrossPremium.GetValueOrDefault();
                            layer1ShareCedantAmount.CCS = ccs.TransactionPremium.GetValueOrDefault();
                        }

                        if (pa != null)
                        {
                            mlreShareMlreChecking.PA = pa.MlreGrossPremium.GetValueOrDefault();
                            mlreShareCedantAmount.PA = pa.TransactionPremium.GetValueOrDefault();
                            layer1ShareMlreChecking.PA = pa.MlreGrossPremium.GetValueOrDefault();
                            layer1ShareCedantAmount.PA = pa.TransactionPremium.GetValueOrDefault();
                        }

                        if (hs != null)
                        {
                            mlreShareMlreChecking.HS = hs.MlreGrossPremium.GetValueOrDefault();
                            mlreShareCedantAmount.HS = hs.TransactionPremium.GetValueOrDefault();
                            layer1ShareMlreChecking.HS = hs.MlreGrossPremium.GetValueOrDefault();
                            layer1ShareCedantAmount.HS = hs.TransactionPremium.GetValueOrDefault();
                        }

                        if (tpd != null)
                        {
                            mlreShareMlreChecking.TPD = tpd.MlreGrossPremium.GetValueOrDefault();
                            mlreShareCedantAmount.TPD = tpd.TransactionPremium.GetValueOrDefault();
                            layer1ShareMlreChecking.TPD = tpd.MlreGrossPremium.GetValueOrDefault();
                            layer1ShareCedantAmount.TPD = tpd.TransactionPremium.GetValueOrDefault();
                        }

                        if (ci != null)
                        {
                            mlreShareMlreChecking.CI = ci.MlreGrossPremium.GetValueOrDefault();
                            mlreShareCedantAmount.CI = ci.TransactionPremium.GetValueOrDefault();
                            layer1ShareMlreChecking.CI = ci.MlreGrossPremium.GetValueOrDefault();
                            layer1ShareCedantAmount.CI = ci.TransactionPremium.GetValueOrDefault();
                        }

                        SoaDataPostValidationBos.Add(mlreShareMlreChecking);
                        SoaDataPostValidationBos.Add(mlreShareCedantAmount);
                        SoaDataPostValidationBos.Add(layer1ShareMlreChecking);
                        SoaDataPostValidationBos.Add(layer1ShareCedantAmount);

                        if (!string.IsNullOrEmpty(riDataGroup.CurrencyCode) && riDataGroup.CurrencyCode != PickListDetailBo.CurrencyCodeMyr)
                        {
                            var mlreShareMlreCheckingMYR = new SoaDataPostValidationBo
                            {
                                Type = SoaDataPostValidationBo.TypeMlreShareMlreChecking,
                                BusinessCode = businessOriginCode,
                                TreatyCode = riDataGroup.TreatyCode,
                                RiskMonth = riDataGroup.RiskPeriodMonth.GetValueOrDefault(),
                                RiskQuarter = GetQuarterInfo(riDataGroup.RiskPeriodMonth, riDataGroup.RiskPeriodYear),

                                NoClaimBonus = riDataGroup.NoClaimBonus.GetValueOrDefault(),
                                SurrenderValue = riDataGroup.SurrenderValue.GetValueOrDefault(),

                                CurrencyCode = PickListDetailBo.CurrencyCodeMyr,
                                CurrencyRate = SoaDataBatchBo.CurrencyRate,
                            };
                            mlreShareMlreCheckingMYR.NoClaimBonus = mlreShareMlreChecking.NoClaimBonus * mlreShareMlreCheckingMYR.CurrencyRate;
                            mlreShareMlreCheckingMYR.SurrenderValue = mlreShareMlreChecking.SurrenderValue * mlreShareMlreCheckingMYR.CurrencyRate;
                            mlreShareMlreCheckingMYR.Gst = mlreShareMlreChecking.Gst * mlreShareMlreCheckingMYR.CurrencyRate;

                            mlreShareMlreCheckingMYR.NbCession = mlreShareMlreChecking.NbCession;
                            mlreShareMlreCheckingMYR.RnCession = mlreShareMlreChecking.RnCession;
                            mlreShareMlreCheckingMYR.AltCession = mlreShareMlreChecking.AltCession;

                            mlreShareMlreCheckingMYR.NbPremium = mlreShareMlreChecking.NbPremium * mlreShareMlreCheckingMYR.CurrencyRate;
                            mlreShareMlreCheckingMYR.RnPremium = mlreShareMlreChecking.RnPremium * mlreShareMlreCheckingMYR.CurrencyRate;
                            mlreShareMlreCheckingMYR.AltPremium = mlreShareMlreChecking.AltPremium * mlreShareMlreCheckingMYR.CurrencyRate;

                            mlreShareMlreCheckingMYR.NbDiscount = mlreShareMlreChecking.NbDiscount * mlreShareMlreCheckingMYR.CurrencyRate;
                            mlreShareMlreCheckingMYR.RnDiscount = mlreShareMlreChecking.RnDiscount * mlreShareMlreCheckingMYR.CurrencyRate;
                            mlreShareMlreCheckingMYR.AltDiscount = mlreShareMlreChecking.AltDiscount * mlreShareMlreCheckingMYR.CurrencyRate;

                            mlreShareMlreCheckingMYR.NbSar = mlreShareMlreChecking.NbSar * mlreShareMlreCheckingMYR.CurrencyRate;
                            mlreShareMlreCheckingMYR.RnSar = mlreShareMlreChecking.RnSar * mlreShareMlreCheckingMYR.CurrencyRate;
                            mlreShareMlreCheckingMYR.AltSar = mlreShareMlreChecking.AltSar * mlreShareMlreCheckingMYR.CurrencyRate;

                            mlreShareMlreCheckingMYR.DTH = mlreShareMlreChecking.DTH * mlreShareMlreCheckingMYR.CurrencyRate;
                            mlreShareMlreCheckingMYR.TPA = mlreShareMlreChecking.TPA * mlreShareMlreCheckingMYR.CurrencyRate;
                            mlreShareMlreCheckingMYR.TPS = mlreShareMlreChecking.TPS * mlreShareMlreCheckingMYR.CurrencyRate;
                            mlreShareMlreCheckingMYR.PPD = mlreShareMlreChecking.PPD * mlreShareMlreCheckingMYR.CurrencyRate;
                            mlreShareMlreCheckingMYR.CCA = mlreShareMlreChecking.CCA * mlreShareMlreCheckingMYR.CurrencyRate;
                            mlreShareMlreCheckingMYR.CCS = mlreShareMlreChecking.CCS * mlreShareMlreCheckingMYR.CurrencyRate;
                            mlreShareMlreCheckingMYR.PA = mlreShareMlreChecking.PA * mlreShareMlreCheckingMYR.CurrencyRate;
                            mlreShareMlreCheckingMYR.HS = mlreShareMlreChecking.HS * mlreShareMlreCheckingMYR.CurrencyRate;
                            mlreShareMlreCheckingMYR.TPD = mlreShareMlreChecking.TPD * mlreShareMlreCheckingMYR.CurrencyRate;
                            mlreShareMlreCheckingMYR.CI = mlreShareMlreChecking.CI * mlreShareMlreCheckingMYR.CurrencyRate;

                            mlreShareMlreCheckingMYR.GetGrossPremium();
                            mlreShareMlreCheckingMYR.GetTotalDiscount();
                            mlreShareMlreCheckingMYR.GetNetTotalAmount();

                            SoaDataPostValidationBos.Add(mlreShareMlreCheckingMYR);


                            var mlreShareCedantAmountMYR = new SoaDataPostValidationBo
                            {
                                Type = SoaDataPostValidationBo.TypeMlreShareCedantAmount,
                                BusinessCode = businessOriginCode,
                                TreatyCode = riDataGroup.TreatyCode,
                                RiskMonth = riDataGroup.RiskPeriodMonth.GetValueOrDefault(),
                                RiskQuarter = GetQuarterInfo(riDataGroup.RiskPeriodMonth, riDataGroup.RiskPeriodYear),

                                NoClaimBonus = riDataGroup.NoClaimBonus.GetValueOrDefault(),
                                SurrenderValue = riDataGroup.SurrenderValue.GetValueOrDefault(),

                                CurrencyCode = PickListDetailBo.CurrencyCodeMyr,
                                CurrencyRate = SoaDataBatchBo.CurrencyRate,
                            };
                            mlreShareCedantAmountMYR.NoClaimBonus = mlreShareCedantAmount.NoClaimBonus * mlreShareCedantAmountMYR.CurrencyRate;
                            mlreShareCedantAmountMYR.SurrenderValue = mlreShareCedantAmount.SurrenderValue * mlreShareCedantAmountMYR.CurrencyRate;
                            mlreShareCedantAmountMYR.Gst = mlreShareCedantAmount.Gst * mlreShareCedantAmountMYR.CurrencyRate;

                            mlreShareCedantAmountMYR.NbCession = mlreShareCedantAmount.NbCession;
                            mlreShareCedantAmountMYR.RnCession = mlreShareCedantAmount.RnCession;
                            mlreShareCedantAmountMYR.AltCession = mlreShareCedantAmount.AltCession;

                            mlreShareCedantAmountMYR.NbPremium = mlreShareCedantAmount.NbPremium * mlreShareCedantAmountMYR.CurrencyRate;
                            mlreShareCedantAmountMYR.RnPremium = mlreShareCedantAmount.RnPremium * mlreShareCedantAmountMYR.CurrencyRate;
                            mlreShareCedantAmountMYR.AltPremium = mlreShareCedantAmount.AltPremium * mlreShareCedantAmountMYR.CurrencyRate;

                            mlreShareCedantAmountMYR.NbDiscount = mlreShareCedantAmount.NbDiscount * mlreShareCedantAmountMYR.CurrencyRate;
                            mlreShareCedantAmountMYR.RnDiscount = mlreShareCedantAmount.RnDiscount * mlreShareCedantAmountMYR.CurrencyRate;
                            mlreShareCedantAmountMYR.AltDiscount = mlreShareCedantAmount.AltDiscount * mlreShareCedantAmountMYR.CurrencyRate;

                            mlreShareCedantAmountMYR.NbSar = mlreShareCedantAmount.NbSar * mlreShareCedantAmountMYR.CurrencyRate;
                            mlreShareCedantAmountMYR.RnSar = mlreShareCedantAmount.RnSar * mlreShareCedantAmountMYR.CurrencyRate;
                            mlreShareCedantAmountMYR.AltSar = mlreShareCedantAmount.AltSar * mlreShareCedantAmountMYR.CurrencyRate;

                            mlreShareCedantAmountMYR.DTH = mlreShareCedantAmount.DTH * mlreShareCedantAmountMYR.CurrencyRate;
                            mlreShareCedantAmountMYR.TPA = mlreShareCedantAmount.TPA * mlreShareCedantAmountMYR.CurrencyRate;
                            mlreShareCedantAmountMYR.TPS = mlreShareCedantAmount.TPS * mlreShareCedantAmountMYR.CurrencyRate;
                            mlreShareCedantAmountMYR.PPD = mlreShareCedantAmount.PPD * mlreShareCedantAmountMYR.CurrencyRate;
                            mlreShareCedantAmountMYR.CCA = mlreShareCedantAmount.CCA * mlreShareCedantAmountMYR.CurrencyRate;
                            mlreShareCedantAmountMYR.CCS = mlreShareCedantAmount.CCS * mlreShareCedantAmountMYR.CurrencyRate;
                            mlreShareCedantAmountMYR.PA = mlreShareCedantAmount.PA * mlreShareCedantAmountMYR.CurrencyRate;
                            mlreShareCedantAmountMYR.HS = mlreShareCedantAmount.HS * mlreShareCedantAmountMYR.CurrencyRate;
                            mlreShareCedantAmountMYR.TPD = mlreShareCedantAmount.TPD * mlreShareCedantAmountMYR.CurrencyRate;
                            mlreShareCedantAmountMYR.CI = mlreShareCedantAmount.CI * mlreShareCedantAmountMYR.CurrencyRate;

                            mlreShareCedantAmountMYR.GetGrossPremium();
                            mlreShareCedantAmountMYR.GetTotalDiscount();
                            mlreShareCedantAmountMYR.GetNetTotalAmount();

                            SoaDataPostValidationBos.Add(mlreShareCedantAmountMYR);
                        }

                        //PrintPostValidation(mlreShareMlreChecking);
                        //PrintPostValidation(mlreShareCedantAmount);
                        //PrintPostValidation(layer1ShareMlreChecking);
                        //PrintPostValidation(layer1ShareCedantAmount);

                        var diffMlreShare = new SoaDataPostValidationDifferenceBo
                        {
                            Type = SoaDataPostValidationDifferenceBo.TypeMlreShare,
                            BusinessCode = businessOriginCode,
                            TreatyCode = riDataGroup.TreatyCode,
                            SoaQuarter = FormatQuarter(SoaDataBatchBo.Quarter),

                            CurrencyCode = riDataGroup.CurrencyCode,
                            CurrencyRate = SoaDataBatchBo.CurrencyRate,

                            GrossPremium = mlreShareCedantAmount.GrossPremium,
                            DifferenceNetTotalAmount = mlreShareCedantAmount.NetTotalAmount - mlreShareMlreChecking.NetTotalAmount,
                        };
                        diffMlreShare.GetDifferencePercentage();

                        var diffLayer1Share = new SoaDataPostValidationDifferenceBo
                        {
                            Type = SoaDataPostValidationDifferenceBo.TypeLayer1Share,
                            BusinessCode = businessOriginCode,
                            TreatyCode = riDataGroup.TreatyCode,
                            SoaQuarter = FormatQuarter(SoaDataBatchBo.Quarter),

                            CurrencyCode = riDataGroup.CurrencyCode,
                            CurrencyRate = SoaDataBatchBo.CurrencyRate,

                            GrossPremium = layer1ShareCedantAmount.GrossPremium,
                            DifferenceNetTotalAmount = layer1ShareCedantAmount.NetTotalAmount - layer1ShareMlreChecking.NetTotalAmount,
                        };
                        diffLayer1Share.GetDifferencePercentage();

                        SoaDataPostValidationDifferenceBos.Add(diffMlreShare);
                        SoaDataPostValidationDifferenceBos.Add(diffLayer1Share);
                        //PrintPostValidationDifference(diffMlreShare);
                        //PrintPostValidationDifference(diffLayer1Share);

                        if (!string.IsNullOrEmpty(riDataGroup.CurrencyCode) && riDataGroup.CurrencyCode != PickListDetailBo.CurrencyCodeMyr)
                        {
                            var diffMlreShareMYR = new SoaDataPostValidationDifferenceBo
                            {
                                Type = SoaDataPostValidationDifferenceBo.TypeMlreShare,
                                BusinessCode = businessOriginCode,
                                TreatyCode = riDataGroup.TreatyCode,
                                SoaQuarter = FormatQuarter(SoaDataBatchBo.Quarter),

                                CurrencyCode = PickListDetailBo.CurrencyCodeMyr,
                                CurrencyRate = riDataGroup.CurrencyRate,
                            };
                            diffMlreShareMYR.GrossPremium = diffMlreShare.GrossPremium * diffMlreShareMYR.CurrencyRate;
                            diffMlreShareMYR.DifferenceNetTotalAmount = diffMlreShare.DifferenceNetTotalAmount * diffMlreShareMYR.CurrencyRate;
                            diffMlreShareMYR.GetDifferencePercentage();

                            SoaDataPostValidationDifferenceBos.Add(diffMlreShareMYR);
                        }
                    }
                    else
                    {
                        var retakafulMlreChecking = new SoaDataPostValidationBo
                        {
                            Type = SoaDataPostValidationBo.TypeRetakafulShareMlreChecking,
                            BusinessCode = businessOriginCode,
                            TreatyCode = riDataGroup.TreatyCode,
                            RiskMonth = riDataGroup.RiskPeriodMonth.GetValueOrDefault(),
                            RiskQuarter = GetQuarterInfo(riDataGroup.RiskPeriodMonth, riDataGroup.RiskPeriodYear),

                            NoClaimBonus = riDataGroup.NoClaimBonus.GetValueOrDefault(),
                            SurrenderValue = riDataGroup.SurrenderValue.GetValueOrDefault(),

                            CurrencyCode = riDataGroup.CurrencyCode,
                            CurrencyRate = SoaDataBatchBo.CurrencyRate,
                        };
                        var retakafulCedantAmount = new SoaDataPostValidationBo
                        {
                            Type = SoaDataPostValidationBo.TypeRetakafulShareCedantAmount,
                            BusinessCode = businessOriginCode,
                            TreatyCode = riDataGroup.TreatyCode,
                            RiskMonth = riDataGroup.RiskPeriodMonth.GetValueOrDefault(),
                            RiskQuarter = GetQuarterInfo(riDataGroup.RiskPeriodMonth, riDataGroup.RiskPeriodYear),

                            NoClaimBonus = riDataGroup.NoClaimBonus.GetValueOrDefault(),
                            SurrenderValue = riDataGroup.SurrenderValue.GetValueOrDefault(),

                            CurrencyCode = riDataGroup.CurrencyCode,
                            CurrencyRate = SoaDataBatchBo.CurrencyRate,
                        };

                        if (nb != null)
                        {
                            retakafulMlreChecking.NbPremium = nb.MlreGrossPremium.GetValueOrDefault();
                            retakafulCedantAmount.NbPremium = nb.TransactionPremium.GetValueOrDefault();
                            retakafulMlreChecking.NbDiscount = nb.MlreTotalDiscount.GetValueOrDefault();
                            retakafulCedantAmount.NbDiscount = nb.TransactionDiscount.GetValueOrDefault();

                            retakafulMlreChecking.NbSar = nb.Aar;
                            retakafulCedantAmount.NbSar = nb.Aar;

                            retakafulMlreChecking.NbCession = nb.Total;
                            retakafulCedantAmount.NbCession = nb.Total;
                        }
                        if (rn != null)
                        {
                            retakafulMlreChecking.RnPremium = rn.MlreGrossPremium.GetValueOrDefault();
                            retakafulCedantAmount.RnPremium = rn.TransactionPremium.GetValueOrDefault();
                            retakafulMlreChecking.RnDiscount = rn.MlreTotalDiscount.GetValueOrDefault();
                            retakafulCedantAmount.RnDiscount = rn.TransactionDiscount.GetValueOrDefault();

                            retakafulMlreChecking.RnSar = rn.Aar;
                            retakafulCedantAmount.RnSar = rn.Aar;

                            retakafulMlreChecking.RnCession = rn.Total;
                            retakafulCedantAmount.RnCession = rn.Total;
                        }
                        if (al != null)
                        {
                            retakafulMlreChecking.AltPremium = al.MlreGrossPremium.GetValueOrDefault();
                            retakafulCedantAmount.AltPremium = al.TransactionPremium.GetValueOrDefault();
                            retakafulMlreChecking.AltDiscount = al.MlreTotalDiscount.GetValueOrDefault();
                            retakafulCedantAmount.AltDiscount = al.TransactionDiscount.GetValueOrDefault();

                            retakafulMlreChecking.AltSar = al.Aar;
                            retakafulCedantAmount.AltSar = al.Aar;

                            retakafulMlreChecking.AltCession = al.Total;
                            retakafulCedantAmount.AltCession = al.Total;
                        }

                        retakafulMlreChecking.GetGrossPremium();
                        retakafulCedantAmount.GetGrossPremium();

                        retakafulMlreChecking.GetTotalDiscount();
                        retakafulCedantAmount.GetTotalDiscount();

                        retakafulMlreChecking.GetNetTotalAmount();
                        retakafulCedantAmount.GetNetTotalAmount();

                        if (dth != null)
                        {
                            retakafulMlreChecking.DTH = dth.TransactionPremium.GetValueOrDefault();
                            retakafulCedantAmount.DTH = dth.TransactionPremium.GetValueOrDefault();
                        }

                        if (tpa != null)
                        {
                            retakafulMlreChecking.TPA = tpa.TransactionPremium.GetValueOrDefault();
                            retakafulCedantAmount.TPA = tpa.TransactionPremium.GetValueOrDefault();
                        }

                        if (tps != null)
                        {
                            retakafulMlreChecking.TPS = tps.TransactionPremium.GetValueOrDefault();
                            retakafulCedantAmount.TPS = tps.TransactionPremium.GetValueOrDefault();
                        }

                        if (ppd != null)
                        {
                            retakafulMlreChecking.PPD = ppd.TransactionPremium.GetValueOrDefault();
                            retakafulCedantAmount.PPD = ppd.TransactionPremium.GetValueOrDefault();
                        }

                        if (cca != null)
                        {
                            retakafulMlreChecking.CCA = cca.TransactionPremium.GetValueOrDefault();
                            retakafulCedantAmount.CCA = cca.TransactionPremium.GetValueOrDefault();
                        }

                        if (ccs != null)
                        {
                            retakafulMlreChecking.CCS = ccs.TransactionPremium.GetValueOrDefault();
                            retakafulCedantAmount.CCS = ccs.TransactionPremium.GetValueOrDefault();
                        }

                        if (pa != null)
                        {
                            retakafulMlreChecking.PA = pa.TransactionPremium.GetValueOrDefault();
                            retakafulCedantAmount.PA = pa.TransactionPremium.GetValueOrDefault();
                        }

                        if (hs != null)
                        {
                            retakafulMlreChecking.HS = hs.TransactionPremium.GetValueOrDefault();
                            retakafulCedantAmount.HS = hs.TransactionPremium.GetValueOrDefault();
                        }

                        if (tpd != null)
                        {
                            retakafulMlreChecking.TPD = tpd.TransactionPremium.GetValueOrDefault();
                            retakafulCedantAmount.TPD = tpd.TransactionPremium.GetValueOrDefault();
                        }

                        if (ci != null)
                        {
                            retakafulMlreChecking.CI = ci.TransactionPremium.GetValueOrDefault();
                            retakafulCedantAmount.CI = ci.TransactionPremium.GetValueOrDefault();
                        }

                        SoaDataPostValidationBos.Add(retakafulMlreChecking);
                        SoaDataPostValidationBos.Add(retakafulCedantAmount);

                        //PrintPostValidation(retakafulMlreChecking);
                        //PrintPostValidation(retakafulCedantAmount);

                        var diffRetakaful = new SoaDataPostValidationDifferenceBo
                        {
                            Type = SoaDataPostValidationDifferenceBo.TypeRetakaful,
                            BusinessCode = businessOriginCode,
                            TreatyCode = riDataGroup.TreatyCode,
                            SoaQuarter = FormatQuarter(SoaDataBatchBo.Quarter),

                            CurrencyCode = riDataGroup.CurrencyCode,
                            CurrencyRate = SoaDataBatchBo.CurrencyRate,

                            GrossPremium = retakafulCedantAmount.GrossPremium,
                            DifferenceNetTotalAmount = retakafulCedantAmount.NetTotalAmount - retakafulMlreChecking.NetTotalAmount,
                        };
                        diffRetakaful.GetDifferencePercentage();

                        SoaDataPostValidationDifferenceBos.Add(diffRetakaful);
                        //PrintPostValidationDifference(diffRetakaful);
                    }

                    //Added update for UpdatedAt for fail checking
                    var boToUpdate = SoaDataBatchBo;
                    SoaDataBatchService.Update(ref boToUpdate);
                }

                #region GROUPS WITHOUT CURRENCY CODE
                //var queryValidationGroups = TempSoaDataPostValidationBos
                //    .GroupBy(q => new { q.Type, q.BusinessCode, q.TreatyCode, q.RiskMonth, q.RiskQuarter })
                //    .Select(q => new SoaDataPostValidationBo
                //    {
                //        Type = q.Key.Type,
                //        BusinessCode = q.Key.BusinessCode,
                //        TreatyCode = q.Key.TreatyCode,
                //        RiskMonth = q.Key.RiskMonth,
                //        RiskQuarter = q.Key.RiskQuarter,

                //        NoClaimBonus = q.Sum(d => d.NoClaimBonus),
                //        SurrenderValue = q.Sum(d => d.SurrenderValue),

                //        NbPremium = q.Sum(d => d.NbPremium),
                //        RnPremium = q.Sum(d => d.RnPremium),
                //        AltPremium = q.Sum(d => d.AltPremium),

                //        NbDiscount = q.Sum(d => d.NbDiscount),
                //        RnDiscount = q.Sum(d => d.RnDiscount),
                //        AltDiscount = q.Sum(d => d.AltDiscount),

                //        NbCession = q.Sum(d => d.NbCession),
                //        RnCession = q.Sum(d => d.RnCession),
                //        AltCession = q.Sum(d => d.AltCession),

                //        NbSar = q.Sum(d => d.NbSar),
                //        RnSar = q.Sum(d => d.RnSar),
                //        AltSar = q.Sum(d => d.AltSar),

                //        DTH = q.Sum(d => d.DTH),
                //        TPA = q.Sum(d => d.TPA),
                //        TPS = q.Sum(d => d.TPS),
                //        PPD = q.Sum(d => d.PPD),
                //        CCA = q.Sum(d => d.CCA),
                //        CCS = q.Sum(d => d.CCS),
                //        PA = q.Sum(d => d.PA),
                //        HS = q.Sum(d => d.HS),
                //    })
                //    .ToList();

                //foreach (var queryValidationGroup in queryValidationGroups)
                //{
                //    var postValidation = new SoaDataPostValidationBo
                //    {
                //        Type = queryValidationGroup.Type,
                //        BusinessCode = queryValidationGroup.BusinessCode,
                //        TreatyCode = queryValidationGroup.TreatyCode,
                //        RiskMonth = queryValidationGroup.RiskMonth,
                //        RiskQuarter = queryValidationGroup.RiskQuarter,

                //        NoClaimBonus = queryValidationGroup.NoClaimBonus,
                //        SurrenderValue = queryValidationGroup.SurrenderValue,

                //        NbPremium = queryValidationGroup.NbPremium,
                //        RnPremium = queryValidationGroup.RnPremium,
                //        AltPremium = queryValidationGroup.AltPremium,

                //        NbDiscount = queryValidationGroup.NbDiscount,
                //        RnDiscount = queryValidationGroup.RnDiscount,
                //        AltDiscount = queryValidationGroup.AltDiscount,

                //        NbCession = queryValidationGroup.NbCession,
                //        RnCession = queryValidationGroup.RnCession,
                //        AltCession = queryValidationGroup.AltCession,

                //        NbSar = queryValidationGroup.NbSar,
                //        RnSar = queryValidationGroup.RnSar,
                //        AltSar = queryValidationGroup.AltSar,

                //        DTH = queryValidationGroup.DTH,
                //        TPA = queryValidationGroup.TPA,
                //        TPS = queryValidationGroup.TPS,
                //        PPD = queryValidationGroup.PPD,
                //        CCA = queryValidationGroup.CCA,
                //        CCS = queryValidationGroup.CCS,
                //        PA = queryValidationGroup.PA,
                //        HS = queryValidationGroup.HS,
                //    };
                //    postValidation.GetGrossPremium();
                //    postValidation.GetTotalDiscount();
                //    postValidation.GetNetTotalAmount();

                //    SoaDataPostValidationBos.Add(postValidation);
                //}
                #endregion

                #region DIFFERENCES
                var queryDifferenceGroups = SoaDataPostValidationDifferenceBos
                    //.Where(q => q.Type == SoaDataPostValidationDifferenceBo.)
                    .GroupBy(q => new { q.Type, q.BusinessCode, q.TreatyCode, q.SoaQuarter, q.CurrencyCode, q.CurrencyRate })
                    .Select(q => new SoaDataPostValidationDifferenceBo
                    {
                        Type = q.Key.Type,
                        BusinessCode = q.Key.BusinessCode,
                        TreatyCode = q.Key.TreatyCode,
                        SoaQuarter = q.Key.SoaQuarter,

                        CurrencyCode = q.Key.CurrencyCode,
                        CurrencyRate = q.Key.CurrencyRate,

                        GrossPremium = q.Sum(d => d.GrossPremium),
                        DifferenceNetTotalAmount = q.Sum(d => d.DifferenceNetTotalAmount),
                    })
                    .OrderBy(q => q.TreatyCode)
                    .ThenBy(q => q.SoaQuarter)
                    .ToList();

                SoaDataPostValidationDifferenceBos = new List<SoaDataPostValidationDifferenceBo> { };
                foreach (var queryDifferenceGroup in queryDifferenceGroups)
                {
                    var diff = new SoaDataPostValidationDifferenceBo
                    {
                        Type = queryDifferenceGroup.Type,
                        BusinessCode = queryDifferenceGroup.BusinessCode,
                        TreatyCode = queryDifferenceGroup.TreatyCode,
                        SoaQuarter = queryDifferenceGroup.SoaQuarter,

                        CurrencyCode = queryDifferenceGroup.CurrencyCode,
                        CurrencyRate = queryDifferenceGroup.CurrencyRate,

                        GrossPremium = queryDifferenceGroup.GrossPremium,
                        DifferenceNetTotalAmount = queryDifferenceGroup.DifferenceNetTotalAmount,
                    };
                    diff.GetDifferencePercentage();
                    SoaDataPostValidationDifferenceBos.Add(diff);
                }
                #endregion

                #region DISCREPANCY CHECK
                var retakafulMlreCheckings = SoaDataPostValidationBos.Where(q => q.Type == SoaDataPostValidationBo.TypeRetakafulShareMlreChecking).ToList();
                var retakafulCedantAmounts = SoaDataPostValidationBos.Where(q => q.Type == SoaDataPostValidationBo.TypeRetakafulShareCedantAmount).ToList();
                CreatePostValidationDisrepancyCheck(retakafulCedantAmounts, retakafulMlreCheckings, SoaDataPostValidationBo.TypeRetakafulShareDiscrepancyCheck);

                var layer1ShareMlreCheckings = SoaDataPostValidationBos.Where(q => q.Type == SoaDataPostValidationBo.TypeLayer1ShareMlreChecking).ToList();
                var layer1ShareCedantAmounts = SoaDataPostValidationBos.Where(q => q.Type == SoaDataPostValidationBo.TypeLayer1ShareCedantAmount).ToList();
                CreatePostValidationDisrepancyCheck(layer1ShareCedantAmounts, layer1ShareMlreCheckings, SoaDataPostValidationBo.TypeLayer1ShareDiscrepancyCheck);

                var mlreShareMlreCheckings = SoaDataPostValidationBos.Where(q => q.Type == SoaDataPostValidationBo.TypeMlreShareMlreChecking).ToList();
                var mlreShareCedantAmounts = SoaDataPostValidationBos.Where(q => q.Type == SoaDataPostValidationBo.TypeMlreShareCedantAmount).ToList();
                CreatePostValidationDisrepancyCheck(mlreShareCedantAmounts, mlreShareMlreCheckings, SoaDataPostValidationBo.TypeMlreShareDiscrepancyCheck);
                #endregion

            }
        }

        public void CreatePostValidationDisrepancyCheck(IList<SoaDataPostValidationBo> CedantAmounts, IList<SoaDataPostValidationBo> MlreCheckings, int type)
        {
            foreach (var ca in CedantAmounts)
            {
                var mcs = MlreCheckings.Where(o => o.BusinessCode == ca.BusinessCode && o.TreatyCode == ca.TreatyCode && o.RiskMonth == ca.RiskMonth && o.RiskQuarter == ca.RiskQuarter && o.CurrencyCode == ca.CurrencyCode && o.CurrencyRate == ca.CurrencyRate);
                if (mcs.Count() > 0)
                {
                    foreach (var mc in mcs)
                    {
                        var disrepancyCheck = new SoaDataPostValidationBo
                        {
                            Type = type,
                            BusinessCode = mc.BusinessCode,
                            TreatyCode = mc.TreatyCode,
                            RiskMonth = mc.RiskMonth,
                            RiskQuarter = mc.RiskQuarter,

                            CurrencyCode = mc.CurrencyCode,
                            CurrencyRate = mc.CurrencyRate,

                            NbPremium = (ca.NbPremium.GetValueOrDefault() - mc.NbPremium.GetValueOrDefault()),
                            RnPremium = (ca.RnPremium.GetValueOrDefault() - mc.RnPremium.GetValueOrDefault()),
                            AltPremium = (ca.AltPremium.GetValueOrDefault() - mc.AltPremium.GetValueOrDefault()),

                            NoClaimBonus = (ca.NoClaimBonus.GetValueOrDefault() - mc.NoClaimBonus.GetValueOrDefault()),
                            SurrenderValue = (ca.SurrenderValue.GetValueOrDefault() - mc.SurrenderValue.GetValueOrDefault()),

                            NbCession = (ca.NbCession.GetValueOrDefault() - mc.NbCession.GetValueOrDefault()),
                            RnCession = (ca.RnCession.GetValueOrDefault() - mc.RnCession.GetValueOrDefault()),
                            AltCession = (ca.AltCession.GetValueOrDefault() - mc.AltCession.GetValueOrDefault()),

                            NbDiscount = (ca.NbDiscount.GetValueOrDefault() - mc.NbDiscount.GetValueOrDefault()),
                            RnDiscount = (ca.RnDiscount.GetValueOrDefault() - mc.RnDiscount.GetValueOrDefault()),
                            AltDiscount = (ca.AltDiscount.GetValueOrDefault() - mc.AltDiscount.GetValueOrDefault()),

                            NbSar = (ca.NbSar.GetValueOrDefault() - mc.NbSar.GetValueOrDefault()),
                            RnSar = (ca.RnSar.GetValueOrDefault() - mc.RnSar.GetValueOrDefault()),
                            AltSar = (ca.AltSar.GetValueOrDefault() - mc.AltSar.GetValueOrDefault()),

                            DTH = (ca.DTH.GetValueOrDefault() - mc.DTH.GetValueOrDefault()),
                            TPA = (ca.TPA.GetValueOrDefault() - mc.TPA.GetValueOrDefault()),
                            TPS = (ca.TPS.GetValueOrDefault() - mc.TPS.GetValueOrDefault()),
                            PPD = (ca.PPD.GetValueOrDefault() - mc.PPD.GetValueOrDefault()),
                            CCA = (ca.CCA.GetValueOrDefault() - mc.CCA.GetValueOrDefault()),
                            CCS = (ca.CCS.GetValueOrDefault() - mc.CCS.GetValueOrDefault()),
                            PA = (ca.PA.GetValueOrDefault() - mc.PA.GetValueOrDefault()),
                            HS = (ca.HS.GetValueOrDefault() - mc.HS.GetValueOrDefault()),
                            TPD = (ca.TPD.GetValueOrDefault() - mc.TPD.GetValueOrDefault()),
                            CI = (ca.CI.GetValueOrDefault() - mc.CI.GetValueOrDefault()),
                        };
                        disrepancyCheck.GetGrossPremium();
                        disrepancyCheck.GetTotalDiscount();
                        disrepancyCheck.GetNetTotalAmount();

                        SoaDataPostValidationBos.Add(disrepancyCheck);

                        //Added update for UpdatedAt for fail checking
                        var boToUpdate = SoaDataBatchBo;
                        SoaDataBatchService.Update(ref boToUpdate);
                    }
                }
                else
                {
                    ca.Type = type;
                    SoaDataPostValidationBos.Add(ca);

                    //Added update for UpdatedAt for fail checking
                    var boToUpdate = SoaDataBatchBo;
                    SoaDataBatchService.Update(ref boToUpdate);
                }
            }
        }

        public void CreateSoaDataByRiData()
        {
            if (SoaDataBatchBo == null)
                return;

            bool updateClaim = true;
            SoaDataBos = new List<SoaDataBo> { };
            using (var db = new AppDbContext(false))
            {
                var riDataGroups = QueryRiDataGroupByAutoCreate(db);
                var claimDataGroups = QueryClaimDataGroupBy(db, true);

                var nbs = QueryRiDataSumByTransactionType(db, PickListDetailBo.TransactionTypeCodeNewBusiness);
                var rns = QueryRiDataSumByTransactionType(db, PickListDetailBo.TransactionTypeCodeRenewal);
                var als = QueryRiDataSumByTransactionType(db, PickListDetailBo.TransactionTypeCodeAlteration);

                if (riDataGroups.IsNullOrEmpty())
                    return;

                foreach (var riDataGroup in riDataGroups)
                {
                    var businessOriginCode = CacheService.BusinessOrigins.Where(q => q.Id == SoaDataBatchBo.TreatyBo?.BusinessOriginPickListDetailId).Select(q => q.Code).FirstOrDefault();
                    var treatyTypeCode = CacheService.TreatyCodeBos.Where(q => q.Code == riDataGroup.TreatyCode && q.TreatyId == SoaDataBatchBo.TreatyId).FirstOrDefault();
                    var treatyCode = riDataGroup.TreatyCode;
                    var riskPeriodMonth = riDataGroup.RiskPeriodMonth;
                    var riskPeriodYear = riDataGroup.RiskPeriodYear;
                    var currencyCode = riDataGroup.CurrencyCode;
                    var bo = new SoaDataBo
                    {
                        BusinessCode = businessOriginCode,
                        TreatyId = SoaDataBatchBo.TreatyBo?.TreatyIdCode,
                        TreatyCode = riDataGroup.TreatyCode,
                        RiskMonth = riDataGroup.RiskPeriodMonth.GetValueOrDefault(),
                        TotalDiscount = riDataGroup.TransactionDiscount.GetValueOrDefault(),
                        NoClaimBonus = riDataGroup.NoClaimBonus.GetValueOrDefault(),
                        SurrenderValue = riDataGroup.SurrenderValue.GetValueOrDefault(),
                        Gst = riDataGroup.GstAmount.GetValueOrDefault(),
                        DatabaseCommission = riDataGroup.DatabaseCommission.GetValueOrDefault(),
                        BrokerageFee = riDataGroup.BrokerageFee.GetValueOrDefault(),
                        SoaReceivedDate = SoaDataBatchBo.StatementReceivedAt,
                        BordereauxReceivedDate = SoaDataBatchBo.RiDataBatchBo.ReceivedAt,
                        CompanyName = SoaDataBatchBo.CedantBo.Name,
                        RiskQuarter = GetQuarterInfo(riDataGroup.RiskPeriodMonth, riDataGroup.RiskPeriodYear),     
                        TreatyType = riDataGroup.TreatyType,
                        TreatyMode = riDataGroup.PremiumFrequencyCode,
                        CurrencyCode = SoaDataBatchBo.CurrencyCodePickListDetailBo?.Code,
                        CurrencyRate = SoaDataBatchBo.CurrencyRate,
                    };

                    if (string.IsNullOrEmpty(riDataGroup.TreatyType))
                    {
                        if (treatyTypeCode != null)
                            bo.TreatyType = treatyTypeCode.TreatyTypePickListDetailBo?.Code;
                    }

                    var nb = nbs?.Where(q => q.TreatyCode == treatyCode && q.RiskPeriodMonth == riskPeriodMonth && q.RiskPeriodYear == riskPeriodYear && q.CurrencyCode == currencyCode && q.PremiumFrequencyCode == riDataGroup.PremiumFrequencyCode).FirstOrDefault();
                    if (nb != null) bo.NbPremium = nb.TransactionPremium.GetValueOrDefault();

                    var rn = rns?.Where(q => q.TreatyCode == treatyCode && q.RiskPeriodMonth == riskPeriodMonth && q.RiskPeriodYear == riskPeriodYear && q.CurrencyCode == currencyCode && q.PremiumFrequencyCode == riDataGroup.PremiumFrequencyCode).FirstOrDefault();
                    if (rn != null) bo.RnPremium = rn.TransactionPremium.GetValueOrDefault();

                    var al = als?.Where(q => q.TreatyCode == treatyCode && q.RiskPeriodMonth == riskPeriodMonth && q.RiskPeriodYear == riskPeriodYear && q.CurrencyCode == currencyCode && q.PremiumFrequencyCode == riDataGroup.PremiumFrequencyCode).FirstOrDefault();
                    if (al != null) bo.AltPremium = al.TransactionPremium.GetValueOrDefault();

                    if (bo.RiskQuarter == FormatQuarter(SoaDataBatchBo.Quarter))
                    {
                        var claimDataGroup = claimDataGroups?.Where(q => q.TreatyCode == treatyCode && q.SoaQuarter == SoaDataBatchBo.Quarter).FirstOrDefault();
                        if (claimDataGroup != null && updateClaim)
                        {
                            if (bo.CurrencyCode == PickListDetailBo.CurrencyCodeMyr)
                                bo.Claim = claimDataGroup.ClaimRecoveryAmt.GetValueOrDefault();
                            else
                                bo.Claim = claimDataGroup.ForeignClaimRecoveryAmt.GetValueOrDefault();
                            updateClaim = false;
                        } 
                    }
                    bo.GetGrossPremium();
                    bo.GetTotalCommission();
                    bo.GetNetTotalAmount();

                    SoaDataBos.Add(bo);

                    if (!string.IsNullOrEmpty(bo.CurrencyCode) && bo.CurrencyCode != PickListDetailBo.CurrencyCodeMyr)
                    {
                        var MYRbo = new SoaDataBo
                        {
                            BusinessCode = businessOriginCode,
                            TreatyId = SoaDataBatchBo.TreatyBo?.TreatyIdCode,
                            TreatyCode = riDataGroup.TreatyCode,
                            RiskMonth = riDataGroup.RiskPeriodMonth,
                            SoaReceivedDate = SoaDataBatchBo.StatementReceivedAt,
                            BordereauxReceivedDate = SoaDataBatchBo.RiDataBatchBo.ReceivedAt,
                            CompanyName = SoaDataBatchBo.CedantBo.Name,
                            RiskQuarter = GetQuarterInfo(riDataGroup.RiskPeriodMonth, riDataGroup.RiskPeriodYear),
                            TreatyType = riDataGroup.TreatyType,
                            TreatyMode = riDataGroup.PremiumFrequencyCode,
                            CurrencyCode = PickListDetailBo.CurrencyCodeMyr,
                            CurrencyRate = SoaDataBatchBo.CurrencyRate,
                        };
                        MYRbo.NbPremium = bo.NbPremium * MYRbo.CurrencyRate;
                        MYRbo.RnPremium = bo.RnPremium * MYRbo.CurrencyRate;
                        MYRbo.AltPremium = bo.AltPremium * MYRbo.CurrencyRate;

                        MYRbo.TotalDiscount = bo.TotalDiscount * MYRbo.CurrencyRate;
                        MYRbo.RiskPremium = bo.RiskPremium * MYRbo.CurrencyRate;
                        MYRbo.NoClaimBonus = bo.NoClaimBonus * MYRbo.CurrencyRate;
                        MYRbo.Levy = bo.Levy * MYRbo.CurrencyRate;
                        MYRbo.ProfitComm = bo.ProfitComm * MYRbo.CurrencyRate;
                        MYRbo.SurrenderValue = bo.SurrenderValue * MYRbo.CurrencyRate;
                        MYRbo.ModcoReserveIncome = bo.ModcoReserveIncome * MYRbo.CurrencyRate;
                        MYRbo.RiDeposit = bo.RiDeposit * MYRbo.CurrencyRate;
                        MYRbo.DatabaseCommission = bo.DatabaseCommission * MYRbo.CurrencyRate;
                        MYRbo.AdministrationContribution = bo.AdministrationContribution * MYRbo.CurrencyRate;
                        MYRbo.ShareOfRiCommissionFromCompulsoryCession = bo.ShareOfRiCommissionFromCompulsoryCession * MYRbo.CurrencyRate;
                        MYRbo.RecaptureFee = bo.RecaptureFee * MYRbo.CurrencyRate;
                        MYRbo.CreditCardCharges = bo.CreditCardCharges * MYRbo.CurrencyRate;
                        MYRbo.BrokerageFee = bo.BrokerageFee * MYRbo.CurrencyRate;
                        MYRbo.Claim = bo.Claim * MYRbo.CurrencyRate;
                        MYRbo.Gst = bo.Gst * MYRbo.CurrencyRate;

                        MYRbo.GetGrossPremium();
                        MYRbo.GetTotalCommission();
                        MYRbo.GetNetTotalAmount();

                        SoaDataBos.Add(MYRbo);
                    }
                }
            }

            if (!SoaDataBos.IsNullOrEmpty())
            {
                foreach (var soaDataBo in SoaDataBos)
                {
                    soaDataBo.SoaDataBatchId = SoaDataBatchBo.Id;
                    soaDataBo.SoaQuarter = FormatQuarter(SoaDataBatchBo.Quarter);
                    soaDataBo.CreatedById = SoaDataBatchBo.CreatedById;
                    soaDataBo.UpdatedById = SoaDataBatchBo.UpdatedById;
                    soaDataBo.MappingStatus = SoaDataBo.MappingStatusSuccess;
                    var bo = soaDataBo;
                    SoaDataService.Create(ref bo);

                    //Added update for UpdatedAt for fail checking
                    var boToUpdate = SoaDataBatchBo;
                    SoaDataBatchService.Update(ref boToUpdate);
                }
            }
        }

        public void CreateSoaDataByClaimData()
        {
            if (SoaDataBatchBo == null)
                return;

            bool updateClaim = true;
            SoaDataBos = new List<SoaDataBo> { };
            using (var db = new AppDbContext(false))
            {
                var claimDataGroups = QueryClaimDataGroupByAutoCreate(db);
                var claimDataSumGroups = QueryClaimDataGroupBy(db, true);

                if (claimDataGroups.IsNullOrEmpty())
                    return;

                foreach (var claimDataGroup in claimDataGroups)
                {
                    var businessOriginCode = CacheService.BusinessOrigins.Where(q => q.Id == SoaDataBatchBo.TreatyBo?.BusinessOriginPickListDetailId).Select(q => q.Code).FirstOrDefault();
                    var treatyTypeCode = CacheService.TreatyCodeBos.Where(q => q.Code == claimDataGroup.TreatyCode && q.TreatyId == SoaDataBatchBo.TreatyId).FirstOrDefault();
                    var treatyCode = claimDataGroup.TreatyCode;

                    if (claimDataGroup.RiskQuarter == SoaDataBatchBo.Quarter)
                    {
                        var bo = new SoaDataBo
                        {
                            BusinessCode = businessOriginCode,
                            TreatyId = SoaDataBatchBo.TreatyBo?.TreatyIdCode,
                            TreatyCode = claimDataGroup.TreatyCode,
                            TreatyType = claimDataGroup.TreatyType,
                            SoaReceivedDate = SoaDataBatchBo.StatementReceivedAt,
                            CompanyName = SoaDataBatchBo.CedantBo.Name,
                            RiskQuarter = FormatQuarter(claimDataGroup.RiskQuarter),
                            CurrencyCode = SoaDataBatchBo.CurrencyCodePickListDetailBo?.Code,
                            CurrencyRate = SoaDataBatchBo.CurrencyRate,
                        };

                        if (string.IsNullOrEmpty(claimDataGroup.TreatyType))
                        {
                            if (treatyTypeCode != null)
                                bo.TreatyType = treatyTypeCode.TreatyTypePickListDetailBo?.Code;
                        }

                        var claimDataSumGroup = claimDataSumGroups?.Where(q => q.TreatyCode == treatyCode && q.SoaQuarter == SoaDataBatchBo.Quarter).FirstOrDefault();
                        if (claimDataSumGroup != null && updateClaim)
                        {
                            if (bo.CurrencyCode == PickListDetailBo.CurrencyCodeMyr)
                                bo.Claim = claimDataSumGroup.ClaimRecoveryAmt.GetValueOrDefault();
                            else
                                bo.Claim = claimDataSumGroup.ForeignClaimRecoveryAmt.GetValueOrDefault();
                            updateClaim = false;
                        }

                        bo.GetGrossPremium();
                        bo.GetTotalCommission();
                        bo.GetNetTotalAmount();

                        SoaDataBos.Add(bo);

                        if (!string.IsNullOrEmpty(bo.CurrencyCode) && bo.CurrencyCode != PickListDetailBo.CurrencyCodeMyr)
                        {
                            var MYRbo = new SoaDataBo
                            {
                                BusinessCode = businessOriginCode,
                                TreatyId = SoaDataBatchBo.TreatyBo?.TreatyIdCode,
                                TreatyCode = claimDataGroup.TreatyCode,
                                SoaReceivedDate = SoaDataBatchBo.StatementReceivedAt,
                                BordereauxReceivedDate = SoaDataBatchBo.RiDataBatchBo.ReceivedAt,
                                CompanyName = SoaDataBatchBo.CedantBo.Name,
                                RiskQuarter = FormatQuarter(claimDataGroup.RiskQuarter),
                                TreatyType = claimDataGroup.TreatyType,
                                CurrencyCode = PickListDetailBo.CurrencyCodeMyr,
                                CurrencyRate = SoaDataBatchBo.CurrencyRate,
                            };
                            MYRbo.TreatyType = bo.TreatyType;
                            MYRbo.Claim = bo.Claim * MYRbo.CurrencyRate;

                            MYRbo.GetGrossPremium();
                            MYRbo.GetTotalCommission();
                            MYRbo.GetNetTotalAmount();

                            SoaDataBos.Add(MYRbo);
                        }
                    }
                        
                }
            }

            if (!SoaDataBos.IsNullOrEmpty())
            {
                foreach (var soaDataBo in SoaDataBos)
                {
                    soaDataBo.SoaDataBatchId = SoaDataBatchBo.Id;
                    soaDataBo.SoaQuarter = FormatQuarter(SoaDataBatchBo.Quarter);
                    soaDataBo.CreatedById = SoaDataBatchBo.CreatedById;
                    soaDataBo.UpdatedById = SoaDataBatchBo.UpdatedById;
                    soaDataBo.MappingStatus = SoaDataBo.MappingStatusSuccess;
                    var bo = soaDataBo;
                    SoaDataService.Create(ref bo);

                    //Added update for UpdatedAt for fail checking
                    var boToUpdate = SoaDataBatchBo;
                    SoaDataBatchService.Update(ref boToUpdate);
                }
            }
        }

        public void CreateCompiledSummary()
        {
            if (SoaDataBatchBo == null)
                return;

            SoaDataCompiledSummaryBos = new List<SoaDataCompiledSummaryBo> { };
            using (var db = new AppDbContext(false))
            {
                var businessOriginCode = CacheService.BusinessOrigins.Where(q => q.Id == TreatyBo?.BusinessOriginPickListDetailId).Select(q => q.Code).FirstOrDefault();
                if (string.IsNullOrEmpty(businessOriginCode))
                    return;

                var riSummaryGroups = QueryRiSummaryGroupBy();
                var claimDataGroups = QueryClaimDataGroupBy(db);

                var riSummaryEndDateGroups = QueryRiSummaryGroupByEndDate();

                var soaDataGroups = db.SoaData
                    .Where(q => q.SoaDataBatchId == SoaDataBatchBo.Id)
                    .Where(q => q.BusinessCode == businessOriginCode)
                    .GroupBy(q => new { q.TreatyCode, q.RiskQuarter, q.CurrencyCode, q.CurrencyRate })
                    .Select(q => new SoaDataGroupBy
                    {
                        TreatyCode = q.Key.TreatyCode,
                        RiskQuarter = q.Key.RiskQuarter,

                        TotalDiscount = q.Sum(d => d.TotalDiscount),
                        RiskPremium = q.Sum(d => d.RiskPremium),
                        Levy = q.Sum(d => d.Levy),
                        Gst = q.Sum(d => d.Gst),
                        ProfitComm = q.Sum(d => d.ProfitComm),
                        ModcoReserveIncome = q.Sum(d => d.ModcoReserveIncome),
                        RiDeposit = q.Sum(d => d.RiDeposit),
                        AdministrationContribution = q.Sum(d => d.AdministrationContribution),
                        ShareOfRiCommissionFromCompulsoryCession = q.Sum(d => d.ShareOfRiCommissionFromCompulsoryCession),
                        RecaptureFee = q.Sum(d => d.RecaptureFee),
                        CreditCardCharges = q.Sum(d => d.CreditCardCharges),

                        CurrencyCode = q.Key.CurrencyCode,
                        CurrencyRate = q.Key.CurrencyRate,
                    })
                    .OrderBy(q => q.TreatyCode)
                    .ToList();

                // Risk Qtr shall be populate based on risk period in RI Data but when there is only Claim Data in SOA, Risk Qtr will follow SOA Qtr
                if (GetRiDataBatchId() != 0)
                {
                    foreach (var riSummaryGroup in riSummaryGroups)
                    {
                        var compiledSummary = new SoaDataCompiledSummaryBo
                        {
                            SoaDataBatchId = SoaDataBatchBo.Id,
                            ReportingType = SoaDataCompiledSummaryBo.ReportingTypeIFRS4,

                            InvoiceDate = DateTime.Now,
                            StatementReceivedDate = SoaDataBatchBo.StatementReceivedAt,

                            TreatyCode = riSummaryGroup.TreatyCode,
                            RiskQuarter = riSummaryGroup.RiskQuarter,
                            SoaQuarter = FormatQuarter(SoaDataBatchBo.Quarter),
                            BusinessCode = businessOriginCode,
                            Frequency = riSummaryGroup.Frequency,

                            CurrencyCode = riSummaryGroup.CurrencyCode,
                            CurrencyRate = riSummaryGroup.CurrencyRate,

                            NbPremium = Util.RoundNullableValue(riSummaryGroup.NbPremium.GetValueOrDefault(), 2),
                            RnPremium = Util.RoundNullableValue(riSummaryGroup.RnPremium.GetValueOrDefault(), 2),
                            AltPremium = Util.RoundNullableValue(riSummaryGroup.AltPremium.GetValueOrDefault(), 2),

                            NbDiscount = Util.RoundNullableValue(riSummaryGroup.NbDiscount.GetValueOrDefault(), 2),
                            RnDiscount = Util.RoundNullableValue(riSummaryGroup.RnDiscount.GetValueOrDefault(), 2),
                            AltDiscount = Util.RoundNullableValue(riSummaryGroup.AltDiscount.GetValueOrDefault(), 2),

                            DTH = Util.RoundNullableValue(riSummaryGroup.DTH.GetValueOrDefault(), 2),
                            TPA = Util.RoundNullableValue(riSummaryGroup.TPA.GetValueOrDefault(), 2),
                            TPS = Util.RoundNullableValue(riSummaryGroup.TPS.GetValueOrDefault(), 2),
                            PPD = Util.RoundNullableValue(riSummaryGroup.PPD.GetValueOrDefault(), 2),
                            CCA = Util.RoundNullableValue(riSummaryGroup.CCA.GetValueOrDefault(), 2),
                            CCS = Util.RoundNullableValue(riSummaryGroup.CCS.GetValueOrDefault(), 2),
                            PA = Util.RoundNullableValue(riSummaryGroup.PA.GetValueOrDefault(), 2),
                            HS = Util.RoundNullableValue(riSummaryGroup.HS.GetValueOrDefault(), 2),
                            TPD = Util.RoundNullableValue(riSummaryGroup.TPD.GetValueOrDefault(), 2),
                            CI = Util.RoundNullableValue(riSummaryGroup.CI.GetValueOrDefault(), 2),

                            NoClaimBonus = Util.RoundNullableValue(riSummaryGroup.NoClaimBonus.GetValueOrDefault(), 2),
                            SurrenderValue = Util.RoundNullableValue(riSummaryGroup.SurrenderValue.GetValueOrDefault(), 2),
                            DatabaseCommission = Util.RoundNullableValue(riSummaryGroup.DatabaseCommission.GetValueOrDefault(), 2),
                            BrokerageFee = Util.RoundNullableValue(riSummaryGroup.BrokerageFee.GetValueOrDefault(), 2),
                            ServiceFee = Util.RoundNullableValue(riSummaryGroup.ServiceFee.GetValueOrDefault(), 2),
                        };
                        compiledSummary.GetSstPayable();
                        compiledSummary.GetTotalAmount();
                        compiledSummary.GetTotalPremium();

                        // NB/RN/ALT SAR shall populate if it is monthly mode
                        // other than monthly mode, will take sum AAR
                        if (compiledSummary.Frequency == PickListDetailBo.PremiumFrequencyCodeMonthly)
                        {
                            if (!riSummaryEndDateGroups.IsNullOrEmpty())
                            {
                                var riSummaryEndDateGroup = riSummaryEndDateGroups?.Where(q => q.TreatyCode == riSummaryGroup.TreatyCode && q.RiskQuarter == riSummaryGroup.RiskQuarter && q.CurrencyCode == riSummaryGroup.CurrencyCode && q.CurrencyRate == riSummaryGroup.CurrencyRate && q.Frequency == riSummaryGroup.Frequency).FirstOrDefault();
                                if (riSummaryEndDateGroup != null)
                                {
                                    compiledSummary.NbCession = riSummaryGroup.NbCession.GetValueOrDefault();
                                    compiledSummary.RnCession = riSummaryGroup.RnCession.GetValueOrDefault();
                                    compiledSummary.AltCession = riSummaryGroup.AltCession.GetValueOrDefault();

                                    compiledSummary.NbSar = Util.RoundNullableValue(riSummaryEndDateGroup.NbSar.GetValueOrDefault(), 2);
                                    compiledSummary.RnSar = Util.RoundNullableValue(riSummaryEndDateGroup.RnSar.GetValueOrDefault(), 2);
                                    compiledSummary.AltSar = Util.RoundNullableValue(riSummaryEndDateGroup.AltSar.GetValueOrDefault(), 2);
                                }
                            }
                        }                        
                        else
                        {
                            if (riSummaryGroup != null)
                            {
                                compiledSummary.NbCession = riSummaryGroup.NbCession.GetValueOrDefault();
                                compiledSummary.RnCession = riSummaryGroup.RnCession.GetValueOrDefault();
                                compiledSummary.AltCession = riSummaryGroup.AltCession.GetValueOrDefault();

                                compiledSummary.NbSar = Util.RoundNullableValue(riSummaryGroup.NbSar.GetValueOrDefault(), 2);
                                compiledSummary.RnSar = Util.RoundNullableValue(riSummaryGroup.RnSar.GetValueOrDefault(), 2);
                                compiledSummary.AltSar = Util.RoundNullableValue(riSummaryGroup.AltSar.GetValueOrDefault(), 2);
                            }
                        }

                        var soaDataGroup = soaDataGroups?.Where(q => q.TreatyCode == riSummaryGroup.TreatyCode && q.RiskQuarter == riSummaryGroup.RiskQuarter && q.CurrencyCode == riSummaryGroup.CurrencyCode && q.CurrencyRate == riSummaryGroup.CurrencyRate).FirstOrDefault();
                        if (soaDataGroup != null)
                        {
                            compiledSummary.TotalDiscount = Util.RoundNullableValue(soaDataGroup.TotalDiscount.GetValueOrDefault(), 2);
                            compiledSummary.RiskPremium = Util.RoundNullableValue(soaDataGroup.RiskPremium.GetValueOrDefault(), 2);
                            compiledSummary.Levy = Util.RoundNullableValue(soaDataGroup.Levy.GetValueOrDefault(), 2);
                            compiledSummary.Gst = Util.RoundNullableValue(soaDataGroup.Gst.GetValueOrDefault(), 2);
                            compiledSummary.ProfitComm = Util.RoundNullableValue(soaDataGroup.ProfitComm.GetValueOrDefault(), 2);
                            compiledSummary.ModcoReserveIncome = Util.RoundNullableValue(soaDataGroup.ModcoReserveIncome.GetValueOrDefault(), 2);
                            compiledSummary.RiDeposit = Util.RoundNullableValue(soaDataGroup.RiDeposit.GetValueOrDefault(), 2);
                            compiledSummary.AdministrationContribution = Util.RoundNullableValue(soaDataGroup.AdministrationContribution.GetValueOrDefault(), 2);
                            compiledSummary.ShareOfRiCommissionFromCompulsoryCession = Util.RoundNullableValue(soaDataGroup.ShareOfRiCommissionFromCompulsoryCession.GetValueOrDefault(), 2);
                            compiledSummary.RecaptureFee = Util.RoundNullableValue(soaDataGroup.RecaptureFee.GetValueOrDefault(), 2);
                            compiledSummary.CreditCardCharges = Util.RoundNullableValue(soaDataGroup.CreditCardCharges.GetValueOrDefault(), 2);
                            compiledSummary.CurrencyCode = soaDataGroup.CurrencyCode;
                            compiledSummary.CurrencyRate = soaDataGroup.CurrencyRate;
                        }

                        if (compiledSummary.RiskQuarter == compiledSummary.SoaQuarter)
                        {
                            var claimDataGroup = claimDataGroups?.Where(q => q.TreatyCode == riSummaryGroup.TreatyCode && q.SoaQuarter == SoaDataBatchBo.Quarter).FirstOrDefault();
                            if (claimDataGroup != null)
                            {
                                if (compiledSummary.CurrencyCode == PickListDetailBo.CurrencyCodeMyr)
                                    compiledSummary.Claim = Util.RoundNullableValue(claimDataGroup.ClaimRecoveryAmt.GetValueOrDefault(), 2);
                                else
                                    compiledSummary.Claim = Util.RoundNullableValue(claimDataGroup.ForeignClaimRecoveryAmt.GetValueOrDefault(), 2);
                            } 
                        }

                        switch (businessOriginCode)
                        {
                            case PickListDetailBo.BusinessOriginCodeWithinMalaysia:
                                compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeWM;
                                break;
                            case PickListDetailBo.BusinessOriginCodeOutsideMalaysia:
                                compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeOM;
                                break;
                            case PickListDetailBo.BusinessOriginCodeServiceFee:
                                compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeSFWM;
                                break;
                        }

                        compiledSummary.GetNetTotalAmount();
                        if (compiledSummary.RiskQuarter != compiledSummary.SoaQuarter)
                        {
                            switch (businessOriginCode)
                            {
                                case PickListDetailBo.BusinessOriginCodeWithinMalaysia:
                                    if (compiledSummary.NetTotalAmount < 0)
                                        compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeCNWM;
                                    else
                                        compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeDNWM;
                                    break;
                                case PickListDetailBo.BusinessOriginCodeOutsideMalaysia:
                                    if (compiledSummary.NetTotalAmount < 0)
                                        compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeCNOM;
                                    else
                                        compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeDNOM;
                                    break;
                                case PickListDetailBo.BusinessOriginCodeServiceFee:
                                    if (compiledSummary.TotalAmount < 0)
                                        compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeCNSFWM;
                                    else
                                        compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeDNSFWM;
                                    break;
                            }
                        }
                        
                        if (compiledSummary.InvoiceType != SoaDataCompiledSummaryBo.InvoiceTypeWM && compiledSummary.InvoiceType != SoaDataCompiledSummaryBo.InvoiceTypeOM)
                            compiledSummary.Amount1 = compiledSummary.NetTotalAmount;                        

                        SoaDataCompiledSummaryBos.Add(compiledSummary);

                        //Added update for UpdatedAt for fail checking
                        var boToUpdate = SoaDataBatchBo;
                        SoaDataBatchService.Update(ref boToUpdate);
                    }
                }
                else
                {
                    foreach (var soaDataGroup in soaDataGroups)
                    {
                        var compiledSummary = new SoaDataCompiledSummaryBo
                        {
                            SoaDataBatchId = SoaDataBatchBo.Id,
                            ReportingType = SoaDataCompiledSummaryBo.ReportingTypeIFRS4,

                            InvoiceDate = DateTime.Now,
                            StatementReceivedDate = SoaDataBatchBo.StatementReceivedAt,

                            TreatyCode = soaDataGroup.TreatyCode,
                            RiskQuarter = soaDataGroup.RiskQuarter,
                            SoaQuarter = FormatQuarter(SoaDataBatchBo.Quarter),
                            BusinessCode = businessOriginCode,

                            TotalDiscount = Util.RoundNullableValue(soaDataGroup.TotalDiscount.GetValueOrDefault(), 2),
                            RiskPremium = Util.RoundNullableValue(soaDataGroup.RiskPremium.GetValueOrDefault(), 2),
                            Levy = Util.RoundNullableValue(soaDataGroup.Levy.GetValueOrDefault(), 2),
                            Gst = Util.RoundNullableValue(soaDataGroup.Gst.GetValueOrDefault(), 2),
                            ProfitComm = Util.RoundNullableValue(soaDataGroup.ProfitComm.GetValueOrDefault(), 2),
                            ModcoReserveIncome = Util.RoundNullableValue(soaDataGroup.ModcoReserveIncome.GetValueOrDefault(), 2),
                            RiDeposit = Util.RoundNullableValue(soaDataGroup.RiDeposit.GetValueOrDefault(), 2),
                            AdministrationContribution = Util.RoundNullableValue(soaDataGroup.AdministrationContribution.GetValueOrDefault(), 2),
                            ShareOfRiCommissionFromCompulsoryCession = Util.RoundNullableValue(soaDataGroup.ShareOfRiCommissionFromCompulsoryCession.GetValueOrDefault(), 2),
                            RecaptureFee = Util.RoundNullableValue(soaDataGroup.RecaptureFee.GetValueOrDefault(), 2),
                            CreditCardCharges = Util.RoundNullableValue(soaDataGroup.CreditCardCharges.GetValueOrDefault(), 2),

                            CurrencyCode = soaDataGroup.CurrencyCode,
                            CurrencyRate = soaDataGroup.CurrencyRate.GetValueOrDefault(),
                        };

                        var riSummaryGroup = riSummaryGroups?.Where(q => q.TreatyCode == soaDataGroup.TreatyCode && q.RiskQuarter == compiledSummary.RiskQuarter && q.CurrencyCode == compiledSummary.CurrencyCode && q.CurrencyRate == compiledSummary.CurrencyRate).FirstOrDefault();
                        if (riSummaryGroup != null)
                        {
                            compiledSummary.NbPremium = Util.RoundNullableValue(riSummaryGroup.NbPremium.GetValueOrDefault(), 2);
                            compiledSummary.RnPremium = Util.RoundNullableValue(riSummaryGroup.RnPremium.GetValueOrDefault(), 2);
                            compiledSummary.AltPremium = Util.RoundNullableValue(riSummaryGroup.AltPremium.GetValueOrDefault(), 2);

                            compiledSummary.NbDiscount = Util.RoundNullableValue(riSummaryGroup.NbDiscount.GetValueOrDefault(), 2);
                            compiledSummary.RnDiscount = Util.RoundNullableValue(riSummaryGroup.RnDiscount.GetValueOrDefault(), 2);
                            compiledSummary.AltDiscount = Util.RoundNullableValue(riSummaryGroup.AltDiscount.GetValueOrDefault(), 2);

                            compiledSummary.DTH = Util.RoundNullableValue(riSummaryGroup.DTH.GetValueOrDefault(), 2);
                            compiledSummary.TPA = Util.RoundNullableValue(riSummaryGroup.TPA.GetValueOrDefault(), 2);
                            compiledSummary.TPS = Util.RoundNullableValue(riSummaryGroup.TPS.GetValueOrDefault(), 2);
                            compiledSummary.PPD = Util.RoundNullableValue(riSummaryGroup.PPD.GetValueOrDefault(), 2);
                            compiledSummary.CCA = Util.RoundNullableValue(riSummaryGroup.CCA.GetValueOrDefault(), 2);
                            compiledSummary.CCS = Util.RoundNullableValue(riSummaryGroup.CCS.GetValueOrDefault(), 2);
                            compiledSummary.PA = Util.RoundNullableValue(riSummaryGroup.PA.GetValueOrDefault(), 2);
                            compiledSummary.HS = Util.RoundNullableValue(riSummaryGroup.HS.GetValueOrDefault(), 2);
                            compiledSummary.TPD = Util.RoundNullableValue(riSummaryGroup.TPD.GetValueOrDefault(), 2);
                            compiledSummary.CI = Util.RoundNullableValue(riSummaryGroup.CI.GetValueOrDefault(), 2);

                            compiledSummary.NoClaimBonus = Util.RoundNullableValue(riSummaryGroup.NoClaimBonus.GetValueOrDefault(), 2);
                            compiledSummary.SurrenderValue = Util.RoundNullableValue(riSummaryGroup.SurrenderValue.GetValueOrDefault(), 2);
                            compiledSummary.DatabaseCommission = Util.RoundNullableValue(riSummaryGroup.DatabaseCommission.GetValueOrDefault(), 2);
                            compiledSummary.BrokerageFee = Util.RoundNullableValue(riSummaryGroup.BrokerageFee.GetValueOrDefault(), 2);
                            compiledSummary.ServiceFee = Util.RoundNullableValue(riSummaryGroup.ServiceFee.GetValueOrDefault(), 2);

                            compiledSummary.Frequency = riSummaryGroup.Frequency;
                            compiledSummary.CurrencyCode = riSummaryGroup.CurrencyCode;
                            compiledSummary.CurrencyRate = riSummaryGroup.CurrencyRate;
                        }
                        compiledSummary.GetSstPayable();
                        compiledSummary.GetTotalAmount();
                        compiledSummary.GetTotalPremium();

                        // NB/RN/ALT SAR shall populate if it is monthly mode
                        // other than monthly mode, will take sum AAR
                        if (compiledSummary.Frequency == PickListDetailBo.PremiumFrequencyCodeMonthly)
                        {
                            if (!riSummaryEndDateGroups.IsNullOrEmpty())
                            {
                                var riSummaryEndDateGroup = riSummaryEndDateGroups?.Where(q => q.TreatyCode == soaDataGroup.TreatyCode && q.RiskQuarter == compiledSummary.RiskQuarter && q.CurrencyCode == compiledSummary.CurrencyCode && q.CurrencyRate == riSummaryGroup.CurrencyRate && q.Frequency == compiledSummary.Frequency).FirstOrDefault();
                                if (riSummaryEndDateGroup != null)
                                {
                                    compiledSummary.NbCession = riSummaryEndDateGroup.NbCession.GetValueOrDefault();
                                    compiledSummary.RnCession = riSummaryEndDateGroup.RnCession.GetValueOrDefault();
                                    compiledSummary.AltCession = riSummaryEndDateGroup.AltCession.GetValueOrDefault();

                                    compiledSummary.NbSar = Util.RoundNullableValue(riSummaryEndDateGroup.NbSar.GetValueOrDefault(), 2);
                                    compiledSummary.RnSar = Util.RoundNullableValue(riSummaryEndDateGroup.RnSar.GetValueOrDefault(), 2);
                                    compiledSummary.AltSar = Util.RoundNullableValue(riSummaryEndDateGroup.AltSar.GetValueOrDefault(), 2);
                                }
                            }
                        }                        
                        else
                        {
                            if (riSummaryGroup != null)
                            {
                                compiledSummary.NbCession = riSummaryGroup.NbCession.GetValueOrDefault();
                                compiledSummary.RnCession = riSummaryGroup.RnCession.GetValueOrDefault();
                                compiledSummary.AltCession = riSummaryGroup.AltCession.GetValueOrDefault();

                                compiledSummary.NbSar = Util.RoundNullableValue(riSummaryGroup.NbSar.GetValueOrDefault(), 2);
                                compiledSummary.RnSar = Util.RoundNullableValue(riSummaryGroup.RnSar.GetValueOrDefault(), 2);
                                compiledSummary.AltSar = Util.RoundNullableValue(riSummaryGroup.AltSar.GetValueOrDefault(), 2);
                            }
                        }

                        if (compiledSummary.RiskQuarter == compiledSummary.SoaQuarter)
                        {
                            var claimDataGroup = claimDataGroups?.Where(q => q.TreatyCode == soaDataGroup.TreatyCode && q.SoaQuarter == SoaDataBatchBo.Quarter).FirstOrDefault();
                            if (claimDataGroup != null)
                            {
                                if (compiledSummary.CurrencyCode == PickListDetailBo.CurrencyCodeMyr)
                                    compiledSummary.Claim = Util.RoundNullableValue(claimDataGroup.ClaimRecoveryAmt.GetValueOrDefault(), 2);
                                else
                                    compiledSummary.Claim = Util.RoundNullableValue(claimDataGroup.ForeignClaimRecoveryAmt.GetValueOrDefault(), 2);
                            }
                        }

                        switch (businessOriginCode)
                        {
                            case PickListDetailBo.BusinessOriginCodeWithinMalaysia:
                                compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeWM;
                                break;
                            case PickListDetailBo.BusinessOriginCodeOutsideMalaysia:
                                compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeOM;
                                break;
                            case PickListDetailBo.BusinessOriginCodeServiceFee:
                                compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeSFWM;
                                break;
                        }

                        compiledSummary.GetNetTotalAmount();
                        if (compiledSummary.RiskQuarter != compiledSummary.SoaQuarter)
                        {
                            switch (businessOriginCode)
                            {
                                case PickListDetailBo.BusinessOriginCodeWithinMalaysia:
                                    if (compiledSummary.NetTotalAmount < 0)
                                        compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeCNWM;
                                    else
                                        compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeDNWM;
                                    break;
                                case PickListDetailBo.BusinessOriginCodeOutsideMalaysia:
                                    if (compiledSummary.NetTotalAmount < 0)
                                        compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeCNOM;
                                    else
                                        compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeDNOM;
                                    break;
                                case PickListDetailBo.BusinessOriginCodeServiceFee:
                                    if (compiledSummary.TotalAmount < 0)
                                        compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeCNSFWM;
                                    else
                                        compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeDNSFWM;
                                    break;
                            }
                        }
                        
                        if (compiledSummary.InvoiceType != SoaDataCompiledSummaryBo.InvoiceTypeWM && compiledSummary.InvoiceType != SoaDataCompiledSummaryBo.InvoiceTypeOM)
                            compiledSummary.Amount1 = compiledSummary.NetTotalAmount;                        

                        SoaDataCompiledSummaryBos.Add(compiledSummary);

                        //Added update for UpdatedAt for fail checking
                        var boToUpdate = SoaDataBatchBo;
                        SoaDataBatchService.Update(ref boToUpdate);
                    }
                }

            }
        }

        public void CreateDiscrepancy()
        {
            if (SoaDataBatchBo == null)
                return;

            SoaDataDiscrepancyBos = new List<SoaDataDiscrepancyBo> { };
            using (var db = new AppDbContext(false))
            {
                var riDataGroups = QueryRiDataDiscrepancySum(db);

                if (riDataGroups.IsNullOrEmpty())
                    return;

                foreach (var riDataGroup in riDataGroups)
                {
                    var businessOriginCode = CacheService.BusinessOrigins.Where(q => q.Id == TreatyBo?.BusinessOriginPickListDetailId).Select(q => q.Code).FirstOrDefault();
                    var treatyCode = riDataGroup.TreatyCode;

                    if (businessOriginCode != PickListDetailBo.BusinessOriginCodeServiceFee)
                    {
                        var mlreShareDiscrepancy = new SoaDataDiscrepancyBo
                        {
                            Type = SoaDataDiscrepancyBo.TypeMlreShare,
                            TreatyCode = riDataGroup.TreatyCode,
                            CedingPlanCode = riDataGroup.CedingPlanCode,

                            CedantAmount = riDataGroup.TransactionPremium.GetValueOrDefault(),
                            MlreChecking = riDataGroup.MlreGrossPremium.GetValueOrDefault(),

                            CurrencyCode = riDataGroup.CurrencyCode,
                            CurrencyRate = riDataGroup.CurrencyRate.GetValueOrDefault(),
                        };
                        mlreShareDiscrepancy.GetDiscrepancy();

                        var layer1ShareDiscrepancy = new SoaDataDiscrepancyBo
                        {
                            Type = SoaDataDiscrepancyBo.TypeLayer1Share,
                            TreatyCode = riDataGroup.TreatyCode,
                            CedingPlanCode = riDataGroup.CedingPlanCode,

                            CedantAmount = riDataGroup.TransactionPremium.GetValueOrDefault(),
                            MlreChecking = riDataGroup.MlreGrossPremium.GetValueOrDefault() * 2,

                            CurrencyCode = riDataGroup.CurrencyCode,
                            CurrencyRate = riDataGroup.CurrencyRate.GetValueOrDefault(),
                        };
                        mlreShareDiscrepancy.GetDiscrepancy();

                        SoaDataDiscrepancyBos.Add(mlreShareDiscrepancy);
                        SoaDataDiscrepancyBos.Add(layer1ShareDiscrepancy);

                        if (riDataGroup.CurrencyCode != PickListDetailBo.CurrencyCodeMyr)
                        {
                            var mlreShareDiscrepancyMYR = new SoaDataDiscrepancyBo
                            {
                                Type = SoaDataDiscrepancyBo.TypeMlreShare,
                                TreatyCode = riDataGroup.TreatyCode,
                                CedingPlanCode = riDataGroup.CedingPlanCode,

                                CedantAmount = riDataGroup.TransactionPremium.GetValueOrDefault(),
                                MlreChecking = riDataGroup.MlreGrossPremium.GetValueOrDefault(),

                                CurrencyCode = PickListDetailBo.CurrencyCodeMyr,
                                CurrencyRate = riDataGroup.CurrencyRate.GetValueOrDefault(),
                            };
                            mlreShareDiscrepancyMYR.CedantAmount = mlreShareDiscrepancy.CedantAmount * mlreShareDiscrepancyMYR.CurrencyRate;
                            mlreShareDiscrepancyMYR.MlreChecking = mlreShareDiscrepancy.MlreChecking * mlreShareDiscrepancyMYR.CurrencyRate;
                            mlreShareDiscrepancyMYR.GetDiscrepancy();

                            SoaDataDiscrepancyBos.Add(mlreShareDiscrepancyMYR);
                        }
                    }
                    else
                    {
                        var retakafulDiscrepancy = new SoaDataDiscrepancyBo
                        {
                            Type = SoaDataDiscrepancyBo.TypeRetakaful,
                            TreatyCode = riDataGroup.TreatyCode,
                            CedingPlanCode = riDataGroup.CedingPlanCode,

                            CedantAmount = riDataGroup.TransactionPremium.GetValueOrDefault(),
                            MlreChecking = riDataGroup.MlreGrossPremium.GetValueOrDefault(),

                            CurrencyCode = riDataGroup.CurrencyCode,
                            CurrencyRate = riDataGroup.CurrencyRate.GetValueOrDefault(),
                        };
                        retakafulDiscrepancy.GetDiscrepancy();

                        SoaDataDiscrepancyBos.Add(retakafulDiscrepancy);
                    }
                }   
            }
        }

        public void CreateRiSummaryValidationIfrs17()
        {
            bool updateClaim = true;
            if (SoaDataRiDataSummaryBos.IsNullOrEmpty())
                SoaDataRiDataSummaryBos = new List<SoaDataRiDataSummaryBo> { };
            using (var db = new AppDbContext(false))
            {
                var riDataGroups = QueryRiDataGroupBy(db, true);
                var claimDataGroups = QueryRiDataSummaryIfrs17ClaimDataGroupBy(db);

                var soaDataGroups = db.SoaData
                    .Where(q => q.SoaDataBatchId == SoaDataBatchBo.Id)
                    .GroupBy(q => new { q.BusinessCode, q.TreatyCode, q.RiskMonth, q.RiskQuarter, q.CurrencyCode, q.CurrencyRate })
                    .Select(q => new SoaDataRiDataSummaryBo
                    {
                        BusinessCode = q.Key.BusinessCode,
                        TreatyCode = q.Key.TreatyCode,
                        RiskMonth = q.Key.RiskMonth,
                        RiskQuarter = q.Key.RiskQuarter,

                        CurrencyCode = q.Key.CurrencyCode,
                        CurrencyRate = q.Key.CurrencyRate,

                        TotalDiscount = q.Sum(d => d.TotalDiscount),
                        GrossPremium = q.Sum(d => d.GrossPremium),
                        NetTotalAmount = q.Sum(d => d.NetTotalAmount),

                        NbPremium = q.Sum(d => d.NbPremium),
                        RnPremium = q.Sum(d => d.RnPremium),
                        AltPremium = q.Sum(d => d.AltPremium),

                        NoClaimBonus = q.Sum(d => d.NoClaimBonus),
                        Claim = q.Sum(d => d.Claim),
                        SurrenderValue = q.Sum(d => d.SurrenderValue),
                        DatabaseCommission = q.Sum(d => d.DatabaseCommission),
                        BrokerageFee = q.Sum(d => d.BrokerageFee),
                        Gst = q.Sum(d => d.Gst),
                    })
                    .OrderBy(q => q.TreatyCode)
                    .ToList();

                if (!riDataGroups.IsNullOrEmpty())
                {
                    var nbs = QueryRiDataSumByTransactionType(db, PickListDetailBo.TransactionTypeCodeNewBusiness, true);
                    var rns = QueryRiDataSumByTransactionType(db, PickListDetailBo.TransactionTypeCodeRenewal, true);
                    var als = QueryRiDataSumByTransactionType(db, PickListDetailBo.TransactionTypeCodeAlteration, true);

                    var nbCountsMonthly = QueryCountRiDataPostValidationTransactionType(db, PickListDetailBo.TransactionTypeCodeNewBusiness, true,true);
                    var nbCounts = QueryCountRiDataPostValidationTransactionType(db, PickListDetailBo.TransactionTypeCodeNewBusiness, false, true);

                    var rnCountsMonthly = QueryCountRiDataPostValidationTransactionType(db, PickListDetailBo.TransactionTypeCodeRenewal, true, true);
                    var rnCounts = QueryCountRiDataPostValidationTransactionType(db, PickListDetailBo.TransactionTypeCodeRenewal, false, true);

                    var alCountsMonthly = QueryCountRiDataPostValidationTransactionType(db, PickListDetailBo.TransactionTypeCodeAlteration, true, true);
                    var alCounts = QueryCountRiDataPostValidationTransactionType(db, PickListDetailBo.TransactionTypeCodeAlteration, false, true);

                    var dths = QueryRiDataPostValidationBenefitCode(db, BenefitBo.CodeDTH, true);
                    var tpas = QueryRiDataPostValidationBenefitCode(db, BenefitBo.CodeTPA, true);
                    var tpss = QueryRiDataPostValidationBenefitCode(db, BenefitBo.CodeTPS, true);
                    var ppds = QueryRiDataPostValidationBenefitCode(db, BenefitBo.CodePPD, true);
                    var ccas = QueryRiDataPostValidationBenefitCode(db, BenefitBo.CodeCCA, true);
                    var ccss = QueryRiDataPostValidationBenefitCode(db, BenefitBo.CodeCCS, true);
                    var pas = QueryRiDataPostValidationBenefitCode(db, BenefitBo.CodePA, true);
                    var hss = QueryRiDataPostValidationBenefitCode(db, BenefitBo.CodeHS, true);
                    var tpds = QueryRiDataPostValidationBenefitCode(db, BenefitBo.CodeTPD, true);
                    var cis = QueryRiDataPostValidationBenefitCode(db, BenefitBo.CodeCI, true);

                    foreach (var riDataGroup in riDataGroups)
                    {
                        var businessOriginCode = CacheService.BusinessOrigins.Where(q => q.Id == TreatyBo?.BusinessOriginPickListDetailId).Select(q => q.Code).FirstOrDefault();
                        var treatyCode = riDataGroup.TreatyCode;
                        var riskPeriodMonth = riDataGroup.RiskPeriodMonth;
                        var riskPeriodYear = riDataGroup.RiskPeriodYear;
                        var currencyCode = riDataGroup.CurrencyCode;
                        var summary = new SoaDataRiDataSummaryBo
                        {
                            Type = SoaDataRiDataSummaryBo.TypeRiDataSummaryIfrs17,
                            BusinessCode = businessOriginCode,
                            TreatyCode = riDataGroup.TreatyCode,
                            RiskMonth = riDataGroup.RiskPeriodMonth.GetValueOrDefault(),
                            TotalDiscount = riDataGroup.TransactionDiscount.GetValueOrDefault(),
                            RiskQuarter = GetQuarterInfo(riDataGroup.RiskPeriodMonth, riDataGroup.RiskPeriodYear),
                            CurrencyCode = riDataGroup.CurrencyCode,
                            CurrencyRate = SoaDataBatchBo.CurrencyRate,
                            Frequency = riDataGroup.PremiumFrequencyCode,
                            ContractCode = riDataGroup.ContractCode,
                            AnnualCohort = riDataGroup.AnnualCohort,
                            SoaQuarter = FormatQuarter(SoaDataBatchBo.Quarter),
                        };
                        if (string.IsNullOrEmpty(summary.CurrencyCode)) summary.CurrencyCode = SoaDataBatchBo.CurrencyCodePickListDetailBo?.Code;

                        var nb = nbs?.Where(q => q.TreatyCode == treatyCode && q.RiskPeriodMonth == riskPeriodMonth && q.RiskPeriodYear == riskPeriodYear && q.CurrencyCode == currencyCode && q.PremiumFrequencyCode == riDataGroup.PremiumFrequencyCode && q.ContractCode == riDataGroup.ContractCode && q.AnnualCohort == riDataGroup.AnnualCohort).FirstOrDefault();
                        if (nb != null)
                        {
                            summary.NbPremium = nb.TransactionPremium.GetValueOrDefault();
                            summary.NbDiscount = nb.TransactionDiscount.GetValueOrDefault();
                            summary.NbSar = nb.Aar;
                        }

                        var rn = rns?.Where(q => q.TreatyCode == treatyCode && q.RiskPeriodMonth == riskPeriodMonth && q.RiskPeriodYear == riskPeriodYear && q.CurrencyCode == currencyCode && q.PremiumFrequencyCode == riDataGroup.PremiumFrequencyCode && q.ContractCode == riDataGroup.ContractCode && q.AnnualCohort == riDataGroup.AnnualCohort).FirstOrDefault();
                        if (rn != null)
                        {
                            summary.RnPremium = rn.TransactionPremium.GetValueOrDefault();
                            summary.RnDiscount = rn.TransactionDiscount.GetValueOrDefault();
                            summary.RnSar = rn.Aar;
                        }

                        var al = als?.Where(q => q.TreatyCode == treatyCode && q.RiskPeriodMonth == riskPeriodMonth && q.RiskPeriodYear == riskPeriodYear && q.CurrencyCode == currencyCode && q.PremiumFrequencyCode == riDataGroup.PremiumFrequencyCode && q.ContractCode == riDataGroup.ContractCode && q.AnnualCohort == riDataGroup.AnnualCohort).FirstOrDefault();
                        if (al != null)
                        {
                            summary.AltPremium = al.TransactionPremium.GetValueOrDefault();
                            summary.AltDiscount = al.TransactionDiscount.GetValueOrDefault();
                            summary.AltSar = al.Aar;
                        }

                        summary.GetGrossPremium();

                        summary.NoClaimBonus = riDataGroup.NoClaimBonus.GetValueOrDefault();
                        summary.DatabaseCommission = riDataGroup.DatabaseCommission.GetValueOrDefault();
                        summary.BrokerageFee = riDataGroup.BrokerageFee.GetValueOrDefault();
                        summary.ServiceFee = riDataGroup.ServiceFee.GetValueOrDefault();
                        summary.Gst = riDataGroup.GstAmount.GetValueOrDefault();
                        summary.SurrenderValue = riDataGroup.SurrenderValue.GetValueOrDefault();

                        if (summary.RiskQuarter == FormatQuarter(SoaDataBatchBo.Quarter))
                        {
                            var claimDataGroup = claimDataGroups?.Where(q => q.TreatyCode == treatyCode && q.SoaQuarter == SoaDataBatchBo.Quarter && q.ContractCode == riDataGroup.ContractCode && q.AnnualCohort == riDataGroup.AnnualCohort).FirstOrDefault();
                            if (claimDataGroup != null && updateClaim)
                            {
                                if (summary.CurrencyCode == PickListDetailBo.CurrencyCodeMyr)
                                    summary.Claim = claimDataGroup.ClaimRecoveryAmt.GetValueOrDefault();
                                else
                                    summary.Claim = claimDataGroup.ForeignClaimRecoveryAmt.GetValueOrDefault();
                                updateClaim = false;
                            }
                        }
                        summary.GetNetTotalAmount();

                        var nbCountMonthly = nbCountsMonthly?.Where(q => q.TreatyCode == treatyCode && q.RiskPeriodMonth == riskPeriodMonth && q.RiskPeriodYear == riskPeriodYear && q.CurrencyCode == currencyCode && q.PremiumFrequencyCode == riDataGroup.PremiumFrequencyCode && q.ContractCode == riDataGroup.ContractCode && q.AnnualCohort == riDataGroup.AnnualCohort).FirstOrDefault();
                        var nbCount = nbCounts?.Where(q => q.TreatyCode == treatyCode && q.RiskPeriodMonth == riskPeriodMonth && q.RiskPeriodYear == riskPeriodYear && q.CurrencyCode == currencyCode && q.ContractCode == riDataGroup.ContractCode && q.PremiumFrequencyCode == riDataGroup.PremiumFrequencyCode && q.AnnualCohort == riDataGroup.AnnualCohort).FirstOrDefault();
                        int? nbCession = 0;
                        if(summary.Frequency == PickListDetailBo.PremiumFrequencyCodeMonthly)
                        {
                            if (nbCountMonthly != null)
                                nbCession += nbCountMonthly.Total;
                        }
                        else
                        {
                            if (nbCount != null)
                                nbCession += nbCount.Total;
                        }

                        var rnCountMonthly = rnCountsMonthly?.Where(q => q.TreatyCode == treatyCode && q.RiskPeriodMonth == riskPeriodMonth && q.RiskPeriodYear == riskPeriodYear && q.CurrencyCode == currencyCode && q.PremiumFrequencyCode == riDataGroup.PremiumFrequencyCode && q.ContractCode == riDataGroup.ContractCode && q.AnnualCohort == riDataGroup.AnnualCohort).FirstOrDefault();
                        var rnCount = rnCounts?.Where(q => q.TreatyCode == treatyCode && q.RiskPeriodMonth == riskPeriodMonth && q.RiskPeriodYear == riskPeriodYear && q.CurrencyCode == currencyCode && q.ContractCode == riDataGroup.ContractCode && q.PremiumFrequencyCode == riDataGroup.PremiumFrequencyCode && q.AnnualCohort == riDataGroup.AnnualCohort).FirstOrDefault();
                        int? rnCession = 0;
                        if (summary.Frequency == PickListDetailBo.PremiumFrequencyCodeMonthly)
                        {
                            if (rnCountMonthly != null)
                                rnCession += rnCountMonthly.Total;
                        }
                        else
                        {
                            if (rnCount != null)
                                rnCession += rnCount.Total;
                        }                        

                        var alCountMonthly = alCountsMonthly?.Where(q => q.TreatyCode == treatyCode && q.RiskPeriodMonth == riskPeriodMonth && q.RiskPeriodYear == riskPeriodYear && q.CurrencyCode == currencyCode && q.PremiumFrequencyCode == riDataGroup.PremiumFrequencyCode && q.ContractCode == riDataGroup.ContractCode && q.AnnualCohort == riDataGroup.AnnualCohort).FirstOrDefault();
                        var alCount = alCounts?.Where(q => q.TreatyCode == treatyCode && q.RiskPeriodMonth == riskPeriodMonth && q.RiskPeriodYear == riskPeriodYear && q.CurrencyCode == currencyCode && q.PremiumFrequencyCode == riDataGroup.PremiumFrequencyCode && q.ContractCode == riDataGroup.ContractCode && q.AnnualCohort == riDataGroup.AnnualCohort).FirstOrDefault();
                        int? alCession = 0;
                        if (summary.Frequency == PickListDetailBo.PremiumFrequencyCodeMonthly)
                        {
                            if (alCountMonthly != null)
                                alCession += alCountMonthly.Total;
                        }
                        else
                        {
                            if (alCount != null)
                                alCession += alCount.Total;
                        }

                        summary.NbCession = nbCession.GetValueOrDefault();
                        summary.RnCession = rnCession.GetValueOrDefault();
                        summary.AltCession = alCession.GetValueOrDefault();


                        var dth = dths?.Where(q => q.TreatyCode == treatyCode && q.RiskPeriodMonth == riskPeriodMonth && q.RiskPeriodYear == riskPeriodYear && q.CurrencyCode == currencyCode && q.PremiumFrequencyCode == riDataGroup.PremiumFrequencyCode && q.ContractCode == riDataGroup.ContractCode && q.AnnualCohort == riDataGroup.AnnualCohort).FirstOrDefault();
                        if (dth != null)
                            summary.DTH = dth.TransactionPremium.GetValueOrDefault();

                        var tpa = tpas?.Where(q => q.TreatyCode == treatyCode && q.RiskPeriodMonth == riskPeriodMonth && q.RiskPeriodYear == riskPeriodYear && q.CurrencyCode == currencyCode && q.PremiumFrequencyCode == riDataGroup.PremiumFrequencyCode && q.ContractCode == riDataGroup.ContractCode && q.AnnualCohort == riDataGroup.AnnualCohort).FirstOrDefault();
                        if (tpa != null)
                            summary.TPA = tpa.TransactionPremium.GetValueOrDefault();

                        var tps = tpss?.Where(q => q.TreatyCode == treatyCode && q.RiskPeriodMonth == riskPeriodMonth && q.RiskPeriodYear == riskPeriodYear && q.CurrencyCode == currencyCode && q.PremiumFrequencyCode == riDataGroup.PremiumFrequencyCode && q.ContractCode == riDataGroup.ContractCode && q.AnnualCohort == riDataGroup.AnnualCohort).FirstOrDefault();
                        if (tps != null)
                            summary.TPS = tps.TransactionPremium.GetValueOrDefault();

                        var ppd = ppds?.Where(q => q.TreatyCode == treatyCode && q.RiskPeriodMonth == riskPeriodMonth && q.RiskPeriodYear == riskPeriodYear && q.CurrencyCode == currencyCode && q.PremiumFrequencyCode == riDataGroup.PremiumFrequencyCode && q.ContractCode == riDataGroup.ContractCode && q.AnnualCohort == riDataGroup.AnnualCohort).FirstOrDefault();
                        if (ppd != null)
                            summary.PPD = ppd.TransactionPremium.GetValueOrDefault();

                        var cca = ccas?.Where(q => q.TreatyCode == treatyCode && q.RiskPeriodMonth == riskPeriodMonth && q.RiskPeriodYear == riskPeriodYear && q.CurrencyCode == currencyCode && q.PremiumFrequencyCode == riDataGroup.PremiumFrequencyCode && q.ContractCode == riDataGroup.ContractCode && q.AnnualCohort == riDataGroup.AnnualCohort).FirstOrDefault();
                        if (cca != null)
                            summary.CCA = cca.TransactionPremium.GetValueOrDefault();

                        var ccs = ccss?.Where(q => q.TreatyCode == treatyCode && q.RiskPeriodMonth == riskPeriodMonth && q.RiskPeriodYear == riskPeriodYear && q.CurrencyCode == currencyCode && q.PremiumFrequencyCode == riDataGroup.PremiumFrequencyCode && q.ContractCode == riDataGroup.ContractCode && q.AnnualCohort == riDataGroup.AnnualCohort).FirstOrDefault();
                        if (ccs != null)
                            summary.CCS = ccs.TransactionPremium.GetValueOrDefault();

                        var pa = pas?.Where(q => q.TreatyCode == treatyCode && q.RiskPeriodMonth == riskPeriodMonth && q.RiskPeriodYear == riskPeriodYear && q.CurrencyCode == currencyCode && q.PremiumFrequencyCode == riDataGroup.PremiumFrequencyCode && q.ContractCode == riDataGroup.ContractCode && q.AnnualCohort == riDataGroup.AnnualCohort).FirstOrDefault();
                        if (pa != null)
                            summary.PA = pa.TransactionPremium.GetValueOrDefault();

                        var hs = hss?.Where(q => q.TreatyCode == treatyCode && q.RiskPeriodMonth == riskPeriodMonth && q.RiskPeriodYear == riskPeriodYear && q.CurrencyCode == currencyCode && q.PremiumFrequencyCode == riDataGroup.PremiumFrequencyCode && q.ContractCode == riDataGroup.ContractCode && q.AnnualCohort == riDataGroup.AnnualCohort).FirstOrDefault();
                        if (hs != null)
                            summary.HS = hs.TransactionPremium.GetValueOrDefault();

                        var tpd = tpds?.Where(q => q.TreatyCode == treatyCode && q.RiskPeriodMonth == riskPeriodMonth && q.RiskPeriodYear == riskPeriodYear && q.CurrencyCode == currencyCode && q.PremiumFrequencyCode == riDataGroup.PremiumFrequencyCode && q.ContractCode == riDataGroup.ContractCode && q.AnnualCohort == riDataGroup.AnnualCohort).FirstOrDefault();
                        if (tpd != null)
                            summary.TPD = tpd.TransactionPremium.GetValueOrDefault();

                        var ci = cis?.Where(q => q.TreatyCode == treatyCode && q.RiskPeriodMonth == riskPeriodMonth && q.RiskPeriodYear == riskPeriodYear && q.CurrencyCode == currencyCode && q.PremiumFrequencyCode == riDataGroup.PremiumFrequencyCode && q.ContractCode == riDataGroup.ContractCode && q.AnnualCohort == riDataGroup.AnnualCohort).FirstOrDefault();
                        if (ci != null)
                            summary.CI = ci.TransactionPremium.GetValueOrDefault();

                        SoaDataRiDataSummaryBos.Add(summary);

                        if (!string.IsNullOrEmpty(summary.CurrencyCode) && summary.CurrencyCode != PickListDetailBo.CurrencyCodeMyr)
                        {
                            var summaryMYR = new SoaDataRiDataSummaryBo
                            {
                                Type = SoaDataRiDataSummaryBo.TypeRiDataSummaryIfrs17,
                                BusinessCode = businessOriginCode,
                                TreatyCode = riDataGroup.TreatyCode,
                                RiskMonth = riDataGroup.RiskPeriodMonth.GetValueOrDefault(),
                                TotalDiscount = riDataGroup.TransactionDiscount.GetValueOrDefault(),
                                RiskQuarter = GetQuarterInfo(riDataGroup.RiskPeriodMonth, riDataGroup.RiskPeriodYear),
                                CurrencyCode = PickListDetailBo.CurrencyCodeMyr,
                                CurrencyRate = SoaDataBatchBo.CurrencyRate,
                                Frequency = riDataGroup.PremiumFrequencyCode,
                                ContractCode = riDataGroup.ContractCode,
                                AnnualCohort = riDataGroup.AnnualCohort,
                                SoaQuarter = FormatQuarter(SoaDataBatchBo.Quarter),
                            };
                            summaryMYR.NbCession = summary.NbCession;
                            summaryMYR.RnCession = summary.RnCession;
                            summaryMYR.AltCession = summary.AltCession;

                            summaryMYR.NbPremium = summary.NbPremium * summaryMYR.CurrencyRate;
                            summaryMYR.RnPremium = summary.RnPremium * summaryMYR.CurrencyRate;
                            summaryMYR.AltPremium = summary.AltPremium * summaryMYR.CurrencyRate;

                            summaryMYR.NbDiscount = summary.NbDiscount * summaryMYR.CurrencyRate;
                            summaryMYR.RnDiscount = summary.RnDiscount * summaryMYR.CurrencyRate;
                            summaryMYR.AltDiscount = summary.AltDiscount * summaryMYR.CurrencyRate;

                            summaryMYR.NbSar = summary.NbSar * summaryMYR.CurrencyRate;
                            summaryMYR.RnSar = summary.RnSar * summaryMYR.CurrencyRate;
                            summaryMYR.AltSar = summary.AltSar * summaryMYR.CurrencyRate;

                            summaryMYR.TotalDiscount = summary.TotalDiscount * summaryMYR.CurrencyRate;
                            summaryMYR.NoClaimBonus = summary.NoClaimBonus * summaryMYR.CurrencyRate;
                            summaryMYR.SurrenderValue = summary.SurrenderValue * summaryMYR.CurrencyRate;
                            summaryMYR.DatabaseCommission = summary.DatabaseCommission * summaryMYR.CurrencyRate;
                            summaryMYR.BrokerageFee = summary.BrokerageFee * summaryMYR.CurrencyRate;
                            summaryMYR.ServiceFee = summary.ServiceFee * summaryMYR.CurrencyRate;
                            summaryMYR.Gst = summary.Gst * summaryMYR.CurrencyRate;
                            summaryMYR.Claim = summary.Claim * summaryMYR.CurrencyRate;

                            summaryMYR.DTH = summary.DTH * summaryMYR.CurrencyRate;
                            summaryMYR.TPA = summary.TPA * summaryMYR.CurrencyRate;
                            summaryMYR.TPS = summary.TPS * summaryMYR.CurrencyRate;
                            summaryMYR.PPD = summary.PPD * summaryMYR.CurrencyRate;
                            summaryMYR.CCA = summary.CCA * summaryMYR.CurrencyRate;
                            summaryMYR.CCS = summary.CCS * summaryMYR.CurrencyRate;
                            summaryMYR.PA = summary.PA * summaryMYR.CurrencyRate;
                            summaryMYR.HS = summary.HS * summaryMYR.CurrencyRate;
                            summaryMYR.TPD = summary.TPD * summaryMYR.CurrencyRate;
                            summaryMYR.CI = summary.CI * summaryMYR.CurrencyRate;

                            summaryMYR.GetGrossPremium();
                            summaryMYR.GetNetTotalAmount();

                            SoaDataRiDataSummaryBos.Add(summaryMYR);
                        }
                    }
                }
                else
                {
                    // Only when Soa Data auto create from Claim Data
                    if (SoaDataBatchBo.IsClaimDataAutoCreate == true && !soaDataGroups.IsNullOrEmpty())
                    {
                        var claimRegisterGroups = QueryClaimDataGroupBy(db);
                        foreach (var soaDataGroup in soaDataGroups)
                        {
                            var summary = new SoaDataRiDataSummaryBo
                            {
                                Type = SoaDataRiDataSummaryBo.TypeRiDataSummaryIfrs17,
                                BusinessCode = soaDataGroup.BusinessCode,
                                TreatyCode = soaDataGroup.TreatyCode,
                                RiskQuarter = soaDataGroup.RiskQuarter,
                                SoaQuarter = FormatQuarter(SoaDataBatchBo.Quarter),
                                CurrencyCode = SoaDataBatchBo.CurrencyCodePickListDetailBo?.Code,
                                CurrencyRate = SoaDataBatchBo.CurrencyRate,
                            };

                            if (summary.RiskQuarter == FormatQuarter(SoaDataBatchBo.Quarter))
                            {
                                var claimDataGroup = claimRegisterGroups?.Where(q => q.TreatyCode == summary.TreatyCode && q.SoaQuarter == SoaDataBatchBo.Quarter).FirstOrDefault();
                                if (claimDataGroup != null && updateClaim)
                                {
                                    if (summary.CurrencyCode == PickListDetailBo.CurrencyCodeMyr)
                                        summary.Claim = claimDataGroup.ClaimRecoveryAmt.GetValueOrDefault();
                                    else
                                        summary.Claim = claimDataGroup.ForeignClaimRecoveryAmt.GetValueOrDefault();
                                    updateClaim = false;
                                }
                            }

                            SoaDataRiDataSummaryBos.Add(summary);

                            if (!string.IsNullOrEmpty(summary.CurrencyCode) && summary.CurrencyCode != PickListDetailBo.CurrencyCodeMyr)
                            {
                                var summaryMYR = new SoaDataRiDataSummaryBo
                                {
                                    Type = SoaDataRiDataSummaryBo.TypeRiDataSummaryIfrs17,
                                    BusinessCode = soaDataGroup.BusinessCode,
                                    TreatyCode = soaDataGroup.TreatyCode,
                                    RiskQuarter = FormatQuarter(soaDataGroup.RiskQuarter),
                                    SoaQuarter = FormatQuarter(SoaDataBatchBo.Quarter),
                                    CurrencyCode = SoaDataBatchBo.CurrencyCodePickListDetailBo?.Code,
                                    CurrencyRate = SoaDataBatchBo.CurrencyRate,
                                };
                                summaryMYR.Claim = summary.Claim * summaryMYR.CurrencyRate;
                                summaryMYR.GetGrossPremium();
                                summaryMYR.GetNetTotalAmount();

                                SoaDataRiDataSummaryBos.Add(summaryMYR);
                            }
                        }
                    }
                }
            }
        }

        public void CreateCompiledSummaryIFRS17()
        {
            if (SoaDataBatchBo == null)
                return;

            if (SoaDataCompiledSummaryBos.IsNullOrEmpty())
                SoaDataCompiledSummaryBos = new List<SoaDataCompiledSummaryBo> { };
            using (var db = new AppDbContext(false))
            {
                var businessOriginCode = CacheService.BusinessOrigins.Where(q => q.Id == TreatyBo?.BusinessOriginPickListDetailId).Select(q => q.Code).FirstOrDefault();
                if (string.IsNullOrEmpty(businessOriginCode))
                    return;

                double? soaDataRiskPremium = 0;
                var soaDataGroups = db.SoaData
                    .Where(q => q.SoaDataBatchId == SoaDataBatchBo.Id)
                    .Where(q => q.BusinessCode == businessOriginCode)
                    .GroupBy(q => new { q.TreatyCode, q.RiskQuarter, q.CurrencyCode, q.CurrencyRate })
                    .Select(q => new SoaDataGroupBy
                    {
                        TreatyCode = q.Key.TreatyCode,
                        RiskQuarter = q.Key.RiskQuarter,

                        TotalDiscount = q.Sum(d => d.TotalDiscount),
                        RiskPremium = q.Sum(d => d.RiskPremium),
                        Levy = q.Sum(d => d.Levy),
                        Gst = q.Sum(d => d.Gst),
                        ProfitComm = q.Sum(d => d.ProfitComm),
                        ModcoReserveIncome = q.Sum(d => d.ModcoReserveIncome),
                        RiDeposit = q.Sum(d => d.RiDeposit),
                        AdministrationContribution = q.Sum(d => d.AdministrationContribution),
                        ShareOfRiCommissionFromCompulsoryCession = q.Sum(d => d.ShareOfRiCommissionFromCompulsoryCession),
                        RecaptureFee = q.Sum(d => d.RecaptureFee),
                        CreditCardCharges = q.Sum(d => d.CreditCardCharges),

                        CurrencyCode = q.Key.CurrencyCode,
                        CurrencyRate = q.Key.CurrencyRate,
                    })
                    .OrderBy(q => q.TreatyCode)
                    .ToList();

                var soaDataCompiledSummaries = SoaDataCompiledSummaryBos
                    .Where(q => q.ReportingType == SoaDataCompiledSummaryBo.ReportingTypeIFRS4)
                    .GroupBy(q => new { q.TreatyCode, q.RiskQuarter, q.CurrencyCode, q.CurrencyRate })
                    .Select(q => new SoaDataCompiledSummaryBo
                    {
                        TreatyCode = q.Key.TreatyCode,
                        RiskQuarter = q.Key.RiskQuarter,

                        TotalPremium = q.Sum(d => d.TotalPremium),

                        CurrencyCode = q.Key.CurrencyCode,
                        CurrencyRate = q.Key.CurrencyRate,
                    })
                    .OrderBy(q => q.TreatyCode)
                    .ToList();

                // Risk Qtr shall be populate based on risk period in RI Data but when there is only Claim Data in SOA, Risk Qtr will follow SOA Qtr
                if (GetRiDataBatchId() != 0)
                {
                    var riSummaryGroups = QueryRiSummaryIfrs17GroupBy(true);
                    var riSummaryEndDateGroups = QueryRiSummaryIfrs17GroupByEndDate(true);

                    var claimDataGroups = QueryClaimDataGroupBy(db, false, true);

                    foreach (var riSummaryGroup in riSummaryGroups)
                    {
                        var compiledSummary = new SoaDataCompiledSummaryBo
                        {
                            SoaDataBatchId = SoaDataBatchBo.Id,
                            ReportingType = SoaDataCompiledSummaryBo.ReportingTypeIFRS17,

                            InvoiceDate = DateTime.Now,
                            StatementReceivedDate = SoaDataBatchBo.StatementReceivedAt,

                            TreatyCode = riSummaryGroup.TreatyCode,
                            RiskQuarter = riSummaryGroup.RiskQuarter,
                            SoaQuarter = FormatQuarter(SoaDataBatchBo.Quarter),
                            BusinessCode = businessOriginCode,
                            Frequency = riSummaryGroup.Frequency,
                            ContractCode = riSummaryGroup.ContractCode,
                            AnnualCohort = riSummaryGroup.AnnualCohort,

                            CurrencyCode = riSummaryGroup.CurrencyCode,
                            CurrencyRate = riSummaryGroup.CurrencyRate,

                            NbPremium = Util.RoundNullableValue(riSummaryGroup.NbPremium.GetValueOrDefault(), 2),
                            RnPremium = Util.RoundNullableValue(riSummaryGroup.RnPremium.GetValueOrDefault(), 2),
                            AltPremium = Util.RoundNullableValue(riSummaryGroup.AltPremium.GetValueOrDefault(), 2),

                            NbDiscount = Util.RoundNullableValue(riSummaryGroup.NbDiscount.GetValueOrDefault(), 2),
                            RnDiscount = Util.RoundNullableValue(riSummaryGroup.RnDiscount.GetValueOrDefault(), 2),
                            AltDiscount = Util.RoundNullableValue(riSummaryGroup.AltDiscount.GetValueOrDefault(), 2),

                            DTH = Util.RoundNullableValue(riSummaryGroup.DTH.GetValueOrDefault(), 2),
                            TPA = Util.RoundNullableValue(riSummaryGroup.TPA.GetValueOrDefault(), 2),
                            TPS = Util.RoundNullableValue(riSummaryGroup.TPS.GetValueOrDefault(), 2),
                            PPD = Util.RoundNullableValue(riSummaryGroup.PPD.GetValueOrDefault(), 2),
                            CCA = Util.RoundNullableValue(riSummaryGroup.CCA.GetValueOrDefault(), 2),
                            CCS = Util.RoundNullableValue(riSummaryGroup.CCS.GetValueOrDefault(), 2),
                            PA = Util.RoundNullableValue(riSummaryGroup.PA.GetValueOrDefault(), 2),
                            HS = Util.RoundNullableValue(riSummaryGroup.HS.GetValueOrDefault(), 2),
                            TPD = Util.RoundNullableValue(riSummaryGroup.TPD.GetValueOrDefault(), 2),
                            CI = Util.RoundNullableValue(riSummaryGroup.CI.GetValueOrDefault(), 2),

                            NoClaimBonus = Util.RoundNullableValue(riSummaryGroup.NoClaimBonus.GetValueOrDefault(), 2),
                            SurrenderValue = Util.RoundNullableValue(riSummaryGroup.SurrenderValue.GetValueOrDefault(), 2),
                            DatabaseCommission = Util.RoundNullableValue(riSummaryGroup.DatabaseCommission.GetValueOrDefault(), 2),
                            BrokerageFee = Util.RoundNullableValue(riSummaryGroup.BrokerageFee.GetValueOrDefault(), 2),
                            ServiceFee = Util.RoundNullableValue(riSummaryGroup.ServiceFee.GetValueOrDefault(), 2),
                        };
                        compiledSummary.GetSstPayable();
                        compiledSummary.GetTotalAmount();
                        compiledSummary.GetTotalPremium();

                        // NB/RN/ALT SAR shall populate if it is monthly mode
                        // other than monthly mode, will take sum AAR
                        if (compiledSummary.Frequency == PickListDetailBo.PremiumFrequencyCodeMonthly)
                        {
                            if (!riSummaryEndDateGroups.IsNullOrEmpty())
                            {
                                var riSummaryEndDateGroup = riSummaryEndDateGroups?.Where(q => q.TreatyCode == riSummaryGroup.TreatyCode && q.RiskQuarter == riSummaryGroup.RiskQuarter && q.CurrencyCode == riSummaryGroup.CurrencyCode
                                        && q.CurrencyRate == riSummaryGroup.CurrencyRate && q.Frequency == riSummaryGroup.Frequency && q.ContractCode == riSummaryGroup.ContractCode && q.AnnualCohort == riSummaryGroup.AnnualCohort).FirstOrDefault();
                                if (riSummaryEndDateGroup != null)
                                {
                                    compiledSummary.NbCession = riSummaryGroup.NbCession.GetValueOrDefault();
                                    compiledSummary.RnCession = riSummaryGroup.RnCession.GetValueOrDefault();
                                    compiledSummary.AltCession = riSummaryGroup.AltCession.GetValueOrDefault();

                                    compiledSummary.NbSar = Util.RoundNullableValue(riSummaryEndDateGroup.NbSar.GetValueOrDefault(), 2);
                                    compiledSummary.RnSar = Util.RoundNullableValue(riSummaryEndDateGroup.RnSar.GetValueOrDefault(), 2);
                                    compiledSummary.AltSar = Util.RoundNullableValue(riSummaryEndDateGroup.AltSar.GetValueOrDefault(), 2);
                                }
                            }
                        }
                        else
                        {
                            if (riSummaryGroup != null)
                            {
                                compiledSummary.NbCession = riSummaryGroup.NbCession.GetValueOrDefault();
                                compiledSummary.RnCession = riSummaryGroup.RnCession.GetValueOrDefault();
                                compiledSummary.AltCession = riSummaryGroup.AltCession.GetValueOrDefault();

                                compiledSummary.NbSar = Util.RoundNullableValue(riSummaryGroup.NbSar.GetValueOrDefault(), 2);
                                compiledSummary.RnSar = Util.RoundNullableValue(riSummaryGroup.RnSar.GetValueOrDefault(), 2);
                                compiledSummary.AltSar = Util.RoundNullableValue(riSummaryGroup.AltSar.GetValueOrDefault(), 2);
                            }
                        }

                        var soaDataGroup = soaDataGroups?.Where(q => q.TreatyCode == riSummaryGroup.TreatyCode && q.RiskQuarter == riSummaryGroup.RiskQuarter && q.CurrencyCode == riSummaryGroup.CurrencyCode && q.CurrencyRate == riSummaryGroup.CurrencyRate).FirstOrDefault();
                        if (soaDataGroup != null)
                        {
                            compiledSummary.TotalDiscount = Util.RoundNullableValue(soaDataGroup.TotalDiscount.GetValueOrDefault(), 2);
                            compiledSummary.Levy = Util.RoundNullableValue(soaDataGroup.Levy.GetValueOrDefault(), 2);
                            compiledSummary.Gst = Util.RoundNullableValue(soaDataGroup.Gst.GetValueOrDefault(), 2);
                            compiledSummary.ProfitComm = Util.RoundNullableValue(soaDataGroup.ProfitComm.GetValueOrDefault(), 2);
                            compiledSummary.ModcoReserveIncome = Util.RoundNullableValue(soaDataGroup.ModcoReserveIncome.GetValueOrDefault(), 2);
                            compiledSummary.RiDeposit = Util.RoundNullableValue(soaDataGroup.RiDeposit.GetValueOrDefault(), 2);
                            compiledSummary.AdministrationContribution = Util.RoundNullableValue(soaDataGroup.AdministrationContribution.GetValueOrDefault(), 2);
                            compiledSummary.ShareOfRiCommissionFromCompulsoryCession = Util.RoundNullableValue(soaDataGroup.ShareOfRiCommissionFromCompulsoryCession.GetValueOrDefault(), 2);
                            compiledSummary.RecaptureFee = Util.RoundNullableValue(soaDataGroup.RecaptureFee.GetValueOrDefault(), 2);
                            compiledSummary.CreditCardCharges = Util.RoundNullableValue(soaDataGroup.CreditCardCharges.GetValueOrDefault(), 2);
                            compiledSummary.CurrencyCode = soaDataGroup.CurrencyCode;
                            compiledSummary.CurrencyRate = soaDataGroup.CurrencyRate;

                            soaDataRiskPremium = soaDataGroup.RiskPremium.GetValueOrDefault();
                        }                        

                        if (compiledSummary.RiskQuarter == compiledSummary.SoaQuarter)
                        {
                            var claimDataGroup = claimDataGroups?.Where(q => q.TreatyCode == riSummaryGroup.TreatyCode && q.SoaQuarter == SoaDataBatchBo.Quarter && q.ContractCode == compiledSummary.ContractCode && q.AnnualCohort == compiledSummary.AnnualCohort).FirstOrDefault();
                            if (claimDataGroup != null)
                            {
                                if (compiledSummary.CurrencyCode == PickListDetailBo.CurrencyCodeMyr)
                                    compiledSummary.Claim = Util.RoundNullableValue(claimDataGroup.ClaimRecoveryAmt.GetValueOrDefault(), 2);
                                else
                                    compiledSummary.Claim = Util.RoundNullableValue(claimDataGroup.ForeignClaimRecoveryAmt.GetValueOrDefault(), 2);
                            }
                        }

                        switch (businessOriginCode)
                        {
                            case PickListDetailBo.BusinessOriginCodeWithinMalaysia:
                                compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeWM;
                                break;
                            case PickListDetailBo.BusinessOriginCodeOutsideMalaysia:
                                compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeOM;
                                break;
                            case PickListDetailBo.BusinessOriginCodeServiceFee:
                                compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeSFWM;
                                break;
                        }

                        compiledSummary.GetNetTotalAmount();
                        if (compiledSummary.RiskQuarter != compiledSummary.SoaQuarter)
                        {
                            switch (businessOriginCode)
                            {
                                case PickListDetailBo.BusinessOriginCodeWithinMalaysia:
                                    if (compiledSummary.NetTotalAmount < 0)
                                        compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeCNWM;
                                    else
                                        compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeDNWM;
                                    break;
                                case PickListDetailBo.BusinessOriginCodeOutsideMalaysia:
                                    if (compiledSummary.NetTotalAmount < 0)
                                        compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeCNOM;
                                    else
                                        compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeDNOM;
                                    break;
                                case PickListDetailBo.BusinessOriginCodeServiceFee:
                                    if (compiledSummary.TotalAmount < 0)
                                        compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeCNSFWM;
                                    else
                                        compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeDNSFWM;
                                    break;
                            }
                        }
                                                
                        if (compiledSummary.InvoiceType != SoaDataCompiledSummaryBo.InvoiceTypeWM && compiledSummary.InvoiceType != SoaDataCompiledSummaryBo.InvoiceTypeOM)
                            compiledSummary.Amount1 = compiledSummary.NetTotalAmount;

                        // Risk Premium update by multiply with premium ratio
                        var soaDataCompiledSummaryBo = soaDataCompiledSummaries.Where(q => q.TreatyCode == compiledSummary.TreatyCode && q.RiskQuarter == compiledSummary.RiskQuarter && q.CurrencyCode == compiledSummary.CurrencyCode && q.CurrencyRate == compiledSummary.CurrencyRate).FirstOrDefault();
                        if (soaDataCompiledSummaryBo != null)
                            compiledSummary.GetRiskPremium(soaDataCompiledSummaryBo.TotalPremium.GetValueOrDefault(), soaDataRiskPremium.GetValueOrDefault());

                        SoaDataCompiledSummaryBos.Add(compiledSummary);

                        //Added update for UpdatedAt for fail checking
                        var boToUpdate = SoaDataBatchBo;
                        SoaDataBatchService.Update(ref boToUpdate);
                    }

                    if (!SoaDataCompiledSummaryBos.IsNullOrEmpty())
                    {
                        // Claim amount in IFRS17 compiled summary also will be updated under invoice Risk Qtr=SOA Qtr. Claim for annual cohort 2010 is also under SOA Qtr 16Q1 so the amount shall be updated under WM invoice 16Q1
                        // Hence, the claim amount for annual cohort 2010 shall be updated under new row with Risk Qtr 16Q1 with its respective annual cohort & contract code
                        var existAnnualCohorts = SoaDataCompiledSummaryBos.Where(o => o.ReportingType == SoaDataCompiledSummaryBo.ReportingTypeIFRS17 && o.RiskQuarter == FormatQuarter(SoaDataBatchBo.Quarter) && o.SoaQuarter == FormatQuarter(SoaDataBatchBo.Quarter)).Select(o => o.AnnualCohort).ToArray();
                        if (!claimDataGroups.IsNullOrEmpty())
                        {
                            foreach (var claimDataGroup in claimDataGroups.Where(q => !existAnnualCohorts.Contains(q.AnnualCohort)))
                            {
                                var compiledSummary = new SoaDataCompiledSummaryBo
                                {
                                    SoaDataBatchId = SoaDataBatchBo.Id,
                                    ReportingType = SoaDataCompiledSummaryBo.ReportingTypeIFRS17,

                                    InvoiceDate = DateTime.Now,
                                    StatementReceivedDate = SoaDataBatchBo.StatementReceivedAt,

                                    TreatyCode = claimDataGroup.TreatyCode,
                                    RiskQuarter = FormatQuarter(SoaDataBatchBo.Quarter),
                                    SoaQuarter = FormatQuarter(SoaDataBatchBo.Quarter),
                                    BusinessCode = businessOriginCode,
                                    ContractCode = claimDataGroup.ContractCode,
                                    AnnualCohort = claimDataGroup.AnnualCohort,
                                };

                                var soaDataCS = SoaDataCompiledSummaryBos.Where(x => x.ReportingType == SoaDataCompiledSummaryBo.ReportingTypeIFRS17 && x.TreatyCode == claimDataGroup.TreatyCode && x.SoaQuarter == FormatQuarter(SoaDataBatchBo.Quarter) && x.AnnualCohort == claimDataGroup.AnnualCohort && x.ContractCode == claimDataGroup.ContractCode).FirstOrDefault();
                                if (soaDataCS != null)
                                {
                                    compiledSummary.CurrencyCode = soaDataCS.CurrencyCode;
                                    compiledSummary.CurrencyRate = soaDataCS.CurrencyRate;
                                    compiledSummary.Frequency = soaDataCS.Frequency;
                                }
                                else
                                {
                                    compiledSummary.CurrencyCode = SoaDataBatchBo.CurrencyCodePickListDetailBo?.Code;
                                    compiledSummary.CurrencyRate = SoaDataBatchBo.CurrencyRate;
                                    compiledSummary.Frequency = SoaDataCompiledSummaryBos?.Where(x => x.ReportingType == SoaDataCompiledSummaryBo.ReportingTypeIFRS4 && x.TreatyCode == claimDataGroup.TreatyCode && x.SoaQuarter == FormatQuarter(SoaDataBatchBo.Quarter) && x.RiskQuarter == FormatQuarter(SoaDataBatchBo.Quarter)).Select(q => q.Frequency).FirstOrDefault();
                                }

                                if (compiledSummary.CurrencyCode == PickListDetailBo.CurrencyCodeMyr)
                                    compiledSummary.Claim = Util.RoundNullableValue(claimDataGroup.ClaimRecoveryAmt.GetValueOrDefault(), 2);
                                else
                                    compiledSummary.Claim = Util.RoundNullableValue(claimDataGroup.ForeignClaimRecoveryAmt.GetValueOrDefault(), 2);

                                switch (businessOriginCode)
                                {
                                    case PickListDetailBo.BusinessOriginCodeWithinMalaysia:
                                        compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeWM;
                                        break;
                                    case PickListDetailBo.BusinessOriginCodeOutsideMalaysia:
                                        compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeOM;
                                        break;
                                    case PickListDetailBo.BusinessOriginCodeServiceFee:
                                        compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeSFWM;
                                        break;
                                }

                                compiledSummary.GetNetTotalAmount();
                                if (compiledSummary.InvoiceType != SoaDataCompiledSummaryBo.InvoiceTypeWM && compiledSummary.InvoiceType != SoaDataCompiledSummaryBo.InvoiceTypeOM)
                                    compiledSummary.Amount1 = compiledSummary.NetTotalAmount;

                                SoaDataCompiledSummaryBos.Add(compiledSummary);

                                //Added update for UpdatedAt for fail checking
                                var boToUpdate = SoaDataBatchBo;
                                SoaDataBatchService.Update(ref boToUpdate);
                            }
                        }
                        
                    }
                    
                }
                else
                {
                    var riSummaryGroups = QueryRiSummaryIfrs17GroupBy();
                    var riSummaryEndDateGroups = QueryRiSummaryIfrs17GroupByEndDate();

                    var claimDataGroups = QueryClaimDataGroupBy(db);

                    foreach (var soaDataGroup in soaDataGroups)
                    {
                        var compiledSummary = new SoaDataCompiledSummaryBo
                        {
                            SoaDataBatchId = SoaDataBatchBo.Id,
                            ReportingType = SoaDataCompiledSummaryBo.ReportingTypeIFRS17,

                            InvoiceDate = DateTime.Now,
                            StatementReceivedDate = SoaDataBatchBo.StatementReceivedAt,

                            TreatyCode = soaDataGroup.TreatyCode,
                            RiskQuarter = soaDataGroup.RiskQuarter,
                            SoaQuarter = FormatQuarter(SoaDataBatchBo.Quarter),
                            BusinessCode = businessOriginCode,

                            TotalDiscount = Util.RoundNullableValue(soaDataGroup.TotalDiscount.GetValueOrDefault(), 2),
                            Levy = Util.RoundNullableValue(soaDataGroup.Levy.GetValueOrDefault(), 2),
                            Gst = Util.RoundNullableValue(soaDataGroup.Gst.GetValueOrDefault(), 2),
                            ProfitComm = Util.RoundNullableValue(soaDataGroup.ProfitComm.GetValueOrDefault(), 2),
                            ModcoReserveIncome = Util.RoundNullableValue(soaDataGroup.ModcoReserveIncome.GetValueOrDefault(), 2),
                            RiDeposit = Util.RoundNullableValue(soaDataGroup.RiDeposit.GetValueOrDefault(), 2),
                            AdministrationContribution = Util.RoundNullableValue(soaDataGroup.AdministrationContribution.GetValueOrDefault(), 2),
                            ShareOfRiCommissionFromCompulsoryCession = Util.RoundNullableValue(soaDataGroup.ShareOfRiCommissionFromCompulsoryCession.GetValueOrDefault(), 2),
                            RecaptureFee = Util.RoundNullableValue(soaDataGroup.RecaptureFee.GetValueOrDefault(), 2),
                            CreditCardCharges = Util.RoundNullableValue(soaDataGroup.CreditCardCharges.GetValueOrDefault(), 2),

                            CurrencyCode = soaDataGroup.CurrencyCode,
                            CurrencyRate = soaDataGroup.CurrencyRate.GetValueOrDefault(),
                        };

                        var riSummaryGroup = riSummaryGroups?.Where(q => q.TreatyCode == soaDataGroup.TreatyCode && q.RiskQuarter == compiledSummary.RiskQuarter && q.CurrencyCode == compiledSummary.CurrencyCode && q.CurrencyRate == compiledSummary.CurrencyRate).FirstOrDefault();
                        if (riSummaryGroup != null)
                        {
                            compiledSummary.NbPremium = Util.RoundNullableValue(riSummaryGroup.NbPremium.GetValueOrDefault(), 2);
                            compiledSummary.RnPremium = Util.RoundNullableValue(riSummaryGroup.RnPremium.GetValueOrDefault(), 2);
                            compiledSummary.AltPremium = Util.RoundNullableValue(riSummaryGroup.AltPremium.GetValueOrDefault(), 2);

                            compiledSummary.NbDiscount = Util.RoundNullableValue(riSummaryGroup.NbDiscount.GetValueOrDefault(), 2);
                            compiledSummary.RnDiscount = Util.RoundNullableValue(riSummaryGroup.RnDiscount.GetValueOrDefault(), 2);
                            compiledSummary.AltDiscount = Util.RoundNullableValue(riSummaryGroup.AltDiscount.GetValueOrDefault(), 2);

                            compiledSummary.DTH = Util.RoundNullableValue(riSummaryGroup.DTH.GetValueOrDefault(), 2);
                            compiledSummary.TPA = Util.RoundNullableValue(riSummaryGroup.TPA.GetValueOrDefault(), 2);
                            compiledSummary.TPS = Util.RoundNullableValue(riSummaryGroup.TPS.GetValueOrDefault(), 2);
                            compiledSummary.PPD = Util.RoundNullableValue(riSummaryGroup.PPD.GetValueOrDefault(), 2);
                            compiledSummary.CCA = Util.RoundNullableValue(riSummaryGroup.CCA.GetValueOrDefault(), 2);
                            compiledSummary.CCS = Util.RoundNullableValue(riSummaryGroup.CCS.GetValueOrDefault(), 2);
                            compiledSummary.PA = Util.RoundNullableValue(riSummaryGroup.PA.GetValueOrDefault(), 2);
                            compiledSummary.HS = Util.RoundNullableValue(riSummaryGroup.HS.GetValueOrDefault(), 2);
                            compiledSummary.TPD = Util.RoundNullableValue(riSummaryGroup.TPD.GetValueOrDefault(), 2);
                            compiledSummary.CI = Util.RoundNullableValue(riSummaryGroup.CI.GetValueOrDefault(), 2);

                            compiledSummary.NoClaimBonus = Util.RoundNullableValue(riSummaryGroup.NoClaimBonus.GetValueOrDefault(), 2);
                            compiledSummary.SurrenderValue = Util.RoundNullableValue(riSummaryGroup.SurrenderValue.GetValueOrDefault(), 2);
                            compiledSummary.DatabaseCommission = Util.RoundNullableValue(riSummaryGroup.DatabaseCommission.GetValueOrDefault(), 2);
                            compiledSummary.BrokerageFee = Util.RoundNullableValue(riSummaryGroup.BrokerageFee.GetValueOrDefault(), 2);
                            compiledSummary.ServiceFee = Util.RoundNullableValue(riSummaryGroup.ServiceFee.GetValueOrDefault(), 2);

                            compiledSummary.Frequency = riSummaryGroup.Frequency;
                            compiledSummary.ContractCode = riSummaryGroup.ContractCode;
                            compiledSummary.AnnualCohort = riSummaryGroup.AnnualCohort;
                        }
                        compiledSummary.GetSstPayable();
                        compiledSummary.GetTotalAmount();
                        compiledSummary.GetTotalPremium();

                        // NB/RN/ALT SAR shall populate if it is monthly mode
                        // other than monthly mode, will take sum AAR
                        if (compiledSummary.Frequency == PickListDetailBo.PremiumFrequencyCodeMonthly)
                        {
                            if (!riSummaryEndDateGroups.IsNullOrEmpty())
                            {
                                var riSummaryEndDateGroup = riSummaryEndDateGroups?.Where(q => q.TreatyCode == soaDataGroup.TreatyCode && q.RiskQuarter == compiledSummary.RiskQuarter && q.CurrencyCode == compiledSummary.CurrencyCode && q.CurrencyRate == riSummaryGroup.CurrencyRate && q.Frequency == compiledSummary.Frequency).FirstOrDefault();
                                if (riSummaryEndDateGroup != null)
                                {
                                    compiledSummary.NbCession = riSummaryEndDateGroup.NbCession.GetValueOrDefault();
                                    compiledSummary.RnCession = riSummaryEndDateGroup.RnCession.GetValueOrDefault();
                                    compiledSummary.AltCession = riSummaryEndDateGroup.AltCession.GetValueOrDefault();

                                    compiledSummary.NbSar = Util.RoundNullableValue(riSummaryEndDateGroup.NbSar.GetValueOrDefault(), 2);
                                    compiledSummary.RnSar = Util.RoundNullableValue(riSummaryEndDateGroup.RnSar.GetValueOrDefault(), 2);
                                    compiledSummary.AltSar = Util.RoundNullableValue(riSummaryEndDateGroup.AltSar.GetValueOrDefault(), 2);
                                }
                            }
                        }
                        else
                        {
                            if (riSummaryGroup != null)
                            {
                                compiledSummary.NbCession = riSummaryGroup.NbCession.GetValueOrDefault();
                                compiledSummary.RnCession = riSummaryGroup.RnCession.GetValueOrDefault();
                                compiledSummary.AltCession = riSummaryGroup.AltCession.GetValueOrDefault();

                                compiledSummary.NbSar = Util.RoundNullableValue(riSummaryGroup.NbSar.GetValueOrDefault(), 2);
                                compiledSummary.RnSar = Util.RoundNullableValue(riSummaryGroup.RnSar.GetValueOrDefault(), 2);
                                compiledSummary.AltSar = Util.RoundNullableValue(riSummaryGroup.AltSar.GetValueOrDefault(), 2);
                            }
                        }

                        if (compiledSummary.RiskQuarter == compiledSummary.SoaQuarter)
                        {
                            var claimDataGroup = claimDataGroups?.Where(q => q.TreatyCode == soaDataGroup.TreatyCode && q.SoaQuarter == SoaDataBatchBo.Quarter).FirstOrDefault();
                            if (claimDataGroup != null)
                            {
                                if (compiledSummary.CurrencyCode == PickListDetailBo.CurrencyCodeMyr)
                                    compiledSummary.Claim = Util.RoundNullableValue(claimDataGroup.ClaimRecoveryAmt.GetValueOrDefault(), 2);
                                else
                                    compiledSummary.Claim = Util.RoundNullableValue(claimDataGroup.ForeignClaimRecoveryAmt.GetValueOrDefault(), 2);
                            }
                        }

                        switch (businessOriginCode)
                        {
                            case PickListDetailBo.BusinessOriginCodeWithinMalaysia:
                                compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeWM;
                                break;
                            case PickListDetailBo.BusinessOriginCodeOutsideMalaysia:
                                compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeOM;
                                break;
                            case PickListDetailBo.BusinessOriginCodeServiceFee:
                                compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeSFWM;
                                break;
                        }

                        compiledSummary.GetNetTotalAmount();
                        if (compiledSummary.RiskQuarter != compiledSummary.SoaQuarter)
                        {
                            switch (businessOriginCode)
                            {
                                case PickListDetailBo.BusinessOriginCodeWithinMalaysia:
                                    if (compiledSummary.NetTotalAmount < 0)
                                        compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeCNWM;
                                    else
                                        compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeDNWM;
                                    break;
                                case PickListDetailBo.BusinessOriginCodeOutsideMalaysia:
                                    if (compiledSummary.NetTotalAmount < 0)
                                        compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeCNOM;
                                    else
                                        compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeDNOM;
                                    break;
                                case PickListDetailBo.BusinessOriginCodeServiceFee:
                                    if (compiledSummary.TotalAmount < 0)
                                        compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeCNSFWM;
                                    else
                                        compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeDNSFWM;
                                    break;
                            }
                        }
                                                
                        if (compiledSummary.InvoiceType != SoaDataCompiledSummaryBo.InvoiceTypeWM && compiledSummary.InvoiceType != SoaDataCompiledSummaryBo.InvoiceTypeOM)
                            compiledSummary.Amount1 = compiledSummary.NetTotalAmount;

                        // Risk Premium update by multiply with premium ratio
                        var soaDataCompiledSummaryBo = soaDataCompiledSummaries.Where(q => q.TreatyCode == compiledSummary.TreatyCode && q.RiskQuarter == compiledSummary.RiskQuarter && q.CurrencyCode == compiledSummary.CurrencyCode && q.CurrencyRate == compiledSummary.CurrencyRate).FirstOrDefault();
                        if (soaDataCompiledSummaryBo != null)
                            compiledSummary.GetRiskPremium(soaDataCompiledSummaryBo.TotalPremium.GetValueOrDefault(), soaDataGroup.RiskPremium.GetValueOrDefault());

                        SoaDataCompiledSummaryBos.Add(compiledSummary);

                        //Added update for UpdatedAt for fail checking
                        var boToUpdate = SoaDataBatchBo;
                        SoaDataBatchService.Update(ref boToUpdate);
                    }
                }
            }
        }

        public void CreateRiSummaryValidationByCellNameIfrs17()
        {
            bool updateClaim = true;
            if (SoaDataRiDataSummaryBos.IsNullOrEmpty())
                SoaDataRiDataSummaryBos = new List<SoaDataRiDataSummaryBo> { };
            using (var db = new AppDbContext(false))
            {
                var riDataGroups = QueryRiDataGroupBy(db, true, true);
                var claimDataGroups = QueryRiDataSummaryIfrs17ClaimDataGroupBy(db);

                var soaDataGroups = db.SoaData
                    .Where(q => q.SoaDataBatchId == SoaDataBatchBo.Id)
                    .GroupBy(q => new { q.BusinessCode, q.TreatyCode, q.RiskMonth, q.RiskQuarter, q.CurrencyCode, q.CurrencyRate })
                    .Select(q => new SoaDataRiDataSummaryBo
                    {
                        BusinessCode = q.Key.BusinessCode,
                        TreatyCode = q.Key.TreatyCode,
                        RiskMonth = q.Key.RiskMonth,
                        RiskQuarter = q.Key.RiskQuarter,

                        CurrencyCode = q.Key.CurrencyCode,
                        CurrencyRate = q.Key.CurrencyRate,

                        TotalDiscount = q.Sum(d => d.TotalDiscount),
                        GrossPremium = q.Sum(d => d.GrossPremium),
                        NetTotalAmount = q.Sum(d => d.NetTotalAmount),

                        NbPremium = q.Sum(d => d.NbPremium),
                        RnPremium = q.Sum(d => d.RnPremium),
                        AltPremium = q.Sum(d => d.AltPremium),

                        NoClaimBonus = q.Sum(d => d.NoClaimBonus),
                        Claim = q.Sum(d => d.Claim),
                        SurrenderValue = q.Sum(d => d.SurrenderValue),
                        DatabaseCommission = q.Sum(d => d.DatabaseCommission),
                        BrokerageFee = q.Sum(d => d.BrokerageFee),
                        Gst = q.Sum(d => d.Gst),
                    })
                    .OrderBy(q => q.TreatyCode)
                    .ToList();

                if (!riDataGroups.IsNullOrEmpty())
                {
                    var nbs = QueryRiDataSumByTransactionType(db, PickListDetailBo.TransactionTypeCodeNewBusiness, true, true);
                    var rns = QueryRiDataSumByTransactionType(db, PickListDetailBo.TransactionTypeCodeRenewal, true, true);
                    var als = QueryRiDataSumByTransactionType(db, PickListDetailBo.TransactionTypeCodeAlteration, true, true);

                    var nbCountsMonthly = QueryCountRiDataPostValidationTransactionType(db, PickListDetailBo.TransactionTypeCodeNewBusiness, true, true, true);
                    var nbCounts = QueryCountRiDataPostValidationTransactionType(db, PickListDetailBo.TransactionTypeCodeNewBusiness, false, true, true);

                    var rnCountsMonthly = QueryCountRiDataPostValidationTransactionType(db, PickListDetailBo.TransactionTypeCodeRenewal, true, true, true);
                    var rnCounts = QueryCountRiDataPostValidationTransactionType(db, PickListDetailBo.TransactionTypeCodeRenewal, false, true, true);

                    var alCountsMonthly = QueryCountRiDataPostValidationTransactionType(db, PickListDetailBo.TransactionTypeCodeAlteration, true, true, true);
                    var alCounts = QueryCountRiDataPostValidationTransactionType(db, PickListDetailBo.TransactionTypeCodeAlteration, false, true, true);

                    var dths = QueryRiDataPostValidationBenefitCode(db, BenefitBo.CodeDTH, true, true);
                    var tpas = QueryRiDataPostValidationBenefitCode(db, BenefitBo.CodeTPA, true, true);
                    var tpss = QueryRiDataPostValidationBenefitCode(db, BenefitBo.CodeTPS, true, true);
                    var ppds = QueryRiDataPostValidationBenefitCode(db, BenefitBo.CodePPD, true, true);
                    var ccas = QueryRiDataPostValidationBenefitCode(db, BenefitBo.CodeCCA, true, true);
                    var ccss = QueryRiDataPostValidationBenefitCode(db, BenefitBo.CodeCCS, true, true);
                    var pas = QueryRiDataPostValidationBenefitCode(db, BenefitBo.CodePA, true, true);
                    var hss = QueryRiDataPostValidationBenefitCode(db, BenefitBo.CodeHS, true, true);
                    var tpds = QueryRiDataPostValidationBenefitCode(db, BenefitBo.CodeTPD, true, true);
                    var cis = QueryRiDataPostValidationBenefitCode(db, BenefitBo.CodeCI, true, true);

                    foreach (var riDataGroup in riDataGroups)
                    {
                        var businessOriginCode = CacheService.BusinessOrigins.Where(q => q.Id == TreatyBo?.BusinessOriginPickListDetailId).Select(q => q.Code).FirstOrDefault();
                        var treatyCode = riDataGroup.TreatyCode;
                        var riskPeriodMonth = riDataGroup.RiskPeriodMonth;
                        var riskPeriodYear = riDataGroup.RiskPeriodYear;
                        var currencyCode = riDataGroup.CurrencyCode;
                        var summary = new SoaDataRiDataSummaryBo
                        {
                            Type = SoaDataRiDataSummaryBo.TypeRiDataSummaryCNIfrs17,
                            BusinessCode = businessOriginCode,
                            TreatyCode = riDataGroup.TreatyCode,
                            RiskMonth = riDataGroup.RiskPeriodMonth.GetValueOrDefault(),
                            TotalDiscount = riDataGroup.TransactionDiscount.GetValueOrDefault(),
                            RiskQuarter = GetQuarterInfo(riDataGroup.RiskPeriodMonth, riDataGroup.RiskPeriodYear),
                            CurrencyCode = riDataGroup.CurrencyCode,
                            CurrencyRate = SoaDataBatchBo.CurrencyRate,
                            Frequency = riDataGroup.PremiumFrequencyCode,
                            ContractCode = riDataGroup.ContractCode,
                            AnnualCohort = riDataGroup.AnnualCohort,
                            Mfrs17CellName = riDataGroup.Mfrs17CellName,
                            SoaQuarter = FormatQuarter(SoaDataBatchBo.Quarter),
                        };
                        if (string.IsNullOrEmpty(summary.CurrencyCode)) summary.CurrencyCode = SoaDataBatchBo.CurrencyCodePickListDetailBo?.Code;

                        var nb = nbs?.Where(q => q.TreatyCode == treatyCode && q.RiskPeriodMonth == riskPeriodMonth && q.RiskPeriodYear == riskPeriodYear && q.CurrencyCode == currencyCode && q.PremiumFrequencyCode == riDataGroup.PremiumFrequencyCode && q.ContractCode == riDataGroup.ContractCode && q.AnnualCohort == riDataGroup.AnnualCohort && q.Mfrs17CellName == riDataGroup.Mfrs17CellName).FirstOrDefault();
                        if (nb != null)
                        {
                            summary.NbPremium = nb.TransactionPremium.GetValueOrDefault();
                            summary.NbDiscount = nb.TransactionDiscount.GetValueOrDefault();
                            summary.NbSar = nb.Aar;
                        }

                        var rn = rns?.Where(q => q.TreatyCode == treatyCode && q.RiskPeriodMonth == riskPeriodMonth && q.RiskPeriodYear == riskPeriodYear && q.CurrencyCode == currencyCode && q.PremiumFrequencyCode == riDataGroup.PremiumFrequencyCode && q.ContractCode == riDataGroup.ContractCode && q.AnnualCohort == riDataGroup.AnnualCohort && q.Mfrs17CellName == riDataGroup.Mfrs17CellName).FirstOrDefault();
                        if (rn != null)
                        {
                            summary.RnPremium = rn.TransactionPremium.GetValueOrDefault();
                            summary.RnDiscount = rn.TransactionDiscount.GetValueOrDefault();
                            summary.RnSar = rn.Aar;
                        }

                        var al = als?.Where(q => q.TreatyCode == treatyCode && q.RiskPeriodMonth == riskPeriodMonth && q.RiskPeriodYear == riskPeriodYear && q.CurrencyCode == currencyCode && q.PremiumFrequencyCode == riDataGroup.PremiumFrequencyCode && q.ContractCode == riDataGroup.ContractCode && q.AnnualCohort == riDataGroup.AnnualCohort && q.Mfrs17CellName == riDataGroup.Mfrs17CellName).FirstOrDefault();
                        if (al != null)
                        {
                            summary.AltPremium = al.TransactionPremium.GetValueOrDefault();
                            summary.AltDiscount = al.TransactionDiscount.GetValueOrDefault();
                            summary.AltSar = al.Aar;
                        }

                        summary.GetGrossPremium();

                        summary.NoClaimBonus = riDataGroup.NoClaimBonus.GetValueOrDefault();
                        summary.DatabaseCommission = riDataGroup.DatabaseCommission.GetValueOrDefault();
                        summary.BrokerageFee = riDataGroup.BrokerageFee.GetValueOrDefault();
                        summary.ServiceFee = riDataGroup.ServiceFee.GetValueOrDefault();
                        summary.Gst = riDataGroup.GstAmount.GetValueOrDefault();
                        summary.SurrenderValue = riDataGroup.SurrenderValue.GetValueOrDefault();

                        if (summary.RiskQuarter == FormatQuarter(SoaDataBatchBo.Quarter))
                        {
                            var claimDataGroup = claimDataGroups?.Where(q => q.TreatyCode == treatyCode && q.SoaQuarter == SoaDataBatchBo.Quarter && q.ContractCode == riDataGroup.ContractCode && q.AnnualCohort == riDataGroup.AnnualCohort).FirstOrDefault();
                            if (claimDataGroup != null && updateClaim)
                            {
                                if (summary.CurrencyCode == PickListDetailBo.CurrencyCodeMyr)
                                    summary.Claim = claimDataGroup.ClaimRecoveryAmt.GetValueOrDefault();
                                else
                                    summary.Claim = claimDataGroup.ForeignClaimRecoveryAmt.GetValueOrDefault();
                                updateClaim = false;
                            }
                        }
                        summary.GetNetTotalAmount();

                        var nbCountMonthly = nbCountsMonthly?.Where(q => q.TreatyCode == treatyCode && q.RiskPeriodMonth == riskPeriodMonth && q.RiskPeriodYear == riskPeriodYear && q.CurrencyCode == currencyCode && q.PremiumFrequencyCode == riDataGroup.PremiumFrequencyCode && q.ContractCode == riDataGroup.ContractCode && q.AnnualCohort == riDataGroup.AnnualCohort && q.Mfrs17CellName == riDataGroup.Mfrs17CellName).FirstOrDefault();
                        var nbCount = nbCounts?.Where(q => q.TreatyCode == treatyCode && q.RiskPeriodMonth == riskPeriodMonth && q.RiskPeriodYear == riskPeriodYear && q.CurrencyCode == currencyCode && q.PremiumFrequencyCode == riDataGroup.PremiumFrequencyCode && q.ContractCode == riDataGroup.ContractCode && q.AnnualCohort == riDataGroup.AnnualCohort && q.Mfrs17CellName == riDataGroup.Mfrs17CellName).FirstOrDefault();
                        int? nbCession = 0;
                        if (summary.Frequency == PickListDetailBo.PremiumFrequencyCodeMonthly)
                        {
                            if (nbCountMonthly != null)
                                nbCession += nbCountMonthly.Total;
                        }
                        else
                        {
                            if (nbCount != null)
                                nbCession += nbCount.Total;
                        }

                        var rnCountMonthly = rnCountsMonthly?.Where(q => q.TreatyCode == treatyCode && q.RiskPeriodMonth == riskPeriodMonth && q.RiskPeriodYear == riskPeriodYear && q.CurrencyCode == currencyCode && q.PremiumFrequencyCode == riDataGroup.PremiumFrequencyCode && q.ContractCode == riDataGroup.ContractCode && q.AnnualCohort == riDataGroup.AnnualCohort && q.Mfrs17CellName == riDataGroup.Mfrs17CellName).FirstOrDefault();
                        var rnCount = rnCounts?.Where(q => q.TreatyCode == treatyCode && q.RiskPeriodMonth == riskPeriodMonth && q.RiskPeriodYear == riskPeriodYear && q.CurrencyCode == currencyCode && q.PremiumFrequencyCode == riDataGroup.PremiumFrequencyCode && q.ContractCode == riDataGroup.ContractCode && q.AnnualCohort == riDataGroup.AnnualCohort && q.Mfrs17CellName == riDataGroup.Mfrs17CellName).FirstOrDefault();
                        int? rnCession = 0;
                        if (summary.Frequency == PickListDetailBo.PremiumFrequencyCodeMonthly)
                        {
                            if (rnCountMonthly != null)
                                rnCession += rnCountMonthly.Total;
                        }
                        else
                        {
                            if (rnCount != null)
                                rnCession += rnCount.Total;
                        }

                        var alCountMonthly = alCountsMonthly?.Where(q => q.TreatyCode == treatyCode && q.RiskPeriodMonth == riskPeriodMonth && q.RiskPeriodYear == riskPeriodYear && q.CurrencyCode == currencyCode && q.PremiumFrequencyCode == riDataGroup.PremiumFrequencyCode && q.ContractCode == riDataGroup.ContractCode && q.AnnualCohort == riDataGroup.AnnualCohort && q.Mfrs17CellName == riDataGroup.Mfrs17CellName).FirstOrDefault();
                        var alCount = alCounts?.Where(q => q.TreatyCode == treatyCode && q.RiskPeriodMonth == riskPeriodMonth && q.RiskPeriodYear == riskPeriodYear && q.CurrencyCode == currencyCode && q.PremiumFrequencyCode == riDataGroup.PremiumFrequencyCode && q.ContractCode == riDataGroup.ContractCode && q.AnnualCohort == riDataGroup.AnnualCohort && q.Mfrs17CellName == riDataGroup.Mfrs17CellName).FirstOrDefault();
                        int? alCession = 0;
                        if (summary.Frequency == PickListDetailBo.PremiumFrequencyCodeMonthly)
                        {
                            if (alCountMonthly != null)
                                alCession += alCountMonthly.Total;
                        }
                        else
                        {
                            if (alCount != null)
                                alCession += alCount.Total;
                        }

                        summary.NbCession = nbCession.GetValueOrDefault();
                        summary.RnCession = rnCession.GetValueOrDefault();
                        summary.AltCession = alCession.GetValueOrDefault();


                        var dth = dths?.Where(q => q.TreatyCode == treatyCode && q.RiskPeriodMonth == riskPeriodMonth && q.RiskPeriodYear == riskPeriodYear && q.CurrencyCode == currencyCode && q.PremiumFrequencyCode == riDataGroup.PremiumFrequencyCode && q.ContractCode == riDataGroup.ContractCode && q.AnnualCohort == riDataGroup.AnnualCohort && q.Mfrs17CellName == riDataGroup.Mfrs17CellName).FirstOrDefault();
                        if (dth != null)
                            summary.DTH = dth.TransactionPremium.GetValueOrDefault();

                        var tpa = tpas?.Where(q => q.TreatyCode == treatyCode && q.RiskPeriodMonth == riskPeriodMonth && q.RiskPeriodYear == riskPeriodYear && q.CurrencyCode == currencyCode && q.PremiumFrequencyCode == riDataGroup.PremiumFrequencyCode && q.ContractCode == riDataGroup.ContractCode && q.AnnualCohort == riDataGroup.AnnualCohort && q.Mfrs17CellName == riDataGroup.Mfrs17CellName).FirstOrDefault();
                        if (tpa != null)
                            summary.TPA = tpa.TransactionPremium.GetValueOrDefault();

                        var tps = tpss?.Where(q => q.TreatyCode == treatyCode && q.RiskPeriodMonth == riskPeriodMonth && q.RiskPeriodYear == riskPeriodYear && q.CurrencyCode == currencyCode && q.PremiumFrequencyCode == riDataGroup.PremiumFrequencyCode && q.ContractCode == riDataGroup.ContractCode && q.AnnualCohort == riDataGroup.AnnualCohort && q.Mfrs17CellName == riDataGroup.Mfrs17CellName).FirstOrDefault();
                        if (tps != null)
                            summary.TPS = tps.TransactionPremium.GetValueOrDefault();

                        var ppd = ppds?.Where(q => q.TreatyCode == treatyCode && q.RiskPeriodMonth == riskPeriodMonth && q.RiskPeriodYear == riskPeriodYear && q.CurrencyCode == currencyCode && q.PremiumFrequencyCode == riDataGroup.PremiumFrequencyCode && q.ContractCode == riDataGroup.ContractCode && q.AnnualCohort == riDataGroup.AnnualCohort && q.Mfrs17CellName == riDataGroup.Mfrs17CellName).FirstOrDefault();
                        if (ppd != null)
                            summary.PPD = ppd.TransactionPremium.GetValueOrDefault();

                        var cca = ccas?.Where(q => q.TreatyCode == treatyCode && q.RiskPeriodMonth == riskPeriodMonth && q.RiskPeriodYear == riskPeriodYear && q.CurrencyCode == currencyCode && q.PremiumFrequencyCode == riDataGroup.PremiumFrequencyCode && q.ContractCode == riDataGroup.ContractCode && q.AnnualCohort == riDataGroup.AnnualCohort && q.Mfrs17CellName == riDataGroup.Mfrs17CellName).FirstOrDefault();
                        if (cca != null)
                            summary.CCA = cca.TransactionPremium.GetValueOrDefault();

                        var ccs = ccss?.Where(q => q.TreatyCode == treatyCode && q.RiskPeriodMonth == riskPeriodMonth && q.RiskPeriodYear == riskPeriodYear && q.CurrencyCode == currencyCode && q.PremiumFrequencyCode == riDataGroup.PremiumFrequencyCode && q.ContractCode == riDataGroup.ContractCode && q.AnnualCohort == riDataGroup.AnnualCohort && q.Mfrs17CellName == riDataGroup.Mfrs17CellName).FirstOrDefault();
                        if (ccs != null)
                            summary.CCS = ccs.TransactionPremium.GetValueOrDefault();

                        var pa = pas?.Where(q => q.TreatyCode == treatyCode && q.RiskPeriodMonth == riskPeriodMonth && q.RiskPeriodYear == riskPeriodYear && q.CurrencyCode == currencyCode && q.PremiumFrequencyCode == riDataGroup.PremiumFrequencyCode && q.ContractCode == riDataGroup.ContractCode && q.AnnualCohort == riDataGroup.AnnualCohort && q.Mfrs17CellName == riDataGroup.Mfrs17CellName).FirstOrDefault();
                        if (pa != null)
                            summary.PA = pa.TransactionPremium.GetValueOrDefault();

                        var hs = hss?.Where(q => q.TreatyCode == treatyCode && q.RiskPeriodMonth == riskPeriodMonth && q.RiskPeriodYear == riskPeriodYear && q.CurrencyCode == currencyCode && q.PremiumFrequencyCode == riDataGroup.PremiumFrequencyCode && q.ContractCode == riDataGroup.ContractCode && q.AnnualCohort == riDataGroup.AnnualCohort && q.Mfrs17CellName == riDataGroup.Mfrs17CellName).FirstOrDefault();
                        if (hs != null)
                            summary.HS = hs.TransactionPremium.GetValueOrDefault();

                        var tpd = tpds?.Where(q => q.TreatyCode == treatyCode && q.RiskPeriodMonth == riskPeriodMonth && q.RiskPeriodYear == riskPeriodYear && q.CurrencyCode == currencyCode && q.PremiumFrequencyCode == riDataGroup.PremiumFrequencyCode && q.ContractCode == riDataGroup.ContractCode && q.AnnualCohort == riDataGroup.AnnualCohort && q.Mfrs17CellName == riDataGroup.Mfrs17CellName).FirstOrDefault();
                        if (tpd != null)
                            summary.TPD = tpd.TransactionPremium.GetValueOrDefault();

                        var ci = cis?.Where(q => q.TreatyCode == treatyCode && q.RiskPeriodMonth == riskPeriodMonth && q.RiskPeriodYear == riskPeriodYear && q.CurrencyCode == currencyCode && q.PremiumFrequencyCode == riDataGroup.PremiumFrequencyCode && q.ContractCode == riDataGroup.ContractCode && q.AnnualCohort == riDataGroup.AnnualCohort && q.Mfrs17CellName == riDataGroup.Mfrs17CellName).FirstOrDefault();
                        if (ci != null)
                            summary.CI = ci.TransactionPremium.GetValueOrDefault();

                        SoaDataRiDataSummaryBos.Add(summary);

                        if (!string.IsNullOrEmpty(summary.CurrencyCode) && summary.CurrencyCode != PickListDetailBo.CurrencyCodeMyr)
                        {
                            var summaryMYR = new SoaDataRiDataSummaryBo
                            {
                                Type = SoaDataRiDataSummaryBo.TypeRiDataSummaryCNIfrs17,
                                BusinessCode = businessOriginCode,
                                TreatyCode = riDataGroup.TreatyCode,
                                RiskMonth = riDataGroup.RiskPeriodMonth.GetValueOrDefault(),
                                TotalDiscount = riDataGroup.TransactionDiscount.GetValueOrDefault(),
                                RiskQuarter = GetQuarterInfo(riDataGroup.RiskPeriodMonth, riDataGroup.RiskPeriodYear),
                                CurrencyCode = PickListDetailBo.CurrencyCodeMyr,
                                CurrencyRate = SoaDataBatchBo.CurrencyRate,
                                Frequency = riDataGroup.PremiumFrequencyCode,
                                ContractCode = riDataGroup.ContractCode,
                                AnnualCohort = riDataGroup.AnnualCohort,
                                Mfrs17CellName = riDataGroup.Mfrs17CellName,
                                SoaQuarter = FormatQuarter(SoaDataBatchBo.Quarter),
                            };
                            summaryMYR.NbCession = summary.NbCession;
                            summaryMYR.RnCession = summary.RnCession;
                            summaryMYR.AltCession = summary.AltCession;

                            summaryMYR.NbPremium = summary.NbPremium * summaryMYR.CurrencyRate;
                            summaryMYR.RnPremium = summary.RnPremium * summaryMYR.CurrencyRate;
                            summaryMYR.AltPremium = summary.AltPremium * summaryMYR.CurrencyRate;

                            summaryMYR.NbDiscount = summary.NbDiscount * summaryMYR.CurrencyRate;
                            summaryMYR.RnDiscount = summary.RnDiscount * summaryMYR.CurrencyRate;
                            summaryMYR.AltDiscount = summary.AltDiscount * summaryMYR.CurrencyRate;

                            summaryMYR.NbSar = summary.NbSar * summaryMYR.CurrencyRate;
                            summaryMYR.RnSar = summary.RnSar * summaryMYR.CurrencyRate;
                            summaryMYR.AltSar = summary.AltSar * summaryMYR.CurrencyRate;

                            summaryMYR.TotalDiscount = summary.TotalDiscount * summaryMYR.CurrencyRate;
                            summaryMYR.NoClaimBonus = summary.NoClaimBonus * summaryMYR.CurrencyRate;
                            summaryMYR.SurrenderValue = summary.SurrenderValue * summaryMYR.CurrencyRate;
                            summaryMYR.DatabaseCommission = summary.DatabaseCommission * summaryMYR.CurrencyRate;
                            summaryMYR.BrokerageFee = summary.BrokerageFee * summaryMYR.CurrencyRate;
                            summaryMYR.ServiceFee = summary.ServiceFee * summaryMYR.CurrencyRate;
                            summaryMYR.Gst = summary.Gst * summaryMYR.CurrencyRate;
                            summaryMYR.Claim = summary.Claim * summaryMYR.CurrencyRate;

                            summaryMYR.DTH = summary.DTH * summaryMYR.CurrencyRate;
                            summaryMYR.TPA = summary.TPA * summaryMYR.CurrencyRate;
                            summaryMYR.TPS = summary.TPS * summaryMYR.CurrencyRate;
                            summaryMYR.PPD = summary.PPD * summaryMYR.CurrencyRate;
                            summaryMYR.CCA = summary.CCA * summaryMYR.CurrencyRate;
                            summaryMYR.CCS = summary.CCS * summaryMYR.CurrencyRate;
                            summaryMYR.PA = summary.PA * summaryMYR.CurrencyRate;
                            summaryMYR.HS = summary.HS * summaryMYR.CurrencyRate;
                            summaryMYR.TPD = summary.TPD * summaryMYR.CurrencyRate;
                            summaryMYR.CI = summary.CI * summaryMYR.CurrencyRate;

                            summaryMYR.GetGrossPremium();
                            summaryMYR.GetNetTotalAmount();

                            SoaDataRiDataSummaryBos.Add(summaryMYR);
                        }

                        //Added update for UpdatedAt for fail checking
                        var boToUpdate = SoaDataBatchBo;
                        SoaDataBatchService.Update(ref boToUpdate);
                    }
                }
                else
                {
                    // Only when Soa Data auto create from Claim Data
                    if (SoaDataBatchBo.IsClaimDataAutoCreate == true && !soaDataGroups.IsNullOrEmpty())
                    {
                        var claimRegisterGroups = QueryClaimDataGroupBy(db);
                        foreach (var soaDataGroup in soaDataGroups)
                        {
                            var summary = new SoaDataRiDataSummaryBo
                            {
                                Type = SoaDataRiDataSummaryBo.TypeRiDataSummaryCNIfrs17,
                                BusinessCode = soaDataGroup.BusinessCode,
                                TreatyCode = soaDataGroup.TreatyCode,
                                RiskQuarter = soaDataGroup.RiskQuarter,
                                SoaQuarter = FormatQuarter(SoaDataBatchBo.Quarter),
                                CurrencyCode = SoaDataBatchBo.CurrencyCodePickListDetailBo?.Code,
                                CurrencyRate = SoaDataBatchBo.CurrencyRate,
                            };

                            if (summary.RiskQuarter == FormatQuarter(SoaDataBatchBo.Quarter))
                            {
                                var claimDataGroup = claimRegisterGroups?.Where(q => q.TreatyCode == summary.TreatyCode && q.SoaQuarter == SoaDataBatchBo.Quarter).FirstOrDefault();
                                if (claimDataGroup != null && updateClaim)
                                {
                                    if (summary.CurrencyCode == PickListDetailBo.CurrencyCodeMyr)
                                        summary.Claim = claimDataGroup.ClaimRecoveryAmt.GetValueOrDefault();
                                    else
                                        summary.Claim = claimDataGroup.ForeignClaimRecoveryAmt.GetValueOrDefault();
                                    updateClaim = false;
                                }
                            }

                            SoaDataRiDataSummaryBos.Add(summary);

                            if (!string.IsNullOrEmpty(summary.CurrencyCode) && summary.CurrencyCode != PickListDetailBo.CurrencyCodeMyr)
                            {
                                var summaryMYR = new SoaDataRiDataSummaryBo
                                {
                                    Type = SoaDataRiDataSummaryBo.TypeRiDataSummaryCNIfrs17,
                                    BusinessCode = soaDataGroup.BusinessCode,
                                    TreatyCode = soaDataGroup.TreatyCode,
                                    RiskQuarter = FormatQuarter(soaDataGroup.RiskQuarter),
                                    SoaQuarter = FormatQuarter(SoaDataBatchBo.Quarter),
                                    CurrencyCode = SoaDataBatchBo.CurrencyCodePickListDetailBo?.Code,
                                    CurrencyRate = SoaDataBatchBo.CurrencyRate,
                                };
                                summaryMYR.Claim = summary.Claim * summaryMYR.CurrencyRate;
                                summaryMYR.GetGrossPremium();
                                summaryMYR.GetNetTotalAmount();

                                SoaDataRiDataSummaryBos.Add(summaryMYR);
                            }

                            //Added update for UpdatedAt for fail checking
                            var boToUpdate = SoaDataBatchBo;
                            SoaDataBatchService.Update(ref boToUpdate);
                        }
                    }
                }
            }
        }

        public void CreateCompiledSummaryByCellNameIFRS17()
        {
            if (SoaDataBatchBo == null)
                return;

            if (SoaDataCompiledSummaryBos.IsNullOrEmpty())
                SoaDataCompiledSummaryBos = new List<SoaDataCompiledSummaryBo> { };
            using (var db = new AppDbContext(false))
            {
                var businessOriginCode = CacheService.BusinessOrigins.Where(q => q.Id == TreatyBo?.BusinessOriginPickListDetailId).Select(q => q.Code).FirstOrDefault();
                if (string.IsNullOrEmpty(businessOriginCode))
                    return;

                double? soaDataRiskPremium = 0;
                var soaDataGroups = db.SoaData
                    .Where(q => q.SoaDataBatchId == SoaDataBatchBo.Id)
                    .Where(q => q.BusinessCode == businessOriginCode)
                    .GroupBy(q => new { q.TreatyCode, q.RiskQuarter, q.CurrencyCode, q.CurrencyRate })
                    .Select(q => new SoaDataGroupBy
                    {
                        TreatyCode = q.Key.TreatyCode,
                        RiskQuarter = q.Key.RiskQuarter,

                        TotalDiscount = q.Sum(d => d.TotalDiscount),
                        RiskPremium = q.Sum(d => d.RiskPremium),
                        Levy = q.Sum(d => d.Levy),
                        Gst = q.Sum(d => d.Gst),
                        ProfitComm = q.Sum(d => d.ProfitComm),
                        ModcoReserveIncome = q.Sum(d => d.ModcoReserveIncome),
                        RiDeposit = q.Sum(d => d.RiDeposit),
                        AdministrationContribution = q.Sum(d => d.AdministrationContribution),
                        ShareOfRiCommissionFromCompulsoryCession = q.Sum(d => d.ShareOfRiCommissionFromCompulsoryCession),
                        RecaptureFee = q.Sum(d => d.RecaptureFee),
                        CreditCardCharges = q.Sum(d => d.CreditCardCharges),

                        CurrencyCode = q.Key.CurrencyCode,
                        CurrencyRate = q.Key.CurrencyRate,
                    })
                    .OrderBy(q => q.TreatyCode)
                    .ToList();

                var soaDataCompiledSummaries = SoaDataCompiledSummaryBos
                    .Where(q => q.ReportingType == SoaDataCompiledSummaryBo.ReportingTypeIFRS4)
                    .GroupBy(q => new { q.TreatyCode, q.RiskQuarter, q.CurrencyCode, q.CurrencyRate })
                    .Select(q => new SoaDataCompiledSummaryBo
                    {
                        TreatyCode = q.Key.TreatyCode,
                        RiskQuarter = q.Key.RiskQuarter,

                        TotalPremium = q.Sum(d => d.TotalPremium),

                        CurrencyCode = q.Key.CurrencyCode,
                        CurrencyRate = q.Key.CurrencyRate,
                    })
                    .OrderBy(q => q.TreatyCode)
                    .ToList();

                // Risk Qtr shall be populate based on risk period in RI Data but when there is only Claim Data in SOA, Risk Qtr will follow SOA Qtr
                if (GetRiDataBatchId() != 0)
                {
                    var riSummaryGroups = QueryRiSummaryGroupByCellName(true);
                    var riSummaryEndDateGroups = QueryRiSummaryGroupByEndDateCellName(true);

                    var claimDataGroups = QueryClaimDataGroupBy(db, false, true);

                    foreach (var riSummaryGroup in riSummaryGroups)
                    {
                        var compiledSummary = new SoaDataCompiledSummaryBo
                        {
                            SoaDataBatchId = SoaDataBatchBo.Id,
                            ReportingType = SoaDataCompiledSummaryBo.ReportingTypeCNIFRS17,

                            InvoiceDate = DateTime.Now,
                            StatementReceivedDate = SoaDataBatchBo.StatementReceivedAt,

                            TreatyCode = riSummaryGroup.TreatyCode,
                            RiskQuarter = riSummaryGroup.RiskQuarter,
                            SoaQuarter = FormatQuarter(SoaDataBatchBo.Quarter),
                            BusinessCode = businessOriginCode,
                            Frequency = riSummaryGroup.Frequency,
                            ContractCode = riSummaryGroup.ContractCode,
                            AnnualCohort = riSummaryGroup.AnnualCohort,
                            Mfrs17CellName = riSummaryGroup.Mfrs17CellName,

                            CurrencyCode = riSummaryGroup.CurrencyCode,
                            CurrencyRate = riSummaryGroup.CurrencyRate,

                            NbPremium = Util.RoundNullableValue(riSummaryGroup.NbPremium.GetValueOrDefault(), 2),
                            RnPremium = Util.RoundNullableValue(riSummaryGroup.RnPremium.GetValueOrDefault(), 2),
                            AltPremium = Util.RoundNullableValue(riSummaryGroup.AltPremium.GetValueOrDefault(), 2),

                            NbDiscount = Util.RoundNullableValue(riSummaryGroup.NbDiscount.GetValueOrDefault(), 2),
                            RnDiscount = Util.RoundNullableValue(riSummaryGroup.RnDiscount.GetValueOrDefault(), 2),
                            AltDiscount = Util.RoundNullableValue(riSummaryGroup.AltDiscount.GetValueOrDefault(), 2),

                            DTH = Util.RoundNullableValue(riSummaryGroup.DTH.GetValueOrDefault(), 2),
                            TPA = Util.RoundNullableValue(riSummaryGroup.TPA.GetValueOrDefault(), 2),
                            TPS = Util.RoundNullableValue(riSummaryGroup.TPS.GetValueOrDefault(), 2),
                            PPD = Util.RoundNullableValue(riSummaryGroup.PPD.GetValueOrDefault(), 2),
                            CCA = Util.RoundNullableValue(riSummaryGroup.CCA.GetValueOrDefault(), 2),
                            CCS = Util.RoundNullableValue(riSummaryGroup.CCS.GetValueOrDefault(), 2),
                            PA = Util.RoundNullableValue(riSummaryGroup.PA.GetValueOrDefault(), 2),
                            HS = Util.RoundNullableValue(riSummaryGroup.HS.GetValueOrDefault(), 2),
                            TPD = Util.RoundNullableValue(riSummaryGroup.TPD.GetValueOrDefault(), 2),
                            CI = Util.RoundNullableValue(riSummaryGroup.CI.GetValueOrDefault(), 2),

                            NoClaimBonus = Util.RoundNullableValue(riSummaryGroup.NoClaimBonus.GetValueOrDefault(), 2),
                            SurrenderValue = Util.RoundNullableValue(riSummaryGroup.SurrenderValue.GetValueOrDefault(), 2),
                            DatabaseCommission = Util.RoundNullableValue(riSummaryGroup.DatabaseCommission.GetValueOrDefault(), 2),
                            BrokerageFee = Util.RoundNullableValue(riSummaryGroup.BrokerageFee.GetValueOrDefault(), 2),
                            ServiceFee = Util.RoundNullableValue(riSummaryGroup.ServiceFee.GetValueOrDefault(), 2),
                        };
                        compiledSummary.GetSstPayable();
                        compiledSummary.GetTotalAmount();
                        compiledSummary.GetTotalPremium();

                        // NB/RN/ALT SAR shall populate if it is monthly mode
                        // other than monthly mode, will take sum AAR
                        if (compiledSummary.Frequency == PickListDetailBo.PremiumFrequencyCodeMonthly)
                        {
                            if (!riSummaryEndDateGroups.IsNullOrEmpty())
                            {
                                var riSummaryEndDateGroup = riSummaryEndDateGroups?.Where(q => q.TreatyCode == riSummaryGroup.TreatyCode && q.RiskQuarter == riSummaryGroup.RiskQuarter && q.CurrencyCode == riSummaryGroup.CurrencyCode
                                        && q.CurrencyRate == riSummaryGroup.CurrencyRate && q.Frequency == riSummaryGroup.Frequency && q.ContractCode == riSummaryGroup.ContractCode && q.AnnualCohort == riSummaryGroup.AnnualCohort 
                                        && q.Mfrs17CellName == riSummaryGroup.Mfrs17CellName).FirstOrDefault();
                                if (riSummaryEndDateGroup != null)
                                {
                                    compiledSummary.NbCession = riSummaryGroup.NbCession.GetValueOrDefault();
                                    compiledSummary.RnCession = riSummaryGroup.RnCession.GetValueOrDefault();
                                    compiledSummary.AltCession = riSummaryGroup.AltCession.GetValueOrDefault();

                                    compiledSummary.NbSar = Util.RoundNullableValue(riSummaryEndDateGroup.NbSar.GetValueOrDefault(), 2);
                                    compiledSummary.RnSar = Util.RoundNullableValue(riSummaryEndDateGroup.RnSar.GetValueOrDefault(), 2);
                                    compiledSummary.AltSar = Util.RoundNullableValue(riSummaryEndDateGroup.AltSar.GetValueOrDefault(), 2);
                                }
                            }
                        }                        
                        else
                        {
                            if (riSummaryGroup != null)
                            {
                                compiledSummary.NbCession = riSummaryGroup.NbCession.GetValueOrDefault();
                                compiledSummary.RnCession = riSummaryGroup.RnCession.GetValueOrDefault();
                                compiledSummary.AltCession = riSummaryGroup.AltCession.GetValueOrDefault();

                                compiledSummary.NbSar = Util.RoundNullableValue(riSummaryGroup.NbSar.GetValueOrDefault(), 2);
                                compiledSummary.RnSar = Util.RoundNullableValue(riSummaryGroup.RnSar.GetValueOrDefault(), 2);
                                compiledSummary.AltSar = Util.RoundNullableValue(riSummaryGroup.AltSar.GetValueOrDefault(), 2);
                            }
                        }

                        var soaDataGroup = soaDataGroups?.Where(q => q.TreatyCode == riSummaryGroup.TreatyCode && q.RiskQuarter == riSummaryGroup.RiskQuarter && q.CurrencyCode == riSummaryGroup.CurrencyCode && q.CurrencyRate == riSummaryGroup.CurrencyRate).FirstOrDefault();
                        if (soaDataGroup != null)
                        {
                            compiledSummary.TotalDiscount = Util.RoundNullableValue(soaDataGroup.TotalDiscount.GetValueOrDefault(), 2);
                            compiledSummary.Levy = Util.RoundNullableValue(soaDataGroup.Levy.GetValueOrDefault(), 2);
                            compiledSummary.Gst = Util.RoundNullableValue(soaDataGroup.Gst.GetValueOrDefault(), 2);
                            compiledSummary.ProfitComm = Util.RoundNullableValue(soaDataGroup.ProfitComm.GetValueOrDefault(), 2);
                            compiledSummary.ModcoReserveIncome = Util.RoundNullableValue(soaDataGroup.ModcoReserveIncome.GetValueOrDefault(), 2);
                            compiledSummary.RiDeposit = Util.RoundNullableValue(soaDataGroup.RiDeposit.GetValueOrDefault(), 2);
                            compiledSummary.AdministrationContribution = Util.RoundNullableValue(soaDataGroup.AdministrationContribution.GetValueOrDefault(), 2);
                            compiledSummary.ShareOfRiCommissionFromCompulsoryCession = Util.RoundNullableValue(soaDataGroup.ShareOfRiCommissionFromCompulsoryCession.GetValueOrDefault(), 2);
                            compiledSummary.RecaptureFee = Util.RoundNullableValue(soaDataGroup.RecaptureFee.GetValueOrDefault(), 2);
                            compiledSummary.CreditCardCharges = Util.RoundNullableValue(soaDataGroup.CreditCardCharges.GetValueOrDefault(), 2);
                            compiledSummary.CurrencyCode = soaDataGroup.CurrencyCode;
                            compiledSummary.CurrencyRate = soaDataGroup.CurrencyRate;

                            soaDataRiskPremium = soaDataGroup.RiskPremium.GetValueOrDefault();
                        }

                        if (compiledSummary.RiskQuarter == compiledSummary.SoaQuarter)
                        {
                            var claimDataGroup = claimDataGroups?.Where(q => q.TreatyCode == riSummaryGroup.TreatyCode && q.SoaQuarter == SoaDataBatchBo.Quarter && q.ContractCode == compiledSummary.ContractCode && q.AnnualCohort == compiledSummary.AnnualCohort).FirstOrDefault();
                            if (claimDataGroup != null)
                            {
                                if (compiledSummary.CurrencyCode == PickListDetailBo.CurrencyCodeMyr)
                                    compiledSummary.Claim = Util.RoundNullableValue(claimDataGroup.ClaimRecoveryAmt.GetValueOrDefault(), 2);
                                else
                                    compiledSummary.Claim = Util.RoundNullableValue(claimDataGroup.ForeignClaimRecoveryAmt.GetValueOrDefault(), 2);
                            }
                        }

                        switch (businessOriginCode)
                        {
                            case PickListDetailBo.BusinessOriginCodeWithinMalaysia:
                                compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeWM;
                                break;
                            case PickListDetailBo.BusinessOriginCodeOutsideMalaysia:
                                compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeOM;
                                break;
                            case PickListDetailBo.BusinessOriginCodeServiceFee:
                                compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeSFWM;
                                break;
                        }

                        compiledSummary.GetNetTotalAmount();
                        if (compiledSummary.RiskQuarter != compiledSummary.SoaQuarter)
                        {
                            switch (businessOriginCode)
                            {
                                case PickListDetailBo.BusinessOriginCodeWithinMalaysia:
                                    if (compiledSummary.NetTotalAmount < 0)
                                        compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeCNWM;
                                    else
                                        compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeDNWM;
                                    break;
                                case PickListDetailBo.BusinessOriginCodeOutsideMalaysia:
                                    if (compiledSummary.NetTotalAmount < 0)
                                        compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeCNOM;
                                    else
                                        compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeDNOM;
                                    break;
                                case PickListDetailBo.BusinessOriginCodeServiceFee:
                                    if (compiledSummary.TotalAmount < 0)
                                        compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeCNSFWM;
                                    else
                                        compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeDNSFWM;
                                    break;
                            }
                        }
                                                
                        if (compiledSummary.InvoiceType != SoaDataCompiledSummaryBo.InvoiceTypeWM && compiledSummary.InvoiceType != SoaDataCompiledSummaryBo.InvoiceTypeOM)
                            compiledSummary.Amount1 = compiledSummary.NetTotalAmount;

                        // Risk Premium update by multiply with premium ratio
                        var soaDataCompiledSummaryBo = soaDataCompiledSummaries.Where(q => q.TreatyCode == compiledSummary.TreatyCode && q.RiskQuarter == compiledSummary.RiskQuarter && q.CurrencyCode == compiledSummary.CurrencyCode && q.CurrencyRate == compiledSummary.CurrencyRate).FirstOrDefault();
                        if (soaDataCompiledSummaryBo != null)
                            compiledSummary.GetRiskPremium(soaDataCompiledSummaryBo.TotalPremium.GetValueOrDefault(), soaDataRiskPremium.GetValueOrDefault());

                        SoaDataCompiledSummaryBos.Add(compiledSummary);

                        //Added update for UpdatedAt for fail checking
                        var boToUpdate = SoaDataBatchBo;
                        SoaDataBatchService.Update(ref boToUpdate);
                    }
                }
                else
                {
                    var riSummaryGroups = QueryRiSummaryGroupByCellName();
                    var riSummaryEndDateGroups = QueryRiSummaryGroupByEndDateCellName();

                    var claimDataGroups = QueryClaimDataGroupBy(db);

                    foreach (var soaDataGroup in soaDataGroups)
                    {
                        var compiledSummary = new SoaDataCompiledSummaryBo
                        {
                            SoaDataBatchId = SoaDataBatchBo.Id,
                            ReportingType = SoaDataCompiledSummaryBo.ReportingTypeCNIFRS17,

                            InvoiceDate = DateTime.Now,
                            StatementReceivedDate = SoaDataBatchBo.StatementReceivedAt,

                            TreatyCode = soaDataGroup.TreatyCode,
                            RiskQuarter = soaDataGroup.RiskQuarter,
                            SoaQuarter = FormatQuarter(SoaDataBatchBo.Quarter),
                            BusinessCode = businessOriginCode,

                            TotalDiscount = Util.RoundNullableValue(soaDataGroup.TotalDiscount.GetValueOrDefault(), 2),
                            RiskPremium = Util.RoundNullableValue(soaDataGroup.RiskPremium.GetValueOrDefault(), 2),
                            Levy = Util.RoundNullableValue(soaDataGroup.Levy.GetValueOrDefault(), 2),
                            Gst = Util.RoundNullableValue(soaDataGroup.Gst.GetValueOrDefault(), 2),
                            ProfitComm = Util.RoundNullableValue(soaDataGroup.ProfitComm.GetValueOrDefault(), 2),
                            ModcoReserveIncome = Util.RoundNullableValue(soaDataGroup.ModcoReserveIncome.GetValueOrDefault(), 2),
                            RiDeposit = Util.RoundNullableValue(soaDataGroup.RiDeposit.GetValueOrDefault(), 2),
                            AdministrationContribution = Util.RoundNullableValue(soaDataGroup.AdministrationContribution.GetValueOrDefault(), 2),
                            ShareOfRiCommissionFromCompulsoryCession = Util.RoundNullableValue(soaDataGroup.ShareOfRiCommissionFromCompulsoryCession.GetValueOrDefault(), 2),
                            RecaptureFee = Util.RoundNullableValue(soaDataGroup.RecaptureFee.GetValueOrDefault(), 2),
                            CreditCardCharges = Util.RoundNullableValue(soaDataGroup.CreditCardCharges.GetValueOrDefault(), 2),

                            CurrencyCode = soaDataGroup.CurrencyCode,
                            CurrencyRate = soaDataGroup.CurrencyRate.GetValueOrDefault(),
                        };

                        var riSummaryGroup = riSummaryGroups?.Where(q => q.TreatyCode == soaDataGroup.TreatyCode && q.RiskQuarter == compiledSummary.RiskQuarter && q.CurrencyCode == compiledSummary.CurrencyCode && q.CurrencyRate == compiledSummary.CurrencyRate).FirstOrDefault();
                        if (riSummaryGroup != null)
                        {
                            compiledSummary.NbPremium = Util.RoundNullableValue(riSummaryGroup.NbPremium.GetValueOrDefault(), 2);
                            compiledSummary.RnPremium = Util.RoundNullableValue(riSummaryGroup.RnPremium.GetValueOrDefault(), 2);
                            compiledSummary.AltPremium = Util.RoundNullableValue(riSummaryGroup.AltPremium.GetValueOrDefault(), 2);

                            compiledSummary.NbDiscount = Util.RoundNullableValue(riSummaryGroup.NbDiscount.GetValueOrDefault(), 2);
                            compiledSummary.RnDiscount = Util.RoundNullableValue(riSummaryGroup.RnDiscount.GetValueOrDefault(), 2);
                            compiledSummary.AltDiscount = Util.RoundNullableValue(riSummaryGroup.AltDiscount.GetValueOrDefault(), 2);

                            compiledSummary.DTH = Util.RoundNullableValue(riSummaryGroup.DTH.GetValueOrDefault(), 2);
                            compiledSummary.TPA = Util.RoundNullableValue(riSummaryGroup.TPA.GetValueOrDefault(), 2);
                            compiledSummary.TPS = Util.RoundNullableValue(riSummaryGroup.TPS.GetValueOrDefault(), 2);
                            compiledSummary.PPD = Util.RoundNullableValue(riSummaryGroup.PPD.GetValueOrDefault(), 2);
                            compiledSummary.CCA = Util.RoundNullableValue(riSummaryGroup.CCA.GetValueOrDefault(), 2);
                            compiledSummary.CCS = Util.RoundNullableValue(riSummaryGroup.CCS.GetValueOrDefault(), 2);
                            compiledSummary.PA = Util.RoundNullableValue(riSummaryGroup.PA.GetValueOrDefault(), 2);
                            compiledSummary.HS = Util.RoundNullableValue(riSummaryGroup.HS.GetValueOrDefault(), 2);
                            compiledSummary.TPD = Util.RoundNullableValue(riSummaryGroup.TPD.GetValueOrDefault(), 2);
                            compiledSummary.CI = Util.RoundNullableValue(riSummaryGroup.CI.GetValueOrDefault(), 2);

                            compiledSummary.NoClaimBonus = Util.RoundNullableValue(riSummaryGroup.NoClaimBonus.GetValueOrDefault(), 2);
                            compiledSummary.SurrenderValue = Util.RoundNullableValue(riSummaryGroup.SurrenderValue.GetValueOrDefault(), 2);
                            compiledSummary.DatabaseCommission = Util.RoundNullableValue(riSummaryGroup.DatabaseCommission.GetValueOrDefault(), 2);
                            compiledSummary.BrokerageFee = Util.RoundNullableValue(riSummaryGroup.BrokerageFee.GetValueOrDefault(), 2);
                            compiledSummary.ServiceFee = Util.RoundNullableValue(riSummaryGroup.ServiceFee.GetValueOrDefault(), 2);

                            compiledSummary.Frequency = riSummaryGroup.Frequency;
                            compiledSummary.ContractCode = riSummaryGroup.ContractCode;
                            compiledSummary.AnnualCohort = riSummaryGroup.AnnualCohort;
                            compiledSummary.Mfrs17CellName = riSummaryGroup.Mfrs17CellName;
                        }
                        compiledSummary.GetSstPayable();
                        compiledSummary.GetTotalAmount();
                        compiledSummary.GetTotalPremium();

                        // NB/RN/ALT SAR shall populate if it is monthly mode
                        // other than monthly mode, will take sum AAR
                        if (compiledSummary.Frequency == PickListDetailBo.PremiumFrequencyCodeMonthly)
                        {
                            if (!riSummaryEndDateGroups.IsNullOrEmpty())
                            {
                                var riSummaryEndDateGroup = riSummaryEndDateGroups?.Where(q => q.TreatyCode == soaDataGroup.TreatyCode && q.RiskQuarter == compiledSummary.RiskQuarter && q.CurrencyCode == compiledSummary.CurrencyCode && q.CurrencyRate == riSummaryGroup.CurrencyRate && q.Frequency == compiledSummary.Frequency).FirstOrDefault();
                                if (riSummaryEndDateGroup != null)
                                {
                                    compiledSummary.NbCession = riSummaryEndDateGroup.NbCession.GetValueOrDefault();
                                    compiledSummary.RnCession = riSummaryEndDateGroup.RnCession.GetValueOrDefault();
                                    compiledSummary.AltCession = riSummaryEndDateGroup.AltCession.GetValueOrDefault();

                                    compiledSummary.NbSar = Util.RoundNullableValue(riSummaryEndDateGroup.NbSar.GetValueOrDefault(), 2);
                                    compiledSummary.RnSar = Util.RoundNullableValue(riSummaryEndDateGroup.RnSar.GetValueOrDefault(), 2);
                                    compiledSummary.AltSar = Util.RoundNullableValue(riSummaryEndDateGroup.AltSar.GetValueOrDefault(), 2);
                                }
                            }
                        }                        
                        else
                        {
                            if (riSummaryGroup != null)
                            {
                                compiledSummary.NbCession = riSummaryGroup.NbCession.GetValueOrDefault();
                                compiledSummary.RnCession = riSummaryGroup.RnCession.GetValueOrDefault();
                                compiledSummary.AltCession = riSummaryGroup.AltCession.GetValueOrDefault();

                                compiledSummary.NbSar = Util.RoundNullableValue(riSummaryGroup.NbSar.GetValueOrDefault(), 2);
                                compiledSummary.RnSar = Util.RoundNullableValue(riSummaryGroup.RnSar.GetValueOrDefault(), 2);
                                compiledSummary.AltSar = Util.RoundNullableValue(riSummaryGroup.AltSar.GetValueOrDefault(), 2);
                            }
                        }

                        if (compiledSummary.RiskQuarter == compiledSummary.SoaQuarter)
                        {
                            var claimDataGroup = claimDataGroups?.Where(q => q.TreatyCode == soaDataGroup.TreatyCode && q.SoaQuarter == SoaDataBatchBo.Quarter).FirstOrDefault();
                            if (claimDataGroup != null)
                            {
                                if (compiledSummary.CurrencyCode == PickListDetailBo.CurrencyCodeMyr)
                                    compiledSummary.Claim = Util.RoundNullableValue(claimDataGroup.ClaimRecoveryAmt.GetValueOrDefault(), 2);
                                else
                                    compiledSummary.Claim = Util.RoundNullableValue(claimDataGroup.ForeignClaimRecoveryAmt.GetValueOrDefault(), 2);
                            }
                        }

                        switch (businessOriginCode)
                        {
                            case PickListDetailBo.BusinessOriginCodeWithinMalaysia:
                                compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeWM;
                                break;
                            case PickListDetailBo.BusinessOriginCodeOutsideMalaysia:
                                compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeOM;
                                break;
                            case PickListDetailBo.BusinessOriginCodeServiceFee:
                                compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeSFWM;
                                break;
                        }

                        compiledSummary.GetNetTotalAmount();
                        if (compiledSummary.RiskQuarter != compiledSummary.SoaQuarter)
                        {
                            switch (businessOriginCode)
                            {
                                case PickListDetailBo.BusinessOriginCodeWithinMalaysia:
                                    if (compiledSummary.NetTotalAmount < 0)
                                        compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeCNWM;
                                    else
                                        compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeDNWM;
                                    break;
                                case PickListDetailBo.BusinessOriginCodeOutsideMalaysia:
                                    if (compiledSummary.NetTotalAmount < 0)
                                        compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeCNOM;
                                    else
                                        compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeDNOM;
                                    break;
                                case PickListDetailBo.BusinessOriginCodeServiceFee:
                                    if (compiledSummary.TotalAmount < 0)
                                        compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeCNSFWM;
                                    else
                                        compiledSummary.InvoiceType = SoaDataCompiledSummaryBo.InvoiceTypeDNSFWM;
                                    break;
                            }
                        }
                        
                        if (compiledSummary.InvoiceType != SoaDataCompiledSummaryBo.InvoiceTypeWM && compiledSummary.InvoiceType != SoaDataCompiledSummaryBo.InvoiceTypeOM)
                            compiledSummary.Amount1 = compiledSummary.NetTotalAmount;

                        // Risk Premium update by multiply with premium ratio
                        var soaDataCompiledSummaryBo = soaDataCompiledSummaries.Where(q => q.TreatyCode == compiledSummary.TreatyCode && q.RiskQuarter == compiledSummary.RiskQuarter && q.CurrencyCode == compiledSummary.CurrencyCode && q.CurrencyRate == compiledSummary.CurrencyRate).FirstOrDefault();
                        if (soaDataCompiledSummaryBo != null)
                            compiledSummary.GetRiskPremium(soaDataCompiledSummaryBo.TotalPremium.GetValueOrDefault(), soaDataGroup.RiskPremium.GetValueOrDefault());

                        SoaDataCompiledSummaryBos.Add(compiledSummary);

                        //Added update for UpdatedAt for fail checking
                        var boToUpdate = SoaDataBatchBo;
                        SoaDataBatchService.Update(ref boToUpdate);
                    }
                }

            }
        }

        public void DeleteSoaDataRiDataSummary()
        {
            if (Test)
                return;

            // DELETE ALL SOA DATA RI DATA SUMMARY BEFORE PROCESS
            PrintMessage("Deleting RI Data Summary records...", true, false);
            SoaDataRiDataSummaryService.DeleteBySoaDataBatchId(SoaDataBatchBo.Id);
            PrintMessage("Deleted RI Data Summary records", true, false);
        }

        public void DeleteSoaDataPostValidation()
        {
            if (Test)
                return;

            // DELETE ALL POST VALIDATION BEFORE PROCESS
            PrintMessage("Deleting Post Validation records...", true, false);
            SoaDataPostValidationService.DeleteBySoaDataBatchId(SoaDataBatchBo.Id);
            PrintMessage("Deleted Post Validation records", true, false);
        }

        public void DeleteSoaDataPostValidationDifferences()
        {
            if (Test)
                return;

            // DELETE ALL POST VALIDATION DIFFERENCE BEFORE PROCESS
            PrintMessage("Deleting Post Validation Difference records...", true, false);
            SoaDataPostValidationDifferenceService.DeleteBySoaDataBatchId(SoaDataBatchBo.Id);
            PrintMessage("Deleted Post Validation  Difference records", true, false);
        }

        public void DeleteSoaDataDiscrepancy()
        {
            if (Test)
                return;

            // DELETE ALL SOA DATA DISCREPANCY BEFORE PROCESS
            PrintMessage("Deleting SOA Data Discrepancy records...", true, false);
            SoaDataDiscrepancyService.DeleteBySoaDataBatchId(SoaDataBatchBo.Id); // DO NOT TRAIL
            PrintMessage("Deleted SOA Data Discrepancy records", true, false);
        }

        public void DeleteSoaData()
        {
            if (Test)
                return;

            // DELETE ALL SOA DATA BEFORE PROCESS
            PrintMessage("Deleting SOA Data records...", true, false);
            SoaDataService.DeleteBySoaDataBatchId(SoaDataBatchBo.Id); // DO NOT TRAIL
            PrintMessage("Deleted SOA Data records", true, false);
        }

        public void DeleteSoaDataCompiledSummary()
        {
            if (Test)
                return;

            // DELETE ALL SOA DATA COMPILED SUMMARY BEFORE PROCESS
            PrintMessage("Deleting SOA Data Compiled records...", true, false);
            SoaDataCompiledSummaryService.DeleteBySoaDataBatchId(SoaDataBatchBo.Id); // DO NOT TRAIL
            PrintMessage("Deleted SOA Data Compiled records", true, false);
        }

        public SoaDataBatchBo LoadSoaDataBatch()
        {
            TreatyBo = null;
            RiDataBatchBo = null;
            ClaimDataBatchBo = null;

            if (CutOffService.IsCutOffProcessing())
                return null;

            SoaDataBatchBo = SoaDataBatchService.FindByDataUpdateStatus(SoaDataBatchBo.DataUpdateStatusSubmitForDataUpdate);
            if (SoaDataBatchBo != null)
            {
                TreatyBo = SoaDataBatchBo.TreatyBo;
                if (SoaDataBatchBo.RiDataBatchId.HasValue)
                    RiDataBatchBo = RiDataBatchService.Find(SoaDataBatchBo.RiDataBatchId.Value);
                if (SoaDataBatchBo.ClaimDataBatchId.HasValue)
                    ClaimDataBatchBo = ClaimDataBatchService.Find(SoaDataBatchBo.ClaimDataBatchId.Value);
                RiDataAutoCreate = SoaDataBatchBo.IsRiDataAutoCreate;
                ClaimDataAutoCreate = SoaDataBatchBo.IsClaimDataAutoCreate;
            }
            return SoaDataBatchBo;
        }

        public void UpdateBatchDataUpdateStatus(int dataStatus, string description, bool setDataUpdatingStatus = false)
        {
            UpdatingStatusHistoryBo = null;
            if (Test)
                return;

            var trail = new TrailObject();
            var statusBo = new StatusHistoryBo
            {
                ModuleId = ModuleBo.Id,
                ObjectId = SoaDataBatchBo.Id,
                Status = dataStatus,
                CreatedById = User.DefaultSuperUserId,
                UpdatedById = User.DefaultSuperUserId,
            };
            StatusHistoryService.Create(ref statusBo, ref trail);

            var batch = SoaDataBatchBo;
            batch.DataUpdateStatus = dataStatus;

            var result = SoaDataBatchService.Update(ref batch, ref trail);
            var userTrailBo = new UserTrailBo(
                SoaDataBatchBo.Id,
                description,
                result,
                trail,
                User.DefaultSuperUserId
            );
            UserTrailService.Create(ref userTrailBo);

            if (setDataUpdatingStatus)
                UpdatingStatusHistoryBo = statusBo;
        }

        public void Save()
        {
            if (Test)
                return;
            if (SoaDataBatchBo == null)
                return;            
            if (!SoaDataRiDataSummaryBos.IsNullOrEmpty())
            {
                foreach (var soaDataRiDataSummaryBo in SoaDataRiDataSummaryBos)
                {
                    soaDataRiDataSummaryBo.SoaDataBatchId = SoaDataBatchBo.Id;                    
                    soaDataRiDataSummaryBo.CreatedById = SoaDataBatchBo.CreatedById;
                    soaDataRiDataSummaryBo.UpdatedById = SoaDataBatchBo.UpdatedById;
                    var bo = soaDataRiDataSummaryBo;
                    SoaDataRiDataSummaryService.Create(ref bo);

                    //Added update for UpdatedAt for fail checking
                    var boToUpdate = SoaDataBatchBo;
                    SoaDataBatchService.Update(ref boToUpdate);
                }
            }
            if (!SoaDataPostValidationBos.IsNullOrEmpty())
            {
                foreach (var soaDataPostValidationBo in SoaDataPostValidationBos)
                {
                    soaDataPostValidationBo.SoaDataBatchId = SoaDataBatchBo.Id;
                    soaDataPostValidationBo.SoaQuarter = FormatQuarter(SoaDataBatchBo.Quarter);
                    soaDataPostValidationBo.CreatedById = SoaDataBatchBo.CreatedById;
                    soaDataPostValidationBo.UpdatedById = SoaDataBatchBo.UpdatedById;
                    var bo = soaDataPostValidationBo;
                    SoaDataPostValidationService.Create(ref bo);

                    //Added update for UpdatedAt for fail checking
                    var boToUpdate = SoaDataBatchBo;
                    SoaDataBatchService.Update(ref boToUpdate);
                }
            }
            if (!SoaDataPostValidationDifferenceBos.IsNullOrEmpty())
            {
                foreach (var soaDataPostValidationDifferenceBo in SoaDataPostValidationDifferenceBos)
                {
                    soaDataPostValidationDifferenceBo.SoaDataBatchId = SoaDataBatchBo.Id;
                    soaDataPostValidationDifferenceBo.CreatedById = SoaDataBatchBo.CreatedById;
                    soaDataPostValidationDifferenceBo.UpdatedById = SoaDataBatchBo.UpdatedById;
                    var bo = soaDataPostValidationDifferenceBo;
                    SoaDataPostValidationDifferenceService.Create(ref bo);

                    //Added update for UpdatedAt for fail checking
                    var boToUpdate = SoaDataBatchBo;
                    SoaDataBatchService.Update(ref boToUpdate);
                }
            }
            if (!SoaDataDiscrepancyBos.IsNullOrEmpty())
            {
                foreach (var soaDataDiscrepancyBo in SoaDataDiscrepancyBos)
                {
                    soaDataDiscrepancyBo.SoaDataBatchId = SoaDataBatchBo.Id;
                    soaDataDiscrepancyBo.CreatedById = SoaDataBatchBo.CreatedById;
                    soaDataDiscrepancyBo.UpdatedById = SoaDataBatchBo.UpdatedById;
                    var bo = soaDataDiscrepancyBo;
                    SoaDataDiscrepancyService.Create(ref bo);

                    //Added update for UpdatedAt for fail checking
                    var boToUpdate = SoaDataBatchBo;
                    SoaDataBatchService.Update(ref boToUpdate);
                }
            }
            if (!SoaDataCompiledSummaryBos.IsNullOrEmpty())
            {
                foreach (var soaDataCompiledSummaryBo in SoaDataCompiledSummaryBos)
                {
                    soaDataCompiledSummaryBo.SoaDataBatchId = SoaDataBatchBo.Id;
                    soaDataCompiledSummaryBo.CreatedById = SoaDataBatchBo.CreatedById;
                    soaDataCompiledSummaryBo.UpdatedById = SoaDataBatchBo.UpdatedById;
                    var bo = soaDataCompiledSummaryBo;
                    SoaDataCompiledSummaryService.Create(ref bo);

                    //Added update for UpdatedAt for fail checking
                    var boToUpdate = SoaDataBatchBo;
                    SoaDataBatchService.Update(ref boToUpdate);
                }
            }
        }

        public void PrintSoaDataRiDataSummary(SoaDataRiDataSummaryBo summary)
        {
            Log = false;
            PrintOutputTitle("Summary");
            PrintDetail("BusinessCode", summary.BusinessCode);
            PrintDetail("TreatyCode", summary.TreatyCode);
            PrintDetail("RiskMonth", summary.RiskMonth);
            PrintMessage();
            PrintDetail("NbPremium", summary.NbPremium);
            PrintDetail("RnPremium", summary.RnPremium);
            PrintDetail("AltPremium", summary.AltPremium);
            PrintMessage();
            PrintDetail("GrossPremium", summary.GrossPremium);
            PrintMessage();
            PrintDetail("TotalDiscount", summary.TotalDiscount);
            PrintDetail("NoClaimBonus", summary.NoClaimBonus);
            PrintDetail("Claim", summary.Claim);
            PrintDetail("SurrenderValue", summary.SurrenderValue);
            PrintDetail("Gst", summary.Gst);
            PrintMessage();
            PrintDetail("NetTotalAmount", summary.NetTotalAmount);
            PrintMessage();
            Log = true;
        }

        public void PrintPostValidation(SoaDataPostValidationBo postValidation)
        {
            Log = false;
            PrintOutputTitle("Post Validation > " + SoaDataPostValidationBo.GetTypeName(postValidation.Type));
            PrintDetail("BusinessCode", postValidation.BusinessCode);
            PrintDetail("TreatyCode", postValidation.TreatyCode);
            PrintDetail("RiskMonth", postValidation.RiskMonth);
            PrintMessage();
            PrintDetail("NbPremium", postValidation.NbPremium);
            PrintDetail("RnPremium", postValidation.RnPremium);
            PrintDetail("AltPremium", postValidation.AltPremium);
            PrintMessage();
            PrintDetail("GrossPremium", postValidation.GrossPremium);
            PrintMessage();
            PrintDetail("NbDiscount", postValidation.NbDiscount);
            PrintDetail("RnDiscount", postValidation.RnDiscount);
            PrintDetail("AltDiscount", postValidation.AltDiscount);
            PrintMessage();
            PrintDetail("TotalDiscount", postValidation.TotalDiscount);
            PrintMessage();
            PrintDetail("NoClaimBonus", postValidation.NoClaimBonus);
            PrintDetail("Claim", postValidation.Claim);
            PrintDetail("SurrenderValue", postValidation.SurrenderValue);
            PrintDetail("Gst", postValidation.Gst);
            PrintMessage();
            PrintDetail("NetTotalAmount", postValidation.NetTotalAmount);
            PrintMessage();
            PrintDetail("NbCession", postValidation.NbCession);
            PrintDetail("RnCession", postValidation.RnCession);
            PrintDetail("AltCession", postValidation.AltCession);
            PrintMessage();
            PrintDetail("NbSar", postValidation.NbSar);
            PrintDetail("RnSar", postValidation.RnSar);
            PrintDetail("AltSar", postValidation.AltSar);
            PrintMessage();
            PrintDetail("DTH", postValidation.DTH);
            PrintDetail("TPA", postValidation.TPA);
            PrintDetail("TPS", postValidation.TPS);
            PrintDetail("PPD", postValidation.PPD);
            PrintDetail("CCA", postValidation.CCA);
            PrintDetail("CCS", postValidation.CCS);
            PrintDetail("PA", postValidation.PA);
            PrintDetail("HS", postValidation.HS);
            PrintMessage();
            Log = true;
        }

        public void PrintPostValidationDifference(SoaDataPostValidationDifferenceBo postValidationDiff)
        {
            Log = false;
            PrintOutputTitle("Post Validation Diff > " + SoaDataPostValidationDifferenceBo.GetTypeName(postValidationDiff.Type));
            PrintDetail("BusinessCode", postValidationDiff.BusinessCode);
            PrintDetail("TreatyCode", postValidationDiff.TreatyCode);
            PrintDetail("RiskMonth", postValidationDiff.RiskMonth);
            PrintMessage();
            PrintDetail("GrossPremium", postValidationDiff.GrossPremium);
            PrintDetail("DifferenceNetTotalAmount", postValidationDiff.DifferenceNetTotalAmount);
            PrintDetail("DifferencePercetage", postValidationDiff.DifferencePercetage);
            PrintMessage();
            Log = true;
        }

        public int GetTreatyId()
        {
            //return TreartId; // for testing purpose
            if (SoaDataBatchBo == null)
                return 0;
            int treatyId = SoaDataBatchBo.TreatyId.GetValueOrDefault();
            return treatyId;
        }

        public int GetRiDataBatchId()
        {
            //return RiDataBatchId; // for testing purpose
            if (RiDataBatchBo == null)
                return 0;
            int riDataBatchId = RiDataBatchBo.Id;
            return riDataBatchId;
        }

        public int GetClaimDataBatchId()
        {
            //return ClaimDataBatchId; // for testing purpose
            if (ClaimDataBatchBo == null)
                return 0;
            int claimDataBatchId = ClaimDataBatchBo.Id;
            return claimDataBatchId;
        }

        public List<RiDataGroupBy> QueryRiDataGroupBy(AppDbContext db, bool ifrs17 = false, bool byCellName = false, bool byFrequencyCode = true)
        {
            int riDataBatchId = GetRiDataBatchId();
            if (riDataBatchId == 0)
                return null;

            if (byCellName) // data grouped by Mfrs17 Cell Name
            {
                if (ifrs17)
                    return db.RiData
                        .Where(q => q.RiDataBatchId == riDataBatchId)
                        .GroupBy(q => new { q.TreatyCode, q.RiskPeriodMonth, q.RiskPeriodYear, q.CurrencyCode, q.PremiumFrequencyCode, q.Mfrs17CellName, q.Mfrs17TreatyCode, AnnualCohort = q.Mfrs17AnnualCohort })
                        .Select(q => new RiDataGroupBy
                        {
                            TreatyCode = q.Key.TreatyCode,
                            RiskPeriodMonth = q.Key.RiskPeriodMonth,
                            RiskPeriodYear = q.Key.RiskPeriodYear,
                            CurrencyCode = q.Key.CurrencyCode,
                            PremiumFrequencyCode = q.Key.PremiumFrequencyCode,
                            ContractCode = q.Key.Mfrs17TreatyCode,
                            AnnualCohort = q.Key.AnnualCohort,
                            Mfrs17CellName = q.Key.Mfrs17CellName,

                            TransactionDiscount = q.Sum(d => d.TransactionDiscount),
                            NoClaimBonus = q.Sum(d => d.NoClaimBonus),
                            SurrenderValue = q.Sum(d => d.SurrenderValue),
                            GstAmount = q.Sum(d => d.GstAmount),
                            DatabaseCommission = q.Sum(d => d.DatabaseCommision),
                            BrokerageFee = q.Sum(d => d.BrokerageFee),
                            ServiceFee = q.Sum(d => d.ServiceFee),
                        })
                        .OrderBy(q => q.TreatyCode)
                        .ThenBy(q => q.RiskPeriodMonth)
                        .ToList();
                else
                    return db.RiData
                        .Where(q => q.RiDataBatchId == riDataBatchId)
                        .GroupBy(q => new { q.TreatyCode, q.RiskPeriodMonth, q.RiskPeriodYear, q.CurrencyCode, q.PremiumFrequencyCode, q.Mfrs17CellName })
                        .Select(q => new RiDataGroupBy
                        {
                            TreatyCode = q.Key.TreatyCode,
                            RiskPeriodMonth = q.Key.RiskPeriodMonth,
                            RiskPeriodYear = q.Key.RiskPeriodYear,
                            CurrencyCode = q.Key.CurrencyCode,
                            PremiumFrequencyCode = q.Key.PremiumFrequencyCode,
                            Mfrs17CellName = q.Key.Mfrs17CellName,

                            TransactionDiscount = q.Sum(d => d.TransactionDiscount),
                            NoClaimBonus = q.Sum(d => d.NoClaimBonus),
                            SurrenderValue = q.Sum(d => d.SurrenderValue),
                            GstAmount = q.Sum(d => d.GstAmount),
                            DatabaseCommission = q.Sum(d => d.DatabaseCommision),
                            BrokerageFee = q.Sum(d => d.BrokerageFee),
                            ServiceFee = q.Sum(d => d.ServiceFee),
                        })
                        .OrderBy(q => q.TreatyCode)
                        .ThenBy(q => q.RiskPeriodMonth)
                        .ToList();
            }
            else
            {
                if (ifrs17)
                {
                    return db.RiData
                        .Where(q => q.RiDataBatchId == riDataBatchId)
                        .GroupBy(q => new { q.TreatyCode, q.RiskPeriodMonth, q.RiskPeriodYear, q.CurrencyCode, q.PremiumFrequencyCode, q.Mfrs17TreatyCode, AnnualCohort = q.Mfrs17AnnualCohort })
                        .Select(q => new RiDataGroupBy
                        {
                            TreatyCode = q.Key.TreatyCode,
                            RiskPeriodMonth = q.Key.RiskPeriodMonth,
                            RiskPeriodYear = q.Key.RiskPeriodYear,
                            CurrencyCode = q.Key.CurrencyCode,
                            PremiumFrequencyCode = q.Key.PremiumFrequencyCode,
                            ContractCode = q.Key.Mfrs17TreatyCode,
                            AnnualCohort = q.Key.AnnualCohort,

                            TransactionDiscount = q.Sum(d => d.TransactionDiscount),
                            NoClaimBonus = q.Sum(d => d.NoClaimBonus),
                            SurrenderValue = q.Sum(d => d.SurrenderValue),
                            GstAmount = q.Sum(d => d.GstAmount),
                            DatabaseCommission = q.Sum(d => d.DatabaseCommision),
                            BrokerageFee = q.Sum(d => d.BrokerageFee),
                            ServiceFee = q.Sum(d => d.ServiceFee),
                        })
                        .OrderBy(q => q.TreatyCode)
                        .ThenBy(q => q.RiskPeriodMonth)
                        .ToList();
                }                    
                else
                {
                    if (byFrequencyCode)
                        return db.RiData
                            .Where(q => q.RiDataBatchId == riDataBatchId)
                            .GroupBy(q => new { q.TreatyCode, q.RiskPeriodMonth, q.RiskPeriodYear, q.CurrencyCode, q.PremiumFrequencyCode })
                            .Select(q => new RiDataGroupBy
                            {
                                TreatyCode = q.Key.TreatyCode,
                                RiskPeriodMonth = q.Key.RiskPeriodMonth,
                                RiskPeriodYear = q.Key.RiskPeriodYear,
                                CurrencyCode = q.Key.CurrencyCode,
                                PremiumFrequencyCode = q.Key.PremiumFrequencyCode,

                                TransactionDiscount = q.Sum(d => d.TransactionDiscount),
                                NoClaimBonus = q.Sum(d => d.NoClaimBonus),
                                SurrenderValue = q.Sum(d => d.SurrenderValue),
                                GstAmount = q.Sum(d => d.GstAmount),
                                DatabaseCommission = q.Sum(d => d.DatabaseCommision),
                                BrokerageFee = q.Sum(d => d.BrokerageFee),
                                ServiceFee = q.Sum(d => d.ServiceFee),
                            })
                            .OrderBy(q => q.TreatyCode)
                            .ThenBy(q => q.RiskPeriodMonth)
                            .ToList();
                    else
                        return db.RiData
                            .Where(q => q.RiDataBatchId == riDataBatchId)
                            .GroupBy(q => new { q.TreatyCode, q.RiskPeriodMonth, q.RiskPeriodYear, q.CurrencyCode })
                            .Select(q => new RiDataGroupBy
                            {
                                TreatyCode = q.Key.TreatyCode,
                                RiskPeriodMonth = q.Key.RiskPeriodMonth,
                                RiskPeriodYear = q.Key.RiskPeriodYear,
                                CurrencyCode = q.Key.CurrencyCode,

                                TransactionDiscount = q.Sum(d => d.TransactionDiscount),
                                NoClaimBonus = q.Sum(d => d.NoClaimBonus),
                                SurrenderValue = q.Sum(d => d.SurrenderValue),
                                GstAmount = q.Sum(d => d.GstAmount),
                                DatabaseCommission = q.Sum(d => d.DatabaseCommision),
                                BrokerageFee = q.Sum(d => d.BrokerageFee),
                                ServiceFee = q.Sum(d => d.ServiceFee),
                            })
                            .OrderBy(q => q.TreatyCode)
                            .ThenBy(q => q.RiskPeriodMonth)
                            .ToList();
                }                    
            }
            
        }

        public List<RiDataGroupByTransactionType> QueryRiDataSumByTransactionType(AppDbContext db, string transactionType, bool ifrs17 = false, bool byCellName = false)
        {
            int riDataBatchId = GetRiDataBatchId();
            if (riDataBatchId == 0)
                return null;

            if (byCellName)
            {
                if (!ifrs17)
                    return db.RiData
                        .Where(q => q.RiDataBatchId == riDataBatchId)
                        .Where(q => q.TransactionTypeCode == transactionType)
                        .GroupBy(q => new { q.TreatyCode, q.RiskPeriodMonth, q.RiskPeriodYear, q.CurrencyCode, q.PremiumFrequencyCode, q.Mfrs17CellName })
                        .Select(q => new RiDataGroupByTransactionType
                        {
                            TreatyCode = q.Key.TreatyCode,
                            RiskPeriodMonth = q.Key.RiskPeriodMonth,
                            RiskPeriodYear = q.Key.RiskPeriodYear,
                            CurrencyCode = q.Key.CurrencyCode,
                            PremiumFrequencyCode = q.Key.PremiumFrequencyCode,
                            Mfrs17CellName = q.Key.Mfrs17CellName,

                            StandardPremium = q.Sum(d => d.StandardPremium),
                            SubstandardPremium = q.Sum(d => d.SubstandardPremium),
                            FlatExtraPremium = q.Sum(d => d.FlatExtraPremium),

                            StandardDiscount = q.Sum(d => d.StandardDiscount),
                            SubstandardDiscount = q.Sum(d => d.SubstandardDiscount),
                            TotalDiscount = q.Sum(d => d.TotalDiscount),

                            Aar = q.Sum(d => d.Aar),

                            TransactionPremium = q.Sum(d => d.TransactionPremium),
                            TransactionDiscount = q.Sum(d => d.TransactionDiscount),
                        })
                        .OrderBy(q => q.TreatyCode)
                        .ThenBy(q => q.RiskPeriodMonth)
                        .ToList();
                else
                    return db.RiData
                        .Where(q => q.RiDataBatchId == riDataBatchId)
                        .Where(q => q.TransactionTypeCode == transactionType)
                        .GroupBy(q => new { q.TreatyCode, q.RiskPeriodMonth, q.RiskPeriodYear, q.CurrencyCode, q.PremiumFrequencyCode, q.Mfrs17CellName, q.Mfrs17TreatyCode, AnnualCohort = q.Mfrs17AnnualCohort })
                        .Select(q => new RiDataGroupByTransactionType
                        {
                            TreatyCode = q.Key.TreatyCode,
                            RiskPeriodMonth = q.Key.RiskPeriodMonth,
                            RiskPeriodYear = q.Key.RiskPeriodYear,
                            CurrencyCode = q.Key.CurrencyCode,
                            PremiumFrequencyCode = q.Key.PremiumFrequencyCode,
                            Mfrs17CellName = q.Key.Mfrs17CellName,
                            ContractCode = q.Key.Mfrs17TreatyCode,
                            AnnualCohort = q.Key.AnnualCohort,

                            StandardPremium = q.Sum(d => d.StandardPremium),
                            SubstandardPremium = q.Sum(d => d.SubstandardPremium),
                            FlatExtraPremium = q.Sum(d => d.FlatExtraPremium),

                            StandardDiscount = q.Sum(d => d.StandardDiscount),
                            SubstandardDiscount = q.Sum(d => d.SubstandardDiscount),
                            TotalDiscount = q.Sum(d => d.TotalDiscount),

                            Aar = q.Sum(d => d.Aar),

                            TransactionPremium = q.Sum(d => d.TransactionPremium),
                            TransactionDiscount = q.Sum(d => d.TransactionDiscount),
                        })
                        .OrderBy(q => q.TreatyCode)
                        .ThenBy(q => q.RiskPeriodMonth)
                        .ToList();
            }
            else
            {
                if (!ifrs17)
                    return db.RiData
                        .Where(q => q.RiDataBatchId == riDataBatchId)
                        .Where(q => q.TransactionTypeCode == transactionType)
                        .GroupBy(q => new { q.TreatyCode, q.RiskPeriodMonth, q.RiskPeriodYear, q.CurrencyCode, q.PremiumFrequencyCode })
                        .Select(q => new RiDataGroupByTransactionType
                        {
                            TreatyCode = q.Key.TreatyCode,
                            RiskPeriodMonth = q.Key.RiskPeriodMonth,
                            RiskPeriodYear = q.Key.RiskPeriodYear,
                            CurrencyCode = q.Key.CurrencyCode,
                            PremiumFrequencyCode = q.Key.PremiumFrequencyCode,

                            StandardPremium = q.Sum(d => d.StandardPremium),
                            SubstandardPremium = q.Sum(d => d.SubstandardPremium),
                            FlatExtraPremium = q.Sum(d => d.FlatExtraPremium),

                            StandardDiscount = q.Sum(d => d.StandardDiscount),
                            SubstandardDiscount = q.Sum(d => d.SubstandardDiscount),
                            TotalDiscount = q.Sum(d => d.TotalDiscount),

                            Aar = q.Sum(d => d.Aar),

                            TransactionPremium = q.Sum(d => d.TransactionPremium),
                            TransactionDiscount = q.Sum(d => d.TransactionDiscount),
                        })
                        .OrderBy(q => q.TreatyCode)
                        .ThenBy(q => q.RiskPeriodMonth)
                        .ToList();
                else
                    return db.RiData
                        .Where(q => q.RiDataBatchId == riDataBatchId)
                        .Where(q => q.TransactionTypeCode == transactionType)
                        .GroupBy(q => new { q.TreatyCode, q.RiskPeriodMonth, q.RiskPeriodYear, q.CurrencyCode, q.PremiumFrequencyCode, q.Mfrs17TreatyCode, AnnualCohort = q.Mfrs17AnnualCohort })
                        .Select(q => new RiDataGroupByTransactionType
                        {
                            TreatyCode = q.Key.TreatyCode,
                            RiskPeriodMonth = q.Key.RiskPeriodMonth,
                            RiskPeriodYear = q.Key.RiskPeriodYear,
                            CurrencyCode = q.Key.CurrencyCode,
                            PremiumFrequencyCode = q.Key.PremiumFrequencyCode,
                            ContractCode = q.Key.Mfrs17TreatyCode,
                            AnnualCohort = q.Key.AnnualCohort,

                            StandardPremium = q.Sum(d => d.StandardPremium),
                            SubstandardPremium = q.Sum(d => d.SubstandardPremium),
                            FlatExtraPremium = q.Sum(d => d.FlatExtraPremium),

                            StandardDiscount = q.Sum(d => d.StandardDiscount),
                            SubstandardDiscount = q.Sum(d => d.SubstandardDiscount),
                            TotalDiscount = q.Sum(d => d.TotalDiscount),

                            Aar = q.Sum(d => d.Aar),

                            TransactionPremium = q.Sum(d => d.TransactionPremium),
                            TransactionDiscount = q.Sum(d => d.TransactionDiscount),
                        })
                        .OrderBy(q => q.TreatyCode)
                        .ThenBy(q => q.RiskPeriodMonth)
                        .ToList();
            }
            
        }

        public List<ClaimRegisterGroupBy> QueryClaimDataGroupBy(AppDbContext db, bool autoCreate = false, bool ifrs17 = false)
        {
            int claimDataBatchId = GetClaimDataBatchId();
            if (claimDataBatchId == 0)
                return null;
            if (autoCreate)
            {                
                if (ClaimDataBatchBo.Status == ClaimDataBatchBo.StatusReportedClaim)
                {
                    // Auto Create - if claim being reported, claim amount from Claim Register regardless of its claim status
                    return db.ClaimRegister
                         .Where(q => q.SoaDataBatchId == SoaDataBatchBo.Id)
                         .GroupBy(q => new { q.TreatyCode, q.SoaQuarter })
                         .Select(q => new ClaimRegisterGroupBy
                         {
                             TreatyCode = q.Key.TreatyCode,
                             SoaQuarter = q.Key.SoaQuarter,

                             ClaimRecoveryAmt = q.Sum(d => d.ClaimRecoveryAmt),
                             ForeignClaimRecoveryAmt = q.Sum(d => d.ForeignClaimRecoveryAmt),
                         })
                         .OrderBy(q => q.TreatyCode)
                         .ThenBy(q => q.SoaQuarter)
                         .ToList();
                }
                else
                {
                    // Auto Create - if the claim success in process claim amount from Claim Data.
                    return db.ClaimData
                        .Where(q => q.ClaimDataBatchId == claimDataBatchId)
                        .Where(q => q.ClaimDataBatch.Status == ClaimDataBatchBo.StatusSuccess)
                        .GroupBy(q => new { q.TreatyCode, q.SoaQuarter })
                        .Select(q => new ClaimRegisterGroupBy
                        {
                            TreatyCode = q.Key.TreatyCode,
                            SoaQuarter = q.Key.SoaQuarter,

                            ClaimRecoveryAmt = q.Sum(d => d.ClaimRecoveryAmt),
                            ForeignClaimRecoveryAmt = q.Sum(d => d.ForeignClaimRecoveryAmt),
                        })
                        .OrderBy(q => q.TreatyCode)
                        .ThenBy(q => q.SoaQuarter)
                        .ToList();
                }
            }
            else
            {
                // Compiled Summary - claim amount from Claim Register regardless of its claim status
                if (!ifrs17)
                    return db.ClaimRegister
                          .Where(q => q.SoaDataBatchId == SoaDataBatchBo.Id)
                          .GroupBy(q => new { q.TreatyCode, q.SoaQuarter })
                          .Select(q => new ClaimRegisterGroupBy
                          {
                              TreatyCode = q.Key.TreatyCode,
                              SoaQuarter = q.Key.SoaQuarter,

                              ClaimRecoveryAmt = q.Sum(d => d.ClaimRecoveryAmt),
                              ForeignClaimRecoveryAmt = q.Sum(d => d.ForeignClaimRecoveryAmt),
                          })
                          .OrderBy(q => q.TreatyCode)
                          .ThenBy(q => q.SoaQuarter)
                          .ToList();
                else
                    return db.ClaimRegister
                      .Where(q => q.SoaDataBatchId == SoaDataBatchBo.Id)
                      .GroupBy(q => new { q.TreatyCode, q.SoaQuarter, q.Mfrs17ContractCode, q.Mfrs17AnnualCohort })
                      .Select(q => new ClaimRegisterGroupBy
                      {
                          TreatyCode = q.Key.TreatyCode,
                          SoaQuarter = q.Key.SoaQuarter,
                          ContractCode = q.Key.Mfrs17ContractCode,
                          AnnualCohort = q.Key.Mfrs17AnnualCohort,

                          ClaimRecoveryAmt = q.Sum(d => d.ClaimRecoveryAmt),
                          ForeignClaimRecoveryAmt = q.Sum(d => d.ForeignClaimRecoveryAmt),
                      })
                      .OrderBy(q => q.TreatyCode)
                      .ThenBy(q => q.SoaQuarter)
                      .ToList();
            }
        }

        public List<ClaimRegisterGroupBy> QueryRiDataSummaryClaimDataGroupBy(AppDbContext db)
        {
            int claimDataBatchId = GetClaimDataBatchId();
            if (claimDataBatchId == 0)
                return null;
            if (SoaDataBatchBo.Status == SoaDataBatchBo.StatusProvisionalApproval)
            {
                return db.ClaimRegister
                    .Where(q => q.SoaDataBatchId == SoaDataBatchBo.Id)
                    .GroupBy(q => new { q.TreatyCode, q.SoaQuarter })
                    .Select(q => new ClaimRegisterGroupBy
                    {
                        TreatyCode = q.Key.TreatyCode,
                        SoaQuarter = q.Key.SoaQuarter,

                        ClaimRecoveryAmt = q.Sum(d => d.ClaimRecoveryAmt),
                        ForeignClaimRecoveryAmt = q.Sum(d => d.ForeignClaimRecoveryAmt),
                    })
                    .OrderBy(q => q.TreatyCode)
                    .ThenBy(q => q.SoaQuarter)
                    .ToList();
            }
            else
            {
                if (ClaimDataBatchBo.Status == ClaimDataBatchBo.StatusReportedClaim)
                {
                    return db.ClaimRegister
                        .Where(q => q.SoaDataBatchId == SoaDataBatchBo.Id)
                        .GroupBy(q => new { q.TreatyCode, q.SoaQuarter })
                        .Select(q => new ClaimRegisterGroupBy
                        {
                            TreatyCode = q.Key.TreatyCode,
                            SoaQuarter = q.Key.SoaQuarter,

                            ClaimRecoveryAmt = q.Sum(d => d.ClaimRecoveryAmt),
                            ForeignClaimRecoveryAmt = q.Sum(d => d.ForeignClaimRecoveryAmt),
                        })
                        .OrderBy(q => q.TreatyCode)
                        .ThenBy(q => q.SoaQuarter)
                        .ToList();
                }
                else                
                {
                    return db.ClaimData
                        .Where(q => q.ClaimDataBatchId == claimDataBatchId)
                        .Where(q => q.ClaimDataBatch.Status == ClaimDataBatchBo.StatusSuccess)
                        .GroupBy(q => new { q.TreatyCode, q.SoaQuarter })
                        .Select(q => new ClaimRegisterGroupBy
                        {
                            TreatyCode = q.Key.TreatyCode,
                            SoaQuarter = q.Key.SoaQuarter,

                            ClaimRecoveryAmt = q.Sum(d => d.ClaimRecoveryAmt),
                            ForeignClaimRecoveryAmt = q.Sum(d => d.ForeignClaimRecoveryAmt),
                        })
                        .OrderBy(q => q.TreatyCode)
                        .ThenBy(q => q.SoaQuarter)
                        .ToList();
                }
            }
        }

        public List<RiDataPostValidation> QueryRiDataPostValidationBenefitCode(AppDbContext db, string benefitCode, bool ifrs17 = false, bool byCellName = false, bool byFrequencyCode = true)
        {
            int riDataBatchId = GetRiDataBatchId();
            if (riDataBatchId == 0)
                return null;

            // Get MLRe Benefit Code by Valuation Benefit Code
            var bo = PickListDetailService.FindByPickListIdCode(PickListBo.ValuationBenefitCode, benefitCode);
            if (bo == null)
                return null;

            List<string> BenefitCodes = db.Benefits.Where(q => q.ValuationBenefitCodePickListDetailId == bo.Id).Select(q => q.Code).ToList();
            if (byCellName)
            {
                if (!ifrs17)
                    return db.RiData
                        .Where(q => q.RiDataBatchId == riDataBatchId)
                        .Where(q => BenefitCodes.Contains(q.MlreBenefitCode))
                        .GroupBy(q => new { q.TreatyCode, q.RiskPeriodMonth, q.RiskPeriodYear, q.CurrencyCode, q.PremiumFrequencyCode, q.Mfrs17CellName })
                        .Select(q => new RiDataPostValidation
                        {
                            TreatyCode = q.Key.TreatyCode,
                            RiskPeriodMonth = q.Key.RiskPeriodMonth,
                            RiskPeriodYear = q.Key.RiskPeriodYear,
                            CurrencyCode = q.Key.CurrencyCode,
                            Mfrs17CellName = q.Key.Mfrs17CellName,
                            PremiumFrequencyCode = q.Key.PremiumFrequencyCode,

                            TransactionPremium = q.Sum(d => d.TransactionPremium),
                            MlreGrossPremium = q.Sum(d => d.MlreGrossPremium),
                        })
                        .OrderBy(q => q.TreatyCode)
                        .ThenBy(q => q.RiskPeriodMonth)
                        .ToList();
                else
                    return db.RiData
                        .Where(q => q.RiDataBatchId == riDataBatchId)
                        .Where(q => BenefitCodes.Contains(q.MlreBenefitCode))
                        .GroupBy(q => new { q.TreatyCode, q.RiskPeriodMonth, q.RiskPeriodYear, q.CurrencyCode, q.PremiumFrequencyCode, q.Mfrs17CellName, q.Mfrs17TreatyCode, AnnualCohort = q.Mfrs17AnnualCohort })
                        .Select(q => new RiDataPostValidation
                        {
                            TreatyCode = q.Key.TreatyCode,
                            RiskPeriodMonth = q.Key.RiskPeriodMonth,
                            RiskPeriodYear = q.Key.RiskPeriodYear,
                            CurrencyCode = q.Key.CurrencyCode,
                            Mfrs17CellName = q.Key.Mfrs17CellName,
                            ContractCode = q.Key.Mfrs17TreatyCode,
                            AnnualCohort = q.Key.AnnualCohort,
                            PremiumFrequencyCode = q.Key.PremiumFrequencyCode,

                            TransactionPremium = q.Sum(d => d.TransactionPremium),
                            MlreGrossPremium = q.Sum(d => d.MlreGrossPremium),
                        })
                        .OrderBy(q => q.TreatyCode)
                        .ThenBy(q => q.RiskPeriodMonth)
                        .ToList();
            }
            else
            { 
                if (ifrs17)
                {
                    return db.RiData
                        .Where(q => q.RiDataBatchId == riDataBatchId)
                        .Where(q => BenefitCodes.Contains(q.MlreBenefitCode))
                        .GroupBy(q => new { q.TreatyCode, q.RiskPeriodMonth, q.RiskPeriodYear, q.CurrencyCode, q.PremiumFrequencyCode, q.Mfrs17TreatyCode, AnnualCohort = q.Mfrs17AnnualCohort })
                        .Select(q => new RiDataPostValidation
                        {
                            TreatyCode = q.Key.TreatyCode,
                            RiskPeriodMonth = q.Key.RiskPeriodMonth,
                            RiskPeriodYear = q.Key.RiskPeriodYear,
                            CurrencyCode = q.Key.CurrencyCode,
                            ContractCode = q.Key.Mfrs17TreatyCode,
                            AnnualCohort = q.Key.AnnualCohort,
                            PremiumFrequencyCode = q.Key.PremiumFrequencyCode,

                            TransactionPremium = q.Sum(d => d.TransactionPremium),
                            MlreGrossPremium = q.Sum(d => d.MlreGrossPremium),
                        })
                        .OrderBy(q => q.TreatyCode)
                        .ThenBy(q => q.RiskPeriodMonth)
                        .ToList();
                }
                else
                {
                    if (byFrequencyCode)
                    {
                            return db.RiData
                                .Where(q => q.RiDataBatchId == riDataBatchId)
                                .Where(q => BenefitCodes.Contains(q.MlreBenefitCode))
                                .GroupBy(q => new { q.TreatyCode, q.RiskPeriodMonth, q.RiskPeriodYear, q.CurrencyCode, q.PremiumFrequencyCode })
                                .Select(q => new RiDataPostValidation
                                {
                                    TreatyCode = q.Key.TreatyCode,
                                    RiskPeriodMonth = q.Key.RiskPeriodMonth,
                                    RiskPeriodYear = q.Key.RiskPeriodYear,
                                    CurrencyCode = q.Key.CurrencyCode,
                                    PremiumFrequencyCode = q.Key.PremiumFrequencyCode,

                                    TransactionPremium = q.Sum(d => d.TransactionPremium),
                                    MlreGrossPremium = q.Sum(d => d.MlreGrossPremium),
                                })
                                .OrderBy(q => q.TreatyCode)
                                .ThenBy(q => q.RiskPeriodMonth)
                                .ToList();
                    }
                    else
                    {
                            return db.RiData
                                .Where(q => q.RiDataBatchId == riDataBatchId)
                                .Where(q => BenefitCodes.Contains(q.MlreBenefitCode))
                                .GroupBy(q => new { q.TreatyCode, q.RiskPeriodMonth, q.RiskPeriodYear, q.CurrencyCode })
                                .Select(q => new RiDataPostValidation
                                {
                                    TreatyCode = q.Key.TreatyCode,
                                    RiskPeriodMonth = q.Key.RiskPeriodMonth,
                                    RiskPeriodYear = q.Key.RiskPeriodYear,
                                    CurrencyCode = q.Key.CurrencyCode,

                                    TransactionPremium = q.Sum(d => d.TransactionPremium),
                                    MlreGrossPremium = q.Sum(d => d.MlreGrossPremium),
                                })
                                .OrderBy(q => q.TreatyCode)
                                .ThenBy(q => q.RiskPeriodMonth)
                                .ToList();
                        }
                }
            }
            
        }

        public List<RiDataPostValidationTransactionType> QueryRiDataPostValidationTransactionType(AppDbContext db, string transactionType)
        {
            int riDataBatchId = GetRiDataBatchId();
            if (riDataBatchId == 0)
                return null;
            return db.RiData
                .Where(q => q.RiDataBatchId == riDataBatchId)
                .Where(q => q.TransactionTypeCode == transactionType)
                .GroupBy(q => new { q.TreatyCode, q.RiskPeriodMonth, q.RiskPeriodYear, q.CurrencyCode })
                .Select(q => new RiDataPostValidationTransactionType
                {
                    TreatyCode = q.Key.TreatyCode,
                    RiskPeriodMonth = q.Key.RiskPeriodMonth,
                    RiskPeriodYear = q.Key.RiskPeriodYear,
                    CurrencyCode = q.Key.CurrencyCode,

                    StandardPremium = q.Sum(d => d.StandardPremium),
                    SubstandardPremium = q.Sum(d => d.SubstandardPremium),
                    FlatExtraPremium = q.Sum(d => d.FlatExtraPremium),

                    StandardDiscount = q.Sum(d => d.StandardDiscount),
                    SubstandardDiscount = q.Sum(d => d.SubstandardDiscount),
                    TotalDiscount = q.Sum(d => d.TotalDiscount),

                    MlreStandardPremium = q.Sum(d => d.MlreStandardPremium),
                    MlreSubstandardPremium = q.Sum(d => d.MlreSubstandardPremium),
                    MlreFlatExtraPremium = q.Sum(d => d.MlreFlatExtraPremium),
                    MlreGrossPremium = q.Sum(d => d.MlreGrossPremium),

                    MlreStandardDiscount = q.Sum(d => d.MlreStandardDiscount),
                    MlreSubstandardDiscount = q.Sum(d => d.MlreSubstandardDiscount),
                    MlreTotalDiscount = q.Sum(d => d.MlreTotalDiscount),

                    Layer1StandardPremium = q.Sum(d => d.Layer1StandardPremium),
                    Layer1SubstandardPremium = q.Sum(d => d.Layer1SubstandardPremium),
                    Layer1FlatExtraPremium = q.Sum(d => d.Layer1FlatExtraPremium),
                    Layer1GrossPremium = q.Sum(d => d.Layer1GrossPremium),

                    Layer1StandardDiscount = q.Sum(d => d.Layer1StandardDiscount),
                    Layer1SubstandardDiscount = q.Sum(d => d.Layer1SubstandardDiscount),
                    Layer1TotalDiscount = q.Sum(d => d.Layer1TotalDiscount),

                    Aar = q.Sum(d => d.Aar),
                    TransactionPremium = q.Sum(d => d.TransactionPremium),
                    TransactionDiscount = q.Sum(d => d.TransactionDiscount),

                    Total = q.Count(),
                })
                .OrderBy(q => q.TreatyCode)
                .ThenBy(q => q.RiskPeriodMonth)
                .ToList();
        }

        public List<RiDataPostValidationTransactionType> QueryCountRiDataPostValidationTransactionType(AppDbContext db, string transactionType, bool monthly = true, bool ifrs17 = false, bool byCellName = false)
        {
            List<int> quarterEndMonth = new List<int> { 3, 6, 9, 12 };

            int riDataBatchId = GetRiDataBatchId();
            if (riDataBatchId == 0)
                return null;
            var query = db.RiData
                .Where(q => q.RiDataBatchId == riDataBatchId)
                .Where(q => q.TransactionTypeCode == transactionType);

            if (monthly)
            {
                if (SoaDataBatchBo == null)
                    return null;

                query = query
                    .Where(q => q.PremiumFrequencyCode == PickListDetailBo.PremiumFrequencyCodeMonthly)
                    .Where(q => q.RiskPeriodMonth.HasValue && quarterEndMonth.Contains(q.RiskPeriodMonth.Value));
            }
            else
            {
                query = query
                    .Where(q => q.PremiumFrequencyCode != PickListDetailBo.PremiumFrequencyCodeMonthly);
            }

            if (byCellName)
            {
                if (!ifrs17)
                    return query
                        .GroupBy(q => new { q.TreatyCode, q.RiskPeriodMonth, q.RiskPeriodYear, q.CurrencyCode, q.PremiumFrequencyCode, q.Mfrs17CellName })
                        .Select(q => new RiDataPostValidationTransactionType
                        {
                            TreatyCode = q.Key.TreatyCode,
                            RiskPeriodMonth = q.Key.RiskPeriodMonth,
                            RiskPeriodYear = q.Key.RiskPeriodYear,
                            CurrencyCode = q.Key.CurrencyCode,
                            Mfrs17CellName = q.Key.Mfrs17CellName,
                            PremiumFrequencyCode = q.Key.PremiumFrequencyCode,

                            Total = q.Count(),
                        })
                        .OrderBy(q => q.TreatyCode)
                        .ThenBy(q => q.RiskPeriodMonth)
                        .ToList();
                else
                    return query
                        .GroupBy(q => new { q.TreatyCode, q.RiskPeriodMonth, q.RiskPeriodYear, q.CurrencyCode, q.PremiumFrequencyCode, q.Mfrs17CellName, q.Mfrs17TreatyCode, AnnualCohort = q.Mfrs17AnnualCohort })
                        .Select(q => new RiDataPostValidationTransactionType
                        {
                            TreatyCode = q.Key.TreatyCode,
                            RiskPeriodMonth = q.Key.RiskPeriodMonth,
                            RiskPeriodYear = q.Key.RiskPeriodYear,
                            CurrencyCode = q.Key.CurrencyCode,
                            Mfrs17CellName = q.Key.Mfrs17CellName,
                            ContractCode = q.Key.Mfrs17TreatyCode,
                            AnnualCohort = q.Key.AnnualCohort,
                            PremiumFrequencyCode = q.Key.PremiumFrequencyCode,

                            Total = q.Count(),
                        })
                        .OrderBy(q => q.TreatyCode)
                        .ThenBy(q => q.RiskPeriodMonth)
                        .ToList();
            }
            else
            {
                if (!ifrs17)
                    return query
                        .GroupBy(q => new { q.TreatyCode, q.RiskPeriodMonth, q.RiskPeriodYear, q.CurrencyCode, q.PremiumFrequencyCode })
                        .Select(q => new RiDataPostValidationTransactionType
                        {
                            TreatyCode = q.Key.TreatyCode,
                            RiskPeriodMonth = q.Key.RiskPeriodMonth,
                            RiskPeriodYear = q.Key.RiskPeriodYear,
                            CurrencyCode = q.Key.CurrencyCode,
                            PremiumFrequencyCode = q.Key.PremiumFrequencyCode,

                            Total = q.Count(),
                        })
                        .OrderBy(q => q.TreatyCode)
                        .ThenBy(q => q.RiskPeriodMonth)
                        .ToList();
                else
                    return query
                        .GroupBy(q => new { q.TreatyCode, q.RiskPeriodMonth, q.RiskPeriodYear, q.CurrencyCode, q.PremiumFrequencyCode, q.Mfrs17TreatyCode, AnnualCohort = q.Mfrs17AnnualCohort })
                        .Select(q => new RiDataPostValidationTransactionType
                        {
                            TreatyCode = q.Key.TreatyCode,
                            RiskPeriodMonth = q.Key.RiskPeriodMonth,
                            RiskPeriodYear = q.Key.RiskPeriodYear,
                            CurrencyCode = q.Key.CurrencyCode,
                            ContractCode = q.Key.Mfrs17TreatyCode,
                            AnnualCohort = q.Key.AnnualCohort,
                            PremiumFrequencyCode = q.Key.PremiumFrequencyCode,

                            Total = q.Count(),
                        })
                        .OrderBy(q => q.TreatyCode)
                        .ThenBy(q => q.RiskPeriodMonth)
                        .ToList();
            }
            
        }

        public List<RiDataPostValidationTransactionType> QueryRiDataDiscrepancySum(AppDbContext db)
        {
            int riDataBatchId = GetRiDataBatchId();
            if (riDataBatchId == 0)
                return null;
            return db.RiData
                .Where(q => q.RiDataBatchId == riDataBatchId)
                .Where(q => q.TransactionTypeCode == PickListDetailBo.TransactionTypeCodeNewBusiness)
                .GroupBy(q => new { q.TreatyCode, q.CedingPlanCode, q.CurrencyCode })
                .Select(q => new RiDataPostValidationTransactionType
                {
                    TreatyCode = q.Key.TreatyCode,
                    CedingPlanCode = q.Key.CedingPlanCode,
                    CurrencyCode = q.Key.CurrencyCode,

                    MlreGrossPremium = q.Sum(d => d.MlreGrossPremium),
                    TransactionPremium = q.Sum(d => d.TransactionPremium),

                    StandardPremium = q.Sum(d => d.StandardPremium),
                    SubstandardPremium = q.Sum(d => d.SubstandardPremium),
                    FlatExtraPremium = q.Sum(d => d.FlatExtraPremium),

                    MlreStandardPremium = q.Sum(d => d.MlreStandardPremium),
                    MlreSubstandardPremium = q.Sum(d => d.MlreSubstandardPremium),
                    MlreFlatExtraPremium = q.Sum(d => d.MlreFlatExtraPremium),

                    Layer1StandardPremium = q.Sum(d => d.Layer1StandardPremium),
                    Layer1SubstandardPremium = q.Sum(d => d.Layer1SubstandardPremium),
                    Layer1FlatExtraPremium = q.Sum(d => d.Layer1FlatExtraPremium),
                })
                .OrderBy(q => q.TreatyCode)
                .ThenBy(q => q.CedingPlanCode)
                .ToList();
        }

        public List<RiDataGroupBy> QueryRiDataGroupByAutoCreate(AppDbContext db)
        {
            int riDataBatchId = GetRiDataBatchId();
            if (riDataBatchId == 0)
                return null;
            return db.RiData
                .Where(q => q.RiDataBatchId == riDataBatchId)
                .GroupBy(q => new { q.TreatyCode, q.RiskPeriodMonth, q.RiskPeriodYear, q.PremiumFrequencyCode, q.TreatyType, q.CurrencyCode })
                .Select(q => new RiDataGroupBy
                {
                    TreatyCode = q.Key.TreatyCode,
                    RiskPeriodMonth = q.Key.RiskPeriodMonth,
                    RiskPeriodYear = q.Key.RiskPeriodYear,
                    TransactionDiscount = q.Sum(d => d.TransactionDiscount),
                    NoClaimBonus = q.Sum(d => d.NoClaimBonus),
                    SurrenderValue = q.Sum(d => d.SurrenderValue),
                    GstAmount = q.Sum(d => d.GstAmount),
                    DatabaseCommission = q.Sum(d => d.DatabaseCommision),
                    BrokerageFee = q.Sum(d => d.BrokerageFee),

                    PremiumFrequencyCode = q.Key.PremiumFrequencyCode,
                    TreatyType = q.Key.TreatyType,
                    CurrencyCode = q.Key.CurrencyCode,
                })
                .OrderBy(q => q.TreatyCode)
                .ThenBy(q => q.RiskPeriodMonth)
                .ToList();
        }

        // Create Soa Data from Claim Data
        public List<ClaimRegisterGroupBy> QueryClaimDataGroupByAutoCreate(AppDbContext db)
        {
            int claimDataBatchId = GetClaimDataBatchId();
            if (claimDataBatchId == 0)
                return null;
            if (ClaimDataBatchBo.Status == ClaimDataBatchBo.StatusReportedClaim)
            {
                // Auto Create - if claim being reported, claim amount from Claim Register regardless of its claim status
                return db.ClaimRegister
                     .Where(q => q.SoaDataBatchId == SoaDataBatchBo.Id)
                     .GroupBy(q => new { q.TreatyCode, q.TreatyType, q.RiskQuarter })
                     .Select(q => new ClaimRegisterGroupBy
                     {
                         TreatyCode = q.Key.TreatyCode,
                         TreatyType = q.Key.TreatyType,
                         RiskQuarter = q.Key.RiskQuarter,

                         ClaimRecoveryAmt = q.Sum(d => d.ClaimRecoveryAmt),
                         ForeignClaimRecoveryAmt = q.Sum(d => d.ForeignClaimRecoveryAmt),
                     })
                     .OrderBy(q => q.TreatyCode)
                     .ThenBy(q => q.RiskQuarter)
                     .ToList();
            }
            else
            {
                // Auto Create - if the claim success in process claim amount from Claim Data.
                return db.ClaimData
                    .Where(q => q.ClaimDataBatchId == claimDataBatchId)
                    .Where(q => q.ClaimDataBatch.Status == ClaimDataBatchBo.StatusSuccess)
                    .GroupBy(q => new { q.TreatyCode, q.TreatyType, q.RiskQuarter })
                     .Select(q => new ClaimRegisterGroupBy
                     {
                         TreatyCode = q.Key.TreatyCode,
                         TreatyType = q.Key.TreatyType,
                         RiskQuarter = q.Key.RiskQuarter,

                         ClaimRecoveryAmt = q.Sum(d => d.ClaimRecoveryAmt),
                         ForeignClaimRecoveryAmt = q.Sum(d => d.ForeignClaimRecoveryAmt),
                     })
                     .OrderBy(q => q.TreatyCode)
                     .ThenBy(q => q.RiskQuarter)
                     .ToList();
            }
        }

        public List<RiSummaryGroupBy> QueryRiSummaryGroupBy()
        {
            return SoaDataRiDataSummaryBos
                .Where(q => q.Type == SoaDataRiDataSummaryBo.TypeRiDataSummaryIfrs4)
                .GroupBy(q => new { q.TreatyCode, q.RiskQuarter, q.CurrencyCode, q.CurrencyRate, q.Frequency })
                .Select(q => new RiSummaryGroupBy
                {
                    TreatyCode = q.Key.TreatyCode,
                    RiskQuarter = q.Key.RiskQuarter,

                    NbPremium = q.Sum(d => d.NbPremium),
                    RnPremium = q.Sum(d => d.RnPremium),
                    AltPremium = q.Sum(d => d.AltPremium),

                    NbDiscount = q.Sum(d => d.NbDiscount),
                    RnDiscount = q.Sum(d => d.RnDiscount),
                    AltDiscount = q.Sum(d => d.AltDiscount),

                    NbCession = q.Sum(d => d.NbCession),
                    RnCession = q.Sum(d => d.RnCession),
                    AltCession = q.Sum(d => d.AltCession),

                    NbSar = q.Sum(d => d.NbSar),
                    RnSar = q.Sum(d => d.RnSar),
                    AltSar = q.Sum(d => d.AltSar),

                    DTH = q.Sum(d => d.DTH),
                    TPA = q.Sum(d => d.TPA),
                    TPS = q.Sum(d => d.TPS),
                    PPD = q.Sum(d => d.PPD),
                    CCA = q.Sum(d => d.CCA),
                    CCS = q.Sum(d => d.CCS),
                    PA = q.Sum(d => d.PA),
                    HS = q.Sum(d => d.HS),
                    TPD = q.Sum(d => d.TPD),
                    CI = q.Sum(d => d.CI),

                    NoClaimBonus = q.Sum(d => d.NoClaimBonus),
                    SurrenderValue = q.Sum(d => d.SurrenderValue),
                    BrokerageFee = q.Sum(d => d.BrokerageFee),
                    DatabaseCommission = q.Sum(d => d.DatabaseCommission),
                    ServiceFee = q.Sum(d => d.ServiceFee),

                    CurrencyCode = q.Key.CurrencyCode,
                    CurrencyRate = q.Key.CurrencyRate,
                    Frequency = q.Key.Frequency,
                })
                .OrderBy(q => q.TreatyCode)
                .ThenBy(q => q.RiskQuarter)
                .ToList();
        }

        public List<RiSummaryGroupBy> QueryRiSummaryGroupByEndDate()
        {
            List<int> quarterEndMonth = new List<int> { 3,6,9,12 };
            return SoaDataRiDataSummaryBos
                .Where(q => q.Type == SoaDataRiDataSummaryBo.TypeRiDataSummaryIfrs4 && (q.RiskMonth.HasValue && quarterEndMonth.Contains(q.RiskMonth.Value)))
                .GroupBy(q => new { q.TreatyCode, q.RiskQuarter, q.CurrencyCode, q.CurrencyRate, q.Frequency })
                .Select(q => new RiSummaryGroupBy
                {
                    TreatyCode = q.Key.TreatyCode,
                    RiskQuarter = q.Key.RiskQuarter,

                    NbPremium = q.Sum(d => d.NbPremium),
                    RnPremium = q.Sum(d => d.RnPremium),
                    AltPremium = q.Sum(d => d.AltPremium),

                    NbDiscount = q.Sum(d => d.NbDiscount),
                    RnDiscount = q.Sum(d => d.RnDiscount),
                    AltDiscount = q.Sum(d => d.AltDiscount),

                    NbCession = q.Sum(d => d.NbCession),
                    RnCession = q.Sum(d => d.RnCession),
                    AltCession = q.Sum(d => d.AltCession),

                    NbSar = q.Sum(d => d.NbSar),
                    RnSar = q.Sum(d => d.RnSar),
                    AltSar = q.Sum(d => d.AltSar),

                    DTH = q.Sum(d => d.DTH),
                    TPA = q.Sum(d => d.TPA),
                    TPS = q.Sum(d => d.TPS),
                    PPD = q.Sum(d => d.PPD),
                    CCA = q.Sum(d => d.CCA),
                    CCS = q.Sum(d => d.CCS),
                    PA = q.Sum(d => d.PA),
                    HS = q.Sum(d => d.HS),
                    TPD = q.Sum(d => d.TPD),
                    CI = q.Sum(d => d.CI),

                    NoClaimBonus = q.Sum(d => d.NoClaimBonus),
                    SurrenderValue = q.Sum(d => d.SurrenderValue),
                    BrokerageFee = q.Sum(d => d.BrokerageFee),
                    DatabaseCommission = q.Sum(d => d.DatabaseCommission),
                    ServiceFee = q.Sum(d => d.ServiceFee),

                    CurrencyCode = q.Key.CurrencyCode,
                    CurrencyRate = q.Key.CurrencyRate,
                    Frequency = q.Key.Frequency,
                })
                .OrderBy(q => q.TreatyCode)
                .ThenBy(q => q.RiskQuarter)
                .ToList();
        }

        public List<RiSummaryGroupBy> QueryRiSummaryIfrs17GroupBy(bool groupBySp = false)
        {
            if (!groupBySp)
                return SoaDataRiDataSummaryBos
                    .Where(q => q.Type == SoaDataRiDataSummaryBo.TypeRiDataSummaryIfrs17)
                    .GroupBy(q => new { q.TreatyCode, q.RiskQuarter, q.CurrencyCode, q.CurrencyRate, q.Frequency, q.ContractCode })
                    .Select(q => new RiSummaryGroupBy
                    {
                        TreatyCode = q.Key.TreatyCode,
                        RiskQuarter = q.Key.RiskQuarter,
                        CurrencyCode = q.Key.CurrencyCode,
                        CurrencyRate = q.Key.CurrencyRate,
                        Frequency = q.Key.Frequency,
                        ContractCode = q.Key.ContractCode,

                        NbPremium = q.Sum(d => d.NbPremium),
                        RnPremium = q.Sum(d => d.RnPremium),
                        AltPremium = q.Sum(d => d.AltPremium),

                        NbDiscount = q.Sum(d => d.NbDiscount),
                        RnDiscount = q.Sum(d => d.RnDiscount),
                        AltDiscount = q.Sum(d => d.AltDiscount),

                        NbCession = q.Sum(d => d.NbCession),
                        RnCession = q.Sum(d => d.RnCession),
                        AltCession = q.Sum(d => d.AltCession),

                        NbSar = q.Sum(d => d.NbSar),
                        RnSar = q.Sum(d => d.RnSar),
                        AltSar = q.Sum(d => d.AltSar),

                        DTH = q.Sum(d => d.DTH),
                        TPA = q.Sum(d => d.TPA),
                        TPS = q.Sum(d => d.TPS),
                        PPD = q.Sum(d => d.PPD),
                        CCA = q.Sum(d => d.CCA),
                        CCS = q.Sum(d => d.CCS),
                        PA = q.Sum(d => d.PA),
                        HS = q.Sum(d => d.HS),
                        TPD = q.Sum(d => d.TPD),
                        CI = q.Sum(d => d.CI),

                        NoClaimBonus = q.Sum(d => d.NoClaimBonus),
                        SurrenderValue = q.Sum(d => d.SurrenderValue),
                        BrokerageFee = q.Sum(d => d.BrokerageFee),
                        DatabaseCommission = q.Sum(d => d.DatabaseCommission),
                        ServiceFee = q.Sum(d => d.ServiceFee),
                    })
                    .OrderBy(q => q.TreatyCode)
                    .ThenBy(q => q.RiskQuarter)
                    .ToList();
            else
                return SoaDataRiDataSummaryBos
                   .Where(q => q.Type == SoaDataRiDataSummaryBo.TypeRiDataSummaryIfrs17)
                   .GroupBy(q => new { q.TreatyCode, q.RiskQuarter, q.CurrencyCode, q.CurrencyRate, q.Frequency, q.ContractCode, q.AnnualCohort })
                   .Select(q => new RiSummaryGroupBy
                   {
                       TreatyCode = q.Key.TreatyCode,
                       RiskQuarter = q.Key.RiskQuarter,
                       CurrencyCode = q.Key.CurrencyCode,
                       CurrencyRate = q.Key.CurrencyRate,
                       Frequency = q.Key.Frequency,
                       ContractCode = q.Key.ContractCode,
                       AnnualCohort = q.Key.AnnualCohort,

                       NbPremium = q.Sum(d => d.NbPremium),
                       RnPremium = q.Sum(d => d.RnPremium),
                       AltPremium = q.Sum(d => d.AltPremium),

                       NbDiscount = q.Sum(d => d.NbDiscount),
                       RnDiscount = q.Sum(d => d.RnDiscount),
                       AltDiscount = q.Sum(d => d.AltDiscount),

                       NbCession = q.Sum(d => d.NbCession),
                       RnCession = q.Sum(d => d.RnCession),
                       AltCession = q.Sum(d => d.AltCession),

                       NbSar = q.Sum(d => d.NbSar),
                       RnSar = q.Sum(d => d.RnSar),
                       AltSar = q.Sum(d => d.AltSar),

                       DTH = q.Sum(d => d.DTH),
                       TPA = q.Sum(d => d.TPA),
                       TPS = q.Sum(d => d.TPS),
                       PPD = q.Sum(d => d.PPD),
                       CCA = q.Sum(d => d.CCA),
                       CCS = q.Sum(d => d.CCS),
                       PA = q.Sum(d => d.PA),
                       HS = q.Sum(d => d.HS),
                       TPD = q.Sum(d => d.TPD),
                       CI = q.Sum(d => d.CI),

                       NoClaimBonus = q.Sum(d => d.NoClaimBonus),
                       SurrenderValue = q.Sum(d => d.SurrenderValue),
                       BrokerageFee = q.Sum(d => d.BrokerageFee),
                       DatabaseCommission = q.Sum(d => d.DatabaseCommission),
                       ServiceFee = q.Sum(d => d.ServiceFee),
                   })
                   .OrderBy(q => q.TreatyCode)
                   .ThenBy(q => q.RiskQuarter)
                   .ToList();
        }

        public List<RiSummaryGroupBy> QueryRiSummaryIfrs17GroupByEndDate(bool groupBySp = false)
        {
            List<int> quarterEndMonth = new List<int> { 3, 6, 9, 12 };
            if (!groupBySp)
                return SoaDataRiDataSummaryBos
                    .Where(q => q.Type == SoaDataRiDataSummaryBo.TypeRiDataSummaryIfrs17 && (q.RiskMonth.HasValue && quarterEndMonth.Contains(q.RiskMonth.Value)))
                    .GroupBy(q => new { q.TreatyCode, q.RiskQuarter, q.CurrencyCode, q.CurrencyRate, q.Frequency, q.ContractCode })
                    .Select(q => new RiSummaryGroupBy
                    {
                        TreatyCode = q.Key.TreatyCode,
                        RiskQuarter = q.Key.RiskQuarter,
                        CurrencyCode = q.Key.CurrencyCode,
                        CurrencyRate = q.Key.CurrencyRate,
                        Frequency = q.Key.Frequency,
                        ContractCode = q.Key.ContractCode,

                        NbPremium = q.Sum(d => d.NbPremium),
                        RnPremium = q.Sum(d => d.RnPremium),
                        AltPremium = q.Sum(d => d.AltPremium),

                        NbDiscount = q.Sum(d => d.NbDiscount),
                        RnDiscount = q.Sum(d => d.RnDiscount),
                        AltDiscount = q.Sum(d => d.AltDiscount),

                        NbCession = q.Sum(d => d.NbCession),
                        RnCession = q.Sum(d => d.RnCession),
                        AltCession = q.Sum(d => d.AltCession),

                        NbSar = q.Sum(d => d.NbSar),
                        RnSar = q.Sum(d => d.RnSar),
                        AltSar = q.Sum(d => d.AltSar),

                        DTH = q.Sum(d => d.DTH),
                        TPA = q.Sum(d => d.TPA),
                        TPS = q.Sum(d => d.TPS),
                        PPD = q.Sum(d => d.PPD),
                        CCA = q.Sum(d => d.CCA),
                        CCS = q.Sum(d => d.CCS),
                        PA = q.Sum(d => d.PA),
                        HS = q.Sum(d => d.HS),
                        TPD = q.Sum(d => d.TPD),
                        CI = q.Sum(d => d.CI),

                        NoClaimBonus = q.Sum(d => d.NoClaimBonus),
                        SurrenderValue = q.Sum(d => d.SurrenderValue),
                        BrokerageFee = q.Sum(d => d.BrokerageFee),
                        DatabaseCommission = q.Sum(d => d.DatabaseCommission),
                        ServiceFee = q.Sum(d => d.ServiceFee),
                    })
                    .OrderBy(q => q.TreatyCode)
                    .ThenBy(q => q.RiskQuarter)
                    .ToList();
            else
                return SoaDataRiDataSummaryBos
               .Where(q => q.Type == SoaDataRiDataSummaryBo.TypeRiDataSummaryIfrs17 && (q.RiskMonth.HasValue && quarterEndMonth.Contains(q.RiskMonth.Value)))
               .GroupBy(q => new { q.TreatyCode, q.RiskQuarter, q.CurrencyCode, q.CurrencyRate, q.Frequency, q.ContractCode, q.AnnualCohort })
               .Select(q => new RiSummaryGroupBy
               {
                   TreatyCode = q.Key.TreatyCode,
                   RiskQuarter = q.Key.RiskQuarter,
                   CurrencyCode = q.Key.CurrencyCode,
                   CurrencyRate = q.Key.CurrencyRate,
                   Frequency = q.Key.Frequency,
                   ContractCode = q.Key.ContractCode,
                   AnnualCohort = q.Key.AnnualCohort,

                   NbPremium = q.Sum(d => d.NbPremium),
                   RnPremium = q.Sum(d => d.RnPremium),
                   AltPremium = q.Sum(d => d.AltPremium),

                   NbDiscount = q.Sum(d => d.NbDiscount),
                   RnDiscount = q.Sum(d => d.RnDiscount),
                   AltDiscount = q.Sum(d => d.AltDiscount),

                   NbCession = q.Sum(d => d.NbCession),
                   RnCession = q.Sum(d => d.RnCession),
                   AltCession = q.Sum(d => d.AltCession),

                   NbSar = q.Sum(d => d.NbSar),
                   RnSar = q.Sum(d => d.RnSar),
                   AltSar = q.Sum(d => d.AltSar),

                   DTH = q.Sum(d => d.DTH),
                   TPA = q.Sum(d => d.TPA),
                   TPS = q.Sum(d => d.TPS),
                   PPD = q.Sum(d => d.PPD),
                   CCA = q.Sum(d => d.CCA),
                   CCS = q.Sum(d => d.CCS),
                   PA = q.Sum(d => d.PA),
                   HS = q.Sum(d => d.HS),
                   TPD = q.Sum(d => d.TPD),
                   CI = q.Sum(d => d.CI),

                   NoClaimBonus = q.Sum(d => d.NoClaimBonus),
                   SurrenderValue = q.Sum(d => d.SurrenderValue),
                   BrokerageFee = q.Sum(d => d.BrokerageFee),
                   DatabaseCommission = q.Sum(d => d.DatabaseCommission),
                   ServiceFee = q.Sum(d => d.ServiceFee),
               })
               .OrderBy(q => q.TreatyCode)
               .ThenBy(q => q.RiskQuarter)
               .ToList();
        }

        public List<ClaimRegisterGroupBy> QueryRiDataSummaryIfrs17ClaimDataGroupBy(AppDbContext db)
        {
            int claimDataBatchId = GetClaimDataBatchId();
            if (claimDataBatchId == 0)
                return null;
            if (SoaDataBatchBo.Status == SoaDataBatchBo.StatusProvisionalApproval)
            {
                return db.ClaimRegister
                    .Where(q => q.SoaDataBatchId == SoaDataBatchBo.Id)
                    .GroupBy(q => new { q.TreatyCode, q.SoaQuarter, q.Mfrs17ContractCode, q.Mfrs17AnnualCohort })
                    .Select(q => new ClaimRegisterGroupBy
                    {
                        TreatyCode = q.Key.TreatyCode,
                        SoaQuarter = q.Key.SoaQuarter,
                        ContractCode = q.Key.Mfrs17ContractCode,
                        AnnualCohort = q.Key.Mfrs17AnnualCohort,

                        ClaimRecoveryAmt = q.Sum(d => d.ClaimRecoveryAmt),
                        ForeignClaimRecoveryAmt = q.Sum(d => d.ForeignClaimRecoveryAmt),
                    })
                    .OrderBy(q => q.TreatyCode)
                    .ThenBy(q => q.SoaQuarter)
                    .ToList();
            }
            else
            {
                if (ClaimDataBatchBo.Status == ClaimDataBatchBo.StatusReportedClaim)
                {
                    return db.ClaimRegister
                        .Where(q => q.SoaDataBatchId == SoaDataBatchBo.Id)
                        .GroupBy(q => new { q.TreatyCode, q.SoaQuarter, q.Mfrs17ContractCode, q.Mfrs17AnnualCohort })
                        .Select(q => new ClaimRegisterGroupBy
                        {
                            TreatyCode = q.Key.TreatyCode,
                            SoaQuarter = q.Key.SoaQuarter,
                            ContractCode = q.Key.Mfrs17ContractCode,
                            AnnualCohort = q.Key.Mfrs17AnnualCohort,

                            ClaimRecoveryAmt = q.Sum(d => d.ClaimRecoveryAmt),
                            ForeignClaimRecoveryAmt = q.Sum(d => d.ForeignClaimRecoveryAmt),
                        })
                        .OrderBy(q => q.TreatyCode)
                        .ThenBy(q => q.SoaQuarter)
                        .ToList();
                }
                else
                {
                    return db.ClaimData
                        .Where(q => q.ClaimDataBatchId == claimDataBatchId)
                        .Where(q => q.ClaimDataBatch.Status == ClaimDataBatchBo.StatusSuccess)
                        .GroupBy(q => new { q.TreatyCode, q.SoaQuarter, q.Mfrs17ContractCode, q.Mfrs17AnnualCohort })
                        .Select(q => new ClaimRegisterGroupBy
                        {
                            TreatyCode = q.Key.TreatyCode,
                            SoaQuarter = q.Key.SoaQuarter,
                            ContractCode = q.Key.Mfrs17ContractCode,
                            AnnualCohort = q.Key.Mfrs17AnnualCohort,

                            ClaimRecoveryAmt = q.Sum(d => d.ClaimRecoveryAmt),
                            ForeignClaimRecoveryAmt = q.Sum(d => d.ForeignClaimRecoveryAmt),
                        })
                        .OrderBy(q => q.TreatyCode)
                        .ThenBy(q => q.SoaQuarter)
                        .ToList();
                }
            }
        }

        public List<RiSummaryGroupBy> QueryRiSummaryGroupByCellName(bool groupBySp = false)
        {
            if (!groupBySp)
                return SoaDataRiDataSummaryBos
                    .Where(q => q.Type == SoaDataRiDataSummaryBo.TypeRiDataSummaryCNIfrs17)
                    .GroupBy(q => new { q.TreatyCode, q.RiskQuarter, q.CurrencyCode, q.CurrencyRate, q.Frequency, q.Mfrs17CellName, q.ContractCode })
                    .Select(q => new RiSummaryGroupBy
                    {
                        TreatyCode = q.Key.TreatyCode,
                        RiskQuarter = q.Key.RiskQuarter,
                        CurrencyCode = q.Key.CurrencyCode,
                        CurrencyRate = q.Key.CurrencyRate,
                        Frequency = q.Key.Frequency,
                        ContractCode = q.Key.ContractCode,
                        Mfrs17CellName = q.Key.Mfrs17CellName,

                        NbPremium = q.Sum(d => d.NbPremium),
                        RnPremium = q.Sum(d => d.RnPremium),
                        AltPremium = q.Sum(d => d.AltPremium),

                        NbDiscount = q.Sum(d => d.NbDiscount),
                        RnDiscount = q.Sum(d => d.RnDiscount),
                        AltDiscount = q.Sum(d => d.AltDiscount),

                        NbCession = q.Sum(d => d.NbCession),
                        RnCession = q.Sum(d => d.RnCession),
                        AltCession = q.Sum(d => d.AltCession),

                        NbSar = q.Sum(d => d.NbSar),
                        RnSar = q.Sum(d => d.RnSar),
                        AltSar = q.Sum(d => d.AltSar),

                        DTH = q.Sum(d => d.DTH),
                        TPA = q.Sum(d => d.TPA),
                        TPS = q.Sum(d => d.TPS),
                        PPD = q.Sum(d => d.PPD),
                        CCA = q.Sum(d => d.CCA),
                        CCS = q.Sum(d => d.CCS),
                        PA = q.Sum(d => d.PA),
                        HS = q.Sum(d => d.HS),
                        TPD = q.Sum(d => d.TPD),
                        CI = q.Sum(d => d.CI),

                        NoClaimBonus = q.Sum(d => d.NoClaimBonus),
                        SurrenderValue = q.Sum(d => d.SurrenderValue),
                        BrokerageFee = q.Sum(d => d.BrokerageFee),
                        DatabaseCommission = q.Sum(d => d.DatabaseCommission),
                        ServiceFee = q.Sum(d => d.ServiceFee),
                    })
                    .OrderBy(q => q.TreatyCode)
                    .ThenBy(q => q.RiskQuarter)
                    .ToList();
            else
                return SoaDataRiDataSummaryBos
                   .Where(q => q.Type == SoaDataRiDataSummaryBo.TypeRiDataSummaryCNIfrs17)
                   .GroupBy(q => new { q.TreatyCode, q.RiskQuarter, q.CurrencyCode, q.CurrencyRate, q.Frequency, q.Mfrs17CellName, q.ContractCode, q.AnnualCohort })
                   .Select(q => new RiSummaryGroupBy
                   {
                       TreatyCode = q.Key.TreatyCode,
                       RiskQuarter = q.Key.RiskQuarter,
                       CurrencyCode = q.Key.CurrencyCode,
                       CurrencyRate = q.Key.CurrencyRate,
                       Frequency = q.Key.Frequency,
                       ContractCode = q.Key.ContractCode,
                       AnnualCohort = q.Key.AnnualCohort,
                       Mfrs17CellName = q.Key.Mfrs17CellName,

                       NbPremium = q.Sum(d => d.NbPremium),
                       RnPremium = q.Sum(d => d.RnPremium),
                       AltPremium = q.Sum(d => d.AltPremium),

                       NbDiscount = q.Sum(d => d.NbDiscount),
                       RnDiscount = q.Sum(d => d.RnDiscount),
                       AltDiscount = q.Sum(d => d.AltDiscount),

                       NbCession = q.Sum(d => d.NbCession),
                       RnCession = q.Sum(d => d.RnCession),
                       AltCession = q.Sum(d => d.AltCession),

                       NbSar = q.Sum(d => d.NbSar),
                       RnSar = q.Sum(d => d.RnSar),
                       AltSar = q.Sum(d => d.AltSar),

                       DTH = q.Sum(d => d.DTH),
                       TPA = q.Sum(d => d.TPA),
                       TPS = q.Sum(d => d.TPS),
                       PPD = q.Sum(d => d.PPD),
                       CCA = q.Sum(d => d.CCA),
                       CCS = q.Sum(d => d.CCS),
                       PA = q.Sum(d => d.PA),
                       HS = q.Sum(d => d.HS),
                       TPD = q.Sum(d => d.TPD),
                       CI = q.Sum(d => d.CI),

                       NoClaimBonus = q.Sum(d => d.NoClaimBonus),
                       SurrenderValue = q.Sum(d => d.SurrenderValue),
                       BrokerageFee = q.Sum(d => d.BrokerageFee),
                       DatabaseCommission = q.Sum(d => d.DatabaseCommission),
                       ServiceFee = q.Sum(d => d.ServiceFee),
                   })
                   .OrderBy(q => q.TreatyCode)
                   .ThenBy(q => q.RiskQuarter)
                   .ToList();
        }

        public List<RiSummaryGroupBy> QueryRiSummaryGroupByEndDateCellName(bool groupBySp = false)
        {
            List<int> quarterEndMonth = new List<int> { 3, 6, 9, 12 };
            if (!groupBySp)
                return SoaDataRiDataSummaryBos
                    .Where(q => q.Type == SoaDataRiDataSummaryBo.TypeRiDataSummaryCNIfrs17 && (q.RiskMonth.HasValue && quarterEndMonth.Contains(q.RiskMonth.Value)))
                    .GroupBy(q => new { q.TreatyCode, q.RiskQuarter, q.CurrencyCode, q.CurrencyRate, q.Frequency, q.Mfrs17CellName, q.ContractCode })
                    .Select(q => new RiSummaryGroupBy
                    {
                        TreatyCode = q.Key.TreatyCode,
                        RiskQuarter = q.Key.RiskQuarter,
                        CurrencyCode = q.Key.CurrencyCode,
                        CurrencyRate = q.Key.CurrencyRate,
                        Frequency = q.Key.Frequency,
                        ContractCode = q.Key.ContractCode,
                        Mfrs17CellName = q.Key.Mfrs17CellName,

                        NbPremium = q.Sum(d => d.NbPremium),
                        RnPremium = q.Sum(d => d.RnPremium),
                        AltPremium = q.Sum(d => d.AltPremium),

                        NbDiscount = q.Sum(d => d.NbDiscount),
                        RnDiscount = q.Sum(d => d.RnDiscount),
                        AltDiscount = q.Sum(d => d.AltDiscount),

                        NbCession = q.Sum(d => d.NbCession),
                        RnCession = q.Sum(d => d.RnCession),
                        AltCession = q.Sum(d => d.AltCession),

                        NbSar = q.Sum(d => d.NbSar),
                        RnSar = q.Sum(d => d.RnSar),
                        AltSar = q.Sum(d => d.AltSar),

                        DTH = q.Sum(d => d.DTH),
                        TPA = q.Sum(d => d.TPA),
                        TPS = q.Sum(d => d.TPS),
                        PPD = q.Sum(d => d.PPD),
                        CCA = q.Sum(d => d.CCA),
                        CCS = q.Sum(d => d.CCS),
                        PA = q.Sum(d => d.PA),
                        HS = q.Sum(d => d.HS),
                        TPD = q.Sum(d => d.TPD),
                        CI = q.Sum(d => d.CI),

                        NoClaimBonus = q.Sum(d => d.NoClaimBonus),
                        SurrenderValue = q.Sum(d => d.SurrenderValue),
                        BrokerageFee = q.Sum(d => d.BrokerageFee),
                        DatabaseCommission = q.Sum(d => d.DatabaseCommission),
                        ServiceFee = q.Sum(d => d.ServiceFee),
                    })
                    .OrderBy(q => q.TreatyCode)
                    .ThenBy(q => q.RiskQuarter)
                    .ToList();
            else
                return SoaDataRiDataSummaryBos
               .Where(q => q.Type == SoaDataRiDataSummaryBo.TypeRiDataSummaryCNIfrs17 && (q.RiskMonth.HasValue && quarterEndMonth.Contains(q.RiskMonth.Value)))
               .GroupBy(q => new { q.TreatyCode, q.RiskQuarter, q.CurrencyCode, q.CurrencyRate, q.Frequency, q.Mfrs17CellName, q.ContractCode, q.AnnualCohort })
               .Select(q => new RiSummaryGroupBy
               {
                   TreatyCode = q.Key.TreatyCode,
                   RiskQuarter = q.Key.RiskQuarter,
                   CurrencyCode = q.Key.CurrencyCode,
                   CurrencyRate = q.Key.CurrencyRate,
                   Frequency = q.Key.Frequency,
                   ContractCode = q.Key.ContractCode,
                   AnnualCohort = q.Key.AnnualCohort,
                   Mfrs17CellName = q.Key.Mfrs17CellName,

                   NbPremium = q.Sum(d => d.NbPremium),
                   RnPremium = q.Sum(d => d.RnPremium),
                   AltPremium = q.Sum(d => d.AltPremium),

                   NbDiscount = q.Sum(d => d.NbDiscount),
                   RnDiscount = q.Sum(d => d.RnDiscount),
                   AltDiscount = q.Sum(d => d.AltDiscount),

                   NbCession = q.Sum(d => d.NbCession),
                   RnCession = q.Sum(d => d.RnCession),
                   AltCession = q.Sum(d => d.AltCession),

                   NbSar = q.Sum(d => d.NbSar),
                   RnSar = q.Sum(d => d.RnSar),
                   AltSar = q.Sum(d => d.AltSar),

                   DTH = q.Sum(d => d.DTH),
                   TPA = q.Sum(d => d.TPA),
                   TPS = q.Sum(d => d.TPS),
                   PPD = q.Sum(d => d.PPD),
                   CCA = q.Sum(d => d.CCA),
                   CCS = q.Sum(d => d.CCS),
                   PA = q.Sum(d => d.PA),
                   HS = q.Sum(d => d.HS),
                   TPD = q.Sum(d => d.TPD),
                   CI = q.Sum(d => d.CI),

                   NoClaimBonus = q.Sum(d => d.NoClaimBonus),
                   SurrenderValue = q.Sum(d => d.SurrenderValue),
                   BrokerageFee = q.Sum(d => d.BrokerageFee),
                   DatabaseCommission = q.Sum(d => d.DatabaseCommission),
                   ServiceFee = q.Sum(d => d.ServiceFee),
               })
               .OrderBy(q => q.TreatyCode)
               .ThenBy(q => q.RiskQuarter)
               .ToList();
        }

        public string GetQuarterInfo(int? month, int? year)
        {
            string qtrString = "";

            if (month.HasValue && year.HasValue)
            {
                string quarter = "";
                if (month <= 3)
                    quarter = "Q1";
                else if (month > 3 && month <= 6)
                    quarter = "Q2";
                else if (month > 6 && month <= 9)
                    quarter = "Q3";
                else if (month > 9 && month <= 12)
                    quarter = "Q4";

                qtrString = FormatQuarter(string.Format("{0} {1}", year, quarter));
                //qtrString = string.Format("{0}{1}", (year % 100), quarter);
                //qtrString = string.Format("{0} {1}", year, quarter);
            }
            return qtrString;
        }

        public string FormatQuarter(string quarter)
        {
            string qtrString = "";
            if (!string.IsNullOrEmpty(quarter))
            {
                string[] q = quarter.Split(' ');
                qtrString = string.Format("{0}{1}", (int.Parse(q[0]) % 100), q[1]);
            }
            return qtrString;
        }
    }
}
