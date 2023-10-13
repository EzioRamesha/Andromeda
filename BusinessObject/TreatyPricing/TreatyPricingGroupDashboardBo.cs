using System.Collections.Generic;

namespace BusinessObject.TreatyPricing
{
    public class TreatyPricingGroupDashboardBo
    {
        public string TurnaroundTimeTitle { get; set; }

        public int? TurnaroundTimeDay { get; set; }

        public bool? IsExceedDay { get; set; }

        public int CedantActiveCase { get; set; }

        public int InternalActiveCase { get; set; }

        public int PersonInChargeId { get; set; }

        public string PersonInChargeName { get; set; }

        public int NoOfCase { get; set; }

        public int TotalActiveCase { get; set; }

        public int TotalActiveCaseTat3 { get; set; }

        public int AverageScore { get; set; }

        public int DepartmentId { get; set; }

        public string DepartmentName { get; set; }

        public static List<TreatyPricingGroupDashboardBo> GetDashboardTurnaroundTimeList()
        {
            return new List<TreatyPricingGroupDashboardBo> {
                new TreatyPricingGroupDashboardBo
                {
                    TurnaroundTimeTitle = "Unassigned",
                    TurnaroundTimeDay = null,
                    IsExceedDay = null,
                },
                new TreatyPricingGroupDashboardBo
                {
                    TurnaroundTimeTitle = "No of Days",
                    TurnaroundTimeDay = 0,
                    IsExceedDay = false,
                },
                new TreatyPricingGroupDashboardBo
                {
                    TurnaroundTimeTitle = "No of Days",
                    TurnaroundTimeDay = 1,
                    IsExceedDay = false,
                },
                new TreatyPricingGroupDashboardBo
                {
                    TurnaroundTimeTitle = "No of Days",
                    TurnaroundTimeDay = 2,
                    IsExceedDay = false,
                },
                new TreatyPricingGroupDashboardBo
                {
                    TurnaroundTimeTitle = "No of Days",
                    TurnaroundTimeDay = 3,
                    IsExceedDay = false,
                },
                new TreatyPricingGroupDashboardBo
                {
                    TurnaroundTimeTitle = "No of Days",
                    TurnaroundTimeDay = 3,
                    IsExceedDay = true,
                },
            };
        }
    }
}
