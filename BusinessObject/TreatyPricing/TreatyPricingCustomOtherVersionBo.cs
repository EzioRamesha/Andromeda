using Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.TreatyPricing
{
    public class TreatyPricingCustomOtherVersionBo
    {
        public int Id { get; set; }

        public int TreatyPricingCustomOtherId { get; set; }

        public virtual TreatyPricingCustomOtherBo TreatyPricingCustomOtherBo { get; set; }

        public int Version { get; set; }

        public int? PersonInChargeId { get; set; }

        public string PersonInChargeName { get; set; }

        public DateTime? EffectiveAt { get; set; }

        public string EffectiveAtStr { get; set; }

        public string FileName { get; set; }

        public string HashFileName { get; set; }

        public string AdditionalRemarks { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        public TreatyPricingCustomOtherVersionBo()
        {

        }

        public TreatyPricingCustomOtherVersionBo(TreatyPricingCustomOtherVersionBo bo)
        {
            TreatyPricingCustomOtherId = bo.TreatyPricingCustomOtherId;
            PersonInChargeId = bo.PersonInChargeId;
            PersonInChargeName = bo.PersonInChargeName;
            EffectiveAt = bo.EffectiveAt;
            EffectiveAtStr = bo.EffectiveAtStr;
            FileName = bo.FileName;
            HashFileName = bo.HashFileName;
            AdditionalRemarks = bo.AdditionalRemarks;
        }

        public string GetLocalDirectory()
        {
            return Util.GetTreatyPricingCustomOtherUploadPath();
        }

        public string GetLocalPath()
        {
            if (string.IsNullOrEmpty(HashFileName))
                return null;
            return Path.Combine(GetLocalDirectory(), HashFileName);
        }

        public string FormatHashFileName()
        {
            HashFileName = Hash.HashFileName(FileName);
            return HashFileName;
        }
    }
}
