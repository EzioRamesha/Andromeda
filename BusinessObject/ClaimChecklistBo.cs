using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class ClaimChecklistBo
    {
        public int Id { get; set; }

        public int ClaimCodeId { get; set; }

        public ClaimCodeBo ClaimCodeBo { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? UpdatedById { get; set; }
    }
}
