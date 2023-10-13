namespace BusinessObject
{
    public class ItemCodeBo
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public int ReportingType { get; set; }

        public string Description { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        public int? BusinessOriginPickListDetailId { get; set; }

        public PickListDetailBo BusinessOriginPickListDetailBo { get; set; }

        public const int ReportingTypeIFRS17 = 1;
        public const int ReportingTypeIFRS4 = 2;

        public const int ReportingTypeMax = 2;

        public static string GetReportingTypeName(int key)
        {
            switch (key)
            {
                case ReportingTypeIFRS17:
                    return "IFRS17";
                case ReportingTypeIFRS4:
                    return "IFRS4";
                default:
                    return "";
            }
        }

        public static int GetReportingType(string name)
        {
            switch (name)
            {
                case "IFRS17":
                    return ReportingTypeIFRS17;
                case "IFRS4":
                    return ReportingTypeIFRS4;
                default:
                    return 0;
            }
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(Description))
            {
                return string.Format("{0} - {1} - {2}", GetReportingTypeName(ReportingType), BusinessOriginPickListDetailBo?.Code, Code);
            }
            return string.Format("{0} - {1} - {2} - {3}", GetReportingTypeName(ReportingType), BusinessOriginPickListDetailBo?.Code, Code, Description);
        }
    }
}
