using BusinessObject.Retrocession;
using Shared;
using Shared.Forms.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class RetroTreatyDetailViewModel
    {
        public int Id { get; set; }

        public int RetroTreatyId { get; set; }

        public int PerLifeRetroConfigurationTreatyId { get; set; }

        [DisplayName("Treaty Code")]
        public string TreatyCode { get; set; }

        [DisplayName("Treaty Type")]
        public string TreatyType { get; set; }

        [DisplayName("Funds Accounting Type")]
        public string FundsAccountingType { get; set; }

        [DisplayName("Reinsurance Effective Start Date")]
        public string ReinsEffectiveStartDateStr { get; set; }

        [DisplayName("Reinsurance Effective End Date")]
        public string ReinsEffectiveEndDateStr { get; set; }

        [DisplayName("Risk Quarter Start Date")]
        public string RiskQuarterStartDateStr { get; set; }

        [DisplayName("Risk Quarter End Date")]
        public string RiskQuarterEndDateStr { get; set; }

        [DisplayName("To Aggregate")]
        public string IsToAggregate { get; set; }

        public double MlreShare { get; set; }

        [Required, ValidateDouble, DisplayName("MLRe Share")]
        public string MlreShareStr { get; set; }

        [DisplayName("Remark")]
        public string Remark { get; set; }

        [DisplayName("Gross Retro Premium")]
        public string GrossRetroPremium { get; set; }

        [DisplayName("Treaty Discount")]
        public string TreatyDiscount { get; set; }

        [DisplayName("Net Retro Premium")]
        public string NetRetroPremium { get; set; }

        [DisplayName("Premium Spread Table Rule")]
        public int? PremiumSpreadTableId { get; set; }

        public string PremiumSpreadTableRule { get; set; }

        [DisplayName("Discount Table Rule")]
        public int? TreatyDiscountTableId { get; set; }

        public string TreatyDiscountTableRule { get; set; }

        public RetroTreatyDetailViewModel() { }

        public RetroTreatyDetailViewModel(RetroTreatyDetailBo retroTreatyDetailBo)
        {
            Set(retroTreatyDetailBo);
        }

        public void Set(RetroTreatyDetailBo bo)
        {
            if (bo != null)
            {
                Id = bo.Id;
                RetroTreatyId = bo.RetroTreatyId;
                PerLifeRetroConfigurationTreatyId = bo.PerLifeRetroConfigurationTreatyId;
                TreatyCode = bo.PerLifeRetroConfigurationTreatyBo?.TreatyCodeBo.Code;
                TreatyType = bo.PerLifeRetroConfigurationTreatyBo?.TreatyTypePickListDetailBo.ToString();
                FundsAccountingType = bo.PerLifeRetroConfigurationTreatyBo?.FundsAccountingTypePickListDetailBo.ToString();
                ReinsEffectiveStartDateStr = bo.PerLifeRetroConfigurationTreatyBo?.ReinsEffectiveStartDateStr;
                ReinsEffectiveEndDateStr = bo.PerLifeRetroConfigurationTreatyBo?.ReinsEffectiveEndDateStr;
                RiskQuarterStartDateStr = bo.PerLifeRetroConfigurationTreatyBo?.RiskQuarterStartDateStr;
                RiskQuarterEndDateStr = bo.PerLifeRetroConfigurationTreatyBo?.RiskQuarterEndDateStr;
                IsToAggregate = bo.PerLifeRetroConfigurationTreatyBo.IsToAggregate ? "Yes" : "No";
                MlreShare = bo.MlreShare;
                MlreShareStr = Util.DoubleToString(bo.MlreShare);
                Remark = bo.Remark;
                GrossRetroPremium = bo.GrossRetroPremium;
                TreatyDiscount = bo.TreatyDiscount;
                NetRetroPremium = bo.NetRetroPremium;
                PremiumSpreadTableId = bo.PremiumSpreadTableId;
                PremiumSpreadTableRule = bo.PremiumSpreadTableBo?.Rule;
                TreatyDiscountTableId = bo.TreatyDiscountTableId;
                TreatyDiscountTableRule = bo.TreatyDiscountTableBo?.Rule;
            }
        }

        public RetroTreatyDetailBo FormBo(int authUserId, RetroTreatyDetailBo bo = null)
        {
            return new RetroTreatyDetailBo
            {
                Id = bo != null ? bo.Id : 0,
                RetroTreatyId = RetroTreatyId,
                PerLifeRetroConfigurationTreatyId = PerLifeRetroConfigurationTreatyId,
                MlreShare = Util.StringToDouble(MlreShareStr).Value,
                Remark = Remark,
                GrossRetroPremium = GrossRetroPremium,
                TreatyDiscount = TreatyDiscount,
                NetRetroPremium = NetRetroPremium,
                PremiumSpreadTableId = PremiumSpreadTableId,
                TreatyDiscountTableId = TreatyDiscountTableId,
                CreatedById = bo != null ? bo.CreatedById : authUserId,
                UpdatedById = authUserId,
            };
        }
    }
}