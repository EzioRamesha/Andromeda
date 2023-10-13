namespace ConsoleApp.Commands.RawFiles.SoaData.Query
{
    public class RiDataPostValidationTransactionType : RiDataPostValidation
    {
        public double? StandardPremium { get; set; }
        public double? SubstandardPremium { get; set; }
        public double? FlatExtraPremium { get; set; }

        public double? StandardDiscount { get; set; }
        public double? SubstandardDiscount { get; set; }
        public double? TotalDiscount { get; set; }

        public double? MlreStandardPremium { get; set; }
        public double? MlreSubstandardPremium { get; set; }
        public double? MlreFlatExtraPremium { get; set; }
        public double? MlreGrossPremium { get; set; }

        public double? MlreStandardDiscount { get; set; }
        public double? MlreSubstandardDiscount { get; set; }
        public double? MlreTotalDiscount { get; set; }

        public double? Layer1StandardPremium { get; set; }
        public double? Layer1SubstandardPremium { get; set; }
        public double? Layer1FlatExtraPremium { get; set; }
        public double? Layer1GrossPremium { get; set; }

        public double? Layer1StandardDiscount { get; set; }
        public double? Layer1SubstandardDiscount { get; set; }
        public double? Layer1TotalDiscount { get; set; }

        public double? Aar { get; set; }
        public double? TransactionDiscount { get; set; }

        public int Total { get; set; }

        public double GetTotalPremium()
        {
            return StandardPremium.GetValueOrDefault() + SubstandardPremium.GetValueOrDefault() + FlatExtraPremium.GetValueOrDefault();
        }

        public double GetTotalMlrePremium()
        {
            return MlreStandardPremium.GetValueOrDefault() + MlreSubstandardPremium.GetValueOrDefault() + MlreFlatExtraPremium.GetValueOrDefault();
        }

        public double GetTotalLayer1Premium()
        {
            return Layer1StandardPremium.GetValueOrDefault() + Layer1SubstandardPremium.GetValueOrDefault() + Layer1FlatExtraPremium.GetValueOrDefault();
        }

        public double GetTotalDiscount()
        {
            return StandardDiscount.GetValueOrDefault() + SubstandardDiscount.GetValueOrDefault() + TotalDiscount.GetValueOrDefault();
        }

        public double GetTotalMlreDiscount()
        {
            return MlreStandardDiscount.GetValueOrDefault() + MlreSubstandardDiscount.GetValueOrDefault() + MlreTotalDiscount.GetValueOrDefault();
        }

        public double GetTotalLayer1Discount()
        {
            return Layer1StandardDiscount.GetValueOrDefault() + Layer1SubstandardDiscount.GetValueOrDefault() + Layer1TotalDiscount.GetValueOrDefault();
        }
    }
}
