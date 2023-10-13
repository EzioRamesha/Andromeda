using BusinessObject;
using BusinessObject.Identity;
using BusinessObject.RiDatas;
using DataAccess.Entities;
using DataAccess.Entities.Identity;
using DataAccess.Entities.RiDatas;
using Newtonsoft.Json;
using Services.RiDatas;
using Shared;
using Shared.DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace WebApp.Models
{
    public class RiDataFileViewModel : IValidatableObject
    {
        public int Id { get; set; }

        public int RiDataBatchId { get; set; }

        [Required]
        [Display(Name = "Ceding Company")]
        public int CedantId { get; set; }

        [Display(Name = "Ceding Company")]
        public CedantBo CedantBo { get; set; }

        [Display(Name = "Treaty")]
        public int? TreatyId { get; set; }

        [Display(Name = "Treaty")]
        public TreatyBo TreatyBo { get; set; }

        [Required]
        [Display(Name = "Configuration")]
        public int? ConfigId { get; set; }

        public RiDataConfigBo RiDataConfigBo { get; set; }

        [Display(Name = "Override Properties")]
        public string OverrideProperties { get; set; }

        [Required]
        public string Quarter { get; set; }

        public int Status { get; set; }

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

        [Display(Name = "Received Date")]
        public string ReceivedAtStr { get; set; }

        public virtual Cedant Cedant { get; set; }

        public virtual Treaty Treaty { get; set; }

        public virtual RiDataConfig riDataConfig { get; set; }

        public RiDataFileViewModel() { Set(); }

        public RiDataFileViewModel(RiDataFileBo riDataFileBo)
        {
            Set(riDataFileBo);
        }

        public void Set(RiDataFileBo riDataFileBo = null)
        {
            if (riDataFileBo != null)
            {
                Id = riDataFileBo.Id;
                RiDataBatchId = riDataFileBo.RiDataBatchId;
                CedantId = riDataFileBo.RiDataBatchBo.CedantId;
                TreatyId = riDataFileBo.RiDataBatchBo.TreatyId;
                ConfigId = riDataFileBo.RiDataConfigId;
                Quarter = riDataFileBo.RiDataBatchBo.Quarter;
                Status = riDataFileBo.Status;
                RecordType = riDataFileBo.RecordType;

                CedantBo = riDataFileBo.RiDataBatchBo.CedantBo;
                TreatyBo = riDataFileBo.RiDataBatchBo.TreatyBo;
                RiDataConfigBo = riDataFileBo.RiDataConfigBo;

                FileType = riDataFileBo.RiDataConfigBo.FileType;
                Worksheet = riDataFileBo.RiDataFileConfig.Worksheet;
                Delimiter = riDataFileBo.RiDataFileConfig.Delimiter;
                HasHeader = riDataFileBo.RiDataFileConfig.HasHeader;
                HeaderRow = riDataFileBo.RiDataFileConfig.HeaderRow;
                StartRow = riDataFileBo.RiDataFileConfig.StartRow;
                EndRow = riDataFileBo.RiDataFileConfig.EndRow;
                StartColumn = riDataFileBo.RiDataFileConfig.StartColumn;
                EndColumn = riDataFileBo.RiDataFileConfig.EndColumn;
                DelimiterName = riDataFileBo.RiDataFileConfig.DelimiterName;
                HasHeaderName = (riDataFileBo.RiDataFileConfig.HasHeader == true ? "Yes" : "No");

                RiDataConfigBo riDataConfigBo = RiDataConfigService.Find(riDataFileBo.RiDataConfigId);
                IsColumnToRowMapping = riDataConfigBo.RiDataFileConfig.IsColumnToRowMapping;
                NumberOfRowMapping = riDataConfigBo.RiDataFileConfig.NumberOfRowMapping;
                IsDataCorrection = riDataConfigBo.RiDataFileConfig.IsDataCorrection;

                PersonInChargeId = riDataFileBo.RiDataBatchBo.CreatedById;
                PersonInChargeBo = riDataFileBo.RiDataBatchBo.CreatedByBo;
                ReceivedAtStr = riDataFileBo.RiDataBatchBo.ReceivedAt.ToString(Util.GetDateFormat());
            }
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

            foreach (var overrideProp in overrideProps)
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

            return results;
        }
    }
}