using BusinessObject;
using BusinessObject.Claims;
using BusinessObject.Identity;
using BusinessObject.RiDatas;
using BusinessObject.Sanctions;
using ConsoleApp.Commands.RawFiles.Sanction;
using DataAccess.Entities.Identity;
using Services;
using Services.Claims;
using Services.Identity;
using Services.RiDatas;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;

namespace ConsoleApp.Commands.RawFiles.ClaimRegister
{
    class ProcessClaimRegister : Command
    {
        public bool Test { get; set; }

        public ModuleBo ModuleBo { get; set; }

        ProcessClaimRegisterBatch ProcessClaimRegisterBatch { get; set; }

        public ClaimRegisterBo ClaimRegisterBo { get; set; }

        public ClaimDataConfigBo ClaimDataConfigBo { get; set; }

        public RiDataWarehouseBo RiDataWarehouseBo { get; set; }

        public ClaimRegisterBo OriginalClaimRegisterBo { get; set; }

        public List<ClaimDataComputationBo> ClaimDataComputationBos { get; set; }

        public List<ClaimDataValidationBo> ClaimDataValidationBos { get; set; }

        public List<string> ProcessingErrors { get; set; }

        public List<string> RedFlagWarnings { get; set; }

        public bool Success { get; set; } = true;

        public bool SuspectedDuplicate { get; set; } = false;

        public ProcessClaimRegister(ProcessClaimRegisterBatch processClaimRegisterBatch, ClaimRegisterBo claimRegisterBo)
        {
            ClaimRegisterBo = claimRegisterBo;
            ProcessClaimRegisterBatch = processClaimRegisterBatch;
            Test = processClaimRegisterBatch.Test;
            ModuleBo = ModuleService.FindByController(ModuleBo.ModuleController.ClaimRegister.ToString());
        }

        public void Process()
        {
            if (!Test)
            {
                UpdateClaimRegisterStatus(ClaimRegisterBo.StatusProcessing, "Processing Claim Register");
            }

            Reset();
            ProcessData();

            try
            {
                DuplicationCheck();
                LoadClaimDataConfig();
                ProcessPostComputation();
                ProcessPostValidation();
            }
            catch
            {
                Success = false;
            }

            try
            {
                ProcessSanctionVerificationChecking();

                ValidateClaimRegister validate = new ValidateClaimRegister(ClaimRegisterBo, false);
                validate.Validate();

                ClaimRegisterBo.HasRedFlag = validate.ClaimRegisterBo.HasRedFlag;
                ClaimRegisterBo.RedFlagWarnings = validate.ClaimRegisterBo.RedFlagWarnings;
            }
            catch
            {
            }

            if (ClaimRegisterBo.ProvisionStatus == ClaimRegisterBo.ProvisionStatusPending)
            {
                var provisionClaimRegister = new ProvisionClaimRegister(ClaimRegisterBo);
                provisionClaimRegister.Provision();

                ClaimRegisterBo = provisionClaimRegister.ClaimRegisterBo;
            }

            if (!Test)
            {
                if (Success)
                {
                    ClaimRegisterBo.RecordType = PickListDetailBo.RecordTypeBordx;
                    if (SuspectedDuplicate)
                    {
                        UpdateClaimRegisterStatus(ClaimRegisterBo.StatusSuspectedDuplication, "Processing Claim Register Success with Suspected Duplicate");
                    }
                    else
                    {
                        ClaimRegisterBo.SetRegisteredValues();
                        UpdateClaimRegisterStatus(ClaimRegisterBo.StatusRegistered, "Processing Claim Register Success");
                    }
                }
                else
                {
                    UpdateClaimRegisterStatus(ClaimRegisterBo.StatusFailed, "Processing Claim Register Failed");
                }
            }
            else
            {
                Save();
            }
        }

