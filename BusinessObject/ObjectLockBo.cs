using BusinessObject.Identity;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class ObjectLockBo
    {
        public int Id { get; set; }

        public int ModuleId { get; set; }

        public int ObjectId { get; set; }

        public int LockedById { get; set; }
        
        public virtual UserBo LockedByBo { get; set; }

        public DateTime ExpiresAt { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        public DateTime CreatedAt { get; set; }

        public void RefreshExpiry(int authUserId)
        {
            int sessionLength = Util.GetConfigInteger("SessionLength");
            ExpiresAt = DateTime.Now.AddMinutes(sessionLength);
            UpdatedById = authUserId;
        }
    }
}
