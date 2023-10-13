using BusinessObject.Identity;
using Shared.Trails.Attributes;
using System;

namespace BusinessObject.TreatyPricing
{
    public class TreatyPricingPerLifeRetroVersionBo
    {
        public int Id { get; set; }

        public int TreatyPricingPerLifeRetroId { get; set; }
        public TreatyPricingPerLifeRetroBo TreatyPricingPerLifeRetroBo { get; set; }

        public int Version { get; set; }

        public int PersonInChargeId { get; set; }
        public UserBo PersonInChargeBo { get; set; }

        public int? RetrocessionaireRetroPartyId { get; set; }
        public virtual RetroPartyBo RetrocessionaireRetroPartyBo { get; set; }

        public string RefundofUnearnedPremium { get; set; }

        public string TerminationPeriod { get; set; }

        public string ResidenceCountry { get; set; }

        public int? PaymentRetrocessionairePremiumPickListDetailId { get; set; }
        public virtual PickListDetailBo PaymentRetrocessionairePremiumPickListDetailBo { get; set; }

        public DateTime? EffectiveDate { get; set; }

        public int? JumboLimitCurrencyCodePickListDetailId { get; set; }
        public virtual PickListDetailBo JumboLimitCurrencyCodePickListDetailBo { get; set; }

        public double? JumboLimit { get; set; }

        public string Remarks { get; set; }

        public int? ProfitSharing { get; set; }

        public string ProfitDescription { get; set; }

        public double? NetProfitPercentage { get; set; }        

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        public string EffectiveDateStr { get; set; }
        public string JumboLimitStr { get; set; }
        public string NetProfitPercentageStr { get; set; }

        public string ProfitCommissionDetail { get; set; }

        public string TierProfitCommission { get; set; }

        [IsJsonProperty("BenefitId")]
        public string Benefits { get; set; }

        public TreatyPricingPerLifeRetroVersionBo() { }

        public TreatyPricingPerLifeRetroVersionBo(TreatyPricingPerLifeRetroVersionBo bo)
        {
            TreatyPricingPerLifeRetroId = bo.TreatyPricingPerLifeRetroId;
            Version = bo.Version;
            PersonInChargeId = bo.PersonInChargeId;
            RetrocessionaireRetroPartyId = bo.RetrocessionaireRetroPartyId;
            RefundofUnearnedPremium = bo.RefundofUnearnedPremium;
            TerminationPeriod = bo.TerminationPeriod;
            ResidenceCountry = bo.ResidenceCountry;
            PaymentRetrocessionairePremiumPickListDetailId = bo.PaymentRetrocessionairePremiumPickListDetailId;
            EffectiveDate = bo.EffectiveDate;
            EffectiveDateStr = bo.EffectiveDateStr;
            JumboLimitCurrencyCodePickListDetailId = bo.JumboLimitCurrencyCodePickListDetailId;
            JumboLimit = bo.JumboLimit;
            JumboLimitStr = bo.JumboLimitStr;
            Remarks = bo.Remarks;
            ProfitSharing = bo.ProfitSharing;
            ProfitDescription = bo.ProfitDescription;
            NetProfitPercentage = bo.NetProfitPercentage;
            NetProfitPercentageStr = bo.NetProfitPercentageStr;
            TierProfitCommission = bo.TierProfitCommission;
            ProfitCommissionDetail = bo.ProfitCommissionDetail;
            Benefits = bo.Benefits;
        }

        public const int ProfitSharingFlat = 1;
        public const int ProfitSharingTier = 2;

        public const int ProfitSharingMax = 2;

        public static string GetProfitSharingName(int key)
        {
            switch (key)
            {
                case ProfitSharingFlat:
                    return "Flat Profit Commission";
                case ProfitSharingTier:
                    return "Tier Profit Commission";
                default:
                    return "";
            }
        }
    }
}
