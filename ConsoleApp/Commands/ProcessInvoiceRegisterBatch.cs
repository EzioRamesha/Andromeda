using BusinessObject;
using BusinessObject.Identity;
using BusinessObject.InvoiceRegisters;
using BusinessObject.SoaDatas;
using DataAccess.Entities.Identity;
using DataAccess.EntityFramework;
using Services;
using Services.Identity;
using Services.InvoiceRegisters;
using Services.SoaDatas;
using Shared;
using Shared.DataAccess;
using Shared.ProcessFile;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;

namespace ConsoleApp.Commands
{
    public class ProcessInvoiceRegisterBatch : Command
    {
        public InvoiceRegisterBatchBo InvoiceRegisterBatchBo { get; set; }

        // IFRS4
        public IList<SoaDataCompiledSummaryBo> SoaDataCompiledSummaryOriginalCurrencyIfrs4Bos { get; set; }
        public IList<SoaDataCompiledSummaryBo> SoaDataCompiledSummaryMyrCurrenyIfrs4Bos { get; set; }

        // IFRS17
        public IList<SoaDataCompiledSummaryBo> SoaDataCompiledSummaryOriginalCurrencyIfrs17Bos { get; set; }
        public IList<SoaDataCompiledSummaryBo> SoaDataCompiledSummaryMyrCurrenyIfrs17Bos { get; set; }

        // IFRS17 (by MFRS17 Cell Name)
        public IList<SoaDataCompiledSummaryBo> SoaDataCompiledSummaryCNOriginalCurrencyIfrs17Bos { get; set; }
        public IList<SoaDataCompiledSummaryBo> SoaDataCompiledSummaryCNMyrCurrenyIfrs17Bos { get; set; }

        public ModuleBo ModuleBo { get; set; }

        public StatusHistoryBo ProcessingStatusHistoryBo { get; set; }

        public InvoiceRegisterBatchStatusFileBo InvoiceRegisterBatchStatusFileBo { get; set; }

        public int TotalRecord { get; set; } = 0;

        public string StatusLogFileFilePath { get; set; }

        public ProcessInvoiceRegisterBatch()
        {
            Title = "ProcessInvoiceRegisterBatch";
            Description = "To process Soa Data Compiled Summary for Invoice Register";
        }

        public override bool Validate()
        {
            try
            {
                // nothing
            }
            catch (Exception e)
            {
                PrintError(e.Message);
                return false;
            }
            return base.Validate();
        }

        public override void Initial()
        {
            ModuleBo = ModuleService.FindByController(ModuleBo.ModuleController.InvoiceRegister.ToString());
            CommitLimit = 500;
        }

        public override void Run()
        {
            try
            {
                if (CutOffService.IsCutOffProcessing())
                {
                    Log = false;
                    PrintMessage(MessageBag.ProcessCannotRunDueToCutOff, true, false);
                    return;
                }
                if (InvoiceRegisterBatchService.CountByStatus(InvoiceRegisterBatchBo.StatusSubmitForProcessing) == 0)
                {
                    Log = false;
                    PrintMessage(MessageBag.NoBatchPendingProcess);
                    return;
                }

                PrintStarting();

                while (LoadInvoiceRegisterBatchBo() != null)
                {                    
                    try
                    {
                        TotalRecord = 0;

                        if (GetProcessCount("Process") > 0)
                            PrintProcessCount();
                        SetProcessCount("Process");

                        UpdateStatus(InvoiceRegisterBatchBo.StatusProcessing, "Processing Invoice Register Batch");

                        CreateStatusLogFile();

                        if (File.Exists(StatusLogFileFilePath))
                            File.Delete(StatusLogFileFilePath);

                        // DELETE PREVIOUS INVOICE REGISTER
                        PrintMessage("Deleting Invoice Register...", true, false);
                        DeleteInvoiceRegister();
                        PrintMessage("Deleted Invoice Register", true, false);

                        CreateInvoiceRegister();

                        UpdateStatus(InvoiceRegisterBatchBo.StatusSuccess, "Successfully Processed Invoice Register Batch");
                        WriteStatusLogFile("Successfully Processed Invoice Register Batch");
                        //UpdateSoaDataBatchRetroStatus();
                    }
                    catch (Exception e)
                    {
                        var message = e.Message;
                        if (e is DbEntityValidationException dbEx)
                            message = Util.CatchDbEntityValidationException(dbEx).ToString();

                        WriteStatusLogFile(message, true);
                        UpdateStatus(InvoiceRegisterBatchBo.StatusFailed, "Failed to Process Invoice Register Batch");
                        WriteStatusLogFile("Failed to Process Invoice Register Batch");
                        PrintError(message);
                    }
                }
                if (GetProcessCount("Process") > 0)
                    PrintProcessCount();

                PrintEnding();
            }
            catch (Exception e)
            {
                PrintMessage(e.ToString());
                PrintError(e.Message);
            }
        }

