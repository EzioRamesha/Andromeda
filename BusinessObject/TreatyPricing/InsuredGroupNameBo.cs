namespace BusinessObject.TreatyPricing
{
    public class InsuredGroupNameBo
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Status { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        public const int StatusActive = 1;
        public const int StatusInactive = 2;

        public static string GetStatusName(int key)
        {
            switch (key)
            {
                case StatusActive:
                    return "Active";
                case StatusInactive:
                    return "Inactive";
                default:
                    return "";
            }
        }

        public static string GetStatusClass(int key)
        {
            switch (key)
            {
                case StatusActive:
                    return "status-success-badge";
                case StatusInactive:
                    return "status-fail-badge";
                default:
                    return "";
            }
        }
    }
}
