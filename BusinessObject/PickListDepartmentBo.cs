using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Identity;

namespace BusinessObject
{
    public class PickListDepartmentBo
    {
        public int Id { get; set; }

        public int PickListId { get; set; }
        public PickListBo PickListBo { get; set; }

        public int DepartmentId { get; set; }
        public DepartmentBo DepartmentBo { get; set; }

        public string UsedBy { get; set; }

        public int CreatedById { get; set; }
        public virtual UserBo CreatedByBo { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? UpdatedById { get; set; }

        public string CreatedAtStr { get; set; }

        public string CreatedByStr { get; set; }

        public string PrimaryKey()
        {
            return string.Format("{0}{1}", PickListId, DepartmentId);
        }
    }
}
