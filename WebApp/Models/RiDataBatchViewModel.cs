using BusinessObject;
using BusinessObject.Identity;
using BusinessObject.RiDatas;
using DataAccess.Entities;
using DataAccess.Entities.Identity;
using DataAccess.Entities.RiDatas;
using DataAccess.EntityFramework;
using Newtonsoft.Json;
using Services;
using Services.RiDatas;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Models
{
    public class RiDataBatchViewModel : IValidatableObject
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Ceding Company")]
        public int CedantId { get; set; }
        [Display(Name = "Ceding Company")]
        public CedantBo CedantBo { get; set; }
        public virtual Cedant Cedant { get; set; }

        [Display(Name = "Treaty ID")]
        public int? TreatyId { get; set; }
        [Display(Name = "Treaty ID")]
        public TreatyBo TreatyBo { get; set; }
        public virtual Treaty Treaty { get; set; }

        [Required]
        [Display(Name = "Configuration")]
        public int ConfigId { get; set; }
        public RiDataConfigBo RiDataConfigBo { get; set; }
        public virtual RiDataConfig RiDataConfig { get; set; }

        [Display(Name = "Override Properties")]
        public string OverrideProperties { get; set; }

        [Required]
        public string Quarter { get; set; }

        [Display(Name = "Mode")]
        public string Mode { get; set; }

        public int Status { get; set; }

        [Display(Name = "Process Warehouse Status")]
        public int ProcessWarehouseStatus { get; set; }

        [RequiredFileUpload]
        public HttpPostedFileBase[] Upload { get; set; }

        [Display(Name = "Upload Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? UploadDate { get; set; }

        public int ModuleId { get; set; }

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

        [Display(Name = "Column to Row Mapping")]
        public bool IsColumnToRowMapping { get; set; }

        [Display(Name = "Number of Row Mapping")]
        public int? NumberOfRowMapping { get; set; }

        [Display(Name = "Data Correction")]
        public bool IsDataCorrection { get; set; }

        public virtual string DelimiterName { get; set; }

        public virtual string HasHeaderName { get; set; }

        [Required, Display(Name = "Record Type")]
        public int RecordType { get; set; }

        [Display(Name = "Person In-Charge")]
        public int PersonInChargeId { get; set; }

        public UserBo PersonInChargeBo { get; set; }

        public User PersonInCharge { get; set; }

        [Required, Display(Name = "Received Date")]
        public string ReceivedAtStr { get; set; }

        [Display(Name = "SOA Data Matching Status")]
        public string SOADataMatchStatus { get; set; }

        public int? SoaDataBatchId { get; set; }

        public string IgnoreFinaliseIds { get; set; }

        public int TotalMappingFailedStatus { get; set; }
        public int TotalPreComputation1FailedStatus { get; set; }
        public int TotalPreComputation2FailedStatus { get; set; }
        public int TotalPreValidationFailedStatus { get; set; }
        public int TotalPostComputationFailedStatus { get; set; }
        public int TotalPostValidationFailedStatus { get; set; }
        public int TotalFinaliseFailedStatus { get; set; }
        public int TotalProcessWarehouseFailedStatus { get; set; }
        public int TotalConflict { get; set; }

        public RiDataBatchViewModel() {
            Set();
        }

        public RiDataBatchViewModel(RiDataBatchBo riDataBatchBo)
        {
            Set(riDataBatchBo);
        }

        public void Set(RiDataBatchBo riDataBatchBo = null)
        {
            ModuleBo moduleBo = ModuleService.FindByController(ModuleBo.ModuleController.RiData.ToString());
            if (riDataBatchBo != null)
            {
                Controllers.RiDataController.LogHelper("Ri Batch " + riDataBatchBo.Id + " Start (Get)RiDataBatch Data for View");

                Id = riDataBatchBo.Id;
                CedantId = riDataBatchBo.CedantId;
                CedantBo = riDataBatchBo.CedantBo;

                TreatyId = riDataBatchBo.TreatyId;
                TreatyBo = riDataBatchBo.TreatyBo;

                ConfigId = riDataBatchBo.RiDataConfigId;
                RiDataConfigBo = riDataBatchBo.RiDataConfigBo;

                Quarter = riDataBatchBo.Quarter;
                Status = riDataBatchBo.Status;
                ProcessWarehouseStatus = riDataBatchBo.ProcessWarehouseStatus;
                ProcessWarehouseStatus = riDataBatchBo.ProcessWarehouseStatus;
                RecordType = riDataBatchBo.RecordType;
                SoaDataBatchId = riDataBatchBo.SoaDataBatchId;

                ModuleId = moduleBo.Id;

                PersonInChargeId = riDataBatchBo.CreatedById;
                PersonInChargeBo = riDataBatchBo.CreatedByBo;
                ReceivedAtStr = riDataBatchBo.ReceivedAt.ToString(Util.GetDateFormat());

                TotalMappingFailedStatus = riDataBatchBo.TotalMappingFailedStatus;
                TotalPreComputation1FailedStatus = riDataBatchBo.TotalPreComputation1FailedStatus;
                TotalPreComputation2FailedStatus = riDataBatchBo.TotalPreComputation2FailedStatus;
                TotalPreValidationFailedStatus = riDataBatchBo.TotalPreValidationFailedStatus;
                TotalPostComputationFailedStatus = riDataBatchBo.TotalPostComputationFailedStatus;
                TotalPostValidationFailedStatus = riDataBatchBo.TotalPostValidationFailedStatus;
                TotalFinaliseFailedStatus = riDataBatchBo.TotalFinaliseFailedStatus;
                TotalProcessWarehouseFailedStatus = riDataBatchBo.TotalProcessWarehouseFailedStatus;
                TotalConflict = riDataBatchBo.TotalConflict;

                // FileConfig
                Worksheet = riDataBatchBo.RiDataFileConfig.Worksheet;
                Delimiter = riDataBatchBo.RiDataFileConfig.Delimiter;
                HasHeader = riDataBatchBo.RiDataFileConfig.HasHeader;
                HeaderRow = riDataBatchBo.RiDataFileConfig.HeaderRow;
                StartRow = riDataBatchBo.RiDataFileConfig.StartRow;
                EndRow = riDataBatchBo.RiDataFileConfig.EndRow;
                StartColumn = riDataBatchBo.RiDataFileConfig.StartColumn;
                EndColumn = riDataBatchBo.RiDataFileConfig.EndColumn;
                DelimiterName = riDataBatchBo.RiDataFileConfig.DelimiterName;
                HasHeaderName = (riDataBatchBo.RiDataFileConfig.HasHeader == true ? "Yes" : "No");
                IsColumnToRowMapping = riDataBatchBo.RiDataFileConfig.IsColumnToRowMapping;
                NumberOfRowMapping = riDataBatchBo.RiDataFileConfig.NumberOfRowMapping;
                IsDataCorrection = riDataBatchBo.RiDataFileConfig.IsDataCorrection;

                IgnoreFinaliseIds = string.Join(",", RiDataService.GetIdsOfIgnoreFinalise(riDataBatchBo.Id).ToArray());
                Controllers.RiDataController.LogHelper("Ri Batch " + riDataBatchBo.Id + " End (Get)RiDataBatch Data for View");
            }
            else
            {
                Status = RiDataBatchBo.StatusPending;
                ProcessWarehouseStatus = RiDataBatchBo.ProcessWarehouseStatusNotApplicable;
                ModuleId = moduleBo.Id;
                ReceivedAtStr = DateTime.Now.ToString(Util.GetDateFormat());
            }            
        }

        public static Expression<Func<RiDataBatch, RiDataBatchViewModel>> Expression()
        {
            return entity => new RiDataBatchViewModel
            {
                Id = entity.Id,
                CedantId = entity.CedantId,
                Cedant = entity.Cedant,
                TreatyId = entity.TreatyId,
                Treaty = entity.Treaty,
                ConfigId = entity.RiDataConfigId,
                RiDataConfig = entity.RiDataConfig,
                Quarter = entity.Quarter,
                Status = entity.Status,
                ProcessWarehouseStatus = entity.ProcessWarehouseStatus,
                UploadDate = entity.CreatedAt,
                PersonInChargeId = entity.CreatedById,
                PersonInCharge = entity.CreatedBy,
            };
        }

        public string GetOverrideProperties(FormCollection form, ref Result result)
        {
            List<string> errors = new List<string>();
            Dictionary<string, object> overridePropList = new Dictionary<string, object>();   
            
            var config = Util.GetConfig("RiDataOverrideProperties");
            List<int> overrideProps = new List<int> { };
            if (!string.IsNullOrEmpty(config)) { 
                overrideProps = config.Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => int.Parse(x)).ToList(); 
            }

            foreach (int overrideProp in overrideProps)
            {
                int dataType = StandardOutputBo.GetDataTypeByType(overrideProp);
                string fieldName = StandardOutputBo.GetTypeName(overrideProp);
                string codeKey = StandardOutputBo.GetCodeByType(overrideProp);
                string value = form.Get(codeKey);
                object output = null;
                if (!string.IsNullOrEmpty(value))
                {
                    switch (overrideProp)
                    {
                        case StandardOutputBo.TypeReportPeriodMonth:
                        case StandardOutputBo.TypeRiskPeriodMonth:
                            if (!Enumerable.Range(1, 12).Contains(int.Parse(value))) 
                            { 
                                errors.Add(string.Format("{0} must be within 1 - 12", fieldName)); 
                            }
                            break;
                        case StandardOutputBo.TypeRiskPeriodYear:
                        case StandardOutputBo.TypeReportPeriodYear:
                            if (!DateTime.TryParseExact(value, "yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _)) 
                            {
                                errors.Add(string.Format("{0} is not a valid year", fieldName));
                            }
                            break;
                        case StandardOutputBo.TypeCurrencyRate:
                            if (!string.IsNullOrEmpty(value))
                            {
                                if (!Util.IsValidDouble(value, out double? d, out _))
                                {
                                    errors.Add(string.Format("{0} is not a valid amount", fieldName));
                                }
                            }
                            break;
                    }

                    switch (dataType)
                    {
                        case StandardOutputBo.DataTypeDate:
                            Util.TryParseDateTime(value, out DateTime? datetime, out string _);
                            output = datetime;
                            break;
                        case StandardOutputBo.DataTypeString:
                            output = value;
                            break;
                        case StandardOutputBo.DataTypeAmount:
                            output = Util.StringToDouble(value);
                            break;
                        case StandardOutputBo.DataTypePercentage:
                            output = Util.StringToDouble(value);
                            break;
                        case StandardOutputBo.DataTypeInteger:
                            if (int.TryParse(value, out int integer))
                            {
                                output = integer;
                            }
                            break;
                        case StandardOutputBo.DataTypeDropDown:
                            output = value;
                            break;
                    }
                }
                overridePropList.Add(overrideProp.ToString(), output);
            }

            foreach (string error in errors)
            {
                result.AddError(error);
            }
            return JsonConvert.SerializeObject(overridePropList);
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

        public List<RemarkBo> GetRemarks(FormCollection form)
        {
            int index = 0;
            List<RemarkBo> remarkBos = new List<RemarkBo> { };

            while (!string.IsNullOrWhiteSpace(form.Get(string.Format("r.content[{0}]", index))))
            {
                string createdAtStr = form.Get(string.Format("r.createdAtStr[{0}]", index));
                string status = form.Get(string.Format("r.status[{0}]", index));
                string content = form.Get(string.Format("r.content[{0}]", index));
                string id = form.Get(string.Format("remarkId[{0}]", index));

                DateTime dateTime = DateTime.Parse(createdAtStr, new CultureInfo("fr-FR", false));

                RemarkBo remarkBo = new RemarkBo
                {
                    ModuleId = ModuleId,
                    ObjectId = Id,
                    Status = int.Parse(status),
                    Content = content,
                    CreatedAt = dateTime,
                    UpdatedAt = dateTime,
                };

                if (!string.IsNullOrEmpty(id))
                    remarkBo.Id = int.Parse(id);

                remarkBos.Add(remarkBo);

                index++;
            }

            return remarkBos;
        }

        public void ProcessRemark(FormCollection form, int authUserId, ref TrailObject trail)
        {
            List<RemarkBo> remarkBos = GetRemarks(form);

            foreach (RemarkBo bo in remarkBos)  
            {
                RemarkBo remarkBo = bo;
                remarkBo.CreatedById = authUserId;
                remarkBo.UpdatedById = authUserId;

                RemarkService.Save(ref remarkBo, ref trail);
            }
        }

        public void UpdateRiData(int authUserId)
        {
            if (!string.IsNullOrEmpty(IgnoreFinaliseIds))
            {
                var ids = IgnoreFinaliseIds.Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => int.Parse(x)).ToList();
                foreach (int id in ids)
                {
                    RiDataBo ridata = RiDataService.Find(id);
                    if (ridata.IgnoreFinalise == false)
                    {
                        ridata.IgnoreFinalise = true;
                        ridata.UpdatedById = authUserId;
                        RiDataService.Update(ref ridata);
                    }
                }

                var idsA = RiDataService.GetIdsOfIgnoreFinalise(Id);
                var res = idsA.Where(q => !ids.Contains(q)).ToList();
                var a = res;
                if (res.Count > 0)
                {
                    foreach (int id in res)
                    {
                        RiDataBo ridata = RiDataService.Find(id);
                        ridata.IgnoreFinalise = false;
                        ridata.UpdatedById = authUserId;
                        RiDataService.Update(ref ridata);
                    }
                }
                //db.Database.ExecuteSqlCommand("UPDATE [RiData] SET [IgnoreFinalise] = {0}, [UpdatedById] = {1}, [UpdatedAt] = {2} WHERE [Id] IN ({3})",
                //        true, authUserId, DateTime.Now, ids);
                //db.SaveChanges();
            }
            else
            {
                var idsA = RiDataService.GetIdsOfIgnoreFinalise(Id);
                if (idsA.Count > 0)
                {
                    using (var db = new AppDbContext(false))
                    {
                        db.Database.ExecuteSqlCommand("UPDATE [RiData] SET [IgnoreFinalise] = {0}, [UpdatedById] = {1}, [UpdatedAt] = {2} WHERE [RiDataBatchId] = {3}",
                            false, authUserId, DateTime.Now, Id);
                        db.SaveChanges();
                    }
                }
            }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> results = new List<ValidationResult>();

            if (FileType == RiDataConfigBo.FileTypeExcel && string.IsNullOrEmpty(Worksheet))
            {
                results.Add(new ValidationResult(
                    string.Format(MessageBag.Required, "The Worksheet Field"),
                    new[] { nameof(Worksheet) }));
            }
            else if (FileType == RiDataConfigBo.FileTypePlainText && Delimiter == null)
            {
                results.Add(new ValidationResult(
                    string.Format(MessageBag.Required, "The Delimiter Field"),
                    new[] { nameof(Delimiter) }));
            }

            if (HasHeader == true && HeaderRow == null)
            {
                results.Add(new ValidationResult(
                    string.Format(MessageBag.Required, "The Header Row Field"),
                    new[] { nameof(HeaderRow) }));
            }
            else if (StartRow != null && HeaderRow != null && HeaderRow >= StartRow)
            {
                results.Add(new ValidationResult(
                "The Header Row must be lower than The Start Row Field",
                new[] { nameof(HeaderRow) }));
            }

            if (EndRow != null && StartRow != null && EndRow < StartRow)
            {
                results.Add(new ValidationResult(
                "The Start Row Field must be lower than The End Row Field",
                new[] { nameof(StartRow) }));
                results.Add(new ValidationResult(
                "The End Row must be greater than The Start Row Field",
                new[] { nameof(EndRow) }));
            }

            if (EndColumn != null && StartColumn != null && EndColumn < StartColumn)
            {
                results.Add(new ValidationResult(
                "The Start Column Field must be lower than The End Column Field",
                new[] { nameof(StartColumn) }));
                results.Add(new ValidationResult(
                "The End Column must be greater than The Start Column Field",
                new[] { nameof(EndColumn) }));
            }

            if (IsColumnToRowMapping == true && NumberOfRowMapping == null)
            {
                results.Add(new ValidationResult(
                string.Format(MessageBag.Required, "The Number of Row Mapping Field"),
                    new[] { nameof(NumberOfRowMapping) }));
            }

            if (!SoaDataBatchId.HasValue)
            {
                results.Add(new ValidationResult(
                    string.Format(MessageBag.Required, "SOA Data Matching"),
                    new[] { nameof(SoaDataBatchId) }));
            }

            return results;
        }
    }

    public class RequiredFileUpload : ValidationAttribute  
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            int maxFileSize = Util.GetConfigInteger("RiDataMaxFileSize", 2);
            long maxContent = Math.Abs((long)(maxFileSize * Math.Pow(1024, 3)));
            string maxFileSizeError = string.Format("Maximum file size: {0} GB", maxFileSize);
       
            int[] status = new int[] { RiDataBatchBo.StatusPending, RiDataBatchBo.StatusPreSuccess, RiDataBatchBo.StatusPreFailed, RiDataBatchBo.StatusPostSuccess, RiDataBatchBo.StatusPostFailed };

            var rawFileModel = validationContext.ObjectInstance as RiDataBatchViewModel;
            if (rawFileModel.Id != 0 && rawFileModel.Status != 0)   // for edit
            {
                if (status.Contains(rawFileModel.Status))
                {
                    var fileHistories = RiDataFileService.GetByRiDataBatchId(rawFileModel.Id);
                    string[] checkExclude = (string.IsNullOrEmpty(rawFileModel.Mode) ? new string[0] : rawFileModel.Mode.Split(','));

                    if (fileHistories.Count() == checkExclude.Length && rawFileModel.Upload[0] == null)
                    {
                        // exclude all files without upload new file
                        return new ValidationResult("Please upload at least one file if you want to exclude all");
                    }
                    else if (rawFileModel.Upload != null)
                    {
                        foreach (var item in rawFileModel.Upload)
                        {
                            if (item != null)
                            {
                                if (rawFileModel.ConfigId != 0)
                                {
                                    var fileType = RiDataConfigService.Find(rawFileModel.ConfigId);
                                    if (ValidateFileMimeType(item, fileType.FileType) == false)
                                    {
                                        if (fileType.FileType == RiDataConfigBo.FileTypeExcel)
                                            return new ValidationResult("Allowed file of type: .xls, .xlsx, .xlsb, .xlsm");
                                        else
                                            return new ValidationResult("Allowed file of type: .txt, .csv, .rpt, .pro");
                                    }
                                }
                                else if (item.ContentLength > maxContent)
                                {
                                    return new ValidationResult(maxFileSizeError);
                                }
                                else
                                    return null;
                            }
                            return null;
                        }
                    }
                }
            }
            else    // for create new
            {
                if (rawFileModel.Upload != null)
                {
                    foreach (var item in rawFileModel.Upload)
                    {
                        if (item == null)
                        {
                            return new ValidationResult("The Upload File is Required");
                        }
                        else
                        {                            
                            if (rawFileModel.ConfigId != 0)
                            {
                                var fileType = RiDataConfigService.Find(rawFileModel.ConfigId);
                                if (ValidateFileMimeType(item, fileType.FileType) == false)
                                {
                                    if (fileType.FileType == RiDataConfigBo.FileTypeExcel)
                                        return new ValidationResult("Allowed file of type: .xls, .xlsx, .xlsb, .xlsm");
                                    else
                                        return new ValidationResult("Allowed file of type: .txt, .csv, .rpt, .pro");
                                }
                            }                            
                            else if (item.ContentLength > maxContent)
                            {
                                return new ValidationResult(maxFileSizeError);
                            }
                            else
                                return null;
                        }
                    }
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
                case RiDataConfigBo.FileTypeExcel:
                    if (Path.GetExtension(file.FileName) == ".csv")
                        return false;
                    if (excel.Contains(mime))
                        return true;
                    break;
                case RiDataConfigBo.FileTypePlainText:
                    if (plain.Contains(mime) || (Path.GetExtension(file.FileName) == ".csv" && excel.Contains(mime)))
                        return true;
                    break;
            }

            return false;
        }
    }
}