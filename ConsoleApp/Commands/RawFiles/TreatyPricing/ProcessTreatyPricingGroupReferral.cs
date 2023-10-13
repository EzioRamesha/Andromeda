using BusinessObject;
using BusinessObject.Identity;
using BusinessObject.TreatyPricing;
using ConsoleApp.Commands.ProcessDatas.Exports;
using DataAccess.Entities.Identity;
using Newtonsoft.Json;
using Services;
using Services.Identity;
using Services.TreatyPricing;
using Shared;
using Shared.ProcessFile;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Globalization;
using System.IO;
using System.Linq;
using Row = Shared.ProcessFile.Row;

namespace ConsoleApp.Commands.RawFiles.TreatyPricing
{
    public class ProcessTreatyPricingGroupReferral : Command
    {
        public int? TreatyPricingGroupReferralFileId { get; set; }

        public TreatyPricingGroupReferralFileBo TreatyPricingGroupReferralFileBo { get; set; }

        public TreatyPricingGroupReferralVersionBo TreatyPricingGroupReferralVersionBo { get; set; }
        public IQueryable<TreatyPricingGroupReferralVersionBo> TreatyPricingGroupReferralVersionBos { get; set; }

        public List<TreatyPricingGroupReferralBo> TreatyPricingGroupReferralBos { get; set; }

        public List<TreatyPricingGroupReferralBo> TreatyPricingGroupReferralVersionBenefitBos { get; set; }

        public List<TreatyPricingGroupReferralHipsTableBo> TreatyPricingGroupReferralHipsTableBos { get; set; }

        public List<TreatyPricingGroupReferralGtlTableBo> TreatyPricingGroupReferralGtlTableBos { get; set; }

        public List<TreatyPricingGroupReferralGhsTableBo> TreatyPricingGroupReferralGhsTableBos { get; set; }

        public ModuleBo ModuleBo { get; set; }

        public Excel Excel { get; set; }

        public List<Row> Rows { get; set; }

        public List<Column> Columns { get; set; }

        public List<Column> EditableColumns { get; set; }

        public IProcessFile DataFile { get; set; }

        public List<string> Errors { get; set; }

        public dynamic CellValue { get; set; }

        public object Value { get; set; }

        public int StartCol { get; set; }

        public int Take { get; set; } = 100;

        public int MaxEmptyRows { get; set; }

        public int UploadedType { get; set; }

        public int Records { get; set; }

        public int UpdatedRecords { get; set; }

        public int? TableTypePickListDetailId { get; set; }
        public PickListDetailBo TableTypePickListDetailBo { get; set; }

        public bool isHeaderMatch = true;

        public bool Success { get; set; }

        public Excel HipsExcel { get; set; }

        // Error count
        public int dtErrorCount { get; set; }
        public int doubleErrorCount { get; set; }
        public int benefitErrorCount { get; set; }
        public int catErrorCount { get; set; }
        public int subCatErrorCount { get; set; }

        public ProcessTreatyPricingGroupReferral()
        {
            Title = "ProcessTreatyPricingGroupReferral";
            Description = "To process Treaty Pricing Group Referral Uploaded Comparison Table Data";
            Options = new string[] {
                "--treatyPricingGroupReferralFileId= : Process by Id",
            };
        }

        public override void Initial()
        {
            base.Initial();
            TreatyPricingGroupReferralFileId = OptionIntegerNullable("treatyPricingGroupReferralFileId");
            //TreatyPricingRateTableIds = new List<string> { };
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
                if (TreatyPricingGroupReferralFileId.HasValue)
                {
                    TreatyPricingGroupReferralFileBo = TreatyPricingGroupReferralFileService.Find(TreatyPricingGroupReferralFileId.Value);
                    if (TreatyPricingGroupReferralFileBo != null && TreatyPricingGroupReferralFileBo.Status != TreatyPricingGroupReferralFileBo.StatusPending)
                    {
                        Log = false;
                        PrintMessage("No Treaty Pricing Group Referral File pending to process");
                        return;
                    }
                }
                else if (TreatyPricingGroupReferralFileService.CountByStatus(TreatyPricingGroupReferralFileBo.StatusPending) == 0)
                {
                    Log = false;
                    PrintMessage("No Treaty Pricing Group Referral File pending to process");
                    return;
                }
                PrintStarting();

                ModuleBo = ModuleService.FindByController(ModuleBo.ModuleController.TreatyPricingGroupReferralFile.ToString());

                while (LoadTreatyPricingGroupReferralFileBo() != null)
                {
                    if (GetProcessCount("Group") > 0)
                        PrintProcessCount();
                    SetProcessCount("Group");

                    PrintOutputTitle(string.Format("Process Treaty Pricing Group Referral File Id: {0}", TreatyPricingGroupReferralFileBo.Id));
                    if (UploadedType == TreatyPricingGroupReferralFileBo.UploadedTypeTable)
                        PrintOutputTitle(string.Format("Process Treaty Pricing Group Referral Table Type: {0}", TableTypePickListDetailBo?.Code));

                    UpdateTreatyPricingGroupReferralFileStatus(TreatyPricingGroupReferralFileBo.StatusProcessing, MessageBag.ProcessTreatyPricingRateTableGroupProcessing);

                    string[] extensions = { ".xls", ".xlsx" };
                    if (!extensions.Contains(Path.GetExtension(TreatyPricingGroupReferralFileBo.FileName).ToLower()))
                    {
                        Errors.Add("The file uploaded not matched with the file extension that allowed");
                        TreatyPricingGroupReferralFileBo.Errors = JsonConvert.SerializeObject(Errors);
                        Success = false;
                    }

                    if (UploadedType != TreatyPricingGroupReferralFileBo.UploadedTypeFile)
                    {
                        if (TableTypePickListDetailBo == null || string.IsNullOrEmpty(TableTypePickListDetailBo?.Code))
                        {
                            Errors.Add(string.Format("Table Type for {0} doesn't exists", TableTypePickListDetailBo?.Description));
                            TreatyPricingGroupReferralFileBo.Errors = JsonConvert.SerializeObject(Errors);
                            Success = false;
                        }
                    }

                    if (Errors.IsNullOrEmpty())
                        ProcessFile();

                    if (Success)
                    {
                        Save();
                        UpdateTreatyPricingGroupReferralFileStatus(TreatyPricingGroupReferralFileBo.StatusSuccess, MessageBag.ProcessTreatyPricingRateTableGroupSuccess);
                    }
                    else
                    {
                        UpdateTreatyPricingGroupReferralFileStatus(TreatyPricingGroupReferralFileBo.StatusFailed, MessageBag.ProcessTreatyPricingRateTableGroupFailed);
                    }
                }
                PrintProcessCount();
                PrintEnding();
            }
            catch (Exception e)
            {
                PrintError(e.Message);
            }
        }

