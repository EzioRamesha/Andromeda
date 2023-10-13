using Shared.ProcessFile;
using System;
using System.Collections.Generic;

namespace BusinessObject.Sanctions
{
    public class SanctionVerificationDetailBo
    {
        public int Id { get; set; }

        public int SanctionVerificationId { get; set; }

        public SanctionVerificationBo SanctionVerificationBo { get; set; }

        public int ModuleId { get; set; }

        public ModuleBo ModuleBo { get; set; }

        public int ObjectId { get; set; }

        public int? BatchId { get; set; }

        public int Rule { get; set; }

        public DateTime? UploadDate { get; set; }

        public string Category { get; set; }

        public string CedingCompany { get; set; }

        public string TreatyCode { get; set; }

        public string CedingPlanCode { get; set; }

        public string PolicyNumber { get; set; }

        public string InsuredName { get; set; }

        public DateTime? InsuredDateOfBirth { get; set; }

        public string InsuredIcNumber { get; set; }

        public string SoaQuarter { get; set; }

        public double? SumReins { get; set; }

        public double? ClaimAmount { get; set; }

        public string SanctionRefNumber { get; set; }

        public List<string> SanctionRefNumbers { get; set; }

        public string SanctionIdNumber { get; set; }

        public List<string> SanctionIdNumbers { get; set; }

        public string SanctionAddress { get; set; }

        public List<string> SanctionAddresses { get; set; }

        public string LineOfBusiness { get; set; }

        public DateTime? PolicyCommencementDate { get; set; }

        public string PolicyStatusCode { get; set; }

        public DateTime? RiskCoverageEndDate { get; set; }

        public double? GrossPremium { get; set; }

        public bool IsWhitelist { get; set; } = false;

        public bool IsExactMatch { get; set; } = false;

        public string Remark { get; set; }

        public int? PreviousDecision { get; set; }

        public string PreviousDecisionStr { get; set; }

        public string PreviousDecisionRemark { get; set; }

        public string Source { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? UpdatedById { get; set; }

        public const int TypeModuleId = 1;
        public const int TypeSource = 2;
        public const int TypeId = 3;
        public const int TypeUploadDate = 4;
        public const int TypeCategory = 5;
        public const int TypeCedingCompany = 6;
        public const int TypeTreatyCode = 7;
        public const int TypeCedingPlanCode = 8;
        public const int TypePolicyNumber = 9;
        public const int TypeInsuredName = 10;
        public const int TypeInsuredDateOfBirth = 11;
        public const int TypeInsuredIcNumber = 12;
        public const int TypeSoaQuarter = 13;
        public const int TypeSumReins = 14;
        public const int TypeClaimAmount = 15;
        public const int TypeIsWhitelist = 16;
        public const int TypeBatchId = 17;
        public const int TypePreviousDecision = 18;

        public const int PreviousDecisionPending = 1;
        public const int PreviousDecisionWhitelist = 2;
        public const int PreviousDecisionExactMatch = 3;

        public const int RuleIdentity = 1;
        public const int RuleNameSymbolRemoval = 2;
        public const int RuleNameKeywordReplacement = 3;
        public const int RuleNameGroupKeyword = 4;
        public const int RuleNameGroupKeywordReplacement = 5;
        public const int MaxRule = 5;

        public const string PolicyStatusCodeActive = "ACTIVE";
        public const string PolicyStatusCodeClosed = "CLOSED";

        public static List<Column> GetColumns()
        {
            return new List<Column>()
            {
                new Column
                {
                    Header = "Check Against",
                    Property = "ModuleId",
                    Type = TypeModuleId,
                },
                new Column
                {
                    Header = "Source",
                    Property = "Source",
                    Type = TypeSource,
                },
                new Column
                {
                    Header = "ID",
                    Property = "ObjectId",
                    Type = TypeId,
                },
                //new Column
                //{
                //    Header = "Date Uploaded",
                //    Property = "UploadDate",
                //    Type = TypeUploadDate,
                //},
                new Column
                {
                    Header = "Ceding Company",
                    Property = "CedingCompany",
                    Type = TypeCedingCompany,
                },
                new Column
                {
                    Header = "Treaty Code",
                    Property = "TreatyCode",
                    Type = TypeTreatyCode,
                },
                new Column
                {
                    Header = "Ceding Plan Code",
                    Property = "CedingPlanCode",
                    Type = TypeCedingPlanCode,
                },
                new Column
                {
                    Header = "Category",
                    Property = "Category",
                    Type = TypeCategory,
                },
                new Column
                {
                    Header = "Policy Number",
                    Property = "PolicyNumber",
                    Type = TypePolicyNumber,
                },
                new Column
                {
                    Header = "Insured Name",
                    Property = "InsuredName",
                    Type = TypeInsuredName,
                },
                new Column
                {
                    Header = "Date of Birth",
                    Property = "InsuredDateOfBirth",
                    Type = TypeInsuredDateOfBirth,
                },
                new Column
                {
                    Header = "NRIC",
                    Property = "InsuredIcNumber",
                    Type = TypeInsuredIcNumber,
                },
                new Column
                {
                    Header = "SOA Quarter",
                    Property = "SoaQuarter",
                    Type = TypeSoaQuarter,
                },
                new Column
                {
                    Header = "Sum Reinsurance",
                    Property = "SumReins",
                    Type = TypeSumReins,
                },
                new Column
                {
                    Header = "Claim Amount",
                    Property = "ClaimAmount",
                    Type = TypeClaimAmount,
                },
                new Column
                {
                    Header = "Whitelist",
                    Property = "IsWhitelist",
                    Type = TypeIsWhitelist,
                },
                new Column
                {
                    Header = "Batch ID",
                    Property = "BatchId",
                    Type = TypeBatchId,
                },
                new Column
                {
                    Header = "Previous Decison",
                    Property = "PreviousDecision",
                    Type = TypePreviousDecision,
                },
            };
        }

        public static string GetPreviousDecisionName(int? previousDecision)
        {
            switch(previousDecision)
            {
                case PreviousDecisionWhitelist:
                    return "Whitelist";
                case PreviousDecisionExactMatch:
                    return "Exact Match";
                default:
                    return "";
            }
        }

        public string GetPreviousDecisionStr()
        {
            PreviousDecisionStr = GetPreviousDecisionName(PreviousDecision);
            if (!string.IsNullOrEmpty(PreviousDecisionRemark))
                PreviousDecisionStr = string.Format("{0} ({1})", PreviousDecisionStr, PreviousDecisionRemark);

            return PreviousDecisionStr;
        }
    }
}
