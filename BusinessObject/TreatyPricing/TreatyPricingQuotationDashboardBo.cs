using System.Collections.Generic;

namespace BusinessObject.TreatyPricing
{
    public class TreatyPricingQuotationDashboardBo
    {
        public int? UserId { get; set; }

        public string UserName { get; set; }

        public int? DepartmentId { get; set; }

        public string DepartmentName { get; set; }

        public int? CedantId { get; set; }

        public string CedantName { get; set; }

        public int? ReinsuranceTypePickListDetailId { get; set; }

        public string ReinsuranceType { get; set; }

        public List<TreatyPricingQuotationWorkflowBo> TreatyPricingQuotationWorkflowBos { get; set; }

        public int TotalCase { get; set; }

        public int? NoOfQuotedCaseExceed14 { get; set; }

        public int? NoOfCaseDueDateBelow5 { get; set; }

        public int? NoOfCaseDueDate6To10 { get; set; }

        public int? NoOfCaseDueDateExceed10 { get; set; }

        public int? ExpectedRIPremiumReceivable { get; set; }

        public int? Status { get; set; }

        public List<int> CaseCountByDepartmentStatus { get; set; }
    }
}
