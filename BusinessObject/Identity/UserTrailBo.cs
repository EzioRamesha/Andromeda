using Shared.DataAccess;
using Shared.Trails;
using System;

namespace BusinessObject.Identity
{
    public class UserTrailBo
    {
        public int Id { get; set; }

        public int Type { get; set; }

        public string Table { get; set; }

        public string Controller { get; set; }

        public int ObjectId { get; set; }

        public string Description { get; set; }

        public string IpAddress { get; set; }

        public string Data { get; set; }

        public DateTime CreatedAt { get; set; }

        public string CreatedAtStr { get; set; }

        public int CreatedById { get; set; }

        public UserBo CreatedByBo { get; set; }

        public const int TypeTrail = 1;
        public const int TypeLogin = 2;
        public const int TypeLogout = 3;

        public UserTrailBo()
        {
            Type = TypeTrail;
        }

        public UserTrailBo(int objectId, string description, Result result, TrailObject trail, int createdById = 0, bool ignoreNull = false)
        {
            Type = TypeTrail;
            Table = result.Table;
            Controller = result.Controller;
            ObjectId = objectId;
            Description = string.IsNullOrEmpty(description) ? Table : string.IsNullOrEmpty(Table) ? description : string.Format("{0} - {1}", Table, description);
            CreatedById = createdById;

            if (ignoreNull)
                Data = trail.ToStringIgnoreNull();
            else
                Data = trail.ToString();
        }

        public static string GetTypeName(int key)
        {
            switch (key)
            {
                case TypeTrail:
                    return "Trail";
                case TypeLogin:
                    return "Login";
                case TypeLogout:
                    return "Logout";
                default:
                    return "";
            }
        }
    }
}
