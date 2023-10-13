using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class EventClaimCodeMappingDetailBo
    {
        public int Id { get; set; }

        public int EventClaimCodeMappingId { get; set; }

        public virtual EventClaimCodeMappingBo EventClaimCodeMappingBo { get; set; }

        public string Combination { get; set; }

        public DateTime CreatedAt { get; set; }

        public int CreatedById { get; set; }

        public string CedingEventCode { get; set; }

        public string CedingClaimType { get; set; }

        public EventClaimCodeMappingDetailBo()
        {
            CreatedAt = DateTime.Now;
        }
    }
}
