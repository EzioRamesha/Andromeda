using BusinessObject.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class ClaimAuthorityLimitMLReBo
    {
        public int Id { get; set; }

        public int DepartmentId { get; set; }

        public DepartmentBo DepartmentBo { get; set; }

        public int UserId { get; set; }

        public UserBo UserBo { get; set; }

        public string Position { get; set; }

        public bool IsAllowOverwriteApproval { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }
    }
}