        public TreatyPricingGroupReferralFileBo LoadTreatyPricingGroupReferralFileBo()
        {
            TreatyPricingGroupReferralFileBo = null;
            Errors = new List<string>();
            if (TreatyPricingGroupReferralFileId.HasValue)
            {
                TreatyPricingGroupReferralFileBo = TreatyPricingGroupReferralFileService.Find(TreatyPricingGroupReferralFileId.Value);
                if (TreatyPricingGroupReferralFileBo != null && TreatyPricingGroupReferralFileBo.Status != TreatyPricingGroupReferralFileBo.StatusPending)
                    TreatyPricingGroupReferralFileBo = null;
            }
            else
                TreatyPricingGroupReferralFileBo = TreatyPricingGroupReferralFileService.FindByStatus(TreatyPricingGroupReferralFileBo.StatusPending);

            if (TreatyPricingGroupReferralFileBo != null)
            {
                UploadedType = TreatyPricingGroupReferralFileBo.UploadedType;
                if (TreatyPricingGroupReferralFileBo.TableTypePickListDetailId.HasValue)
                {
                    TableTypePickListDetailId = TreatyPricingGroupReferralFileBo.TableTypePickListDetailId;
                    TableTypePickListDetailBo = TreatyPricingGroupReferralFileBo.TableTypePickListDetailBo;
                }

                if (TreatyPricingGroupReferralFileBo.TreatyPricingGroupReferralId.HasValue)
                {
                    TreatyPricingGroupReferralVersionBo = TreatyPricingGroupReferralVersionService.GetLatestVersionByTreatyPricingGroupReferralId(TreatyPricingGroupReferralFileBo.TreatyPricingGroupReferralId.Value);
                }
            }

            return TreatyPricingGroupReferralFileBo;
        }

        public void UpdateTreatyPricingGroupReferralFileStatus(int status, string description)
        {
            var trail = new TrailObject();

            var treatyPricingGroupReferralFileBo = TreatyPricingGroupReferralFileBo;
            treatyPricingGroupReferralFileBo.Status = status;

            var result = TreatyPricingGroupReferralFileService.Update(ref treatyPricingGroupReferralFileBo, ref trail);
            var userTrailBo = new UserTrailBo(
                treatyPricingGroupReferralFileBo.Id,
                description,
                result,
                trail,
                User.DefaultSuperUserId
            );
            UserTrailService.Create(ref userTrailBo);
        }

        public string GetFilePath()
        {
            if (TreatyPricingGroupReferralFileBo != null)
                return TreatyPricingGroupReferralFileBo.GetLocalPath();
            return null;
        }

        public List<Column> GetColumnsByUploadedType(int uploadedType)
        {
            Columns = new List<Column>() { };

            if (uploadedType == TreatyPricingGroupReferralFileBo.UploadedTypeFile)
                Columns = TreatyPricingGroupReferralVersionBo.GetTrackingColumns();

            if (uploadedType == TreatyPricingGroupReferralFileBo.UploadedTypeTable)
            {
                if (TableTypePickListDetailBo != null)
                {
                    switch (TableTypePickListDetailBo.Code)
                    {
                        case PickListDetailBo.TableTypeHips:
                            Columns = TreatyPricingGroupReferralHipsTableBo.GetColumns();
                            break;
                        case PickListDetailBo.TableTypeGtlClaim:
                            Columns = TreatyPricingGroupReferralGtlTableBo.GetColumns(TreatyPricingGroupReferralGtlTableBo.TypeGtlClaim);
                            break;
                        case PickListDetailBo.TableTypeGtlRate:
                            Columns = TreatyPricingGroupReferralGtlTableBo.GetColumns(TreatyPricingGroupReferralGtlTableBo.TypeGtlUnitRates);
                            break;
                        case PickListDetailBo.TableTypeGtlAge:
                            Columns = TreatyPricingGroupReferralGtlTableBo.GetColumns(TreatyPricingGroupReferralGtlTableBo.TypeGtlAgeBanded);
                            break;
                        case PickListDetailBo.TableTypeGtlSa:
                            Columns = TreatyPricingGroupReferralGtlTableBo.GetColumns(TreatyPricingGroupReferralGtlTableBo.TypeGtlBasisOfSa);
                            break;
                        case PickListDetailBo.TableTypeGhsClaim:
                            Columns = TreatyPricingGroupReferralGhsTableBo.GetColumns();
                            break;
                    }
                }
            }
            return Columns;
        }

