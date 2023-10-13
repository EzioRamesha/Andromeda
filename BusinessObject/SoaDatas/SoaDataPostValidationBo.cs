using Shared.ProcessFile;
using System;
using System.Collections.Generic;

namespace BusinessObject.SoaDatas
{
    public class SoaDataPostValidationBo
    {
        public int Id { get; set; }


        public int SoaDataBatchId { get; set; }
        public virtual SoaDataBatchBo SoaDataBatchBo { get; set; }


        public int Type { get; set; }


        public string BusinessCode { get; set; }
        public string TreatyCode { get; set; }
        public string SoaQuarter { get; set; }
        public string RiskQuarter { get; set; }
        public int? RiskMonth { get; set; }


        public double? NbPremium { get; set; } = 0;
        public double? RnPremium { get; set; } = 0;
        public double? AltPremium { get; set; } = 0;


        public double? GrossPremium { get; set; } = 0;

        public double? NbDiscount { get; set; } = 0;
        public double? RnDiscount { get; set; } = 0;
        public double? AltDiscount { get; set; } = 0;

        public double? TotalDiscount { get; set; } = 0;
        public double? NoClaimBonus { get; set; } = 0;
        public double? Claim { get; set; } = 0;
        public double? SurrenderValue { get; set; } = 0;
        public double? Gst { get; set; } = 0;

        public double? NetTotalAmount { get; set; } = 0;

        public string CurrencyCode { get; set; }
        public double? CurrencyRate { get; set; }

        public int? NbCession { get; set; } = 0;
        public int? RnCession { get; set; } = 0;
        public int? AltCession { get; set; } = 0;

        public double? NbSar { get; set; } = 0;
        public double? RnSar { get; set; } = 0;
        public double? AltSar { get; set; } = 0;

        public double? DTH { get; set; } = 0;
        public double? TPA { get; set; } = 0;
        public double? TPS { get; set; } = 0;
        public double? PPD { get; set; } = 0;
        public double? CCA { get; set; } = 0;
        public double? CCS { get; set; } = 0;
        public double? PA { get; set; } = 0;
        public double? HS { get; set; } = 0;
        public double? TPD { get; set; } = 0;
        public double? CI { get; set; } = 0;

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        public const int TypeMlreShareMlreChecking = 1;
        public const int TypeMlreShareCedantAmount = 2;
        public const int TypeLayer1ShareMlreChecking = 3;
        public const int TypeLayer1ShareCedantAmount = 4;
        public const int TypeRetakafulShareMlreChecking = 5;
        public const int TypeRetakafulShareCedantAmount = 6;
        public const int TypeMlreShareDiscrepancyCheck = 7;
        public const int TypeLayer1ShareDiscrepancyCheck = 8;
        public const int TypeRetakafulShareDiscrepancyCheck = 9;

        public static string GetTypeName(int key)
        {
            switch (key)
            {
                case TypeMlreShareMlreChecking:
                    return "MLRe's Share MLRe Checking";
                case TypeMlreShareCedantAmount:
                    return "MLRe's Share Cedant Amount";
                case TypeLayer1ShareMlreChecking:
                    return "Layer1's Share MLRe Checking";
                case TypeLayer1ShareCedantAmount:
                    return "Layer1's Share Cedant Amount";
                case TypeRetakafulShareMlreChecking:
                    return "Retakaful MLRe Checking";
                case TypeRetakafulShareCedantAmount:
                    return "Retakaful Cedant Amount";
                default:
                    return "";
            }
        }

        public double? GetGrossPremium()
        {
            GrossPremium = NbPremium.GetValueOrDefault() + RnPremium.GetValueOrDefault() + AltPremium.GetValueOrDefault();
            return GrossPremium;
        }

        public double? GetTotalDiscount()
        {
            TotalDiscount = NbDiscount.GetValueOrDefault() + RnDiscount.GetValueOrDefault() + AltDiscount.GetValueOrDefault();
            return TotalDiscount;
        }

        public double? GetNetTotalAmount()
        {
            // Gross Premium minus all expenses and claims
            NetTotalAmount = GrossPremium.GetValueOrDefault() - TotalDiscount.GetValueOrDefault() - NoClaimBonus.GetValueOrDefault() - SurrenderValue.GetValueOrDefault();
            if (Equals(double.NaN, NetTotalAmount)) NetTotalAmount = 0;
            return NetTotalAmount;
        }

        public static List<Column> GetColumns()
        {
            var columns = new List<Column>
            {
                new Column
                {
                    Header = "Treaty Code",
                    Property = "TreatyCode",
                },
                new Column
                {
                    Header = "Soa Qtr",
                    Property = "SoaQuarter",
                },
                new Column
                {
                    Header = "Risk Qtr",
                    Property = "RiskQuarter",
                },
                new Column
                {
                    Header = "Risk Month",
                    Property = "RiskMonth",
                },
                new Column
                {
                    Header = "NB Premium",
                    Property = "NbPremium",
                },
                new Column
                {
                    Header = "RN Premium",
                    Property = "RnPremium",
                },
                new Column
                {
                    Header = "ALT Premium",
                    Property = "AltPremium",
                },
                new Column
                {
                    Header = "Gross Premium",
                    Property = "GrossPremium",
                },
                new Column
                {
                    Header = "NB Discount",
                    Property = "NbDiscount",
                },
                new Column
                {
                    Header = "RN Discount",
                    Property = "RnDiscount",
                },
                new Column
                {
                    Header = "ALT Discount",
                    Property = "AltDiscount",
                },
                new Column
                {
                    Header = "Total Discount",
                    Property = "TotalDiscount",
                },
                new Column
                {
                    Header = "No Claim Bonus",
                    Property = "NoClaimBonus",
                },
                new Column
                {
                    Header = "Surr Value",
                    Property = "SurrenderValue",
                },
                new Column
                {
                    Header = "Net Total Amount",
                    Property = "NetTotalAmount",
                },
                new Column
                {
                    Header = "NB Cession",
                    Property = "NbCession",
                },
                new Column
                {
                    Header = "RN Cession",
                    Property = "RnCession",
                },
                new Column
                {
                    Header = "ALT Cession",
                    Property = "AltCession",
                },
                 new Column
                {
                    Header = "NB Sar",
                    Property = "NbSar",
                },
                new Column
                {
                    Header = "RN Sar",
                    Property = "RnSar",
                },
                new Column
                {
                    Header = "ALT Sar",
                    Property = "AltSar",
                },
                new Column
                {
                    Header = "DTH",
                    Property = "DTH",
                },
                new Column
                {
                    Header = "TPD",
                    Property = "TPD",
                },
                new Column
                {
                    Header = "CI",
                    Property = "CI",
                },
                new Column
                {
                    Header = "PA",
                    Property = "PA",
                },
                new Column
                {
                    Header = "HS",
                    Property = "HS",
                },
                new Column 
                { 
                    Header = "Currency Code", 
                    Property = "CurrencyCode" 
                },
                new Column 
                { 
                    Header = "Currency Rate", 
                    Property = "CurrencyRate" 
                }
            };
            return columns;
        }
    }
}
