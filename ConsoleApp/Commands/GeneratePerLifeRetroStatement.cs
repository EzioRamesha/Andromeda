using BusinessObject;
using Services;
using Shared;
using Shared.ProcessFile;
using System.Collections.Generic;
using System.IO;

namespace ConsoleApp.Commands
{
    public class GeneratePerLifeRetroStatement : Command
    {
        public PerLifeRetroStatementBo PerLifeRetroStatementBo { get; set; }

        public Excel PerLifeRetroStatementExcel { get; set; }

        public string Directory { get; set; }

        public string FileName { get; set; }

        public string FilePath { get; set; }

        public List<Column> Column { get; set; }

        public const int TypeCedingCompany = 1; // [2,2]
        public const int TypeTreatyCode = 2; // [3,2]
        public const int TypeTreatyNo = 3; // [4,2]
        public const int TypeSchedule = 4; // [5,2]
        public const int TypeTreatyType = 5; // [6,2]
        public const int TypeFromMlreTo = 6; // [7,2]
        public const int TypeAccountsFor = 7; // [8,2]
        public const int TypeDateReportCompleted = 8; // [9,2]
        public const int TypeDateSendToRetro = 9; // [10,2]

        // Data Set 1
        public const int TypeInvoiceNo1 = 10; // [11,5]
        public const int TypeAccountingPeriod = 11; // [12,5]
        public const int TypeReserveCededBegin = 12; // [15,5]
        public const int TypeReserveCededEnd = 13; // [17,5]
        public const int TypeRiskChargeCededBegin = 14; // [19,5]
        public const int TypeRiskChargeCededEnd = 15; // [21,5]
        public const int TypeAverageReserveCeded = 16; // [23,5]
        public const int TypeRiPremiumNB = 17; // [26,5]
        public const int TypeRiPremiumRN = 18; // [27,5]
        public const int TypeRiPremiumALT = 19; // [28,5]
        public const int TypeQuarterlyRiskPremium = 20; // [30,5]
        public const int TypeRetrocessionMarketingFee = 21; // []
        public const int TypeRiDiscountNB = 22; // [33,5]
        public const int TypeRiDiscountRN = 23; // [34,5]
        public const int TypeRiDiscountALT = 24; // [35,5]
        public const int TypeAgreedDatabaseComm = 25; // []
        public const int TypeGstPayable = 26; // [37,5]
        public const int TypeNoClaimBonus = 27; // []
        public const int TypeClaims = 28; // [39,5]
        public const int TypeProfitComm = 29; // [41,5]
        public const int TypeSurrenderValue = 30; // [43,5]
        public const int TypePaymentToTheReinsurer = 31; // [45,5]
        public const int TypeTotalNoOfPolicyNB = 32; // [48,5]
        public const int TypeTotalNoOfPolicyRN = 33; // [49,5]
        public const int TypeTotalNoOfPolicyALT = 34; // [50,5]
        public const int TypeTotalSumReinsuredNB = 35; // [53,5]
        public const int TypeTotalSumReinsuredRN = 36; // [54,5]
        public const int TypeTotalSumReinsuredALT = 37; // [55,5]

        // Data Set 2
        public const int TypeInvoiceNo2 = 38; // [11,7]
        public const int TypeAccountingPeriod2 = 39; // [12,7]
        public const int TypeReserveCededBegin2 = 40; // [15,]
        public const int TypeReserveCededEnd2 = 41; // [17,7]
        public const int TypeRiskChargeCededBegin2 = 42; // [19,7]
        public const int TypeRiskChargeCededEnd2 = 43; // [21,7]
        public const int TypeAverageReserveCeded2 = 44; // [23,7]
        public const int TypeRiPremiumNB2 = 45; // [26,7]
        public const int TypeRiPremiumRN2 = 46; // [27,7]
        public const int TypeRiPremiumALT2 = 47; // [28,7]
        public const int TypeQuarterlyRiskPremium2 = 48; // [30,7]
        public const int TypeRetrocessionMarketingFee2 = 49; // []
        public const int TypeRiDiscountNB2 = 50; // [33,7]
        public const int TypeRiDiscountRN2 = 51; // [34,7]
        public const int TypeRiDiscountALT2 = 52; // [35,7]
        public const int TypeAgreedDatabaseComm2 = 53; // []
        public const int TypeGstPayable2 = 54; // [37,7]
        public const int TypeNoClaimBonus2 = 55; // []
        public const int TypeClaims2 = 56; // [39,7]
        public const int TypeProfitComm2 = 57; // [41,7]
        public const int TypeSurrenderValue2 = 58; // [43,7]
        public const int TypePaymentToTheReinsurer2 = 59; // [45,7]
        public const int TypeTotalNoOfPolicyNB2 = 60; // [48,7]
        public const int TypeTotalNoOfPolicyRN2 = 61; // [49,7]
        public const int TypeTotalNoOfPolicyALT2 = 62; // [50,7]
        public const int TypeTotalSumReinsuredNB2 = 63; // [53,7]
        public const int TypeTotalSumReinsuredRN2 = 64; // [54,7]
        public const int TypeTotalSumReinsuredALT2 = 65; // [55,7]

