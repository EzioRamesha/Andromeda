using Newtonsoft.Json;
using Shared;
using Shared.ProcessFile;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Globalization;
using System.Linq;

namespace BusinessObject.Claims
{
    public class ClaimDataBo
    {
        public int Id { get; set; }

        public int ClaimDataBatchId { get; set; }
        public virtual ClaimDataBatchBo ClaimDataBatchBo { get; set; }

        public int? ClaimDataFileId { get; set; }
        public virtual ClaimDataFileBo ClaimDataFileBo { get; set; }

        public string ClaimId { get; set; }
        public string ClaimCode { get; set; }

        public bool CopyAndOverwriteData { get; set; }

        public int MappingStatus { get; set; }
        public int PreComputationStatus { get; set; }
        public int PreValidationStatus { get; set; }
        public int ReportingStatus { get; set; }

        public string Errors { get; set; }
        public dynamic ErrorObject { get; set; }
        public IDictionary<string, object> ErrorDictionary { get; set; }
        public string CustomField { get; set; }
        public dynamic CustomFieldObject { get; set; }
        public IDictionary<string, object> CustomFieldDictionary { get; set; }

        public bool MappingValidate { get; set; }
        public bool FormulaValidate { get; set; }
        public bool TreatyCodeMappingValidate { get; set; }


        [MaxLength(150)]
        public string PolicyNumber { get; set; }
        public double? PolicyTerm { get; set; }

        public double? ClaimRecoveryAmt { get; set; }
        [MaxLength(30)]
        public string ClaimTransactionType { get; set; }

        [MaxLength(35)]
        public string TreatyCode { get; set; }
        [MaxLength(35)]
        public string TreatyType { get; set; }

        public double? AarPayable { get; set; }
        public double? AnnualRiPrem { get; set; }

        [MaxLength(255)]
        public string CauseOfEvent { get; set; }
        [MaxLength(30)]
        public string CedantClaimEventCode { get; set; }
        [MaxLength(30)]
        public string CedantClaimType { get; set; }
        public DateTime? CedantDateOfNotification { get; set; }
        [MaxLength(30)]
        public string CedingBenefitRiskCode { get; set; }
        [MaxLength(30)]
        public string CedingBenefitTypeCode { get; set; }
        [MaxLength(30)]
        public string CedingClaimType { get; set; }
        [MaxLength(30)]
        public string CedingCompany { get; set; }
        [MaxLength(30)]
        public string CedingEventCode { get; set; }
        [MaxLength(30)]
        public string CedingPlanCode { get; set; }

        public double? CurrencyRate { get; set; }
        [MaxLength(3)]
        public string CurrencyCode { get; set; }

        public DateTime? DateApproved { get; set; }
        public DateTime? DateOfEvent { get; set; }

        [MaxLength(30)]
        public string EntryNo { get; set; }
        public double? ExGratia { get; set; }
        public double? ForeignClaimRecoveryAmt { get; set; }
        [MaxLength(30)]
        public string FundsAccountingTypeCode { get; set; }

        public DateTime? InsuredDateOfBirth { get; set; }
        [MaxLength(1)]
        public string InsuredGenderCode { get; set; }
        [MaxLength(128)]
        public string InsuredName { get; set; }
        [MaxLength(1)]
        public string InsuredTobaccoUse { get; set; }

        public DateTime? LastTransactionDate { get; set; }
        [MaxLength(30)]
        public string LastTransactionQuarter { get; set; }
        public double? LateInterest { get; set; }
        public double? Layer1SumRein { get; set; }

        public int? Mfrs17AnnualCohort { get; set; }
        [MaxLength(30)]
        public string Mfrs17ContractCode { get; set; }
        [MaxLength(30)]
        public string MlreBenefitCode { get; set; }
        [MaxLength(30)]
        public string MlreEventCode { get; set; }
        public DateTime? MlreInvoiceDate { get; set; }
        [MaxLength(30)]
        public string MlreInvoiceNumber { get; set; }
        public double? MlreRetainAmount { get; set; }
        public double? MlreShare { get; set; }

