using BusinessObject;
using BusinessObject.TreatyPricing;
using Services;
using Services.TreatyPricing;
using Shared;
using Shared.DataAccess;
using Shared.Forms.Attributes;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace WebApp.Models
{
    public class TreatyPricingAdvantageProgramViewModel : ObjectVersion
    {
        public int Id { get; set; }

        public int ModuleId { get; set; }

        [Required, DisplayName("Cedant")]
        public int TreatyPricingCedantId { get; set; }
        public TreatyPricingCedantBo TreatyPricingCedantBo { get; set; }

        [Required, StringLength(60), DisplayName("Advantage Program ID")]
        public string Code { get; set; }

        [Required, DisplayName("Advantage Program Name")]
        public string Name { get; set; }

        public string Description { get; set; }

        public int Status { get; set; }

        [DisplayName("Version")]
        public int Version { get; set; }

        [Required, DisplayName("Person In-Charge")]
        public int PersonInChargeId { get; set; }

        [DisplayName("Effective Date")]
        public DateTime EffcetiveAt { get; set; }

        [DisplayName("Effective Date")]
        public string EffcetiveAtStr { get; set; }

        [DisplayName("Advantage Program - Retention")]
        public string Retention { get; set; }

        [DisplayName("Advantage Program - MLRe's Share")]
        public string MlreShare { get; set; }

        [DisplayName("Note")]
        public string Remarks { get; set; }

        public int WorkflowId { get; set; }

        public TreatyPricingAdvantageProgramViewModel()
        {
            Set();
        }

        public TreatyPricingAdvantageProgramViewModel(TreatyPricingAdvantageProgramBo advantageProgramBo)
        {
            Set(advantageProgramBo);
            SetVersionObjects(advantageProgramBo.TreatyPricingAdvantageProgramVersionBos);

            PersonInChargeId = CurrentVersionObject != null ? int.Parse(CurrentVersionObject.GetPropertyValue("PersonInChargeId").ToString()) : 0;
            ModuleId = ModuleService.FindByController(ModuleBo.ModuleController.TreatyPricingCedant.ToString()).Id;
        }

        public void Set(TreatyPricingAdvantageProgramBo bo = null)
        {
            if (bo == null)
                return;

            Id = bo.Id;
            TreatyPricingCedantId = bo.TreatyPricingCedantId;
            TreatyPricingCedantBo = bo.TreatyPricingCedantBo;
            Code = bo.Code;
            Name = bo.Name;
            Description = bo.Description;
            Status = bo.Status;
        }

        public TreatyPricingAdvantageProgramBo FormBo(int createdById, int updatedById)
        {
            return new TreatyPricingAdvantageProgramBo
            {
                Id = Id,
                TreatyPricingCedantId = TreatyPricingCedantId,
                TreatyPricingCedantBo = TreatyPricingCedantService.Find(TreatyPricingCedantId),
                Name = Name,
                Code = Code,
                Description = Description,
                Status = Status,
                CreatedById = createdById,
                UpdatedById = updatedById,
            };
        }

        public TreatyPricingAdvantageProgramVersionBo GetVersionBo(TreatyPricingAdvantageProgramVersionBo bo)
        {
            bo.PersonInChargeId = PersonInChargeId;
            bo.EffcetiveAtStr = EffcetiveAtStr;
            bo.EffectiveAt = Util.GetParseDateTime(EffcetiveAtStr) ?? DateTime.Now.Date;
            bo.Retention = Retention;
            bo.MlreShare = MlreShare;
            bo.Remarks = Remarks;

            return bo;
        }

        public List<TreatyPricingAdvantageProgramVersionBenefitBo> GetBenefits(FormCollection form, ref Result result, int currentVersionObjectId)
        {
            int index = 0;
            List<TreatyPricingAdvantageProgramVersionBenefitBo> bos = new List<TreatyPricingAdvantageProgramVersionBenefitBo> { };
            while (form.AllKeys.Contains(string.Format("benefitId[{0}]", index)))
            {
                string benefitId = form.Get(string.Format("benefitId[{0}]", index));
                string id = form.Get(string.Format("verBenefitId[{0}]", index));
                string extraMortality = form.Get(string.Format("extraMortality[{0}]", index));
                string sumAssured = form.Get(string.Format("sumAssured[{0}]", index));

                TreatyPricingAdvantageProgramVersionBenefitBo bo = new TreatyPricingAdvantageProgramVersionBenefitBo
                {
                    TreatyPricingAdvantageProgramVersionId = currentVersionObjectId,
                    BenefitId = int.Parse(benefitId),
                    BenefitCode = BenefitService.Find(int.Parse(benefitId)).Code
                };

                if (!string.IsNullOrEmpty(extraMortality)) bo.ExtraMortalityStr = Util.DoubleToString(extraMortality, 2);
                if (!string.IsNullOrEmpty(sumAssured)) bo.SumAssuredStr = Util.DoubleToString(sumAssured, 2);
                if (!string.IsNullOrEmpty(id)) bo.Id = int.Parse(id);

                List<string> errors = bo.Validate();
                foreach (string error in errors)
                    result.AddError(error); 

                bos.Add(bo);
                index++;
            }
            return bos;
        }

        public void ProcessBenefits(List<TreatyPricingAdvantageProgramVersionBenefitBo> bos, int authUserId, ref TrailObject trail, int currentVersionObjectId)
        {
            List<int> savedIds = new List<int> { };
            if (!bos.IsNullOrEmpty())
            {
                foreach (TreatyPricingAdvantageProgramVersionBenefitBo bo in bos)
                {
                    bo.ExtraMortality = Util.StringToDouble(bo.ExtraMortalityStr);
                    bo.SumAssured = Util.StringToDouble(bo.SumAssuredStr);
                    bo.CreatedById = authUserId;
                    bo.UpdatedById = authUserId;

                    var VersionBenefitBo = bo;
                    TreatyPricingAdvantageProgramVersionBenefitService.Save(ref VersionBenefitBo);

                    savedIds.Add(VersionBenefitBo.Id);
                }
                TreatyPricingAdvantageProgramVersionBenefitService.GetByTreatyPricingAdvantageProgramVersionIdExcept(currentVersionObjectId, savedIds, ref trail);
            }
        }
    }
}