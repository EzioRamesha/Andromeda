using Newtonsoft.Json;
using Shared;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BusinessObject.RiDatas
{
    public class RiDataPreValidationBo
    {
        [JsonIgnore]
        public int Id { get; set; }

        [JsonIgnore]
        public int RiDataConfigId { get; set; }

        [JsonIgnore]
        public RiDataConfigBo RiDataConfigBo { get; set; }

        public int Step { get; set; }

        [JsonIgnore]
        public int SortIndex { get; set; }

        [MaxLength(255)]
        public string Description { get; set; }

        [MaxLength(512)]
        public string Condition { get; set; }

        [JsonIgnore]
        public int CreatedById { get; set; }

        [JsonIgnore]
        public int? UpdatedById { get; set; }

        public const int StepPreValidation = 1;
        public const int StepPostValidation = 2;
        public const int MaxStep = 2;

        //public const string StartingDelimiter = "{";
        //public const string EndingDelimiter = "}";

        public const string DisplayName = "Pre-Validation";

        public static string GetStepName(int key)
        {
            switch (key)
            {
                case StepPreValidation:
                    return "Pre-Validation";
                case StepPostValidation:
                    return "Post-Validation";
                default:
                    return "";
            }
        }

        public List<string> Validate()
        {
            List<string> errors = new List<string> { };

            this.ValidateNullAndMaxLength("Description", "Description", ref errors);
            this.ValidateNullAndMaxLength("Condition", "Condition", ref errors);

            return errors;
        }

        public bool IsEmpty()
        {
            return Description == null && Condition == null;
        }

        public static List<RiDataPreValidationBo> GetDefault()
        {
            return new List<RiDataPreValidationBo>
            {
                new RiDataPreValidationBo { Step = StepPreValidation, Description = "TREATY_CODE is not null", Condition = "string.IsNullOrEmpty({TREATY_CODE})" },
                new RiDataPreValidationBo { Step = StepPreValidation, Description = "REINS_BASIS_CODE is not null", Condition = "string.IsNullOrEmpty({REINS_BASIS_CODE})" },
                new RiDataPreValidationBo { Step = StepPreValidation, Description = "FUNDS_ACCOUNTING_TYPE_CODE is not null", Condition = "string.IsNullOrEmpty({FUNDS_ACCOUNTING_TYPE_CODE})" },
                new RiDataPreValidationBo { Step = StepPreValidation, Description = "PREMIUM_FREQUENCY_CODE is not null", Condition = "string.IsNullOrEmpty({PREMIUM_FREQUENCY_CODE})" },
                new RiDataPreValidationBo { Step = StepPreValidation, Description = "REPORT_PERIOD_MONTH is not invalid number", Condition = "{REPORT_PERIOD_MONTH} <= 0 || {REPORT_PERIOD_MONTH} > 12" },
                new RiDataPreValidationBo { Step = StepPreValidation, Description = "REPORT_PERIOD_YEAR is not null", Condition = "{REPORT_PERIOD_YEAR} <= 0" },
                new RiDataPreValidationBo { Step = StepPreValidation, Description = "RISK_PERIOD_MONTH is not invalid number", Condition = "{RISK_PERIOD_MONTH} <= 0 || {RISK_PERIOD_MONTH} > 12" },
                new RiDataPreValidationBo { Step = StepPreValidation, Description = "RISK_PERIOD_YEAR is not null", Condition = "{RISK_PERIOD_YEAR} <= 0" },
                new RiDataPreValidationBo { Step = StepPreValidation, Description = "TRANSACTION_TYPE_CODE is not null", Condition = "string.IsNullOrEmpty({TRANSACTION_TYPE_CODE})" },
                new RiDataPreValidationBo { Step = StepPreValidation, Description = "POLICY_NUMBER is not null", Condition = "string.IsNullOrEmpty({POLICY_NUMBER})" },
                new RiDataPreValidationBo { Step = StepPreValidation, Description = "REINS_EFF_DATE_POL is not null", Condition = "{REINS_EFF_DATE_POL} == DateTime.MinValue || ({REINS_EFF_DATE_POL}).Date > DateTime.Now.Date" },
                new RiDataPreValidationBo { Step = StepPreValidation, Description = "CEDING_PLAN_CODE is not null", Condition = "string.IsNullOrEmpty({CEDING_PLAN_CODE})" },
                new RiDataPreValidationBo { Step = StepPreValidation, Description = "CEDING_BENEFIT_TYPE_CODE is not null", Condition = "string.IsNullOrEmpty({CEDING_BENEFIT_TYPE_CODE})" },
                new RiDataPreValidationBo { Step = StepPreValidation, Description = "CEDING_BENEFIT_RISK_CODE is not null", Condition = "string.IsNullOrEmpty({CEDING_BENEFIT_RISK_CODE})" },
                new RiDataPreValidationBo { Step = StepPreValidation, Description = "MLRE_BENEFIT_CODE is not null", Condition = "string.IsNullOrEmpty({MLRE_BENEFIT_CODE})" },
                new RiDataPreValidationBo { Step = StepPreValidation, Description = "CURR_SUM_ASSURED is not null", Condition = "{CURR_SUM_ASSURED} == null" },
                new RiDataPreValidationBo { Step = StepPreValidation, Description = "AAR is not null", Condition = "{AAR} == null" },
                new RiDataPreValidationBo { Step = StepPreValidation, Description = "INSURED_GENDER_CODE is not null", Condition = "string.IsNullOrEmpty({INSURED_GENDER_CODE})" },
                new RiDataPreValidationBo { Step = StepPreValidation, Description = "INSURED_TOBACCO_USE is not null", Condition = "string.IsNullOrEmpty({INSURED_TOBACCO_USE})" },
                new RiDataPreValidationBo { Step = StepPreValidation, Description = "INSURED_DATE_OF_BIRTH is not null", Condition = "{INSURED_DATE_OF_BIRTH} == DateTime.MinValue || ({INSURED_DATE_OF_BIRTH}).Date > DateTime.Now.Date" },
                new RiDataPreValidationBo { Step = StepPreValidation, Description = "INSURED_OCCUPATION_CODE is not null", Condition = "string.IsNullOrEmpty({INSURED_OCCUPATION_CODE})" },
                new RiDataPreValidationBo { Step = StepPreValidation, Description = "INSURED_ATTAINED_AGE is not null", Condition = "{INSURED_ATTAINED_AGE} == null" },
                new RiDataPreValidationBo { Step = StepPreValidation, Description = "RATE_TABLE is not null", Condition = "string.IsNullOrEmpty({RATE_TABLE})" },
                new RiDataPreValidationBo { Step = StepPreValidation, Description = "UNDERWRITER_RATING is not less than 100", Condition = "{UNDERWRITER_RATING} < 100" },
                new RiDataPreValidationBo { Step = StepPreValidation, Description = "GROSS_PREMIUM is not null", Condition = "{GROSS_PREMIUM} == null" },
                new RiDataPreValidationBo { Step = StepPreValidation, Description = "NET_PREMIUM is not null", Condition = "{NET_PREMIUM} == null" },
                new RiDataPreValidationBo { Step = StepPostValidation, Description = "MFRS17_BASIC_RIDER is not null", Condition = "string.IsNullOrEmpty({MFRS17_BASIC_RIDER})" },
                new RiDataPreValidationBo { Step = StepPostValidation, Description = "MFRS17_CELL_NAME is not null", Condition = "string.IsNullOrEmpty({MFRS17_CELL_NAME})" },
                new RiDataPreValidationBo { Step = StepPostValidation, Description = "MFRS17_CONTRACT_CODE is not null", Condition = "string.IsNullOrEmpty({MFRS17_CONTRACT_CODE})" },
                new RiDataPreValidationBo { Step = StepPreValidation, Description = "TREATY_TYPE is not null", Condition = "string.IsNullOrEmpty({TREATY_TYPE})" },
                new RiDataPreValidationBo { Step = StepPostValidation, Description = "LOA_CODE is not null", Condition = "string.IsNullOrEmpty({LOA_CODE})" },
                new RiDataPreValidationBo { Step = StepPreValidation, Description = "RISK_PERIOD_START_DATE is not null", Condition = "{RISK_PERIOD_START_DATE} == DateTime.MinValue" },
                new RiDataPreValidationBo { Step = StepPreValidation, Description = "RISK_PERIOD_END_DATE is not null", Condition = "{RISK_PERIOD_END_DATE} == DateTime.MinValue" },
                new RiDataPreValidationBo { Step = StepPostValidation, Description = "MFRS17_ANNUAL_COHORT is not null", Condition = "{MFRS17_ANNUAL_COHORT} == null || {MFRS17_ANNUAL_COHORT} == 0" },
            };
        }
    }
}
