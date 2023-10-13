using Shared;
using Shared.ProcessFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BusinessObject
{
    public class RateTableBo
    {
        public int Id { get; set; }

        public int? RateTableMappingUploadId { get; set; }

        public RateTableMappingUploadBo RateTableMappingUploadBo { get; set; }

        public string TreatyCode { get; set; }

        public int? BenefitId { get; set; }

        public BenefitBo BenefitBo { get; set; }

        public string BenefitCode { get; set; }

        public string CedingPlanCode { get; set; }

        public int? PremiumFrequencyCodePickListDetailId { get; set; }

        public PickListDetailBo PremiumFrequencyCodePickListDetailBo { get; set; }

        public string PremiumFrequencyCode { get; set; }

        public double? PolicyAmountFrom { get; set; }

        public double? PolicyAmountTo { get; set; }

        public int? AttainedAgeFrom { get; set; }

        public int? AttainedAgeTo { get; set; }

        public DateTime? ReinsEffDatePolStartDate { get; set; }

        public DateTime? ReinsEffDatePolEndDate { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? UpdatedById { get; set; }

        public string CedingTreatyCode { get; set; }

        public string CedingPlanCode2 { get; set; }

        public string CedingBenefitTypeCode { get; set; }

        public string CedingBenefitRiskCode { get; set; }

        public string GroupPolicyNumber { get; set; }

        public int? ReinsBasisCodePickListDetailId { get; set; }

        public PickListDetailBo ReinsBasisCodePickListDetailBo { get; set; }

        public string ReinsBasisCode { get; set; }

        public double? AarFrom { get; set; }

        public double? AarTo { get; set; }

        // Phase 2
        public double? PolicyTermFrom { get; set; }

        public double? PolicyTermTo { get; set; }

        public double? PolicyDurationFrom { get; set; }

        public double? PolicyDurationTo { get; set; }

        public int? RateId { get; set; }

        public RateBo RateBo { get; set; }

        public string RateCode { get; set; }

        public int? CedantId { get; set; }

        public CedantBo CedantBo { get; set; }

        public string CedantCode { get; set; }

        public string RiDiscountCode { get; set; }

        public string LargeDiscountCode { get; set; }

        public string GroupDiscountCode { get; set; }

        public DateTime? ReportingStartDate { get; set; }

        public DateTime? ReportingEndDate { get; set; }

        public const int ColumnId = 1;
        public const int ColumnCedantCode = 2;
        public const int ColumnTreatyCode = 3;
        public const int ColumnCedingTreatyCode = 4;
        public const int ColumnCedingPlanCode = 5;
        public const int ColumnCedingPlanCode2 = 6;
        public const int ColumnCedingBenefitTypeCode = 7;
        public const int ColumnCedingBenefitRiskCode = 8;
        public const int ColumnPolicyTermFrom = 9;
        public const int ColumnPolicyTermTo = 10;
        public const int ColumnPolicyDurationFrom = 11;
        public const int ColumnPolicyDurationTo = 12;
        public const int ColumnGroupPolicyNumber = 13;
        public const int ColumnBenefitCode = 14;
        public const int ColumnReinsEffDatePolStartDate = 15;
        public const int ColumnReinsEffDatePolEndDate = 16;
        public const int ColumnReportingStartDate = 17;
        public const int ColumnReportingEndDate = 18;
        public const int ColumnAttainedAgeFrom = 19;
        public const int ColumnAttainedAgeTo = 20;
        public const int ColumnPremiumFrequencyCode = 21;
        public const int ColumnPolicyAmountFrom = 22;
        public const int ColumnPolicyAmountTo = 23;
        public const int ColumnAarFrom = 24;
        public const int ColumnAarTo = 25;
        public const int ColumnReinsBasisCode = 26;
        public const int ColumnRateCode = 27;
        public const int ColumnRiDiscountCode = 28;
        public const int ColumnLargeDiscountCode = 29;
        public const int ColumnGroupDiscountCode = 30;
        public const int ColumnAction = 31;

        public static List<Column> GetColumns()
        {
            return new List<Column>
            {
                new Column
                {
                    Header = "ID",
                    ColIndex = ColumnId,
                    Property = "Id",
                },
                new Column
                {
                    Header = "Ceding Company",
                    ColIndex = ColumnCedantCode,
                    Property = "CedantCode",
                },
                new Column
                {
                    Header = "Treaty Code",
                    ColIndex = ColumnTreatyCode,
                    Property = "TreatyCode",
                },
                new Column
                {
                    Header = "Ceding Treaty Code",
                    ColIndex = ColumnCedingTreatyCode,
                    Property = "CedingTreatyCode",
                },
                new Column
                {
                    Header = "Ceding Plan Code",
                    ColIndex = ColumnCedingPlanCode,
                    Property = "CedingPlanCode",
                },
                new Column
                {
                    Header = "Ceding Plan Code 2",
                    ColIndex = ColumnCedingPlanCode2,
                    Property = "CedingPlanCode2",
                },
                new Column
                {
                    Header = "Ceding Benefit Type Code",
                    ColIndex = ColumnCedingBenefitTypeCode,
                    Property = "CedingBenefitTypeCode",
                },
                new Column
                {
                    Header = "Ceding Benefit Risk Code",
                    ColIndex = ColumnCedingBenefitRiskCode,
                    Property = "CedingBenefitRiskCode",
                },
                new Column
                {
                    Header = "Policy Term From",
                    ColIndex = ColumnPolicyTermFrom,
                    Property = "PolicyTermFrom",
                },
                new Column
                {
                    Header = "Policy Term To",
                    ColIndex = ColumnPolicyTermTo,
                    Property = "PolicyTermTo",
                },
                new Column
                {
                    Header = "Policy Duration From",
                    ColIndex = ColumnPolicyDurationFrom,
                    Property = "PolicyDurationFrom",
                },
                new Column
                {
                    Header = "Policy Duration To",
                    ColIndex = ColumnPolicyDurationTo,
                    Property = "PolicyDurationTo",
                },
                new Column
                {
                    Header = "Group Policy Number",
                    ColIndex = ColumnGroupPolicyNumber,
                    Property = "GroupPolicyNumber",
                },
                new Column
                {
                    Header = "MLRe Benefit Code",
                    ColIndex = ColumnBenefitCode,
                    Property = "BenefitCode",
                },
                new Column
                {
                    Header = "Policy Reinsurance Effective Start Date",
                    ColIndex = ColumnReinsEffDatePolStartDate,
                    Property = "ReinsEffDatePolStartDate",
                },
                new Column
                {
                    Header = "Policy Reinsurance Effective End Date",
                    ColIndex = ColumnReinsEffDatePolEndDate,
                    Property = "ReinsEffDatePolEndDate",
                },
                new Column
                {
                    Header = "Reporting Start Date",
                    ColIndex = ColumnReportingStartDate,
                    Property = "ReportingStartDate",
                },
                new Column
                {
                    Header = "Reporting End Date",
                    ColIndex = ColumnReportingEndDate,
                    Property = "ReportingEndDate",
                },
                new Column
                {
                    Header = "Insured Attained Age From",
                    ColIndex = ColumnAttainedAgeFrom,
                    Property = "AttainedAgeFrom",
                },
                new Column
                {
                    Header = "Insured Attained Age To",
                    ColIndex = ColumnAttainedAgeTo,
                    Property = "AttainedAgeTo",
                },
                new Column
                {
                    Header = "Premium Frequency Code",
                    ColIndex = ColumnPremiumFrequencyCode,
                    Property = "PremiumFrequencyCode",
                },
                new Column
                {
                    Header = "ORI Sum Assured From",
                    ColIndex = ColumnPolicyAmountFrom,
                    Property = "PolicyAmountFrom",
                },
                new Column
                {
                    Header = "ORI Sum Assured To",
                    ColIndex = ColumnPolicyAmountTo,
                    Property = "PolicyAmountTo",
                },
                new Column
                {
                    Header = "AAR From",
                    ColIndex = ColumnAarFrom,
                    Property = "AarFrom",
                },
                new Column
                {
                    Header = "AAR To",
                    ColIndex = ColumnAarTo,
                    Property = "AarTo",
                },
                new Column
                {
                    Header = "Reinsurance Basis Code",
                    ColIndex = ColumnReinsBasisCode,
                    Property = "ReinsBasisCode",
                },
                new Column
                {
                    Header = "Rate Table Code",
                    ColIndex = ColumnRateCode,
                    Property = "RateCode",
                },
                new Column
                {
                    Header = "RI Discount Code",
                    ColIndex = ColumnRiDiscountCode,
                    Property = "RiDiscountCode",
                },
                new Column
                {
                    Header = "Large Discount Code",
                    ColIndex = ColumnLargeDiscountCode,
                    Property = "LargeDiscountCode",
                },
                new Column
                {
                    Header = "Group Discount Code",
                    ColIndex = ColumnGroupDiscountCode,
                    Property = "GroupDiscountCode",
                },
                new Column
                {
                    Header = "Action",
                    ColIndex = ColumnAction,
                },
            };
        }
    }
}
