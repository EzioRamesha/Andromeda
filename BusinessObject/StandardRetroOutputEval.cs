using BusinessObject.Retrocession;
using DynamicExpresso;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class StandardRetroOutputEval
    {
        public PerLifeAggregationMonthlyDataBo PerLifeAggregationMonthlyDataBo { get; set; }

        public double? MlreShare { get; set; }
        public double? PremiumSpread { get; set; }
        public double? Discount { get; set; }

        public double? RetroGrossPremium { get; set; }
        public double? RetroDiscount { get; set; }
        public double? RetroNetPremium { get; set; }

        public string Formula { get; set; }
        public string FormattedFormula { get; set; }

        public int DoublePrecision { get; set; }
        public string DoubleToStringFormat { get; set; }

        public List<string> Errors { get; set; } = new List<string> { };

        public const string DelimiterStart = "{";
        public const string DelimiterEnd = "}";
        public const char EscapeCharacter = '\\';

        public StandardRetroOutputEval()
        {
            DoublePrecision = Util.GetConfigInteger("DoublePrecision", 5);
            DoubleToStringFormat = string.Format("{{0:F{0}}}", DoublePrecision);
        }

        public List<StandardRetroOutputBo> GetVariables(string script)
        {
            List<StandardRetroOutputBo> standardRetroOutputBos = new List<StandardRetroOutputBo>();
            if (string.IsNullOrEmpty(script))
                return standardRetroOutputBos;

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

                string dummyValue = "";
                int? dataType;
                int? soType = null;
                switch (codeName)
                {
                    case "MLRE_SHARE":
                        if (!MlreShare.HasValue)
                            throw new Exception(string.Format("{0} cannot be used as it is empty", codeName));
                        dummyValue = MlreShare.ToString();
                        dataType = StandardOutputBo.DataTypePercentage;
                        break;
                    case "PREMIUM_SPREAD":
                        //if (!PremiumSpread.HasValue)
                        //    throw new Exception(string.Format("{0} cannot be used as it is empty", codeName));
                        //dummyValue = PremiumSpread.ToString();
                        dataType = StandardOutputBo.DataTypeAmount;
                        break;
                    case "DISCOUNT":
                        //if (!Discount.HasValue)
                        //    throw new Exception(string.Format("{0} cannot be used as it is empty", codeName));
                        //dummyValue = Discount.ToString();
                        dataType = StandardOutputBo.DataTypeAmount;
                        break;
                    default:
                        soType = StandardRetroOutputBo.GetTypeByCode(codeName, false);
                        if (soType == null)
                        {
                            throw new Exception(string.Format("Standard Output {0} not found", codeName));
                        }
                        dataType = StandardRetroOutputBo.GetDataTypeByType(soType.Value);
                        break;
                }

                StandardRetroOutputBo stadardRetroOutputBo = new StandardRetroOutputBo()
                {
                    Code = codeName,
                    Type = soType ?? 0,
                    DataType = dataType.Value,
                    DataTypeName = StandardOutputBo.GetDataTypeName(dataType.Value),
                    DummyValue = dummyValue,
                    DisableDummyValue = !string.IsNullOrEmpty(dummyValue)
                };
                standardRetroOutputBos.Add(stadardRetroOutputBo);

                string replaceField = string.Format("{0}{1}{2}", DelimiterStart, codeName, DelimiterEnd);
                script = script.Replace(replaceField, "");

                currentIndex = indexStart;
            }

            return standardRetroOutputBos;
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

                int? dataType;
                dynamic value;
                switch (codeName)
                {
                    case "MLRE_SHARE":
                        if (!MlreShare.HasValue)
                            throw new Exception(string.Format("{0} cannot be used as it is empty", codeName));
                        value = MlreShare;
                        dataType = StandardOutputBo.DataTypePercentage;
                        break;
                    case "PREMIUM_SPREAD":
                        if (!PremiumSpread.HasValue)
                            throw new Exception(string.Format("{0} cannot be used as it is empty", codeName));
                        value = PremiumSpread;
                        dataType = StandardOutputBo.DataTypeAmount;
                        break;
                    case "DISCOUNT":
                        if (!Discount.HasValue)
                            throw new Exception(string.Format("{0} cannot be used as it is empty", codeName));
                        value = Discount;
                        dataType = StandardOutputBo.DataTypeAmount;
                        break;
                    case "RETRO_GROSS_PREMIUM":
                        if (!RetroGrossPremium.HasValue && PerLifeAggregationMonthlyDataBo != null)
                            throw new Exception(string.Format("{0} cannot be used as it is empty", codeName));
                        value = RetroGrossPremium ?? 0;
                        dataType = StandardOutputBo.DataTypeAmount;
                        break;
                    case "RETRO_DISCOUNT":
                        if (!RetroDiscount.HasValue && PerLifeAggregationMonthlyDataBo != null)
                            throw new Exception(string.Format("{0} cannot be used as it is empty", codeName));
                        value = RetroDiscount ?? 0;
                        dataType = StandardOutputBo.DataTypeAmount;
                        break;
                    case "RETRO_NET_PREMIUM":
                        if (!RetroNetPremium.HasValue && PerLifeAggregationMonthlyDataBo != null)
                            throw new Exception(string.Format("{0} cannot be used as it is empty", codeName));
                        value = RetroNetPremium ?? 0;
                        dataType = StandardOutputBo.DataTypeAmount;
                        break;
                    default:
                        int? soType = StandardRetroOutputBo.GetTypeByCode(codeName, false);
                        if (soType == null)
                        {
                            throw new Exception(string.Format("Standard Output {0} not found", codeName));
                        }
                        dataType = StandardRetroOutputBo.GetDataTypeByType(soType.Value);
                        if (PerLifeAggregationMonthlyDataBo != null)
                        {
                            value = PerLifeAggregationMonthlyDataBo.GetPropertyValue(StandardRetroOutputBo.GetPropertyNameByType(soType.Value));
                        }
                        else
                        {
                            value = StandardRetroOutputBo.GetRandomValueByType(soType.Value);
                        }
                        break;
                }

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
                        value = string.Format("\"{0}\"", value);
                        break;
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

        public string FormatFormula()
        {
            FormattedFormula = Format(Formula);
            return FormattedFormula;
        }

        public object EvalFormula()
        {
            object result;
            int tries = 0;
            while (true)
            {
                try
                {
                    result = (new Interpreter()).Eval(FormatFormula());
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
