using BusinessObject.Claims;
using BusinessObject.RiDatas;
using DynamicExpresso;
using Shared;
using System;
using System.Collections.Generic;

namespace BusinessObject
{
    public class StandardClaimDataOutputEval
    {
        public ClaimDataBo ClaimDataBo { get; set; }

        public ClaimDataBo OriginalClaimDataBo { get; set; }

        public ClaimRegisterBo ClaimRegisterBo { get; set; }

        public ClaimRegisterBo OriginalClaimRegisterBo { get; set; }

        public RiDataBo RiDataBo { get; set; }

        public RiDataWarehouseBo RiDataWarehouseBo { get; set; }

        public string Condition { get; set; }

        public string Formula { get; set; }

        public string FormattedCondition { get; set; }

        public string FormattedFormula { get; set; }

        public bool Validate { get; set; } = false;

        public bool Nullable { get; set; } = false;

        public bool EnableRiData { get; set; } = false;

        public bool EnableOriginal { get; set; } = false;

        public string Quarter { get; set; }

        public List<string> Errors { get; set; } = new List<string> { };

        public const string DelimiterStart = "{";
        public const string DelimiterEnd = "}";
        public const char EscapeCharacter = '\\';
        public const string RiDataIndicator = "RIDATA.";
        public const string OriginalIndicator = "ORI.";

        public const string QuarterPattern = @"^[0-9]{4}\s[Q]{1}[1-4]{1}$";

        public static List<StandardClaimDataOutputBo> GetVariables(string script, bool enableRiData = false, bool enableOriginal = false)
        {
            List<StandardClaimDataOutputBo> standardClaimDataOutputBos = new List<StandardClaimDataOutputBo>();
            if (string.IsNullOrEmpty(script))
                return standardClaimDataOutputBos;

            bool exit = false;
            int currentIndex = 0;
            while (!exit)
            {
                int indexStart = script.IndexOf(DelimiterStart, currentIndex);
                int indexEnd = script.IndexOf(DelimiterEnd, currentIndex);
                int nextIndexStart = script.IndexOf(DelimiterStart, indexStart + 1);

                if (indexStart == -1 && indexEnd == -1)
                    break;

                if (indexStart > 0 && script[indexStart - 1] == EscapeCharacter)
                {
                    currentIndex = indexStart + 1;
                    continue;
                }

                if (indexEnd > 0 && script[indexEnd - 1] == EscapeCharacter)
                {
                    currentIndex = indexEnd + 1;
                    continue;
                }

                if (indexEnd == -1 || (nextIndexStart != -1 && nextIndexStart < indexEnd))
                {
                    throw new Exception("Missing closing delimiter '}' for variables");
                }

                if (indexStart == -1 || indexStart > indexEnd)
                {
                    throw new Exception("Missing opening delimiter '{' for variables");
                }

                string codeName = script.Substring(indexStart + DelimiterStart.Length, indexEnd - indexStart - DelimiterEnd.Length);
                int? soType;
                if (enableRiData && codeName.StartsWith(RiDataIndicator))
                {
                    soType = StandardOutputBo.GetTypeByCode(codeName.Substring(RiDataIndicator.Length), false);
                }
                else if (enableOriginal && codeName.StartsWith(OriginalIndicator))
                {
                    soType = StandardClaimDataOutputBo.GetTypeByCode(codeName.Substring(OriginalIndicator.Length), false);
                }
                else
                {
                    soType = StandardClaimDataOutputBo.GetTypeByCode(codeName, false);
                }

                if (soType == null)
                {
                    throw new Exception(string.Format("Standard Output {0} not found", codeName));
                }

                int dataType;
                bool isRiData = false;
                bool isOriginal = false;
                if (codeName.StartsWith(RiDataIndicator))
                {
                    dataType = StandardOutputBo.GetDataTypeByType(soType.Value);
                    isRiData = true;
                }
                else
                {
                    dataType = StandardClaimDataOutputBo.GetDataTypeByType(soType.Value);
                    if (codeName.StartsWith(OriginalIndicator))
                    {
                        isOriginal = true;
                    }
                }

                StandardClaimDataOutputBo standardClaimDataOutputBo = new StandardClaimDataOutputBo()
                {
                    Code = codeName,
                    Type = soType.Value,
                    DataType = dataType,
                    DataTypeName = StandardClaimDataOutputBo.GetDataTypeName(dataType),
                    DummyValue = "",
                    IsRiData = isRiData,
                    IsOriginal = isOriginal,
                };
                standardClaimDataOutputBos.Add(standardClaimDataOutputBo);

                string replaceField = string.Format("{0}{1}{2}", DelimiterStart, codeName, DelimiterEnd);
                script = script.Replace(replaceField, "");

                currentIndex = indexStart;
            }

            return standardClaimDataOutputBos;
        }