        public void CreateInvoiceRegister()
        {
            WriteStatusLogFile("Starting Create Invoice Register...", true);
            WriteStatusLogFile("Starting Compute Soa Data Compiled Summary", true);

            Process();
                        
            WriteStatusLogFile("Completed Compute Soa Data Compiled Summary", true);
            WriteStatusLogFile("Completed Create Invoice Register...", true);
        }

        public void Process()
        {
            WriteStatusLogFile(string.Format("Total Soa Data Compiled Summary IFRS4 (MYR Currency) to be compute: {0}", (SoaDataCompiledSummaryMyrCurrenyIfrs4Bos == null ? 0 : SoaDataCompiledSummaryMyrCurrenyIfrs4Bos.Count)), true);
            WriteStatusLogFile(string.Format("Total Soa Data Compiled Summary IFRS4 (Original Currency) to be compute: {0}", (SoaDataCompiledSummaryOriginalCurrencyIfrs4Bos == null ? 0 : SoaDataCompiledSummaryOriginalCurrencyIfrs4Bos.Count)), true);
            WriteStatusLogFile(string.Format("Total Soa Data Compiled Summary IFRS17 (MYR Currency) to be compute: {0}", (SoaDataCompiledSummaryMyrCurrenyIfrs17Bos == null ? 0 : SoaDataCompiledSummaryMyrCurrenyIfrs17Bos.Count)), true);
            WriteStatusLogFile(string.Format("Total Soa Data Compiled Summary IFRS17 (Original Currency) to be compute: {0}", (SoaDataCompiledSummaryOriginalCurrencyIfrs17Bos == null ? 0 : SoaDataCompiledSummaryOriginalCurrencyIfrs17Bos.Count)), true);

            WriteStatusLogFile(string.Format("Total Soa Data Compiled Summary IFRS17 by MFRS17 Cell Name (MYR Currency) to be compute: {0}", (SoaDataCompiledSummaryCNMyrCurrenyIfrs17Bos == null ? 0 : SoaDataCompiledSummaryCNMyrCurrenyIfrs17Bos.Count)), true);
            WriteStatusLogFile(string.Format("Total Soa Data Compiled Summary IFRS17 by MFRS17 Cell Name (Original Currency) to be compute: {0}", (SoaDataCompiledSummaryCNOriginalCurrencyIfrs17Bos == null ? 0 : SoaDataCompiledSummaryCNMyrCurrenyIfrs17Bos.Count)), true);

            if (SoaDataCompiledSummaryMyrCurrenyIfrs4Bos != null)
            {
                int total = SoaDataCompiledSummaryMyrCurrenyIfrs4Bos.Count;
                foreach (var bo in SoaDataCompiledSummaryMyrCurrenyIfrs4Bos)
                {
                    var invoiceRegisters = new List<InvoiceRegisterBo> { };

                    var invoiceMyrCurrencyIfrs4 = ProcessIfrs4(bo, InvoiceRegisterBo.ReportingTypeIFRS4);
                    invoiceRegisters.Add(invoiceMyrCurrencyIfrs4);

                    if (!SoaDataCompiledSummaryOriginalCurrencyIfrs4Bos.IsNullOrEmpty())
                    {
                        var soaDataCompiledSummaryOriginalCurrencyIfrs4 = SoaDataCompiledSummaryOriginalCurrencyIfrs4Bos?.Where(q => q.TreatyCode == bo.TreatyCode && q.BusinessCode == bo.BusinessCode && q.InvoiceType == bo.InvoiceType && q.RiskQuarter == bo.RiskQuarter && q.SoaQuarter == bo.SoaQuarter && q.CurrencyCode != PickListDetailBo.CurrencyCodeMyr && q.Frequency == bo.Frequency).FirstOrDefault();
                        if (soaDataCompiledSummaryOriginalCurrencyIfrs4 != null)
                        {
                            var invoiceOriginalCurrencyIfrs4 = ProcessIfrs4(soaDataCompiledSummaryOriginalCurrencyIfrs4, InvoiceRegisterBo.ReportingTypeIFRS4);
                            invoiceRegisters.Add(invoiceOriginalCurrencyIfrs4);
                        }
                    }

                    if (!SoaDataCompiledSummaryMyrCurrenyIfrs17Bos.IsNullOrEmpty())
                    {
                        var soaDataCompiledSummaryMyrCurrenyIfrs17 = SoaDataCompiledSummaryMyrCurrenyIfrs17Bos?.Where(q => q.TreatyCode == bo.TreatyCode && q.BusinessCode == bo.BusinessCode && q.InvoiceType == bo.InvoiceType && q.RiskQuarter == bo.RiskQuarter && q.SoaQuarter == bo.SoaQuarter && q.CurrencyCode == PickListDetailBo.CurrencyCodeMyr && q.Frequency == bo.Frequency).ToList();
                        if (!soaDataCompiledSummaryMyrCurrenyIfrs17.IsNullOrEmpty())
                        {
                            var invoiceMyrCurrencyIfrs17 = ProcessIfrs17(soaDataCompiledSummaryMyrCurrenyIfrs17, InvoiceRegisterBo.ReportingTypeIFRS17);
                            invoiceRegisters.AddRange(invoiceMyrCurrencyIfrs17);
                        }
                    }

                    if (!SoaDataCompiledSummaryOriginalCurrencyIfrs17Bos.IsNullOrEmpty())
                    {
                        var soaDataCompiledSummaryOriginalCurrencyIfrs17 = SoaDataCompiledSummaryOriginalCurrencyIfrs17Bos?.Where(q => q.TreatyCode == bo.TreatyCode && q.BusinessCode == bo.BusinessCode && q.InvoiceType == bo.InvoiceType && q.RiskQuarter == bo.RiskQuarter && q.SoaQuarter == bo.SoaQuarter && q.CurrencyCode != PickListDetailBo.CurrencyCodeMyr && q.Frequency == bo.Frequency).ToList();
                        if (!soaDataCompiledSummaryOriginalCurrencyIfrs17.IsNullOrEmpty())
                        {
                            var invoiceOriginalCurrencyIfrs17 = ProcessIfrs17(soaDataCompiledSummaryOriginalCurrencyIfrs17, InvoiceRegisterBo.ReportingTypeIFRS17);
                            invoiceRegisters.AddRange(invoiceOriginalCurrencyIfrs17);
                        }
                    }

                    // data grouped by MFRS17 Cell Name
                    if (!SoaDataCompiledSummaryCNMyrCurrenyIfrs17Bos.IsNullOrEmpty())
                    {
                        var soaDataCompiledSummaryCNMyrCurrenyIfrs17Bos = SoaDataCompiledSummaryCNMyrCurrenyIfrs17Bos?.Where(q => q.TreatyCode == bo.TreatyCode && q.BusinessCode == bo.BusinessCode && q.InvoiceType == bo.InvoiceType && q.RiskQuarter == bo.RiskQuarter && q.SoaQuarter == bo.SoaQuarter && q.CurrencyCode == PickListDetailBo.CurrencyCodeMyr && q.Frequency == bo.Frequency).ToList();
                        if (!soaDataCompiledSummaryCNMyrCurrenyIfrs17Bos.IsNullOrEmpty())
                        {
                            var invoiceCNMyrCurrencyIfrs17 = ProcessIfrs17(soaDataCompiledSummaryCNMyrCurrenyIfrs17Bos, InvoiceRegisterBo.ReportingTypeCNIFRS17);
                            invoiceRegisters.AddRange(invoiceCNMyrCurrencyIfrs17);
                        }
                    }

                    if (!SoaDataCompiledSummaryCNOriginalCurrencyIfrs17Bos.IsNullOrEmpty())
                    {
                        var soaDataCompiledSummaryCNOriginalCurrencyIfrs17Bos = SoaDataCompiledSummaryCNOriginalCurrencyIfrs17Bos?.Where(q => q.TreatyCode == bo.TreatyCode && q.BusinessCode == bo.BusinessCode && q.InvoiceType == bo.InvoiceType && q.RiskQuarter == bo.RiskQuarter && q.SoaQuarter == bo.SoaQuarter && q.CurrencyCode != PickListDetailBo.CurrencyCodeMyr && q.Frequency == bo.Frequency).ToList();
                        if (!soaDataCompiledSummaryCNOriginalCurrencyIfrs17Bos.IsNullOrEmpty())
                        {
                            var invoiceCNOriginalCurrencyIfrs17 = ProcessIfrs17(soaDataCompiledSummaryCNOriginalCurrencyIfrs17Bos, InvoiceRegisterBo.ReportingTypeCNIFRS17);
                            invoiceRegisters.AddRange(invoiceCNOriginalCurrencyIfrs17);
                        }
                    }

                    var invoiceReference = GenerateReferenceNo(InvoiceRegisterBatchBo.BatchDate.Year);
                    foreach (var invoiceRegister in invoiceRegisters)
                    {
                        var ir = invoiceRegister;
                        ir.InvoiceDate = InvoiceRegisterBatchBo.BatchDate;
                        ir.InvoiceReference = invoiceReference;
                        ir.CreatedById = InvoiceRegisterBatchBo.CreatedById;
                        ir.UpdatedById = InvoiceRegisterBatchBo.UpdatedById;
                        InvoiceRegisterService.Create(ref ir);
                    }

                    TotalRecord++;
                    //WriteInvoiceRegisterSummary(b);
                }
                SetProcessCount("Saved");
            }
        }

