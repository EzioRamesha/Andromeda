using BusinessObject;
using BusinessObject.Identity;
using DataAccess.Entities.Identity;
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
    public class GenerateE2 : Command
    {
        public List<Column> Cols { get; set; }

        public Excel E2Ifrs17 { get; set; }

        public Excel E2Ifrs4 { get; set; }

        public RetroRegisterBatchBo RetroRegisterBatchBo { get; set; }

        public RetroRegisterBo RetroRegisterBo { get; set; }

        public IList<RetroRegisterBo> RetroRegisterBos { get; set; }

        public ModuleBo ModuleBo { get; set; }

        public string FileName1 { get; set; }

        public string FileName2 { get; set; }

        public string FilePath { get; set; }

        public int Total { get; set; } = 0;

        public int Take { get; set; } = 100;

        public int Skip { get; set; } = 0;

        public int Index { get; set; }

        public double? TotalAmount { get; set; } = 0;

        public List<string> ClaimId { get; set; }

        public IList<PickListDetailBo> RetroRegisterPickListBo { get; set; }

        public List<int> RetroRegisterFieldIds { get; set; }

        public StatusHistoryBo ProcessingStatusHistoryBo { get; set; }

        public RetroRegisterBatchStatusFileBo RetroRegisterBatchStatusFileBo { get; set; }

        public string StatusLogFileFilePath { get; set; }

        // To manually generate IFRS4/IFRS17 output file in main screen > edit Retro Register
        public bool GenerateIFRS4 { get; set; } = false;
        public bool GenerateIFRS17 { get; set; } = false;
        public string FileName { get; set; }


        public const int TypeLayout = 1;
        public const int TypeJournalType = 2;
        public const int TypeAccountCode = 3;
        public const int TypeAccountDescription = 4;
        public const int TypePeriod = 5;
        public const int TypeTransactionDate = 6;
        public const int TypeAmount = 7;
        public const int TypeDebitCredit = 8;
        public const int TypeTransactionReference = 9;
        public const int TypeDescription = 10;
        public const int TypeL1 = 11;
        public const int TypeL2 = 12;
        public const int TypeL3 = 13;
        public const int TypeL4 = 14;
        public const int TypeL5 = 15;
        public const int TypeL6 = 16;
        public const int TypeL7 = 17;
        public const int TypeL8 = 18;
        public const int TypeL9 = 19;
        public const int TypeL10 = 20;
        public const int TypeAddDescription1 = 21;
        public const int TypeAddDescription2 = 22;
        public const int TypeAddDescription3 = 23;
        public const int TypeAddDescription4 = 24;
        public const int TypeAddDescription5 = 25;

        public GenerateE2()
        {
            Title = "GenerateE2";
            Description = "To generate E2 excel files";
        }

        public override void Initial()
        {
            ModuleBo = ModuleService.FindByController(ModuleBo.ModuleController.RetroRegister.ToString());
            RetroRegisterPickListBo = PickListDetailService.GetByPickListId(PickListBo.RetroRegisterField);
            RetroRegisterFieldIds = RetroRegisterPickListBo.Select(q => q.Id).ToList();
        }

        public override void Run()
        {
            try
            {
                if (RetroRegisterBatchService.CountByStatus(RetroRegisterBatchBo.StatusSubmitForGenerate) == 0)
                {
                    PrintMessage(MessageBag.NoMfrs17reportingPendingGenerate);
                    return;
                }

                
                PrintStarting();
                while (LoadRetroRegisterBatchBo() != null)
                {
                    try
                    {
                        if (GetProcessCount("Process") > 0)
                            PrintProcessCount();
                        SetProcessCount("Process");

                        UpdateStatus(RetroRegisterBatchBo.StatusGenerating, "Generating SUNGL Files");

                        CreateStatusLogFile();

                        if (File.Exists(StatusLogFileFilePath))
                            File.Delete(StatusLogFileFilePath);

                        // DELETE PREVIOUS SUNGL FILES
                        PrintMessage("Deleting Sungl Files...", true, false);
                        DeleteBatchSungFile();
                        PrintMessage("Deleted Sungl Files", true, false);

                        ProcessData();

                        UpdateStatus(RetroRegisterBatchBo.StatusGenerateComplete, "Successfully Generating SUNGL Files");
                        WriteStatusLogFile("Successfully Generating SUNGL Files");
                    }
                    catch (Exception e)
                    {
                        var message = e.Message;
                        if (e is DbEntityValidationException dbEx)
                            message = Util.CatchDbEntityValidationException(dbEx).ToString();

                        WriteStatusLogFile(message, true);
                        UpdateStatus(RetroRegisterBatchBo.StatusGenerateFailed, "Failed to Generate SUNGL Files");
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

        public void ProcessData()
        {
            WriteStatusLogFile("Starting Generate SUNGL...", true);

            GetColumns();

            PrepareE2Excel();
            OpenE2Template();

            Total = RetroRegisterService.CountByRetroRegisterBatchId(RetroRegisterBatchBo.Id, RetroRegisterBo.ReportingTypeIFRS4);
            WriteStatusLogFile(string.Format("Total Retro Register Summary to be compute for IFRS4: {0}", Total), true);
            for (Skip = 0; Skip < Total + Take; Skip += Take)
            {
                if (Skip >= Total)
                    break;

                RetroRegisterBo = null;
                foreach (var bo in RetroRegisterBos.Where(q => q.ReportingType == RetroRegisterBo.ReportingTypeIFRS4).Skip(Skip).Take(Take))
                {
                    WriteStatusLogFile(string.Format("Computing Retro Register Summary: {0}", bo.Id));
                    RetroRegisterBo = bo;

                    //ClaimId = ClaimRegisterService.GetDistinctClaimIds(bo.DirectRetroBo.SoaDataBatchId);

                    ProcessE2Ifrs4(RetroRegisterBo);

                    WriteStatusLogFile(string.Format("Completed Compute Retro Register Summary: {0}", bo.Id), true);
                }
            }

            Total = RetroRegisterService.CountByRetroRegisterBatchId(RetroRegisterBatchBo.Id, RetroRegisterBo.ReportingTypeIFRS17);
            WriteStatusLogFile(string.Format("Total Retro Register Summary to be compute for IFRS17: {0}", Total), true);

            foreach (var statementNo in RetroRegisterBos.Where(q => q.ReportingType == RetroRegisterBo.ReportingTypeIFRS17).GroupBy(q => q.RetroStatementNo).Select(q => q.Key).ToArray())
            {
                Index = 1;
                TotalAmount = 0;

                RetroRegisterBo = null;
                foreach (var bo in RetroRegisterBos.Where(q => q.ReportingType == RetroRegisterBo.ReportingTypeIFRS17 && q.RetroStatementNo == statementNo).OrderByDescending(q => q.AnnualCohort.HasValue).ThenBy(q => q.AnnualCohort))
                {
                    WriteStatusLogFile(string.Format("Computing Retro Register Summary: {0}", bo.Id));
                    
                    RetroRegisterBo = bo;
                    //ClaimId = ClaimRegisterService.GetDistinctClaimIds(bo.DirectRetroBo.SoaDataBatchId);

                    //RetroRegisterFieldIds = RetroRegisterPickListBo.Where(q => q.Code != PickListDetailBo.RetroRegisterFieldClaim).Select(q => q.Id).ToList();
                    ProcessE2Ifrs17(RetroRegisterBo, RetroRegisterFieldIds);

                    WriteStatusLogFile(string.Format("Completed Compute Retro Register Summary: {0}", bo.Id), true);
                }

                // for Balance Sheet
                foreach (var treatyNo in RetroRegisterBos.Where(q => q.ReportingType == RetroRegisterBo.ReportingTypeIFRS17 && q.RetroStatementNo == statementNo && !string.IsNullOrEmpty(q.TreatyNumber)).GroupBy(q => q.TreatyNumber.Trim()).Select(q => q.Key).ToArray())
                {
                    var AccountMappingBalanceSheetBos = AccountCodeMappingService.FindByE2Ifrs17Param(treatyNumber: treatyNo, isBalanceSheet: true);
                    foreach (var accountMappingBalanceSheetBo in AccountMappingBalanceSheetBos.OrderByDescending(q => q.RetroRegisterFieldPickListDetailId))
                    {
                        if (accountMappingBalanceSheetBo.RetroRegisterFieldPickListDetailId.HasValue)
                        {
                            foreach (var bo in RetroRegisterBos.Where(q => q.ReportingType == RetroRegisterBo.ReportingTypeIFRS17 && q.RetroStatementNo == statementNo && q.TreatyNumber == treatyNo && q.AnnualCohort.HasValue).OrderBy(q => q.AnnualCohort))
                            {
                                RetroRegisterBo = bo;

                                ProcessE2frs17Data(Index, accountMappingBalanceSheetBo);
                                Index++;
                            }
                        }
                        else
                        {
                            foreach (var bo in RetroRegisterBos.Where(q => q.ReportingType == RetroRegisterBo.ReportingTypeIFRS17 && q.RetroStatementNo == statementNo && q.TreatyNumber == treatyNo).OrderByDescending(q => q.AnnualCohort.HasValue).ThenBy(q => q.AnnualCohort))
                            {
                                RetroRegisterBo = bo;

                                ProcessE2frs17Data(Index, accountMappingBalanceSheetBo);
                                Index++;
                            }
                        }
                    }
                }                            

                // for Claim
                //foreach (var bo in RetroRegisterBos.Where(q => q.ReportingType == RetroRegisterBo.ReportingTypeIFRS17 && q.RetroStatementNo == statementNo))
                //{
                //    RetroRegisterBo = bo;

                //    RetroRegisterFieldIds = RetroRegisterPickListBo.Where(q => q.Code == PickListDetailBo.RetroRegisterFieldClaim).Select(q => q.Id).ToList();
                //    ProcessE2Ifrs17(RetroRegisterBo, RetroRegisterFieldIds);
                //}

            }

            SaveE2();
            SaveFile();

            WriteStatusLogFile("Completed Generate SUNGL...", true);
        }

        public void PrepareE2Excel()
        {
            var templateFilepath = Util.GetWebAppDocumentFilePath("E2_MFRS17_Template.xlsx");
            FileName1 = string.Format("E2_IFRS17").AppendDateTimeFileName(".xlsx");
            var filepath = Path.Combine(Util.GetE2Path(), FileName1);
            E2Ifrs17 = new Excel(templateFilepath, filepath, 15);

            var templateFilepath2 = Util.GetWebAppDocumentFilePath("E2_Template.xlsx");
            FileName2 = string.Format("E2_IFRS4").AppendDateTimeFileName(".xlsx");
            filepath = Path.Combine(Util.GetE2Path(), FileName2);
            E2Ifrs4 = new Excel(templateFilepath2, filepath, 15);
        }

        public void OpenE2Template()
        {
            E2Ifrs17.OpenTemplate();
            E2Ifrs4.OpenTemplate();
        }

        public void SaveE2()
        {
            E2Ifrs17.Save();
            E2Ifrs4.Save();
        }

        public void ProcessE2Ifrs4(RetroRegisterBo bo)
        {
            Index = 1;
            TotalAmount = 0;

            //var ClaimCodes = ClaimRegisterService.GetDistinctClaimCodes(RetroRegisterBo.TreatyCodeBo.Code, RetroRegisterBo.DirectRetroBo.SoaDataBatchId);

            var AccountMappingBos = AccountCodeMappingService.FindByE2Ifrs4Param(bo.TreatyType, RetroRegisterFieldIds);
            var RetroPartyBo = RetroPartyService.Find(bo.RetroPartyId);
            var ClaimAccountMappingBo = AccountCodeMappingService.FindClaimEntryAccountCode();

            if (AccountMappingBos != null && AccountMappingBos.Count() != 0)
            {
                foreach (var AccountMappingBo in AccountMappingBos.OrderBy(q => q.RetroRegisterFieldPickListDetailBo.SortIndex))
                {
                    ProcessE2frs4Data(Index, null, AccountMappingBo);
                    Index++;
                }
                ProcessE2frs4Data((Index == 0 ? Index : Index + 1), null, ClaimAccountMappingBo); // To include claim entry account code (414161)
                ProcessE2frs4Data(Index + 1, RetroPartyBo); // To include Retro Party                
            }
            else
            {
                ProcessE2frs4Data(Index, null, ClaimAccountMappingBo);
                ProcessE2frs4Data(Index + 1, RetroPartyBo);                
            }
        }

        public void ProcessE2Ifrs17(RetroRegisterBo bo, List<int> retroRegisterFieldIds = null, bool isBalanceSheet = false)
        {
            var AccountMappingBos = AccountCodeMappingService.FindByE2Ifrs17Param(GetContractCode(bo.ContractCode), "", retroRegisterFieldIds, isBalanceSheet);

            if (AccountMappingBos != null && AccountMappingBos.Count() != 0)
            {
                if (retroRegisterFieldIds != null)
                    AccountMappingBos = AccountMappingBos.OrderBy(q => q.RetroRegisterFieldPickListDetailBo.SortIndex).ToList();

                foreach (var AccountMappingBo in AccountMappingBos)
                {
                    ProcessE2frs17Data(Index, AccountMappingBo);
                    Index++;
                }
            }
        }

        public void ProcessE2frs17Data(int layoutStart, AccountCodeMappingBo AccountMappingBo = null)
        {
            if (RetroRegisterBo == null)
                return;

            string layout = layoutStart == 1 ? "1;3;6" : "6";
            string journal = "GJ";

            foreach (var col in Cols)
            {
                if (!col.ColIndex.HasValue)
                    continue;

                object v1 = null;
                switch (col.ColIndex)
                {
                    case TypeLayout:
                        v1 = layout;
                        break;
                    case TypeJournalType:
                        v1 = journal;
                        break;
                    case TypeAccountCode:
                        if (AccountMappingBo != null && AccountMappingBo.AccountCodeBo != null)
                            v1 = AccountMappingBo.AccountCodeBo.Code;
                        break;
                    case TypeAccountDescription:
                        if (AccountMappingBo != null && AccountMappingBo.AccountCodeBo != null)
                        {
                            if (!AccountMappingBo.IsBalanceSheet)
                                v1 = AccountMappingBo.AccountCodeBo.Description;
                            else
                                v1 = string.Format("{0}-[{1}]", AccountMappingBo.AccountCodeBo.Description, RetroRegisterBo.TreatyNumber);
                        }
                        break;
                    case TypePeriod:
                        if (RetroRegisterBo.RetroStatementDate != DateTime.MinValue && RetroRegisterBo.RetroStatementDate.HasValue)
                            v1 = SetAccountingPeriod(RetroRegisterBo.RetroStatementDate.Value);
                        break;
                    case TypeTransactionDate:
                        if (RetroRegisterBo.RetroStatementDate != DateTime.MinValue && RetroRegisterBo.RetroStatementDate.HasValue)
                            v1 = RetroRegisterBo.RetroStatementDate.Value;
                        break;
                    case TypeAmount:
                        if (AccountMappingBo != null)
                        {
                            if (!AccountMappingBo.IsBalanceSheet && AccountMappingBo.RetroRegisterFieldPickListDetailId.HasValue) // P&L
                                v1 = SetFormatValue(AccountMappingBo.RetroRegisterFieldPickListDetailBo.Code);
                            else 
                                v1 = BalanceSheetAmount(GetContractCode(RetroRegisterBo.ContractCode), AccountMappingBo.RetroRegisterFieldPickListDetailId.HasValue); // BalanceSheet
                        }
                        break;
                    case TypeDebitCredit:
                        if (AccountMappingBo != null)
                        {
                            if (!AccountMappingBo.IsBalanceSheet && AccountMappingBo.RetroRegisterFieldPickListDetailId.HasValue) // P&L
                            {
                                var amount = RetroRegisterBo.GetPropertyValue(AccountMappingBo.RetroRegisterFieldPickListDetailBo.Code) ?? 0;
                                if (AccountMappingBo.DebitCreditIndicatorPositive.HasValue && AccountMappingBo.DebitCreditIndicatorNegative.HasValue)
                                    v1 = Convert.ToDouble(amount) < 0 ? AccountCodeMappingBo.GetDebitCreditIndicatorName(AccountMappingBo.DebitCreditIndicatorNegative.Value) : AccountCodeMappingBo.GetDebitCreditIndicatorName(AccountMappingBo.DebitCreditIndicatorPositive.Value);
                            }
                            else
                            {
                                v1 = BalanceSheetDebitCredit(GetContractCode(RetroRegisterBo.ContractCode), AccountMappingBo);
                            }
                        }
                        break;
                    case TypeTransactionReference:
                        v1 = RetroRegisterBo.RetroStatementNo;
                        break;
                    case TypeDescription:
                        if (AccountMappingBo != null)
                        {
                            if (AccountMappingBo.RetroRegisterFieldPickListDetailBo != null && AccountMappingBo.RetroRegisterFieldPickListDetailBo.Code == PickListDetailBo.RetroRegisterFieldClaim)
                                v1 = "Reversal Provision";
                            else
                                v1 = RetroRegisterBo.AccountFor;
                        }
                        else
                            v1 = RetroRegisterBo.AccountFor;
                        break;
                    case TypeL1:
                        if (AccountMappingBo != null && AccountMappingBo.RetroRegisterFieldPickListDetailId.HasValue)
                        {
                            if (!AccountMappingBo.IsBalanceSheet)
                            {
                                switch (AccountMappingBo.RetroRegisterFieldPickListDetailBo.Code)
                                {
                                    case "Gross1st":
                                    case "Discount1st":
                                        v1 = "First Year";
                                        break;
                                    case "GrossRen":
                                    case "DiscountRen":
                                        v1 = "Renewal Year";
                                        break;
                                    case "AltPremium":
                                    case "DiscountAlt":
                                        v1 = "Alteration";
                                        break;
                                }
                            }                            
                        }
                        break;
                    case TypeL2:
                        v1 = RetroRegisterBo.RetroPartyBo?.Party;
                        break;
                    case TypeL3:
                        v1 = RetroRegisterBo.AnnualCohort;
                        break;
                    case TypeL8:
                        v1 = RetroRegisterBo.RiskQuarter;
                        break;
                    case TypeL10:
                        if (AccountMappingBo != null)
                            v1 = RetroRegisterBo.ContractCode;
                        break;
                    case TypeAddDescription1:
                        if (AccountMappingBo != null)
                        {
                            if (!AccountMappingBo.IsBalanceSheet && AccountMappingBo.RetroRegisterFieldPickListDetailId.HasValue)
                                v1 = AccountMappingBo.RetroRegisterFieldPickListDetailBo.Description;
                        }
                        break;
                    default:
                        break;
                }
                E2Ifrs17.WriteCell(E2Ifrs17.RowIndex, col.ColIndex.Value, v1);
            }
            E2Ifrs17.RowIndex++;
        }

        public void ProcessE2frs4Data(int layoutStart, RetroPartyBo RetroPartyBo = null, AccountCodeMappingBo AccountMappingBo = null)
        {
            if (RetroRegisterBo == null)
                return;

            string layout = layoutStart == 1 ? "1;3;6" : "6";
            string journal = "GJ";

            foreach (var col in Cols)
            {
                if (!col.ColIndex.HasValue)
                    continue;

                object v1 = null;

                switch (col.ColIndex)
                {
                    case TypeLayout:
                        v1 = layout;
                        break;
                    case TypeJournalType:
                        v1 = journal;
                        break;
                    case TypeAccountCode:
                        if (AccountMappingBo != null && AccountMappingBo.AccountCodeBo != null)
                            v1 = AccountMappingBo.AccountCodeBo.Code;

                        if (RetroPartyBo != null)
                            v1 = RetroPartyBo.AccountCode;
                        break;
                    case TypeAccountDescription:
                        if (AccountMappingBo != null && AccountMappingBo.AccountCodeBo != null)
                            v1 = AccountMappingBo.AccountCodeBo.Description;

                        if (RetroPartyBo != null)
                            v1 = RetroPartyBo.AccountCodeDescription;
                        break;
                    case TypePeriod:
                        if (RetroRegisterBo.RetroStatementDate != DateTime.MinValue && RetroRegisterBo.RetroStatementDate.HasValue)
                            v1 = SetAccountingPeriod(RetroRegisterBo.RetroStatementDate.Value);
                        break;
                    case TypeTransactionDate:
                        if (RetroRegisterBo.RetroStatementDate != DateTime.MinValue && RetroRegisterBo.RetroStatementDate.HasValue)
                            v1 = RetroRegisterBo.RetroStatementDate.Value;
                        break;
                    case TypeAmount:
                        if (AccountMappingBo != null && AccountMappingBo.RetroRegisterFieldPickListDetailId != null)
                        {
                            v1 = SetFormatValue(AccountMappingBo.RetroRegisterFieldPickListDetailBo.Code);
                            TotalAmount += Convert.ToDouble(v1);
                        }

                        if (RetroPartyBo != null)
                            v1 = Util.RoundNullableValue(TotalAmount ?? 0, 2);
                        break;
                    case TypeDebitCredit:
                        if (AccountMappingBo != null && AccountMappingBo.RetroRegisterFieldPickListDetailId != null)
                        {
                            var amount = RetroRegisterBo.GetPropertyValue(AccountMappingBo.RetroRegisterFieldPickListDetailBo.Code) ?? 0;
                            if (AccountMappingBo.DebitCreditIndicatorPositive.HasValue && AccountMappingBo.DebitCreditIndicatorNegative.HasValue)
                                v1 = Convert.ToDouble(amount) < 0 ? AccountCodeMappingBo.GetDebitCreditIndicatorName(AccountMappingBo.DebitCreditIndicatorNegative.Value) : AccountCodeMappingBo.GetDebitCreditIndicatorName(AccountMappingBo.DebitCreditIndicatorPositive.Value);
                        }

                        if (RetroPartyBo != null)
                            v1 = TotalAmount < 0 ? "D" : "C";
                        break;
                    case TypeTransactionReference:
                        v1 = RetroRegisterBo.RetroStatementNo;
                        break;
                    case TypeDescription:
                        if (AccountMappingBo != null && AccountMappingBo.RetroRegisterFieldPickListDetailId != null)
                        {
                            if (AccountMappingBo.RetroRegisterFieldPickListDetailBo.Code == PickListDetailBo.RetroRegisterFieldClaim)
                                v1 = "Reversal Provision";
                            else
                                v1 = RetroRegisterBo.AccountFor;
                        }
                        else
                            v1 = RetroRegisterBo.AccountFor;

                        if (RetroPartyBo != null)
                            v1 = RetroRegisterBo.AccountFor;
                        break;
                    case TypeL1:
                        v1 = RetroRegisterBo.TreatyType;
                        break;
                    case TypeL8:
                        v1 = RetroRegisterBo.RiskQuarter;
                        break;
                    default:
                        break;
                }
                E2Ifrs4.WriteCell(E2Ifrs4.RowIndex, col.ColIndex.Value, v1);
            }
            E2Ifrs4.RowIndex++;
        }

        public List<Column> GetColumns()
        {
            Cols = new List<Column>
            {
                new Column {
                    ColIndex = TypeLayout,
                },
                new Column {
                    ColIndex = TypeJournalType,
                },
                new Column {
                    ColIndex = TypeAccountCode,
                },
                new Column {
                    ColIndex = TypeAccountDescription,
                },
                new Column {
                    ColIndex = TypePeriod,
                },
                new Column {
                    ColIndex = TypeTransactionDate,
                },
                new Column {
                    ColIndex = TypeAmount,
                },
                new Column {
                    ColIndex = TypeDebitCredit,
                },
                new Column {
                    ColIndex = TypeTransactionReference,
                },
                new Column {
                    ColIndex = TypeDescription,
                },
                new Column {
                    ColIndex = TypeL1,
                },
                new Column {
                    ColIndex = TypeL2,
                },
                new Column {
                    ColIndex = TypeL3,
                },
                new Column {
                    ColIndex = TypeL4,
                },
                new Column {
                    ColIndex = TypeL5,
                },
                new Column {
                    ColIndex = TypeL6,
                },
                new Column {
                    ColIndex = TypeL7,
                },
                new Column {
                    ColIndex = TypeL8,
                },
                new Column {
                    ColIndex = TypeL9,
                },
                new Column {
                    ColIndex = TypeL10,
                },
                new Column {
                    ColIndex = TypeAddDescription1,
                },
                new Column {
                    ColIndex = TypeAddDescription2,
                },
                new Column {
                    ColIndex = TypeAddDescription3,
                },
                new Column {
                    ColIndex = TypeAddDescription4,
                },
                new Column {
                    ColIndex = TypeAddDescription5,
                },
            };

            return Cols;
        }

        public RetroRegisterBatchBo LoadRetroRegisterBatchBo()
        {
            RetroRegisterBatchBo = RetroRegisterBatchService.FindByStatus(RetroRegisterBatchBo.StatusSubmitForGenerate);
            if (RetroRegisterBatchBo != null)
            {
                RetroRegisterBos = RetroRegisterService.GetByRetroRegisterBatchId(RetroRegisterBatchBo.Id);
            }
            return RetroRegisterBatchBo;
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

            var retro = RetroRegisterBatchBo;
            RetroRegisterBatchBo.Status = status;

            Result result = RetroRegisterBatchService.Update(ref retro, ref trail);
            UserTrailBo userTrailBo = new UserTrailBo(
                RetroRegisterBatchBo.Id,
                des,
                result,
                trail,
                User.DefaultSuperUserId
            );
            UserTrailService.Create(ref userTrailBo);

            if (status == RetroRegisterBatchBo.StatusGenerating)
                ProcessingStatusHistoryBo = statusBo;
        }

        public void SaveFile()
        {
            var trail = new TrailObject();
            var bo = new RetroRegisterBatchFileBo
            {
                RetroRegisterBatchId = RetroRegisterBatchBo.Id,
                FileName = FileName1,
                HashFileName = FileName1,
                Status = RetroRegisterBatchFileBo.StatusCompleted,
                CreatedById = User.DefaultSuperUserId,
                UpdatedById = User.DefaultSuperUserId,
            };
            RetroRegisterBatchFileService.Create(ref bo, ref trail);

            var bo2 = new RetroRegisterBatchFileBo
            {
                RetroRegisterBatchId = RetroRegisterBatchBo.Id,
                FileName = FileName2,
                HashFileName = FileName2,
                Status = RetroRegisterBatchFileBo.StatusCompleted,
                CreatedById = User.DefaultSuperUserId,
                UpdatedById = User.DefaultSuperUserId,
            };
            RetroRegisterBatchFileService.Create(ref bo2, ref trail);
        }

        public void DeleteBatchSungFile()
        {
            WriteStatusLogFile("Deleting Sungl Files...");
            var files = RetroRegisterBatchFileService.GetByRetroRegisterBatchId(RetroRegisterBatchBo.Id);
            foreach (RetroRegisterBatchFileBo file in files)
            {
                string fileE2 = Path.Combine(Util.GetE2Path(), file.HashFileName);
                if (File.Exists(fileE2))
                    File.Delete(fileE2);
            }
            RetroRegisterBatchFileService.DeleteAllByRetroRegisterBatchId(RetroRegisterBatchBo.Id);
            WriteStatusLogFile("Deleted Sungl Files", true);
        }

        public void GenerateE2IFRS4()
        {
            if (RetroRegisterBo == null)
                return;

            RetroRegisterPickListBo = PickListDetailService.GetByPickListId(PickListBo.RetroRegisterField);
            RetroRegisterFieldIds = RetroRegisterPickListBo.Select(q => q.Id).ToList();

            try
            {
                GetColumns();

                HandleTempDirectory();

                E2Ifrs4.OpenTemplate();

                switch (RetroRegisterBo.Type)
                {
                    case RetroRegisterBo.TypeDirectRetro:
                        ProcessE2Ifrs4(RetroRegisterBo);
                        break;
                    case RetroRegisterBo.TypePerLifeRetro:
                        ProcessE2Ifrs4(RetroRegisterBo);
                        break;
                }

                E2Ifrs4.Save();
            }
            catch (Exception e)
            {
                PrintMessage(e);
            }
        }

        public void GenerateE2IFRS17()
        {
            if (RetroRegisterBo == null)
                return;

            //if (RetroRegisterBos.IsNullOrEmpty())
            //    return;

            RetroRegisterPickListBo = PickListDetailService.GetByPickListId(PickListBo.RetroRegisterField);
            RetroRegisterFieldIds = RetroRegisterPickListBo.Select(q => q.Id).ToList();

            var ifrs4RetroRegisterBo = RetroRegisterBo;
            try
            {
                GetColumns();

                HandleTempDirectory();

                E2Ifrs17.OpenTemplate();

                switch (RetroRegisterBo.Type)
                {
                    case RetroRegisterBo.TypeDirectRetro:                        
                        foreach (var statementNo in RetroRegisterBos.GroupBy(q => q.RetroStatementNo).Select(q => q.Key).ToArray())
                        {
                            Index = 1;
                            TotalAmount = 0;

                            RetroRegisterBo = null;
                            foreach (var bo in RetroRegisterBos.Where(q => q.RetroStatementNo == statementNo).OrderByDescending(q => q.AnnualCohort.HasValue).ThenBy(q => q.AnnualCohort))
                            {
                                RetroRegisterBo = bo;
                                ProcessE2Ifrs17(RetroRegisterBo, RetroRegisterFieldIds);
                            }

                            // for Balance Sheet
                            foreach (var treatyNo in RetroRegisterBos.Where(q => q.ReportingType == RetroRegisterBo.ReportingTypeIFRS17 && q.RetroStatementNo == statementNo && !string.IsNullOrEmpty(q.TreatyNumber)).GroupBy(q => q.TreatyNumber.Trim()).Select(q => q.Key).ToArray())
                            {
                                var AccountMappingBalanceSheetBos = AccountCodeMappingService.FindByE2Ifrs17Param(treatyNumber: treatyNo, isBalanceSheet: true);
                                foreach (var accountMappingBalanceSheetBo in AccountMappingBalanceSheetBos.OrderByDescending(q => q.RetroRegisterFieldPickListDetailId))
                                {
                                    if (accountMappingBalanceSheetBo.RetroRegisterFieldPickListDetailId.HasValue)
                                    {
                                        foreach (var bo in RetroRegisterBos.Where(q => q.ReportingType == RetroRegisterBo.ReportingTypeIFRS17 && q.RetroStatementNo == statementNo && q.TreatyNumber == treatyNo && q.AnnualCohort.HasValue).OrderBy(q => q.AnnualCohort))
                                        {
                                            RetroRegisterBo = bo;

                                            ProcessE2frs17Data(Index, accountMappingBalanceSheetBo);
                                            Index++;
                                        }
                                    }
                                    else
                                    {
                                        foreach (var bo in RetroRegisterBos.Where(q => q.ReportingType == RetroRegisterBo.ReportingTypeIFRS17 && q.RetroStatementNo == statementNo && q.TreatyNumber == treatyNo).OrderByDescending(q => q.AnnualCohort.HasValue).ThenBy(q => q.AnnualCohort))
                                        {
                                            RetroRegisterBo = bo;

                                            ProcessE2frs17Data(Index, accountMappingBalanceSheetBo);
                                            Index++;
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    case RetroRegisterBo.TypePerLifeRetro:
                        foreach (var statementNo in RetroRegisterBos.GroupBy(q => q.RetroStatementNo).Select(q => q.Key).ToArray())
                        {
                            Index = 1;
                            TotalAmount = 0;

                            RetroRegisterBo = null;
                            foreach (var bo in RetroRegisterBos.Where(q => q.RetroStatementNo == statementNo).OrderByDescending(q => q.AnnualCohort.HasValue).ThenBy(q => q.AnnualCohort))
                            {
                                RetroRegisterBo = bo;
                                ProcessE2Ifrs17(RetroRegisterBo, RetroRegisterFieldIds);
                            }

                            // for Balance Sheet
                            foreach (var treatyNo in RetroRegisterBos.Where(q => q.ReportingType == RetroRegisterBo.ReportingTypeIFRS17 && q.RetroStatementNo == statementNo && !string.IsNullOrEmpty(q.TreatyNumber)).GroupBy(q => q.TreatyNumber.Trim()).Select(q => q.Key).ToArray())
                            {
                                var AccountMappingBalanceSheetBos = AccountCodeMappingService.FindByE2Ifrs17Param(treatyNumber: treatyNo, isBalanceSheet: true);
                                foreach (var accountMappingBalanceSheetBo in AccountMappingBalanceSheetBos.OrderByDescending(q => q.RetroRegisterFieldPickListDetailId))
                                {
                                    if (accountMappingBalanceSheetBo.RetroRegisterFieldPickListDetailId.HasValue)
                                    {
                                        foreach (var bo in RetroRegisterBos.Where(q => q.ReportingType == RetroRegisterBo.ReportingTypeIFRS17 && q.RetroStatementNo == statementNo && q.TreatyNumber == treatyNo && q.AnnualCohort.HasValue).OrderBy(q => q.AnnualCohort))
                                        {
                                            RetroRegisterBo = bo;

                                            ProcessE2frs17Data(Index, accountMappingBalanceSheetBo);
                                            Index++;
                                        }
                                    }
                                    else
                                    {
                                        foreach (var bo in RetroRegisterBos.Where(q => q.ReportingType == RetroRegisterBo.ReportingTypeIFRS17 && q.RetroStatementNo == statementNo && q.TreatyNumber == treatyNo).OrderByDescending(q => q.AnnualCohort.HasValue).ThenBy(q => q.AnnualCohort))
                                        {
                                            RetroRegisterBo = bo;

                                            ProcessE2frs17Data(Index, accountMappingBalanceSheetBo);
                                            Index++;
                                        }
                                    }
                                }
                            }
                        }
                        break;
                }

                E2Ifrs17.Save();
            }
            catch (Exception e)
            {
                PrintMessage(e);
            }
        }

        public void HandleTempDirectory()
        {
            string extension = ".xlsx";
            var templateFilepath = Util.GetWebAppDocumentFilePath("E2_MFRS17_Template.xlsx");
            var templateFilepath2 = Util.GetWebAppDocumentFilePath("E2_Template.xlsx");

            string PrefixFileName = "";
            if (GenerateIFRS4) PrefixFileName = string.Format("E2_IFRS4");
            if (GenerateIFRS17) PrefixFileName = string.Format("E2_IFRS17");

            var directory = Util.GetTemporaryPath();
            FileName = PrefixFileName.AppendDateTimeFileName(extension);
            FilePath = Path.Combine(directory, FileName);            

            if (GenerateIFRS17) E2Ifrs17 = new Excel(templateFilepath, FilePath, 15);
            if (GenerateIFRS4) E2Ifrs4 = new Excel(templateFilepath2, FilePath, 15);

            // Delete all previous files
            Util.DeleteFiles(directory, $@"{PrefixFileName}*");
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

        public string SetAccountingPeriod(DateTime? invoiceDate)
        {
            int Year = DateTime.Parse(invoiceDate?.ToString()).Year;
            int Month = DateTime.Parse(invoiceDate?.ToString()).Month;
            int Day = DateTime.Parse(invoiceDate?.ToString()).Day;

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

                if ((startDate <= invoiceDate) && (invoiceDate <= endDate))
                    accountingPeriod = period.ToString();
            }
            return accountingPeriod;
        }

        public double SetFormatValue(string code)
        {
            string format = RetroRegisterBo.GetFormatValueByCode(code);
            object value = RetroRegisterBo.GetPropertyValue(code) ?? 0;

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

        public double BalanceSheetAmount(string contractCode, bool premium = false)
        {
            var AccountMappingBos = AccountCodeMappingService.FindByE2Ifrs17Param(contractCode);

            double amount = 0;
            if (premium) // premium
            {
                if (!AccountMappingBos.IsNullOrEmpty())
                {
                    var PremiumAccountMappingBos = AccountMappingBos.Where(q => RetroRegisterBo.GetBalanceSheetPremiumItems().Contains(q.RetroRegisterFieldPickListDetailBo.Code)).ToList();

                    double premiumAmount = 0;                  
                    foreach (var accountMappingBo in PremiumAccountMappingBos)
                    {
                        premiumAmount = premiumAmount + SetFormatValue(accountMappingBo.RetroRegisterFieldPickListDetailBo.Code);
                    }
                    amount = premiumAmount;
                }
            }
            else // trade creditor
            {
                if (!AccountMappingBos.IsNullOrEmpty())
                {
                    double creditorAmount = 0;
                    var TradeCreditorAccountMappingBos = AccountMappingBos.Where(q => RetroRegisterBo.GetBalanceSheetCreditorItems().Contains(q.RetroRegisterFieldPickListDetailBo.Code)).ToList();

                    foreach (var accountMappingBo in TradeCreditorAccountMappingBos)
                    {
                        creditorAmount = creditorAmount + SetFormatValue(accountMappingBo.RetroRegisterFieldPickListDetailBo.Code);
                    }
                    amount = creditorAmount;
                }
            }
            return amount;
        }

        public string BalanceSheetDebitCredit(string contractCode, AccountCodeMappingBo AccountMappingBo)
        {
            double amount = BalanceSheetAmount(contractCode, AccountMappingBo.RetroRegisterFieldPickListDetailId.HasValue);
            if (AccountMappingBo.DebitCreditIndicatorPositive.HasValue && AccountMappingBo.DebitCreditIndicatorNegative.HasValue)
                return (Convert.ToDouble(amount) < 0 ? AccountCodeMappingBo.GetDebitCreditIndicatorName(AccountMappingBo.DebitCreditIndicatorNegative.Value) : AccountCodeMappingBo.GetDebitCreditIndicatorName(AccountMappingBo.DebitCreditIndicatorPositive.Value));

            return "";
        }
    }
}