        public int? PendingProvisionDay { get; set; }
        public int? PolicyDuration { get; set; }

        [MaxLength(30)]
        public string ReinsBasisCode { get; set; }
        public DateTime? ReinsEffDatePol { get; set; }

        [MaxLength(128)]
        public string RetroParty1 { get; set; }
        [MaxLength(128)]
        public string RetroParty2 { get; set; }
        [MaxLength(128)]
        public string RetroParty3 { get; set; }
        public double? RetroRecovery1 { get; set; }
        public double? RetroRecovery2 { get; set; }
        public double? RetroRecovery3 { get; set; }
        public DateTime? RetroStatementDate1 { get; set; }
        public DateTime? RetroStatementDate2 { get; set; }
        public DateTime? RetroStatementDate3 { get; set; }
        [MaxLength(30)]
        public string RetroStatementId1 { get; set; }
        [MaxLength(30)]
        public string RetroStatementId2 { get; set; }
        [MaxLength(30)]
        public string RetroStatementId3 { get; set; }
        public int? RiskPeriodMonth { get; set; }
        public int? RiskPeriodYear { get; set; }
        [MaxLength(30)]
        public string RiskQuarter { get; set; }
        public double? SaFactor { get; set; }
        [MaxLength(30)]
        public string SoaQuarter { get; set; }
        public double? SumIns { get; set; }
        public double? TempA1 { get; set; }
        public double? TempA2 { get; set; }
        public DateTime? TempD1 { get; set; }
        public DateTime? TempD2 { get; set; }
        public int? TempI1 { get; set; }
        public int? TempI2 { get; set; }
        [MaxLength(150)]
        public string TempS1 { get; set; }
        [MaxLength(50)]
        public string TempS2 { get; set; }
        public DateTime? TransactionDateWop { get; set; }

        public int CreatedById { get; set; }
        public int? UpdatedById { get; set; }

        public DateTime? IssueDatePol { get; set; }

        public DateTime? PolicyExpiryDate { get; set; }

        public DateTime? DateOfReported { get; set; }

        [MaxLength(30)]
        public string CedingTreatyCode { get; set; }

        [MaxLength(10)]
        public string CampaignCode { get; set; }

        public DateTime? DateOfIntimation { get; set; }

        public const int MappingStatusPending = 1;
        public const int MappingStatusSuccess = 2;
        public const int MappingStatusFailed = 3;
        public const int MappingStatusMax = 3;

        public const int PreComputationStatusPending = 1;
        public const int PreComputationStatusSuccess = 2;
        public const int PreComputationStatusFailed = 3;
        public const int PreComputationStatusMax = 3;

        public const int PreValidationStatusPending = 1;
        public const int PreValidationStatusSuccess = 2;
        public const int PreValidationStatusFailed = 3;
        public const int PreValidationStatusMax = 3;

        public const int ReportingStatusPending = 1;
        public const int ReportingStatusSuccess = 2;
        public const int ReportingStatusFailed = 3;
        public const int ReportingStatusMax = 3;

        public const string TreatyCodeMappingTitle = "Computation: Treaty Code Mapping: ";

        public static List<Column> GetColumns()
        {
            var columns = new List<Column>
            {
                new Column
                {
                    Header = "ID",
                    Property = "Id",
                },
                new Column
                {
                    Header = "Mapping Status",
                    Property = "MappingStatus",
                },
                new Column
                {
                    Header = "Pre-Computation Status",
                    Property = "PreComputationStatus",
                },
                new Column
                {
                    Header = "Pre-Validation Status",
                    Property = "PreValidationStatus",
                },
                new Column
                {
                    Header = "Reporting Status",
                    Property = "ReportingStatus",
                },
            };

            // add all standard fields
            for (int i = 1; i <= StandardClaimDataOutputBo.TypeMax; i++)
            {
                columns.Add(new Column
                {
                    Header = StandardClaimDataOutputBo.GetCodeByType(i),
                    Property = StandardClaimDataOutputBo.GetPropertyNameByType(i),
                });
            }

            columns.Add(new Column
            {
                Header = "Errors",
                Property = "Errors",
            });

            return columns;
        }

