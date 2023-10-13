using Shared.ProcessFile;
using System;
using System.Collections.Generic;

namespace BusinessObject
{
    public class TreatyBenefitCodeMappingBo
    {
        public int Id { get; set; }

        public int CedantId { get; set; }

        public CedantBo CedantBo { get; set; }

        public int? TreatyBenefitCodeMappingUploadId { get; set; }

        public TreatyBenefitCodeMappingUploadBo TreatyBenefitCodeMappingUploadBo { get; set; }

        public string CedantCode { get; set; }

        public int BenefitId { get; set; }

        public BenefitBo BenefitBo { get; set; }

        public string BenefitCode { get; set; }

        public int TreatyCodeId { get; set; }

        public TreatyCodeBo TreatyCodeBo { get; set; }

        public string TreatyCode { get; set; }

        public string CedingPlanCode { get; set; }

        public string Description { get; set; }

        public string CedingBenefitTypeCode { get; set; }

        public string CedingBenefitRiskCode { get; set; }

        public string CedingTreatyCode { get; set; }

        public string CampaignCode { get; set; }

        public DateTime? ReinsEffDatePolStartDate { get; set; }

        public DateTime? ReinsEffDatePolEndDate { get; set; }

        public int? ReinsBasisCodePickListDetailId { get; set; }

        public PickListDetailBo ReinsBasisCodePickListDetailBo { get; set; }

        public string ReinsBasisCode { get; set; }

        public int? AttainedAgeFrom { get; set; }

        public int? AttainedAgeTo { get; set; }

        public DateTime? ReportingStartDate { get; set; }

