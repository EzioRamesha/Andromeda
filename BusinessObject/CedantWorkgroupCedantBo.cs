using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class CedantWorkgroupCedantBo
    {
        public int CedantWorkgroupId { get; set; }

        public int CedantId { get; set; }

        public virtual CedantWorkgroupBo CedantWorkgroupBo { get; set; }

        public virtual CedantBo CedantBo { get; set; }

        public string CedantName { get; set; }

        public string PrimaryKey()
        {
            return string.Format("{0}|{1}", CedantWorkgroupId, CedantId);
        }
    }
}
