using BusinessObject.SoaDatas;
using DynamicExpresso;
using Shared;
using System;
using System.Collections.Generic;

namespace BusinessObject
{
    public class StandardSoaDataOutputEval
    {
        public SoaDataBo SoaDataBo { get; set; }

        public string Condition { get; set; }

        public string Formula { get; set; }

        public string FormattedCondition { get; set; }

        public string FormattedFormula { get; set; }

        public bool Validate { get; set; } = false;

        public bool Nullable { get; set; } = false;

        public string Quarter { get; set; }

        public List<string> Errors { get; set; } = new List<string> { };

        public const string DelimiterStart = "{";
        public const string DelimiterEnd = "}";
        public const char EscapeCharacter = '\\';

        public const string QuarterPattern = @"^[0-9]{4}\s[Q]{1}[1-4]{1}$";

        public static List<StandardSoaDataOutputBo> GetVariables(string script)
        {
            List<StandardSoaDataOutputBo> standardSoaDataOutputBos = new List<StandardSoaDataOutputBo>();
            if (string.IsNullOrEmpty(script))
                return standardSoaDataOutputBos;

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
                int? soType = StandardSoaDataOutputBo.GetTypeByCode(codeName, false);

                if (soType == null)
                {
                    throw new Exception(string.Format("Standard Output {0} not found", codeName));
                }

                int dataType = StandardSoaDataOutputBo.GetDataTypeByType(soType.Value);
                StandardSoaDataOutputBo standardSoaDataOutputBo = new StandardSoaDataOutputBo()
                {
                    Code = codeName,
                    Type = soType.Value,
                    DataType = dataType,
                    DataTypeName = StandardSoaDataOutputBo.GetDataTypeName(dataType),
                    DummyValue = ""
                };
                standardSoaDataOutputBos.Add(standardSoaDataOutputBo);

                string replaceField = string.Format("{0}{1}{2}", DelimiterStart, codeName, DelimiterEnd);
                script = script.Replace(replaceField, "");

                currentIndex = indexStart;
            }

            return standardSoaDataOutputBos;
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
                int? soType = StandardSoaDataOutputBo.GetTypeByCode(codeName, false);
                dynamic value;

                if (soType == null)
                {
                    throw new Exception(string.Format("Standard Output {0} not Found", codeName));
                }
                else
                {
                    bool isNull = false;
                    int dataType = StandardSoaDataOutputBo.GetDataTypeByType(soType.Value);
                    if (SoaDataBo != null)
                    {
                        value = SoaDataBo.GetPropertyValue(StandardSoaDataOutputBo.GetPropertyNameByType(soType.Value));
                        if (value == null)
                        {
                            value = "null";
                            isNull = true;
                        }
                    }
                    else
                    {
                        value = StandardSoaDataOutputBo.GetRandomValueByType(soType.Value);
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

        public bool EvalCondition()
        {
            object result;
            try
            {
                result = (new Interpreter()).Eval(FormatCondition());
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
                result = (new Interpreter()).Eval(FormatFormula());
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

