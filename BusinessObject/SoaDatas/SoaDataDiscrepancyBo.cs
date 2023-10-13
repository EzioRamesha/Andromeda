namespace BusinessObject.SoaDatas
{
    public class SoaDataDiscrepancyBo
    {
        public int Id { get; set; }

        public int SoaDataBatchId { get; set; }
        public virtual SoaDataBatchBo SoaDataBatchBo { get; set; }

        public int Type { get; set; }

        public string TreatyCode { get; set; }
        public string CedingPlanCode { get; set; }

        public string CurrencyCode { get; set; }
        public double? CurrencyRate { get; set; }

        public double? CedantAmount { get; set; } = 0;
        public double? MlreChecking { get; set; } = 0;
        public double? Discrepancy { get; set; } = 0;

        public int CreatedById { get; set; }
        public int? UpdatedById { get; set; }


        public const int TypeMlreShare = 1;
        public const int TypeLayer1Share = 2;
        public const int TypeRetakaful = 3;

        public static string GetTypeName(int key)
        {
            switch (key)
            {
                case TypeMlreShare:
                    return "MLRe's Share Difference";
                case TypeLayer1Share:
                    return "Layer1's Share Difference";
                case TypeRetakaful:
                    return "Retakaful Difference";
                default:
                    return "";
            }
        }

        public double? GetDiscrepancy()
        {
            Discrepancy = CedantAmount.GetValueOrDefault() - MlreChecking.GetValueOrDefault();
            return Discrepancy;
        }
    }
}
