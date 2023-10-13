using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.TreatyPricing
{
    public class TreatyPricingPickListDetailBo
    {
        public int Id { get; set; }

        public int PickListId { get; set; }

        public virtual PickListBo PickListBo { get; set; }

        // Constant in TreatyPricingCedantBo
        public int ObjectType { get; set; }

        public int ObjectId { get; set; }

        public string PickListDetailCode { get; set; }

        public int CreatedById { get; set; }
    }
}
