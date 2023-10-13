using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class EventClaimCodeMappingBo
    {
        public int Id { get; set; }

        public int CedantId { get; set; }

        public string CedantCode { get; set; }

        public int EventCodeId { get; set; }

        public string MLReEventCode { get; set; }

        public string CedingEventCode { get; set; }

        public string CedingClaimType { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        public CedantBo CedantBo { get; set; }

        public EventCodeBo EventCodeBo { get; set; }
    }
}
