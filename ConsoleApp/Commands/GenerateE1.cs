using BusinessObject;
using BusinessObject.Identity;
using BusinessObject.InvoiceRegisters;
using BusinessObject.SoaDatas;
using DataAccess.Entities.Identity;
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
    public class GenerateE1 : Command
    {
        public List<Column> ColsIfrs4 { get; set; }

        public List<Column> ColsIfrs17 { get; set; }

        public Excel E1Ifrs4 { get; set; }

        public Excel E1Ifrs17 { get; set; }

        public InvoiceRegisterBatchBo InvoiceRegisterBatchBo { get; set; }

        // IFRS4
        public InvoiceRegisterBo InvoiceRegisterOriginalCurrencyIfrs4Bo { get; set; }
        public InvoiceRegisterBo InvoiceRegisterMyrCurrencyIfrs4Bo { get; set; }

        public IList<InvoiceRegisterBo> InvoiceRegisterOriginalCurrencyIfrs4Bos { get; set; }
        public IList<InvoiceRegisterBo> InvoiceRegisterMyrCurrencyIfrs4Bos { get; set; }

        // IFRS17
        public InvoiceRegisterBo InvoiceRegisterOriginalCurrencyIfrs17Bo { get; set; }
        public InvoiceRegisterBo InvoiceRegisterMyrCurrencyIfrs17Bo { get; set; }

        public IList<InvoiceRegisterBo> InvoiceRegisterOriginalCurrencyIfrs17Bos { get; set; }
        public IList<InvoiceRegisterBo> InvoiceRegisterMyrCurrencyIfrs17Bos { get; set; }

        public TreatyCodeBo TreatyCodeBo { get; set; }

        public IList<ItemCodeMappingBo> ItemCodeMappingBos { get; set; }

        public IList<AccountCodeMappingBo> AccountCodeMappingBos { get; set; }

        public IList<PickListDetailBo> InvoiceFieldPickListBo { get; set; }

        public List<int> InvoiceFieldIds { get; set; }

        //public string Path { get; set; }

        public string Filename1 { get; set; }

        public string Filename2 { get; set; }

        public string FilePath { get; set; }

        public int Type { get; set; }

        public bool ReportingTypeIfrs17 { get; set; } = false;

        public string BusinessUnit { get; set; } // UFO
        public string SalesTypes { get; set; } // OM/WM/SF/CNOM/DNOM ..
        public string AccountingPeriod { get; set; } // 2019/001
        public double? TotalPaidMyr1 { get; set; } // 98,463.20
        public double? TotalPaidMyr2 { get; set; } // 98,463.20
        public string CurrencyCode { get; set; } // PHP
        public double? CurrencyRate { get; set; } //0.07829
        public double? CurrencyRate2 { get; set; } //4.13983
        public double? TotalPaid1 { get; set; } // 1,250,803.00
        public double? TotalPaid2 { get; set; } // 1,250,803.00

        public double TotalPaidAmount1 { get; set; } = 0;
        public double TotalPaidAmount2 { get; set; } = 0;

        public int Total { get; set; } = 0;

        public int Take { get; set; } = 100;

        public int Skip { get; set; } = 0;

        public int Index { get; set; }

        public ModuleBo ModuleBo { get; set; }

        public StatusHistoryBo ProcessingStatusHistoryBo { get; set; }

        public InvoiceRegisterBatchStatusFileBo InvoiceRegisterBatchStatusFileBo { get; set; }

        public string StatusLogFileFilePath { get; set; }


        // General field
        public const int TypeLayoutIdentifier = 1;
        public const int TypeCustomerCode = 2;
        public const int TypeInvoiceDate = 3;
        public const int TypeStatementReceivedDate = 4;
        public const int TypeClientName = 5;
        public const int TypePartyCode = 6;
        public const int TypeRiskQuarter = 7;
        public const int TypeTreatyCode = 8;
        public const int TypeTreatyType = 9;
        public const int TypeLob = 10;
        public const int TypeAccountsFor = 11;
        public const int TypePaymemtReceived = 12;
        public const int TypePaymemtReference = 13;
        public const int TypePaymemtAmount = 14;
        public const int TypePaymemtReceivedDate = 15;
        public const int TypeItemCode = 16;
        public const int TypeItemDescription = 17;

        #region Additional field (MFRS4)
        // WM
        public const int Ifrs4WMTypeAmount = 18;
        public const int Ifrs4WMTypeInvoiceRef = 19;

        // CNWM, DNWM
        public const int Ifrs4CNDNWMTypeAmount = 18;
        public const int Ifrs4CNDNWMTypeReasonOfAdjustment1 = 19;
        public const int Ifrs4CNDNWMTypeInvoiceNo1 = 20;
        public const int Ifrs4CNDNWMTypeAmount1 = 21;
        public const int Ifrs4CNDNWMTypeReasonOfAdjustment2 = 22;
        public const int Ifrs4CNDNWMTypeInvoiceNo2 = 23;
        public const int Ifrs4CNDNWMTypeAmount2 = 24;
        public const int Ifrs4CNDNWMTypeInvoiceRef = 25;

        // OM
        public const int Ifrs4OMTypeAmountMyr = 18;
        public const int Ifrs4OMTypeAmountOtherCurrency = 19;
        public const int Ifrs4OMTypeCurrencyRate = 20;
        public const int Ifrs4OMTypeInvoiceRef = 21;

        // CNOM, DNOM
        public const int Ifrs4CNDNOMTypeAmountMyr = 18;
        public const int Ifrs4CNDNOMTypeAmountOtherCurrency = 19;
        public const int Ifrs4CNDNOMTypeReasonOfAdjustment1 = 20;
        public const int Ifrs4CNDNOMTypeInvoiceNo1 = 21;
        public const int Ifrs4CNDNOMTypeAmount1 = 22;
        public const int Ifrs4CNDNOMTypeReasonOfAdjustment2 = 23;
        public const int Ifrs4CNDNOMTypeInvoiceNo2 = 24;
        public const int Ifrs4CNDNOMTypeAmount2 = 25;
        public const int Ifrs4CNDNOMTypeCurrencyRate = 26;
        public const int Ifrs4CNDNOMTypeInvoiceRef = 27;

        // SFWM
        public const int Ifrs4SFWMTypeAmount = 18;
        public const int Ifrs4SFWMTypeSSTCode = 19;
        public const int Ifrs4SFWMTypeInvoiceRef = 20;

        // CNSFWM, DNSFWM
        public const int Ifrs4CNDNSFWMTypeAmount = 18;
        public const int Ifrs4CNDNSFWMTypeReasonOfAdjustment1 = 19;
        public const int Ifrs4CNDNSFWMTypeInvoiceNo1 = 20;
        public const int Ifrs4CNDNSFWMTypeAmount1 = 21;
        public const int Ifrs4CNDNSFWMTypeReasonOfAdjustment2 = 22;
        public const int Ifrs4CNDNSFWMTypeInvoiceNo2 = 23;
        public const int Ifrs4CNDNSFWMTypeAmount2 = 24;
        public const int Ifrs4CNDNSFWMTypeSSTCode = 25;
        public const int Ifrs4CNDNSFWMTypeInvoiceRef = 26;
        #endregion

        #region Additional field (MFRS17)
        // WM
        public const int Ifrs17WMTypeAmount = 18;
        public const int Ifrs17WMTypeAnnualCohort = 19;
        public const int Ifrs17WMTypeInvoiceRef = 20;

        // CNWM, DNWM
        public const int Ifrs17CNDNWMTypeAmount = 18;
        public const int Ifrs17CNDNWMTypeReasonOfAdjustment1 = 19;
        public const int Ifrs17CNDNWMTypeInvoiceNo1 = 20;
        public const int Ifrs17CNDNWMTypeAmount1 = 21;
        public const int Ifrs17CNDNWMTypeReasonOfAdjustment2 = 22;
        public const int Ifrs17CNDNWMTypeInvoiceNo2 = 23;
        public const int Ifrs17CNDNWMTypeAmount2 = 24;
        public const int Ifrs17CNDNWMTypeAnnualCohort = 25;
        public const int Ifrs17CNDNWMTypeInvoiceRef = 26;

        // OM
        public const int Ifrs17OMTypeAmountMyr = 18;
        public const int Ifrs17OMTypeAmountOtherCurrency = 19;
        public const int Ifrs17OMTypeCurrencyRate = 20;
        public const int Ifrs17OMTypeAnnualCohort = 21;
        public const int Ifrs17OMTypeInvoiceRef = 22;

        // CNOM, DNOM
        public const int Ifrs17CNDNOMTypeAmountMyr = 18;
        public const int Ifrs17CNDNOMTypeAmountOtherCurrency = 19;
        public const int Ifrs17CNDNOMTypeReasonOfAdjustment1 = 20;
        public const int Ifrs17CNDNOMTypeInvoiceNo1 = 21;
        public const int Ifrs17CNDNOMTypeAmount1 = 22;
        public const int Ifrs17CNDNOMTypeReasonOfAdjustment2 = 23;
        public const int Ifrs17CNDNOMTypeInvoiceNo2 = 24;
        public const int Ifrs17CNDNOMTypeAmount2 = 25;
        public const int Ifrs17CNDNOMTypeCurrencyRate = 26;
        public const int Ifrs17CNDNOMTypeAnnualCohort = 27;
        public const int Ifrs17CNDNOMTypeInvoiceRef = 28;

        // SFWM
        public const int Ifrs17SFWMTypeAmount = 18;
        public const int Ifrs17SFWMTypeSSTCode = 19;
        public const int Ifrs17SFWMTypeAnnualCohort = 20;
        public const int Ifrs17SFWMTypeInvoiceRef = 21;

        // CNSFWM, DNSFWM
        public const int Ifrs17CNDNSFWMTypeAmount = 18;
        public const int Ifrs17CNDNSFWMTypeReasonOfAdjustment1 = 19;
        public const int Ifrs17CNDNSFWMTypeInvoiceNo1 = 20;
        public const int Ifrs17CNDNSFWMTypeAmount1 = 21;
        public const int Ifrs17CNDNSFWMTypeReasonOfAdjustment2 = 22;
        public const int Ifrs17CNDNSFWMTypeInvoiceNo2 = 23;
        public const int Ifrs17CNDNSFWMTypeAmount2 = 24;
        public const int Ifrs17CNDNSFWMTypeSSTCode = 25;
        public const int Ifrs17CNDNSFWMTypeAnnualCohort = 26;
        public const int Ifrs17CNDNSFWMTypeInvoiceRef = 27;
        #endregion

        public GenerateE1()
        {
            Title = "GenerateE1";
            Description = "To generate E1 excel files";
        }

        public override void Initial()
        {
            base.Initial();

            //Testing
            BusinessUnit = "UF1";
            SalesTypes = "OM";
            AccountingPeriod = "2019/001";
            TotalPaidMyr1 = 98463.20;
            TotalPaidMyr2 = 98463.20;
            CurrencyCode = "PHP";
            CurrencyRate = 0.07829;
            CurrencyRate2 = 4.13983;
            TotalPaid1 = 1250803.00;
            TotalPaid2 = 1250803.00;

            ModuleBo = ModuleService.FindByController(ModuleBo.ModuleController.InvoiceRegister.ToString());
            InvoiceFieldPickListBo = PickListDetailService.GetByPickListId(PickListBo.InvoiceField);
            InvoiceFieldIds = InvoiceFieldPickListBo.Select(q => q.Id).ToList();
        }

        public override void Run()
        {
            try
            {
                if (InvoiceRegisterBatchService.CountByStatus(InvoiceRegisterBatchBo.StatusSubmitForGenerate) == 0)
                {
                    PrintMessage(MessageBag.NoMfrs17reportingPendingGenerate);
                    return;
                }

                PrintStarting();

                while (LoadInvoiceRegisterBatchBo() != null)
                {
                    try
                    {
                        if (GetProcessCount("Process") > 0)
                            PrintProcessCount();
                        SetProcessCount("Process");

                        UpdateStatus(InvoiceRegisterBatchBo.StatusGenerating, "Generating SUNGL Files");

                        CreateStatusLogFile();

                        if (File.Exists(StatusLogFileFilePath))
                            File.Delete(StatusLogFileFilePath);

                        // DELETE PREVIOUS SUNGL FILES
                        PrintMessage("Deleting Sungl Files...", true, false);
                        DeleteBatchSungFile();
                        PrintMessage("Deleted Sungl Files", true, false);

                        ProcessIfrs4();
                        ProcessIfrs17();

                        UpdateStatusInvoice();

                        UpdateStatus(InvoiceRegisterBatchBo.StatusGenerateComplete, "Successfully Generating SUNGL Files");
                        WriteStatusLogFile("Successfully Generating SUNGL Files");
                    }
                    catch (Exception e)
                    {
                        var message = e.Message;
                        if (e is DbEntityValidationException dbEx)
                            message = Util.CatchDbEntityValidationException(dbEx).ToString();

                        WriteStatusLogFile(message, true);
                        UpdateStatus(InvoiceRegisterBatchBo.StatusGenerateFailed, "Failed to Generate SUNGL Files");
                        WriteStatusLogFile("Failed to Generate SUNGL Files");
                        PrintError(message);
                    }
                }

                if (GetProcessCount("Process") > 0)
                    PrintProcessCount();

                PrintEnding();
            }
            catch (Exception e)
            {
                PrintError(e.Message);
            }
        }

        public void ProcessIfrs4()
        {
            WriteStatusLogFile("Starting Generate IFRS4 SUNGL...", true);

            if (InvoiceRegisterMyrCurrencyIfrs4Bos.IsNullOrEmpty())
                return;

            for (int i = 1; i <= InvoiceRegisterBo.InvoiceTypeMax; i++)
            {
                Type = i;
                WriteStatusLogFile(string.Format("Type: {0}", InvoiceRegisterBo.GetInvoiceTypeName(i)), true);

                var invoiceRegisterMYRs = InvoiceRegisterMyrCurrencyIfrs4Bos?.Where(q => q.InvoiceType == i).ToList();
                var invoiceRegisterORIs = InvoiceRegisterOriginalCurrencyIfrs4Bos?.Where(q => q.InvoiceType == i).ToList();

                WriteStatusLogFile(string.Format("Total {0} IFRS4 Invoice Summary to be compute: {1}", InvoiceRegisterBo.GetInvoiceTypeName(i), invoiceRegisterMYRs.Count), true);
                if (!invoiceRegisterMYRs.IsNullOrEmpty())
                {
                    Total = invoiceRegisterMYRs.Count();
                    if (Total > 0)
                    {
                        PrepareE1Excel();
                        OpenE1Template();

                        // reset total amount
                        TotalPaidAmount1 = 0;
                        TotalPaidAmount2 = 0;

                        TotalPaid1 = 0;
                        TotalPaid2 = 0;

                        for (Skip = 0; Skip < Total + Take; Skip += Take)
                        {
                            if (Skip >= Total)
                                break;

                            InvoiceRegisterOriginalCurrencyIfrs4Bo = null;
                            InvoiceRegisterMyrCurrencyIfrs4Bo = null;
                            foreach (var bo in invoiceRegisterMYRs.Skip(Skip).Take(Take))
                            {
                                InvoiceRegisterMyrCurrencyIfrs4Bo = bo;
                                InvoiceRegisterOriginalCurrencyIfrs4Bo = invoiceRegisterORIs?.Where(q => q.InvoiceReference == bo.InvoiceReference).FirstOrDefault();
                                ProcessE1();
                            } 
                        }

                        BusinessUnit = InvoiceRegisterBo.GetBusinessUnit(Type);
                        AccountingPeriod = SetAccountingPeriod(InvoiceRegisterBatchBo.BatchDate);
                        CurrencyCode = CurrencyCode;
                        SalesTypes = InvoiceRegisterBo.GetInvoiceTypeName(Type);
                        TotalPaidMyr1 = TotalPaidAmount1;
                        TotalPaidMyr2 = TotalPaidAmount2;
                        CurrencyRate = CurrencyRate;
                        CurrencyRate2 = CurrencyRate2;
                        TotalPaid1 = TotalPaid1;
                        TotalPaid2 = TotalPaid2;

                        SetHeaderData();

                        SaveE1();
                        SaveFile();
                    }
                    E1Ifrs4 = null;
                }
            }
            WriteStatusLogFile("Completed Generate IFRS4 SUNGL...", true);
        }

        public void ProcessIfrs17()
        {
            WriteStatusLogFile("Starting Generate IFRS17 SUNGL...", true);

            if (InvoiceRegisterMyrCurrencyIfrs17Bos.IsNullOrEmpty())
                return;

            ReportingTypeIfrs17 = true;
            for (int i = 1; i <= InvoiceRegisterBo.InvoiceTypeMax; i++)
            {
                Type = i;
                WriteStatusLogFile(string.Format("Type: {0}", InvoiceRegisterBo.GetInvoiceTypeName(i)), true);

                var invoiceRegisterMYRIfrs17s = InvoiceRegisterMyrCurrencyIfrs17Bos?.Where(q => q.InvoiceType == i).ToList();
                var invoiceRegisterORIIfrs17s = InvoiceRegisterOriginalCurrencyIfrs17Bos?.Where(q => q.InvoiceType == i).ToList();

                WriteStatusLogFile(string.Format("Total {0} IFRS17 Invoice Summary to be compute: {1}", InvoiceRegisterBo.GetInvoiceTypeName(i), invoiceRegisterMYRIfrs17s.Count), true);
                if (!invoiceRegisterMYRIfrs17s.IsNullOrEmpty())
                {
                    Total = invoiceRegisterMYRIfrs17s.Count();
                    if (Total > 0)
                    {
                        PrepareE1Excel();
                        OpenE1Template();

                        // reset total amount
                        TotalPaidAmount1 = 0;
                        TotalPaidAmount2 = 0;

                        TotalPaid1 = 0;
                        TotalPaid2 = 0;

                        var modifiedContractCode = invoiceRegisterMYRIfrs17s.Select(q => q.ModifiedContractCode).ToList();
                        AccountCodeMappingBos = AccountCodeMappingService.FindByE1Ifrs17Param(modifiedContractCode);

                        foreach(var refNo in invoiceRegisterMYRIfrs17s.GroupBy(q => q.InvoiceReference).Select(q => q.Key).ToArray())
                        {
                            Index = 1;
                            foreach (var item in AccountCodeMappingBos.OrderBy(q => q.AccountCode))
                            {
                                var accountIndex = 1;
                                var InvoiceFields = item.InvoiceField.Split(',').Select(x => x.Trim()).ToList();
                                foreach (var invoiceField in InvoiceFields)
                                {
                                    InvoiceRegisterOriginalCurrencyIfrs17Bo = null;
                                    InvoiceRegisterMyrCurrencyIfrs17Bo = null;
                                    foreach (var bo in invoiceRegisterMYRIfrs17s.Where(q => q.ModifiedContractCode == item.ModifiedContractCodeBo?.ModifiedContractCode && q.InvoiceReference == refNo))
                                    {
                                        InvoiceRegisterMyrCurrencyIfrs17Bo = bo;
                                        InvoiceRegisterOriginalCurrencyIfrs17Bo = invoiceRegisterORIIfrs17s?.Where(q => q.InvoiceReference == bo.InvoiceReference && q.ContractCode == bo.ContractCode && q.AnnualCohort == bo.AnnualCohort).FirstOrDefault();

                                        TreatyCodeBo = TreatyCodeService.Find(bo.TreatyCodeId);
                                        var BusinessOriginBo = PickListDetailService.FindByPickListIdCode(PickListBo.BusinessOrigin, InvoiceRegisterBo.GetBusinessOriginByInvoiceType(bo.InvoiceType));
                                        if (TreatyCodeBo.TreatyTypePickListDetailBo != null)
                                            ItemCodeMappingBos = ItemCodeMappingService.FindByE1Param(TreatyCodeBo.TreatyTypePickListDetailBo.Code, BusinessOriginBo.Id, ItemCodeBo.ReportingTypeIFRS17, InvoiceFieldIds);

                                        var itemCode = ItemCodeMappingBos?.Where(b => b.InvoiceFieldPickListDetailBo.Code == invoiceField).FirstOrDefault();
                                        if (itemCode != null)
                                        {
                                            GetColumnsIfrs17();

                                            string layout = Index == 1 ? "1;2;4;5;6;10;19;22;23;24;25" : "2;4;5;6;10;19;22;23;24;25";

                                            foreach (var col in ColsIfrs17)
                                            {
                                                if (!col.ColIndex.HasValue)
                                                    continue;

                                                object v1 = null;
                                                switch (col.ColIndex)   // General field
                                                {
                                                    case TypeLayoutIdentifier:
                                                        v1 = layout;
                                                        break;
                                                    case TypeCustomerCode:
                                                        if (accountIndex == 1)
                                                            v1 = item.AccountCodeBo.Code;
                                                        break;
                                                    case TypeInvoiceDate:
                                                        v1 = InvoiceRegisterMyrCurrencyIfrs17Bo.InvoiceDate;
                                                        break;
                                                    case TypeStatementReceivedDate:
                                                        if (InvoiceRegisterMyrCurrencyIfrs17Bo.StatementReceivedDate.HasValue)
                                                            v1 = InvoiceRegisterMyrCurrencyIfrs17Bo.StatementReceivedDate.Value;
                                                        break;
                                                    case TypeClientName:
                                                        v1 = InvoiceRegisterMyrCurrencyIfrs17Bo.CedantBo.Name;
                                                        break;
                                                    case TypePartyCode:
                                                        v1 = InvoiceRegisterMyrCurrencyIfrs17Bo.CedantBo.PartyCode;
                                                        break;
                                                    case TypeRiskQuarter:
                                                        v1 = InvoiceRegisterMyrCurrencyIfrs17Bo.RiskQuarter;
                                                        break;
                                                    case TypeTreatyCode:
                                                        v1 = InvoiceRegisterMyrCurrencyIfrs17Bo.TreatyCodeBo.Code;
                                                        break;
                                                    case TypeTreatyType:
                                                        if (InvoiceRegisterMyrCurrencyIfrs17Bo.TreatyCodeBo != null)
                                                        {
                                                            if (InvoiceRegisterMyrCurrencyIfrs17Bo.TreatyCodeBo.TreatyTypePickListDetailBo != null)
                                                            {
                                                                v1 = InvoiceRegisterMyrCurrencyIfrs17Bo.TreatyCodeBo.TreatyTypePickListDetailBo.Code;
                                                            }
                                                        }
                                                        break;
                                                    case TypeLob:   // LOB not to be link to any field and leave as empty
                                                        break;
                                                    case TypeAccountsFor:
                                                        v1 = InvoiceRegisterMyrCurrencyIfrs17Bo.AccountDescription;
                                                        break;
                                                    case TypePaymemtReceived:
                                                        v1 = "No";
                                                        break;
                                                    case TypePaymemtReference:
                                                        v1 = InvoiceRegisterMyrCurrencyIfrs17Bo.PaymentReference;
                                                        break;
                                                    case TypePaymemtAmount:
                                                        v1 = InvoiceRegisterMyrCurrencyIfrs17Bo.PaymentAmount;
                                                        break;
                                                    case TypePaymemtReceivedDate:
                                                        if (InvoiceRegisterMyrCurrencyIfrs17Bo.PaymentReceivedDate.HasValue)
                                                            v1 = InvoiceRegisterMyrCurrencyIfrs17Bo.PaymentReceivedDate.Value;
                                                        break;
                                                    case TypeItemCode:
                                                        v1 = string.Format("{0} {1}", itemCode.ItemCodeBo?.Code, InvoiceRegisterMyrCurrencyIfrs17Bo.ModifiedContractCode);
                                                        break;
                                                    case TypeItemDescription:
                                                        v1 = itemCode.ItemCodeBo?.Description.Trim();
                                                        break;
                                                }

                                                switch (Type)   // Additional field
                                                {
                                                    case InvoiceRegisterBo.InvoiceTypeWM:
                                                        switch (col.ColIndex)
                                                        {
                                                            case Ifrs17WMTypeAmount:
                                                                v1 = SetFormatValue(invoiceField, InvoiceRegisterBo.ReportingTypeIFRS17);
                                                                TotalPaidAmount2 += Convert.ToDouble(v1);
                                                                break;
                                                            case Ifrs17WMTypeAnnualCohort:
                                                                v1 = InvoiceRegisterMyrCurrencyIfrs17Bo.AnnualCohort;
                                                                break;
                                                            case Ifrs17WMTypeInvoiceRef:
                                                                v1 = InvoiceRegisterMyrCurrencyIfrs17Bo.InvoiceReference;
                                                                break;
                                                        }
                                                        break;
                                                    case InvoiceRegisterBo.InvoiceTypeCNWM:
                                                    case InvoiceRegisterBo.InvoiceTypeDNWM:
                                                        switch (col.ColIndex)
                                                        {
                                                            case Ifrs17CNDNWMTypeAmount:
                                                                v1 = SetFormatValue(invoiceField, InvoiceRegisterBo.ReportingTypeIFRS17);
                                                                TotalPaidAmount2 += Convert.ToDouble(v1);
                                                                break;
                                                            case Ifrs17CNDNSFWMTypeReasonOfAdjustment1:
                                                                v1 = InvoiceRegisterMyrCurrencyIfrs17Bo.ReasonOfAdjustment1;
                                                                break;
                                                            case Ifrs17CNDNSFWMTypeInvoiceNo1:
                                                                v1 = InvoiceRegisterMyrCurrencyIfrs17Bo.InvoiceNumber1;
                                                                break;
                                                            case Ifrs17CNDNWMTypeAmount1:
                                                                v1 = InvoiceRegisterMyrCurrencyIfrs17Bo.Amount1 ?? 0;
                                                                break;
                                                            case Ifrs17CNDNSFWMTypeReasonOfAdjustment2:
                                                                v1 = InvoiceRegisterMyrCurrencyIfrs17Bo.ReasonOfAdjustment2;
                                                                break;
                                                            case Ifrs17CNDNWMTypeInvoiceNo2:
                                                                v1 = InvoiceRegisterMyrCurrencyIfrs17Bo.InvoiceNumber2;
                                                                break;
                                                            case Ifrs17CNDNWMTypeAmount2:
                                                                v1 = InvoiceRegisterMyrCurrencyIfrs17Bo.Amount2 ?? 0;
                                                                break;
                                                            case Ifrs17CNDNWMTypeAnnualCohort:
                                                                v1 = InvoiceRegisterMyrCurrencyIfrs17Bo.AnnualCohort;
                                                                break;
                                                            case Ifrs17CNDNWMTypeInvoiceRef:
                                                                v1 = InvoiceRegisterMyrCurrencyIfrs17Bo.InvoiceReference;
                                                                break;
                                                        }
                                                        break;
                                                    case InvoiceRegisterBo.InvoiceTypeOM:
                                                        switch (col.ColIndex)
                                                        {
                                                            case Ifrs17OMTypeAmountMyr:
                                                                v1 = SetFormatValue(invoiceField, InvoiceRegisterBo.ReportingTypeIFRS17);
                                                                TotalPaidAmount2 += Convert.ToDouble(v1);
                                                                break;
                                                            case Ifrs17OMTypeAmountOtherCurrency:
                                                                if (InvoiceRegisterOriginalCurrencyIfrs17Bo != null)
                                                                {
                                                                    v1 = SetFormatOriginalValue(invoiceField, InvoiceRegisterBo.ReportingTypeIFRS17);
                                                                    TotalPaid2 += Convert.ToDouble(v1);
                                                                }
                                                                break;
                                                            case Ifrs17OMTypeCurrencyRate:
                                                                v1 = InvoiceRegisterMyrCurrencyIfrs17Bo.CurrencyRate ?? 0;
                                                                break;
                                                            case Ifrs17OMTypeAnnualCohort:
                                                                v1 = InvoiceRegisterMyrCurrencyIfrs17Bo.AnnualCohort;
                                                                break;
                                                            case Ifrs17OMTypeInvoiceRef:
                                                                v1 = InvoiceRegisterMyrCurrencyIfrs17Bo.InvoiceReference;
                                                                break;
                                                        }
                                                        break;
                                                    case InvoiceRegisterBo.InvoiceTypeCNOM:
                                                    case InvoiceRegisterBo.InvoiceTypeDNOM:
                                                        switch (col.ColIndex)
                                                        {
                                                            case Ifrs17CNDNOMTypeAmountMyr:
                                                                v1 = SetFormatValue(invoiceField, InvoiceRegisterBo.ReportingTypeIFRS17);
                                                                TotalPaidAmount2 += Convert.ToDouble(v1);
                                                                break;
                                                            case Ifrs17CNDNOMTypeAmountOtherCurrency:
                                                                if (InvoiceRegisterOriginalCurrencyIfrs17Bo != null)
                                                                {
                                                                    v1 = SetFormatOriginalValue(invoiceField, InvoiceRegisterBo.ReportingTypeIFRS17);
                                                                    TotalPaid2 += Convert.ToDouble(v1);
                                                                }
                                                                break;
                                                            case Ifrs17CNDNOMTypeReasonOfAdjustment1:
                                                                v1 = InvoiceRegisterMyrCurrencyIfrs17Bo.ReasonOfAdjustment1;
                                                                break;
                                                            case Ifrs17CNDNOMTypeInvoiceNo1:
                                                                v1 = InvoiceRegisterMyrCurrencyIfrs17Bo.InvoiceNumber1;
                                                                break;
                                                            case Ifrs17CNDNOMTypeAmount1:
                                                                if (InvoiceRegisterOriginalCurrencyIfrs17Bo != null)
                                                                    v1 = InvoiceRegisterOriginalCurrencyIfrs17Bo.Amount1 ?? 0;
                                                                break;
                                                            case Ifrs17CNDNOMTypeReasonOfAdjustment2:
                                                                v1 = InvoiceRegisterMyrCurrencyIfrs17Bo.ReasonOfAdjustment2;
                                                                break;
                                                            case Ifrs17CNDNOMTypeInvoiceNo2:
                                                                v1 = InvoiceRegisterMyrCurrencyIfrs17Bo.InvoiceNumber2;
                                                                break;
                                                            case Ifrs17CNDNOMTypeAmount2:
                                                                if (InvoiceRegisterOriginalCurrencyIfrs17Bo != null)
                                                                    v1 = InvoiceRegisterOriginalCurrencyIfrs17Bo.Amount2 ?? 0;
                                                                break;
                                                            case Ifrs17CNDNOMTypeCurrencyRate:
                                                                v1 = InvoiceRegisterMyrCurrencyIfrs17Bo.CurrencyRate ?? 0;
                                                                break;
                                                            case Ifrs17CNDNOMTypeAnnualCohort:
                                                                v1 = InvoiceRegisterMyrCurrencyIfrs17Bo.AnnualCohort;
                                                                break;
                                                            case Ifrs17CNDNOMTypeInvoiceRef:
                                                                v1 = InvoiceRegisterMyrCurrencyIfrs17Bo.InvoiceReference;
                                                                break;
                                                        }
                                                        break;
                                                    case InvoiceRegisterBo.InvoiceTypeSFWM:
                                                        switch (col.ColIndex)
                                                        {
                                                            case Ifrs17SFWMTypeAmount:
                                                                v1 = SetFormatValue(invoiceField, InvoiceRegisterBo.ReportingTypeIFRS17);
                                                                TotalPaidAmount2 += Convert.ToDouble(v1);
                                                                break;
                                                            case Ifrs17SFWMTypeSSTCode:
                                                                break;
                                                            case Ifrs17SFWMTypeAnnualCohort:
                                                                v1 = InvoiceRegisterMyrCurrencyIfrs17Bo.AnnualCohort;
                                                                break;
                                                            case Ifrs17SFWMTypeInvoiceRef:
                                                                v1 = InvoiceRegisterMyrCurrencyIfrs17Bo.InvoiceReference;
                                                                break;
                                                        }
                                                        break;
                                                    case InvoiceRegisterBo.InvoiceTypeCNSFWM:
                                                    case InvoiceRegisterBo.InvoiceTypeDNSFWM:
                                                        switch (col.ColIndex)
                                                        {
                                                            case Ifrs17CNDNSFWMTypeAmount:
                                                                v1 = SetFormatValue(invoiceField, InvoiceRegisterBo.ReportingTypeIFRS17);
                                                                TotalPaidAmount2 += Convert.ToDouble(v1);
                                                                break;
                                                            case Ifrs17CNDNSFWMTypeReasonOfAdjustment1:
                                                                v1 = InvoiceRegisterMyrCurrencyIfrs17Bo.ReasonOfAdjustment1;
                                                                break;
                                                            case Ifrs17CNDNSFWMTypeInvoiceNo1:
                                                                v1 = InvoiceRegisterMyrCurrencyIfrs17Bo.InvoiceNumber1;
                                                                break;
                                                            case Ifrs17CNDNSFWMTypeAmount1:
                                                                v1 = InvoiceRegisterMyrCurrencyIfrs17Bo.Amount1 ?? 0;
                                                                break;
                                                            case Ifrs17CNDNSFWMTypeReasonOfAdjustment2:
                                                                v1 = InvoiceRegisterMyrCurrencyIfrs17Bo.ReasonOfAdjustment2;
                                                                break;
                                                            case Ifrs17CNDNSFWMTypeInvoiceNo2:
                                                                v1 = InvoiceRegisterMyrCurrencyIfrs17Bo.InvoiceNumber2;
                                                                break;
                                                            case Ifrs17CNDNSFWMTypeAmount2:
                                                                v1 = InvoiceRegisterMyrCurrencyIfrs17Bo.Amount2 ?? 0;
                                                                break;
                                                            case Ifrs17CNDNSFWMTypeSSTCode:
                                                                break;
                                                            case Ifrs17CNDNSFWMTypeAnnualCohort:
                                                                v1 = InvoiceRegisterMyrCurrencyIfrs17Bo.AnnualCohort;
                                                                break;
                                                            case Ifrs17CNDNSFWMTypeInvoiceRef:
                                                                v1 = InvoiceRegisterMyrCurrencyIfrs17Bo.InvoiceReference;
                                                                break;
                                                        }
                                                        break;
                                                }
                                                E1Ifrs17.WriteCell(E1Ifrs17.RowIndex, col.ColIndex.Value, v1);
                                            }
                                            E1Ifrs17.RowIndex++;
                                            Index++;

                                            // To set Currency Code & Rate
                                            switch (Type)
                                            {
                                                case InvoiceRegisterBo.InvoiceTypeOM:
                                                    if (InvoiceRegisterOriginalCurrencyIfrs4Bo != null)
                                                    {
                                                        CurrencyCode = InvoiceRegisterOriginalCurrencyIfrs17Bo.CurrencyCode;
                                                        CurrencyRate2 = InvoiceRegisterOriginalCurrencyIfrs17Bo.CurrencyRate;
                                                    }
                                                    CurrencyRate = InvoiceRegisterMyrCurrencyIfrs17Bo.CurrencyRate;
                                                    break;
                                                case InvoiceRegisterBo.InvoiceTypeCNOM:
                                                case InvoiceRegisterBo.InvoiceTypeDNOM:
                                                    if (InvoiceRegisterOriginalCurrencyIfrs4Bo != null)
                                                    {
                                                        CurrencyCode = InvoiceRegisterOriginalCurrencyIfrs17Bo.CurrencyCode;
                                                        CurrencyRate = InvoiceRegisterOriginalCurrencyIfrs17Bo.CurrencyRate;
                                                    }
                                                    break;
                                                default:
                                                    CurrencyCode = InvoiceRegisterMyrCurrencyIfrs17Bo.CurrencyCode;
                                                    CurrencyRate = InvoiceRegisterMyrCurrencyIfrs17Bo.CurrencyRate;
                                                    break;
                                            }
                                        }

                                        accountIndex++;
                                    }
                                }
                            }


                        }

                        

                        BusinessUnit = InvoiceRegisterBo.GetBusinessUnit(Type);
                        AccountingPeriod = SetAccountingPeriod(InvoiceRegisterBatchBo.BatchDate);
                        CurrencyCode = CurrencyCode;
                        SalesTypes = InvoiceRegisterBo.GetInvoiceTypeName(Type);
                        TotalPaidMyr1 = TotalPaidAmount1;
                        TotalPaidMyr2 = TotalPaidAmount2;
                        CurrencyRate = CurrencyRate;
                        CurrencyRate2 = CurrencyRate2;
                        TotalPaid1 = TotalPaid1;
                        TotalPaid2 = TotalPaid2;

                        SetHeaderData();

                        SaveE1();
                        SaveFile();
                    }
                    E1Ifrs17 = null;
                }
            }
            WriteStatusLogFile("Completed Generate IFRS17 SUNGL...", true);
        }

        public void ProcessE1()
        {
            if (InvoiceRegisterMyrCurrencyIfrs4Bo == null)
                return;

            TreatyCodeBo = TreatyCodeService.Find(InvoiceRegisterMyrCurrencyIfrs4Bo.TreatyCodeId);
            var BusinessOriginBo = PickListDetailService.FindByPickListIdCode(PickListBo.BusinessOrigin, InvoiceRegisterBo.GetBusinessOriginByInvoiceType(InvoiceRegisterMyrCurrencyIfrs4Bo.InvoiceType));
            if (TreatyCodeBo.TreatyTypePickListDetailBo != null)
                //ItemCodeMappingBos = ItemCodeMappingService.GetByTreatyTypePickListDetailId(TreatyCodeBo.TreatyTypePickListDetailId.Value); // Old query
                ItemCodeMappingBos = ItemCodeMappingService.FindByE1Param(TreatyCodeBo.TreatyTypePickListDetailBo.Code, BusinessOriginBo.Id, ItemCodeBo.ReportingTypeIFRS4, InvoiceFieldIds); // New query

            GetColumnsIfrs4();
            ProcessColumnsIfrs4();
        }

        public void ProcessColumnsIfrs4()
        {
            if (ItemCodeMappingBos == null)
                return;

            Index = 1;
            foreach (var item in ItemCodeMappingBos.OrderBy(q => q.InvoiceFieldPickListDetailBo.SortIndex))
            {
                string layout = Index == 1 ? "1;2;4;5;6;10;19;22;23;24;25" : "2;4;5;6;10;19;22;23;24;25";

                foreach (var col in ColsIfrs4)
                {
                    if (!col.ColIndex.HasValue)
                        continue;

                    object v1 = null;
                    switch (col.ColIndex)   // General field
                    {
                        case TypeLayoutIdentifier:
                            v1 = layout;
                            break;
                        case TypeCustomerCode:
                            if (Index == 1)
                                v1 = InvoiceRegisterMyrCurrencyIfrs4Bo.CedantBo.AccountCode;
                            break;
                        case TypeInvoiceDate:
                            v1 = InvoiceRegisterMyrCurrencyIfrs4Bo.InvoiceDate;
                            break;
                        case TypeStatementReceivedDate:
                            if (InvoiceRegisterMyrCurrencyIfrs4Bo.StatementReceivedDate.HasValue)
                                v1 = InvoiceRegisterMyrCurrencyIfrs4Bo.StatementReceivedDate.Value;
                            break;
                        case TypeClientName:
                            v1 = InvoiceRegisterMyrCurrencyIfrs4Bo.CedantBo.Name;
                            break;
                        case TypePartyCode:
                            v1 = InvoiceRegisterMyrCurrencyIfrs4Bo.CedantBo.PartyCode;
                            break;
                        case TypeRiskQuarter:
                            v1 = InvoiceRegisterMyrCurrencyIfrs4Bo.RiskQuarter;
                            break;
                        case TypeTreatyCode:
                            v1 = InvoiceRegisterMyrCurrencyIfrs4Bo.TreatyCodeBo.Code;
                            break;
                        case TypeTreatyType:
                            if (TreatyCodeBo.TreatyTypePickListDetailBo != null)
                                v1 = TreatyCodeBo.TreatyTypePickListDetailBo.Code;
                            break;
                        case TypeLob:   // LOB not to be link to any field and leave as empty
                            break;
                        case TypeAccountsFor:
                            v1 = InvoiceRegisterMyrCurrencyIfrs4Bo.AccountDescription;
                            break;
                        case TypePaymemtReceived:
                            v1 = "No";
                            break;
                        case TypePaymemtReference:
                            v1 = InvoiceRegisterMyrCurrencyIfrs4Bo.PaymentReference;
                            break;
                        case TypePaymemtAmount:
                            v1 = InvoiceRegisterMyrCurrencyIfrs4Bo.PaymentAmount;
                            break;
                        case TypePaymemtReceivedDate:
                            if (InvoiceRegisterMyrCurrencyIfrs4Bo.PaymentReceivedDate.HasValue)
                                v1 = InvoiceRegisterMyrCurrencyIfrs4Bo.PaymentReceivedDate.Value;
                            break;
                        case TypeItemCode:
                            v1 = item.ItemCodeBo.Code;
                            break;
                        case TypeItemDescription:
                            v1 = item.ItemCodeBo.Description.Trim();
                            break;
                    }

                    switch (Type)   // Additional field
                    {
                        case InvoiceRegisterBo.InvoiceTypeWM:
                            switch (col.ColIndex)
                            {
                                case Ifrs4WMTypeAmount:
                                    v1 = SetFormatValue(item.InvoiceFieldPickListDetailBo.Code, InvoiceRegisterBo.ReportingTypeIFRS4);
                                    TotalPaidAmount1 += Convert.ToDouble(v1);
                                    break;
                                case Ifrs4WMTypeInvoiceRef:
                                    v1 = InvoiceRegisterMyrCurrencyIfrs4Bo.InvoiceReference;
                                    break;
                            } 
                            break;
                        case InvoiceRegisterBo.InvoiceTypeCNWM:
                        case InvoiceRegisterBo.InvoiceTypeDNWM:
                            switch (col.ColIndex)
                            {
                                case Ifrs4CNDNWMTypeAmount:
                                    v1 = SetFormatValue(item.InvoiceFieldPickListDetailBo.Code, InvoiceRegisterBo.ReportingTypeIFRS4);
                                    TotalPaidAmount1 += Convert.ToDouble(v1);
                                    break;
                                case Ifrs4CNDNSFWMTypeReasonOfAdjustment1:
                                    v1 = InvoiceRegisterMyrCurrencyIfrs4Bo.ReasonOfAdjustment1;
                                    break;
                                case Ifrs4CNDNSFWMTypeInvoiceNo1:
                                    v1 = InvoiceRegisterMyrCurrencyIfrs4Bo.InvoiceNumber1;
                                    break;
                                case Ifrs4CNDNWMTypeAmount1:
                                    v1 = InvoiceRegisterMyrCurrencyIfrs4Bo.Amount1 ?? 0;
                                    break;
                                case Ifrs4CNDNSFWMTypeReasonOfAdjustment2:
                                    v1 = InvoiceRegisterMyrCurrencyIfrs4Bo.ReasonOfAdjustment2;
                                    break;
                                case Ifrs4CNDNWMTypeInvoiceNo2:
                                    v1 = InvoiceRegisterMyrCurrencyIfrs4Bo.InvoiceNumber2;
                                    break;
                                case Ifrs4CNDNWMTypeAmount2:
                                    v1 = InvoiceRegisterMyrCurrencyIfrs4Bo.Amount2 ?? 0;
                                    break;
                                case Ifrs4CNDNWMTypeInvoiceRef:
                                    v1 = InvoiceRegisterMyrCurrencyIfrs4Bo.InvoiceReference;
                                    break;
                            }
                            break;
                        case InvoiceRegisterBo.InvoiceTypeOM:
                            switch (col.ColIndex)
                            {
                                case Ifrs4OMTypeAmountMyr:
                                    v1 = SetFormatValue(item.InvoiceFieldPickListDetailBo.Code, InvoiceRegisterBo.ReportingTypeIFRS4);
                                    TotalPaidAmount1 += Convert.ToDouble(v1);
                                    break;
                                case Ifrs4OMTypeAmountOtherCurrency:
                                    if (InvoiceRegisterOriginalCurrencyIfrs4Bo != null)
                                    {
                                        v1 = SetFormatOriginalValue(item.InvoiceFieldPickListDetailBo.Code, InvoiceRegisterBo.ReportingTypeIFRS4);
                                        TotalPaid1 += Convert.ToDouble(v1);
                                    }
                                    break;
                                case Ifrs4OMTypeCurrencyRate:
                                    v1 = InvoiceRegisterMyrCurrencyIfrs4Bo.CurrencyRate ?? 0;
                                    break;
                                case Ifrs4OMTypeInvoiceRef:
                                    v1 = InvoiceRegisterMyrCurrencyIfrs4Bo.InvoiceReference;
                                    break;
                            }
                            break;
                        case InvoiceRegisterBo.InvoiceTypeCNOM:
                        case InvoiceRegisterBo.InvoiceTypeDNOM:
                            switch (col.ColIndex)
                            {
                                case Ifrs4CNDNOMTypeAmountMyr:
                                    v1 = SetFormatValue(item.InvoiceFieldPickListDetailBo.Code, InvoiceRegisterBo.ReportingTypeIFRS4);
                                    TotalPaidAmount1 += Convert.ToDouble(v1);
                                    break;
                                case Ifrs4CNDNOMTypeAmountOtherCurrency:
                                    if (InvoiceRegisterOriginalCurrencyIfrs4Bo != null)
                                    {
                                        v1 = SetFormatOriginalValue(item.InvoiceFieldPickListDetailBo.Code, InvoiceRegisterBo.ReportingTypeIFRS4);
                                        TotalPaid1 += Convert.ToDouble(v1);
                                    }
                                    break;
                                case Ifrs4CNDNOMTypeReasonOfAdjustment1:
                                    v1 = InvoiceRegisterMyrCurrencyIfrs4Bo.ReasonOfAdjustment1;
                                    break;
                                case Ifrs4CNDNOMTypeInvoiceNo1:
                                    v1 = InvoiceRegisterMyrCurrencyIfrs4Bo.InvoiceNumber1;
                                    break;
                                case Ifrs4CNDNOMTypeAmount1:
                                    if (InvoiceRegisterOriginalCurrencyIfrs4Bo != null)
                                        v1 = InvoiceRegisterOriginalCurrencyIfrs4Bo.Amount1 ?? 0;
                                    break;
                                case Ifrs4CNDNOMTypeReasonOfAdjustment2:
                                    v1 = InvoiceRegisterMyrCurrencyIfrs4Bo.ReasonOfAdjustment2;
                                    break;
                                case Ifrs4CNDNOMTypeInvoiceNo2:
                                    v1 = InvoiceRegisterMyrCurrencyIfrs4Bo.InvoiceNumber2;
                                    break;
                                case Ifrs4CNDNOMTypeAmount2:
                                    if (InvoiceRegisterOriginalCurrencyIfrs4Bo != null)
                                        v1 = InvoiceRegisterOriginalCurrencyIfrs4Bo.Amount2 ?? 0;
                                    break;
                                case Ifrs4CNDNOMTypeCurrencyRate:
                                    v1 = InvoiceRegisterMyrCurrencyIfrs4Bo.CurrencyRate ?? 0;
                                    break;
                                case Ifrs4CNDNOMTypeInvoiceRef:
                                    v1 = InvoiceRegisterMyrCurrencyIfrs4Bo.InvoiceReference;
                                    break;
                            }
                            break;
                        case InvoiceRegisterBo.InvoiceTypeSFWM:
                            switch (col.ColIndex)
                            {
                                case Ifrs4SFWMTypeAmount:
                                    v1 = SetFormatValue(item.InvoiceFieldPickListDetailBo.Code, InvoiceRegisterBo.ReportingTypeIFRS4);
                                    TotalPaidAmount1 += Convert.ToDouble(v1);
                                    break;
                                case Ifrs4SFWMTypeSSTCode:
                                    break;
                                case Ifrs4SFWMTypeInvoiceRef:
                                    v1 = InvoiceRegisterMyrCurrencyIfrs4Bo.InvoiceReference;
                                    break;
                            }
                            break;
                        case InvoiceRegisterBo.InvoiceTypeCNSFWM:
                        case InvoiceRegisterBo.InvoiceTypeDNSFWM:
                            switch (col.ColIndex)
                            {
                                case Ifrs4CNDNSFWMTypeAmount:
                                    v1 = SetFormatValue(item.InvoiceFieldPickListDetailBo.Code, InvoiceRegisterBo.ReportingTypeIFRS4);
                                    TotalPaidAmount1 += Convert.ToDouble(v1);
                                    break;
                                case Ifrs4CNDNSFWMTypeReasonOfAdjustment1:
                                    v1 = InvoiceRegisterMyrCurrencyIfrs4Bo.ReasonOfAdjustment1;
                                    break;
                                case Ifrs4CNDNSFWMTypeInvoiceNo1:
                                    v1 = InvoiceRegisterMyrCurrencyIfrs4Bo.InvoiceNumber1;
                                    break;
                                case Ifrs4CNDNSFWMTypeAmount1:
                                    v1 = InvoiceRegisterMyrCurrencyIfrs4Bo.Amount1 ?? 0;
                                    break;
                                case Ifrs4CNDNSFWMTypeReasonOfAdjustment2:
                                    v1 = InvoiceRegisterMyrCurrencyIfrs4Bo.ReasonOfAdjustment2;
                                    break;
                                case Ifrs4CNDNSFWMTypeInvoiceNo2:
                                    v1 = InvoiceRegisterMyrCurrencyIfrs4Bo.InvoiceNumber2;
                                    break;
                                case Ifrs4CNDNSFWMTypeAmount2:
                                    v1 = InvoiceRegisterMyrCurrencyIfrs4Bo.Amount2 ?? 0;
                                    break;
                                case Ifrs4CNDNSFWMTypeSSTCode:
                                    break;
                                case Ifrs4CNDNSFWMTypeInvoiceRef:
                                    v1 = InvoiceRegisterMyrCurrencyIfrs4Bo.InvoiceReference;
                                    break;
                            }
                            break;
                    }
                    E1Ifrs4.WriteCell(E1Ifrs4.RowIndex, col.ColIndex.Value, v1);
                }
                E1Ifrs4.RowIndex++;
                Index++;

                // To set Currency Code & Rate
                switch (Type)
                {                    
                    case InvoiceRegisterBo.InvoiceTypeOM:
                        if (InvoiceRegisterOriginalCurrencyIfrs4Bo != null)
                        {
                            CurrencyCode = InvoiceRegisterOriginalCurrencyIfrs4Bo.CurrencyCode;
                            CurrencyRate2 = InvoiceRegisterOriginalCurrencyIfrs4Bo.CurrencyRate;
                        }
                        CurrencyRate = InvoiceRegisterMyrCurrencyIfrs4Bo.CurrencyRate;                        
                        break;
                    case InvoiceRegisterBo.InvoiceTypeCNOM:
                    case InvoiceRegisterBo.InvoiceTypeDNOM:
                        if (InvoiceRegisterOriginalCurrencyIfrs4Bo != null)
                        {
                            CurrencyCode = InvoiceRegisterOriginalCurrencyIfrs4Bo.CurrencyCode;
                            CurrencyRate = InvoiceRegisterOriginalCurrencyIfrs4Bo.CurrencyRate;
                        } 
                        break;
                    default:
                        CurrencyCode = InvoiceRegisterMyrCurrencyIfrs4Bo.CurrencyCode;
                        CurrencyRate = InvoiceRegisterMyrCurrencyIfrs4Bo.CurrencyRate;
                        break;
                }
            }
        }

        public void PrepareE1Excel()
        {
            var templateFilepathIfrs4 = "";
            var templateFilepathIfrs17 = "";
            switch (Type)
            {
                case InvoiceRegisterBo.InvoiceTypeWM:
                    templateFilepathIfrs4 = Util.GetWebAppDocumentFilePath("E1_WM_Template.xlsx");
                    templateFilepathIfrs17 = Util.GetWebAppDocumentFilePath("E1_WM_MFRS17_Template.xlsx");
                    break;
                case InvoiceRegisterBo.InvoiceTypeCNWM:
                case InvoiceRegisterBo.InvoiceTypeDNWM:
                    templateFilepathIfrs4 = Util.GetWebAppDocumentFilePath("E1_CNDNWM_Template.xlsx");
                    templateFilepathIfrs17 = Util.GetWebAppDocumentFilePath("E1_CNDNWM_MFRS17_Template.xlsx");
                    break;
                case InvoiceRegisterBo.InvoiceTypeOM:
                    templateFilepathIfrs4 = Util.GetWebAppDocumentFilePath("E1_OM_Template.xlsx");
                    templateFilepathIfrs17 = Util.GetWebAppDocumentFilePath("E1_OM_MFRS17_Template.xlsx");
                    break;
                case InvoiceRegisterBo.InvoiceTypeCNOM:
                case InvoiceRegisterBo.InvoiceTypeDNOM:
                    templateFilepathIfrs4 = Util.GetWebAppDocumentFilePath("E1_CNDNOM_Template.xlsx");
                    templateFilepathIfrs17 = Util.GetWebAppDocumentFilePath("E1_CNDNOM_MFRS17_Template.xlsx");
                    break;
                case InvoiceRegisterBo.InvoiceTypeSFWM:
                    templateFilepathIfrs4 = Util.GetWebAppDocumentFilePath("E1_SFWM_Template.xlsx");
                    templateFilepathIfrs17 = Util.GetWebAppDocumentFilePath("E1_SFWM_MFRS17_Template.xlsx");
                    break;
                case InvoiceRegisterBo.InvoiceTypeCNSFWM:
                case InvoiceRegisterBo.InvoiceTypeDNSFWM:
                    templateFilepathIfrs4 = Util.GetWebAppDocumentFilePath("E1_CNDNSFWM_Template.xlsx");
                    templateFilepathIfrs17 = Util.GetWebAppDocumentFilePath("E1_CNDNSFWM_MFRS17_Template.xlsx");
                    break;
            }

            if (ReportingTypeIfrs17)
            {
                Filename2 = string.Format("E1_IFRS17_{0}", InvoiceRegisterBo.GetInvoiceTypeName(Type)).AppendDateTimeFileName(".xlsx");
                var filepath = Path.Combine(Util.GetE1Path(), Filename2);
                E1Ifrs17 = new Excel(templateFilepathIfrs17, filepath, 9);
            }
            else
            {
                Filename1 = string.Format("E1_IFRS4_{0}", InvoiceRegisterBo.GetInvoiceTypeName(Type)).AppendDateTimeFileName(".xlsx");
                var filepath = Path.Combine(Util.GetE1Path(), Filename1);
                E1Ifrs4 = new Excel(templateFilepathIfrs4, filepath, 9);
            }
        }

        public void OpenE1Template()
        {
            if (!ReportingTypeIfrs17)
                E1Ifrs4.OpenTemplate();
            else
                E1Ifrs17.OpenTemplate();
        }

        public void SaveE1()
        {
            if (!ReportingTypeIfrs17)
                E1Ifrs4.Save();
            else
                E1Ifrs17.Save();
        }

        public void SetHeaderData()
        {
            if (ReportingTypeIfrs17)
            {
                E1Ifrs17.WriteCell(1, 2, BusinessUnit);
                E1Ifrs17.WriteCell(1, 5, AccountingPeriod);
                E1Ifrs17.WriteCell(1, 8, CurrencyCode);
                E1Ifrs17.WriteCell(2, 2, SalesTypes);
                E1Ifrs17.WriteCell(2, 5, TotalPaidMyr2);

                switch (Type)
                {
                    case InvoiceRegisterBo.InvoiceTypeOM:
                        E1Ifrs17.WriteCell(1, 11, CurrencyRate);
                        E1Ifrs17.WriteCell(1, 13, CurrencyRate2);
                        E1Ifrs17.WriteCell(2, 8, TotalPaid2);
                        break;
                    case InvoiceRegisterBo.InvoiceTypeCNOM:
                    case InvoiceRegisterBo.InvoiceTypeDNOM:
                        E1Ifrs17.WriteCell(1, 11, CurrencyRate);
                        E1Ifrs17.WriteCell(2, 8, TotalPaid2);
                        break;
                }
            }
            else
            {
                E1Ifrs4.WriteCell(1, 2, BusinessUnit);
                E1Ifrs4.WriteCell(1, 5, AccountingPeriod);
                E1Ifrs4.WriteCell(1, 8, CurrencyCode);
                E1Ifrs4.WriteCell(2, 2, SalesTypes);
                E1Ifrs4.WriteCell(2, 5, TotalPaidMyr1);

                switch (Type)
                {
                    case InvoiceRegisterBo.InvoiceTypeOM:
                        E1Ifrs4.WriteCell(1, 11, CurrencyRate);
                        E1Ifrs4.WriteCell(1, 13, CurrencyRate2);
                        E1Ifrs4.WriteCell(2, 8, TotalPaid1);
                        break;
                    case InvoiceRegisterBo.InvoiceTypeCNOM:
                    case InvoiceRegisterBo.InvoiceTypeDNOM:
                        E1Ifrs4.WriteCell(1, 11, CurrencyRate);
                        E1Ifrs4.WriteCell(2, 8, TotalPaid1);
                        break;
                }
            }
        }

        public List<Column> GetColumnsIfrs4()
        {
            ColsIfrs4 = new List<Column>
            {
                new Column { ColIndex = TypeLayoutIdentifier },
                new Column { ColIndex = TypeCustomerCode },
                new Column { ColIndex = TypeInvoiceDate },
                new Column { ColIndex = TypeStatementReceivedDate },
                new Column { ColIndex = TypeClientName },
                new Column { ColIndex = TypePartyCode },
                new Column { ColIndex = TypeRiskQuarter },
                new Column { ColIndex = TypeTreatyCode },
                new Column { ColIndex = TypeTreatyType },
                new Column { ColIndex = TypeLob },
                new Column { ColIndex = TypeAccountsFor },
                new Column { ColIndex = TypePaymemtReceived },
                new Column { ColIndex = TypePaymemtReference },
                new Column { ColIndex = TypePaymemtAmount },
                new Column { ColIndex = TypePaymemtReceivedDate },
                new Column { ColIndex = TypeItemCode },
                new Column { ColIndex = TypeItemDescription },
            };

            switch (Type)
            {
                case InvoiceRegisterBo.InvoiceTypeWM:
                    ColsIfrs4.Add(new Column { ColIndex = Ifrs4WMTypeAmount });
                    ColsIfrs4.Add(new Column { ColIndex = Ifrs4WMTypeInvoiceRef });
                    break;
                case InvoiceRegisterBo.InvoiceTypeCNWM:
                case InvoiceRegisterBo.InvoiceTypeDNWM:
                    ColsIfrs4.Add(new Column { ColIndex = Ifrs4CNDNWMTypeAmount });
                    ColsIfrs4.Add(new Column { ColIndex = Ifrs4CNDNWMTypeReasonOfAdjustment1 });
                    ColsIfrs4.Add(new Column { ColIndex = Ifrs4CNDNWMTypeInvoiceNo1 });
                    ColsIfrs4.Add(new Column { ColIndex = Ifrs4CNDNWMTypeAmount1 });
                    ColsIfrs4.Add(new Column { ColIndex = Ifrs4CNDNWMTypeReasonOfAdjustment2 });
                    ColsIfrs4.Add(new Column { ColIndex = Ifrs4CNDNWMTypeInvoiceNo2 });
                    ColsIfrs4.Add(new Column { ColIndex = Ifrs4CNDNWMTypeAmount2 });
                    ColsIfrs4.Add(new Column { ColIndex = Ifrs4CNDNWMTypeInvoiceRef });
                    break;
                case InvoiceRegisterBo.InvoiceTypeOM:
                    ColsIfrs4.Add(new Column { ColIndex = Ifrs4OMTypeAmountMyr });
                    ColsIfrs4.Add(new Column { ColIndex = Ifrs4OMTypeAmountOtherCurrency });
                    ColsIfrs4.Add(new Column { ColIndex = Ifrs4OMTypeCurrencyRate });
                    ColsIfrs4.Add(new Column { ColIndex = Ifrs4OMTypeInvoiceRef });
                    break;
                case InvoiceRegisterBo.InvoiceTypeCNOM:
                case InvoiceRegisterBo.InvoiceTypeDNOM:
                    ColsIfrs4.Add(new Column { ColIndex = Ifrs4CNDNOMTypeAmountMyr });
                    ColsIfrs4.Add(new Column { ColIndex = Ifrs4CNDNOMTypeAmountOtherCurrency });
                    ColsIfrs4.Add(new Column { ColIndex = Ifrs4CNDNOMTypeReasonOfAdjustment1 });
                    ColsIfrs4.Add(new Column { ColIndex = Ifrs4CNDNOMTypeInvoiceNo1 });
                    ColsIfrs4.Add(new Column { ColIndex = Ifrs4CNDNOMTypeAmount1 });
                    ColsIfrs4.Add(new Column { ColIndex = Ifrs4CNDNOMTypeReasonOfAdjustment2 });
                    ColsIfrs4.Add(new Column { ColIndex = Ifrs4CNDNOMTypeInvoiceNo2 });
                    ColsIfrs4.Add(new Column { ColIndex = Ifrs4CNDNOMTypeAmount2 });
                    ColsIfrs4.Add(new Column { ColIndex = Ifrs4CNDNOMTypeCurrencyRate });
                    ColsIfrs4.Add(new Column { ColIndex = Ifrs4CNDNOMTypeInvoiceRef });
                    break;
                case InvoiceRegisterBo.InvoiceTypeSFWM:
                    ColsIfrs4.Add(new Column { ColIndex = Ifrs4SFWMTypeAmount });
                    ColsIfrs4.Add(new Column { ColIndex = Ifrs4SFWMTypeSSTCode });
                    ColsIfrs4.Add(new Column { ColIndex = Ifrs4SFWMTypeInvoiceRef });
                    break;
                case InvoiceRegisterBo.InvoiceTypeCNSFWM:
                case InvoiceRegisterBo.InvoiceTypeDNSFWM:

                    ColsIfrs4.Add(new Column { ColIndex = Ifrs4CNDNSFWMTypeAmount });
                    ColsIfrs4.Add(new Column { ColIndex = Ifrs4CNDNSFWMTypeReasonOfAdjustment1 });
                    ColsIfrs4.Add(new Column { ColIndex = Ifrs4CNDNSFWMTypeInvoiceNo1 });
                    ColsIfrs4.Add(new Column { ColIndex = Ifrs4CNDNSFWMTypeAmount1 });
                    ColsIfrs4.Add(new Column { ColIndex = Ifrs4CNDNSFWMTypeReasonOfAdjustment2 });
                    ColsIfrs4.Add(new Column { ColIndex = Ifrs4CNDNSFWMTypeInvoiceNo2 });
                    ColsIfrs4.Add(new Column { ColIndex = Ifrs4CNDNSFWMTypeAmount2 });
                    ColsIfrs4.Add(new Column { ColIndex = Ifrs4CNDNSFWMTypeSSTCode });
                    ColsIfrs4.Add(new Column { ColIndex = Ifrs4CNDNSFWMTypeInvoiceRef });
                    break;
            }
            return ColsIfrs4;
        }

        public List<Column> GetColumnsIfrs17()
        {
            ColsIfrs17 = new List<Column>
            {
                new Column { ColIndex = TypeLayoutIdentifier },
                new Column { ColIndex = TypeCustomerCode },
                new Column { ColIndex = TypeInvoiceDate },
                new Column { ColIndex = TypeStatementReceivedDate },
                new Column { ColIndex = TypeClientName },
                new Column { ColIndex = TypePartyCode },
                new Column { ColIndex = TypeRiskQuarter },
                new Column { ColIndex = TypeTreatyCode },
                new Column { ColIndex = TypeTreatyType },
                new Column { ColIndex = TypeLob },
                new Column { ColIndex = TypeAccountsFor },
                new Column { ColIndex = TypePaymemtReceived },
                new Column { ColIndex = TypePaymemtReference },
                new Column { ColIndex = TypePaymemtAmount },
                new Column { ColIndex = TypePaymemtReceivedDate },
                new Column { ColIndex = TypeItemCode },
                new Column { ColIndex = TypeItemDescription },
            };

            switch (Type)
            {
                case InvoiceRegisterBo.InvoiceTypeWM:
                    ColsIfrs17.Add(new Column { ColIndex = Ifrs17WMTypeAmount });
                    ColsIfrs17.Add(new Column { ColIndex = Ifrs17WMTypeAnnualCohort });
                    ColsIfrs17.Add(new Column { ColIndex = Ifrs17WMTypeInvoiceRef });
                    break;
                case InvoiceRegisterBo.InvoiceTypeCNWM:
                case InvoiceRegisterBo.InvoiceTypeDNWM:
                    ColsIfrs17.Add(new Column { ColIndex = Ifrs17CNDNWMTypeAmount });
                    ColsIfrs17.Add(new Column { ColIndex = Ifrs17CNDNWMTypeReasonOfAdjustment1 });
                    ColsIfrs17.Add(new Column { ColIndex = Ifrs17CNDNWMTypeInvoiceNo1 });
                    ColsIfrs17.Add(new Column { ColIndex = Ifrs17CNDNWMTypeAmount1 });
                    ColsIfrs17.Add(new Column { ColIndex = Ifrs17CNDNWMTypeReasonOfAdjustment2 });
                    ColsIfrs17.Add(new Column { ColIndex = Ifrs17CNDNWMTypeInvoiceNo2 });
                    ColsIfrs17.Add(new Column { ColIndex = Ifrs17CNDNWMTypeAmount2 });
                    ColsIfrs17.Add(new Column { ColIndex = Ifrs17CNDNWMTypeAnnualCohort });
                    ColsIfrs17.Add(new Column { ColIndex = Ifrs17CNDNWMTypeInvoiceRef });
                    break;
                case InvoiceRegisterBo.InvoiceTypeOM:
                    ColsIfrs17.Add(new Column { ColIndex = Ifrs17OMTypeAmountMyr });
                    ColsIfrs17.Add(new Column { ColIndex = Ifrs17OMTypeAmountOtherCurrency });
                    ColsIfrs17.Add(new Column { ColIndex = Ifrs17OMTypeCurrencyRate });
                    ColsIfrs17.Add(new Column { ColIndex = Ifrs17OMTypeAnnualCohort });
                    ColsIfrs17.Add(new Column { ColIndex = Ifrs17OMTypeInvoiceRef });
                    break;
                case InvoiceRegisterBo.InvoiceTypeCNOM:
                case InvoiceRegisterBo.InvoiceTypeDNOM:
                    ColsIfrs17.Add(new Column { ColIndex = Ifrs17CNDNOMTypeAmountMyr });
                    ColsIfrs17.Add(new Column { ColIndex = Ifrs17CNDNOMTypeAmountOtherCurrency });
                    ColsIfrs17.Add(new Column { ColIndex = Ifrs17CNDNOMTypeReasonOfAdjustment1 });
                    ColsIfrs17.Add(new Column { ColIndex = Ifrs17CNDNOMTypeInvoiceNo1 });
                    ColsIfrs17.Add(new Column { ColIndex = Ifrs17CNDNOMTypeAmount1 });
                    ColsIfrs17.Add(new Column { ColIndex = Ifrs17CNDNOMTypeReasonOfAdjustment2 });
                    ColsIfrs17.Add(new Column { ColIndex = Ifrs17CNDNOMTypeInvoiceNo2 });
                    ColsIfrs17.Add(new Column { ColIndex = Ifrs17CNDNOMTypeAmount2 });
                    ColsIfrs17.Add(new Column { ColIndex = Ifrs17CNDNOMTypeCurrencyRate });
                    ColsIfrs17.Add(new Column { ColIndex = Ifrs17CNDNOMTypeAnnualCohort });
                    ColsIfrs17.Add(new Column { ColIndex = Ifrs17CNDNOMTypeInvoiceRef });
                    break;
                case InvoiceRegisterBo.InvoiceTypeSFWM:
                    ColsIfrs17.Add(new Column { ColIndex = Ifrs17SFWMTypeAmount });
                    ColsIfrs17.Add(new Column { ColIndex = Ifrs17SFWMTypeSSTCode });
                    ColsIfrs17.Add(new Column { ColIndex = Ifrs17SFWMTypeAnnualCohort });
                    ColsIfrs17.Add(new Column { ColIndex = Ifrs17SFWMTypeInvoiceRef });
                    break;
                case InvoiceRegisterBo.InvoiceTypeCNSFWM:
                case InvoiceRegisterBo.InvoiceTypeDNSFWM:

                    ColsIfrs17.Add(new Column { ColIndex = Ifrs17CNDNSFWMTypeAmount });
                    ColsIfrs17.Add(new Column { ColIndex = Ifrs17CNDNSFWMTypeReasonOfAdjustment1 });
                    ColsIfrs17.Add(new Column { ColIndex = Ifrs17CNDNSFWMTypeInvoiceNo1 });
                    ColsIfrs17.Add(new Column { ColIndex = Ifrs17CNDNSFWMTypeAmount1 });
                    ColsIfrs17.Add(new Column { ColIndex = Ifrs17CNDNSFWMTypeReasonOfAdjustment2 });
                    ColsIfrs17.Add(new Column { ColIndex = Ifrs17CNDNSFWMTypeInvoiceNo2 });
                    ColsIfrs17.Add(new Column { ColIndex = Ifrs17CNDNSFWMTypeAmount2 });
                    ColsIfrs17.Add(new Column { ColIndex = Ifrs17CNDNSFWMTypeSSTCode });
                    ColsIfrs17.Add(new Column { ColIndex = Ifrs17CNDNSFWMTypeAnnualCohort });
                    ColsIfrs17.Add(new Column { ColIndex = Ifrs17CNDNSFWMTypeInvoiceRef });
                    break;
            }
            return ColsIfrs17;
        }

        public InvoiceRegisterBatchBo LoadInvoiceRegisterBatchBo()
        {
            InvoiceRegisterBatchBo = InvoiceRegisterBatchService.FindByStatus(InvoiceRegisterBatchBo.StatusSubmitForGenerate);
            if (InvoiceRegisterBatchBo != null)
            {
                InvoiceRegisterOriginalCurrencyIfrs4Bos = InvoiceRegisterService.GetByInvoiceRegisterBatchId(InvoiceRegisterBatchBo.Id, InvoiceRegisterBo.ReportingTypeIFRS4, true);
                InvoiceRegisterMyrCurrencyIfrs4Bos = InvoiceRegisterService.GetByInvoiceRegisterBatchId(InvoiceRegisterBatchBo.Id, InvoiceRegisterBo.ReportingTypeIFRS4, false);

                InvoiceRegisterOriginalCurrencyIfrs17Bos = InvoiceRegisterService.GetByInvoiceRegisterBatchId(InvoiceRegisterBatchBo.Id, InvoiceRegisterBo.ReportingTypeIFRS17, true);
                InvoiceRegisterMyrCurrencyIfrs17Bos = InvoiceRegisterService.GetByInvoiceRegisterBatchId(InvoiceRegisterBatchBo.Id, InvoiceRegisterBo.ReportingTypeIFRS17, false);
            }
            return InvoiceRegisterBatchBo;
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

            var invoice = InvoiceRegisterBatchBo;
            InvoiceRegisterBatchBo.Status = status;

            Result result = InvoiceRegisterBatchService.Update(ref invoice, ref trail);
            UserTrailBo userTrailBo = new UserTrailBo(
                InvoiceRegisterBatchBo.Id,
                des,
                result,
                trail,
                User.DefaultSuperUserId
            );
            UserTrailService.Create(ref userTrailBo);

            if (status == InvoiceRegisterBatchBo.StatusGenerating)
                ProcessingStatusHistoryBo = statusBo;
        }

        public void UpdateStatusInvoice()
        {
            List<int> ids = InvoiceRegisterBatchSoaDataService.GetIdsByInvoiceRegisterBatchId(InvoiceRegisterBatchBo.Id);
            foreach (int id in ids)
            {
                SoaDataBatchBo bo = SoaDataBatchService.Find(id);
                bo.InvoiceStatus = SoaDataBatchBo.InvoiceStatusInvoicing;
                SoaDataBatchService.Update(ref bo);

                WriteStatusLogFile(string.Format("Updating Invoice Status for Soa Data: {0}", bo.Id));
            }
        }

        public string GetPropertyName(int key)
        {
            return StandardSoaDataOutputBo.GetPropertyNameByType(key);
        }

        public void SaveFile()
        {
            var trail = new TrailObject();
            var bo = new InvoiceRegisterBatchFileBo
            {
                InvoiceRegisterBatchId = InvoiceRegisterBatchBo.Id,
                FileName = (ReportingTypeIfrs17 ? Filename2 : Filename1),
                HashFileName = (ReportingTypeIfrs17 ? Filename2 : Filename1),
                Type = Type,
                DataUpdate = false,
                Status = InvoiceRegisterBatchFileBo.StatusCompleted,
                CreatedById = User.DefaultSuperUserId,
                UpdatedById = User.DefaultSuperUserId,
            };
            InvoiceRegisterBatchFileService.Create(ref bo, ref trail);
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

        public void WriteStatusLogFile(object line, bool nextLine = false)
        {
            using (var textFile = new TextFile(StatusLogFileFilePath, true, true))
            {
                textFile.WriteLine(line);
                if (nextLine)
                    textFile.WriteLine("");
            }
        }

        public void DeleteBatchSungFile()
        {
            WriteStatusLogFile("Deleting Sungl Files...");
            var files = InvoiceRegisterBatchFileService.GetAllByInvoiceRegisterBatchId(InvoiceRegisterBatchBo.Id);
            foreach (InvoiceRegisterBatchFileBo file in files)
            {
                string fileE1 = Path.Combine(Util.GetE1Path(), file.HashFileName);
                if (File.Exists(fileE1))
                    File.Delete(fileE1);
            }
            InvoiceRegisterBatchFileService.DeleteAllByInvoiceRegisterBatchId(InvoiceRegisterBatchBo.Id);
            WriteStatusLogFile("Deleted Sungl Files", true);
        }

        public string SetAccountingPeriod(DateTime? batchDate)
        {
            int Year = DateTime.Parse(batchDate?.ToString()).Year;
            int Month = DateTime.Parse(batchDate?.ToString()).Month;
            int Day = DateTime.Parse(batchDate?.ToString()).Day;

            string accountingPeriod = "";
            var PeriodDateRanges = Enumerable.Range(0, 12)
                .Select(x => new
                {
                    StartDate = new DateTime(Year, (x == 0 ? x + 1 : x), (x == 0 ? 1 : 21)),
                    EndDate = new DateTime(Year, x + 1, (x == 11 ? 31 : 20)),
                    Period = string.Format("{0}/{1}", Year, (x + 1).ToString("000")),
                });

            foreach (var range in PeriodDateRanges)
            {
                DateTime startDate = DateTime.Parse(range.GetPropertyValue("StartDate").ToString());
                DateTime endDate = DateTime.Parse(range.GetPropertyValue("EndDate").ToString());
                var period = range.GetPropertyValue("Period");

                if ((startDate <= batchDate) && (batchDate <= endDate))
                    accountingPeriod = period.ToString();
            }
            return accountingPeriod;
        }

        public double SetFormatValue(string code, int reportingType)
        {
            string format = InvoiceRegisterBo.GetFormatValueByCode(code);

            if (code == "ShareOfRiCommissionFromCompulsor")
                code = "ShareOfRiCommissionFromCompulsoryCession";

            object value = null;
            if (reportingType == InvoiceRegisterBo.ReportingTypeIFRS17)
                value = InvoiceRegisterMyrCurrencyIfrs17Bo.GetPropertyValue(code) ?? 0;
            else if (reportingType == InvoiceRegisterBo.ReportingTypeIFRS4)
                value = InvoiceRegisterMyrCurrencyIfrs4Bo.GetPropertyValue(code) ?? 0;

            double amount = Convert.ToDouble(value);
            if (format == "-")
                amount = amount * -1;

            return amount;
        }

        public double SetFormatOriginalValue(string code, int reportingType)
        {
            string format = InvoiceRegisterBo.GetFormatValueByCode(code);

            if (code == "ShareOfRiCommissionFromCompulsor")
                code = "ShareOfRiCommissionFromCompulsoryCession";

            object value = null;
            if (reportingType == InvoiceRegisterBo.ReportingTypeIFRS17)
                value = InvoiceRegisterOriginalCurrencyIfrs17Bo.GetPropertyValue(code) ?? 0;
            else if (reportingType == InvoiceRegisterBo.ReportingTypeIFRS4)
                value = InvoiceRegisterOriginalCurrencyIfrs4Bo.GetPropertyValue(code) ?? 0;

            double amount = Convert.ToDouble(value);
            if (format == "-")
                amount = amount * -1;

            return amount;
        }

        public string GetContractCode(string code)
        {
            if (!string.IsNullOrEmpty(code))
            {
                if (code.Length > 3)
                    return code.Substring(0, code.Length - 3);
                else
                    return code;
            }
            return "";
        }
    }
}
