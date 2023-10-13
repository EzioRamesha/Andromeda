using BusinessObject;
using BusinessObject.Identity;
using BusinessObject.SoaDatas;
using ConsoleApp.Commands.RawFiles.ClaimRegister;
using DataAccess.Entities.Identity;
using DataAccess.EntityFramework;
using Services;
using Services.Identity;
using Services.RiDatas;
using Services.SoaDatas;
using Shared;
using Shared.DataAccess;
using Shared.ProcessFile;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleApp.Commands.ProcessDatas
{
    public class ProcessDirectRetro : Command
    {
        public DirectRetroBo DirectRetroBo { get; set; }

        public DirectRetroConfigurationBo DirectRetroConfigurationBo { get; set; }

        public ModuleBo ModuleBo { get; set; }

        public StatusHistoryBo ProcessingStatusHistoryBo { get; set; }

        public DirectRetroStatusFileBo DirectRetroStatusFileBo { get; set; }

        public FinanceProvisioningBo FinanceProvisioningBo { get; set; }

        public string StatusLogFileFilePath { get; set; }

        public List<string> TransactionTypes { get; set; }

        public bool Error { get; set; } = false;

        public IList<ProcessDirectRetroRiData> ProcessDirectRetroRiData { get; set; }

        public int Take { get; set; } = 500;

        public bool IsEnabledDebug { get; set; } = false;

        public int TotalFailedTypeValidation { get; set; }
        public List<int> TotalFailedTypeValidationRiDataIds { get; set; }
        public int TotalFailedTypeNoRetroParty { get; set; }
        public List<int> TotalFailedTypeNoRetroPartyRiDataIds { get; set; }
        public int TotalFailedTypeExceedRetroParty { get; set; }
        public List<int> TotalFailedTypeExceedRetroPartyRiDataIds { get; set; }
        public int TotalFailedTypePremiumSpread { get; set; }
        public List<int> TotalFailedTypePremiumSpreadRiDataIds { get; set; }
        public int TotalFailedTypePremiumSpreadDetail { get; set; }
        public List<int> TotalFailedTypePremiumSpreadDetailRiDataIds { get; set; }

        public bool IsNoRiDataBatchLinked { get; set; } = false;
        public bool IsNoClaimRegisterBatchFound { get; set; } = false;

        public ProcessDirectRetro()
        {
            Title = "ProcessDirectRetro";
            Description = "To process RI Data, Claims and Retro Summary";
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
            ModuleBo = ModuleService.FindByController(ModuleBo.ModuleController.DirectRetro.ToString());
            TransactionTypes = new List<string> { PickListDetailBo.TransactionTypeCodeNewBusiness, PickListDetailBo.TransactionTypeCodeRenewal, PickListDetailBo.TransactionTypeCodeAlteration };
            Error = false;
            Take = Util.GetConfigInteger("ProcessDirectRetroItems", 500);
            IsEnabledDebug = Util.GetConfigBoolean("EnabledDirectRetroDebug");

            TotalFailedTypeValidation = 0;
            TotalFailedTypeNoRetroParty = 0;
            TotalFailedTypeExceedRetroParty = 0;
            TotalFailedTypePremiumSpread = 0;
            TotalFailedTypePremiumSpreadDetail = 0;

            TotalFailedTypeValidationRiDataIds = new List<int> { };
            TotalFailedTypeNoRetroPartyRiDataIds = new List<int> { };
            TotalFailedTypeExceedRetroPartyRiDataIds = new List<int> { };
            TotalFailedTypePremiumSpreadRiDataIds = new List<int> { };
            TotalFailedTypePremiumSpreadDetailRiDataIds = new List<int> { };

            IsNoRiDataBatchLinked = false;
            IsNoClaimRegisterBatchFound = false;
        }

        public override void Run()
        {
            try
            {
                if (DirectRetroService.CountByRetroStatus(DirectRetroBo.RetroStatusSubmitForProcessing) == 0)
                {
                    PrintMessage(MessageBag.NoDirectRetroPendingProcess);
                    return;
                }

                PrintStarting();

                while (LoadDirectRetroBo() != null)
                {
                    try
                    {
                        if (GetProcessCount("Process") > 0)
                            PrintProcessCount();
                        SetProcessCount("Process");

                        UpdateRetroStatus(DirectRetroBo.RetroStatusProcessing, "Processing Direct Retro");

                        PrintLine();
                        PrintDetail("Direct Retro Id", DirectRetroBo.Id);

                        CreateStatusLogFile();

                        if (File.Exists(StatusLogFileFilePath))
                            File.Delete(StatusLogFileFilePath);

                        // Requested to remove validation on SoaDataBatch Status
                        //if (DirectRetroBo.SoaDataBatchBo.Status != SoaDataBatchBo.StatusApproved)
                        //    throw new Exception(string.Format("SOA Data Batch Not yet Approved"));

                        // DELETE PREVIOUS RETRO SUMMARY
                        PrintMessage("Deleting Retro Summary...");
                        DeleteRetroSummary();
                        PrintMessage("Deleted Retro Summary");

                        if (DirectRetroBo.SoaDataBatchBo.IsProfitCommissionData == true)
                        {
                            WriteStatusLogFile(string.Format("Profit Commission SOA Data - Proceed to complete"), true);
                            UpdateRetroStatus(DirectRetroBo.RetroStatusCompleted, "Successfully Processed Direct Retro");
                            WriteStatusLogFile("Successfully Processed Direct Retro");
                            UpdateSoaDataBatchRetroStatus();
                            continue;
                        }

                        if (DirectRetroConfigurationBo == null)
                        {
                            WriteStatusLogFile(string.Format("Direct Retro Configuration not found"), true);
                            UpdateRetroStatus(DirectRetroBo.RetroStatusCompleted, "Successfully Processed Direct Retro");
                            WriteStatusLogFile("Successfully Processed Direct Retro");
                            UpdateSoaDataBatchRetroStatus();
                            continue;
                        }

                        WriteStatusLogFile(string.Format("Direct Retro Configuration found, Name: {0}", DirectRetroConfigurationBo.Name), true);

                        PrintMessage("Computing RI Data...");
                        ComputeRiData();
                        PrintMessage("Computed RI Data");

                        PrintMessage("Computing Claim Register...");
                        ComputeClaimRegister();
                        PrintMessage("Computed Claim Register");

                        PrintMessage("Computing Retro Summary...");
                        ComputeRetroSummary();
                        PrintMessage("Computed Retro Summary");

                        if (IsNoRiDataBatchLinked && IsNoClaimRegisterBatchFound)
                        {
                            Error = true;
                        }

                        if (!Error)
                        {
                            UpdateRetroStatus(DirectRetroBo.RetroStatusCompleted, "Successfully Processed Direct Retro");
                            PrintMessage("Successfully Processed Direct Retro");
                            WriteStatusLogFile("Successfully Processed Direct Retro");
                            UpdateSoaDataBatchRetroStatus();
                        }
                        else
                        {
                            UpdateRetroStatus(DirectRetroBo.RetroStatusFailed, "Failed to process Direct Retro");
                            PrintMessage("Failed to Process Direct Retro");
                            WriteStatusLogFile("Failed to Process Direct Retro");
                        }
                    }
                    catch (Exception e)
                    {
                        WriteStatusLogFile(e.Message, true);
                        UpdateRetroStatus(DirectRetroBo.RetroStatusFailed, "Failed to process Direct Retro");
                        PrintMessage("Failed to Process Direct Retro");
                        WriteStatusLogFile("Failed to Process Direct Retro");
                        PrintError(e.Message);
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

        public DirectRetroBo LoadDirectRetroBo()
        {
            DirectRetroBo = DirectRetroService.FindByRetroStatus(DirectRetroBo.RetroStatusSubmitForProcessing);
            if (DirectRetroBo != null)
            {
                DirectRetroConfigurationBo = DirectRetroConfigurationService.FindByTreatyCode(DirectRetroBo.TreatyCodeBo.Code);
            }
            return DirectRetroBo;
        }

        public void ComputeRiData()
        {
            if (!DirectRetroBo.SoaDataBatchBo.RiDataBatchId.HasValue)
            {
                WriteStatusLogFile(string.Format("No RI Data Batch Linked to SOA Data Batch Id:{0}", DirectRetroBo.SoaDataBatchBo.Id), true);
                IsNoRiDataBatchLinked = true;
                //Error = true;
                return;
            }

            int total = RiDataService.CountBySoaDataBatchIdByTreatyCode(DirectRetroBo.SoaDataBatchId, DirectRetroBo.TreatyCodeBo.Code, TransactionTypes);

            if (total == 0)
            {
                WriteStatusLogFile("No RI Data found within the Quarter", true);
                Error = true;
                return;
            }

            WriteStatusLogFile("Starting Compute RI Data...", true);
            WriteStatusLogFile(string.Format("Total RI Data to be compute: {0}", total), true);
            try
            {
                for (int skip = 0; skip < (total + Take); skip += Take)
                {
                    if (skip >= total)
                        break;

                    ProcessDirectRetroRiData = new List<ProcessDirectRetroRiData> { };

                    foreach (var bo in RiDataService.GetBySoaDataBatchIdByTreatyCode(DirectRetroBo.SoaDataBatchId, DirectRetroBo.TreatyCodeBo.Code, skip, Take, TransactionTypes))
                    {
                        ProcessDirectRetroRiData.Add(new ProcessDirectRetroRiData(bo, DirectRetroConfigurationBo.Id));
                    }
                    Parallel.ForEach(ProcessDirectRetroRiData, p => p.Process());

                    if (IsEnabledDebug)
                    {
                        TotalFailedTypeValidationRiDataIds.AddRange(ProcessDirectRetroRiData.Where(q => q.FailedType == DirectRetroBo.FailedTypeValidation).Select(q => q.RiDataBo.Id).ToList());
                        TotalFailedTypeNoRetroPartyRiDataIds.AddRange(ProcessDirectRetroRiData.Where(q => q.FailedType == DirectRetroBo.FailedTypeNoRetroParty).Select(q => q.RiDataBo.Id).ToList());
                        TotalFailedTypeExceedRetroPartyRiDataIds.AddRange(ProcessDirectRetroRiData.Where(q => q.FailedType == DirectRetroBo.FailedTypeExceedRetroParty).Select(q => q.RiDataBo.Id).ToList());
                        TotalFailedTypePremiumSpreadRiDataIds.AddRange(ProcessDirectRetroRiData.Where(q => q.FailedType == DirectRetroBo.FailedTypePremiumSpread).Select(q => q.RiDataBo.Id).ToList());
                        TotalFailedTypePremiumSpreadDetailRiDataIds.AddRange(ProcessDirectRetroRiData.Where(q => q.FailedType == DirectRetroBo.FailedTypePremiumSpreadDetail).Select(q => q.RiDataBo.Id).ToList());
                    }
                    else
                    {
                        TotalFailedTypeValidation += ProcessDirectRetroRiData.Where(q => q.FailedType == DirectRetroBo.FailedTypeValidation).Count();
                        TotalFailedTypeNoRetroParty += ProcessDirectRetroRiData.Where(q => q.FailedType == DirectRetroBo.FailedTypeNoRetroParty).Count();
                        TotalFailedTypeExceedRetroParty += ProcessDirectRetroRiData.Where(q => q.FailedType == DirectRetroBo.FailedTypeExceedRetroParty).Count();
                        TotalFailedTypePremiumSpread += ProcessDirectRetroRiData.Where(q => q.FailedType == DirectRetroBo.FailedTypePremiumSpread).Count();
                        TotalFailedTypePremiumSpreadDetail += ProcessDirectRetroRiData.Where(q => q.FailedType == DirectRetroBo.FailedTypePremiumSpreadDetail).Count();
                    }
                }

                if (IsEnabledDebug)
                {
                    TotalFailedTypeValidation = TotalFailedTypeValidationRiDataIds.Count();
                    TotalFailedTypeNoRetroParty = TotalFailedTypeNoRetroPartyRiDataIds.Count();
                    TotalFailedTypeExceedRetroParty = TotalFailedTypeExceedRetroPartyRiDataIds.Count();
                    TotalFailedTypePremiumSpread = TotalFailedTypePremiumSpreadRiDataIds.Count();
                    TotalFailedTypePremiumSpreadDetail = TotalFailedTypePremiumSpreadDetailRiDataIds.Count();

                    string totalFailedTypeValidationRiDataIds = string.Join(", ", TotalFailedTypeValidationRiDataIds);
                    string totalFailedTypeNoRetroPartyRiDataIds = string.Join(", ", TotalFailedTypeNoRetroPartyRiDataIds);
                    string totalFailedTypeExceedRetroPartyRiDataIds = string.Join(", ", TotalFailedTypeExceedRetroPartyRiDataIds);
                    string totalFailedTypePremiumSpreadRiDataIds = string.Join(", ", TotalFailedTypePremiumSpreadRiDataIds);
                    string totalFailedTypePremiumSpreadDetailRiDataIds = string.Join(", ", TotalFailedTypePremiumSpreadDetailRiDataIds);

                    WriteStatusLogFile(string.Format("{0}: {1}", DirectRetroBo.GetFailedTypeName(DirectRetroBo.FailedTypeValidation), TotalFailedTypeValidation));
                    WriteStatusLogFile(string.Format("RI Data' Id: {0}", totalFailedTypeValidationRiDataIds), true);
                    WriteStatusLogFile(string.Format("{0}: {1}", DirectRetroBo.GetFailedTypeName(DirectRetroBo.FailedTypeNoRetroParty), TotalFailedTypeNoRetroParty));
                    WriteStatusLogFile(string.Format("RI Data' Id: {0}", totalFailedTypeNoRetroPartyRiDataIds), true);
                    WriteStatusLogFile(string.Format("{0}: {1}", DirectRetroBo.GetFailedTypeName(DirectRetroBo.FailedTypeExceedRetroParty), TotalFailedTypeExceedRetroParty));
                    WriteStatusLogFile(string.Format("RI Data' Id: {0}", totalFailedTypeExceedRetroPartyRiDataIds), true);
                    WriteStatusLogFile(string.Format("{0}: {1}", DirectRetroBo.GetFailedTypeName(DirectRetroBo.FailedTypePremiumSpread), TotalFailedTypePremiumSpread));
                    WriteStatusLogFile(string.Format("RI Data' Id: {0}", totalFailedTypePremiumSpreadRiDataIds), true);
                    WriteStatusLogFile(string.Format("{0}: {1}", DirectRetroBo.GetFailedTypeName(DirectRetroBo.FailedTypePremiumSpreadDetail), TotalFailedTypePremiumSpreadDetail));
                    WriteStatusLogFile(string.Format("RI Data' Id: {0}", totalFailedTypePremiumSpreadDetailRiDataIds), true);
                }
                else
                {
                    WriteStatusLogFile(string.Format("{0}: {1}", DirectRetroBo.GetFailedTypeName(DirectRetroBo.FailedTypeValidation), TotalFailedTypeValidation));
                    WriteStatusLogFile(string.Format("{0}: {1}", DirectRetroBo.GetFailedTypeName(DirectRetroBo.FailedTypeNoRetroParty), TotalFailedTypeNoRetroParty));
                    WriteStatusLogFile(string.Format("{0}: {1}", DirectRetroBo.GetFailedTypeName(DirectRetroBo.FailedTypeExceedRetroParty), TotalFailedTypeExceedRetroParty));
                    WriteStatusLogFile(string.Format("{0}: {1}", DirectRetroBo.GetFailedTypeName(DirectRetroBo.FailedTypePremiumSpread), TotalFailedTypePremiumSpread));
                    WriteStatusLogFile(string.Format("{0}: {1}", DirectRetroBo.GetFailedTypeName(DirectRetroBo.FailedTypePremiumSpreadDetail), TotalFailedTypePremiumSpreadDetail));
                }

                if (TotalFailedTypeValidation + TotalFailedTypeNoRetroParty + TotalFailedTypeExceedRetroParty + TotalFailedTypePremiumSpread + TotalFailedTypePremiumSpreadDetail > 0)
                    Error = true;

            }
            catch (Exception e)
            {
                PrintError(e.Message);
                WriteStatusLogFile("Failed to Compute RI Data", true);
                throw new Exception(e.Message);
            }
            WriteStatusLogFile("Completed Compute RI Data", true);
        }

        public void ComputeClaimRegister()
        {
            //if (!DirectRetroBo.SoaDataBatchBo.ClaimDataBatchId.HasValue)
            //{
            //    WriteStatusLogFile("No Claim Data Batch Found to be verified", true);
            //    return;
            //}

            int total = ClaimRegisterService.CountPendingDirectRetro(DirectRetroBo.SoaDataBatchId, DirectRetroBo.TreatyCodeBo.Code);
            if (total == 0)
            {
                WriteStatusLogFile(string.Format("No Claim Register Linked to SOA Data Batch Id:{0}", DirectRetroBo.SoaDataBatchBo.Id), true);
                IsNoClaimRegisterBatchFound = true;
                //Error = true;
                return;
            }

            var directRetroProvision = new ProvisionDirectRetroClaimRegisterBatch()
            {
                Title = Title,
                SoaDataBatchId = DirectRetroBo.SoaDataBatchId,
                TreatyCode = DirectRetroBo.TreatyCodeBo.Code
            };
            directRetroProvision.Run();

            //RetrieveFinanceProvisioning();

            //foreach (var bo in ClaimRegisterService.GetPendingDirectRetro(DirectRetroBo.SoaDataBatchId, DirectRetroBo.TreatyCodeBo.Code, 0, 100000))
            //{
            //    List<string> errors = ValidateDirectRetroProvisioning(bo);

            //    if (errors.Count == 0)
            //    {
            //        DirectRetroConfigurationDetailBos = DirectRetroConfigurationDetailService.GetByClaimProvisioningParam(DirectRetroConfigurationBo.Id, bo.RiskPeriodMonth, bo.RiskPeriodYear, bo.IssueDatePol, bo.ReinsEffDatePol, false);
            //        if (DirectRetroConfigurationDetailBos.Count == 0)
            //        {
            //            WriteStatusLogFile("There are no Retro Parties found with the params", true);
            //            errors.Add(string.Format("There are no Retro Parties found with the params"));
            //            Error = true;
            //        }
            //        else if (DirectRetroConfigurationDetailBos.Count > 3)
            //        {
            //            WriteStatusLogFile("There are more than 3 Retro Parties found with the params", true);
            //            errors.Add(string.Format("There are more than 3 Retro Parties found with the params"));
            //            Error = true;
            //        }
            //        else
            //        {
            //            ReversePreviousDirectRetroProvision(bo);

            //            int index = 1;
            //            foreach (var detailBo in DirectRetroConfigurationDetailBos)
            //            {
            //                string retroPartyField = string.Format("RetroParty{0}", index);
            //                string retroRecoveryField = string.Format("RetroRecovery{0}", index);
            //                string retroShareField = string.Format("RetroShare{0}", index);

            //                double share = detailBo.Share;
            //                double computeShare = share / 100;

            //                bo.SetPropertyValue(retroPartyField, detailBo.RetroPartyBo.Party);
            //                bo.SetPropertyValue(retroShareField, share);

            //                double claimAmount = computeShare * bo.ClaimRecoveryAmt.Value;
            //                bo.SetPropertyValue(retroRecoveryField, claimAmount);

            //                CreateDirectRetroProvisionTransaction(bo, detailBo.RetroPartyBo.Party, claimAmount, true);

            //                index++;
            //            }

            //        }
            //    }

            //    if (errors.Count > 0)
            //    {
            //        bo.ProvisionErrors = JsonConvert.SerializeObject(errors);
            //    }
            //    var claimRegisterBo = bo;
            //    ClaimRegisterService.Update(ref claimRegisterBo);
            //}

            //UpdateFinanceProvisioning();

            //int unProcess = ClaimRegisterService.CountBySoaDataBatchIdByTreatyCodeExceptByProvisionStatus(DirectRetroBo.SoaDataBatchId, DirectRetroBo.TreatyCodeBo.Code, ClaimRegisterBo.ProvisionStatusProvisioned, ClaimTransactionTypes);

            //if (unProcess > 0)
            //{
            //    WriteStatusLogFile(string.Format("There are {0} number of Claim Register(s) that has not been Provisioned", unProcess), true);
            //    Error = true;
            //    return;
            //}
        }

        public List<string> ValidateDirectRetroProvisioning(ClaimRegisterBo bo)
        {
            List<string> errors = new List<string>();

            List<int> required = new List<int>
            {
                //StandardClaimDataOutputBo.TypeTreatyCode,
                //StandardClaimDataOutputBo.TypeRiskPeriodMonth,
                //StandardClaimDataOutputBo.TypeRiskPeriodYear,
                //StandardClaimDataOutputBo.TypeReinsEffDatePol,
                //StandardClaimDataOutputBo.TypeIssueDatePol,
                StandardClaimDataOutputBo.TypeClaimRecoveryAmt,
            };

            foreach (int type in required)
            {
                string property = StandardClaimDataOutputBo.GetPropertyNameByType(type);
                object value = bo.GetPropertyValue(property);
                if (value == null)
                {
                    errors.Add(string.Format("{0} is empty", property));
                }
                else if (value is string @string && string.IsNullOrEmpty(@string))
                {
                    errors.Add(string.Format("{0} is empty", property));
                }
            }

            return errors;
        }

        public void ReversePreviousDirectRetroProvision(ClaimRegisterBo bo)
        {
            IList<DirectRetroProvisioningTransactionBo> transactionBos = DirectRetroProvisioningTransactionService.GetByClaimRegisterId(bo.Id);
            foreach (DirectRetroProvisioningTransactionBo directRetroProvisioningTransactionBo in transactionBos)
            {
                DirectRetroProvisioningTransactionBo transactionBo = directRetroProvisioningTransactionBo;
                transactionBo.IsLatestProvision = false;
                DirectRetroProvisioningTransactionService.Update(ref transactionBo);

                double reverseProvisionAmount = transactionBo.RetroRecovery * -1;
                CreateDirectRetroProvisionTransaction(bo, transactionBo.RetroParty, reverseProvisionAmount);
            }
        }

        public void CreateDirectRetroProvisionTransaction(ClaimRegisterBo bo, string retroParty, double amount, bool isLatestProvision = false)
        {
            DirectRetroProvisioningTransactionBo transactionBo = new DirectRetroProvisioningTransactionBo()
            {
                ClaimRegisterId = bo.Id,
                FinanceProvisioningId = FinanceProvisioningBo?.Id,
                IsLatestProvision = isLatestProvision,
                ClaimId = bo.ClaimId,
                CedingCompany = bo.CedingCompany,
                EntryNo = bo.EntryNo,
                Quarter = bo.SoaQuarter,
                RetroParty = retroParty,
                RetroRecovery = amount,
                TreatyCode = bo.TreatyCode,
                TreatyType = bo.TreatyType,
                ClaimCode = bo.ClaimCode,
                CreatedById = User.DefaultSuperUserId,
                UpdatedById = User.DefaultSuperUserId,
            };

            if (FinanceProvisioningBo != null)
                FinanceProvisioningBo.DrProvisionAmount += amount;

            DirectRetroProvisioningTransactionService.Create(ref transactionBo);
        }

        public void ComputeRetroSummary()
        {
            WriteStatusLogFile("Starting Compute Retro Summary...", true);

            //QuarterObject quarterObject = new QuarterObject(DirectRetroBo.SoaQuarter);
            List<RetroSummaryBo> retroSummaryBos = new List<RetroSummaryBo>();
            List<RetroSummaryBo> retroSummaryIfrs17Bos = new List<RetroSummaryBo>();
            List<string> claimTransactionTypes = new List<string> { PickListDetailBo.ClaimTransactionTypeNew, PickListDetailBo.ClaimTransactionTypeAdjustment };

            try
            {
                foreach (string transactionType in TransactionTypes)
                {
                    using (var db = new AppDbContext(false))
                    {
                        List<RetroSummaryBo> riDataRetroSummarybos = db.RiData
                               .Where(q => q.RiDataBatch.SoaDataBatchId == DirectRetroBo.SoaDataBatchId)
                               .Where(q => q.TreatyCode == DirectRetroBo.TreatyCodeBo.Code)
                               .Where(q => q.TransactionTypeCode == transactionType)
                               .GroupBy(g => new { g.TreatyCode, g.RiskPeriodYear, g.RiskPeriodMonth, g.RetroParty1, g.RetroParty2, g.RetroParty3, g.RetroShare1, g.RetroShare2, g.RetroShare3, g.RetroPremiumSpread1, g.RetroPremiumSpread2, g.RetroPremiumSpread3 })
                               .Select(r => new RetroSummaryBo
                               {
                                   Month = r.Key.RiskPeriodMonth,
                                   Year = r.Key.RiskPeriodYear,
                                   Type = transactionType,
                                   TreatyCode = r.Key.TreatyCode,
                                   NoOfPolicy = r.Count(),
                                   TotalSar = r.Sum(x => x.Aar),
                                   TotalRiPremium = r.Sum(x => x.TransactionPremium),
                                   TotalDiscount = r.Sum(x => x.TransactionDiscount),
                                   RetroParty1 = r.Key.RetroParty1,
                                   RetroParty2 = r.Key.RetroParty2,
                                   RetroParty3 = r.Key.RetroParty3,
                                   RetroShare1 = r.Key.RetroShare1,
                                   RetroShare2 = r.Key.RetroShare2,
                                   RetroShare3 = r.Key.RetroShare3,
                                   RetroPremiumSpread1 = r.Key.RetroPremiumSpread1,
                                   RetroPremiumSpread2 = r.Key.RetroPremiumSpread2,
                                   RetroPremiumSpread3 = r.Key.RetroPremiumSpread3,
                                   RetroRiPremium1 = r.Sum(x => x.RetroReinsurancePremium1),
                                   RetroRiPremium2 = r.Sum(x => x.RetroReinsurancePremium2),
                                   RetroRiPremium3 = r.Sum(x => x.RetroReinsurancePremium3),
                                   RetroDiscount1 = r.Sum(x => x.RetroDiscount1),
                                   RetroDiscount2 = r.Sum(x => x.RetroDiscount2),
                                   RetroDiscount3 = r.Sum(x => x.RetroDiscount3),
                                   TotalDirectRetroAar = r.Sum(x => x.TotalDirectRetroAar),
                               })
                               .ToList();

                        if (riDataRetroSummarybos != null && riDataRetroSummarybos.Count > 0)
                            retroSummaryBos.AddRange(riDataRetroSummarybos);

                        List<RetroSummaryBo> riDataRetroSummaryIfrs17bos = db.RiData
                               .Where(q => q.RiDataBatch.SoaDataBatchId == DirectRetroBo.SoaDataBatchId)
                               .Where(q => q.TreatyCode == DirectRetroBo.TreatyCodeBo.Code)
                               .Where(q => q.TransactionTypeCode == transactionType)
                               .GroupBy(g => new { g.TreatyCode, g.RiskPeriodYear, g.RiskPeriodMonth, g.RetroParty1, g.RetroParty2, g.RetroParty3, g.RetroShare1, g.RetroShare2, g.RetroShare3, g.RetroPremiumSpread1, g.RetroPremiumSpread2, g.RetroPremiumSpread3, g.Mfrs17AnnualCohort, g.Mfrs17TreatyCode })
                               .Select(r => new RetroSummaryBo
                               {
                                   Month = r.Key.RiskPeriodMonth,
                                   Year = r.Key.RiskPeriodYear,
                                   Type = transactionType,
                                   TreatyCode = r.Key.TreatyCode,
                                   NoOfPolicy = r.Count(),
                                   TotalSar = r.Sum(x => x.Aar),
                                   TotalRiPremium = r.Sum(x => x.TransactionPremium),
                                   TotalDiscount = r.Sum(x => x.TransactionDiscount),
                                   RetroParty1 = r.Key.RetroParty1,
                                   RetroParty2 = r.Key.RetroParty2,
                                   RetroParty3 = r.Key.RetroParty3,
                                   RetroShare1 = r.Key.RetroShare1,
                                   RetroShare2 = r.Key.RetroShare2,
                                   RetroShare3 = r.Key.RetroShare3,
                                   RetroPremiumSpread1 = r.Key.RetroPremiumSpread1,
                                   RetroPremiumSpread2 = r.Key.RetroPremiumSpread2,
                                   RetroPremiumSpread3 = r.Key.RetroPremiumSpread3,
                                   RetroRiPremium1 = r.Sum(x => x.RetroReinsurancePremium1),
                                   RetroRiPremium2 = r.Sum(x => x.RetroReinsurancePremium2),
                                   RetroRiPremium3 = r.Sum(x => x.RetroReinsurancePremium3),
                                   RetroDiscount1 = r.Sum(x => x.RetroDiscount1),
                                   RetroDiscount2 = r.Sum(x => x.RetroDiscount2),
                                   RetroDiscount3 = r.Sum(x => x.RetroDiscount3),
                                   TotalDirectRetroAar = r.Sum(x => x.TotalDirectRetroAar),
                                   Mfrs17AnnualCohort = r.Key.Mfrs17AnnualCohort,
                                   Mfrs17ContractCode = r.Key.Mfrs17TreatyCode,
                               })
                               .ToList();

                        if (riDataRetroSummaryIfrs17bos != null && riDataRetroSummaryIfrs17bos.Count > 0)
                            retroSummaryIfrs17Bos.AddRange(riDataRetroSummaryIfrs17bos);
                    }
                }

                using (var db = new AppDbContext(false))
                {
                    List<RetroSummaryBo> claimsRetroSummarybos = db.ClaimRegister
                           .Where(q => q.SoaDataBatchId == DirectRetroBo.SoaDataBatchId)
                           .Where(q => q.TreatyCode == DirectRetroBo.TreatyCodeBo.Code)
                           .Where(q => claimTransactionTypes.Contains(q.ClaimTransactionType))
                           .Where(q => q.TreatyType != PickListDetailBo.TreatyTypeTakaful)
                           .GroupBy(g => new { g.TreatyCode, g.RetroParty1, g.RetroParty2, g.RetroParty3, g.RetroShare1, g.RetroShare2, g.RetroShare3 })
                           .Select(r => new RetroSummaryBo
                           {
                               TreatyCode = r.Key.TreatyCode,
                               NoOfClaims = r.Count(),
                               TotalClaims = r.Sum(x => x.ClaimRecoveryAmt),
                               RetroParty1 = r.Key.RetroParty1,
                               RetroParty2 = r.Key.RetroParty2,
                               RetroParty3 = r.Key.RetroParty3,
                               RetroShare1 = r.Key.RetroShare1,
                               RetroShare2 = r.Key.RetroShare2,
                               RetroShare3 = r.Key.RetroShare3,
                               RetroClaims1 = r.Sum(x => x.RetroRecovery1),
                               RetroClaims2 = r.Sum(x => x.RetroRecovery2),
                               RetroClaims3 = r.Sum(x => x.RetroRecovery3),
                           })
                           .ToList();

                    if (claimsRetroSummarybos != null && claimsRetroSummarybos.Count > 0)
                        retroSummaryBos.AddRange(claimsRetroSummarybos);

                    List<RetroSummaryBo> claimsRetroSummaryIfrs17bos = db.ClaimRegister
                           .Where(q => q.SoaDataBatchId == DirectRetroBo.SoaDataBatchId)
                           .Where(q => q.TreatyCode == DirectRetroBo.TreatyCodeBo.Code)
                           .Where(q => claimTransactionTypes.Contains(q.ClaimTransactionType))
                           .Where(q => q.TreatyType != PickListDetailBo.TreatyTypeTakaful)
                           .GroupBy(g => new { g.TreatyCode, g.RetroParty1, g.RetroParty2, g.RetroParty3, g.RetroShare1, g.RetroShare2, g.RetroShare3, g.Mfrs17AnnualCohort, g.Mfrs17ContractCode })
                           .Select(r => new RetroSummaryBo
                           {
                               TreatyCode = r.Key.TreatyCode,
                               NoOfClaims = r.Count(),
                               TotalClaims = r.Sum(x => x.ClaimRecoveryAmt),
                               RetroParty1 = r.Key.RetroParty1,
                               RetroParty2 = r.Key.RetroParty2,
                               RetroParty3 = r.Key.RetroParty3,
                               RetroShare1 = r.Key.RetroShare1,
                               RetroShare2 = r.Key.RetroShare2,
                               RetroShare3 = r.Key.RetroShare3,
                               RetroClaims1 = r.Sum(x => x.RetroRecovery1),
                               RetroClaims2 = r.Sum(x => x.RetroRecovery2),
                               RetroClaims3 = r.Sum(x => x.RetroRecovery3),
                               Mfrs17AnnualCohort = r.Key.Mfrs17AnnualCohort,
                               Mfrs17ContractCode = r.Key.Mfrs17ContractCode,
                           })
                           .ToList();

                    if (claimsRetroSummaryIfrs17bos != null && claimsRetroSummaryIfrs17bos.Count > 0)
                        retroSummaryIfrs17Bos.AddRange(claimsRetroSummaryIfrs17bos);
                }

                foreach (RetroSummaryBo bo in retroSummaryBos)
                {
                    if (bo.Month.HasValue && bo.Year.HasValue)
                    {
                        QuarterObject qo = new QuarterObject(bo.Month.Value, bo.Year.Value);
                        bo.RiskQuarter = qo?.Quarter;
                    }

                    bo.TotalSar = Util.RoundNullableValue(bo.TotalSar, 2);
                    bo.TotalDiscount = Util.RoundNullableValue(bo.TotalDiscount, 2);
                    bo.TotalClaims = Util.RoundNullableValue(bo.TotalClaims, 2);
                    bo.RetroRiPremium1 = Util.RoundNullableValue(bo.RetroRiPremium1, 2);
                    bo.RetroRiPremium2 = Util.RoundNullableValue(bo.RetroRiPremium2, 2);
                    bo.RetroRiPremium3 = Util.RoundNullableValue(bo.RetroRiPremium3, 2);
                    bo.RetroDiscount1 = Util.RoundNullableValue(bo.RetroDiscount1, 2);
                    bo.RetroDiscount2 = Util.RoundNullableValue(bo.RetroDiscount2, 2);
                    bo.RetroDiscount3 = Util.RoundNullableValue(bo.RetroDiscount3, 2);
                    bo.RetroClaims1 = Util.RoundNullableValue(bo.RetroClaims1, 2);
                    bo.RetroClaims2 = Util.RoundNullableValue(bo.RetroClaims2, 2);
                    bo.RetroClaims3 = Util.RoundNullableValue(bo.RetroClaims3, 2);
                    bo.TotalDirectRetroAar = Util.RoundNullableValue(bo.TotalDirectRetroAar, 2);

                    bo.DirectRetroId = DirectRetroBo.Id;
                    bo.ReportingType = RetroSummaryBo.ReportingTypeIFRS4;
                    bo.CreatedById = User.DefaultSuperUserId;
                    RetroSummaryService.Create(bo);
                }

                foreach (RetroSummaryBo bo in retroSummaryIfrs17Bos)
                {
                    if (bo.Month.HasValue && bo.Year.HasValue)
                    {
                        QuarterObject qo = new QuarterObject(bo.Month.Value, bo.Year.Value);
                        bo.RiskQuarter = qo?.Quarter;
                    }

                    bo.TotalSar = Util.RoundNullableValue(bo.TotalSar, 2);
                    bo.TotalDiscount = Util.RoundNullableValue(bo.TotalDiscount, 2);
                    bo.TotalClaims = Util.RoundNullableValue(bo.TotalClaims, 2);
                    bo.RetroRiPremium1 = Util.RoundNullableValue(bo.RetroRiPremium1, 2);
                    bo.RetroRiPremium2 = Util.RoundNullableValue(bo.RetroRiPremium2, 2);
                    bo.RetroRiPremium3 = Util.RoundNullableValue(bo.RetroRiPremium3, 2);
                    bo.RetroDiscount1 = Util.RoundNullableValue(bo.RetroDiscount1, 2);
                    bo.RetroDiscount2 = Util.RoundNullableValue(bo.RetroDiscount2, 2);
                    bo.RetroDiscount3 = Util.RoundNullableValue(bo.RetroDiscount3, 2);
                    bo.RetroClaims1 = Util.RoundNullableValue(bo.RetroClaims1, 2);
                    bo.RetroClaims2 = Util.RoundNullableValue(bo.RetroClaims2, 2);
                    bo.RetroClaims3 = Util.RoundNullableValue(bo.RetroClaims3, 2);
                    bo.TotalDirectRetroAar = Util.RoundNullableValue(bo.TotalDirectRetroAar, 2);

                    bo.DirectRetroId = DirectRetroBo.Id;
                    bo.ReportingType = RetroSummaryBo.ReportingTypeIFRS17;
                    bo.CreatedById = User.DefaultSuperUserId;
                    RetroSummaryService.Create(bo);
                }
            }
            catch (Exception e)
            {
                PrintError(e.Message);
                WriteStatusLogFile("Failed to Compute Retro Summary", true);
                throw new Exception(e.Message);
            }
            WriteStatusLogFile("Completed Compute Retro Summary", true);
        }

        public void LogValueSet(string property, dynamic value)
        {
            WriteStatusLogFile(string.Format("{0}: {1}", property, value));
        }

        // Only if reprocessing
        public void DeleteRetroSummary()
        {
            WriteStatusLogFile("Deleting Retro Summary...");
            RetroSummaryService.DeleteByDirectRetroId(DirectRetroBo.Id); // Do not trail
            WriteStatusLogFile("Deleted Retro Summary", true);
        }

        public void UpdateRetroStatus(int retroStatus, string des)
        {
            TrailObject trail = new TrailObject();
            StatusHistoryBo statusBo = new StatusHistoryBo
            {
                ModuleId = ModuleBo.Id,
                ObjectId = DirectRetroBo.Id,
                Status = retroStatus,
                CreatedById = User.DefaultSuperUserId,
                UpdatedById = User.DefaultSuperUserId,
            };
            StatusHistoryService.Create(ref statusBo, ref trail);

            var directRetro = DirectRetroBo;
            DirectRetroBo.RetroStatus = retroStatus;
            Result result = DirectRetroService.Update(ref directRetro, ref trail);
            UserTrailBo userTrailBo = new UserTrailBo(
                DirectRetroBo.Id,
                des,
                result,
                trail,
                User.DefaultSuperUserId
            );
            UserTrailService.Create(ref userTrailBo);

            if (retroStatus == DirectRetroBo.RetroStatusProcessing)
                ProcessingStatusHistoryBo = statusBo;
        }

        public void CreateStatusLogFile()
        {
            if (DirectRetroBo == null)
                return;
            if (ProcessingStatusHistoryBo == null)
                return;

            TrailObject trail = new TrailObject();
            DirectRetroStatusFileBo = new DirectRetroStatusFileBo
            {
                DirectRetroId = DirectRetroBo.Id,
                StatusHistoryId = ProcessingStatusHistoryBo.Id,
                StatusHistoryBo = ProcessingStatusHistoryBo,
                CreatedById = User.DefaultSuperUserId,
                UpdatedById = User.DefaultSuperUserId,
            };
            var fileBo = DirectRetroStatusFileBo;
            var result = DirectRetroStatusFileService.Create(ref fileBo, ref trail);

            UserTrailBo userTrailBo = new UserTrailBo(
                fileBo.Id,
                "Create Direct Retro Status File",
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

        public void UpdateSoaDataBatchRetroStatus()
        {
            int totalNotCompleted = DirectRetroService.CountBySoaDataBatchIdByExceptRetroStatus(DirectRetroBo.SoaDataBatchId, DirectRetroBo.RetroStatusCompleted);

            if (totalNotCompleted > 0)
            {
                return;
            }

            int totalDirectRetro = DirectRetroService.CountBySoaDataBatchId(DirectRetroBo.SoaDataBatchId);
            int totalDirectRetroConfig = DirectRetroConfigurationService.CountByTreatyId(DirectRetroBo.TreatyCodeBo.TreatyId);

            if (totalDirectRetro != totalDirectRetroConfig)
            {
                return;
            }

            TrailObject trail = new TrailObject();
            var soaDataBatchBo = DirectRetroBo.SoaDataBatchBo;
            DirectRetroBo.SoaDataBatchBo.DirectStatus = SoaDataBatchBo.DirectStatusCompleted;
            Result result = SoaDataBatchService.Update(ref soaDataBatchBo, ref trail);
            UserTrailBo userTrailBo = new UserTrailBo(
                DirectRetroBo.SoaDataBatchBo.Id,
                "Update SOA Data Batch Status",
                result,
                trail,
                User.DefaultSuperUserId
            );
            UserTrailService.Create(ref userTrailBo);
        }

        public void RetrieveFinanceProvisioning()
        {
            FinanceProvisioningBo = FinanceProvisioningService.FindByStatus(FinanceProvisioningBo.StatusPending);

            if (FinanceProvisioningBo != null)
                return;

            FinanceProvisioningBo = new FinanceProvisioningBo
            {
                Date = DateTime.Today,
                Status = FinanceProvisioningBo.StatusPending,
                CreatedById = User.DefaultSuperUserId,
                UpdatedById = User.DefaultSuperUserId,
            };
            var trail = new TrailObject();
            var financeProvisioningBo = FinanceProvisioningBo;
            var result = FinanceProvisioningService.Create(ref financeProvisioningBo, ref trail);

            var userTrailBo = new UserTrailBo(
                FinanceProvisioningBo.Id,
                "Create Finance Provisioning",
                result,
                trail,
                User.DefaultSuperUserId
            );
            UserTrailService.Create(ref userTrailBo);
        }

        public void UpdateFinanceProvisioning()
        {
            var trail = new TrailObject();
            var financeProvisioningBo = FinanceProvisioningBo;

            var result = FinanceProvisioningService.Update(ref financeProvisioningBo, ref trail);
            var userTrailBo = new UserTrailBo(
                FinanceProvisioningBo.Id,
                "Update Finance Provisioning",
                result,
                trail,
                User.DefaultSuperUserId
            );
            UserTrailService.Create(ref userTrailBo);
        }
    }
}
