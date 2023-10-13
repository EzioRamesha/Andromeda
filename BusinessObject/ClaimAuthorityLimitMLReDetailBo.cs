using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class ClaimAuthorityLimitMLReDetailBo
    {
        public int Id { get; set; }

        public int ClaimAuthorityLimitMLReId { get; set; }

        public ClaimAuthorityLimitMLReBo ClaimAuthorityLimitMLReBo { get; set; }

        public int ClaimCodeId { get; set; }

        public ClaimCodeBo ClaimCodeBo { get; set; }

        public double ClaimAuthorityLimitValue { get; set; }

        public DateTime CreatedAt { get; set; }

        public int CreatedById { get; set; }

        public string ClaimAuthorityLimitValueStr { get; set; }

        public ClaimAuthorityLimitMLReDetailBo()
        {
            CreatedAt = DateTime.Now;
        }

        public List<string> Validate()
        {
            List<string> errors = new List<string> { };

            if (ClaimCodeId == 0)
                errors.Add(string.Format(MessageBag.Required, "Claim Code"));

            if (string.IsNullOrEmpty(ClaimAuthorityLimitValueStr) || string.IsNullOrWhiteSpace(ClaimAuthorityLimitValueStr))
                errors.Add(string.Format(MessageBag.Required, "Amount"));
            else if (!Util.IsValidDouble(ClaimAuthorityLimitValueStr, out double? d, out _))
                errors.Add(string.Format("Invalid Amount Input: {0}", ClaimAuthorityLimitValueStr));

            return errors;
        }
    }
}
