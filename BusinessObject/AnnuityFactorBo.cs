using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class AnnuityFactorBo
    {
        public int Id { get; set; }

        public int CedantId { get; set; }

        public virtual CedantBo CedantBo { get; set; }

        public string CedantCode { get; set; }

        public string CedingPlanCode { get; set; }

        public DateTime? ReinsEffDatePolStartDate { get; set; }

        public DateTime? ReinsEffDatePolEndDate { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? UpdatedById { get; set; }
    }
}