        public string Format(string script)
        {
            if (string.IsNullOrEmpty(script))
                return script;

            bool exit = false;
            int currentIndex = 0;
            while (!exit)
            {
                int indexStart = script.IndexOf(DelimiterStart, currentIndex);
                int indexEnd = script.IndexOf(DelimiterEnd, currentIndex);
                int nextIndexStart = script.IndexOf(DelimiterStart, indexStart + 1);

                if (indexStart == -1 && indexEnd == -1)
                    break;

                if (indexStart > 0 && script[indexStart - 1] == EscapeCharacter)
                {
                    currentIndex = indexStart + 1;
                    continue;
                }

                if (indexEnd > 0 && script[indexEnd - 1] == EscapeCharacter)
                {
                    currentIndex = indexEnd + 1;
                    continue;
                }

                if (indexEnd == -1 || (nextIndexStart != -1 && nextIndexStart < indexEnd))
                {
                    throw new Exception("Missing closing delimiter '}' for variables");
                }

                if (indexStart == -1 || indexStart > indexEnd)
                {
                    throw new Exception("Missing opening delimiter '{' for variables");
                }

                string codeName = script.Substring(indexStart + DelimiterStart.Length, indexEnd - indexStart - DelimiterEnd.Length);
                int? soType;
                if (EnableRiData && codeName.StartsWith(RiDataIndicator))
                {
                    soType = StandardOutputBo.GetTypeByCode(codeName.Substring(RiDataIndicator.Length), false);
                }
                else if (EnableOriginal && codeName.StartsWith(OriginalIndicator))
                {
                    soType = StandardClaimDataOutputBo.GetTypeByCode(codeName.Substring(OriginalIndicator.Length), false);
                }
                else
                {
                    soType = StandardClaimDataOutputBo.GetTypeByCode(codeName, false);
                }

                dynamic value;

                if (soType == null)
                {
                    throw new Exception(string.Format("Standard Output {0} not Found", codeName));
                }
                else
                {
                    bool isNull = false;
                    int dataType = 0;
                    if (codeName.StartsWith(RiDataIndicator))
                    {
                        dataType = StandardOutputBo.GetDataTypeByType(soType.Value);
                        if (RiDataBo != null)
                        {
                            value = RiDataBo.GetPropertyValue(StandardOutputBo.GetPropertyNameByType(soType.Value));
                            if (value == null)
                            {
                                value = "null";
                                isNull = true;
                            }
                        }
                        else if (RiDataWarehouseBo != null)
                        {
                            value = RiDataWarehouseBo.GetPropertyValue(StandardOutputBo.GetPropertyNameByType(soType.Value));
                            if (value == null)
                            {
                                value = "null";
                                isNull = true;
                            }
                        }
                        else
                        {
                            value = StandardOutputBo.GetRandomValueByType(soType.Value);
                        }
                    }
                    else if (codeName.StartsWith(OriginalIndicator))
                    {
                        dataType = StandardClaimDataOutputBo.GetDataTypeByType(soType.Value);
                        if (OriginalClaimDataBo != null)
                        {
                            value = OriginalClaimDataBo.GetPropertyValue(StandardClaimDataOutputBo.GetPropertyNameByType(soType.Value));
                            if (value == null)
                            {
                                value = "null";
                                isNull = true;
                            }
                        }
                        else if (OriginalClaimRegisterBo != null)
                        {
                            value = OriginalClaimRegisterBo.GetPropertyValue(StandardClaimDataOutputBo.GetPropertyNameByType(soType.Value));
                            if (value == null)
                            {
                                value = "null";
                                isNull = true;
                            }
                        }
                        else
                        {
                            value = StandardClaimDataOutputBo.GetRandomValueByType(soType.Value);
                        }
                    }
                    else
                    {
                        dataType = StandardClaimDataOutputBo.GetDataTypeByType(soType.Value);
                        if (ClaimDataBo != null)
                        {
                            value = ClaimDataBo.GetPropertyValue(StandardClaimDataOutputBo.GetPropertyNameByType(soType.Value));
                            if (value == null)
                            {
                                value = "null";
                                isNull = true;
                            }
                        }
                        else if (ClaimRegisterBo != null)
                        {
                            value = ClaimRegisterBo.GetPropertyValue(StandardClaimDataOutputBo.GetPropertyNameByType(soType.Value));
                            if (value == null)
                            {
                                value = "null";
                                isNull = true;
                            }
                        }
                        else
                        {
                            value = StandardClaimDataOutputBo.GetRandomValueByType(soType.Value);
                        }
                    }

                    if (!isNull)
                    {
                        switch (dataType)
                        {
                            case StandardOutputBo.DataTypeDate:
                                if (value is DateTime valueDateTime)
                                    value = string.Format("DateTime.Parse(\"{0}\")", valueDateTime.ToString(Util.GetDateFormat()));
                                else
                                    value = "";
                                break;
                            case StandardOutputBo.DataTypeAmount:
                                value = string.Format("({0} * 1.0)", value);
                                break;
                            case StandardOutputBo.DataTypeString:
                            case StandardOutputBo.DataTypeDropDown:
                                value = string.Format("\"{0}\"", value);
                                break;
                        }
                    }
                    else if (!Nullable)
                    {
                        switch (dataType)
                        {
                            case StandardOutputBo.DataTypeDate:
                                value = string.Format("DateTime.Parse(\"{0}\")", DateTime.MinValue.ToString(Util.GetDateFormat()));
                                break;
                            case StandardOutputBo.DataTypeString:
                            case StandardOutputBo.DataTypeDropDown:
                                value = "\"\"";
                                break;
                            case StandardOutputBo.DataTypeAmount:
                                value = "0.00";
                                break;
                            default:
                                value = "0";
                                break;
                        }
                    }
                }

                string actualValue = Convert.ToString(value);
                string replaceField = string.Format("{0}{1}{2}", DelimiterStart, codeName, DelimiterEnd);

                script = script.Replace(replaceField, actualValue);
                currentIndex = indexStart - 1 + actualValue.Length;
            }

            script = script.Replace(EscapeCharacter + DelimiterStart, DelimiterStart);
            script = script.Replace(EscapeCharacter + DelimiterEnd, DelimiterEnd);

            return script;
        }