        public void Reset()
        {
            ClaimRegisterBo.Errors = null;
            ClaimRegisterBo.ProcessingStatus = ClaimRegisterBo.ProcessingStatusPending;
            ClaimRegisterBo.DuplicationCheckStatus = ClaimRegisterBo.DuplicationCheckStatusPending;
            ClaimRegisterBo.PostComputationStatus = ClaimRegisterBo.PostComputationStatusPending;
            ClaimRegisterBo.PostValidationStatus = ClaimRegisterBo.PostValidationStatusPending;

            ProcessingErrors = new List<string>();
            OriginalClaimRegisterBo = null;
        }

        public void LoadClaimDataConfig()
        {
            ClaimDataConfigBo = ClaimDataConfigService.Find(ClaimRegisterBo.ClaimDataConfigId, false);
            if (ClaimDataConfigBo != null)
            {
                if (ClaimDataConfigBo.Status == ClaimDataConfigBo.StatusApproved)
                {
                    ClaimDataComputationBos = (List<ClaimDataComputationBo>)ClaimDataComputationService.GetByClaimDataConfigId(ClaimDataConfigBo.Id, ClaimDataComputationBo.StepPostComputation);
                    ClaimDataValidationBos = (List<ClaimDataValidationBo>)ClaimDataValidationService.GetByClaimDataConfigId(ClaimDataConfigBo.Id, ClaimDataValidationBo.StepPostValidation);
                }
                else
                {
                    var claimRegister = ClaimRegisterBo;
                    string error = string.Format(MessageBag.ClaimDataConfigStatusInactive, ClaimDataConfigBo.GetStatusName(ClaimDataConfigBo.Status));

                    claimRegister.SetError("PostComputationErrors", error);
                    claimRegister.SetError("PostValidationErrors", error);
                    throw new Exception();
                }
            }
        }


        public void DuplicationCheck()
        {
            var claimRegister = ClaimRegisterBo;

            claimRegister.OriginalClaimRegisterId = null;
            bool hasDuplicate = ClaimRegisterService.HasDuplicate(claimRegister);

            claimRegister.DuplicationCheckStatus = hasDuplicate ? ClaimRegisterBo.DuplicationCheckStatusHasDuplicate : ClaimRegisterBo.DuplicationCheckStatusNoDuplicate;

            if (claimRegister.ClaimTransactionType == PickListDetailBo.ClaimTransactionTypeAdjustment)
            {
                if (!hasDuplicate)
                {
                    claimRegister.SetError("OriginalClaimRegisterError", "No duplicate found for this Adjustment Claim");
                    Success = false;
                    return;
                }

                int count = ClaimRegisterService.CountOriginal(claimRegister);
                if (count == 0)
                {
                    claimRegister.SetError("OriginalClaimRegisterError", "No original claim found with parameters");
                    Success = false;
                }
                else if (count > 1)
                {
                    claimRegister.SetError("OriginalClaimRegisterError", "Multiple original claims found with parameters");
                    SuspectedDuplicate = true;
                }
                else
                {
                    OriginalClaimRegisterBo = ClaimRegisterService.FindOriginal(claimRegister);
                    if (OriginalClaimRegisterBo == null)
                    {
                        claimRegister.SetError("OriginalClaimRegisterError", "Original claim found does not exist");
                        Success = false;
                    }

                    claimRegister.OriginalClaimRegisterId = OriginalClaimRegisterBo.Id;
                    claimRegister.ClaimId = OriginalClaimRegisterBo.ClaimId;
                }
            }
            else
            {
                if (hasDuplicate)
                {
                    Success = false;
                    return;
                }

                SuspectedDuplicate = ClaimRegisterService.HasSuspectedDuplicate(claimRegister);
            }
        }

        public void ProcessData()
        {
            var claimRegister = ClaimRegisterBo;
            try
            {
                ProcessEventClaimCodeMapping(ref claimRegister);
                MatchRiData(ref claimRegister);
                ProcessClaimCodeMapping(ref claimRegister);

                claimRegister.ProcessingStatus = ClaimRegisterBo.ProcessingStatusSuccess;
            }
            catch (Exception e)
            {
                if (!string.IsNullOrEmpty(e.Message))
                    AddProcessingError(e.Message);

                if (e.StackTrace.Length > 0)
                {
                    PrintError(e.Message);
                    if (e.InnerException != null)
                    {
                        PrintError(e.InnerException.Message);
                    }
                    PrintError(e.StackTrace);
                }

                claimRegister.SetError("ProcessingErrors", ProcessingErrors);
                claimRegister.ProcessingStatus = ClaimRegisterBo.ProcessingStatusFailed;

                Success = false;
            }
        }

