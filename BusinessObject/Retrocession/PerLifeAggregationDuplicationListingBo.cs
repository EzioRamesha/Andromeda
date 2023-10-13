using System;

namespace BusinessObject.Retrocession
{
    public class PerLifeAggregationDuplicationListingBo
    {
        public int Id { get; set; }
        public string TreatyCodeStr { get; set; }
        public int? TreatyCodeId { get; set; }
        public TreatyCodeBo TreatyCodeBo { get; set; }

        public string CedantPlanCode { get; set; }

        public string InsuredName { get; set; }

        public DateTime? InsuredDateOfBirth { get; set; }
        public string InsuredDateOfBirthStr { get; set; }

        public string PolicyNumber { get; set; }

        public string InsuredGenderCode { get; set; }
        public int? InsuredGenderCodePickListDetailId { get; set; }
        public PickListDetailBo InsuredGenderCodePickListDetailBo { get; set; }

        public string MLReBenefitCode { get; set; }
        public string MLReBenefitCodeStr { get; set; }
        public int? MLReBenefitCodeId { get; set; }
        public BenefitBo MLReBenefitCodeBo { get; set; }

        public string CedingBenefitRiskCode { get; set; }

        public string CedingBenefitTypeCode { get; set; }

        public DateTime? ReinsuranceEffectiveDate { get; set; }
        public string ReinsuranceEffecitveDateStr { get; set; }

        public string FundsAccountingType { get; set; }
        public int? FundsAccountingTypePickListDetailId { get; set; }
        public PickListDetailBo FundsAccountingTypePickListDetailBo { get; set; }

        public string ReinsBasisCode { get; set; }
        public int? ReinsBasisCodePickListDetailId { get; set; }
        public PickListDetailBo ReinsBasisCodePickListDetailBo { get; set; }

        public string TransactionType { get; set; }
        public int? TransactionTypePickListDetailId { get; set; }
        public PickListDetailBo TransactionTypePickListDetailBo { get; set; }

        public int? ProceedToAggregate { get; set; }

        public DateTime? DateUpdated { get; set; }
        public string DateUpdatedStr { get; set; }

        public string ExceptionStatus { get; set; }
        public int? ExceptionStatusPickListDetailId { get; set; }
        public PickListDetailBo ExceptionStatusPickListDetailBo { get; set; }

        public string Remarks { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? UpdatedById { get; set; }

        public const int ProceedToAggregateY = 1;
        public const int ProceedToAggregateN = 2;

        public string GetProceedToAggregateName(int key)
        {
            switch (key)
            {
                case ProceedToAggregateY:
                    return "Y";
                case ProceedToAggregateN:
                    return "N";
                default:
                    return "";
            }
        }
    }
}
