using BusinessObject;
using BusinessObject.Identity;
using DataAccess.Entities.Identity;
using DataAccess.EntityFramework;
using Services;
using Services.Identity;
using Services.RiDatas;
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
    public class ProcessRetroRegisterBatch : Command
    {
        public RetroRegisterBatchBo RetroRegisterBatchBo { get; set; }

        public IList<DirectRetroBo> DirectRetroBos { get; set; }

        public IList<RetroStatementBo> RetroStatementBos { get; set; }

        public IList<RetroSummaryBo> RetroSummaryBos { get; set; }

        public IList<DateTime> PublicHolidayDetailBos { get; set; }

        public DateTime CurrentDate { get; set; } = DateTime.Now;

        public ModuleBo ModuleBo { get; set; }

        public RetroRegisterBatchStatusFileBo RetroRegisterBatchStatusFileBo { get; set; }

        public StatusHistoryBo ProcessingStatusHistoryBo { get; set; }

        public int TotalRecord { get; set; }

        public string StatusLogFileFilePath { get; set; }

        public string AccountingPeriod { get; set; }

        public ProcessRetroRegisterBatch()
        {
            Title = "ProcessRetroRegisterBatch";
            Description = "To process Direct Retro Retro Statement for Retro Register";
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
                if (RetroRegisterBatchService.CountByStatus(RetroRegisterBatchBo.StatusSubmitForProcessing) == 0)
                {
                    Log = false;
                    PrintMessage(MessageBag.NoBatchPendingProcess);
                    return;
                }

                PrintStarting();

                var currentHolidayBo = PublicHolidayService.FindByYear(CurrentDate.Year);
                if (currentHolidayBo != null)
                    PublicHolidayDetailBos = PublicHolidayDetailService.GetByPublicHolidayId(currentHolidayBo.Id).Select(q => q.PublicHolidayDate).ToList();

                while (LoadRetroRegisterBatchBo() != null)
                {
                    try
                    {
                        TotalRecord = 0;

                        if (GetProcessCount("Process") > 0)
                            PrintProcessCount();
                        SetProcessCount("Process");

                        UpdateStatus(RetroRegisterBatchBo.StatusProcessing, "Processing Retro Register Batch");

                        CreateStatusLogFile();

                        if (File.Exists(StatusLogFileFilePath))
                            File.Delete(StatusLogFileFilePath);

                        // DELETE PREVIOUS RETRO REGISTER
                        PrintMessage("Deleting Retro Register...", true, false);
                        DeleteRetroRegister();
                        PrintMessage("Deleted Retro Register", true, false);

                        CreateRetroRegister();

                        SetTotalRecord();

                        UpdateRetroStatements();

                        UpdateDirectRetroStatus(DirectRetroBo.RetroStatusStatementIssued);

                        UpdateStatus(RetroRegisterBatchBo.StatusSuccess, "Successfully Processed Retro Register Batch");
                        WriteStatusLogFile("Successfully Processed Retro Register Batch");
                    }
                    catch (Exception e)
                    {
                        SetTotalRecord();

                        var message = e.Message;
                        if (e is DbEntityValidationException dbEx)
                            message = Util.CatchDbEntityValidationException(dbEx).ToString();

                        WriteStatusLogFile(message, true);
                        UpdateStatus(RetroRegisterBatchBo.StatusFailed, "Failed to Process Retro Register Batch");
                        WriteStatusLogFile("Failed to Process Retro Register Batch");
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

        public void CreateRetroRegister()
        {
            WriteStatusLogFile("Starting Create Retro Register...", true);
            WriteStatusLogFile(string.Format("Total Quartely Report to be compute: {0}", (RetroStatementBos == null ? 0 : RetroStatementBos.Count)), true);
            
            if (RetroStatementBos != null)
            {
                int total = RetroStatementBos.Count;
                int take = 1;
                for (int skip = 0; skip < (total + take); skip += take)
                {
                    if (skip >= total)
                        break;

                    foreach (var bo in RetroStatementBos.Skip(skip).Take(take))
                    {
                        string businessOriginCode = "";
                        var treatyCode = TreatyCodeService.Find(bo.DirectRetroBo.TreatyCodeId);
                        if (treatyCode != null)
                        {
                            if (treatyCode.TreatyBo != null)
                            {
                                if (treatyCode.TreatyBo.BusinessOriginPickListDetailBo != null)
                                    businessOriginCode = treatyCode.TreatyBo.BusinessOriginPickListDetailBo.Code;
                            }
                        }

                        WriteStatusLogFile(string.Format("Computing Quartely Report format: {0}", bo.Id));

                        RetroSummaryBos = RetroSummaryService.GetByDirectRetroIdReportingType(bo.DirectRetroId, RetroSummaryBo.ReportingTypeIFRS17);

                        if (!string.IsNullOrEmpty(bo.AccountingPeriod))
                        {
                            WriteStatusLogFile(string.Format("Computing Quartely Report Quarter: {0}", bo.AccountingPeriod));
                            var retroRegisters = new List<RetroRegisterBo> { };

                            AccountingPeriod = bo.AccountingPeriod;

                            var retro1Ifrs4 = ProcessAccountingPeriodIfrs4(bo, 1);
                            retroRegisters.Add(retro1Ifrs4);

                            var retro1Ifrs17 = ProcessAccountingPeriodIfrs17(bo, 1);
                            if (!retro1Ifrs17.IsNullOrEmpty())
                                retroRegisters.AddRange(retro1Ifrs17);

                            var retroStatementNo = GenerateStatementNo(businessOriginCode, CurrentDate.Year);
                            foreach (var retroRegister in retroRegisters)
                            {
                                var rDb = retroRegister;
                                rDb.RetroRegisterBatchId = RetroRegisterBatchBo.Id;
                                rDb.Type = RetroRegisterBo.TypeDirectRetro;
                                rDb.RetroStatementNo = retroStatementNo;
                                rDb.CreatedById = RetroRegisterBatchBo.CreatedById;
                                rDb.UpdatedById = RetroRegisterBatchBo.UpdatedById;
                                RetroRegisterService.Create(ref rDb);
                            }

                            TotalRecord++;
                        }

                        if (!string.IsNullOrEmpty(bo.AccountingPeriod2))
                        {
                            WriteStatusLogFile(string.Format("Computing Quartely Report Quarter: {0}", bo.AccountingPeriod2));
                            var retroRegisters = new List<RetroRegisterBo> { };

                            AccountingPeriod = bo.AccountingPeriod2;

                            var retro2Ifrs4 = ProcessAccountingPeriodIfrs4(bo, 2);
                            retroRegisters.Add(retro2Ifrs4);

                            var retro2Ifrs17 = ProcessAccountingPeriodIfrs17(bo, 2);
                            if (!retro2Ifrs17.IsNullOrEmpty())
                                retroRegisters.AddRange(retro2Ifrs17);

                            var retroStatementNo = GenerateStatementNo(businessOriginCode, CurrentDate.Year);
                            foreach (var retroRegister in retroRegisters)
                            {
                                var rDb = retroRegister;
                                rDb.RetroRegisterBatchId = RetroRegisterBatchBo.Id;
                                rDb.Type = RetroRegisterBo.TypeDirectRetro;
                                rDb.RetroStatementNo = retroStatementNo;
                                rDb.CreatedById = RetroRegisterBatchBo.CreatedById;
                                rDb.UpdatedById = RetroRegisterBatchBo.UpdatedById;
                                RetroRegisterService.Create(ref rDb);
                            }

                            TotalRecord++;
                        }

                        if (!string.IsNullOrEmpty(bo.AccountingPeriod3))
                        {
                            WriteStatusLogFile(string.Format("Computing Quartely Report Quarter: {0}", bo.AccountingPeriod3));
                            var retroRegisters = new List<RetroRegisterBo> { };

                            AccountingPeriod = bo.AccountingPeriod3;

                            var retro3Ifrs4 = ProcessAccountingPeriodIfrs4(bo, 3);
                            retroRegisters.Add(retro3Ifrs4);

                            var retro3Ifrs17 = ProcessAccountingPeriodIfrs17(bo, 3);
                            if (!retro3Ifrs17.IsNullOrEmpty())
                                retroRegisters.AddRange(retro3Ifrs17);

                            var retroStatementNo = GenerateStatementNo(businessOriginCode, CurrentDate.Year);
                            foreach (var retroRegister in retroRegisters)
                            {
                                var rDb = retroRegister;
                                rDb.RetroRegisterBatchId = RetroRegisterBatchBo.Id;
                                rDb.Type = RetroRegisterBo.TypeDirectRetro;
                                rDb.RetroStatementNo = retroStatementNo;
                                rDb.CreatedById = RetroRegisterBatchBo.CreatedById;
                                rDb.UpdatedById = RetroRegisterBatchBo.UpdatedById;
                                RetroRegisterService.Create(ref rDb);
                            }

                            TotalRecord++;
                        }

                        WriteStatusLogFile(string.Format("Completed Compute Quartely Report format: {0}", bo.Id), true);
                    }
                }

                SetProcessCount("Saved");
            }

            WriteStatusLogFile("Completed Compute Quartely Report", true);
            WriteStatusLogFile("Completed Create Retro Register...", true);
        }        

        public RetroRegisterBo ProcessAccountingPeriodIfrs4(RetroStatementBo bo, int accountingPeriodNo)
        {
            var retroRegisterBo = new RetroRegisterBo
            {
                ReportingType = RetroRegisterBo.ReportingTypeIFRS4,

                RetroPartyId = bo.RetroPartyId,
                OriginalSoaQuarter = bo.DirectRetroBo.SoaQuarter,
                DirectRetroId = bo.DirectRetroId,

                CedantId = bo.DirectRetroBo.CedantId,
                TreatyCodeId = bo.DirectRetroBo.TreatyCodeId,
                TreatyNumber = bo.TreatyNo,
                Schedule = bo.Schedule,
                TreatyType = bo.TreatyType,
                AccountFor = bo.AccountsFor,
                PreparedById = RetroRegisterBatchBo.CreatedById,
                RetroConfirmationDate = null,

                RiskQuarter = AccountingPeriod,
            };

            switch (accountingPeriodNo)
            {
                case 1:
                    retroRegisterBo.ReserveCededBegin = bo.ReserveCededBegin;
                    retroRegisterBo.ReserveCededEnd = bo.ReserveCededEnd;
                    retroRegisterBo.RiskChargeCededBegin = bo.RiskChargeCededBegin;
                    retroRegisterBo.RiskChargeCededEnd = bo.RiskChargeCededEnd;
                    retroRegisterBo.AverageReserveCeded = bo.AverageReserveCeded;

                    retroRegisterBo.Gross1st = bo.RiPremiumNB;
                    retroRegisterBo.GrossRen = bo.RiPremiumRN;
                    retroRegisterBo.AltPremium = bo.RiPremiumALT;

                    retroRegisterBo.Discount1st = bo.RiDiscountNB;
                    retroRegisterBo.DiscountRen = bo.RiDiscountRN;
                    retroRegisterBo.DiscountAlt = bo.RiDiscountALT;

                    retroRegisterBo.RiskPremium = bo.QuarterlyRiskPremium;
                    retroRegisterBo.Claims = bo.Claims;
                    retroRegisterBo.ProfitCommission = bo.ProfitComm;
                    retroRegisterBo.SurrenderVal = bo.SurrenderValue;
                    retroRegisterBo.RetrocessionMarketingFee = bo.RetrocessionMarketingFee;
                    retroRegisterBo.AgreedDBCommission = bo.AgreedDatabaseComm;
                    retroRegisterBo.NoClaimBonus = bo.NoClaimBonus;
                    retroRegisterBo.GstPayable = bo.GstPayable;

                    retroRegisterBo.NbCession = bo.TotalNoOfPolicyNB;
                    retroRegisterBo.RnCession = bo.TotalNoOfPolicyRN;
                    retroRegisterBo.AltCession = bo.TotalNoOfPolicyALT;

                    retroRegisterBo.NbSumReins = Util.RoundNullableValue(bo.TotalSumReinsuredNB, 2);
                    retroRegisterBo.RnSumReins = Util.RoundNullableValue(bo.TotalSumReinsuredRN, 2);
                    retroRegisterBo.AltSumReins = Util.RoundNullableValue(bo.TotalSumReinsuredALT, 2);
                    break;
                case 2:
                    retroRegisterBo.ReserveCededBegin = bo.ReserveCededBegin2;
                    retroRegisterBo.ReserveCededEnd = bo.ReserveCededEnd2;
                    retroRegisterBo.RiskChargeCededBegin = bo.RiskChargeCededBegin2;
                    retroRegisterBo.RiskChargeCededEnd = bo.RiskChargeCededEnd2;
                    retroRegisterBo.AverageReserveCeded = bo.AverageReserveCeded2;

                    retroRegisterBo.Gross1st = bo.RiPremiumNB2;
                    retroRegisterBo.GrossRen = bo.RiPremiumRN2;
                    retroRegisterBo.AltPremium = bo.RiPremiumALT2;

                    retroRegisterBo.Discount1st = bo.RiDiscountNB2;
                    retroRegisterBo.DiscountRen = bo.RiDiscountRN2;
                    retroRegisterBo.DiscountAlt = bo.RiDiscountALT2;

                    retroRegisterBo.RiskPremium = bo.QuarterlyRiskPremium2;
                    retroRegisterBo.Claims = bo.Claims2;
                    retroRegisterBo.ProfitCommission = bo.ProfitComm2;
                    retroRegisterBo.SurrenderVal = bo.SurrenderValue2;
                    retroRegisterBo.RetrocessionMarketingFee = bo.RetrocessionMarketingFee2;
                    retroRegisterBo.AgreedDBCommission = bo.AgreedDatabaseComm2;
                    retroRegisterBo.NoClaimBonus = bo.NoClaimBonus2;
                    retroRegisterBo.GstPayable = bo.GstPayable2;

                    retroRegisterBo.NbCession = bo.TotalNoOfPolicyNB2;
                    retroRegisterBo.RnCession = bo.TotalNoOfPolicyRN2;
                    retroRegisterBo.AltCession = bo.TotalNoOfPolicyALT2;

                    retroRegisterBo.NbSumReins = Util.RoundNullableValue(bo.TotalSumReinsuredNB2, 2);
                    retroRegisterBo.RnSumReins = Util.RoundNullableValue(bo.TotalSumReinsuredRN2, 2);
                    retroRegisterBo.AltSumReins = Util.RoundNullableValue(bo.TotalSumReinsuredALT2, 2);
                    break;
                case 3:
                    retroRegisterBo.ReserveCededBegin = bo.ReserveCededBegin3;
                    retroRegisterBo.ReserveCededEnd = bo.ReserveCededEnd3;
                    retroRegisterBo.RiskChargeCededBegin = bo.RiskChargeCededBegin3;
                    retroRegisterBo.RiskChargeCededEnd = bo.RiskChargeCededEnd3;
                    retroRegisterBo.AverageReserveCeded = bo.AverageReserveCeded3;

                    retroRegisterBo.Gross1st = bo.RiPremiumNB3;
                    retroRegisterBo.GrossRen = bo.RiPremiumRN3;
                    retroRegisterBo.AltPremium = bo.RiPremiumALT3;

                    retroRegisterBo.Discount1st = bo.RiDiscountNB3;
                    retroRegisterBo.DiscountRen = bo.RiDiscountRN3;
                    retroRegisterBo.DiscountAlt = bo.RiDiscountALT3;

                    retroRegisterBo.RiskPremium = bo.QuarterlyRiskPremium3;
                    retroRegisterBo.Claims = bo.Claims3;
                    retroRegisterBo.ProfitCommission = bo.ProfitComm3;
                    retroRegisterBo.SurrenderVal = bo.SurrenderValue3;
                    retroRegisterBo.RetrocessionMarketingFee = bo.RetrocessionMarketingFee3;
                    retroRegisterBo.AgreedDBCommission = bo.AgreedDatabaseComm3;
                    retroRegisterBo.NoClaimBonus = bo.NoClaimBonus3;
                    retroRegisterBo.GstPayable = bo.GstPayable3;

                    retroRegisterBo.NbCession = bo.TotalNoOfPolicyNB3;
                    retroRegisterBo.RnCession = bo.TotalNoOfPolicyRN3;
                    retroRegisterBo.AltCession = bo.TotalNoOfPolicyALT3;

                    retroRegisterBo.NbSumReins = Util.RoundNullableValue(bo.TotalSumReinsuredNB3, 2);
                    retroRegisterBo.RnSumReins = Util.RoundNullableValue(bo.TotalSumReinsuredRN3, 2);
                    retroRegisterBo.AltSumReins = Util.RoundNullableValue(bo.TotalSumReinsuredALT3, 2);
                    break;
            }

            retroRegisterBo.GetYear1st();
            retroRegisterBo.GetRenewal();
            retroRegisterBo.GetValuationGross1st();
            retroRegisterBo.GetValuationGrossRen();
            retroRegisterBo.GetValuationDiscount1st();
            retroRegisterBo.GetValuationDiscountRen();
            retroRegisterBo.GetValuationCom1st();
            retroRegisterBo.GetValuationComRen();
            retroRegisterBo.GetNetTotalAmount();

            retroRegisterBo.RetroStatementDate = RetroRegisterBatchBo.BatchDate;
            retroRegisterBo.ReportCompletedDate = RetroRegisterBatchBo.BatchDate;
            retroRegisterBo.SendToRetroDate = AddBusinessDays(RetroRegisterBatchBo.BatchDate, 2, PublicHolidayDetailBos);

            var businessOriginCode = "";
            var treatyCode = TreatyCodeService.Find(bo.DirectRetroBo.TreatyCodeId);
            if (treatyCode != null)
            {
                if (treatyCode.TreatyBo != null)
                {
                    if (treatyCode.TreatyBo.BusinessOriginPickListDetailBo != null)
                        businessOriginCode = treatyCode.TreatyBo.BusinessOriginPickListDetailBo.Code;

                    if (treatyCode.LineOfBusinessPickListDetailBo != null)
                        retroRegisterBo.LOB = treatyCode.LineOfBusinessPickListDetailBo.Code;
                }

                if (bo.DirectRetroBo.SoaDataBatchBo.RiDataBatchId.HasValue)
                    retroRegisterBo.Frequency = GetPremiumFrequencyCode(bo.DirectRetroBo.SoaDataBatchBo.RiDataBatchId, treatyCode.Code);
            }
            retroRegisterBo.RetroStatementType = RetroRegisterBo.GetTypeByCode(businessOriginCode);

            return retroRegisterBo;
        }

        public List<RetroRegisterBo> ProcessAccountingPeriodIfrs17(RetroStatementBo bo, int accountingPeriodNo)
        {
            var retroRegisters = new List<RetroRegisterBo> { };

            var retroData = ProcessDataIfrs17(bo);
            if (!retroData.IsNullOrEmpty())
                retroRegisters.AddRange(retroData);

            var retroOmittedClaim = ProcessDataOmittedClaimIfrs17(retroRegisters, bo);
            if (!retroOmittedClaim.IsNullOrEmpty())
                retroRegisters.AddRange(retroOmittedClaim);

            var retroWithoutAnnualCohort = ProcessDataWithoutAnnualCohortIfrs17(bo, accountingPeriodNo);
            retroRegisters.Add(retroWithoutAnnualCohort);

            // Risk Premium update by multiply with premium ratio 
            double totalPremiums = retroRegisters.Sum(q => q.Gross1st) ?? 0;
            totalPremiums += retroRegisters.Sum(q => q.GrossRen) ?? 0;
            totalPremiums += retroRegisters.Sum(q => q.AltPremium) ?? 0;

            double retroStatementRiskPremium = 0;
            switch (accountingPeriodNo)
            {
                case 1:
                    retroStatementRiskPremium = bo.QuarterlyRiskPremium.GetValueOrDefault();
                    break;
                case 2:
                    retroStatementRiskPremium = bo.QuarterlyRiskPremium2.GetValueOrDefault();
                    break;
                case 3:
                    retroStatementRiskPremium = bo.QuarterlyRiskPremium3.GetValueOrDefault();
                    break;
            }

            foreach (var retroRegister in retroRegisters.Where(q => q.AnnualCohort.HasValue).OrderBy(q => q.AnnualCohort))
            {
                double premiums = retroRegister.Gross1st.GetValueOrDefault() + retroRegister.GrossRen.GetValueOrDefault() + retroRegister.AltPremium.GetValueOrDefault();

                double RiskPremium = (premiums / totalPremiums) * retroStatementRiskPremium;
                if (Equals(double.NaN, RiskPremium)) RiskPremium = 0;

                retroRegister.RiskPremium = Util.RoundNullableValue(RiskPremium, 2);
            }

            return retroRegisters;
        }

        public List<RetroRegisterBo> ProcessDataIfrs17(RetroStatementBo bo)
        {
            var retroRegisters = new List<RetroRegisterBo> { };
            foreach (var retroSummaryGroupBy in QueryRetroSummaryGroupBy(bo.RetroPartyBo.Party, AccountingPeriod))
            {
                var retroRegisterBo = new RetroRegisterBo
                {
                    ReportingType = RetroRegisterBo.ReportingTypeIFRS17,

                    RetroPartyId = bo.RetroPartyId,
                    OriginalSoaQuarter = bo.DirectRetroBo.SoaQuarter,
                    DirectRetroId = bo.DirectRetroId,

                    CedantId = bo.DirectRetroBo.CedantId,
                    TreatyCodeId = bo.DirectRetroBo.TreatyCodeId,
                    TreatyNumber = bo.TreatyNo,
                    Schedule = bo.Schedule,
                    TreatyType = bo.TreatyType,
                    AccountFor = bo.AccountsFor,
                    PreparedById = RetroRegisterBatchBo.CreatedById,
                    RetroConfirmationDate = null,

                    RiskQuarter = AccountingPeriod,
                    AnnualCohort = retroSummaryGroupBy.Mfrs17AnnualCohort,
                    ContractCode = retroSummaryGroupBy.Mfrs17ContractCode,
                };

                var nbs = RetroSummaryBos.Where(q => q.Type == PickListDetailBo.TransactionTypeCodeNewBusiness).Where(q => q.RiskQuarter == AccountingPeriod)
                    .Where(q => q.Mfrs17AnnualCohort == retroSummaryGroupBy.Mfrs17AnnualCohort).Where(q => q.Mfrs17ContractCode == retroSummaryGroupBy.Mfrs17ContractCode).ToList();
                var rns = RetroSummaryBos.Where(q => q.Type == PickListDetailBo.TransactionTypeCodeRenewal).Where(q => q.RiskQuarter == AccountingPeriod)
                    .Where(q => q.Mfrs17AnnualCohort == retroSummaryGroupBy.Mfrs17AnnualCohort).Where(q => q.Mfrs17ContractCode == retroSummaryGroupBy.Mfrs17ContractCode).ToList();
                var als = RetroSummaryBos.Where(q => q.Type == PickListDetailBo.TransactionTypeCodeAlteration).Where(q => q.RiskQuarter == AccountingPeriod)
                    .Where(q => q.Mfrs17AnnualCohort == retroSummaryGroupBy.Mfrs17AnnualCohort).Where(q => q.Mfrs17ContractCode == retroSummaryGroupBy.Mfrs17ContractCode).ToList();

                QuarterObject qo = new QuarterObject(AccountingPeriod);
                double claimAmount3 = 0;

                if (nbs != null && nbs.Count() > 0)
                {
                    var retro1 = nbs.Where(q => q.RetroParty1 == bo.RetroPartyBo.Party).ToList();
                    var retro2 = nbs.Where(q => q.RetroParty2 == bo.RetroPartyBo.Party).ToList();
                    var retro3 = nbs.Where(q => q.RetroParty3 == bo.RetroPartyBo.Party).ToList();

                    double riPremium3 = retro1.Sum(q => q.RetroRiPremium1) ?? 0;
                    riPremium3 += retro2.Sum(q => q.RetroRiPremium2) ?? 0;
                    riPremium3 += retro3.Sum(q => q.RetroRiPremium3) ?? 0;
                    retroRegisterBo.Gross1st = Util.RoundNullableValue(riPremium3, 2);

                    double riDiscount3 = retro1.Sum(q => q.RetroDiscount1) ?? 0;
                    riDiscount3 += retro2.Sum(q => q.RetroDiscount2) ?? 0;
                    riDiscount3 += retro3.Sum(q => q.RetroDiscount3) ?? 0;
                    retroRegisterBo.Discount1st = Util.RoundNullableValue(riDiscount3, 2);

                    retroRegisterBo.NbCession = RiDataService.CountNoOfPolicyByRetroParty(bo.DirectRetroBo, PickListDetailBo.TransactionTypeCodeNewBusiness, bo.RetroPartyBo.Party, qo.MonthStart, qo.MonthEnd, qo.Year, retroSummaryGroupBy.Mfrs17ContractCode, retroSummaryGroupBy.Mfrs17AnnualCohort);
                    retroRegisterBo.NbSumReins = Util.RoundNullableValue(RiDataService.CountSumRiAmountByRetroParty(bo.DirectRetroBo, PickListDetailBo.TransactionTypeCodeNewBusiness, bo.RetroPartyBo.Party, qo.MonthStart, qo.MonthEnd, qo.Year, retroSummaryGroupBy.Mfrs17ContractCode, retroSummaryGroupBy.Mfrs17AnnualCohort), 2);
                }

                if (rns != null && rns.Count() > 0)
                {
                    var retro1 = rns.Where(q => q.RetroParty1 == bo.RetroPartyBo.Party).ToList();
                    var retro2 = rns.Where(q => q.RetroParty2 == bo.RetroPartyBo.Party).ToList();
                    var retro3 = rns.Where(q => q.RetroParty3 == bo.RetroPartyBo.Party).ToList();

                    double riPremium3 = retro1.Sum(q => q.RetroRiPremium1) ?? 0;
                    riPremium3 += retro2.Sum(q => q.RetroRiPremium2) ?? 0;
                    riPremium3 += retro3.Sum(q => q.RetroRiPremium3) ?? 0;
                    retroRegisterBo.GrossRen = Util.RoundNullableValue(riPremium3, 2);

                    double riDiscount3 = retro1.Sum(q => q.RetroDiscount1) ?? 0;
                    riDiscount3 += retro2.Sum(q => q.RetroDiscount2) ?? 0;
                    riDiscount3 += retro3.Sum(q => q.RetroDiscount3) ?? 0;
                    retroRegisterBo.DiscountRen = Util.RoundNullableValue(riDiscount3, 2);

                    retroRegisterBo.RnCession = RiDataService.CountNoOfPolicyByRetroParty(bo.DirectRetroBo, PickListDetailBo.TransactionTypeCodeRenewal, bo.RetroPartyBo.Party, qo.MonthStart, qo.MonthEnd, qo.Year, retroSummaryGroupBy.Mfrs17ContractCode, retroSummaryGroupBy.Mfrs17AnnualCohort);
                    retroRegisterBo.RnSumReins = Util.RoundNullableValue(RiDataService.CountSumRiAmountByRetroParty(bo.DirectRetroBo, PickListDetailBo.TransactionTypeCodeRenewal, bo.RetroPartyBo.Party, qo.MonthStart, qo.MonthEnd, qo.Year, retroSummaryGroupBy.Mfrs17ContractCode, retroSummaryGroupBy.Mfrs17AnnualCohort), 2);
                }

                if (als != null && als.Count() > 0)
                {
                    var retro1 = als.Where(q => q.RetroParty1 == bo.RetroPartyBo.Party).ToList();
                    var retro2 = als.Where(q => q.RetroParty2 == bo.RetroPartyBo.Party).ToList();
                    var retro3 = als.Where(q => q.RetroParty3 == bo.RetroPartyBo.Party).ToList();

                    double riPremium3 = retro1.Sum(q => q.RetroRiPremium1) ?? 0;
                    riPremium3 += retro2.Sum(q => q.RetroRiPremium2) ?? 0;
                    riPremium3 += retro3.Sum(q => q.RetroRiPremium3) ?? 0;
                    retroRegisterBo.AltPremium = Util.RoundNullableValue(riPremium3, 2);

                    double riDiscount3 = retro1.Sum(q => q.RetroDiscount1) ?? 0;
                    riDiscount3 += retro2.Sum(q => q.RetroDiscount2) ?? 0;
                    riDiscount3 += retro3.Sum(q => q.RetroDiscount3) ?? 0;
                    retroRegisterBo.DiscountAlt = Util.RoundNullableValue(riDiscount3, 2);

                    retroRegisterBo.AltCession = RiDataService.CountNoOfPolicyByRetroParty(bo.DirectRetroBo, PickListDetailBo.TransactionTypeCodeAlteration, bo.RetroPartyBo.Party, qo.MonthStart, qo.MonthEnd, qo.Year, retroSummaryGroupBy.Mfrs17ContractCode, retroSummaryGroupBy.Mfrs17AnnualCohort);
                    retroRegisterBo.AltSumReins = Util.RoundNullableValue(RiDataService.CountSumRiAmountByRetroParty(bo.DirectRetroBo, PickListDetailBo.TransactionTypeCodeAlteration, bo.RetroPartyBo.Party, qo.MonthStart, qo.MonthEnd, qo.Year, retroSummaryGroupBy.Mfrs17ContractCode, retroSummaryGroupBy.Mfrs17AnnualCohort), 2);
                }

                if (bo.DirectRetroBo.SoaQuarter == AccountingPeriod)
                {
                    var claimRetro1 = RetroSummaryBos.Where(q => q.RetroParty1 == bo.RetroPartyBo.Party).Where(q => q.Mfrs17AnnualCohort == retroSummaryGroupBy.Mfrs17AnnualCohort).Where(q => q.Mfrs17ContractCode == retroSummaryGroupBy.Mfrs17ContractCode).ToList();
                    var claimRetro2 = RetroSummaryBos.Where(q => q.RetroParty2 == bo.RetroPartyBo.Party).Where(q => q.Mfrs17AnnualCohort == retroSummaryGroupBy.Mfrs17AnnualCohort).Where(q => q.Mfrs17ContractCode == retroSummaryGroupBy.Mfrs17ContractCode).ToList();
                    var claimRetro3 = RetroSummaryBos.Where(q => q.RetroParty3 == bo.RetroPartyBo.Party).Where(q => q.Mfrs17AnnualCohort == retroSummaryGroupBy.Mfrs17AnnualCohort).Where(q => q.Mfrs17ContractCode == retroSummaryGroupBy.Mfrs17ContractCode).ToList();

                    claimAmount3 += claimRetro1.Sum(q => q.RetroClaims1) ?? 0;
                    claimAmount3 += claimRetro2.Sum(q => q.RetroClaims2) ?? 0;
                    claimAmount3 += claimRetro3.Sum(q => q.RetroClaims3) ?? 0;
                }

                retroRegisterBo.Claims = Util.RoundNullableValue(claimAmount3, 2);
                retroRegisterBo.NoClaimBonus = Util.RoundNullableValue(RiDataService.CountTotalNoClaimBonus(bo.DirectRetroBo, bo.RetroPartyBo.Party, qo.MonthStart, qo.MonthEnd, qo.Year, retroSummaryGroupBy.Mfrs17ContractCode, retroSummaryGroupBy.Mfrs17AnnualCohort), 2);
                retroRegisterBo.AgreedDBCommission = Util.RoundNullableValue(RiDataService.CountTotalDatabaseCommission(bo.DirectRetroBo, bo.RetroPartyBo.Party, qo.MonthStart, qo.MonthEnd, qo.Year, retroSummaryGroupBy.Mfrs17ContractCode, retroSummaryGroupBy.Mfrs17AnnualCohort), 2);

                retroRegisterBo.GetYear1st();
                retroRegisterBo.GetRenewal();
                retroRegisterBo.GetValuationGross1st();
                retroRegisterBo.GetValuationGrossRen();
                retroRegisterBo.GetValuationDiscount1st();
                retroRegisterBo.GetValuationDiscountRen();
                retroRegisterBo.GetValuationCom1st();
                retroRegisterBo.GetValuationComRen();
                retroRegisterBo.GetNetTotalAmount();

                retroRegisterBo.RetroStatementDate = RetroRegisterBatchBo.BatchDate;
                retroRegisterBo.ReportCompletedDate = RetroRegisterBatchBo.BatchDate;
                retroRegisterBo.SendToRetroDate = AddBusinessDays(RetroRegisterBatchBo.BatchDate, 2, PublicHolidayDetailBos);

                var businessOriginCode = "";
                var treatyCode = TreatyCodeService.Find(bo.DirectRetroBo.TreatyCodeId);
                if (treatyCode != null)
                {
                    if (treatyCode.TreatyBo != null)
                    {
                        if (treatyCode.TreatyBo.BusinessOriginPickListDetailBo != null)
                            businessOriginCode = treatyCode.TreatyBo.BusinessOriginPickListDetailBo.Code;

                        if (treatyCode.LineOfBusinessPickListDetailBo != null)
                            retroRegisterBo.LOB = treatyCode.LineOfBusinessPickListDetailBo.Code;
                    }

                    if (bo.DirectRetroBo.SoaDataBatchBo.RiDataBatchId.HasValue)
                        retroRegisterBo.Frequency = GetPremiumFrequencyCode(bo.DirectRetroBo.SoaDataBatchBo.RiDataBatchId, treatyCode.Code);
                }
                retroRegisterBo.RetroStatementType = RetroRegisterBo.GetTypeByCode(businessOriginCode);

                retroRegisters.Add(retroRegisterBo);
            }
            return retroRegisters;
        }

        public List<RetroRegisterBo> ProcessDataOmittedClaimIfrs17(List<RetroRegisterBo> retroRegistersBos, RetroStatementBo bo)
        {
            // If no RI Data under risk qtr 17Q4 with 2016 annual cohort but there is claim data with 2016 annual cohort in Ri Summary. 
            // Hence, system to capture claim amount for 2016 annual cohort under new row of 2016.
            var retroRegisters = new List<RetroRegisterBo> { };
            if (!retroRegistersBos.IsNullOrEmpty())
            {
                var existAnnualCohorts = retroRegistersBos.Select(q => q.AnnualCohort).ToArray();
                var RetroSummaryClaims = RetroSummaryBos
                    .Where(q => q.RetroParty1 == bo.RetroPartyBo.Party || q.RetroParty2 == bo.RetroPartyBo.Party || q.RetroParty3 == bo.RetroPartyBo.Party)
                    .Where(q => string.IsNullOrEmpty(q.RiskQuarter)).ToList();

                foreach (var retroSummaryGroupBy in RetroSummaryClaims.Where(q => !existAnnualCohorts.Contains(q.Mfrs17AnnualCohort)))
                {
                    var retroRegisterBo = new RetroRegisterBo
                    {
                        ReportingType = RetroRegisterBo.ReportingTypeIFRS17,

                        RetroPartyId = bo.RetroPartyId,
                        OriginalSoaQuarter = bo.DirectRetroBo.SoaQuarter,
                        DirectRetroId = bo.DirectRetroId,

                        CedantId = bo.DirectRetroBo.CedantId,
                        TreatyCodeId = bo.DirectRetroBo.TreatyCodeId,
                        TreatyNumber = bo.TreatyNo,
                        Schedule = bo.Schedule,
                        TreatyType = bo.TreatyType,
                        AccountFor = bo.AccountsFor,
                        PreparedById = RetroRegisterBatchBo.CreatedById,
                        RetroConfirmationDate = null,

                        RiskQuarter = AccountingPeriod,
                        AnnualCohort = retroSummaryGroupBy.Mfrs17AnnualCohort,
                        ContractCode = retroSummaryGroupBy.Mfrs17ContractCode,
                    };

                    double claimAmount = 0;
                    if (bo.DirectRetroBo.SoaQuarter == AccountingPeriod)
                    {
                        if (retroSummaryGroupBy.RetroParty1 == bo.RetroPartyBo.Party) claimAmount += retroSummaryGroupBy.RetroClaims1 ?? 0;
                        if (retroSummaryGroupBy.RetroParty2 == bo.RetroPartyBo.Party) claimAmount += retroSummaryGroupBy.RetroClaims2 ?? 0;
                        if (retroSummaryGroupBy.RetroParty3 == bo.RetroPartyBo.Party) claimAmount += retroSummaryGroupBy.RetroClaims3 ?? 0;
                    }
                    retroRegisterBo.Claims = Util.RoundNullableValue(claimAmount, 2);
                    retroRegisterBo.GetNetTotalAmount();

                    retroRegisterBo.RetroStatementDate = RetroRegisterBatchBo.BatchDate;
                    retroRegisterBo.ReportCompletedDate = RetroRegisterBatchBo.BatchDate;
                    retroRegisterBo.SendToRetroDate = AddBusinessDays(RetroRegisterBatchBo.BatchDate, 2, PublicHolidayDetailBos);

                    var businessOriginCode = "";
                    var treatyCode = TreatyCodeService.Find(bo.DirectRetroBo.TreatyCodeId);
                    if (treatyCode != null)
                    {
                        if (treatyCode.TreatyBo != null)
                        {
                            if (treatyCode.TreatyBo.BusinessOriginPickListDetailBo != null)
                                businessOriginCode = treatyCode.TreatyBo.BusinessOriginPickListDetailBo.Code;

                            if (treatyCode.LineOfBusinessPickListDetailBo != null)
                                retroRegisterBo.LOB = treatyCode.LineOfBusinessPickListDetailBo.Code;
                        }

                        if (bo.DirectRetroBo.SoaDataBatchBo.RiDataBatchId.HasValue)
                            retroRegisterBo.Frequency = GetPremiumFrequencyCode(bo.DirectRetroBo.SoaDataBatchBo.RiDataBatchId, treatyCode.Code);
                    }
                    retroRegisterBo.RetroStatementType = RetroRegisterBo.GetTypeByCode(businessOriginCode);

                    retroRegisters.Add(retroRegisterBo);
                }
            }
            return retroRegisters;
        }

        public RetroRegisterBo ProcessDataWithoutAnnualCohortIfrs17(RetroStatementBo bo, int accountingPeriodNo)
        {
            var retroRegisterBo = new RetroRegisterBo
            {
                ReportingType = RetroRegisterBo.ReportingTypeIFRS17,

                RetroPartyId = bo.RetroPartyId,
                OriginalSoaQuarter = bo.DirectRetroBo.SoaQuarter,
                DirectRetroId = bo.DirectRetroId,

                CedantId = bo.DirectRetroBo.CedantId,
                TreatyCodeId = bo.DirectRetroBo.TreatyCodeId,
                TreatyNumber = bo.TreatyNo,
                Schedule = bo.Schedule,
                TreatyType = bo.TreatyType,
                AccountFor = bo.AccountsFor,
                PreparedById = RetroRegisterBatchBo.CreatedById,
                RetroConfirmationDate = null,

                RiskQuarter = AccountingPeriod,
            };

            switch (accountingPeriodNo)
            {
                case 1:
                    retroRegisterBo.ReserveCededBegin = bo.ReserveCededBegin;
                    retroRegisterBo.ReserveCededEnd = bo.ReserveCededEnd;
                    retroRegisterBo.RiskChargeCededBegin = bo.RiskChargeCededBegin;
                    retroRegisterBo.RiskChargeCededEnd = bo.RiskChargeCededEnd;
                    retroRegisterBo.AverageReserveCeded = bo.AverageReserveCeded;

                    retroRegisterBo.Claims = bo.Claims;
                    retroRegisterBo.ProfitCommission = bo.ProfitComm;
                    retroRegisterBo.SurrenderVal = bo.SurrenderValue;
                    retroRegisterBo.RetrocessionMarketingFee = bo.RetrocessionMarketingFee;
                    retroRegisterBo.AgreedDBCommission = bo.AgreedDatabaseComm;
                    retroRegisterBo.NoClaimBonus = bo.NoClaimBonus;
                    retroRegisterBo.GstPayable = bo.GstPayable;
                    break;
                case 2:
                    retroRegisterBo.ReserveCededBegin = bo.ReserveCededBegin2;
                    retroRegisterBo.ReserveCededEnd = bo.ReserveCededEnd2;
                    retroRegisterBo.RiskChargeCededBegin = bo.RiskChargeCededBegin2;
                    retroRegisterBo.RiskChargeCededEnd = bo.RiskChargeCededEnd2;
                    retroRegisterBo.AverageReserveCeded = bo.AverageReserveCeded2;

                    retroRegisterBo.Claims = bo.Claims2;
                    retroRegisterBo.ProfitCommission = bo.ProfitComm2;
                    retroRegisterBo.SurrenderVal = bo.SurrenderValue2;
                    retroRegisterBo.RetrocessionMarketingFee = bo.RetrocessionMarketingFee2;
                    retroRegisterBo.AgreedDBCommission = bo.AgreedDatabaseComm2;
                    retroRegisterBo.NoClaimBonus = bo.NoClaimBonus2;
                    retroRegisterBo.GstPayable = bo.GstPayable2;
                    break;
                case 3:
                    retroRegisterBo.ReserveCededBegin = bo.ReserveCededBegin3;
                    retroRegisterBo.ReserveCededEnd = bo.ReserveCededEnd3;
                    retroRegisterBo.RiskChargeCededBegin = bo.RiskChargeCededBegin3;
                    retroRegisterBo.RiskChargeCededEnd = bo.RiskChargeCededEnd3;
                    retroRegisterBo.AverageReserveCeded = bo.AverageReserveCeded3;

                    retroRegisterBo.Claims = bo.Claims3;
                    retroRegisterBo.ProfitCommission = bo.ProfitComm3;
                    retroRegisterBo.SurrenderVal = bo.SurrenderValue3;
                    retroRegisterBo.RetrocessionMarketingFee = bo.RetrocessionMarketingFee3;
                    retroRegisterBo.AgreedDBCommission = bo.AgreedDatabaseComm3;
                    retroRegisterBo.NoClaimBonus = bo.NoClaimBonus3;
                    retroRegisterBo.GstPayable = bo.GstPayable3;
                    break;
            }

            retroRegisterBo.GetYear1st();
            retroRegisterBo.GetRenewal();
            retroRegisterBo.GetValuationGross1st();
            retroRegisterBo.GetValuationGrossRen();
            retroRegisterBo.GetValuationDiscount1st();
            retroRegisterBo.GetValuationDiscountRen();
            retroRegisterBo.GetValuationCom1st();
            retroRegisterBo.GetValuationComRen();
            retroRegisterBo.GetNetTotalAmount();

            retroRegisterBo.RetroStatementDate = RetroRegisterBatchBo.BatchDate;
            retroRegisterBo.ReportCompletedDate = RetroRegisterBatchBo.BatchDate;
            retroRegisterBo.SendToRetroDate = AddBusinessDays(RetroRegisterBatchBo.BatchDate, 2, PublicHolidayDetailBos);

            var businessOriginCode = "";
            var treatyCode = TreatyCodeService.Find(bo.DirectRetroBo.TreatyCodeId);
            if (treatyCode != null)
            {
                if (treatyCode.TreatyBo != null)
                {
                    if (treatyCode.TreatyBo.BusinessOriginPickListDetailBo != null)
                        businessOriginCode = treatyCode.TreatyBo.BusinessOriginPickListDetailBo.Code;

                    if (treatyCode.LineOfBusinessPickListDetailBo != null)
                        retroRegisterBo.LOB = treatyCode.LineOfBusinessPickListDetailBo.Code;
                }

                if (bo.DirectRetroBo.SoaDataBatchBo.RiDataBatchId.HasValue)
                    retroRegisterBo.Frequency = GetPremiumFrequencyCode(bo.DirectRetroBo.SoaDataBatchBo.RiDataBatchId, treatyCode.Code);
            }
            retroRegisterBo.RetroStatementType = RetroRegisterBo.GetTypeByCode(businessOriginCode);

            return retroRegisterBo;
        }

        public void UpdateRetroStatements()
        {
            if (RetroStatementBos != null)
            {
                WriteStatusLogFile("Starting Updating Retro Statement...", true);

                foreach (var retroStatementBo in RetroStatementBos)
                {
                    var bo = retroStatementBo;
                    bo.DateReportCompleted = RetroRegisterBatchBo.BatchDate;
                    bo.DateSendToRetro = AddBusinessDays(RetroRegisterBatchBo.BatchDate, 2, PublicHolidayDetailBos);
                    bo.UpdatedById = RetroRegisterBatchBo.UpdatedById;
                    RetroStatementService.Save(ref bo);

                    WriteStatusLogFile(string.Format("{0}: {1}", "Retro Statement ID", bo.Id));
                    WriteStatusLogFile(string.Format("{0}: {1}", "Date Report Completed", bo.DateReportCompleted?.ToString(Util.GetDateFormat())));
                    WriteStatusLogFile(string.Format("{0}: {1}", "Date Send to Retro", bo.DateSendToRetro?.ToString(Util.GetDateFormat())));

                }
                WriteStatusLogFile("Completed Updating Retro Statement...", true);
            }
        }

        public void UpdateDirectRetroStatus(int status)
        {
            WriteStatusLogFile("Starting Updating Status of Direct Retro...", true);

            foreach(var directRetroBo in DirectRetroBos)
            {
                var bo = directRetroBo;
                bo.RetroStatus = status;
                bo.UpdatedById = RetroRegisterBatchBo.UpdatedById;
                DirectRetroService.Save(ref bo);

                WriteStatusLogFile(string.Format("{0}: {1}", "Direct Retro ID", bo.Id));
                WriteStatusLogFile(string.Format("{0}: {1}", "Direct Retro Status", DirectRetroBo.GetRetroStatusName(status)));
            }

            WriteStatusLogFile("Completed Updating Status of Direct Retro...", true);
        }

        public RetroRegisterBatchBo LoadRetroRegisterBatchBo()
        {
            if (CutOffService.IsCutOffProcessing())
                return null;

            RetroRegisterBatchBo = RetroRegisterBatchService.FindByStatus(RetroRegisterBatchBo.StatusSubmitForProcessing);
            if (RetroRegisterBatchBo != null)
            {                
                List<int> ids = RetroRegisterBatchDirectRetroService.GetIdsByRetroRegisterBatchId(RetroRegisterBatchBo.Id);
                DirectRetroBos = DirectRetroService.GetByIds(ids);
                if (ids != null)
                {
                    LoadRetroStatementBo(ids);
                }
            }
            return RetroRegisterBatchBo;
        }

        public IList<RetroStatementBo> LoadRetroStatementBo(List<int> ids)
        {
            foreach (int id in ids)
            {
                RetroStatementBos = RetroStatementService.GetByDirectRetroId(id);
            }            
            return RetroStatementBos;
        }

        public void SetTotalRecord()
        {
            TrailObject trail = new TrailObject();
            var reporting = RetroRegisterBatchBo;
            RetroRegisterBatchBo.TotalInvoice = TotalRecord;
            Result result = RetroRegisterBatchService.Update(ref reporting, ref trail);
            UserTrailBo userTrailBo = new UserTrailBo(
                RetroRegisterBatchBo.Id,
                "Update Retro Register Batch",
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
                ObjectId = RetroRegisterBatchBo.Id,
                Status = status,
                CreatedById = User.DefaultSuperUserId,
                UpdatedById = User.DefaultSuperUserId,
            };
            StatusHistoryService.Create(ref statusBo, ref trail);

            var reporting = RetroRegisterBatchBo;
            RetroRegisterBatchBo.Status = status;
            Result result = RetroRegisterBatchService.Update(ref reporting, ref trail);
            UserTrailBo userTrailBo = new UserTrailBo(
                RetroRegisterBatchBo.Id,
                des,
                result,
                trail,
                User.DefaultSuperUserId
            );
            UserTrailService.Create(ref userTrailBo);

            if (status == RetroRegisterBatchBo.StatusProcessing)
                ProcessingStatusHistoryBo = statusBo;
        }

        public void DeleteRetroRegister()
        {
            WriteStatusLogFile("Deleting Retro Register...");
            RetroRegisterService.DeleteAllByRetroRegisterBatchId(RetroRegisterBatchBo.Id);
            WriteStatusLogFile("Deleted Retro Register", true);
        }

        public string GenerateStatementNo(string businessOriginCode, int currentYear)
        {
            return RetroRegisterService.GetNextStatementNo(currentYear, businessOriginCode);
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

        public void CreateStatusLogFile()
        {
            if (RetroRegisterBatchBo == null)
                return;
            if (ProcessingStatusHistoryBo == null)
                return;

            TrailObject trail = new TrailObject();
            RetroRegisterBatchStatusFileBo = new RetroRegisterBatchStatusFileBo
            {
                RetroRegisterBatchId = RetroRegisterBatchBo.Id,
                StatusHistoryId = ProcessingStatusHistoryBo.Id,
                StatusHistoryBo = ProcessingStatusHistoryBo,
                CreatedById = User.DefaultSuperUserId,
                UpdatedById = User.DefaultSuperUserId,
            };
            var fileBo = RetroRegisterBatchStatusFileBo;
            var result = RetroRegisterBatchStatusFileService.Create(ref fileBo, ref trail);

            UserTrailBo userTrailBo = new UserTrailBo(
                fileBo.Id,
                "Create Retro Register Batch Status File",
                result,
                trail,
                User.DefaultSuperUserId
            );
            UserTrailService.Create(ref userTrailBo);

            StatusLogFileFilePath = fileBo.GetFilePath();
            Util.MakeDir(StatusLogFileFilePath);
        }

        public DateTime AddBusinessDays(DateTime current, int days, IEnumerable<DateTime> holidays = null)
        {
            var sign = Math.Sign(days);
            var unsignedDays = Math.Abs(days);
            for (var i = 0; i < unsignedDays; i++)
            {
                do
                {
                    current = current.AddDays(sign);
                }
                while (current.DayOfWeek == DayOfWeek.Saturday
                    || current.DayOfWeek == DayOfWeek.Sunday
                    || (holidays != null && holidays.Contains(current.Date))
                    );
            }
            return current;
        }

        public List<RetroSummaryBo> QueryRetroSummaryGroupBy(string retroParty, string accountingPeriod)
        {
            var queryCount = RetroSummaryBos
                .Where(q => q.RetroParty1 == retroParty || q.RetroParty2 == retroParty || q.RetroParty3 == retroParty)
                .Where(q => q.RiskQuarter == accountingPeriod)
                .Count();

            if (queryCount != 0)
            {
                return RetroSummaryBos
                    .Where(q => q.RetroParty1 == retroParty || q.RetroParty2 == retroParty || q.RetroParty3 == retroParty)
                    .Where(q => q.RiskQuarter == accountingPeriod)
                    .GroupBy(q => new { q.Mfrs17AnnualCohort, q.Mfrs17ContractCode, q.RiskQuarter })
                    .Select(q => new RetroSummaryBo
                    {
                        RiskQuarter = q.Key.RiskQuarter,
                        Mfrs17ContractCode = q.Key.Mfrs17ContractCode,
                        Mfrs17AnnualCohort = q.Key.Mfrs17AnnualCohort,
                    })
                    .ToList();
            }
            else
            {
                return RetroSummaryBos
                    .Where(q => q.RetroParty1 == retroParty || q.RetroParty2 == retroParty || q.RetroParty3 == retroParty)
                    .GroupBy(q => new { q.Mfrs17AnnualCohort, q.Mfrs17ContractCode, q.RiskQuarter })
                    .Select(q => new RetroSummaryBo
                    {
                        RiskQuarter = q.Key.RiskQuarter,
                        Mfrs17ContractCode = q.Key.Mfrs17ContractCode,
                        Mfrs17AnnualCohort = q.Key.Mfrs17AnnualCohort,
                    })
                    .ToList();
            }
        }
    }
}
