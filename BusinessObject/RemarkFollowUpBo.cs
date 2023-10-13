using BusinessObject.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class RemarkFollowUpBo
    {
        public int Id { get; set; }

        public int RemarkId { get; set; }

        public RemarkBo RemarkBo { get; set; }

        public int Status { get; set; }

        public string StatusName { get; set; }

        public DateTime? FollowUpAt { get; set; }

        public string FollowUpAtStr { get; set; }

        public int FollowUpUserId { get; set; }

        public UserBo FollowUpUserBo { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        public const int StatusPending = 1;
        public const int StatusCompleted = 2;
        public const int StatusMax = 2;

        public static string GetStatusName(int key)
        {
            switch (key)
            {
                case StatusPending:
                    return "Pending";
                case StatusCompleted:
                    return "Complete";
                default:
                    return "";
            }
        }

        public static string GetStatusClass(int key)
        {
            switch (key)
            {
                case StatusPending:
                    return "status-pending-badge";
                case StatusCompleted:
                    return "status-success-badge";
                default:
                    return "";
            }
        }
    }
}
