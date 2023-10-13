using BusinessObject;
using Services;
using Shared;
using Shared.ProcessFile;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;

namespace ConsoleApp.Commands.ProcessDatas
{
    public class ProcessPerLifeRetroStatement : Command
    {
        public PerLifeRetroStatementBo RetroStatementBo { get; set; }

        public string FilePath { get; set; }

        public HttpPostedFileBase PostedFile { get; set; }

        public Excel Excel { get; set; }

        public List<Column> Column { get; set; }

        public List<string> Errors { get; set; }

        public bool IsSwissRe { get; set; } = false;

        public List<string> SwissReNames { get; set; }

        //public const int TypeCedingCompany = 1; // [2,3]
        //public const int TypeTreatyNo = 2; // [3,3]

        // Data Set 1
        public const int TypeAccountingPeriod = 3; // [7,5]
        public const int TypeReserveCededBegin = 4; // [11,5]
        public const int TypeReserveCededEnd = 5; // [13,5]
        public const int TypeRiskChargeCededBegin = 6; // [15,5]
        public const int TypeRiskChargeCededEnd = 7; // [17,5]
        public const int TypeAverageReserveCeded = 8; // [19,5]
        public const int TypeRiPremiumNB = 9; // [22,5]
        public const int TypeRiPremiumRN = 10; // [23,5]
        public const int TypeRiPremiumALT = 11; // [24,5]
        public const int TypeQuarterlyRiskPremium = 12; // [26,5]
        public const int TypeRetrocessionMarketingFee = 13; // []
        public const int TypeRiDiscountNB = 14; // [29,5]
        public const int TypeRiDiscountRN = 15; // [30,5]
        public const int TypeRiDiscountALT = 16; // [31,5]
        public const int TypeAgreedDatabaseComm = 17; // []
        public const int TypeGstPayable = 18; // [33,5]
        public const int TypeNoClaimBonus = 19; // []
        public const int TypeClaims = 20; // [35,5]
        public const int TypeProfitComm = 21; // [37,5]
        public const int TypeSurrenderValue = 22; // [39,5]
        public const int TypePaymentToTheReinsurer = 23; // [41,5]
        public const int TypeTotalNoOfPolicyNB = 24; // [44,5]
        public const int TypeTotalNoOfPolicyRN = 25; // [45,5]
        public const int TypeTotalNoOfPolicyALT = 26; // [46,5]
        public const int TypeTotalSumReinsuredNB = 27; // [49,5]
        public const int TypeTotalSumReinsuredRN = 28; // [50,5]
        public const int TypeTotalSumReinsuredALT = 29; // [51,5]

        // Data Set 2
        public const int TypeAccountingPeriod2 = 30; // [7,7]
        public const int TypeReserveCededBegin2 = 31; // [11,]
        public const int TypeReserveCededEnd2 = 32; // [13,7]
        public const int TypeRiskChargeCededBegin2 = 33; // [15,7]
        public const int TypeRiskChargeCededEnd2 = 34; // [17,7]
        public const int TypeAverageReserveCeded2 = 35; // [19,7]
        public const int TypeRiPremiumNB2 = 36; // [22,7]
        public const int TypeRiPremiumRN2 = 37; // [23,7]
        public const int TypeRiPremiumALT2 = 38; // [24,7]
        public const int TypeQuarterlyRiskPremium2 = 39; // [26,7]
        public const int TypeRetrocessionMarketingFee2 = 40; // []
        public const int TypeRiDiscountNB2 = 41; // [29,7]
        public const int TypeRiDiscountRN2 = 42; // [30,7]
        public const int TypeRiDiscountALT2 = 43; // [31,7]
        public const int TypeAgreedDatabaseComm2 = 44; // []
        public const int TypeGstPayable2 = 45; // [33,7]
        public const int TypeNoClaimBonus2 = 46; // []
        public const int TypeClaims2 = 47; // [35,7]
        public const int TypeProfitComm2 = 48; // [37,7]
        public const int TypeSurrenderValue2 = 49; // [39,7]
        public const int TypePaymentToTheReinsurer2 = 50; // [41,7]
        public const int TypeTotalNoOfPolicyNB2 = 51; // [44,7]
        public const int TypeTotalNoOfPolicyRN2 = 52; // [45,7]
        public const int TypeTotalNoOfPolicyALT2 = 53; // [46,7]
        public const int TypeTotalSumReinsuredNB2 = 54; // [49,7]
        public const int TypeTotalSumReinsuredRN2 = 55; // [50,7]
        public const int TypeTotalSumReinsuredALT2 = 56; // [51,7]

