using BusinessObject;
using BusinessObject.Identity;
using BusinessObject.TreatyPricing;
using DataAccess.Entities.Identity;
using Newtonsoft.Json;
using Services;
using Services.Identity;
using Services.TreatyPricing;
using Shared;
using Shared.DataAccess;
using Shared.ProcessFile;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;

namespace ConsoleApp.Commands.RawFiles.TreatyPricing
{
    public class ProcessTreatyPricingRateTableGroup : Command
    {
        public int? TreatyPricingRateTableGroupId { get; set; }

        public TreatyPricingRateTableGroupBo TreatyPricingRateTableGroupBo { get; set; }

        public IList<TreatyPricingRateTableBo> TreatyPricingRateTableBos { get; set; }

        public List<string> TreatyPricingRateTableIds { get; set; }

        public TreatyPricingRateTableBo TreatyPricingRateTableBo { get; set; }

        public TreatyPricingRateTableVersionBo TreatyPricingRateTableVersionBo { get; set; }

        public TreatyPricingRateTableRateBo TreatyPricingRateTableRateBo { get; set; }

        public List<TreatyPricingRateTableRateBo> TreatyPricingRateTableRateBos { get; set; }

        public TreatyPricingRateTableRateBo PreviousRateBo { get; set; }

        public TreatyPricingRateTableRateBo CurrentRateBo { get; set; }

        public TreatyPricingRateTableOriginalRateBo TreatyPricingRateTableOriginalRateBo { get; set; }

        public List<TreatyPricingRateTableOriginalRateBo> TreatyPricingRateTableOriginalRateBos { get; set; }

        public List<Column> Columns { get; set; }

        public List<Column> RateColumns { get; set; }

        public ModuleBo ModuleBo { get; set; }

        public Excel Excel { get; set; }

        public List<string> Errors { get; set; }

        public dynamic CellValue { get; set; }

        public object Value { get; set; }

        public int PossibleRateTable { get; set; }

        public int StartCol { get; set; }

        public bool IsNewRate { get; set; }

        public bool IsVersionUpdate { get; set; }

        public bool IsStatusChange { get; set; }

        public string NewStatus { get; set; }

        public bool IsProcessVersionRate { get; set; }

        public const int HeaderCol = 2;

        public const int RateTableCol = 9;

        public const int RowRateTableGroupId = 1;
        public const int RowRateTableId = 2;
        public const int RowVersion = 3;
        public const int RowStatus = 4;
        public const int RowBenefitCode = 5;
        public const int RowBenefitName = 6;
        public const int RowRiDiscount = 7;
        public const int RowCoinsuranceRiDiscount = 8;
        public const int RowProfitComm = 9;
        public const int RowRateGuarantee = 10;
        public const int RowAdvantageProgram = 11;
        public const int RowAgeBasis = 12;
        public const int RowRateFormat = 13; // Rate Header
        public const int RowRateIndicator = 14; // Set Indicator
        public const int RowRate = 15; // Yes/No

        public const int RowStartAge = 16; // 0
        public const int RowEndAge = 116; // 100

        // Rate col
        public const int TypeMns = 1;
        public const int TypeMs = 2;
        public const int TypeFns = 3;
        public const int TypeFs = 4;
        public const int TypeM = 5;
        public const int TypeF = 6;
        public const int TypeUnisex = 7;
        public const int TypeUnitRate = 8;
        public const int TypeOccClass = 9;

        public ProcessTreatyPricingRateTableGroup()
        {
            Title = "ProcessTreatyPricingRateTableGroup";
            Description = "To process Treaty Pricing Rate Table Group";
            Options = new string[] {
                "--treatyPricingRateTableGroupId= : Process by Id",
            };
        }

