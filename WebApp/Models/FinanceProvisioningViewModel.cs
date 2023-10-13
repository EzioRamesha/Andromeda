using BusinessObject;
using DataAccess.Entities;
using DataAccess.Entities.Identity;
using Shared;
using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace WebApp.Models
{
    public class FinanceProvisioningViewModel
    {
        public int Id { get; set; }

        [DisplayName("Date")]
        public DateTime Date { get; set; }

        [DisplayName("Date")]
        public string DateStr { get; set; }

        [DisplayName("Status")]
        public int Status { get; set; }

        [DisplayName("No of Claim Provision Records")]
        public int ClaimsProvisionRecord { get; set; }

        [DisplayName("No of Direct Retro Provision Records")]
        public int DrProvisionRecord { get; set; }

        [DisplayName("Claim Provison Amount (MYR)")]
        public double ClaimsProvisionAmount { get; set; }

        [DisplayName("Direct Retro Provison Amount (MYR)")]
        public double DrProvisionAmount { get; set; }

        [DisplayName("Claims Provision SunGL File")]
        public string ClaimsProvisionFile { get; set; }

        [DisplayName("Claims Recoverable (Direct Retro) SunGL File")]
        public string ClaimsRecoverableFile { get; set; }

        public FinanceProvisioningViewModel()
        {
            Set();
        }

        public FinanceProvisioningViewModel(FinanceProvisioningBo financeProvisioningBo)
        {
            Set(financeProvisioningBo);
        }

        public void Set(FinanceProvisioningBo bo = null)
        {
            if (bo != null)
            {
                Id = bo.Id;
                Date = bo.Date;
                DateStr = bo.Date.ToString(Util.GetDateFormat());
                Status = bo.Status;
                ClaimsProvisionRecord = bo.ClaimsProvisionRecord;
                DrProvisionRecord = bo.DrProvisionRecord;
                ClaimsProvisionAmount = bo.ClaimsProvisionAmount;
                DrProvisionAmount = bo.DrProvisionAmount;
            }
        }

        public FinanceProvisioningBo FormBo(int createdById, int updatedById)
        {
            var bo = new FinanceProvisioningBo
            {
                Id = Id,
                Date = DateTime.Parse(DateStr),
                Status = Status,
                ClaimsProvisionRecord = ClaimsProvisionRecord,
                DrProvisionRecord = DrProvisionRecord,
                ClaimsProvisionAmount = ClaimsProvisionAmount,
                DrProvisionAmount = DrProvisionAmount,
                CreatedById = createdById,
                UpdatedById = updatedById,
            };

            return bo;
        }

        public static Expression<Func<FinanceProvisioning, FinanceProvisioningViewModel>> Expression()
        {
            return entity => new FinanceProvisioningViewModel
            {
                Id = entity.Id,
                Date = entity.Date,
                Status = entity.Status,
                ClaimsProvisionRecord = entity.ClaimsProvisionRecord,
                DrProvisionRecord = entity.DrProvisionRecord,
                ClaimsProvisionAmount = entity.ClaimsProvisionAmount,
                DrProvisionAmount = entity.DrProvisionAmount
            };
        }
    }

    public class ProvisioningClaimRegisterListingViewModel
    {
        public int Id { get; set; }

        [DisplayName("Red Flag")]
        public bool HasRedFlag { get; set; }

        [DisplayName("Entry No")]
        public string EntryNo { get; set; }

        [DisplayName("SOA Quarter")]
        public string SoaQuarter { get; set; }

        [DisplayName("Claim ID")]
        public string ClaimId { get; set; }

        [DisplayName("Claim Transaction Type")]
        public string ClaimTransactionType { get; set; }

        [DisplayName("Refferal Case")]
        public bool IsReferralCase { get; set; }

        [DisplayName("RI Data Warehouse")]
        public int? RiDataWarehouseId { get; set; }

        [DisplayName("Record Type")]
        public string RecordType { get; set; }

        [DisplayName("Treaty Code")]
        public string TreatyCode { get; set; }

        [DisplayName("Policy No")]
        public string PolicyNumber { get; set; }

        [DisplayName("Ceding Company")]
        public string CedingCompany { get; set; }

        [DisplayName("Insured Name")]
        public string InsuredName { get; set; }

        [DisplayName("Date of Birth")]
        public DateTime? InsuredDateOfBirth { get; set; }

        [DisplayName("Last Transaction Date")]
        public DateTime? LastTransactionDate { get; set; }

        [DisplayName("Date of Report")]
        public DateTime? DateOfReported { get; set; }

        [DisplayName("Date Notified")]
        public DateTime? CedantDateOfNotification { get; set; }

        [DisplayName("Date Registered")]
        public DateTime? DateOfRegister { get; set; }

        [DisplayName("Date of Commencement")]
        public DateTime? ReinsEffDatePol { get; set; }

        [DisplayName("Date of Event")]
        public DateTime? DateOfEvent { get; set; }

        [DisplayName("Policy Duration")]
        public int? PolicyDuration { get; set; }

        [DisplayName("Target Date to Issue Invoice")]
        public DateTime? TargetDateToIssueInvoice { get; set; }

        [DisplayName("Sum Reinsured (MYR)")]
        public double? SumIns { get; set; }

        [DisplayName("Cause of Event")]
        public string CauseOfEvent { get; set; }

        [DisplayName("Person In-Charge (Claims)")]
        public int? PicClaimId { get; set; }
        public User PicClaim { get; set; }

        [DisplayName("Person In-Charge (DA&A)")]
        public int? PicDaaId { get; set; }
        public User PicDaa { get; set; }

        [DisplayName("Status")]
        public int ClaimStatus { get; set; }

        [DisplayName("Provision Status")]
        public int ProvisionStatus { get; set; }

        [DisplayName("Offset Status")]
        public int OffsetStatus { get; set; }

        public static Expression<Func<ClaimRegister, ProvisioningClaimRegisterListingViewModel>> Expression()
        {
            return entity => new ProvisioningClaimRegisterListingViewModel
            {
                Id = entity.Id,
                HasRedFlag = entity.HasRedFlag,
                EntryNo = entity.EntryNo,
                SoaQuarter = entity.SoaQuarter,
                ClaimId = entity.ClaimId,
                ClaimTransactionType = entity.ClaimTransactionType,
                IsReferralCase = entity.IsReferralCase,
                RiDataWarehouseId = entity.RiDataWarehouseId,
                RecordType = entity.RecordType,
                TreatyCode = entity.TreatyCode,
                PolicyNumber = entity.PolicyNumber,
                CedingCompany = entity.CedingCompany,
                InsuredName = entity.InsuredName,
                InsuredDateOfBirth = entity.InsuredDateOfBirth,
                LastTransactionDate = entity.LastTransactionDate,
                DateOfReported = entity.DateOfReported,
                CedantDateOfNotification = entity.CedantDateOfNotification,
                DateOfRegister = entity.DateOfRegister,
                ReinsEffDatePol = entity.ReinsEffDatePol,
                DateOfEvent = entity.DateOfEvent,
                PolicyDuration = entity.PolicyDuration,
                TargetDateToIssueInvoice = entity.TargetDateToIssueInvoice,
                SumIns = entity.SumIns,
                CauseOfEvent = entity.CauseOfEvent,
                PicClaimId = entity.PicClaimId,
                PicClaim = entity.PicClaim,
                PicDaaId = entity.PicDaaId,
                PicDaa = entity.PicDaa,
                ClaimStatus = entity.ClaimStatus,
                ProvisionStatus = entity.ProvisionStatus,
                OffsetStatus = entity.OffsetStatus,
            };
        }
    }
}