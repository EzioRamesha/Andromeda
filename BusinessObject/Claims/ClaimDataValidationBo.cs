using Newtonsoft.Json;
using Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Claims
{
    public class ClaimDataValidationBo
    {
        [JsonIgnore]
        public int Id { get; set; }

        [JsonIgnore]
        public int ClaimDataConfigId { get; set; }

        [JsonIgnore]
        public ClaimDataConfigBo ClaimDataConfigBo { get; set; }

        public int Step { get; set; }

        [JsonIgnore]
        public int SortIndex { get; set; }

        [MaxLength(128)]
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

        public static List<ClaimDataValidationBo> GetDefault()
        {
            return new List<ClaimDataValidationBo>
            {
                new ClaimDataValidationBo() { Step = StepPreValidation },
                new ClaimDataValidationBo() { Step = StepPostValidation, Description = "MFRS17_CONTRACT_CODE is not null", Condition = "string.IsNullOrEmpty({MFRS17_CONTRACT_CODE})" },
                new ClaimDataValidationBo() { Step = StepPostValidation, Description = "MFRS17_ANNUAL_COHORT is not null", Condition = "{MFRS17_ANNUAL_COHORT} == null || {MFRS17_ANNUAL_COHORT} == 0" }
            };
        }

        public List<string> Validate(ClaimDataFileConfig claimDataFileConfig)
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

        
    }
}
