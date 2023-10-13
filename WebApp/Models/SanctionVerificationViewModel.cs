using BusinessObject.Sanctions;
using DataAccess.Entities;
using DataAccess.Entities.Identity;
using DataAccess.Entities.Sanctions;
using Services.Sanctions;
using Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace WebApp.Models
{
    public class SanctionVerificationViewModel : IValidatableObject
    {
        public int Id { get; set; }

        [DisplayName("Source")]
        public int SourceId { get; set; }

        public Source Source { get; set; }

        [DisplayName("RI Data")]
        public bool IsRiData { get; set; }

        [DisplayName("Claim Register")]
        public bool IsClaimRegister { get; set; }

        [DisplayName("Referral Claim")]
        public bool IsReferralClaim { get; set; }

        [DisplayName("Type")]
        public int Type { get; set; }

        [DisplayName("Batch ID")]
        public int? BatchId { get; set; }

        [DisplayName("Status")]
        public int Status { get; set; }

        [DisplayName("No of Record(s)")]
        public int Record { get; set; }

        [DisplayName("Unprocessed Record(s)")]
        public int UnprocessedRecords { get; set; }

        [DisplayName("Processing Start Date & Time")]
        public DateTime? ProcessStartAt { get; set; }

        public string ProcessStartAtStr { get; set; }

        [DisplayName("Processing End Date & Time")]
        public DateTime? ProcessEndAt { get; set; }

        public string ProcessEndAtStr { get; set; }

        [DisplayName("Uploaded By")]
        public int CreatedById { get; set; }

        public User CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        public string SelectedIds { get; set; }

        public SanctionVerificationViewModel()
        {
            Set();
        }

        public SanctionVerificationViewModel(SanctionVerificationBo sanctionVerificationBo)
        {
            Set(sanctionVerificationBo);
        }

        public void Set(SanctionVerificationBo bo = null)
        {
            if (bo != null)
            {
                Id = bo.Id;
                SourceId = bo.SourceId;
                IsRiData = bo.IsRiData;
                IsClaimRegister = bo.IsClaimRegister;
                IsReferralClaim = bo.IsReferralClaim;
                Type = bo.Type;
                BatchId = bo.BatchId;
                Status = bo.Status;
                Record = bo.Record;
                UnprocessedRecords = bo.UnprocessedRecords;
                ProcessStartAt = bo.ProcessStartAt;
                ProcessStartAtStr = bo.ProcessStartAt?.ToString(Util.GetDateTimeFormat());
                ProcessEndAt = bo.ProcessEndAt;
                ProcessEndAtStr = bo.ProcessEndAt?.ToString(Util.GetDateTimeFormat());
            }
        }

        public SanctionVerificationBo FormBo(int createdById, int updatedById)
        {
            var bo = new SanctionVerificationBo
            {
                Id = Id,
                SourceId = SourceId,
                SourceBo = SourceService.Find(SourceId),
                IsRiData = IsRiData,
                IsClaimRegister = IsClaimRegister,
                IsReferralClaim = IsReferralClaim,
                Type = Type,
                BatchId = BatchId,
                Status = Status,
                Record = Record,
                UnprocessedRecords = UnprocessedRecords,
                ProcessStartAt = ProcessStartAt,
                ProcessEndAt = ProcessEndAt,
                CreatedById = createdById,
                UpdatedById = updatedById,
            };

            return bo;
        }

        public static Expression<Func<SanctionVerification, SanctionVerificationViewModel>> Expression()
        {
            return entity => new SanctionVerificationViewModel
            {
                Id = entity.Id,
                SourceId = entity.SourceId,
                Source = entity.Source,
                IsRiData = entity.IsRiData,
                IsClaimRegister = entity.IsClaimRegister,
                IsReferralClaim = entity.IsReferralClaim,
                Type = entity.Type,
                BatchId = entity.BatchId,
                Status = entity.Status,
                Record = entity.Record,
                UnprocessedRecords = entity.UnprocessedRecords,
                ProcessStartAt = entity.ProcessStartAt,
                ProcessEndAt = entity.ProcessEndAt,
                CreatedById = entity.CreatedById,
                CreatedBy = entity.CreatedBy,
                CreatedAt = entity.CreatedAt,
            };
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> results = new List<ValidationResult>();

            if (!IsRiData && !IsClaimRegister && !IsReferralClaim)
            {
                results.Add(new ValidationResult("At least 1 database must be selected"));
            }

            return results;
        }
    }

    public class SanctionVerificationDetailListingViewModel
    {
        public int Id { get; set; }

        [DisplayName("Checking Against")]
        public int ModuleId { get; set; }

        public Module Module { get; set; }

        public int ObjectId { get; set; }

        [DisplayName("Date Uploaded")]
        public DateTime? UploadDate { get; set; }

        [DisplayName("Category")]
        public string Category { get; set; }

        [DisplayName("Ceding Company")]
        public string CedingCompany { get; set; }

        [DisplayName("Treaty Code")]
        public string TreatyCode { get; set; }

        [DisplayName("Ceding Plan Code")]
        public string CedingPlanCode { get; set; }

        [DisplayName("Policy Number")]
        public string PolicyNumber { get; set; }

        [DisplayName("Insured Name")]
        public string InsuredName { get; set; }

        [DisplayName("Date of Birth")]
        public DateTime? InsuredDateOfBirth { get; set; }

        [DisplayName("NRIC")]
        public string InsuredIcNumber { get; set; }

        [DisplayName("SOA Quarter")]
        public string SoaQuarter { get; set; }

        [DisplayName("Sum Reinsured")]
        public double? SumReins { get; set; }

        [DisplayName("Claim Amount")]
        public double? ClaimAmount { get; set; }

        [DisplayName("Whitelist")]
        public bool IsWhitelist { get; set; }

        [DisplayName("Exact Match")]
        public bool IsExactMatch { get; set; }

        [DisplayName("Batch ID")]
        public int? BatchId { get; set; }

        [DisplayName("Sanction's Ref Number")]
        public string SanctionRefNumber { get; set; }

        [DisplayName("Sanction's ID Numbers")]
        public string SanctionIdNumber { get; set; }

        [DisplayName("Sanction's Addresses")]
        public string SanctionAddress { get; set; }

        [DisplayName("Line Of Bussiness")]
        public string LineOfBusiness { get; set; }

        [DisplayName("Policy Commenecement Code")]
        public DateTime? PolicyCommencementDate { get; set; }

        [DisplayName("Policy Status Code")]
        public string PolicyStatusCode { get; set; }

        [DisplayName("Risk Coverage End Date")]
        public DateTime? RiskCoverageEndDate { get; set; }
        
        [DisplayName("Gross Premium")]
        public double? GrossPremium { get; set; }

        [DisplayName("Remark")]
        public string Remark { get; set; }

        [DisplayName("Previous Decision")]
        public int? PreviousDecision { get; set; }

        public string PreviousDecisionRemark { get; set; }

        public static Expression<Func<SanctionVerificationDetail, SanctionVerificationDetailListingViewModel>> Expression()
        {
            return entity => new SanctionVerificationDetailListingViewModel
            {
                Id = entity.Id,
                ModuleId = entity.ModuleId,
                Module = entity.Module,
                ObjectId = entity.ObjectId,
                UploadDate = entity.UploadDate,
                Category = entity.Category,
                CedingCompany = entity.CedingCompany,
                TreatyCode = entity.TreatyCode,
                CedingPlanCode = entity.CedingPlanCode,
                PolicyNumber = entity.PolicyNumber,
                InsuredName = entity.InsuredName,
                InsuredDateOfBirth = entity.InsuredDateOfBirth,
                InsuredIcNumber = entity.InsuredIcNumber,
                SoaQuarter = entity.SoaQuarter,
                SumReins = entity.SumReins,
                ClaimAmount = entity.ClaimAmount,
                IsWhitelist = entity.IsWhitelist,
                IsExactMatch = entity.IsExactMatch,
                BatchId = entity.BatchId,
                SanctionRefNumber = entity.SanctionRefNumber,
                SanctionIdNumber = entity.SanctionIdNumber,
                SanctionAddress = entity.SanctionAddress,
                LineOfBusiness = entity.LineOfBusiness,
                PolicyCommencementDate = entity.PolicyCommencementDate,
                PolicyStatusCode = entity.PolicyStatusCode,
                RiskCoverageEndDate = entity.RiskCoverageEndDate,
                GrossPremium = entity.GrossPremium,
                Remark = entity.Remark,
                PreviousDecision = entity.PreviousDecision,
                PreviousDecisionRemark = entity.PreviousDecisionRemark,
            };
        }
    }
}