        public void ProcessFile()
        {
            GetColumnsByUploadedType(UploadedType);
            foreach (var column in Columns)
                column.Header = column.Header.Trim().RemoveNewLines();

            PrintOutputTitle("Process File");

            ClearDataByUploadType(UploadedType);
            try
            {
                var filePath = GetFilePath();
                if (!File.Exists(filePath))
                    throw new Exception(string.Format(MessageBag.FileNotExists, filePath));

                //DataFile = new Excel(filePath, worksheet: 1, rowRead: Take) { };
                DataFile = new Excel(filePath);

                Records = 0;
                dtErrorCount = 0;
                doubleErrorCount = 0;
                benefitErrorCount = 0;
                catErrorCount = 0;
                subCatErrorCount = 0;
                while (DataFile.GetNextRow() != null)
                {
                    if (DataFile.GetRowIndex() == 1)
                    {
                        SetHeader();
                        if (IsSuccess)
                        {
                            continue;
                        }
                        else
                        {
                            Success = false;
                            break;
                        }
                    }

                    Records++;
                    SetProcessCount(number: Records);

                    ProcessData();
                }

                if (DataFile != null)
                    DataFile.Close();

                if (!Errors.IsNullOrEmpty())
                {
                    TreatyPricingGroupReferralFileBo.Errors = JsonConvert.SerializeObject(Errors);
                    Success = false;
                }
                else
                {
                    TreatyPricingGroupReferralFileBo.Errors = null;
                    Success = true;
                }
            }
            catch (Exception e)
            {
                if (DataFile != null)
                    DataFile.Close();

                var errors = new List<string> { };
                var message = e.Message;
                if (e is DbEntityValidationException dbEx)
                    message = Util.CatchDbEntityValidationException(dbEx).ToString();

                errors.Add(message);

                TreatyPricingGroupReferralFileBo.Errors = JsonConvert.SerializeObject(errors);
                Success = false;
            }
        }

        public void SetHeader()
        {
            List<string> headerCols = new List<string> { };
            while (DataFile.GetNextCol() != null)
            {
                var value = DataFile.GetValue();
                if (value == null)
                    continue;

                headerCols.Add(value.Trim());
            }

            var notMatchCols = Columns.Where(p => !headerCols.Any(p2 => p2 == p.Header));
            if (notMatchCols.Count() > 0)
            {
                foreach (var column in notMatchCols)
                    Errors.Add(string.Format("Column in header row does not exist {0}", column.Header));
            }

            if (!isHeaderMatch)
                Errors.Add("The formatting for file uploaded is invalid.");

            EditableColumns = Columns.Where(q => q.ColIndex.HasValue).ToList();
        }

        public void ProcessData()
        {
            switch (UploadedType)
            {
                case TreatyPricingGroupReferralFileBo.UploadedTypeFile:
                    ProcessGroupCaseTrackingData();
                    break;
                case TreatyPricingGroupReferralFileBo.UploadedTypeTable:
                    switch (TreatyPricingGroupReferralFileBo.TableTypePickListDetailBo.Code)
                    {
                        case PickListDetailBo.TableTypeHips:
                            ProcessHipsData();
                            break;
                        case PickListDetailBo.TableTypeGtlClaim:
                            ProcessGtlData(TreatyPricingGroupReferralGtlTableBo.TypeGtlClaim);
                            break;
                        case PickListDetailBo.TableTypeGtlRate:
                            ProcessGtlData(TreatyPricingGroupReferralGtlTableBo.TypeGtlUnitRates);
                            break;
                        case PickListDetailBo.TableTypeGtlAge:
                            ProcessGtlData(TreatyPricingGroupReferralGtlTableBo.TypeGtlAgeBanded);
                            break;
                        case PickListDetailBo.TableTypeGtlSa:
                            ProcessGtlData(TreatyPricingGroupReferralGtlTableBo.TypeGtlBasisOfSa);
                            break;
                        case PickListDetailBo.TableTypeGhsClaim:
                            ProcessGhsData();
                            break;

                    }
                    break;
            }
        }

        public void ProcessGroupCaseTrackingData()
        {
            var bo = new TreatyPricingGroupReferralBo();
            while (DataFile.GetNextCol() != null)
            {
                int colIndex = DataFile.GetColIndex().Value;
                Column column = EditableColumns.Where(q => q.ColIndex == colIndex).FirstOrDefault();

                object rawValue = DataFile.GetValue();
                string value = rawValue != null ? rawValue.ToString() : null;

                string property = "";
                switch (column.Property)
                {
                    case "GroupReferralStatus":
                        property = "Status";
                        break;
                    case "GroupReferralDescription":
                        property = "Description";
                        break;
                    case "GroupReferralCode":
                        property = "Code";
                        break;
                    default:
                        property = column.Property;
                        break;
                }

                if (value == null || value is string @string && string.IsNullOrEmpty(@string))
                {
                    if (colIndex == TreatyPricingGroupReferralVersionBo.ColumnStatus)
                        Errors.Add(string.Format(MessageBag.RequiredWithRow, "Status", DataFile.GetRowIndex()));

                    bo.SetPropertyValue(property, null);
                }
                else
                {
                    switch (colIndex)
                    {
                        case TreatyPricingGroupReferralVersionBo.ColumnStatus:
                            int key = TreatyPricingGroupReferralBo.GetStatusKey(value.ToString());
                            if (key == 0)
                                Errors.Add(string.Format("Status only can be Won/Loss at row #{0}", DataFile.GetRowIndex()));
                            else
                                bo.SetPropertyValue(property, key);
                            break;
                        case TreatyPricingGroupReferralVersionBo.ColumnCedantCode:
                            var cedantBo = CedantService.FindByCode(value.ToString());
                            if (cedantBo != null)
                                bo.SetPropertyValue(property, cedantBo.Id);
                            break;
                        case TreatyPricingGroupReferralVersionBo.ColumnInsuredGroupName:
                            var insuredGroupNameBo = InsuredGroupNameService.FindByCode(value.ToString());
                            if (insuredGroupNameBo != null)
                                bo.SetPropertyValue(property, insuredGroupNameBo.Id);
                            break;
                        case TreatyPricingGroupReferralVersionBo.ColumnReferredType:
                            var referredTypeBo = PickListDetailService.FindByPickListIdCode(PickListBo.ReferredType, value.ToString());
                            if (referredTypeBo != null)
                                bo.SetPropertyValue(property, referredTypeBo.Id);
                            break;
                        case TreatyPricingGroupReferralVersionBo.ColumnWonVersion:
                            bo.SetPropertyValue(property, value.ToString());
                            break;
                        case TreatyPricingGroupReferralVersionBo.ColumnCoverageStartDate:
                            if (ParseDateTime(value, out DateTime? datetime, out string error1))
                            {
                                bo.SetPropertyValue(property, datetime.Value);
                            }
                            else
                            {
                                dtErrorCount++;
                                SetProcessCount(string.Format("{0}{1}", column.Header, "Error"), dtErrorCount);
                                Errors.Add(string.Format("The {0} format can not be read: {1} at row #{2}", column.Header, value, DataFile.GetRowIndex()));
                            }
                            break;
                        default:
                            if (Util.IsValidDouble(value, out double? output, out string error, true, 2))
                                bo.SetPropertyValue(property, output.ToString());
                            else
                                bo.SetPropertyValue(property, value);
                            break;
                    }
                }
            }

            TreatyPricingGroupReferralBos.Add(bo);
        }

