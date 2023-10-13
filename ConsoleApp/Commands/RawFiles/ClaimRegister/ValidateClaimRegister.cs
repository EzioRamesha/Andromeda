using BusinessObject;
using BusinessObject.RiDatas;
using Newtonsoft.Json;
using Services;
using Services.RiDatas;
using Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Commands.RawFiles.ClaimRegister
{
    public class ValidateClaimRegister
    {
        public ClaimRegisterBo ClaimRegisterBo { get; set; }

        public List<string> RedFlags { get; set; }

        public bool Save { get; set; }

        public ValidateClaimRegister(ClaimRegisterBo claimRegisterBo, bool save = true)
        {
            ClaimRegisterBo = claimRegisterBo;
            if (!string.IsNullOrEmpty(ClaimRegisterBo.RedFlagWarnings))
            {
                RedFlags = ClaimRegisterBo.GetRedFlagWarnings();
            }
            else
            {
                RedFlags = new List<string>();
            }
            Save = save;
        }

        public void Validate()
        {
            RedFlags = RedFlags.Where(q => q == ClaimRegisterBo.RedFlagCreateAdjustment.ToString()).ToList();

            AssessEarlyClaim();
            AssessFacultativeClaim();
            AssessExceedingClaim();
            AssessExceedTreatyShare();
            AssessDuplicate();
            AssessLateNotification();
            AssessCedantLimit();

            if (RedFlags.Count > 0)
            {
                RedFlags = RedFlags.Distinct().ToList();

                ClaimRegisterBo.HasRedFlag = true;
                ClaimRegisterBo.RedFlagWarnings = JsonConvert.SerializeObject(RedFlags);
            }
            else
            {
                ClaimRegisterBo.HasRedFlag = false;
                ClaimRegisterBo.RedFlagWarnings = null;
            }

            if (Save)
            {
                var claimRegisterBo = ClaimRegisterBo;
                ClaimRegisterService.Update(ref claimRegisterBo);
            }
        }

        public void AssessEarlyClaim()
        {
            if (!ValidateEmptyProperty("ReinsEffDatePol") || !ValidateEmptyProperty("DateOfEvent") || !ValidateEmptyProperty("MlreBenefitCode") || !ValidateEmptyProperty("TreatyType"))
                return;

            if (ClaimRegisterBo.TreatyType == PickListDetailBo.TreatyTypeGroup)
                return;

            DateTime dateOfEvent = ClaimRegisterBo.DateOfEvent.Value;
            DateTime dateOfCommencement = ClaimRegisterBo.ReinsEffDatePol.Value;

            if (ClaimRegisterBo.MlreBenefitCode != BenefitBo.CodeHTH && ClaimRegisterBo.MlreBenefitCode != BenefitBo.CodeHCB)
            {
                int monthDiff = ((dateOfEvent.Year - dateOfCommencement.Year) * 12) + (dateOfEvent.Month - dateOfCommencement.Month);
                if (monthDiff < 24)
                {
                    AddWarning(ClaimRegisterBo.RedFlagEarlyClaims);
                }
            }

            if (dateOfCommencement < dateOfEvent)
            {
                AddWarning(ClaimRegisterBo.RedFlagEarlyClaims2);
            }
        }

        public void AssessFacultativeClaim()
        {   
            if (!ValidateEmptyProperty("TreatyType"))
                return;

            if (ClaimRegisterBo.TreatyType == PickListDetailBo.TreatyTypeFacultative)
            {
                AddWarning(ClaimRegisterBo.RedFlagFacClaims);
            }
        }

        public void AssessExceedingClaim()
        {
            if (!ValidateEmptyProperty("MlreBenefitCode") || !ValidateEmptyProperty("ClaimRecoveryAmt"))
                return;

            if (ClaimRegisterBo.MlreBenefitCode == BenefitBo.CodeDTH || ClaimRegisterBo.MlreBenefitCode == BenefitBo.CodeTPD || ClaimRegisterBo.MlreBenefitCode == BenefitBo.CodeCI)
            {
                if (ClaimRegisterBo.ClaimRecoveryAmt > 200000)
                {
                    AddWarning(ClaimRegisterBo.RedFlagClaimExceed200000);
                }
            }

            if (ClaimRegisterBo.MlreBenefitCode == BenefitBo.CodeHTH)
            {
                double totalClaim = ClaimRegisterService.SumClaimByClaimRegister(ClaimRegisterBo);
                if (totalClaim > 50000)
                {
                    AddWarning(ClaimRegisterBo.RedFlagClaimExceed50000);
                }
            }
        }

        public void AssessExceedTreatyShare()
        {
            if (!ValidateEmptyProperty("RiDataWarehouseId") || !ValidateEmptyProperty("ClaimRecoveryAmt") || !ValidateEmptyProperty("ReinsBasisCode"))
                return;

            RiDataWarehouseBo bo = RiDataWarehouseService.Find(ClaimRegisterBo.RiDataWarehouseId.Value);
            if (bo == null || !bo.CurrSumAssured.HasValue)
                return;

            double perc = (ClaimRegisterBo.ClaimRecoveryAmt.Value / bo.CurrSumAssured.Value) * 100;
            if ((ClaimRegisterBo.ReinsBasisCode == PickListDetailBo.ReinsBasisCodeAdvantageProgram && perc > 90) || perc == 100)
            {
                AddWarning(ClaimRegisterBo.RedFlagExceedTreatyShare);
            }
        }

        public void AssessDuplicate()
        {
            int? redFlag = ClaimRegisterService.HasRedFlagDuplicate(ClaimRegisterBo);
            if (redFlag.HasValue)
                AddWarning(redFlag.Value);
        }

        public void AssessLateNotification()
        {
            if (!ValidateEmptyProperty("DateOfReported"))
                return;

            // Not taking the month into account
            int yearDiff = DateTime.Today.Year - ClaimRegisterBo.DateOfReported.Value.Year;

            // Take the month into account
            double yearMonthDiff = DateTime.Today.Year - ClaimRegisterBo.DateOfReported.Value.Year;
            int monthDiff = DateTime.Today.Month - ClaimRegisterBo.DateOfReported.Value.Month;
            if (monthDiff < 0)
            {
                monthDiff += 12;
                yearMonthDiff -= 1;
            }
            yearMonthDiff += (monthDiff / 12);

            if (yearDiff > 6)
            {
                AddWarning(ClaimRegisterBo.RedFlagClaimNotified6Years);
            }
        }

        public void AssessCedantLimit()
        {
            if (!ValidateEmptyProperty("ClaimCode") || !ValidateEmptyProperty("ReinsEffDatePol") || !ValidateEmptyProperty("DateOfEvent") || !ValidateEmptyProperty("FundsAccountingTypeCode") || !ValidateEmptyProperty("ClaimRecoveryAmt"))
                return;

            DateTime dateOfEvent = ClaimRegisterBo.DateOfEvent.Value;
            DateTime dateOfCommencement = ClaimRegisterBo.ReinsEffDatePol.Value;

            int monthDiff = ((dateOfEvent.Year - dateOfCommencement.Year) * 12) + (dateOfEvent.Month - dateOfCommencement.Month);
            bool isContestable = true;
            if (monthDiff > Util.GetConfigInteger("NonContestablePeriod", 24))
            {
                isContestable = false;
            }

            var limit = ClaimAuthorityLimitCedantDetailService.FindByParams(isContestable, ClaimRegisterBo.CedingCompany, ClaimRegisterBo.ClaimCode, ClaimRegisterBo.FundsAccountingTypeCode);
            if (limit != null && ClaimRegisterBo.ClaimRecoveryAmt > limit.ClaimAuthorityLimitValue)
            {
                AddWarning(ClaimRegisterBo.RedFlagExceedCedantLimit);
            }
        }

        public bool ValidateEmptyProperty(string propertyName)
        {
            object value = ClaimRegisterBo.GetPropertyValue(propertyName);
            if (value == null)
            {
                DisplayNameAttribute attribute = ClaimRegisterBo.GetAttributeFrom<DisplayNameAttribute>(propertyName);
                string name = attribute != null ? attribute.DisplayName : propertyName;
                string warning = string.Format(MessageBag.IsEmpty, name);

                if (!RedFlags.Contains(warning))
                {
                    AddWarning(warning);
                }
                return false;
            }
            return true;
        }

        public void AddWarning(object warning)
        {
            RedFlags.Add(warning.ToString());
        }
    }
}
