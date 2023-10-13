using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class RateTableDetailBo
    {
        public int Id { get; set; }

        public int RateTableId { get; set; }

        public virtual RateTableBo RateTableBo { get; set; }

        public string Combination { get; set; }

        public DateTime CreatedAt { get; set; }

        public int CreatedById { get; set; }

        public string TreatyCode { get; set; }

        public string CedingPlanCode { get; set; }

        public string CedingTreatyCode { get; set; }

        public string CedingPlanCode2 { get; set; }

        public string CedingBenefitTypeCode { get; set; }

        public string CedingBenefitRiskCode { get; set; }

        public string GroupPolicyNumber { get; set; }

        public RateTableDetailBo()
        {
            CreatedAt = DateTime.Now;
        }
    }
}