        public void ProcessHipsData()
        {
            var bo = new TreatyPricingGroupReferralHipsTableBo();
            while (DataFile.GetNextCol() != null)
            {
                int colIndex = DataFile.GetColIndex().Value;
                Column column = EditableColumns.Where(q => q.ColIndex == colIndex).FirstOrDefault();

                object rawValue = DataFile.GetValue();
                string value = rawValue != null ? rawValue.ToString() : null;
                string property = column != null ? column.Property : null;

                if (property != null)
                {
                    if (value == null || value is string @string && string.IsNullOrEmpty(@string))
                    {
                        bo.SetPropertyValue(property, null);
                    }
                    else
                    {
                        switch (colIndex)
                        {
                            case TreatyPricingGroupReferralHipsTableBo.ColumnCoverageStartDate:
                                if (ParseDateTime(value, out DateTime? datetime, out string error1))
                                {
                                    bo.SetPropertyValue(property, datetime.Value);
                                }
                                else
                                {
                                    dtErrorCount++;
                                    SetProcessCount(string.Format("{0}{1}", column.Header, "Error"), dtErrorCount);
                                    Errors.Add(string.Format("The {0} format can not be read: {1} at row #{2}", column.Header, value, DataFile.GetRowIndex()));
                                }
                                break;
                            case TreatyPricingGroupReferralHipsTableBo.ColumnCategory:
                                var HipsCategoryBo = HipsCategoryService.FindByCode(value.ToString());
                                if (HipsCategoryBo == null)
                                {
                                    catErrorCount++;
                                    SetProcessCount(string.Format("{0}{1}", column.Header, "Error"), catErrorCount);
                                    Errors.Add(string.Format("The {0} doesn't exists: {1} at row #{2}", column.Header, value, DataFile.GetRowIndex()));
                                }
                                else
                                {
                                    bo.SetPropertyValue(property, HipsCategoryBo.Id);
                                }
                                break;
                            case TreatyPricingGroupReferralHipsTableBo.ColumnSubCategory:
                                var HipsSubCategoryBo = HipsCategoryDetailService.FindBySubcategory(value.ToString());
                                if (HipsSubCategoryBo == null)
                                {
                                    subCatErrorCount++;
                                    SetProcessCount(string.Format("{0}{1}", column.Header, "Error"), subCatErrorCount);
                                    Errors.Add(string.Format("The {0} doesn't exists: {1} at row #{2}", column.Header, value, DataFile.GetRowIndex()));
                                }
                                else
                                {
                                    bo.SetPropertyValue(property, HipsSubCategoryBo.Id);
                                }
                                break;
                            default:
                                if (Util.IsValidDouble(value, out double? output, out string error, true, 2))
                                    bo.SetPropertyValue(property, output.ToString());
                                else
                                    bo.SetPropertyValue(property, value);
                                //Errors.Add(string.Format("The {0} format can not be read: {1} at row #{2}", header, value, col.RowIndex));
                                break;
                        }
                    }
                }
            }

            TreatyPricingGroupReferralHipsTableBos.Add(bo);
        }