        public DateTime? ReportingEndDate { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? UpdatedById { get; set; }

        // Phase 2
        public string ProfitComm { get; set; }

        public int? ProfitCommPickListDetailId { get; set; }

        public PickListDetailBo ProfitCommPickListDetailBo { get; set; }

        public int? MaxExpiryAge { get; set; }

        public int? MinIssueAge { get; set; }

        public int? MaxIssueAge { get; set; }

        public double? MaxUwRating { get; set; }

        public double? ApLoading { get; set; }

        public double? MinAar { get; set; }

        public double? MaxAar { get; set; }

        public double? AblAmount { get; set; }

        public double? RetentionShare { get; set; }

        public double? RetentionCap { get; set; }

        public double? RiShare { get; set; }

        public double? RiShareCap { get; set; }

        public double? ServiceFee { get; set; }

        public double? WakalahFee { get; set; }

        public double? UnderwriterRatingFrom { get; set; }

        public double? UnderwriterRatingTo { get; set; }

        public double? RiShare2 { get; set; }

        public double? RiShareCap2 { get; set; }

        public double? OriSumAssuredFrom { get; set; }

        public double? OriSumAssuredTo { get; set; }

        public DateTime? EffectiveDate { get; set; }

        public int? ReinsuranceIssueAgeFrom { get; set; }

        public int? ReinsuranceIssueAgeTo { get; set; }

        public const int ColumnId = 1;
        public const int ColumnCedantCode = 2;
        public const int ColumnCedingPlanCode = 3;
        public const int ColumnDescription = 4;
        public const int ColumnCedingBenefitTypeCode = 5;
        public const int ColumnCedingBenefitRiskCode = 6;
        public const int ColumnCedingTreatyCode = 7;
        public const int ColumnCampaignCode = 8;
        public const int ColumnReinsEffDatePolStartDate = 9;
        public const int ColumnReinsEffDatePolEndDate = 10;
        public const int ColumnReinsBasisCode = 11;
        public const int ColumnAttainedAgeFrom = 12;
        public const int ColumnAttainedAgeTo = 13;
        public const int ColumnReportingStartDate = 14;
        public const int ColumnReportingEndDate = 15;
        public const int ColumnUnderwriterRatingFrom = 16;
        public const int ColumnUnderwriterRatingTo = 17;
        public const int ColumnOriSumAssuredFrom = 18;
        public const int ColumnOriSumAssuredTo = 19;
        public const int ColumnReinsuranceIssueAgeFrom = 20;
        public const int ColumnReinsuranceIssueAgeTo = 21;
        public const int ColumnTreatyCode = 22;
        public const int ColumnBenefitCode = 23;
        public const int ColumnProfitComm = 24;
        public const int ColumnMaxExpiryAge = 25;
        public const int ColumnMinIssueAge = 26;
        public const int ColumnMaxIssueAge = 27;
        public const int ColumnMaxUwRating = 28;
        public const int ColumnApLoading = 29;
        public const int ColumnMinAar = 30;
        public const int ColumnMaxAar = 31;
        public const int ColumnAblAmount = 32;
        public const int ColumnRetentionShare = 33;
        public const int ColumnRetentionCap = 34;
        public const int ColumnRiShare = 35;
        public const int ColumnRiShareCap = 36;
        public const int ColumnRiShare2 = 37;
        public const int ColumnRiShareCap2 = 38;
        public const int ColumnServiceFee = 39;
        public const int ColumnWakalahFee = 40;
        public const int ColumnEffectiveDate = 41;
        public const int ColumnAction = 42;

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
                    Header = "Ceding Plan Code",
                    ColIndex = ColumnCedingPlanCode,
                    Property = "CedingPlanCode",
                },
                new Column
                {
                    Header = "Description",
                    ColIndex = ColumnDescription,
                    Property = "Description",
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
                    Header = "Ceding Treaty Code",
                    ColIndex = ColumnCedingTreatyCode,
                    Property = "CedingTreatyCode",
                },
                new Column
                {
                    Header = "Campaign Code",
                    ColIndex = ColumnCampaignCode,
                    Property = "CampaignCode",
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
                    Header = "Reinsurance Basis Code",
                    ColIndex = ColumnReinsBasisCode,
                    Property = "ReinsBasisCode",
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
                    Header = "Underwriter Rating From",
                    ColIndex = ColumnUnderwriterRatingFrom,
                    Property = "UnderwriterRatingFrom",
                },
                new Column
                {
                    Header = "Underwriter Rating To",
                    ColIndex = ColumnUnderwriterRatingTo,
                    Property = "UnderwriterRatingTo",
                },
                new Column
                {
                    Header = "Ori Sum Assured From",
                    ColIndex = ColumnOriSumAssuredFrom,
                    Property = "OriSumAssuredFrom",
                },
                new Column
                {
                    Header = "Ori Sum Assured To",
                    ColIndex = ColumnOriSumAssuredTo,
                    Property = "OriSumAssuredTo",
                },
                new Column
                {
                    Header = "Reinsurance Issue Age From",
                    ColIndex = ColumnReinsuranceIssueAgeFrom,
                    Property = "ReinsuranceIssueAgeFrom",
                },
                new Column
                {
                    Header = "Reinsurance Issue Age To",
                    ColIndex = ColumnReinsuranceIssueAgeTo,
                    Property = "ReinsuranceIssueAgeTo",
                },
                new Column
                {
                    Header = "Treaty Code",
                    ColIndex = ColumnTreatyCode,
                    Property = "TreatyCode",
                },
                new Column
                {
                    Header = "MLRe Benefit Code",
                    ColIndex = ColumnBenefitCode,
                    Property = "BenefitCode",
                },
                new Column
                {
                    Header = "Profit Commission",
                    ColIndex = ColumnProfitComm,
                    Property = "ProfitComm",
                },
                new Column
                {
                    Header = "Maximum Age at Expiry",
                    ColIndex = ColumnMaxExpiryAge,
                    Property = "MaxExpiryAge",
                },
                new Column
                {
                    Header = "Minimum Issue Age",
                    ColIndex = ColumnMinIssueAge,
                    Property = "MinIssueAge",
                },
                new Column
                {
                    Header = "Maximum Issue Age",
                    ColIndex = ColumnMaxIssueAge,
                    Property = "MaxIssueAge",
                },
                new Column
                {
                    Header = "Maximum Underwriting Rating",
                    ColIndex = ColumnMaxUwRating,
                    Property = "MaxUwRating",
                },
                new Column
                {
                    Header = "AP Loading",
                    ColIndex = ColumnApLoading,
                    Property = "ApLoading",
                },
                new Column
                {
                    Header = "Minimum AAR",
                    ColIndex = ColumnMinAar,
                    Property = "MinAar",
                },
                new Column
                {
                    Header = "Maximum AAR",
                    ColIndex = ColumnMaxAar,
                    Property = "MaxAar",
                },
                new Column
                {
                    Header = "ABL Amount",
                    ColIndex = ColumnAblAmount,
                    Property = "AblAmount",
                },
                new Column
                {
                    Header = "Retention Share",
                    ColIndex = ColumnRetentionShare,
                    Property = "RetentionShare",
                },
                new Column
                {
                    Header = "Retention Cap",
                    ColIndex = ColumnRetentionCap,
                    Property = "RetentionCap",
                },
                new Column
                {
                    Header = "RI Share 1",
                    ColIndex = ColumnRiShare,
                    Property = "RiShare",
                },
                new Column
                {
                    Header = "RI Share Cap 1",
                    ColIndex = ColumnRiShareCap,
                    Property = "RiShareCap",
                },
                new Column
                {
                    Header = "RI Share 2",
                    ColIndex = ColumnRiShare2,
                    Property = "RiShare2",
                },
                new Column
                {
                    Header = "RI Share Cap 2",
                    ColIndex = ColumnRiShareCap2,
                    Property = "RiShareCap2",
                },
                new Column
                {
                    Header = "Service Fee",
                    ColIndex = ColumnServiceFee,
                    Property = "ServiceFee",
                },
                new Column
                {
                    Header = "Wakalah Fee",
                    ColIndex = ColumnWakalahFee,
                    Property = "WakalahFee",
                },
                new Column
                {
                    Header = "Treaty/Product Effective Date",
                    ColIndex = ColumnEffectiveDate,
                    Property = "EffectiveDate",
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
