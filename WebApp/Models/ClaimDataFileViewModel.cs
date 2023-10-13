using BusinessObject;
using BusinessObject.Claims;
using BusinessObject.Identity;
using DataAccess.Entities.Identity;
using Newtonsoft.Json;
using Services.Claims;
using Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Models
{
    public class ClaimDataFileViewModel
    {
        // Header Values
        public int Id { get; set; }

        public int Status { get; set; }

        public int ClaimDataBatchId { get; set; }

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
        public int? ClaimDataConfigId { get; set; }

        public ClaimDataConfigBo ClaimDataConfigBo { get; set; }

        [Required]
        public string Quarter { get; set; }

        public int? CurrencyCodeId { get; set; }

        public PickListDetailBo CurrencyCodeBo { get; set; }

        [Display(Name = "Currency Conversion Rate")]
        public double? CurrencyRate { get; set; }

        [Display(Name = "Person In-Charge")]
        public int PersonInChargeId { get; set; }

        public UserBo PersonInChargeBo { get; set; }

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

        // Override Properties
        [Display(Name = "Override Properties")]
        public string OverridePropertiesStr { get; set; }

        [Display(Name = "Override Properties")]
        public Dictionary<string, object> OverrideProperties { get; set; }

        public ClaimDataFileViewModel() { Set(); }

        public ClaimDataFileViewModel(ClaimDataFileBo claimDataFileBo)
        {
            Set(claimDataFileBo);
        }

        public void Set(ClaimDataFileBo claimDataFileBo = null)
        {
            if (claimDataFileBo != null)
            {
                Id = claimDataFileBo.Id;
                CedantId = claimDataFileBo.ClaimDataBatchBo.CedantId;
                TreatyId = claimDataFileBo.ClaimDataBatchBo.TreatyId;
                ClaimDataBatchId = claimDataFileBo.ClaimDataBatchId;
                ClaimDataConfigId = claimDataFileBo.ClaimDataConfigId;
                PersonInChargeId = claimDataFileBo.CreatedById;
                CurrencyCodeId = claimDataFileBo.CurrencyCodeId;
                CurrencyRate = claimDataFileBo.CurrencyRate;
                Quarter = claimDataFileBo.ClaimDataBatchBo.Quarter;
                Status = claimDataFileBo.Status;
                OverridePropertiesStr = claimDataFileBo.OverrideProperties;

                CedantBo = claimDataFileBo.ClaimDataBatchBo.CedantBo;
                TreatyBo = claimDataFileBo.ClaimDataBatchBo.TreatyBo;
                ClaimDataConfigBo = claimDataFileBo.ClaimDataConfigBo;
                PersonInChargeBo = claimDataFileBo.CreatedByBo;
                CurrencyCodeBo = claimDataFileBo.CurrencyCodeBo;

                FileType = claimDataFileBo.ClaimDataConfigBo.FileType;
                Worksheet = claimDataFileBo.ClaimDataFileConfig.Worksheet;
                Delimiter = claimDataFileBo.ClaimDataFileConfig.Delimiter;
                HasHeader = claimDataFileBo.ClaimDataFileConfig.HasHeader;
                HeaderRow = claimDataFileBo.ClaimDataFileConfig.HeaderRow;
                StartRow = claimDataFileBo.ClaimDataFileConfig.StartRow;
                EndRow = claimDataFileBo.ClaimDataFileConfig.EndRow;
                StartColumn = claimDataFileBo.ClaimDataFileConfig.StartColumn;
                EndColumn = claimDataFileBo.ClaimDataFileConfig.EndColumn;
                ClaimDataFileConfig = claimDataFileBo.ClaimDataFileConfig;
            }
        }

        public void Get(ref ClaimDataFileBo claimDataFileBo)
        {
            ClaimDataConfigBo claimDataConfigBo = ClaimDataConfigService.Find(ClaimDataConfigId);
            claimDataFileBo.Id = Id;
            claimDataFileBo.TreatyId = TreatyId;
            claimDataFileBo.ClaimDataConfigId = ClaimDataConfigId;
            claimDataFileBo.CreatedById = PersonInChargeId;
            claimDataFileBo.CurrencyCodeId = CurrencyCodeId;
            claimDataFileBo.CurrencyRate = CurrencyRate;
            claimDataFileBo.Status = Status;
            claimDataFileBo.OverrideProperties = OverridePropertiesStr;

            claimDataFileBo.ClaimDataFileConfig = claimDataConfigBo.ClaimDataFileConfig;
            claimDataFileBo.ClaimDataFileConfig.Worksheet = Worksheet;
            claimDataFileBo.ClaimDataFileConfig.Delimiter = Delimiter;
            claimDataFileBo.ClaimDataFileConfig.HasHeader = HasHeader;
            claimDataFileBo.ClaimDataFileConfig.HeaderRow = HeaderRow;
            claimDataFileBo.ClaimDataFileConfig.StartRow = StartRow;
            claimDataFileBo.ClaimDataFileConfig.EndRow = EndRow;
            claimDataFileBo.ClaimDataFileConfig.StartColumn = StartColumn;
            claimDataFileBo.ClaimDataFileConfig.EndColumn = EndColumn;

            ClaimDataFileConfig = claimDataFileBo.ClaimDataFileConfig;
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
                int dataType = StandardClaimDataOutputBo.GetDataTypeByType(int.Parse(property));

                if (form != null)
                {
                    string code = StandardClaimDataOutputBo.GetCodeByType(int.Parse(property));
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

                    if (int.Parse(property) == StandardClaimDataOutputBo.TypeCedantDateOfNotification && value == null)
                    {
                        errors.Add(code, string.Format(MessageBag.Required, code));
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
    }
}