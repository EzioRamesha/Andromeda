using BusinessObject;
using BusinessObject.Identity;
using DataAccess.Entities.Identity;
using Services;
using Services.Identity;
using Shared;
using Shared.DataAccess;
using Shared.ProcessFile;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace ConsoleApp.Commands.ProcessDatas
{
    public class ProcessTreatyDiscountTable : Command
    {
        public List<Column> Columns { get; set; }

        public string FilePath { get; set; }

        public HttpPostedFileBase PostedFile { get; set; }

        public int? AuthUserId { get; set; }

        public TextFile TextFile { get; set; }

        public List<string> Errors { get; set; }

        public List<string> UpdateAction { get; set; } = new List<string> { "u", "update" };

        public List<string> DeleteAction { get; set; } = new List<string> { "d", "del", "delete" };

        public char[] charsToTrim = { ',', '.', ' ' };

        public ProcessTreatyDiscountTable()
        {
            Title = "ProcessTreatyDiscountTable";
            Description = "To read Treaty Discount Table csv file and insert into database";
            Arguments = new string[]
            {
                "filePath",
            };
            Hide = true;
            Errors = new List<string> { };
            GetColumns();
        }

        public override bool Validate()
        {
            string filepath = CommandInput.Arguments[0];
            if (!File.Exists(filepath))
            {
                PrintError(string.Format(MessageBag.FileNotExists, filepath));
                return false;
            }
            else
            {
                FilePath = filepath;
            }

            return base.Validate();
        }

        public override void Run()
        {
            PrintStarting();

            Process();

            PrintEnding();
        }

        public void Process()
        {
            if (PostedFile != null)
            {
                TextFile = new TextFile(PostedFile.InputStream);
            }
            else if (File.Exists(FilePath))
            {
                TextFile = new TextFile(FilePath);
            }
            else
            {
                throw new Exception("No file can be read");
            }

            TrailObject trail;
            Result result;
            while (TextFile.GetNextRow() != null)
            {
                if (TextFile.RowIndex == 1)
                    continue; // Skip header row

                if (PrintCommitBuffer())
                {
                }
                SetProcessCount();

                bool error = false;

                TreatyDiscountTableBo tdt = null;
                TreatyDiscountTableDetailBo tdtd = null;
                try
                {
                    tdt = SetParentData();
                    tdtd = SetChildData();

                    if (string.IsNullOrEmpty(tdt.Rule))
                    {
                        SetProcessCount("Rule Empty");
                        Errors.Add(string.Format("Please enter the Rule at row {0}", TextFile.RowIndex));
                        error = true;
                    }

                    var typeStr = TextFile.GetColValue(TreatyDiscountTableBo.ColumnType);
                    if (string.IsNullOrEmpty(typeStr))
                    {
                        SetProcessCount("Type Empty");
                        Errors.Add(string.Format("Please enter the Type at row {0}", TextFile.RowIndex));
                        error = true;
                    }
                    else
                    {
                        var type = TreatyDiscountTableBo.GetTypeKey(typeStr);
                        if (type == 0)
                        {
                            SetProcessCount("Type Invalid");
                            Errors.Add(string.Format("Please enter valid Type (Direct Retro/Per Life) at row {0}", TextFile.RowIndex));
                            error = true;
                        }
                        else
                        {
                            tdt.Type = type;
                        }
                    }

                    // Child
                    var isChild = tdtd.Id != 0 || !string.IsNullOrEmpty(tdtd.BenefitCode) ||
                        !string.IsNullOrEmpty(tdtd.AgeFromStr) || !string.IsNullOrEmpty(tdtd.AgeToStr) ||
                        !string.IsNullOrEmpty(tdtd.AARFromStr) || !string.IsNullOrEmpty(tdtd.AARToStr) ||
                        !string.IsNullOrEmpty(tdtd.DiscountStr);

                    // Validate child if have child
                    if (isChild)
                    {
                        if (string.IsNullOrEmpty(tdtd.CedingPlanCode))
                        {
                            SetProcessCount("Ceding Plan Code Empty");
                            Errors.Add(string.Format("Please enter the Ceding Plan Code at row {0}", TextFile.RowIndex));
                            error = true;
                        }

                        if (!string.IsNullOrEmpty(tdtd.BenefitCode))
                        {
                            string[] benefitCodes = tdtd.BenefitCode.ToArraySplitTrim();
                            foreach (string benefitCodeStr in benefitCodes)
                            {
                                var benefitBo = BenefitService.FindByCode(benefitCodeStr);
                                if (benefitBo != null)
                                {
                                    if (benefitBo.Status == BenefitBo.StatusInactive)
                                    {
                                        SetProcessCount("Benefit Inactive");
                                        Errors.Add(string.Format("{0}: {1} at row {2}", MessageBag.BenefitStatusInactive, benefitCodeStr, TextFile.RowIndex));
                                        error = true;
                                    }
                                }
                                else
                                {
                                    SetProcessCount("Benefit Code Not Found");
                                    Errors.Add(string.Format("{0} at row {1}", string.Format(MessageBag.BenefitCodeNotFound, benefitCodeStr), TextFile.RowIndex));
                                    error = true;
                                }
                            }
                        }
                        else
                        {
                            tdtd.BenefitCode = null;
                        }

                        if (tdt.Type == TreatyDiscountTableBo.TypeDirectRetro)
                        {
                            if (!string.IsNullOrEmpty(tdtd.AgeFromStr))
                            {
                                if (int.TryParse(tdtd.AgeFromStr, out int ageFrom))
                                {
                                    tdtd.AgeFrom = ageFrom;
                                }
                                else
                                {
                                    SetProcessCount("Age From Invalid Numeric");
                                    Errors.Add(string.Format("The Age From is not a numeric: {0} at row {1}", tdtd.AgeFromStr, TextFile.RowIndex));
                                    error = true;
                                }
                            }
                            else
                            {
                                tdtd.AgeFrom = null;
                            }

                            if (!string.IsNullOrEmpty(tdtd.AgeToStr))
                            {
                                if (int.TryParse(tdtd.AgeToStr, out int ageTo))
                                {
                                    tdtd.AgeTo = ageTo;
                                }
                                else
                                {
                                    SetProcessCount("Age To Invalid Numeric");
                                    Errors.Add(string.Format("The Age To is not a numeric: {0} at row {1}", tdtd.AgeToStr, TextFile.RowIndex));
                                    error = true;
                                }
                            }
                            else
                            {
                                tdtd.AgeTo = null;
                            }
                        }
                        else
                        {
                            tdtd.AgeFrom = null;
                            tdtd.AgeTo = null;
                        }

                        if (tdt.Type == TreatyDiscountTableBo.TypePerLife)
                        {
                            if (!string.IsNullOrEmpty(tdtd.AARFromStr))
                            {
                                if (double.TryParse(tdtd.AARFromStr, out double aarFrom))
                                {
                                    tdtd.AARFrom = aarFrom;
                                }
                                else
                                {
                                    SetProcessCount("AAR From Invalid");
                                    Errors.Add(string.Format("The AAR From is invalid: {0} at row {1}", tdtd.AARFromStr, TextFile.RowIndex));
                                    error = true;
                                }
                            }
                            else
                            {
                                tdtd.AARFrom = null;
                            }

                            if (!string.IsNullOrEmpty(tdtd.AARToStr))
                            {
                                if (double.TryParse(tdtd.AARToStr, out double aarTo))
                                {
                                    tdtd.AARTo = aarTo;
                                }
                                else
                                {
                                    SetProcessCount("AAR To Invalid");
                                    Errors.Add(string.Format("The AAR To is invalid: {0} at row {1}", tdtd.AARToStr, TextFile.RowIndex));
                                    error = true;
                                }
                            }
                            else
                            {
                                tdtd.AARTo = null;
                            }
                        }
                        else
                        {
                            tdtd.AARFrom = null;
                            tdtd.AARTo = null;
                        }

                        if (!string.IsNullOrEmpty(tdtd.DiscountStr))
                        {
                            if (double.TryParse(tdtd.DiscountStr, out double discount))
                            {
                                tdtd.Discount = discount;
                            }
                            else
                            {
                                SetProcessCount("Discount Invalid");
                                Errors.Add(string.Format("The Discount is invalid: {0} at row {1}", tdtd.DiscountStr, TextFile.RowIndex));
                                error = true;
                            }
                        }
                        else
                        {
                            SetProcessCount("Discount Empty");
                            Errors.Add(string.Format("Please enter the Discount at row {0}", TextFile.RowIndex));
                            error = true;
                        }
                    }
                }
                catch (Exception e)
                {
                    SetProcessCount("Error");
                    Errors.Add(string.Format("Error Triggered: {0} at row {1}", e.Message, TextFile.RowIndex));
                    error = true;
                }
                string parentAction = TextFile.GetColValue(TreatyDiscountTableBo.ColumnAction);
                string childAction = TextFile.GetColValue(TreatyDiscountTableBo.ColumnDetailAction);

                if (error)
                {
                    continue;
                }

                // Parent
                TreatyDiscountTableBo treatyDiscountTableBo;
                if (tdt.Id != 0)
                {
                    treatyDiscountTableBo = TreatyDiscountTableService.Find(tdt.Id);
                }
                else
                {
                    treatyDiscountTableBo = TreatyDiscountTableService.FindByRule(tdt.Rule);
                    tdt.Id = treatyDiscountTableBo?.Id ?? 0;
                }

                if (UpdateAction.Contains(parentAction.ToLower().Trim()))
                {
                    // Handle Update
                    if (tdtd.Id != 0 || !string.IsNullOrEmpty(tdtd.CedingPlanCode) || !string.IsNullOrEmpty(childAction))
                    {
                        SetProcessCount("Child Data Not Allowed");
                        Errors.Add(string.Format("Unable to process Row {0} due to child data exist in Parent's Update Action", TextFile.RowIndex));
                        continue;
                    }

                    if (treatyDiscountTableBo == null)
                    {
                        AddParentNotFoundError(tdt);
                        continue;
                    }

                    UpdateParentData(ref treatyDiscountTableBo, tdt);

                    trail = new TrailObject();
                    result = TreatyDiscountTableService.Update(ref treatyDiscountTableBo, ref trail);

                    if (!result.Valid)
                    {
                        foreach (var e in result.ToErrorArray())
                        {
                            Errors.Add(string.Format("{0} at row {1}", e, TextFile.RowIndex));
                        }
                        error = true;
                    }
                    if (error)
                    {
                        continue;
                    }

                    TrailParent(result, treatyDiscountTableBo, trail, "Update");

                    continue;
                }
                else if (DeleteAction.Contains(parentAction.ToLower().Trim()))
                {
                    // Handle Delete
                    if (tdtd.Id != 0 || !string.IsNullOrEmpty(tdtd.CedingPlanCode) || !string.IsNullOrEmpty(childAction))
                    {
                        SetProcessCount("Child Data Not Allowed");
                        Errors.Add(string.Format("Unable to process Row {0} due to child data exist in Parent's Delete Action", TextFile.RowIndex));
                        continue;
                    }

                    if (treatyDiscountTableBo == null)
                    {
                        AddParentNotFoundError(tdt);
                        continue;
                    }

                    trail = new TrailObject();
                    result = TreatyDiscountTableService.Delete(treatyDiscountTableBo, ref trail);

                    if (!result.Valid)
                    {
                        foreach (var e in result.ToErrorArray())
                        {
                            Errors.Add(string.Format("{0} at row {1}", e, TextFile.RowIndex));
                        }
                        continue;
                    }

                    TrailParent(result, treatyDiscountTableBo, trail, "Delete");

                    continue;
                }
                else
                {
                    // Handle Create
                    if (treatyDiscountTableBo == null)
                    {
                        trail = new TrailObject();
                        result = TreatyDiscountTableService.Create(ref tdt, ref trail);
                        if (result.Valid)
                        {
                            treatyDiscountTableBo = tdt;
                            TrailParent(result, tdt, trail, "Create");
                        }
                        else
                        {
                            Errors.Add(string.Format("Unable to process Row {0} due to error occured during create Treaty Discount Table", TextFile.RowIndex));
                            foreach (var e in result.ToErrorArray())
                            {
                                Errors.Add(string.Format("{0} at row {1}", e, TextFile.RowIndex));
                            }
                            continue;
                        }
                    }

                    switch (childAction.ToLower().Trim())
                    {
                        case "u":
                        case "update":
                            TreatyDiscountTableDetailBo tdtdDb = TreatyDiscountTableDetailService.Find(tdtd.Id);
                            if (tdtdDb == null)
                            {
                                AddChildNotFoundError(tdtd);
                                continue;
                            }

                            // Range
                            var errorMsg = ValidateRange(tdtd);
                            if (!string.IsNullOrEmpty(errorMsg))
                            {
                                SetProcessCount("Existing Child Record Found");
                                Errors.Add(string.Format(errorMsg + " at row {0}", TextFile.RowIndex));
                                continue;
                            }

                            // Duplicate
                            if (IsChildDuplicate(treatyDiscountTableBo, tdtd))
                            {
                                SetProcessCount("Existing Child Record Found");
                                Errors.Add(string.Format("The Treaty Discount Table exists with same Combination at row {0}", TextFile.RowIndex));
                                continue;
                            }

                            tdtd.TreatyDiscountTableId = treatyDiscountTableBo.Id;
                            UpdateChildData(ref tdtdDb, tdtd);

                            trail = new TrailObject();
                            result = TreatyDiscountTableDetailService.Update(ref tdtdDb, ref trail);

                            if (!result.Valid)
                            {
                                foreach (var e in result.ToErrorArray())
                                {
                                    Errors.Add(string.Format("{0} at row {1}", e, TextFile.RowIndex));
                                }
                                continue;
                            }

                            TrailChild(result, tdtdDb, trail, "Update");

                            break;
                        case "d":
                        case "del":
                        case "delete":
                            tdtdDb = TreatyDiscountTableDetailService.Find(tdtd.Id);
                            if (tdtdDb == null)
                            {
                                AddChildNotFoundError(tdtd);
                                continue;
                            }

                            trail = new TrailObject();
                            result = TreatyDiscountTableDetailService.Delete(tdtd, ref trail);

                            if (!result.Valid)
                            {
                                foreach (var e in result.ToErrorArray())
                                {
                                    Errors.Add(string.Format("{0} at row {1}", e, TextFile.RowIndex));
                                }
                                continue;
                            }

                            TrailChild(result, tdtd, trail, "Delete");

                            break;
                        default:
                            if (tdt.Id != 0 && TreatyDiscountTableDetailService.IsExists(tdtd.Id))
                            {
                                SetProcessCount("Child Record Found");
                                Errors.Add(string.Format("The Treaty Discount Table Detail ID exists: {0} at row {1}", tdtd.Id, TextFile.RowIndex));
                                continue;
                            }

                            if (tdt.Id == 0 && string.IsNullOrEmpty(tdt.Rule))
                                continue;

                            // Range
                            errorMsg = ValidateRange(tdtd);
                            if (!string.IsNullOrEmpty(errorMsg))
                            {
                                SetProcessCount("Existing Child Record Found");
                                Errors.Add(string.Format(errorMsg + " at row {0}", TextFile.RowIndex));
                                continue;
                            }

                            // Duplicate
                            if (IsChildDuplicate(treatyDiscountTableBo, tdtd))
                            {
                                SetProcessCount("Existing Child Record Found");
                                Errors.Add(string.Format("The Treaty Discount Table exists with same Combination at row {0}", TextFile.RowIndex));
                                continue;
                            }

                            trail = new TrailObject();
                            tdtd.TreatyDiscountTableId = treatyDiscountTableBo.Id;
                            result = TreatyDiscountTableDetailService.Create(ref tdtd, ref trail);

                            if (!result.Valid)
                            {
                                foreach (var e in result.ToErrorArray())
                                {
                                    Errors.Add(string.Format("{0} at row {1}", e, TextFile.RowIndex));
                                }
                                continue;
                            }

                            TrailChild(result, tdtd, trail, "Create");
                            break;
                    }
                    continue;
                }
            }

            PrintProcessCount();

            TextFile.Close();

            foreach (string error in Errors)
            {
                PrintError(error);
            }
        }

        public TreatyDiscountTableBo SetParentData()
        {
            var tdt = new TreatyDiscountTableBo
            {
                Id = 0,
                Rule = TextFile.GetColValue(TreatyDiscountTableBo.ColumnRule),
                //Type = TextFile.GetColValue(TreatyDiscountTableBo.ColumnType),
                Description = TextFile.GetColValue(TreatyDiscountTableBo.ColumnDescription),
                CreatedById = AuthUserId != null ? AuthUserId.Value : User.DefaultSuperUserId,
                UpdatedById = AuthUserId != null ? AuthUserId : User.DefaultSuperUserId,
            };

            tdt.Rule = tdt.Rule?.Trim();
            tdt.Description = tdt.Description?.Trim();

            string idStr = TextFile.GetColValue(TreatyDiscountTableBo.ColumnId);
            if (!string.IsNullOrEmpty(idStr) && int.TryParse(idStr, out int id))
            {
                tdt.Id = id;
            }

            return tdt;
        }

        public TreatyDiscountTableDetailBo SetChildData()
        {
            var tdtd = new TreatyDiscountTableDetailBo
            {
                Id = 0,
                CedingPlanCode = TextFile.GetColValue(TreatyDiscountTableBo.ColumnCedingPlanCode),
                BenefitCode = TextFile.GetColValue(TreatyDiscountTableBo.ColumnBenefitCode),
                AgeFromStr = TextFile.GetColValue(TreatyDiscountTableBo.ColumnAgeFrom),
                AgeToStr = TextFile.GetColValue(TreatyDiscountTableBo.ColumnAgeTo),
                AARFromStr = TextFile.GetColValue(TreatyDiscountTableBo.ColumnAarFrom),
                AARToStr = TextFile.GetColValue(TreatyDiscountTableBo.ColumnAarTo),
                DiscountStr = TextFile.GetColValue(TreatyDiscountTableBo.ColumnDiscount),
                CreatedById = AuthUserId != null ? AuthUserId.Value : User.DefaultSuperUserId,
                UpdatedById = AuthUserId != null ? AuthUserId : User.DefaultSuperUserId,
            };

            tdtd.CedingPlanCode = tdtd.CedingPlanCode?.Trim()?.TrimEnd(charsToTrim);
            tdtd.BenefitCode = tdtd.BenefitCode?.Trim()?.TrimEnd(charsToTrim);
            tdtd.AgeFromStr = tdtd.AgeFromStr?.Trim();
            tdtd.AgeToStr = tdtd.AgeToStr?.Trim();
            tdtd.AARFromStr = tdtd.AARFromStr?.Trim();
            tdtd.AARToStr = tdtd.AARToStr?.Trim();
            tdtd.DiscountStr = tdtd.DiscountStr?.Trim();

            string idStr = TextFile.GetColValue(TreatyDiscountTableBo.ColumnDetailId);
            if (!string.IsNullOrEmpty(idStr) && int.TryParse(idStr, out int id))
            {
                tdtd.Id = id;
            }

            return tdtd;
        }

        public void UpdateParentData(ref TreatyDiscountTableBo tdtDb, TreatyDiscountTableBo tdt)
        {
            tdtDb.Rule = tdt.Rule;
            tdtDb.Type = tdt.Type;
            tdtDb.Description = tdt.Description;
            tdtDb.UpdatedById = tdt.UpdatedById;
        }

        public void UpdateChildData(ref TreatyDiscountTableDetailBo tdtdDb, TreatyDiscountTableDetailBo tdtd)
        {
            tdtdDb.TreatyDiscountTableId = tdtd.TreatyDiscountTableId;
            tdtdDb.CedingPlanCode = tdtd.CedingPlanCode;
            tdtdDb.BenefitCode = tdtd.BenefitCode;
            tdtdDb.AgeFrom = tdtd.AgeFrom;
            tdtdDb.AgeTo = tdtd.AgeTo;
            tdtdDb.AARFrom = tdtd.AARFrom;
            tdtdDb.AARTo = tdtd.AARTo;
            tdtdDb.Discount = tdtd.Discount;
            tdtdDb.UpdatedById = tdtd.UpdatedById;
        }

        public void TrailParent(Result result, TreatyDiscountTableBo tdt, TrailObject trail, string action)
        {
            if (result.Valid)
            {
                UserTrailBo userTrailBo = new UserTrailBo(
                    tdt.Id,
                    string.Format("{0} Treaty Discount Table", action),
                    result,
                    trail,
                    tdt.UpdatedById.Value
                );
                UserTrailService.Create(ref userTrailBo);

                SetProcessCount(string.Format("{0} Parent", action));
            }
        }

        public void TrailChild(Result result, TreatyDiscountTableDetailBo tdtd, TrailObject trail, string action)
        {
            if (result.Valid)
            {
                UserTrailBo userTrailBo = new UserTrailBo(
                    tdtd.Id,
                    string.Format("{0} Treaty Discount Table Detail", action),
                    result,
                    trail,
                    tdtd.UpdatedById.Value
                );
                UserTrailService.Create(ref userTrailBo);

                SetProcessCount(string.Format("{0} Child", action));
            }
        }

        public bool ValidateDateTimeFormat(int type, ref TreatyDiscountTableBo tdt)
        {
            string header = Columns.Where(m => m.ColIndex == type).Select(m => m.Header).FirstOrDefault();
            string property = Columns.Where(m => m.ColIndex == type).Select(m => m.Property).FirstOrDefault();

            bool valid = true;
            string value = TextFile.GetColValue(type);
            if (!string.IsNullOrEmpty(value))
            {
                if (Util.TryParseDateTime(value, out DateTime? datetime, out string error))
                {
                    tdt.SetPropertyValue(property, datetime.Value);
                }
                else
                {
                    SetProcessCount(string.Format(header, "Error"));
                    Errors.Add(string.Format("The {0} format can not be read: {1} at row {2}", header, value, TextFile.RowIndex));
                    valid = false;
                }
            }
            return valid;
        }

        public void AddParentNotFoundError(TreatyDiscountTableBo tdt)
        {
            SetProcessCount("Record Not Found");
            Errors.Add(string.Format("The Treaty Discount Table ID doesn't exists: {0} at row {1}", tdt.Id, TextFile.RowIndex));
        }

        public void AddChildNotFoundError(TreatyDiscountTableDetailBo tdtd)
        {
            SetProcessCount("Record Not Found");
            Errors.Add(string.Format("The Treaty Discount Table Detail ID doesn't exists: {0} at row {1}", tdtd.Id, TextFile.RowIndex));
        }

        public List<Column> GetColumns()
        {
            Columns = TreatyDiscountTableBo.GetColumns();
            return Columns;
        }

        public string ValidateRange(TreatyDiscountTableDetailBo tdtd)
        {
            string error = null;

            if (tdtd.AgeFrom.HasValue && !tdtd.AgeTo.HasValue)
            {
                SetProcessCount("Age To Required");
                error = string.Format(MessageBag.Required, "Age To");
            }
            else if (!tdtd.AgeFrom.HasValue && tdtd.AgeTo.HasValue)
            {
                SetProcessCount("Age From Required");
                error = string.Format(MessageBag.Required, "Age From");
            }
            else if (tdtd.AgeFrom.HasValue && tdtd.AgeTo.HasValue && tdtd.AgeTo < tdtd.AgeFrom)
            {
                SetProcessCount("Age To Greater or Equal to Age From");
                error = string.Format(MessageBag.GreaterOrEqualTo, "Age To", "Age From");
            }

            if (tdtd.AARFrom.HasValue && !tdtd.AARTo.HasValue)
            {
                SetProcessCount("AAR To Required");
                error = string.Format(MessageBag.Required, "AAR To");
            }
            else if (!tdtd.AARFrom.HasValue && tdtd.AARTo.HasValue)
            {
                SetProcessCount("AAR From Required");
                error = string.Format(MessageBag.Required, "AAR From");
            }
            else if (tdtd.AARFrom.HasValue && tdtd.AARTo.HasValue && tdtd.AARTo < tdtd.AARFrom)
            {
                SetProcessCount("AAR To Greater or Equal to AAR From");
                error = string.Format(MessageBag.GreaterOrEqualTo, "AAR To", "AAR From");
            }

            return error;
        }

        public bool IsChildDuplicate(TreatyDiscountTableBo treatyDiscountTableBo, TreatyDiscountTableDetailBo tdtd)
        {
            var bos = TreatyDiscountTableDetailService.GetByTreatyDiscountTableIdExcludeId(treatyDiscountTableBo.Id, tdtd.Id);

            var cedingPlanCodes = Util.ToArraySplitTrim(tdtd.CedingPlanCode).ToList();
            var benefitCodes = Util.ToArraySplitTrim(tdtd.BenefitCode).ToList();

            var filteredBos = new List<TreatyDiscountTableDetailBo>();

            if (treatyDiscountTableBo.Type == TreatyDiscountTableBo.TypeDirectRetro && tdtd.AgeFrom.HasValue && tdtd.AgeTo.HasValue)
            {
                filteredBos = bos.Where(q =>
                (
                    q.AgeFrom <= tdtd.AgeFrom && q.AgeTo >= tdtd.AgeFrom
                    ||
                    q.AgeFrom <= tdtd.AgeTo && q.AgeTo >= tdtd.AgeTo
                )
                || (q.AgeFrom == null && q.AgeTo == null)
                ).ToList();
            }
            else if (treatyDiscountTableBo.Type == TreatyDiscountTableBo.TypePerLife && tdtd.AARFrom.HasValue && tdtd.AARTo.HasValue)
            {
                filteredBos = bos.Where(q =>
                (
                    q.AARFrom <= tdtd.AARFrom && q.AARTo >= tdtd.AARFrom
                    ||
                    q.AARFrom <= tdtd.AARTo && q.AARTo >= tdtd.AARTo
                )
                || (q.AARFrom == null && q.AARTo == null)
                ).ToList();
            }
            else
            {
                filteredBos = treatyDiscountTableBo.Type == TreatyDiscountTableBo.TypeDirectRetro ? bos.Where(q => q.AgeFrom == null && q.AgeTo == null).ToList() : bos.Where(q => q.AARFrom == null && q.AARTo == null).ToList();
            }

            foreach (var filteredBo in filteredBos)
            {
                var cpcList = Util.ToArraySplitTrim(filteredBo.CedingPlanCode).ToList();
                var bcList = Util.ToArraySplitTrim(filteredBo.BenefitCode).ToList();

                var cpcIntersect = cpcList.Intersect(cedingPlanCodes);
                var bcIntersect = bcList.Intersect(benefitCodes);

                if ((cpcIntersect.Count() > 0 && string.IsNullOrEmpty(tdtd.BenefitCode)) || (cpcIntersect.Count() > 0 && bcIntersect.Count() > 0))
                    return true;
            }

            return false;
        }
    }
}
