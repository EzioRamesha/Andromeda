using Shared;
using Shared.ProcessFile;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Retrocession
{
    public class PerLifeAggregationMonthlyDataBo
    {
        public int Id { get; set; }

        public int PerLifeAggregationDetailDataId { get; set; }

        public PerLifeAggregationDetailDataBo PerLifeAggregationDetailDataBo { get; set; }

        public int RiskYear { get; set; }

        public int RiskMonth { get; set; }

        public string UniqueKeyPerLife { get; set; }

        public string RetroPremFreq { get; set; }

        public double Aar { get; set; }

        public double? SumOfAar { get; set; }

        public double NetPremium { get; set; }

        public double? SumOfNetPremium { get; set; }

        public double RetroRatio { get; set; }

        public double? RetentionLimit { get; set; }

        public double? DistributedRetentionLimit { get; set; }

        public double? RetroAmount { get; set; }

        public double? DistributedRetroAmount { get; set; }

        public double? AccumulativeRetainAmount { get; set; }

        public double? RetroGrossPremium { get; set; }

        public double? RetroNetPremium { get; set; }

        public double? RetroDiscount { get; set; }

        public bool RetroIndicator { get; set; }

        public string Errors { get; set; }

        public string RetroBenefitCode { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? UpdatedById { get; set; }

        public List<PerLifeAggregationMonthlyRetroDataBo> PerLifeAggregationMonthlyRetroDataBos { get; set; }

        // RI Data Warehouse History
        public int RiDataWarehouseHistoryId { get; set; }

        public string Quarter { get; set; }

        public int? EndingPolicyStatus { get; set; }

        public int RecordType { get; set; }

        public string TreatyCode { get; set; }

        public string ReinsBasisCode { get; set; }

        public string FundsAccountingTypeCode { get; set; }

        public string PremiumFrequencyCode { get; set; }

        public int? ReportPeriodMonth { get; set; }

        public int? ReportPeriodYear { get; set; }

        public int? RiskPeriodMonth { get; set; }

        public int? RiskPeriodYear { get; set; }

        public string TransactionTypeCode { get; set; }

        public string PolicyNumber { get; set; }

        public DateTime? IssueDatePol { get; set; }

        public DateTime? IssueDateBen { get; set; }

        public DateTime? ReinsEffDatePol { get; set; }

        public DateTime? ReinsEffDateBen { get; set; }

        public string CedingPlanCode { get; set; }

        public string CedingBenefitTypeCode { get; set; }

        public string CedingBenefitRiskCode { get; set; }

        public string MlreBenefitCode { get; set; }

        public double? OriSumAssured { get; set; }

        public double? CurrSumAssured { get; set; }

        public double? AmountCededB4MlreShare { get; set; }

        public double? RetentionAmount { get; set; }

        public double? AarOri { get; set; }

        //public double? Aar { get; set; }

        public double? AarSpecial1 { get; set; }

        public double? AarSpecial2 { get; set; }

        public double? AarSpecial3 { get; set; }

        public string InsuredName { get; set; }

        public string InsuredGenderCode { get; set; }

        public string InsuredTobaccoUse { get; set; }

        public DateTime? InsuredDateOfBirth { get; set; }

        public string InsuredOccupationCode { get; set; }

        public string InsuredRegisterNo { get; set; }

        public int? InsuredAttainedAge { get; set; }

        public string InsuredNewIcNumber { get; set; }

        public string InsuredOldIcNumber { get; set; }

        public string InsuredName2nd { get; set; }

        public string InsuredGenderCode2nd { get; set; }

        public string InsuredTobaccoUse2nd { get; set; }

        public DateTime? InsuredDateOfBirth2nd { get; set; }

        public int? InsuredAttainedAge2nd { get; set; }

        public string InsuredNewIcNumber2nd { get; set; }

        public string InsuredOldIcNumber2nd { get; set; }

        public int? ReinsuranceIssueAge { get; set; }

        public int? ReinsuranceIssueAge2nd { get; set; }

        public double? PolicyTerm { get; set; }

        public DateTime? PolicyExpiryDate { get; set; }

        public double? DurationYear { get; set; }

        public double? DurationDay { get; set; }

        public double? DurationMonth { get; set; }

        public string PremiumCalType { get; set; }

        public double? CedantRiRate { get; set; }

        public string RateTable { get; set; }

        public int? AgeRatedUp { get; set; }

        public double? DiscountRate { get; set; }

        public string LoadingType { get; set; }

        public double? UnderwriterRating { get; set; }

        public double? UnderwriterRatingUnit { get; set; }

        public int? UnderwriterRatingTerm { get; set; }

        public double? UnderwriterRating2 { get; set; }

        public double? UnderwriterRatingUnit2 { get; set; }

        public int? UnderwriterRatingTerm2 { get; set; }

        public double? UnderwriterRating3 { get; set; }

        public double? UnderwriterRatingUnit3 { get; set; }

        public int? UnderwriterRatingTerm3 { get; set; }

        public double? FlatExtraAmount { get; set; }

        public double? FlatExtraUnit { get; set; }

        public int? FlatExtraTerm { get; set; }

        public double? FlatExtraAmount2 { get; set; }

        public int? FlatExtraTerm2 { get; set; }

        public double? StandardPremium { get; set; }

        public double? SubstandardPremium { get; set; }

        public double? FlatExtraPremium { get; set; }

        public double? GrossPremium { get; set; }

        public double? StandardDiscount { get; set; }

        public double? SubstandardDiscount { get; set; }

        public double? VitalityDiscount { get; set; }

        public double? TotalDiscount { get; set; }

        //public double? NetPremium { get; set; }

        public double? AnnualRiPrem { get; set; }

        public double? RiCovPeriod { get; set; }

        public DateTime? AdjBeginDate { get; set; }

        public DateTime? AdjEndDate { get; set; }

        public string PolicyNumberOld { get; set; }

        public string PolicyStatusCode { get; set; }

        public double? PolicyGrossPremium { get; set; }

        public double? PolicyStandardPremium { get; set; }

        public double? PolicySubstandardPremium { get; set; }

        public double? PolicyTermRemain { get; set; }

        public double? PolicyAmountDeath { get; set; }

        public double? PolicyReserve { get; set; }

        public string PolicyPaymentMethod { get; set; }

        public int? PolicyLifeNumber { get; set; }

        public string FundCode { get; set; }

        public string LineOfBusiness { get; set; }

        public double? ApLoading { get; set; }

        public double? LoanInterestRate { get; set; }

        public int? DefermentPeriod { get; set; }

        public int? RiderNumber { get; set; }

        public string CampaignCode { get; set; }

        public string Nationality { get; set; }

        public string TerritoryOfIssueCode { get; set; }

        public string CurrencyCode { get; set; }

        public string StaffPlanIndicator { get; set; }

        public string CedingTreatyCode { get; set; }

        public string CedingPlanCodeOld { get; set; }

        public string CedingBasicPlanCode { get; set; }

        public double? CedantSar { get; set; }

        public string CedantReinsurerCode { get; set; }

        public double? AmountCededB4MlreShare2 { get; set; }

        public string CessionCode { get; set; }

        public string CedantRemark { get; set; }

        public string GroupPolicyNumber { get; set; }

        public string GroupPolicyName { get; set; }

        public int? NoOfEmployee { get; set; }

        public int? PolicyTotalLive { get; set; }

        public string GroupSubsidiaryName { get; set; }

        public string GroupSubsidiaryNo { get; set; }

        public double? GroupEmployeeBasicSalary { get; set; }

        public string GroupEmployeeJobType { get; set; }

        public string GroupEmployeeJobCode { get; set; }

        public double? GroupEmployeeBasicSalaryRevise { get; set; }

        public double? GroupEmployeeBasicSalaryMultiplier { get; set; }

        public string CedingPlanCode2 { get; set; }

        public string DependantIndicator { get; set; }

        public int? GhsRoomBoard { get; set; }

        public double? PolicyAmountSubstandard { get; set; }

        public double? Layer1RiShare { get; set; }

        public int? Layer1InsuredAttainedAge { get; set; }

        public int? Layer1InsuredAttainedAge2nd { get; set; }

        public double? Layer1StandardPremium { get; set; }

        public double? Layer1SubstandardPremium { get; set; }

        public double? Layer1GrossPremium { get; set; }

        public double? Layer1StandardDiscount { get; set; }

        public double? Layer1SubstandardDiscount { get; set; }

        public double? Layer1TotalDiscount { get; set; }

        public double? Layer1NetPremium { get; set; }

        public double? Layer1GrossPremiumAlt { get; set; }

        public double? Layer1TotalDiscountAlt { get; set; }

        public double? Layer1NetPremiumAlt { get; set; }

        public string SpecialIndicator1 { get; set; }

        public string SpecialIndicator2 { get; set; }

        public string SpecialIndicator3 { get; set; }

        public string IndicatorJointLife { get; set; }

        public double? TaxAmount { get; set; }

        public string GstIndicator { get; set; }

        public double? GstGrossPremium { get; set; }

        public double? GstTotalDiscount { get; set; }

        public double? GstVitality { get; set; }

        public double? GstAmount { get; set; }

        public string Mfrs17BasicRider { get; set; }

        public string Mfrs17CellName { get; set; }

        public string Mfrs17TreatyCode { get; set; }

        public string LoaCode { get; set; }

        public double? CurrencyRate { get; set; }

        public double? NoClaimBonus { get; set; }

        public double? SurrenderValue { get; set; }

        public double? DatabaseCommision { get; set; }

        public double? GrossPremiumAlt { get; set; }

        public double? NetPremiumAlt { get; set; }

        public double? Layer1FlatExtraPremium { get; set; }

        public double? TransactionPremium { get; set; }

        public double? OriginalPremium { get; set; }

        public double? TransactionDiscount { get; set; }

        public double? OriginalDiscount { get; set; }

        public double? BrokerageFee { get; set; }

        public double? MaxUwRating { get; set; }

        public double? RetentionCap { get; set; }

        public double? AarCap { get; set; }

        public double? RiRate { get; set; }

        public double? RiRate2 { get; set; }

        public double? AnnuityFactor { get; set; }

        public double? SumAssuredOffered { get; set; }

        public double? UwRatingOffered { get; set; }

        public double? FlatExtraAmountOffered { get; set; }

        public double? FlatExtraDuration { get; set; }

        public DateTime? EffectiveDate { get; set; }

        public DateTime? OfferLetterSentDate { get; set; }

        public DateTime? RiskPeriodStartDate { get; set; }

        public DateTime? RiskPeriodEndDate { get; set; }

        public int? Mfrs17AnnualCohort { get; set; }

        public int? MaxExpiryAge { get; set; }

        public int? MinIssueAge { get; set; }

        public int? MaxIssueAge { get; set; }

        public double? MinAar { get; set; }

        public double? MaxAar { get; set; }

        public double? CorridorLimit { get; set; }

        public double? Abl { get; set; }

        public int? RatePerBasisUnit { get; set; }

        public double? RiDiscountRate { get; set; }

        public double? LargeSaDiscount { get; set; }

        public double? GroupSizeDiscount { get; set; }

        public int? EwarpNumber { get; set; }

        public string EwarpActionCode { get; set; }

        public double? RetentionShare { get; set; }

        public double? AarShare { get; set; }

        public string ProfitComm { get; set; }

        public double? TotalDirectRetroAar { get; set; }

        public double? TotalDirectRetroGrossPremium { get; set; }

        public double? TotalDirectRetroDiscount { get; set; }

        public double? TotalDirectRetroNetPremium { get; set; }

        public string TreatyType { get; set; }

        public double? MaxApLoading { get; set; }

        public int? MlreInsuredAttainedAgeAtCurrentMonth { get; set; }

        public int? MlreInsuredAttainedAgeAtPreviousMonth { get; set; }

        public bool? InsuredAttainedAgeCheck { get; set; }

        public bool? MaxExpiryAgeCheck { get; set; }

        public int? MlrePolicyIssueAge { get; set; }

        public bool? PolicyIssueAgeCheck { get; set; }

        public bool? MinIssueAgeCheck { get; set; }

        public bool? MaxIssueAgeCheck { get; set; }

        public bool? MaxUwRatingCheck { get; set; }

        public bool? ApLoadingCheck { get; set; }

        public bool? EffectiveDateCheck { get; set; }

        public bool? MinAarCheck { get; set; }

        public bool? MaxAarCheck { get; set; }

        public bool? CorridorLimitCheck { get; set; }

        public bool? AblCheck { get; set; }

        public bool? RetentionCheck { get; set; }

        public bool? AarCheck { get; set; }

        public double? MlreStandardPremium { get; set; }

        public double? MlreSubstandardPremium { get; set; }

        public double? MlreFlatExtraPremium { get; set; }

        public double? MlreGrossPremium { get; set; }

        public double? MlreStandardDiscount { get; set; }

        public double? MlreSubstandardDiscount { get; set; }

        public double? MlreLargeSaDiscount { get; set; }

        public double? MlreGroupSizeDiscount { get; set; }

        public double? MlreVitalityDiscount { get; set; }

        public double? MlreTotalDiscount { get; set; }

        public double? MlreNetPremium { get; set; }

        public double? NetPremiumCheck { get; set; }

        public double? ServiceFeePercentage { get; set; }

        public double? ServiceFee { get; set; }

        public double? MlreBrokerageFee { get; set; }

        public double? MlreDatabaseCommission { get; set; }

        public bool? ValidityDayCheck { get; set; }

        public bool? SumAssuredOfferedCheck { get; set; }

        public bool? UwRatingCheck { get; set; }

        public bool? FlatExtraAmountCheck { get; set; }

        public bool? FlatExtraDurationCheck { get; set; }

        public DateTime LastUpdatedDate { get; set; }

        public string RetroParty1 { get; set; }

        public string RetroParty2 { get; set; }

        public string RetroParty3 { get; set; }

        public double? RetroShare1 { get; set; }

        public double? RetroShare2 { get; set; }

        public double? RetroShare3 { get; set; }

        public double? RetroAar1 { get; set; }

        public double? RetroAar2 { get; set; }

        public double? RetroAar3 { get; set; }

        public double? RetroReinsurancePremium1 { get; set; }

        public double? RetroReinsurancePremium2 { get; set; }

        public double? RetroReinsurancePremium3 { get; set; }

        public double? RetroDiscount1 { get; set; }

        public double? RetroDiscount2 { get; set; }

        public double? RetroDiscount3 { get; set; }

        public double? RetroNetPremium1 { get; set; }

        public double? RetroNetPremium2 { get; set; }

        public double? RetroNetPremium3 { get; set; }

        public double? AarShare2 { get; set; }

        public double? AarCap2 { get; set; }

        public double? WakalahFeePercentage { get; set; }

        public string TreatyNumber { get; set; }

        public static List<Column> GetColumns()
        {
            return new List<Column>
            {
                new Column
                {
                    Header = "ID",
                    Property = "Id",
                },
                new Column
                {
                    Header = "Treaty Code",
                    Property = "TreatyCode",
                },
                new Column
                {
                    Header = "Risk Year",
                    Property = "RiskYear",
                },
                new Column
                {
                    Header = "Risk Month",
                    Property = "RiskMonth",
                },
                new Column
                {
                    Header = "Unique Key Per Life",
                    Property = "UniqueKeyPerLife",
                },
                new Column
                {
                    Header = "Retro Prem Freq",
                    Property = "RetroPremFreq",
                },
                new Column
                {
                    Header = "AAR",
                    Property = "Aar",
                },
                new Column
                {
                    Header = "Sum of Aar",
                    Property = "SumOfAar",
                },
                new Column
                {
                    Header = "Net Premium",
                    Property = "NetPremium",
                },
                new Column
                {
                    Header = "Sum of Net Premium",
                    Property = "SumOfNetPremium",
                },
                new Column
                {
                    Header = "Retention Limit",
                    Property = "RetentionLimit",
                },
                new Column
                {
                    Header = "Distributed Retention Limit",
                    Property = "DistributedRetentionLimit",
                },
                new Column
                {
                    Header = "Retro Amount",
                    Property = "RetroAmount",
                },
                new Column
                {
                    Header = "Distributed Retro Amount",
                    Property = "DistributedRetroAmount",
                },
                new Column
                {
                    Header = "Accumulative Retain Amount",
                    Property = "AccumulativeRetainAmount",
                },
                new Column
                {
                    Header = "Retro Gross Premium",
                    Property = "RetroGrossPremium",
                },
                new Column
                {
                    Header = "Retro Net Premium",
                    Property = "RetroNetPremium",
                },
                new Column
                {
                    Header = "Retro Discount",
                    Property = "RetroDiscount",
                },
                new Column
                {
                    Header = "Retro Indicator",
                    Property = "RetroIndicator",
                },
                new Column
                {
                    Header = "Errors",
                    Property = "Errors",
                },
                new Column
                {
                    Header = "Retro Benefit Code",
                    Property = "RetroBenefitCode",
                },
    };
        }

        public static List<Column> GetColumnsWithStandardRetroOutput()
        {
            var columns = new List<Column>
            {
                new Column
                {
                    Header = "ID",
                    Property = "Id",
                },
            };

            for (int i = 1; i <= StandardRetroOutputBo.TypeMax; i++)
            {
                columns.Add(new Column
                {
                    Header = StandardRetroOutputBo.GetTypeName(i),
                    Property = i == StandardRetroOutputBo.TypeMfrs17ContractCode ? "Mfrs17TreatyCode" : StandardRetroOutputBo.GetPropertyNameByType(i),
                });
            }

            return columns;
        }

        public static List<Column> GetRetroRiDataColumns()
        {
            return new List<Column>
            {
                new Column
                {
                    Header = "ID",
                    Property = "Id",
                },
                new Column
                {
                    Header = "Treaty Code",
                    Property = "TreatyCode",
                },
                new Column
                {
                    Header = "Risk Year",
                    Property = "RiskYear",
                },
                new Column
                {
                    Header = "Risk Month",
                    Property = "RiskMonth",
                },
                new Column
                {
                    Header = "Insured Gender Code",
                    Property = "InsuredGenderCode",
                },
                new Column
                {
                    Header = "Insured Date of Birth",
                    Property = "InsuredDateOfBirth",
                },
                new Column
                {
                    Header = "Policy Number",
                    Property = "PolicyNumber",
                },
                new Column
                {
                    Header = "Reins Eff Date Pol",
                    Property = "ReinsEffDatePol",
                },
                new Column
                {
                    Header = "AAR",
                    Property = "Aar",
                },
                new Column
                {
                    Header = "Gross Premium",
                    Property = "GrossPremium",
                },
                new Column
                {
                    Header = "Net Premium",
                    Property = "NetPremium",
                },
                new Column
                {
                    Header = "Premium Frequency Code",
                    Property = "PremiumFrequencyCode",
                },
                new Column
                {
                    Header = "Retro Premium Frequency Code",
                    Property = "RetroPremFreq",
                },
                new Column
                {
                    Header = "Ceding Plan Code",
                    Property = "CedingPlanCode",
                },
                new Column
                {
                    Header = "Ceding Benefit Type Code",
                    Property = "CedingBenefitTypeCode",
                },
                new Column
                {
                    Header = "Ceding Benefit Risk Code",
                    Property = "CedingBenefitRiskCode",
                },
                new Column
                {
                    Header = "MLRe Benefit Code",
                    Property = "MlreBenefitCode",
                },
                new Column
                {
                    Header = "Retro Benefit Code",
                    Property = "RetroBenefitCode",
                },
            };
        }

        public static List<Column> GetRetentionPremiumColumns(List<string> retroParties = null)
        {
            var columns = new List<Column>
            {
                new Column
                {
                    Header = "ID",
                    Property = "Id",
                },
                new Column
                {
                    Header = "Filter Key",
                    Property = "UniqueKeyPerLife",
                },
                new Column
                {
                    Header = "Policy Number",
                    Property = "PolicyNumber",
                },
                new Column
                {
                    Header = "Insured Gender Code",
                    Property = "InsuredGenderCode",
                },
                new Column
                {
                    Header = "Mlre Benefit Code",
                    Property = "MlreBenefitCode",
                },
                new Column
                {
                    Header = "Retro Benefit Code",
                    Property = "RetroBenefitCode",
                },
                new Column
                {
                    Header = "Territory Of Issue Code",
                    Property = "TerritoryOfIssueCode",
                },
                new Column
                {
                    Header = "Currency Code",
                    Property = "CurrencyCode",
                },
                new Column
                {
                    Header = "Insured Tobacco Use",
                    Property = "InsuredTobaccoUse",
                },
                new Column
                {
                    Header = "Reinsurance Issue Age",
                    Property = "ReinsuranceIssueAge",
                },
                new Column
                {
                    Header = "Reinsurance Effective Date",
                    Property = "ReinsEffDatePol",
                },
                new Column
                {
                    Header = "Underwriter Rating",
                    Property = "UnderwriterRating",
                },
                new Column
                {
                    Header = "EFC Prem Freq",
                    Property = "RetroPremFreq",
                },
                new Column
                {
                    Header = "Sum of Net Premium",
                    Property = "SumOfNetPremium",
                },
                new Column
                {
                    Header = "Net Premium",
                    Property = "NetPremium",
                },
                new Column
                {
                    Header = "Sum of AAR",
                    Property = "SumOfAar",
                },
                new Column
                {
                    Header = "AAR",
                    Property = "Aar",
                },
                new Column
                {
                    Header = "Retention Limit",
                    Property = "RetentionLimit",
                },
                new Column
                {
                    Header = "Distributed Retention Limit",
                    Property = "DistributedRetentionLimit",
                },
                new Column
                {
                    Header = "Retro Amount",
                    Property = "RetroAmount",
                },
                new Column
                {
                    Header = "Distributed Retro Amount",
                    Property = "DistributedRetroAmount",
                },
                new Column
                {
                    Header = "Accumulative Retain Amount",
                    Property = "AccumulativeRetainAmount",
                },
                new Column
                {
                    Header = "Errors",
                    Property = "Errors",
                },
            };
            
            // Retro Gross Premium
            foreach (var party in retroParties)
            {
                columns.Add(new Column
                {
                    Header = string.Format("{0} - {1}", party, "Gross Premium"),
                    Property = string.Format("{0}_{1}", party, "GrossPremium"),
                });
            }

            // Retro Net Premium
            foreach (var party in retroParties)
            {
                columns.Add(new Column
                {
                    Header = string.Format("{0} - {1}", party, "Net Premium"),
                    Property = string.Format("{0}_{1}", party, "NetPremium"),
                });
            }

            // Retro Discount
            foreach (var party in retroParties)
            {
                columns.Add(new Column
                {
                    Header = string.Format("{0} - {1}", party, "Retro Discount"),
                    Property = string.Format("{0}_{1}", party, "RetroDiscount"),
                });
            }

            return columns;
        }

        public bool SetData(int datatype, string property, object value)
        {
            if (value is string valueStr)
                value = valueStr.Trim();
            switch (datatype)
            {
                // DropDown can be null or empty to do mapping with the PickListDetails
                case StandardOutputBo.DataTypeDropDown:
                    break;
                default:
                    if (value == null || value is string @string && string.IsNullOrEmpty(@string))
                    {
                        this.SetPropertyValue(property, null);
                        return true;
                    }
                    break;
            }

            switch (datatype)
            {
                case StandardOutputBo.DataTypeDate:
                    return SetDate(property, value);
                case StandardOutputBo.DataTypeString:
                    return SetString(property, value);
                case StandardOutputBo.DataTypeAmount:
                    return SetAmount(property, value);
                case StandardOutputBo.DataTypePercentage:
                    return SetPercentange(property, value);
                case StandardOutputBo.DataTypeInteger:
                    return SetInteger(property, value);
                case StandardOutputBo.DataTypeDropDown:
                    return SetDropDown(property, value);
                case StandardOutputBo.DataTypeBoolean:
                    return SetBoolean(property, value);
            }
            return false;
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

        public bool SetBoolean(string property, object value)
        {
            string s = value?.ToString();
            bool? bl;
            if (bool.TryParse(s, out bool b))
            {
                bl = b;
            }
            else
            {
                bl = null;
            }
            this.SetPropertyValue(property, bl);
            return true;
        }
    }
}
