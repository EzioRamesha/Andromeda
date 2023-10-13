using Shared.ProcessFile;
using System;
using System.Collections.Generic;

namespace BusinessObject.Retrocession
{
    public class PerLifeClaimRetroDataBo
    {
        public int Id { get; set; }

        public int PerLifeClaimDataId { get; set; }

        public PerLifeClaimDataBo PerLifeClaimDataBo { get; set; }

        public double? MlreShare { get; set; }

        public double? RetroClaimRecoveryAmount { get; set; }

        public double? LateInterest { get; set; }

        public double? ExGratia { get; set; }

        public int? RetroRecoveryId { get; set; }

        public int? RetroTreatyId { get; set; }

        public RetroTreatyBo RetroTreatyBo { get; set; }

        public double? RetroRatio { get; set; }

        public double? Aar { get; set; }

        public double? ComputedRetroRecoveryAmount { get; set; }

        public double? ComputedRetroLateInterest { get; set; }

        public double? ComputedRetroExGratia { get; set; }

        public string ReportedSoaQuarter { get; set; }

        public double? RetroRecoveryAmount { get; set; }

        public double? RetroLateInterest { get; set; }

        public double? RetroExGratia { get; set; }

        public int ComputedClaimCategory { get; set; }
        public string ComputedClaimCategoryStr { get; set; }

        public int ClaimCategory { get; set; }
        public string ClaimCategoryStr { get; set; }

        // for display in comparison
        public string ClaimRegisterOffsetStatusStr { get; set; }

        public string ClaimRecoveryStatusStr { get; set; }

        public string ClaimRecoveryDecisionStr { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? UpdatedById { get; set; }



        public static List<Column> GetColumns()
        {
            return new List<Column>
            {
                new Column()
                {
                    Header = "ID",
                    Property = "Id",
                },
                new Column()
                {
                    Header = "Entry No",
                    Property = "ClaimRegisterHistory.EntryNo",
                },
                new Column()
                {
                    Header = "SOA Quarter",
                    Property = "ClaimRegisterHistory.SoaQuarter",
                },
                new Column()
                {
                    Header = "Claim ID",
                    Property = "ClaimRegisterHistory.ClaimId",
                },
                new Column()
                {
                    Header = "Claim Type",
                    Property = "ClaimRegisterHistory.ClaimTransactionType",
                },
                new Column()
                {
                    Header = "Referral Case",
                    Property = "ClaimRegisterHistory.IsReferralCase",
                },
                new Column()
                {
                    Header = "Record Type",
                    Property = "ClaimRegisterHistory.Description",
                },
                new Column()
                {
                    Header = "Treaty Code",
                    Property = "ClaimRegisterHistory.TreatyCode",
                },
                new Column()
                {
                    Header = "Policy No",
                    Property = "ClaimRegisterHistory.PolicyNumber",
                },
                new Column()
                {
                    Header = "Ceding Company",
                    Property = "ClaimRegisterHistory.CedingCompany",
                },
                new Column()
                {
                    Header = "Assured Name",
                    Property = "ClaimRegisterHistory.InsuredName",
                },
                new Column()
                {
                    Header = "Date of Birth",
                    Property = "ClaimRegisterHistory.InsuredDateOfBirth",
                },
                new Column()
                {
                    Header = "Date of Transaction",
                    Property = "ClaimRegisterHistory.LastTransactionDate",
                },
                new Column()
                {
                    Header = "Date of Report",
                    Property = "ClaimRegisterHistory.DateOfReported",
                },
                new Column()
                {
                    Header = "Date Notified",
                    Property = "ClaimRegisterHistory.CedantDateOfNotification",
                },
                new Column()
                {
                    Header = "Date Registered",
                    Property = "ClaimRegisterHistory.DateOfRegister",
                },
                new Column()
                {
                    Header = "Date of Commencement",
                    Property = "ClaimRegisterHistory.DateOfIntimation",
                },
                new Column()
                {
                    Header = "Date of Event",
                    Property = "ClaimRegisterHistory.DateOfEvent",
                },
                new Column()
                {
                    Header = "Policy Duration",
                    Property = "ClaimRegisterHistory.PolicyDuration",
                },
                new Column()
                {
                    Header = "Target Date to Issue Invoice",
                    Property = "ClaimRegisterHistory.TargetDateToIssueInvoice",
                },
                new Column()
                {
                    Header = "Sum Reinsured (MYR)",
                    Property = "ClaimRegisterHistory.Layer1SumRein",
                },
                new Column()
                {
                    Header = "Cause of Event",
                    Property = "ClaimRegisterHistory.CauseOfEvent",
                },
                new Column()
                {
                    Header = "Person In-Charge (Claims)",
                    Property = "ClaimRegisterHistory.ClaimRegister.PicClaim.FullName",
                },
                new Column()
                {
                    Header = "Person In-Charge (DA&A)",
                    Property = "ClaimRegisterHistory.ClaimRegister.PicDaa.FullName",
                },
                new Column()
                {
                    Header = "Claim Status",
                    Property = "ClaimRegisterHistory.ClaimStatus",
                },
                new Column()
                {
                    Header = "Provision Status",
                    Property = "ClaimRegisterHistory.ProvisionStatus",
                },
                new Column()
                {
                    Header = "Offset Status",
                    Property = "ClaimRegisterHistory.OffsetStatus",
                }
            };
        }
    }
}
