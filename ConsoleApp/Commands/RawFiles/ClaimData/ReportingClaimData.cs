using BusinessObject;
using BusinessObject.Claims;
using DataAccess.Entities.Identity;
using Services;
using Services.Claims;
using Shared;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace ConsoleApp.Commands.RawFiles.ClaimData
{
    public class ReportingClaimData
    {
        public ReportingClaimDataBatch ReportingClaimDataBatch { get; set; }
        public ClaimDataBo ClaimDataBo { get; set; }
        public ClaimRegisterBo ClaimRegisterBo { get; set; }

        public bool Valid { get; set; }

        public const string QuarterRegexPattern = @"^[0-9]{4}\s[Q]{1}[1-4]{1}$";

        public ReportingClaimData(ReportingClaimDataBatch reportingClaimDataBatch, ClaimDataBo claimDataBo)
        {
            ReportingClaimDataBatch = reportingClaimDataBatch;
            ClaimDataBo = claimDataBo;
            Valid = true;
        }

        public void Report()
        {
            if (ReportingClaimDataBatch == null)
                return;
            if (ClaimDataBo == null)
                return;

            if (!ValidateClaimData())
            {
                UpdateClaimDataReportingStatus(ClaimDataBo.ReportingStatusFailed);
                Valid = false;
                return;
            }

            ClaimDataFileBo claimDataFileBo = null;
            if (!ReportingClaimDataBatch.ClaimDataFileBos.IsNullOrEmpty())
                claimDataFileBo = ReportingClaimDataBatch.ClaimDataFileBos.Where(q => q.Id == ClaimDataBo.ClaimDataFileId).FirstOrDefault();

            ClaimRegisterBo = new ClaimRegisterBo
            {
                ClaimDataBatchId = ClaimDataBo.ClaimDataBatchId,
                ClaimDataId = ClaimDataBo.Id,
                ClaimDataConfigId = claimDataFileBo?.ClaimDataConfigId,
                SoaDataBatchId = ReportingClaimDataBatch.ClaimDataBatchBo.SoaDataBatchId,
                PicDaaId = ReportingClaimDataBatch.ClaimDataBatchBo.CreatedById,
                DateOfReported = DateTime.Today,
                CreatedById = ClaimDataBo.CreatedById,
                UpdatedById = ClaimDataBo.UpdatedById,
            };

            for (int i = 1; i <= StandardClaimDataOutputBo.TypeMax; i++)
            {
                string property = StandardClaimDataOutputBo.GetPropertyNameByType(i);
                object value = ClaimDataBo.GetPropertyValue(property);

                switch(i)
                {
                    case StandardClaimDataOutputBo.TypeClaimRecoveryAmt:
                    case StandardClaimDataOutputBo.TypeRetroRecovery1:
                    case StandardClaimDataOutputBo.TypeRetroRecovery2:
                    case StandardClaimDataOutputBo.TypeRetroRecovery3:
                        value = Util.StringToDouble(value, precision: 2);
                        break;
                    case StandardClaimDataOutputBo.TypeDateOfReported:
                        continue;
                    default:
                        break;
                }

                ClaimRegisterBo.SetPropertyValue(property, value);
            }

            if (!string.IsNullOrEmpty(ClaimRegisterBo.TreatyCode) && string.IsNullOrEmpty(ClaimRegisterBo.TreatyType))
            {
                var treatyCodeBo = TreatyCodeService.FindByCode(ClaimRegisterBo.TreatyCode);
                if (treatyCodeBo != null)
                {
                    ClaimRegisterBo.TreatyType = treatyCodeBo.TreatyTypePickListDetailBo?.Code;
                }
            }

            UpdateClaimDataReportingStatus(ClaimDataBo.ReportingStatusSuccess);

            //Save();
        }

        public bool ValidateClaimData()
        {
            bool valid = true;
            Regex rgx = new Regex(QuarterRegexPattern);
            if (!string.IsNullOrEmpty(ClaimDataBo.SoaQuarter) && !rgx.IsMatch(ClaimDataBo.SoaQuarter))
            {
                ClaimDataBo.SetError("SoaQuarter", "Invalid Quarter Format");
                valid = false;
            }

            if (!string.IsNullOrEmpty(ClaimDataBo.RiskQuarter) && !rgx.IsMatch(ClaimDataBo.RiskQuarter))
            {
                ClaimDataBo.SetError("RiskQuarter", "Invalid Quarter Format");
                valid = false;
            }

            if (!string.IsNullOrEmpty(ClaimDataBo.LastTransactionQuarter) && !rgx.IsMatch(ClaimDataBo.LastTransactionQuarter))
            {
                ClaimDataBo.SetError("LastTransactionQuarter", "Invalid Quarter Format");
                valid = false;
            }

            return valid;
        }

        public void UpdateClaimDataReportingStatus(int status)
        {
            var claimData = ClaimDataBo;
            claimData.ReportingStatus = status;
            claimData.UpdatedById = User.DefaultSuperUserId;

            ClaimDataService.Update(ref claimData);
        }

        //public void Save()
        //{
        //    if (ReportingClaimDataBatch.Test)
        //        return;
        //    if (ClaimRegisterBo == null)
        //        return;

        //    var claimRegister = ClaimRegisterBo;
        //    ClaimRegisterService.Create(ref claimRegister);
        //}
    }
}
