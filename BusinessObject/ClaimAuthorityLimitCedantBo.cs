using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class ClaimAuthorityLimitCedantBo
    {
        public int Id { get; set; }

        public int CedantId { get; set; }

        public virtual CedantBo CedantBo { get; set; }

        public string Remarks { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }
    }
}
