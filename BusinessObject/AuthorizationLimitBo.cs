using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class AuthorizationLimitBo
    {
        public int Id { get; set; }

        public int AccessGroupId { get; set; }

        public double? PositiveAmountFrom { get; set; }

        public double? PositiveAmountTo { get; set; }

        public double? NegativeAmountFrom { get; set; }

        public double? NegativeAmountTo { get; set; }

        public double? Percentage { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }
    }
}