        public InvoiceRegisterBo ProcessIfrs4(SoaDataCompiledSummaryBo bo, int reportingType)
        {
            var invoiceRegisterBo = new InvoiceRegisterBo()
            {
                InvoiceRegisterBatchId = InvoiceRegisterBatchBo.Id,
                InvoiceType = bo.InvoiceType,
                InvoiceNumber = "",
                StatementReceivedDate = bo.StatementReceivedDate,
                CedantId = bo.SoaDataBatchBo.CedantId,
                RiskQuarter = bo.RiskQuarter,
                AccountDescription = bo.AccountDescription,

                Year1st = bo.NbPremium,
                Gross1st = bo.NbPremium,
                GrossRenewal = bo.RnPremium,
                AltPremium = bo.AltPremium,
                Discount1st = bo.NbDiscount,
                DiscountRen = bo.RnDiscount,
                DiscountAlt = bo.AltDiscount,

                NBCession = bo.NbCession,
                NBSumReins = bo.NbSar,
                RNCession = bo.RnCession,
                RNSumReins = bo.RnSar,
                AltCession = bo.AltCession,
                AltSumReins = bo.AltSar,

                DTH = bo.DTH,
                TPA = bo.TPA,
                TPS = bo.TPS,
                PPD = bo.PPD,
                CCA = bo.CCA,
                CCS = bo.CCS,
                PA = bo.PA,
                HS = bo.HS,
                TPD = bo.TPD,
                CI = bo.CI,

                RiskPremium = bo.RiskPremium,
                NoClaimBonus = bo.NoClaimBonus,
                Levy = bo.Levy,
                Claim = bo.Claim,
                ProfitComm = bo.ProfitComm,
                SurrenderValue = bo.SurrenderValue,
                Gst = bo.Gst,
                ModcoReserveIncome = bo.ModcoReserveIncome,
                ReinsDeposit = bo.RiDeposit,
                DatabaseCommission = bo.DatabaseCommission,
                AdministrationContribution = bo.AdministrationContribution,
                ShareOfRiCommissionFromCompulsoryCession = bo.ShareOfRiCommissionFromCompulsoryCession,
                RecaptureFee = bo.RecaptureFee,
                CreditCardCharges = bo.CreditCardCharges,
                BrokerageFee = bo.BrokerageFee,

                Remark = "",
                CurrencyCode = bo.CurrencyCode,
                CurrencyRate = bo.CurrencyRate,
                SoaQuarter = bo.SoaQuarter,

                ReasonOfAdjustment1 = bo.ReasonOfAdjustment1,
                ReasonOfAdjustment2 = bo.ReasonOfAdjustment2,
                InvoiceNumber1 = bo.InvoiceNumber1,
                InvoiceDate1 = bo.InvoiceDate1,
                Amount1 = bo.Amount1,
                InvoiceNumber2 = bo.InvoiceNumber2,
                InvoiceDate2 = bo.InvoiceDate2,
                Amount2 = bo.Amount2,

                Frequency = bo.Frequency,
                PaymentReference = null,
                PaymentAmount = null,
                PaymentReceivedDate = null,
                PreparedById = InvoiceRegisterBatchBo.CreatedById,
                ValuationMode = bo.Frequency,

                SoaDataBatchId = bo.SoaDataBatchId,
                ContractCode = bo.ContractCode,
                AnnualCohort = bo.AnnualCohort,
                Mfrs17CellName = bo.Mfrs17CellName,
                ReportingType = reportingType,
            };
            invoiceRegisterBo.GetRenewal();
            invoiceRegisterBo.GetTotalPaid();
            invoiceRegisterBo.GetValuationGross1st();
            invoiceRegisterBo.GetValuationGrossRen();
            invoiceRegisterBo.GetValuationDiscount1st();
            invoiceRegisterBo.GetValuationDiscountRen();
            invoiceRegisterBo.GetValuationCom1st();
            invoiceRegisterBo.GetValuationComRen();
            invoiceRegisterBo.GetValuationProfitCom();
            invoiceRegisterBo.GetValuationRiskPremium();
            invoiceRegisterBo.GetValuationClaims();

            var treatyCode = TreatyCodeService.FindByCedantIdCode(bo.SoaDataBatchBo.CedantId, bo.TreatyCode);
            if (treatyCode != null)
                invoiceRegisterBo.TreatyCodeId = treatyCode.Id;

            if (string.IsNullOrEmpty(bo.Frequency))
            {
                invoiceRegisterBo.Frequency = GetPremiumFrequencyCode(bo.SoaDataBatchBo.RiDataBatchId, bo.TreatyCode);
                invoiceRegisterBo.ValuationMode = GetPremiumFrequencyCode(bo.SoaDataBatchBo.RiDataBatchId, bo.TreatyCode);
            }

            return invoiceRegisterBo;
        }

