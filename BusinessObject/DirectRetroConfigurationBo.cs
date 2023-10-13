using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class DirectRetroConfigurationBo
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int TreatyCodeId { get; set; }

        public TreatyCodeBo TreatyCodeBo { get; set; }

        public string RetroParty { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? UpdatedById { get; set; }
    }
}