        public string FormatCondition()
        {
            FormattedCondition = Format(Condition);
            return FormattedCondition;
        }

        public string FormatFormula()
        {
            FormattedFormula = Format(Formula);
            return FormattedFormula;
        }

        public Interpreter GetInterpreter()
        {
            var interpreter = new Interpreter();
            interpreter.Reference(typeof(MidpointRounding));
            interpreter.Reference(typeof(decimal));

            Func<double, int, double> round = (value, precision) => (double)decimal.Round((decimal)value, precision, MidpointRounding.AwayFromZero);
            interpreter.SetFunction("RoundValue", round);

            double d = 0;
            Func<string, double> isNumber = (x) => !string.IsNullOrEmpty(x) && double.TryParse(x, out d) ? double.Parse(x) : 0;
            interpreter.SetFunction("IsNumber", isNumber);

            return interpreter;
        }

        public bool EvalCondition()
        {
            object result;
            try
            {
                result = GetInterpreter().Eval(FormatCondition());
                if (result == null || result.GetType().ToString() != "System.Boolean")
                {
                    throw new Exception("Condition does not return true/false");
                }
            }
            catch (Exception e)
            {
                result = null;
                Errors.Add(e.Message);

                return false;
            }
            return (bool)result;
        }

        public object EvalFormula()
        {
            object result;
            try
            {
                result = GetInterpreter().Eval(FormatFormula());
            }
            catch (Exception e)
            {
                result = null;
                Errors.Add(e.Message);
            }
            return result;
        }
    }
}
