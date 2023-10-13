using Shared.ProcessFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class RateBo
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public int ValuationRate { get; set; }

        public int RatePerBasis { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? UpdatedById { get; set; }

        public const int ValuationRate1 = 1;
        public const int ValuationRate2 = 2;
        public const int ValuationRate3 = 3;
        public const int ValuationRate4 = 4;
        public const int ValuationRate5 = 5;
        public const int ValuationRate6 = 6;
        public const int ValuationRate7 = 7;
        public const int ValuationRate8 = 8;
        public const int ValuationRate9 = 9;
        public const int ValuationRate10 = 10;

        public const int MaxValuationRate = 10;

        public static string GetValuationRateName(int key)
        {
            switch (key)
            {
                case ValuationRate1:
                    return string.Format(
                        "{{{0}}}",
                        StandardOutputBo.GetCodeByType(StandardOutputBo.TypeRateTable)
                        );
                case ValuationRate2:
                    return string.Format(
                        "{{{0}}}+{{{1}}}",
                        StandardOutputBo.GetCodeByType(StandardOutputBo.TypeRateTable),
                        StandardOutputBo.GetCodeByType(StandardOutputBo.TypeInsuredGenderCode)
                        );
                case ValuationRate3:
                    return string.Format(
                        "{{{0}}}+{{{1}}}",
                        StandardOutputBo.GetCodeByType(StandardOutputBo.TypeRateTable),
                        StandardOutputBo.GetCodeByType(StandardOutputBo.TypeInsuredTobaccoUse)
                        );
                case ValuationRate4:
                    return string.Format(
                        "{{{0}}}+{{{1}}}+{{{2}}}",
                        StandardOutputBo.GetCodeByType(StandardOutputBo.TypeRateTable),
                        StandardOutputBo.GetCodeByType(StandardOutputBo.TypeInsuredGenderCode),
                        StandardOutputBo.GetCodeByType(StandardOutputBo.TypeInsuredTobaccoUse)
                        );
                case ValuationRate5:
                    return string.Format(
                        "{{{0}}}+{{{1}}}",
                        StandardOutputBo.GetCodeByType(StandardOutputBo.TypeRateTable),
                        StandardOutputBo.GetCodeByType(StandardOutputBo.TypeInsuredOccupationCode)
                        );
                case ValuationRate6:
                    return string.Format(
                        "{{{0}}}+{{{1}}}+{{{2}}}+{{{3}}}",
                        StandardOutputBo.GetCodeByType(StandardOutputBo.TypeRateTable),
                        StandardOutputBo.GetCodeByType(StandardOutputBo.TypeInsuredGenderCode),
                        StandardOutputBo.GetCodeByType(StandardOutputBo.TypeInsuredTobaccoUse),
                        StandardOutputBo.GetCodeByType(StandardOutputBo.TypePolicyTermRemain)
                        );
                case ValuationRate7:
                    return string.Format(
                        "{{{0}}}+{{{1}}}+{{{2}}}",
                        StandardOutputBo.GetCodeByType(StandardOutputBo.TypeRateTable),
                        StandardOutputBo.GetCodeByType(StandardOutputBo.TypeMlrePolicyIssueAge),
                        StandardOutputBo.GetCodeByType(StandardOutputBo.TypePolicyTermRemain)
                        );
                case ValuationRate8:
                    return string.Format(
                        "{{{0}}}+{{{1}}}+{{{2}}}+{{{3}}}+{{{4}}}",
                        StandardOutputBo.GetCodeByType(StandardOutputBo.TypeRateTable),
                        StandardOutputBo.GetCodeByType(StandardOutputBo.TypePolicyTerm),
                        StandardOutputBo.GetCodeByType(StandardOutputBo.TypeMlrePolicyIssueAge),
                        StandardOutputBo.GetCodeByType(StandardOutputBo.TypeInsuredGenderCode),
                        StandardOutputBo.GetCodeByType(StandardOutputBo.TypeInsuredTobaccoUse)
                        );
                case ValuationRate9:
                    return string.Format(
                        "{{{0}}}+{{{1}}}+{{{2}}}",
                        StandardOutputBo.GetCodeByType(StandardOutputBo.TypeRateTable),
                        StandardOutputBo.GetCodeByType(StandardOutputBo.TypeInsuredGenderCode),
                        StandardOutputBo.GetCodeByType(StandardOutputBo.TypeInsuredOccupationCode)
                        );
                case ValuationRate10:
                    return string.Format(
                        "{{{0}}}+{{{1}}}+{{{2}}}+{{{3}}}",
                        StandardOutputBo.GetCodeByType(StandardOutputBo.TypeRateTable),
                        StandardOutputBo.GetCodeByType(StandardOutputBo.TypeInsuredGenderCode),
                        StandardOutputBo.GetCodeByType(StandardOutputBo.TypeInsuredTobaccoUse),
                        StandardOutputBo.GetCodeByType(StandardOutputBo.TypeInsuredOccupationCode)
                        );
                default:
                    return "";
            }
        }

        public static List<int> GetFieldsByValuationRate(int key)
        {
            List<int> fields = new List<int> { };
            switch (key)
            {
                case ValuationRate1:
                    fields.Add(RateDetailBo.TypeAttainedAge);
                    fields.Add(RateDetailBo.TypeIssueAge);
                    fields.Add(RateDetailBo.TypePolicyTerm);
                    break;
                case ValuationRate2:
                    fields.Add(RateDetailBo.TypeInsuredGenderCode);
                    fields.Add(RateDetailBo.TypeAttainedAge);
                    break;
                case ValuationRate3:
                    fields.Add(RateDetailBo.TypeCedingTobaccoUse);
                    fields.Add(RateDetailBo.TypeAttainedAge);
                    break;
                case ValuationRate4:
                    fields.Add(RateDetailBo.TypeInsuredGenderCode);
                    fields.Add(RateDetailBo.TypeCedingTobaccoUse);
                    fields.Add(RateDetailBo.TypeAttainedAge);
                    break;
                case ValuationRate5:
                    fields.Add(RateDetailBo.TypeCedingOccupationCode);
                    break;
                case ValuationRate6:
                    fields.Add(RateDetailBo.TypeInsuredGenderCode);
                    fields.Add(RateDetailBo.TypeCedingTobaccoUse);
                    fields.Add(RateDetailBo.TypePolicyTermRemain);
                    fields.Add(RateDetailBo.TypeAttainedAge);
                    break;
                case ValuationRate7:
                    fields.Add(RateDetailBo.TypeIssueAge);
                    fields.Add(RateDetailBo.TypePolicyTermRemain);
                    fields.Add(RateDetailBo.TypeAttainedAge);
                    break;
                case ValuationRate8:
                    fields.Add(RateDetailBo.TypeInsuredGenderCode);
                    fields.Add(RateDetailBo.TypeCedingTobaccoUse);
                    fields.Add(RateDetailBo.TypeIssueAge);
                    fields.Add(RateDetailBo.TypePolicyTerm);
                    fields.Add(RateDetailBo.TypeAttainedAge);
                    break;
                case ValuationRate9:
                    fields.Add(RateDetailBo.TypeInsuredGenderCode);
                    fields.Add(RateDetailBo.TypeCedingOccupationCode);
                    fields.Add(RateDetailBo.TypeIssueAge);
                    fields.Add(RateDetailBo.TypeAttainedAge);
                    break;
                case ValuationRate10:
                    fields.Add(RateDetailBo.TypeInsuredGenderCode);
                    fields.Add(RateDetailBo.TypeCedingTobaccoUse);
                    fields.Add(RateDetailBo.TypeCedingOccupationCode);
                    fields.Add(RateDetailBo.TypeIssueAge);
                    fields.Add(RateDetailBo.TypeAttainedAge);
                    break;
                default:
                    fields.Add(RateDetailBo.TypeInsuredGenderCode);
                    fields.Add(RateDetailBo.TypeCedingTobaccoUse);
                    fields.Add(RateDetailBo.TypeCedingOccupationCode);
                    fields.Add(RateDetailBo.TypeAttainedAge);
                    fields.Add(RateDetailBo.TypeIssueAge);
                    fields.Add(RateDetailBo.TypePolicyTerm);
                    fields.Add(RateDetailBo.TypePolicyTermRemain);
                    break;
            }
            return fields;
        }

        public static List<int> GetRequiredStandardOutputFields(int key, bool isCurrentAge = false)
        {
            List<int> fields = new List<int> { };
            switch (key)
            {
                case ValuationRate2:
                    fields.Add(StandardOutputBo.TypeInsuredGenderCode);
                    break;
                case ValuationRate3:
                    fields.Add(StandardOutputBo.TypeInsuredTobaccoUse);
                    break;
                case ValuationRate4:
                    fields.Add(StandardOutputBo.TypeInsuredGenderCode);
                    fields.Add(StandardOutputBo.TypeInsuredTobaccoUse);
                    break;
                case ValuationRate5:
                    fields.Add(StandardOutputBo.TypeInsuredOccupationCode);
                    break;
                case ValuationRate6:
                    fields.Add(StandardOutputBo.TypeInsuredGenderCode);
                    fields.Add(StandardOutputBo.TypeInsuredTobaccoUse);
                    fields.Add(StandardOutputBo.TypePolicyTermRemain);
                    break;
                case ValuationRate7:
                    fields.Add(StandardOutputBo.TypeMlrePolicyIssueAge);
                    fields.Add(StandardOutputBo.TypePolicyTermRemain);
                    break;
                case ValuationRate8:
                    fields.Add(StandardOutputBo.TypeInsuredGenderCode);
                    fields.Add(StandardOutputBo.TypeInsuredTobaccoUse);
                    fields.Add(StandardOutputBo.TypeMlrePolicyIssueAge);
                    fields.Add(StandardOutputBo.TypePolicyTerm);
                    break;
                case ValuationRate9:
                    fields.Add(StandardOutputBo.TypeInsuredGenderCode);
                    fields.Add(StandardOutputBo.TypeInsuredOccupationCode);
                    fields.Add(StandardOutputBo.TypeMlrePolicyIssueAge);
                    break;
                case ValuationRate10:
                    fields.Add(StandardOutputBo.TypeInsuredGenderCode);
                    fields.Add(StandardOutputBo.TypeInsuredTobaccoUse);
                    fields.Add(StandardOutputBo.TypeInsuredOccupationCode);
                    fields.Add(StandardOutputBo.TypeMlrePolicyIssueAge);
                    break;
                default:
                    break;
            }

            switch (key)
            {
                case ValuationRate2:
                case ValuationRate3:
                case ValuationRate4:
                case ValuationRate6:
                case ValuationRate7:
                case ValuationRate8:
                case ValuationRate9:
                case ValuationRate10:
                    if (isCurrentAge)
                        fields.Add(StandardOutputBo.TypeMlreInsuredAttainedAgeAtCurrentMonth);
                    else
                        fields.Add(StandardOutputBo.TypeMlreInsuredAttainedAgeAtCurrentMonth);
                    break;
                default:
                    break;
            }

            return fields;
        }

        public static List<int> GetRequiredDropDownFields(int key)
        {
            List<int> fields = new List<int> { };
            switch (key)
            {
                case ValuationRate2:
                    fields.Add(StandardOutputBo.TypeInsuredGenderCode);
                    break;
                case ValuationRate3:
                    fields.Add(StandardOutputBo.TypeInsuredTobaccoUse);
                    break;
                case ValuationRate4:
                    fields.Add(StandardOutputBo.TypeInsuredGenderCode);
                    fields.Add(StandardOutputBo.TypeInsuredTobaccoUse);
                    break;
                case ValuationRate5:
                    fields.Add(StandardOutputBo.TypeInsuredOccupationCode);
                    break;
                case ValuationRate6:
                    fields.Add(StandardOutputBo.TypeInsuredGenderCode);
                    fields.Add(StandardOutputBo.TypeInsuredOccupationCode);
                    break;
                case ValuationRate8:
                    fields.Add(StandardOutputBo.TypeInsuredGenderCode);
                    fields.Add(StandardOutputBo.TypeInsuredTobaccoUse);
                    break;
                case ValuationRate9:
                    fields.Add(StandardOutputBo.TypeInsuredGenderCode);
                    fields.Add(StandardOutputBo.TypeInsuredOccupationCode);
                    break;
                case ValuationRate10:
                    fields.Add(StandardOutputBo.TypeInsuredGenderCode);
                    fields.Add(StandardOutputBo.TypeInsuredTobaccoUse);
                    fields.Add(StandardOutputBo.TypeInsuredOccupationCode);
                    break;
                default:
                    break;
            }
            return fields;
        }
    }
}
