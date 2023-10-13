using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Commands.RawFiles.SoaData.Query
{
    public class RiSummaryGroupBy
    {
        public string TreatyCode { get; set; }
        public string RiskQuarter { get; set; }
        public double? NbPremium { get; set; }
        public double? RnPremium { get; set; }
        public double? AltPremium { get; set; }
        public double? NbDiscount { get; set; }
        public double? RnDiscount { get; set; }
        public double? AltDiscount { get; set; }
        public int? NbCession { get; set; }
        public int? RnCession { get; set; }
        public int? AltCession { get; set; }
        public double? NbSar { get; set; }
        public double? RnSar { get; set; }
        public double? AltSar { get; set; }
        public double? DTH { get; set; }
        public double? TPA { get; set; }
        public double? TPS { get; set; }
        public double? PPD { get; set; }
        public double? CCA { get; set; }
        public double? CCS { get; set; }
        public double? PA { get; set; }
        public double? HS { get; set; }
        public double? TPD { get; set; }
        public double? CI { get; set; }
        public double? NoClaimBonus { get; set; }
        public double? SurrenderValue { get; set; }
        public double? DatabaseCommission { get; set; }
        public double? BrokerageFee { get; set; }
        public double? ServiceFee { get; set; }

        public string CurrencyCode { get; set; }
        public double? CurrencyRate { get; set; }
        public string Frequency { get; set; }

        public string ContractCode { get; set; }
        public int? AnnualCohort { get; set; }
        public string Mfrs17CellName { get; set; }
    }
}
