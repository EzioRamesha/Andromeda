using BusinessObject;
using Services;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Shared.Trails;
using DataAccess.Entities.Identity;
using Shared.DataAccess;
using BusinessObject.Identity;
using Services.Identity;
using Newtonsoft.Json;

namespace ConsoleApp.Commands.RawFiles.ReferralClaim
{
    public class ProcessReferralClaimAssessment : Command
    {
        public bool Test { get; set; }

        public ModuleBo ModuleBo { get; set; }

        ProcessReferralClaimAssessmentBatch ProcessReferralClaimAssessmentBatch { get; set; }

        public ReferralClaimBo ReferralClaimBo { get; set; }

        public List<string> Errors { get; set; }

        public bool Success { get; set; } = true;

        public ProcessReferralClaimAssessment(ProcessReferralClaimAssessmentBatch processReferralClaimAssessmentBatch, ReferralClaimBo referralClaimBo)
        {
            ReferralClaimBo = referralClaimBo;
            ProcessReferralClaimAssessmentBatch = processReferralClaimAssessmentBatch;
            Test = processReferralClaimAssessmentBatch.Test;
            ModuleBo = ModuleService.FindByController(ModuleBo.ModuleController.ReferralClaim.ToString());

            Errors = new List<string>();
        }

        public void Process()
        {
            AssessEarlyClaim();
            AssessFacultativeClaim();
            AssessExceedingClaim();
            AssessDuplicate();
            AssessLateNotification();
            AssessClaimAuthorityLimit();
            AssessCedantLimit();

            Success = Errors.Count == 0;
            ReferralClaimBo.Error = JsonConvert.SerializeObject(Errors);
            if (!Test)
            {
                if (Success)
                    UpdateReferralClaimStatus(ReferralClaimBo.StatusPendingChecklist, "Process Referral Claim Assessment Success");
                else
                    UpdateReferralClaimStatus(ReferralClaimBo.StatusPendingClarification, "Process Referral Claim Assessment Failed");
            }
        }

        public void AssessFacultativeClaim()
        {
            if (!ValidateEmptyProperty("TreatyType"))
                return;

            if (ReferralClaimBo.TreatyType == PickListDetailBo.TreatyTypeFacultative)
            {
                Errors.Add("Facultative Claim");
            }
        }

        public void AssessEarlyClaim()
        {
            if (!ValidateEmptyProperty("DateOfCommencement") || !ValidateEmptyProperty("DateOfEvent") || !ValidateEmptyProperty("MlreBenefitCode") || !ValidateEmptyProperty("TreatyType"))
                return;

            if (ReferralClaimBo.MlreBenefitCode == BenefitBo.CodeHTH || ReferralClaimBo.MlreBenefitCode == BenefitBo.CodeHCB || ReferralClaimBo.TreatyType == PickListDetailBo.TreatyTypeGroup)
                return;

            DateTime dateOfEvent = ReferralClaimBo.DateOfEvent.Value;
            DateTime dateOfCommencement = ReferralClaimBo.DateOfCommencement.Value;

            int monthDiff = ((dateOfEvent.Year - dateOfCommencement.Year) * 12) + (dateOfEvent.Month - dateOfCommencement.Month);
            if (monthDiff < 24)
            {
                Errors.Add("Early Claims less than 24 months from DOC to DOE");
            }
        }
        
        public void AssessExceedingClaim()
        {
            if (!ValidateEmptyProperty("MlreBenefitCode") || !ValidateEmptyProperty("ClaimRecoveryAmount"))
                return;

            if (ReferralClaimBo.MlreBenefitCode == BenefitBo.CodeDTH || ReferralClaimBo.MlreBenefitCode == BenefitBo.CodeTPD || ReferralClaimBo.MlreBenefitCode == BenefitBo.CodeCI)
            {
                if (ReferralClaimBo.ClaimRecoveryAmount > 200000)
                {
                    Errors.Add(string.Format("Claim exceed RM200,000 for {0}", ReferralClaimBo.MlreBenefitCode));
                }
            }

            if (ReferralClaimBo.MlreBenefitCode == BenefitBo.CodeHTH)
            {
                double totalClaim = ReferralClaimService.SumClaimByReferralClaim(ReferralClaimBo);
                if (totalClaim > 50000)
                {
                    Errors.Add(string.Format("Claim exceed RM50,000 for {0}", ReferralClaimBo.MlreBenefitCode));
                }
            }
        }

        public void AssessDuplicate()
        {
            if (ReferralClaimService.HasDuplicate(ReferralClaimBo) || ClaimRegisterService.HasReferralClaimDuplicate(ReferralClaimBo))
            {
                Errors.Add("Duplicate Claims found");
            }
        }

