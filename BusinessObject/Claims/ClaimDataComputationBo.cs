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
    public class ClaimDataComputationBo
    {
        [JsonIgnore]
        public int Id { get; set; }

        [JsonIgnore]
        public int ClaimDataConfigId { get; set; }

        [JsonIgnore]
        public ClaimDataConfigBo ClaimDataConfigBo { get; set; }

        public int Step { get; set; }

        public int SortIndex { get; set; }

        [MaxLength(128)]
        public string Description { get; set; }

        [MaxLength(512)]
        public string Condition { get; set; }

        [JsonIgnore]
        public int StandardClaimDataOutputId { get; set; }

        public string StandardClaimDataOutputCode { get; set; }

        [JsonIgnore]
        public StandardClaimDataOutputBo StandardClaimDataOutputBo { get; set; }

        public int Mode { get; set; }

        [MaxLength(512)]
        public string CalculationFormula { get; set; }

        [JsonIgnore]
        public int CreatedById { get; set; }

        [JsonIgnore]
        public int? UpdatedById { get; set; }

        public const int StepPreComputation = 1;
        public const int StepPostComputation = 2;
        public const int MaxStep = 2;

        public const int ModeFormula = 1;
        public const int ModeTable = 2;
        public const int MaxMode = 2;

        public const int TableTreaty = 1;
        public const int MaxTable = 1;

        public const string DisplayName = "Computation";

        public static string GetStepName(int key)
        {
            switch (key)
            {
                case StepPreComputation:
                    return "Pre-Computation";
                case StepPostComputation:
                    return "Post-Computation";
                default:
                    return "";
            }
        }

        public static string GetModeName(int key)
        {
            switch (key)
            {
                case ModeFormula:
                    return "Formula";
                case ModeTable:
                    return "Table";
                default:
                    return "";
            }
        }

        public static string GetTableName(int key)
        {
            switch (key)
            {
                case TableTreaty:
                    return "Treaty Code Mapping";
                default:
                    return "";
            }
        }

        public static List<ClaimDataComputationBo> GetDefault()
        {
            return new List<ClaimDataComputationBo>
            {
                new ClaimDataComputationBo
                {
                    Step = StepPreComputation,
                },
                new ClaimDataComputationBo
                {
                    Step = StepPostComputation,
                    SortIndex = 1,
                    Description = "To update MFRS17 Contract Code",
                    Condition = "true",
                    StandardClaimDataOutputId = StandardClaimDataOutputBo.TypeMfrs17ContractCode,
                    Mode = ModeFormula,
                    CalculationFormula = "{RIDATA.MFRS17_CONTRACT_CODE}",
                },
                new ClaimDataComputationBo
                {
                    Step = StepPostComputation,
                    SortIndex = 2,
                    Description = "To update MFRS17 Annual Cohort",
                    Condition = "true",
                    StandardClaimDataOutputId = StandardClaimDataOutputBo.TypeMfrs17AnnualCohort,
                    Mode = ModeFormula,
                    CalculationFormula = "{REINS_EFF_DATE_POL}.Year",
                },
            };
        }

        public List<string> Validate(ClaimDataFileConfig claimDataFileConfig)
        {
            List<string> errors = new List<string> { };

            this.ValidateNullAndMaxLength("Description", "Description", ref errors);
            this.ValidateNullAndMaxLength("Condition", "Condition", ref errors);

            if (StandardClaimDataOutputId == 0)
                errors.Add(string.Format(MessageBag.Required, "Standard Output"));

            this.ValidateNullAndMaxLength("CalculationFormula", "Calculation Formula", ref errors);

            return errors;
        }

        public StandardClaimDataOutputBo GetStandardClaimDataOutputBo()
        {
            StandardClaimDataOutputBo = StandardClaimDataOutputBo.GetByType(StandardClaimDataOutputBo.Type);
            return StandardClaimDataOutputBo;
        }

        public int? GetTableKey()
        {
            if (int.TryParse(CalculationFormula, out int table))
                return table;
            return null;
        }

        public bool IsEmpty()
        {
            return Description == null && Condition == null && CalculationFormula == null && StandardClaimDataOutputId == 0;
        }
    }
}
