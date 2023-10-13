using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class DiscountTableBo
    {
        public int Id { get; set; }

        public int CedantId { get; set; }

        public CedantBo CedantBo { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? UpdatedById { get; set; }

        public IList<RiDiscountBo> RiDiscountBos { get; set; }

        public IList<LargeDiscountBo> LargeDiscountBos { get; set; }

        public IList<GroupDiscountBo> GroupDiscountBos { get; set; }
    }
}
