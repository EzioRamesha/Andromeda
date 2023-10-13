using BusinessObject;
using DataAccess.Entities;
using Services;
using Shared.Forms.Attributes;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace WebApp.Models
{
    public class Mfrs17ReportingViewModel
    {
        public int Id { get; set; }

        [Required, Display(Name = "Quarter")]
        [ValidateQuarter]
        public string Quarter { get; set; }

        [Required, Display(Name = "Status")]
        public int Status { get; set; }

        [Display(Name = "Total Records")]
        public int TotalRecord { get; set; }

        [Display(Name = "Type")]
        public int? GenerateType { get; set; }

        [Display(Name = "Generate Modified Only")]
        public bool GenerateModifiedOnly { get; set; }

        [Display(Name = "Generate (%)")]
        public double? GeneratePercentage { get; set; }

        [Required, Display(Name = "Cut Off Quarter")]
        public int CutOffId { get; set; }

        public CutOff CutOff { get; set; }

        public CutOffBo CutOffBo { get; set; }

        public int ModuleId { get; set; }

        [Display(Name = "Resume Generate")]
        public bool IsResume { get; set; }

        public Mfrs17ReportingViewModel()
        {
            Set();
        }

        public Mfrs17ReportingViewModel(Mfrs17ReportingBo mfrs17ReportingBo)
        {
            Set(mfrs17ReportingBo);
        }

        public void Set(Mfrs17ReportingBo mfrs17ReportingBo = null)
        {
            ModuleBo moduleBo = ModuleService.FindByController(ModuleBo.ModuleController.Mfrs17Reporting.ToString());
            ModuleId = moduleBo.Id;
            GenerateModifiedOnly = false;
            if (mfrs17ReportingBo != null)
            {
                Id = mfrs17ReportingBo.Id;
                Quarter = mfrs17ReportingBo.Quarter;
                Status = mfrs17ReportingBo.Status;
                TotalRecord = mfrs17ReportingBo.TotalRecord;
                GenerateType = mfrs17ReportingBo.GenerateType;
                GenerateModifiedOnly = mfrs17ReportingBo.GenerateModifiedOnly.HasValue ? mfrs17ReportingBo.GenerateModifiedOnly.Value : false;
                GeneratePercentage = mfrs17ReportingBo.GeneratePercentage;
                GenerateModifiedOnly = mfrs17ReportingBo.GenerateModifiedOnly ?? false;
                CutOffId = mfrs17ReportingBo.CutOffId;
                IsResume = mfrs17ReportingBo.IsResume ?? false;
            }
        }

        public static Expression<Func<Mfrs17Reporting, Mfrs17ReportingViewModel>> Expression()
        {
            return entity => new Mfrs17ReportingViewModel
            {
                Id = entity.Id,
                Quarter = entity.Quarter,
                Status = entity.Status,
                TotalRecord = entity.TotalRecord,
                GenerateType = entity.GenerateType,
                GeneratePercentage = entity.GeneratePercentage,
                CutOffId = entity.CutOffId,
                CutOff = entity.CutOff,
            };
        }

        public Mfrs17ReportingBo FormBo(int createdById, int updatedById)
        {
            var bo = new Mfrs17ReportingBo
            {
                Id = Id,
                Quarter = Quarter,
                Status = Status,
                TotalRecord = TotalRecord,
                GenerateType = GenerateType,
                GeneratePercentage = GeneratePercentage,
                CutOffId = CutOffId,
                CutOffBo = CutOffService.Find(CutOffId),
                IsResume = IsResume,
                CreatedById = createdById,
                UpdatedById = updatedById,
            };

            return bo;
        }

        public void ProcessCedantDetails(FormCollection form, int authUserId, ref TrailObject trail)
        {
            int maxIndex = int.Parse(form.Get("CedantDetails.MaxIndex"));
            int index = 0;

            while (index <= maxIndex)
            {
                string statusStr = form.Get(string.Format("CedantDetails.Status[{0}]", index));
                if (string.IsNullOrEmpty(statusStr))
                {
                    index++;
                    continue;
                }

                int status = int.Parse(statusStr);

                int cedantId = int.Parse(form.Get(string.Format("CedantDetails.CedantId[{0}]", index)));
                string treatyCode = form.Get(string.Format("CedantDetails.TreatyCode[{0}]", index));
                int premiumFrequencyCodePickListDetailId = int.Parse(form.Get(string.Format("CedantDetails.PremiumFrequencyCodePickListDetailId[{0}]", index)));
                string cedingPlanCode = form.Get(string.Format("CedantDetails.CedingPlanCode[{0}]", index));
                string riskQuarter = form.Get(string.Format("CedantDetails.RiskQuarter[{0}]", index));
                DateTime latestDataStartDate = DateTime.Parse(form.Get(string.Format("CedantDetails.LatestDataStartDateStr[{0}]", index)));
                DateTime latestDataEndDate = DateTime.Parse(form.Get(string.Format("CedantDetails.LatestDataEndDateStr[{0}]", index)));
                int record = int.Parse(form.Get(string.Format("CedantDetails.Record[{0}]", index)));

                string premiumFrequencyCode = PickListDetailService.Find(premiumFrequencyCodePickListDetailId)?.Code;
                IList<Mfrs17ReportingDetailBo> mfrs17CellMappingDetailBos = !string.IsNullOrEmpty(premiumFrequencyCode) && premiumFrequencyCode.ToLower() == "m" ?
                    Mfrs17ReportingDetailService.GetByGroupedDetail(Id, cedantId, treatyCode, premiumFrequencyCodePickListDetailId, riskQuarter, cedingPlanCode, latestDataStartDate, latestDataEndDate) :
                    Mfrs17ReportingDetailService.GetByGroupedDetail(Id, cedantId, treatyCode, premiumFrequencyCodePickListDetailId, riskQuarter, cedingPlanCode);

                foreach (Mfrs17ReportingDetailBo bo in mfrs17CellMappingDetailBos)
                {
                    bool updated = false;
                    if (bo.Status != status || bo.LatestDataStartDate != latestDataStartDate || bo.LatestDataEndDate != latestDataEndDate)
                        updated = true;

                    if (updated)
                    {
                        Mfrs17ReportingDetailBo mfrs17ReportingDetailBo = bo;
                        mfrs17ReportingDetailBo.Status = status;
                        mfrs17ReportingDetailBo.IsModified = false;
                        mfrs17ReportingDetailBo.LatestDataStartDate = latestDataStartDate;
                        mfrs17ReportingDetailBo.LatestDataEndDate = latestDataEndDate;
                        mfrs17ReportingDetailBo.UpdatedById = authUserId;
                        Mfrs17ReportingDetailService.Update(ref mfrs17ReportingDetailBo, ref trail);
                    }
                }

                if (mfrs17CellMappingDetailBos.Count == 0)
                {
                    Mfrs17ReportingDetailBo mfrs17ReportingDetailBo = new Mfrs17ReportingDetailBo
                    {
                        Mfrs17ReportingId = Id,
                        CedantId = cedantId,
                        TreatyCode = treatyCode,
                        PremiumFrequencyCodePickListDetailId = premiumFrequencyCodePickListDetailId,
                        CedingPlanCode = cedingPlanCode,
                        RiskQuarter = riskQuarter,
                        LatestDataStartDate = latestDataStartDate,
                        LatestDataEndDate = latestDataEndDate,
                        Record = record,
                        Status = status,
                        IsModified = false,
                        CreatedById = authUserId
                    };
                    Mfrs17ReportingDetailService.Create(ref mfrs17ReportingDetailBo, ref trail);
                }

                index++;
            }

        }

        public void ProcessStatusHistory(FormCollection form, int authUserId, ref TrailObject trail)
        {
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

            if (latestStatusHistoryBo == null && Status == Mfrs17ReportingBo.StatusSubmitForProcessing)
            {
                StatusHistoryBo statusHistoryBo = new StatusHistoryBo
                {
                    ModuleId = ModuleId,
                    ObjectId = Id,
                    Status = Mfrs17ReportingBo.StatusPending,
                    CreatedById = authUserId,
                    UpdatedById = authUserId,
                };
                StatusHistoryService.Save(ref statusHistoryBo, ref trail);
                statusHistoryBo = new StatusHistoryBo
                {
                    ModuleId = ModuleId,
                    ObjectId = Id,
                    Status = Status,
                    CreatedById = authUserId,
                    UpdatedById = authUserId,
                };
                StatusHistoryService.Save(ref statusHistoryBo, ref trail);
            }
            else if (isChangeStatus)
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
            else if (isRequiredUpdate)
            {
                StatusHistoryService.Update(ref latestStatusHistoryBo, ref trail);
            }
        }

        public void UpdateGenerateStatus(int authUserId)
        {
            var mfrs17ReportingDetailBos = Mfrs17ReportingDetailService.GetByMfrs17ReportingId(Id);
            if (mfrs17ReportingDetailBos != null)
            {
                foreach (var mfrs17ReportingDetailBo in mfrs17ReportingDetailBos)
                {
                    if (GenerateModifiedOnly && !mfrs17ReportingDetailBo.IsModified)
                        continue;

                    var bo = mfrs17ReportingDetailBo;
                    bo.GenerateStatus = Mfrs17ReportingDetailBo.GenerateStatusPending;
                    bo.UpdatedById = authUserId;
                    Mfrs17ReportingDetailService.Save(ref bo);
                }
            }

        }
    }
}