        // Data Set 3
        public const int TypeAccountingPeriod3 = 57; // [7,9]
        public const int TypeReserveCededBegin3 = 58; // [11,9]
        public const int TypeReserveCededEnd3 = 59; // [13,9]
        public const int TypeRiskChargeCededBegin3 = 60; // [15,9]
        public const int TypeRiskChargeCededEnd3 = 61; // [17,9]
        public const int TypeAverageReserveCeded3 = 62; // [19,9]
        public const int TypeRiPremiumNB3 = 63; // [22,9]
        public const int TypeRiPremiumRN3 = 64; // [23,9]
        public const int TypeRiPremiumALT3 = 65; // [24,9]
        public const int TypeQuarterlyRiskPremium3 = 66; // [26,9]
        public const int TypeRetrocessionMarketingFee3 = 67; // []
        public const int TypeRiDiscountNB3 = 68; // [29,9]
        public const int TypeRiDiscountRN3 = 69; // [30,9]
        public const int TypeRiDiscountALT3 = 70; // [31,9]
        public const int TypeAgreedDatabaseComm3 = 71; // []
        public const int TypeGstPayable3 = 72; // [33,9]
        public const int TypeNoClaimBonus3 = 73; // []
        public const int TypeClaims3 = 74; // [35,9]
        public const int TypeProfitComm3 = 75; // [37,9]
        public const int TypeSurrenderValue3 = 76; // [39,9]
        public const int TypePaymentToTheReinsurer3 = 77; // [41,9]
        public const int TypeTotalNoOfPolicyNB3 = 78; // [44,9]
        public const int TypeTotalNoOfPolicyRN3 = 79; // [45,9]
        public const int TypeTotalNoOfPolicyALT3 = 80; // [46,9]
        public const int TypeTotalSumReinsuredNB3 = 81; // [49,9]
        public const int TypeTotalSumReinsuredRN3 = 82; // [50,9]
        public const int TypeTotalSumReinsuredALT3 = 83; // [51,9]

        public ProcessPerLifeRetroStatement()
        {
            Title = "ProcessPerLifeRetroStatement";
            Description = "To retrieve data from excel file";
            Errors = new List<string> { };
        }

        public override void Run()
        {
            PrintStarting();
            Process();
            PrintEnding();
        }

        public PerLifeRetroStatementBo Process()
        {
            if (RetroStatementBo == null)
            {
                PrintError("Retro Statement Not Found");
                Errors.Add("Retro Statement Not Found");
                return RetroStatementBo;
            }

            if (PostedFile == null)
            {
                PrintError("File not uploaded");
                Errors.Add("File not uploaded");
                return RetroStatementBo;
            }

            string directory = RetroStatementBo.GetLocalDirectory();
            FilePath = Path.Combine(directory, PostedFile.FileName);
            Util.MakeDir(FilePath);
            PostedFile.SaveAs(FilePath);

            if (!File.Exists(FilePath))
            {
                PrintError(string.Format(MessageBag.FileNotExists, FilePath));
                Errors.Add(string.Format(MessageBag.FileNotExists, FilePath));
                return RetroStatementBo;
            }

            var retroParty = RetroPartyService.Find(RetroStatementBo.RetroPartyId);

            if (retroParty == null)
            {
                PrintError("Retro Party Not Exist");
                Errors.Add("Retro Party Not Exist");
                return RetroStatementBo;
            }

            Column = GetColumns();

            try
            {
                Excel = new Excel(FilePath, true);
            }
            catch (Exception e)
            {
                Errors.Add(e.Message);
                Util.DeleteFiles(directory, PostedFile.FileName);
                return RetroStatementBo;
            }

            try
            {
                SetValue();
            }
            catch (Exception e)
            {
                Errors.Add(e.Message);
                Util.DeleteFiles(directory, PostedFile.FileName);
            }

            foreach (string error in Errors)
            {
                PrintError(error);
            }

            return RetroStatementBo;
        }