        public IList<InvoiceRegisterBo> ProcessIfrs17(IList<SoaDataCompiledSummaryBo> SoaDataCompiledSummaryIfrs17Bos, int reportingType)
        {
            var invoiceRegisters = new List<InvoiceRegisterBo> { };
            foreach (var bo in SoaDataCompiledSummaryIfrs17Bos)
            {
                var invoiceRegisterBo = new InvoiceRegisterBo()
                {
                    InvoiceRegisterBatchId = InvoiceRegisterBatchBo.Id,
                    InvoiceType = bo.InvoiceType,
                    InvoiceNumber = "",
                    StatementReceivedDate = bo.StatementReceivedDate,
                    CedantId = bo.SoaDataBatchBo.CedantId,
                    RiskQuarter = bo.RiskQuarter,
                    AccountDescription = bo.AccountDescription,

                    Year1st = bo.NbPremium,
                    Gross1st = bo.NbPremium,
                    GrossRenewal = bo.RnPremium,
                    AltPremium = bo.AltPremium,
                    Discount1st = bo.NbDiscount,
                    DiscountRen = bo.RnDiscount,
                    DiscountAlt = bo.AltDiscount,

                    NBCession = bo.NbCession,
                    NBSumReins = bo.NbSar,
                    RNCession = bo.RnCession,
                    RNSumReins = bo.RnSar,
                    AltCession = bo.AltCession,
                    AltSumReins = bo.AltSar,

                    DTH = bo.DTH,
                    TPA = bo.TPA,
                    TPS = bo.TPS,
                    PPD = bo.PPD,
                    CCA = bo.CCA,
                    CCS = bo.CCS,
                    PA = bo.PA,
                    HS = bo.HS,
                    TPD = bo.TPD,
                    CI = bo.CI,

                    RiskPremium = bo.RiskPremium,
                    NoClaimBonus = bo.NoClaimBonus,
                    Levy = bo.Levy,
                    Claim = bo.Claim,
                    ProfitComm = bo.ProfitComm,
                    SurrenderValue = bo.SurrenderValue,
                    Gst = bo.Gst,
                    ModcoReserveIncome = bo.ModcoReserveIncome,
                    ReinsDeposit = bo.RiDeposit,
                    DatabaseCommission = bo.DatabaseCommission,
                    AdministrationContribution = bo.AdministrationContribution,
                    ShareOfRiCommissionFromCompulsoryCession = bo.ShareOfRiCommissionFromCompulsoryCession,
                    RecaptureFee = bo.RecaptureFee,
                    CreditCardCharges = bo.CreditCardCharges,
                    BrokerageFee = bo.BrokerageFee,

                    Remark = "",
                    CurrencyCode = bo.CurrencyCode,
                    CurrencyRate = bo.CurrencyRate,
                    SoaQuarter = bo.SoaQuarter,

                    ReasonOfAdjustment1 = bo.ReasonOfAdjustment1,
                    ReasonOfAdjustment2 = bo.ReasonOfAdjustment2,
                    InvoiceNumber1 = bo.InvoiceNumber1,
                    InvoiceDate1 = bo.InvoiceDate1,
                    Amount1 = bo.Amount1,
                    InvoiceNumber2 = bo.InvoiceNumber2,
                    InvoiceDate2 = bo.InvoiceDate2,
                    Amount2 = bo.Amount2,

                    Frequency = bo.Frequency,
                    PaymentReference = null,
                    PaymentAmount = null,
                    PaymentReceivedDate = null,
                    PreparedById = InvoiceRegisterBatchBo.CreatedById,
                    ValuationMode = bo.Frequency,

                    SoaDataBatchId = bo.SoaDataBatchId,
                    ContractCode = bo.ContractCode,
                    AnnualCohort = bo.AnnualCohort,
                    Mfrs17CellName = bo.Mfrs17CellName,
                    ReportingType = reportingType,
                };
                invoiceRegisterBo.GetRenewal();
                invoiceRegisterBo.GetTotalPaid();
                invoiceRegisterBo.GetValuationGross1st();
                invoiceRegisterBo.GetValuationGrossRen();
                invoiceRegisterBo.GetValuationDiscount1st();
                invoiceRegisterBo.GetValuationDiscountRen();
                invoiceRegisterBo.GetValuationCom1st();
                invoiceRegisterBo.GetValuationComRen();
                invoiceRegisterBo.GetValuationProfitCom();
                invoiceRegisterBo.GetValuationRiskPremium();
                invoiceRegisterBo.GetValuationClaims();

                var treatyCode = TreatyCodeService.FindByCedantIdCode(bo.SoaDataBatchBo.CedantId, bo.TreatyCode);
                if (treatyCode != null)
                    invoiceRegisterBo.TreatyCodeId = treatyCode.Id;

                if (string.IsNullOrEmpty(bo.Frequency))
                {
                    invoiceRegisterBo.Frequency = GetPremiumFrequencyCode(bo.SoaDataBatchBo.RiDataBatchId, bo.TreatyCode);
                    invoiceRegisterBo.ValuationMode = GetPremiumFrequencyCode(bo.SoaDataBatchBo.RiDataBatchId, bo.TreatyCode);
                }

                invoiceRegisters.Add(invoiceRegisterBo);
            }

            return invoiceRegisters;
        }

