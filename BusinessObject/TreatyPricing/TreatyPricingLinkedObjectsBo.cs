using Shared;
using System;
using System.Collections.Generic;

namespace BusinessObject.TreatyPricing
{
    public class TreatyPricingLinkedObjectsBo
    {
        public int Id { get; set; }

        public int ObjectModuleId { get; set; }

        public int ObjectId { get; set; }

        public int ObjectVersionId { get; set; }

        public string ObjectModuleName { get; set; }

        public string ObjectCode { get; set; }

        public string ObjectName { get; set; }

        public int ObjectVersion { get; set; }

        public string ObjectModuleCode { get; set; }
    }
}
