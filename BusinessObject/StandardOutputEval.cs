using BusinessObject.RiDatas;
using DynamicExpresso;
using Shared;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace BusinessObject
{
    public class StandardOutputEval
    {
        public RiDataBo RiDataBo { get; set; }
        public RiDataBo OriginalRiDataBo { get; set; }
        public RiDataWarehouseBo OriginalRiDataWarehouseBo { get; set; }

        public string Condition { get; set; }
        public string Formula { get; set; }

        public string FormattedCondition { get; set; }
        public string FormattedFormula { get; set; }

        public bool Nullable { get; set; } = false;
        public string Quarter { get; set; }
        public string OriginalQuarter { get; set; }

        public int DoublePrecision { get; set; }
        public string DoubleToStringFormat { get; set; }

        public bool EnableOriginal { get; set; } = false;

        public List<string> Errors { get; set; } = new List<string> { };

        public const string DelimiterStart = "{";
        public const string DelimiterEnd = "}";
        public const char EscapeCharacter = '\\';
        public const string OriginalIndicator = "ORI.";

        public const string QuarterPattern = @"^[0-9]{4}\s[Q]{1}[1-4]{1}$";

        public StandardOutputEval()
        {
            DoublePrecision = Util.GetConfigInteger("DoublePrecision", 5);
            DoubleToStringFormat = string.Format("{{0:F{0}}}", DoublePrecision);
        }

        public static List<StandardOutputBo> GetVariables(string script, ref bool hasQuarter, ref bool hasOriQuarter, bool enableOriginal = false)
        {
            List<StandardOutputBo> standardOutputBos = new List<StandardOutputBo>();
            if (string.IsNullOrEmpty(script))
                return standardOutputBos;

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
                string actualName = codeName;
                bool isOriginal = false;
                if (enableOriginal && codeName.StartsWith(OriginalIndicator))
                {
                    isOriginal = true;
                    actualName = actualName.Substring(OriginalIndicator.Length);
                }
                int? soType = StandardOutputBo.GetTypeByCode(actualName, false);

                if (actualName == "SOA_QUARTER" || actualName == "SOA_QUARTER_YEAR")
                {
                    if (codeName.StartsWith(OriginalIndicator))
                    {
                        hasOriQuarter = true;
                    }
                    else
                    {
                        hasQuarter = true;
                    }
                    currentIndex = indexEnd + 1;
                    continue;
                }
                else if (soType == null)
                {
                    throw new Exception(string.Format("Standard Output {0} not found", codeName));
                }
                else if (soType == StandardOutputBo.TypeCustomField)
                {
                    throw new Exception("Custom Field cannot be used in formulas");
                }

                int dataType = StandardOutputBo.GetDataTypeByType(soType.Value);
                StandardOutputBo stadardOutputBo = new StandardOutputBo()
                {
                    Code = codeName,
                    Type = soType.Value,
                    DataType = dataType,
                    DataTypeName = StandardOutputBo.GetDataTypeName(dataType),
                    DummyValue = "",
                    IsOriginal = isOriginal
                };
                standardOutputBos.Add(stadardOutputBo);

                string replaceField = string.Format("{0}{1}{2}", DelimiterStart, codeName, DelimiterEnd);
                script = script.Replace(replaceField, "");

                currentIndex = indexStart;
            }

            return standardOutputBos;
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
                string actualName = codeName;
                if (codeName.StartsWith(OriginalIndicator))
                {
                    if (EnableOriginal)
                        actualName = actualName.Substring(OriginalIndicator.Length);
                    else
                        throw new Exception("ORI Standard Output cannot be used in this formula");
                }
                int? soType = StandardOutputBo.GetTypeByCode(actualName, false);
                dynamic value;

                if (actualName == "SOA_QUARTER" || actualName == "SOA_QUARTER_YEAR")
                {
                    value = "null";
                    if (RiDataBo == null)
                    {
                        Quarter = "2020 Q4";
                    }

                    if (OriginalRiDataBo == null && OriginalRiDataWarehouseBo == null)
                    {
                        OriginalQuarter = "2020 Q4";
                    }

                    Regex rgx = new Regex(QuarterPattern);
                    string[] qtr;
                    if (codeName.StartsWith(OriginalIndicator))
                    {
                        if (!rgx.IsMatch(OriginalQuarter))
                            throw new Exception("Ori Quarter format is incorrect");

                        qtr = OriginalQuarter.Split(' ');
                    }
                    else
                    {
                        if (!rgx.IsMatch(Quarter))
                            throw new Exception("Quarter format is incorrect");

                        qtr = Quarter.Split(' ');
                    }

                    if (actualName == "SOA_QUARTER_YEAR")
                    {
                        value = qtr[0];
                    }
                    else
                    {
                        value = qtr[1][1];
                    }
                }
                else if (soType == null)
                {
                    throw new Exception(string.Format("Standard Output {0} not Found", codeName));
                }
                else if (soType == StandardOutputBo.TypeCustomField)
                {
                    throw new Exception("Custom Field cannot be used in formulas");
                }
                else
                {
                    bool isNull = false;
                    int dataType = StandardOutputBo.GetDataTypeByType(soType.Value);

                    if (codeName.StartsWith(OriginalIndicator))
                    {
                        if (OriginalRiDataBo != null)
                        {
                            value = OriginalRiDataBo.GetPropertyValue(StandardOutputBo.GetPropertyNameByType(soType.Value));
                            if (value == null)
                            {
                                value = "null";
                                isNull = true;
                            }
                        }
                        else if (OriginalRiDataWarehouseBo != null)
                        {
                            value = OriginalRiDataWarehouseBo.GetPropertyValue(StandardOutputBo.GetPropertyNameByType(soType.Value));
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
                    else
                    {
                        if (RiDataBo != null)
                        {
                            value = RiDataBo.GetPropertyValue(StandardOutputBo.GetPropertyNameByType(soType.Value));
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
                            case StandardOutputBo.DataTypePercentage:
                                if (value is double b)
                                {
                                    value = string.Format(DoubleToStringFormat, b);
                                    value = string.Format("({0} / 100)", value);
                                }
                                else
                                {
                                    double.TryParse(value?.ToString(), out double valueDouble);
                                    value = string.Format(DoubleToStringFormat, valueDouble);
                                    value = string.Format("({0} / 100)", value);
                                }
                                break;
                            case StandardOutputBo.DataTypeAmount:
                                if (value is double d)
                                    value = string.Format(DoubleToStringFormat, d);
                                else
                                {
                                    double.TryParse(value.ToString(), out double valueDouble);
                                    value = string.Format(DoubleToStringFormat, valueDouble);
                                }
                                break;
                            case StandardOutputBo.DataTypeString:
                            case StandardOutputBo.DataTypeDropDown:
                                value = string.Format("\"{0}\"", value.Replace("\"", ""));
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
                            case StandardOutputBo.DataTypePercentage:
                            case StandardOutputBo.DataTypeAmount:
                                value = string.Format(DoubleToStringFormat, 0.0);
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
            script = script.Replace("\\", "\\\\");

            return script;
        }

        public string FormatCondition()
        {
            FormattedCondition = Format(Condition);
            return FormattedCondition;
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

        public string FormatFormula()
        {
            FormattedFormula = Format(Formula);
            return FormattedFormula;
        }

        public bool EvalCondition()
        {
            object result = false;
            int tries = 0;
            while (true)
            {
                try
                {
                    result = GetInterpreter().Eval(FormatCondition());
                    if (result == null || result.GetType().ToString() != "System.Boolean")
                    {
                        throw new Exception("Condition does not return true/false");
                    }
                    break;
                }
                catch (Exception e)
                {
                    result = null;
                    if (e.Message == "String was not recognized as a valid DateTime." && tries < 2)
                    {
                        tries++;
                        continue;
                    }

                    Errors.Add(e.Message);
                    return false;
                }
            }
            return (bool)result;
        }

        public object EvalFormula()
        {
            object result;
            int tries = 0;
            while (true)
            {
                try
                {
                    result = GetInterpreter().Eval(FormatFormula());
                    break;
                }
                catch (Exception e)
                {
                    result = null;
                    if (e.Message == "String was not recognized as a valid DateTime." && tries < 2)
                    {
                        tries++;
                        continue;
                    }
                    Errors.Add(e.Message);
                    break;
                }
            }
            return result;
        }
    }
}
