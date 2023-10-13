using Shared.ProcessFile;
using System;
using System.Collections.Generic;

namespace BusinessObject.TreatyPricing
{
    public class TreatyPricingGroupReferralGtlTableBo
    {
        public int Id { get; set; }

        public int TreatyPricingGroupReferralId { get; set; }
        public virtual TreatyPricingGroupReferralBo TreatyPricingGroupReferralBo { get; set; }

        public int? TreatyPricingGroupReferralFileId { get; set; }
        public TreatyPricingGroupReferralFileBo TreatyPricingGroupReferralFileBo { get; set; }

        public int Type { get; set; }

        public string BenefitCode { get; set; }

        public string Designation { get; set; }

        public DateTime? CoverageStartDate { get; set; }

        public DateTime? CoverageEndDate { get; set; }

        public DateTime? EventDate { get; set; }

        public DateTime? ClaimListDate { get; set; }

        public string ClaimantsName { get; set; }

        public string CauseOfClaim { get; set; }

        public string ClaimType { get; set; }

        public string AgeBandRange { get; set; }

        public string BasisOfSA { get; set; }

        public string GrossClaim { get; set; }

        public string RiClaim { get; set; }

        public string RiskRate { get; set; }

        public string GrossRate { get; set; }

        public int CreatedById { get; set; }
        public int? UpdatedById { get; set; }

        public string CoverageStartDateStr { get; set; }
        public string EventDateStr { get; set; }


        public const int TypeGtlClaim = 1;
        public const int TypeGtlUnitRates = 2;
        public const int TypeGtlAgeBanded = 3;
        public const int TypeGtlBasisOfSa = 4;
        public const int TypeGtlMax = 4;

        public const int ColumnCoverageStartDate = 1;
        // Claim Experience
        public const int ColumnEventDate = 2;
        public const int ColumnName = 3;
        public const int ColumnClaimCauses = 4;
        public const int ColumnClaimType = 5;
        public const int ColumnClaimGross = 6;
        public const int ColumnClaimRi = 7;
        // Unit Rates
        public const int ColumnBenefit = 2;
        public const int ColumnRiskRate = 3;
        public const int ColumnGrossRate = 4;
        // Age Banded
        public const int ColumnAgeBand = 3;
        public const int ColumnRateRisk = 4;
        public const int ColumnRateGross = 5;
        // Basis of SA
        public const int ColumnDesignation = 3;
        public const int ColumnBasis = 4;

        public static List<Column> GetColumns(int type)
        {
            var columns = new List<Column> 
            {
                new Column
                {
                    Header = "Coverage Start Date",
                    ColIndex = ColumnCoverageStartDate,
                    Property = "CoverageStartDate",
                },
            };

            switch (type)
            {
                case TypeGtlClaim:
                    columns.Add(new Column { Header = "Date of Event", ColIndex = ColumnEventDate, Property = "EventDate", });
                    columns.Add(new Column { Header = "Claimant's Name/ID", ColIndex = ColumnName, Property = "ClaimantsName", });
                    columns.Add(new Column { Header = "Cause of Claim", ColIndex = ColumnClaimCauses, Property = "CauseOfClaim", });
                    columns.Add(new Column { Header = "Type of Claim", ColIndex = ColumnClaimType, Property = "ClaimType", });
                    columns.Add(new Column { Header = "Gross Claim", ColIndex = ColumnClaimGross, Property = "GrossClaim", });
                    columns.Add(new Column { Header = "RI Claim", ColIndex = ColumnClaimRi, Property = "RiClaim", });
                    break;
                case TypeGtlUnitRates:
                    columns.Add(new Column { Header = "Benefit", ColIndex = ColumnBenefit, Property = "BenefitCode", });
                    columns.Add(new Column { Header = "Risk Rate per 1000", ColIndex = ColumnRiskRate, Property = "RiskRate", });
                    columns.Add(new Column { Header = "Gross Rate per 1000", ColIndex = ColumnGrossRate, Property = "GrossRate", });
                    break;
                case TypeGtlAgeBanded:
                    columns.Add(new Column { Header = "Benefit", ColIndex = ColumnBenefit, Property = "BenefitCode", });
                    columns.Add(new Column { Header = "Age Band", ColIndex = ColumnAgeBand, Property = "AgeBandRange", });
                    columns.Add(new Column { Header = "Risk Rate per 1000", ColIndex = ColumnRateRisk, Property = "RiskRate", });
                    columns.Add(new Column { Header = "Gross Rate per 1000", ColIndex = ColumnRateGross, Property = "GrossRate", });
                    break;
                case TypeGtlBasisOfSa:
                    columns.Add(new Column { Header = "Benefit", ColIndex = ColumnBenefit, Property = "BenefitCode", });
                    columns.Add(new Column { Header = "Designation", ColIndex = ColumnDesignation, Property = "Designation", });
                    columns.Add(new Column { Header = "Basis of SA", ColIndex = ColumnBasis, Property = "BasisOfSA", });
                    break;
            }
            return columns;
        }
    }
}
