namespace BusinessObject
{
    public class TreatyOldCodeBo
    {
        public int TreatyCodeId { get; set; }

        public int OldTreatyCodeId { get; set; }

        public virtual TreatyCodeBo TreatyCodeBo { get; set; }

        public virtual TreatyCodeBo OldTreatyCodeBo { get; set; }

        public string PrimaryKey()
        {
            return string.Format("{0}|{1}", TreatyCodeId, OldTreatyCodeId);
        }
    }
}