        public override void Initial()
        {
            base.Initial();
            TreatyPricingRateTableGroupId = OptionIntegerNullable("treatyPricingRateTableGroupId");
            StartCol = HeaderCol + 1; // Read From 3rd column (1st & 2nd cols is header)
            TreatyPricingRateTableIds = new List<string> { };
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

        public override void Run()
        {
            try
            {
                if (TreatyPricingRateTableGroupId.HasValue)
                {
                    TreatyPricingRateTableGroupBo = TreatyPricingRateTableGroupService.Find(TreatyPricingRateTableGroupId.Value);
                    if (TreatyPricingRateTableGroupBo != null && TreatyPricingRateTableGroupBo.Status != TreatyPricingRateTableGroupBo.StatusPending)
                    {
                        Log = false;
                        PrintMessage(MessageBag.NoTreatyPricingRateTableGroupPendingProcess);
                        return;
                    }
                }
                else if (TreatyPricingRateTableGroupService.CountByStatus(TreatyPricingRateTableGroupBo.StatusPending) == 0)
                {
                    Log = false;
                    PrintMessage(MessageBag.NoTreatyPricingRateTableGroupPendingProcess);
                    return;
                }
                PrintStarting();

                ModuleBo = ModuleService.FindByController(ModuleBo.ModuleController.TreatyPricingCedant.ToString());

                while (LoadTreatyPricingRateTableGroupBo() != null)
                {
                    if (GetProcessCount("Group") > 0)
                        PrintProcessCount();
                    SetProcessCount("Group");

                    PrintOutputTitle(string.Format("Process Treaty Pricing Rate Table Group Id: {0}", TreatyPricingRateTableGroupBo.Id));

                    UpdateTreatyPricingRateTableGroupStatus(TreatyPricingRateTableGroupBo.StatusProcessing, MessageBag.ProcessTreatyPricingRateTableGroupProcessing);

                    bool success = ProcessFile();

                    if (success)
                    {
                        UpdateTreatyPricingRateTableGroupStatus(TreatyPricingRateTableGroupBo.StatusSuccess, MessageBag.ProcessTreatyPricingRateTableGroupSuccess);
                    }
                    else
                    {
                        UpdateTreatyPricingRateTableGroupStatus(TreatyPricingRateTableGroupBo.StatusFailed, MessageBag.ProcessTreatyPricingRateTableGroupFailed);
                    }
                }

                PrintEnding();
            }
            catch (Exception e)
            {
                PrintError(e.Message);
            }
        }

        public TreatyPricingRateTableGroupBo LoadTreatyPricingRateTableGroupBo()
        {
            TreatyPricingRateTableGroupBo = null;
            Errors = new List<string>();
            if (TreatyPricingRateTableGroupId.HasValue)
            {
                TreatyPricingRateTableGroupBo = TreatyPricingRateTableGroupService.Find(TreatyPricingRateTableGroupId.Value);
                if (TreatyPricingRateTableGroupBo != null && TreatyPricingRateTableGroupBo.Status != TreatyPricingRateTableGroupBo.StatusPending)
                    TreatyPricingRateTableGroupBo = null;
            }
            else
                TreatyPricingRateTableGroupBo = TreatyPricingRateTableGroupService.FindByStatus(TreatyPricingRateTableGroupBo.StatusPending);

            if (TreatyPricingRateTableGroupBo != null)
                TreatyPricingRateTableBos = TreatyPricingRateTableService.GetByTreatyPricingRateTableGroupId(TreatyPricingRateTableGroupBo.Id);

            return TreatyPricingRateTableGroupBo;
        }

        public void UpdateTreatyPricingRateTableGroupStatus(int status, string description)
        {
            var trail = new TrailObject();

            var treatyPricingRateTableGroup = TreatyPricingRateTableGroupBo;
            treatyPricingRateTableGroup.Status = status;
            treatyPricingRateTableGroup.NoOfRateTable = TreatyPricingRateTableService.CountByTreatyPricingRateTableGroupId(TreatyPricingRateTableGroupBo.Id);

            var result = TreatyPricingRateTableGroupService.Update(ref treatyPricingRateTableGroup, ref trail);
            var userTrailBo = new UserTrailBo(
                TreatyPricingRateTableGroupBo.Id,
                description,
                result,
                trail,
                User.DefaultSuperUserId
            );
            UserTrailService.Create(ref userTrailBo);
        }

        public string GetFilePath()
        {
            if (TreatyPricingRateTableGroupBo != null)
                return TreatyPricingRateTableGroupBo.GetLocalPath();
            return null;
        }

        public bool ProcessFile()
        {
            if (TreatyPricingRateTableGroupBo == null)
                return false;

            PrintOutputTitle("Process File");

            try
            {
                var filePath = GetFilePath();
                if (!File.Exists(filePath))
                    throw new Exception(string.Format(MessageBag.FileNotExists, filePath));

                Excel = new Excel(GetFilePath());

                if (Excel == null)
                    throw new Exception(string.Format(MessageBag.FileNotSupport, filePath));

                GetPossibleRateTable();

                int startCol = StartCol;
                if (ValidateData())
                {
                    StartCol = startCol;
                    ProcessData();
                }

                if (Excel != null)
                    Excel.Close();

                if (!Errors.IsNullOrEmpty())
                {
                    TreatyPricingRateTableGroupBo.Errors = JsonConvert.SerializeObject(Errors);
                    return false;
                }
                else
                {
                    TreatyPricingRateTableGroupBo.Errors = null;
                }
            }
            catch (Exception e)
            {
                if (Excel != null)
                    Excel.Close();

                var errors = new List<string> { };
                var message = e.Message;
                if (e is DbEntityValidationException dbEx)
                    message = Util.CatchDbEntityValidationException(dbEx).ToString();

                errors.Add(message);

                TreatyPricingRateTableGroupBo.Errors = JsonConvert.SerializeObject(errors);
                return false;
            }

            return true;
        }

        public bool ReadCellValue(int rowIndex, int colIndex)
        {
            CellValue = (Excel.XWorkSheet.Cells[rowIndex, colIndex] as Microsoft.Office.Interop.Excel.Range).Value;
            Value = CellValue;
            if (CellValue == null || (CellValue is string && string.IsNullOrEmpty(CellValue)))
                return false;
            return true;
        }

        public bool ReadRequiredCellValue(int rowIndex, int colIndex, int rateTable, string property, ref List<string> errors)
        {
            if (!ReadCellValue(rowIndex, colIndex))
            {
                errors.Add(string.Format("Rate Table {0}: {1}'s Value is empty", rateTable, property));
                return false;
            }
            return true;
        }

        public void GetPossibleRateTable()
        {
            if (StartCol >= Excel.TotalCol)
                throw new Exception(string.Format("No readable column for Rate Table found"));

            PossibleRateTable = 0;
            for (int i = StartCol; i < Excel.TotalCol; i += RateTableCol)
            {
                if (ReadCellValue(RowRateTableGroupId, StartCol) || ReadCellValue(RowRateTableId, StartCol))
                    PossibleRateTable++;
            }

            if (PossibleRateTable == 0)
                throw new Exception(string.Format("No Rate Table column found"));
        }

        public bool ValidateData()
        {
            for (int i = 1; i <= PossibleRateTable; i++)
            {
                if (GetProcessCount("Validate Rate Table") > 0)
                    PrintProcessCount();
                SetProcessCount("Validate Rate Table");

                //Reset Values
                TreatyPricingRateTableBo = null;
                TreatyPricingRateTableVersionBo = null;
                TreatyPricingRateTableRateBos = new List<TreatyPricingRateTableRateBo> { };
                TreatyPricingRateTableOriginalRateBos = new List<TreatyPricingRateTableOriginalRateBo> { };
                IsNewRate = false;
                IsVersionUpdate = false;
                IsStatusChange = false;
                NewStatus = "";
                IsProcessVersionRate = true;
                RateColumns = GetRateColumns();

                ProcessRateTableGroupId(i, true);
                ProcessRateTable(i, true);
                ProcessRateTableVersion(i, true);
                ProcessRateTableRate(i, true);

                StartCol += RateTableCol;
            }
            PrintProcessCount();

            if (!Errors.IsNullOrEmpty())
                return false;
            return true;
        }

        public void ProcessData()
        {
            for (int i = 1; i <= PossibleRateTable; i++)
            {
                if (GetProcessCount("Process Rate Table") > 0)
                    PrintProcessCount();
                SetProcessCount("Process Rate Table");

                //Reset Values
                TreatyPricingRateTableBo = null;
                TreatyPricingRateTableVersionBo = null;
                TreatyPricingRateTableRateBos = new List<TreatyPricingRateTableRateBo> { };
                TreatyPricingRateTableOriginalRateBos = new List<TreatyPricingRateTableOriginalRateBo> { };
                IsNewRate = false;
                IsVersionUpdate = false;
                IsStatusChange = false;
                NewStatus = "";
                IsProcessVersionRate = true;
                RateColumns = GetRateColumns();

                ProcessRateTableGroupId(i);
                ProcessRateTable(i);
                ProcessRateTableVersion(i);
                ProcessRateTableRate(i);

                Save();

                StartCol += RateTableCol;
            }
            PrintProcessCount();
        }

        public bool ProcessRateTableGroupId(int rateTable, bool isValidating = false)
        {
            if (isValidating)
            {
                List<string> errors = new List<string> { };
                if (ReadRequiredCellValue(RowRateTableGroupId, StartCol, rateTable, "RateTableGroupId", ref errors))
                {
                    string rateTableGroupId = CellValue is string ? CellValue?.Trim() : Value.ToString()?.Trim();
                    if (rateTableGroupId.Equals(TreatyPricingRateTableGroupBo.Code))
                        return true;
                    errors.Add(string.Format("Rate Table {0}: {1}'s Value: \"{2}\" does not match with current RateTableGroupId: \"{3}\"", rateTable, "RateTableGroupId", rateTableGroupId, TreatyPricingRateTableGroupBo.Code));
                }

                if (!errors.IsNullOrEmpty())
                {
                    Errors.AddRange(errors);
                    return false;
                }
            }
            return true;
        }

        public bool ProcessRateTable(int rateTable, bool isValidating = false)
        {
            List<string> errors = new List<string> { };
            if (ReadRequiredCellValue(RowRateTableId, StartCol, rateTable, "RateTableId", ref errors))
            {
                string rateTableId = CellValue is string ? CellValue?.Trim() : Value.ToString()?.Trim();

                if (rateTableId.Trim().ToLower() == "new")
                {
                    IsNewRate = true;
                    TreatyPricingRateTableBo = new TreatyPricingRateTableBo();
                }
                else
                {
                    TreatyPricingRateTableBo = TreatyPricingRateTableBos.Where(q => q.Code == rateTableId).FirstOrDefault();
                    if (isValidating && TreatyPricingRateTableBo == null)
                        errors.Add(string.Format("Rate Table {0}: {1}'s Value \"{2}\" not existed", rateTable, "RateTableId", rateTableId));
                }

                if (!IsNewRate && isValidating)
                {
                    if (TreatyPricingRateTableIds.Contains(rateTableId))
                    {
                        errors.Add(string.Format("Rate Table {0}: {1}'s Value \"{2}\" found duplicate within the file", rateTable, "RateTableId", rateTableId));
                    }
                    else
                    {
                        TreatyPricingRateTableIds.Add(rateTableId);
                    }
                }
            }

            if (ReadRequiredCellValue(RowBenefitCode, StartCol, rateTable, "BenefitCode", ref errors))
            {
                string benefitCode = CellValue is string ? CellValue?.Trim() : Value.ToString()?.Trim();
                BenefitBo benefitBo = BenefitService.FindByCode(benefitCode);
                if (isValidating)
                {
                    if (benefitBo == null)
                    {
                        errors.Add(string.Format("Rate Table {0}: {1}'s Value \"{2}\" not exists", rateTable, "BenefitCode", benefitCode));
                    }
                    else if (TreatyPricingRateTableBo != null && TreatyPricingRateTableBo.Id > 0 && benefitBo != null && benefitBo.Id != TreatyPricingRateTableBo.BenefitId)
                    {
                        errors.Add(string.Format("Rate Table {0}: {1}'s Value \"{2}\" does not match with current Rate Table's benefit code: \"{3}\"", rateTable, "BenefitCode", benefitCode, TreatyPricingRateTableBo.BenefitBo.Code));
                    }
                }
                else
                {
                    TreatyPricingRateTableBo.BenefitId = benefitBo.Id;
                    TreatyPricingRateTableBo.BenefitBo = benefitBo;
                }
            }

            if (ReadRequiredCellValue(RowStatus, StartCol, rateTable, "Status", ref errors))
            {
                string status = CellValue is string ? CellValue?.Trim() : Value.ToString()?.Trim();
                NewStatus = status.Trim().ToLower();

                if (isValidating)
                {
                    if (NewStatus != "active" && NewStatus != "inactive")
                    {
                        errors.Add(string.Format("Rate Table {0}: {1}'s Value must be either \"Active\" or \"Inactive\" only", rateTable, "Status"));
                    }
                    else if (IsNewRate && NewStatus != "active")
                    {
                        errors.Add(string.Format("Rate Table {0}: {1}'s Value must be \"Active\" for new Rate Table", rateTable, "Status"));
                    }
                    else if (!IsNewRate && TreatyPricingRateTableBo != null && TreatyPricingRateTableBo.Id != 0)
                    {
                        string currentStatus = TreatyPricingRateTableBo.GetStatusName(TreatyPricingRateTableBo.Status).ToLower();
                        IsStatusChange = currentStatus != NewStatus;
                    }
                }
                else
                {
                    if (!IsNewRate && TreatyPricingRateTableBo != null && TreatyPricingRateTableBo.Id != 0)
                    {
                        string currentStatus = TreatyPricingRateTableBo.GetStatusName(TreatyPricingRateTableBo.Status).ToLower();
                        IsStatusChange = currentStatus != NewStatus;
                    }
                    int statusKey = 0;
                    switch (NewStatus)
                    {
                        case "active":
                            statusKey = TreatyPricingRateTableBo.StatusActive;
                            break;
                        case "inactive":
                            statusKey = TreatyPricingRateTableBo.StatusInactive;
                            break;
                    }
                    TreatyPricingRateTableBo.Status = statusKey;
                }
            }

            if (isValidating && !errors.IsNullOrEmpty())
            {
                Errors.AddRange(errors);
                return false;
            }
            return true;
        }

        public bool ProcessRateTableVersion(int rateTable, bool isValidating = false)
        {
            List<string> errors = new List<string> { };
            if (ReadRequiredCellValue(RowVersion, StartCol, rateTable, "Version", ref errors))
            {
                int? version = Util.GetParseInt(Value.ToString()?.Trim());
                if (isValidating)
                {
                    if (!version.HasValue)
                        errors.Add(string.Format("Rate Table {0}: {1}'s Value \"{2}\" is not numeric", rateTable, "Version", CellValue.ToString()));
                    else
                    {
                        if (!IsNewRate && TreatyPricingRateTableBo != null && TreatyPricingRateTableBo.Id != 0)
                        {
                            var bo = TreatyPricingRateTableVersionService.FindLatestByTreatyPricingRateTableId(TreatyPricingRateTableBo.Id);
                            IsVersionUpdate = bo != null ? bo.Version != version : false; // Bo should Won't be null
                            if (IsVersionUpdate && version > bo.Version + 1)
                                errors.Add(string.Format("Rate Table {0}: {1}'s Value \"{2}\" must be one number greater than current Rate Table's version: \"{3}\"", rateTable, "Version", version.Value, bo.Version));

                            if (NewStatus == "inactive" && IsStatusChange && IsVersionUpdate)
                            {
                                errors.Add(string.Format("Rate Table {0}: {1}'s Value must be same as current version to set status as Inactive", rateTable, "Version"));
                            }
                            else if (!IsVersionUpdate)
                            {
                                IsProcessVersionRate = false;
                            }
                        }
                        else if (version.Value != 1)
                        {
                            errors.Add(string.Format("Rate Table {0}: {1}'s Value \"{2}\" must be equal to 1 for new Rate Table", rateTable, "Version", version.Value));
                        }
                    }
                }
                else
                {
                    if (!IsNewRate && TreatyPricingRateTableBo != null && TreatyPricingRateTableBo.Id != 0)
                    {
                        var bo = TreatyPricingRateTableVersionService.FindLatestByTreatyPricingRateTableId(TreatyPricingRateTableBo.Id);
                        IsVersionUpdate = bo != null ? bo.Version != version : false;

                        if (!IsVersionUpdate)
                        {
                            IsProcessVersionRate = false;
                        }
                    }
                    if (IsProcessVersionRate)
                        TreatyPricingRateTableVersionBo = new TreatyPricingRateTableVersionBo { Version = version.Value, };
                }
            }

            if (IsProcessVersionRate)
            {
                if (isValidating)
                {
                    if (ReadCellValue(RowRateGuarantee, StartCol))
                    {
                        string rateGuarantee = CellValue is string ? CellValue?.Trim() : Value.ToString()?.Trim();

                        var bo = PickListDetailService.FindByPickListIdCode(PickListBo.RateGuarantee, rateGuarantee);
                        if (bo == null)
                        {
                            errors.Add(string.Format("Rate Table {0}: {1}'s Value \"{2}\" not exists", rateTable, "RateGuarantee", rateGuarantee));
                        }
                    }

                    if (ReadRequiredCellValue(RowAgeBasis, StartCol, rateTable, "AgeBasis", ref errors))
                    {
                        string ageBasis = CellValue is string ? CellValue?.Trim() : Value.ToString()?.Trim();

                        var bo = PickListDetailService.FindByPickListIdCode(PickListBo.AgeBasis, ageBasis);
                        if (bo == null)
                        {
                            errors.Add(string.Format("Rate Table {0}: {1}'s Value \"{2}\" not exists", rateTable, "AgeBasis", ageBasis));
                        }
                    }
                }
                else
                {
                    if (ReadCellValue(RowBenefitName, StartCol))
                    {
                        string benefitName = CellValue is string ? CellValue?.Trim() : Value.ToString()?.Trim();
                        TreatyPricingRateTableVersionBo.BenefitName = benefitName;
                    }

                    if (ReadCellValue(RowRiDiscount, StartCol))
                    {
                        string riDiscount = CellValue is string ? CellValue?.Trim() : Value.ToString()?.Trim();
                        TreatyPricingRateTableVersionBo.RiDiscount = riDiscount;
                    }

                    if (ReadCellValue(RowCoinsuranceRiDiscount, StartCol))
                    {
                        string coinsuranceRiDiscount = CellValue is string ? CellValue?.Trim() : Value.ToString()?.Trim();
                        TreatyPricingRateTableVersionBo.CoinsuranceRiDiscount = coinsuranceRiDiscount;
                    }

                    if (ReadCellValue(RowProfitComm, StartCol))
                    {
                        string profitCommission = CellValue is string ? CellValue?.Trim() : Value.ToString()?.Trim();
                        TreatyPricingRateTableVersionBo.ProfitCommission = profitCommission;
                    }

                    if (ReadCellValue(RowRateGuarantee, StartCol))
                    {
                        string rateGuarantee = CellValue is string ? CellValue?.Trim() : Value.ToString()?.Trim();
                        var bo = PickListDetailService.FindByPickListIdCode(PickListBo.RateGuarantee, rateGuarantee);
                        TreatyPricingRateTableVersionBo.RateGuaranteePickListDetailId = bo?.Id;
                        TreatyPricingRateTableVersionBo.RateGuaranteePickListDetailBo = bo;
                    }

                    if (ReadCellValue(RowAdvantageProgram, StartCol))
                    {
                        string advantageProgram = CellValue is string ? CellValue?.Trim() : Value.ToString()?.Trim();
                        TreatyPricingRateTableVersionBo.AdvantageProgram = advantageProgram;
                    }

                    if (ReadRequiredCellValue(RowAgeBasis, StartCol, rateTable, "AgeBasis", ref errors))
                    {
                        string ageBasis = CellValue is string ? CellValue?.Trim() : Value.ToString()?.Trim();
                        var bo = PickListDetailService.FindByPickListIdCode(PickListBo.AgeBasis, ageBasis);
                        TreatyPricingRateTableVersionBo.AgeBasisPickListDetailId = bo?.Id;
                        TreatyPricingRateTableVersionBo.AgeBasisPickListDetailBo = bo;
                    }
                }
            }

            //if (isValidating && !errors.IsNullOrEmpty())
            //{
            //    StartCol += RateTableCol;
            //    Errors.AddRange(errors);
            //    return false;
            //}

            if (isValidating && !errors.IsNullOrEmpty())
            {
                //StartCol += RateTableCol;
                Errors.AddRange(errors);
                return false;
            }
            return true;
        }

        public bool ProcessRateTableRate(int rateTable, bool isValidating = false)
        {
            if (!IsProcessVersionRate)
                return true;

            List<string> errors = new List<string> { };
            if (ProcessRateHeader(rateTable, ref errors))
            {
                int age = 0;
                for (int i = RowStartAge; i <= RowEndAge; i++)
                {
                    if (isValidating)
                    {
                        foreach (Column col in RateColumns)
                        {
                            if (!col.IsValueExist)
                                continue;

                            if (ReadCellValue(i, col.ColIndex.Value))
                            {
                                double? d = Util.StringToDouble(Value.ToString()?.Trim(), true, 2);
                                if (!d.HasValue)
                                    errors.Add(string.Format("Rate Table {0}: {1}'s Value \"{2}\" for age {3} is not numeric", rateTable, col.Property, Value.ToString(), age));
                            }
                        }
                    }
                    else
                    {
                        TreatyPricingRateTableRateBo = new TreatyPricingRateTableRateBo { Age = age };
                        TreatyPricingRateTableOriginalRateBo = new TreatyPricingRateTableOriginalRateBo { Age = age };
                        foreach (Column col in RateColumns)
                        {
                            if (!col.IsValueExist)
                                continue;

                            if (ReadCellValue(i, col.ColIndex.Value))
                            {
                                double? d = Util.StringToDouble(Value.ToString()?.Trim(), true, 2);

                                TreatyPricingRateTableRateBo.SetPropertyValue(col.Property, d);
                                TreatyPricingRateTableOriginalRateBo.SetPropertyValue(col.Property, d);
                            }
                        }
                        PreviousRateBo = TreatyPricingRateTableRateBo.Clone(CurrentRateBo);
                        CurrentRateBo = TreatyPricingRateTableRateBo.Clone(TreatyPricingRateTableRateBo);
                        TreatyPricingRateTableRateBo.ConvertRateToANxB(TreatyPricingRateTableVersionBo?.AgeBasisPickListDetailBo?.Code, PreviousRateBo, CurrentRateBo);
                        TreatyPricingRateTableRateBo.ConvertSmokerAggregatedRate();
                        if (!TreatyPricingRateTableRateBo.IsEmpty())
                        {
                            TreatyPricingRateTableRateBos.Add(TreatyPricingRateTableRateBo);
                        }

                        // Original
                        TreatyPricingRateTableOriginalRateBo.ConvertSmokerAggregatedRate();
                        if (!TreatyPricingRateTableOriginalRateBo.IsEmpty())
                        {
                            TreatyPricingRateTableOriginalRateBos.Add(TreatyPricingRateTableOriginalRateBo);
                        }
                    }
                    age++;
                }
            }

            //if (isValidating && !errors.IsNullOrEmpty())
            //{
            //    StartCol += RateTableCol;
            //    Errors.AddRange(errors);
            //    return false;
            //}

            if (isValidating && !errors.IsNullOrEmpty())
            {
                //StartCol += RateTableCol;
                Errors.AddRange(errors);
                return false;
            }
            return true;
        }

        public bool ProcessRateHeader(int rateTable, ref List<string> errors)
        {
            foreach (var col in RateColumns)
            {
                int rowIndex = RowRateFormat;
                int colIndex = StartCol + col.Type - 1;

                if (ReadRequiredCellValue(rowIndex, colIndex, rateTable, col.Header, ref errors))
                {
                    if (Value != null)
                    {
                        var header = Value.ToString().Trim().ToLower().RemoveNewLines();

                        var column = RateColumns.Where(q => q.Header.ToLower() == header).FirstOrDefault();
                        if (column != null)
                        {
                            column.ColIndex = colIndex;

                            if (ReadRequiredCellValue(RowRate, colIndex, rateTable, col.Header, ref errors))
                            {
                                string isRateInUse = CellValue is string ? CellValue?.Trim() : Value.ToString()?.Trim();

                                if (isRateInUse.ToLower() != "yes" && isRateInUse.ToLower() != "y" && isRateInUse.ToLower() != "no" && isRateInUse.ToLower() != "n")
                                    errors.Add(string.Format("Rate Table {0}: {1}'s \"{2}\" unable to identify if Rate (In used)", rateTable, col.Header, isRateInUse));
                                else if (isRateInUse.ToLower() == "yes" || isRateInUse.ToLower() == "y")
                                    column.IsValueExist = true;
                            }
                        }
                    }
                }
            }

            if (RateColumns.Any(q => !q.ColIndex.HasValue))
                errors.Add(string.Format("Rate Table {0}: Unable to map Rate headers correctly", rateTable));

            if (!errors.IsNullOrEmpty())
                return false;
            return true;
        }

        public static List<Column> GetRateColumns()
        {
            return new List<Column>
            {
                new Column()
                        {
                            Type = TypeMns,
                            Header = "MNS",
                            Property = "MaleNonSmoker",
                            IsValueExist = false,
                },
                new Column()
                        {
                            Type = TypeMs,
                            Header = "MS",
                            Property = "MaleSmoker",
                            IsValueExist = false,
                },
                new Column()
                        {
                            Type = TypeFns,
                            Header = "FNS",
                            Property = "FemaleNonSmoker",
                            IsValueExist = false,
                },
                new Column()
                        {
                            Type = TypeFs,
                            Header = "FS",
                            Property = "FemaleSmoker",
                            IsValueExist = false,
                },
                new Column()
                        {
                            Type = TypeM,
                            Header = "M",
                            Property = "Male",
                            IsValueExist = false,
                },
                new Column()
                        {
                            Type = TypeF,
                            Header = "F",
                            Property = "Female",
                            IsValueExist = false,
                },
                new Column()
                        {
                            Type = TypeUnisex,
                            Header = "Unisex",
                            Property = "Unisex",
                            IsValueExist = false,
                },
                new Column()
                        {
                            Type = TypeUnitRate,
                            Header = "Unit Rate",
                            Property = "UnitRate",
                            IsValueExist = false,
                },
                new Column()
                        {
                            Type = TypeOccClass,
                            Header = "Occ Class",
                            Property = "OccupationClass",
                            IsValueExist = false,
                },
            };
        }

        public void Save()
        {
            if (TreatyPricingRateTableBo == null)
                return;

            var trail = new TrailObject();
            Result result = TreatyPricingRateTableService.Result();
            Result versionResult = TreatyPricingRateTableVersionService.Result();
            Result rateResult = TreatyPricingRateTableRateService.Result();
            Result originalRateResult = TreatyPricingRateTableOriginalRateService.Result();

            if (IsNewRate && TreatyPricingRateTableBo.Id == 0)
            {
                var treatyPricingRateTableBo = TreatyPricingRateTableBo;

                string[] arrTempId;
                int newNumber;
                var latestBo = TreatyPricingRateTableService.FindLatestByTreatyPricingCedantId(TreatyPricingRateTableGroupBo.TreatyPricingCedantId);
                if (latestBo == null)
                {
                    string rateTableGroupId = TreatyPricingRateTableGroupBo.Code; //AIA_GRT_2021_001
                    string tempId = rateTableGroupId.Replace("GRT", "RT");
                    arrTempId = tempId.ToArraySplitTrim('_');
                    newNumber = 1;
                }
                else
                {
                    arrTempId = latestBo.Code.ToArraySplitTrim('_');
                    newNumber = int.Parse(arrTempId[arrTempId.Length - 1]) + 1;
                }

                var newNumberStr = newNumber.ToString().PadLeft(3, '0');
                arrTempId[arrTempId.Length - 1] = newNumberStr;
                treatyPricingRateTableBo.Code = string.Join("_", arrTempId);

                treatyPricingRateTableBo.TreatyPricingRateTableGroupId = TreatyPricingRateTableGroupBo.Id;
                treatyPricingRateTableBo.CreatedById = User.DefaultSuperUserId;
                treatyPricingRateTableBo.UpdatedById = User.DefaultSuperUserId;
                result = TreatyPricingRateTableService.Create(ref treatyPricingRateTableBo, ref trail);
                TreatyPricingRateTableBo.Id = treatyPricingRateTableBo.Id;
            }
            else
            {
                var treatyPricingRateTableBo = TreatyPricingRateTableBo;
                treatyPricingRateTableBo.UpdatedById = User.DefaultSuperUserId;
                result = TreatyPricingRateTableService.Update(ref treatyPricingRateTableBo, ref trail);
            }

            if (result.Valid && IsProcessVersionRate)
            {
                if (TreatyPricingRateTableVersionBo != null && TreatyPricingRateTableVersionBo.Id == 0)
                {
                    var treatyPricingRateTableVersionBo = TreatyPricingRateTableVersionBo;
                    treatyPricingRateTableVersionBo.TreatyPricingRateTableId = TreatyPricingRateTableBo.Id;
                    treatyPricingRateTableVersionBo.PersonInChargeId = TreatyPricingRateTableGroupBo.UploadedById;
                    treatyPricingRateTableVersionBo.CreatedById = User.DefaultSuperUserId;
                    treatyPricingRateTableVersionBo.UpdatedById = User.DefaultSuperUserId;
                    versionResult = TreatyPricingRateTableVersionService.Create(ref treatyPricingRateTableVersionBo, ref trail);
                    TreatyPricingRateTableVersionBo.Id = treatyPricingRateTableVersionBo.Id;
                }
                else
                {
                    versionResult.Valid = false;
                }

                if (versionResult.Valid)
                {
                    foreach (var bo in TreatyPricingRateTableRateBos)
                    {
                        var treatyPricingRateTableRateBo = bo;
                        treatyPricingRateTableRateBo.TreatyPricingRateTableVersionId = TreatyPricingRateTableVersionBo.Id;
                        treatyPricingRateTableRateBo.CreatedById = User.DefaultSuperUserId;
                        treatyPricingRateTableRateBo.UpdatedById = User.DefaultSuperUserId;
                        rateResult = TreatyPricingRateTableRateService.Create(ref treatyPricingRateTableRateBo, ref trail);
                    }

                    foreach (var bo in TreatyPricingRateTableOriginalRateBos)
                    {
                        var treatyPricingRateTableOriginalRateBo = bo;
                        treatyPricingRateTableOriginalRateBo.TreatyPricingRateTableVersionId = TreatyPricingRateTableVersionBo.Id;
                        treatyPricingRateTableOriginalRateBo.CreatedById = User.DefaultSuperUserId;
                        treatyPricingRateTableOriginalRateBo.UpdatedById = User.DefaultSuperUserId;
                        originalRateResult = TreatyPricingRateTableOriginalRateService.Create(ref treatyPricingRateTableOriginalRateBo, ref trail);
                    }
                }
            }

            if (result.Valid && versionResult.Valid && rateResult.Valid && originalRateResult.Valid)
            {
                if (IsNewRate)
                {
                    UserTrailBo userTrailBo = new UserTrailBo(
                       TreatyPricingRateTableBo.Id,
                       "Create Treaty Pricing Rate Table",
                       result,
                       trail,
                       User.DefaultSuperUserId
                   );
                    UserTrailService.Create(ref userTrailBo);
                }
                if (!IsNewRate && !IsProcessVersionRate)
                {
                    UserTrailBo userTrailBo = new UserTrailBo(
                       TreatyPricingRateTableBo.Id,
                       "Update Treaty Pricing Rate Table",
                       result,
                       trail,
                       User.DefaultSuperUserId
                   );
                    UserTrailService.Create(ref userTrailBo);
                }
                else if (!IsNewRate && IsProcessVersionRate)
                {
                    UserTrailBo userTrailBo = new UserTrailBo(
                       TreatyPricingRateTableVersionBo.Id,
                       "Update Treaty Pricing Rate Table's Version",
                       versionResult,
                       trail,
                       User.DefaultSuperUserId
                   );
                    UserTrailService.Create(ref userTrailBo);
                }
            }
        }
    }
}
