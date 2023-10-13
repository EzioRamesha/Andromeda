using Shared.ProcessFile;
using System;
using System.Collections.Generic;

namespace BusinessObject.TreatyPricing
{
    public class TreatyPricingGroupReferralGhsTableBo
    {
        public int Id { get; set; }

        public int TreatyPricingGroupReferralId { get; set; }
        public virtual TreatyPricingGroupReferralBo TreatyPricingGroupReferralBo { get; set; }

        public int? TreatyPricingGroupReferralFileId { get; set; }
        public TreatyPricingGroupReferralFileBo TreatyPricingGroupReferralFileBo { get; set; }

        public DateTime? CoverageStartDate { get; set; }

        public DateTime? EventDate { get; set; }

        public DateTime? ClaimListDate { get; set; }

        public string ClaimantsName { get; set; }

        public string CauseOfClaim { get; set; }

        public string RbCovered { get; set; }

        public string AolCovered { get; set; }

        public string Relationship { get; set; }

        public string HospitalCovered { get; set; }

        public string GrossClaimIncurred { get; set; }

        public string GrossClaimPaid { get; set; }

        public string GrossClaimPaidIbnr { get; set; }

        public string RiClaimPaid { get; set; }

        public int CreatedById { get; set; }
        public int? UpdatedById { get; set; }

        public string CoverageStartDateStr { get; set; }
        public string EventDateStr { get; set; }


        public const int ColumnCoverageStartDate = 1;
        public const int ColumnEventDate = 2;
        public const int ColumnName = 3;
        public const int ColumnClaimCauses = 4;
        public const int ColumnRbCovered = 5;
        public const int ColumnAolCovered = 6;
        public const int ColumnRelationship = 7;
        public const int ColumnHospitalCovered = 8;
        public const int ColumnGrossClaimIncurred = 9;
        public const int ColumnGrossClaimPaid = 10;
        public const int ColumnGrossClaimPaidIbnr = 11;
        public const int ColumnRiClaimPaid = 12;

        public static List<Column> GetColumns()
        {
            var columns = new List<Column>
            {
                new Column
                {
                    Header = "Coverage Start Date",
                    ColIndex = ColumnCoverageStartDate,
                    Property = "CoverageStartDate",
                },
                new Column
                {
                    Header = "Date of Event",
                    ColIndex = ColumnEventDate,
                    Property = "EventDate",
                },
                new Column
                {
                    Header = "Claimant's Name/ID",
                    ColIndex = ColumnName,
                    Property = "ClaimantsName",
                },
                new Column
                {
                    Header = "Cause of Claim",
                    ColIndex = ColumnClaimCauses,
                    Property = "CauseOfClaim",
                },
                new Column
                {
                    Header = "R&B Covered",
                    ColIndex = ColumnRbCovered,
                    Property = "RbCovered",
                },
                new Column
                {
                    Header = "AOL Covered",
                    ColIndex = ColumnAolCovered,
                    Property = "AolCovered",
                },
                new Column
                {
                    Header = "Relationship",
                    ColIndex = ColumnRelationship,
                    Property = "Relationship",
                },
                new Column
                {
                    Header = "Hospital Covered",
                    ColIndex = ColumnHospitalCovered,
                    Property = "HospitalCovered",
                },
                new Column
                {
                    Header = "Gross Claim Incurred",
                    ColIndex = ColumnGrossClaimIncurred,
                    Property = "GrossClaimIncurred",
                },
                new Column
                {
                    Header = "Gross Claim Paid",
                    ColIndex = ColumnGrossClaimPaid,
                    Property = "GrossClaimPaid",
                },
                new Column
                {
                    Header = "Gross Claim Paid (IBNR)",
                    ColIndex = ColumnGrossClaimPaidIbnr,
                    Property = "GrossClaimPaidIbnr",
                },
                new Column
                {
                    Header = "RI Claim Paid",
                    ColIndex = ColumnRiClaimPaid,
                    Property = "RiClaimPaid",
                },
            };

            return columns;
        }
    }
}