        // Data Set 3
        public const int TypeInvoiceNo3 = 66; // [11,9]
        public const int TypeAccountingPeriod3 = 67; // [12,9]
        public const int TypeReserveCededBegin3 = 68; // [15,9]
        public const int TypeReserveCededEnd3 = 69; // [17,9]
        public const int TypeRiskChargeCededBegin3 = 70; // [19,9]
        public const int TypeRiskChargeCededEnd3 = 71; // [21,9]
        public const int TypeAverageReserveCeded3 = 72; // [23,9]
        public const int TypeRiPremiumNB3 = 73; // [26,9]
        public const int TypeRiPremiumRN3 = 74; // [27,9]
        public const int TypeRiPremiumALT3 = 75; // [28,9]
        public const int TypeQuarterlyRiskPremium3 = 76; // [30,9]
        public const int TypeRetrocessionMarketingFee3 = 77; // []
        public const int TypeRiDiscountNB3 = 78; // [33,9]
        public const int TypeRiDiscountRN3 = 79; // [34,9]
        public const int TypeRiDiscountALT3 = 80; // [35,9]
        public const int TypeAgreedDatabaseComm3 = 81; // []
        public const int TypeGstPayable3 = 82; // [37,9]
        public const int TypeNoClaimBonus3 = 83; // []
        public const int TypeClaims3 = 84; // [39,9]
        public const int TypeProfitComm3 = 85; // [41,9]
        public const int TypeSurrenderValue3 = 86; // [43,9]
        public const int TypePaymentToTheReinsurer3 = 87; // [45,9]
        public const int TypeTotalNoOfPolicyNB3 = 88; // [48,9]
        public const int TypeTotalNoOfPolicyRN3 = 89; // [49,9]
        public const int TypeTotalNoOfPolicyALT3 = 90; // [50,9]
        public const int TypeTotalSumReinsuredNB3 = 91; // [53,9]
        public const int TypeTotalSumReinsuredRN3 = 92; // [54,9]
        public const int TypeTotalSumReinsuredALT3 = 93; // [55,9]

        public GeneratePerLifeRetroStatement()
        {
            Title = "GeneratePerLifeRetroStatement";
            Description = "To generate Per Life Retro SOA Retro Statement excel file";
        }

        public override void Run()
        {
            PrintStarting();
            Process();
            PrintEnding();
        }

        public void Process()
        {
            if (PerLifeRetroStatementBo == null)
            {
                PrintError("Retro Statement Not Found");
                return;
            }

            GenerateDefaultFile();

            PerLifeRetroStatementExcel.OpenTemplate();

            foreach (Column col in Column)
            {
                object value = null;

                switch (col.Type)
                {
                    case TypeInvoiceNo1:
                        if (string.IsNullOrEmpty(PerLifeRetroStatementBo.AccountingPeriod))
                        {
                            value = null;
                        }
                        else
                        {
                            value = "No Invoice Created";
                        }
                        break;
                    case TypeInvoiceNo2:
                        if (string.IsNullOrEmpty(PerLifeRetroStatementBo.AccountingPeriod2))
                        {
                            value = null;
                        }
                        else
                        {
                            value = "No Invoice Created";
                        }
                        break;
                    case TypeInvoiceNo3:
                        if (string.IsNullOrEmpty(PerLifeRetroStatementBo.AccountingPeriod3))
                        {
                            value = null;
                        }
                        else
                        {
                            value = "No Invoice Created";
                        }
                        break;
                    default:
                        value = PerLifeRetroStatementBo.GetPropertyValue(col.Property);
                        break;
                }

                PerLifeRetroStatementExcel.WriteCell(col.RowIndex.Value, col.ColIndex.Value, value);
            }

            PerLifeRetroStatementExcel.Save();
        }

        public double CalculateTotal()
        {
            double riPremiumNB = PerLifeRetroStatementBo.RiPremiumNB ?? 0;
            double riPremiumRN = PerLifeRetroStatementBo.RiPremiumRN ?? 0;

            double quarterlyRiskPremium = PerLifeRetroStatementBo.QuarterlyRiskPremium ?? 0;

            double riDiscountNB = PerLifeRetroStatementBo.RiDiscountNB ?? 0;
            double riDiscountRN = PerLifeRetroStatementBo.RiDiscountRN ?? 0;

            return (riPremiumNB + riPremiumRN + quarterlyRiskPremium) - (riDiscountNB + riDiscountRN);
        }

        public void GenerateDefaultFile()
        {
            Column = GetColumns();

            var templateFilepath = Util.GetWebAppDocumentFilePath("RetroStatement_Default_Export.xlsx");
            FileName = string.Format("PerLifeRetroSOA_RetroStatement_{0}", PerLifeRetroStatementBo.RetroPartyBo.Party).AppendDateTimeFileName(".xlsx");
            Directory = Util.GetRetroStatementDownloadPath();
            FilePath = Path.Combine(Directory, FileName);
            PerLifeRetroStatementExcel = new Excel(templateFilepath, FilePath, 1);
        }