        public void AssessLateNotification()
        {
            if (!ValidateEmptyProperty("ReceivedAt"))
                return;

            // Not taking the month into account
            int yearDiff = DateTime.Today.Year - ReferralClaimBo.ReceivedAt.Value.Year;

            // Take the month into account
            double yearMonthDiff = DateTime.Today.Year - ReferralClaimBo.ReceivedAt.Value.Year;
            int monthDiff = DateTime.Today.Month - ReferralClaimBo.ReceivedAt.Value.Month;
            if (monthDiff < 0)
            {
                monthDiff += 12;
                yearMonthDiff -= 1;
            }
            yearMonthDiff += (monthDiff / 12);

            if (yearDiff > 6)
            {
                Errors.Add("Claim notified above 6 years from DOE");
            }
        }

        public void AssessClaimAuthorityLimit()
        {
            if (!ValidateEmptyProperty("AssessedById") || !ValidateEmptyProperty("ClaimCode"))
                return;

            ClaimAuthorityLimitMLReBo authorityLimitBo = ClaimAuthorityLimitMLReService.FindByUserId(ReferralClaimBo.AssessedById.Value);
            if (authorityLimitBo == null)
            {
                Errors.Add("No Claim Authority Limit found for this Assessor");
                return;
            }

            ClaimAuthorityLimitMLReDetailBo authorityLimitDetailBo = ClaimAuthorityLimitMLReDetailService.FindByClaimAuthorityLimitMLReIdClaimCode(authorityLimitBo.Id, ReferralClaimBo.ClaimCode);
            if (authorityLimitDetailBo == null)
            {
                Errors.Add("No Claim Authority Limit found for this Assessor & Claim Code");
                return;
            }

            if (authorityLimitDetailBo.ClaimAuthorityLimitValue < ReferralClaimBo.ClaimRecoveryAmount)
            {
                Errors.Add("Claim Amount exceeds Assessor Authorized Limit");
            }
        }

        public void AssessCedantLimit()
        {
            if (!ValidateEmptyProperty("ClaimCode") || !ValidateEmptyProperty("DateOfCommencement") || !ValidateEmptyProperty("DateOfEvent") || !ValidateEmptyProperty("ClaimRecoveryAmount"))
                return;

            DateTime dateOfEvent = ReferralClaimBo.DateOfEvent.Value;
            DateTime dateOfCommencement = ReferralClaimBo.DateOfCommencement.Value;

            int monthDiff = ((dateOfEvent.Year - dateOfCommencement.Year) * 12) + (dateOfEvent.Month - dateOfCommencement.Month);
            bool isContestable = true;
            if (monthDiff > Util.GetConfigInteger("NonContestablePeriod", 24))
            {
                isContestable = false;
            }

            var limit = ClaimAuthorityLimitCedantDetailService.FindByParams(isContestable, ReferralClaimBo.CedingCompany, ReferralClaimBo.ClaimCode);
            if (limit != null && ReferralClaimBo.ClaimRecoveryAmount > limit.ClaimAuthorityLimitValue)
            {
                Errors.Add("Claim exceed cedant limit");
            }
        }

        public bool ValidateEmptyProperty(string propertyName)
        {
            object value = ReferralClaimBo.GetPropertyValue(propertyName);
            if (value == null)
            {
                DisplayNameAttribute attribute = ReferralClaimBo.GetAttributeFrom<DisplayNameAttribute>(propertyName);
                string name = attribute != null ? attribute.DisplayName : propertyName;
                Errors.Add(string.Format(MessageBag.IsEmpty, name));
                return false;
            }
            return true;
        }

        public void UpdateReferralClaimStatus(int status, string des)
        {
            TrailObject trail = new TrailObject();
            StatusHistoryBo statusBo = new StatusHistoryBo
            {
                ModuleId = ModuleBo.Id,
                ObjectId = ReferralClaimBo.Id,
                Status = status,
                CreatedById = User.DefaultSuperUserId,
                UpdatedById = User.DefaultSuperUserId,
            };
            StatusHistoryService.Create(ref statusBo, ref trail);

            var referralClaim = ReferralClaimBo;
            referralClaim.UpdatedById = User.DefaultSuperUserId;
            referralClaim.Status = status;

            Result result = ReferralClaimService.Update(ref referralClaim, ref trail);
            UserTrailBo userTrailBo = new UserTrailBo(
                ReferralClaimBo.Id,
                des,
                result,
                trail,
                User.DefaultSuperUserId
            );
            UserTrailService.Create(ref userTrailBo);
        }
    }
}
