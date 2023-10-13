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
    public class ProcessFacMasterListing : Command
    {
        public List<Column> Cols { get; set; }

        public FacMasterListingUploadBo FacMasterListingUploadBo { get; set; }

        public string FilePath { get; set; }

        public HttpPostedFileBase PostedFile { get; set; }

        public int? AuthUserId { get; set; }

        public TextFile TextFile { get; set; }

        public List<string> Errors { get; set; }

        public char[] charsToTrim = { ',', '.', ' ' };

        public const int TypeUniqueId = 1;
        public const int TypeEwarpNumber = 2;
        public const int TypeInsuredName = 3;
        public const int TypeInsuredDateOfBirth = 4;
        public const int TypeInsuredGenderCode = 5;
        public const int TypeCedantCode = 6;
        public const int TypePolicyNumber = 7;
        public const int TypeFlatExtraAmountOffered = 8;
        public const int TypeFlatExtraDuration = 9;
        public const int TypeBenefitCode = 10;
        public const int TypeSumAssuredOffered = 11;
        public const int TypeEwarpActionCode = 12;
        public const int TypeUwRatingOffered = 13;
        public const int TypeOfferLetterSentDate = 14;
        public const int TypeUwOpinion = 15;
        public const int TypeRemark = 16;
        public const int TypeCedingBenefitTypeCode = 17;

        public ProcessFacMasterListing()
        {
            Title = "ProcessFacMasterListing";
            Description = "To read Fac Master Listing csv file and insert into database";
            Hide = false;
            Errors = new List<string> { };
            GetColumns();
        }

        public override bool Validate()
        {
            return base.Validate();
        }

        public override void Run()
        {
            PrintStarting();

            if (FacMasterListingUploadService.CountByStatus(FacMasterListingUploadBo.StatusPendingProcess) > 0)
            {
                foreach (var bo in FacMasterListingUploadService.GetByStatus(FacMasterListingUploadBo.StatusPendingProcess))
                {
                    FilePath = bo.GetLocalPath();
                    FacMasterListingUploadBo = bo;
                    Errors = new List<string>();
                    Process();

                    if (Errors.Count > 0)
                    {
                        UpdateFileStatus(FacMasterListingUploadBo.StatusFailed, "Process Fac Master Listing File Failed");
                    }
                    else
                        UpdateFileStatus(FacMasterListingUploadBo.StatusSuccess, "Process Fac Master Listing File Success");
                }
            }

            PrintEnding();
        }

        public void Process()
        {
            UpdateFileStatus(FacMasterListingUploadBo.StatusProcessing, "Processing Fac Master Listing File");

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

            PrintMessage("Processing FacMasterListingUpload Id: " + FacMasterListingUploadBo.Id);
            PrintMessage();

            TrailObject trail;
            Result result;

            DeleteExistingData();

            while (TextFile.GetNextRow() != null)
            {
                if (TextFile.RowIndex == 1)
                    continue; // Skip header row

                if (PrintCommitBuffer())
                {
                }
                SetProcessCount();

                bool error = false;

                FacMasterListingBo fml = null;
                try
                {
                    fml = SetData();

                    if (string.IsNullOrEmpty(fml.UniqueId))
                    {
                        SetProcessCount("Unique Id Empty");
                        Errors.Add(string.Format("Please enter the Unique Id at row {0}", TextFile.RowIndex));
                        error = true;
                    }

                    var ewarpNumberStr = TextFile.GetColValue(TypeEwarpNumber);
                    if (!string.IsNullOrEmpty(ewarpNumberStr))
                    {
                        if (int.TryParse(ewarpNumberStr, out int ewarpNumber))
                        {
                            fml.EwarpNumber = ewarpNumber;
                        }
                        else
                        {
                            SetProcessCount("eWarp Number Invalid Numeric");
                            Errors.Add(string.Format("The eWarp Number is not a numeric: {0} at row {1}", ewarpNumberStr, TextFile.RowIndex));
                            error = true;
                        }
                    }
                    else
                    {
                        SetProcessCount("eWarp Number Empty");
                        Errors.Add(string.Format("Please enter the eWarp Number at row {0}", TextFile.RowIndex));
                        error = true;
                    }

                    if (string.IsNullOrEmpty(fml.InsuredName))
                    {
                        SetProcessCount("Insured Name Empty");
                        Errors.Add(string.Format("Please enter the Insured Name at row {0}", TextFile.RowIndex));
                        error = true;
                    }

                    var insuredDateOfBirthStr = TextFile.GetColValue(TypeInsuredDateOfBirth);
                    if (!string.IsNullOrEmpty(insuredDateOfBirthStr))
                    {
                        if (!ValidateDateTimeFormat(TypeInsuredDateOfBirth, ref fml))
                        {
                            error = true;
                        }
                    }
                    else
                    {
                        fml.InsuredDateOfBirth = null;
                    }

                    if (!string.IsNullOrEmpty(fml.InsuredGenderCode))
                    {
                        PickListDetailBo igc = PickListDetailService.FindByPickListIdCode(PickListBo.InsuredGenderCode, fml.InsuredGenderCode);
                        if (igc == null)
                        {
                            SetProcessCount("Insured Gender Code Not Found");
                            Errors.Add(string.Format("The Insured Gender Code doesn't exists: {0} at row {1}", fml.InsuredGenderCode, TextFile.RowIndex));
                            error = true;
                        }
                        else
                        {
                            fml.InsuredGenderCodePickListDetailId = igc.Id;
                            fml.InsuredGenderCodePickListDetailBo = igc;
                        }
                    }
                    else
                    {
                        fml.InsuredGenderCodePickListDetailId = null;
                    }

                    if (!string.IsNullOrEmpty(fml.CedantCode))
                    {
                        CedantBo cedantBo = CedantService.FindByCode(fml.CedantCode);
                        if (cedantBo == null)
                        {
                            SetProcessCount("Cedant Code Not Found");
                            Errors.Add(string.Format("The Cedant Code doesn't exists: {0} at row {1}", fml.CedantCode, TextFile.RowIndex));
                            error = true;
                        }
                        else if (cedantBo.Status == CedantBo.StatusInactive)
                        {
                            SetProcessCount("Cedant Inactive");
                            Errors.Add(string.Format("{0}: {1} at row {2}", MessageBag.CedantStatusInactive, fml.CedantCode, TextFile.RowIndex));
                            error = true;
                        }
                        else
                        {
                            fml.CedantId = cedantBo.Id;
                            fml.CedantBo = cedantBo;
                        }
                    }
                    else
                    {
                        SetProcessCount("Cedant Code Empty");
                        Errors.Add(string.Format("Please enter the Cedant Code at row {0}", TextFile.RowIndex));
                        error = true;
                    }

                    if (string.IsNullOrEmpty(fml.PolicyNumber))
                    {
                        SetProcessCount("Policy Number Empty");
                        Errors.Add(string.Format("Please enter the Policy Number at row {0}", TextFile.RowIndex));
                        error = true;
                    }

                    var flatExtraAmountOfferedStr = TextFile.GetColValue(TypeFlatExtraAmountOffered);
                    if (!string.IsNullOrEmpty(flatExtraAmountOfferedStr))
                    {
                        if (Util.IsValidDouble(flatExtraAmountOfferedStr, out double? output, out string _))
                        {
                            fml.FlatExtraAmountOffered = output.Value;
                        }
                        else
                        {
                            SetProcessCount("Flat Extra Amount Offered Invalid");
                            Errors.Add(string.Format("The Flat Extra Amount Offered is invalid: {0} at row {1}", flatExtraAmountOfferedStr, TextFile.RowIndex));
                            error = true;
                        }
                    }
                    else
                    {
                        fml.FlatExtraAmountOffered = null;
                    }

                    var flatExtraDurationStr = TextFile.GetColValue(TypeFlatExtraDuration);
                    if (!string.IsNullOrEmpty(flatExtraDurationStr))
                    {
                        if (int.TryParse(flatExtraDurationStr, out int flatExtraDuration))
                        {
                            fml.FlatExtraDuration = flatExtraDuration;
                        }
                        else
                        {
                            fml.FlatExtraDuration = null;
                        }
                    }
                    else
                    {
                        fml.FlatExtraDuration = null;
                    }

                    if (!string.IsNullOrEmpty(fml.BenefitCode))
                    {
                        string[] benefitCodes = fml.BenefitCode.ToArraySplitTrim();
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
                        SetProcessCount("Benefit Code Empty");
                        Errors.Add(string.Format("Please enter the Benefit Code at row {0}", TextFile.RowIndex));
                        error = true;
                    }

                    var sumAssuredOfferedStr = TextFile.GetColValue(TypeSumAssuredOffered);
                    if (!string.IsNullOrEmpty(sumAssuredOfferedStr))
                    {
                        if (Util.IsValidDouble(sumAssuredOfferedStr, out double? output, out string _))
                        {
                            fml.SumAssuredOffered = output.Value;
                        }
                        else
                        {
                            SetProcessCount("Sum Assured Offered Invalid");
                            Errors.Add(string.Format("The Sum Assured Offered is invalid: {0} at row {1}", sumAssuredOfferedStr, TextFile.RowIndex));
                            error = true;
                        }
                    }
                    else
                    {
                        fml.SumAssuredOffered = null;
                    }

                    if (string.IsNullOrEmpty(fml.EwarpActionCode))
                    {
                        SetProcessCount("eWarp Action Code Empty");
                        Errors.Add(string.Format("Please enter the eWarp Action Code at row {0}", TextFile.RowIndex));
                        error = true;
                    }

                    var uwRatingOfferedStr = TextFile.GetColValue(TypeUwRatingOffered);
                    if (!string.IsNullOrEmpty(uwRatingOfferedStr))
                    {
                        if (Util.IsValidDouble(uwRatingOfferedStr, out double? output, out string _))
                        {
                            fml.UwRatingOffered = output.Value;
                        }
                        else
                        {
                            SetProcessCount("UW Rating Offered Invalid");
                            Errors.Add(string.Format("The UW Rating Offered is invalid: {0} at row {1}", uwRatingOfferedStr, TextFile.RowIndex));
                            error = true;
                        }
                    }
                    else
                    {
                        fml.UwRatingOffered = null;
                    }

                    var offerLetterSentDateStr = TextFile.GetColValue(TypeOfferLetterSentDate);
                    if (!string.IsNullOrEmpty(offerLetterSentDateStr))
                    {
                        if (!ValidateDateTimeFormat(TypeOfferLetterSentDate, ref fml))
                        {
                            error = true;
                        }
                    }
                    else
                    {
                        fml.OfferLetterSentDate = null;
                    }

                    if (string.IsNullOrEmpty(fml.UwOpinion))
                    {
                        fml.UwOpinion = null;
                    }

                    if (string.IsNullOrEmpty(fml.Remark))
                    {
                        fml.Remark = null;
                    }

                    if (!string.IsNullOrEmpty(fml.CedingBenefitTypeCode))
                    {
                        string[] cedingBenefitTypeCodes = fml.CedingBenefitTypeCode.ToArraySplitTrim();
                        foreach (string cedingBenefitTypeStr in cedingBenefitTypeCodes)
                        {
                            var cedingBenefitTypeCodeBo = PickListDetailService.FindByPickListIdCode(PickListBo.CedingBenefitTypeCode, cedingBenefitTypeStr);
                            if (cedingBenefitTypeCodeBo == null)
                            {
                                SetProcessCount("Ceding Benefit Type Code Not Found");
                                Errors.Add(string.Format("{0} at row {1}", string.Format("The Ceding Benefit Type Code is not found: {0}", cedingBenefitTypeStr), TextFile.RowIndex));
                                error = true;
                            }
                        }
                    }
                    else
                    {
                        fml.CedingBenefitTypeCode = null;
                    }
                }
                catch (Exception e)
                {
                    SetProcessCount("Error");
                    Errors.Add(string.Format("Error Triggered: {0} at row {1}", e.Message, TextFile.RowIndex));
                    error = true;
                }

                if (error)
                {
                    continue;
                }

                if (fml.Id != 0 && FacMasterListingService.IsExists(fml.Id))
                {
                    SetProcessCount("Record Found");
                    Errors.Add(string.Format("The Fac Master Listing ID exists: {0} at row {1}", fml.Id, TextFile.RowIndex));
                    continue;
                }

                Result mappingResult = FacMasterListingService.ValidateMapping(fml);
                if (!mappingResult.Valid)
                {
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
                result = FacMasterListingService.Create(ref fml, ref trail);

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

                FacMasterListingService.ProcessMappingDetail(fml, fml.CreatedById); // DO NOT TRAIL
                Trail(result, fml, trail, "Create");
            }

            PrintProcessCount();

            TextFile.Close();

            foreach (string error in Errors)
            {
                PrintError(error);
            }
        }

        public void DeleteExistingData()
        {
            IList<FacMasterListingBo> facMasterListingBo = FacMasterListingService.Get();
            foreach (FacMasterListingBo bo in facMasterListingBo)
            {
                TrailObject trail = new TrailObject();
                Result result = FacMasterListingService.Delete(bo, ref trail);
                if (result.Valid)
                {
                    UserTrailBo userTrailBo = new UserTrailBo(
                        bo.Id,
                        "Delete FAC Master Listing",
                        result,
                        trail,
                        User.DefaultSuperUserId
                    );
                    UserTrailService.Create(ref userTrailBo);
                }
            }
        }

        public void ExportToCsv(IEnumerable<FacMasterListingBo> query)
        {
            string filename = "FacMasterListing".AppendDateTimeFileName(".csv");
            string path = Util.GetTemporaryPath();

            FilePath = string.Format("{0}/{1}", path, filename);
            Util.MakeDir(FilePath);

            // Delete all previous files
            Util.DeleteFiles(path, @"FacMasterListing*");

            // Header
            ExportWriteLine(string.Join(",", Cols.Select(p => p.Header)));

            if (query != null)
            {
                int total = query.Count();
                int take = 50;
                for (int skip = 0; skip < total + take; skip += take)
                {
                    if (skip >= total)
                        break;

                    foreach (var facMasterListing in query.OrderBy(q => q.Id).Skip(skip).Take(take).ToList())
                    {
                        List<string> values = new List<string> { };
                        foreach (var col in Cols)
                        {
                            if (string.IsNullOrEmpty(col.Property))
                            {
                                values.Add("");
                                continue;
                            }

                            string value = "";
                            object v = null;

                            switch (col.ColIndex)
                            {
                                case TypeFlatExtraAmountOffered:
                                case TypeSumAssuredOffered:
                                case TypeUwRatingOffered:
                                    v = Util.DoubleToString(facMasterListing.GetPropertyValue(col.Property));
                                    break;
                                default:
                                    v = facMasterListing.GetPropertyValue(col.Property);
                                    break;
                            }

                            if (v != null)
                            {
                                if (v is DateTime d)
                                {
                                    value = d.ToString(Util.GetDateFormat());
                                }
                                else
                                {
                                    value = v.ToString();
                                }
                            }

                            values.Add(string.Format("\"{0}\"", value));
                        }
                        string line = string.Join(",", values.ToArray());
                        ExportWriteLine(line);
                    }

                    total = query.Count();
                }
            }
        }

        public void ExportWriteLine(object line)
        {
            using (var textFile = new TextFile(FilePath, true, true))
            {
                textFile.WriteLine(line);
            }
        }

        public FacMasterListingBo SetData()
        {
            var rt = new FacMasterListingBo
            {
                Id = 0,
                UniqueId = TextFile.GetColValue(TypeUniqueId),
                InsuredName = TextFile.GetColValue(TypeInsuredName),
                InsuredGenderCode = TextFile.GetColValue(TypeInsuredGenderCode),
                CedantCode = TextFile.GetColValue(TypeCedantCode),
                PolicyNumber = TextFile.GetColValue(TypePolicyNumber),
                BenefitCode = TextFile.GetColValue(TypeBenefitCode),
                EwarpActionCode = TextFile.GetColValue(TypeEwarpActionCode),
                UwOpinion = TextFile.GetColValue(TypeUwOpinion),
                Remark = TextFile.GetColValue(TypeRemark),
                CedingBenefitTypeCode = TextFile.GetColValue(TypeCedingBenefitTypeCode),
                CreatedById = AuthUserId != null ? AuthUserId.Value : User.DefaultSuperUserId,
                UpdatedById = AuthUserId != null ? AuthUserId : User.DefaultSuperUserId,
            };

            rt.UniqueId = rt.UniqueId?.Trim();
            rt.InsuredName = rt.InsuredName?.Trim();
            rt.InsuredGenderCode = rt.InsuredGenderCode?.Trim();
            rt.CedantCode = rt.CedantCode?.Trim();
            rt.PolicyNumber = rt.PolicyNumber?.TrimEnd(charsToTrim);
            rt.BenefitCode = rt.BenefitCode?.TrimEnd(charsToTrim);
            rt.EwarpActionCode = rt.EwarpActionCode?.Trim();
            rt.UwOpinion = rt.UwOpinion?.Trim();
            rt.Remark = rt.Remark?.Trim();
            rt.CedingBenefitTypeCode = rt.CedingBenefitTypeCode?.Trim();

            return rt;
        }

        public void UpdateData(ref FacMasterListingBo fmlDb, FacMasterListingBo fml)
        {
            fmlDb.UniqueId = fml.UniqueId;
            fmlDb.EwarpNumber = fml.EwarpNumber;
            fmlDb.InsuredName = fml.InsuredName;
            fmlDb.InsuredDateOfBirth = fml.InsuredDateOfBirth;
            fmlDb.InsuredGenderCodePickListDetailId = fml.InsuredGenderCodePickListDetailId;
            fmlDb.CedantId = fml.CedantId;
            fmlDb.PolicyNumber = fml.PolicyNumber;
            fmlDb.FlatExtraAmountOffered = fml.FlatExtraAmountOffered;
            fmlDb.FlatExtraDuration = fml.FlatExtraDuration;
            fmlDb.BenefitCode = fml.BenefitCode;
            fmlDb.SumAssuredOffered = fml.SumAssuredOffered;
            fmlDb.EwarpActionCode = fml.EwarpActionCode;
            fmlDb.UwRatingOffered = fml.UwRatingOffered;
            fmlDb.OfferLetterSentDate = fml.OfferLetterSentDate;
            fmlDb.UwOpinion = fml.UwOpinion;
            fmlDb.Remark = fml.Remark;
            fmlDb.CedingBenefitTypeCode = fml.CedingBenefitTypeCode;
            fmlDb.UpdatedById = fml.UpdatedById;
        }

        public void Trail(Result result, FacMasterListingBo fml, TrailObject trail, string action)
        {
            if (result.Valid)
            {
                UserTrailBo userTrailBo = new UserTrailBo(
                    fml.Id,
                    string.Format("{0} Fac Master Listing", action),
                    result,
                    trail,
                    fml.UpdatedById.Value
                );
                UserTrailService.Create(ref userTrailBo);

                SetProcessCount(action);
            }
        }

        public bool ValidateDateTimeFormat(int type, ref FacMasterListingBo fml)
        {
            string header = Cols.Where(m => m.ColIndex == type).Select(m => m.Header).FirstOrDefault();
            string property = Cols.Where(m => m.ColIndex == type).Select(m => m.Property).FirstOrDefault();

            bool valid = true;
            string value = TextFile.GetColValue(type);
            if (!string.IsNullOrEmpty(value))
            {
                if (Util.TryParseDateTime(value, out DateTime? datetime, out string error))
                {
                    fml.SetPropertyValue(property, datetime.Value);
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

        public void AddNotFoundError(FacMasterListingBo fml)
        {
            SetProcessCount("Record Not Found");
            Errors.Add(string.Format("The FAC Master Listing ID doesn't exists: {0} at row {1}", fml.Id, TextFile.RowIndex));
        }

        public List<Column> GetColumns()
        {
            Cols = new List<Column>
            {
                new Column
                {
                    Header = "Unique Id",
                    ColIndex = TypeUniqueId,
                    Property = "UniqueId",
                },
                new Column
                {
                    Header = "eWarp Number",
                    ColIndex = TypeEwarpNumber,
                    Property = "EwarpNumber",
                },
                new Column
                {
                    Header = "Insured Name",
                    ColIndex = TypeInsuredName,
                    Property = "InsuredName",
                },
                new Column
                {
                    Header = "Insured Date Of Birth",
                    ColIndex = TypeInsuredDateOfBirth,
                    Property = "InsuredDateOfBirth",
                },
                new Column
                {
                    Header = "Insured Gender Code",
                    ColIndex = TypeInsuredGenderCode,
                    Property = "InsuredGenderCode",
                },
                new Column
                {
                    Header = "Ceding Company",
                    ColIndex = TypeCedantCode,
                    Property = "CedantCode",
                },
                new Column
                {
                    Header = "Policy Number",
                    ColIndex = TypePolicyNumber,
                    Property = "PolicyNumber",
                },
                new Column
                {
                    Header = "Flat Extra Amount Offered",
                    ColIndex = TypeFlatExtraAmountOffered,
                    Property = "FlatExtraAmountOffered",
                },
                new Column
                {
                    Header = "Flat Extra Duration",
                    ColIndex = TypeFlatExtraDuration,
                    Property = "FlatExtraDuration",
                },
                new Column
                {
                    Header = "MLRe Benefit Code",
                    ColIndex = TypeBenefitCode,
                    Property = "BenefitCode",
                },
                new Column
                {
                    Header = "Sum Assured Offered",
                    ColIndex = TypeSumAssuredOffered,
                    Property = "SumAssuredOffered",
                },
                new Column
                {
                    Header = "eWarp Action Code",
                    ColIndex = TypeEwarpActionCode,
                    Property = "EwarpActionCode",
                },
                new Column
                {
                    Header = "UW Rating Offered",
                    ColIndex = TypeUwRatingOffered,
                    Property = "UwRatingOffered",
                },
                new Column
                {
                    Header = "Offer Letter Sent Date",
                    ColIndex = TypeOfferLetterSentDate,
                    Property = "OfferLetterSentDate",
                },
                new Column
                {
                    Header = "UW Opinion",
                    ColIndex = TypeUwOpinion,
                    Property = "UwOpinion",
                },
                new Column
                {
                    Header = "Remark",
                    ColIndex = TypeRemark,
                    Property = "Remark",
                },
                new Column
                {
                    Header = "Ceding Benefit Type Code",
                    ColIndex = TypeCedingBenefitTypeCode,
                    Property = "CedingBenefitTypeCode",
                },
            };

            return Cols;
        }

        public void UpdateFileStatus(int status, string description)
        {
            var facMasterListingUploadBo = FacMasterListingUploadBo;
            facMasterListingUploadBo.Status = status;

            if (Errors.Count > 0)
            {
                facMasterListingUploadBo.Errors = JsonConvert.SerializeObject(Errors);
            }

            var trail = new TrailObject();
            var result = FacMasterListingUploadService.Update(ref facMasterListingUploadBo, ref trail);

            var userTrailBo = new UserTrailBo(
                facMasterListingUploadBo.Id,
                description,
                result,
                trail,
                User.DefaultSuperUserId
            );
            UserTrailService.Create(ref userTrailBo);
        }
    }
}
