using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Sanctions
{
    public class SanctionWhitelistBo
    {
        public int Id { get; set; }

        public string PolicyNumber { get; set; }

        public string InsuredName { get; set; }

        public string Reason { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }
    }
}