        public void ProcessPostComputation()
        {
            var claimRegister = ClaimRegisterBo;
            StandardClaimDataOutputEval soe = new StandardClaimDataOutputEval()
            {
                ClaimRegisterBo = claimRegister,
                RiDataWarehouseBo = RiDataWarehouseBo ?? new RiDataWarehouseBo(),
                OriginalClaimRegisterBo = OriginalClaimRegisterBo,
                EnableRiData = true,
                EnableOriginal = OriginalClaimRegisterBo != null,
            };

            List<string> errors = new List<string>();
            foreach (ClaimDataComputationBo computation in ClaimDataComputationBos)
            {
                soe.Condition = computation.Condition;
                soe.Formula = computation.CalculationFormula;

                bool condition = soe.EvalCondition();
                if (condition)
                {
                    string property = StandardClaimDataOutputBo.GetPropertyNameByType(computation.StandardClaimDataOutputBo.Type);
                    var value = soe.EvalFormula();
                    if (value == null)
                    {
                        var msg = string.Format("The computation result is null, Formatted Formula: {0}", soe.FormattedFormula);
                        errors.Add(string.Format("#{0} Formula Error: {1}", computation.SortIndex, msg));
                    }
                    else
                    {
                        try
                        {
                            claimRegister.SetClaimData(computation.StandardClaimDataOutputBo.DataType, property, value);
                            soe.ClaimRegisterBo = claimRegister;
                        }
                        catch (Exception e)
                        {
                            var msg = "Formula Error: " + e.Message;
                            errors.Add(string.Format("#{0} Formula Error: {1}", computation.SortIndex, msg));
                        }
                    }
                }
            }

            if (errors.Count > 0)
            {
                claimRegister.SetError("PostComputationErrors", errors);
                claimRegister.PostComputationStatus = ClaimRegisterBo.PostComputationStatusFailed;
                //throw new Exception();
                Success = false;
            }
            else
            {
                claimRegister.PostComputationStatus = ClaimRegisterBo.PostComputationStatusSuccess;
            }
        }

        public void ProcessPostValidation()
        {
            List<string> errors = new List<string>();
            var claimRegister = ClaimRegisterBo;
            StandardClaimDataOutputEval soe = new StandardClaimDataOutputEval()
            {
                ClaimRegisterBo = claimRegister,
                RiDataWarehouseBo = RiDataWarehouseBo ?? new RiDataWarehouseBo(),
                OriginalClaimRegisterBo = OriginalClaimRegisterBo,
                EnableRiData = true,
                EnableOriginal = OriginalClaimRegisterBo != null,
            };

            bool success = true;
            foreach (ClaimDataValidationBo validation in ClaimDataValidationBos)
            {
                soe.Condition = validation.Condition;
                bool condition = soe.EvalCondition();
                if (condition)
                {
                    success = false;
                    //claimRegister.SetError("PostValidationErrors", string.Format("#{0} {1}", validation.SortIndex, validation.Description));
                    errors.Add(string.Format("#{0} {1}", validation.SortIndex, validation.Description));
                    break;
                }
            }

            // Claim amount validation against AAR
            if (RiDataWarehouseBo != null)
            {
                double claimAmount = claimRegister.CurrencyCode == PickListDetailBo.CurrencyCodeMyr ? claimRegister.ClaimRecoveryAmt ?? 0 : claimRegister.ForeignClaimRecoveryAmt ?? 0;
                if (claimRegister.ClaimTransactionType == PickListDetailBo.ClaimTransactionTypeAdjustment && OriginalClaimRegisterBo != null)
                {
                    claimAmount += ClaimRegisterService.SumRelatedClaimsClaimAmount(OriginalClaimRegisterBo.Id, claimRegister.Id);
                }

                if (claimAmount > RiDataWarehouseBo.Aar)
                {
                    success = false;
                    errors.Add("Total Claim Amount has exceeded AAR from RI Data");
                }
            }
            else
            {
                success = false;
                errors.Add("Claim Amount Validation not performed as RI Data was not found");
            }

            if (!success && errors.Count > 0)
            {
                claimRegister.SetError("PostValidationErrors", errors);
            }
            claimRegister.PostValidationStatus = success ? ClaimRegisterBo.PostValidationStatusSuccess : ClaimRegisterBo.PostValidationStatusFailed;
        }

