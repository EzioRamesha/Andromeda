using Newtonsoft.Json;
using Shared;
using System;
using System.Collections.Generic;

namespace BusinessObject.RiDatas
{
    public class RiDataMappingBo
    {
        [JsonIgnore]
        public int Id { get; set; }

        [JsonIgnore]
        public int RiDataConfigId { get; set; }

        [JsonIgnore]
        public RiDataConfigBo RiDataConfigBo { get; set; }

        [JsonIgnore]
        public int StandardOutputId { get; set; }

        public string StandardOutputCode { get; set; }

        [JsonIgnore]
        public StandardOutputBo StandardOutputBo { get; set; }

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

        public IList<RiDataMappingDetailBo> RiDataMappingDetailBos { get; set; } = new List<RiDataMappingDetailBo>();

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
        public const int DefaultValueBoolean = 4;

        public const string DisplayName = "Raw Data Mapping";

        public RiDataMappingBo()
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
                case StandardOutputBo.DataTypeBoolean:
                    formulaList.Add(TransformFormulaAuto);
                    break;
                default:
                    break;
            }

            formulaList.Add(TransformFormulaFixedValue);
            return formulaList;
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

        public static List<int> GetRequiredStandardOutputs()
        {
            List<int> standardOutputs = new List<int>
            {
                StandardOutputBo.TypeTreatyCode,
                StandardOutputBo.TypePremiumFrequencyCode,
                StandardOutputBo.TypeRiskPeriodMonth,
                StandardOutputBo.TypeRiskPeriodYear,
            };
            return standardOutputs;
        }

        public static List<RiDataMappingBo> GetDefault()
        {
            return new List<RiDataMappingBo>
            {
                new RiDataMappingBo { StandardOutputId = StandardOutputBo.TypeTreatyCode },
                new RiDataMappingBo { StandardOutputId = StandardOutputBo.TypeReinsBasisCode },
                new RiDataMappingBo { StandardOutputId = StandardOutputBo.TypeFundsAccountingTypeCode },
                new RiDataMappingBo { StandardOutputId = StandardOutputBo.TypePremiumFrequencyCode },
                new RiDataMappingBo { StandardOutputId = StandardOutputBo.TypeReportPeriodMonth },
                new RiDataMappingBo { StandardOutputId = StandardOutputBo.TypeReportPeriodYear },
                new RiDataMappingBo { StandardOutputId = StandardOutputBo.TypeRiskPeriodMonth },
                new RiDataMappingBo { StandardOutputId = StandardOutputBo.TypeRiskPeriodYear },
                new RiDataMappingBo { StandardOutputId = StandardOutputBo.TypeTransactionTypeCode },
                new RiDataMappingBo { StandardOutputId = StandardOutputBo.TypePolicyNumber },
                new RiDataMappingBo { StandardOutputId = StandardOutputBo.TypeReinsEffDatePol },
                new RiDataMappingBo { StandardOutputId = StandardOutputBo.TypeCedingBenefitTypeCode },
                new RiDataMappingBo { StandardOutputId = StandardOutputBo.TypeMlreBenefitCode },
                new RiDataMappingBo { StandardOutputId = StandardOutputBo.TypeAar },
                new RiDataMappingBo { StandardOutputId = StandardOutputBo.TypeInsuredName },
                new RiDataMappingBo { StandardOutputId = StandardOutputBo.TypeInsuredGenderCode },
                new RiDataMappingBo { StandardOutputId = StandardOutputBo.TypeInsuredDateOfBirth },
                new RiDataMappingBo { StandardOutputId = StandardOutputBo.TypeInsuredAttainedAge },
                new RiDataMappingBo { StandardOutputId = StandardOutputBo.TypeRateTable },
                new RiDataMappingBo { StandardOutputId = StandardOutputBo.TypeUnderwriterRating },
                new RiDataMappingBo { StandardOutputId = StandardOutputBo.TypeGrossPremium },
                new RiDataMappingBo { StandardOutputId = StandardOutputBo.TypeNetPremium },
                new RiDataMappingBo { StandardOutputId = StandardOutputBo.TypeTerritoryOfIssueCode },
                new RiDataMappingBo { StandardOutputId = StandardOutputBo.TypeCurrencyCode },
                new RiDataMappingBo { StandardOutputId = StandardOutputBo.TypeTreatyType },
            };
        }

        public int? GetDefaultValueType()
        {
            int? defaultValueType = null;

            if (TransformFormula == 0)
            {
                defaultValueType = null;
            }
            else if (TransformFormula == TransformFormulaInputTable)
            {
                defaultValueType = DefaultValueMappingDetail;
            }
            else if (StandardOutputBo.DataType == StandardOutputBo.DataTypeBoolean) 
            {
                defaultValueType = DefaultValueBoolean;
            }
            else if (StandardOutputBo.DataType == StandardOutputBo.DataTypeDropDown || ((StandardOutputBo.Type == StandardOutputBo.TypeMlreBenefitCode || StandardOutputBo.Type == StandardOutputBo.TypeTreatyCode) && TransformFormula == TransformFormulaFixedValue))
            {
                defaultValueType = DefaultValueDropDown;
            }
            else if (StandardOutputBo.Type != StandardOutputBo.TypeCustomField)
            {
                defaultValueType = DefaultValueString;
            }

            DefaultValueType = defaultValueType;
            return defaultValueType;
        }

        public bool IsDefaultValuePickList()
        {
            if (StandardOutputBo == null)
                return false;

            return StandardOutputBo.DataType == StandardOutputBo.DataTypeDropDown && TransformFormula == TransformFormulaFixedValue; 
        } 
        
        public bool IsDefaultValueBenefit()
        {
            if (StandardOutputBo == null)
                return false;

            return StandardOutputBo.Type == StandardOutputBo.TypeMlreBenefitCode && TransformFormula == TransformFormulaFixedValue; 
        } 
        
        public bool IsDefaultValueTreatyCode()
        {
            if (StandardOutputBo == null)
                return false;

            return StandardOutputBo.Type == StandardOutputBo.TypeTreatyCode && TransformFormula == TransformFormulaFixedValue; 
        } 
        
        public bool IsEmpty()
        {
            return RawColumnName == null && Row == 0 && Length == null && StandardOutputId == 0;
        } 

        public List<string> Validate()
        {
            List<string> errors = new List<string> { };

            if (StandardOutputId == 0)
                errors.Add(string.Format(MessageBag.Required, "Standard Output"));

            if (TransformFormula == 0 && StandardOutputBo != null && StandardOutputBo.Type != StandardOutputBo.TypeCustomField)
                errors.Add(string.Format(MessageBag.Required, "Transform Formula"));

            List<int> transformFormulaRequired = GetInputRequired();

            if (transformFormulaRequired.Contains(TransformFormula) && string.IsNullOrEmpty(DefaultValue))
            {
                errors.Add(string.Format(MessageBag.Required, "Input Value"));
            }
            else if (TransformFormula == TransformFormulaInputFormat && (!DefaultValue.Contains("M") || !DefaultValue.Contains("y")))
            {
                errors.Add(MessageBag.InvalidDateFormat);
            }

            if (TransformFormula == TransformFormulaSubstring)
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

            return errors;
        }

        public StandardOutputBo GetStandardOutputBo()
        {
            StandardOutputBo = StandardOutputBo.GetByType(StandardOutputBo.Type);
            return StandardOutputBo;
        }
    }
}
