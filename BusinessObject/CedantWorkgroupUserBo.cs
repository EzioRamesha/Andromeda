using BusinessObject.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class CedantWorkgroupUserBo
    {
        public int CedantWorkgroupId { get; set; }

        public int UserId { get; set; }

        public virtual CedantWorkgroupBo CedantWorkgroupBo { get; set; }

        public virtual UserBo UserBo { get; set; }

        public string UserName { get; set; }

        public string PrimaryKey()
        {
            return string.Format("{0}|{1}", CedantWorkgroupId, UserId);
        }
    }
}
