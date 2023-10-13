using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class DirectRetroConfigurationMappingBo
    {
        public int Id { get; set; }

        public int DirectRetroConfigurationId { get; set; }

        public DirectRetroConfigurationBo DirectRetroConfigurationBo { get; set; }

        public string Combination { get; set; }

        public string RetroParty { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreatedAt { get; set; }

        public DirectRetroConfigurationMappingBo()
        {
            CreatedAt = DateTime.Now;
        }
    }
}
