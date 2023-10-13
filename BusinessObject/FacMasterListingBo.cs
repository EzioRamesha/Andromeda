using Shared.ProcessFile;
using System;
using System.Collections.Generic;

namespace BusinessObject
{
    public class FacMasterListingBo
    {
        public int Id { get; set; }

        public string UniqueId { get; set; }

        public int? EwarpNumber { get; set; }

        public string InsuredName { get; set; }

        public DateTime? InsuredDateOfBirth { get; set; }

        public int? InsuredGenderCodePickListDetailId { get; set; }

        public PickListDetailBo InsuredGenderCodePickListDetailBo { get; set; }

        public string InsuredGenderCode { get; set; }

        public int? CedantId { get; set; }

        public CedantBo CedantBo { get; set; }

        public string CedantCode { get; set; }

        public string PolicyNumber { get; set; }

        public double? FlatExtraAmountOffered { get; set; }

        public int? FlatExtraDuration { get; set; }

        public string BenefitCode { get; set; }

        public double? SumAssuredOffered { get; set; }

        public string EwarpActionCode { get; set; }

        public double? UwRatingOffered { get; set; }

        public DateTime? OfferLetterSentDate { get; set; }

        public string UwOpinion { get; set; }

        public string Remark { get; set; }

        public string CedingBenefitTypeCode { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? UpdatedById { get; set; }

        public const int TypeUniqueId = 1;
        public const int TypeEwarpNumber = 2;
        public const int TypeInsuredName = 3;
        public const int TypeInsuredDateOfBirth = 4;
        public const int TypeInsuredGenderCode = 5;
        public const int TypeCedantCode = 6;
        public const int TypePolicyNumber = 7;
        public const int TypeFlatExtraAmountOffered = 8;
        public const int TypeFlatExtraDuration = 9;
        public const int TypeBenefitCode = 10;
        public const int TypeSumAssuredOffered = 11;
        public const int TypeEwarpActionCode = 12;
        public const int TypeUwRatingOffered = 13;
        public const int TypeOfferLetterSentDate = 14;
        public const int TypeUwOpinion = 15;
        public const int TypeRemark = 16;
        public const int TypeCedingBenefitTypeCode = 17;

        public static List<Column> GetColumns()
        {
            return new List<Column>
            {
                new Column
                {
                    Header = "Unique Id",
                    ColIndex = TypeUniqueId,
                    Property = "UniqueId",
                },
                new Column
                {
                    Header = "eWarp Number",
                    ColIndex = TypeEwarpNumber,
                    Property = "EwarpNumber",
                },
                new Column
                {
                    Header = "Insured Name",
                    ColIndex = TypeInsuredName,
                    Property = "InsuredName",
                },
                new Column
                {
                    Header = "Insured Date Of Birth",
                    ColIndex = TypeInsuredDateOfBirth,
                    Property = "InsuredDateOfBirth",
                },
                new Column
                {
                    Header = "Insured Gender Code",
                    ColIndex = TypeInsuredGenderCode,
                    Property = "InsuredGenderCode",
                },
                new Column
                {
                    Header = "Ceding Company",
                    ColIndex = TypeCedantCode,
                    Property = "CedantCode",
                },
                new Column
                {
                    Header = "Policy Number",
                    ColIndex = TypePolicyNumber,
                    Property = "PolicyNumber",
                },
                new Column
                {
                    Header = "Flat Extra Amount Offered",
                    ColIndex = TypeFlatExtraAmountOffered,
                    Property = "FlatExtraAmountOffered",
                },
                new Column
                {
                    Header = "Flat Extra Duration",
                    ColIndex = TypeFlatExtraDuration,
                    Property = "FlatExtraDuration",
                },
                new Column
                {
                    Header = "MLRe Benefit Code",
                    ColIndex = TypeBenefitCode,
                    Property = "BenefitCode",
                },
                new Column
                {
                    Header = "Sum Assured Offered",
                    ColIndex = TypeSumAssuredOffered,
                    Property = "SumAssuredOffered",
                },
                new Column
                {
                    Header = "eWarp Action Code",
                    ColIndex = TypeEwarpActionCode,
                    Property = "EwarpActionCode",
                },
                new Column
                {
                    Header = "UW Rating Offered",
                    ColIndex = TypeUwRatingOffered,
                    Property = "UwRatingOffered",
                },
                new Column
                {
                    Header = "Offer Letter Sent Date",
                    ColIndex = TypeOfferLetterSentDate,
                    Property = "OfferLetterSentDate",
                },
                new Column
                {
                    Header = "UW Opinion",
                    ColIndex = TypeUwOpinion,
                    Property = "UwOpinion",
                },
                new Column
                {
                    Header = "Remark",
                    ColIndex = TypeRemark,
                    Property = "Remark",
                },
                new Column
                {
                    Header = "Ceding Benefit Type Code",
                    ColIndex = TypeCedingBenefitTypeCode,
                    Property = "CedingBenefitTypeCode",
                },
            };
        }
    }
}
