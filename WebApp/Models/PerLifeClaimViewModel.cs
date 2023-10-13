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
    public class PerLifeClaimViewModel : IValidatableObject
    {
        public int Id { get; set; }

        public int ModuleId { get; set; }

        [DisplayName("Snapshot Version")]
        public int CutOffId { get; set; }
        public CutOffBo CutOffBo { get; set; }

        [DisplayName("Snapshot Version")]
        public CutOff CutOff { get; set; }

        [Required, DisplayName("Person In Charge")]
        public int? PersonInChargeId { get; set; }

        public User PersonInCharge { get; set; }

        public UserBo PersonInChargeBo { get; set; }

        [DisplayName("Processing Date")]
        [ValidateDate]
        public string ProcessingDateStr { get; set; }

        [DisplayName("Processing Date")]
        public DateTime? ProcessingDate { get; set; }

        [DisplayName("Funds Account Type")]
        public int FundsAccountingTypePickListDetailId { get; set; }
        public PickListDetailBo FundsAccountingTypePickListDetailBo { get; set; }

        [DisplayName("Status")]
        public int Status { get; set; }

        [Required, DisplayName("SOA Quarter")]
        public string SoaQuarter { get; set; }


        //Per Life Claim Data
        public int PerLifeClaimId { get; set; }
        public int PerLifeClaimRetroDataId { get; set; }
        public int ClaimRegisterHistoryId { get; set; }
        public virtual ClaimRegisterHistoryBo ClaimRegisterHistoryBo { get; set; }

        public int? PerLifeAggregationDetailDataId { get; set; }
        public virtual PerLifeAggregationDetailDataBo PerLifeAggregationDetailDataBo { get; set; }

        public bool IsException { get; set; } = false;

        public int? ClaimCategory { get; set; }

        public bool IsExcludePerformClaimRecovery { get; set; } = false;

        public int? ClaimRecoveryStatus { get; set; }

        public int? ClaimRecoveryDecision { get; set; }

        public int? MovementType { get; set; }

        public string PerLifeRetro { get; set; }

        public int? RetroOutputId { get; set; }

        public int? RetainPoolId { get; set; }

        public int? NoOfRetroTreaty { get; set; }

        public int? RetroRecoveryId { get; set; }

        public string LateInterestShareFlag { get; set; }

        public string ExGratiaShareFlag { get; set; }

        // Claim Retro Data
        public double? MlreShare { get; set; }

        public double? RetroClaimRecoveryAmount { get; set; }

        public double? LateInterest { get; set; }

        public double? ExGratia { get; set; }

        public int? RetroTreatyId { get; set; }
        public virtual RetroTreatyBo RetroTreatyBo { get; set; }

        public double? RetroRatio { get; set; }

        public double? Aar { get; set; }

        public double? ComputedRetroRecoveryAmount { get; set; }

        public double? ComputedRetroLateInterest { get; set; }

        public double? ComputedRetroExGratia { get; set; }
        public string ReportedSoaQuarter { get; set; }

        public double? RetroRecoveryAmount { get; set; }

        public double? RetroLateInterest { get; set; }

        public double? RetroExGratia { get; set; }
        public int ComputedClaimCategory { get; set; }
        public int ClaimRetroDataCategory { get; set; }
        public string ClaimRetroDataRecovered { get; set; }

        // Claim Register Data
        public ClaimRegisterHistory ClaimRegisterHistory { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public int CreatedById { get; set; }
        public int? UpdatedById { get; set; }

        // Active Tab
        public int? ActiveTab { get; set; }

        public PerLifeClaimViewModel() { }
        public PerLifeClaimViewModel(PerLifeClaimBo perLifeClaimBo)
        {
            Set(perLifeClaimBo);
            ModuleId = ModuleService.FindByController(ModuleBo.ModuleController.PerLifeClaim.ToString()).Id;
        }

        public static Expression<Func<PerLifeClaim, PerLifeClaimViewModel>> Expression()
        {
            return entity => new PerLifeClaimViewModel
            {
                Id = entity.Id,
                FundsAccountingTypePickListDetailId = entity.FundsAccountingTypePickListDetailId,
                SoaQuarter = entity.SoaQuarter,
                CutOffId = entity.CutOffId,
                CutOff = entity.CutOff,
                Status = entity.Status,
                PersonInCharge = entity.PersonInCharge,
                ProcessingDate = entity.ProcessingDate
            };
        }

        public void Set(PerLifeClaimBo bo)
        {
            if (bo != null)
            {
                Id = bo.Id;
                CutOffId = bo.CutOffId;
                CutOffBo = CutOffService.Find(bo.CutOffId);
                FundsAccountingTypePickListDetailId = bo.FundsAccountingTypePickListDetailId;
                FundsAccountingTypePickListDetailBo = PickListDetailService.Find(bo.FundsAccountingTypePickListDetailId);
                Status = bo.Status;
                SoaQuarter = bo.SoaQuarter;
                PersonInChargeId = bo.PersonInChargeId;
                PersonInChargeBo = bo.PersonInChargeBo;
                ProcessingDate = bo.ProcessingDate;
                ProcessingDateStr = bo.ProcessingDate?.ToString(Util.GetDateFormat());
            }
        }

        public PerLifeClaimBo FormBo(int createdById, int updatedById)
        {
            var bo = new PerLifeClaimBo();
            bo.CutOffId = CutOffId;
            bo.CutOffBo = CutOffService.Find(CutOffId);
            bo.FundsAccountingTypePickListDetailId = FundsAccountingTypePickListDetailId;
            bo.FundsAccountingTypePickListDetailBo = PickListDetailService.Find(FundsAccountingTypePickListDetailId);
            bo.Status = Status;
            bo.SoaQuarter = SoaQuarter;
            ProcessingDate = ProcessingDate;
            bo.PersonInChargeId = PersonInChargeId;
            bo.PersonInChargeBo = UserService.Find(PersonInChargeId);
            bo.CreatedById = createdById;
            bo.UpdatedById = updatedById;

            return bo;
        }

        public static Expression<Func<PerLifeClaimData, PerLifeClaimViewModel>> DataExpression()
        {
            return entity => new PerLifeClaimViewModel
            {
                PerLifeClaimId = entity.PerLifeClaimId,
                ClaimRegisterHistoryId = entity.ClaimRegisterHistoryId,
                PerLifeAggregationDetailDataId = entity.PerLifeAggregationDetailDataId,
                IsException = entity.IsException,
                ClaimCategory = entity.ClaimCategory,
                IsExcludePerformClaimRecovery = entity.IsExcludePerformClaimRecovery,
                ClaimRecoveryStatus = entity.ClaimRecoveryStatus,
                ClaimRecoveryDecision = entity.ClaimRecoveryDecision,
                MovementType = entity.MovementType,
                PerLifeRetro = entity.PerLifeRetro,

                //Claim Register Data
                ClaimRegisterHistory = entity.ClaimRegisterHistory
            };
        }

        public static Expression<Func<PerLifeClaimRetroData, PerLifeClaimViewModel>> ClaimRetroDataExpression()
        {
            return entity => new PerLifeClaimViewModel
            {
                PerLifeClaimRetroDataId = entity.Id,
                PerLifeClaimId = entity.PerLifeClaimDataId,
                SoaQuarter = entity.PerLifeClaimData.PerLifeClaim.SoaQuarter,
                MlreShare = entity.MlreShare,
                RetroClaimRecoveryAmount = entity.RetroClaimRecoveryAmount,
                LateInterest = entity.LateInterest,
                ExGratia = entity.ExGratia,
                RetroTreatyId = entity.RetroTreatyId,
                RetroRatio = entity.RetroRatio,
                Aar = entity.Aar,
                ComputedRetroRecoveryAmount = entity.ComputedRetroRecoveryAmount,
                ComputedRetroLateInterest = entity.ComputedRetroLateInterest,
                ComputedRetroExGratia = entity.ComputedRetroExGratia,
                ReportedSoaQuarter = entity.ReportedSoaQuarter,
                RetroRecoveryAmount = entity.RetroRecoveryAmount,
                RetroLateInterest = entity.RetroLateInterest,
                RetroExGratia = entity.RetroExGratia,
                ComputedClaimCategory = entity.ComputedClaimCategory,
                ClaimRetroDataCategory = entity.ClaimCategory,

                //Claim Register Data
                ClaimRegisterHistory = entity.PerLifeClaimData.ClaimRegisterHistory
            };
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> results = new List<ValidationResult>();

            if (!string.IsNullOrEmpty(SoaQuarter) && CutOffId != 0)
            {
                var CufOffBo = CutOffService.Find(CutOffId);

                if (CufOffBo == null)
                {
                    results.Add(new ValidationResult(
                        "Snapshot Version not found",
                        new[] { nameof(CutOffId) }
                    ));
                }
                //else
                //{
                //    QuarterObject soaQuarter = new QuarterObject(SoaQuarter);
                //    if ((CufOffBo.Year < soaQuarter.Year) ||
                //        CufOffBo.Year == soaQuarter.Year && CufOffBo.Month < soaQuarter.MonthStart)
                //    {
                //        results.Add(new ValidationResult(
                //            "Snapshot Version should be greater or equal to SOA Quarter",
                //            new[] { nameof(CutOffId) }
                //        ));
                //    }
                //}
            }

            return results;
        }

        public void ProcessStatusHistory(int authUserId, ref TrailObject trail)
        {
            ModuleBo moduleBo = ModuleService.FindByController(ModuleBo.ModuleController.PerLifeClaim.ToString());
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