        public static string GetMappingStatusName(int? key)
        {
            switch (key)
            {
                case MappingStatusPending:
                    return "Pending";
                case MappingStatusSuccess:
                    return "Success";
                case MappingStatusFailed:
                    return "Failed";
                default:
                    return "";
            }
        }

        public static string GetPreComputationStatusName(int? key)
        {
            switch (key)
            {
                case PreComputationStatusPending:
                    return "Pending";
                case PreComputationStatusSuccess:
                    return "Success";
                case PreComputationStatusFailed:
                    return "Failed";
                default:
                    return "";
            }
        }

        public static string GetPreValidationStatusName(int? key)
        {
            switch (key)
            {
                case PreValidationStatusPending:
                    return "Pending";
                case PreValidationStatusSuccess:
                    return "Success";
                case PreValidationStatusFailed:
                    return "Failed";
                default:
                    return "";
            }
        }

        public static string GetReportingStatusName(int? key)
        {
            switch (key)
            {
                case ReportingStatusPending:
                    return "Pending";
                case ReportingStatusSuccess:
                    return "Success";
                case ReportingStatusFailed:
                    return "Failed";
                default:
                    return "";
            }
        }

        public ClaimDataBo()
        {
            ErrorObject = new ExpandoObject();
            ErrorDictionary = (IDictionary<string, object>)ErrorObject;
            CustomFieldObject = new ExpandoObject();
            CustomFieldDictionary = (IDictionary<string, object>)CustomFieldObject;

            CopyAndOverwriteData = false;
            FormulaValidate = true;
            MappingValidate = true;
            TreatyCodeMappingValidate = true;

            MappingStatus = MappingStatusPending;
            PreComputationStatus = PreComputationStatusPending;
            PreValidationStatus = PreValidationStatusPending;
            ReportingStatus = ReportingStatusPending;
        }

        public void SetError(string property, dynamic value)
        {
            ErrorDictionary[property] = value;
            Errors = JsonConvert.SerializeObject(ErrorObject);
        }

        public bool SetCustomField(object value, ClaimDataMappingBo mapping)
        {
            if (value == null)
            {
                CustomFieldDictionary[mapping.RawColumnName] = null;
            }
            else
            {
                switch (Type.GetTypeCode(value.GetType()))
                {
                    case TypeCode.String:
                        CustomFieldDictionary[mapping.RawColumnName] = value;
                        break;
                    default:
                        CustomFieldDictionary[mapping.RawColumnName] = value.ToString();
                        break;
                }
            }
            CustomField = JsonConvert.SerializeObject(CustomFieldObject);
            return true;
        }

        public bool SetClaimData(int datatype, string property, object value, ClaimDataMappingBo mapping = null)
        {
            if (value is string valueStr)
                value = valueStr.Trim();
            switch (datatype)
            {
                // DropDown can be null or empty to do mapping with the PickListDetails
                case StandardOutputBo.DataTypeDropDown:
                    break;
                default:
                    if (value == null || value is string @string && string.IsNullOrEmpty(@string))
                    {
                        this.SetPropertyValue(property, null);
                        return true;
                    }
                    break;
            }

            switch (datatype)
            {
                case StandardOutputBo.DataTypeDate:
                    return SetDate(property, value, mapping);
                case StandardOutputBo.DataTypeString:
                    return SetString(property, value, mapping);
                case StandardOutputBo.DataTypeAmount:
                    return SetAmount(property, value, mapping);
                case StandardOutputBo.DataTypePercentage:
                    return SetPercentange(property, value, mapping);
                case StandardOutputBo.DataTypeInteger:
                    return SetInteger(property, value, mapping);
                case StandardOutputBo.DataTypeDropDown:
                    return SetDropDown(property, value, mapping);
            }
            return false;
        }