        public void ProcessGtlData(int type)
        {
            var bo = new TreatyPricingGroupReferralGtlTableBo();
            while (DataFile.GetNextCol() != null)
            {
                int colIndex = DataFile.GetColIndex().Value;
                Column column = EditableColumns.Where(q => q.ColIndex == colIndex).FirstOrDefault();

                object rawValue = DataFile.GetValue();
                string value = rawValue != null ? rawValue.ToString() : null;
                string property = column.Property;

                if (value == null || value is string @string && string.IsNullOrEmpty(@string))
                {
                    bo.SetPropertyValue(property, null);
                }
                else
                {
                    if (type == TreatyPricingGroupReferralGtlTableBo.TypeGtlClaim)
                    {
                        switch (colIndex)
                        {
                            case TreatyPricingGroupReferralGtlTableBo.ColumnCoverageStartDate:
                            case TreatyPricingGroupReferralGtlTableBo.ColumnEventDate:
                                if (ParseDateTime(value, out DateTime? datetime, out string error1))
                                {
                                    bo.SetPropertyValue(property, datetime.Value);
                                }
                                else
                                {
                                    dtErrorCount++;
                                    SetProcessCount(string.Format("{0}{1}", column.Header, "Error"), dtErrorCount);
                                    Errors.Add(string.Format("The {0} format can not be read: {1} at row #{2}", column.Header, value, DataFile.GetRowIndex()));
                                }
                                break;
                            case TreatyPricingGroupReferralGtlTableBo.ColumnClaimGross:
                            case TreatyPricingGroupReferralGtlTableBo.ColumnClaimRi:
                                if (Util.IsValidDouble(value, out double? output, out string error, true, 2))
                                {
                                    bo.SetPropertyValue(property, output.ToString());
                                }
                                else
                                {
                                    doubleErrorCount++;
                                    SetProcessCount(string.Format("{0}{1}", column.Header, "Error"), doubleErrorCount);
                                    Errors.Add(string.Format("The {0} format can not be read: {1} at row #{2}", column.Header, value, DataFile.GetRowIndex()));
                                }
                                break;
                            default:
                                bo.SetPropertyValue(property, value);
                                break;
                        }
                    }

                    if (type == TreatyPricingGroupReferralGtlTableBo.TypeGtlUnitRates)
                    {
                        switch (colIndex)
                        {
                            case TreatyPricingGroupReferralGtlTableBo.ColumnCoverageStartDate:
                                //!DateTime.TryParseExact(value.ToString(), "YYYY-MM-DD", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime datetime)
                                //DateTime.TryParse(paramObject.ReportCompletedDate.ToString(), out dt)
                                if (ParseDateTime(value, out DateTime? datetime, out string error1))
                                {
                                    bo.SetPropertyValue(property, datetime.Value);
                                }
                                else
                                {
                                    dtErrorCount++;
                                    SetProcessCount(string.Format("{0}{1}", column.Header, "Error"), dtErrorCount);
                                    Errors.Add(string.Format("The {0} format can not be read: {1} at row #{2}", column.Header, value, DataFile.GetRowIndex()));
                                }
                                break;
                            case TreatyPricingGroupReferralGtlTableBo.ColumnRiskRate:
                            case TreatyPricingGroupReferralGtlTableBo.ColumnGrossRate:
                                if (Util.IsValidDouble(value, out double? output, out string error, true, 2))
                                {
                                    bo.SetPropertyValue(property, output.ToString());
                                }
                                else
                                {
                                    doubleErrorCount++;
                                    SetProcessCount(string.Format("{0}{1}", column.Header, "Error"), doubleErrorCount);
                                    Errors.Add(string.Format("The {0} format can not be read: {1} at row #{2}", column.Header, value, DataFile.GetRowIndex()));
                                }
                                break;
                            case TreatyPricingGroupReferralGtlTableBo.ColumnBenefit:
                                var BenefitBo = TreatyPricingGroupReferralVersionBenefitService.GetByTreatyPricingGroupReferralVersionIdBenefitCode(TreatyPricingGroupReferralVersionBo.Id, value.ToString());
                                if (BenefitBo == null)
                                {
                                    benefitErrorCount++;
                                    SetProcessCount(string.Format("{0}{1}", column.Header, "Error"), benefitErrorCount);
                                    Errors.Add(string.Format("The {0} doesn't exists: {1} at row #{2}", column.Header, value, DataFile.GetRowIndex()));
                                }
                                else
                                {
                                    bo.SetPropertyValue(property, value);
                                }
                                break;
                            default:
                                bo.SetPropertyValue(property, value);
                                break;
                        }
                    }

                    if (type == TreatyPricingGroupReferralGtlTableBo.TypeGtlAgeBanded)
                    {
                        switch (colIndex)
                        {
                            case TreatyPricingGroupReferralGtlTableBo.ColumnCoverageStartDate:
                                if (ParseDateTime(value, out DateTime? datetime, out string error1))
                                {
                                    bo.SetPropertyValue(property, datetime.Value);
                                }
                                else
                                {
                                    dtErrorCount++;
                                    SetProcessCount(string.Format("{0}{1}", column.Header, "Error"), dtErrorCount);
                                    Errors.Add(string.Format("The {0} format can not be read: {1} at row #{2}", column.Header, value, DataFile.GetRowIndex()));
                                }
                                break;
                            case TreatyPricingGroupReferralGtlTableBo.ColumnRateGross:
                            case TreatyPricingGroupReferralGtlTableBo.ColumnRateRisk:
                                if (Util.IsValidDouble(value, out double? output, out string error, true, 2))
                                {
                                    bo.SetPropertyValue(property, output.ToString());
                                }
                                else
                                {
                                    doubleErrorCount++;
                                    SetProcessCount(string.Format("{0}{1}", column.Header, "Error"), doubleErrorCount);
                                    Errors.Add(string.Format("The {0} format can not be read: {1} at row #{2}", column.Header, value, DataFile.GetRowIndex()));
                                }
                                break;
                            case TreatyPricingGroupReferralGtlTableBo.ColumnBenefit:
                                var BenefitBo = TreatyPricingGroupReferralVersionBenefitService.GetByTreatyPricingGroupReferralVersionIdBenefitCode(TreatyPricingGroupReferralVersionBo.Id, value.ToString());
                                if (BenefitBo == null)
                                {
                                    benefitErrorCount++;
                                    SetProcessCount(string.Format("{0}{1}", column.Header, "Error"), benefitErrorCount);
                                    Errors.Add(string.Format("The {0} doesn't exists: {1} at row #{2}", column.Header, value, DataFile.GetRowIndex()));
                                }
                                else
                                {
                                    bo.SetPropertyValue(property, value);
                                }
                                break;
                            default:
                                bo.SetPropertyValue(property, value);
                                break;
                        }
                    }

                    if (type == TreatyPricingGroupReferralGtlTableBo.TypeGtlBasisOfSa)
                    {
                        switch (colIndex)
                        {
                            case TreatyPricingGroupReferralGtlTableBo.ColumnCoverageStartDate:
                                if (ParseDateTime(value, out DateTime? datetime, out string error1))
                                {
                                    bo.SetPropertyValue(column.Property, datetime.Value);
                                }
                                else
                                {
                                    dtErrorCount++;
                                    SetProcessCount(string.Format("{0}{1}", column.Header, "Error"), dtErrorCount);
                                    Errors.Add(string.Format("The {0} format can not be read: {1} at row #{2}", column.Header, value, DataFile.GetRowIndex()));
                                }
                                break;
                            case TreatyPricingGroupReferralGtlTableBo.ColumnBenefit:
                                var BenefitBo = TreatyPricingGroupReferralVersionBenefitService.GetByTreatyPricingGroupReferralVersionIdBenefitCode(TreatyPricingGroupReferralVersionBo.Id, value.ToString());
                                if (BenefitBo == null)
                                {
                                    doubleErrorCount++;
                                    SetProcessCount(string.Format("{0}{1}", column.Header, "Error"), doubleErrorCount);
                                    Errors.Add(string.Format("The {0} doesn't exists: {1} at row #{2}", column.Header, value, DataFile.GetRowIndex()));
                                }
                                else
                                {
                                    bo.SetPropertyValue(property, value);
                                }
                                break;
                            default:
                                if (Util.IsValidDouble(value, out double? output, out string error, true, 2))
                                    bo.SetPropertyValue(column.Property, output.ToString());
                                else
                                    bo.SetPropertyValue(column.Property, value);
                                break;
                        }
                    }

                }
            }

            bo.Type = type;
            TreatyPricingGroupReferralGtlTableBos.Add(bo);
        }

