using BusinessObject;
using BusinessObject.TreatyPricing;
using DataAccess.Entities;
using DataAccess.Entities.TreatyPricing;
using Services;
using Shared;
using Shared.Forms.Attributes;
using Shared.Trails.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WebApp.Models
{
    public class TreatyPricingPerLifeRetroViewModel : ObjectVersion
    {
        public int Id { get; set; }

        public int ModuleId { get; set; }

        [Required, DisplayName("Per Life Treaty Code")]
        public string Code { get; set; }

        [Required, DisplayName("Retro Party")]
        public int? RetroPartyId { get; set; }
        //[Required, DisplayName("Retro Party")]
        //public RetroPartyBo RetroPartyBo { get; set; }
        public virtual RetroParty RetroParty { get; set; }

        [Required]
        public int Type { get; set; }

        [DisplayName("Retrocessionaire’s Share (%)")]
        public double? RetrocessionaireShare { get; set; }
        public string RetrocessionaireShareStr { get; set; }

        public string Description { get; set; }

        public int Version { get; set; }

        [Required, DisplayName("Person In-Charge")]
        public int PersonInChargeId { get; set; }

        [DisplayName("Refund of Unearned Premium")]
        public string RefundofUnearnedPremium { get; set; }

        [DisplayName("Retrocessionaire")]
        public int? RetrocessionaireRetroPartyId { get; set; }

        [DisplayName("Notification Period for Termination of New Business"), StringLength(128)]
        public string TerminationPeriod { get; set; }

        [DisplayName("Country of Residence")]
        public string ResidenceCountry { get; set; }

        [DisplayName("Payment of Retrocessionaire Premium")]
        public int? PaymentRetrocessionairePremiumPickListDetailId { get; set; }

        [DisplayName("Effective Date (Product / Revision)")]
        public DateTime? EffectiveDate { get; set; }
        [DisplayName("Effective Date (Product / Revision)")]
        public string EffectiveDateStr { get; set; }

        [DisplayName("Payment of Retrocessionaire Premium")]
        public int? JumboLimitCurrencyCodePickListDetailId { get; set; }

        [DisplayName("Amount")]
        public double? JumboLimit { get; set; }
        [DisplayName("Amount")]
        public string JumboLimitStr { get; set; }

        public string Remarks { get; set; }

        [DisplayName("Profit Sharing")]
        public int? ProfitSharing { get; set; }

        [DisplayName("Description")]
        public string ProfitDescription { get; set; }

        [DisplayName("Percentage of Net Profit")]
        public double? NetProfitPercentage { get; set; }

        [DisplayName("Percentage of Net Profit")]
        [ValidateDouble]
        public string NetProfitPercentageStr { get; set; }

        [DisplayName("Profit Commission Detail")]
        public string ProfitCommissionDetail { get; set; }

        [DisplayName("Tier Profit Commission")]
        public string TierProfitCommission { get; set; }

        public string Benefits { get; set; }

        public TreatyPricingPerLifeRetroViewModel()
        {
            Set();
        }

        public TreatyPricingPerLifeRetroViewModel(TreatyPricingPerLifeRetroBo perLifeRetroBo)
        {
            Set(perLifeRetroBo);
            SetVersionObjects(perLifeRetroBo.TreatyPricingPerLifeRetroVersionBos);

            PersonInChargeId = CurrentVersionObject != null ? int.Parse(CurrentVersionObject.GetPropertyValue("PersonInChargeId").ToString()) : 0;
            ModuleId = ModuleService.FindByController(ModuleBo.ModuleController.TreatyPricingPerLifeRetro.ToString()).Id;
        }

        public void Set(TreatyPricingPerLifeRetroBo bo = null)
        {
            if (bo == null)
                return;

            Id = bo.Id;
            Code = bo.Code;
            RetroPartyId = bo.RetroPartyId;
            Type = bo.Type;
            RetrocessionaireShare = bo.RetrocessionaireShare;
            Description = bo.Description;
            RetrocessionaireShareStr = bo.RetrocessionaireShareStr;
        }

        public static Expression<Func<TreatyPricingPerLifeRetro, TreatyPricingPerLifeRetroViewModel>> Expression()
        {
            return entity => new TreatyPricingPerLifeRetroViewModel
            {
                Id = entity.Id,
                Code = entity.Code,
                RetroPartyId = entity.RetroPartyId,
                RetroParty = entity.RetroParty,
                Type = entity.Type,
                Description = entity.Description,
                RetrocessionaireShare = entity.RetrocessionaireShare,
            };
        }

        public TreatyPricingPerLifeRetroBo FormBo(int createdById, int updatedById)
        {
            return new TreatyPricingPerLifeRetroBo
            {
                Id = Id,
                Code = Code,
                RetroPartyId = RetroPartyId,
                Type = Type,
                RetrocessionaireShare = RetrocessionaireShare,
                Description = Description,
                CreatedById = createdById,
                UpdatedById = updatedById,
            };
        }

        public TreatyPricingPerLifeRetroVersionBo GetVersionBo(TreatyPricingPerLifeRetroVersionBo bo)
        {
            bo.PersonInChargeId = PersonInChargeId;
            bo.RetrocessionaireRetroPartyId = RetrocessionaireRetroPartyId;
            bo.RefundofUnearnedPremium = RefundofUnearnedPremium;
            bo.TerminationPeriod = TerminationPeriod;
            bo.ResidenceCountry = ResidenceCountry;
            bo.PaymentRetrocessionairePremiumPickListDetailId = PaymentRetrocessionairePremiumPickListDetailId;
            bo.EffectiveDateStr = EffectiveDateStr;
            bo.EffectiveDate = Util.GetParseDateTime(EffectiveDateStr) ?? DateTime.Now.Date;
            bo.JumboLimitCurrencyCodePickListDetailId = JumboLimitCurrencyCodePickListDetailId;
            bo.JumboLimitStr = JumboLimitStr;
            bo.JumboLimit = Util.StringToDouble(JumboLimitStr, true, 2);
            bo.Remarks = Remarks;
            bo.ProfitSharing = ProfitSharing;
            bo.ProfitDescription = ProfitDescription;
            bo.ProfitCommissionDetail = ProfitCommissionDetail;
            bo.TierProfitCommission = TierProfitCommission;
            bo.Benefits = Benefits;

            if (ProfitSharing.HasValue && ProfitSharing == TreatyPricingPerLifeRetroVersionBo.ProfitSharingFlat)
            {
                bo.NetProfitPercentageStr = NetProfitPercentageStr;
                bo.NetProfitPercentage = Util.StringToDouble(NetProfitPercentageStr);
            }
            else
            {
                bo.NetProfitPercentageStr = null;
                bo.NetProfitPercentage = null;
            }

            return bo;
        }
    }
}