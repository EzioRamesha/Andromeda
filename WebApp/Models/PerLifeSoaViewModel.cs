using BusinessObject;
using BusinessObject.Identity;
using BusinessObject.Retrocession;
using DataAccess.Entities;
using DataAccess.Entities.Identity;
using DataAccess.Entities.Retrocession;
using Services;
using Shared.Forms.Attributes;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace WebApp.Models
{
    public class PerLifeSoaViewModel
    {
        public int Id { get; set; }

        [Required, DisplayName("Retro Party")]
        public int RetroPartyId { get; set; }
        [DisplayName("Retro Party")]
        public RetroPartyBo RetroPartyBo { get; set; }
        public virtual RetroParty RetroParty { get; set; }

        [Required, DisplayName("Retro Treaty Code")]
        public int RetroTreatyId { get; set; }
        [DisplayName("Retro Treaty Code")]
        public RetroTreatyBo RetroTreatyBo { get; set; }
        public virtual RetroTreaty RetroTreaty { get; set; }

        public int Status { get; set; }

        [Required, DisplayName("Quarter")]
        public string SoaQuarter { get; set; }

        public string PreviousSoaQuarter { get; set; }

        [DisplayName("Processing Date")]
        public DateTime? ProcessingDate { get; set; }

        [DisplayName("Processing Date")]
        [ValidateDate]
        public string ProcessingDateStr { get; set; }

        [Required, DisplayName("Person In Charge")]
        public int? PersonInChargeId { get; set; }
        public User PersonInCharge { get; set; }
        public UserBo PersonInChargeBo { get; set; }

        public int InvoiceStatus { get; set; }

        public bool IsProfitCommissionData { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        public int ModuleId { get; set; }

        public int No { get; set; }

        public PerLifeSoaViewModel() { Set(); }

        public PerLifeSoaViewModel(PerLifeSoaBo perLifeSoaBo)
        {
            Set(perLifeSoaBo);
        }

        public void Set(PerLifeSoaBo perLifeSoaBo = null)
        {
            ModuleBo moduleBo = ModuleService.FindByController(ModuleBo.ModuleController.PerLifeSoa.ToString());
            if (perLifeSoaBo != null)
            {
                Id = perLifeSoaBo.Id;
                RetroPartyId = perLifeSoaBo.RetroPartyId;
                RetroPartyBo = perLifeSoaBo.RetroPartyBo;
                RetroTreatyId = perLifeSoaBo.RetroTreatyId;
                RetroTreatyBo = perLifeSoaBo.RetroTreatyBo;
                SoaQuarter = perLifeSoaBo.SoaQuarter;
                Status = perLifeSoaBo.Status;
                ModuleId = moduleBo.Id;
                ProcessingDate = perLifeSoaBo.ProcessingDate;
                ProcessingDateStr = perLifeSoaBo.ProcessingDateStr;
                InvoiceStatus = perLifeSoaBo.InvoiceStatus;
                PersonInChargeId = perLifeSoaBo.PersonInChargeId;
                PersonInChargeBo = perLifeSoaBo.PersonInChargeBo;
                IsProfitCommissionData = perLifeSoaBo.IsProfitCommissionData;

                int quarterNo = int.Parse(perLifeSoaBo.SoaQuarter.Substring(6, 1));
                int year = int.Parse(perLifeSoaBo.SoaQuarter.Substring(0, 4));
                PreviousSoaQuarter = string.Format("{0} Q{1}", (quarterNo == 1 ? year - 1 : year), (quarterNo == 1 ? 4 : quarterNo - 1));
            }
            else
            {
                Status = PerLifeSoaBo.StatusPending;
                ModuleId = moduleBo.Id;
            }
        }

        public void Get(ref PerLifeSoaBo perLifeSoaBo)
        {
            perLifeSoaBo.Id = Id;
            perLifeSoaBo.Status = Status;
            perLifeSoaBo.RetroPartyId = RetroPartyId;
            perLifeSoaBo.RetroTreatyId = RetroTreatyId;
            perLifeSoaBo.SoaQuarter = SoaQuarter;
            perLifeSoaBo.InvoiceStatus = InvoiceStatus;
            perLifeSoaBo.PersonInChargeId = PersonInChargeId;
            perLifeSoaBo.IsProfitCommissionData = IsProfitCommissionData;
            if (!string.IsNullOrEmpty(ProcessingDateStr))
            {
                perLifeSoaBo.ProcessingDate = DateTime.Parse(ProcessingDateStr);
            }
        }

        public static Expression<Func<PerLifeSoa, PerLifeSoaViewModel>> Expression()
        {
            return entity => new PerLifeSoaViewModel
            {
                Id = entity.Id,
                RetroPartyId = entity.RetroPartyId,
                RetroParty = entity.RetroParty,
                RetroTreatyId = entity.RetroTreatyId,
                RetroTreaty = entity.RetroTreaty,
                Status = entity.Status,
                SoaQuarter = entity.SoaQuarter,
                InvoiceStatus = entity.InvoiceStatus,
                PersonInChargeId = entity.PersonInChargeId,
                PersonInCharge = entity.PersonInCharge,
                ProcessingDate = entity.ProcessingDate,
                IsProfitCommissionData = entity.IsProfitCommissionData,
            };
        }

        public void ProcessStatusHistory(FormCollection form, int authUserId, ref TrailObject trail)
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

            string statusRemark = form.Get(string.Format("StatusRemark"));
            if (!string.IsNullOrWhiteSpace(statusRemark))
            {
                RemarkBo remarkBo = new RemarkBo
                {
                    ModuleId = ModuleId,
                    ObjectId = Id,
                    Status = Status,
                    Content = statusRemark,
                    CreatedById = authUserId,
                    UpdatedById = authUserId,
                };

                RemarkService.Save(ref remarkBo, ref trail);
            }
        }

        public List<RemarkBo> GetRemarks(FormCollection form)
        {
            int index = 0;
            List<RemarkBo> remarkBos = new List<RemarkBo> { };

            while (!string.IsNullOrWhiteSpace(form.Get(string.Format("r.content[{0}]", index))))
            {
                string createdAtStr = form.Get(string.Format("r.createdAtStr[{0}]", index));
                string status = form.Get(string.Format("r.status[{0}]", index));
                string content = form.Get(string.Format("r.content[{0}]", index));
                string id = form.Get(string.Format("remarkId[{0}]", index));

                DateTime dateTime = DateTime.Parse(createdAtStr, new CultureInfo("fr-FR", false));

                RemarkBo remarkBo = new RemarkBo
                {
                    ModuleId = ModuleId,
                    ObjectId = Id,
                    Status = int.Parse(status),
                    Content = content,
                    CreatedAt = dateTime,
                    UpdatedAt = dateTime,
                };

                if (!string.IsNullOrEmpty(id))
                    remarkBo.Id = int.Parse(id);

                remarkBos.Add(remarkBo);

                index++;
            }

            return remarkBos;
        }

        public void ProcessRemark(FormCollection form, int authUserId, ref TrailObject trail)
        {
            List<RemarkBo> remarkBos = GetRemarks(form);

            foreach (RemarkBo bo in remarkBos)
            {
                RemarkBo remarkBo = bo;
                remarkBo.CreatedById = authUserId;
                remarkBo.UpdatedById = authUserId;

                RemarkService.Save(ref remarkBo, ref trail);
            }
        }
    }
}