        public InvoiceRegisterBatchBo LoadInvoiceRegisterBatchBo()
        {
            if (CutOffService.IsCutOffProcessing())
                return null;

            InvoiceRegisterBatchBo = InvoiceRegisterBatchService.FindByStatus(InvoiceRegisterBatchBo.StatusSubmitForProcessing);
            if (InvoiceRegisterBatchBo != null)
            {
                List<int> ids = InvoiceRegisterBatchSoaDataService.GetIdsByInvoiceRegisterBatchId(InvoiceRegisterBatchBo.Id);
                if (ids != null)
                {
                    SoaDataCompiledSummaryOriginalCurrencyIfrs4Bos = SoaDataCompiledSummaryService.GetBySoaDataBatchIds(ids, SoaDataCompiledSummaryBo.ReportingTypeIFRS4, true);
                    SoaDataCompiledSummaryMyrCurrenyIfrs4Bos = SoaDataCompiledSummaryService.GetBySoaDataBatchIds(ids, SoaDataCompiledSummaryBo.ReportingTypeIFRS4, false);

                    SoaDataCompiledSummaryOriginalCurrencyIfrs17Bos = SoaDataCompiledSummaryService.GetBySoaDataBatchIds(ids, SoaDataCompiledSummaryBo.ReportingTypeIFRS17, true);
                    SoaDataCompiledSummaryMyrCurrenyIfrs17Bos = SoaDataCompiledSummaryService.GetBySoaDataBatchIds(ids, SoaDataCompiledSummaryBo.ReportingTypeIFRS17, false);

                    // data grouped by MFRS17 Cell Name
                    SoaDataCompiledSummaryCNOriginalCurrencyIfrs17Bos = SoaDataCompiledSummaryService.GetBySoaDataBatchIds(ids, SoaDataCompiledSummaryBo.ReportingTypeCNIFRS17, true);
                    SoaDataCompiledSummaryCNMyrCurrenyIfrs17Bos = SoaDataCompiledSummaryService.GetBySoaDataBatchIds(ids, SoaDataCompiledSummaryBo.ReportingTypeCNIFRS17, false);
                }
            }
            return InvoiceRegisterBatchBo;
        }