        public static List<Column> GetColumns()
        {
            return new List<Column>
            {
                new Column
                {
                    Type = TypeCedingCompany,
                    RowIndex = 2,
                    ColIndex = 3,
                    Header = "Ceding Company",
                    Property = "CedingCompany",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeTreatyCode,
                    RowIndex = 3,
                    ColIndex = 3,
                    Header = "Treaty Code",
                    Property = "TreatyCode",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeTreatyNo,
                    RowIndex = 4,
                    ColIndex = 3,
                    Header = "Treaty No",
                    Property = "TreatyNo",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeSchedule,
                    RowIndex = 5,
                    ColIndex = 3,
                    Header = "Schedule",
                    Property = "Schedule",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeTreatyType,
                    RowIndex = 6,
                    ColIndex = 3,
                    Header = "Treaty Type",
                    Property = "TreatyType",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeFromMlreTo,
                    RowIndex = 7,
                    ColIndex = 3,
                    Header = "From MLRe To",
                    Property = "FromMlreTo",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeAccountsFor,
                    RowIndex = 8,
                    ColIndex = 3,
                    Header = "Accounts For",
                    Property = "AccountsFor",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeDateReportCompleted,
                    RowIndex = 9,
                    ColIndex = 3,
                    Header = "Date Report Completed",
                    Property = "DateReportCompleted",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeDateSendToRetro,
                    RowIndex = 9,
                    ColIndex = 3,
                    Header = "Date Send to Retro",
                    Property = "DateSendToRetro",
                    IsExcel = true,
                },

                // Data Set 1
                new Column
                {
                    Type = TypeInvoiceNo1,
                    RowIndex = 11,
                    ColIndex = 5,
                    Header = "Invoice No 1",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeAccountingPeriod,
                    RowIndex = 12,
                    ColIndex = 5,
                    Header = "Accounting Period 1",
                    Property = "AccountingPeriod",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeReserveCededBegin,
                    RowIndex = 15,
                    ColIndex = 5,
                    Header = "Reserve Ceded at the beginning of Accounting Period 1",
                    Property = "ReserveCededBegin",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeReserveCededEnd,
                    RowIndex = 17,
                    ColIndex = 5,
                    Header = "Reserve Ceded at the end of Accounting Period 1",
                    Property = "ReserveCededEnd",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeRiskChargeCededBegin,
                    RowIndex = 19,
                    ColIndex = 5,
                    Header = "Risk Charge Ceded at the beginning of Accounting Period 1",
                    Property = "RiskChargeCededBegin",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeRiskChargeCededEnd,
                    RowIndex = 21,
                    ColIndex = 5,
                    Header = "Risk Charge Ceded at the end of Accounting Period 1",
                    Property = "RiskChargeCededEnd",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeAverageReserveCeded,
                    RowIndex = 23,
                    ColIndex = 5,
                    Header = "Average Reserve Ceded 1",
                    Property = "AverageReserveCeded",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeRiPremiumNB,
                    RowIndex = 26,
                    ColIndex = 5,
                    Header = "Reinsurance Premium - New Business 1",
                    Property = "RiPremiumNB",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeRiPremiumRN,
                    RowIndex = 27,
                    ColIndex = 5,
                    Header = "Reinsurance Premium - Renewal 1",
                    Property = "RiPremiumRN",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeRiPremiumALT,
                    RowIndex = 28,
                    ColIndex = 5,
                    Header = "Reinsurance Premium - Alteration 1",
                    Property = "RiPremiumALT",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeQuarterlyRiskPremium,
                    RowIndex = 30,
                    ColIndex = 5,
                    Header = "Quarterly Risk Premium 1",
                    Property = "QuarterlyRiskPremium",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeRiDiscountNB,
                    RowIndex = 33,
                    ColIndex = 5,
                    Header = "Reinsurance Discount - New Business 1",
                    Property = "RiDiscountNB",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeRiDiscountRN,
                    RowIndex = 34,
                    ColIndex = 5,
                    Header = "Reinsurance Discount - Renewal 1",
                    Property = "RiDiscountRN",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeRiDiscountALT,
                    RowIndex = 35,
                    ColIndex = 5,
                    Header = "Reinsurance Discount - Alteration 1",
                    Property = "RiDiscountALT",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeGstPayable,
                    RowIndex = 37,
                    ColIndex = 5,
                    Header = "GST Payable 1",
                    Property = "GstPayable",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeClaims,
                    RowIndex = 39,
                    ColIndex = 5,
                    Header = "Claims 1",
                    Property = "Claims",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeProfitComm,
                    RowIndex = 41,
                    ColIndex = 5,
                    Header = "Profit Commission 1",
                    Property = "ProfitComm",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeSurrenderValue,
                    RowIndex = 43,
                    ColIndex = 5,
                    Header = "Surrender Value 1",
                    Property = "SurrenderValue",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypePaymentToTheReinsurer,
                    RowIndex = 45,
                    ColIndex = 5,
                    Header = "Payment to the Reinsurer 1",
                    Property = "PaymentToTheReinsurer",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeTotalNoOfPolicyNB,
                    RowIndex = 48,
                    ColIndex = 5,
                    Header = "Total No. of Policy - New Business 1",
                    Property = "TotalNoOfPolicyNB",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeTotalNoOfPolicyRN,
                    RowIndex = 49,
                    ColIndex = 5,
                    Header = "Total No. of Policy - Renewal 1",
                    Property = "TotalNoOfPolicyRN",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeTotalNoOfPolicyALT,
                    RowIndex = 50,
                    ColIndex = 5,
                    Header = "Total No. of Policy - Alteration 1",
                    Property = "TotalNoOfPolicyALT",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeTotalSumReinsuredNB,
                    RowIndex = 53,
                    ColIndex = 5,
                    Header = "Total Sum Reinsured - New Business 1",
                    Property = "TotalSumReinsuredNB",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeTotalSumReinsuredRN,
                    RowIndex = 54,
                    ColIndex = 5,
                    Header = "Total Sum Reinsured - Renewal 1",
                    Property = "TotalSumReinsuredRN",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeTotalSumReinsuredALT,
                    RowIndex = 55,
                    ColIndex = 5,
                    Header = "Total Sum Reinsured - Alteration 1",
                    Property = "TotalSumReinsuredALT",
                    IsExcel = true,
                },

                // Data Set 2
                new Column
                {
                    Type = TypeInvoiceNo2,
                    RowIndex = 11,
                    ColIndex = 7,
                    Header = "Invoice No 2",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeAccountingPeriod2,
                    RowIndex = 12,
                    ColIndex = 7,
                    Header = "Accounting Period 2",
                    Property = "AccountingPeriod2",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeReserveCededBegin2,
                    RowIndex = 15,
                    ColIndex = 7,
                    Header = "Reserve Ceded at the beginning of Accounting Period 2",
                    Property = "ReserveCededBegin2",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeReserveCededEnd,
                    RowIndex = 17,
                    ColIndex = 7,
                    Header = "Reserve Ceded at the end of Accounting Period 2",
                    Property = "ReserveCededEnd2",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeRiskChargeCededBegin2,
                    RowIndex = 19,
                    ColIndex = 7,
                    Header = "Risk Charge Ceded at the beginning of Accounting Period 2",
                    Property = "RiskChargeCededBegin2",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeRiskChargeCededEnd2,
                    RowIndex = 21,
                    ColIndex = 7,
                    Header = "Risk Charge Ceded at the end of Accounting Period 2",
                    Property = "RiskChargeCededEnd2",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeAverageReserveCeded2,
                    RowIndex = 23,
                    ColIndex = 7,
                    Header = "Average Reserve Ceded 2",
                    Property = "AverageReserveCeded2",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeRiPremiumNB2,
                    RowIndex = 26,
                    ColIndex = 7,
                    Header = "Reinsurance Premium - New Business 2",
                    Property = "RiPremiumNB2",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeRiPremiumRN2,
                    RowIndex = 27,
                    ColIndex = 7,
                    Header = "Reinsurance Premium - Renewal 2",
                    Property = "RiPremiumRN2",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeRiPremiumALT2,
                    RowIndex = 28,
                    ColIndex = 7,
                    Header = "Reinsurance Premium - Alteration 2",
                    Property = "RiPremiumALT2",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeQuarterlyRiskPremium2,
                    RowIndex = 30,
                    ColIndex = 7,
                    Header = "Quarterly Risk Premium 2",
                    Property = "QuarterlyRiskPremium2",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeRiDiscountNB2,
                    RowIndex = 33,
                    ColIndex = 7,
                    Header = "Reinsurance Discount - New Business 2",
                    Property = "RiDiscountNB2",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeRiDiscountRN2,
                    RowIndex = 34,
                    ColIndex = 7,
                    Header = "Reinsurance Discount - Renewal 2",
                    Property = "RiDiscountRN2",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeRiDiscountALT2,
                    RowIndex = 35,
                    ColIndex = 7,
                    Header = "Reinsurance Discount - Alteration 2",
                    Property = "RiDiscountALT2",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeGstPayable2,
                    RowIndex = 37,
                    ColIndex = 7,
                    Header = "GST Payable 2",
                    Property = "GstPayable2",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeClaims2,
                    RowIndex = 39,
                    ColIndex = 7,
                    Header = "Claims 2",
                    Property = "Claims2",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeProfitComm2,
                    RowIndex = 41,
                    ColIndex = 7,
                    Header = "Profit Commission 2",
                    Property = "ProfitComm2",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeSurrenderValue2,
                    RowIndex = 43,
                    ColIndex = 7,
                    Header = "Surrender Value 2",
                    Property = "SurrenderValue2",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypePaymentToTheReinsurer2,
                    RowIndex = 45,
                    ColIndex = 7,
                    Header = "Payment to the Reinsurer 2",
                    Property = "PaymentToTheReinsurer2",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeTotalNoOfPolicyNB2,
                    RowIndex = 48,
                    ColIndex = 7,
                    Header = "Total No. of Policy - New Business 2",
                    Property = "TotalNoOfPolicyNB2",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeTotalNoOfPolicyRN2,
                    RowIndex = 49,
                    ColIndex = 7,
                    Header = "Total No. of Policy - Renewal 2",
                    Property = "TotalNoOfPolicyRN2",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeTotalNoOfPolicyALT2,
                    RowIndex = 50,
                    ColIndex = 7,
                    Header = "Total No. of Policy - Alteration 2",
                    Property = "TotalNoOfPolicyALT2",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeTotalSumReinsuredNB2,
                    RowIndex = 53,
                    ColIndex = 7,
                    Header = "Total Sum Reinsured - New Business 2",
                    Property = "TotalSumReinsuredNB2",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeTotalSumReinsuredRN2,
                    RowIndex = 54,
                    ColIndex = 7,
                    Header = "Total Sum Reinsured - Renewal 2",
                    Property = "TotalSumReinsuredRN2",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeTotalSumReinsuredALT2,
                    RowIndex = 55,
                    ColIndex = 7,
                    Header = "Total Sum Reinsured - Alteration 2",
                    Property = "TotalSumReinsuredALT2",
                    IsExcel = true,
                },
                
                // Data Set 3
                new Column
                {
                    Type = TypeInvoiceNo3,
                    RowIndex = 11,
                    ColIndex = 9,
                    Header = "Invoice No 3",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeAccountingPeriod3,
                    RowIndex = 12,
                    ColIndex = 9,
                    Header = "Accounting Period 3",
                    Property = "AccountingPeriod3",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeReserveCededBegin3,
                    RowIndex = 15,
                    ColIndex = 9,
                    Header = "Reserve Ceded at the beginning of Accounting Period 3",
                    Property = "ReserveCededBegin3",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeReserveCededEnd3,
                    RowIndex = 17,
                    ColIndex = 9,
                    Header = "Reserve Ceded at the end of Accounting Period 3",
                    Property = "ReserveCededEnd3",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeRiskChargeCededBegin3,
                    RowIndex = 19,
                    ColIndex = 9,
                    Header = "Risk Charge Ceded at the beginning of Accounting Period 3",
                    Property = "RiskChargeCededBegin3",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeRiskChargeCededEnd3,
                    RowIndex = 21,
                    ColIndex = 9,
                    Header = "Risk Charge Ceded at the end of Accounting Period 3",
                    Property = "RiskChargeCededEnd3",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeAverageReserveCeded3,
                    RowIndex = 23,
                    ColIndex = 9,
                    Header = "Average Reserve Ceded 3",
                    Property = "AverageReserveCeded3",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeRiPremiumNB3,
                    RowIndex = 26,
                    ColIndex = 9,
                    Header = "Reinsurance Premium - New Business 3",
                    Property = "RiPremiumNB3",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeRiPremiumRN3,
                    RowIndex = 27,
                    ColIndex = 9,
                    Header = "Reinsurance Premium - Renewal 3",
                    Property = "RiPremiumRN3",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeRiPremiumALT3,
                    RowIndex = 28,
                    ColIndex = 9,
                    Header = "Reinsurance Premium - Alteration 3",
                    Property = "RiPremiumALT3",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeQuarterlyRiskPremium3,
                    RowIndex = 30,
                    ColIndex = 9,
                    Header = "Quarterly Risk Premium 3",
                    Property = "QuarterlyRiskPremium3",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeRiDiscountNB3,
                    RowIndex = 33,
                    ColIndex = 9,
                    Header = "Reinsurance Discount - New Business 3",
                    Property = "RiDiscountNB3",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeRiDiscountRN3,
                    RowIndex = 34,
                    ColIndex = 9,
                    Header = "Reinsurance Discount - Renewal 3",
                    Property = "RiDiscountRN3",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeRiDiscountALT3,
                    RowIndex = 35,
                    ColIndex = 9,
                    Header = "Reinsurance Discount - Alteration 3",
                    Property = "RiDiscountALT3",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeGstPayable3,
                    RowIndex = 37,
                    ColIndex = 9,
                    Header = "GST Payable 3",
                    Property = "GstPayable3",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeClaims3,
                    RowIndex = 39,
                    ColIndex = 9,
                    Header = "Claims 3",
                    Property = "Claims3",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeProfitComm3,
                    RowIndex = 41,
                    ColIndex = 9,
                    Header = "Profit Commission 3",
                    Property = "ProfitComm3",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeSurrenderValue3,
                    RowIndex = 43,
                    ColIndex = 9,
                    Header = "Surrender Value 3",
                    Property = "SurrenderValue3",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypePaymentToTheReinsurer3,
                    RowIndex = 45,
                    ColIndex = 9,
                    Header = "Payment to the Reinsurer 3",
                    Property = "PaymentToTheReinsurer3",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeTotalNoOfPolicyNB3,
                    RowIndex = 48,
                    ColIndex = 9,
                    Header = "Total No. of Policy - New Business 3",
                    Property = "TotalNoOfPolicyNB3",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeTotalNoOfPolicyRN3,
                    RowIndex = 49,
                    ColIndex = 9,
                    Header = "Total No. of Policy - Renewal 3",
                    Property = "TotalNoOfPolicyRN3",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeTotalNoOfPolicyALT3,
                    RowIndex = 50,
                    ColIndex = 9,
                    Header = "Total No. of Policy - Alteration 3",
                    Property = "TotalNoOfPolicyALT3",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeTotalSumReinsuredNB3,
                    RowIndex = 53,
                    ColIndex = 9,
                    Header = "Total Sum Reinsured - New Business 3",
                    Property = "TotalSumReinsuredNB3",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeTotalSumReinsuredRN3,
                    RowIndex = 54,
                    ColIndex = 9,
                    Header = "Total Sum Reinsured - Renewal 3",
                    Property = "TotalSumReinsuredRN3",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeTotalSumReinsuredALT3,
                    RowIndex = 55,
                    ColIndex = 9,
                    Header = "Total Sum Reinsured - Alteration 3",
                    Property = "TotalSumReinsuredALT3",
                    IsExcel = true,
                },
            };
        }

