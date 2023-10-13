using BusinessObject.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class ObjectPermissionBo
    {
        public int Id { get; set; }

        public int ObjectId { get; set; }

        public int Type { get; set; }

        public int DepartmentId { get; set; }

        public DepartmentBo DepartmentBo { get; set; }

        public int CreatedById { get; set; }

        public const int TypeRemark = 1;
        public const int TypeDocument = 2;
        public const int MaxType = 2;

        public static string GetPermissionName(bool isPrivate)
        {
            return isPrivate ? "Private" : "Public";
        }
    }
}
