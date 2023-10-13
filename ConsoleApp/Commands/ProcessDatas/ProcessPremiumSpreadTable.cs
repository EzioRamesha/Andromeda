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
    public class ProcessPremiumSpreadTable : Command
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

        public ProcessPremiumSpreadTable()
        {
            Title = "ProcessPremiumSpreadTable";
            Description = "To read Premium Spread Table csv file and insert into database";
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

                PremiumSpreadTableBo pst = null;
                PremiumSpreadTableDetailBo pstd = null;
                try
                {
                    pst = SetParentData();
                    pstd = SetChildData();

                    if (string.IsNullOrEmpty(pst.Rule))
                    {
                        SetProcessCount("Rule Empty");
                        Errors.Add(string.Format("Please enter the Rule at row {0}", TextFile.RowIndex));
                        error = true;
                    }

                    var typeStr = TextFile.GetColValue(PremiumSpreadTableBo.ColumnType);
                    if (string.IsNullOrEmpty(typeStr))
                    {
                        SetProcessCount("Type Empty");
                        Errors.Add(string.Format("Please enter the Type at row {0}", TextFile.RowIndex));
                        error = true;
                    }
                    else
                    {
                        var type = PremiumSpreadTableBo.GetTypeKey(typeStr);
                        if (type == 0)
                        {
                            SetProcessCount("Type Invalid");
                            Errors.Add(string.Format("Please enter valid Type (Direct Retro/Per Life) at row {0}", TextFile.RowIndex));
                            error = true;
                        }
                        else
                        {
                            pst.Type = type;
                        }
                    }

                    // Child
                    var isChild = pstd.Id != 0 || !string.IsNullOrEmpty(pstd.BenefitCode) || !string.IsNullOrEmpty(pstd.AgeFromStr) || !string.IsNullOrEmpty(pstd.AgeToStr) || !string.IsNullOrEmpty(pstd.PremiumSpreadStr);
                    if ((pstd.Id != 0 && string.IsNullOrEmpty(pstd.CedingPlanCode)) ||
                        ((!string.IsNullOrEmpty(pstd.BenefitCode) || !string.IsNullOrEmpty(pstd.AgeFromStr) || !string.IsNullOrEmpty(pstd.AgeToStr) || !string.IsNullOrEmpty(pstd.PremiumSpreadStr)) && string.IsNullOrEmpty(pstd.CedingPlanCode))
                    )
                    {
                        SetProcessCount("Ceding Plan Code Empty");
                        Errors.Add(string.Format("Please enter the Ceding Plan Code at row {0}", TextFile.RowIndex));
                        error = true;
                        isChild = true;
                    }

                    // Validate child if have child
                    if (isChild)
                    {
                        if (!string.IsNullOrEmpty(pstd.BenefitCode))
                        {
                            string[] benefitCodes = pstd.BenefitCode.ToArraySplitTrim();
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
                            pstd.BenefitCode = null;
                        }

                        if (!string.IsNullOrEmpty(pstd.AgeFromStr))
                        {
                            if (int.TryParse(pstd.AgeFromStr, out int ageFrom))
                            {
                                pstd.AgeFrom = ageFrom;
                            }
                            else
                            {
                                SetProcessCount("Age From Invalid Numeric");
                                Errors.Add(string.Format("The Age From is not a numeric: {0} at row {1}", pstd.AgeFromStr, TextFile.RowIndex));
                                error = true;
                            }
                        }
                        else
                        {
                            pstd.AgeFrom = null;
                        }

                        if (!string.IsNullOrEmpty(pstd.AgeToStr))
                        {
                            if (int.TryParse(pstd.AgeToStr, out int ageTo))
                            {
                                pstd.AgeTo = ageTo;
                            }
                            else
                            {
                                SetProcessCount("Age To Invalid Numeric");
                                Errors.Add(string.Format("The Age To is not a numeric: {0} at row {1}", pstd.AgeToStr, TextFile.RowIndex));
                                error = true;
                            }
                        }
                        else
                        {
                            pstd.AgeTo = null;
                        }

                        if (!string.IsNullOrEmpty(pstd.PremiumSpreadStr))
                        {
                            if (double.TryParse(pstd.PremiumSpreadStr, out double premiumSpread))
                            {
                                pstd.PremiumSpread = premiumSpread;
                            }
                            else
                            {
                                SetProcessCount("Premium Spread Invalid");
                                Errors.Add(string.Format("The Premium Spread is invalid: {0} at row {1}", pstd.PremiumSpreadStr, TextFile.RowIndex));
                                error = true;
                            }
                        }
                        else
                        {
                            SetProcessCount("Premium Spread Empty");
                            Errors.Add(string.Format("Please enter the Premium Spread at row {0}", TextFile.RowIndex));
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
                string parentAction = TextFile.GetColValue(PremiumSpreadTableBo.ColumnAction);
                string childAction = TextFile.GetColValue(PremiumSpreadTableBo.ColumnDetailAction);

                if (error)
                {
                    continue;
                }

                // Parent
                PremiumSpreadTableBo premiumSpreadTableBo;
                if (pst.Id != 0)
                {
                    premiumSpreadTableBo = PremiumSpreadTableService.Find(pst.Id);
                }
                else
                {
                    premiumSpreadTableBo = PremiumSpreadTableService.FindByRule(pst.Rule);
                    pst.Id = premiumSpreadTableBo?.Id ?? 0;
                }

                if (UpdateAction.Contains(parentAction.ToLower().Trim()))
                {
                    // Handle Update
                    if (pstd.Id != 0 || !string.IsNullOrEmpty(pstd.CedingPlanCode) || !string.IsNullOrEmpty(childAction))
                    {
                        SetProcessCount("Child Data Not Allowed");
                        Errors.Add(string.Format("Unable to process Row {0} due to child data exist in Parent's Update Action", TextFile.RowIndex));
                        continue;
                    }

                    if (premiumSpreadTableBo == null)
                    {
                        AddParentNotFoundError(pst);
                        continue;
                    }

                    UpdateParentData(ref premiumSpreadTableBo, pst);

                    trail = new TrailObject();
                    result = PremiumSpreadTableService.Update(ref premiumSpreadTableBo, ref trail);

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

                    TrailParent(result, premiumSpreadTableBo, trail, "Update");

                    continue;
                }
                else if (DeleteAction.Contains(parentAction.ToLower().Trim()))
                {
                    // Handle Delete
                    if (pstd.Id != 0 || !string.IsNullOrEmpty(pstd.CedingPlanCode) || !string.IsNullOrEmpty(childAction))
                    {
                        SetProcessCount("Child Data Not Allowed");
                        Errors.Add(string.Format("Unable to process Row {0} due to child data exist in Parent's Delete Action", TextFile.RowIndex));
                        continue;
                    }

                    if (premiumSpreadTableBo == null)
                    {
                        AddParentNotFoundError(pst);
                        continue;
                    }

                    trail = new TrailObject();
                    result = PremiumSpreadTableService.Delete(premiumSpreadTableBo, ref trail);

                    if (!result.Valid)
                    {
                        foreach (var e in result.ToErrorArray())
                        {
                            Errors.Add(string.Format("{0} at row {1}", e, TextFile.RowIndex));
                        }
                        continue;
                    }

                    TrailParent(result, premiumSpreadTableBo, trail, "Delete");

                    continue;
                }
                else
                {
                    // Handle Create
                    if (premiumSpreadTableBo == null)
                    {
                        trail = new TrailObject();
                        result = PremiumSpreadTableService.Create(ref pst, ref trail);
                        if (result.Valid)
                        {
                            premiumSpreadTableBo = pst;
                            TrailParent(result, pst, trail, "Create");
                        }
                        else
                        {
                            Errors.Add(string.Format("Unable to process Row {0} due to error occured during create Premium Spread Table", TextFile.RowIndex));
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
                            PremiumSpreadTableDetailBo pstdDb = PremiumSpreadTableDetailService.Find(pstd.Id);
                            if (pstdDb == null)
                            {
                                AddChildNotFoundError(pstd);
                                continue;
                            }

                            // Age Range
                            var errorMsg = ValidateAgeRange(pstd);
                            if (!string.IsNullOrEmpty(errorMsg))
                            {
                                SetProcessCount("Existing Child Record Found");
                                Errors.Add(string.Format(errorMsg + " at row {0}", TextFile.RowIndex));
                                continue;
                            }

                            // Duplicate
                            if (IsChildDuplicate(premiumSpreadTableBo, pstd))
                            {
                                SetProcessCount("Existing Child Record Found");
                                Errors.Add(string.Format("The Premium Spread Table exists with same Combination at row {0}", TextFile.RowIndex));
                                continue;
                            }

                            pstd.PremiumSpreadTableId = premiumSpreadTableBo.Id;
                            UpdateChildData(ref pstdDb, pstd);

                            trail = new TrailObject();
                            result = PremiumSpreadTableDetailService.Update(ref pstdDb, ref trail);

                            if (!result.Valid)
                            {
                                foreach (var e in result.ToErrorArray())
                                {
                                    Errors.Add(string.Format("{0} at row {1}", e, TextFile.RowIndex));
                                }
                                continue;
                            }

                            TrailChild(result, pstdDb, trail, "Update");

                            break;
                        case "d":
                        case "del":
                        case "delete":
                            pstdDb = PremiumSpreadTableDetailService.Find(pstd.Id);
                            if (pstdDb == null)
                            {
                                AddChildNotFoundError(pstd);
                                continue;
                            }

                            trail = new TrailObject();
                            result = PremiumSpreadTableDetailService.Delete(pstd, ref trail);

                            if (!result.Valid)
                            {
                                foreach (var e in result.ToErrorArray())
                                {
                                    Errors.Add(string.Format("{0} at row {1}", e, TextFile.RowIndex));
                                }
                                continue;
                            }

                            TrailChild(result, pstd, trail, "Delete");

                            break;
                        default:
                            if (pst.Id != 0 && PremiumSpreadTableDetailService.IsExists(pstd.Id))
                            {
                                SetProcessCount("Child Record Found");
                                Errors.Add(string.Format("The Premium Spread Table Detail ID exists: {0} at row {1}", pstd.Id, TextFile.RowIndex));
                                continue;
                            }

                            if (pst.Id == 0 && string.IsNullOrEmpty(pst.Rule))
                                continue;

                            // Age Range
                            errorMsg = ValidateAgeRange(pstd);
                            if (!string.IsNullOrEmpty(errorMsg))
                            {
                                SetProcessCount("Existing Child Record Found");
                                Errors.Add(string.Format(errorMsg + " at row {0}", TextFile.RowIndex));
                                continue;
                            }

                            // Duplicate
                            if (IsChildDuplicate(premiumSpreadTableBo, pstd))
                            {
                                SetProcessCount("Existing Child Record Found");
                                Errors.Add(string.Format("The Premium Spread Table exists with same Combination at row {0}", TextFile.RowIndex));
                                continue;
                            }

                            trail = new TrailObject();
                            pstd.PremiumSpreadTableId = premiumSpreadTableBo.Id;
                            result = PremiumSpreadTableDetailService.Create(ref pstd, ref trail);

                            if (!result.Valid)
                            {
                                foreach (var e in result.ToErrorArray())
                                {
                                    Errors.Add(string.Format("{0} at row {1}", e, TextFile.RowIndex));
                                }
                                continue;
                            }

                            TrailChild(result, pstd, trail, "Create");
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

        public PremiumSpreadTableBo SetParentData()
        {
            var pst = new PremiumSpreadTableBo
            {
                Id = 0,
                Rule = TextFile.GetColValue(PremiumSpreadTableBo.ColumnRule),
                //Type = TextFile.GetColValue(PremiumSpreadTableBo.ColumnType),
                Description = TextFile.GetColValue(PremiumSpreadTableBo.ColumnDescription),
                CreatedById = AuthUserId != null ? AuthUserId.Value : User.DefaultSuperUserId,
                UpdatedById = AuthUserId != null ? AuthUserId : User.DefaultSuperUserId,
            };

            pst.Rule = pst.Rule?.Trim();
            pst.Description = pst.Description?.Trim();

            string idStr = TextFile.GetColValue(PremiumSpreadTableBo.ColumnId);
            if (!string.IsNullOrEmpty(idStr) && int.TryParse(idStr, out int id))
            {
                pst.Id = id;
            }

            return pst;
        }

        public PremiumSpreadTableDetailBo SetChildData()
        {
            var pstd = new PremiumSpreadTableDetailBo
            {
                Id = 0,
                CedingPlanCode = TextFile.GetColValue(PremiumSpreadTableBo.ColumnCedingPlanCode),
                BenefitCode = TextFile.GetColValue(PremiumSpreadTableBo.ColumnBenefitCode),
                AgeFromStr = TextFile.GetColValue(PremiumSpreadTableBo.ColumnAgeFrom),
                AgeToStr = TextFile.GetColValue(PremiumSpreadTableBo.ColumnAgeTo),
                PremiumSpreadStr = TextFile.GetColValue(PremiumSpreadTableBo.ColumnPremiumSpread),
                CreatedById = AuthUserId != null ? AuthUserId.Value : User.DefaultSuperUserId,
                UpdatedById = AuthUserId != null ? AuthUserId : User.DefaultSuperUserId,
            };

            pstd.CedingPlanCode = pstd.CedingPlanCode?.Trim()?.TrimEnd(charsToTrim);
            pstd.BenefitCode = pstd.BenefitCode?.Trim()?.TrimEnd(charsToTrim);
            pstd.AgeFromStr = pstd.AgeFromStr?.Trim();
            pstd.AgeToStr = pstd.AgeToStr?.Trim();
            pstd.PremiumSpreadStr = pstd.PremiumSpreadStr?.Trim();

            string idStr = TextFile.GetColValue(PremiumSpreadTableBo.ColumnDetailId);
            if (!string.IsNullOrEmpty(idStr) && int.TryParse(idStr, out int id))
            {
                pstd.Id = id;
            }

            return pstd;
        }

        public void UpdateParentData(ref PremiumSpreadTableBo pstDb, PremiumSpreadTableBo pst)
        {
            pstDb.Rule = pst.Rule;
            pstDb.Type = pst.Type;
            pstDb.Description = pst.Description;
            pstDb.UpdatedById = pst.UpdatedById;
        }

        public void UpdateChildData(ref PremiumSpreadTableDetailBo pstdDb, PremiumSpreadTableDetailBo pstd)
        {
            pstdDb.PremiumSpreadTableId = pstd.PremiumSpreadTableId;
            pstdDb.CedingPlanCode = pstd.CedingPlanCode;
            pstdDb.BenefitCode = pstd.BenefitCode;
            pstdDb.AgeFrom = pstd.AgeFrom;
            pstdDb.AgeTo = pstd.AgeTo;
            pstdDb.PremiumSpread = pstd.PremiumSpread;
            pstdDb.UpdatedById = pstd.UpdatedById;
        }

        public void TrailParent(Result result, PremiumSpreadTableBo pst, TrailObject trail, string action)
        {
            if (result.Valid)
            {
                UserTrailBo userTrailBo = new UserTrailBo(
                    pst.Id,
                    string.Format("{0} Premium Spread Table", action),
                    result,
                    trail,
                    pst.UpdatedById.Value
                );
                UserTrailService.Create(ref userTrailBo);

                SetProcessCount(string.Format("{0} Parent", action));
            }
        }

        public void TrailChild(Result result, PremiumSpreadTableDetailBo pstd, TrailObject trail, string action)
        {
            if (result.Valid)
            {
                UserTrailBo userTrailBo = new UserTrailBo(
                    pstd.Id,
                    string.Format("{0} Premium Spread Table Detail", action),
                    result,
                    trail,
                    pstd.UpdatedById.Value
                );
                UserTrailService.Create(ref userTrailBo);

                SetProcessCount(string.Format("{0} Child", action));
            }
        }

        public bool ValidateDateTimeFormat(int type, ref PremiumSpreadTableBo pst)
        {
            string header = Columns.Where(m => m.ColIndex == type).Select(m => m.Header).FirstOrDefault();
            string property = Columns.Where(m => m.ColIndex == type).Select(m => m.Property).FirstOrDefault();

            bool valid = true;
            string value = TextFile.GetColValue(type);
            if (!string.IsNullOrEmpty(value))
            {
                if (Util.TryParseDateTime(value, out DateTime? datetime, out string error))
                {
                    pst.SetPropertyValue(property, datetime.Value);
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

        public void AddParentNotFoundError(PremiumSpreadTableBo pst)
        {
            SetProcessCount("Record Not Found");
            Errors.Add(string.Format("The Premium Spread Table ID doesn't exists: {0} at row {1}", pst.Id, TextFile.RowIndex));
        }

        public void AddChildNotFoundError(PremiumSpreadTableDetailBo pstd)
        {
            SetProcessCount("Record Not Found");
            Errors.Add(string.Format("The Premium Spread Table Detail ID doesn't exists: {0} at row {1}", pstd.Id, TextFile.RowIndex));
        }

        public List<Column> GetColumns()
        {
            Columns = PremiumSpreadTableBo.GetColumns();
            return Columns;
        }

        public string ValidateAgeRange(PremiumSpreadTableDetailBo pstd)
        {
            string error = null;

            if (pstd.AgeFrom.HasValue && !pstd.AgeTo.HasValue)
            {
                SetProcessCount("Age To Required");
                error = string.Format(MessageBag.Required, "Age To");
            }
            else if (!pstd.AgeFrom.HasValue && pstd.AgeTo.HasValue)
            {
                SetProcessCount("Age From Required");
                error = string.Format(MessageBag.Required, "Age From");
            }
            else if (pstd.AgeFrom.HasValue && pstd.AgeTo.HasValue && pstd.AgeTo < pstd.AgeFrom)
            {
                SetProcessCount("Age To Greater or Equal to Age From");
                error = string.Format(MessageBag.GreaterOrEqualTo, "Age To", "Age From");
            }

            return error;
        }

        public bool IsChildDuplicate(PremiumSpreadTableBo premiumSpreadTableBo, PremiumSpreadTableDetailBo pstd)
        {
            var bos = PremiumSpreadTableDetailService.GetByPremiumSpreadTableIdExcludeId(premiumSpreadTableBo.Id, pstd.Id);

            var cedingPlanCodes = Util.ToArraySplitTrim(pstd.CedingPlanCode).ToList();
            var benefitCodes = Util.ToArraySplitTrim(pstd.BenefitCode).ToList();

            var filteredBos = new List<PremiumSpreadTableDetailBo>();

            if (pstd.AgeFrom.HasValue && pstd.AgeTo.HasValue)
            {
                filteredBos = bos.Where(q =>
                (
                    q.AgeFrom <= pstd.AgeFrom && q.AgeTo >= pstd.AgeFrom
                    ||
                    q.AgeFrom <= pstd.AgeTo && q.AgeTo >= pstd.AgeTo
                )
                || (q.AgeFrom == null && q.AgeTo == null)
                ).ToList();
            }
            else
            {
                filteredBos = bos.Where(q => q.AgeFrom == null && q.AgeTo == null).ToList();
            }

            foreach (var filteredBo in filteredBos)
            {
                var cpcList = Util.ToArraySplitTrim(filteredBo.CedingPlanCode).ToList();
                var bcList = Util.ToArraySplitTrim(filteredBo.BenefitCode).ToList();

                var cpcIntersect = cpcList.Intersect(cedingPlanCodes);
                var bcIntersect = bcList.Intersect(benefitCodes);

                if ((cpcIntersect.Count() > 0 && string.IsNullOrEmpty(pstd.BenefitCode)) || (cpcIntersect.Count() > 0 && bcIntersect.Count() > 0))
                    return true;
            }

            return false;
        }
    }
}
