using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class AnnuityFactorMappingBo
    {
        public int Id { get; set; }

        public int AnnuityFactorId { get; set; }

        public virtual AnnuityFactorBo AnnuityFactorBo { get; set; }

        public string Combination { get; set; }

        public string CedingPlanCode { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreatedAt { get; set; }

        public AnnuityFactorMappingBo()
        {
            CreatedAt = DateTime.Now;
        }
    }
}
