using BusinessObject;
using BusinessObject.SoaDatas;
using DataAccess.Entities;
using DataAccess.Entities.RiDatas;
using DataAccess.Entities.SoaDatas;
using PagedList;
using Services;
using Services.SoaDatas;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Models
{
    public class DirectRetroViewModel
    {
        public int Id { get; set; }

        [Required, DisplayName("Ceding Company")]
        public int CedantId { get; set; }

        public Cedant Cedant { get; set; }

        public CedantBo CedantBo { get; set; }

        [Required, DisplayName("Treaty Code")]
        public int TreatyCodeId { get; set; }

        public TreatyCode TreatyCode { get; set; }

        public TreatyCodeBo TreatyCodeBo { get; set; }

        [Required, DisplayName("SOA Quarter")]
        public string SoaQuarter { get; set; }

        [Required, DisplayName("SOA Data Batch")]
        public int SoaDataBatchId { get; set; }

        public SoaDataBatch SoaDataBatch { get; set; }

        public SoaDataBatchBo SoaDataBatchBo { get; set; }

        [DisplayName("Retro Status")]
        public int RetroStatus { get; set; }

        public int ModuleId { get; set; }

        public DirectRetroViewModel() {
            Set();
        }

        public DirectRetroViewModel(DirectRetroBo directRetroBo)
        {
            Set(directRetroBo);
        }

        public void Set(DirectRetroBo bo = null)
        {
            ModuleBo moduleBo = ModuleService.FindByController(ModuleBo.ModuleController.DirectRetro.ToString());
            ModuleId = moduleBo.Id;

            if (bo != null)
            {
                Id = bo.Id;
                CedantId = bo.CedantId;
                CedantBo = bo.CedantBo;
                TreatyCodeId = bo.TreatyCodeId;
                TreatyCodeBo = bo.TreatyCodeBo;
                SoaQuarter = bo.SoaQuarter;
                SoaDataBatchId = bo.SoaDataBatchId;
                SoaDataBatchBo = bo.SoaDataBatchBo;
                RetroStatus = bo.RetroStatus;
            }
        }

        public DirectRetroBo FormBo(int createdById, int updatedById)
        {
            var bo = new DirectRetroBo
            {
                Id = Id,
                CedantId = CedantId,
                CedantBo = CedantService.Find(CedantId),
                TreatyCodeId = TreatyCodeId,
                TreatyCodeBo = TreatyCodeService.Find(TreatyCodeId),
                SoaQuarter = SoaQuarter,
                SoaDataBatchId = SoaDataBatchId,
                SoaDataBatchBo = SoaDataBatchService.Find(SoaDataBatchId),
                RetroStatus = RetroStatus,
                CreatedById = createdById,
                UpdatedById = updatedById,
            };

            return bo;
        }

        public static Expression<Func<DirectRetro, DirectRetroViewModel>> Expression()
        {
            return entity => new DirectRetroViewModel
            {
                Id = entity.Id,
                CedantId = entity.CedantId,
                Cedant = entity.Cedant,
                TreatyCodeId = entity.TreatyCodeId,
                TreatyCode = entity.TreatyCode,
                SoaQuarter = entity.SoaQuarter,
                SoaDataBatchId = entity.SoaDataBatchId,
                SoaDataBatch = entity.SoaDataBatch,
                RetroStatus = entity.RetroStatus,
            };
        }

        public void ProcessStatusHistory(FormCollection form, int authUserId, ref TrailObject trail)
        {
            bool isChangeStatus = false;
            bool isRequiredUpdate = false;
            StatusHistoryBo latestStatusHistoryBo = StatusHistoryService.FindLatestByModuleIdObjectId(ModuleId, Id);

            if (latestStatusHistoryBo == null || latestStatusHistoryBo.Status != RetroStatus)
            {
                isChangeStatus = true;
            }
            else if (latestStatusHistoryBo != null || latestStatusHistoryBo.Status == RetroStatus)
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
                    Status = RetroStatus,
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

    public class RetroRiDataListingViewModel
    {
        public int Id { get; set; }
        public string PolicyNumber { get; set; }
        public string TreatyCode { get; set; }
        public string TransactionTypeCode { get; set; }
        public string ReinsBasisCode { get; set; }
        public int? ReportPeriodMonth { get; set; }
        public int? ReportPeriodYear { get; set; }
        public int? RiskPeriodMonth { get; set; }
        public int? RiskPeriodYear { get; set; }
        public string InsuredName { get; set; }
        public string RetroParty1 { get; set; }
        public double? RetroShare1 { get; set; }
        public double? RetroAar1 { get; set; }
        public double? RetroReinsurancePremium1 { get; set; }
        public double? RetroDiscount1 { get; set; }
        public double? RetroNetPremium1 { get; set; }
        public string RetroParty2 { get; set; }
        public double? RetroShare2 { get; set; }
        public double? RetroAar2 { get; set; }
        public double? RetroReinsurancePremium2 { get; set; }
        public double? RetroDiscount2 { get; set; }
        public double? RetroNetPremium2 { get; set; }
        public string RetroParty3 { get; set; }
        public double? RetroShare3 { get; set; }
        public double? RetroAar3 { get; set; }
        public double? RetroReinsurancePremium3 { get; set; }
        public double? RetroDiscount3 { get; set; }
        public double? RetroNetPremium3 { get; set; }

        public static Expression<Func<RiData, RetroRiDataListingViewModel>> Expression()
        {
            return entity => new RetroRiDataListingViewModel
            {
                Id = entity.Id,
                PolicyNumber = entity.PolicyNumber,
                TreatyCode = entity.TreatyCode,
                TransactionTypeCode = entity.TransactionTypeCode,
                ReinsBasisCode = entity.ReinsBasisCode,
                ReportPeriodMonth = entity.ReportPeriodMonth,
                ReportPeriodYear = entity.ReportPeriodYear,
                RiskPeriodMonth = entity.RiskPeriodMonth,
                RiskPeriodYear = entity.RiskPeriodYear,
                InsuredName = entity.InsuredName,
                RetroParty1 = entity.RetroParty1,
                RetroShare1 = entity.RetroShare1,
                RetroAar1 = entity.RetroAar1,
                RetroReinsurancePremium1 = entity.RetroReinsurancePremium1,
                RetroDiscount1 = entity.RetroDiscount1,
                RetroNetPremium1 = entity.RetroNetPremium1,
                RetroParty2 = entity.RetroParty2,
                RetroShare2 = entity.RetroShare2,
                RetroAar2 = entity.RetroAar2,
                RetroReinsurancePremium2 = entity.RetroReinsurancePremium2,
                RetroDiscount2 = entity.RetroDiscount2,
                RetroNetPremium2 = entity.RetroNetPremium2,
                RetroParty3 = entity.RetroParty3,
                RetroShare3 = entity.RetroShare3,
                RetroAar3 = entity.RetroAar3,
                RetroReinsurancePremium3 = entity.RetroReinsurancePremium3,
                RetroDiscount3 = entity.RetroDiscount3,
                RetroNetPremium3 = entity.RetroNetPremium3,
            };
        }
    }

    public class RetroClaimRegisterListingViewModel
    {
        public int Id { get; set; }
        public string EntryNo { get; set; }
        public string ClaimTransactionType { get; set; }
        public string MlreEventCode { get; set; }
        public string ClaimCode { get; set; }
        public string ClaimId { get; set; }
        public string PolicyNumber { get; set; }
        public string TreatyType { get; set; }
        public DateTime? DateOfRegister { get; set; }
        public string InsuredName { get; set; }
        public string InsuredGenderCode { get; set; }
        public double? AarPayable { get; set; }
        public string RetroParty1 { get; set; }
        public double? RetroShare1 { get; set; }
        public double? RetroRecovery1 { get; set; }
        public string RetroParty2 { get; set; }
        public double? RetroShare2 { get; set; }
        public double? RetroRecovery2 { get; set; }
        public string RetroParty3 { get; set; }
        public double? RetroShare3 { get; set; }
        public double? RetroRecovery3 { get; set; }

        public static Expression<Func<ClaimRegister, RetroClaimRegisterListingViewModel>> Expression()
        {
            return entity => new RetroClaimRegisterListingViewModel
            {
                Id = entity.Id,
                EntryNo = entity.EntryNo,
                ClaimTransactionType = entity.ClaimTransactionType,
                MlreEventCode = entity.MlreEventCode,
                ClaimCode = entity.ClaimCode,
                ClaimId = entity.ClaimId,
                PolicyNumber = entity.PolicyNumber,
                TreatyType = entity.TreatyType,
                DateOfRegister = entity.DateOfRegister,
                InsuredName = entity.InsuredName,
                InsuredGenderCode = entity.InsuredGenderCode,
                AarPayable = entity.AarPayable,
                RetroParty1 = entity.RetroParty1,
                RetroShare1 = entity.RetroShare1,
                RetroRecovery1 = entity.RetroRecovery1,
                RetroParty2 = entity.RetroParty2,
                RetroShare2 = entity.RetroShare2,
                RetroRecovery2 = entity.RetroRecovery2,
                RetroParty3 = entity.RetroParty3,
                RetroShare3 = entity.RetroShare3,
                RetroRecovery3 = entity.RetroRecovery3,
            };
        }
    }
}