        public void ProcessSanctionVerificationChecking()
        {
            var checking = new SanctionVerificationChecking()
            {
                ModuleBo = ModuleBo,
                ObjectId = ClaimRegisterBo.Id,
                BatchId = ClaimRegisterBo.ClaimDataBatchId,
                Category = ClaimRegisterBo.FundsAccountingTypeCode,
                InsuredName = ClaimRegisterBo.InsuredName,
                InsuredDateOfBirth = ClaimRegisterBo.InsuredDateOfBirth,
                IsClaimRegister = true,
                CedingCompany = ClaimRegisterBo.CedingCompany,
                TreatyCode = ClaimRegisterBo.TreatyCode,
                CedingPlanCode = ClaimRegisterBo.CedingPlanCode,
                PolicyNumber = ClaimRegisterBo.PolicyNumber,
                SoaQuarter = ClaimRegisterBo.SoaQuarter,
                SumReins = ClaimRegisterBo.AarPayable,
                ClaimAmount = ClaimRegisterBo.ClaimRecoveryAmt,
                LineOfBusiness = RiDataWarehouseBo?.LineOfBusiness,
                PolicyCommencementDate = RiDataWarehouseBo?.IssueDatePol,
                PolicyStatusCode = SanctionVerificationDetailBo.PolicyStatusCodeActive,
                GrossPremium = RiDataWarehouseBo?.GrossPremium
            };

            checking.Check();
            if (checking.IsFound)
            {
                checking.Save();
            }
        }

        public void ProcessEventClaimCodeMapping(ref ClaimRegisterBo claimRegister)
        {
            string title = ClaimRegisterBo.EventClaimCodeMappingTitle;
            AddProcessingError(claimRegister.ValidateEventClaimCodeMapping());

            CedantBo cedantBo = CedantService.FindByCode(claimRegister.CedingCompany);
            if (cedantBo == null)
            {
                AddProcessingError(ClaimRegisterBo.FormatError(string.Format(MessageBag.NoRecordFoundWithName, "CedingCompany"), title));
            }

            if (!IsProcessingSuccess())
                throw new Exception("");

            int cedantId = cedantBo.Id;
            string cedingEventCode = claimRegister.CedingEventCode;
            string cedingClaimType = claimRegister.CedingClaimType;

            var count = EventClaimCodeMappingDetailService.CountByCedantParams(cedantId, cedingEventCode, cedingClaimType, true);
            if (count == 1)
            {
                EventClaimCodeMappingDetailBo detailBo = EventClaimCodeMappingDetailService.FindByCedantParams(cedantId, cedingEventCode, cedingClaimType, true);
                if (detailBo.EventClaimCodeMappingBo == null)
                    throw new Exception(ClaimRegisterBo.FormatError(string.Format(MessageBag.NoRecordFoundWithName, "EventClaimCodeMapping"), title));

                if (detailBo.EventClaimCodeMappingBo.EventCodeBo == null)
                    throw new Exception(ClaimRegisterBo.FormatError(string.Format(MessageBag.NoRecordFoundWithName, "EventCode"), title));

                claimRegister.MlreEventCode = detailBo.EventClaimCodeMappingBo.EventCodeBo.Code;
            }
            else if (count > 1)
            {
                throw new Exception(ClaimRegisterBo.FormatError("Multiple records matched!", title));
            }
            else
            {
                throw new Exception(ClaimRegisterBo.FormatError("No record was matched by parameters!", title));
            }
        }