        public void SetDetail(InvoiceRegisterBo bo)
        {
            TrailObject trail = new TrailObject();
            Result result = InvoiceRegisterService.Create(ref bo, ref trail);

            SetProcessCount("Saved");

            UserTrailBo userTrailBo = new UserTrailBo(
               bo.Id,
               "Create Invoice Register",
               result,
               trail,
               User.DefaultSuperUserId
           );
            UserTrailService.Create(ref userTrailBo);
        }

        public void UpdateStatus(int status, string des)
        {
            TrailObject trail = new TrailObject();
            StatusHistoryBo statusBo = new StatusHistoryBo
            {
                ModuleId = ModuleBo.Id,
                ObjectId = InvoiceRegisterBatchBo.Id,
                Status = status,
                CreatedById = User.DefaultSuperUserId,
                UpdatedById = User.DefaultSuperUserId,
            };
            StatusHistoryService.Create(ref statusBo, ref trail);

            var reporting = InvoiceRegisterBatchBo;
            InvoiceRegisterBatchBo.Status = status;
            InvoiceRegisterBatchBo.TotalInvoice = TotalRecord;
            Result result = InvoiceRegisterBatchService.Update(ref reporting, ref trail);
            UserTrailBo userTrailBo = new UserTrailBo(
                InvoiceRegisterBatchBo.Id,
                des,
                result,
                trail,
                User.DefaultSuperUserId
            );
            UserTrailService.Create(ref userTrailBo);

            if (status == InvoiceRegisterBatchBo.StatusProcessing)
                ProcessingStatusHistoryBo = statusBo;
        }

