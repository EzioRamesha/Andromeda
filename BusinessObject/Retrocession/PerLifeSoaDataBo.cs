using Shared.ProcessFile;
using System;
using System.Collections.Generic;

namespace BusinessObject.Retrocession
{
    public class PerLifeSoaDataBo
    {
        public int Id { get; set; }

        public int PerLifeSoaId { get; set; }
        public PerLifeSoaBo PerLifeSoaBo { get; set; }

        public int? PerLifeAggregationDetailDataId { get; set; }
        public PerLifeAggregationDetailDataBo PerLifeAggregationDetailDataBo { get; set; }

        public int? PerLifeClaimDataId { get; set; }
        public PerLifeClaimDataBo PerLifeClaimDataBo { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        // Claims
        public int? ClaimCategory { get; set; }
        public string RetroSoaQuarter { get; set; }
        public string EntryNo { get; set; }
        public string ClaimId { get; set; }
        public string ClaimTransactionType { get; set; }
        public string InsuredName { get; set; }
        public DateTime? InsuredDateOfBirth { get; set; }
        public string InsuredGenderCode { get; set; }
        public string PolicyNumber { get; set; }
        public DateTime? CedantDateOfNotification { get; set; }
        public string ReinsBasisCode { get; set; }
        public DateTime? ReinsEffDatePol { get; set; }
        public string TreatyCode { get; set; }
        public string ClaimCode { get; set; }
        public string MlreBenefitCode { get; set; }
        public double? ClaimRecoveryAmt { get; set; }
        public double? LateInterest { get; set; }
        public double? MlreRetainAmount { get; set; }
        public double? ExGratia { get; set; }
        public double? RetroRecoveryAmount { get; set; }
        public double? RetroLateInterest { get; set; }
        public double? RetroExGratia { get; set; }
        public string CauseOfEvent { get; set; }
        public DateTime? DateOfEvent { get; set; }
        public int ClaimStatus { get; set; }
        public int OffsetStatus { get; set; }

        public static List<Column> GetClaimColumns(bool ClaimCategoryPendingClaim = false)
        {
            var columns = new List<Column>
            {
                new Column
                {
                    Header = "Retro SOA Quarter",
                    Property = "SoaQuarter",
                },
                new Column
                {
                    Header = "Entry No",
                    Property = "EntryNo",
                },
                new Column
                {
                    Header = "Claim ID",
                    Property = "ClaimId",
                },
                new Column
                {
                    Header = "Claim Transaction Type",
                    Property = "ClaimTransactionType",
                },
                new Column
                {
                    Header = "Insured Name",
                    Property = "InsuredName",
                },
                new Column
                {
                    Header = "Policy Number",
                    Property = "PolicyNumber",
                },
                new Column
                {
                    Header = "Insured Gender Code",
                    Property = "InsuredGenderCode",
                },
                new Column
                {
                    Header = "Cedant Date of Notification",
                    Property = "CedantDateOfNotification",
                },
                new Column
                {
                    Header = "Insured Date of Birth",
                    Property = "InsuredDateOfBirth",
                },
                new Column
                {
                    Header = "Reins Effective Date Policy",
                    Property = "ReinsEffDatePol",
                },
                new Column
                {
                    Header = "Treaty Code",
                    Property = "TreatyCode",
                },
                new Column
                {
                    Header = "Reins Basis Code",
                    Property = "ReinsBasisCode",
                },
                new Column
                {
                    Header = "Claim Code",
                    Property = "ClaimCode",
                },
                new Column
                {
                    Header = "MLRe Benefit Code",
                    Property = "MlreBenefitCode",
                },
                new Column
                {
                    Header = "Claim Recovery Amount",
                    Property = "ClaimRecoveryAmt",
                },
                new Column
                {
                    Header = "Late Interest",
                    Property = "LateInterest",
                },
                new Column
                {
                    Header = "Ex Gratia",
                    Property = "ExGratia",
                },
                new Column
                {
                    Header = "MLRe Retain Amount",
                    Property = "MlreRetainAmount",
                },
                //new Column
                //{
                //    Header = "Recovery Amount",
                //    Property = "",
                //},
                //new Column
                //{
                //    Header = "Late Interest",
                //    Property = "",
                //},
                //new Column
                //{
                //    Header = "Ex-Gratia",
                //    Property = "",
                //},
                new Column
                {
                    Header = "Date of Event",
                    Property = "DateOfEvent",
                },
                new Column
                {
                    Header = "Cause Of Event",
                    Property = "CauseOfEvent",
                },
            };

            if (ClaimCategoryPendingClaim)
            {
                columns.Add(new Column
                {
                    Header = "Claim Status",
                    Property = "ClaimStatus",
                });
                columns.Add(new Column
                {
                    Header = "Offset Status",
                    Property = "OffsetStatus",
                });
            }

            return columns;
        }
    }
}