        public void ProcessGhsData()
        {
            var bo = new TreatyPricingGroupReferralGhsTableBo();
            while (DataFile.GetNextCol() != null)
            {
                int colIndex = DataFile.GetColIndex().Value;
                Column column = EditableColumns.Where(q => q.ColIndex == colIndex).FirstOrDefault();

                object rawValue = DataFile.GetValue();
                string value = rawValue != null ? rawValue.ToString() : null;
                string property = column.Property;

                if (value == null || value is string @string && string.IsNullOrEmpty(@string))
                {
                    bo.SetPropertyValue(property, null);
                }
                else
                {
                    switch (colIndex)
                    {
                        case TreatyPricingGroupReferralGhsTableBo.ColumnCoverageStartDate:
                        case TreatyPricingGroupReferralGhsTableBo.ColumnEventDate:
                            if (ParseDateTime(value, out DateTime? datetime, out string error1))
                            {
                                bo.SetPropertyValue(property, datetime.Value);
                            }
                            else
                            {
                                dtErrorCount++;
                                SetProcessCount(string.Format("{0}{1}", column.Header, "Error"), dtErrorCount);
                                Errors.Add(string.Format("The {0} format can not be read: {1} at row #{2}", column.Header, value, DataFile.GetRowIndex()));
                            }
                            break;
                        case TreatyPricingGroupReferralGhsTableBo.ColumnRbCovered:
                        case TreatyPricingGroupReferralGhsTableBo.ColumnAolCovered:
                        case TreatyPricingGroupReferralGhsTableBo.ColumnGrossClaimIncurred:
                        case TreatyPricingGroupReferralGhsTableBo.ColumnGrossClaimPaid:
                        case TreatyPricingGroupReferralGhsTableBo.ColumnGrossClaimPaidIbnr:
                        case TreatyPricingGroupReferralGhsTableBo.ColumnRiClaimPaid:
                            if (Util.IsValidDouble(value, out double? output, out string error, true, 2))
                            {
                                bo.SetPropertyValue(property, output.ToString());
                            }
                            else
                            {
                                doubleErrorCount++;
                                SetProcessCount(string.Format("{0}{1}", column.Header, "Error"), doubleErrorCount);
                                Errors.Add(string.Format("The {0} format can not be read: {1} at row #{2}", column.Header, value, DataFile.GetRowIndex()));
                            }
                            break;
                        default:
                            if (Util.IsValidDouble(value, out output, out error, true, 2)) // data type string but the value is double/int
                                bo.SetPropertyValue(property, output.ToString());
                            else
                                bo.SetPropertyValue(property, value);
                            break;
                    }
                }
            }

            TreatyPricingGroupReferralGhsTableBos.Add(bo);
        }

