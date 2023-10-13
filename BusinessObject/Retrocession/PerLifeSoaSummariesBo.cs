namespace BusinessObject.Retrocession
{
    public class PerLifeSoaSummariesBo
    {
        public int Id { get; set; }

        public int PerLifeSoaId { get; set; }
        public PerLifeSoaBo PerLifeSoaBo { get; set; }

        public string RowLabel { get; set; }

        public int WMOM { get; set; }

        public double? Automatic { get; set; }

        public double? Facultative { get; set; }

        public double? Advantage { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        public double? Total { get; set; }
        public string AutomaticStr { get; set; }
        public string FacultativeStr { get; set; }
        public string AdvantageStr { get; set; }
        public string TotalStr { get; set; }


        public const int WMOMWithin = 1;
        public const int WMOMOutside = 2;
        public const int WMOMMax = 2;
    }
}