        public string GenerateReferenceNo(int year)
        {
            return InvoiceRegisterService.GetNextReferenceNo(year);
        }

        public void DeleteInvoiceRegister()
        {
            WriteStatusLogFile("Deleting Invoice Register...");
            InvoiceRegisterService.DeleteAllByInvoiceRegisterBatchId(InvoiceRegisterBatchBo.Id);
            WriteStatusLogFile("Deleted Invoice Register", true);
        }

        public void CreateStatusLogFile()
        {
            if (InvoiceRegisterBatchBo == null)
                return;
            if (ProcessingStatusHistoryBo == null)
                return;

            TrailObject trail = new TrailObject();
            InvoiceRegisterBatchStatusFileBo = new InvoiceRegisterBatchStatusFileBo
            {
                InvoiceRegisterBatchId = InvoiceRegisterBatchBo.Id,
                StatusHistoryId = ProcessingStatusHistoryBo.Id,
                StatusHistoryBo = ProcessingStatusHistoryBo,
                CreatedById = User.DefaultSuperUserId,
                UpdatedById = User.DefaultSuperUserId,
            };
            var fileBo = InvoiceRegisterBatchStatusFileBo;
            var result = InvoiceRegisterBatchStatusFileService.Create(ref fileBo, ref trail);

            UserTrailBo userTrailBo = new UserTrailBo(
                fileBo.Id,
                "Create Invoice Register Batch Status File",
                result,
                trail,
                User.DefaultSuperUserId
            );
            UserTrailService.Create(ref userTrailBo);

            StatusLogFileFilePath = fileBo.GetFilePath();
            Util.MakeDir(StatusLogFileFilePath);
        }

