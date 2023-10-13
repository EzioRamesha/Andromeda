using BusinessObject.Identity;
using Shared.ProcessFile;
using Shared.Trails.Attributes;
using System;
using System.Collections.Generic;

namespace BusinessObject.TreatyPricing
{
    public class TreatyPricingGroupReferralVersionBo
    {
        public int Id { get; set; }

        public int TreatyPricingGroupReferralId { get; set; }
        public TreatyPricingGroupReferralBo TreatyPricingGroupReferralBo { get; set; }

        public int Version { get; set; }
        public string VersionStr { get; set; }
        public int? GroupReferralPersonInChargeId { get; set; }
        public virtual UserBo GroupReferralPersonInChargeBo { get; set; }

        public int? GroupReferralVersionBenefitId { get; set; }
        public TreatyPricingGroupReferralVersionBenefitBo TreatyPricingGroupReferralVersionBenefitBo { get; set; }
        public IList<TreatyPricingGroupReferralVersionBenefitBo> TreatyPricingGroupReferralVersionBenefitBos { get; set; }

        public string CedantPersonInCharge { get; set; }

        public int? RequestTypePickListDetailId { get; set; }
        public virtual PickListDetailBo RequestTypePickListDetailBo { get; set; }

        public int? PremiumTypePickListDetailId { get; set; }
        public virtual PickListDetailBo PremiumTypePickListDetailBo { get; set; }

        public double? GrossRiskPremium { get; set; }
        public string GrossRiskPremiumStr { get; set; }

        public double? ReinsurancePremium { get; set; }
        public string ReinsurancePremiumStr { get; set; }

        public double? GrossRiskPremiumGTL { get; set; }
        public string GrossRiskPremiumGTLStr { get; set; }

        public double? ReinsurancePremiumGTL { get; set; }
        public string ReinsurancePremiumGTLStr { get; set; }

        public double? GrossRiskPremiumGHS { get; set; }
        public string GrossRiskPremiumGHSStr { get; set; }

        public double? ReinsurancePremiumGHS { get; set; }
        public string ReinsurancePremiumGHSStr { get; set; }

        public double? AverageSumAssured { get; set; }
        public string AverageSumAssuredStr { get; set; }

        public double? GroupSize { get; set; }
        public string GroupSizeStr { get; set; }

        public int IsCompulsoryOrVoluntary { get; set; }
        public string CompulsoryOrVoluntary { get; set; }

        public string UnderwritingMethod { get; set; }

        public string Remarks { get; set; }

        public DateTime? RequestReceivedDate { get; set; }
        public string RequestReceivedDateStr { get; set; }

        public DateTime? EnquiryToClientDate { get; set; }
        public string EnquiryToClientDateStr { get; set; }

        public DateTime? ClientReplyDate { get; set; }
        public string ClientReplyDateStr { get; set; }

        public DateTime? QuotationSentDate { get; set; }
        public string QuotationSentDateStr { get; set; }

        public int? Score { get; set; }

        public bool HasPerLifeRetro { get; set; }

        public string ChecklistRemark { get; set; }

        public bool ChecklistPendingUnderwriting { get; set; }

        public bool ChecklistPendingHealth { get; set; }

        public bool ChecklistPendingClaims { get; set; }

        public bool ChecklistPendingBD { get; set; }

        public bool ChecklistPendingCR { get; set; }

        public int? QuotationTAT { get; set; }

        public int? InternalTAT { get; set; }

        public DateTime? QuotationValidityDate { get; set; }
        public string QuotationValidityDateStr { get; set; }

        public string QuotationValidityDay { get; set; }

        public int? FirstQuotationSentWeek { get; set; }

        public int? FirstQuotationSentMonth { get; set; }

        public string FirstQuotationSentQuarter { get; set; }

