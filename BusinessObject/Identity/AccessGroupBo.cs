namespace BusinessObject.Identity
{
    public class AccessGroupBo
    {
        public int Id { get; set; }

        public int DepartmentId { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        public const int RequestTypeNew = 1;
        public const int RequestTypeAmendment = 2;

        public const int MaxRequestType = 2;

        public static string GetRequestTypeName(int key)
        {
            switch (key)
            {
                case RequestTypeNew:
                    return "Create New";
                case RequestTypeAmendment:
                    return "Amendments";
                default:
                    return "";
            }
        }
    }
}
