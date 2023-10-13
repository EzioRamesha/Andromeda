using BusinessObject;
using BusinessObject.Claims;
using BusinessObject.Identity;
using DataAccess.Entities;
using DataAccess.Entities.Claims;
using DataAccess.Entities.Identity;
using Newtonsoft.Json;
using Services;
using Services.Claims;
using Shared;
using Shared.Forms.Attributes;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Models
{
    public class ClaimDataBatchViewModel : IValidatableObject
    {
        // Header Values
        public int Id { get; set; }

        public int ModuleId { get; set; }

        public int Status { get; set; }

        [Required]
        [Display(Name = "Ceding Company")]
        public int CedantId { get; set; }

        [Display(Name = "Ceding Company")]
        public CedantBo CedantBo { get; set; }

        public Cedant Cedant { get; set; }

        [Display(Name = "Treaty ID")]
        public int? TreatyId { get; set; }

        [Display(Name = "Treaty ID")]
        public TreatyBo TreatyBo { get; set; }

        public Treaty Treaty { get; set; }

        [Required]
        public string Quarter { get; set; }

        [Required]
        [Display(Name = "Configuration")]
        public int ClaimDataConfigId { get; set; }

        public ClaimDataConfigBo ClaimDataConfigBo { get; set; }

        public ClaimDataConfig ClaimDataConfig { get; set; }

        [Display(Name = "SOA Data Matching Status")]
        public string SoaDataMatchStatus { get; set; }

        [Required]
        [Display(Name = "SOA Data Matching")]
        public int? SoaDataBatchId { get; set; }

        [Required]
        [Display(Name = "Claim Transaction Type")]
        public int? ClaimTransactionTypePickListDetailId { get; set; }

        [Display(Name = "Person In-Charge")]
        public int PersonInChargeId { get; set; }

        public UserBo PersonInChargeBo { get; set; }

        public User PersonInCharge { get; set; }

        public int TotalMappingFailedStatus { get; set; }
        public int TotalPreComputationFailedStatus { get; set; }
        public int TotalPreValidationFailedStatus { get; set; }

        [DisplayName("Received Date")]
        public DateTime ReceivedAt { get; set; }
        [DisplayName("Received Date")]
        [ValidateDate]
        public string ReceivedAtStr { get; set; }

        // SOA
        // Not done yet

        // File Config
        public ClaimDataFileConfig ClaimDataFileConfig { get; set; }

        public int FileType { get; set; }

        [Display(Name = "Worksheet")]
        public string Worksheet { get; set; }

        [Display(Name = "Delimiter")]
        public int? Delimiter { get; set; }

        [Display(Name = "Has Header")]
        public bool HasHeader { get; set; }

        [Display(Name = "Header Row")]
        public int? HeaderRow { get; set; }

        [Display(Name = "Start Row")]
        public int? StartRow { get; set; }

        [Display(Name = "End Row")]
        public int? EndRow { get; set; }

        [Display(Name = "Start Column")]
        public int? StartColumn { get; set; }

        [Display(Name = "End Column")]
        public int? EndColumn { get; set; }

        // Upload File
        [FileUploadValidation]
        public HttpPostedFileBase[] Upload { get; set; }

        [Display(Name = "Upload Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? UploadDate { get; set; }

        public string IncludedFiles { get; set; }

        // Override Properties
        [Display(Name = "Override Properties")]
        public string OverridePropertiesStr { get; set; }

        [Display(Name = "Override Properties")]
        public Dictionary<string, object> OverrideProperties { get; set; }

        public ClaimDataBatchViewModel() { Set(); }

        public ClaimDataBatchViewModel(ClaimDataBatchBo claimDataBatchBo)
        {
            Set(claimDataBatchBo);
        }

        public void Set(ClaimDataBatchBo claimDataBatchBo = null)
        {
            ModuleBo moduleBo = ModuleService.FindByController(ModuleBo.ModuleController.ClaimData.ToString());
            ModuleId = moduleBo.Id;

            if (claimDataBatchBo != null)
            {
                Id = claimDataBatchBo.Id;
                CedantId = claimDataBatchBo.CedantId;
                TreatyId = claimDataBatchBo.ClaimDataConfigBo.TreatyId;
                ClaimDataConfigId = claimDataBatchBo.ClaimDataConfigId;
                SoaDataBatchId = claimDataBatchBo.SoaDataBatchId;
                ClaimTransactionTypePickListDetailId = claimDataBatchBo.ClaimTransactionTypePickListDetailId;
                PersonInChargeId = claimDataBatchBo.CreatedById;
                Quarter = claimDataBatchBo.Quarter;
                Status = claimDataBatchBo.Status;
                OverridePropertiesStr = claimDataBatchBo.OverrideProperties;

                TotalMappingFailedStatus = claimDataBatchBo.TotalMappingFailedStatus;
                TotalPreComputationFailedStatus = claimDataBatchBo.TotalPreComputationFailedStatus;
                TotalPreValidationFailedStatus = claimDataBatchBo.TotalPreValidationFailedStatus;

                ReceivedAt = claimDataBatchBo.ReceivedAt;
                ReceivedAtStr = claimDataBatchBo.ReceivedAt.ToString(Util.GetDateFormat());

                CedantBo = claimDataBatchBo.CedantBo;
                TreatyBo = claimDataBatchBo.ClaimDataConfigBo.TreatyBo;
                ClaimDataConfigBo = claimDataBatchBo.ClaimDataConfigBo;
                PersonInChargeBo = claimDataBatchBo.CreatedByBo;

                FileType = claimDataBatchBo.ClaimDataConfigBo.FileType;
                Worksheet = claimDataBatchBo.ClaimDataFileConfig.Worksheet;
                Delimiter = claimDataBatchBo.ClaimDataFileConfig.Delimiter;
                HasHeader = claimDataBatchBo.ClaimDataFileConfig.HasHeader;
                HeaderRow = claimDataBatchBo.ClaimDataFileConfig.HeaderRow;
                StartRow = claimDataBatchBo.ClaimDataFileConfig.StartRow;
                EndRow = claimDataBatchBo.ClaimDataFileConfig.EndRow;
                StartColumn = claimDataBatchBo.ClaimDataFileConfig.StartColumn;
                EndColumn = claimDataBatchBo.ClaimDataFileConfig.EndColumn;
                ClaimDataFileConfig = claimDataBatchBo.ClaimDataFileConfig;
            }
            else
            {
                Status = ClaimDataBatchBo.StatusPending;
                ReceivedAtStr = DateTime.Today.ToString(Util.GetDateFormat());
            }
        }

        public void SetBos(ClaimDataBatchBo claimDataBatchBo)
        {
            CedantBo = claimDataBatchBo.CedantBo;
            TreatyBo = claimDataBatchBo.TreatyBo;
            ClaimDataConfigBo = ClaimDataConfigService.Find(ClaimDataConfigId);
            PersonInChargeBo = claimDataBatchBo.CreatedByBo;
        }

        public void Get(ref ClaimDataBatchBo claimDataBatchBo)
        {
            ClaimDataConfigBo claimDataConfigBo = ClaimDataConfigService.Find(ClaimDataConfigId);
            claimDataBatchBo.Id = Id;
            claimDataBatchBo.CedantId = CedantId;
            claimDataBatchBo.TreatyId = TreatyId;
            claimDataBatchBo.ClaimDataConfigId = ClaimDataConfigId;
            claimDataBatchBo.SoaDataBatchId = SoaDataBatchId;
            claimDataBatchBo.ClaimTransactionTypePickListDetailId = ClaimTransactionTypePickListDetailId;
            claimDataBatchBo.CreatedById = PersonInChargeId;
            claimDataBatchBo.Quarter = Quarter;
            claimDataBatchBo.Status = Status;
            claimDataBatchBo.OverrideProperties = OverridePropertiesStr;
            claimDataBatchBo.ReceivedAt = DateTime.Parse(ReceivedAtStr);

            claimDataBatchBo.ClaimDataFileConfig = claimDataConfigBo.ClaimDataFileConfig;
            claimDataBatchBo.ClaimDataFileConfig.Worksheet = Worksheet;
            claimDataBatchBo.ClaimDataFileConfig.Delimiter = Delimiter;
            claimDataBatchBo.ClaimDataFileConfig.HasHeader = HasHeader;
            claimDataBatchBo.ClaimDataFileConfig.HeaderRow = HeaderRow;
            claimDataBatchBo.ClaimDataFileConfig.StartRow = StartRow;
            claimDataBatchBo.ClaimDataFileConfig.EndRow = EndRow;
            claimDataBatchBo.ClaimDataFileConfig.StartColumn = StartColumn;
            claimDataBatchBo.ClaimDataFileConfig.EndColumn = EndColumn;

            ClaimDataFileConfig = claimDataBatchBo.ClaimDataFileConfig;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> results = new List<ValidationResult>();

            if (FileType == ClaimDataConfigBo.FileTypePlainText && (Delimiter == null || Delimiter == 0))
            {
                results.Add(new ValidationResult(
                string.Format(MessageBag.Required, "Delimiter"),
                new[] { nameof(Delimiter) }));
            }

            if (HasHeader && HeaderRow != null && StartRow != null)
            {
                if (HeaderRow >= StartRow)
                {
                    results.Add(new ValidationResult(
                    string.Format(MessageBag.GreaterThan, "Start Row", "Header Row"),
                    new[] { nameof(StartRow) }));
                }
            }

            if (StartRow != null && EndRow != null)
            {
                if (StartRow > EndRow)
                {
                    results.Add(new ValidationResult(
                    string.Format(MessageBag.GreaterThan, "End Row", "Start Row"),
                    new[] { nameof(EndRow) }));
                }
            }

            if (StartColumn != null && EndColumn != null)
            {
                if (StartColumn > EndColumn)
                {
                    results.Add(new ValidationResult(
                    string.Format(MessageBag.GreaterThan, "End Column", "Start Column"),
                    new[] { nameof(EndColumn) }));
                }
            }

            return results;
        }

        public static Expression<Func<ClaimDataBatch, ClaimDataBatchViewModel>> Expression()
        {
            return entity => new ClaimDataBatchViewModel
            {
                Id = entity.Id,
                CedantId = entity.CedantId,
                Cedant = entity.Cedant,
                TreatyId = entity.TreatyId,
                Treaty = entity.Treaty,
                ClaimDataConfigId = entity.ClaimDataConfigId,
                ClaimDataConfig = entity.ClaimDataConfig,
                Quarter = entity.Quarter,
                Status = entity.Status,
                UploadDate = entity.CreatedAt,
                PersonInChargeId = entity.CreatedById,
                PersonInCharge = entity.CreatedBy,
            };
        }

        public Dictionary<string, string> GetOverrideProperties(FormCollection form = null)
        {
            Dictionary<string, object> overrideProperties = new Dictionary<string, object>();
            Dictionary<string, string> overridePropertiesStr = new Dictionary<string, string>();
            Dictionary<string, string> errors = new Dictionary<string, string>();

            string[] properties = Util.GetConfig("ClaimDataOverrideProperties").Split(',').ToArray();

            Dictionary<string, object> currentOverrideProperties = null;
            if (form == null && !string.IsNullOrEmpty(OverridePropertiesStr))
            {
                currentOverrideProperties = JsonConvert.DeserializeObject<Dictionary<string, object>>(OverridePropertiesStr);
            }

            foreach (string property in properties)
            {
                object value = null;
                string valueStr = null;
                int propertyType = int.Parse(property);
                int dataType = StandardClaimDataOutputBo.GetDataTypeByType(propertyType);

                if (form != null)
                {
                    string code = StandardClaimDataOutputBo.GetCodeByType(propertyType);
                    valueStr = form.Get(code);
                    if (!string.IsNullOrEmpty(valueStr))
                    {
                        switch (dataType)
                        {
                            case StandardOutputBo.DataTypeDate:
                                if (Util.TryParseDateTime(valueStr, out DateTime? date, out string _))
                                    value = date;
                                break;
                            case StandardOutputBo.DataTypeAmount:
                            case StandardOutputBo.DataTypePercentage:
                                value = Util.StringToDouble(valueStr);
                                break;
                            case StandardOutputBo.DataTypeInteger:
                                if (int.TryParse(valueStr, out int integer))
                                    value = integer;
                                break;
                            case StandardOutputBo.DataTypeString:
                            case StandardOutputBo.DataTypeDropDown:
                                value = valueStr;
                                break;
                        }
                    }

                    if (ClaimDataBatchBo.RequiredOverrideProperties().Contains(propertyType) && value == null)
                    {
                        string name = StandardClaimDataOutputBo.GetTypeName(propertyType);
                        errors.Add(code, string.Format(MessageBag.Required, name));
                    }
                }
                else if (currentOverrideProperties != null && currentOverrideProperties.ContainsKey(property) && currentOverrideProperties[property] != null)
                {
                    value = currentOverrideProperties[property];
                    valueStr = value.ToString();
                    switch (dataType)
                    {
                        case StandardOutputBo.DataTypeDate:
                            if (Util.TryParseDateTime(valueStr, out DateTime? date, out string _))
                                valueStr = date?.ToString(Util.GetDateFormat());
                            break;
                        case StandardOutputBo.DataTypeAmount:
                        case StandardOutputBo.DataTypePercentage:
                            valueStr = Util.DoubleToString(value);
                            break;
                    }
                }
                else
                {
                    switch (propertyType)
                    {
                        case StandardClaimDataOutputBo.TypeCurrencyCode:
                            value = valueStr = PickListDetailBo.CurrencyCodeMyr;
                            break;
                        case StandardClaimDataOutputBo.TypeCurrencyRate:
                            value = valueStr = Util.DoubleToString(1);
                            break;
                        default:
                            break;
                    }
                }

                overrideProperties.Add(property, value);
                overridePropertiesStr.Add(property, valueStr);
            }
            OverrideProperties = overrideProperties;
            OverridePropertiesStr = JsonConvert.SerializeObject(overridePropertiesStr);

            return errors;
        }

        public object GetHtmlViewData(int dataType, object value)
        {
            string inputClass = "form-control";
            string placeholder = "Type here";
            string type = "text";
            string onclick = "";

            switch (dataType)
            {
                case StandardOutputBo.DataTypeDate:
                    placeholder = "DD MM YYYY";
                    inputClass += " datepicker";
                    onclick = "openDatePicker(this.id)";
                    break;
                case StandardOutputBo.DataTypeAmount:
                case StandardOutputBo.DataTypePercentage:
                    inputClass += " text-right";
                    placeholder = "0.00";
                    break;
                case StandardOutputBo.DataTypeInteger:
                    type = "number";
                    break;
            }

            var viewData = new
            {
                htmlAttributes = new
                {
                    @class = inputClass,
                    placeholder = placeholder,
                    type = type,
                    @Value = value,
                    onclick = onclick
                }
            };

            return viewData;
        }

        public void ProcessStatusHistory(FormCollection form, int authUserId, ref TrailObject trail)
        {
            StatusHistoryBo statusHistoryBo = new StatusHistoryBo
            {
                ModuleId = ModuleId,
                ObjectId = Id,
                Status = Status,
                CreatedById = authUserId,
                UpdatedById = authUserId,
            };
            StatusHistoryService.Save(ref statusHistoryBo, ref trail);

            string statusRemark = form.Get(string.Format("StatusRemark"));
            if (!string.IsNullOrWhiteSpace(statusRemark))
            {
                RemarkBo remarkBo = new RemarkBo
                {
                    ModuleId = ModuleId,
                    ObjectId = Id,
                    Status = Status,
                    Content = statusRemark,
                    CreatedById = authUserId,
                    UpdatedById = authUserId,
                };

                RemarkService.Save(ref remarkBo, ref trail);
            }
        }

        public void ProcessFileUpload(int authUserId, ref TrailObject trail)
        {
            foreach (var uploadItem in Upload)
            {
                if (uploadItem == null)
                    continue;

                RawFileBo rawFileBo = new RawFileBo
                {
                    Type = RawFileBo.TypeClaimData,
                    Status = RawFileBo.StatusPending,
                    FileName = uploadItem.FileName,
                    CreatedById = authUserId,
                    UpdatedById = authUserId,
                };

                rawFileBo.FormatHashFileName();
                string path = rawFileBo.GetLocalPath();
                Util.MakeDir(path);
                uploadItem.SaveAs(path);

                RawFileService.Create(ref rawFileBo, ref trail);

                ClaimDataFileBo claimDataFileBo = new ClaimDataFileBo
                {
                    ClaimDataBatchId = Id,
                    RawFileId = rawFileBo.Id,
                    TreatyId = TreatyId,
                    ClaimDataConfigId = ClaimDataConfigId,
                    ClaimDataFileConfig = ClaimDataFileConfig,
                    OverrideProperties = OverridePropertiesStr,
                    Mode = ClaimDataFileBo.ModeInclude,
                    Status = ClaimDataFileBo.StatusPending,
                    CreatedById = authUserId,
                    UpdatedById = authUserId,
                };

                ClaimDataFileService.Create(ref claimDataFileBo, ref trail);
            }
        }

        public IList<ClaimDataFileBo> GetExistingFile(bool isReload = false)
        {
            IList<ClaimDataFileBo> claimDataFileBos = ClaimDataFileService.GetByClaimDataBatchId(Id);
            if (!isReload)
                return claimDataFileBos;

            var includedFilesArr = string.IsNullOrEmpty(IncludedFiles) ? new List<int>() : IncludedFiles.Split(',').Select(Int32.Parse).ToList();
            foreach (ClaimDataFileBo bo in claimDataFileBos)
            {
                bo.Mode = includedFilesArr.Contains(bo.Id) ? ClaimDataFileBo.ModeInclude : ClaimDataFileBo.ModeExclude;
            }
            return claimDataFileBos;
        }

        public void ProcessExistingFile(int authUserId, ref TrailObject trail)
        {
            IList<ClaimDataFileBo> claimDataFileBos = ClaimDataFileService.GetByClaimDataBatchId(Id);
            var includedFilesArr = string.IsNullOrEmpty(IncludedFiles) ? new List<int>() :  IncludedFiles.Split(',').Select(Int32.Parse).ToList();
            foreach (var bo in claimDataFileBos)
            {
                ClaimDataFileBo claimDataFileBo = bo;
                int mode = includedFilesArr.Contains(claimDataFileBo.Id) ? ClaimDataFileBo.ModeInclude : ClaimDataFileBo.ModeExclude;
                if (claimDataFileBo.Mode == mode)
                    continue;

                claimDataFileBo.Mode = mode;
                claimDataFileBo.UpdatedById = authUserId;
                ClaimDataFileService.Update(ref claimDataFileBo, ref trail);
            }
        }
    }

    public class FileUploadValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            int maxFileSize = Util.GetConfigInteger("ClaimDataMaxFileSize", 2);
            long maxContent = Math.Abs((long)(maxFileSize * Math.Pow(1024, 3)));
            string maxFileSizeError = string.Format("Maximum file size: {0} GB", maxFileSize);

            var model = validationContext.ObjectInstance as ClaimDataBatchViewModel;
            int status = model.Status;
            if (!ClaimDataBatchBo.CanUpload(status))
                return null;

            if (string.IsNullOrEmpty(model.IncludedFiles) && model.Upload[0] == null)
            {
                // exclude all files without upload new file
                if (model.Id != 0)
                    return new ValidationResult("Please upload at least one file if you want to exclude all");

                return new ValidationResult("The Upload File is Required");
            }

            if (model.Upload == null)
                return null;

            foreach (var item in model.Upload)
            {
                if (item == null)
                    continue;

                if (model.ClaimDataConfigId != 0)
                {
                    ClaimDataConfigBo claimDataConfigBo = ClaimDataConfigService.Find(model.ClaimDataConfigId);
                    if (!ValidateFileMimeType(item, claimDataConfigBo.FileType))
                    {
                        if (claimDataConfigBo.FileType == ClaimDataConfigBo.FileTypeExcel)
                            return new ValidationResult("Allowed file of type: .xls, .xlsx, .xlsb, .xlsm");
                        else
                            return new ValidationResult("Allowed file of type: .txt, .csv, .rpt, .pro");
                    }
                }

                if (item.ContentLength > maxContent)
                {
                    return new ValidationResult(maxFileSizeError);
                }
            }
            return null;
        }

        public static bool ValidateFileMimeType(HttpPostedFileBase file, int fileType)
        {
            var excel = new string[]
            {
                "application/vnd.ms-excel",
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "application/vnd.ms-excel.sheet.binary.macroEnabled.12",
                "application/vnd.ms-excel.sheet.macroEnabled.12",
            };
            var plain = new string[]
            {
                "application/octet-stream",
                "text/plain",
                "text/csv",
            };
            var mime = file.ContentType;

            switch (fileType)
            {
                case ClaimDataConfigBo.FileTypeExcel:
                    if (Path.GetExtension(file.FileName) == ".csv")
                        return false;
                    if (excel.Contains(mime))
                        return true;
                    break;
                case ClaimDataConfigBo.FileTypePlainText:
                    if (plain.Contains(mime) || (Path.GetExtension(file.FileName) == ".csv" && excel.Contains(mime)))
                        return true;
                    break;
            }

            return false;
        }
    }

}