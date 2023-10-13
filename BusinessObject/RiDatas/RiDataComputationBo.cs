using Newtonsoft.Json;
using Shared;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BusinessObject.RiDatas
{
    public class RiDataComputationBo
    {
        [JsonIgnore]
        public int Id { get; set; }

        [JsonIgnore]
        public int RiDataConfigId { get; set; }

        [JsonIgnore]
        public RiDataConfigBo RiDataConfigBo { get; set; }

        public int Step { get; set; }

        public int SortIndex { get; set; }

        [MaxLength(128)]
        public string Description { get; set; }

        [MaxLength(512)]
        public string Condition { get; set; }

        [JsonIgnore]
        public int? StandardOutputId { get; set; }
        public string StandardOutputCode { get; set; }

        [JsonIgnore]
        public StandardOutputBo StandardOutputBo { get; set; }
        [JsonIgnore]
        public StandardOutputEval StandardOutputEval { get; set; }

        public int Mode { get; set; }

        [MaxLength(512)]
        public string CalculationFormula { get; set; }

        [JsonIgnore]
        public int CreatedById { get; set; }

        [JsonIgnore]
        public int? UpdatedById { get; set; }

        public const int ModeFormula = 1;
        public const int ModeTable = 2;
        public const int ModeRiskDate = 3;
        public const int MaxMode = 3;

        public const int TableBenefit = 1;
        public const int TableTreaty = 2;
        public const int TableCellBasicRider = 3;
        public const int TableCellCellName = 4;
        public const int TableRateTableCode = 5;
        public const int TableCellTreatyCode = 6;
        // Added in Phase 2
        public const int TableCellLoaCode = 7;
        public const int TableFeatureProfitComm = 8;
        public const int TableFeatureMaxAgeAtExpiry = 9;
        public const int TableFeatureMinIssueAge = 10;
        public const int TableFeatureMaxIssueAge = 11;
        public const int TableFeatureMaxUwRating = 12;
        public const int TableFeatureApLoading = 13;
        public const int TableFeatureMinAar = 14;
        public const int TableFeatureMaxAar = 15;
        public const int TableFeatureAblAmount = 16;
        public const int TableFeatureRetentionShare = 17;
        public const int TableFeatureRetentionCap = 18;
        public const int TableFeatureRiShare = 19;
        public const int TableFeatureRiShareCap = 20;
        public const int TableFeatureServiceFee = 21;
        public const int TableFeatureWakalahFee = 22;
        public const int TableRateTableRatePerBasis = 23;
        public const int TableRateTableRateByPreviousAge = 24;
        public const int TableRateTableRiDiscount = 25;
        public const int TableRateTableLargeDiscount = 26;
        public const int TableRateTableGroupDiscount = 27;
        public const int TableAnnuityFactor = 28;
        public const int TableFacEwarpNumber = 29;
        public const int TableFacEwarpActionCode = 30;
        public const int TableFacOfferLetterSentDate = 31;
        public const int TableFacSumAssuredOffered = 32;
        public const int TableFacUwRatingOffered = 33;
        public const int TableFacFlatExtraAmountOffered = 34;
        public const int TableFacFlatExtraDuration = 35;
        public const int TableFeatureRiShare2= 36;
        public const int TableFeatureRiShareCap2 = 37;
        public const int TableRateTableRateByCurrentAge = 38;
        public const int TableFeatureEffectiveDate = 39;

        public const int MaxTable = 39;

        // Added in Phase 2
        public const int RiskDateOption1StartDate = 1;
        public const int RiskDateOption1EndDate = 2;
        public const int RiskDateOption2StartDate = 3;
        public const int RiskDateOption2EndDate = 4;

        public const int MaxRiskDateOption = 4;

        public const int StepPreComputation1 = 1;
        public const int StepPreComputation2 = 2;
        public const int StepPostComputation = 3;
        public const int MaxStep = 3;

        //public const string StartingDelimiter = "{";
        //public const string EndingDelimiter = "}";

        public const string DisplayName = "Pre-Computation";

        public static string GetModeName(int key)
        {
            switch (key)
            {
                case ModeFormula:
                    return "Formula";
                case ModeTable:
                    return "Table";
                case ModeRiskDate:
                    return "Risk Date";
                default:
                    return "";
            }
        }

        public static string GetTableName(string key)
        {
            if (int.TryParse(key, out int k))
            {
                return GetTableName(k);
            }
            return "";
        }

        public static string GetTableName(int key)
        {
            switch (key)
            {
                case TableBenefit:
                    return "Benefit Code Mapping";
                case TableTreaty:
                    return "Treaty Code Mapping";
                case TableCellBasicRider:
                    return "Cell Mapping - Basic Rider";
                case TableCellCellName:
                    return "Cell Mapping - Cell Name";
                case TableRateTableCode:
                    return "Rate Table Mapping - Code";
                case TableCellTreatyCode:
                    return "Cell Mapping - MFRS17 Contract Code"; // Phase 1: MFRS17 Treaty Code
                case TableCellLoaCode:
                    return "Cell Mapping - LOA Code";
                case TableFeatureProfitComm:
                    return "Product Feature Mapping - Profit Commission";
                case TableFeatureMaxAgeAtExpiry:
                    return "Product Feature Mapping - Max Age at Expiry";
                case TableFeatureMinIssueAge:
                    return "Product Feature Mapping - Min Issue Age";
                case TableFeatureMaxIssueAge:
                    return "Product Feature Mapping - Max Issue Age";
                case TableFeatureMaxUwRating:
                    return "Product Feature Mapping - Max UW Rating";
                case TableFeatureApLoading:
                    return "Product Feature Mapping - AP Loading";
                case TableFeatureMinAar:
                    return "Product Feature Mapping - Min AAR";
                case TableFeatureMaxAar:
                    return "Product Feature Mapping - Max AAR";
                case TableFeatureAblAmount:
                    return "Product Feature Mapping - ABL Amount";
                case TableFeatureRetentionShare:
                    return "Product Feature Mapping - Retention Share";
                case TableFeatureRetentionCap:
                    return "Product Feature Mapping - Retention Cap";
                case TableFeatureRiShare:
                    return "Product Feature Mapping - RI Share 1";
                case TableFeatureRiShareCap:
                    return "Product Feature Mapping - RI Share Cap 1";
                case TableFeatureServiceFee:
                    return "Product Feature Mapping - Service Fee";
                case TableFeatureWakalahFee:
                    return "Product Feature Mapping - Wakalah Fee";
                case TableRateTableRatePerBasis:
                    return "Rate Table Mapping - Rate Per Basis";
                case TableRateTableRateByPreviousAge:
                    return "Rate Table Mapping - Rate (Previous Age)";
                case TableRateTableRiDiscount:
                    return "Rate Table Mapping - RI Discount";
                case TableRateTableLargeDiscount:
                    return "Rate Table Mapping - Large Discount";
                case TableRateTableGroupDiscount:
                    return "Rate Table Mapping - Group Discount";
                case TableAnnuityFactor:
                    return "Annuity Factor Mapping";
                case TableFacEwarpNumber:
                    return "FAC Mapping - eWarp Number";
                case TableFacEwarpActionCode:
                    return "FAC Mapping - eWarp Action Code";
                case TableFacOfferLetterSentDate:
                    return "FAC Mapping - Offer Letter Sent Date";
                case TableFacSumAssuredOffered:
                    return "FAC Mapping - Sum Assured Offered";
                case TableFacUwRatingOffered:
                    return "FAC Mapping - UW Rating Offered";
                case TableFacFlatExtraAmountOffered:
                    return "FAC Mapping - Flat Extra Amount Offered";
                case TableFacFlatExtraDuration:
                    return "FAC Mapping - Flat Extra Duration";
                case TableFeatureRiShare2:
                    return "Product Feature Mapping - RI Share 2";
                case TableFeatureRiShareCap2:
                    return "Product Feature Mapping - RI Share Cap 2";
                case TableRateTableRateByCurrentAge:
                    return "Rate Table Mapping - Rate (Current Age)";
                case TableFeatureEffectiveDate:
                    return "Product Feature Mapping - Effective Date";
                default:
                    return "";
            }
        }
        
        public static string GetStepName(int key)
        {
            switch (key)
            {
                case StepPreComputation1:
                    return "Pre-Computation 1";
                case StepPreComputation2:
                    return "Pre-Computation 2";
                case StepPostComputation:
                    return "Post-Computation";
                default:
                    return "";
            }
        }

        public static string GetRiskDateOptionName(int key)
        {
            switch (key)
            {
                case RiskDateOption1StartDate:
                    return "Option 1 - Effective Day of Month (Start Date)";
                case RiskDateOption1EndDate:
                    return "Option 1 - Effective Day of Month (End Date)";
                case RiskDateOption2StartDate:
                    return "Option 2 - 1st of Month (Start Date)";
                case RiskDateOption2EndDate:
                    return "Option 2 - 1st of Month (End Date)";
                default:
                    return "";
            }
        }

        public static List<RiDataComputationBo> GetDefault()
        {
            return new List<RiDataComputationBo>
            {
                //new RiDataComputationBo
                //{
                //    Step = StepPreComputation1,
                //    Description = "To update rate table",
                //    Condition = "true",
                //    StandardOutputId = StandardOutputBo.TypeRateTable,
                //    Mode = ModeTable,
                //    CalculationFormula = TableRateTableCode.ToString(),
                //},
                new RiDataComputationBo
                {
                    Step = StepPreComputation1
                },
                new RiDataComputationBo
                {
                    Step = StepPostComputation,
                    SortIndex = 1,
                    Description = "To update basic rider (valuation)",
                    Condition = "true",
                    StandardOutputId = StandardOutputBo.TypeMfrs17BasicRider,
                    Mode = ModeTable,
                    CalculationFormula = TableCellBasicRider.ToString(),
                },
                new RiDataComputationBo
                {
                    Step = StepPostComputation,
                    SortIndex = 2,
                    Description = "To update cell name (valuation)",
                    Condition = "true",
                    StandardOutputId = StandardOutputBo.TypeMfrs17CellName,
                    Mode = ModeTable,
                    CalculationFormula = TableCellCellName.ToString(),
                },
                new RiDataComputationBo
                {
                    Step = StepPostComputation,
                    SortIndex = 3,
                    Description = "To update MFRS17 contract code (valuation)", // Phase 1: MFRS17 treaty code
                    Condition = "true",
                    StandardOutputId = StandardOutputBo.TypeMfrs17TreatyCode,
                    Mode = ModeTable,
                    CalculationFormula = TableCellTreatyCode.ToString(),
                },
                new RiDataComputationBo
                {
                    Step = StepPostComputation,
                    SortIndex = 4,
                    Description = "To update LOA code (valuation)",
                    Condition = "true",
                    StandardOutputId = StandardOutputBo.TypeLoaCode,
                    Mode = ModeTable,
                    CalculationFormula = TableCellLoaCode.ToString(),
                },
                new RiDataComputationBo
                {
                    Step = StepPreComputation2,
                    SortIndex = 1,
                    Description = "Max expiry age",
                    Condition = "true",
                    StandardOutputId = StandardOutputBo.TypeMaxExpiryAge,
                    Mode = ModeTable,
                    CalculationFormula = TableFeatureMaxAgeAtExpiry.ToString(),
                },
                new RiDataComputationBo
                {
                    Step = StepPreComputation2,
                    SortIndex = 2,
                    Description = "Min Entry age",
                    Condition = "true",
                    StandardOutputId = StandardOutputBo.TypeMinIssueAge,
                    Mode = ModeTable,
                    CalculationFormula = TableFeatureMinIssueAge.ToString(),
                },
                new RiDataComputationBo
                {
                    Step = StepPreComputation2,
                    SortIndex = 3,
                    Description = "Max Entry age",
                    Condition = "true",
                    StandardOutputId = StandardOutputBo.TypeMaxIssueAge,
                    Mode = ModeTable,
                    CalculationFormula = TableFeatureMaxIssueAge.ToString(),
                },
                new RiDataComputationBo
                {
                    Step = StepPreComputation2,
                    SortIndex = 4,
                    Description = "Max Underwriting Rating",
                    Condition = "true",
                    StandardOutputId = StandardOutputBo.TypeMaxUwRating,
                    Mode = ModeTable,
                    CalculationFormula = TableFeatureMaxUwRating.ToString(),
                },
                new RiDataComputationBo
                {
                    Step = StepPreComputation2,
                    SortIndex = 5,
                    Description = "ABL",
                    Condition = "true",
                    StandardOutputId = StandardOutputBo.TypeAbl,
                    Mode = ModeTable,
                    CalculationFormula = TableFeatureAblAmount.ToString(),
                },
                new RiDataComputationBo
                {
                    Step = StepPreComputation2,
                    SortIndex = 6,
                    Description = "Retention Share",
                    Condition = "true",
                    StandardOutputId = StandardOutputBo.TypeRetentionShare,
                    Mode = ModeTable,
                    CalculationFormula = TableFeatureRetentionShare.ToString(),
                },
                new RiDataComputationBo
                {
                    Step = StepPreComputation2,
                    SortIndex = 7,
                    Description = "Retention Cap",
                    Condition = "true",
                    StandardOutputId = StandardOutputBo.TypeRetentionCap,
                    Mode = ModeTable,
                    CalculationFormula = TableFeatureRetentionCap.ToString(),
                },
                new RiDataComputationBo
                {
                    Step = StepPreComputation2,
                    SortIndex = 8,
                    Description = "AAR % QS",
                    Condition = "true",
                    StandardOutputId = StandardOutputBo.TypeAarShare,
                    Mode = ModeTable,
                    CalculationFormula = TableFeatureRiShare.ToString(),
                },
                new RiDataComputationBo
                {
                    Step = StepPreComputation2,
                    SortIndex = 9,
                    Description = "AAR Cap QS",
                    Condition = "true",
                    StandardOutputId = StandardOutputBo.TypeAarCap,
                    Mode = ModeTable,
                    CalculationFormula = TableFeatureRiShareCap.ToString(),
                },
                new RiDataComputationBo
                {
                    Step = StepPreComputation2,
                    SortIndex = 10,
                    Description = "AAR % Sp",
                    Condition = "true",
                    StandardOutputId = StandardOutputBo.TypeAarShare2,
                    Mode = ModeTable,
                    CalculationFormula = TableFeatureRiShare2.ToString(),
                },
                new RiDataComputationBo
                {
                    Step = StepPreComputation2,
                    SortIndex = 11,
                    Description = "AAR Cap Sp",
                    Condition = "true",
                    StandardOutputId = StandardOutputBo.TypeAarCap2,
                    Mode = ModeTable,
                    CalculationFormula = TableFeatureRiShareCap2.ToString(),
                },
                new RiDataComputationBo
                {
                    Step = StepPreComputation2,
                    SortIndex = 12,
                    Description = "PC entitlement",
                    Condition = "true",
                    StandardOutputId = StandardOutputBo.TypeProfitComm,
                    Mode = ModeTable,
                    CalculationFormula = TableFeatureProfitComm.ToString(),
                },
                new RiDataComputationBo
                {
                    Step = StepPreComputation2,
                    SortIndex = 13,
                    Description = "Update valuation rate table",
                    Condition = "true",
                    StandardOutputId = StandardOutputBo.TypeRateTable,
                    Mode = ModeTable,
                    CalculationFormula = TableRateTableCode.ToString(),
                },
                new RiDataComputationBo
                {
                    Step = StepPostComputation,
                    SortIndex = 5,
                    Description = "Min Issue OK",
                    Condition = "{REINSURANCE_ISSUE_AGE} >= {MIN_ISSUE_AGE}",
                    StandardOutputId = StandardOutputBo.TypeMinIssueAgeCheck,
                    Mode = ModeFormula,
                    CalculationFormula = "true",
                },
                new RiDataComputationBo
                {
                    Step = StepPostComputation,
                    SortIndex = 6,
                    Description = "Min Issue KO",
                    Condition = "{REINSURANCE_ISSUE_AGE} < {MIN_ISSUE_AGE}",
                    StandardOutputId = StandardOutputBo.TypeMinIssueAgeCheck,
                    Mode = ModeFormula,
                    CalculationFormula = "false",
                },
                new RiDataComputationBo
                {
                    Step = StepPostComputation,
                    SortIndex = 7,
                    Description = "Max Issue Age OK",
                    Condition = "{REINSURANCE_ISSUE_AGE} <= {MAX_ISSUE_AGE}",
                    StandardOutputId = StandardOutputBo.TypeMaxIssueAgeCheck,
                    Mode = ModeFormula,
                    CalculationFormula = "true",
                },
                new RiDataComputationBo
                {
                    Step = StepPostComputation,
                    SortIndex = 8,
                    Description = "Max Issue Age KO",
                    Condition = "{REINSURANCE_ISSUE_AGE} > {MAX_ISSUE_AGE}",
                    StandardOutputId = StandardOutputBo.TypeMaxIssueAgeCheck,
                    Mode = ModeFormula,
                    CalculationFormula = "false",
                },
                new RiDataComputationBo
                {
                    Step = StepPostComputation,
                    SortIndex = 9,
                    Description = "Max Expiry Age OK",
                    Condition = "{INSURED_ATTAINED_AGE} <= {MAX_EXPIRY_AGE}",
                    StandardOutputId = StandardOutputBo.TypeMaxExpiryAgeCheck,
                    Mode = ModeFormula,
                    CalculationFormula = "true",
                },
                new RiDataComputationBo
                {
                    Step = StepPostComputation,
                    SortIndex = 10,
                    Description = "Max Expiry Age KO",
                    Condition = "{INSURED_ATTAINED_AGE} > {MAX_EXPIRY_AGE}",
                    StandardOutputId = StandardOutputBo.TypeMaxExpiryAgeCheck,
                    Mode = ModeFormula,
                    CalculationFormula = "false",
                },
                new RiDataComputationBo
                {
                    Step = StepPostComputation,
                    SortIndex = 11,
                    Description = "Issue Age OK",
                    Condition = "({REINSURANCE_ISSUE_AGE} - {MLRE_POLICY_ISSUE_AGE}) <= 1 && ({REINSURANCE_ISSUE_AGE} - {MLRE_POLICY_ISSUE_AGE}) >= -1",
                    StandardOutputId = StandardOutputBo.TypePolicyIssueAgeCheck,
                    Mode = ModeFormula,
                    CalculationFormula = "true",
                },
                new RiDataComputationBo
                {
                    Step = StepPostComputation,
                    SortIndex = 12,
                    Description = "Issue Age KO",
                    Condition = "({REINSURANCE_ISSUE_AGE} - {MLRE_POLICY_ISSUE_AGE}) > 1 || ({REINSURANCE_ISSUE_AGE} - {MLRE_POLICY_ISSUE_AGE}) < -1",
                    StandardOutputId = StandardOutputBo.TypePolicyIssueAgeCheck,
                    Mode = ModeFormula,
                    CalculationFormula = "false",
                },
                new RiDataComputationBo
                {
                    Step = StepPostComputation,
                    SortIndex = 13,
                    Description = "Attained Age OK",
                    Condition = "({INSURED_ATTAINED_AGE} - {MLRE_INSURED_ATTAINED_AGE_AT_CURRENT_MONTH}) <= 1 && ({INSURED_ATTAINED_AGE} - {MLRE_INSURED_ATTAINED_AGE_AT_CURRENT_MONTH}) >= -1",
                    StandardOutputId = StandardOutputBo.TypeInsuredAttainedAgeCheck,
                    Mode = ModeFormula,
                    CalculationFormula = "true",
                },
                new RiDataComputationBo
                {
                    Step = StepPostComputation,
                    SortIndex = 14,
                    Description = "Attained Age KO",
                    Condition = "({INSURED_ATTAINED_AGE} - {MLRE_INSURED_ATTAINED_AGE_AT_CURRENT_MONTH}) > 1 || ({INSURED_ATTAINED_AGE} - {MLRE_INSURED_ATTAINED_AGE_AT_CURRENT_MONTH}) < -1",
                    StandardOutputId = StandardOutputBo.TypeInsuredAttainedAgeCheck,
                    Mode = ModeFormula,
                    CalculationFormula = "false",
                },
                new RiDataComputationBo
                {
                    Step = StepPostComputation,
                    SortIndex = 15,
                    Description = "Max UW rating OK",
                    Condition = "{UNDERWRITER_RATING} <= {MAX_UW_RATING}",
                    StandardOutputId = StandardOutputBo.TypeMaxUwRatingCheck,
                    Mode = ModeFormula,
                    CalculationFormula = "true",
                },
                new RiDataComputationBo
                {
                    Step = StepPostComputation,
                    SortIndex = 16,
                    Description = "Max UW rating KO",
                    Condition = "{UNDERWRITER_RATING} > {MAX_UW_RATING}",
                    StandardOutputId = StandardOutputBo.TypeMaxUwRatingCheck,
                    Mode = ModeFormula,
                    CalculationFormula = "false",
                },
                new RiDataComputationBo
                {
                    Step = StepPostComputation,
                    SortIndex = 17,
                    Description = "ABL OK",
                    Condition = "{ORI_SUM_ASSURED} <= {ABL}",
                    StandardOutputId = StandardOutputBo.TypeAblCheck,
                    Mode = ModeFormula,
                    CalculationFormula = "true",
                },
                new RiDataComputationBo
                {
                    Step = StepPostComputation,
                    SortIndex = 18,
                    Description = "ABL KO",
                    Condition = "{ORI_SUM_ASSURED} > {ABL}",
                    StandardOutputId = StandardOutputBo.TypeAblCheck,
                    Mode = ModeFormula,
                    CalculationFormula = "false",
                },
                new RiDataComputationBo
                {
                    Step = StepPostComputation,
                    SortIndex = 19,
                    Description = "To update MFRS17 annual cohort",
                    Condition = "true",
                    StandardOutputId = StandardOutputBo.TypeMfrs17AnnualCohort,
                    Mode = ModeFormula,
                    CalculationFormula = "{REINS_EFF_DATE_POL}.Year",
                },
            };
        }

        public List<string> Validate()
        {
            List<string> errors = new List<string> { };

            this.ValidateNullAndMaxLength("Description", "Description", ref errors);
            this.ValidateNullAndMaxLength("Condition", "Condition", ref errors);

            if (StandardOutputId == 0 || StandardOutputId == null)
                errors.Add(string.Format(MessageBag.Required, "Standard Output"));

            if (Mode == 0)
                errors.Add(string.Format(MessageBag.Required, "Mode"));

            
            if (Mode == ModeFormula)
            {
                this.ValidateNullAndMaxLength("CalculationFormula", "Calculation Formula", ref errors);
            }

            if ((Mode == ModeTable || Mode == ModeRiskDate) && (Util.GetParseInt(CalculationFormula) == null || Util.GetParseInt(CalculationFormula) == 0))
                errors.Add(string.Format(MessageBag.Required, "Calculation Formula"));

            return errors;
        }

        public int? GetTableKey()
        {
            if (int.TryParse(CalculationFormula, out int table))
                return table;
            return null;
        }

        public bool IsEmpty()
        {
            return Description == null && Condition == null && Mode == 0 && CalculationFormula == null && StandardOutputId == 0;
        }

        public StandardOutputBo GetStandardOutputBo()
        {
            if (StandardOutputBo == null)
                return null;

            StandardOutputBo = StandardOutputBo.GetByType(StandardOutputBo.Type);
            return StandardOutputBo;
        }

        public StandardOutputEval GetStandardOutputEval(RiDataBo riDataBo, string quarter)
        {
            StandardOutputEval = new StandardOutputEval
            {
                Condition = Condition,
                Formula = CalculationFormula,
                RiDataBo = riDataBo,
                Quarter = quarter,
            };
            return StandardOutputEval;
        }
    }
}
