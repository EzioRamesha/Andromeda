using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class BenefitDetailBo
    {
        public int Id { get; set; }

        public int BenefitId { get; set; }

        public BenefitBo BenefitBo { get; set; }

        public int EventCodeId { get; set; }

        public EventCodeBo EventCodeBo { get; set; }

        public int ClaimCodeId { get; set; }

        public ClaimCodeBo ClaimCodeBo { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        public List<string> Validate()
        {
            List<string> errors = new List<string> { };

            if (EventCodeId == 0)
                errors.Add(string.Format(MessageBag.Required, "MLRe Event Code"));

            if (ClaimCodeId == 0)
                errors.Add(string.Format(MessageBag.Required, "Claim Code"));

            return errors;
        }
    }
}
