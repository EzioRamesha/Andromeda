using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Sanctions
{
    public class SanctionBlacklistBo
    {
        public int Id { get; set; }

        public string PolicyNumber { get; set; }

        public string InsuredName { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }
    }
}
