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
    public class ProcessProductFeatureMapping : Command
    {
        public List<Column> Columns { get; set; }

        public TreatyBenefitCodeMappingUploadBo TreatyBenefitCodeMappingUploadBo { get; set; }

        public string FilePath { get; set; }

        public HttpPostedFileBase PostedFile { get; set; }

        public int? AuthUserId { get; set; }

        public TextFile TextFile { get; set; }

        public List<string> Errors { get; set; }

        public char[] charsToTrim = { ',', '.', ' ' };

        public ProcessProductFeatureMapping()
        {
            Title = "ProcessProductFeatureMapping"; //+ DateTime.Now.Minute + DateTime.Now.Second;
            Description = "To read Product Feature Mapping csv file and insert into database";
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
            if (TreatyBenefitCodeMappingUploadService.CountByStatus(TreatyBenefitCodeMappingUploadBo.StatusPendingProcess) > 0)
            {
                foreach (var bo in TreatyBenefitCodeMappingUploadService.GetByStatus(TreatyBenefitCodeMappingUploadBo.StatusPendingProcess))
                {
                    FilePath = bo.GetLocalPath();
                    TreatyBenefitCodeMappingUploadBo = bo;
                    Errors = new List<string>();
                    Process();

                    if (Errors.Count > 0)
                    {
                        UpdateFileStatus(TreatyBenefitCodeMappingUploadBo.StatusFailed, "Process Product Feature Mapping File Failed");
                    }
                    else
                        UpdateFileStatus(TreatyBenefitCodeMappingUploadBo.StatusSuccess, "Process Product Feature Mapping File Success");
                }
            }

            PrintEnding();
        }

        public void Process()
        {
            UpdateFileStatus(TreatyBenefitCodeMappingUploadBo.StatusProcessing, "Processing Product Feature Mapping File");

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

                TreatyBenefitCodeMappingBo tbcm = null;
                try
                {
                    tbcm = SetData();

                    #region Validation
                    if (!string.IsNullOrEmpty(tbcm.CedantCode))
                    {
                        CedantBo cedantBo = CedantService.FindByCode(tbcm.CedantCode);
                        if (cedantBo == null)
                        {
                            SetProcessCount("Cedant Code Not Found");
                            Errors.Add(string.Format("The Cedant Code doesn't exists: {0} at row {1}", tbcm.CedantCode, TextFile.RowIndex));
                            error = true;
                        }
                        else if (cedantBo.Status == CedantBo.StatusInactive)
                        {
                            SetProcessCount("Cedant Inactive");
                            Errors.Add(string.Format("{0}: {1} at row {2}", MessageBag.CedantStatusInactive, tbcm.CedantCode, TextFile.RowIndex));
                            error = true;
                        }
                        else
                        {
                            tbcm.CedantId = cedantBo.Id;
                            tbcm.CedantBo = cedantBo;
                        }
                    }
                    else
                    {
                        SetProcessCount("Cedant Code Empty");
                        Errors.Add(string.Format("Please enter the Cedant Code at row {0}", TextFile.RowIndex));
                        error = true;
                    }

                    if (string.IsNullOrEmpty(tbcm.CedingPlanCode))
                    {
                        SetProcessCount("Ceding Plan Code Empty");
                        Errors.Add(string.Format("Please enter the Ceding Plan Code at row {0}", TextFile.RowIndex));
                        error = true;
                    }

                    if (string.IsNullOrEmpty(tbcm.Description))
                    {
                        tbcm.Description = null;
                    }

                    if (!string.IsNullOrEmpty(tbcm.CedingBenefitTypeCode))
                    {
                        string[] cedingBenefitTypeCodes = tbcm.CedingBenefitTypeCode.ToArraySplitTrim();
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
                        SetProcessCount("Ceding Benefit Type Code Empty");
                        Errors.Add(string.Format("Please enter the Ceding Benefit Type Code at row {0}", TextFile.RowIndex));
                        error = true;
                    }

                    if (string.IsNullOrEmpty(tbcm.CedingBenefitRiskCode))
                    {
                        tbcm.CedingBenefitRiskCode = null;
                    }

                    if (string.IsNullOrEmpty(tbcm.CedingTreatyCode))
                    {
                        tbcm.CedingTreatyCode = null;
                    }

                    if (string.IsNullOrEmpty(tbcm.CampaignCode))
                    {
                        tbcm.CampaignCode = null;
                    }

                    var reinsEffDatePolStartDate = TextFile.GetColValue(TreatyBenefitCodeMappingBo.ColumnReinsEffDatePolStartDate);
                    if (!string.IsNullOrEmpty(reinsEffDatePolStartDate))
                    {
                        if (!ValidateDateTimeFormat(TreatyBenefitCodeMappingBo.ColumnReinsEffDatePolStartDate, ref tbcm))
                        {
                            error = true;
                        }
                    }
                    else
                    {
                        SetProcessCount("Reins Eff Date Pol Start Date Empty");
                        Errors.Add(string.Format("Please enter the Reins Eff Date Pol Start Date at row {0}", TextFile.RowIndex));
                        error = true;
                    }

                    string reinsEffDatePolEndDate = TextFile.GetColValue(TreatyBenefitCodeMappingBo.ColumnReinsEffDatePolEndDate);
                    if (!string.IsNullOrEmpty(reinsEffDatePolEndDate))
                    {
                        if (!ValidateDateTimeFormat(TreatyBenefitCodeMappingBo.ColumnReinsEffDatePolEndDate, ref tbcm))
                        {
                            error = true;
                        }
                    }
                    else
                    {
                        tbcm.ReinsEffDatePolEndDate = DateTime.Parse(Util.GetDefaultEndDate());
                    }

                    if (!string.IsNullOrEmpty(tbcm.ReinsBasisCode))
                    {
                        PickListDetailBo rbc = PickListDetailService.FindByPickListIdCode(PickListBo.ReinsBasisCode, tbcm.ReinsBasisCode);
                        if (rbc == null)
                        {
                            SetProcessCount("Reinsurance Basic Code Not Found");
                            Errors.Add(string.Format("The Reinsurance Basic Code doesn't exists: {0} at row {1}", tbcm.ReinsBasisCode, TextFile.RowIndex));
                            error = true;
                        }
                        else
                        {
                            tbcm.ReinsBasisCodePickListDetailId = rbc.Id;
                            tbcm.ReinsBasisCodePickListDetailBo = rbc;
                        }
                    }
                    else
                    {
                        SetProcessCount("Reinsurance Basic Code Empty");
                        Errors.Add(string.Format("Please enter the Reinsurance Basic Code at row {0}", TextFile.RowIndex));
                        error = true;
                    }

                    var attainedAgeFromStr = TextFile.GetColValue(TreatyBenefitCodeMappingBo.ColumnAttainedAgeFrom);
                    if (!string.IsNullOrEmpty(attainedAgeFromStr))
                    {
                        if (int.TryParse(attainedAgeFromStr, out int attainedAgeFrom))
                        {
                            tbcm.AttainedAgeFrom = attainedAgeFrom;
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
                        tbcm.AttainedAgeFrom = null;
                    }

                    var attainedAgeToStr = TextFile.GetColValue(TreatyBenefitCodeMappingBo.ColumnAttainedAgeTo);
                    if (!string.IsNullOrEmpty(attainedAgeToStr))
                    {
                        if (int.TryParse(attainedAgeToStr, out int attainedAgeTo))
                        {
                            tbcm.AttainedAgeTo = attainedAgeTo;
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
                        tbcm.AttainedAgeTo = null;
                    }

                    var reportingStartDateStr = TextFile.GetColValue(TreatyBenefitCodeMappingBo.ColumnReportingStartDate);
                    if (!string.IsNullOrEmpty(reportingStartDateStr))
                    {
                        if (!ValidateDateTimeFormat(TreatyBenefitCodeMappingBo.ColumnReportingStartDate, ref tbcm))
                        {
                            error = true;
                        }
                    }
                    else
                    {
                        tbcm.ReportingStartDate = null;
                    }

                    var reportingEndDateStr = TextFile.GetColValue(TreatyBenefitCodeMappingBo.ColumnReportingEndDate);
                    if (!string.IsNullOrEmpty(reportingEndDateStr))
                    {
                        if (!ValidateDateTimeFormat(TreatyBenefitCodeMappingBo.ColumnReportingEndDate, ref tbcm))
                        {
                            error = true;
                        }
                    }
                    else if (tbcm.ReportingStartDate != null)
                    {
                        tbcm.ReportingEndDate = DateTime.Parse(Util.GetDefaultEndDate());
                    }
                    else
                    {
                        tbcm.ReportingEndDate = null;
                    }

                    if (!string.IsNullOrEmpty(tbcm.TreatyCode))
                    {
                        if (tbcm.CedantBo == null)
                        {
                            SetProcessCount("Cedant Code is Required to validate Treaty Code");
                            Errors.Add(string.Format("Cedant Code is Required to validate Treaty Code at row {0}", TextFile.RowIndex));
                            error = true;
                        }
                        else
                        {
                            TreatyCodeBo treatyCodeBo = TreatyCodeService.FindByCode(tbcm.TreatyCode);
                            if (treatyCodeBo == null)
                            {
                                SetProcessCount("Treaty Code Not Found");
                                Errors.Add(string.Format("The Treaty Code doesn't exists: {0} at row {1}", tbcm.TreatyCode, TextFile.RowIndex));
                                error = true;
                            }
                            else if (treatyCodeBo.TreatyBo.CedantId != tbcm.CedantBo.Id)
                            {
                                SetProcessCount("Treaty Code Not Matched");
                                Errors.Add(string.Format("{0} at row {1}", string.Format(MessageBag.TreatyCodeNotBelongsToCedant, tbcm.TreatyCode, tbcm.CedantBo.Code), TextFile.RowIndex));
                                error = true;
                            }
                            else if (treatyCodeBo.Status == TreatyCodeBo.StatusInactive)
                            {
                                SetProcessCount("Treaty Code Inactive");
                                Errors.Add(string.Format("{0}: {1} at row {2}", MessageBag.TreatyCodeStatusInactive, tbcm.TreatyCode, TextFile.RowIndex));
                                error = true;
                            }
                            else
                            {
                                tbcm.TreatyCodeId = treatyCodeBo.Id;
                                tbcm.TreatyCodeBo = treatyCodeBo;
                            }
                        }
                    }
                    else
                    {
                        SetProcessCount("Treaty Code Empty");
                        Errors.Add(string.Format("Please enter the Treaty Code at row {0}", TextFile.RowIndex));
                        error = true;
                    }

                    if (!string.IsNullOrEmpty(tbcm.BenefitCode))
                    {
                        BenefitBo benefitBo = BenefitService.FindByCode(tbcm.BenefitCode);
                        if (benefitBo == null)
                        {
                            SetProcessCount("Benefit Code Not Found");
                            Errors.Add(string.Format("The Benefit Code doesn't exists: {0} at row {1}", tbcm.BenefitCode, TextFile.RowIndex));
                            error = true;
                        }
                        else if (benefitBo.Status == BenefitBo.StatusInactive)
                        {
                            SetProcessCount("Benefit Inactive");
                            Errors.Add(string.Format("{0}: {1} at row {2}", MessageBag.BenefitStatusInactive, tbcm.TreatyCode, TextFile.RowIndex));
                            error = true;
                        }
                        else
                        {
                            tbcm.BenefitId = benefitBo.Id;
                            tbcm.BenefitBo = benefitBo;
                        }
                    }
                    else
                    {
                        SetProcessCount("Benefit Code Empty");
                        Errors.Add(string.Format("Please enter the Benefit Code at row {0}", TextFile.RowIndex));
                        error = true;
                    }

                    if (!string.IsNullOrEmpty(tbcm.ProfitComm))
                    {
                        PickListDetailBo pc = PickListDetailService.FindByPickListIdCode(PickListBo.ProfitComm, tbcm.ProfitComm);
                        if (pc == null)
                        {
                            SetProcessCount("Profit Commission Not Found");
                            Errors.Add(string.Format("The Profit Commission doesn't exists: {0} at row {1}", tbcm.ProfitComm, TextFile.RowIndex));
                            error = true;
                        }
                        else
                        {
                            tbcm.ProfitCommPickListDetailId = pc.Id;
                            tbcm.ProfitCommPickListDetailBo = pc;
                        }
                    }

                    var maxExpiryAgeStr = TextFile.GetColValue(TreatyBenefitCodeMappingBo.ColumnMaxExpiryAge);
                    if (!string.IsNullOrEmpty(maxExpiryAgeStr))
                    {
                        if (int.TryParse(maxExpiryAgeStr, out int maxExpiryAge))
                        {
                            tbcm.MaxExpiryAge = maxExpiryAge;
                        }
                        else
                        {
                            SetProcessCount("Maximum Age at Expiry Invalid Numeric");
                            Errors.Add(string.Format("The Maximum Age at Expiry is not a numeric: {0} at row {1}", maxExpiryAgeStr, TextFile.RowIndex));
                            error = true;
                        }
                    }
                    else
                    {
                        tbcm.MaxExpiryAge = null;
                    }

                    var minIssueAgeStr = TextFile.GetColValue(TreatyBenefitCodeMappingBo.ColumnMinIssueAge);
                    if (!string.IsNullOrEmpty(minIssueAgeStr))
                    {
                        if (int.TryParse(minIssueAgeStr, out int minIssueAge))
                        {
                            tbcm.MinIssueAge = minIssueAge;
                        }
                        else
                        {
                            SetProcessCount("Minimum Issue Age Invalid Numeric");
                            Errors.Add(string.Format("The Minimum Issue Age is not a numeric: {0} at row {1}", minIssueAgeStr, TextFile.RowIndex));
                            error = true;
                        }
                    }
                    else
                    {
                        tbcm.MinIssueAge = null;
                    }

                    var maxIssueAgeStr = TextFile.GetColValue(TreatyBenefitCodeMappingBo.ColumnMaxIssueAge);
                    if (!string.IsNullOrEmpty(maxIssueAgeStr))
                    {
                        if (int.TryParse(maxIssueAgeStr, out int maxIssueAge))
                        {
                            tbcm.MaxIssueAge = maxIssueAge;
                        }
                        else
                        {
                            SetProcessCount("Maximum Issue Age Invalid Numeric");
                            Errors.Add(string.Format("The Maximum Issue Age is not a numeric: {0} at row {1}", maxIssueAgeStr, TextFile.RowIndex));
                            error = true;
                        }
                    }
                    else
                    {
                        tbcm.MaxIssueAge = null;
                    }

                    var maxUwRatingStr = TextFile.GetColValue(TreatyBenefitCodeMappingBo.ColumnMaxUwRating);
                    if (!string.IsNullOrEmpty(maxUwRatingStr))
                    {
                        if (Util.IsValidDouble(maxUwRatingStr, out double? output, out string _))
                        {
                            tbcm.MaxUwRating = output.Value;
                        }
                        else
                        {
                            SetProcessCount("Maximum Underwriting Rating Invalid");
                            Errors.Add(string.Format("The Maximum Underwriting Rating is invalid: {0} at row {1}", maxUwRatingStr, TextFile.RowIndex));
                            error = true;
                        }
                    }
                    else
                    {
                        tbcm.MaxUwRating = null;
                    }

                    var apLoadingStr = TextFile.GetColValue(TreatyBenefitCodeMappingBo.ColumnApLoading);
                    if (!string.IsNullOrEmpty(apLoadingStr))
                    {
                        if (Util.IsValidDouble(apLoadingStr, out double? output, out string _))
                        {
                            tbcm.ApLoading = output.Value;
                        }
                        else
                        {
                            SetProcessCount("AP Loading Invalid");
                            Errors.Add(string.Format("The AP Loading is invalid: {0} at row {1}", apLoadingStr, TextFile.RowIndex));
                            error = true;
                        }
                    }
                    else
                    {
                        tbcm.ApLoading = null;
                    }

                    var minAarStr = TextFile.GetColValue(TreatyBenefitCodeMappingBo.ColumnMinAar);
                    if (!string.IsNullOrEmpty(minAarStr))
                    {
                        if (Util.IsValidDouble(minAarStr, out double? output, out string _))
                        {
                            tbcm.MinAar = output.Value;
                        }
                        else
                        {
                            SetProcessCount("Minimum AAR Invalid");
                            Errors.Add(string.Format("The Minimum AAR is invalid: {0} at row {1}", minAarStr, TextFile.RowIndex));
                            error = true;
                        }
                    }
                    else
                    {
                        tbcm.MinAar = null;
                    }

                    var maxAarStr = TextFile.GetColValue(TreatyBenefitCodeMappingBo.ColumnMaxAar);
                    if (!string.IsNullOrEmpty(maxAarStr))
                    {
                        if (Util.IsValidDouble(maxAarStr, out double? output, out string _))
                        {
                            tbcm.MaxAar = output.Value;
                        }
                        else
                        {
                            SetProcessCount("Maximum AAR Invalid");
                            Errors.Add(string.Format("The Maximum AAR is invalid: {0} at row {1}", maxAarStr, TextFile.RowIndex));
                            error = true;
                        }
                    }
                    else
                    {
                        tbcm.MaxAar = null;
                    }

                    var ablAmountStr = TextFile.GetColValue(TreatyBenefitCodeMappingBo.ColumnAblAmount);
                    if (!string.IsNullOrEmpty(ablAmountStr))
                    {
                        if (Util.IsValidDouble(ablAmountStr, out double? output, out string _))
                        {
                            tbcm.AblAmount = output.Value;
                        }
                        else
                        {
                            SetProcessCount("ABL Amount Invalid");
                            Errors.Add(string.Format("The ABL Amount is invalid: {0} at row {1}", ablAmountStr, TextFile.RowIndex));
                            error = true;
                        }
                    }
                    else
                    {
                        tbcm.AblAmount = null;
                    }

                    var retentionShareStr = TextFile.GetColValue(TreatyBenefitCodeMappingBo.ColumnRetentionShare);
                    if (!string.IsNullOrEmpty(retentionShareStr))
                    {
                        if (Util.IsValidDouble(retentionShareStr, out double? output, out string _))
                        {
                            tbcm.RetentionShare = output.Value;
                        }
                        else
                        {
                            SetProcessCount("Retention Share Invalid");
                            Errors.Add(string.Format("The Retention Share is invalid: {0} at row {1}", retentionShareStr, TextFile.RowIndex));
                            error = true;
                        }
                    }
                    else
                    {
                        tbcm.RetentionShare = null;
                    }

                    var retentionCapStr = TextFile.GetColValue(TreatyBenefitCodeMappingBo.ColumnRetentionCap);
                    if (!string.IsNullOrEmpty(retentionCapStr))
                    {
                        if (Util.IsValidDouble(retentionCapStr, out double? output, out string _))
                        {
                            tbcm.RetentionCap = output.Value;
                        }
                        else
                        {
                            SetProcessCount("Retention Cap Invalid");
                            Errors.Add(string.Format("The Retention Cap is invalid: {0} at row {1}", retentionCapStr, TextFile.RowIndex));
                            error = true;
                        }
                    }
                    else
                    {
                        tbcm.RetentionCap = null;
                    }

                    var underwriterRatingFromStr = TextFile.GetColValue(TreatyBenefitCodeMappingBo.ColumnUnderwriterRatingFrom);
                    if (!string.IsNullOrEmpty(underwriterRatingFromStr))
                    {
                        if (Util.IsValidDouble(underwriterRatingFromStr, out double? output, out string _))
                        {
                            tbcm.UnderwriterRatingFrom = output.Value;
                        }
                        else
                        {
                            SetProcessCount("Underwriter Rating From Invalid");
                            Errors.Add(string.Format("The Underwriter Rating From is invalid: {0} at row {1}", underwriterRatingFromStr, TextFile.RowIndex));
                            error = true;
                        }
                    }
                    else
                    {
                        tbcm.UnderwriterRatingFrom = null;
                    }

                    var underwriterRatingToStr = TextFile.GetColValue(TreatyBenefitCodeMappingBo.ColumnUnderwriterRatingTo);
                    if (!string.IsNullOrEmpty(underwriterRatingToStr))
                    {
                        if (Util.IsValidDouble(underwriterRatingToStr, out double? output, out string _))
                        {
                            tbcm.UnderwriterRatingTo = output.Value;
                        }
                        else
                        {
                            SetProcessCount("Underwriter Rating To Invalid");
                            Errors.Add(string.Format("The Underwriter Rating To is invalid: {0} at row {1}", underwriterRatingToStr, TextFile.RowIndex));
                            error = true;
                        }
                    }
                    else
                    {
                        tbcm.UnderwriterRatingTo = null;
                    }

                    var riShareStr = TextFile.GetColValue(TreatyBenefitCodeMappingBo.ColumnRiShare);
                    if (!string.IsNullOrEmpty(riShareStr))
                    {
                        if (Util.IsValidDouble(riShareStr, out double? output, out string _))
                        {
                            tbcm.RiShare = output.Value;
                        }
                        else
                        {
                            SetProcessCount("RI Share 1 Invalid");
                            Errors.Add(string.Format("The RI Share 1 is invalid: {0} at row {1}", riShareStr, TextFile.RowIndex));
                            error = true;
                        }
                    }
                    else
                    {
                        tbcm.RiShare = null;
                    }

                    var riShareCapStr = TextFile.GetColValue(TreatyBenefitCodeMappingBo.ColumnRiShareCap);
                    if (!string.IsNullOrEmpty(riShareCapStr))
                    {
                        if (Util.IsValidDouble(riShareCapStr, out double? output, out string _))
                        {
                            tbcm.RiShareCap = output.Value;
                        }
                        else
                        {
                            SetProcessCount("RI Share Cap 1 Invalid");
                            Errors.Add(string.Format("The RI Share Cap 1 is invalid: {0} at row {1}", riShareCapStr, TextFile.RowIndex));
                            error = true;
                        }
                    }
                    else
                    {
                        tbcm.RiShareCap = null;
                    }

                    var riShare2Str = TextFile.GetColValue(TreatyBenefitCodeMappingBo.ColumnRiShare2);
                    if (!string.IsNullOrEmpty(riShare2Str))
                    {
                        if (Util.IsValidDouble(riShare2Str, out double? output, out string _))
                        {
                            tbcm.RiShare2 = output.Value;
                        }
                        else
                        {
                            SetProcessCount("RI Share 2 Invalid");
                            Errors.Add(string.Format("The RI Share 2 is invalid: {0} at row {1}", riShare2Str, TextFile.RowIndex));
                            error = true;
                        }
                    }
                    else
                    {
                        tbcm.RiShare2 = null;
                    }

                    var riShareCap2Str = TextFile.GetColValue(TreatyBenefitCodeMappingBo.ColumnRiShareCap2);
                    if (!string.IsNullOrEmpty(riShareCap2Str))
                    {
                        if (Util.IsValidDouble(riShareCap2Str, out double? output, out string _))
                        {
                            tbcm.RiShareCap2 = output.Value;
                        }
                        else
                        {
                            SetProcessCount("RI Share Cap 2 Invalid");
                            Errors.Add(string.Format("The RI Share Cap 2 is invalid: {0} at row {1}", riShareCap2Str, TextFile.RowIndex));
                            error = true;
                        }
                    }
                    else
                    {
                        tbcm.RiShareCap2 = null;
                    }

                    var serviceFeeStr = TextFile.GetColValue(TreatyBenefitCodeMappingBo.ColumnServiceFee);
                    if (!string.IsNullOrEmpty(serviceFeeStr))
                    {
                        if (Util.IsValidDouble(serviceFeeStr, out double? output, out string _))
                        {
                            tbcm.ServiceFee = output.Value;
                        }
                        else
                        {
                            SetProcessCount("Service Fee Invalid");
                            Errors.Add(string.Format("The Service Fee is invalid: {0} at row {1}", serviceFeeStr, TextFile.RowIndex));
                            error = true;
                        }
                    }
                    else
                    {
                        tbcm.ServiceFee = null;
                    }

                    var wakalahFeeStr = TextFile.GetColValue(TreatyBenefitCodeMappingBo.ColumnWakalahFee);
                    if (!string.IsNullOrEmpty(wakalahFeeStr))
                    {
                        if (Util.IsValidDouble(wakalahFeeStr, out double? output, out string _))
                        {
                            tbcm.WakalahFee = output.Value;
                        }
                        else
                        {
                            SetProcessCount("Wakalah Fee Invalid");
                            Errors.Add(string.Format("The Wakalah Fee is invalid: {0} at row {1}", wakalahFeeStr, TextFile.RowIndex));
                            error = true;
                        }
                    }
                    else
                    {
                        tbcm.WakalahFee = null;
                    }

                    var oriSumAssuredFromStr = TextFile.GetColValue(TreatyBenefitCodeMappingBo.ColumnOriSumAssuredFrom);
                    if (!string.IsNullOrEmpty(oriSumAssuredFromStr))
                    {
                        if (Util.IsValidDouble(oriSumAssuredFromStr, out double? output, out string _))
                        {
                            tbcm.OriSumAssuredFrom = output.Value;
                        }
                        else
                        {
                            SetProcessCount("Ori Sum Assured From Invalid");
                            Errors.Add(string.Format("The Ori Sum Assured From is invalid: {0} at row {1}", oriSumAssuredFromStr, TextFile.RowIndex));
                            error = true;
                        }
                    }
                    else
                    {
                        tbcm.OriSumAssuredFrom = null;
                    }

                    var oriSumAssuredToStr = TextFile.GetColValue(TreatyBenefitCodeMappingBo.ColumnOriSumAssuredTo);
                    if (!string.IsNullOrEmpty(oriSumAssuredToStr))
                    {
                        if (Util.IsValidDouble(oriSumAssuredToStr, out double? output, out string _))
                        {
                            tbcm.OriSumAssuredTo = output.Value;
                        }
                        else
                        {
                            SetProcessCount("Ori Sum Assured To Invalid");
                            Errors.Add(string.Format("The Ori Sum Assured To is invalid: {0} at row {1}", oriSumAssuredToStr, TextFile.RowIndex));
                            error = true;
                        }
                    }
                    else
                    {
                        tbcm.OriSumAssuredTo = null;
                    }

                    var effectiveDateStr = TextFile.GetColValue(TreatyBenefitCodeMappingBo.ColumnEffectiveDate);
                    if (!string.IsNullOrEmpty(effectiveDateStr))
                    {
                        if (!ValidateDateTimeFormat(TreatyBenefitCodeMappingBo.ColumnEffectiveDate, ref tbcm))
                        {
                            error = true;
                        }
                    }
                    else
                    {
                        tbcm.EffectiveDate = null;
                    }

                    var reinsuranceIssueAgeFromStr = TextFile.GetColValue(TreatyBenefitCodeMappingBo.ColumnReinsuranceIssueAgeFrom);
                    if (!string.IsNullOrEmpty(reinsuranceIssueAgeFromStr))
                    {
                        if (int.TryParse(reinsuranceIssueAgeFromStr, out int reinsuranceIssueAgeFrom))
                        {
                            tbcm.ReinsuranceIssueAgeFrom = reinsuranceIssueAgeFrom;
                        }
                        else
                        {
                            SetProcessCount("Reinsurance Issue Age From Invalid Numeric");
                            Errors.Add(string.Format("The Reinsurance Issue Age From is not a numeric: {0} at row {1}", reinsuranceIssueAgeFromStr, TextFile.RowIndex));
                            error = true;
                        }
                    }
                    else
                    {
                        tbcm.ReinsuranceIssueAgeFrom = null;
                    }

                    var reinsuranceIssueAgeToStr = TextFile.GetColValue(TreatyBenefitCodeMappingBo.ColumnReinsuranceIssueAgeTo);
                    if (!string.IsNullOrEmpty(reinsuranceIssueAgeToStr))
                    {
                        if (int.TryParse(reinsuranceIssueAgeToStr, out int reinsuranceIssueAgeTo))
                        {
                            tbcm.ReinsuranceIssueAgeTo = reinsuranceIssueAgeTo;
                        }
                        else
                        {
                            SetProcessCount("Reinsurance Issue Age To Invalid Numeric");
                            Errors.Add(string.Format("The Reinsurance Issue Age To is not a numeric: {0} at row {1}", reinsuranceIssueAgeToStr, TextFile.RowIndex));
                            error = true;
                        }
                    }
                    else
                    {
                        tbcm.ReinsuranceIssueAgeTo = null;
                    }
                    #endregion
                }
                catch (Exception e)
                {
                    SetProcessCount("Error");
                    Errors.Add(string.Format("Error Triggered: {0} at row {1}", e.Message, TextFile.RowIndex));
                    error = true;
                }
                string action = TextFile.GetColValue(TreatyBenefitCodeMappingBo.ColumnAction);

                if (error)
                {
                    continue;
                }

                switch (action.ToLower().Trim())
                {
                    case "u":
                    case "update":

                        TreatyBenefitCodeMappingBo tbcmDb = TreatyBenefitCodeMappingService.Find(tbcm.Id);
                        if (tbcmDb == null)
                        {
                            AddNotFoundError(tbcm);
                            continue;
                        }

                        var rangeResult = TreatyBenefitCodeMappingService.ValidateRange(tbcm);
                        var mappingResult = TreatyBenefitCodeMappingService.ValidateMapping(tbcm);
                        if (!rangeResult.Valid || !mappingResult.Valid)
                        {
                            foreach (var e in rangeResult.ToErrorArray())
                            {
                                Errors.Add(string.Format("{0} at row {1}", e, TextFile.RowIndex));
                            }
                            foreach (var e in mappingResult.ToErrorArray())
                            {
                                Errors.Add(string.Format("{0} at row {1}", e, TextFile.RowIndex));
                            }
                            error = true;
                        }
                        if (error)
                        {
                            continue;
                        }

                        UpdateData(ref tbcmDb, tbcm);

                        trail = new TrailObject();
                        result = TreatyBenefitCodeMappingService.Update(ref tbcmDb, ref trail);

                        TreatyBenefitCodeMappingService.ProcessMappingDetail(tbcmDb, GetAuthUserId());  // DO NOT TRAIL

                        Trail(result, tbcmDb, trail, "Update");

                        break;

                    case "d":
                    case "del":
                    case "delete":

                        if (tbcm.Id != 0 && TreatyBenefitCodeMappingService.IsExists(tbcm.Id))
                        {
                            trail = new TrailObject();
                            result = TreatyBenefitCodeMappingService.Delete(tbcm, ref trail);
                            Trail(result, tbcm, trail, "Delete");
                        }
                        else
                        {
                            AddNotFoundError(tbcm);
                            continue;
                        }

                        break;

                    default:

                        if (tbcm.Id != 0 && TreatyBenefitCodeMappingService.IsExists(tbcm.Id))
                        {
                            SetProcessCount("Record Found");
                            Errors.Add(string.Format("The Product Feature Mapping ID exists: {0} at row {1}", tbcm.Id, TextFile.RowIndex));
                            continue;
                        }

                        rangeResult = TreatyBenefitCodeMappingService.ValidateRange(tbcm);
                        mappingResult = TreatyBenefitCodeMappingService.ValidateMapping(tbcm);
                        if (!rangeResult.Valid || !mappingResult.Valid)
                        {
                            foreach (var e in rangeResult.ToErrorArray())
                            {
                                Errors.Add(string.Format("{0} at row {1}", e, TextFile.RowIndex));
                            }
                            foreach (var e in mappingResult.ToErrorArray())
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
                        result = TreatyBenefitCodeMappingService.Create(ref tbcm, ref trail);

                        TreatyBenefitCodeMappingService.ProcessMappingDetail(tbcm, GetAuthUserId()); // DO NOT TRAIL

                        Trail(result, tbcm, trail, "Create");
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

        public TreatyBenefitCodeMappingBo SetData()
        {
            var tbcm = new TreatyBenefitCodeMappingBo
            {
                Id = 0,
                CedantCode = TextFile.GetColValue(TreatyBenefitCodeMappingBo.ColumnCedantCode),
                CedingPlanCode = TextFile.GetColValue(TreatyBenefitCodeMappingBo.ColumnCedingPlanCode),
                Description = TextFile.GetColValue(TreatyBenefitCodeMappingBo.ColumnDescription),
                CedingBenefitTypeCode = TextFile.GetColValue(TreatyBenefitCodeMappingBo.ColumnCedingBenefitTypeCode),
                CedingBenefitRiskCode = TextFile.GetColValue(TreatyBenefitCodeMappingBo.ColumnCedingBenefitRiskCode),
                CedingTreatyCode = TextFile.GetColValue(TreatyBenefitCodeMappingBo.ColumnCedingTreatyCode),
                CampaignCode = TextFile.GetColValue(TreatyBenefitCodeMappingBo.ColumnCampaignCode),
                ReinsBasisCode = TextFile.GetColValue(TreatyBenefitCodeMappingBo.ColumnReinsBasisCode),
                TreatyCode = TextFile.GetColValue(TreatyBenefitCodeMappingBo.ColumnTreatyCode),
                BenefitCode = TextFile.GetColValue(TreatyBenefitCodeMappingBo.ColumnBenefitCode),
                ProfitComm = TextFile.GetColValue(TreatyBenefitCodeMappingBo.ColumnProfitComm),
                CreatedById = GetAuthUserId(),
                UpdatedById = GetAuthUserId(),
            };

            tbcm.Description = tbcm.Description.Trim();
            tbcm.CedingPlanCode = tbcm.CedingPlanCode?.TrimEnd(charsToTrim);
            tbcm.CedingBenefitTypeCode = tbcm.CedingBenefitTypeCode?.TrimEnd(charsToTrim);
            tbcm.CedingBenefitRiskCode = tbcm.CedingBenefitRiskCode?.TrimEnd(charsToTrim);
            tbcm.TreatyCode = tbcm.TreatyCode?.TrimEnd(charsToTrim);
            tbcm.CampaignCode = tbcm.CampaignCode?.TrimEnd(charsToTrim);

            string idStr = TextFile.GetColValue(TreatyBenefitCodeMappingBo.ColumnId);
            if (!string.IsNullOrEmpty(idStr) && int.TryParse(idStr, out int id))
            {
                tbcm.Id = id;
            }

            return tbcm;
        }

        public void UpdateData(ref TreatyBenefitCodeMappingBo tbcmDb, TreatyBenefitCodeMappingBo tbcm)
        {
            tbcmDb.CedantId = tbcm.CedantId;
            tbcmDb.CedingPlanCode = tbcm.CedingPlanCode;
            tbcmDb.Description = tbcm.Description;
            tbcmDb.CedingBenefitTypeCode = tbcm.CedingBenefitTypeCode;
            tbcmDb.CedingBenefitRiskCode = tbcm.CedingBenefitRiskCode;
            tbcmDb.CedingTreatyCode = tbcm.CedingTreatyCode;
            tbcmDb.CampaignCode = tbcm.CampaignCode;
            tbcmDb.ReinsEffDatePolStartDate = tbcm.ReinsEffDatePolStartDate;
            tbcmDb.ReinsEffDatePolEndDate = tbcm.ReinsEffDatePolEndDate;
            tbcmDb.ReinsBasisCodePickListDetailId = tbcm.ReinsBasisCodePickListDetailId;
            tbcmDb.AttainedAgeFrom = tbcm.AttainedAgeFrom;
            tbcmDb.AttainedAgeTo = tbcm.AttainedAgeTo;
            tbcmDb.ReportingStartDate = tbcm.ReportingStartDate;
            tbcmDb.ReportingEndDate = tbcm.ReportingEndDate;

            // Phase 2
            tbcmDb.ProfitCommPickListDetailId = tbcm.ProfitCommPickListDetailId;
            tbcmDb.MaxExpiryAge = tbcm.MaxExpiryAge;
            tbcmDb.MinIssueAge = tbcm.MinIssueAge;
            tbcmDb.MaxIssueAge = tbcm.MaxIssueAge;
            tbcmDb.MaxUwRating = tbcm.MaxUwRating;
            tbcmDb.ApLoading = tbcm.ApLoading;
            tbcmDb.MinAar = tbcm.MinAar;
            tbcmDb.MaxAar = tbcm.MaxAar;
            tbcmDb.AblAmount = tbcm.AblAmount;
            tbcmDb.RetentionShare = tbcm.RetentionShare;
            tbcmDb.RetentionCap = tbcm.RetentionCap;
            tbcmDb.RiShare = tbcm.RiShare;
            tbcmDb.RiShareCap = tbcm.RiShareCap;
            tbcmDb.ServiceFee = tbcm.ServiceFee;
            tbcmDb.WakalahFee = tbcm.WakalahFee;
            tbcmDb.UnderwriterRatingFrom = tbcm.UnderwriterRatingFrom;
            tbcmDb.UnderwriterRatingTo = tbcm.UnderwriterRatingTo;
            tbcmDb.RiShare2 = tbcm.RiShare2;
            tbcmDb.RiShareCap2 = tbcm.RiShareCap2;

            tbcmDb.OriSumAssuredFrom = tbcm.OriSumAssuredFrom;
            tbcmDb.OriSumAssuredTo = tbcm.OriSumAssuredTo;

            tbcmDb.EffectiveDate = tbcm.EffectiveDate;

            tbcmDb.ReinsuranceIssueAgeFrom = tbcm.ReinsuranceIssueAgeFrom;
            tbcmDb.ReinsuranceIssueAgeTo = tbcm.ReinsuranceIssueAgeTo;

            tbcmDb.BenefitId = tbcm.BenefitId;
            tbcmDb.TreatyCodeId = tbcm.TreatyCodeId;
            tbcmDb.UpdatedById = tbcm.UpdatedById;
        }

        public int GetAuthUserId()
        {
            return AuthUserId != null ? AuthUserId.Value : User.DefaultSuperUserId;
        }

        public void Trail(Result result, TreatyBenefitCodeMappingBo tbcm, TrailObject trail, string action)
        {
            if (result.Valid)
            {
                UserTrailBo userTrailBo = new UserTrailBo(
                    tbcm.Id,
                    string.Format("{0} Product Feature Mapping", action),
                    result,
                    trail,
                    tbcm.UpdatedById.Value
                );
                UserTrailService.Create(ref userTrailBo);

                SetProcessCount(action);
            }
        }

        public bool ValidateDateTimeFormat(int type, ref TreatyBenefitCodeMappingBo tbcm)
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

        public void AddNotFoundError(TreatyBenefitCodeMappingBo tbcm)
        {
            SetProcessCount("Record Not Found");
            Errors.Add(string.Format("The Product Feature Mapping ID doesn't exists: {0} at row {1}", tbcm.Id, TextFile.RowIndex));
        }

        public List<Column> GetColumns()
        {
            Columns = TreatyBenefitCodeMappingBo.GetColumns();
            return Columns;
        }

        public void UpdateFileStatus(int status, string description)
        {
            var treatyBenefitCodeMappingUploadBo = TreatyBenefitCodeMappingUploadBo;
            treatyBenefitCodeMappingUploadBo.Status = status;

            if (Errors.Count > 0)
            {
                treatyBenefitCodeMappingUploadBo.Errors = JsonConvert.SerializeObject(Errors);
            }

            var trail = new TrailObject();
            var result = TreatyBenefitCodeMappingUploadService.Update(ref treatyBenefitCodeMappingUploadBo, ref trail);

            var userTrailBo = new UserTrailBo(
                treatyBenefitCodeMappingUploadBo.Id,
                description,
                result,
                trail,
                User.DefaultSuperUserId
            );
            UserTrailService.Create(ref userTrailBo);
        }
    }
}
