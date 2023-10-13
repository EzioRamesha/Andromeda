using Newtonsoft.Json;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Claims
{
    public class ClaimDataMappingBo
    {
        [JsonIgnore]
        public int Id { get; set; }

        [JsonIgnore]
        public int ClaimDataConfigId { get; set; }

        [JsonIgnore]
        public ClaimDataConfigBo ClaimDataConfigBo { get; set; }

        [JsonIgnore]
        public int StandardClaimDataOutputId { get; set; }

        public string StandardClaimDataOutputCode { get; set; }

        [JsonIgnore]
        public StandardClaimDataOutputBo StandardClaimDataOutputBo { get; set; }

        [JsonIgnore]
        public int SortIndex { get; set; }

        // This is for column to row mapping
        public int? Row { get; set; }

        public string RawColumnName { get; set; }

        public int? Length { get; set; }

        public int TransformFormula { get; set; }

        public string TransformFormulaName { get; set; }

        public int? DefaultValueType { get; set; }

        public string DefaultValue { get; set; }

        [JsonIgnore]
        public List<dynamic> DefaultObjectList { get; set; }

        [JsonIgnore]
        public int? DefaultObjectId { get; set; }

        // this is for process file setting
        public int? Col { get; set; }

        [JsonIgnore]
        public int CreatedById { get; set; }

        [JsonIgnore]
        public int? UpdatedById { get; set; }

        public IList<ClaimDataMappingDetailBo> ClaimDataMappingDetailBos { get; set; } = new List<ClaimDataMappingDetailBo>();

        public const int TransformFormulaAuto = 1;
        public const int TransformFormulaInputFormat = 2;
        public const int TransformFormulaNoTransform = 3;
        public const int TransformFormulaSubstring = 4;
        public const int TransformFormulaUsDollar = 5;
        public const int TransformFormulaDivide100 = 6;
        public const int TransformFormulaTimes100 = 7;
        public const int TransformFormulaInputTable = 8;
        public const int TransformFormulaFixedValue = 9;

        public const int TransformFormulaMax = 9;

        public const int DefaultValueString = 1;
        public const int DefaultValueDropDown = 2;
        public const int DefaultValueMappingDetail = 3;

        public const string DisplayName = "Raw Data Mapping";

        public ClaimDataMappingBo()
        {
            Row = 1;
        }

        public static string GetTransformFormulaName(int key)
        {
            switch (key)
            {
                case TransformFormulaAuto:
                    return "Auto";
                case TransformFormulaInputFormat:
                    return "Input Format";
                case TransformFormulaFixedValue:
                    return "Fixed Value";
                case TransformFormulaNoTransform:
                    return "No Transform";
                case TransformFormulaSubstring:
                    return "Substring";
                case TransformFormulaUsDollar:
                    return "US Dollar";
                case TransformFormulaDivide100:
                    return "Divide by 100";
                case TransformFormulaTimes100:
                    return "Remove Percentage Sign";
                case TransformFormulaInputTable:
                    return "Input Table";
                default:
                    return "";
            }
        }

        public static IList<int> GetTransformFormulaListByFieldType(int key)
        {
            IList<int> formulaList = new List<int>() { };
            switch (key)
            {
                case StandardOutputBo.DataTypeDate:
                    formulaList.Add(TransformFormulaAuto);
                    formulaList.Add(TransformFormulaInputFormat);
                    break;
                case StandardOutputBo.DataTypeString:
                    formulaList.Add(TransformFormulaNoTransform);
                    formulaList.Add(TransformFormulaSubstring);
                    break;
                case StandardOutputBo.DataTypeAmount:
                    formulaList.Add(TransformFormulaAuto);
                    formulaList.Add(TransformFormulaUsDollar);
                    formulaList.Add(TransformFormulaDivide100);
                    break;
                case StandardOutputBo.DataTypePercentage:
                    formulaList.Add(TransformFormulaAuto);
                    break;
                case StandardOutputBo.DataTypeInteger:
                    formulaList.Add(TransformFormulaAuto);
                    formulaList.Add(TransformFormulaSubstring);
                    break;
                case StandardOutputBo.DataTypeDropDown:
                    formulaList.Add(TransformFormulaInputTable);
                    break;
                default:
                    break;
            }

            formulaList.Add(TransformFormulaFixedValue);
            return formulaList;
        }

        public static List<ClaimDataMappingBo> GetDefault()
        {
            return new List<ClaimDataMappingBo>
            {
                new ClaimDataMappingBo { StandardClaimDataOutputId = StandardClaimDataOutputBo.TypePolicyNumber },
                new ClaimDataMappingBo { StandardClaimDataOutputId = StandardClaimDataOutputBo.TypeClaimRecoveryAmt },
                new ClaimDataMappingBo { StandardClaimDataOutputId = StandardClaimDataOutputBo.TypeClaimTransactionType },
                new ClaimDataMappingBo { StandardClaimDataOutputId = StandardClaimDataOutputBo.TypeTreatyCode },
                new ClaimDataMappingBo { StandardClaimDataOutputId = StandardClaimDataOutputBo.TypeTreatyType },
                new ClaimDataMappingBo { StandardClaimDataOutputId = StandardClaimDataOutputBo.TypeCedingClaimType },
                new ClaimDataMappingBo { StandardClaimDataOutputId = StandardClaimDataOutputBo.TypeCedingEventCode },
                new ClaimDataMappingBo { StandardClaimDataOutputId = StandardClaimDataOutputBo.TypeDateOfEvent },
                new ClaimDataMappingBo { StandardClaimDataOutputId = StandardClaimDataOutputBo.TypeFundsAccountingTypeCode },
                new ClaimDataMappingBo { StandardClaimDataOutputId = StandardClaimDataOutputBo.TypeInsuredName },
                new ClaimDataMappingBo { StandardClaimDataOutputId = StandardClaimDataOutputBo.TypeReinsBasisCode },
                new ClaimDataMappingBo { StandardClaimDataOutputId = StandardClaimDataOutputBo.TypeReinsEffDatePol },
            };
        }

        public static List<int> GetInputRequired()
        {
            List<int> transformFormulas = new List<int>
            {
                TransformFormulaInputFormat,
                TransformFormulaSubstring,
                TransformFormulaFixedValue,
            };
            return transformFormulas;
        }

        public int? GetDefaultValueType()
        {
            int? defaultValueType = DefaultValueString;
            if (TransformFormula == 0 || StandardClaimDataOutputBo == null)
            {
                defaultValueType = null;
            }
            else if (TransformFormula == TransformFormulaInputTable)
            {
                defaultValueType = DefaultValueMappingDetail;
            }
            else if (StandardClaimDataOutputBo.DataType == StandardOutputBo.DataTypeDropDown || ((StandardClaimDataOutputBo.Type == StandardClaimDataOutputBo.TypeMlreBenefitCode || StandardClaimDataOutputBo.Type == StandardClaimDataOutputBo.TypeTreatyCode) && TransformFormula == TransformFormulaFixedValue))
            {
                defaultValueType = DefaultValueDropDown;
            }

            DefaultValueType = defaultValueType;
            return defaultValueType;
        }

        public bool IsDefaultValuePickList()
        {
            if (StandardClaimDataOutputBo == null)
                return false;

            return StandardClaimDataOutputBo.DataType == StandardOutputBo.DataTypeDropDown && TransformFormula == TransformFormulaFixedValue;
        }

        public bool IsDefaultValueBenefit()
        {
            if (StandardClaimDataOutputBo == null)
                return false;

            return StandardClaimDataOutputBo.Type == StandardClaimDataOutputBo.TypeMlreBenefitCode && TransformFormula == TransformFormulaFixedValue;
        }

        public bool IsDefaultValueTreatyCode()
        {
            if (StandardClaimDataOutputBo == null)
                return false;

            return StandardClaimDataOutputBo.Type == StandardClaimDataOutputBo.TypeTreatyCode && TransformFormula == TransformFormulaFixedValue;
        }

        public bool IsEmpty()
        {
            return RawColumnName == null && Row == 0 && Length == null && StandardClaimDataOutputId == 0;
        }

        public List<string> Validate(ClaimDataFileConfig claimDataFileConfig)
        {
            List<string> errors = new List<string> { };

            if (StandardClaimDataOutputId == 0)
                errors.Add(string.Format(MessageBag.Required, "Standard Output"));

            List<int> transformFormulaRequired = GetInputRequired();

            if (TransformFormula == 0)
            {
                errors.Add(string.Format(MessageBag.Required, "Transform Formula"));
            }
            else if (transformFormulaRequired.Contains(TransformFormula) && string.IsNullOrEmpty(DefaultValue))
            {
                errors.Add(string.Format(MessageBag.Required, "Input Value"));
            }
            else if (TransformFormula == TransformFormulaSubstring)
            {
                DefaultValue = DefaultValue.Replace(" ", string.Empty);
                string[] arguments = DefaultValue.Split(',');

                if (arguments.Length > 2)
                {
                    errors.Add("There is an error with the format of the substring");
                }
                else
                {
                    if (!int.TryParse(arguments[0], out _))
                    {
                        errors.Add("The {start position} of the substring must be a number");
                    }

                    if (arguments.Length > 1 && !int.TryParse(arguments[1], out _))
                    {
                        errors.Add("The {length} of the substring must be a number");
                    }
                }
            }
            else if (TransformFormula == TransformFormulaInputFormat && (!DefaultValue.Contains("M") || !DefaultValue.Contains("y")))
            {
                errors.Add(MessageBag.InvalidDateFormat);
            }

            if (TransformFormula != TransformFormulaFixedValue && claimDataFileConfig.Delimiter != ClaimDataConfigBo.DelimiterFixedLength)
            {
                if (!claimDataFileConfig.HasHeader && !int.TryParse(RawColumnName, out _))
                {
                    errors.Add("Raw Column Name must be numeric when there is no header");
                }
                else if (string.IsNullOrEmpty(RawColumnName))
                {
                    errors.Add(string.Format(MessageBag.Required, "Raw Column Name"));
                }
            }

            if (claimDataFileConfig.IsColumnToRowMapping && Row == null)
                errors.Add(string.Format(MessageBag.Required, "Row"));

            if (claimDataFileConfig.Delimiter == ClaimDataConfigBo.DelimiterFixedLength && Length == null)
                errors.Add(string.Format(MessageBag.Required, "Length"));

            List<string> rawValues = new List<string> { };
            int index = 1;
            foreach (ClaimDataMappingDetailBo claimDataMappingDetailBo in ClaimDataMappingDetailBos)
            {
                List<string> mappingDetailErrors = claimDataMappingDetailBo.Validate(ref rawValues);

                foreach (string error in mappingDetailErrors)
                {
                    errors.Add(string.Format("{0} at #{1} in Input Table", error, index));
                }

                index++;
            }

            return errors;
        }
    }
}
