using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class ClaimCodeMappingDetailBo
    {
        public int Id { get; set; }

        public int ClaimCodeMappingId { get; set; }

        public virtual ClaimCodeMappingBo ClaimCodeMappingBo { get; set; }

        public string Combination { get; set; }

        public string MlreEventCode { get; set; }

        public string MlreBenefitCode { get; set; }

        public DateTime CreatedAt { get; set; }

        public int CreatedById { get; set; }

        public ClaimCodeMappingDetailBo()
        {
            CreatedAt = DateTime.Now;
        }
    }
}