        public void ProcessClaimCodeMapping(ref ClaimRegisterBo claimRegister)
        {
            string title = ClaimRegisterBo.ClaimCodeMappingTitle;
            AddProcessingError(claimRegister.ValidateClaimCodeMapping());

            if (!IsProcessingSuccess())
                throw new Exception("");

            string mlreEventCode = claimRegister.MlreEventCode;
            string mlreBenefitCode = claimRegister.MlreBenefitCode;

            //var count = ClaimCodeMappingDetailService.CountByParams(mlreEventCode, mlreBenefitCode, true);
            var count = BenefitDetailService.CountByParams(mlreEventCode, mlreBenefitCode);
            if (count == 1)
            {
                BenefitDetailBo detailBo = BenefitDetailService.FindByParams(mlreEventCode, mlreBenefitCode);
                //if (detailBo.ClaimCodeMappingBo == null)
                //    throw new Exception(ClaimRegisterBo.FormatError(string.Format(MessageBag.NoRecordFoundWithName, "ClaimCodeMapping"), title));

                if (detailBo.ClaimCodeBo == null)
                    throw new Exception(ClaimRegisterBo.FormatError(string.Format(MessageBag.NoRecordFoundWithName, "ClaimCode"), title));

                claimRegister.ClaimCode = detailBo.ClaimCodeBo.Code;
                claimRegister.Checklist = ClaimChecklistDetailService.GetJsonByClaimCode(claimRegister.ClaimCode);
            }
            else if (count > 1)
            {
                throw new Exception(ClaimRegisterBo.FormatError("Multiple records matched!", title));
            }
            else
            {
                throw new Exception(ClaimRegisterBo.FormatError("No record was matched by parameters!", title));
            }
        }

