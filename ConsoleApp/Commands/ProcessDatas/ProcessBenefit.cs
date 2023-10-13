using BusinessObject;
using BusinessObject.Identity;
using DataAccess.Entities;
using Services;
using Services.Identity;
using Shared;
using Shared.DataAccess;
using Shared.ProcessFile;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;

namespace ConsoleApp.Commands.ProcessDatas
{
    public class ProcessBenefit : Command
    {
        public List<Column> Columns { get; set; }

        public string FilePath { get; set; }

        public HttpPostedFileBase PostedFile { get; set; }

        public int? AuthUserId { get; set; }

        public TextFile TextFile { get; set; }

        public List<string> Errors { get; set; }

        public List<string> MLReEventCodes { get; set; }

        public List<string> ClaimCodes { get; set; }

        public ProcessBenefit()
        {
            Title = "ProcessBenefit";
            Description = "To read Benefit csv file and insert into database";
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

                var entity = new Benefit();
                BenefitBo b = null;
                try
                {
                    b = SetData();

                    if (string.IsNullOrEmpty(b.Code))
                    {
                        SetProcessCount("MLRe Benefit Code Empty");
                        Errors.Add(string.Format("Please enter the MLRe Benefit Code at row {0}", TextFile.RowIndex));
                        error = true;
                    }
                    else
                    {
                        var maxLengthAttr = entity.GetAttributeFrom<MaxLengthAttribute>("Code");
                        if (maxLengthAttr != null && b.Code.Length > maxLengthAttr.Length)
                        {
                            SetProcessCount("MLRe Benefit Code exceeded max length");
                            Errors.Add(string.Format("MLRe Benefit Code length can not be more than {0} characters at row {1}", maxLengthAttr.Length, TextFile.RowIndex));
                            error = true;
                        }
                    }

                    if (string.IsNullOrEmpty(b.Type))
                    {
                        SetProcessCount("Benefit Type Empty");
                        Errors.Add(string.Format("Please enter the Benefit Type at row {0}", TextFile.RowIndex));
                        error = true;
                    }
                    else
                    {
                        var maxLengthAttr = entity.GetAttributeFrom<MaxLengthAttribute>("Type");
                        if (maxLengthAttr != null && b.Type.Length > maxLengthAttr.Length)
                        {
                            SetProcessCount("Type exceeded max length");
                            Errors.Add(string.Format("Type length can not be more than {0} characters at row {1}", maxLengthAttr.Length, TextFile.RowIndex));
                            error = true;
                        }
                    }

                    if (string.IsNullOrEmpty(b.Description))
                    {
                        SetProcessCount("Description Empty");
                        Errors.Add(string.Format("Please enter the Description at row {0}", TextFile.RowIndex));
                        error = true;
                    }
                    else
                    {
                        var maxLengthAttr = entity.GetAttributeFrom<MaxLengthAttribute>("Description");
                        if (maxLengthAttr != null && b.Description.Length > maxLengthAttr.Length)
                        {
                            SetProcessCount("Description exceeded max length");
                            Errors.Add(string.Format("Description length can not be more than {0} characters at row {1}", maxLengthAttr.Length, TextFile.RowIndex));
                            error = true;
                        }
                    }

                    if (!string.IsNullOrEmpty(b.StatusStr))
                    {
                        int key = BenefitBo.GetStatusKey(b.StatusStr);

                        if (key == 0)
                        {
                            SetProcessCount("Invalid Status");
                            Errors.Add(string.Format("Status only can be Active/Inactive at row {0}", TextFile.RowIndex));
                            error = true;
                        }
                        else
                        {
                            b.Status = key;
                        }
                    }
                    else
                    {
                        SetProcessCount("Status Empty");
                        Errors.Add(string.Format("Please enter the Status at row {0}", TextFile.RowIndex));
                        error = true;
                    }

                    var effectiveStartDate = TextFile.GetColValue(BenefitBo.ColumnEffectiveStartDate);
                    if (!string.IsNullOrEmpty(effectiveStartDate))
                    {
                        if (!ValidateDateTimeFormat(BenefitBo.ColumnEffectiveStartDate, ref b))
                        {
                            error = true;
                        }
                    }
                    else
                    {
                        SetProcessCount("Effetive Start Date Empty");
                        Errors.Add(string.Format("Please enter the Effetive Start Date at row {0}", TextFile.RowIndex));
                        error = true;
                    }

                    string effectiveEndDate = TextFile.GetColValue(BenefitBo.ColumnEffectiveEndDate);
                    if (!string.IsNullOrEmpty(effectiveEndDate))
                    {
                        if (!ValidateDateTimeFormat(BenefitBo.ColumnEffectiveEndDate, ref b))
                        {
                            error = true;
                        }
                    }
                    else
                    {
                        b.EffectiveEndDate = DateTime.Parse(Util.GetDefaultEndDate());
                    }

                    if (!string.IsNullOrEmpty(b.ValuationBenefitCode))
                    {
                        PickListDetailBo rbc = PickListDetailService.FindByPickListIdCode(PickListBo.ValuationBenefitCode, b.ValuationBenefitCode);
                        if (rbc == null)
                        {
                            SetProcessCount("Valuation Benefit Code Not Found");
                            Errors.Add(string.Format("The Valuation Benefit Code doesn't exists: {0} at row {1}", b.ValuationBenefitCode, TextFile.RowIndex));
                            error = true;
                        }
                        else
                        {
                            b.ValuationBenefitCodePickListDetailId = rbc.Id;
                            b.ValuationBenefitCodePickListDetailBo = rbc;
                        }
                    }
                    else
                    {
                        SetProcessCount("Valuation Benefit Code Empty");
                        Errors.Add(string.Format("Please enter the Valuation Benefit Code at row {0}", TextFile.RowIndex));
                        error = true;
                    }

                    if (!string.IsNullOrEmpty(b.BenefitCategoryCode))
                    {
                        PickListDetailBo rbc = PickListDetailService.FindByPickListIdCode(PickListBo.BenefitCategory, b.BenefitCategoryCode);
                        if (rbc == null)
                        {
                            SetProcessCount("Benefit Category Not Found");
                            Errors.Add(string.Format("The Benefit Category doesn't exists: {0} at row {1}", b.BenefitCategoryCode, TextFile.RowIndex));
                            error = true;
                        }
                        else
                        {
                            b.BenefitCategoryPickListDetailId = rbc.Id;
                            b.BenefitCategoryPickListDetailBo = rbc;
                        }
                    }
                    else
                    {
                        SetProcessCount("Benefit Category Empty");
                        Errors.Add(string.Format("Please enter the Benefit Category at row {0}", TextFile.RowIndex));
                        error = true;
                    }

                    var GSTStr = TextFile.GetColValue(BenefitBo.ColumnGST);
                    if (!string.IsNullOrEmpty(GSTStr))
                    {
                        if (Util.StringToBool(GSTStr, out bool p))
                        {
                            b.GST = p;
                        }
                        else
                        {
                            SetProcessCount("GST Invalid Numeric");
                            Errors.Add(string.Format("The GST is not valid (Y/N): {0} at row {1}", GSTStr, TextFile.RowIndex));
                            error = true;
                        }
                    }
                    else
                    {
                        b.GST = false;
                    }
                                        
                    var MLReEventCodeStr = TextFile.GetColValue(BenefitBo.ColumnMLReEventCode);
                    var ClaimCodeStr = TextFile.GetColValue(BenefitBo.ColumnClaimCode);
                    if (!string.IsNullOrEmpty(MLReEventCodeStr) && !string.IsNullOrEmpty(ClaimCodeStr))
                    {
                        bool codeFound = true;
                        bool claimFound = true;
                        List<string> codeExcept = new List<string> { };
                        List<string> claimExcept = new List<string> { };

                        string[] mlreEventCodes = Util.ToArraySplitTrim(MLReEventCodeStr);
                        MLReEventCodes = mlreEventCodes.ToList();
                        string[] claimCodes = Util.ToArraySplitTrim(ClaimCodeStr);
                        ClaimCodes = claimCodes.ToList();
                        if (MLReEventCodes.Count() == ClaimCodes.Count())
                        {
                            foreach (string mlreventCode in MLReEventCodes)
                            {
                                EventCodeBo ec = EventCodeService.FindByCode(mlreventCode);
                                if (ec == null)
                                {
                                    codeExcept.Add(mlreventCode);
                                    codeFound = false;
                                }
                            }

                            foreach (string claimCode in ClaimCodes)
                            {
                                ClaimCodeBo cc = ClaimCodeService.FindByCode(claimCode);
                                if (cc == null)
                                {
                                    claimExcept.Add(claimCode);
                                    claimFound = false;
                                }
                            }

                            if (!codeFound)
                            {
                                SetProcessCount("MLRe Event Code Not Found");
                                Errors.Add(string.Format("The MLRe Event Code doesn't exists: {0} at row {1}", string.Join(",", codeExcept), TextFile.RowIndex));
                                error = true;
                            }

                            if (!claimFound)
                            {
                                SetProcessCount("Claim Code Not Found");
                                Errors.Add(string.Format("The Claim Code doesn't exists: {0} at row {1}", string.Join(",", claimExcept), TextFile.RowIndex));
                                error = true;
                            }
                        }
                        else
                        {
                            SetProcessCount("MLRe Event Code & Claim Code Invalid Length");
                            Errors.Add(string.Format("The MLRe Event Code & Claim Code length is not match at row {0}", TextFile.RowIndex));
                            error = true;
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(MLReEventCodeStr) && !string.IsNullOrEmpty(ClaimCodeStr))
                        {
                            SetProcessCount("MLRe Event Code Empty");
                            Errors.Add(string.Format("Please enter the MLRe Event Code at row {0}", TextFile.RowIndex));
                            error = true;
                        }

                        if (!string.IsNullOrEmpty(MLReEventCodeStr) && string.IsNullOrEmpty(ClaimCodeStr))
                        {
                            SetProcessCount("Claim Code Empty");
                            Errors.Add(string.Format("Please enter the Claim Code at row {0}", TextFile.RowIndex));
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
                string action = TextFile.GetColValue(BenefitBo.ColumnAction);

                if (error)
                {
                    continue;
                }

                switch (action.ToLower().Trim())
                {
                    case "u":
                    case "update":

                        BenefitBo bDb = BenefitService.Find(b.Id);
                        if (bDb == null)
                        {
                            AddNotFoundError(b);
                            continue;
                        }

                        UpdateData(ref bDb, b);

                        trail = new TrailObject();
                        result = BenefitService.Update(ref bDb, ref trail);

                        if (!result.Valid)
                        {
                            SetProcessCount("MLRe Benefit Code Taken");
                            Errors.Add(string.Format("MLRe Benefit Code Taken at row {0}", TextFile.RowIndex));
                            continue;
                        }

                        ProcessBenefitDetails(bDb);
                        Trail(result, bDb, trail, "Update");

                        break;

                    case "d":
                    case "del":
                    case "delete":

                        if (b.Id != 0 && BenefitService.IsExists(b.Id))
                        {
                            trail = new TrailObject();
                            result = BenefitService.Delete(b, ref trail);

                            if (!result.Valid)
                            {
                                SetProcessCount("Benefit Code Been In Used and could not be deleted");
                                Errors.Add(string.Format("Benefit Code In Used at row {0}", TextFile.RowIndex));
                                continue;
                            }

                            Trail(result, b, trail, "Delete");
                        }
                        else
                        {
                            AddNotFoundError(b);
                            continue;
                        }

                        break;

                    default:

                        if (b.Id != 0 && BenefitService.IsExists(b.Id))
                        {
                            SetProcessCount("Record Found");
                            Errors.Add(string.Format("The Benefit ID exists: {0} at row {1}", b.Id, TextFile.RowIndex));
                            continue;
                        }

                        trail = new TrailObject();
                        result = BenefitService.Create(ref b, ref trail);

                        if (!result.Valid)
                        {
                            SetProcessCount("MLRe Benefit Code Taken");
                            Errors.Add(string.Format("MLRe Benefit Code Taken at row {0}", TextFile.RowIndex));
                            continue;
                        }

                        ProcessBenefitDetails(b);
                        Trail(result, b, trail, "Create");

                        break;
                }
            }

            PrintProcessCount();

            TextFile.Close();

            foreach (string error in Errors)
            {
                PrintError(error);
            }
        }

        public BenefitBo SetData()
        {
            var b = new BenefitBo
            {
                Id = 0,
                Type = TextFile.GetColValue(BenefitBo.ColumnBenefitType),
                Code = TextFile.GetColValue(BenefitBo.ColumnCode),
                Description = TextFile.GetColValue(BenefitBo.ColumnDescription),
                StatusStr = TextFile.GetColValue(BenefitBo.ColumnStatus),
                ValuationBenefitCode = TextFile.GetColValue(BenefitBo.ColumnValuationBenefitCode),
                BenefitCategoryCode = TextFile.GetColValue(BenefitBo.ColumnBenefitCategoryCode),
                CreatedById = AuthUserId != null ? AuthUserId.Value : DataAccess.Entities.Identity.User.DefaultSuperUserId,
                UpdatedById = AuthUserId != null ? AuthUserId : DataAccess.Entities.Identity.User.DefaultSuperUserId,
            };

            // Trim string
            b.Type = b.Type?.Trim();
            b.Code = b.Code?.Trim();
            b.Description = b.Description?.Trim();
            b.StatusStr = b.StatusStr?.Trim();
            b.ValuationBenefitCode = b.ValuationBenefitCode?.Trim();
            b.BenefitCategoryCode = b.BenefitCategoryCode?.Trim();

            string idStr = TextFile.GetColValue(BenefitBo.ColumnId);
            if (!string.IsNullOrEmpty(idStr) && int.TryParse(idStr, out int id))
            {
                b.Id = id;
            }

            return b;
        }

        public void UpdateData(ref BenefitBo bDb, BenefitBo b)
        {
            bDb.Type = b.Type;
            bDb.Code = b.Code;
            bDb.Description = b.Description;
            bDb.Status = b.Status;
            bDb.StatusStr = b.StatusStr;
            bDb.UpdatedById = b.UpdatedById;
            bDb.EffectiveStartDate = b.EffectiveStartDate;
            bDb.EffectiveEndDate = b.EffectiveEndDate;
            bDb.ValuationBenefitCodePickListDetailId = b.ValuationBenefitCodePickListDetailId;
            bDb.ValuationBenefitCode = b.ValuationBenefitCode;
            bDb.BenefitCategoryPickListDetailId = b.BenefitCategoryPickListDetailId;
            bDb.BenefitCategoryCode = b.BenefitCategoryCode;
            bDb.GST = b.GST;
        }

        public void Trail(Result result, BenefitBo b, TrailObject trail, string action)
        {
            if (result.Valid)
            {
                UserTrailBo userTrailBo = new UserTrailBo(
                    b.Id,
                    string.Format("{0} Benefit", action),
                    result,
                    trail,
                    b.UpdatedById.Value
                );
                UserTrailService.Create(ref userTrailBo);

                SetProcessCount(action);
            }
        }

        public bool ValidateDateTimeFormat(int type, ref BenefitBo tbcm)
        {
            string header = Columns.Where(m => m.ColIndex == type).Select(m => m.Header).FirstOrDefault();
            string property = Columns.Where(m => m.ColIndex == type).Select(q => q.Property).FirstOrDefault();

            bool valid = true;
            string value = TextFile.GetColValue(type);
            if (!string.IsNullOrEmpty(value))
            {
                if (Util.TryParseDateTime(value, out DateTime? datetime, out string error))
                {
                    tbcm.SetPropertyValue(property, datetime.Value);
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

        public void AddNotFoundError(BenefitBo b)
        {
            SetProcessCount("Record Not Found");
            Errors.Add(string.Format("The Benefit ID doesn't exists: {0} at row {1}", b.Id, TextFile.RowIndex));
        }

        public void ProcessBenefitDetails(BenefitBo b)
        {
            if (MLReEventCodes != null && ClaimCodes != null && MLReEventCodes.Any() && ClaimCodes.Any())
            {
                var combinedBenefitDetails = MLReEventCodes.Zip(ClaimCodes, (a1, a2) => string.Format("{0},{1}", a1, a2));

                BenefitDetailService.DeleteAllByBenefitId(b.Id);
                foreach (string cbs in combinedBenefitDetails)
                {
                    string[] cb = Util.ToArraySplitTrim(cbs);

                    EventCodeBo ec = EventCodeService.FindByCode(cb[0]);
                    ClaimCodeBo cc = ClaimCodeService.FindByCode(cb[1]);

                    var bo = new BenefitDetailBo
                    {
                        EventCodeId = ec.Id,
                        EventCodeBo = ec,
                        ClaimCodeId = cc.Id,
                        ClaimCodeBo = cc,
                        BenefitId = b.Id,
                        BenefitBo = b,
                        CreatedById = AuthUserId != null ? AuthUserId.Value : DataAccess.Entities.Identity.User.DefaultSuperUserId,
                        UpdatedById = AuthUserId != null ? AuthUserId.Value : DataAccess.Entities.Identity.User.DefaultSuperUserId,
                    };
                    BenefitDetailService.Create(bo);
                }
            }
        }

        public List<Column> GetColumns()
        {
            Columns = BenefitBo.GetColumns();
            return Columns;
        }
    }
}