        public void LogValueSet(string property, dynamic value)
        {
            WriteStatusLogFile(string.Format("{0}: {1}", property, value));
        }

        public void WriteStatusLogFile(object line, bool nextLine = false)
        {
            using (var textFile = new TextFile(StatusLogFileFilePath, true, true))
            {
                textFile.WriteLine(line);
                if (nextLine)
                    textFile.WriteLine("");
            }
        }

        public void WriteInvoiceRegisterSummary(InvoiceRegisterBo summary)
        {
            LogValueSet("BatchId", summary.InvoiceRegisterBatchId);
            LogValueSet("InvoiceType", InvoiceRegisterBo.GetInvoiceTypeName(summary.InvoiceType));
            LogValueSet("InvoiceReference", summary.InvoiceReference);
            LogValueSet("InvoiceDate", summary.InvoiceDate.ToString(Util.GetDateFormat()));
            LogValueSet("StatementReceivedDate", summary.StatementReceivedDate?.ToString(Util.GetDateFormat()));

            LogValueSet("RiskQuarter", summary.RiskQuarter);            
            LogValueSet("SoaQuarter", summary.SoaQuarter);
            LogValueSet("CurrencyCode", summary.CurrencyCode);
            LogValueSet("CurrencyRate", summary.CurrencyRate);

            LogValueSet("TotalPaid", summary.TotalPaid);

            LogValueSet("ValuationGross1st", summary.ValuationGross1st);
            LogValueSet("ValuationGrossRen", summary.ValuationGrossRen);
            LogValueSet("ValuationDiscount1st", summary.ValuationDiscount1st);
            LogValueSet("ValuationDiscountRen", summary.ValuationDiscountRen);
            LogValueSet("ValuationCom1st", summary.ValuationCom1st);
            LogValueSet("ValuationComRen", summary.ValuationComRen);
            LogValueSet("ValuationClaims", summary.ValuationClaims);
            LogValueSet("ValuationProfitCom", summary.ValuationProfitCom);
            LogValueSet("ValuationMode", summary.ValuationMode);
            LogValueSet("ValuationRiskPremium", summary.ValuationRiskPremium);
        }

        public string GetPremiumFrequencyCode(int? riDataBatchId, string treatyCode)
        {
            using (var db = new AppDbContext(false))
            {
                return db.RiData
                    .Where(q => q.RiDataBatchId == riDataBatchId && q.TreatyCode == treatyCode)
                    .GroupBy(q => q.PremiumFrequencyCode)
                    .Select(q => q.Key)
                    .FirstOrDefault();
            }
        }
    }
}
