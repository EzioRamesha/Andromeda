using Newtonsoft.Json;
using Shared;
using Shared.ProcessFile;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.SoaDatas
{
    public class SoaDataBo
    {
        public int Id { get; set; }

        public int SoaDataBatchId { get; set; }
        public virtual SoaDataBatchBo SoaDataBatchBo { get; set; }

        public int? SoaDataFileId { get; set; }
        public virtual SoaDataFileBo SoaDataFileBo { get; set; }


        public int MappingStatus { get; set; }
        public bool MappingValidate { get; set; }


        public string Errors { get; set; }
        public dynamic ErrorObject { get; set; }
        public IDictionary<string, object> ErrorDictionary { get; set; }


        public string CompanyName { get; set; }

        public string BusinessCode { get; set; }

        public string TreatyId { get; set; }

        public string TreatyCode { get; set; }

        public string TreatyMode { get; set; }

        public string TreatyType { get; set; }

        public string PlanBlock { get; set; }

        public int? RiskMonth { get; set; }

        public string SoaQuarter { get; set; }

        public string RiskQuarter { get; set; }

        public double? NbPremium { get; set; }

        public double? RnPremium { get; set; }

        public double? AltPremium { get; set; }

        public double? GrossPremium { get; set; }

        public double? TotalDiscount { get; set; }

        public double? RiskPremium { get; set; }

        public double? NoClaimBonus { get; set; }

        public double? Levy { get; set; }

        public double? Claim { get; set; }

        public double? ProfitComm { get; set; }

        public double? SurrenderValue { get; set; }

        public double? Gst { get; set; }

        public double? ModcoReserveIncome { get; set; }

        public double? RiDeposit { get; set; }

        public double? DatabaseCommission { get; set; }

        public double? AdministrationContribution { get; set; }

        public double? ShareOfRiCommissionFromCompulsoryCession { get; set; }

        public double? RecaptureFee { get; set; }

        public double? CreditCardCharges { get; set; }

        public double? BrokerageFee { get; set; }

        public double? TotalCommission { get; set; }

        public double? NetTotalAmount { get; set; }

        public DateTime? SoaReceivedDate { get; set; }

        public DateTime? BordereauxReceivedDate { get; set; }

        public string StatementStatus { get; set; }

        public string Remarks1 { get; set; }

        public string CurrencyCode { get; set; }

        public double? CurrencyRate { get; set; }

        public string SoaStatus { get; set; }

        public DateTime? ConfirmationDate { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        public const int MappingStatusPending = 1;
        public const int MappingStatusSuccess = 2;
        public const int MappingStatusFailed = 3;
        public const int MappingStatusMax = 3;

        public static string GetMappingStatusName(int? key)
        {
            switch (key)
            {
                case MappingStatusPending:
                    return "Pending";
                case MappingStatusSuccess:
                    return "Success";
                case MappingStatusFailed:
                    return "Failed";
                default:
                    return "";
            }
        }

        public SoaDataBo()
        {
            ErrorObject = new ExpandoObject();
            ErrorDictionary = (IDictionary<string, object>)ErrorObject;

            MappingStatus = MappingStatusPending;
            MappingValidate = true;
        }

        public static List<Column> GetColumns()
        {
            var columns = new List<Column> { };

            // add all standard fields
            for (int i = 1; i <= StandardSoaDataOutputBo.TypeMax; i++)
            {
                int colIndex = i;
                // Skip TotalCommission
                if (i == StandardSoaDataOutputBo.TypeTotalCommission)
                    continue;
                if (i > StandardSoaDataOutputBo.TypeTotalCommission)
                    colIndex -= 1;
                // Skip CompanyName
                if (i == StandardSoaDataOutputBo.TypeCompanyName)
                    continue;
                if (i > StandardSoaDataOutputBo.TypeCompanyName)
                    colIndex -= 1;
                // Skip BusinessCode
                if (i == StandardSoaDataOutputBo.TypeBusinessCode)
                    continue;
                if (i > StandardSoaDataOutputBo.TypeBusinessCode)
                    colIndex -= 1;
                // Skip GrossPremium
                if (i == StandardSoaDataOutputBo.TypeGrossPremium)
                    continue;
                if (i > StandardSoaDataOutputBo.TypeGrossPremium)
                    colIndex -= 1;
                // Skip NetTotalAmount
                if (i == StandardSoaDataOutputBo.TypeNetTotalAmount)
                    continue;
                if (i > StandardSoaDataOutputBo.TypeNetTotalAmount)
                    colIndex -= 1;

                columns.Add(new Column
                {
                    Type = i,
                    Header = StandardSoaDataOutputBo.GetCodeByType(i),
                    Property = StandardSoaDataOutputBo.GetPropertyNameByType(i),
                    ColIndex = colIndex,
                });
            }

            return columns;
        }

        public double? GetTotalCommission()
        {
            TotalCommission = DatabaseCommission.GetValueOrDefault()
                + AdministrationContribution.GetValueOrDefault() 
                - ShareOfRiCommissionFromCompulsoryCession.GetValueOrDefault()
                + CreditCardCharges.GetValueOrDefault()
                + BrokerageFee.GetValueOrDefault();
            if (Equals(double.NaN, TotalCommission)) TotalCommission = 0;
            return TotalCommission;
        }

        public void SetError(string property, dynamic value)
        {
            ErrorDictionary[property] = value;
            Errors = JsonConvert.SerializeObject(ErrorObject);
        }

        public bool SetDate(string property, object value)
        {
            var date = DateTime.Parse(value.ToString());
            this.SetPropertyValue(property, date);
            return true;
        }

        public bool SetString(string property, object value)
        {
            string output = value?.ToString();
            if (output != null)
            {
                int length = output.Length;
                var maxLengthAttr = this.GetAttributeFrom<MaxLengthAttribute>(property);
                if (maxLengthAttr != null && length > 0 && length > maxLengthAttr.Length)
                {
                    throw new Exception(string.Format(MessageBag.MaxLength, length, maxLengthAttr.Length));
                }
            }
            this.SetPropertyValue(property, output);
            return true;
        }

        public bool SetAmount(string property, object value)
        {
            string s = value.ToString();
            if (Util.IsValidDouble(s, out double? output, out string error, true))
                this.SetPropertyValue(property, output);
            else
                throw new Exception(error);
            return true;
        }

        public bool SetPercentange(string property, object value)
        {
            string s = value.ToString().Replace('%', '\0');
            if (Util.IsValidDouble(s, out double? output, out string error, true))
                this.SetPropertyValue(property, output);
            else
                throw new Exception(error);
            return true;
        }

        public bool SetInteger(string property, object value)
        {
            string s = value.ToString();
            this.SetPropertyValue(property, int.Parse(s));
            return true;
        }

        public bool SetDropDown(string property, object value)
        {
            string s = value?.ToString();
            if (s != null)
            {
                int length = s.Length;
                var maxLengthAttr = this.GetAttributeFrom<MaxLengthAttribute>(property);
                if (maxLengthAttr != null && length > 0 && length > maxLengthAttr.Length)
                {
                    throw new Exception(string.Format(MessageBag.MaxLength, length, maxLengthAttr.Length));
                }
            }
            this.SetPropertyValue(property, s);
            return true;
        }

        public bool ProcessSoaData(Column mappingCol, object value)
        {
            var success = true;
            var soaDataProperty = mappingCol.Property;
            var soaDataType = mappingCol.Type;
            var soaDataDataType = StandardSoaDataOutputBo.GetDataTypeByType(soaDataType);
            if (value is string valueStr)
                value = valueStr.Trim();
            switch (soaDataDataType)
            {
                // DropDown can be null or empty to do mapping with the PickListDetails
                case StandardOutputBo.DataTypeDropDown:
                    break;
                default:
                    if (value == null || value is string @string && string.IsNullOrEmpty(@string))
                    {
                        this.SetPropertyValue(soaDataProperty, null);
                        return success;
                    }
                    break;
            }

            try
            {
                switch (soaDataDataType)
                {
                    case StandardOutputBo.DataTypeDate:
                        SetDate(soaDataProperty, value);
                        break;
                    case StandardOutputBo.DataTypeString:
                        SetString(soaDataProperty, value);
                        break;
                    case StandardOutputBo.DataTypeAmount:
                        SetAmount(soaDataProperty, value);
                        break;
                    case StandardOutputBo.DataTypePercentage:
                        SetPercentange(soaDataProperty, value);
                        break;
                    case StandardOutputBo.DataTypeInteger:
                        SetInteger(soaDataProperty, value);
                        break;
                    case StandardOutputBo.DataTypeDropDown:
                        this.SetPropertyValue(soaDataProperty, value);
                        break;
                }
            }
            catch (Exception e)
            {
                success = false;
                MappingValidate = false;
                SetError(soaDataProperty, string.Format("Mapping Error: {0}", e.Message));
            }

            return success;
        }

        public double? GetGrossPremium()
        {
            GrossPremium = NbPremium.GetValueOrDefault() + RnPremium.GetValueOrDefault() + AltPremium.GetValueOrDefault();
            if (Equals(double.NaN, GrossPremium)) GrossPremium = 0;
            return GrossPremium;
        }

        public double? GetNetTotalAmount()
        {
            NetTotalAmount = GrossPremium.GetValueOrDefault() - TotalDiscount.GetValueOrDefault() + RiskPremium.GetValueOrDefault() 
                - NoClaimBonus.GetValueOrDefault() - Levy.GetValueOrDefault() - Claim.GetValueOrDefault() - ProfitComm.GetValueOrDefault() 
                - SurrenderValue.GetValueOrDefault() + Gst.GetValueOrDefault() + ModcoReserveIncome.GetValueOrDefault() - RiDeposit.GetValueOrDefault() 
                - DatabaseCommission.GetValueOrDefault() - AdministrationContribution.GetValueOrDefault() + ShareOfRiCommissionFromCompulsoryCession.GetValueOrDefault() 
                + RecaptureFee.GetValueOrDefault() - CreditCardCharges.GetValueOrDefault() - BrokerageFee.GetValueOrDefault();
            if (Equals(double.NaN, NetTotalAmount)) NetTotalAmount = 0;
            return NetTotalAmount;
        }

        public double? GetSummaryNetTotalAmount()
        {
            //Only for SOA Validation, calculate based on the listing
            if (BusinessCode == PickListDetailBo.BusinessOriginCodeServiceFee)
            {
                NetTotalAmount = NbPremium.GetValueOrDefault() 
                    + RnPremium.GetValueOrDefault() 
                    - AltPremium.GetValueOrDefault() 
                    - TotalDiscount.GetValueOrDefault() 
                    - Claim.GetValueOrDefault();
            }
            else if (BusinessCode != PickListDetailBo.BusinessOriginCodeServiceFee)
            {
                NetTotalAmount = GrossPremium.GetValueOrDefault() 
                    - (TotalDiscount.GetValueOrDefault() 
                    + NoClaimBonus.GetValueOrDefault() 
                    + Claim.GetValueOrDefault()
                    + SurrenderValue.GetValueOrDefault()) 
                    + Gst.GetValueOrDefault();
            }
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
            }

            return columns;
        }
    }
}
public class TargetPlanningReportOutput
{
    public string CedingCompany { get; set; }

    public string TreatyId { get; set; }

    public string PersonInCharge { get; set; }

    public string TreatyCode { get; set; }

    public List<string> OutputData { get; set; }
}