        public bool SetDate(string property, object value, ClaimDataMappingBo mapping = null)
        {
            DateTime? date = null;
            if (mapping != null)
            {
                if (mapping.TransformFormula == ClaimDataMappingBo.TransformFormulaFixedValue)
                {
                    // QUESTION: what is user input date format? 2020-06-24? 20200624? 2020/06/24?
                    date = DateTime.Parse(mapping.DefaultValue);
                    this.SetPropertyValue(property, date);
                    return true;
                }
                switch (Type.GetTypeCode(value.GetType()))
                {
                    case TypeCode.Double:
                        if (mapping.TransformFormula == ClaimDataMappingBo.TransformFormulaInputFormat)
                        {
                            CultureInfo provider = CultureInfo.InvariantCulture;
                            date = DateTime.ParseExact(value.ToString(), mapping.DefaultValue, provider);
                        }
                        else
                        {
                            // This is for excel format double variable type
                            date = DateTime.FromOADate((double)value);
                        }
                        break;
                    case TypeCode.String:
                        if (mapping.TransformFormula == ClaimDataMappingBo.TransformFormulaInputFormat)
                        {
                            CultureInfo provider = CultureInfo.InvariantCulture;
                            date = DateTime.ParseExact((string)value, mapping.DefaultValue, provider);
                        }
                        else
                        {
                            date = DateTime.Parse((string)value);
                        }
                        break;
                    case TypeCode.DateTime:
                        date = (DateTime)value;
                        break;
                }
                this.SetPropertyValue(property, date);
            }
            else
            {
                date = DateTime.Parse(value.ToString());
                this.SetPropertyValue(property, date);
            }
            return true;
        }

        public bool SetString(string property, object value, ClaimDataMappingBo mapping = null)
        {
            string s = value?.ToString();
            string output;
            if (mapping != null && !string.IsNullOrEmpty(s))
            {
                if (mapping.TransformFormula == ClaimDataMappingBo.TransformFormulaSubstring)
                {
                    if (string.IsNullOrEmpty(mapping.DefaultValue))
                    {
                        throw new Exception("Substring params is empty");
                    }

                    mapping.DefaultValue.GetSubStringParams(out int start, out int subStringLength);
                    if (s.Length > start)
                    {
                        string sub;
                        if (subStringLength == 0)
                            sub = s.Substring(start);
                        else
                            sub = s.Substring(start, subStringLength);
                        output = sub;
                    }
                    else
                    {
                        throw new Exception(string.Format(MessageBag.SubStringPosition, start, s.Length));
                    }
                }
                else if (mapping.TransformFormula == ClaimDataMappingBo.TransformFormulaFixedValue)
                {
                    output = mapping.DefaultValue;
                }
                else
                {
                    output = s;
                }
            }
            else
            {
                output = s;
            }

            if (output != null)
            {
                int length = output.Length;
                var maxLengthAttr = this.GetAttributeFrom<MaxLengthAttribute>(property);
                if (maxLengthAttr != null && length > 0 && length > maxLengthAttr.Length)
                {
                    switch (property)
                    {
                        case "CauseOfEvent":
                        case "TempS1":
                            output = output.Substring(0, maxLengthAttr.Length);
                            break;
                        default:
                            throw new Exception(string.Format(MessageBag.MaxLength, length, maxLengthAttr.Length));
                    }

                }
            }
            this.SetPropertyValue(property, output);

            return true;
        }

        public bool SetAmount(string property, object value, ClaimDataMappingBo mapping = null)
        {
            string s = value.ToString();
            if (mapping != null)
            {
                if (mapping.TransformFormula == ClaimDataMappingBo.TransformFormulaFixedValue)
                {
                    if (Util.IsValidDouble(mapping.DefaultValue, out double? output, out string error, true))
                    {
                        this.SetPropertyValue(property, output);
                    }
                    else
                    {
                        throw new Exception(error);
                    }
                    return true;
                }

                if (mapping.TransformFormula == ClaimDataMappingBo.TransformFormulaDivide100)
                {
                    if (Util.IsValidDouble(s, out double? output, out string error))
                    {
                        if (output.HasValue)
                        {
                            double d = Util.RoundValue(output.Value / 100);
                            this.SetPropertyValue(property, d);
                        }
                    }
                    else
                    {
                        throw new Exception(error);
                    }
                }
                else
                {
                    if (Util.IsValidDouble(s, out double? output, out string error, true))
                    {
                        this.SetPropertyValue(property, output);
                    }
                    else
                    {
                        throw new Exception(error);
                    }
                }
            }
            else
            {
                if (Util.IsValidDouble(s, out double? output, out string error, true))
                {
                    this.SetPropertyValue(property, output);
                }
                else
                {
                    throw new Exception(error);
                }
            }

            return true;
        }

