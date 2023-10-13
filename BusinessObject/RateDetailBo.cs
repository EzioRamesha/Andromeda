using Shared;
using Shared.DataAccess;
using Shared.ProcessFile;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace BusinessObject
{
    public class RateDetailBo
    {
        public int Id { get; set; }

        public int RateId { get; set; }

        public RateBo RateBo { get; set; }

        [DisplayName("Gender")]
        public int? InsuredGenderCodePickListDetailId { get; set; }

        public PickListDetailBo InsuredGenderCodePickListDetailBo { get; set; }

        public string InsuredGenderCode { get; set; }

        [DisplayName("Smoker")]
        public int? CedingTobaccoUsePickListDetailId { get; set; }

        public PickListDetailBo CedingTobaccoUsePickListDetailBo { get; set; }

        public string CedingTobaccoUse { get; set; }

        [DisplayName("Occupation")]
        public int? CedingOccupationCodePickListDetailId { get; set; }

        public PickListDetailBo CedingOccupationCodePickListDetailBo { get; set; }

        public string CedingOccupationCode { get; set; }

        [DisplayName("Attained Age")]
        public int? AttainedAge { get; set; }

        [DisplayName("Issue Age")]
        public int? IssueAge { get; set; }

        [DisplayName("Policy Term")]
        public double? PolicyTerm { get; set; }

        [DisplayName("Policy Term Remain")]
        public double? PolicyTermRemain { get; set; }

        public double RateValue { get; set; }

        [DisplayName("Rate")]
        public string RateValueStr { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? UpdatedById { get; set; }

        public const int TypeRateDetailId = 1;
        public const int TypeInsuredGenderCode = 2;
        public const int TypeCedingTobaccoUse = 3;
        public const int TypeCedingOccupationCode = 4;
        public const int TypeAttainedAge = 5;
        public const int TypeIssueAge = 6;
        public const int TypePolicyTerm = 7;
        public const int TypePolicyTermRemain = 8;
        public const int TypeRateValue = 9;
        public const int TypeAction = 10;

        public static List<Column> GetColumns()
        {
            List<Column> Cols = new List<Column>
            {
                new Column
                {
                    Header = "ID",
                    ColIndex = TypeRateDetailId,
                    Property = "Id",
                },
                new Column
                {
                    Header = "Gender",
                    ColIndex = TypeInsuredGenderCode,
                    Property = "InsuredGenderCode",
                },
                new Column
                {
                    Header = "Smoker",
                    ColIndex = TypeCedingTobaccoUse,
                    Property = "CedingTobaccoUse",
                },
                new Column
                {
                    Header = "Occupation Code",
                    ColIndex = TypeCedingOccupationCode,
                    Property = "CedingOccupationCode",
                },
                new Column
                {
                    Header = "Attained Age",
                    ColIndex = TypeAttainedAge,
                    Property = "AttainedAge",
                },
                new Column
                {
                    Header = "Issue Age",
                    ColIndex = TypeIssueAge,
                    Property = "IssueAge",
                },
                new Column
                {
                    Header = "Term",
                    ColIndex = TypePolicyTerm,
                    Property = "PolicyTerm",
                },
                new Column
                {
                    Header = "Term Remain",
                    ColIndex = TypePolicyTermRemain,
                    Property = "PolicyTermRemain",
                },
                new Column
                {
                    Header = "Rate",
                    ColIndex = TypeRateValue,
                    Property = "RateValue",
                },
                new Column
                {
                    Header = "Action",
                    ColIndex = TypeAction,
                },
            };

            return Cols;
        }

        public Result Validate(int valuationRate, int index)
        {
            Result result = new Result();
            switch (valuationRate)
            {
                case RateBo.ValuationRate2:
                    ValidateValue("InsuredGenderCodePickListDetailId", index, ref result);
                    ValidateValue("AttainedAge", index, ref result);
                    ValidateValue("RateValueStr", index, ref result);
                    break;
                case RateBo.ValuationRate3:
                    ValidateValue("CedingTobaccoUsePickListDetailId", index, ref result);
                    ValidateValue("AttainedAge", index, ref result);
                    ValidateValue("RateValueStr", index, ref result);
                    break;
                case RateBo.ValuationRate4:
                    ValidateValue("InsuredGenderCodePickListDetailId", index, ref result);
                    ValidateValue("CedingTobaccoUsePickListDetailId", index, ref result);
                    ValidateValue("AttainedAge", index, ref result);
                    ValidateValue("RateValueStr", index, ref result);
                    break;
                case RateBo.ValuationRate5:
                    ValidateValue("CedingOccupationCodePickListDetailId", index, ref result);
                    ValidateValue("RateValueStr", index, ref result);
                    break;
                case RateBo.ValuationRate6:
                    ValidateValue("InsuredGenderCodePickListDetailId", index, ref result);
                    ValidateValue("CedingTobaccoUsePickListDetailId", index, ref result);
                    ValidateValue("AttainedAge", index, ref result);
                    ValidateValue("PolicyTermRemain", index, ref result);
                    ValidateValue("RateValueStr", index, ref result);
                    break;
                case RateBo.ValuationRate7:
                    ValidateValue("AttainedAge", index, ref result);
                    ValidateValue("IssueAge", index, ref result);
                    ValidateValue("PolicyTermRemain", index, ref result);
                    ValidateValue("RateValueStr", index, ref result);
                    break;
                case RateBo.ValuationRate8:
                    ValidateValue("InsuredGenderCodePickListDetailId", index, ref result);
                    ValidateValue("CedingTobaccoUsePickListDetailId", index, ref result);
                    ValidateValue("IssueAge", index, ref result);
                    //ValidateValue("AttainedAge", index, ref result); // Allowed to be null
                    ValidateValue("PolicyTerm", index, ref result);
                    ValidateValue("RateValueStr", index, ref result);
                    break;
                case RateBo.ValuationRate9:
                    ValidateValue("InsuredGenderCodePickListDetailId", index, ref result);
                    ValidateValue("CedingOccupationCodePickListDetailId", index, ref result);
                    //ValidateValue("IssueAge", index, ref result); // Allowed to be null
                    //ValidateValue("AttainedAge", index, ref result); // Allowed to be null
                    ValidateValue("RateValueStr", index, ref result);
                    break;
                case RateBo.ValuationRate10:
                    ValidateValue("InsuredGenderCodePickListDetailId", index, ref result);
                    ValidateValue("CedingTobaccoUsePickListDetailId", index, ref result);
                    ValidateValue("CedingOccupationCodePickListDetailId", index, ref result);
                    //ValidateValue("IssueAge", index, ref result); // Allowed to be null
                    //ValidateValue("AttainedAge", index, ref result); // Allowed to be null
                    ValidateValue("RateValueStr", index, ref result);
                    break;
                default:
                    ValidateValue("RateValueStr", index, ref result);
                    break;
            }
            return result;
        }

        public void ValidateValue(string propertyName, int index, ref Result result)
        {
            var name = this.GetAttributeFrom<DisplayNameAttribute>(propertyName).DisplayName;
            var value = this.GetPropertyValue(propertyName);

            if (value == null)
            {
                result.AddError(string.Format("{0} is required at row #{1}", name, index));
                return;
            }

            switch (Type.GetTypeCode(value.GetType()))
            {
                case TypeCode.String:
                    if (string.IsNullOrEmpty(value.ToString()) || string.IsNullOrWhiteSpace(value.ToString()))
                    {
                        result.AddError(string.Format("{0} is required at row #{1}", name, index));
                    }
                    else
                    {
                        if (!Util.IsValidDouble(value.ToString(), out _, out _))
                        {
                            result.AddError(string.Format("{0} format is not valid at row #{1}", name, index));
                        }
                    }
                    break;
                case TypeCode.Int32:
                    switch (propertyName)
                    {
                        case "InsuredGenderCodePickListDetailId":
                        case "CedingTobaccoUsePickListDetailId":
                        case "CedingOccupationCodePickListDetailId":
                            if (value == null || int.Parse(value.ToString()) == 0)
                                result.AddError(string.Format("{0} is required at row #{1}", name, index));
                            break;
                        default:
                            if (value == null)
                                result.AddError(string.Format("{0} is required at row #{1}", name, index));
                            break;
                    }
                    break;
            }
        }
    }
}