        public void SetValue()
        {
            foreach (Column c in Column)
            {
                var cellValue = (Excel.XWorkSheet.Cells[c.RowIndex, c.ColIndex] as Microsoft.Office.Interop.Excel.Range).Value;

                if (cellValue == null || (cellValue is string && string.IsNullOrEmpty(cellValue)))
                {
                    RetroStatementBo.SetPropertyValue(c.Property, null);
                    continue;
                }

                switch (c.Type)
                {
                    //case TypeCedingCompany: // String
                    //case TypeTreatyNo:
                    //    ValidateStringDataType(cellValue, c);
                    //    break;
                    case TypeAccountingPeriod:
                    case TypeAccountingPeriod2:
                    case TypeAccountingPeriod3:
                        ValidateStringDataType(cellValue, c, true);
                        break;
                    case TypeTotalNoOfPolicyNB: // Integer
                    case TypeTotalNoOfPolicyRN:
                    case TypeTotalNoOfPolicyALT:
                    case TypeTotalNoOfPolicyNB2:
                    case TypeTotalNoOfPolicyRN2:
                    case TypeTotalNoOfPolicyALT2:
                    case TypeTotalNoOfPolicyNB3:
                    case TypeTotalNoOfPolicyRN3:
                    case TypeTotalNoOfPolicyALT3:
                        ValidateIntegerDataType(cellValue, c);
                        break;
                    default: // Double
                        ValidateDoubleDataType(cellValue, c);
                        break;
                }

            }

            // Calculate Payment to the Reinsurer
            // Data Set 1
            double riPremiumNB = RetroStatementBo.RiPremiumNB ?? 0;
            double riPremiumRN = RetroStatementBo.RiPremiumRN ?? 0;
            double riPremiumALT = RetroStatementBo.RiPremiumALT ?? 0;
            double quarterlyRiskPremium = RetroStatementBo.QuarterlyRiskPremium ?? 0;
            double retrocessionMarketingFee = RetroStatementBo.RetrocessionMarketingFee ?? 0;
            double riDiscountNB = RetroStatementBo.RiDiscountNB ?? 0;
            double riDiscountRN = RetroStatementBo.RiDiscountRN ?? 0;
            double riDiscountALT = RetroStatementBo.RiDiscountALT ?? 0;
            double agreedDatabaseComm = RetroStatementBo.AgreedDatabaseComm ?? 0;
            double gstPayable = RetroStatementBo.GstPayable ?? 0;
            double noClaimBonus = RetroStatementBo.NoClaimBonus ?? 0;
            double claims = RetroStatementBo.Claims ?? 0;
            double profitComm = RetroStatementBo.ProfitComm ?? 0;
            double surrenderValue = RetroStatementBo.SurrenderValue ?? 0;

            // Data Set 2
            double riPremiumNB2 = RetroStatementBo.RiPremiumNB2 ?? 0;
            double riPremiumRN2 = RetroStatementBo.RiPremiumRN2 ?? 0;
            double riPremiumALT2 = RetroStatementBo.RiPremiumALT2 ?? 0;
            double quarterlyRiskPremium2 = RetroStatementBo.QuarterlyRiskPremium2 ?? 0;
            double retrocessionMarketingFee2 = RetroStatementBo.RetrocessionMarketingFee2 ?? 0;
            double riDiscountNB2 = RetroStatementBo.RiDiscountNB2 ?? 0;
            double riDiscountRN2 = RetroStatementBo.RiDiscountRN2 ?? 0;
            double riDiscountALT2 = RetroStatementBo.RiDiscountALT2 ?? 0;
            double agreedDatabaseComm2 = RetroStatementBo.AgreedDatabaseComm2 ?? 0;
            double gstPayable2 = RetroStatementBo.GstPayable2 ?? 0;
            double noClaimBonus2 = RetroStatementBo.NoClaimBonus2 ?? 0;
            double claims2 = RetroStatementBo.Claims2 ?? 0;
            double profitComm2 = RetroStatementBo.ProfitComm2 ?? 0;
            double surrenderValue2 = RetroStatementBo.SurrenderValue2 ?? 0;

            // Data Set 3
            double riPremiumNB3 = RetroStatementBo.RiPremiumNB3 ?? 0;
            double riPremiumRN3 = RetroStatementBo.RiPremiumRN3 ?? 0;
            double riPremiumALT3 = RetroStatementBo.RiPremiumALT3 ?? 0;
            double quarterlyRiskPremium3 = RetroStatementBo.QuarterlyRiskPremium3 ?? 0;
            double retrocessionMarketingFee3 = RetroStatementBo.RetrocessionMarketingFee3 ?? 0;
            double riDiscountNB3 = RetroStatementBo.RiDiscountNB3 ?? 0;
            double riDiscountRN3 = RetroStatementBo.RiDiscountRN3 ?? 0;
            double riDiscountALT3 = RetroStatementBo.RiDiscountALT3 ?? 0;
            double agreedDatabaseComm3 = RetroStatementBo.AgreedDatabaseComm3 ?? 0;
            double gstPayable3 = RetroStatementBo.GstPayable3 ?? 0;
            double noClaimBonus3 = RetroStatementBo.NoClaimBonus3 ?? 0;
            double claims3 = RetroStatementBo.Claims3 ?? 0;
            double profitComm3 = RetroStatementBo.ProfitComm3 ?? 0;
            double surrenderValue3 = RetroStatementBo.SurrenderValue3 ?? 0;


            if (IsSwissRe)
            {
                RetroStatementBo.PaymentToTheReinsurer = riPremiumNB + riPremiumRN + riPremiumALT +
                    retrocessionMarketingFee -
                    riDiscountNB - riDiscountRN - riDiscountALT -
                    agreedDatabaseComm -
                    noClaimBonus -
                    claims -
                    surrenderValue;

                RetroStatementBo.PaymentToTheReinsurer2 = riPremiumNB2 + riPremiumRN2 + riPremiumALT2 +
                    retrocessionMarketingFee2 -
                    riDiscountNB2 - riDiscountRN2 - riDiscountALT2 -
                    agreedDatabaseComm2 -
                    noClaimBonus2 -
                    claims2 -
                    surrenderValue2;

                RetroStatementBo.PaymentToTheReinsurer3 = riPremiumNB3 + riPremiumRN3 + riPremiumALT3 +
                    retrocessionMarketingFee3 -
                    riDiscountNB3 - riDiscountRN3 - riDiscountALT3 -
                    agreedDatabaseComm3 -
                    noClaimBonus3 -
                    claims3 -
                    surrenderValue3;
            }
            else
            {
                RetroStatementBo.PaymentToTheReinsurer = riPremiumNB + riPremiumRN + riPremiumALT +
                    quarterlyRiskPremium -
                    riDiscountNB - riDiscountRN - riDiscountALT +
                    gstPayable -
                    claims -
                    profitComm -
                    surrenderValue;

                RetroStatementBo.PaymentToTheReinsurer2 = riPremiumNB2 + riPremiumRN2 + riPremiumALT2 +
                    quarterlyRiskPremium2 -
                    riDiscountNB2 - riDiscountRN2 - riDiscountALT2 +
                    gstPayable2 -
                    claims2 -
                    profitComm2 -
                    surrenderValue2;

                RetroStatementBo.PaymentToTheReinsurer3 = riPremiumNB3 + riPremiumRN3 + riPremiumALT3 +
                    quarterlyRiskPremium3 -
                    riDiscountNB3 - riDiscountRN3 - riDiscountALT3 +
                    gstPayable3 -
                    claims3 -
                    profitComm3 -
                    surrenderValue3;
            }
        }

        public void ValidateStringDataType(dynamic cellValue, Column c, bool isQuarter = false)
        {
            object value = cellValue;
            if (cellValue is string)
            {
                RetroStatementBo.SetPropertyValue(c.Property, isQuarter ? Util.FormatQuarter(value.ToString()) : value);
            }
            else
            {
                RetroStatementBo.SetPropertyValue(c.Property, isQuarter ? Util.FormatQuarter(value.ToString()) : value.ToString());
            }
        }

