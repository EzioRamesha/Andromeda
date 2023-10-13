using BusinessObject;
using BusinessObject.Identity;
using DataAccess.Entities.Identity;
using Newtonsoft.Json;
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
    public class ProcessRateTableMapping : Command
    {
        public List<Column> Columns { get; set; }

        public RateTableMappingUploadBo RateTableMappingUploadBo { get; set; }

        public string FilePath { get; set; }

        public HttpPostedFileBase PostedFile { get; set; }

        public int? AuthUserId { get; set; }

        public TextFile TextFile { get; set; }

        public List<string> Errors { get; set; }

        public char[] charsToTrim = { ',', '.', ' ' };

        public ProcessRateTableMapping()
        {
            Title = "ProcessRateTableMapping";
            Description = "To read Rate Table Mapping csv file and insert into database";
            //Arguments = new string[]
            //{
            //    "filePath",
            //};
            Hide = false;
            Errors = new List<string> { };
            GetColumns();
        }

        public override bool Validate()
        {
            //string filepath = CommandInput.Arguments[0];
            //if (!File.Exists(filepath))
            //{
            //    PrintError(string.Format(MessageBag.FileNotExists, filepath));
            //    return false;
            //}
            //else
            //{
            //    FilePath = filepath;
            //}

            return base.Validate();
        }

        public override void Run()
        {
            PrintStarting();

            //Process();
            if (RateTableMappingUploadService.CountByStatus(RateTableMappingUploadBo.StatusPendingProcess) > 0)
            {
                foreach (var bo in RateTableMappingUploadService.GetByStatus(RateTableMappingUploadBo.StatusPendingProcess))
                {
                    FilePath = bo.GetLocalPath();
                    RateTableMappingUploadBo = bo;
                    Errors = new List<string>();
                    Process();

                    if (Errors.Count > 0)
                    {
                        UpdateFileStatus(RateTableMappingUploadBo.StatusFailed, "Process Rate Table Mapping File Failed");
                    }
                    else
                        UpdateFileStatus(RateTableMappingUploadBo.StatusSuccess, "Process Rate Table Mapping File Success");
                }
            }

            PrintEnding();
        }

        public void Process()
        {
            UpdateFileStatus(RateTableMappingUploadBo.StatusProcessing, "Processing Rate Table Mapping File");

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

                RateTableBo rt = null;
                try
                {
                    rt = SetData();

                    if (!string.IsNullOrEmpty(rt.CedantCode))
                    {
                        CedantBo cedantBo = CedantService.FindByCode(rt.CedantCode);
                        if (cedantBo == null)
                        {
                            SetProcessCount("Cedant Code Not Found");
                            Errors.Add(string.Format("The Cedant Code doesn't exists: {0} at row {1}", rt.CedantCode, TextFile.RowIndex));
                            error = true;
                        }
                        else
                        {
                            rt.CedantId = cedantBo.Id;
                            rt.CedantBo = cedantBo;
                        }
                    }
                    else
                    {
                        SetProcessCount("Cedant Code Empty");
                        Errors.Add(string.Format("Please enter the Cedant Code at row {0}", TextFile.RowIndex));
                        error = true;
                    }

                    if (rt.CedantId.HasValue)
                    {
                        if (!string.IsNullOrEmpty(rt.TreatyCode))
                        {
                            string[] treatyCodes = rt.TreatyCode.ToArraySplitTrim();
                            foreach (string treatyCodeStr in treatyCodes)
                            {
                                var treatyCodeBo = TreatyCodeService.FindByCedantIdCode(rt.CedantId.Value, treatyCodeStr);
                                if (treatyCodeBo != null)
                                {
                                    if (treatyCodeBo.Status == TreatyCodeBo.StatusInactive)
                                    {
                                        SetProcessCount("Treaty Code Inactive");
                                        Errors.Add(string.Format("{0}: {1} at row {2}", MessageBag.TreatyCodeStatusInactive, treatyCodeStr, TextFile.RowIndex));
                                        error = true;
                                    }
                                }
                                else
                                {
                                    SetProcessCount("Treaty Code Not Found");
                                    Errors.Add(string.Format("{0} at row {1}", string.Format(MessageBag.TreatyCodeNotFound, treatyCodeStr), TextFile.RowIndex));
                                    error = true;
                                }
                            }
                        }
                        else
                        {
                            SetProcessCount("Treaty Code Empty");
                            Errors.Add(string.Format("Please enter the Treaty Code at row {0}", TextFile.RowIndex));
                            error = true;
                        }
                    }

                    if (string.IsNullOrEmpty(rt.CedingTreatyCode))
                    {
                        rt.CedingTreatyCode = null;
                    }

                    if (!string.IsNullOrEmpty(rt.BenefitCode))
                    {
                        BenefitBo benefitBo = BenefitService.FindByCode(rt.BenefitCode);
                        if (benefitBo == null)
                        {
                            SetProcessCount("Benefit Code Not Found");
                            Errors.Add(string.Format("The Benefit Code doesn't exists: {0} at row {1}", rt.BenefitCode, TextFile.RowIndex));
                            error = true;
                        }
                        else if (benefitBo.Status == BenefitBo.StatusInactive)
                        {
                            SetProcessCount("Benefit Inactive");
                            Errors.Add(string.Format("{0}: {1} at row {2}", MessageBag.BenefitStatusInactive, rt.BenefitCode, TextFile.RowIndex));
                            error = true;
                        }
                        else
                        {
                            rt.BenefitId = benefitBo.Id;
                            rt.BenefitBo = benefitBo;
                        }
                    }
                    else
                    {
                        rt.BenefitId = null;
                    }

                    if (string.IsNullOrEmpty(rt.CedingPlanCode))
                    {
                        rt.CedingPlanCode = null;
                    }

                    if (string.IsNullOrEmpty(rt.CedingPlanCode2))
                    {
                        rt.CedingPlanCode2 = null;
                    }

                    if (!string.IsNullOrEmpty(rt.CedingBenefitTypeCode))
                    {
                        string[] cedingBenefitTypeCodes = rt.CedingBenefitTypeCode.ToArraySplitTrim();
                        foreach (string cedingBenefitTypeCodeStr in cedingBenefitTypeCodes)
                        {
                            PickListDetailBo pickListDetailBo = PickListDetailService.FindByPickListIdCode(PickListBo.CedingBenefitTypeCode, cedingBenefitTypeCodeStr);
                            if (pickListDetailBo == null)
                            {
                                SetProcessCount("Ceding Benefit Type Code Not Found");
                                Errors.Add(string.Format("{0} : {1} at row {2}", "Ceding Benefit Code not exist", cedingBenefitTypeCodeStr, TextFile.RowIndex));
                                error = true;
                            }
                        }
                    }
                    else
                    {
                        rt.CedingBenefitTypeCode = null;
                    }

                    var policyTermFromStr = TextFile.GetColValue(RateTableBo.ColumnPolicyTermFrom);
                    if (!string.IsNullOrEmpty(policyTermFromStr))
                    {
                        if (Util.IsValidDouble(policyTermFromStr, out double? output, out string _))
                        {
                            rt.PolicyTermFrom = output;
                        }
                        else
                        {
                            SetProcessCount("Policy Term From Invalid");
                            Errors.Add(string.Format("The Policy Term From is invalid: {0} at row {1}", policyTermFromStr, TextFile.RowIndex));
                            error = true;
                        }
                    }
                    else
                    {
                        rt.PolicyTermFrom = null;
                    }

                    var policyTermToStr = TextFile.GetColValue(RateTableBo.ColumnPolicyTermTo);
                    if (!string.IsNullOrEmpty(policyTermToStr))
                    {
                        if (Util.IsValidDouble(policyTermToStr, out double? output, out string _))
                        {
                            rt.PolicyTermTo = output;
                        }
                        else
                        {
                            SetProcessCount("Policy Term To Invalid");
                            Errors.Add(string.Format("The Policy Term To is invalid {0} at row {1}", policyTermToStr, TextFile.RowIndex));
                            error = true;
                        }
                    }
                    else
                    {
                        rt.PolicyTermTo = null;
                    }

                    var policyDurationFromStr = TextFile.GetColValue(RateTableBo.ColumnPolicyDurationFrom);
                    if (!string.IsNullOrEmpty(policyDurationFromStr))
                    {
                        if (Util.IsValidDouble(policyDurationFromStr, out double? output, out string _))
                        {
                            rt.PolicyDurationFrom = output;
                        }
                        else
                        {
                            SetProcessCount("Policy Duration From Invalid");
                            Errors.Add(string.Format("The Policy Duration From is invalid: {0} at row {1}", policyDurationFromStr, TextFile.RowIndex));
                            error = true;
                        }
                    }
                    else
                    {
                        rt.PolicyDurationFrom = null;
                    }

                    var policyDurationToStr = TextFile.GetColValue(RateTableBo.ColumnPolicyDurationTo);
                    if (!string.IsNullOrEmpty(policyDurationToStr))
                    {
                        if (Util.IsValidDouble(policyDurationToStr, out double? output, out string _))
                        {
                            rt.PolicyDurationTo = output;
                        }
                        else
                        {
                            SetProcessCount("Policy Duration To Invalid");
                            Errors.Add(string.Format("The Policy Duration To is invalid: {0} at row {1}", policyDurationToStr, TextFile.RowIndex));
                            error = true;
                        }
                    }
                    else
                    {
                        rt.PolicyDurationTo = null;
                    }

                    if (string.IsNullOrEmpty(rt.CedingBenefitRiskCode))
                    {
                        rt.CedingBenefitRiskCode = null;
                    }

                    if (string.IsNullOrEmpty(rt.GroupPolicyNumber))
                    {
                        rt.GroupPolicyNumber = null;
                    }

                    if (!string.IsNullOrEmpty(rt.PremiumFrequencyCode))
                    {
                        PickListDetailBo pfc = PickListDetailService.FindByPickListIdCode(PickListBo.PremiumFrequencyCode, rt.PremiumFrequencyCode);
                        if (pfc == null)
                        {
                            SetProcessCount("Premium Frequency Code Not Found");
                            Errors.Add(string.Format("The Premium Frequency Code doesn't exists: {0} at row {1}", rt.PremiumFrequencyCode, TextFile.RowIndex));
                            error = true;
                        }
                        else
                        {
                            rt.PremiumFrequencyCodePickListDetailId = pfc.Id;
                            rt.PremiumFrequencyCodePickListDetailBo = pfc;
                        }
                    }
                    else
                    {
                        rt.PremiumFrequencyCodePickListDetailId = null;
                    }

                    if (!string.IsNullOrEmpty(rt.ReinsBasisCode))
                    {
                        PickListDetailBo rbc = PickListDetailService.FindByPickListIdCode(PickListBo.ReinsBasisCode, rt.ReinsBasisCode);
                        if (rbc == null)
                        {
                            SetProcessCount("Reinsurance Basis Code Not Found");
                            Errors.Add(string.Format("The Reinsurance Basis Code doesn't exists: {0} at row {1}", rt.ReinsBasisCode, TextFile.RowIndex));
                            error = true;
                        }
                        else
                        {
                            rt.ReinsBasisCodePickListDetailId = rbc.Id;
                            rt.ReinsBasisCodePickListDetailBo = rbc;
                        }
                    }
                    else
                    {
                        rt.ReinsBasisCodePickListDetailId = null;
                    }

                    var attainedAgeFromStr = TextFile.GetColValue(RateTableBo.ColumnAttainedAgeFrom);
                    if (!string.IsNullOrEmpty(attainedAgeFromStr))
                    {
                        if (int.TryParse(attainedAgeFromStr, out int attainedAgeFrom))
                        {
                            rt.AttainedAgeFrom = attainedAgeFrom;
                        }
                        else
                        {
                            SetProcessCount("Attained Age From Invalid Numeric");
                            Errors.Add(string.Format("The Attained Age From is not a numeric: {0} at row {1}", attainedAgeFromStr, TextFile.RowIndex));
                            error = true;
                        }
                    }
                    else
                    {
                        rt.AttainedAgeFrom = null;
                    }

                    var attainedAgeToStr = TextFile.GetColValue(RateTableBo.ColumnAttainedAgeTo);
                    if (!string.IsNullOrEmpty(attainedAgeToStr))
                    {
                        if (int.TryParse(attainedAgeToStr, out int attainedAgeTo))
                        {
                            rt.AttainedAgeTo = attainedAgeTo;
                        }
                        else
                        {
                            SetProcessCount("Attained Age To Invalid Numeric");
                            Errors.Add(string.Format("The Attained Age To is not a numeric: {0} at row {1}", attainedAgeToStr, TextFile.RowIndex));
                            error = true;
                        }
                    }
                    else
                    {
                        rt.AttainedAgeTo = null;
                    }

                    var reinsEffDatePolStartDate = TextFile.GetColValue(RateTableBo.ColumnReinsEffDatePolStartDate);
                    if (!string.IsNullOrEmpty(reinsEffDatePolStartDate))
                    {
                        if (!ValidateDateTimeFormat(RateTableBo.ColumnReinsEffDatePolStartDate, ref rt))
                        {
                            error = true;
                        }
                    }
                    else
                    {
                        rt.ReinsEffDatePolStartDate = null;
                    }

                    var reinsEffDatePolEndDate = TextFile.GetColValue(RateTableBo.ColumnReinsEffDatePolEndDate);
                    if (!string.IsNullOrEmpty(reinsEffDatePolEndDate))
                    {
                        if (!ValidateDateTimeFormat(RateTableBo.ColumnReinsEffDatePolEndDate, ref rt))
                        {
                            error = true;
                        }
                    }
                    else if (rt.ReinsEffDatePolStartDate != null)
                    {
                        rt.ReinsEffDatePolEndDate = DateTime.Parse(Util.GetDefaultEndDate());
                    }
                    else
                    {
                        rt.ReinsEffDatePolEndDate = null;
                    }

                    var reportingStartDate = TextFile.GetColValue(RateTableBo.ColumnReportingStartDate);
                    if (!string.IsNullOrEmpty(reportingStartDate))
                    {
                        if (!ValidateDateTimeFormat(RateTableBo.ColumnReportingStartDate, ref rt))
                        {
                            error = true;
                        }
                    }
                    else
                    {
                        rt.ReportingStartDate = null;
                    }

                    var reportingEndDate = TextFile.GetColValue(RateTableBo.ColumnReportingEndDate);
                    if (!string.IsNullOrEmpty(reportingEndDate))
                    {
                        if (!ValidateDateTimeFormat(RateTableBo.ColumnReportingEndDate, ref rt))
                        {
                            error = true;
                        }
                    }
                    else if (rt.ReportingStartDate != null)
                    {
                        rt.ReportingEndDate = DateTime.Parse(Util.GetDefaultEndDate());
                    }
                    else
                    {
                        rt.ReportingEndDate = null;
                    }

                    var policyAmountFromStr = TextFile.GetColValue(RateTableBo.ColumnPolicyAmountFrom);
                    if (!string.IsNullOrEmpty(policyAmountFromStr))
                    {
                        if (Util.IsValidDouble(policyAmountFromStr, out double? output, out string _))
                        {
                            rt.PolicyAmountFrom = output.Value;
                        }
                        else
                        {
                            SetProcessCount("ORI Sum Assured From Invalid");
                            Errors.Add(string.Format("The ORI Sum Assured From is invalid: {0} at row {1}", policyAmountFromStr, TextFile.RowIndex));
                            error = true;
                        }
                    }
                    else
                    {
                        rt.PolicyAmountFrom = null;
                    }

                    var policyAmountToStr = TextFile.GetColValue(RateTableBo.ColumnPolicyAmountTo);
                    if (!string.IsNullOrEmpty(policyAmountToStr))
                    {
                        if (Util.IsValidDouble(policyAmountToStr, out double? output, out string _))
                        {
                            rt.PolicyAmountTo = output.Value;
                        }
                        else
                        {
                            SetProcessCount("ORI Sum Assured To Invalid");
                            Errors.Add(string.Format("The ORI Sum Assured To is invalid: {0} at row {1}", policyAmountToStr, TextFile.RowIndex));
                            error = true;
                        }
                    }
                    else
                    {
                        rt.PolicyAmountTo = null;
                    }

                    var aarFromStr = TextFile.GetColValue(RateTableBo.ColumnAarFrom);
                    if (!string.IsNullOrEmpty(aarFromStr))
                    {
                        if (Util.IsValidDouble(aarFromStr, out double? output, out string _))
                        {
                            rt.AarFrom = output.Value;
                        }
                        else
                        {
                            SetProcessCount("AAR From Invalid");
                            Errors.Add(string.Format("The AAR From is invalid: {0} at row {1}", aarFromStr, TextFile.RowIndex));
                            error = true;
                        }
                    }
                    else
                    {
                        rt.AarFrom = null;
                    }

                    var aarToStr = TextFile.GetColValue(RateTableBo.ColumnAarTo);
                    if (!string.IsNullOrEmpty(aarToStr))
                    {
                        if (Util.IsValidDouble(aarToStr, out double? output, out string _))
                        {
                            rt.AarTo = output.Value;
                        }
                        else
                        {
                            SetProcessCount("AAR To Invalid");
                            Errors.Add(string.Format("The AAR To is invalid: {0} at row {1}", aarToStr, TextFile.RowIndex));
                            error = true;
                        }
                    }
                    else
                    {
                        rt.AarTo = null;
                    }

                    if (!string.IsNullOrEmpty(rt.RateCode))
                    {
                        RateBo rateBo = RateService.FindByCode(rt.RateCode);
                        if (rateBo == null)
                        {
                            SetProcessCount("Rate Table Code Not Found");
                            Errors.Add(string.Format("The Rate Table Code doesn't exists: {0} at row {1}", rt.RateCode, TextFile.RowIndex));
                            error = true;
                        }
                        else
                        {
                            rt.RateId = rateBo.Id;
                            rt.RateBo = rateBo;
                        }
                    }
                    else
                    {
                        rt.RateId = null;
                    }

                    if (!string.IsNullOrEmpty(rt.RiDiscountCode) && rt.CedantId.HasValue)
                    {
                        RiDiscountBo riDiscountBo = RiDiscountService.FindByDiscountCodeCedantId(rt.RiDiscountCode, rt.CedantId.Value);
                        if (riDiscountBo == null)
                        {
                            SetProcessCount("RI Discount Code Not Found");
                            Errors.Add(string.Format("The RI Discount Code doesn't exists: {0} for Cedant: {1} at row {2}", rt.RiDiscountCode, rt.CedantBo?.Code, TextFile.RowIndex));
                            error = true;
                        }
                    }
                    else
                    {
                        rt.RiDiscountCode = null;
                    }

                    if (!string.IsNullOrEmpty(rt.LargeDiscountCode) && rt.CedantId.HasValue)
                    {
                        LargeDiscountBo largeDiscountBo = LargeDiscountService.FindByDiscountCodeCedantId(rt.LargeDiscountCode, rt.CedantId.Value);
                        if (largeDiscountBo == null)
                        {
                            SetProcessCount("Large Discount Code Not Found");
                            Errors.Add(string.Format("The Large Discount Code doesn't exists: {0} for Cedant: {1} at row {2}", rt.LargeDiscountCode, rt.CedantBo?.Code, TextFile.RowIndex));
                            error = true;
                        }
                    }
                    else
                    {
                        rt.LargeDiscountCode = null;
                    }

                    if (!string.IsNullOrEmpty(rt.GroupDiscountCode) && rt.CedantId.HasValue)
                    {
                        GroupDiscountBo groupDiscountBo = GroupDiscountService.FindByDiscountCodeCedantId(rt.GroupDiscountCode, rt.CedantId.Value);
                        if (groupDiscountBo == null)
                        {
                            SetProcessCount("Group Discount Code Not Found");
                            Errors.Add(string.Format("The Group Discount Code doesn't exists: {0} for Cedant: {1} at row {2}", rt.GroupDiscountCode, rt.CedantBo?.Code, TextFile.RowIndex));
                            error = true;
                        }
                    }
                    else
                    {
                        rt.GroupDiscountCode = null;
                    }
                }
                catch (Exception e)
                {
                    SetProcessCount("Error");
                    Errors.Add(string.Format("Error Triggered: {0} at row {1}", e.Message, TextFile.RowIndex));
                    error = true;
                }
                string action = TextFile.GetColValue(RateTableBo.ColumnAction);

                if (error)
                {
                    continue;
                }

                switch (action.ToLower().Trim())
                {
                    case "u":
                    case "update":

                        RateTableBo rtDb = RateTableService.Find(rt.Id);
                        if (rtDb == null)
                        {
                            AddNotFoundError(rt);
                            continue;
                        }

                        var rangeResult = RateTableService.ValidateRange(rt);
                        var mappingResult = RateTableService.ValidateMapping(rt);
                        var mappedValueResult = RateTableService.ValidateMappedValue(rt);
                        if (!rangeResult.Valid || !mappingResult.Valid || !mappedValueResult.Valid)
                        {
                            foreach (var e in rangeResult.ToErrorArray())
                            {
                                Errors.Add(string.Format("{0} at row {1}", e, TextFile.RowIndex));
                            }
                            foreach (var e in mappingResult.ToErrorArray())
                            {
                                Errors.Add(string.Format("{0} at row {1}", e, TextFile.RowIndex));
                            }
                            foreach (var e in mappedValueResult.ToErrorArray())
                            {
                                Errors.Add(string.Format("{0} at row {1}", e, TextFile.RowIndex));
                            }
                            error = true;
                        }
                        if (error)
                        {
                            continue;
                        }

                        UpdateData(ref rtDb, rt);

                        trail = new TrailObject();
                        result = RateTableService.Update(ref rtDb, ref trail);

                        RateTableService.ProcessMappingDetail(rt, rt.CreatedById); // DO NOT TRAIL
                        Trail(result, rtDb, trail, "Update");

                        break;

                    case "d":
                    case "del":
                    case "delete":

                        if (rt.Id != 0 && RateTableService.IsExists(rt.Id))
                        {
                            trail = new TrailObject();
                            result = RateTableService.Delete(rt, ref trail);
                            Trail(result, rt, trail, "Delete");
                        }
                        else
                        {
                            AddNotFoundError(rt);
                            continue;
                        }

                        break;

                    default:

                        if (rt.Id != 0 && RateTableService.IsExists(rt.Id))
                        {
                            SetProcessCount("Record Found");
                            Errors.Add(string.Format("The Rate Table Mapping ID exists: {0} at row {1}", rt.Id, TextFile.RowIndex));
                            continue;
                        }


                        rangeResult = RateTableService.ValidateRange(rt);
                        mappingResult = RateTableService.ValidateMapping(rt);
                        mappedValueResult = RateTableService.ValidateMappedValue(rt);
                        if (!rangeResult.Valid || !mappingResult.Valid || !mappedValueResult.Valid)
                        {
                            foreach (var e in rangeResult.ToErrorArray())
                            {
                                Errors.Add(string.Format("{0} at row {1}", e, TextFile.RowIndex));
                            }
                            foreach (var e in mappingResult.ToErrorArray())
                            {
                                Errors.Add(string.Format("{0} at row {1}", e, TextFile.RowIndex));
                            }
                            foreach (var e in mappedValueResult.ToErrorArray())
                            {
                                Errors.Add(string.Format("{0} at row {1}", e, TextFile.RowIndex));
                            }
                            error = true;
                        }
                        if (error)
                        {
                            continue;
                        }

                        trail = new TrailObject();
                        result = RateTableService.Create(ref rt, ref trail);

                        RateTableService.ProcessMappingDetail(rt, rt.CreatedById); // DO NOT TRAIL
                        Trail(result, rt, trail, "Create");

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

        public RateTableBo SetData()
        {
            var rt = new RateTableBo
            {
                Id = 0,
                TreatyCode = TextFile.GetColValue(RateTableBo.ColumnTreatyCode),
                CedingTreatyCode = TextFile.GetColValue(RateTableBo.ColumnCedingTreatyCode),
                BenefitCode = TextFile.GetColValue(RateTableBo.ColumnBenefitCode),
                CedingPlanCode = TextFile.GetColValue(RateTableBo.ColumnCedingPlanCode),
                CedingPlanCode2 = TextFile.GetColValue(RateTableBo.ColumnCedingPlanCode2),
                CedingBenefitTypeCode = TextFile.GetColValue(RateTableBo.ColumnCedingBenefitTypeCode),
                CedingBenefitRiskCode = TextFile.GetColValue(RateTableBo.ColumnCedingBenefitRiskCode),
                GroupPolicyNumber = TextFile.GetColValue(RateTableBo.ColumnGroupPolicyNumber),
                PremiumFrequencyCode = TextFile.GetColValue(RateTableBo.ColumnPremiumFrequencyCode),
                ReinsBasisCode = TextFile.GetColValue(RateTableBo.ColumnReinsBasisCode),
                RateCode = TextFile.GetColValue(RateTableBo.ColumnRateCode),
                CedantCode = TextFile.GetColValue(RateTableBo.ColumnCedantCode),
                RiDiscountCode = TextFile.GetColValue(RateTableBo.ColumnRiDiscountCode),
                LargeDiscountCode = TextFile.GetColValue(RateTableBo.ColumnLargeDiscountCode),
                GroupDiscountCode = TextFile.GetColValue(RateTableBo.ColumnGroupDiscountCode),
                CreatedById = AuthUserId != null ? AuthUserId.Value : DataAccess.Entities.Identity.User.DefaultSuperUserId,
                UpdatedById = AuthUserId != null ? AuthUserId : DataAccess.Entities.Identity.User.DefaultSuperUserId,
            };

            rt.TreatyCode = rt.TreatyCode?.TrimEnd(charsToTrim);
            rt.CedingTreatyCode = rt.CedingTreatyCode?.TrimEnd(charsToTrim);
            rt.CedingPlanCode = rt.CedingPlanCode?.TrimEnd(charsToTrim);
            rt.CedingPlanCode2 = rt.CedingPlanCode2?.TrimEnd(charsToTrim);
            rt.CedingBenefitTypeCode = rt.CedingBenefitTypeCode?.TrimEnd(charsToTrim);
            rt.CedingBenefitRiskCode = rt.CedingBenefitRiskCode?.TrimEnd(charsToTrim);
            rt.GroupPolicyNumber = rt.GroupPolicyNumber?.TrimEnd(charsToTrim);

            rt.RiDiscountCode = rt.RiDiscountCode?.Trim();
            rt.LargeDiscountCode = rt.LargeDiscountCode?.Trim();
            rt.GroupDiscountCode = rt.GroupDiscountCode?.Trim();

            string idStr = TextFile.GetColValue(RateTableBo.ColumnId);
            if (!string.IsNullOrEmpty(idStr) && int.TryParse(idStr, out int id))
            {
                rt.Id = id;
            }

            return rt;
        }

        public void UpdateData(ref RateTableBo rtDb, RateTableBo rt)
        {
            rtDb.TreatyCode = rt.TreatyCode;
            rtDb.CedingTreatyCode = rt.CedingTreatyCode;
            rtDb.BenefitId = rt.BenefitId;
            rtDb.CedingPlanCode = rt.CedingPlanCode;
            rtDb.CedingPlanCode2 = rt.CedingPlanCode2;
            rtDb.CedingBenefitTypeCode = rt.CedingBenefitTypeCode;
            rtDb.CedingBenefitRiskCode = rt.CedingBenefitRiskCode;
            rtDb.GroupPolicyNumber = rt.GroupPolicyNumber;
            rtDb.PremiumFrequencyCodePickListDetailId = rt.PremiumFrequencyCodePickListDetailId;
            rtDb.ReinsBasisCodePickListDetailId = rt.ReinsBasisCodePickListDetailId;
            rtDb.PolicyAmountFrom = rt.PolicyAmountFrom;
            rtDb.PolicyAmountTo = rt.PolicyAmountTo;
            rtDb.AttainedAgeFrom = rt.AttainedAgeFrom;
            rtDb.AttainedAgeTo = rt.AttainedAgeTo;
            rtDb.AarFrom = rt.AarFrom;
            rtDb.AarTo = rt.AarTo;
            rtDb.ReinsEffDatePolStartDate = rt.ReinsEffDatePolStartDate;
            rtDb.ReinsEffDatePolEndDate = rt.ReinsEffDatePolEndDate;
            rtDb.UpdatedById = rt.UpdatedById;

            // Phase 2
            rtDb.PolicyTermFrom = rt.PolicyTermFrom;
            rtDb.PolicyTermTo = rt.PolicyTermTo;
            rtDb.PolicyDurationFrom = rt.PolicyDurationFrom;
            rtDb.PolicyDurationTo = rt.PolicyDurationTo;
            rtDb.RateId = rt.RateId;
            rtDb.CedantId = rt.CedantId;
            rtDb.RiDiscountCode = rt.RiDiscountCode;
            rtDb.LargeDiscountCode = rt.LargeDiscountCode;
            rtDb.GroupDiscountCode = rt.GroupDiscountCode;

            rtDb.ReportingStartDate = rt.ReportingStartDate;
            rtDb.ReportingEndDate = rt.ReportingEndDate;
        }

        public void Trail(Result result, RateTableBo rt, TrailObject trail, string action)
        {
            if (result.Valid)
            {
                UserTrailBo userTrailBo = new UserTrailBo(
                    rt.Id,
                    string.Format("{0} Rate Table Mapping", action),
                    result,
                    trail,
                    rt.UpdatedById.Value
                );
                UserTrailService.Create(ref userTrailBo);

                SetProcessCount(action);
            }
        }

        public bool ValidateDateTimeFormat(int type, ref RateTableBo rt)
        {
            string header = Columns.Where(m => m.ColIndex == type).Select(m => m.Header).FirstOrDefault();
            string property = Columns.Where(m => m.ColIndex == type).Select(m => m.Property).FirstOrDefault();

            bool valid = true;
            string value = TextFile.GetColValue(type);
            if (!string.IsNullOrEmpty(value))
            {
                if (Util.TryParseDateTime(value, out DateTime? datetime, out string error))
                {
                    rt.SetPropertyValue(property, datetime.Value);
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

        public void AddNotFoundError(RateTableBo cm)
        {
            SetProcessCount("Record Not Found");
            Errors.Add(string.Format("The Rate Table ID doesn't exists: {0} at row {1}", cm.Id, TextFile.RowIndex));
        }

        public List<Column> GetColumns()
        {
            Columns = RateTableBo.GetColumns();
            return Columns;
        }

        public void UpdateFileStatus(int status, string description)
        {
            var rateTableMappingUploadBo = RateTableMappingUploadBo;
            rateTableMappingUploadBo.Status = status;

            if (Errors.Count > 0)
            {
                rateTableMappingUploadBo.Errors = JsonConvert.SerializeObject(Errors);
            }

            var trail = new TrailObject();
            var result = RateTableMappingUploadService.Update(ref rateTableMappingUploadBo, ref trail);

            var userTrailBo = new UserTrailBo(
                rateTableMappingUploadBo.Id,
                description,
                result,
                trail,
                User.DefaultSuperUserId
            );
            UserTrailService.Create(ref userTrailBo);
        }
    }
}
