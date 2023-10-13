using BusinessObject;
using BusinessObject.Identity;
using BusinessObject.Sanctions;
using DataAccess.Entities.Identity;
using DataAccess.EntityFramework;
using Services;
using Services.Identity;
using Services.Sanctions;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ConsoleApp.Commands.RawFiles.Sanction
{
    public class SanctionVerificationChecking
    {
        public ModuleBo ModuleBo { get; set; }

        public int ObjectId { get; set; }

        public int? BatchId { get; set; }

        public string Category { get; set; }

        public string InsuredName { get; set; }

        public string FormattedName { get; set; }

        public DateTime? InsuredDateOfBirth { get; set; }

        public string InsuredIcNumber { get; set; }

        public bool IsFound { get; set; }

        public bool IsRiData { get; set; } = false;

        public bool IsClaimRegister { get; set; } = false;

        public bool IsReferralClaim { get; set; } = false;

        public bool Validate { get; set; } = false;

        public int[] SanctionIds { get; set; }

        public IList<SanctionBo> SanctionBos { get; set; }

        public IList<SanctionVerificationBo> SanctionVerificationBos { get; set; }

        public List<int> SanctionVerificationDetailIds { get; set; }

        public SanctionIdentityBo SanctionIdentityBo { get; set; }


        // Values to Save
        public string CedingCompany { get; set; }

        public string TreatyCode { get; set; }

        public string CedingPlanCode { get; set; }

        public string PolicyNumber { get; set; }

        public string SoaQuarter { get; set; }

        public double? SumReins { get; set; }

        public double? ClaimAmount { get; set; }

        public string LineOfBusiness { get; set; }

        public DateTime? PolicyCommencementDate { get; set; }

        public string PolicyStatusCode { get; set; }

        public DateTime? RiskCoverageEndDate { get; set; }

        public double? GrossPremium { get; set; }

        public DateTime? ProcessingStartDateTime { get; set; }
        public DateTime? ProcessingEndDateTime { get; set; }


        public SanctionVerificationChecking()
        {
            IsFound = false;
            SanctionVerificationBos = new List<SanctionVerificationBo>();
            ProcessingStartDateTime = DateTime.Now;
        }
        public void Check()
        {
            //if (SearchByIdNumber())
            //{
            //    IsFound = true;
            //}

            //if (!IsFound)
            //{
            //    if (string.IsNullOrEmpty(InsuredName))
            //        return;

            //    SanctionIds = new List<int>();
            //    if (InsuredDateOfBirth.HasValue)
            //    {
            //        SanctionIds = SanctionBirthDateService.GetSanctionIdByBirthDateSource(InsuredDateOfBirth.Value);
            //    }

            //    Regex regex = new Regex(@"[^a-zA-Z0-9 ]+");
            //    FormattedName = (regex.Replace(InsuredName, string.Empty)).ToUpper();

            //    for (int type = SanctionFormatNameBo.TypeSymbolRemoval; type <= SanctionFormatNameBo.TypeMax; type++)
            //    {
            //        if (SearchByName(type))
            //        {
            //            IsFound = true;
            //            break;
            //        }
            //    }
            //}

            StoredProcedure search = new StoredProcedure(StoredProcedure.SanctionVerificationSearch);
            search.AddParameter("Category", string.IsNullOrEmpty(Category) ? null : Category);
            search.AddParameter("InsuredName", InsuredName);
            search.AddParameter("InsuredDateOfBirth", InsuredDateOfBirth.HasValue ? InsuredDateOfBirth : null);
            search.AddParameter("InsuredIcNumber", string.IsNullOrEmpty(InsuredIcNumber) ? null : InsuredIcNumber);
            search.AddParameter("IgnoreSanctionIds", 1);
            search.AddParameter("ResultSanctionIds", isOutputParam: true);

            EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewRiDataRowProcessingInstance(134, "SanctionVerificationChecking");

            connectionStrategy.Execute(() => search.Execute());

            int rule = 0;
            if (!string.IsNullOrEmpty(search.ReturnResult) && int.TryParse(search.ReturnResult, out _))
            {
                rule = int.Parse(search.ReturnResult);
            }
            IsFound = rule != 0;
            if (IsFound)
            {
                string[] sanctionStrIds = Util.ToArraySplitTrim(search.GetOutput("ResultSanctionIds"));
                SanctionIds = Array.ConvertAll(sanctionStrIds, s => int.Parse(s));
                LoadSanctionBos();

                foreach (var sanctionBo in SanctionBos)
                {
                    AddSanctionVerification(sanctionBo, rule);
                }
            }
        }

        //public bool SearchByIdNumber()
        //{
        //    if (string.IsNullOrEmpty(InsuredIcNumber))
        //        return false;

        //    SanctionIds = new List<int>();
        //    SanctionIds = SanctionIdentityService.GetSanctionIdByIdNumber(InsuredIcNumber);

        //    if (SanctionIds != null && SanctionIds.Count() > 0)
        //        return true;

        //    return false;
        //}

        //public bool SearchByName(int type)
        //{
        //    IList<int> sanctionIds;

        //    if (type == SanctionFormatNameBo.TypeGroupKeyword)
        //    {
        //        List<string> splitName = FormattedName.Split(' ').Select(q => q.Trim()).ToList();
        //        sanctionIds = SanctionFormatNameService.GetByGroupNameType(splitName, SanctionIds);
        //    }
        //    else
        //    {
        //        string formattedName = FormattedName.Replace(" ", string.Empty);
        //        sanctionIds = SanctionFormatNameService.GetByNameType(formattedName, type, SanctionIds);
        //    }

        //    if (sanctionIds.Count() != 0)
        //    {
        //        SanctionIds = sanctionIds;
        //        return true;
        //    }

        //    return false;
        //}

        public void LoadSanctionBos()
        {
            SanctionBos = SanctionService.GetBySanctionId(SanctionIds);
            //if (Validate)
            //{
            //    SanctionBos = new List<SanctionBo>();
            //    foreach (var bo in bos)
            //    {
            //        if (!SanctionVerificationDetailService.IsWhitelistOrExactMatch(bo.SanctionBatchBo.SourceId, ModuleBo.Id, ObjectId))
            //        {
            //            SanctionBos.Add(bo);
            //        }
            //    }
            //}
            //else
            //{
            //    SanctionBos = bos;
            //}
        }

        public void AddSanctionVerification(SanctionBo sanctionBo, int rule = 0)
        {
            int sourceId = sanctionBo.SanctionBatchBo.SourceId;
            SanctionVerificationBo bo = SanctionVerificationBos.Where(q => q.SourceId == sourceId).FirstOrDefault();

            if (bo == null)
            {
                var detailBo = new SanctionVerificationDetailBo()
                {
                    ModuleId = ModuleBo.Id,
                    ObjectId = ObjectId,
                    BatchId = BatchId,
                    Rule = rule,
                    Category = Category,
                    CedingCompany = CedingCompany,
                    TreatyCode = TreatyCode,
                    CedingPlanCode = CedingPlanCode,
                    PolicyNumber = PolicyNumber,
                    InsuredName = InsuredName,
                    InsuredDateOfBirth = InsuredDateOfBirth,
                    InsuredIcNumber = InsuredIcNumber,
                    SoaQuarter = SoaQuarter,
                    SumReins = SumReins,
                    ClaimAmount = ClaimAmount,
                    LineOfBusiness = LineOfBusiness,
                    PolicyCommencementDate = PolicyCommencementDate,
                    PolicyStatusCode = PolicyStatusCode,
                    RiskCoverageEndDate = RiskCoverageEndDate,
                    GrossPremium = GrossPremium,
                    SanctionRefNumbers = new List<string>(),
                    SanctionIdNumbers = new List<string>(),
                    SanctionAddresses = new List<string>(),
                    CreatedById = User.DefaultSuperUserId,
                    UpdatedById = User.DefaultSuperUserId,
                };

                SanctionVerificationDetailService.SetPreviousDecision(ref detailBo);
                var detailBos = new List<SanctionVerificationDetailBo>()
                {
                    detailBo
                };

                bo = new SanctionVerificationBo()
                {
                    SourceId = sourceId,
                    IsRiData = IsRiData,
                    IsClaimRegister = IsClaimRegister,
                    IsReferralClaim = IsReferralClaim,
                    Type = SanctionVerificationBo.TypeData,
                    BatchId = BatchId,
                    Status = SanctionVerificationBo.StatusCompleted,
                    Record = 1,
                    UnprocessedRecords = 1,
                    SanctionVerificationDetailBos = detailBos,
                    CreatedById = User.DefaultSuperUserId,
                    UpdatedById = User.DefaultSuperUserId,
                };

                SanctionVerificationBos.Add(bo);
            }

            if (!string.IsNullOrEmpty(sanctionBo.RefNumber))
                (bo.SanctionVerificationDetailBos[0]).SanctionRefNumbers.Add(sanctionBo.RefNumber);
            if (!sanctionBo.SanctionAddressBos.IsNullOrEmpty())
                (bo.SanctionVerificationDetailBos[0]).SanctionIdNumbers.AddRange(sanctionBo.SanctionIdentityBos.Select(q => q.IdNumber).ToList());
            if (!sanctionBo.SanctionAddressBos.IsNullOrEmpty())
                (bo.SanctionVerificationDetailBos[0]).SanctionAddresses.AddRange(sanctionBo.SanctionAddressBos.Select(q => q.Address).ToList());
        }

        public void Merge(SanctionVerificationChecking checking)
        {
            if (!checking.IsFound)
                return;

            foreach (var verification in checking.SanctionVerificationBos)
            {
                SanctionVerificationBo bo = SanctionVerificationBos.Where(q => q.SourceId == verification.SourceId).FirstOrDefault();
                if (bo == null)
                {
                    SanctionVerificationBos.Add(verification);
                }
                else
                {
                    bo.SanctionVerificationDetailBos.AddRange(verification.SanctionVerificationDetailBos);
                    int records = bo.SanctionVerificationDetailBos.Count();
                    bo.Record = records;
                    bo.UnprocessedRecords = records;
                }
            }
        }

        public List<SanctionVerificationDetailBo> ValidateDetails(int sourceId, List<SanctionVerificationDetailBo> bos) 
        {
            if (!Validate)
                return bos;

            List<SanctionVerificationDetailBo> newBos = new List<SanctionVerificationDetailBo>();
            foreach (var bo in bos)
            {
                var dbBo = SanctionVerificationDetailService.FindDataByObject(sourceId, bo.ModuleId, bo.ObjectId);
                if (dbBo != null)
                {
                    SanctionVerificationDetailIds.Add(dbBo.Id);
                }
                else
                {
                    newBos.Add(bo);
                }
            }

            return newBos;
        }

        public void Save()
        {
            if (SanctionVerificationBos.IsNullOrEmpty())
                return;

            SanctionVerificationDetailIds = new List<int>();
            ProcessingEndDateTime = DateTime.Now;

            foreach (var sanctionVerificationBo in SanctionVerificationBos)
            {
                var trail = new TrailObject();
                var bo = sanctionVerificationBo;

                bo.SanctionVerificationDetailBos = ValidateDetails(bo.SourceId, bo.SanctionVerificationDetailBos);
                if (bo.SanctionVerificationDetailBos.IsNullOrEmpty())
                    continue;

                bo.ProcessStartAt = ProcessingStartDateTime;
                bo.ProcessEndAt = ProcessingEndDateTime;

                Result result = SanctionVerificationService.Create(ref bo, ref trail);
                if (result.Valid)
                {
                    foreach (var sanctionVerificationDetailBo in bo.SanctionVerificationDetailBos)
                    {
                        var detailBo = sanctionVerificationDetailBo;
                        detailBo.SanctionVerificationId = bo.Id;

                        detailBo.SanctionRefNumber = string.Join("\\n", detailBo.SanctionRefNumbers);
                        detailBo.SanctionIdNumber = string.Join("\\n", detailBo.SanctionIdNumbers);
                        detailBo.SanctionAddress = string.Join("\\n", detailBo.SanctionAddresses);

                        SanctionVerificationDetailService.Create(ref detailBo, ref trail);

                        SanctionVerificationDetailIds.Add(detailBo.Id);
                    }

                    UserTrailBo userTrailBo = new UserTrailBo(
                        bo.Id,
                        "Create Sanction Verification",
                        result,
                        trail,
                        User.DefaultSuperUserId
                    );
                    UserTrailService.Create(ref userTrailBo);
                }
            }
        }
    }
}
