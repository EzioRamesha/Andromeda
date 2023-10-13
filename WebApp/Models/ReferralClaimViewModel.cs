using BusinessObject;
using BusinessObject.RiDatas;
using DataAccess.Entities;
using DataAccess.Entities.Identity;
using Services;
using Services.RiDatas;
using Shared;
using Shared.Forms.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WebApp.Models
{
    public class ReferralClaimViewModel : IValidatableObject
    {
        public int Id { get; set; }

        public int? ClaimRegisterId { get; set; }
        public ClaimRegisterBo ClaimRegisterBo { get; set; }

        public int? RiDataWarehouseId { get; set; }
        public RiDataWarehouseBo RiDataWarehouseBo { get; set; }

        public int? ReferralRiDataId { get; set; }

        public int Status { get; set; }

        [DisplayName("Referral ID")]
        public string ReferralId { get; set; }

        [DisplayName("Record Type")]
        public string RecordType { get; set; }

        [DisplayName("Insured Name")]
        [StringLength(128)]
        public string InsuredName { get; set; }

        [DisplayName("Policy No")]
        [StringLength(150)]
        public string PolicyNumber { get; set; }

        [DisplayName("Gender")]
        public string InsuredGenderCode { get; set; }

        [DisplayName("Tobacco Usage")]
        public string InsuredTobaccoUsage { get; set; }

        [DisplayName("Referral Reason")]
        public int? ReferralReasonId { get; set; }

        public string ReferralReason { get; set; }

        [DisplayName("Group Name")]
        [StringLength(255)]
        public string GroupName { get; set; }

        [DisplayName("Date Received Full Documents")]
        public DateTime? DateReceivedFullDocuments { get; set; }
        [DisplayName("Date Received Full Documents"), ValidateDate]
        public string DateReceivedFullDocumentsStr { get; set; }
        public string DateReceivedFullDocumentsTime { get; set; }

        [DisplayName("Date Of Birth")]
        public DateTime? InsuredDateOfBirth { get; set; }
        [DisplayName("Date Of Birth"), ValidateDate]
        public string InsuredDateOfBirthStr { get; set; }

        [DisplayName("IC No")]
        [StringLength(15)]
        public string InsuredIcNumber { get; set; }

        [DisplayName("Date Of Commencement")]
        public DateTime? DateOfCommencement { get; set; }
        [DisplayName("Date Of Commencement"), ValidateDate]
        public string DateOfCommencementStr { get; set; }

        [DisplayName("Ceding Company")]
        public string CedingCompany { get; set; }

        [DisplayName("Claim Code")]
        public string ClaimCode { get; set; }

        [DisplayName("Ceding Plan Code")]
        [StringLength(30)]
        public string CedingPlanCode { get; set; }

        [DisplayName("Sum Insured")]
        public double? SumInsured { get; set; }
        [DisplayName("Sum Insured"), ValidateDouble]
        public string SumInsuredStr { get; set; }

        [DisplayName("AAR Payable")]
        public double? SumReinsured { get; set; }
        [DisplayName("AAR Payable"), ValidateDouble]
        public string SumReinsuredStr { get; set; }

        [DisplayName("Benefit SubCode")]
        [StringLength(255)]
        public string BenefitSubCode { get; set; }

        [DisplayName("Date Of Event")]
        public DateTime? DateOfEvent { get; set; }
        [DisplayName("Date Of Event"), ValidateDate]
        public string DateOfEventStr { get; set; }

        [DisplayName("Risk Quarter")]
        public string RiskQuarter { get; set; }

        [DisplayName("Cause of Event")]
        [StringLength(255)]
        public string CauseOfEvent { get; set; }

        [DisplayName("MLRe Benefit Code")]
        public string MlreBenefitCode { get; set; }

        [DisplayName("Claim Recovery Amount")]
        public double? ClaimRecoveryAmount { get; set; }
        [DisplayName("Claim Recovery Amount"), ValidateDouble]
        public string ClaimRecoveryAmountStr { get; set; }

        [DisplayName("Reinsurance Basis Code")]
        public string ReinsBasisCode { get; set; }

        [DisplayName("Claim Category")]
        public int? ClaimCategoryId { get; set; }

        [DisplayName("Retakaful")]
        public bool IsRgalRetakaful { get; set; }

        [DisplayName("Date Received")]
        public DateTime? ReceivedAt { get; set; }
        [DisplayName("Date Received"), ValidateDate]
        public string ReceivedAtStr { get; set; }
        public string ReceivedAtTime { get; set; }

        [DisplayName("1st Date Responded")]
        public DateTime? RespondedAt { get; set; }
        [DisplayName("1st Date Responded"), ValidateDate]
        public string RespondedAtStr { get; set; }
        public string RespondedAtTime { get; set; }

        [DisplayName("2nd Date Responded")]
        public DateTime? DocRespondedAt { get; set; }
        [DisplayName("2nd Date Responded"), ValidateDate]
        public string DocRespondedAtStr { get; set; }
        public string DocRespondedAtTime { get; set; }

        [DisplayName("1st Turn Around Time")]
        public long? TurnAroundTime { get; set; }

        public int TurnAroundTimeHours { get; set; }
        public int TurnAroundTimeMinutes { get; set; }

        [DisplayName("2nd Turn Around Time")]
        public long? DocTurnAroundTime { get; set; }

        public int DocTurnAroundTimeHours { get; set; }
        public int DocTurnAroundTimeMinutes { get; set; }

        [DisplayName("1st Reason for Delay")]
        public int? DelayReasonId { get; set; }
        public string DelayReason { get; set; }

        [DisplayName("2nd Reason for Delay")]
        public int? DocDelayReasonId { get; set; }

        [DisplayName("Retro")]
        public bool IsRetro { get; set; }

        [DisplayName("Retrocessionaire Name")]
        [StringLength(255)]
        public string RetrocessionaireName { get; set; }

        [DisplayName("Total Retro Amount")]
        public double? RetrocessionaireShare { get; set; }
        [DisplayName("Total Retro Amount"), ValidateDouble]
        public string RetrocessionaireShareStr { get; set; }

        [DisplayName("Referral Reasons to Retro")]
        public int? RetroReferralReasonId { get; set; }

        [DisplayName("Referral Reasons to MLRe from Cedant")]
        public int? MlreReferralReasonId { get; set; }

        [DisplayName("Retro Reviewed By")]
        public string RetroReviewedBy { get; set; }

        [DisplayName("Date Reviewed")]
        public DateTime? RetroReviewedAt { get; set; }
        [DisplayName("Date Reviewed"), ValidateDate]
        public string RetroReviewedAtStr { get; set; }

        [DisplayName("Value Added Service")]
        public bool IsValueAddedService { get; set; }

        [DisplayName("Value Added Service Details")]
        [StringLength(255)]
        public string ValueAddedServiceDetails { get; set; }

        [DisplayName("Claim Case Study")]
        public bool IsClaimCaseStudy { get; set; }

        [DisplayName("Case Study Material Date Completed")]
        public DateTime? CompletedCaseStudyMaterialAt { get; set; }
        [DisplayName("Case Study Material Date Completed"), ValidateDate]
        public string CompletedCaseStudyMaterialAtStr { get; set; }

        [DisplayName("Assessed By")]
        public int? AssessedById { get; set; }

        [DisplayName("Date Assessed")]
        public DateTime? AssessedAt { get; set; }
        [DisplayName("Date Assessed"), ValidateDate]
        public string AssessedAtStr { get; set; }

        [DisplayName("Comments By Assessor")]
        [StringLength(255)]
        public string AssessorComments { get; set; }

        [DisplayName("Reviewed By")]
        public int? ReviewedById { get; set; }

        [DisplayName("Date Reviewed")]
        public DateTime? ReviewedAt { get; set; }
        [DisplayName("Date Reviewed"), ValidateDate]
        public string ReviewedAtStr { get; set; }

        [DisplayName("Comments By Reviewer")]
        [StringLength(255)]
        public string ReviewerComments { get; set; }

        [DisplayName("Claims Decision")]
        public int? ClaimsDecision { get; set; }

        [DisplayName("Claims Decision Date")]
        public DateTime? ClaimsDecisionDate { get; set; }
        [DisplayName("Claims Decision Date"), ValidateDate]
        public string ClaimsDecisionDateStr { get; set; }

        [DisplayName("Assigned By")]
        public int? AssignedById { get; set; }

        [DisplayName("Date Assigned")]
        public DateTime? AssignedAt { get; set; }
        [DisplayName("Date Assigned"), ValidateDate]
        public string AssignedAtStr { get; set; }

        [DisplayName("Treaty Code")]
        public string TreatyCode { get; set; }

        [DisplayName("Treaty Type")]
        public string TreatyType { get; set; }

        [DisplayName("MLRe Share")]
        public double? TreatyShare { get; set; }
        [DisplayName("MLRe Share"), ValidateDouble]
        public string TreatyShareStr { get; set; }

        [DisplayName("Checklist")]
        public string Checklist { get; set; }

        [DisplayName("Person-In-Charge")]
        public int? PersonInChargeId { get; set; }
        public User PersonInCharge { get; set; }
        public string PersonInChargeName { get; set; }

        public string Error { get; set; }

        public int ModuleId { get; set; }

        public ReferralClaimViewModel()
        {
            Set();
        }

        public ReferralClaimViewModel(ReferralClaimBo referralClaimBo)
        {
            Set(referralClaimBo);
        }

        public void Set(ReferralClaimBo bo = null)
        {
            ModuleBo moduleBo = ModuleService.FindByController(ModuleBo.ModuleController.ReferralClaim.ToString());
            ModuleId = moduleBo.Id;
            if (bo != null)
            {
                Id = bo.Id;
                ClaimRegisterId = bo.ClaimRegisterId;
                RiDataWarehouseId = bo.RiDataWarehouseId;
                ReferralRiDataId = bo.ReferralRiDataId;
                Status = bo.Status;
                ReferralId = bo.ReferralId;
                RecordType = bo.RecordType;
                InsuredName = bo.InsuredName;
                PolicyNumber = bo.PolicyNumber;
                InsuredGenderCode = bo.InsuredGenderCode;
                InsuredTobaccoUsage = bo.InsuredTobaccoUsage;
                ReferralReasonId = bo.ReferralReasonId;
                ReferralReason = bo.ReferralReasonBo?.Reason;
                GroupName = bo.GroupName;
                DateReceivedFullDocuments = bo.DateReceivedFullDocuments;
                InsuredDateOfBirth = bo.InsuredDateOfBirth;
                InsuredIcNumber = bo.InsuredIcNumber;
                DateOfCommencement = bo.DateOfCommencement;
                CedingCompany = bo.CedingCompany;
                ClaimCode = bo.ClaimCode;
                CedingPlanCode = bo.CedingPlanCode;
                SumInsured = bo.SumInsured;
                SumReinsured = bo.SumReinsured;
                //BenefitSubCode = bo.BenefitSubCode;
                DateOfEvent = bo.DateOfEvent;
                RiskQuarter = bo.RiskQuarter;
                CauseOfEvent = bo.CauseOfEvent;
                MlreBenefitCode = bo.MlreBenefitCode;
                ClaimRecoveryAmount = bo.ClaimRecoveryAmount;
                ReinsBasisCode = bo.ReinsBasisCode;
                ClaimCategoryId = bo.ClaimCategoryId;
                IsRgalRetakaful = bo.IsRgalRetakaful;
                ReceivedAt = bo.ReceivedAt;
                RespondedAt = bo.RespondedAt;
                DocRespondedAt = bo.DocRespondedAt;
                TurnAroundTime = bo.TurnAroundTime;
                DocTurnAroundTime = bo.DocTurnAroundTime;
                DelayReasonId = bo.DelayReasonId;
                DelayReason = bo.DelayReasonBo?.Reason;
                DocDelayReasonId = bo.DocDelayReasonId;
                IsRetro = bo.IsRetro;
                RetrocessionaireName = bo.RetrocessionaireName;
                RetrocessionaireShare = bo.RetrocessionaireShare;
                RetroReferralReasonId = bo.RetroReferralReasonId;
                MlreReferralReasonId = bo.MlreReferralReasonId;
                RetroReviewedBy = bo.RetroReviewedBy;
                RetroReviewedAt = bo.RetroReviewedAt;
                IsValueAddedService = bo.IsValueAddedService;
                ValueAddedServiceDetails = bo.ValueAddedServiceDetails;
                IsClaimCaseStudy = bo.IsClaimCaseStudy;
                CompletedCaseStudyMaterialAt = bo.CompletedCaseStudyMaterialAt;
                AssessedById = bo.AssessedById;
                AssessedAt = bo.AssessedAt;
                AssessorComments = bo.AssessorComments;
                ReviewedById = bo.ReviewedById;
                ReviewedAt = bo.ReviewedAt;
                ReviewerComments = bo.ReviewerComments;
                ClaimsDecision = bo.ClaimsDecision;
                ClaimsDecisionDate = bo.ClaimsDecisionDate;
                AssignedById = bo.AssignedById;
                AssignedAt = bo.AssignedAt;
                TreatyCode = bo.TreatyCode;
                TreatyType = bo.TreatyType;
                TreatyShare = bo.TreatyShare;
                Checklist = bo.Checklist;
                Error = bo.Error;
                PersonInChargeId = bo.PersonInChargeId;
                PersonInChargeName = bo.PersonInChargeBo?.FullName;

                DateReceivedFullDocumentsStr = bo.DateReceivedFullDocuments?.ToString(Util.GetDateFormat());
                DateReceivedFullDocumentsTime = bo.DateReceivedFullDocuments?.ToString("hh:mm tt");
                InsuredDateOfBirthStr = bo.InsuredDateOfBirth?.ToString(Util.GetDateFormat());
                DateOfCommencementStr = bo.DateOfCommencement?.ToString(Util.GetDateFormat());
                DateOfEventStr = bo.DateOfEvent?.ToString(Util.GetDateFormat());
                ReceivedAtStr = bo.ReceivedAt?.ToString(Util.GetDateFormat());
                ReceivedAtTime = bo.ReceivedAt?.ToString("hh:mm tt");
                RespondedAtStr = bo.RespondedAt?.ToString(Util.GetDateFormat());
                RespondedAtTime = bo.RespondedAt?.ToString("hh:mm tt");
                DocRespondedAtStr = bo.DocRespondedAt?.ToString(Util.GetDateFormat());
                DocRespondedAtTime = bo.DocRespondedAt?.ToString("hh:mm tt");
                RetroReviewedAtStr = bo.RetroReviewedAt?.ToString(Util.GetDateFormat());
                CompletedCaseStudyMaterialAtStr = bo.CompletedCaseStudyMaterialAt?.ToString(Util.GetDateFormat());
                AssessedAtStr = bo.AssessedAt?.ToString(Util.GetDateFormat());
                ReviewedAtStr = bo.ReviewedAt?.ToString(Util.GetDateFormat());
                AssignedAtStr = bo.AssignedAt?.ToString(Util.GetDateFormat());
                ClaimsDecisionDateStr = bo.ClaimsDecisionDate?.ToString(Util.GetDateFormat());

                SumInsuredStr = Util.DoubleToString(bo.SumInsured, 2);
                SumReinsuredStr = Util.DoubleToString(bo.SumReinsured, 2);
                ClaimRecoveryAmountStr = Util.DoubleToString(bo.ClaimRecoveryAmount, 2);
                RetrocessionaireShareStr = Util.DoubleToString(bo.RetrocessionaireShare, 2);
                TreatyShareStr = Util.DoubleToString(bo.TreatyShare, 2);
            }
            else
            {
                Status = ReferralClaimBo.StatusNewCase;
            }
        }

        public ReferralClaimBo FormBo(int authUserId, ReferralClaimBo bo = null)
        {
            if (bo == null)
            {
                bo = new ReferralClaimBo()
                {
                    CreatedById = authUserId
                };
            }

            bool isClosedRegistered = bo.Status == ReferralClaimBo.StatusClosedRegistered;
            if (!isClosedRegistered)
            {
                bo.ReferralReasonId = ReferralReasonId;
                bo.GroupName = GroupName;
                bo.DateOfCommencement = Util.GetParseDateTime(DateOfCommencementStr);
                bo.CedingCompany = CedingCompany;
                bo.ClaimCode = ClaimCode;
                bo.CedingPlanCode = CedingPlanCode;
                bo.SumInsured = Util.StringToDouble(SumInsuredStr);
                bo.SumReinsured = Util.StringToDouble(SumReinsuredStr);
                bo.MlreBenefitCode = MlreBenefitCode;
                bo.ClaimRecoveryAmount = Util.StringToDouble(ClaimRecoveryAmountStr);
                bo.RiskQuarter = RiskQuarter;
                bo.ReinsBasisCode = ReinsBasisCode;
                bo.IsRgalRetakaful = IsRgalRetakaful;
                bo.ReceivedAt = Util.GetParseDateTime(ReceivedAtStr, ReceivedAtTime);
                bo.RespondedAt = Util.GetParseDateTime(RespondedAtStr, RespondedAtTime);
                bo.TurnAroundTime = TurnAroundTime;
                bo.DelayReasonId = DelayReasonId;
                bo.TreatyCode = TreatyCode;
                bo.TreatyType = TreatyType;
                bo.TreatyShare = Util.StringToDouble(TreatyShareStr);
                //bo.Checklist = Checklist;
                bo.PersonInChargeId = PersonInChargeId;

                bo.RiDataWarehouseId = RiDataWarehouseId;
                //bo.ReferralRiDataId = ReferralRiDataId;
            }

            //bo.ClaimRegisterId = ClaimRegisterId;
            bo.Status = Status;
            //bo.ReferralId = ReferralId;
            bo.RecordType = RecordType;
            bo.InsuredName = InsuredName;
            bo.PolicyNumber = PolicyNumber;
            bo.InsuredGenderCode = InsuredGenderCode;
            bo.InsuredTobaccoUsage = InsuredTobaccoUsage;
            bo.DateReceivedFullDocuments = Util.GetParseDateTime(DateReceivedFullDocumentsStr, DateReceivedFullDocumentsTime);
            bo.InsuredDateOfBirth = Util.GetParseDateTime(InsuredDateOfBirthStr);
            bo.InsuredIcNumber = InsuredIcNumber;
            //bo.BenefitSubCode = BenefitSubCode;
            bo.DateOfEvent = Util.GetParseDateTime(DateOfEventStr);
            bo.CauseOfEvent = CauseOfEvent;
            bo.ClaimCategoryId = ClaimCategoryId;
            bo.DocRespondedAt = Util.GetParseDateTime(DocRespondedAtStr, DocRespondedAtTime);
            bo.DocTurnAroundTime = DocTurnAroundTime;
            bo.DocDelayReasonId = DocDelayReasonId;
            bo.IsRetro = IsRetro;
            bo.RetrocessionaireName = IsRetro ? RetrocessionaireName : null;
            bo.RetrocessionaireShare = IsRetro ? Util.StringToDouble(RetrocessionaireShareStr) : null;
            bo.RetroReferralReasonId = IsRetro ? RetroReferralReasonId : null;
            bo.MlreReferralReasonId = IsRetro ? MlreReferralReasonId : null;
            bo.RetroReviewedBy = IsRetro ? RetroReviewedBy : null;
            bo.RetroReviewedAt = IsRetro ? Util.GetParseDateTime(RetroReviewedAtStr) : null;
            bo.IsValueAddedService = IsValueAddedService;
            bo.ValueAddedServiceDetails = IsValueAddedService ? ValueAddedServiceDetails : null;
            bo.IsClaimCaseStudy = IsClaimCaseStudy;
            bo.CompletedCaseStudyMaterialAt = IsClaimCaseStudy ? Util.GetParseDateTime(CompletedCaseStudyMaterialAtStr) : null;
            bo.AssessedById = AssessedById;
            bo.AssessedAt = Util.GetParseDateTime(AssessedAtStr);
            bo.AssessorComments = AssessorComments;
            bo.ReviewedById = ReviewedById;
            bo.ReviewedAt = Util.GetParseDateTime(ReviewedAtStr);
            bo.ReviewerComments = ReviewerComments;
            bo.ClaimsDecision = ClaimsDecision;
            bo.ClaimsDecisionDate = Util.GetParseDateTime(ClaimsDecisionDateStr);
            bo.AssignedById = AssignedById;
            bo.AssignedAt = Util.GetParseDateTime(AssignedAtStr);

            return bo;
        }

        public static Expression<Func<ReferralClaim, ReferralClaimViewModel>> Expression()
        {
            return entity => new ReferralClaimViewModel()
            {
                Id = entity.Id,
                Status = entity.Status,
                ReferralId = entity.ReferralId,
                TurnAroundTime = entity.TurnAroundTime,
                DocTurnAroundTime = entity.DocTurnAroundTime,
                PolicyNumber = entity.PolicyNumber,
                ReceivedAt = entity.ReceivedAt,
                RespondedAt = entity.RespondedAt,
                DocRespondedAt = entity.DocRespondedAt,
                DateReceivedFullDocuments = entity.DateReceivedFullDocuments,
                DateOfCommencement = entity.DateOfCommencement,
                DateOfEvent = entity.DateOfEvent,
                TreatyCode = entity.TreatyCode,
                RecordType = entity.RecordType,
                InsuredName = entity.InsuredName,
                CedingCompany = entity.CedingCompany,
                ClaimRecoveryAmount = entity.ClaimRecoveryAmount,
                PersonInChargeId = entity.PersonInChargeId,
                PersonInCharge = entity.PersonInCharge,
                ClaimsDecision = entity.ClaimsDecision
            };
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> results = new List<ValidationResult>();

            ReceivedAt = Util.GetParseDateTime(ReceivedAtStr, ReceivedAtTime);
            RespondedAt = Util.GetParseDateTime(RespondedAtStr, RespondedAtTime);
            DateReceivedFullDocuments = Util.GetParseDateTime(DateReceivedFullDocumentsStr, DateReceivedFullDocumentsTime);
            DocRespondedAt = Util.GetParseDateTime(DocRespondedAtStr, DocRespondedAtTime);

            List<string> requiredFields = ReferralClaimBo.GetRequiredFields(Status);
            foreach (string propertyName in requiredFields)
            {
                if (this.GetPropertyValue(propertyName) != null)
                    continue;

                string propName = propertyName;
                switch (propertyName)
                {
                    case "ReceivedAt":
                    case "RespondedAt":
                        propName += "Str";
                        break;
                    default:
                        break;
                }

                string displayName = this.GetAttributeFrom<DisplayNameAttribute>(propName).DisplayName;
                results.Add(new ValidationResult(string.Format(MessageBag.Required, displayName), new[] { propName }));
            }

            if (ReceivedAt.HasValue && RespondedAt.HasValue)
            {
                if (ReceivedAt > RespondedAt)
                {
                    results.Add(new ValidationResult("Date Received cannot be later than Date Responded", new[] { nameof(RespondedAtStr) }));
                }
            }
            if (DateReceivedFullDocuments.HasValue && DocRespondedAt.HasValue)
            {
                if (DateReceivedFullDocuments > DocRespondedAt)
                {
                    results.Add(new ValidationResult("Date Received Full Documents cannot be later than Date Responded", new[] { nameof(DocRespondedAtStr) }));
                }
            }
            if (DateReceivedFullDocuments.HasValue && !DocRespondedAt.HasValue)
            {
                results.Add(new ValidationResult(string.Format(MessageBag.Required, "Date Responded"), new[] { nameof(DocRespondedAtStr) }));
            }
            TimeSpan maxTimeSpan = new TimeSpan(Util.GetConfigInteger("ExpectedReferralTurnAroundTime"), 0, 0);
            if (TurnAroundTime > maxTimeSpan.Ticks && DelayReasonId == null)
            {
                results.Add(new ValidationResult(string.Format(MessageBag.Required, "Reason for Delay"), new[] { nameof(DelayReasonId) }));
            }

            return results;
        }
    }

    public class ReferralRiDataFileViewModel
    {
        public int Id { get; set; }

        public int RawFileId { get; set; }

        public RawFile RawFile { get; set; }

        public int Records { get; set; }

        public int UpdatedRecords { get; set; }

        public string Error { get; set; }

        public DateTime CreatedAt { get; set; }

        public int CreatedById { get; set; }

        public User CreatedBy { get; set; }

        public static Expression<Func<ReferralRiDataFile, ReferralRiDataFileViewModel>> Expression()
        {
            return entity => new ReferralRiDataFileViewModel()
            {
                Id = entity.Id,
                RawFileId = entity.RawFileId,
                RawFile = entity.RawFile,
                Records = entity.Records,
                UpdatedRecords = entity.UpdatedRecords,
                Error = entity.Error,
                CreatedAt = entity.CreatedAt,
                CreatedById = entity.CreatedById,
                CreatedBy = entity.CreatedBy,
            };
        }
    }
}