        public bool SetPercentange(string property, object value, ClaimDataMappingBo mapping = null)
        {
            string s = value.ToString();
            if (mapping != null)
            {
                if (mapping.TransformFormula == ClaimDataMappingBo.TransformFormulaFixedValue)
                {
                    // QUESTION: what is user input format? 90%? 80 (80%)? 10.50%? 10.50 (10.50%)?
                    if (Util.IsValidDouble(mapping.DefaultValue, out double? d, out string e, true))
                    {
                        this.SetPropertyValue(property, d);
                    }
                    else
                    {
                        throw new Exception(e);
                    }
                    return true;
                }

                s = s.Replace('%', '\0');
                if (Util.IsValidDouble(s, out double? output, out string error, true))
                {
                    this.SetPropertyValue(property, output);
                }
                else
                {
                    throw new Exception(error);
                }
            }
            else
            {
                if (Util.IsValidDouble(s, out double? output, out string error, true))
                {
                    this.SetPropertyValue(property, output);
                }
                else
                {
                    throw new Exception(error);
                }
            }
            return true;
        }

        public bool SetInteger(string property, object value, ClaimDataMappingBo mapping = null)
        {
            string s = value.ToString();
            if (mapping != null)
            {
                if (mapping.TransformFormula == ClaimDataMappingBo.TransformFormulaSubstring)
                {
                    if (string.IsNullOrEmpty(mapping.DefaultValue))
                    {
                        throw new Exception("Substring params is empty");
                    }

                    mapping.DefaultValue.GetSubStringParams(out int start, out int subStringLength);
                    if (s.Length > start)
                    {
                        if (subStringLength == 0)
                            s = s.Substring(start);
                        else
                            s = s.Substring(start, subStringLength);

                        if (Int32.TryParse(s, out int integer))
                        {
                            this.SetPropertyValue(property, integer);
                        }
                        else
                        {
                            throw new Exception("The value is not numeric: " + s.ToString());
                        }
                    }
                    else
                    {
                        throw new Exception(string.Format(MessageBag.SubStringPosition, start, s.Length));
                    }
                }
                else if (mapping.TransformFormula == ClaimDataMappingBo.TransformFormulaFixedValue)
                {
                    double d = double.Parse(mapping.DefaultValue);
                    this.SetPropertyValue(property, Convert.ToInt32(d));
                }
                else
                {
                    double d = double.Parse(s);
                    this.SetPropertyValue(property, Convert.ToInt32(d));
                }
            }
            else
            {
                this.SetPropertyValue(property, int.Parse(s));
            }
            return true;
        }

