using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Retrocession
{
    public class RetroTreatyDetailBo
    {
        public int Id { get; set; }

        public int RetroTreatyId { get; set; }

        public virtual RetroTreatyBo RetroTreatyBo { get; set; }

        public int PerLifeRetroConfigurationTreatyId { get; set; }

        public virtual PerLifeRetroConfigurationTreatyBo PerLifeRetroConfigurationTreatyBo { get; set; }

        public int? PremiumSpreadTableId { get; set; }

        public virtual PremiumSpreadTableBo PremiumSpreadTableBo { get; set; }

        public string PremiumSpreadRule { get; set; }

        public int? TreatyDiscountTableId { get; set; }

        public virtual TreatyDiscountTableBo TreatyDiscountTableBo { get; set; }

        public string TreatyDiscountRule { get; set; }

        public double MlreShare { get; set; }

        public string MlreShareStr { get; set; }

        public string GrossRetroPremium { get; set; }

        public string TreatyDiscount { get; set; }

        public string NetRetroPremium { get; set; }

        public string Remark { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        // Retro Config
        public string ConfigTreatyCode { get; set; }

        public int ConfigTreatyCodeId { get; set; }

        public TreatyCodeBo ConfigTreatyCodeBo { get; set; }

        public string ConfigTreatyType { get; set; }

        public int ConfigTreatyTypeId { get; set; }

        public PickListDetailBo ConfigTreatyTypeBo { get; set; }

        public string ConfigFundsAccountingType { get; set; }

        public int ConfigFundsAccountingTypeId { get; set; }

        public PickListDetailBo ConfigFundsAccountingTypeBo { get; set; }

        public DateTime ConfigEffectiveStartDate { get; set; }

        public DateTime ConfigEffectiveEndDate { get; set; }

        public DateTime ConfigRiskQuarterStartDate { get; set; }

        public DateTime ConfigRiskQuarterEndDate { get; set; }

        public bool ConfigIsToAggregate { get; set; }

        public string ConfigRemark { get; set; }

        public RetroTreatyDetailBo()
        {
            MlreShare = 100;
        }
    }
}