        public void MatchRiData(ref ClaimRegisterBo claimRegister)
        {
            if (claimRegister.RiDataWarehouseId.HasValue)
            {
                RiDataWarehouseBo = RiDataWarehouseService.Find(claimRegister.RiDataWarehouseId);
                if (string.IsNullOrEmpty(claimRegister.MlreBenefitCode))
                    claimRegister.MlreBenefitCode = RiDataWarehouseBo?.MlreBenefitCode;
                if (!claimRegister.InsuredDateOfBirth.HasValue)
                    claimRegister.InsuredDateOfBirth = RiDataWarehouseBo?.InsuredDateOfBirth;
                return;
            }

            string title = ClaimRegisterBo.RiDataMappingTitle;
            AddProcessingError(claimRegister.ValidateRiDataMapping());

            if (!IsProcessingSuccess())
                throw new Exception("");

            RiDataWarehouseBo riDataWarehouseBo = null;
            string fundsAccountingTypeCode = claimRegister.FundsAccountingTypeCode;

            List<string> benefitCodes = BenefitDetailService.GetBenefitCodeByEventCode(claimRegister.MlreEventCode);
            if (benefitCodes.IsNullOrEmpty())
                throw new Exception(ClaimRegisterBo.FormatError("No benefit matched with MLRe Event Code!", title));

            //string oriMlreBenefitCode = claimRegister.MlreBenefitCode;
            //claimRegister.MlreBenefitCode = detailBo.BenefitBo.Code;

            int step = 1;
            while (step <= 2 && riDataWarehouseBo == null)
            {
                if (fundsAccountingTypeCode == ProcessClaimRegisterBatch.GroupFundsAccountingTypeBo.Code)
                {
                    riDataWarehouseBo = RiDataWarehouseService.FindByGroupParam(claimRegister, benefitCodes, step);
                    if (riDataWarehouseBo == null)
                    {
                        // Search by old treaty code
                        var oldTreatyCode = TreatyOldCodeService.GetByTreatyCode(claimRegister.TreatyCode);
                        if (!string.IsNullOrEmpty(oldTreatyCode))
                        {
                            ClaimRegisterBo crBo = new ClaimRegisterBo();
                            crBo.TreatyCode = oldTreatyCode;
                            crBo.PolicyNumber = claimRegister.PolicyNumber;
                            crBo.CedingPlanCode = claimRegister.CedingPlanCode;
                            crBo.MlreBenefitCode = claimRegister.MlreBenefitCode;
                            crBo.InsuredName = claimRegister.InsuredName;
                            crBo.DateOfEvent = claimRegister.DateOfEvent;

                            riDataWarehouseBo = RiDataWarehouseService.FindByGroupParam(crBo, benefitCodes, step);
                        }
                    }
                }
                else if (fundsAccountingTypeCode == ProcessClaimRegisterBatch.IndividualFundsAccountingTypeBo.Code)
                {
                    riDataWarehouseBo = RiDataWarehouseService.FindByIndividualParam(claimRegister, benefitCodes, step);
                    if (riDataWarehouseBo == null)
                    {
                        // Search by old treaty code
                        var oldTreatyCode = TreatyOldCodeService.GetByTreatyCode(claimRegister.TreatyCode);
                        if (!string.IsNullOrEmpty(oldTreatyCode))
                        {
                            ClaimRegisterBo crBo = new ClaimRegisterBo();
                            crBo.TreatyCode = oldTreatyCode;
                            crBo.PolicyNumber = claimRegister.PolicyNumber;
                            crBo.CedingPlanCode = claimRegister.CedingPlanCode;
                            crBo.MlreBenefitCode = claimRegister.MlreBenefitCode;
                            crBo.InsuredName = claimRegister.InsuredName;
                            crBo.DateOfEvent = claimRegister.DateOfEvent;

                            riDataWarehouseBo = RiDataWarehouseService.FindByIndividualParam(crBo, benefitCodes, step);
                        }
                    }
                }
                step++;
            }

            if (riDataWarehouseBo != null)
            {
                RiDataWarehouseBo = riDataWarehouseBo;
                claimRegister.RiDataWarehouseId = riDataWarehouseBo.Id;
                claimRegister.MlreBenefitCode = riDataWarehouseBo.MlreBenefitCode;
                if (!claimRegister.InsuredDateOfBirth.HasValue)
                    claimRegister.InsuredDateOfBirth = riDataWarehouseBo.InsuredDateOfBirth;
            }
            else
            {
                throw new Exception(ClaimRegisterBo.FormatError("No record was matched by parameters!", title));
            }
        }

        public void AddProcessingError(object error)
        {
            if (error == null)
                return;

            if (error is string s && !string.IsNullOrEmpty(s))
            {
                ProcessingErrors.Add(s);
            }
            else if (error is List<string> errors)
            {
                if (errors.Count == 0)
                    return;

                ProcessingErrors.AddRange(errors);
            }
        }

        public bool IsProcessingSuccess()
        {
            return ProcessingErrors.Count == 0;
        }

        public void UpdateClaimRegisterStatus(int status, string des)
        {
            TrailObject trail = new TrailObject();

            ClaimRegisterBo.ClaimStatus = status;
            var claimRegister = ClaimRegisterBo;

            Result result = ClaimRegisterService.Update(ref claimRegister, ref trail);
            if (result.Valid)
            {
                StatusHistoryBo statusBo = new StatusHistoryBo
                {
                    ModuleId = ModuleBo.Id,
                    ObjectId = claimRegister.Id,
                    Status = status,
                    CreatedById = User.DefaultSuperUserId,
                    UpdatedById = User.DefaultSuperUserId,
                };
                StatusHistoryService.Create(ref statusBo, ref trail);

                UserTrailBo userTrailBo = new UserTrailBo(
                    claimRegister.Id,
                    des,
                    result,
                    trail,
                    User.DefaultSuperUserId
                );
                UserTrailService.Create(ref userTrailBo);
            }
        }

        public void Save()
        {
            var claimRegister = ClaimRegisterBo;
            claimRegister.UpdatedById = User.DefaultSuperUserId;
            ClaimRegisterService.Update(ref claimRegister);
        }
    }
}