        public static List<Column> GetSwissReColumns()
        {
            return new List<Column>
            {
                new Column
                {
                    Type = TypeCedingCompany,
                    RowIndex = 2,
                    ColIndex = 3,
                    Header = "Ceding Company",
                    Property = "CedingCompany",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeTreatyCode,
                    RowIndex = 3,
                    ColIndex = 3,
                    Header = "Treaty Code",
                    Property = "TreatyCode",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeTreatyNo,
                    RowIndex = 4,
                    ColIndex = 3,
                    Header = "Treaty No",
                    Property = "TreatyNo",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeSchedule,
                    RowIndex = 5,
                    ColIndex = 3,
                    Header = "Schedule",
                    Property = "Schedule",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeTreatyType,
                    RowIndex = 6,
                    ColIndex = 3,
                    Header = "Treaty Type",
                    Property = "TreatyType",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeFromMlreTo,
                    RowIndex = 7,
                    ColIndex = 3,
                    Header = "From MLRe To",
                    Property = "FromMlreTo",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeAccountsFor,
                    RowIndex = 8,
                    ColIndex = 3,
                    Header = "Accounts For",
                    Property = "AccountsFor",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeDateReportCompleted,
                    RowIndex = 9,
                    ColIndex = 3,
                    Header = "Date Report Completed",
                    Property = "DateReportCompleted",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeDateSendToRetro,
                    RowIndex = 9,
                    ColIndex = 3,
                    Header = "Date Send to Retro",
                    Property = "DateSendToRetro",
                    IsExcel = true,
                },

                // Data Set 1
                new Column
                {
                    Type = TypeInvoiceNo1,
                    RowIndex = 11,
                    ColIndex = 5,
                    Header = "Invoice No 1",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeAccountingPeriod,
                    RowIndex = 12,
                    ColIndex = 5,
                    Header = "AccountingPeriod 1",
                    Property = "AccountingPeriod",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeRiPremiumNB,
                    RowIndex = 16,
                    ColIndex = 5,
                    Header = "Reinsurance Premium - New Business 1",
                    Property = "RiPremiumNB",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeRiPremiumRN,
                    RowIndex = 17,
                    ColIndex = 5,
                    Header = "Reinsurance Premium - Renewal 1",
                    Property = "RiPremiumRN",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeRiPremiumALT,
                    RowIndex = 18,
                    ColIndex = 5,
                    Header = "Reinsurance Premium - Alteration 1",
                    Property = "RiPremiumALT",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeRetrocessionMarketingFee,
                    RowIndex = 20,
                    ColIndex = 5,
                    Header = "Retrocession Marketing Fee 1",
                    Property = "RetrocessionMarketingFee",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeRiDiscountNB,
                    RowIndex = 23,
                    ColIndex = 5,
                    Header = "Reinsurance Discount - New Business 1",
                    Property = "RiDiscountNB",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeRiDiscountRN,
                    RowIndex = 24,
                    ColIndex = 5,
                    Header = "Reinsurance Discount - Renewal 1",
                    Property = "RiDiscountRN",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeRiDiscountALT,
                    RowIndex = 25,
                    ColIndex = 5,
                    Header = "Reinsurance Discount - Alteration 1",
                    Property = "RiDiscountALT",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeAgreedDatabaseComm,
                    RowIndex = 27,
                    ColIndex = 5,
                    Header = "Agreed Database Commission 1",
                    Property = "AgreedDatabaseComm",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeNoClaimBonus,
                    RowIndex = 29,
                    ColIndex = 5,
                    Header = "No Claim Bonus 1",
                    Property = "NoClaimBonus",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeClaims,
                    RowIndex = 31,
                    ColIndex = 5,
                    Header = "Claims 1",
                    Property = "Claims",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeSurrenderValue,
                    RowIndex = 33,
                    ColIndex = 5,
                    Header = "Surrender Payout 1",
                    Property = "SurrenderValue",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypePaymentToTheReinsurer,
                    RowIndex = 35,
                    ColIndex = 5,
                    Header = "Payment to the Reinsurer 1",
                    Property = "PaymentToTheReinsurer",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeTotalNoOfPolicyNB,
                    RowIndex = 38,
                    ColIndex = 5,
                    Header = "Total No. of Policy - New Business 1",
                    Property = "TotalNoOfPolicyNB",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeTotalNoOfPolicyRN,
                    RowIndex = 39,
                    ColIndex = 5,
                    Header = "Total No. of Policy - Renewal 1",
                    Property = "TotalNoOfPolicyRN",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeTotalNoOfPolicyALT,
                    RowIndex = 40,
                    ColIndex = 5,
                    Header = "Total No. of Policy - Alteration 1",
                    Property = "TotalNoOfPolicyALT",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeTotalSumReinsuredNB,
                    RowIndex = 43,
                    ColIndex = 5,
                    Header = "Total Sum Reinsured - New Business 1",
                    Property = "TotalSumReinsuredNB",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeTotalSumReinsuredRN,
                    RowIndex = 44,
                    ColIndex = 5,
                    Header = "Total Sum Reinsured - Renewal 1",
                    Property = "TotalSumReinsuredRN",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeTotalSumReinsuredALT,
                    RowIndex = 45,
                    ColIndex = 5,
                    Header = "Total Sum Reinsured - Alteration 1",
                    Property = "TotalSumReinsuredALT",
                    IsExcel = true,
                },

                // Data Set 2
                new Column
                {
                    Type = TypeInvoiceNo2,
                    RowIndex = 11,
                    ColIndex = 7,
                    Header = "Invoice No 2",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeAccountingPeriod2,
                    RowIndex = 12,
                    ColIndex = 7,
                    Header = "Accounting Period 2",
                    Property = "AccountingPeriod2",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeRiPremiumNB2,
                    RowIndex = 16,
                    ColIndex = 7,
                    Header = "Reinsurance Premium - New Business 2",
                    Property = "RiPremiumNB2",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeRiPremiumRN2,
                    RowIndex = 17,
                    ColIndex = 7,
                    Header = "Reinsurance Premium - Renewal 2",
                    Property = "RiPremiumRN2",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeRiPremiumALT2,
                    RowIndex = 18,
                    ColIndex = 7,
                    Header = "Reinsurance Premium - Alteration 2",
                    Property = "RiPremiumALT2",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeRetrocessionMarketingFee2,
                    RowIndex = 20,
                    ColIndex = 7,
                    Header = "Retrocession Marketing Fee 2",
                    Property = "RetrocessionMarketingFee2",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeRiDiscountNB2,
                    RowIndex = 23,
                    ColIndex = 7,
                    Header = "Reinsurance Discount - New Business 2",
                    Property = "RiDiscountNB2",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeRiDiscountRN2,
                    RowIndex = 24,
                    ColIndex = 7,
                    Header = "Reinsurance Discount - Renewal 2",
                    Property = "RiDiscountRN2",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeRiDiscountALT2,
                    RowIndex = 25,
                    ColIndex = 7,
                    Header = "Reinsurance Discount - Alteration 2",
                    Property = "RiDiscountALT2",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeAgreedDatabaseComm2,
                    RowIndex = 27,
                    ColIndex = 7,
                    Header = "Agreed Database Commission 2",
                    Property = "AgreedDatabaseComm2",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeNoClaimBonus2,
                    RowIndex = 29,
                    ColIndex = 7,
                    Header = "No Claim Bonus 2",
                    Property = "NoClaimBonus2",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeClaims2,
                    RowIndex = 31,
                    ColIndex = 7,
                    Header = "Claims 2",
                    Property = "Claims2",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeSurrenderValue2,
                    RowIndex = 33,
                    ColIndex = 7,
                    Header = "Surrender Payout 2",
                    Property = "SurrenderValue2",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypePaymentToTheReinsurer2,
                    RowIndex = 35,
                    ColIndex = 7,
                    Header = "Payment to the Reinsurer 2",
                    Property = "PaymentToTheReinsurer2",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeTotalNoOfPolicyNB2,
                    RowIndex = 38,
                    ColIndex = 7,
                    Header = "Total No. of Policy - New Business 2",
                    Property = "TotalNoOfPolicyNB2",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeTotalNoOfPolicyRN2,
                    RowIndex = 39,
                    ColIndex = 7,
                    Header = "Total No. of Policy - Renewal 2",
                    Property = "TotalNoOfPolicyRN2",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeTotalNoOfPolicyALT2,
                    RowIndex = 40,
                    ColIndex = 7,
                    Header = "Total No. of Policy - Alteration 2",
                    Property = "TotalNoOfPolicyALT2",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeTotalSumReinsuredNB2,
                    RowIndex = 43,
                    ColIndex = 7,
                    Header = "Total Sum Reinsured - New Business 2",
                    Property = "TotalSumReinsuredNB2",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeTotalSumReinsuredRN2,
                    RowIndex = 44,
                    ColIndex = 7,
                    Header = "Total Sum Reinsured - Renewal 2",
                    Property = "TotalSumReinsuredRN2",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeTotalSumReinsuredALT2,
                    RowIndex = 45,
                    ColIndex = 7,
                    Header = "Total Sum Reinsured - Alteration 2",
                    Property = "TotalSumReinsuredALT2",
                    IsExcel = true,
                },

                // Data Set 3
                new Column
                {
                    Type = TypeInvoiceNo3,
                    RowIndex = 11,
                    ColIndex = 9,
                    Header = "Invoice No 3",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeAccountingPeriod3,
                    RowIndex = 12,
                    ColIndex = 9,
                    Header = "Accounting Period 3",
                    Property = "AccountingPeriod3",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeRiPremiumNB3,
                    RowIndex = 16,
                    ColIndex = 9,
                    Header = "Reinsurance Premium - New Business 3",
                    Property = "RiPremiumNB3",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeRiPremiumRN3,
                    RowIndex = 17,
                    ColIndex = 9,
                    Header = "Reinsurance Premium - Renewal 3",
                    Property = "RiPremiumRN3",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeRiPremiumALT3,
                    RowIndex = 18,
                    ColIndex = 9,
                    Header = "Reinsurance Premium - Alteration 3",
                    Property = "RiPremiumALT3",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeRetrocessionMarketingFee3,
                    RowIndex = 20,
                    ColIndex = 9,
                    Header = "Retrocession Marketing Fee 3",
                    Property = "RetrocessionMarketingFee3",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeRiDiscountNB3,
                    RowIndex = 23,
                    ColIndex = 9,
                    Header = "Reinsurance Discount - New Business 3",
                    Property = "RiDiscountNB3",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeRiDiscountRN3,
                    RowIndex = 24,
                    ColIndex = 9,
                    Header = "Reinsurance Discount - Renewal 3",
                    Property = "RiDiscountRN3",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeRiDiscountALT3,
                    RowIndex = 25,
                    ColIndex = 9,
                    Header = "Reinsurance Discount - Alteration 3",
                    Property = "RiDiscountALT3",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeAgreedDatabaseComm3,
                    RowIndex = 27,
                    ColIndex = 9,
                    Header = "Agreed Database Commission 3",
                    Property = "AgreedDatabaseComm3",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeNoClaimBonus3,
                    RowIndex = 29,
                    ColIndex = 9,
                    Header = "No Claim Bonus 3",
                    Property = "NoClaimBonus3",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeClaims3,
                    RowIndex = 31,
                    ColIndex = 9,
                    Header = "Claims 3",
                    Property = "Claims3",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeSurrenderValue3,
                    RowIndex = 33,
                    ColIndex = 9,
                    Header = "Surrender Payout 3",
                    Property = "SurrenderValue3",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypePaymentToTheReinsurer3,
                    RowIndex = 35,
                    ColIndex = 9,
                    Header = "Payment to the Reinsurer 3",
                    Property = "PaymentToTheReinsurer3",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeTotalNoOfPolicyNB3,
                    RowIndex = 38,
                    ColIndex = 9,
                    Header = "Total No. of Policy - New Business 3",
                    Property = "TotalNoOfPolicyNB3",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeTotalNoOfPolicyRN3,
                    RowIndex = 39,
                    ColIndex = 9,
                    Header = "Total No. of Policy - Renewal 3",
                    Property = "TotalNoOfPolicyRN3",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeTotalNoOfPolicyALT3,
                    RowIndex = 40,
                    ColIndex = 9,
                    Header = "Total No. of Policy - Alteration 3",
                    Property = "TotalNoOfPolicyALT3",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeTotalSumReinsuredNB3,
                    RowIndex = 43,
                    ColIndex = 9,
                    Header = "Total Sum Reinsured - New Business 3",
                    Property = "TotalSumReinsuredNB3",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeTotalSumReinsuredRN3,
                    RowIndex = 44,
                    ColIndex = 9,
                    Header = "Total Sum Reinsured - Renewal 3",
                    Property = "TotalSumReinsuredRN3",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeTotalSumReinsuredALT3,
                    RowIndex = 45,
                    ColIndex = 9,
                    Header = "Total Sum Reinsured - Alteration 3",
                    Property = "TotalSumReinsuredALT3",
                    IsExcel = true,
                },
            };
        }
    }
}
