using Shared.ProcessFile;
using System;
using System.Collections.Generic;

namespace BusinessObject.SoaDatas
{
    public class SoaDataRiDataSummaryBo
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

        public double? NbPremium { get; set; }

        public double? RnPremium { get; set; }

        public double? AltPremium { get; set; }

        public double? GrossPremium { get; set; }

        public double? TotalDiscount { get; set; }

        public double? NoClaimBonus { get; set; }

        public double? Claim { get; set; }

        public double? SurrenderValue { get; set; }

        public double? Gst { get; set; }

        public double? DatabaseCommission { get; set; }

        public double? BrokerageFee { get; set; }

        public double? ServiceFee { get; set; }

        public double? NetTotalAmount { get; set; }

        public string CurrencyCode { get; set; }

        public double? CurrencyRate { get; set; }


        public double? NbDiscount { get; set; } = 0;
        public double? RnDiscount { get; set; } = 0;
        public double? AltDiscount { get; set; } = 0;


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

        public string ContractCode { get; set; }
        public int? AnnualCohort { get; set; }
        public string Mfrs17CellName { get; set; }
        public string Frequency { get; set; }

        public string TreatyId { get; set; }
        public int CreatedById { get; set; }
        public int? UpdatedById { get; set; }


        public const int TypeSoaData = 1;
        public const int TypeRiDataSummary = 2;
        public const int TypeDifferences = 3;
        public const int TypeRiDataSummaryIfrs17 = 4;
        public const int TypeRiDataSummaryCNIfrs17 = 5;
        public const int TypeRiDataSummaryIfrs4 = 6;

        public SoaDataRiDataSummaryBo()
        {
            NbPremium = 0;
            RnPremium = 0;
            AltPremium = 0;
            GrossPremium = 0;
            TotalDiscount = 0;
            NoClaimBonus = 0;
            Claim = 0;
            SurrenderValue = 0;
            Gst = 0;
        }

        public double? GetGrossPremium()
        {
            GrossPremium = NbPremium.GetValueOrDefault() + RnPremium.GetValueOrDefault() + AltPremium.GetValueOrDefault();
            return GrossPremium;
        }

        public double? GetNetTotalAmount()
        {
            // Gross Premium minus all expenses and claims
            NetTotalAmount = GrossPremium.GetValueOrDefault()
                - TotalDiscount.GetValueOrDefault() 
                - NoClaimBonus.GetValueOrDefault() 
                - Claim.GetValueOrDefault() 
                - SurrenderValue.GetValueOrDefault() 
                + Gst.GetValueOrDefault();
            if (Equals(double.NaN, NetTotalAmount)) NetTotalAmount = 0;
            return NetTotalAmount;
        }

        public static List<Column> GetColumns(bool TypeRetakaful = false)
        {
            var columns = new List<Column>
            {
                new Column
                {
                    Header = "Treaty Id",
                    Property = "TreatyId",
                },
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
                    Header = "Total Discount",
                    Property = "TotalDiscount",
                },               
            };

            if (TypeRetakaful)
            {
                columns.Add(new Column
                {
                    Header = "Claim",
                    Property = "Claim",
                });
                columns.Add(new Column
                {
                    Header = "Net Total Amount",
                    Property = "NetTotalAmount",
                });
            }
            else
            {
                columns.Add(new Column
                {
                    Header = "No Claim Bonus",
                    Property = "NoClaimBonus",
                });
                columns.Add(new Column
                {
                    Header = "Claim",
                    Property = "Claim",
                });
                columns.Add(new Column
                {
                    Header = "Surr Value",
                    Property = "SurrenderValue",
                });
                columns.Add(new Column
                {
                    Header = "GST",
                    Property = "Gst",
                });
                columns.Add(new Column
                {
                    Header = "Net Total Amount",
                    Property = "NetTotalAmount",
                });
                columns.Add(new Column 
                { 
                    Header = "Currency Code", 
                    Property = "CurrencyCode" 
                });
                columns.Add(new Column 
                { 
                    Header = "Currency Rate", 
                    Property = "CurrencyRate" 
                });
            }

            return columns;
        }
    }
}
