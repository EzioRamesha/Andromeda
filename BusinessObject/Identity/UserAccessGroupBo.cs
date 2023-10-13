namespace BusinessObject.Identity
{
    public class UserAccessGroupBo
    {
        public int UserId { get; set; }

        public int AccessGroupId { get; set; }

        public virtual UserBo UserBo { get; set; }

        public virtual AccessGroupBo AccessGroupBo { get; set; }
    }
}