        public void ValidateIntegerDataType(dynamic cellValue, Column c)
        {
            object value = cellValue;
            if (cellValue is int)
            {
                RetroStatementBo.SetPropertyValue(c.Property, value);
            }
            else
            {
                int? intValue = Util.GetParseInt(value.ToString());
                if (!intValue.HasValue)
                {
                    Errors.Add(string.Format("{0}'s value: \"{0}\" is not numeric", c.Header, value.ToString()));
                    return;
                }
                RetroStatementBo.SetPropertyValue(c.Property, intValue);
            }
        }

        public void ValidateDoubleDataType(dynamic cellValue, Column c)
        {
            object value = cellValue;
            if (cellValue is double)
            {
                RetroStatementBo.SetPropertyValue(c.Property, value);
            }
            else
            {
                double? doubleValue = Util.StringToDouble(value.ToString(), true, 2);
                if (!doubleValue.HasValue)
                {
                    Errors.Add(string.Format("{0}'s value: \"{0}\" is not valid double format", c.Header, value.ToString()));
                    return;
                }
                RetroStatementBo.SetPropertyValue(c.Property, doubleValue);
            }
        }

        public static List<Column> GetColumns()
        {
            return new List<Column>
            {
                //new Column
                //{
                //    Type = TypeCedingCompany,
                //    RowIndex = 2,
                //    ColIndex = 3,
                //    Header = "Ceding Company",
                //    Property = "CedingCompany",
                //    IsExcel = true,
                //},
                //new Column
                //{
                //    Type = TypeTreatyNo,
                //    RowIndex = 3,
                //    ColIndex = 3,
                //    Header = "Treaty No",
                //    Property = "TreatyNo",
                //    IsExcel = true,
                //},

                // Data Set 1
                new Column
                {
                    Type = TypeAccountingPeriod,
                    RowIndex = 7,
                    ColIndex = 5,
                    Header = "Accounting Period 1",
                    Property = "AccountingPeriod",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeReserveCededBegin,
                    RowIndex = 11,
                    ColIndex = 5,
                    Header = "Reserve Ceded at the beginning of Accounting Period 1",
                    Property = "ReserveCededBegin",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeReserveCededEnd,
                    RowIndex = 13,
                    ColIndex = 5,
                    Header = "Reserve Ceded at the end of Accounting Period 1",
                    Property = "ReserveCededEnd",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeRiskChargeCededBegin,
                    RowIndex = 15,
                    ColIndex = 5,
                    Header = "Risk Charge Ceded at the beginning of Accounting Period 1",
                    Property = "RiskChargeCededBegin",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeRiskChargeCededEnd,
                    RowIndex = 17,
                    ColIndex = 5,
                    Header = "Risk Charge Ceded at the end of Accounting Period 1",
                    Property = "RiskChargeCededEnd",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeAverageReserveCeded,
                    RowIndex = 19,
                    ColIndex = 5,
                    Header = "Average Reserve Ceded 1",
                    Property = "AverageReserveCeded",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeRiPremiumNB,
                    RowIndex = 22,
                    ColIndex = 5,
                    Header = "Reinsurance Premium - New Business 1",
                    Property = "RiPremiumNB",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeRiPremiumRN,
                    RowIndex = 23,
                    ColIndex = 5,
                    Header = "Reinsurance Premium - Renewal 1",
                    Property = "RiPremiumRN",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeRiPremiumALT,
                    RowIndex = 24,
                    ColIndex = 5,
                    Header = "Reinsurance Premium - Alteration 1",
                    Property = "RiPremiumALT",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeQuarterlyRiskPremium,
                    RowIndex = 26,
                    ColIndex = 5,
                    Header = "Quarterly Risk Premium 1",
                    Property = "QuarterlyRiskPremium",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeRiDiscountNB,
                    RowIndex = 29,
                    ColIndex = 5,
                    Header = "Reinsurance Discount - New Business 1",
                    Property = "RiDiscountNB",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeRiDiscountRN,
                    RowIndex = 30,
                    ColIndex = 5,
                    Header = "Reinsurance Discount - Renewal 1",
                    Property = "RiDiscountRN",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeRiDiscountALT,
                    RowIndex = 31,
                    ColIndex = 5,
                    Header = "Reinsurance Discount - Alteration 1",
                    Property = "RiDiscountALT",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeGstPayable,
                    RowIndex = 33,
                    ColIndex = 5,
                    Header = "GST Payable 1",
                    Property = "GstPayable",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeClaims,
                    RowIndex = 35,
                    ColIndex = 5,
                    Header = "Claims 1",
                    Property = "Claims",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeProfitComm,
                    RowIndex = 37,
                    ColIndex = 5,
                    Header = "Profit Commission 1",
                    Property = "ProfitComm",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeSurrenderValue,
                    RowIndex = 39,
                    ColIndex = 5,
                    Header = "Surrender Value 1",
                    Property = "SurrenderValue",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypePaymentToTheReinsurer,
                    RowIndex = 41,
                    ColIndex = 5,
                    Header = "Payment to the Reinsurer 1",
                    Property = "PaymentToTheReinsurer",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeTotalNoOfPolicyNB,
                    RowIndex = 44,
                    ColIndex = 5,
                    Header = "Total No. of Policy - New Business 1",
                    Property = "TotalNoOfPolicyNB",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeTotalNoOfPolicyRN,
                    RowIndex = 45,
                    ColIndex = 5,
                    Header = "Total No. of Policy - Renewal 1",
                    Property = "TotalNoOfPolicyRN",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeTotalNoOfPolicyALT,
                    RowIndex = 46,
                    ColIndex = 5,
                    Header = "Total No. of Policy - Alteration 1",
                    Property = "TotalNoOfPolicyALT",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeTotalSumReinsuredNB,
                    RowIndex = 49,
                    ColIndex = 5,
                    Header = "Total Sum Reinsured - New Business 1",
                    Property = "TotalSumReinsuredNB",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeTotalSumReinsuredRN,
                    RowIndex = 50,
                    ColIndex = 5,
                    Header = "Total Sum Reinsured - Renewal 1",
                    Property = "TotalSumReinsuredRN",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeTotalSumReinsuredALT,
                    RowIndex = 51,
                    ColIndex = 5,
                    Header = "Total Sum Reinsured - Alteration 1",
                    Property = "TotalSumReinsuredALT",
                    IsExcel = true,
                },

                // Data Set 2
                new Column
                {
                    Type = TypeAccountingPeriod2,
                    RowIndex = 7,
                    ColIndex = 7,
                    Header = "Accounting Period 2",
                    Property = "AccountingPeriod2",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeReserveCededBegin2,
                    RowIndex = 11,
                    ColIndex = 7,
                    Header = "Reserve Ceded at the beginning of Accounting Period 2",
                    Property = "ReserveCededBegin2",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeReserveCededEnd2,
                    RowIndex = 13,
                    ColIndex = 7,
                    Header = "Reserve Ceded at the end of Accounting Period 2",
                    Property = "ReserveCededEnd2",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeRiskChargeCededBegin2,
                    RowIndex = 15,
                    ColIndex = 7,
                    Header = "Risk Charge Ceded at the beginning of Accounting Period 2",
                    Property = "RiskChargeCededBegin2",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeRiskChargeCededEnd2,
                    RowIndex = 17,
                    ColIndex = 7,
                    Header = "Risk Charge Ceded at the end of Accounting Period 2",
                    Property = "RiskChargeCededEnd2",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeAverageReserveCeded2,
                    RowIndex = 19,
                    ColIndex = 7,
                    Header = "Average Reserve Ceded 2",
                    Property = "AverageReserveCeded2",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeRiPremiumNB2,
                    RowIndex = 22,
                    ColIndex = 7,
                    Header = "Reinsurance Premium - New Business 2",
                    Property = "RiPremiumNB2",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeRiPremiumRN2,
                    RowIndex = 23,
                    ColIndex = 7,
                    Header = "Reinsurance Premium - Renewal 2",
                    Property = "RiPremiumRN2",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeRiPremiumALT2,
                    RowIndex = 24,
                    ColIndex = 7,
                    Header = "Reinsurance Premium - Alteration 2",
                    Property = "RiPremiumALT2",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeQuarterlyRiskPremium2,
                    RowIndex = 26,
                    ColIndex = 7,
                    Header = "Quarterly Risk Premium 2",
                    Property = "QuarterlyRiskPremium2",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeRiDiscountNB2,
                    RowIndex = 29,
                    ColIndex = 7,
                    Header = "Reinsurance Discount - New Business 2",
                    Property = "RiDiscountNB2",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeRiDiscountRN2,
                    RowIndex = 30,
                    ColIndex = 7,
                    Header = "Reinsurance Discount - Renewal 2",
                    Property = "RiDiscountRN2",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeRiDiscountALT2,
                    RowIndex = 31,
                    ColIndex = 7,
                    Header = "Reinsurance Discount - Alteration 2",
                    Property = "RiDiscountALT2",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeGstPayable2,
                    RowIndex = 33,
                    ColIndex = 7,
                    Header = "GST Payable 2",
                    Property = "GstPayable2",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeClaims2,
                    RowIndex = 35,
                    ColIndex = 7,
                    Header = "Claims 2",
                    Property = "Claims2",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeProfitComm2,
                    RowIndex = 37,
                    ColIndex = 7,
                    Header = "Profit Commission 2",
                    Property = "ProfitComm2",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeSurrenderValue2,
                    RowIndex = 39,
                    ColIndex = 7,
                    Header = "Surrender Value 2",
                    Property = "SurrenderValue2",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypePaymentToTheReinsurer2,
                    RowIndex = 41,
                    ColIndex = 7,
                    Header = "Payment to the Reinsurer 2",
                    Property = "PaymentToTheReinsurer2",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeTotalNoOfPolicyNB2,
                    RowIndex = 44,
                    ColIndex = 7,
                    Header = "Total No. of Policy - New Business 2",
                    Property = "TotalNoOfPolicyNB2",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeTotalNoOfPolicyRN2,
                    RowIndex = 45,
                    ColIndex = 7,
                    Header = "Total No. of Policy - Renewal 2",
                    Property = "TotalNoOfPolicyRN2",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeTotalNoOfPolicyALT2,
                    RowIndex = 46,
                    ColIndex = 7,
                    Header = "Total No. of Policy - Alteration 2",
                    Property = "TotalNoOfPolicyALT2",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeTotalSumReinsuredNB2,
                    RowIndex = 49,
                    ColIndex = 7,
                    Header = "Total Sum Reinsured - New Business 2",
                    Property = "TotalSumReinsuredNB2",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeTotalSumReinsuredRN2,
                    RowIndex = 50,
                    ColIndex = 7,
                    Header = "Total Sum Reinsured - Renewal 2",
                    Property = "TotalSumReinsuredRN2",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeTotalSumReinsuredALT2,
                    RowIndex = 51,
                    ColIndex = 7,
                    Header = "Total Sum Reinsured - Alteration 2",
                    Property = "TotalSumReinsuredALT2",
                    IsExcel = true,
                },
                
                // Data Set 3
                new Column
                {
                    Type = TypeAccountingPeriod3,
                    RowIndex = 7,
                    ColIndex = 9,
                    Header = "Accounting Period 3",
                    Property = "AccountingPeriod3",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeReserveCededBegin3,
                    RowIndex = 11,
                    ColIndex = 9,
                    Header = "Reserve Ceded at the beginning of Accounting Period 3",
                    Property = "ReserveCededBegin3",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeReserveCededEnd3,
                    RowIndex = 13,
                    ColIndex = 9,
                    Header = "Reserve Ceded at the end of Accounting Period 3",
                    Property = "ReserveCededEnd3",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeRiskChargeCededBegin3,
                    RowIndex = 15,
                    ColIndex = 9,
                    Header = "Risk Charge Ceded at the beginning of Accounting Period 3",
                    Property = "RiskChargeCededBegin3",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeRiskChargeCededEnd3,
                    RowIndex = 17,
                    ColIndex = 9,
                    Header = "Risk Charge Ceded at the end of Accounting Period 3",
                    Property = "RiskChargeCededEnd3",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeAverageReserveCeded3,
                    RowIndex = 19,
                    ColIndex = 9,
                    Header = "Average Reserve Ceded 3",
                    Property = "AverageReserveCeded3",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeRiPremiumNB3,
                    RowIndex = 22,
                    ColIndex = 9,
                    Header = "Reinsurance Premium - New Business 3",
                    Property = "RiPremiumNB3",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeRiPremiumRN3,
                    RowIndex = 23,
                    ColIndex = 9,
                    Header = "Reinsurance Premium - Renewal 3",
                    Property = "RiPremiumRN3",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeRiPremiumALT3,
                    RowIndex = 24,
                    ColIndex = 9,
                    Header = "Reinsurance Premium - Alteration 3",
                    Property = "RiPremiumALT3",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeQuarterlyRiskPremium3,
                    RowIndex = 26,
                    ColIndex = 9,
                    Header = "Quarterly Risk Premium 3",
                    Property = "QuarterlyRiskPremium3",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeRiDiscountNB3,
                    RowIndex = 29,
                    ColIndex = 9,
                    Header = "Reinsurance Discount - New Business 3",
                    Property = "RiDiscountNB3",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeRiDiscountRN3,
                    RowIndex = 30,
                    ColIndex = 9,
                    Header = "Reinsurance Discount - Renewal 3",
                    Property = "RiDiscountRN3",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeRiDiscountALT3,
                    RowIndex = 31,
                    ColIndex = 9,
                    Header = "Reinsurance Discount - Alteration 3",
                    Property = "RiDiscountALT3",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeGstPayable3,
                    RowIndex = 33,
                    ColIndex = 9,
                    Header = "GST Payable 3",
                    Property = "GstPayable3",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeClaims3,
                    RowIndex = 35,
                    ColIndex = 9,
                    Header = "Claims 3",
                    Property = "Claims3",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeProfitComm3,
                    RowIndex = 37,
                    ColIndex = 9,
                    Header = "Profit Commission 3",
                    Property = "ProfitComm3",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeSurrenderValue3,
                    RowIndex = 39,
                    ColIndex = 9,
                    Header = "Surrender Value 3",
                    Property = "SurrenderValue3",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypePaymentToTheReinsurer3,
                    RowIndex = 41,
                    ColIndex = 9,
                    Header = "Payment to the Reinsurer 3",
                    Property = "PaymentToTheReinsurer3",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeTotalNoOfPolicyNB3,
                    RowIndex = 44,
                    ColIndex = 9,
                    Header = "Total No. of Policy - New Business 3",
                    Property = "TotalNoOfPolicyNB3",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeTotalNoOfPolicyRN3,
                    RowIndex = 45,
                    ColIndex = 9,
                    Header = "Total No. of Policy - Renewal 3",
                    Property = "TotalNoOfPolicyRN3",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeTotalNoOfPolicyALT3,
                    RowIndex = 46,
                    ColIndex = 9,
                    Header = "Total No. of Policy - Alteration 3",
                    Property = "TotalNoOfPolicyALT3",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeTotalSumReinsuredNB3,
                    RowIndex = 49,
                    ColIndex = 9,
                    Header = "Total Sum Reinsured - New Business 3",
                    Property = "TotalSumReinsuredNB3",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeTotalSumReinsuredRN3,
                    RowIndex = 50,
                    ColIndex = 9,
                    Header = "Total Sum Reinsured - Renewal 3",
                    Property = "TotalSumReinsuredRN3",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeTotalSumReinsuredALT3,
                    RowIndex = 51,
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
                //new Column
                //{
                //    Type = TypeCedingCompany,
                //    RowIndex = 2,
                //    ColIndex = 3,
                //    Header = "Ceding Company",
                //    Property = "CedingCompany",
                //    IsExcel = true,
                //},
                //new Column
                //{
                //    Type = TypeTreatyNo,
                //    RowIndex = 3,
                //    ColIndex = 3,
                //    Header = "Treaty No",
                //    Property = "TreatyNo",
                //    IsExcel = true,
                //},

                // Data Set 1
                new Column
                {
                    Type = TypeAccountingPeriod,
                    RowIndex = 8,
                    ColIndex = 5,
                    Header = "AccountingPeriod 1",
                    Property = "AccountingPeriod",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeRiPremiumNB,
                    RowIndex = 13,
                    ColIndex = 5,
                    Header = "Reinsurance Premium - New Business 1",
                    Property = "RiPremiumNB",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeRiPremiumRN,
                    RowIndex = 14,
                    ColIndex = 5,
                    Header = "Reinsurance Premium - Renewal 1",
                    Property = "RiPremiumRN",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeRiPremiumALT,
                    RowIndex = 15,
                    ColIndex = 5,
                    Header = "Reinsurance Premium - Alteration 1",
                    Property = "RiPremiumALT",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeRetrocessionMarketingFee,
                    RowIndex = 17,
                    ColIndex = 5,
                    Header = "Retrocession Marketing Fee 1",
                    Property = "RetrocessionMarketingFee",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeRiDiscountNB,
                    RowIndex = 20,
                    ColIndex = 5,
                    Header = "Reinsurance Discount - New Business 1",
                    Property = "RiDiscountNB",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeRiDiscountRN,
                    RowIndex = 21,
                    ColIndex = 5,
                    Header = "Reinsurance Discount - Renewal 1",
                    Property = "RiDiscountRN",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeRiDiscountALT,
                    RowIndex = 22,
                    ColIndex = 5,
                    Header = "Reinsurance Discount - Alteration 1",
                    Property = "RiDiscountALT",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeAgreedDatabaseComm,
                    RowIndex = 24,
                    ColIndex = 5,
                    Header = "Agreed Database Commission 1",
                    Property = "AgreedDatabaseComm",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeNoClaimBonus,
                    RowIndex = 26,
                    ColIndex = 5,
                    Header = "No Claim Bonus 1",
                    Property = "NoClaimBonus",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeClaims,
                    RowIndex = 28,
                    ColIndex = 5,
                    Header = "Claims 1",
                    Property = "Claims",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeSurrenderValue,
                    RowIndex = 30,
                    ColIndex = 5,
                    Header = "Surrender Payout 1",
                    Property = "SurrenderValue",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypePaymentToTheReinsurer,
                    RowIndex = 32,
                    ColIndex = 5,
                    Header = "Payment to the Reinsurer 1",
                    Property = "PaymentToTheReinsurer",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeTotalNoOfPolicyNB,
                    RowIndex = 35,
                    ColIndex = 5,
                    Header = "Total No. of Policy - New Business 1",
                    Property = "TotalNoOfPolicyNB",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeTotalNoOfPolicyRN,
                    RowIndex = 36,
                    ColIndex = 5,
                    Header = "Total No. of Policy - Renewal 1",
                    Property = "TotalNoOfPolicyRN",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeTotalNoOfPolicyALT,
                    RowIndex = 37,
                    ColIndex = 5,
                    Header = "Total No. of Policy - Alteration 1",
                    Property = "TotalNoOfPolicyALT",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeTotalSumReinsuredNB,
                    RowIndex = 40,
                    ColIndex = 5,
                    Header = "Total Sum Reinsured - New Business 1",
                    Property = "TotalSumReinsuredNB",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeTotalSumReinsuredRN,
                    RowIndex = 41,
                    ColIndex = 5,
                    Header = "Total Sum Reinsured - Renewal 1",
                    Property = "TotalSumReinsuredRN",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeTotalSumReinsuredALT,
                    RowIndex = 42,
                    ColIndex = 5,
                    Header = "Total Sum Reinsured - Alteration 1",
                    Property = "TotalSumReinsuredALT",
                    IsExcel = true,
                },

                // Data Set 2
                new Column
                {
                    Type = TypeAccountingPeriod2,
                    RowIndex = 8,
                    ColIndex = 7,
                    Header = "Accounting Period 2",
                    Property = "AccountingPeriod2",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeRiPremiumNB2,
                    RowIndex = 13,
                    ColIndex = 7,
                    Header = "Reinsurance Premium - New Business 2",
                    Property = "RiPremiumNB2",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeRiPremiumRN2,
                    RowIndex = 14,
                    ColIndex = 7,
                    Header = "Reinsurance Premium - Renewal 2",
                    Property = "RiPremiumRN2",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeRiPremiumALT2,
                    RowIndex = 15,
                    ColIndex = 7,
                    Header = "Reinsurance Premium - Alteration 2",
                    Property = "RiPremiumALT2",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeRetrocessionMarketingFee2,
                    RowIndex = 17,
                    ColIndex = 7,
                    Header = "Retrocession Marketing Fee 2",
                    Property = "RetrocessionMarketingFee2",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeRiDiscountNB2,
                    RowIndex = 20,
                    ColIndex = 7,
                    Header = "Reinsurance Discount - New Business 2",
                    Property = "RiDiscountNB2",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeRiDiscountRN2,
                    RowIndex = 21,
                    ColIndex = 7,
                    Header = "Reinsurance Discount - Renewal 2",
                    Property = "RiDiscountRN2",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeRiDiscountALT2,
                    RowIndex = 22,
                    ColIndex = 7,
                    Header = "Reinsurance Discount - Alteration 2",
                    Property = "RiDiscountALT2",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeAgreedDatabaseComm2,
                    RowIndex = 24,
                    ColIndex = 7,
                    Header = "Agreed Database Commission 2",
                    Property = "AgreedDatabaseComm2",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeNoClaimBonus2,
                    RowIndex = 26,
                    ColIndex = 7,
                    Header = "No Claim Bonus 2",
                    Property = "NoClaimBonus2",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeClaims2,
                    RowIndex = 28,
                    ColIndex = 7,
                    Header = "Claims 2",
                    Property = "Claims2",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeSurrenderValue2,
                    RowIndex = 30,
                    ColIndex = 7,
                    Header = "Surrender Payout 2",
                    Property = "SurrenderValue2",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypePaymentToTheReinsurer2,
                    RowIndex = 32,
                    ColIndex = 7,
                    Header = "Payment to the Reinsurer 2",
                    Property = "PaymentToTheReinsurer2",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeTotalNoOfPolicyNB2,
                    RowIndex = 35,
                    ColIndex = 7,
                    Header = "Total No. of Policy - New Business 2",
                    Property = "TotalNoOfPolicyNB2",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeTotalNoOfPolicyRN2,
                    RowIndex = 36,
                    ColIndex = 7,
                    Header = "Total No. of Policy - Renewal 2",
                    Property = "TotalNoOfPolicyRN2",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeTotalNoOfPolicyALT2,
                    RowIndex = 37,
                    ColIndex = 7,
                    Header = "Total No. of Policy - Alteration 2",
                    Property = "TotalNoOfPolicyALT2",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeTotalSumReinsuredNB2,
                    RowIndex = 40,
                    ColIndex = 7,
                    Header = "Total Sum Reinsured - New Business 2",
                    Property = "TotalSumReinsuredNB2",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeTotalSumReinsuredRN2,
                    RowIndex = 41,
                    ColIndex = 7,
                    Header = "Total Sum Reinsured - Renewal 2",
                    Property = "TotalSumReinsuredRN2",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeTotalSumReinsuredALT2,
                    RowIndex = 42,
                    ColIndex = 7,
                    Header = "Total Sum Reinsured - Alteration 2",
                    Property = "TotalSumReinsuredALT2",
                    IsExcel = true,
                },

                // Data Set 3
                new Column
                {
                    Type = TypeAccountingPeriod3,
                    RowIndex = 8,
                    ColIndex = 9,
                    Header = "Accounting Period 3",
                    Property = "AccountingPeriod3",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeRiPremiumNB3,
                    RowIndex = 13,
                    ColIndex = 9,
                    Header = "Reinsurance Premium - New Business 3",
                    Property = "RiPremiumNB3",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeRiPremiumRN3,
                    RowIndex = 14,
                    ColIndex = 9,
                    Header = "Reinsurance Premium - Renewal 3",
                    Property = "RiPremiumRN3",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeRiPremiumALT3,
                    RowIndex = 15,
                    ColIndex = 9,
                    Header = "Reinsurance Premium - Alteration 3",
                    Property = "RiPremiumALT3",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeRetrocessionMarketingFee3,
                    RowIndex = 17,
                    ColIndex = 9,
                    Header = "Retrocession Marketing Fee 3",
                    Property = "RetrocessionMarketingFee3",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeRiDiscountNB3,
                    RowIndex = 20,
                    ColIndex = 9,
                    Header = "Reinsurance Discount - New Business 3",
                    Property = "RiDiscountNB3",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeRiDiscountRN3,
                    RowIndex = 21,
                    ColIndex = 9,
                    Header = "Reinsurance Discount - Renewal 3",
                    Property = "RiDiscountRN3",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeRiDiscountALT3,
                    RowIndex = 22,
                    ColIndex = 9,
                    Header = "Reinsurance Discount - Alteration 3",
                    Property = "RiDiscountALT3",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeAgreedDatabaseComm3,
                    RowIndex = 24,
                    ColIndex = 9,
                    Header = "Agreed Database Commission 3",
                    Property = "AgreedDatabaseComm3",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeNoClaimBonus3,
                    RowIndex = 26,
                    ColIndex = 9,
                    Header = "No Claim Bonus 3",
                    Property = "NoClaimBonus3",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeClaims3,
                    RowIndex = 28,
                    ColIndex = 9,
                    Header = "Claims 3",
                    Property = "Claims3",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeSurrenderValue3,
                    RowIndex = 30,
                    ColIndex = 9,
                    Header = "Surrender Payout 3",
                    Property = "SurrenderValue3",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypePaymentToTheReinsurer3,
                    RowIndex = 32,
                    ColIndex = 9,
                    Header = "Payment to the Reinsurer 3",
                    Property = "PaymentToTheReinsurer3",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeTotalNoOfPolicyNB3,
                    RowIndex = 35,
                    ColIndex = 9,
                    Header = "Total No. of Policy - New Business 3",
                    Property = "TotalNoOfPolicyNB3",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeTotalNoOfPolicyRN3,
                    RowIndex = 36,
                    ColIndex = 9,
                    Header = "Total No. of Policy - Renewal 3",
                    Property = "TotalNoOfPolicyRN3",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeTotalNoOfPolicyALT3,
                    RowIndex = 37,
                    ColIndex = 9,
                    Header = "Total No. of Policy - Alteration 3",
                    Property = "TotalNoOfPolicyALT3",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeTotalSumReinsuredNB3,
                    RowIndex = 40,
                    ColIndex = 9,
                    Header = "Total Sum Reinsured - New Business 3",
                    Property = "TotalSumReinsuredNB3",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeTotalSumReinsuredRN3,
                    RowIndex = 41,
                    ColIndex = 9,
                    Header = "Total Sum Reinsured - Renewal 3",
                    Property = "TotalSumReinsuredRN3",
                    IsExcel = true,
                },
                new Column
                {
                    Type = TypeTotalSumReinsuredALT3,
                    RowIndex = 42,
                    ColIndex = 9,
                    Header = "Total Sum Reinsured - Alteration 3",
                    Property = "TotalSumReinsuredALT3",
                    IsExcel = true,
                },
            };
        }
    }
}
