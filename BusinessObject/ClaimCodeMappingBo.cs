using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class ClaimCodeMappingBo
    {
        public int Id { get; set; }

        public string MlreEventCode { get; set; }

        public string MlreBenefitCode { get; set; }

        public int ClaimCodeId { get; set; }

        public ClaimCodeBo ClaimCodeBo { get; set; }

        public string ClaimCode { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? UpdatedById { get; set; }
    }
}