        public bool SetDropDown(string property, object value, ClaimDataMappingBo mapping = null)
        {
            string s = value?.ToString();
            string output;
            if (mapping != null)
            {
                if (mapping.TransformFormula == ClaimDataMappingBo.TransformFormulaFixedValue)
                {
                    output = mapping.DefaultValue;
                }
                else if (mapping.ClaimDataMappingDetailBos != null && mapping.ClaimDataMappingDetailBos.Count > 0)
                {
                    if (string.IsNullOrEmpty(s))
                    {
                        var emptyRawValues = mapping.ClaimDataMappingDetailBos.Where(q => q.IsRawValueEmpty).ToList();
                        if (emptyRawValues.Count > 0)
                        {
                            if (emptyRawValues.Count == 1)
                            {
                                var emptyRawValue = emptyRawValues[0];
                                if (emptyRawValue.IsPickDetailIdEmpty)
                                    output = null;
                                else
                                    output = emptyRawValue.PickListDetailBo.Code;
                            }
                            else
                            {
                                throw new Exception("The Empty Raw Value Details more than one");
                            }
                        }
                        else
                        {
                            output = null;
                        }
                    }
                    else
                    {
                        var detailBos = mapping.ClaimDataMappingDetailBos.Where(q => !q.IsRawValueEmpty && q.RawValue.ToLower() == s.ToLower()).ToList();
                        if (detailBos.Count > 0)
                        {
                            if (detailBos.Count == 1)
                            {
                                var detailBo = detailBos[0];
                                if (detailBo.IsPickDetailIdEmpty)
                                    output = null;
                                else
                                    output = detailBo.PickListDetailBo.Code;
                            }
                            else
                            {
                                throw new Exception("The Mapping Raw Value Details more than one");
                            }
                        }
                        else
                        {
                            throw new Exception(string.Format("The Raw Value not found in Mapping Detail: {0}", s ?? Util.Null));
                        }
                    }
                }
                else
                {
                    throw new Exception("No mapping detail configured");
                }
            }
            else
            {
                output = s;
            }

            if (output != null)
            {
                int length = output.Length;
                var maxLengthAttr = this.GetAttributeFrom<MaxLengthAttribute>(property);
                if (maxLengthAttr != null && length > 0 && length > maxLengthAttr.Length)
                {
                    throw new Exception(string.Format(MessageBag.MaxLength, length, maxLengthAttr.Length));
                }
            }
            this.SetPropertyValue(property, output);

            return true;
        }

        public string FormatDropDownError(string title, int type, string msg)
        {
            return FormatDropDownError(title, StandardClaimDataOutputBo.GetTypeName(type), msg);
        }

        public string FormatDropDownError(string title, string field, string msg)
        {
            return string.Format("{0}{1}: {2}", title, field, msg);
        }

        public string FormatTreatyMappingError(int type, string msg)
        {
            return FormatTreatyMappingError(StandardClaimDataOutputBo.GetTypeName(type), msg);
        }

        public string FormatTreatyMappingError(string field, string msg)
        {
            return string.Format("{0}{1}: {2}", TreatyCodeMappingTitle, field, msg);
        }

        public string FormatTreatyMappingError(string msg)
        {
            return string.Format("{0}{1}", TreatyCodeMappingTitle, msg);
        }

        public List<string> ValidateTreatyMapping()
        {
            // Mapping Columns
            var required = new List<int>
            {
                StandardClaimDataOutputBo.TypeCedingPlanCode,
                StandardClaimDataOutputBo.TypeCedingBenefitTypeCode,
                //StandardClaimDataOutputBo.TypeCedingBenefitRiskCode, // optional field
                //StandardClaimDataOutputBo.TypeCedingTreatyCode, // optional field
                //StandardClaimDataOutputBo.TypeCampaignCode, // optional field
                StandardClaimDataOutputBo.TypeReinsEffDatePol,
                //StandardClaimDataOutputBo.TypeReinsBasisCode, // optional field
            };

            var errors = new List<string> { };
            foreach (int type in required)
            {
                var empty = FormatTreatyMappingError(type, "Empty");
                var property = StandardClaimDataOutputBo.GetPropertyNameByType(type);
                var value = this.GetPropertyValue(property);
                if (value == null)
                    errors.Add(empty);
                else if (value is string @string && string.IsNullOrEmpty(@string))
                    errors.Add(empty);
            }

            TreatyCodeMappingValidate = errors.Count == 0;
            return errors;
        }

        public bool ProcessClaimData(ClaimDataMappingBo mapping, object value)
        {
            var success = true;
            var so = mapping.StandardClaimDataOutputBo;
            if (so == null)
                return success;

            if (so.Type == StandardClaimDataOutputBo.TypeCustomField)
            {
                SetCustomField(value, mapping);
                return success;
            }

            string property = StandardClaimDataOutputBo.GetPropertyNameByType(so.Type);
            try
            {
                SetClaimData(so.DataType, property, value, mapping);
            }
            catch (Exception e)
            {
                success = false;
                MappingValidate = false;
                SetError(property, string.Format("Mapping Error: {0}", e.Message));
            }
            return success;
        }
    }
}
