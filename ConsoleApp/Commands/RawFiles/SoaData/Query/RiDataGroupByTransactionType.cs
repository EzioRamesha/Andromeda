namespace ConsoleApp.Commands.RawFiles.SoaData.Query
{
    public class RiDataGroupByTransactionType : RiDataGroupBy
    {
        public double? StandardPremium { get; set; }
        public double? SubstandardPremium { get; set; }
        public double? FlatExtraPremium { get; set; }

        public double? StandardDiscount { get; set; }
        public double? SubstandardDiscount { get; set; }

        public double? TransactionPremium { get; set; }
        public double? TransactionDiscount { get; set; }

        public double? Aar { get; set; }

        public double GetTotalPremium()
        {
            return StandardPremium.GetValueOrDefault() + SubstandardPremium.GetValueOrDefault() + FlatExtraPremium.GetValueOrDefault();
        }

        public double GetTotalDiscount()
        {
            return StandardDiscount.GetValueOrDefault() + SubstandardDiscount.GetValueOrDefault() + TotalDiscount.GetValueOrDefault();
        }
    }
}
