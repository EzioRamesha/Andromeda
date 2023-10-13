namespace BusinessObject.Identity
{
    public class UserPasswordBo
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string PasswordHash { get; set; }

        public int CreatedById { get; set; }

        public virtual UserBo UserBo { get; set; }
    }
}