        public void Save()
        {
            if (TreatyPricingGroupReferralFileBo == null)
                return;

            UpdatedRecords = 0;
            if (!TreatyPricingGroupReferralBos.IsNullOrEmpty())
            {
                foreach (var treatyPricingGroupReferralBo in TreatyPricingGroupReferralBos)
                {
                    var bo = TreatyPricingGroupReferralService.FindByCode(treatyPricingGroupReferralBo.Code);
                    var verBo = TreatyPricingGroupReferralVersionService.GetByTreatyPricingGroupReferralId(bo.Id).OrderByDescending(a => a.Version).FirstOrDefault();
                    var checklistBos = TreatyPricingGroupReferralChecklistService.GetByTreatyPricingGroupReferralVersionId(verBo.Id);
                    var checklistBosStr = JsonConvert.SerializeObject(checklistBos);

                    bo.Status = treatyPricingGroupReferralBo.Status;
                    bo.WonVersion = treatyPricingGroupReferralBo.WonVersion;
                    bo.PolicyNumber = treatyPricingGroupReferralBo.PolicyNumber;
                    if (treatyPricingGroupReferralBo.CoverageStartDate.HasValue)
                    {
                        bo.CoverageStartDate = treatyPricingGroupReferralBo.CoverageStartDate.Value;
                        bo.CoverageEndDate = treatyPricingGroupReferralBo.CoverageStartDate.Value.AddYears(1);
                    }
                    bo.UpdatedById = TreatyPricingGroupReferralFileBo.UpdatedById;
                    bo.WorkflowStatus = TreatyPricingGroupReferralService.GenerateWorkflowStatus(checklistBosStr, verBo, bo);
                    TreatyPricingGroupReferralService.Update(ref bo);

                    var benefitBos = new List<TreatyPricingGroupReferralVersionBenefitBo>();
                    if (!string.IsNullOrEmpty(verBo.TreatyPricingGroupReferralVersionBenefit))
                        benefitBos = JsonConvert.DeserializeObject<List<TreatyPricingGroupReferralVersionBenefitBo>>(verBo.TreatyPricingGroupReferralVersionBenefit);

                    foreach (var detailBo in benefitBos)
                    {
                        var benefitBo = detailBo;
                        benefitBo.BenefitBo = BenefitService.Find(benefitBo.BenefitId);
                        if (benefitBo.BenefitBo != null)
                        {
                            if (benefitBo.BenefitBo.Code == "MSE")
                            {
                                if (!string.IsNullOrEmpty(treatyPricingGroupReferralBo.CommissionMarginMSE)) benefitBo.CommissionMargin = treatyPricingGroupReferralBo.CommissionMarginMSE;
                                if (!string.IsNullOrEmpty(treatyPricingGroupReferralBo.ExpenseMarginMSE)) benefitBo.ExpenseMargin = treatyPricingGroupReferralBo.ExpenseMarginMSE;
                                if (!string.IsNullOrEmpty(treatyPricingGroupReferralBo.ProfitMarginMSE)) benefitBo.ProfitMargin = treatyPricingGroupReferralBo.ProfitMarginMSE;
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(treatyPricingGroupReferralBo.CommissionMarginDEA)) benefitBo.CommissionMargin = treatyPricingGroupReferralBo.CommissionMarginDEA;
                                if (!string.IsNullOrEmpty(treatyPricingGroupReferralBo.ExpenseMarginDEA)) benefitBo.ExpenseMargin = treatyPricingGroupReferralBo.ExpenseMarginDEA;
                                if (!string.IsNullOrEmpty(treatyPricingGroupReferralBo.ProfitMarginDEA)) benefitBo.ProfitMargin = treatyPricingGroupReferralBo.ProfitMarginDEA;
                            }
                            TreatyPricingGroupReferralVersionBenefitService.Update(ref benefitBo);
                        }

                    }

                    UpdatedRecords++;
                }
            }
            if (!TreatyPricingGroupReferralHipsTableBos.IsNullOrEmpty())
            {
                foreach (var treatyPricingGroupReferralHipsTableBos in TreatyPricingGroupReferralHipsTableBos)
                {
                    treatyPricingGroupReferralHipsTableBos.TreatyPricingGroupReferralId = TreatyPricingGroupReferralFileBo.TreatyPricingGroupReferralId.GetValueOrDefault();
                    treatyPricingGroupReferralHipsTableBos.TreatyPricingGroupReferralFileId = TreatyPricingGroupReferralFileBo.Id;
                    treatyPricingGroupReferralHipsTableBos.CreatedById = TreatyPricingGroupReferralFileBo.CreatedById;
                    treatyPricingGroupReferralHipsTableBos.UpdatedById = TreatyPricingGroupReferralFileBo.UpdatedById;
                    var bo = treatyPricingGroupReferralHipsTableBos;
                    TreatyPricingGroupReferralHipsTableService.Create(ref bo);

                    UpdatedRecords++;
                }
            }
            if (!TreatyPricingGroupReferralGtlTableBos.IsNullOrEmpty())
            {
                foreach (var treatyPricingGroupReferralGtlTableBos in TreatyPricingGroupReferralGtlTableBos)
                {
                    treatyPricingGroupReferralGtlTableBos.TreatyPricingGroupReferralId = TreatyPricingGroupReferralFileBo.TreatyPricingGroupReferralId.GetValueOrDefault();
                    treatyPricingGroupReferralGtlTableBos.TreatyPricingGroupReferralFileId = TreatyPricingGroupReferralFileBo.Id;
                    treatyPricingGroupReferralGtlTableBos.CreatedById = TreatyPricingGroupReferralFileBo.CreatedById;
                    treatyPricingGroupReferralGtlTableBos.UpdatedById = TreatyPricingGroupReferralFileBo.UpdatedById;
                    var bo = treatyPricingGroupReferralGtlTableBos;
                    TreatyPricingGroupReferralGtlTableService.Create(ref bo);

                    UpdatedRecords++;
                }
            }
            if (!TreatyPricingGroupReferralGhsTableBos.IsNullOrEmpty())
            {
                foreach (var treatyPricingGroupReferralGhsTableBos in TreatyPricingGroupReferralGhsTableBos)
                {
                    treatyPricingGroupReferralGhsTableBos.TreatyPricingGroupReferralId = TreatyPricingGroupReferralFileBo.TreatyPricingGroupReferralId.GetValueOrDefault();
                    treatyPricingGroupReferralGhsTableBos.TreatyPricingGroupReferralFileId = TreatyPricingGroupReferralFileBo.Id;
                    treatyPricingGroupReferralGhsTableBos.CreatedById = TreatyPricingGroupReferralFileBo.CreatedById;
                    treatyPricingGroupReferralGhsTableBos.UpdatedById = TreatyPricingGroupReferralFileBo.UpdatedById;
                    var bo = treatyPricingGroupReferralGhsTableBos;
                    TreatyPricingGroupReferralGhsTableService.Create(ref bo);

                    UpdatedRecords++;
                }
            }
            return;
        }

        public void ClearDataByUploadType(int uploadedType)
        {
            if (uploadedType == TreatyPricingGroupReferralFileBo.UploadedTypeFile)
                TreatyPricingGroupReferralBos = new List<TreatyPricingGroupReferralBo> { };

            if (uploadedType == TreatyPricingGroupReferralFileBo.UploadedTypeTable)
            {
                if (TableTypePickListDetailBo != null)
                {
                    switch (TableTypePickListDetailBo.Code)
                    {
                        case PickListDetailBo.TableTypeHips:
                            TreatyPricingGroupReferralHipsTableBos = new List<TreatyPricingGroupReferralHipsTableBo> { };
                            DeleteTreatyPricingGroupReferralHipsTable();
                            break;
                        case PickListDetailBo.TableTypeGtlClaim:
                            TreatyPricingGroupReferralGtlTableBos = new List<TreatyPricingGroupReferralGtlTableBo> { };
                            DeleteTreatyPricingGroupReferralGtlTable(TreatyPricingGroupReferralGtlTableBo.TypeGtlClaim);
                            break;
                        case PickListDetailBo.TableTypeGtlRate:
                            TreatyPricingGroupReferralGtlTableBos = new List<TreatyPricingGroupReferralGtlTableBo> { };
                            DeleteTreatyPricingGroupReferralGtlTable(TreatyPricingGroupReferralGtlTableBo.TypeGtlUnitRates);
                            break;
                        case PickListDetailBo.TableTypeGtlAge:
                            TreatyPricingGroupReferralGtlTableBos = new List<TreatyPricingGroupReferralGtlTableBo> { };
                            DeleteTreatyPricingGroupReferralGtlTable(TreatyPricingGroupReferralGtlTableBo.TypeGtlAgeBanded);
                            break;
                        case PickListDetailBo.TableTypeGtlSa:
                            TreatyPricingGroupReferralGtlTableBos = new List<TreatyPricingGroupReferralGtlTableBo> { };
                            DeleteTreatyPricingGroupReferralGtlTable(TreatyPricingGroupReferralGtlTableBo.TypeGtlBasisOfSa);
                            break;
                        case PickListDetailBo.TableTypeGhsClaim:
                            TreatyPricingGroupReferralGhsTableBos = new List<TreatyPricingGroupReferralGhsTableBo> { };
                            DeleteTreatyPricingGroupReferralGhsTable();
                            break;
                    }
                }
            }
            return;
        }

        public void DeleteTreatyPricingGroupReferralHipsTable()
        {
            // DELETE ALL HIPS DATA BEFORE PROCESS
            PrintMessage("Deleting HIPS Data records...", true, false);
            TreatyPricingGroupReferralHipsTableService.DeleteByTreatyPricingGroupReferralId(TreatyPricingGroupReferralFileBo.TreatyPricingGroupReferralId.GetValueOrDefault());
            PrintMessage("Deleted HIPS Data records", true, false);
        }

        public void DeleteTreatyPricingGroupReferralGtlTable(int type)
        {
            // DELETE ALL GTL DATA BEFORE PROCESS
            PrintMessage("Deleting GTL Data records...", true, false);
            TreatyPricingGroupReferralGtlTableService.DeleteByTreatyPricingGroupReferralId(TreatyPricingGroupReferralFileBo.TreatyPricingGroupReferralId.GetValueOrDefault(), type);
            PrintMessage("Deleted GTL Data records", true, false);
        }

        public void DeleteTreatyPricingGroupReferralGhsTable()
        {
            // DELETE ALL GHS DATA BEFORE PROCESS
            PrintMessage("Deleting GHS Data records...", true, false);
            TreatyPricingGroupReferralGhsTableService.DeleteByTreatyPricingGroupReferralId(TreatyPricingGroupReferralFileBo.TreatyPricingGroupReferralId.GetValueOrDefault());
            PrintMessage("Deleted GHS Data records", true, false);
        }

        public bool IsSuccess
        {
            get
            {
                return Errors.Count == 0;
            }
        }

        public static bool ParseDateTime(string value, out DateTime? datetime, out string error)
        {
            var formats = new string[] { "dd/MM/yyyy hh:mm:ss tt", "d/MM/yyyy hh:mm:ss tt", "d/M/yyyy hh:mm:ss tt", "dd/M/yyyy hh:mm:ss tt", "yyyy-MM-dd", "yyyy-M-d" };

            datetime = null;
            error = "";
            try
            {
                CultureInfo provider = CultureInfo.InvariantCulture;
                if (DateTime.TryParseExact(value, formats, provider, DateTimeStyles.None, out DateTime dt))
                    datetime = dt;
                else
                    return false;
            }
            catch (Exception e)
            {
                error = e.Message;
                return false;
            }
            return true;
        }

        public string GenerateHipsTemplate()
        {
            var templateFilepath = Util.GetWebAppDocumentFilePath("HIPS_Template.xlsx");

            string PrefixFileName = string.Format("HIPS_Template");
            var directory = Util.GetTemporaryPath();
            var filename = PrefixFileName.AppendDateTimeFileName(".xlsx");
            var filepath = Path.Combine(directory, filename);

            HipsExcel = new Excel(templateFilepath, filepath, 2);

            // Delete all previous files
            Util.DeleteFiles(directory, $@"{PrefixFileName}*");

            HipsExcel.OpenTemplate();

            int rowIndex = 1;

            var bos = HipsCategoryDetailService.GetAll();
            foreach (var bo in bos)
            {
                rowIndex++;

                HipsExcel.WriteCell(rowIndex, 2, bo.HipsCategoryBo.Code);
                HipsExcel.WriteCell(rowIndex, 3, bo.Subcategory);
                HipsExcel.WriteCell(rowIndex, 4, bo.Description);
            }

            HipsExcel.Save();

            return filepath;
        }
    }
}
