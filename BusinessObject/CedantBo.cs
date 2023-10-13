namespace BusinessObject
{
    public class CedantBo
    {
        public int Id { get; set; }

        public int? CedingCompanyTypePickListDetailId { get; set; }

        public virtual PickListDetailBo CedingCompanyTypePickListDetailBo { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }
        
        public string PartyCode { get; set; }

        public int Status { get; set; }

        public string Remarks { get; set; }

        public string AccountCode { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        public const int TypeInsurer = 1;
        public const int TypeTakafulOperator = 2;
        public const int TypeReinsurance = 3;
        public const int TypeRetakafulOperator = 4;

        public const int StatusActive = 1;
        public const int StatusInactive = 2;

        public static string GetTypeName(int key)
        {
            switch (key)
            {
                case TypeInsurer:
                    return "Insurer";
                case TypeTakafulOperator:
                    return "Takaful Operator";
                case TypeReinsurance:
                    return "Reinsurance";
                case TypeRetakafulOperator:
                    return "Retakaful Operator";
                default:
                    return "";
            }
        }

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

        public override string ToString()
        {
            if (string.IsNullOrEmpty(Name))
            {
                return Code;
            }
            return string.Format("{0} - {1}", Code, Name);
        }
    }
}
