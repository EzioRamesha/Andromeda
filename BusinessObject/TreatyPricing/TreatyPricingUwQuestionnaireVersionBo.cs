using BusinessObject.Identity;
using System;
using System.Collections.Generic;

namespace BusinessObject.TreatyPricing
{
    public class TreatyPricingUwQuestionnaireVersionBo
    {
        public int Id { get; set; }

        public int TreatyPricingUwQuestionnaireId { get; set; }
        public virtual TreatyPricingUwQuestionnaireBo TreatyPricingUwQuestionnaireBo { get; set; }

        public int Version { get; set; }

        public int PersonInChargeId { get; set; }
        public UserBo PersonInChargeBo { get; set; }

        public DateTime? EffectiveAt { get; set; }

        public int Type { get; set; }

        public string Remarks { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        public string EffectiveAtStr { get; set; }
        public string TypeName { get; set; }


        // Uw Questionnaire Comparison Report
        public string CedantName { get; set; }
        public string UwQuestionnaireId { get; set; }
        public string UwQuestionnaireName { get; set; }
        public string StatusName { get; set; }
        public string Description { get; set; }
        public string VersionStr { get; set; }
        public string LinkedProducts { get; set; }
        public string BenefitCode { get; set; }
        public string DistributionChannel { get; set; }
        public IList<TreatyPricingUwQuestionnaireVersionDetailBo> TreatyPricingUwQuestionnaireVersionDetailBos { get; set; }


        public const int TypeGio = 1;
        public const int TypeSimplified = 2;
        public const int TypeFull = 3;
        public const int TypeMax = 3;

        public TreatyPricingUwQuestionnaireVersionBo() { }

        public TreatyPricingUwQuestionnaireVersionBo(TreatyPricingUwQuestionnaireVersionBo bo)
        {
            TreatyPricingUwQuestionnaireId = bo.TreatyPricingUwQuestionnaireId;
            Version = bo.Version;
            PersonInChargeId = bo.PersonInChargeId;
            EffectiveAt = bo.EffectiveAt;
            Type = bo.Type;
            Remarks = bo.Remarks;
        }

        public static string GetTypeName(int key)
        {
            switch (key)
            {
                case TypeGio:
                    return "GIO";
                case TypeSimplified:
                    return "Simplified";
                case TypeFull:
                    return "Full";
                default:
                    return "";
            }
        }
    }
}