        public int? FirstQuotationSentYear { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        [IsJsonProperty("BenefitId")]
        public string TreatyPricingGroupReferralVersionBenefit { get; set; }


        // Get Group Referral for download
        public string GroupReferralCode { get; set; }
        public string GroupReferralDescription { get; set; }
        public int CedantId { get; set; }
        public int GroupReferralStatus { get; set; }
        public int? InsuredGroupNameId { get; set; }
        public int? IndustryNamePickListDetailId { get; set; }
        public int? ReferredTypePickListDetailId { get; set; }
        public virtual PickListDetailBo ReferredTypePickListDetailBo { get; set; }
        public string PolicyNumber { get; set; }
        public DateTime? FirstReferralDate { get; set; }
        public DateTime? CoverageStartDate { get; set; }
        public DateTime? CoverageEndDate { get; set; }
        public bool HasRiGroupSlip { get; set; }
        public string WonVersion { get; set; }
        public int? RiGroupSlipPersonInChargeId { get; set; }
        public DateTime? RiGroupSlipConfirmationDate { get; set; }
        public int? RiGroupSlipStatus { get; set; }
        public string RiGroupSlipCode { get; set; }
        public double? AverageScore { get; set; }
        public int? TreatyPricingGroupMasterLetterId { get; set; }
        public int? WorkflowStatusId { get; set; }
        public string CommissionMarginDEA { get; set; }
        public string CommissionMarginMSE { get; set; }
        public string ExpenseMarginDEA { get; set; }
        public string ExpenseMarginMSE { get; set; }
        public string ProfitMarginDEA { get; set; }
        public string ProfitMarginMSE { get; set; }

        public const int IsCompulsory = 1;
        public const int IsVoluntary = 2;
        public const int IsCompulsoryOrVoluntaryMax = 2;

        public const int ColumnCedantCode = 1;
        public const int ColumnInsuredGroupName = 2;
        public const int ColumnGRCode = 3;
        public const int ColumnGRDescription = 4;
        public const int ColumnReferredType = 5;
        public const int ColumnCoverageStartDate = 6;
        public const int ColumnCedantPIC = 7;
        public const int ColumnCommissionMarginDEA = 8;
        public const int ColumnCommissionMarginMSE = 9;
        public const int ColumnExpenseMarginDEA = 10;
        public const int ColumnExpenseMarginMSE = 11;
        public const int ColumnProfitMarginDEA = 12;
        public const int ColumnProfitMarginMSE = 13;
        public const int ColumnFirstReferralDate = 14;
        public const int ColumnPolicyNumber = 15;
        public const int ColumnStatus = 16;
        public const int ColumnWonVersion = 17;

        public TreatyPricingGroupReferralVersionBo() { }

        public TreatyPricingGroupReferralVersionBo(TreatyPricingGroupReferralVersionBo bo)
        {
            TreatyPricingGroupReferralId = bo.TreatyPricingGroupReferralId;
            Version = bo.Version;
            GroupReferralPersonInChargeId = bo.GroupReferralPersonInChargeId;
            CedantPersonInCharge = bo.CedantPersonInCharge;
            RequestTypePickListDetailId = bo.RequestTypePickListDetailId;
            PremiumTypePickListDetailId = bo.PremiumTypePickListDetailId;
            GrossRiskPremium = bo.GrossRiskPremium;
            ReinsurancePremium = bo.ReinsurancePremium;
            GrossRiskPremiumGTL = bo.GrossRiskPremiumGTL;
            ReinsurancePremiumGTL = bo.ReinsurancePremiumGTL;
            GrossRiskPremiumGHS = bo.GrossRiskPremiumGHS;
            ReinsurancePremiumGHS = bo.ReinsurancePremiumGHS;
            AverageSumAssured = bo.AverageSumAssured;
            GroupSize = bo.GroupSize;
            IsCompulsoryOrVoluntary = bo.IsCompulsoryOrVoluntary;
            UnderwritingMethod = bo.UnderwritingMethod;
            Remarks = bo.Remarks;
            RequestReceivedDate = bo.RequestReceivedDate;
            EnquiryToClientDate = bo.EnquiryToClientDate;
            ClientReplyDate = bo.ClientReplyDate;
            QuotationSentDate = bo.QuotationSentDate;
            Score = bo.Score;
            HasPerLifeRetro = bo.HasPerLifeRetro;
            QuotationTAT = bo.QuotationTAT;
            InternalTAT = bo.InternalTAT;
            QuotationValidityDate = bo.QuotationValidityDate;
            QuotationValidityDay = bo.QuotationValidityDay;
            FirstQuotationSentWeek = bo.FirstQuotationSentWeek;
            FirstQuotationSentMonth = bo.FirstQuotationSentMonth;
            FirstQuotationSentQuarter = bo.FirstQuotationSentQuarter;
            FirstQuotationSentYear = bo.FirstQuotationSentYear;

            RequestReceivedDateStr = bo.RequestReceivedDateStr;
            EnquiryToClientDateStr = bo.EnquiryToClientDateStr;
            ClientReplyDateStr = bo.ClientReplyDateStr;
            QuotationSentDateStr = bo.QuotationSentDateStr;
            GrossRiskPremiumStr = bo.GrossRiskPremiumStr;
            ReinsurancePremiumStr = bo.ReinsurancePremiumStr;
            GrossRiskPremiumGTLStr = bo.GrossRiskPremiumGTLStr;
            ReinsurancePremiumGTLStr = bo.ReinsurancePremiumGTLStr;
            GrossRiskPremiumGHSStr = bo.GrossRiskPremiumGHSStr;
            ReinsurancePremiumGHSStr = bo.ReinsurancePremiumGHSStr;
            AverageSumAssuredStr = bo.AverageSumAssuredStr;
            GroupSizeStr = bo.GroupSizeStr;
            QuotationValidityDateStr = bo.QuotationValidityDateStr;
        }

        public static string GetCompulsoryOrVoluntaryName(int key)
        {
            switch (key)
            {
                case IsCompulsory:
                    return "Compulsory";
                case IsVoluntary:
                    return "Voluntary";
                default:
                    return "";
            }
        }

        public static List<Column> GetColumns()
        {
            var columns = new List<Column>
            {
                new Column
                {
                    Header = "Ceding Company",
                    Property = "CedantId",
                },
                new Column
                {
                    Header = "Insured Group Name",
                    Property = "InsuredGroupNameId",
                },
                new Column
                {
                    Header = "Industry Name",
                    Property = "IndustryNamePickListDetailId",
                },
                new Column
                {
                    Header = "Referred Type",
                    Property = "ReferredTypePickListDetailId",
                },
                new Column
                {
                    Header = "Type of Request",
                    Property = "RequestTypePickListDetailId",
                },
                new Column
                {
                    Header = "Expected Group Size",
                    Property = "GroupSize",
                },
                new Column
                {
                    Header = "Expected Annual Gross/Risk Premium",
                    Property = "GrossRiskPremium",
                },
                new Column
                {
                    Header = "Expected Annual Reinsurance Premium",
                    Property = "ReinsurancePremium",
                },
                new Column
                {
                    Header = "Group Referral ID",
                    Property = "GroupReferralCode",
                },
                new Column
                {
                    Header = "Group Referral Description",
                    Property = "GroupReferralDescription",
                },
                new Column
                {
                    Header = "Quotation Latest Version",
                    Property = "Version",
                },
                new Column
                {
                    Header = "Group Referral PIC",
                    Property = "GroupReferralPersonInChargeId",
                },
                new Column
                {
                    Header = "RI Group Slip ID",
                    Property = "RiGroupSlipCode",
                },
                new Column
                {
                    Header = "RI Group Slip PIC",
                    Property = "RiGroupSlipPersonInChargeId",
                },
                new Column
                {
                    Header = "RI Group Slip Status",
                    Property = "RiGroupSlipStatus",
                },
                new Column
                {
                    Header = "RI Group Slip COnfirmation Date",
                    Property = "RiGroupSlipConfirmationDate",
                },
                new Column
                {
                    Header = "Group Master Letter ID",
                    Property = "TreatyPricingGroupMasterLetterId",
                },
                new Column
                {
                    Header = "First Referral Date",
                    Property = "FirstReferralDate",
                },
                new Column
                {
                    Header = "Coverage Start Date",
                    Property = "CoverageStartDate",
                },
                new Column
                {
                    Header = "Coverage End Date",
                    Property = "CoverageEndDate",
                },
                new Column
                {
                    Header = "Current Quotation TAT",
                    Property = "QuotationTAT",
                },
                new Column
                {
                    Header = "Current Internal TAT",
                    Property = "InternalTAT",
                },
                 new Column
                {
                    Header = "Score",
                    Property = "AverageScore",
                },
                new Column
                {
                    Header = "Status",
                    Property = "GroupReferralStatus",
                },
                new Column
                {
                    Header = "Workflow Status",
                    Property = "WorkflowStatusId",
                },
                new Column
                {
                    Header = "Won Version",
                    Property = "WonVersion",
                },
                new Column
                {
                    Header = "Checklist Pending Underwriting",
                    Property = "ChecklistPendingUnderwriting",
                },
                new Column
                {
                    Header = "Checklist Pending Health",
                    Property = "ChecklistPendingHealth",
                },
                new Column
                {
                    Header = "Checklist Pending Claim",
                    Property = "ChecklistPendingClaims",
                },
                new Column
                {
                    Header = "Checklist Pending BD",
                    Property = "ChecklistPendingBD",
                },
                new Column
                {
                    Header = "Checklist Pending C&R",
                    Property = "ChecklistPendingCR",
                },
            };
            return columns;
        }

        public static List<Column> GetTrackingColumns()
        {
            var columns = new List<Column>
            {
                new Column
                {
                    Header = "Ceding Company",
                    ColIndex = ColumnCedantCode,
                    Property = "CedantId",
                },
                new Column
                {
                    Header = "Insured Group Name",
                    ColIndex = ColumnInsuredGroupName,
                    Property = "InsuredGroupNameId",
                },
                new Column
                {
                    Header = "Group Referral ID",
                    ColIndex = ColumnGRCode,
                    Property = "GroupReferralCode",
                },
                new Column
                {
                    Header = "Group Referral Description",
                    ColIndex = ColumnGRDescription,
                    Property = "GroupReferralDescription",
                },
                new Column
                {
                    Header = "Referred Type",
                    ColIndex = ColumnReferredType,
                    Property = "ReferredTypePickListDetailId",
                },
                new Column
                {
                    Header = "Coverage Start Date",
                    ColIndex = ColumnCoverageStartDate,
                    Property = "CoverageStartDate",
                },
                new Column
                {
                    Header = "Cedant's Person In-Charge",
                    ColIndex = ColumnCedantPIC,
                    Property = "CedantPersonInCharge",
                },
                new Column
                {
                    Header = "Commission Margin (DEA / DEA_N)",
                    ColIndex = ColumnCommissionMarginDEA,
                    Property = "CommissionMarginDEA",
                },
                new Column
                {
                    Header = "Commission Margin (MSE)",
                    ColIndex = ColumnCommissionMarginMSE,
                    Property = "CommissionMarginMSE",
                },
                new Column
                {
                    Header = "Expense Margin (DEA / DEA_N)",
                    ColIndex = ColumnExpenseMarginDEA,
                    Property = "ExpenseMarginDEA",
                },
                new Column
                {
                    Header = "Expense Margin (MSE)",
                    ColIndex = ColumnExpenseMarginMSE,
                    Property = "ExpenseMarginMSE",
                },
                new Column
                {
                    Header = "Profit Margin (DEA / DEA_N)",
                    ColIndex = ColumnProfitMarginDEA,
                    Property = "ProfitMarginDEA",
                },
                new Column
                {
                    Header = "Profit Margin (MSE)",
                    ColIndex = ColumnProfitMarginMSE,
                    Property = "ProfitMarginMSE",
                },
                new Column
                {
                    Header = "First Referral Date",
                    ColIndex = ColumnFirstReferralDate,
                    Property = "FirstReferralDate",
                },
                new Column
                {
                    Header = "Policy Number",
                    ColIndex = ColumnPolicyNumber,
                    Property = "PolicyNumber",
                },
                new Column
                {
                    Header = "Status (Won/Loss)",
                    ColIndex = ColumnStatus,
                    Property = "GroupReferralStatus",
                },
                new Column
                {
                    Header = "Won Version",
                    ColIndex = ColumnWonVersion,
                    Property = "WonVersion",
                },
            };
            return columns;
        }
    }
}
