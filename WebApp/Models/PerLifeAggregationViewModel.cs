using BusinessObject;
using BusinessObject.Identity;
using BusinessObject.Retrocession;
using DataAccess.Entities;
using DataAccess.Entities.Identity;
using DataAccess.Entities.Retrocession;
using Services;
using Services.Identity;
using Shared;
using Shared.Forms.Attributes;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace WebApp.Models
{
    public class PerLifeAggregationViewModel
    {
        public int Id { get; set; }

        [Required, DisplayName("Funds Accounting Type")]
        public int FundsAccountingTypePickListDetailId { get; set; }

        public PickListDetail FundsAccountingTypePickListDetail { get; set; }

        public PickListDetailBo FundsAccountingTypePickListDetailBo { get; set; }

        [Required, DisplayName("Snapshot Version")]
        public int CutOffId { get; set; }

        public CutOff CutOff { get; set; }

        public CutOffBo CutOffBo { get; set; }

        [Required, StringLength(64), DisplayName("SOA Quarter")]
        public string SoaQuarter { get; set; }

        [DisplayName("Processing Date")]
        public DateTime? ProcessingDate { get; set; }

        [DisplayName("Processing Date")]
        [ValidateDate]
        public string ProcessingDateStr { get; set; }

        [Required, DisplayName("Retro Status")]
        public int Status { get; set; }

        [Required, DisplayName("Person In Charge")]
        public int? PersonInChargeId { get; set; }

        public User PersonInCharge { get; set; }

        public UserBo PersonInChargeBo { get; set; }

        [DisplayName("Errors")]
        public string Errors { get; set; }

        public string SelectedIds { get; set; }

        public int? ActiveTab { get; set; }

        public int ModuleId { get; set; }

        public PerLifeAggregationViewModel() { }

        public PerLifeAggregationViewModel(PerLifeAggregationBo perLifeAggregationBo)
        {
            Set(perLifeAggregationBo);
        }

        public void Set(PerLifeAggregationBo bo)
        {
            if (bo != null)
            {
                Id = bo.Id;
                FundsAccountingTypePickListDetailId = bo.FundsAccountingTypePickListDetailId;
                FundsAccountingTypePickListDetailBo = bo.FundsAccountingTypePickListDetailBo;
                CutOffId = bo.CutOffId;
                CutOffBo = bo.CutOffBo;
                SoaQuarter = bo.SoaQuarter;
                ProcessingDate = bo.ProcessingDate;
                ProcessingDateStr = bo.ProcessingDate?.ToString(Util.GetDateFormat());
                Status = bo.Status;
                PersonInChargeId = bo.PersonInChargeId;
                PersonInChargeBo = bo.PersonInChargeBo;
                Errors = bo.Errors;
            }
        }

        public PerLifeAggregationBo FormBo(int createdById, int updatedById)
        {
            var bo = new PerLifeAggregationBo
            {
                Id = Id,
                FundsAccountingTypePickListDetailId = FundsAccountingTypePickListDetailId,
                FundsAccountingTypePickListDetailBo = PickListDetailService.Find(FundsAccountingTypePickListDetailId),
                CutOffId = CutOffId,
                CutOffBo = CutOffService.Find(CutOffId),
                SoaQuarter = SoaQuarter,
                ProcessingDate = ProcessingDate,
                Status = Status,
                PersonInChargeId = PersonInChargeId,
                PersonInChargeBo = UserService.Find(PersonInChargeId),
                Errors = Errors,

                CreatedById = createdById,
                UpdatedById = updatedById,
            };

            if (!string.IsNullOrEmpty(ProcessingDateStr))
            {
                bo.ProcessingDate = DateTime.Parse(ProcessingDateStr);
            }

            return bo;
        }

        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    List<ValidationResult> results = new List<ValidationResult>();

        //    if (!string.IsNullOrEmpty(SoaQuarter) && CutOffId != 0)
        //    {
        //        var cutOffBo = CutOffService.Find(CutOffId);

        //        if (cutOffBo == null)
        //        {
        //            results.Add(new ValidationResult(
        //                "Snapshot Version not found",
        //                new[] { nameof(CutOffId) }
        //            ));
        //        }
        //        else
        //        {
        //            QuarterObject soaQuarter = new QuarterObject(SoaQuarter);
        //            QuarterObject cutOffQuarter = new QuarterObject(cutOffBo.Quarter);
        //            if ((cutOffQuarter.Year > soaQuarter.Year) ||
        //                cutOffQuarter.Year == soaQuarter.Year && cutOffQuarter.MonthStart > soaQuarter.MonthStart)
        //            {
        //                results.Add(new ValidationResult(
        //                    "Snapshot Version should be before or equal to SOA Quarter",
        //                    new[] { nameof(CutOffId) }
        //                ));
        //            }
        //        }
        //    }

        //    return results;
        //}

        public static Expression<Func<PerLifeAggregation, PerLifeAggregationViewModel>> Expression()
        {
            return entity => new PerLifeAggregationViewModel
            {
                Id = entity.Id,
                FundsAccountingTypePickListDetailId = entity.FundsAccountingTypePickListDetailId,
                FundsAccountingTypePickListDetail = entity.FundsAccountingTypePickListDetail,
                CutOffId = entity.CutOffId,
                CutOff = entity.CutOff,
                SoaQuarter = entity.SoaQuarter,
                ProcessingDate = entity.ProcessingDate,
                Status = entity.Status,
                PersonInChargeId = entity.PersonInChargeId,
                PersonInCharge = entity.PersonInCharge,
            };
        }

        public void ProcessStatusHistory(FormCollection form, int authUserId, ref TrailObject trail)
        {
            ModuleBo moduleBo = ModuleService.FindByController(ModuleBo.ModuleController.PerLifeAggregation.ToString());
            ModuleId = moduleBo.Id;

            bool isChangeStatus = false;
            bool isRequiredUpdate = false;
            StatusHistoryBo latestStatusHistoryBo = StatusHistoryService.FindLatestByModuleIdObjectId(ModuleId, Id);

            if (latestStatusHistoryBo == null || latestStatusHistoryBo.Status != Status)
            {
                isChangeStatus = true;
            }
            else if (latestStatusHistoryBo != null || latestStatusHistoryBo.Status == Status)
            {
                if (latestStatusHistoryBo.CreatedById != authUserId)
                    isChangeStatus = true;
                else
                    isRequiredUpdate = true;
            }

            if (isChangeStatus)
            {
                StatusHistoryBo statusHistoryBo = new StatusHistoryBo
                {
                    ModuleId = ModuleId,
                    ObjectId = Id,
                    Status = Status,
                    CreatedById = authUserId,
                    UpdatedById = authUserId,
                };
                StatusHistoryService.Save(ref statusHistoryBo, ref trail);
            }

            // Only update datetime
            if (isRequiredUpdate)
            {
                StatusHistoryService.Update(ref latestStatusHistoryBo, ref trail);
            }
        }
    }
}