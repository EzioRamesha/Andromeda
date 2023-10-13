using BusinessObject;
using BusinessObject.RiDatas;
using BusinessObject.Sanctions;
using ConsoleApp.Commands.RawFiles.Sanction;
using DataAccess.EntityFramework;
using Newtonsoft.Json.Linq;
using Services;
using Services.RiDatas;
using Shared;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading;
using Row = Shared.ProcessFile.Row;

namespace ConsoleApp.Commands.RawFiles.RiData
{
    public class ProcessRowRiData : Command
    {
        public ProcessRiDataBatch ProcessRiDataBatch { get; set; }
        public string Quarter { get; set; }
        public Row Row { get; set; }
        public List<RiDataBo> RiDataBos { get; set; }
        public LogRiDataFile LogRiDataFile { get; set; }
        public int CedantId { get; set; }

        public int ProcessRetries { get; set; }
        public int BufferWithinRetries { get; set; }

        //public int RiDataBatchId { get; set; }


        public SanctionVerificationChecking SanctionVerificationChecking { get; set; }

        public ProcessRowRiData(ProcessRiDataBatch processRiDataBatch, MappingRiData mappingRiData)
        {
            ProcessRiDataBatch = processRiDataBatch;
            CedantId = processRiDataBatch.RiDataBatchBo.CedantId;
            Quarter = processRiDataBatch.RiDataBatchBo.Quarter;
            Row = mappingRiData.Row;
            RiDataBos = mappingRiData.RiDataBos;
            LogRiDataFile = null;
            ProcessRetries = Util.GetConfigInteger("RiDataProcessRetries", 3);
            BufferWithinRetries = Util.GetConfigInteger("RiDataBufferWithinRetriesInMilliseconds", 1000);
            Title = processRiDataBatch.Title;
            //RiDataBatchId = processRiDataBatch.RiDataBatchBo.Id;
        }

        public void Process()
        {
            if (RiDataBos.IsNullOrEmpty())
                return;

            try
            {
                LogRiDataFile = new LogRiDataFile(ProcessRiDataBatch.RiDataFileBo);
                LogRiDataFile.ProcessDataRow++;
                LogRiDataFile.RiDataCount += RiDataBos.Count;

                foreach (var riDataBo in RiDataBos)
                    if (riDataBo.MappingValidate)
                        riDataBo.MappingStatus = RiDataBo.MappingStatusSuccess;
                    else
                        riDataBo.MappingStatus = RiDataBo.MappingStatusFailed;

                //ProcessFixedValue();
                //ProcessOverrideProperties();
                //ProcessRemoveSalutation();
                //ProcessRiDataCorrection();

                //ProcessValidateReportPeriodDate(ProcessRiDataBatch.RiDataBatchBo.Quarter);

                //ProcessRiDataComputation(RiDataComputationBo.StepPreComputation1); // Pre-Computation 1
                //ProcessRiDataComputation(RiDataComputationBo.StepPreComputation2); // Pre-Computation 2
                //ProcessRiDataPreValidation(RiDataPreValidationBo.StepPreValidation); // Pre-Validation

                try
                {
                    ProcessFixedValue();
                }
                catch (Exception e)
                {
                    if (e is RetryLimitExceededException dex)
                    {
                        PrintError("Retry exceed limit at ProcessFixedValue(): " + dex.ToString());
                        throw dex;
                    }
                    else
                    {
                        PrintError("Error at ProcessFixedValue(): " + e.ToString());
                        throw e;
                    }
                }

                try
                {
                    ProcessOverrideProperties();
                }
                catch (Exception e)
                {
                    if (e is RetryLimitExceededException dex)
                    {
                        PrintError("Retry exceed limit at ProcessOverrideProperties(): " + dex.ToString());
                        throw dex;
                    }
                    else
                    {
                        PrintError("Error at ProcessOverrideProperties(): " + e.ToString());
                        throw e;
                    }
                }

                try
                {
                    ProcessRemoveSalutation();
                }
                catch (Exception e)
                {
                    if (e is RetryLimitExceededException dex)
                    {
                        PrintError("Retry exceed limit at ProcessRemoveSalutation(): " + dex.ToString());
                        throw dex;
                    }
                    else
                    {
                        PrintError("Error at ProcessRemoveSalutation(): " + e.ToString());
                        throw e;
                    }
                }

                try
                {
                    ProcessRiDataCorrection();
                }
                catch (Exception e)
                {
                    if (e is RetryLimitExceededException dex)
                    {
                        PrintError("Retry exceed limit at at ProcessRiDataCorrection(): " + dex.ToString());
                        throw dex;
                    }
                    else
                    {
                        PrintError("Error at ProcessRiDataCorrection(): " + e.ToString());
                        throw e;
                    }
                }

                try
                {
                    ProcessRiDataComputation(RiDataComputationBo.StepPreComputation1); // Pre-Computation 1
                }
                catch (Exception e)
                {
                    if (e is RetryLimitExceededException dex)
                    {
                        PrintError("Retry exceed limit at at ProcessRiDataComputation(RiDataComputationBo.StepPreComputation1): " + dex.ToString());
                        throw dex;
                    }
                    else
                    {
                        PrintError("Error at ProcessRiDataComputation(RiDataComputationBo.StepPreComputation1): " + e.ToString());
                        throw e;
                    }
                }

                try
                {
                    ProcessRiDataComputation(RiDataComputationBo.StepPreComputation2); // Pre-Computation 2
                }
                catch (Exception e)
                {
                    if (e is RetryLimitExceededException dex)
                    {
                        PrintError("Retry exceed limit at at ProcessRiDataComputation(RiDataComputationBo.StepPreComputation2): " + dex.ToString());
                        throw dex;
                    }
                    else
                    {
                        PrintError("Error at ProcessRiDataComputation(RiDataComputationBo.StepPreComputation2): " + e.ToString());
                        throw e;
                    }
                }

                try
                {
                    ProcessRiDataPreValidation(RiDataPreValidationBo.StepPreValidation); // Pre-Validation
                }
                catch (Exception e)
                {
                    if (e is RetryLimitExceededException dex)
                    {
                        PrintError("Retry exceed limit at at ProcessRiDataPreValidation(RiDataPreValidationBo.StepPreValidation): " + dex.ToString());
                        throw dex;
                    }
                    else
                    {
                        PrintError("Error at ProcessRiDataPreValidation(RiDataPreValidationBo.StepPreValidation): " + e.ToString());
                        throw e;
                    }
                }

                Save();

                return;
            }
            catch (Exception e)
            {
                // Create new log file with random number to avoid logging to the same file error
                //if (retries == 0)
                //{
                //    Random randomNumber = new Random();

                //    Title = Title + "Id" + RiDataBatchId + "_" + randomNumber.Next(9999);
                //}
                var message = e.Message;
                if (e is DbEntityValidationException dbEx)
                    message = Util.CatchDbEntityValidationException(dbEx).ToString();

                if (e is RetryLimitExceededException dex)
                    message = dex.ToString();

                if (Row != null && Row.RowIndex != 0)
                {
                    PrintError(String.Format("Pre Process Error at row: {0}, Error: {1}", Row.RowIndex, message));
                    //PrintMessage("Pre Process Error at row: " + Row.RowIndex + " Error: " + e.ToString());
                }
                else
                {
                    PrintError(String.Format("Pre Process for row Error: {0}", message));
                    //PrintMessage("Pre Process for row Error: " + e.ToString());
                }

                if (e.InnerException != null)
                {
                    if (e.InnerException is DbEntityValidationException idbEx)
                        PrintError(Util.CatchDbEntityValidationException(idbEx).ToString());
                    PrintError(e.InnerException.Message);
                }

                if (e.StackTrace.Length > 0)
                {
                    PrintError("e.StackTrace: " + e.StackTrace);
                }

                throw e;
            }

        }

        public void ProcessPost()
        {
            if (RiDataBos.IsNullOrEmpty())
                return;

            try
            {
                LogRiDataFile = new LogRiDataFile(ProcessRiDataBatch.RiDataFileBo);
                LogRiDataFile.ProcessDataRow++;
                LogRiDataFile.RiDataCount += RiDataBos.Count;

                // Reset the Erorr message
                foreach (var riDataBo in RiDataBos)
                {
                    riDataBo.Errors = null;
                }

                //ProcessRiDataComputation(RiDataComputationBo.StepPostComputation); // Post-Computation
                //ProcessRiDataPreValidation(RiDataPreValidationBo.StepPostValidation); // Post-Validation

                try
                {
                    ProcessRiDataComputation(RiDataComputationBo.StepPostComputation); // Pre-Validation
                }
                catch (RetryLimitExceededException dex)
                {
                    PrintError("Retry exceed limit at ProcessFixedValue(): " + dex.ToString());
                    throw dex;
                }
                catch (Exception e)
                {
                    PrintError("Error at ProcessRiDataComputation(RiDataComputationBo.StepPostComputation): " + e.ToString());
                    throw e;
                }

                try
                {
                    ProcessRiDataPreValidation(RiDataPreValidationBo.StepPostValidation); // Post-Validation
                }
                catch (RetryLimitExceededException dex)
                {
                    PrintError("Retry exceed limit at ProcessFixedValue(): " + dex.ToString());
                    throw dex;
                }
                catch (Exception e)
                {
                    PrintError("Error at ProcessRiDataPreValidation(RiDataPreValidationBo.StepPostValidation): " + e.ToString());
                    throw e;
                }

                Update();

                return;
            }
            catch (Exception e)
            {
                var message = e.Message;
                if (e is DbEntityValidationException dbEx)
                    message = Util.CatchDbEntityValidationException(dbEx).ToString();

                if (Row != null && Row.RowIndex != 0)
                {
                    PrintError(String.Format("Post Process Error at row: {0}, Error: {1}", Row.RowIndex, message));
                    //PrintMessage("Post Process Error at row: " + Row.RowIndex + " Error: " + e.ToString());
                }
                else
                {
                    PrintError(String.Format("Post Process for row Error: {0}", e.ToString()));
                    //PrintMessage("Post Process for row Error: " + e.ToString());
                }

                if (e.InnerException != null)
                {
                    if (e.InnerException is DbEntityValidationException idbEx)
                        PrintError(Util.CatchDbEntityValidationException(idbEx).ToString());
                    PrintError(e.InnerException.Message);
                }

                if (e.StackTrace.Length > 0)
                {
                    PrintError("e.StackTrace: " + e.StackTrace);
                }

                throw e;

            }
        }

        public void ProcessFixedValue()
        {
            LogRiDataFile.SwFixedValue.Start();

            var mappings = ProcessRiDataBatch.RiDataMappingBos.Where(q => q.TransformFormula == RiDataMappingBo.TransformFormulaFixedValue).ToList();
            foreach (var mapping in mappings)
            {
                var value = mapping.DefaultValue;
                if (string.IsNullOrEmpty(value))
                    value = null;

                if (mapping.Row.HasValue && mapping.Row > 0)
                {
                    // Specific row
                    var riDataBo = RiDataBos[mapping.Row.Value - 1];
                    bool success = riDataBo.ProcessRiData(mapping, value, out StandardOutputBo so, out object before, out object after, out string error);
                    riDataBo.Log.FixedValue.LineDelimiter();
                    riDataBo.Log.FixedValue.Property(so.Property);
                    riDataBo.Log.FixedValue.Detail("SortIndex", mapping.SortIndex);
                    riDataBo.Log.FixedValue.Detail("RawColumnName", mapping.RawColumnName);
                    riDataBo.Log.FixedValue.Detail("Length", mapping.Length);
                    riDataBo.Log.FixedValue.Detail("TransformFormulaName", mapping.TransformFormulaName);
                    if (success)
                    {
                        riDataBo.Log.FixedValue.Before(before);
                        riDataBo.Log.FixedValue.After(after);
                    }
                    else
                    {
                        riDataBo.Log.FixedValue.Error(error);
                        LogRiDataFile.MappingErrorCount++;
                    }
                }
                else if (!RiDataBos.IsNullOrEmpty() && RiDataBos.Count > 0)
                {
                    // All RI Data
                    foreach (var riDataBo in RiDataBos)
                    {
                        bool success = riDataBo.ProcessRiData(mapping, value, out StandardOutputBo so, out object before, out object after, out string error);
                        riDataBo.Log.FixedValue.LineDelimiter();
                        riDataBo.Log.FixedValue.Property(so.Property);
                        riDataBo.Log.FixedValue.Detail("SortIndex", mapping.SortIndex);
                        riDataBo.Log.FixedValue.Detail("RawColumnName", mapping.RawColumnName);
                        riDataBo.Log.FixedValue.Detail("Length", mapping.Length);
                        riDataBo.Log.FixedValue.Detail("TransformFormulaName", mapping.TransformFormulaName);
                        if (success)
                        {
                            riDataBo.Log.FixedValue.Before(before);
                            riDataBo.Log.FixedValue.After(after);
                        }
                        else
                        {
                            riDataBo.Log.FixedValue.Error(error);
                            LogRiDataFile.MappingErrorCount++;
                        }
                    }
                }
            }

            LogRiDataFile.SwFixedValue.Stop();
        }

        public void ProcessOverrideProperties()
        {
            LogRiDataFile.SwOverrideProperties.Start();

            var obj = JObject.Parse(ProcessRiDataBatch.RiDataFileBo.OverrideProperties);
            foreach (JProperty prop in obj.Properties())
            {
                var type = int.Parse(prop.Name);
                var value = prop.Value.ToString();
                var so = StandardOutputBo.GetByType(type);
                foreach (var riDataBo in RiDataBos)
                {
                    riDataBo.Log.OverrideProperty.LineDelimiter();
                    riDataBo.Log.OverrideProperty.Property(so.Property);
                    try
                    {
                        if (!string.IsNullOrEmpty(value))
                        {
                            riDataBo.Log.OverrideProperty.Before(riDataBo.GetPropertyValue(so.Property));
                            riDataBo.SetRiData(so.DataType, so.Property, value);
                            riDataBo.Log.OverrideProperty.After(riDataBo.GetPropertyValue(so.Property));
                        }
                        else
                        {
                            riDataBo.Log.OverrideProperty.ValueIsNull();
                        }
                    }
                    catch (Exception e)
                    {
                        var error = string.Format("Override Property Error: {0}", e.Message);
                        riDataBo.Log.OverrideProperty.Add(error);
                        riDataBo.SetError(so.Property, error);
                    }
                }
            }

            LogRiDataFile.SwOverrideProperties.Stop();
        }

        public void ProcessRemoveSalutation()
        {
            if (ProcessRiDataBatch == null)
                return;
            if (ProcessRiDataBatch.CacheService == null)
                return;
            if (ProcessRiDataBatch.CacheService.SalutationBos.IsNullOrEmpty())
                return;

            foreach (var riDataBo in RiDataBos)
            {
                var removed = false;
                if (string.IsNullOrEmpty(riDataBo.InsuredName) && string.IsNullOrEmpty(riDataBo.InsuredName2nd))
                    continue;

                riDataBo.Log.RemoveSalutation.LineDelimiter();
                if (!string.IsNullOrEmpty(riDataBo.InsuredName))
                    riDataBo.Log.RemoveSalutation.Property("InsuredName");
                if (!string.IsNullOrEmpty(riDataBo.InsuredName2nd))
                    riDataBo.Log.RemoveSalutation.Property("InsuredName2nd");

                // NOTED: THIS IS FOR TESTING PURPOSE AND DO NOT UNCOMMENT IT
                //riDataBo.InsuredName = "Ms. " + "TEST NAME";

                foreach (var salutation in ProcessRiDataBatch.CacheService.SalutationBos)
                {
                    string formattedSalutationWithSpace = salutation.Name.ToLower() + ' ';
                    string formattedSalutationWithDot = salutation.Name.ToLower().Last() == '.' ? salutation.Name.ToLower() : salutation.Name.ToLower() + '.';
                    if (!string.IsNullOrEmpty(riDataBo.InsuredName) &&
                        (riDataBo.InsuredName.ToLower().StartsWith(formattedSalutationWithSpace) || riDataBo.InsuredName.ToLower().StartsWith(formattedSalutationWithDot))
                    )
                    {
                        riDataBo.Log.RemoveSalutation.Add("==");
                        riDataBo.Log.RemoveSalutation.Detail("Salutation", salutation.Name);
                        riDataBo.Log.RemoveSalutation.Before(riDataBo.InsuredName);
                        riDataBo.InsuredName = riDataBo.InsuredName.Remove(0, salutation.Name.Length).Trim();

                        if (riDataBo.InsuredName.StartsWith("."))
                            riDataBo.InsuredName = riDataBo.InsuredName.Remove(0, 1).Trim();

                        riDataBo.Log.RemoveSalutation.After(riDataBo.InsuredName);
                        removed = true;
                    }
                    if (!string.IsNullOrEmpty(riDataBo.InsuredName2nd) &&
                        (riDataBo.InsuredName2nd.ToLower().StartsWith(formattedSalutationWithSpace) || riDataBo.InsuredName2nd.ToLower().StartsWith(formattedSalutationWithDot))
                    )
                    {
                        riDataBo.Log.RemoveSalutation.Add("==");
                        riDataBo.Log.RemoveSalutation.Detail("Salutation", salutation.Name);
                        riDataBo.Log.RemoveSalutation.Before(riDataBo.InsuredName2nd);
                        riDataBo.InsuredName2nd = riDataBo.InsuredName2nd.Remove(0, salutation.Name.Length).Trim();

                        if (riDataBo.InsuredName2nd.StartsWith("."))
                            riDataBo.InsuredName2nd = riDataBo.InsuredName2nd.Remove(0, 1).Trim();

                        riDataBo.Log.RemoveSalutation.After(riDataBo.InsuredName2nd);
                        removed = true;
                    }
                }

                if (!removed)
                    riDataBo.Log.RemoveSalutation.Add("No salutation was removed ");
            }
        }

        public void ProcessRiDataCorrection()
        {
            var on = ProcessRiDataBatch.RiDataConfigBo.RiDataFileConfig.IsDataCorrection; // Refer from RiDataConfigs.Configs (NOT RiDataFiles.Configs)
            if (on)
                LogRiDataFile.SwDataCorrection.Start();

            foreach (var riDataBo in RiDataBos)
            {
                GlobalProcessRowRiDataConnectionStrategy.SetRiDataId(riDataBo.Id);
                if (!on)
                {
                    riDataBo.Log.DataCorrection.Failed("RiDataConfigBo.RiDataFileConfig.IsDataCorrection FALSE");
                    continue;
                }

                if (string.IsNullOrEmpty(riDataBo.PolicyNumber))
                    continue;
                // InsuredRegisterNo is optional
                //if (string.IsNullOrEmpty(riDataBo.InsuredRegisterNo))
                //    continue;

                riDataBo.Log.DataCorrection.LineDelimiter();

                RiDataCorrectionBo riDataCorrection = null;
                if (!string.IsNullOrEmpty(riDataBo.InsuredRegisterNo))
                {
                    riDataCorrection = RiDataCorrectionService.FindByCedantIdTreatyCodePolicyRegNo(
                        ProcessRiDataBatch.RiDataConfigBo.CedantId,
                        riDataBo.PolicyNumber,
                        riDataBo.InsuredRegisterNo,
                        riDataBo.TreatyCode
                    );

                    if (riDataCorrection == null)
                    {
                        riDataCorrection = RiDataCorrectionService.FindByCedantIdTreatyCodePolicyRegNo(
                            ProcessRiDataBatch.RiDataConfigBo.CedantId,
                            riDataBo.PolicyNumber,
                            riDataBo.InsuredRegisterNo
                        );
                    }
                }
                else
                {
                    // Try to find the record without InsuredRegisterNo
                    riDataCorrection = RiDataCorrectionService.FindByCedantIdTreatyCodePolicyRegNo(
                        ProcessRiDataBatch.RiDataConfigBo.CedantId,
                        riDataBo.PolicyNumber,
                        treatyCode: riDataBo.TreatyCode
                    );
                }
                if (riDataCorrection == null)
                {
                    riDataCorrection = RiDataCorrectionService.FindByCedantIdTreatyCodePolicyRegNo(
                        ProcessRiDataBatch.RiDataConfigBo.CedantId,
                        riDataBo.PolicyNumber
                    );
                }
                if (riDataCorrection != null)
                {
                    riDataBo.Log.DataCorrection.Found();
                    riDataBo.Log.DataCorrection.MappedValue("InsuredGenderCode", riDataCorrection.InsuredGenderCodePickListDetailBo?.Code);
                    riDataBo.Log.DataCorrection.MappedValue("InsuredDateOfBirth", riDataCorrection.InsuredDateOfBirth);
                    riDataBo.Log.DataCorrection.MappedValue("InsuredName", riDataCorrection.InsuredName);
                    riDataBo.Log.DataCorrection.MappedValue("CampaignCode", riDataCorrection.CampaignCode);
                    riDataBo.Log.DataCorrection.MappedValue("ReinsBasisCode", riDataCorrection.ReinsBasisCodePickListDetailBo?.Code);
                    riDataBo.Log.DataCorrection.MappedValue("ApLoading", riDataCorrection.ApLoading);

                    if (riDataCorrection.InsuredGenderCodePickListDetailBo != null)
                        riDataBo.InsuredGenderCode = riDataCorrection.InsuredGenderCodePickListDetailBo.Code;

                    if (riDataCorrection.InsuredDateOfBirth != null)
                        riDataBo.InsuredDateOfBirth = riDataCorrection.InsuredDateOfBirth;

                    if (!string.IsNullOrEmpty(riDataCorrection.InsuredName))
                        riDataBo.InsuredName = riDataCorrection.InsuredName;

                    if (!string.IsNullOrEmpty(riDataCorrection.CampaignCode))
                        riDataBo.CampaignCode = riDataCorrection.CampaignCode;

                    if (riDataCorrection.ReinsBasisCodePickListDetailBo != null)
                        riDataBo.ReinsBasisCode = riDataCorrection.ReinsBasisCodePickListDetailBo.Code;

                    if (riDataCorrection.ApLoading.HasValue)
                        riDataBo.ApLoading = riDataCorrection.ApLoading;
                }
                else
                {
                    riDataBo.Log.DataCorrection.NotFound();
                }
            }

            if (on)
                LogRiDataFile.SwDataCorrection.Stop();
        }

        public void ProcessRiDataComputation(int step = RiDataComputationBo.StepPreComputation1)
        {
            IList<RiDataComputationBo> computations = null;
            if (!ProcessRiDataBatch.RiDataComputationBos.IsNullOrEmpty())
                computations = ProcessRiDataBatch.RiDataComputationBos.Where(q => q.Step == step).ToList();

            // Since Treaty Number Mapping is needed in Pre-Com 1, just ignore the RiDataLookup skip boolean
            //bool hasAdjustment = RiDataBos.Any(q => q.RecordType == RiDataBatchBo.RecordTypeAdjustment);
            //bool hasRiDataLookup = step == RiDataComputationBo.StepPreComputation1 && hasAdjustment;
            //bool hasTreatyNumberMapping = step == RiDataComputationBo.StepPreComputation1;
            if (computations.IsNullOrEmpty())
            {
                foreach (var riDataBo in RiDataBos)
                {
                    GlobalProcessRowRiDataConnectionStrategy.SetRiDataId(riDataBo.Id);
                    if (step == RiDataComputationBo.StepPreComputation1)
                    {
                        // Only in Pre-Computation 1
                        //Map Treaty Number & Treaty Type after Treaty Mapping
                        ProcessTreatyNumberTreatyTypeMapping(riDataBo, step);

                        // Only in Pre-Computation 1
                        // RiDataLookUp
                        ProcessRiDataLookup(riDataBo, step);

                        riDataBo.UpdateComputationStatus(step);
                    }
                    else
                    {
                        riDataBo.Log.Computation.Title("ProcessRiDataComputation");
                        riDataBo.Log.Computation.Failed(MessageBag.NoComputationConfigured);
                        riDataBo.Log.Computation.Detail("Step", RiDataComputationBo.GetStepName(step));
                        switch (step)
                        {
                            case RiDataComputationBo.StepPreComputation1:
                                riDataBo.PreComputation1Status = RiDataBo.PreComputation1StatusSuccess;
                                break;
                            case RiDataComputationBo.StepPreComputation2:
                                riDataBo.PreComputation2Status = RiDataBo.PreComputation2StatusSuccess;
                                break;
                            case RiDataComputationBo.StepPostComputation:
                                riDataBo.PostComputationStatus = RiDataBo.PostComputationStatusSuccess;
                                break;
                        }
                    }
                }
                return;
            }

            var formulas = computations.Where(q => q.Mode == RiDataComputationBo.ModeFormula).ToList();
            var tables = computations.Where(q => q.Mode == RiDataComputationBo.ModeTable).ToList();
            var riskDates = computations.Where(q => q.Mode == RiDataComputationBo.ModeRiskDate).ToList();

            // Product Feature Mapping - Benefit
            var benefits = tables.Where(q => q.CalculationFormula == RiDataComputationBo.TableBenefit.ToString()).ToList();

            // Product Feature Mapping - Treaty
            var treaties = tables.Where(q => q.CalculationFormula == RiDataComputationBo.TableTreaty.ToString()).ToList();

            // Product Feature Mapping
            var profitComms = tables.Where(q => q.CalculationFormula == RiDataComputationBo.TableFeatureProfitComm.ToString()).ToList();
            var maxAgeAtExpiries = tables.Where(q => q.CalculationFormula == RiDataComputationBo.TableFeatureMaxAgeAtExpiry.ToString()).ToList();
            var minIssueAges = tables.Where(q => q.CalculationFormula == RiDataComputationBo.TableFeatureMinIssueAge.ToString()).ToList();
            var maxIssueAges = tables.Where(q => q.CalculationFormula == RiDataComputationBo.TableFeatureMaxIssueAge.ToString()).ToList();
            var maxUwRatings = tables.Where(q => q.CalculationFormula == RiDataComputationBo.TableFeatureMaxUwRating.ToString()).ToList();
            var apLoadings = tables.Where(q => q.CalculationFormula == RiDataComputationBo.TableFeatureApLoading.ToString()).ToList();
            var minAars = tables.Where(q => q.CalculationFormula == RiDataComputationBo.TableFeatureMinAar.ToString()).ToList();
            var maxAars = tables.Where(q => q.CalculationFormula == RiDataComputationBo.TableFeatureMaxAar.ToString()).ToList();
            var ablAmounts = tables.Where(q => q.CalculationFormula == RiDataComputationBo.TableFeatureAblAmount.ToString()).ToList();
            var retentionShares = tables.Where(q => q.CalculationFormula == RiDataComputationBo.TableFeatureRetentionShare.ToString()).ToList();
            var retentionCaps = tables.Where(q => q.CalculationFormula == RiDataComputationBo.TableFeatureRetentionCap.ToString()).ToList();
            var riShares = tables.Where(q => q.CalculationFormula == RiDataComputationBo.TableFeatureRiShare.ToString()).ToList();
            var riShareCaps = tables.Where(q => q.CalculationFormula == RiDataComputationBo.TableFeatureRiShareCap.ToString()).ToList();
            var riShare2s = tables.Where(q => q.CalculationFormula == RiDataComputationBo.TableFeatureRiShare2.ToString()).ToList();
            var riShareCap2s = tables.Where(q => q.CalculationFormula == RiDataComputationBo.TableFeatureRiShareCap2.ToString()).ToList();
            var serviceFees = tables.Where(q => q.CalculationFormula == RiDataComputationBo.TableFeatureServiceFee.ToString()).ToList();
            var wakalahFees = tables.Where(q => q.CalculationFormula == RiDataComputationBo.TableFeatureWakalahFee.ToString()).ToList();
            var effectiveDates = tables.Where(q => q.CalculationFormula == RiDataComputationBo.TableFeatureEffectiveDate.ToString()).ToList();

            // Cell Mapping
            var cellBasicRiders = tables.Where(q => q.CalculationFormula == RiDataComputationBo.TableCellBasicRider.ToString()).ToList();
            var cellNames = tables.Where(q => q.CalculationFormula == RiDataComputationBo.TableCellCellName.ToString()).ToList();
            var cellMfrs17TreatyCodes = tables.Where(q => q.CalculationFormula == RiDataComputationBo.TableCellTreatyCode.ToString()).ToList();
            var cellLoaCodes = tables.Where(q => q.CalculationFormula == RiDataComputationBo.TableCellLoaCode.ToString()).ToList();

            // Rate Table Mapping
            var rateTableCodes = tables.Where(q => q.CalculationFormula == RiDataComputationBo.TableRateTableCode.ToString()).ToList();
            var ratePerBasises = tables.Where(q => q.CalculationFormula == RiDataComputationBo.TableRateTableRatePerBasis.ToString()).ToList();
            var ratesByPreviousAge = tables.Where(q => q.CalculationFormula == RiDataComputationBo.TableRateTableRateByPreviousAge.ToString()).ToList();
            var ratesByCurrentAge = tables.Where(q => q.CalculationFormula == RiDataComputationBo.TableRateTableRateByCurrentAge.ToString()).ToList();
            var riDiscounts = tables.Where(q => q.CalculationFormula == RiDataComputationBo.TableRateTableRiDiscount.ToString()).ToList();
            var largeDiscounts = tables.Where(q => q.CalculationFormula == RiDataComputationBo.TableRateTableLargeDiscount.ToString()).ToList();
            var groupDiscounts = tables.Where(q => q.CalculationFormula == RiDataComputationBo.TableRateTableGroupDiscount.ToString()).ToList();
            var annuityFactors = tables.Where(q => q.CalculationFormula == RiDataComputationBo.TableAnnuityFactor.ToString()).ToList();

            // Risk Date
            var riskDateOption1StartDate = riskDates.Where(q => q.CalculationFormula == RiDataComputationBo.RiskDateOption1StartDate.ToString()).ToList();
            var riskDateOption1EndDate = riskDates.Where(q => q.CalculationFormula == RiDataComputationBo.RiskDateOption1EndDate.ToString()).ToList();
            var riskDateOption2StartDate = riskDates.Where(q => q.CalculationFormula == RiDataComputationBo.RiskDateOption2StartDate.ToString()).ToList();
            var riskDateOption2EndDate = riskDates.Where(q => q.CalculationFormula == RiDataComputationBo.RiskDateOption2EndDate.ToString()).ToList();

            // FAC Master Listing
            var ewarpNumbers = tables.Where(q => q.CalculationFormula == RiDataComputationBo.TableFacEwarpNumber.ToString()).ToList();
            var ewarpActionCodes = tables.Where(q => q.CalculationFormula == RiDataComputationBo.TableFacEwarpActionCode.ToString()).ToList();
            var offerLetterSentDates = tables.Where(q => q.CalculationFormula == RiDataComputationBo.TableFacOfferLetterSentDate.ToString()).ToList();
            var sumAssuredOffereds = tables.Where(q => q.CalculationFormula == RiDataComputationBo.TableFacSumAssuredOffered.ToString()).ToList();
            var uwRatingOffereds = tables.Where(q => q.CalculationFormula == RiDataComputationBo.TableFacUwRatingOffered.ToString()).ToList();
            var flatExtraAmountOffereds = tables.Where(q => q.CalculationFormula == RiDataComputationBo.TableFacFlatExtraAmountOffered.ToString()).ToList();
            var flatExtraDurations = tables.Where(q => q.CalculationFormula == RiDataComputationBo.TableFacFlatExtraDuration.ToString()).ToList();

            foreach (var riDataBo in RiDataBos)
            {
                GlobalProcessRowRiDataConnectionStrategy.SetRiDataId(riDataBo.Id);

                // Reset Value for Post Computation
                if (step == RiDataComputationBo.StepPostComputation)
                    riDataBo.ResetPostComputationValue();

                bool hasOriginalEntry = step == RiDataComputationBo.StepPostComputation && riDataBo.RecordType == RiDataBatchBo.RecordTypeAdjustment;
                if (hasOriginalEntry)
                {
                    RiDataWarehouseBo originalENtry = riDataBo.OriginalEntryId.HasValue ? RiDataWarehouseService.Find(riDataBo.OriginalEntryId.Value) : null;

                    riDataBo.OriginalEntryBo = originalENtry;
                    riDataBo.OriginalEntryQuarter = originalENtry?.Quarter;
                }

                // Reset for Pre-2 and Post Computation
                riDataBo.FormulaValidate = true;
                riDataBo.BenefitCodeMappingValidate = true;
                riDataBo.TreatyCodeMappingValidate = true;
                riDataBo.TreatyNumberMappingValidate = true;
                riDataBo.ProductFeatureMappingValidate = true;
                riDataBo.CellMappingValidate = true;
                riDataBo.RateTableMappingValidate = true;
                riDataBo.AnnuityFactorMappingValidate = true;
                riDataBo.RiDataLookupValidate = true;
                riDataBo.RiskDateValidate = true;
                riDataBo.FacMappingValidate = true;

                ProcessFormula(formulas, riDataBo, step);
                ProcessTreatyMapping(treaties, riDataBo, step);

                // Only in Pre-Computation 1
                //Map Treaty Number after Treaty Mapping
                ProcessTreatyNumberTreatyTypeMapping(riDataBo, step);

                ProcessBenefitMapping(benefits, riDataBo, step);

                ProcessProductFeatureMapping(
                    profitComms,
                    maxAgeAtExpiries,
                    minIssueAges,
                    maxIssueAges,
                    maxUwRatings,
                    apLoadings,
                    minAars,
                    maxAars,
                    ablAmounts,
                    retentionShares,
                    retentionCaps,
                    riShares,
                    riShareCaps,
                    riShare2s,
                    riShareCap2s,
                    serviceFees,
                    wakalahFees,
                    effectiveDates,
                    riDataBo,
                    step
                );

                ProcessCellMapping(cellBasicRiders, cellNames, cellMfrs17TreatyCodes, cellLoaCodes, riDataBo, step);
                ProcessRateTableMapping(rateTableCodes, ratePerBasises, ratesByPreviousAge, ratesByCurrentAge, riDiscounts, largeDiscounts, groupDiscounts, riDataBo, step);

                ProcessAnnuityFactorMapping(annuityFactors, riDataBo, step);

                // Only in Pre-Computation 1
                // RiDataLookUp
                ProcessRiDataLookup(riDataBo, step);

                ProcessRiskDate(riskDateOption1StartDate, riskDateOption1EndDate, riskDateOption2StartDate, riskDateOption2EndDate, riDataBo, step);

                ProcessFacMapping(
                    ewarpNumbers,
                    ewarpActionCodes,
                    offerLetterSentDates,
                    sumAssuredOffereds,
                    uwRatingOffereds,
                    flatExtraAmountOffereds,
                    flatExtraDurations,
                    riDataBo,
                    step
                );

                riDataBo.UpdateComputationStatus(step);
            }
        }

        public void ComputationErrorCount(int step = RiDataComputationBo.StepPreComputation1, string errorCountName = null)
        {
            switch (step)
            {
                case RiDataComputationBo.StepPreComputation1:
                    LogRiDataFile.PreComputation1ErrorCount++;
                    break;
                case RiDataComputationBo.StepPreComputation2:
                    LogRiDataFile.PreComputation2ErrorCount++;
                    break;
                case RiDataComputationBo.StepPostComputation:
                    LogRiDataFile.PostComputationErrorCount++;
                    break;
            }
            if (!string.IsNullOrEmpty(errorCountName))
            {
                int count = (int)LogRiDataFile.GetPropertyValue(errorCountName);
                count++;
                LogRiDataFile.SetPropertyValue(errorCountName, count);
            }
        }

        public void ProcessFormula(IList<RiDataComputationBo> computations, RiDataBo riDataBo, int step)
        {
            if (computations.IsNullOrEmpty())
            {
                riDataBo.Log.Formula.LineDelimiter();
                riDataBo.Log.Formula.Failed(MessageBag.NoComputationConfigured);
                return;
            }

            switch (step)
            {
                case RiDataComputationBo.StepPreComputation1:
                    LogRiDataFile.SwPreComputation1.Start();
                    break;
                case RiDataComputationBo.StepPreComputation2:
                    LogRiDataFile.SwPreComputation2.Start();
                    break;
                case RiDataComputationBo.StepPostComputation:
                    LogRiDataFile.SwPostComputation.Start();
                    break;
            }

            foreach (var computation in computations)
            {
                var so = computation.GetStandardOutputBo();
                //var soe = computation.GetStandardOutputEval(riDataBo, Quarter);
                var soe = new StandardOutputEval
                {
                    Condition = computation.Condition,
                    Formula = computation.CalculationFormula,
                    RiDataBo = riDataBo,
                    Quarter = Quarter,
                    EnableOriginal = false
                };

                var success = true;
                if (step != RiDataComputationBo.StepPreComputation1 && riDataBo.RecordType == RiDataBatchBo.RecordTypeAdjustment && riDataBo.OriginalEntryBo != null)
                {
                    soe.OriginalRiDataWarehouseBo = riDataBo.OriginalEntryBo;
                    soe.OriginalQuarter = riDataBo.OriginalEntryQuarter;
                    soe.EnableOriginal = true;
                }

                var condition = soe.EvalCondition();
                riDataBo.Log.Formula.LineDelimiter();
                riDataBo.Log.Formula.Detail("Step", RiDataComputationBo.GetStepName(step));
                riDataBo.Log.Formula.Property(so.Property);
                riDataBo.Log.Formula.Detail("Condition", soe.Condition);
                riDataBo.Log.Formula.Detail("Formatted Condition", soe.FormattedCondition);
                if (!soe.Errors.IsNullOrEmpty())
                {
                    success = false;

                    foreach (string error in soe.Errors)
                    {
                        riDataBo.SetError(so.Property, error);
                        riDataBo.Log.Formula.Error(error);
                    }
                }
                else if (condition)
                {
                    var value = soe.EvalFormula();
                    riDataBo.Log.Formula.Detail("Formula", soe.Formula);
                    riDataBo.Log.Formula.Detail("Formatted Formula", soe.FormattedFormula);
                    if (!soe.Errors.IsNullOrEmpty())
                    {
                        success = false;

                        foreach (string error in soe.Errors)
                        {
                            riDataBo.SetError(so.Property, error);
                            riDataBo.Log.Formula.Error(error);
                        }
                    }
                    else if (value == null)
                    {
                        // the computation result should not be null
                        success = false;

                        riDataBo.SetError(so.Property, string.Format("Formula Error: {0} Formatted Formula: {1}", MessageBag.ComputationResultNull, soe.FormattedFormula));
                        riDataBo.Log.Formula.Error(MessageBag.ComputationResultNull);
                    }
                    else
                    {
                        try
                        {
                            riDataBo.Log.Formula.Before(riDataBo.GetPropertyValue(so.Property));
                            riDataBo.SetRiData(so.DataType, so.Property, value);
                            riDataBo.Log.Formula.After(riDataBo.GetPropertyValue(so.Property));
                        }
                        catch (Exception e)
                        {
                            success = false;
                            riDataBo.SetError(so.Property, string.Format("Formula Error: {0}", e.Message));
                            riDataBo.Log.Formula.Error(e.Message);
                        }
                    }
                }
                else
                {
                    riDataBo.Log.Formula.Failed(MessageBag.ComputationConditionFailed);
                }

                if (!success)
                {
                    riDataBo.Log.Formula.Failed();
                    riDataBo.Log.Formula.Detail("Value", riDataBo.GetPropertyValue(so.Property));
                    riDataBo.SetComputationValidates(step, RiDataBo.ValidateKeyFormula);
                    ComputationErrorCount(step, "FormulaErrorCount");
                }
            }

            switch (step)
            {
                case RiDataComputationBo.StepPreComputation1:
                    LogRiDataFile.SwPreComputation1.Stop();
                    break;
                case RiDataComputationBo.StepPreComputation2:
                    LogRiDataFile.SwPreComputation2.Stop();
                    break;
                case RiDataComputationBo.StepPostComputation:
                    LogRiDataFile.SwPostComputation.Stop();
                    break;
            }
        }

        public void SetComputationErrors(IList<RiDataComputationBo> computations, List<string> errors, string errorCountName, RiDataBo riDataBo, int step)
        {
            if (computations.IsNullOrEmpty())
                return;

            foreach (var computation in computations)
            {
                var so = computation.GetStandardOutputBo();
                riDataBo.SetError(so.Property, errors);
                ComputationErrorCount(step, errorCountName);
            }
        }

        public void SetComputationErrors(IList<RiDataComputationBo> computations, List<string> errors, List<string> extraErrors, string errorCountName, RiDataBo riDataBo, int step)
        {
            if (computations.IsNullOrEmpty())
                return;

            foreach (var computation in computations)
            {
                var newErrors = new List<string> { };
                newErrors.AddRange(errors);
                newErrors.AddRange(extraErrors);

                var so = computation.GetStandardOutputBo();
                if (so != null)
                    riDataBo.SetError(so.Property, newErrors);
                ComputationErrorCount(step, errorCountName);
            }
        }

        public void Compute(
            IList<RiDataComputationBo> computations,
            object value,
            string errorCountName,
            RiDataBo riDataBo,
            int step,
            string title,
            int validateKey,
            bool validateCondition = false,
            bool enableEmptyValue = false
        )
        {
            if (computations.IsNullOrEmpty())
                return;

            riDataBo.Log.Computation.Title("Compute");
            if (value == null && !enableEmptyValue)
            {
                foreach (var computation in computations)
                {
                    var so = computation.GetStandardOutputBo();
                    riDataBo.Log.Computation.Detail("Computation Type", RiDataComputationBo.GetTableName(computation.CalculationFormula));
                    riDataBo.Log.Computation.Property(so.Property);
                    riDataBo.Log.Computation.ValueIsNull();
                }
                return;
            }

            foreach (var computation in computations)
            {
                var errors = new List<string> { };
                var so = computation.GetStandardOutputBo();
                var condition = true;

                riDataBo.Log.Computation.Detail("Computation Type", RiDataComputationBo.GetTableName(computation.CalculationFormula));
                riDataBo.Log.Computation.Property(so.Property);
                riDataBo.Log.Computation.Detail("Value", value);

                if (validateCondition)
                {
                    //var soe = computation.GetStandardOutputEval(riDataBo, Quarter);
                    var soe = new StandardOutputEval
                    {
                        Condition = computation.Condition,
                        Formula = computation.CalculationFormula,
                        RiDataBo = riDataBo,
                        Quarter = Quarter,
                        EnableOriginal = false
                    };
                    condition = soe.EvalCondition();
                    riDataBo.Log.Computation.Detail("Condition", soe.Condition);
                    riDataBo.Log.Computation.Detail("Formatted Condition", soe.FormattedCondition);
                }

                if (condition)
                {
                    string error;
                    if (value != null)
                    {
                        switch (value.GetType().Name)
                        {
                            case "String":
                                so.IsDataTypeString(title, out error);
                                if (!string.IsNullOrEmpty(error))
                                    errors.Add(error);
                                break;
                            case "Double":
                                so.IsDataTypeDouble(title, out error);
                                if (!string.IsNullOrEmpty(error))
                                    errors.Add(error);
                                break;
                            case "Int32":
                                so.IsDataTypeInt(title, out error);
                                if (!string.IsNullOrEmpty(error))
                                    errors.Add(error);
                                break;
                            case "DateTime":
                                so.IsDataTypeDate(title, out error);
                                if (!string.IsNullOrEmpty(error))
                                    errors.Add(error);
                                break;
                            case "Boolean":
                                so.IsDataTypeBoolean(title, out error);
                                if (!string.IsNullOrEmpty(error))
                                    errors.Add(error);
                                break;
                            default:
                                errors.Add(string.Format(MessageBag.UnableSetValue, value.GetType().FullName));
                                break;
                        }
                    }

                    if (errors.IsNullOrEmpty())
                    {
                        riDataBo.Log.Computation.Before(riDataBo.GetPropertyValue(so.Property));
                        riDataBo.SetPropertyValue(so.Property, value);
                        riDataBo.Log.Computation.After(riDataBo.GetPropertyValue(so.Property));
                    }
                    else
                    {
                        riDataBo.SetComputationValidates(step, validateKey);
                        riDataBo.Log.Computation.Errors(errors);
                        riDataBo.SetError(so.Property, errors);
                        ComputationErrorCount(step, errorCountName);
                    }
                }
                else
                {
                    riDataBo.Log.Computation.Failed(MessageBag.ComputationConditionFailed);
                }
            }
        }

        public IList<RiDataComputationBo> ValidateConditions(IList<RiDataComputationBo> computations, RiDataBo riDataBo)
        {
            if (computations.IsNullOrEmpty())
            {
                riDataBo.Log.Computation.LineDelimiter(20, '-');
                riDataBo.Log.Computation.Add("ValidateConditions");
                riDataBo.Log.Computation.Failed(MessageBag.NoComputationConfigured);
                return null;
            }

            var newComputations = new List<RiDataComputationBo> { };
            foreach (var computation in computations)
            {
                var so = computation.GetStandardOutputBo();
                //var soe = computation.GetStandardOutputEval(riDataBo, Quarter);
                var soe = new StandardOutputEval
                {
                    Condition = computation.Condition,
                    Formula = computation.CalculationFormula,
                    RiDataBo = riDataBo,
                    Quarter = Quarter,
                    EnableOriginal = false
                };
                var condition = soe.EvalCondition();

                riDataBo.Log.Computation.LineDelimiter(20, '-');
                riDataBo.Log.Computation.Add("ValidateConditions");
                riDataBo.Log.Computation.Detail("Computation Type", RiDataComputationBo.GetTableName(computation.CalculationFormula));
                if (so != null)
                    riDataBo.Log.Computation.Property(so.Property);
                riDataBo.Log.Computation.Detail("Condition", soe.Condition);
                riDataBo.Log.Computation.Detail("Formatted Condition", soe.FormattedCondition);

                if (condition)
                {
                    riDataBo.Log.Computation.Success(MessageBag.ComputationConditionPassed);
                    newComputations.Add(computation);
                }
                else
                {
                    riDataBo.Log.Computation.Failed(MessageBag.ComputationConditionFailed);
                }
            }
            riDataBo.Log.Computation.Add();
            return newComputations;
        }

        public void ProcessTreatyMapping(IList<RiDataComputationBo> computations, RiDataBo riDataBo, int step)
        {
            riDataBo.Log.Computation.Title("ProcessTreatyMapping");
            if (computations.IsNullOrEmpty())
            {
                riDataBo.Log.Computation.Failed(MessageBag.NoComputationConfigured);
                return;
            }

            computations = ValidateConditions(computations, riDataBo);
            if (computations.IsNullOrEmpty())
            {
                riDataBo.Log.Computation.Failed(MessageBag.NoComputationCondition);
                return;
            }

            LogRiDataFile.SwTreatyMapping.Start();

            // NOTED: THIS IS FOR TESTING PURPOSE AND DO NOT UNCOMMENT IT
            //riDataBo.CedingPlanCode = "T21";
            //riDataBo.CedingBenefitTypeCode = "CLR";
            //riDataBo.CedingBenefitRiskCode = "CBRC1";
            //riDataBo.CedingTreatyCode = "CTC1";
            //riDataBo.CampaignCode = "CC1";
            //riDataBo.ReinsEffDatePol = new DateTime(2013, 3, 31);
            //riDataBo.ReinsBasisCode = "AP";

            riDataBo.Log.Computation.Parameters();
            riDataBo.Log.Computation.AddLines(FormatRiDataByTypes(RiDataBo.ParamsTreatyMapping(), riDataBo));
            riDataBo.Log.Computation.Add();

            TreatyBenefitCodeMappingDetailBo detailBo = null;
            var errors = riDataBo.ValidateTreatyMapping(out DateTime? reportDate);
            if (errors.IsNullOrEmpty())
            {
                var types = new List<int>
                {
                    StandardOutputBo.TypeCedingBenefitTypeCode,
                    StandardOutputBo.TypeReinsBasisCode,
                };
                GlobalProcessRowRiDataConnectionStrategy.SetRiDataId(riDataBo.Id);
                var validateErrors = RiDataService.ValidateDropDownCodes(RiDataBo.TreatyCodeMappingTitle, types, riDataBo);
                errors.AddRange(validateErrors);

                if (errors.IsNullOrEmpty())
                {
                    riDataBo.Log.Computation.Detail("ReportDate", reportDate);

                    // Search by params
                    var count = TreatyBenefitCodeMappingDetailService.CountByTreatyParams(CedantId, riDataBo, ProcessRiDataBatch.CacheService, reportDate, true); // groupById
                    if (count > 0)
                    {
                        if (count == 1)
                        {
                            detailBo = ProcessRiDataBatch.CacheService.FindByTreatyParams(CedantId, riDataBo, reportDate, true);
                            // If not found in cache
                            if (detailBo == null)
                            {
                                detailBo = TreatyBenefitCodeMappingDetailService.FindByTreatyParams(CedantId, riDataBo, ProcessRiDataBatch.CacheService, reportDate, true); // groupById
                                //ProcessRiDataBatch.CacheService.TreatyBenefitCodeMappingDetailBosForTreatyMapping.Add(detailBo);
                                ProcessRiDataBatch.CacheService.AddMappingBo(1, detailBo);
                            }
                        }
                        else
                        {
                            errors.Add(riDataBo.FormatTreatyMappingError(MessageBag.MultipleMappingRecordsFound));
                            //riDataBo.Log.Add("--Matched Parameters Records");
                            //var detailBos = TreatyBenefitCodeMappingDetailService.GetByTreatyParams(riDataBo, ProcessRiDataBatch.CacheService);
                            //foreach (var d in detailBos)
                            //    riDataBo.Log.Add(string.Format(MessageBag.MappingDetailCombination, d.TreatyBenefitCodeMappingId, d.Id, d.Combination));
                            //riDataBo.Log.Add(string.Format(MessageBag.TotalMappingDetailCombination, "TreatyBenefitCodeMapping", detailBos.Select(d => d.TreatyBenefitCodeMappingId).Distinct().Count()));
                            //riDataBo.Log.Add();
                        }
                    }
                    else
                    {
                        errors.Add(riDataBo.FormatTreatyMappingError(MessageBag.NoRecordMatchParams));
                    }
                }
            }

            string treatyCode = null;
            TreatyBenefitCodeMappingBo found = null;
            if (detailBo != null)
                if (detailBo.TreatyBenefitCodeMappingBo == null)
                    errors.Add(riDataBo.FormatTreatyMappingError("TreatyBenefitCodeMapping", MessageBag.NoRecordFound));
                else if (detailBo.TreatyBenefitCodeMappingBo.TreatyCodeBo == null)
                    errors.Add(riDataBo.FormatTreatyMappingError("TreatyCode", MessageBag.NoRecordFound));
                else
                    found = detailBo.TreatyBenefitCodeMappingBo;

            if (found != null)
                treatyCode = found.TreatyCodeBo.Code;

            if (errors.IsNullOrEmpty())
            {
                if (treatyCode != null)
                {
                    riDataBo.Log.Computation.Found();
                    riDataBo.Log.Computation.Parameter("CedingPlanCode", found.CedingPlanCode);
                    riDataBo.Log.Computation.Parameter("CedingBenefitTypeCode", found.CedingBenefitTypeCode);
                    riDataBo.Log.Computation.Parameter("CedingBenefitRiskCode", found.CedingBenefitRiskCode);
                    riDataBo.Log.Computation.Parameter("CedingTreatyCode", found.CedingTreatyCode);
                    riDataBo.Log.Computation.Parameter("CampaignCode", found.CampaignCode);
                    riDataBo.Log.Computation.Parameter("ReinsEffDatePolStartDate", found.ReinsEffDatePolStartDate);
                    riDataBo.Log.Computation.Parameter("ReinsEffDatePolEndDate", found.ReinsEffDatePolEndDate);
                    riDataBo.Log.Computation.Parameter("ReinsBasisCode", found.ReinsBasisCodePickListDetailId);
                    riDataBo.Log.Computation.Add();
                    riDataBo.Log.Computation.MappedValue("TreatyCode", treatyCode);
                }
                else
                {
                    riDataBo.Log.Computation.NotFound();
                }
            }
            else
            {
                riDataBo.SetComputationValidates(step, RiDataBo.ValidateKeyTreatyCodeMapping);
                riDataBo.Log.Computation.Errors(errors);
                SetComputationErrors(computations, errors, "TreatyCodeMappingErrorCount", riDataBo, step);

                // if error triggered
                treatyCode = null;
            }

            Compute(computations, treatyCode, "TreatyCodeMappingErrorCount", riDataBo, step, RiDataBo.TreatyCodeMappingTitle, RiDataBo.ValidateKeyTreatyCodeMapping);

            LogRiDataFile.SwTreatyMapping.Stop();
        }

        public void ProcessTreatyNumberTreatyTypeMapping(RiDataBo riDataBo, int step)
        {
            if (step != RiDataComputationBo.StepPreComputation1)
                return;

            riDataBo.Log.Computation.Title("ProcessTreatyNumberTreatyTypeMapping");

            LogRiDataFile.SwTreatyNumberTreatyTypeMapping.Start();

            riDataBo.Log.Computation.Parameters();
            riDataBo.Log.Computation.AddLines(FormatRiDataByTypes(RiDataBo.ParamsTreatyNumberTreatyTypeMapping(), riDataBo));
            riDataBo.Log.Computation.Add();

            TreatyCodeBo treatyCodeBo = null;
            var treatyNumberErrors = new List<string> { };
            var treatyTypeErrors = new List<string> { };

            var errors = riDataBo.ValidateTreatyNumberTreatyTypeMapping();
            //if (riDataBo.TreatyNumberMappingValidate)
            //{
            //    treatyCodeBo = TreatyCodeService.FindByCode(riDataBo.TreatyCode);

            //    if (treatyCodeBo == null)
            //    {
            //        errors.Add(riDataBo.FormatTreatyNumberTreatyTypeMappingError(string.Format(MessageBag.NoRecordFoundIn, "Treaty Code Table")));
            //    }
            //    else
            //    {
            //        if (string.IsNullOrEmpty(treatyCodeBo.TreatyNo))
            //            treatyNumberErrors.Add(riDataBo.FormatTreatyNumberTreatyTypeMappingError(string.Format(MessageBag.IsEmpty, "Treaty Number")));
            //        if (treatyCodeBo.TreatyTypePickListDetailBo == null || (treatyCodeBo.TreatyTypePickListDetailBo != null && string.IsNullOrEmpty(treatyCodeBo.TreatyTypePickListDetailBo.Code)))
            //            treatyTypeErrors.Add(riDataBo.FormatTreatyNumberTreatyTypeMappingError(string.Format(MessageBag.IsEmpty, "Treaty Type")));
            //    }
            //}
            if (riDataBo.TreatyNumberMappingValidate)
            {
                GlobalProcessRowRiDataConnectionStrategy.SetRiDataId(riDataBo.Id);
                treatyCodeBo = ProcessRiDataBatch.CacheService.FindByTreatyNumberTreatyTypeParams(riDataBo.TreatyCode);

                if (treatyCodeBo == null)
                {
                    // If not found in cache
                    treatyCodeBo = TreatyCodeService.FindByCode(riDataBo.TreatyCode);

                    if (treatyCodeBo == null)
                    {
                        errors.Add(riDataBo.FormatTreatyNumberTreatyTypeMappingError(string.Format(MessageBag.NoRecordFoundIn, "Treaty Code Table")));
                    }
                    else
                    {
                        ProcessRiDataBatch.CacheService.AddMappingBo(8, treatyCodeBo);

                        if (string.IsNullOrEmpty(treatyCodeBo.TreatyNo))
                            treatyNumberErrors.Add(riDataBo.FormatTreatyNumberTreatyTypeMappingError(string.Format(MessageBag.IsEmpty, "Treaty Number")));
                        if (treatyCodeBo.TreatyTypePickListDetailBo == null || (treatyCodeBo.TreatyTypePickListDetailBo != null && string.IsNullOrEmpty(treatyCodeBo.TreatyTypePickListDetailBo.Code)))
                            treatyTypeErrors.Add(riDataBo.FormatTreatyNumberTreatyTypeMappingError(string.Format(MessageBag.IsEmpty, "Treaty Type")));
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(treatyCodeBo.TreatyNo))
                        treatyNumberErrors.Add(riDataBo.FormatTreatyNumberTreatyTypeMappingError(string.Format(MessageBag.IsEmpty, "Treaty Number")));
                    if (treatyCodeBo.TreatyTypePickListDetailBo == null || (treatyCodeBo.TreatyTypePickListDetailBo != null && string.IsNullOrEmpty(treatyCodeBo.TreatyTypePickListDetailBo.Code)))
                        treatyTypeErrors.Add(riDataBo.FormatTreatyNumberTreatyTypeMappingError(string.Format(MessageBag.IsEmpty, "Treaty Type")));
                }
            }

            string treatyNumber = null;
            string treatyType = null;
            if (errors.IsNullOrEmpty())
            {
                if (treatyCodeBo != null)
                {
                    riDataBo.Log.Computation.Found();
                    riDataBo.Log.Computation.Parameter("TreatyCode", treatyCodeBo.Code);
                    riDataBo.Log.Computation.Add();
                    riDataBo.Log.Computation.MappedValue("TreatyNumber", treatyCodeBo.TreatyNo);
                    riDataBo.Log.Computation.MappedValue("TreatyType", treatyCodeBo.TreatyTypePickListDetailBo?.Code);

                    treatyNumber = treatyCodeBo.TreatyNo;
                    treatyType = treatyCodeBo.TreatyTypePickListDetailBo?.Code;
                }
                else
                {
                    riDataBo.Log.Computation.NotFound();
                }
            }
            else
            {
                riDataBo.SetComputationValidates(step, RiDataBo.ValidateKeyTreatyNumberTreatyTypeMapping);
                riDataBo.Log.Computation.Errors(errors);
                riDataBo.SetError("TreatyNumber", errors);
                ComputationErrorCount(step, "TreatyNumberMappingErrorCount");
                riDataBo.SetError("TreatyType", errors);
                ComputationErrorCount(step, "TreatyTypeMappingErrorCount");

                treatyNumber = null;
                treatyType = null;
            }

            if (treatyNumberErrors.IsNullOrEmpty())
            {
                if (!string.IsNullOrEmpty(treatyNumber))
                {
                    riDataBo.Log.Computation.Before(riDataBo.GetPropertyValue("TreatyNumber"));
                    riDataBo.SetPropertyValue("TreatyNumber", treatyNumber);
                    riDataBo.Log.Computation.After(riDataBo.GetPropertyValue("TreatyNumber"));
                }
                else
                {
                    riDataBo.Log.Computation.Property("TreatyNumber");
                    riDataBo.Log.Computation.ValueIsNull();
                }
            }
            else
            {
                riDataBo.SetComputationValidates(step, RiDataBo.ValidateKeyTreatyNumberTreatyTypeMapping);
                riDataBo.SetError("TreatyNumber", treatyNumberErrors);
                ComputationErrorCount(step, "TreatyNumberMappingErrorCount");
            }

            if (treatyTypeErrors.IsNullOrEmpty())
            {
                if (!string.IsNullOrEmpty(treatyType))
                {
                    riDataBo.Log.Computation.Before(riDataBo.GetPropertyValue("TreatyType"));
                    riDataBo.SetPropertyValue("TreatyType", treatyType);
                    riDataBo.Log.Computation.After(riDataBo.GetPropertyValue("TreatyType"));
                }
                else
                {
                    riDataBo.Log.Computation.Property("TreatyType");
                    riDataBo.Log.Computation.ValueIsNull();
                }
            }
            else
            {
                riDataBo.SetComputationValidates(step, RiDataBo.ValidateKeyTreatyNumberTreatyTypeMapping);
                riDataBo.SetError("TreatyType", treatyTypeErrors);
                ComputationErrorCount(step, "TreatyTypeMappingErrorCount");
            }

            LogRiDataFile.SwTreatyNumberTreatyTypeMapping.Stop();
        }

        public void ProcessBenefitMapping(IList<RiDataComputationBo> computations, RiDataBo riDataBo, int step)
        {
            riDataBo.Log.Computation.Title("ProcessBenefitMapping");
            if (computations.IsNullOrEmpty())
            {
                riDataBo.Log.Computation.Failed(MessageBag.NoComputationConfigured);
                return;
            }

            computations = ValidateConditions(computations, riDataBo);
            if (computations.IsNullOrEmpty())
            {
                riDataBo.Log.Computation.Failed(MessageBag.NoComputationCondition);
                return;
            }

            LogRiDataFile.SwBenefitMapping.Start();

            // NOTED: THIS IS FOR TESTING PURPOSE AND DO NOT UNCOMMENT IT
            //riDataBo.CedingPlanCode = "WPPR";
            //riDataBo.CedingBenefitTypeCode = "DTH";
            //riDataBo.CedingBenefitRiskCode = "DTH";
            //riDataBo.InsuredAttainedAge = 30;
            //riDataBo.ReportPeriodMonth = 7;
            //riDataBo.ReportPeriodYear = 2020;

            riDataBo.Log.Computation.Parameters();
            riDataBo.Log.Computation.AddLines(FormatRiDataByTypes(RiDataBo.ParamsBenefitMapping(), riDataBo));
            riDataBo.Log.Computation.Add();

            TreatyBenefitCodeMappingDetailBo detailBo = null;
            var errors = riDataBo.ValidateBenefitMapping(out DateTime? reportDate);
            if (riDataBo.BenefitCodeMappingValidate)
            {
                GlobalProcessRowRiDataConnectionStrategy.SetRiDataId(riDataBo.Id);
                var types = new List<int>
                {
                    StandardOutputBo.TypeCedingBenefitTypeCode,
                };
                var validateErrors = RiDataService.ValidateDropDownCodes(RiDataBo.BenefitCodeMappingTitle, types, riDataBo);
                errors.AddRange(validateErrors);

                if (errors.IsNullOrEmpty())
                {
                    riDataBo.Log.Computation.Detail("ReportDate", reportDate);

                    // Search by params
                    var count = TreatyBenefitCodeMappingDetailService.CountByBenefitParams(CedantId, riDataBo, ProcessRiDataBatch.CacheService, reportDate, true); // groupById
                    if (count > 0)
                    {
                        if (count == 1)
                        {
                            detailBo = ProcessRiDataBatch.CacheService.FindByBenefitParams(CedantId, riDataBo, reportDate, true);
                            // If not found in cache
                            if (detailBo == null)
                            {
                                detailBo = TreatyBenefitCodeMappingDetailService.FindByBenefitParams(CedantId, riDataBo, ProcessRiDataBatch.CacheService, reportDate, true); // groupById
                                //ProcessRiDataBatch.CacheService.TreatyBenefitCodeMappingDetailBosForBenefitMapping.Add(detailBo);
                                ProcessRiDataBatch.CacheService.AddMappingBo(2, detailBo);
                            }
                        }
                        else
                        {
                            errors.Add(riDataBo.FormatBenefitMappingError(MessageBag.MultipleMappingRecordsFound));
                            //riDataBo.Log.Add("--Matched Parameters Records");
                            //var detailBos = TreatyBenefitCodeMappingDetailService.GetByBenefitParams(riDataBo, reportDate);
                            //foreach (var d in detailBos)
                            //    riDataBo.Log.Add(string.Format(MessageBag.MappingDetailCombination, d.TreatyBenefitCodeMappingId, d.Id, d.Combination));
                            //riDataBo.Log.Add(string.Format(MessageBag.TotalMappingDetailCombination, "TreatyBenefitCodeMapping", detailBos.Select(d => d.TreatyBenefitCodeMappingId).Distinct().Count()));
                            //riDataBo.Log.Add();
                        }
                    }
                    else
                    {
                        errors.Add(riDataBo.FormatBenefitMappingError(MessageBag.NoRecordMatchParams));
                    }
                }
            }

            string benefitCode = null;
            TreatyBenefitCodeMappingBo found = null;
            if (detailBo != null)
                if (detailBo.TreatyBenefitCodeMappingBo == null)
                    errors.Add(riDataBo.FormatBenefitMappingError("TreatyBenefitCodeMapping", MessageBag.NoRecordFound));
                else
                    found = detailBo.TreatyBenefitCodeMappingBo;

            if (found != null)
                if (found.BenefitBo == null)
                    errors.Add(riDataBo.FormatBenefitMappingError("Benefit", MessageBag.NoRecordFound));
                else
                    benefitCode = found.BenefitBo.Code;

            if (errors.IsNullOrEmpty())
            {
                if (benefitCode != null)
                {
                    riDataBo.Log.Computation.Found();
                    riDataBo.Log.Computation.Parameter("CedingPlanCode", found.CedingPlanCode);
                    riDataBo.Log.Computation.Parameter("CedingBenefitTypeCode", found.CedingBenefitTypeCode);
                    riDataBo.Log.Computation.Parameter("CedingBenefitRiskCode", found.CedingBenefitRiskCode);
                    riDataBo.Log.Computation.Parameter("AttainedAgeFrom", found.AttainedAgeFrom);
                    riDataBo.Log.Computation.Parameter("AttainedAgeTo", found.AttainedAgeTo);
                    riDataBo.Log.Computation.Parameter("ReportingStartDate", found.ReportingStartDate);
                    riDataBo.Log.Computation.Parameter("ReportingEndDate", found.ReportingEndDate);
                    riDataBo.Log.Computation.Add();
                    riDataBo.Log.Computation.MappedValue("BenefitCode", benefitCode);
                }
                else
                {
                    riDataBo.Log.Computation.NotFound();
                }
            }
            else
            {
                riDataBo.SetComputationValidates(step, RiDataBo.ValidateKeyBenefitCodeMapping);
                riDataBo.Log.Computation.Errors(errors);
                SetComputationErrors(computations, errors, "BenefitCodeMappingErrorCount", riDataBo, step);

                // if error triggered
                benefitCode = null;
            }

            Compute(computations, benefitCode, "BenefitCodeMappingErrorCount", riDataBo, step, RiDataBo.BenefitCodeMappingTitle, RiDataBo.ValidateKeyBenefitCodeMapping);

            LogRiDataFile.SwBenefitMapping.Stop();
        }

        public void ComputeProductFeatureMapping(IList<RiDataComputationBo> computations, object value, string errorCountName, RiDataBo riDataBo, int step)
        {
            Compute(computations, value, errorCountName, riDataBo, step, RiDataBo.ProductFeatureMappingTitle, RiDataBo.ValidateKeyProductFeatureMapping);
        }

        public void ProcessProductFeatureMapping(
            IList<RiDataComputationBo> profitComms,
            IList<RiDataComputationBo> maxAgeAtExpiries,
            IList<RiDataComputationBo> minIssueAges,
            IList<RiDataComputationBo> maxIssueAges,
            IList<RiDataComputationBo> maxUwRatings,
            IList<RiDataComputationBo> apLoadings,
            IList<RiDataComputationBo> minAars,
            IList<RiDataComputationBo> maxAars,
            IList<RiDataComputationBo> ablAmounts,
            IList<RiDataComputationBo> retentionShares,
            IList<RiDataComputationBo> retentionCaps,
            IList<RiDataComputationBo> riShares,
            IList<RiDataComputationBo> riShareCaps,
            IList<RiDataComputationBo> riShare2s,
            IList<RiDataComputationBo> riShareCap2s,
            IList<RiDataComputationBo> serviceFees,
            IList<RiDataComputationBo> wakalahFees,
            IList<RiDataComputationBo> effectiveDates,
            RiDataBo riDataBo,
            int step
        )
        {
            riDataBo.Log.Computation.Title("ProcessProductFeatureMapping");
            if (
                profitComms.IsNullOrEmpty() &&
                maxAgeAtExpiries.IsNullOrEmpty() &&
                minIssueAges.IsNullOrEmpty() &&
                maxIssueAges.IsNullOrEmpty() &&
                maxUwRatings.IsNullOrEmpty() &&
                apLoadings.IsNullOrEmpty() &&
                minAars.IsNullOrEmpty() &&
                maxAars.IsNullOrEmpty() &&
                ablAmounts.IsNullOrEmpty() &&
                retentionShares.IsNullOrEmpty() &&
                retentionCaps.IsNullOrEmpty() &&
                riShares.IsNullOrEmpty() &&
                riShareCaps.IsNullOrEmpty() &&
                riShare2s.IsNullOrEmpty() &&
                riShareCap2s.IsNullOrEmpty() &&
                serviceFees.IsNullOrEmpty() &&
                wakalahFees.IsNullOrEmpty() &&
                effectiveDates.IsNullOrEmpty()
            )
            {
                riDataBo.Log.Computation.Failed(MessageBag.NoComputationConfigured);
                return;
            }

            profitComms = ValidateConditions(profitComms, riDataBo);
            maxAgeAtExpiries = ValidateConditions(maxAgeAtExpiries, riDataBo);
            minIssueAges = ValidateConditions(minIssueAges, riDataBo);
            maxIssueAges = ValidateConditions(maxIssueAges, riDataBo);
            maxUwRatings = ValidateConditions(maxUwRatings, riDataBo);
            apLoadings = ValidateConditions(apLoadings, riDataBo);
            minAars = ValidateConditions(minAars, riDataBo);
            maxAars = ValidateConditions(maxAars, riDataBo);
            ablAmounts = ValidateConditions(ablAmounts, riDataBo);
            retentionShares = ValidateConditions(retentionShares, riDataBo);
            retentionCaps = ValidateConditions(retentionCaps, riDataBo);
            riShares = ValidateConditions(riShares, riDataBo);
            riShareCaps = ValidateConditions(riShareCaps, riDataBo);
            riShare2s = ValidateConditions(riShare2s, riDataBo);
            riShareCap2s = ValidateConditions(riShareCap2s, riDataBo);
            serviceFees = ValidateConditions(serviceFees, riDataBo);
            wakalahFees = ValidateConditions(wakalahFees, riDataBo);
            effectiveDates = ValidateConditions(effectiveDates, riDataBo);
            if (
                profitComms.IsNullOrEmpty() &&
                maxAgeAtExpiries.IsNullOrEmpty() &&
                minIssueAges.IsNullOrEmpty() &&
                maxIssueAges.IsNullOrEmpty() &&
                maxUwRatings.IsNullOrEmpty() &&
                apLoadings.IsNullOrEmpty() &&
                minAars.IsNullOrEmpty() &&
                maxAars.IsNullOrEmpty() &&
                ablAmounts.IsNullOrEmpty() &&
                retentionShares.IsNullOrEmpty() &&
                retentionCaps.IsNullOrEmpty() &&
                riShares.IsNullOrEmpty() &&
                riShareCaps.IsNullOrEmpty() &&
                riShare2s.IsNullOrEmpty() &&
                riShareCap2s.IsNullOrEmpty() &&
                serviceFees.IsNullOrEmpty() &&
                wakalahFees.IsNullOrEmpty() &&
                effectiveDates.IsNullOrEmpty()
            )
            {
                riDataBo.Log.Computation.Failed(MessageBag.NoComputationCondition);
                return;
            }

            LogRiDataFile.SwProductFeatureMapping.Start();

            // NOTED: THIS IS FOR TESTING PURPOSE AND DO NOT UNCOMMENT IT
            //riData.TreatyCode = "GEL-08";
            //riData.ReinsBasisCode = "AUTO";
            //riData.CedingPlanCode = "0248"; // optional field
            //riData.CedingPlanCode = null; // optional field
            //riData.MlreBenefitCode = "DEA"; // optional field
            //riData.MlreBenefitCode = null; // optional field
            //riData.ReinsEffDatePol = new DateTime(2013, 3, 31); // optional field
            //riData.ReinsEffDatePol = null; // optional field

            riDataBo.Log.Computation.Parameters();
            riDataBo.Log.Computation.AddLines(FormatRiDataByTypes(RiDataBo.ParamsProductFeatureMapping(), riDataBo));
            riDataBo.Log.Computation.Add();

            TreatyBenefitCodeMappingDetailBo detailBo = null;
            var profitCommErrors = new List<string> { };
            var maxAgeAtExpiryErrors = new List<string> { };
            var minIssueAgeErrors = new List<string> { };
            var maxIssueAgeErrors = new List<string> { };
            var maxUwRatingErrors = new List<string> { };
            var apLoadingErrors = new List<string> { };
            var minAarErrors = new List<string> { };
            var maxAarErrors = new List<string> { };
            var ablAmountErrors = new List<string> { };
            var retentionShareErrors = new List<string> { };
            var retentionCapErrors = new List<string> { };
            var riShareErrors = new List<string> { };
            var riShareCapErrors = new List<string> { };
            var riShare2Errors = new List<string> { };
            var riShareCap2Errors = new List<string> { };
            var serviceFeeErrors = new List<string> { };
            var wakalahFeeErrors = new List<string> { };
            var effectiveDateErrors = new List<string> { };
            var errors = riDataBo.ValidateProductFeatureMapping(out DateTime? reportDate);
            if (riDataBo.ProductFeatureMappingValidate)
            {
                GlobalProcessRowRiDataConnectionStrategy.SetRiDataId(riDataBo.Id);
                if (errors.IsNullOrEmpty())
                {
                    riDataBo.Log.Computation.Detail("ReportDate", reportDate);

                    // Search by params
                    var count = TreatyBenefitCodeMappingDetailService.CountByProductFeatureParams(CedantId, riDataBo, ProcessRiDataBatch.CacheService, reportDate, true); // groupById
                    if (count > 0)
                    {
                        if (count == 1)
                        {
                            detailBo = ProcessRiDataBatch.CacheService.FindByProductFeatureParams(CedantId, riDataBo, reportDate, true);
                            // If not found in cache
                            if (detailBo == null)
                            {
                                detailBo = TreatyBenefitCodeMappingDetailService.FindByProductFeatureParams(CedantId, riDataBo, ProcessRiDataBatch.CacheService, reportDate, true); // groupById
                                //ProcessRiDataBatch.CacheService.TreatyBenefitCodeMappingDetailBosForProductFeatureMapping.Add(detailBo);
                                ProcessRiDataBatch.CacheService.AddMappingBo(3, detailBo);
                            }
                        }
                        else
                        {
                            errors.Add(riDataBo.FormatProductFeatureMappingError(MessageBag.MultipleMappingRecordsFound));
                            //riDataBo.Log.Add("--Matched Parameters Records");
                            //var detailBos = TreatyBenefitCodeMappingDetailService.GetByProductFeatureParams(riDataBo);
                            //foreach (var d in detailBos)
                            //    riDataBo.Log.Add(string.Format(MessageBag.MappingDetailCombination, d.TreatyBenefitCodeMappingId, d.Id, d.Combination));
                            //riDataBo.Log.Add(string.Format(MessageBag.TotalMappingDetailCombination, "TreatyBenefitCodeMapping", detailBos.Select(d => d.TreatyBenefitCodeMappingId).Distinct().Count()));
                            //riDataBo.Log.Add();
                        }
                    }
                    else
                    {
                        errors.Add(riDataBo.FormatProductFeatureMappingError(MessageBag.NoRecordMatchParams));
                    }
                }
            }

            string profitComm = null;
            int? maxAgeAtExpiry = null;
            int? minIssueAge = null;
            int? maxIssueAge = null;
            double? maxUwRating = null;
            double? apLoading = null;
            double? minAar = null;
            double? maxAar = null;
            double? ablAmount = null;
            double? retentionShare = null;
            double? retentionCap = null;
            double? riShare = null;
            double? riShareCap = null;
            double? riShare2 = null;
            double? riShareCap2 = null;
            double? serviceFee = null;
            double? wakalahFee = null;
            DateTime? effectiveDate = null;
            TreatyBenefitCodeMappingBo found = null;
            if (detailBo != null)
                if (detailBo.TreatyBenefitCodeMappingBo == null)
                    errors.Add(riDataBo.FormatProductFeatureMappingError("ProductFeatureMapping", MessageBag.NoRecordFound));
                else
                    found = detailBo.TreatyBenefitCodeMappingBo;

            if (found != null)
            {
                profitComm = found.ProfitCommPickListDetailBo?.Code;
                if (string.IsNullOrEmpty(profitComm) && !profitComms.IsNullOrEmpty())
                    profitCommErrors.Add(riDataBo.FormatProductFeatureMappingError("Profit Commission", "Empty"));

                maxAgeAtExpiry = found.MaxExpiryAge;
                if (!maxAgeAtExpiry.HasValue && !maxAgeAtExpiries.IsNullOrEmpty())
                    maxAgeAtExpiryErrors.Add(riDataBo.FormatProductFeatureMappingError("Max Age at Expiry", "Empty"));

                minIssueAge = found.MinIssueAge;
                if (!minIssueAge.HasValue && !minIssueAges.IsNullOrEmpty())
                    minIssueAgeErrors.Add(riDataBo.FormatProductFeatureMappingError("Min Issue Age", "Empty"));

                maxIssueAge = found.MaxIssueAge;
                if (!maxIssueAge.HasValue && !maxIssueAges.IsNullOrEmpty())
                    maxIssueAgeErrors.Add(riDataBo.FormatProductFeatureMappingError("Max Issue Age", "Empty"));

                maxUwRating = found.MaxUwRating;
                if (!maxUwRating.HasValue && !maxUwRatings.IsNullOrEmpty())
                    maxUwRatingErrors.Add(riDataBo.FormatProductFeatureMappingError("Max UW Rating", "Empty"));

                apLoading = found.ApLoading;
                if (!apLoading.HasValue && !apLoadings.IsNullOrEmpty())
                    apLoadingErrors.Add(riDataBo.FormatProductFeatureMappingError("AP Loading", "Empty"));

                minAar = found.MinAar;
                if (!minAar.HasValue && !minAars.IsNullOrEmpty())
                    minAarErrors.Add(riDataBo.FormatProductFeatureMappingError("Min AAR", "Empty"));

                maxAar = found.MaxAar;
                if (!maxAar.HasValue && !maxAars.IsNullOrEmpty())
                    maxAarErrors.Add(riDataBo.FormatProductFeatureMappingError("Max AAR", "Empty"));

                ablAmount = found.AblAmount;
                if (!ablAmount.HasValue && !ablAmounts.IsNullOrEmpty())
                    ablAmountErrors.Add(riDataBo.FormatProductFeatureMappingError("ABL Amount", "Empty"));

                retentionShare = found.RetentionShare;
                if (!retentionShare.HasValue && !retentionShares.IsNullOrEmpty())
                    retentionShareErrors.Add(riDataBo.FormatProductFeatureMappingError("Retention Share", "Empty"));

                retentionCap = found.RetentionCap;
                if (!retentionCap.HasValue && !retentionCaps.IsNullOrEmpty())
                    retentionCapErrors.Add(riDataBo.FormatProductFeatureMappingError("Retention Cap", "Empty"));

                riShare = found.RiShare;
                if (!riShare.HasValue && !riShares.IsNullOrEmpty())
                    riShareErrors.Add(riDataBo.FormatProductFeatureMappingError("RI Share 1", "Empty"));

                riShareCap = found.RiShareCap;
                if (!riShareCap.HasValue && !riShareCaps.IsNullOrEmpty())
                    riShareCapErrors.Add(riDataBo.FormatProductFeatureMappingError("RI Share Cap 1", "Empty"));

                riShare2 = found.RiShare2;
                if (!riShare2.HasValue && !riShare2s.IsNullOrEmpty())
                    riShare2Errors.Add(riDataBo.FormatProductFeatureMappingError("RI Share 2", "Empty"));

                riShareCap2 = found.RiShareCap2;
                if (!riShareCap2.HasValue && !riShareCap2s.IsNullOrEmpty())
                    riShareCap2Errors.Add(riDataBo.FormatProductFeatureMappingError("RI Share Cap 2", "Empty"));

                serviceFee = found.ServiceFee;
                if (!serviceFee.HasValue && !serviceFees.IsNullOrEmpty())
                    serviceFeeErrors.Add(riDataBo.FormatProductFeatureMappingError("Service Fee", "Empty"));

                wakalahFee = found.WakalahFee;
                if (!wakalahFee.HasValue && !wakalahFees.IsNullOrEmpty())
                    wakalahFeeErrors.Add(riDataBo.FormatProductFeatureMappingError("Wakalah Fee", "Empty"));

                effectiveDate = found.EffectiveDate;
                if (!effectiveDate.HasValue && !effectiveDates.IsNullOrEmpty())
                    effectiveDateErrors.Add(riDataBo.FormatProductFeatureMappingError("Effective Date", "Empty"));
            }

            if (
                errors.IsNullOrEmpty() &&
                profitCommErrors.IsNullOrEmpty() &&
                maxAgeAtExpiryErrors.IsNullOrEmpty() &&
                minIssueAgeErrors.IsNullOrEmpty() &&
                maxIssueAgeErrors.IsNullOrEmpty() &&
                maxUwRatingErrors.IsNullOrEmpty() &&
                apLoadingErrors.IsNullOrEmpty() &&
                minAarErrors.IsNullOrEmpty() &&
                maxAarErrors.IsNullOrEmpty() &&
                ablAmountErrors.IsNullOrEmpty() &&
                retentionShareErrors.IsNullOrEmpty() &&
                retentionCapErrors.IsNullOrEmpty() &&
                riShareErrors.IsNullOrEmpty() &&
                riShareCapErrors.IsNullOrEmpty() &&
                riShare2Errors.IsNullOrEmpty() &&
                riShareCap2Errors.IsNullOrEmpty() &&
                serviceFeeErrors.IsNullOrEmpty() &&
                wakalahFeeErrors.IsNullOrEmpty() &&
                effectiveDateErrors.IsNullOrEmpty()
            )
            {
                if (found != null)
                {
                    riDataBo.Log.Computation.Found();
                    riDataBo.Log.Computation.Parameter("CedingPlanCode", found.CedingPlanCode);
                    riDataBo.Log.Computation.Parameter("CampaignCode", found.CampaignCode);
                    riDataBo.Log.Computation.Parameter("ReinsEffDatePolStartDate", found.ReinsEffDatePolStartDate);
                    riDataBo.Log.Computation.Parameter("ReinsEffDatePolEndDate", found.ReinsEffDatePolEndDate);
                    riDataBo.Log.Computation.Parameter("AttainedAgeFrom", found.AttainedAgeFrom);
                    riDataBo.Log.Computation.Parameter("AttainedAgeTo", found.AttainedAgeTo);
                    riDataBo.Log.Computation.Parameter("TreatyCode", found.TreatyCodeBo?.Code);
                    riDataBo.Log.Computation.Parameter("MLReBenefitCode", found.BenefitBo?.Code);
                    riDataBo.Log.Computation.Add();
                    riDataBo.Log.Computation.MappedValue("MaxAgeAtExpiry", maxAgeAtExpiry);
                    riDataBo.Log.Computation.MappedValue("MinIssueAge", minIssueAge);
                    riDataBo.Log.Computation.MappedValue("MaxIssueAge", maxIssueAge);
                    riDataBo.Log.Computation.MappedValue("MaxUwRating", maxUwRating);
                    riDataBo.Log.Computation.MappedValue("ApLoading", apLoading);
                    riDataBo.Log.Computation.MappedValue("MinAar", minAar);
                    riDataBo.Log.Computation.MappedValue("MaxAar", maxAar);
                    riDataBo.Log.Computation.MappedValue("AblAmount", ablAmount);
                    riDataBo.Log.Computation.MappedValue("RertentionAmount", retentionShare);
                    riDataBo.Log.Computation.MappedValue("RetentionCap", retentionCap);
                    riDataBo.Log.Computation.MappedValue("RiShare", riShare);
                    riDataBo.Log.Computation.MappedValue("RiShareCap", riShareCap);
                    riDataBo.Log.Computation.MappedValue("RiShare2", riShare2);
                    riDataBo.Log.Computation.MappedValue("RiShareCap2", riShareCap2);
                    riDataBo.Log.Computation.MappedValue("ServiceFee", serviceFee);
                    riDataBo.Log.Computation.MappedValue("WakalahFee", wakalahFee);
                    riDataBo.Log.Computation.MappedValue("EffectiveDate", effectiveDate);
                }
                else
                {
                    riDataBo.Log.Computation.NotFound();
                }
            }
            else
            {
                riDataBo.SetComputationValidates(step, RiDataBo.ValidateKeyProductFeatureMapping);

                SetComputationErrors(profitComms, errors, profitCommErrors, "FeatureProfitCommMappingErrorCount", riDataBo, step);
                SetComputationErrors(maxAgeAtExpiries, errors, maxAgeAtExpiryErrors, "FeatureMaxAgeAtExpiryMappingErrorCount", riDataBo, step);
                SetComputationErrors(minIssueAges, errors, minIssueAgeErrors, "FeatureMinIssueAgeMappingErrorCount", riDataBo, step);
                SetComputationErrors(maxIssueAges, errors, maxIssueAgeErrors, "FeatureMaxIssueAgeMappingErrorCount", riDataBo, step);
                SetComputationErrors(maxUwRatings, errors, maxUwRatingErrors, "FeatureMaxUwRatingMappingErrorCount", riDataBo, step);
                SetComputationErrors(apLoadings, errors, apLoadingErrors, "FeatureApLoadingMappingErrorCount", riDataBo, step);
                SetComputationErrors(minAars, errors, minAarErrors, "FeatureMinAarMappingErrorCount", riDataBo, step);
                SetComputationErrors(maxAars, errors, maxAarErrors, "FeatureMaxAarMappingErrorCount", riDataBo, step);
                SetComputationErrors(ablAmounts, errors, ablAmountErrors, "FeatureAblAmountMappingErrorCount", riDataBo, step);
                SetComputationErrors(retentionShares, errors, retentionShareErrors, "FeatureRetentionShareMappingErrorCount", riDataBo, step);
                SetComputationErrors(retentionCaps, errors, retentionCapErrors, "FeatureRetentionCapMappingErrorCount", riDataBo, step);
                SetComputationErrors(riShares, errors, riShareErrors, "FeatureRiShareMappingErrorCount", riDataBo, step);
                SetComputationErrors(riShareCaps, errors, riShareCapErrors, "FeatureRiShareCapMappingErrorCount", riDataBo, step);
                SetComputationErrors(riShare2s, errors, riShare2Errors, "FeatureRiShare2MappingErrorCount", riDataBo, step);
                SetComputationErrors(riShareCap2s, errors, riShareCap2Errors, "FeatureRiShareCap2MappingErrorCount", riDataBo, step);
                SetComputationErrors(serviceFees, errors, serviceFeeErrors, "FeatureServiceFeeMappingErrorCount", riDataBo, step);
                SetComputationErrors(wakalahFees, errors, wakalahFeeErrors, "FeatureWakalahFeeMappingErrorCount", riDataBo, step);
                SetComputationErrors(effectiveDates, errors, effectiveDateErrors, "FeatureEffectiveDateMappingErrorCount", riDataBo, step);

                errors.AddRange(profitCommErrors);
                errors.AddRange(maxAgeAtExpiryErrors);
                errors.AddRange(minIssueAgeErrors);
                errors.AddRange(maxIssueAgeErrors);
                errors.AddRange(maxUwRatingErrors);
                errors.AddRange(apLoadingErrors);
                errors.AddRange(minAarErrors);
                errors.AddRange(maxAarErrors);
                errors.AddRange(ablAmountErrors);
                errors.AddRange(retentionShareErrors);
                errors.AddRange(retentionCapErrors);
                errors.AddRange(riShareErrors);
                errors.AddRange(riShareCapErrors);
                errors.AddRange(riShare2Errors);
                errors.AddRange(riShareCap2Errors);
                errors.AddRange(serviceFeeErrors);
                errors.AddRange(wakalahFeeErrors);
                errors.AddRange(effectiveDateErrors);

                riDataBo.Log.Computation.Errors(errors);

                // if error triggered
                profitComm = null;
                maxAgeAtExpiry = null;
                minIssueAge = null;
                maxIssueAge = null;
                maxUwRating = null;
                apLoading = null;
                minAar = null;
                maxAar = null;
                ablAmount = null;
                retentionShare = null;
                retentionCap = null;
                riShare = null;
                riShareCap = null;
                riShare2 = null;
                riShareCap2 = null;
                serviceFee = null;
                wakalahFee = null;
                effectiveDate = null;
            }

            ComputeProductFeatureMapping(profitComms, profitComm, "FeatureProfitCommMappingErrorCount", riDataBo, step);

            ComputeProductFeatureMapping(maxAgeAtExpiries, maxAgeAtExpiry, "FeatureMaxAgeAtExpiryMappingErrorCount", riDataBo, step);
            ComputeProductFeatureMapping(minIssueAges, minIssueAge, "FeatureMinIssueAgeMappingErrorCount", riDataBo, step);
            ComputeProductFeatureMapping(maxIssueAges, maxIssueAge, "FeatureMaxIssueAgeMappingErrorCount", riDataBo, step);

            ComputeProductFeatureMapping(maxUwRatings, maxUwRating, "FeatureMaxUwRatingMappingErrorCount", riDataBo, step);
            ComputeProductFeatureMapping(apLoadings, apLoading, "FeatureApLoadingMappingErrorCount", riDataBo, step);
            ComputeProductFeatureMapping(minAars, minAar, "FeatureMinAarMappingErrorCount", riDataBo, step);
            ComputeProductFeatureMapping(maxAars, maxAar, "FeatureMaxAarMappingErrorCount", riDataBo, step);
            ComputeProductFeatureMapping(ablAmounts, ablAmount, "FeatureAblAmountMappingErrorCount", riDataBo, step);
            ComputeProductFeatureMapping(retentionShares, retentionShare, "FeatureRetentionShareMappingErrorCount", riDataBo, step);
            ComputeProductFeatureMapping(retentionCaps, retentionCap, "FeatureRetentionCapMappingErrorCount", riDataBo, step);
            ComputeProductFeatureMapping(riShares, riShare, "FeatureRiShareMappingErrorCount", riDataBo, step);
            ComputeProductFeatureMapping(riShareCaps, riShareCap, "FeatureRiShareCapMappingErrorCount", riDataBo, step);
            ComputeProductFeatureMapping(riShare2s, riShare2, "FeatureRiShare2MappingErrorCount", riDataBo, step);
            ComputeProductFeatureMapping(riShareCap2s, riShareCap2, "FeatureRiShareCap2MappingErrorCount", riDataBo, step);
            ComputeProductFeatureMapping(serviceFees, serviceFee, "FeatureServiceFeeMappingErrorCount", riDataBo, step);
            ComputeProductFeatureMapping(wakalahFees, wakalahFee, "FeatureWakalahFeeMappingErrorCount", riDataBo, step);
            ComputeProductFeatureMapping(effectiveDates, effectiveDate, "FeatureEffectiveDateMappingErrorCount", riDataBo, step);

            LogRiDataFile.SwProductFeatureMapping.Stop();
        }

        public void ComputeCellMapping(IList<RiDataComputationBo> computations, object value, string errorCountName, RiDataBo riDataBo, int step)
        {
            Compute(computations, value, errorCountName, riDataBo, step, RiDataBo.CellMappingTitle, RiDataBo.ValidateKeyCellMapping);
        }

        public void ProcessCellMapping(
            IList<RiDataComputationBo> cellBasicRiders,
            IList<RiDataComputationBo> cellNames,
            IList<RiDataComputationBo> cellMfrs17TreatyCodes,
            IList<RiDataComputationBo> cellLoaCodes,
            RiDataBo riDataBo,
            int step
        )
        {
            riDataBo.Log.Computation.Title("ProcessCellMapping");
            if (
                cellBasicRiders.IsNullOrEmpty() &&
                cellNames.IsNullOrEmpty() &&
                cellMfrs17TreatyCodes.IsNullOrEmpty() &&
                cellLoaCodes.IsNullOrEmpty()
            )
            {
                riDataBo.Log.Computation.Failed(MessageBag.NoComputationConfigured);
                return;
            }

            cellBasicRiders = ValidateConditions(cellBasicRiders, riDataBo);
            cellNames = ValidateConditions(cellNames, riDataBo);
            cellMfrs17TreatyCodes = ValidateConditions(cellMfrs17TreatyCodes, riDataBo);
            cellLoaCodes = ValidateConditions(cellLoaCodes, riDataBo);
            if (
                cellBasicRiders.IsNullOrEmpty() &&
                cellNames.IsNullOrEmpty() &&
                cellMfrs17TreatyCodes.IsNullOrEmpty() &&
                cellLoaCodes.IsNullOrEmpty()
            )
            {
                riDataBo.Log.Computation.Failed(MessageBag.NoComputationCondition);
                return;
            }

            LogRiDataFile.SwCellMapping.Start();

            // NOTED: THIS IS FOR TESTING PURPOSE AND DO NOT UNCOMMENT IT
            //riDataBo.TreatyCode = "GEL-08";
            //riDataBo.ReinsBasisCode = "AUTO";
            //riDataBo.CedingPlanCode = "0248"; // optional field
            //riDataBo.MlreBenefitCode = "DEA"; // optional field
            //riDataBo.MlreBenefitCode = null; // optional field
            //riDataBo.ReinsEffDatePol = new DateTime(2013, 3, 31); // optional field
            //riDataBo.ReinsEffDatePol = null; // optional field

            riDataBo.Log.Computation.Parameters();
            riDataBo.Log.Computation.AddLines(FormatRiDataByTypes(RiDataBo.ParamsCellMapping(), riDataBo));
            riDataBo.Log.Computation.Add();

            Mfrs17CellMappingDetailBo detailBo = null;
            var cellBasicRiderErrors = new List<string> { };
            var cellNameErrors = new List<string> { };
            var cellMfrs17TreatyCodeErrors = new List<string> { };
            var cellLoaCodeErrors = new List<string> { };
            var errors = riDataBo.ValidateCellMapping();
            if (riDataBo.CellMappingValidate)
            {
                GlobalProcessRowRiDataConnectionStrategy.SetRiDataId(riDataBo.Id);
                var types = new List<int>
                {
                    StandardOutputBo.TypeReinsBasisCode,
                };
                var validateErrors = RiDataService.ValidateDropDownCodes(RiDataBo.CellMappingTitle, types, riDataBo);
                errors.AddRange(validateErrors);

                var treatyCodeBo = TreatyCodeService.FindByCode(riDataBo.TreatyCode);
                if (treatyCodeBo == null)
                    errors.Add(riDataBo.FormatCellMappingError(StandardOutputBo.TypeTreatyCode, string.Format(MessageBag.NoRecordFoundIn, "Treaty Code") + " TreatyCode: " + riDataBo.TreatyCode));

                if (errors.IsNullOrEmpty())
                {
                    // Search by params
                    var count = Mfrs17CellMappingDetailService.CountByParams(riDataBo, ProcessRiDataBatch.CacheService, true); // groupById
                    if (count > 0)
                    {
                        if (count == 1)
                        {
                            detailBo = ProcessRiDataBatch.CacheService.FindByCellMappingParams(riDataBo, true);
                            // If not found in cache
                            if (detailBo == null)
                            {
                                detailBo = Mfrs17CellMappingDetailService.FindByParams(riDataBo, ProcessRiDataBatch.CacheService, true); // groupById
                                //ProcessRiDataBatch.CacheService.Mfrs17CellMappingDetailBosForCellMapping.Add(detailBo);
                                ProcessRiDataBatch.CacheService.AddMappingBo(4, detailBo);
                            }
                        }
                        else
                        {
                            errors.Add(riDataBo.FormatCellMappingError(MessageBag.MultipleMappingRecordsFound));
                            //riDataBo.Log.Add("--Matched Parameters Records");
                            //var detailBos = Mfrs17CellMappingDetailService.GetByParams(riDataBo, ProcessRiDataBatch.CacheService);
                            //foreach (var d in detailBos)
                            //    riDataBo.Log.Add(string.Format(MessageBag.MappingDetailCombination, d.Mfrs17CellMappingId, d.Id, d.Combination));
                            //riDataBo.Log.Add(string.Format(MessageBag.TotalMappingDetailCombination, "Mfrs17CellMapping", detailBos.Select(d => d.Mfrs17CellMappingId).Distinct().Count()));
                            //riDataBo.Log.Add();
                        }
                    }
                    else
                    {
                        errors.Add(riDataBo.FormatCellMappingError(MessageBag.NoRecordMatchParams));
                    }
                }
            }

            string basicRider = null;
            string cellName = null;
            string mfrs17TreatyCode = null;
            string loaCode = null;
            Mfrs17CellMappingBo found = null;
            if (detailBo != null)
                if (detailBo.Mfrs17CellMappingBo == null)
                    errors.Add(riDataBo.FormatCellMappingError("Mfrs17CellMapping", MessageBag.NoRecordFound));
                else
                    found = detailBo.Mfrs17CellMappingBo;

            if (found != null)
            {
                if (!cellBasicRiders.IsNullOrEmpty())
                    if (found.BasicRiderPickListDetailBo == null)
                        cellBasicRiderErrors.Add(riDataBo.FormatCellMappingError("Basic Rider", string.Format(MessageBag.NoRecordFoundIn, "Pick List")));
                    else
                        basicRider = found.BasicRiderPickListDetailBo.Code;

                if (!cellNames.IsNullOrEmpty())
                    if (string.IsNullOrEmpty(found.CellName))
                        cellNameErrors.Add(riDataBo.FormatCellMappingError("Cell Name", "Empty"));
                    else
                        cellName = found.CellName;

                if (!cellMfrs17TreatyCodes.IsNullOrEmpty())
                {
                    if (!found.Mfrs17ContractCodeDetailId.HasValue)
                    {
                        cellMfrs17TreatyCodeErrors.Add(riDataBo.FormatCellMappingError("MFRS17 Contract Code", "Empty"));
                    }
                    else
                    {
                        if (found.Mfrs17ContractCodeDetailBo == null)
                            cellMfrs17TreatyCodeErrors.Add(riDataBo.FormatCellMappingError("MFRS17 Contract Code", string.Format(MessageBag.NoRecordFoundIn, "MFRS17 Contract Code")));
                        else
                            mfrs17TreatyCode = found.Mfrs17ContractCodeDetailBo.ContractCode;
                    }
                }

                //if (!cellMfrs17TreatyCodes.IsNullOrEmpty())
                //    if (string.IsNullOrEmpty(found.Mfrs17TreatyCode))
                //        cellMfrs17TreatyCodeErrors.Add(riDataBo.FormatCellMappingError("MFRS17 Contract Code", "Empty"));
                //    else
                //        mfrs17TreatyCode = found.Mfrs17TreatyCode;

                if (!cellLoaCodes.IsNullOrEmpty())
                    if (string.IsNullOrEmpty(found.LoaCode))
                        cellLoaCodeErrors.Add(riDataBo.FormatCellMappingError("LOA Code", "Empty"));
                    else
                        loaCode = found.LoaCode;
            }

            if (
                errors.IsNullOrEmpty() &&
                cellBasicRiderErrors.IsNullOrEmpty() &&
                cellNameErrors.IsNullOrEmpty() &&
                cellMfrs17TreatyCodeErrors.IsNullOrEmpty() &&
                cellLoaCodeErrors.IsNullOrEmpty()
            )
            {
                if (found != null)
                {
                    riDataBo.Log.Computation.Found();
                    riDataBo.Log.Computation.Parameter("TreatyCode", found.TreatyCode);
                    riDataBo.Log.Computation.Parameter("ProfitComm", found.ProfitComm);
                    riDataBo.Log.Computation.Parameter("ReinsBasisCode", found.ReinsBasisCodePickListDetailBo.Code);
                    riDataBo.Log.Computation.Parameter("CedingPlanCode", found.CedingPlanCode);
                    riDataBo.Log.Computation.Parameter("BenefitCode", found.BenefitCode);
                    riDataBo.Log.Computation.Parameter("ReinsEffDatePolStartDate", found.ReinsEffDatePolStartDate);
                    riDataBo.Log.Computation.Parameter("ReinsEffDatePolEndDate", found.ReinsEffDatePolEndDate);
                    riDataBo.Log.Computation.Add();
                    riDataBo.Log.Computation.MappedValue("BasicRider", basicRider);
                    riDataBo.Log.Computation.MappedValue("CellName", cellName);
                    riDataBo.Log.Computation.MappedValue("Mfrs17TreatyCode", mfrs17TreatyCode);
                    riDataBo.Log.Computation.MappedValue("LoaCode", loaCode);
                }
                else
                {
                    riDataBo.Log.Computation.NotFound();
                }
            }
            else
            {
                riDataBo.SetComputationValidates(step, RiDataBo.ValidateKeyCellMapping);

                SetComputationErrors(cellBasicRiders, errors, cellBasicRiderErrors, "CellBasicRiderMappingErrorCount", riDataBo, step);
                SetComputationErrors(cellNames, errors, cellNameErrors, "CellNameMappingErrorCount", riDataBo, step);
                SetComputationErrors(cellMfrs17TreatyCodes, errors, cellMfrs17TreatyCodeErrors, "CellMfrs17TreatyCodeMappingErrorCount", riDataBo, step);
                SetComputationErrors(cellLoaCodes, errors, cellLoaCodeErrors, "CellLoaCodeMappingErrorCount", riDataBo, step);

                errors.AddRange(cellBasicRiderErrors);
                errors.AddRange(cellNameErrors);
                errors.AddRange(cellMfrs17TreatyCodeErrors);
                errors.AddRange(cellLoaCodeErrors);

                riDataBo.Log.Computation.Errors(errors);

                // if error triggered
                basicRider = null;
                cellName = null;
                mfrs17TreatyCode = null;
                loaCode = null;
            }

            ComputeCellMapping(cellBasicRiders, basicRider, "CellBasicRiderMappingErrorCount", riDataBo, step);
            ComputeCellMapping(cellNames, cellName, "CellNameMappingErrorCount", riDataBo, step);
            ComputeCellMapping(cellMfrs17TreatyCodes, mfrs17TreatyCode, "CellMfrs17TreatyCodeMappingErrorCount", riDataBo, step);
            ComputeCellMapping(cellLoaCodes, loaCode, "CellLoaCodeMappingErrorCount", riDataBo, step);

            LogRiDataFile.SwCellMapping.Stop();
        }

        public void ProcessRateTableMapping(
            IList<RiDataComputationBo> rateTableCodes,
            IList<RiDataComputationBo> ratePerBasises,
            IList<RiDataComputationBo> ratesByPreviousAge,
            IList<RiDataComputationBo> ratesByCurrentAge,
            IList<RiDataComputationBo> riDiscounts,
            IList<RiDataComputationBo> largeDiscounts,
            IList<RiDataComputationBo> groupDiscounts,
            RiDataBo riDataBo,
            int step
        )
        {
            riDataBo.Log.Computation.Title("ProcessRateTableMapping");
            if (
                rateTableCodes.IsNullOrEmpty() &&
                ratePerBasises.IsNullOrEmpty() &&
                ratesByPreviousAge.IsNullOrEmpty() &&
                ratesByCurrentAge.IsNullOrEmpty() &&
                riDiscounts.IsNullOrEmpty() &&
                largeDiscounts.IsNullOrEmpty() &&
                groupDiscounts.IsNullOrEmpty()
            )
            {
                riDataBo.Log.Computation.Failed(MessageBag.NoComputationConfigured);
                return;
            }

            rateTableCodes = ValidateConditions(rateTableCodes, riDataBo);
            ratePerBasises = ValidateConditions(ratePerBasises, riDataBo);
            ratesByPreviousAge = ValidateConditions(ratesByPreviousAge, riDataBo);
            ratesByCurrentAge = ValidateConditions(ratesByCurrentAge, riDataBo);
            riDiscounts = ValidateConditions(riDiscounts, riDataBo);
            largeDiscounts = ValidateConditions(largeDiscounts, riDataBo);
            groupDiscounts = ValidateConditions(groupDiscounts, riDataBo);
            if (
                rateTableCodes.IsNullOrEmpty() &&
                ratePerBasises.IsNullOrEmpty() &&
                ratesByPreviousAge.IsNullOrEmpty() &&
                ratesByCurrentAge.IsNullOrEmpty() &&
                riDiscounts.IsNullOrEmpty() &&
                largeDiscounts.IsNullOrEmpty() &&
                groupDiscounts.IsNullOrEmpty()
            )
            {
                riDataBo.Log.Computation.Failed(MessageBag.NoComputationCondition);
                return;
            }

            LogRiDataFile.SwRateTableMapping.Start();

            // NOTED: THIS IS FOR TESTING PURPOSE AND DO NOT UNCOMMENT IT
            //riDataBo.TreatyCode = "UNI-03";
            //riDataBo.CedingPlanCode = "P/T/23/N"; // optional field
            //riDataBo.CedingTreatyCode = null; // optional field (new)
            //riDataBo.CedingPlanCode2 = null; // optional field (new)
            //riDataBo.CedingBenefitTypeCode = null; // optional field (new)
            //riDataBo.CedingBenefitRiskCode = null; // optional field (new)
            //riDataBo.GroupPolicyNumber = null; // optional field (new)
            //riDataBo.ReinsBasisCode = "AUTO"; // optional field (new)
            //riDataBo.MlreBenefitCode = "DEA"; // optional field
            //riDataBo.InsuredGenderCode = "M"; // optional field
            //riDataBo.InsuredTobaccoUse = null; // optional field
            //riDataBo.InsuredOccupationCode = "I"; // optional field
            //riDataBo.InsuredAttainedAge = 30; // optional field
            //riDataBo.PremiumFrequencyCode = "A"; // optional field
            //riDataBo.Aar = 5000; // optional field (new)
            //riDataBo.OriSumAssured = 5000; // optional field
            //riDataBo.ReinsEffDatePol = new DateTime(2013, 3, 31); // optional field

            riDataBo.Log.Computation.Parameters();
            riDataBo.Log.Computation.AddLines(FormatRiDataByTypes(RiDataBo.ParamsRateTableMapping(), riDataBo));
            riDataBo.Log.Computation.Add();

            RateTableDetailBo detailBo = null;
            var rateTableCodeErrors = new List<string> { };
            var ratePerBasisErrors = new List<string> { };
            var rateByPreviousAgeErrors = new List<string> { };
            var rateByCurrentAgeErrors = new List<string> { };
            var riDiscountErrors = new List<string> { };
            var largeDiscountErrors = new List<string> { };
            var groupDiscountErrors = new List<string> { };
            var errors = riDataBo.ValidateRateTableMapping(out DateTime? reportDate);
            if (riDataBo.RateTableMappingValidate)
            {
                GlobalProcessRowRiDataConnectionStrategy.SetRiDataId(riDataBo.Id);
                var types = new List<int>
                {
                    StandardOutputBo.TypePremiumFrequencyCode,
                    StandardOutputBo.TypeReinsBasisCode, // (new)
                };
                var validateErrors = RiDataService.ValidateDropDownCodes(RiDataBo.RateTableMappingTitle, types, riDataBo);
                errors.AddRange(validateErrors);

                if (errors.IsNullOrEmpty())
                {
                    riDataBo.Log.Computation.Detail("ReportDate", reportDate);

                    // Search by params
                    var count = RateTableDetailService.CountByParams(riDataBo, ProcessRiDataBatch.CacheService, reportDate, true); // groupById
                    if (count > 0)
                    {
                        if (count == 1)
                        {
                            detailBo = ProcessRiDataBatch.CacheService.FindByRateTableMappingParams(riDataBo, reportDate, true);
                            // If not found in cache
                            if (detailBo == null)
                            {
                                detailBo = RateTableDetailService.FindByParams(riDataBo, ProcessRiDataBatch.CacheService, reportDate, true); // groupById
                                //ProcessRiDataBatch.CacheService.RateTableDetailBosForRateTableMapping.Add(detailBo);
                                ProcessRiDataBatch.CacheService.AddMappingBo(5, detailBo);
                            }
                        }
                        else
                        {
                            errors.Add(riDataBo.FormatRateTableMappingError(MessageBag.MultipleMappingRecordsFound));
                            //riDataBo.Log.Add("--Matched Parameters Records");
                            //var detailBos = RateTableDetailService.GetByParams(riDataBo);
                            //foreach (var d in detailBos)
                            //    riDataBo.Log.Add(string.Format(MessageBag.MappingDetailCombination, d.RateTableId, d.Id, d.Combination));
                            //riDataBo.Log.Add(string.Format(MessageBag.TotalMappingDetailCombination, "RateTable", detailBos.Select(d => d.RateTableId).Distinct().Count()));
                            //riDataBo.Log.Add();
                        }
                    }
                    else
                    {
                        errors.Add(riDataBo.FormatRateTableMappingError(MessageBag.NoRecordMatchParams));
                    }
                }
            }

            RateBo rateBo = null;
            RateTableBo found = null;
            string riDiscountCode = null;
            string largeDiscountCode = null;
            string groupDiscountCode = null;
            if (detailBo != null)
                if (detailBo.RateTableBo == null)
                    errors.Add(riDataBo.FormatRateTableMappingError("RateTableMapping", MessageBag.NoRecordFound));
                else
                    found = detailBo.RateTableBo;

            if (found != null)
            {
                rateBo = found.RateBo;
                if (rateBo == null)
                {
                    if (!rateTableCodes.IsNullOrEmpty())
                        rateTableCodeErrors.Add(riDataBo.FormatRateTableMappingError("RateTable", MessageBag.NoRecordFound));

                    if (!ratePerBasises.IsNullOrEmpty())
                        ratePerBasisErrors.Add(riDataBo.FormatRateTableMappingError("RateTable", MessageBag.NoRecordFound));

                    if (!ratesByPreviousAge.IsNullOrEmpty())
                        rateByPreviousAgeErrors.Add(riDataBo.FormatRateTableMappingError("RateTable", MessageBag.NoRecordFound));

                    if (!ratesByCurrentAge.IsNullOrEmpty())
                        rateByCurrentAgeErrors.Add(riDataBo.FormatRateTableMappingError("RateTable", MessageBag.NoRecordFound));
                }

                riDiscountCode = found.RiDiscountCode;
                largeDiscountCode = found.LargeDiscountCode;
                groupDiscountCode = found.GroupDiscountCode;

                if (string.IsNullOrEmpty(riDiscountCode) && !riDiscounts.IsNullOrEmpty())
                    riDiscountErrors.Add(riDataBo.FormatRateTableMappingError("RiDiscount", string.Format(MessageBag.IsNullWithName, "RI Discount Code")));

                if (string.IsNullOrEmpty(largeDiscountCode) && !largeDiscounts.IsNullOrEmpty())
                    largeDiscountErrors.Add(riDataBo.FormatRateTableMappingError("LargeDiscount", string.Format(MessageBag.IsNullWithName, "Large Discount Code")));

                if (string.IsNullOrEmpty(groupDiscountCode) && !groupDiscounts.IsNullOrEmpty())
                    groupDiscountErrors.Add(riDataBo.FormatRateTableMappingError("GroupDiscount", string.Format(MessageBag.IsNullWithName, "Group Discount Code")));
            }

            if (
                errors.IsNullOrEmpty() &&
                rateTableCodeErrors.IsNullOrEmpty() &&
                ratePerBasisErrors.IsNullOrEmpty() &&
                rateByPreviousAgeErrors.IsNullOrEmpty() &&
                rateByCurrentAgeErrors.IsNullOrEmpty() &&
                riDiscountErrors.IsNullOrEmpty() &&
                largeDiscountErrors.IsNullOrEmpty() &&
                groupDiscountErrors.IsNullOrEmpty()
            )
            {
                if (found != null)
                {
                    riDataBo.Log.Computation.Found();
                    riDataBo.Log.Computation.Parameter("RateTableId", found.Id);
                    riDataBo.Log.Computation.Parameter("TreatyCode", found.TreatyCode);
                    riDataBo.Log.Computation.Parameter("CedingPlanCode", found.CedingPlanCode);
                    riDataBo.Log.Computation.Parameter("CedingPlanCode2", found.CedingPlanCode2);
                    riDataBo.Log.Computation.Parameter("CedingBenefitTypeCode", found.CedingBenefitTypeCode);
                    riDataBo.Log.Computation.Parameter("CedingBenefitRiskCode", found.CedingBenefitRiskCode);
                    riDataBo.Log.Computation.Parameter("GroupPolicyNumber", found.GroupPolicyNumber);
                    riDataBo.Log.Computation.Parameter("ReinsBasisCode", found.ReinsBasisCodePickListDetailBo != null ? found.ReinsBasisCodePickListDetailBo.Code : Util.Null);
                    riDataBo.Log.Computation.Parameter("BenefitBo", found.BenefitBo != null ? found.BenefitBo.Code : Util.Null);
                    riDataBo.Log.Computation.Parameter("AttainedAgeFrom", found.AttainedAgeFrom);
                    riDataBo.Log.Computation.Parameter("AttainedAgeTo", found.AttainedAgeTo);
                    riDataBo.Log.Computation.Parameter("PolicyAmountFrom", found.PolicyAmountFrom);
                    riDataBo.Log.Computation.Parameter("PolicyAmountTo", found.PolicyAmountTo);
                    riDataBo.Log.Computation.Parameter("AarFrom", found.AarFrom);
                    riDataBo.Log.Computation.Parameter("AarTo", found.AarTo);
                    riDataBo.Log.Computation.Parameter("ReinsEffDatePolStartDate", found.ReinsEffDatePolStartDate);
                    riDataBo.Log.Computation.Parameter("ReinsEffDatePolEndDate", found.ReinsEffDatePolEndDate);

                    riDataBo.Log.Computation.Add();
                    if (rateBo != null)
                        riDataBo.Log.Computation.MappedValue("RateTable", rateBo.Code);
                    //if (riDiscountBo != null)
                    //    riDataBo.Log.Computation.MappedValue("RiDiscount", riDiscountBo.DiscountCode);
                    //if (largeDiscountBo != null)
                    //    riDataBo.Log.Computation.MappedValue("LargeDiscount", largeDiscountBo.DiscountCode);
                    //if (groupDiscountBo != null)
                    //    riDataBo.Log.Computation.MappedValue("GroupDiscount", groupDiscountBo.DiscountCode);
                }
                else
                {
                    riDataBo.Log.Computation.NotFound();
                }
            }
            else
            {
                riDataBo.SetComputationValidates(step, RiDataBo.ValidateKeyRateTableMapping);

                SetComputationErrors(rateTableCodes, errors, rateTableCodeErrors, "RateTableCodeMappingErrorCount", riDataBo, step);
                SetComputationErrors(ratePerBasises, errors, ratePerBasisErrors, "RateTableRatePerBasisMappingErrorCount", riDataBo, step);
                SetComputationErrors(ratesByPreviousAge, errors, rateByPreviousAgeErrors, "RateTableRateByPreviousAgeMappingErrorCount", riDataBo, step);
                SetComputationErrors(ratesByCurrentAge, errors, rateByCurrentAgeErrors, "RateTableRateByCurrentAgeMappingErrorCount", riDataBo, step);
                SetComputationErrors(riDiscounts, errors, riDiscountErrors, "RateTableRiDiscountMappingErrorCount", riDataBo, step);
                SetComputationErrors(largeDiscounts, errors, largeDiscountErrors, "RateTableLargeDiscountMappingErrorCount", riDataBo, step);
                SetComputationErrors(groupDiscounts, errors, groupDiscountErrors, "RateTableGroupDiscountMappingErrorCount", riDataBo, step);

                errors.AddRange(rateTableCodeErrors);
                errors.AddRange(ratePerBasisErrors);
                errors.AddRange(rateByPreviousAgeErrors);
                errors.AddRange(rateByCurrentAgeErrors);
                errors.AddRange(riDiscountErrors);
                errors.AddRange(largeDiscountErrors);
                errors.AddRange(groupDiscountErrors);

                riDataBo.Log.Computation.Errors(errors);

                // if error triggered
                rateBo = null;
                riDiscountCode = null;
                largeDiscountCode = null;
                groupDiscountCode = null;
            }

            string valuationRateName = null;
            int? ratePerBasis = null;
            if (rateBo != null)
            {
                valuationRateName = riDataBo.FormatValuationRateName(rateBo.Code, rateBo.ValuationRate);
                ratePerBasis = rateBo.RatePerBasis;
            }

            Compute(rateTableCodes, valuationRateName, "RateTableRatePerBasisMappingErrorCount", riDataBo, step, RiDataBo.RateTableMappingTitle, RiDataBo.ValidateKeyRateTableMapping);
            Compute(ratePerBasises, ratePerBasis, "RateTableRatePerBasisMappingErrorCount", riDataBo, step, RiDataBo.RateTableMappingTitle, RiDataBo.ValidateKeyRateTableMapping);
            ComputeRate(ratesByPreviousAge, "RateTableRateByPreviousAgeMappingErrorCount", riDataBo, step, rateBo, "Rate Table - Rate (Previous Age)", false);
            ComputeRate(ratesByCurrentAge, "RateTableRateByCurrentAgeMappingErrorCount", riDataBo, step, rateBo, "Rate Table - Rate (Current Age)", true);

            ComputeRiDiscount(riDiscounts, riDataBo, step, riDiscountCode);
            ComputeLargeDiscount(largeDiscounts, riDataBo, step, largeDiscountCode);
            ComputeGroupDiscount(groupDiscounts, riDataBo, step, groupDiscountCode);

            LogRiDataFile.SwRateTableMapping.Stop();
        }

        public void ComputeRate(
            IList<RiDataComputationBo> computations,
            string errorCountName,
            RiDataBo riDataBo,
            int step,
            RateBo rateBo,
            string title,
            bool isCurrentAge = false)
        {
            if (rateBo == null || computations.IsNullOrEmpty())
                return;

            int valuationRate = rateBo.ValuationRate;
            var fields = RateBo.GetRequiredStandardOutputFields(valuationRate, isCurrentAge);

            GlobalProcessRowRiDataConnectionStrategy.SetRiDataId(riDataBo.Id);

            riDataBo.Log.Computation.Title("ComputeRate");
            riDataBo.Log.Computation.AddLines(FormatRiDataByTypes(RiDataBo.ParamsRateMapping(fields), riDataBo));

            RateDetailBo detailBo = null;
            var errors = riDataBo.ValidateRateMapping(fields);
            if (errors.IsNullOrEmpty())
            {
                var types = RateBo.GetRequiredDropDownFields(rateBo.ValuationRate);
                var validateErrors = RiDataService.ValidateDropDownCodes(RiDataBo.RateMappingTitle, types, riDataBo);
                errors.AddRange(validateErrors);

                var count = RateDetailService.CountByRateIdByParams(rateBo.Id, riDataBo, ProcessRiDataBatch.CacheService, isCurrentAge);
                if (count > 0)
                {
                    if (count == 1)
                    {
                        detailBo = RateDetailService.FindByRateIdByParams(rateBo.Id, riDataBo, ProcessRiDataBatch.CacheService, isCurrentAge);
                    }
                    else
                    {
                        errors.Add(riDataBo.FormatRateTableMappingError(title, MessageBag.MultipleMappingRecordsFound));
                    }
                }
                else
                {
                    errors.Add(riDataBo.FormatRateTableMappingError(title, MessageBag.NoRecordMatchParams));
                }
            }

            if (errors.IsNullOrEmpty())
            {
                if (detailBo != null)
                {
                    riDataBo.Log.Computation.Found();
                    riDataBo.Log.Computation.Parameter("InsuredGenderCode", detailBo.InsuredGenderCodePickListDetailBo?.Code);
                    riDataBo.Log.Computation.Parameter("CedingTobaccoUse", detailBo.CedingTobaccoUsePickListDetailBo?.Code);
                    riDataBo.Log.Computation.Parameter("CedingOccupationCode", detailBo.CedingOccupationCodePickListDetailBo?.Code);
                    riDataBo.Log.Computation.Parameter("AttainedAge", detailBo.AttainedAge);
                    riDataBo.Log.Computation.Parameter("IssueAge", detailBo.AttainedAge);
                    riDataBo.Log.Computation.Parameter("PolicyTerm", detailBo.PolicyTerm);
                    riDataBo.Log.Computation.Parameter("PolicyTermRemain", detailBo.PolicyTermRemain);
                    riDataBo.Log.Computation.Add();
                    riDataBo.Log.Computation.MappedValue("Rate", detailBo.RateValue);
                }
                else
                {
                    riDataBo.Log.Computation.NotFound();
                }
            }
            else
            {
                riDataBo.SetComputationValidates(step, RiDataBo.ValidateKeyRateTableMapping);
                riDataBo.Log.Computation.Errors(errors);
                SetComputationErrors(computations, errors, errorCountName, riDataBo, step);
                detailBo = null;
            }

            if (detailBo != null)
            {
                double rateValue = detailBo.RateValue;
                Compute(computations, rateValue, errorCountName, riDataBo, step, RiDataBo.RateTableMappingTitle, RiDataBo.ValidateKeyRateTableMapping);
            }
        }

        public void ComputeRiDiscount(IList<RiDataComputationBo> computations, RiDataBo riDataBo, int step, string riDiscountCode)
        {
            if (computations.IsNullOrEmpty() || string.IsNullOrEmpty(riDiscountCode))
                return;

            riDataBo.Log.Computation.Title("ComputeRiDiscount");
            riDataBo.Log.Computation.AddLines(FormatRiDataByTypes(RiDataBo.ParamsRiDiscountMapping(), riDataBo));

            RiDiscountBo riDiscountBo = null;
            var errors = riDataBo.ValidateRiDiscountMapping();
            if (errors.IsNullOrEmpty())
            {
                var count = RiDiscountService.CountByParams(riDataBo, riDiscountCode);
                if (count > 0)
                {
                    if (count == 1)
                    {
                        riDiscountBo = RiDiscountService.FindByParams(riDataBo, riDiscountCode);
                    }
                    else
                    {
                        errors.Add(riDataBo.FormatRiDiscountMappingError(MessageBag.MultipleMappingRecordsFound));
                    }
                }
                else
                {
                    errors.Add(riDataBo.FormatRiDiscountMappingError(MessageBag.NoRecordMatchParams));
                }
            }

            if (errors.IsNullOrEmpty())
            {
                if (riDiscountBo != null)
                {
                    riDataBo.Log.Computation.Found();
                    riDataBo.Log.Computation.Parameter("EffectiveStartDate", riDiscountBo.EffectiveStartDate);
                    riDataBo.Log.Computation.Parameter("EffectiveEndDate", riDiscountBo.EffectiveEndDate);
                    riDataBo.Log.Computation.Parameter("DurationFrom", riDiscountBo.DurationFrom);
                    riDataBo.Log.Computation.Parameter("DurationTo", riDiscountBo.DurationTo);
                    riDataBo.Log.Computation.Add();
                    riDataBo.Log.Computation.MappedValue("RI Discount", riDiscountBo.Discount);
                }
                else
                {
                    riDataBo.Log.Computation.NotFound();
                }
            }
            else
            {
                riDataBo.SetComputationValidates(step, RiDataBo.ValidateKeyRateTableMapping);
                riDataBo.Log.Computation.Errors(errors);
                SetComputationErrors(computations, errors, "RateTableRiDiscountMappingErrorCount", riDataBo, step);

                // if error triggered
                riDiscountBo = null;
            }

            if (riDiscountBo != null)
            {
                double discountValue = riDiscountBo.Discount;
                Compute(computations, discountValue, "RateTableRiDiscountMappingErrorCount", riDataBo, step, RiDataBo.RateTableMappingTitle, RiDataBo.ValidateKeyRateTableMapping);
            }
        }

        public void ComputeLargeDiscount(IList<RiDataComputationBo> computations, RiDataBo riDataBo, int step, string largeDiscountCode)
        {
            if (computations.IsNullOrEmpty() || string.IsNullOrEmpty(largeDiscountCode))
                return;

            riDataBo.Log.Computation.Title("ComputeLargeDiscount");
            riDataBo.Log.Computation.AddLines(FormatRiDataByTypes(RiDataBo.ParamsLargeDiscountMapping(), riDataBo));

            LargeDiscountBo largeDiscountBo = null;
            var errors = riDataBo.ValidateLargeDiscountMapping();
            if (errors.IsNullOrEmpty())
            {
                var count = LargeDiscountService.CountByParams(riDataBo, largeDiscountCode);
                if (count > 0)
                {
                    if (count == 1)
                    {
                        largeDiscountBo = LargeDiscountService.FindByParams(riDataBo, largeDiscountCode);
                    }
                    else
                    {
                        errors.Add(riDataBo.FormatLargeDiscountMappingError(MessageBag.MultipleMappingRecordsFound));
                    }
                }
                else
                {
                    errors.Add(riDataBo.FormatLargeDiscountMappingError(MessageBag.NoRecordMatchParams));
                }
            }

            if (errors.IsNullOrEmpty())
            {
                if (largeDiscountBo != null)
                {
                    riDataBo.Log.Computation.Found();
                    riDataBo.Log.Computation.Parameter("EffectiveStartDate", largeDiscountBo.EffectiveStartDate);
                    riDataBo.Log.Computation.Parameter("EffectiveEndDate", largeDiscountBo.EffectiveEndDate);
                    riDataBo.Log.Computation.Parameter("OriSumAssuredFrom", largeDiscountBo.AarFrom);
                    riDataBo.Log.Computation.Parameter("OriSumAssuredTo", largeDiscountBo.AarTo);
                    riDataBo.Log.Computation.Add();
                    riDataBo.Log.Computation.MappedValue("Large Discount", largeDiscountBo.Discount);
                }
                else
                {
                    riDataBo.Log.Computation.NotFound();
                }
            }
            else
            {
                riDataBo.SetComputationValidates(step, RiDataBo.ValidateKeyRateTableMapping);
                riDataBo.Log.Computation.Errors(errors);
                SetComputationErrors(computations, errors, "RateTableLargeDiscountMappingErrorCount", riDataBo, step);

                // if error triggered
                largeDiscountBo = null;
            }

            if (largeDiscountBo != null)
            {
                double discountValue = largeDiscountBo.Discount;
                Compute(computations, discountValue, "RateTableLargeDiscountMappingErrorCount", riDataBo, step, RiDataBo.RateTableMappingTitle, RiDataBo.ValidateKeyRateTableMapping);
            }
        }

        public void ComputeGroupDiscount(IList<RiDataComputationBo> computations, RiDataBo riDataBo, int step, string groupDiscountCode)
        {
            if (computations.IsNullOrEmpty() || string.IsNullOrEmpty(groupDiscountCode))
                return;

            riDataBo.Log.Computation.Title("ComputeGroupDiscount");
            riDataBo.Log.Computation.AddLines(FormatRiDataByTypes(RiDataBo.ParamsGroupDiscountMapping(), riDataBo));

            GroupDiscountBo groupDiscountBo = null;
            var errors = riDataBo.ValidateGroupDiscountMapping();
            if (errors.IsNullOrEmpty())
            {
                var count = GroupDiscountService.CountByParams(riDataBo, groupDiscountCode);
                if (count > 0)
                {
                    if (count == 1)
                    {
                        groupDiscountBo = GroupDiscountService.FindByParams(riDataBo, groupDiscountCode);
                    }
                    else
                    {
                        errors.Add(riDataBo.FormatGroupDiscountMappingError(MessageBag.MultipleMappingRecordsFound));
                    }
                }
                else
                {
                    errors.Add(riDataBo.FormatGroupDiscountMappingError(MessageBag.NoRecordMatchParams));
                }
            }

            if (errors.IsNullOrEmpty())
            {
                if (groupDiscountBo != null)
                {
                    riDataBo.Log.Computation.Found();
                    riDataBo.Log.Computation.Parameter("EffectiveStartDate", groupDiscountBo.EffectiveStartDate);
                    riDataBo.Log.Computation.Parameter("EffectiveEndDate", groupDiscountBo.EffectiveEndDate);
                    riDataBo.Log.Computation.Parameter("GroupSizeFrom", groupDiscountBo.GroupSizeFrom);
                    riDataBo.Log.Computation.Parameter("GroupSizeTo", groupDiscountBo.GroupSizeTo);
                    riDataBo.Log.Computation.Add();
                    riDataBo.Log.Computation.MappedValue("Group Discount", groupDiscountBo.Discount);
                }
                else
                {
                    riDataBo.Log.Computation.NotFound();
                }
            }
            else
            {
                riDataBo.SetComputationValidates(step, RiDataBo.ValidateKeyRateTableMapping);
                riDataBo.Log.Computation.Errors(errors);
                SetComputationErrors(computations, errors, "RateTableGroupDiscountMappingErrorCount", riDataBo, step);

                // if error triggered
                groupDiscountBo = null;
            }

            if (groupDiscountBo != null)
            {
                double discountValue = groupDiscountBo.Discount;
                Compute(computations, discountValue, "RateTableGroupDiscountMappingErrorCount", riDataBo, step, RiDataBo.RateTableMappingTitle, RiDataBo.ValidateKeyRateTableMapping);
            }
        }

        public void ProcessAnnuityFactorMapping(IList<RiDataComputationBo> computations, RiDataBo riDataBo, int step)
        {
            riDataBo.Log.Computation.Title("ProcessAnnuityFactorMapping");
            if (computations.IsNullOrEmpty())
            {
                riDataBo.Log.Computation.Failed(MessageBag.NoComputationConfigured);
                return;
            }

            computations = ValidateConditions(computations, riDataBo);
            if (computations.IsNullOrEmpty())
            {
                riDataBo.Log.Computation.Failed(MessageBag.NoComputationCondition);
                return;
            }

            LogRiDataFile.SwAnnuityFactorMapping.Start();

            // NOTED: THIS IS FOR TESTING PURPOSE AND DO NOT UNCOMMENT IT
            //riData.CedingPlanCode = "T21";
            //riData.ReinsEffDatePol = new DateTime(2013, 3, 31);
            //riData.PolicyTermRemain = 1;

            riDataBo.Log.Computation.Parameters();
            riDataBo.Log.Computation.AddLines(FormatRiDataByTypes(RiDataBo.ParamsAnnuityFactorMapping(), riDataBo));
            riDataBo.Log.Computation.Add();

            AnnuityFactorMappingBo detailBo = null;
            var errors = riDataBo.ValidateAnnuityFactorMapping();
            if (riDataBo.AnnuityFactorMappingValidate)
            {
                GlobalProcessRowRiDataConnectionStrategy.SetRiDataId(riDataBo.Id);
                if (errors.IsNullOrEmpty())
                {
                    // Search by params
                    var count = AnnuityFactorMappingService.CountByParams(riDataBo, true); // groupById
                    if (count > 0)
                    {
                        if (count == 1)
                        {
                            detailBo = ProcessRiDataBatch.CacheService.FindByAnnuityFactorMappingParams(riDataBo, true);
                            // If not found in cache
                            if (detailBo == null)
                            {
                                detailBo = AnnuityFactorMappingService.FindByParams(riDataBo, true); // groupById
                                //ProcessRiDataBatch.CacheService.AnnuityFactorMappingBosForAnnuityFactorMapping.Add(detailBo);
                                ProcessRiDataBatch.CacheService.AddMappingBo(6, detailBo);
                            }
                        }
                        else
                        {
                            errors.Add(riDataBo.FormatAnnuityFactorMappingError(MessageBag.MultipleMappingRecordsFound));
                            //riDataBo.Log.Add("--Matched Parameters Records");
                            //var detailBos = AnnuityFactorMappingService.GetByParams(riDataBo);
                            //foreach (var d in detailBos)
                            //    riDataBo.Log.Add(string.Format(MessageBag.MappingDetailCombination, d.AnnuityFactorId, d.Id, d.Combination));
                            //riDataBo.Log.Add(string.Format(MessageBag.TotalMappingDetailCombination, "AnnuityFactorMapping", detailBos.Select(d => d.AnnuityFactorId).Distinct().Count()));
                            //riDataBo.Log.Add();
                        }
                    }
                    else
                    {
                        errors.Add(riDataBo.FormatAnnuityFactorMappingError(MessageBag.NoRecordMatchParams));
                    }
                }
            }

            AnnuityFactorBo found = null;
            if (detailBo != null)
                if (detailBo.AnnuityFactorBo == null)
                    errors.Add(riDataBo.FormatTreatyMappingError("AnnuityFactorMapping", MessageBag.NoRecordFound));
                else
                    found = detailBo.AnnuityFactorBo;

            double? annuityFactor = null;
            AnnuityFactorDetailBo annuityFactorDetailBo = null;
            if (found != null)
            {
                // Search by params
                var count = AnnuityFactorDetailService.CountByAnnuityFactorIdByParams(found.Id, riDataBo, ProcessRiDataBatch.CacheService);
                if (count > 0)
                    if (count == 1)
                        annuityFactorDetailBo = AnnuityFactorDetailService.FindByAnnuityFactorIdByParams(found.Id, riDataBo, ProcessRiDataBatch.CacheService);
                    else
                        errors.Add(riDataBo.FormatAnnuityFactorRateError(MessageBag.MultipleMappingRecordsFound));
                else
                    errors.Add(riDataBo.FormatAnnuityFactorRateError(MessageBag.NoRecordMatchParams));
            }

            if (annuityFactorDetailBo != null)
                annuityFactor = annuityFactorDetailBo.AnnuityFactorValue;

            if (errors.IsNullOrEmpty())
            {
                if (annuityFactor != null)
                {
                    riDataBo.Log.Computation.Found();
                    riDataBo.Log.Computation.Parameter("CedingPlanCode", found.CedingPlanCode);
                    riDataBo.Log.Computation.Parameter("ReinsEffDatePolStartDate", found.ReinsEffDatePolStartDate);
                    riDataBo.Log.Computation.Parameter("ReinsEffDatePolEndDate", found.ReinsEffDatePolEndDate);
                    riDataBo.Log.Computation.Parameter("PolicyTermRemain", annuityFactorDetailBo.PolicyTermRemain);
                    riDataBo.Log.Computation.Add();
                    riDataBo.Log.Computation.MappedValue("AnnuityFactor", annuityFactor);
                }
                else
                {
                    riDataBo.Log.Computation.NotFound();
                }
            }
            else
            {
                riDataBo.SetComputationValidates(step, RiDataBo.ValidateKeyAnnuityFactorMapping);
                riDataBo.Log.Computation.Errors(errors);
                SetComputationErrors(computations, errors, "AnnuityFactorMappingErrorCount", riDataBo, step);

                // if error triggered
                annuityFactor = null;
            }

            Compute(computations, annuityFactor, "AnnuityFactorMappingErrorCount", riDataBo, step, RiDataBo.AnnuityFactorRateTitle, RiDataBo.ValidateKeyAnnuityFactorMapping);

            LogRiDataFile.SwAnnuityFactorMapping.Stop();
        }

        public void ProcessRiDataLookup(RiDataBo riDataBo, int step)
        {
            if (step != RiDataComputationBo.StepPreComputation1)
                return;

            if (riDataBo.RecordType != RiDataBatchBo.RecordTypeAdjustment)
                return;

            LogRiDataFile.SwRiDataLookup.Start();

            // NOTED: THIS IS FOR TESTING PURPOSE AND DO NOT UNCOMMENT IT
            //riData.PolicyNumber = "T21";
            //riData.CedingPlanCode = "T21";
            //riData.RiskPeriodMonth = 5;
            //riData.RiskPeriodYear = 2020;
            //riData.MlreBenefitCode = "DTH";
            //riData.TreatyCode = "AIA-05";
            //riData.RiderNumber = 6;

            riDataBo.Log.Computation.Title("ProcessRiDataLookup");
            riDataBo.Log.Computation.AddLines(FormatRiDataByTypes(RiDataBo.ParamsRiDataLookup(), riDataBo));
            riDataBo.Log.Computation.Add();

            RiDataWarehouseBo bo = null;
            var errors = riDataBo.ValidateRiDataLookup();
            if (riDataBo.RiDataLookupValidate)
            {
                if (errors.IsNullOrEmpty())
                {
                    GlobalProcessRowRiDataConnectionStrategy.SetRiDataId(riDataBo.Id);
                    // Search by params
                    var count = RiDataWarehouseService.CountByLookupParams(riDataBo);
                    if (count > 0)
                    {
                        if (count == 1)
                        {
                            bo = RiDataWarehouseService.FindByLookupParams(riDataBo);
                        }
                        else
                        {
                            errors.Add(riDataBo.FormatRiDataLookupError(MessageBag.MultipleMappingRecordsFound));
                        }
                    }
                    else
                    {
                        // Search by params (adjustment record with new treaty code where original record having old treaty code)
                        var oldTreatyCode = TreatyOldCodeService.GetByTreatyCode(riDataBo.TreatyCode);
                        if (!string.IsNullOrEmpty(oldTreatyCode))
                        {
                            RiDataBo rdBo = new RiDataBo();
                            rdBo.TreatyCode = oldTreatyCode;
                            rdBo.PolicyNumber = riDataBo.PolicyNumber;
                            rdBo.CedingPlanCode = riDataBo.CedingPlanCode;
                            rdBo.MlreBenefitCode = riDataBo.MlreBenefitCode;
                            rdBo.InsuredName = riDataBo.InsuredName;
                            rdBo.CedingBenefitTypeCode = riDataBo.CedingBenefitTypeCode;
                            rdBo.CedingBenefitRiskCode = riDataBo.CedingBenefitRiskCode;
                            rdBo.CedingPlanCode2 = riDataBo.CedingPlanCode2;
                            rdBo.CessionCode = riDataBo.CessionCode;
                            rdBo.RiskPeriodMonth = riDataBo.RiskPeriodMonth;
                            rdBo.RiskPeriodYear = riDataBo.RiskPeriodYear;
                            rdBo.RiderNumber = riDataBo.RiderNumber;

                            count = RiDataWarehouseService.CountByLookupParams(rdBo);
                            if (count == 1)
                            {
                                bo = RiDataWarehouseService.FindByLookupParams(rdBo);
                            }
                            else
                            {
                                errors.Add(riDataBo.FormatRiDataLookupError(MessageBag.MultipleMappingRecordsFound));
                            }
                        }
                        else
                        {
                            errors.Add(riDataBo.FormatRiDataLookupError(MessageBag.NoRecordMatchParams));
                        }
                    }
                }
            }

            int? riDataWarehouseId = null;
            if (bo != null)
                riDataWarehouseId = bo.Id;

            string property = "OriginalEntryId";
            if (errors.IsNullOrEmpty())
            {
                if (riDataWarehouseId.HasValue)
                {
                    riDataBo.Log.Computation.Found();
                    riDataBo.Log.Computation.Parameter("PolicyNumber", bo.PolicyNumber);
                    riDataBo.Log.Computation.Parameter("CedingPlanCode", bo.CedingPlanCode);
                    riDataBo.Log.Computation.Parameter("RiskPeriodMonth", bo.RiskPeriodMonth);
                    riDataBo.Log.Computation.Parameter("RiskPeriodYear", bo.RiskPeriodYear);
                    riDataBo.Log.Computation.Parameter("MLReBenefitCode", bo.MlreBenefitCode);
                    riDataBo.Log.Computation.Parameter("TreatyCode", bo.TreatyCode);
                    riDataBo.Log.Computation.Parameter("RiderNumber", bo.RiderNumber);
                    riDataBo.Log.Computation.Add();
                    riDataBo.Log.Computation.MappedValue("RiDataWarehouseId", riDataWarehouseId);
                }
                else
                {
                    riDataBo.Log.Computation.NotFound();
                }
            }
            else
            {
                riDataBo.SetError(property, errors);
                riDataBo.Log.Computation.Errors(errors);

                riDataBo.SetComputationValidates(step, RiDataBo.ValidateKeyRiDataLookup);
                LogRiDataFile.PreComputation1ErrorCount++;
                LogRiDataFile.RiDataLookupErrorCount++;

                // if error triggered
                riDataWarehouseId = null;
            }

            if (riDataWarehouseId.HasValue)
            {
                riDataBo.Log.Computation.Property(property);
                riDataBo.Log.Computation.Before(riDataBo.GetPropertyValue(property));
                riDataBo.SetPropertyValue(property, riDataWarehouseId.Value);
                riDataBo.Log.Computation.After(riDataBo.GetPropertyValue(property));

                riDataBo.OriginalEntryBo = bo;
                riDataBo.OriginalEntryQuarter = bo?.Quarter;
            }

            LogRiDataFile.SwRiDataLookup.Stop();
        }

        public void ProcessRiskDate(
            IList<RiDataComputationBo> riskDateOption1StartDate,
            IList<RiDataComputationBo> riskDateOption1EndDate,
            IList<RiDataComputationBo> riskDateOption2StartDate,
            IList<RiDataComputationBo> riskDateOption2EndDate,
            RiDataBo riDataBo,
            int step
        )
        {
            riDataBo.Log.Computation.Title("ProcessRiskDate");
            if (riskDateOption1StartDate.IsNullOrEmpty() &&
                riskDateOption1EndDate.IsNullOrEmpty() &&
                riskDateOption2StartDate.IsNullOrEmpty() &&
                riskDateOption2EndDate.IsNullOrEmpty()
            )
            {
                riDataBo.Log.Computation.Failed(MessageBag.NoComputationConfigured);
                return;
            }

            riskDateOption1StartDate = ValidateConditions(riskDateOption1StartDate, riDataBo);
            riskDateOption1EndDate = ValidateConditions(riskDateOption1EndDate, riDataBo);
            riskDateOption2StartDate = ValidateConditions(riskDateOption2StartDate, riDataBo);
            riskDateOption2EndDate = ValidateConditions(riskDateOption2EndDate, riDataBo);
            if (riskDateOption1StartDate.IsNullOrEmpty() &&
                riskDateOption1EndDate.IsNullOrEmpty() &&
                riskDateOption2StartDate.IsNullOrEmpty() &&
                riskDateOption2EndDate.IsNullOrEmpty()
            )
            {
                riDataBo.Log.Computation.Failed(MessageBag.NoComputationCondition);
                return;
            }

            LogRiDataFile.SwRiskDate.Start();

            // NOTED: THIS IS FOR TESTING PURPOSE AND DO NOT UNCOMMENT IT
            //riData.ReinsEffDatePol = new DateTime(2013, 3, 15);
            //riData.RiskPeriodMonth = 3;
            //riData.RiskPeriodYear = 2020;
            //riData.PremiumFrequencyCode = "A";

            riDataBo.Log.Computation.Parameters();
            riDataBo.Log.Computation.AddLines(FormatRiDataByTypes(RiDataBo.ParamsRiskDate(), riDataBo));
            riDataBo.Log.Computation.Add();

            DateTime? riskStartDate1 = null;
            DateTime? riskEndDate1 = null;
            DateTime? riskStartDate2 = null;
            DateTime? riskEndDate2 = null;

            GlobalProcessRowRiDataConnectionStrategy.SetRiDataId(riDataBo.Id);
            var errors = riDataBo.ValidateRiskDate();
            if (riDataBo.RiskDateValidate)
            {
                var types = new List<int>
                {
                    StandardOutputBo.TypePremiumFrequencyCode,
                };
                var validateErrors = RiDataService.ValidateDropDownCodes(RiDataBo.RiskDateTitle, types, riDataBo);
                errors.AddRange(validateErrors);
                if (errors.IsNullOrEmpty())
                {
                    riDataBo.Log.Computation.Detail("ReinsEffDatePol", riDataBo.ReinsEffDatePol);
                    riDataBo.Log.Computation.Detail("RiskPeriodMonth", riDataBo.RiskPeriodMonth);
                    riDataBo.Log.Computation.Detail("RiskPeriodYear", riDataBo.RiskPeriodYear);
                    riDataBo.Log.Computation.Detail("PremiumFrequencyCode", riDataBo.PremiumFrequencyCode);
                    riDataBo.Log.Computation.Add();

                    try
                    {
                        if (!riskDateOption1StartDate.IsNullOrEmpty())
                            riDataBo.GetRiskPeriodStartEndDate(RiDataComputationBo.RiskDateOption1StartDate, out riskStartDate1, out riskEndDate1);
                    }
                    catch (Exception e)
                    {
                        errors.Add(string.Format("RiskDateOption1StartDate error: {0}", e.Message));
                    }

                    try
                    {
                        if (!riskDateOption1EndDate.IsNullOrEmpty())
                            riDataBo.GetRiskPeriodStartEndDate(RiDataComputationBo.RiskDateOption1EndDate, out riskStartDate1, out riskEndDate1);
                    }
                    catch (Exception e)
                    {
                        errors.Add(string.Format("RiskDateOption1EndDate error: {0}", e.Message));
                    }

                    try
                    {
                        if (!riskDateOption2StartDate.IsNullOrEmpty())
                            riDataBo.GetRiskPeriodStartEndDate(RiDataComputationBo.RiskDateOption2StartDate, out riskStartDate2, out riskEndDate2);
                    }
                    catch (Exception e)
                    {
                        errors.Add(string.Format("RiskDateOption2StartDate error: {0}", e.Message));
                    }

                    try
                    {
                        if (!riskDateOption2EndDate.IsNullOrEmpty())
                            riDataBo.GetRiskPeriodStartEndDate(RiDataComputationBo.RiskDateOption2EndDate, out riskStartDate2, out riskEndDate2);
                    }
                    catch (Exception e)
                    {
                        errors.Add(string.Format("RiskDateOption2EndDate error: {0}", e.Message));
                    }
                }
            }

            if (errors.IsNullOrEmpty())
            {
                if (!riskDateOption1StartDate.IsNullOrEmpty() && riskStartDate1.HasValue)
                    Compute(riskDateOption1StartDate, riskStartDate1, "RiskDateOption1StartDateErrorCount", riDataBo, step, RiDataBo.RiskDateTitle, RiDataBo.ValidateKeyRiskDate);
                if (!riskDateOption1EndDate.IsNullOrEmpty() && riskEndDate1.HasValue)
                    Compute(riskDateOption1EndDate, riskEndDate1, "RiskDateOption1EndDateErrorCount", riDataBo, step, RiDataBo.RiskDateTitle, RiDataBo.ValidateKeyRiskDate);
                if (!riskDateOption2StartDate.IsNullOrEmpty() && riskStartDate2.HasValue)
                    Compute(riskDateOption2StartDate, riskStartDate2, "RiskDateOption2StartDateErrorCount", riDataBo, step, RiDataBo.RiskDateTitle, RiDataBo.ValidateKeyRiskDate);
                if (!riskDateOption2EndDate.IsNullOrEmpty() && riskEndDate2.HasValue)
                    Compute(riskDateOption2EndDate, riskEndDate2, "RiskDateOption2EndDateErrorCount", riDataBo, step, RiDataBo.RiskDateTitle, RiDataBo.ValidateKeyRiskDate);
            }
            else
            {
                riDataBo.SetComputationValidates(step, RiDataBo.ValidateKeyRiskDate);
                SetComputationErrors(riskDateOption1StartDate, errors, new List<string> { }, "RiskDateOption1StartDateErrorCount", riDataBo, step);
                SetComputationErrors(riskDateOption1EndDate, errors, new List<string> { }, "RiskDateOption1EndDateErrorCount", riDataBo, step);
                SetComputationErrors(riskDateOption2StartDate, errors, new List<string> { }, "RiskDateOption2StartDateErrorCount", riDataBo, step);
                SetComputationErrors(riskDateOption2EndDate, errors, new List<string> { }, "RiskDateOption2EndDateErrorCount", riDataBo, step);

                riDataBo.Log.Computation.Errors(errors);
            }

            LogRiDataFile.SwRiskDate.Stop();
        }

        public void ComputeRiskDate(RiDataBo riDataBo, DateTime startDate, DateTime endDate)
        {
            string property1 = "RiskPeriodStartDate";
            string property2 = "RiskPeriodEndDate";

            riDataBo.Log.Computation.Title("ComputeRiskDate");

            riDataBo.Log.Computation.Property(property1);
            riDataBo.Log.Computation.Before(riDataBo.GetPropertyValue(property1));
            riDataBo.SetPropertyValue(property1, startDate);
            riDataBo.Log.Computation.After(riDataBo.GetPropertyValue(property1));

            riDataBo.Log.Computation.Property(property2);
            riDataBo.Log.Computation.Before(riDataBo.GetPropertyValue(property2));
            riDataBo.SetPropertyValue(property2, endDate);
            riDataBo.Log.Computation.After(riDataBo.GetPropertyValue(property2));
        }

        public void ComputeFacMapping(IList<RiDataComputationBo> computations, object input, string errorCountName, RiDataBo riDataBo, int step)
        {
            Compute(computations, input, errorCountName, riDataBo, step, RiDataBo.FacMappingTitle, RiDataBo.ValidateKeyFacMapping, enableEmptyValue: true);
        }

        public void ProcessFacMapping(
            IList<RiDataComputationBo> ewarpNumbers,
            IList<RiDataComputationBo> ewarpActionCodes,
            IList<RiDataComputationBo> offerLetterSentDates,
            IList<RiDataComputationBo> sumAssuredOffereds,
            IList<RiDataComputationBo> uwRatingOffereds,
            IList<RiDataComputationBo> flatExtraAmountOffereds,
            IList<RiDataComputationBo> flatExtraDurations,
            RiDataBo riDataBo,
            int step
        )
        {
            riDataBo.Log.Computation.Title("ProcessFacMapping");
            if (
                ewarpNumbers.IsNullOrEmpty() &&
                ewarpActionCodes.IsNullOrEmpty() &&
                offerLetterSentDates.IsNullOrEmpty() &&
                sumAssuredOffereds.IsNullOrEmpty() &&
                uwRatingOffereds.IsNullOrEmpty() &&
                flatExtraAmountOffereds.IsNullOrEmpty() &&
                flatExtraDurations.IsNullOrEmpty()
            )
            {
                riDataBo.Log.Computation.Failed(MessageBag.NoComputationConfigured);
                return;
            }

            ewarpNumbers = ValidateConditions(ewarpNumbers, riDataBo);
            ewarpActionCodes = ValidateConditions(ewarpActionCodes, riDataBo);
            offerLetterSentDates = ValidateConditions(offerLetterSentDates, riDataBo);
            sumAssuredOffereds = ValidateConditions(sumAssuredOffereds, riDataBo);
            uwRatingOffereds = ValidateConditions(uwRatingOffereds, riDataBo);
            flatExtraAmountOffereds = ValidateConditions(flatExtraAmountOffereds, riDataBo);
            flatExtraDurations = ValidateConditions(flatExtraDurations, riDataBo);
            if (
                ewarpNumbers.IsNullOrEmpty() &&
                ewarpActionCodes.IsNullOrEmpty() &&
                offerLetterSentDates.IsNullOrEmpty() &&
                sumAssuredOffereds.IsNullOrEmpty() &&
                uwRatingOffereds.IsNullOrEmpty() &&
                flatExtraAmountOffereds.IsNullOrEmpty() &&
                flatExtraDurations.IsNullOrEmpty()
            )
            {
                riDataBo.Log.Computation.Failed(MessageBag.NoComputationCondition);
                return;
            }

            LogRiDataFile.SwFacMapping.Start();

            // NOTED: THIS IS FOR TESTING PURPOSE AND DO NOT UNCOMMENT IT
            //riData.PolicyNumber = "UCM1010101";
            //riData.InsuredName = "BLA BLA";
            //riData.MlreBenefitCode = "DAT";

            riDataBo.Log.Computation.Parameters();
            riDataBo.Log.Computation.AddLines(FormatRiDataByTypes(RiDataBo.ParamsFacMapping(), riDataBo));
            riDataBo.Log.Computation.Add();

            FacMasterListingDetailBo detailBo = null;
            var ewarpNumberErrors = new List<string> { };
            var ewarpActionCodeErrors = new List<string> { };
            var offerLetterSentDateErrors = new List<string> { };
            var sumAssuredOfferedErrors = new List<string> { };
            var uwRatingOfferedErrors = new List<string> { };
            var flatExtraAmountOfferedErrors = new List<string> { };
            var flatExtraDurationErrors = new List<string> { };
            var errors = riDataBo.ValidateFacMapping();
            if (riDataBo.FacMappingValidate)
            {
                GlobalProcessRowRiDataConnectionStrategy.SetRiDataId(riDataBo.Id);
                if (errors.IsNullOrEmpty())
                {
                    // Search by params
                    var count = FacMasterListingDetailService.CountByParams(riDataBo, true); // groupById
                    if (count > 0)
                    {
                        if (count == 1)
                        {
                            detailBo = ProcessRiDataBatch.CacheService.FindByFacMappingParams(riDataBo, true);
                            // If not found in cache
                            if (detailBo == null)
                            {
                                detailBo = FacMasterListingDetailService.FindByParams(riDataBo, true); // groupById
                                //ProcessRiDataBatch.CacheService.FacMasterListingDetailBosForFacMapping.Add(detailBo);
                                ProcessRiDataBatch.CacheService.AddMappingBo(7, detailBo);
                            }
                        }
                        else
                        {
                            errors.Add(riDataBo.FormatFacMappingError(MessageBag.MultipleMappingRecordsFound));
                            //riDataBo.Log.Add("--Matched Parameters Records");
                            //var detailBos = FacMasterListingDetailService.GetByParams(riDataBo);
                            //foreach (var d in detailBos)
                            //    riDataBo.Log.Add(string.Format(MessageBag.MappingDetailCombination, d.FacMasterListingId, d.Id, d.Combination));
                            //riDataBo.Log.Add(string.Format(MessageBag.TotalMappingDetailCombination, "FacMasterListing", detailBos.Select(d => d.FacMasterListingId).Distinct().Count()));
                            //riDataBo.Log.Add();
                        }
                    }
                    else
                    {
                        errors.Add(riDataBo.FormatFacMappingError(MessageBag.NoRecordMatchParams));
                    }
                }
            }

            //int? ewarpNumber = null;
            //string ewarpActionCode = null;
            //DateTime? offerLetterSentDate = null;
            //double? sumAssuredOffered = null;
            //double? uwRatingOffered = null;
            //double? flatExtraAmountOffered = null;
            //int? flatExtraDuration = null;
            FacMasterListingBo found = null;
            if (detailBo != null)
                if (detailBo.FacMasterListingBo == null)
                    errors.Add(riDataBo.FormatFacMappingError("FacMasterListing", MessageBag.NoRecordFound));
                else
                    found = detailBo.FacMasterListingBo;

            // Remove empty validation
            //if (found != null)
            //{
            //    ewarpNumber = found.EwarpNumber;
            //    if (!ewarpNumber.HasValue && !ewarpNumbers.IsNullOrEmpty())
            //        ewarpNumberErrors.Add(riDataBo.FormatFacMappingError("eWarp Number", "Empty"));

            //    ewarpActionCode = found.EwarpActionCode;
            //    if (string.IsNullOrEmpty(ewarpActionCode) && !ewarpActionCodes.IsNullOrEmpty())
            //        ewarpActionCodeErrors.Add(riDataBo.FormatFacMappingError("eWarp Action Code", "Empty"));

            //    offerLetterSentDate = found.OfferLetterSentDate;
            //    if (!offerLetterSentDate.HasValue && !offerLetterSentDates.IsNullOrEmpty())
            //        offerLetterSentDateErrors.Add(riDataBo.FormatFacMappingError("Offer Letter Sent Date", "Empty"));

            //    sumAssuredOffered = found.SumAssuredOffered;
            //    if (!sumAssuredOffered.HasValue && !sumAssuredOffereds.IsNullOrEmpty())
            //        sumAssuredOfferedErrors.Add(riDataBo.FormatFacMappingError("Sum Assured Offered", "Empty"));

            //    uwRatingOffered = found.UwRatingOffered;
            //    if (!uwRatingOffered.HasValue && !uwRatingOffereds.IsNullOrEmpty())
            //        uwRatingOfferedErrors.Add(riDataBo.FormatFacMappingError("UW Rating Offered", "Empty"));

            //    flatExtraAmountOffered = found.FlatExtraAmountOffered;
            //    if (!flatExtraAmountOffered.HasValue && !flatExtraAmountOffereds.IsNullOrEmpty())
            //        flatExtraAmountOfferedErrors.Add(riDataBo.FormatFacMappingError("Flat Extra Amount Offered", "Empty"));

            //    flatExtraDuration = found.FlatExtraDuration;
            //    if (!flatExtraDuration.HasValue && !flatExtraDurations.IsNullOrEmpty())
            //        flatExtraDurationErrors.Add(riDataBo.FormatFacMappingError("Flat Extra Duration", "Empty"));
            //}

            if (
                errors.IsNullOrEmpty() &&
                ewarpNumberErrors.IsNullOrEmpty() &&
                ewarpActionCodeErrors.IsNullOrEmpty() &&
                offerLetterSentDateErrors.IsNullOrEmpty() &&
                sumAssuredOfferedErrors.IsNullOrEmpty() &&
                uwRatingOfferedErrors.IsNullOrEmpty() &&
                flatExtraAmountOfferedErrors.IsNullOrEmpty() &&
                flatExtraDurationErrors.IsNullOrEmpty()
            )
            {
                if (found != null)
                {
                    riDataBo.Log.Computation.Found();
                    riDataBo.Log.Computation.Parameter("InsuredName", found.InsuredName);
                    riDataBo.Log.Computation.Parameter("PolicyNumber", found.PolicyNumber);
                    riDataBo.Log.Computation.Parameter("MLReBenefitCode", found.BenefitCode);
                    riDataBo.Log.Computation.Add();
                    riDataBo.Log.Computation.MappedValue("EwarpNumber", found.EwarpNumber);
                    riDataBo.Log.Computation.MappedValue("EwarpActionCode", found.EwarpActionCode);
                    riDataBo.Log.Computation.MappedValue("OfferLetterSentDate", found.OfferLetterSentDate);
                    riDataBo.Log.Computation.MappedValue("SumAssuredOffered", found.SumAssuredOffered);
                    riDataBo.Log.Computation.MappedValue("UwRatingOffered", found.UwRatingOffered);
                    riDataBo.Log.Computation.MappedValue("FlatExtraAmountOffered", found.FlatExtraAmountOffered);
                    riDataBo.Log.Computation.MappedValue("FlatExtraDuration", found.FlatExtraDuration);

                    ComputeFacMapping(ewarpNumbers, found.EwarpNumber, "FacEwarpNumberErrorCount", riDataBo, step);
                    ComputeFacMapping(ewarpActionCodes, found.EwarpActionCode, "FacEwarpActionCodeErrorCount", riDataBo, step);
                    ComputeFacMapping(offerLetterSentDates, found.OfferLetterSentDate, "FacOfferLetterSentDateErrorCount", riDataBo, step);
                    ComputeFacMapping(sumAssuredOffereds, found.SumAssuredOffered, "FacSumAssuredOfferedErrorCount", riDataBo, step);
                    ComputeFacMapping(uwRatingOffereds, found.UwRatingOffered, "FacUwRatingOfferedErrorCount", riDataBo, step);
                    ComputeFacMapping(flatExtraAmountOffereds, found.FlatExtraAmountOffered, "FacFlatExtraAmountOfferedErrorCount", riDataBo, step);
                    ComputeFacMapping(flatExtraDurations, found.FlatExtraDuration, "FacFlatExtraDurationErrorCount", riDataBo, step);
                }
                else
                {
                    riDataBo.Log.Computation.NotFound();
                }
            }
            else
            {
                riDataBo.SetComputationValidates(step, RiDataBo.ValidateKeyFacMapping);
                SetComputationErrors(ewarpNumbers, errors, ewarpNumberErrors, "FacEwarpNumberErrorCount", riDataBo, step);
                SetComputationErrors(ewarpActionCodes, errors, ewarpActionCodeErrors, "FacEwarpActionCodeErrorCount", riDataBo, step);
                SetComputationErrors(offerLetterSentDates, errors, offerLetterSentDateErrors, "FacOfferLetterSentDateErrorCount", riDataBo, step);
                SetComputationErrors(sumAssuredOffereds, errors, sumAssuredOfferedErrors, "FacSumAssuredOfferedErrorCount", riDataBo, step);
                SetComputationErrors(uwRatingOffereds, errors, uwRatingOfferedErrors, "FacUwRatingOfferedErrorCount", riDataBo, step);
                SetComputationErrors(flatExtraAmountOffereds, errors, flatExtraAmountOfferedErrors, "FacFlatExtraAmountOfferedErrorCount", riDataBo, step);
                SetComputationErrors(flatExtraDurations, errors, flatExtraDurationErrors, "FacFlatExtraDurationErrorCount", riDataBo, step);

                errors.AddRange(ewarpNumberErrors);
                errors.AddRange(ewarpActionCodeErrors);
                errors.AddRange(offerLetterSentDateErrors);
                errors.AddRange(sumAssuredOfferedErrors);
                errors.AddRange(uwRatingOfferedErrors);
                errors.AddRange(flatExtraAmountOfferedErrors);
                errors.AddRange(flatExtraDurationErrors);

                riDataBo.Log.Computation.Errors(errors);

                // if error triggered
                //ewarpNumber = null;
                //ewarpActionCode = null;
                //offerLetterSentDate = null;
                //sumAssuredOffered = null;
                //uwRatingOffered = null;
                //flatExtraAmountOffered = null;
                //flatExtraDuration = null;
            }

            // Move to above if function as able to map empty value
            //ComputeFacMapping(ewarpNumbers, ewarpNumber, "FacEwarpNumberErrorCount", riDataBo, step);
            //ComputeFacMapping(ewarpActionCodes, ewarpActionCode, "FacEwarpActionCodeErrorCount", riDataBo, step);
            //ComputeFacMapping(offerLetterSentDates, offerLetterSentDate, "FacOfferLetterSentDateErrorCount", riDataBo, step);
            //ComputeFacMapping(sumAssuredOffereds, sumAssuredOffered, "FacSumAssuredOfferedErrorCount", riDataBo, step);
            //ComputeFacMapping(uwRatingOffereds, uwRatingOffered, "FacUwRatingOfferedErrorCount", riDataBo, step);
            //ComputeFacMapping(flatExtraAmountOffereds, flatExtraAmountOffered, "FacFlatExtraAmountOfferedErrorCount", riDataBo, step);
            //ComputeFacMapping(flatExtraDurations, flatExtraDuration, "FacFlatExtraDurationErrorCount", riDataBo, step);

            LogRiDataFile.SwFacMapping.Stop();
        }

        public void ProcessRiDataPreValidation(int step = RiDataPreValidationBo.StepPreValidation)
        {
            List<RiDataPreValidationBo> validations = null;
            if (!ProcessRiDataBatch.RiDataPreValidationBos.IsNullOrEmpty())
                validations = ProcessRiDataBatch.RiDataPreValidationBos.Where(q => q.Step == step).ToList();

            if (validations.IsNullOrEmpty() && step == RiDataPreValidationBo.StepPreValidation)
            {
                foreach (var riDataBo in RiDataBos)
                {
                    riDataBo.Log.Validation.LineDelimiter();
                    riDataBo.Log.Validation.Failed(MessageBag.NoComputationConfigured);
                    riDataBo.Log.Validation.Detail("Step", RiDataPreValidationBo.GetStepName(step));
                    riDataBo.PreValidationStatus = RiDataBo.PreValidationStatusSuccess;

                    var error = riDataBo.ValidateReportPeriodDate(ProcessRiDataBatch.RiDataBatchBo.Quarter.Trim());
                    if (!string.IsNullOrEmpty(error))
                    {
                        riDataBo.PreValidationStatus = RiDataBo.PreValidationStatusFailed;
                        riDataBo.SetError("PreValidationError", error);
                        LogRiDataFile.PreValidationErrorCount++;
                    }
                }
                return;
            }

            // Moved inside foreach function below
            //if (validations.IsNullOrEmpty() && step == RiDataPreValidationBo.StepPostValidation && ProcessRiDataBatch.RiDataBatchBo.RecordType == RiDataBatchBo.RecordTypeAdjustment)
            //{
            //    foreach (var riDataBo in RiDataBos)
            //    {
            //        riDataBo.Log.Validation.LineDelimiter();
            //        riDataBo.Log.Validation.Failed(MessageBag.NoComputationConfigured);
            //        riDataBo.Log.Validation.Detail("Step", RiDataPreValidationBo.GetStepName(step));
            //        riDataBo.PostValidationStatus = RiDataBo.PostValidationStatusSuccess;
            //    }
            //    return;
            //}

            switch (step)
            {
                case RiDataPreValidationBo.StepPreValidation:
                    LogRiDataFile.SwPreValidation.Start();
                    break;
                case RiDataPreValidationBo.StepPostValidation:
                    LogRiDataFile.SwPostValidation.Start();
                    break;
            }

            foreach (var riDataBo in RiDataBos)
            {
                if (validations.IsNullOrEmpty() && step == RiDataPreValidationBo.StepPostValidation && riDataBo.RecordType == RiDataBatchBo.RecordTypeAdjustment)
                {
                    riDataBo.Log.Validation.LineDelimiter();
                    riDataBo.Log.Validation.Failed(MessageBag.NoComputationConfigured);
                    riDataBo.Log.Validation.Detail("Step", RiDataPreValidationBo.GetStepName(step));
                    riDataBo.PostValidationStatus = RiDataBo.PostValidationStatusSuccess;
                    continue;
                }

                var soe = new StandardOutputEval
                {
                    RiDataBo = riDataBo,
                    Quarter = Quarter,
                };

                if (!validations.IsNullOrEmpty())
                {
                    foreach (var validation in validations)
                    {
                        soe.Condition = validation.Condition;
                        bool condition = soe.EvalCondition();

                        riDataBo.Log.Validation.LineDelimiter();
                        riDataBo.Log.Validation.Detail("Step", RiDataPreValidationBo.GetStepName(step));
                        riDataBo.Log.Validation.Detail("Condition", soe.Condition);
                        riDataBo.Log.Validation.Detail("FormattedCondition", soe.FormattedCondition);
                        riDataBo.Log.Validation.Detail("#Item", validation.SortIndex);
                        riDataBo.Log.Validation.Detail("#Description", validation.Description);

                        if (condition)
                        {
                            // if condition true that's means failed
                            riDataBo.Log.Validation.Add();
                            riDataBo.Log.Validation.Add("**STOPPED**");

                            switch (step)
                            {
                                case RiDataPreValidationBo.StepPreValidation:
                                    riDataBo.PreValidationStatus = RiDataBo.PreValidationStatusFailed;
                                    riDataBo.SetError("PreValidationError", string.Format("#{0} {1}", validation.SortIndex, validation.Description));
                                    LogRiDataFile.PreValidationErrorCount++;
                                    break;
                                case RiDataPreValidationBo.StepPostValidation:
                                    riDataBo.PostValidationStatus = RiDataBo.PostValidationStatusFailed;
                                    riDataBo.SetError("PostValidationError", string.Format("#{0} {1}", validation.SortIndex, validation.Description));
                                    LogRiDataFile.PostValidationErrorCount++;
                                    break;
                            }
                            break;
                        }
                    }
                }

                if (step == RiDataPreValidationBo.StepPreValidation)
                {
                    var error = riDataBo.ValidateReportPeriodDate(ProcessRiDataBatch.RiDataBatchBo.Quarter.Trim());
                    if (!string.IsNullOrEmpty(error))
                    {
                        riDataBo.PreValidationStatus = RiDataBo.PreValidationStatusFailed;
                        riDataBo.SetError("PreValidationError", error);
                        LogRiDataFile.PreValidationErrorCount++;
                    }
                }

                // Moved duplicate validation to post-validation
                if (riDataBo.RecordType == RiDataBatchBo.RecordTypeNew && step == RiDataPreValidationBo.StepPostValidation)
                {
                    if (RiDataService.IsDuplicate(riDataBo))
                    {
                        riDataBo.PostValidationStatus = RiDataBo.PostValidationStatusFailed;
                        riDataBo.SetError("PostValidationError", "Duplicate RI Data record found!");
                        LogRiDataFile.PostValidationErrorCount++;
                    }
                }

                // Moved Original RI Data Warehouse lookup to post-validation
                if (step == RiDataPreValidationBo.StepPostValidation)
                {
                    var lookUpRiDataWarehouseId = RiDataWarehouseService.LookUpRiDataWarehouseIdForPostValidation(riDataBo);
                    if (!lookUpRiDataWarehouseId.HasValue || lookUpRiDataWarehouseId == 0)
                    {
                        // Search by params (adjustment record with new treaty code where original record having old treaty code)
                        var oldTreatyCode = TreatyOldCodeService.GetByTreatyCode(riDataBo.TreatyCode);
                        if (!string.IsNullOrEmpty(oldTreatyCode))
                        {
                            RiDataBo rdBo = new RiDataBo();
                            rdBo.TreatyCode = oldTreatyCode;
                            rdBo.PolicyNumber = riDataBo.PolicyNumber;
                            rdBo.CedingPlanCode = riDataBo.CedingPlanCode;
                            rdBo.MlreBenefitCode = riDataBo.MlreBenefitCode;
                            rdBo.InsuredName = riDataBo.InsuredName;
                            rdBo.CedingBenefitTypeCode = riDataBo.CedingBenefitTypeCode;
                            rdBo.CedingBenefitRiskCode = riDataBo.CedingBenefitRiskCode;
                            rdBo.CedingPlanCode2 = riDataBo.CedingPlanCode2;
                            rdBo.CessionCode = riDataBo.CessionCode;
                            rdBo.RiskPeriodMonth = riDataBo.RiskPeriodMonth;
                            rdBo.RiskPeriodYear = riDataBo.RiskPeriodYear;
                            rdBo.RiderNumber = riDataBo.RiderNumber;

                            lookUpRiDataWarehouseId = RiDataWarehouseService.LookUpRiDataWarehouseIdForPostValidation(rdBo);
                        }
                    }

                    if (!lookUpRiDataWarehouseId.HasValue || lookUpRiDataWarehouseId == 0)
                    {
                        if (riDataBo.RecordType == RiDataBo.RecordTypeAdjustment && riDataBo.TransactionTypeCode == "AL")
                        {
                            riDataBo.PostValidationStatus = RiDataBo.PostValidationStatusFailed;
                            riDataBo.SetError("PostValidationError", "Record not found RI Data Warehouse for RI Data with Record Type: Adjustment and TransactionTypeCode: AL ");
                            LogRiDataFile.PostValidationErrorCount++;
                        }
                        else if (riDataBo.RecordType == RiDataBo.RecordTypeAdjustment)
                        {
                            riDataBo.PostValidationStatus = RiDataBo.PostValidationStatusFailed;
                            riDataBo.SetError("PostValidationError", "Record not found in RI Data Warehouse for RI Data with Record Type: Adjustment");
                            LogRiDataFile.PostValidationErrorCount++;
                        }
                        else if (riDataBo.TransactionTypeCode == "AL")
                        {
                            riDataBo.PostValidationStatus = RiDataBo.PostValidationStatusFailed;
                            riDataBo.SetError("PostValidationError", "Record not found RI Data Warehouse for RI Data with TransactionTypeCode: AL");
                            LogRiDataFile.PostValidationErrorCount++;
                        }
                    }
                }

                // If looping all validations done means success
                switch (step)
                {
                    case RiDataPreValidationBo.StepPreValidation:
                        if (riDataBo.PreValidationStatus != RiDataBo.PreValidationStatusFailed)
                            riDataBo.PreValidationStatus = RiDataBo.PreValidationStatusSuccess;
                        break;
                    case RiDataPreValidationBo.StepPostValidation:
                        if (riDataBo.PostValidationStatus != RiDataBo.PostValidationStatusFailed)
                            riDataBo.PostValidationStatus = RiDataBo.PostValidationStatusSuccess;
                        break;
                }
            }

            switch (step)
            {
                case RiDataPreValidationBo.StepPreValidation:
                    LogRiDataFile.SwPreValidation.Stop();
                    break;
                case RiDataPreValidationBo.StepPostValidation:
                    LogRiDataFile.SwPostValidation.Stop();
                    break;
            }
        }

        public void Save()
        {
            if (ProcessRiDataBatch.Test)
                return;
            if (RiDataBos.IsNullOrEmpty())
                return;

            SanctionVerificationChecking = new SanctionVerificationChecking();
            foreach (var riDataBo in RiDataBos)
            {
                GlobalProcessRowRiDataConnectionStrategy.SetRiDataId(riDataBo.Id);
                var riData = riDataBo;
                //riData.RecordType = ProcessRiDataBatch.RiDataBatchBo.RecordType;
                riData.RiDataBatchId = ProcessRiDataBatch.RiDataFileBo.RiDataBatchId;
                riData.RiDataFileId = ProcessRiDataBatch.RiDataFileBo.Id;
                riData.CreatedById = ProcessRiDataBatch.RiDataFileBo.CreatedById;
                riData.UpdatedById = ProcessRiDataBatch.RiDataFileBo.UpdatedById;

                try
                {
                    RiDataService.Create(ref riData);
                }
                catch (RetryLimitExceededException dex)
                {
                    PrintError("Error retrying Save() for row: " + Row.RowIndex + " " + dex.Message + dex.StackTrace);
                }
                catch (Exception e)
                {
                    PrintError("Error Data Save() for row: " + Row.RowIndex + " " + e.Message + e.StackTrace);
                }

                try
                {
                    ProcessSanctionVerificationChecking(riData);
                }
                catch (RetryLimitExceededException dex)
                {
                    PrintError("Error retrying ProcessSanctionVerificationChecking for row: " + Row.RowIndex + " " + dex.Message + dex.StackTrace);
                }

                #region manual retry                
                //int retries = 0;
                //while (retries <= ProcessRetries)
                //{
                //    try
                //    {
                //        RiDataService.Create(ref riData);

                //        retries = ProcessRetries + 1;
                //    }
                //    catch (Exception e)
                //    {
                //        if (retries >= ProcessRetries)
                //        {
                //            throw e;
                //        }
                //        else
                //        {
                //            if (Row != null && Row.RowIndex != 0)
                //            {
                //                PrintError("Retrying Pre Processing for row: " + Row.RowIndex + " for " + (retries + 1) + " time, buffer for " + BufferWithinRetries + " milliseconds", true);
                //            }
                //            else
                //            {
                //                PrintError("Retrying Pre Processing at row for " + (retries + 1) + " time, buffer for " + BufferWithinRetries + " milliseconds", true);
                //            }
                //            Thread.Sleep(BufferWithinRetries);
                //        }
                //        retries++;
                //    }
                //}
                #endregion
            }
        }

        public void Update()
        {
            if (ProcessRiDataBatch.Test)
                return;
            if (RiDataBos.IsNullOrEmpty())
                return;

            foreach (var riDataBo in RiDataBos)
            {
                var riData = riDataBo;
                riData.UpdatedById = ProcessRiDataBatch.RiDataFileBo.UpdatedById;

                GlobalProcessRowRiDataConnectionStrategy.SetRiDataId(riData.Id);
                try
                {
                    RiDataService.Update(ref riData);
                }
                catch (Exception e)
                {
                    if (e is RetryLimitExceededException dex)
                    {
                        PrintError("Error retrying Update(ref riData) for row: " + Row.RowIndex + " " + dex.Message + dex.StackTrace);
                        throw dex;
                    }
                    else
                    {
                        PrintError("Error Update() for row: " + Row.RowIndex + " " + e.ToString());
                        throw e;
                    }
                }

                #region manual retry
                //int retries = 0;
                //while (retries <= ProcessRetries)
                //{
                //    try
                //    {
                //        RiDataService.Update(ref riData);

                //        retries = ProcessRetries + 1;
                //    }
                //    catch (Exception e)
                //    {
                //        if (retries >= ProcessRetries)
                //        {
                //            throw e;
                //        }
                //        else
                //        {
                //            if (Row != null && Row.RowIndex != 0)
                //            {
                //                PrintError("Retrying Pre Processing for row: " + Row.RowIndex + " for " + (retries + 1) + " time, buffer for " + BufferWithinRetries + " milliseconds", true);
                //            }
                //            else
                //            {
                //                PrintError("Retrying Pre Processing at row for " + (retries + 1) + " time, buffer for " + BufferWithinRetries + " milliseconds", true);
                //            }
                //            Thread.Sleep(BufferWithinRetries);
                //        }
                //        retries++;
                //    }
                //}
                #endregion
            }
        }

        public void ProcessSanctionVerificationChecking(RiDataBo riData)
        {
            var checking = new SanctionVerificationChecking()
            {
                ModuleBo = ProcessRiDataBatch.ModuleBo,
                ObjectId = riData.Id,
                BatchId = ProcessRiDataBatch.RiDataBatchBo.Id,
                Category = riData.FundsAccountingTypeCode,
                InsuredName = riData.InsuredName,
                InsuredDateOfBirth = riData.InsuredDateOfBirth,
                InsuredIcNumber = riData.InsuredNewIcNumber,
                IsRiData = true,
                CedingCompany = ProcessRiDataBatch.RiDataBatchBo.CedantBo.Code,
                TreatyCode = riData.TreatyCode,
                CedingPlanCode = riData.CedingPlanCode,
                PolicyNumber = riData.PolicyNumber,
                SoaQuarter = ProcessRiDataBatch.RiDataBatchBo.Quarter,
                SumReins = riData.Aar,
                LineOfBusiness = riData.LineOfBusiness,
                PolicyCommencementDate = riData.IssueDatePol,
                PolicyStatusCode = riData.PolicyStatusCode,
                RiskCoverageEndDate = riData.RiskPeriodEndDate,
                GrossPremium = riData.GrossPremium
            };

            checking.Check();
            if (checking.IsFound)
            {
                SanctionVerificationChecking.IsFound = true;
                SanctionVerificationChecking.Merge(checking);
            }
        }

        public List<string> FormatRiDataByTypes(List<int> types, RiDataBo riDataBo)
        {
            var lines = new List<string> { };
            foreach (int type in types)
                lines.Add(ProcessRiDataBatch.FormatRiDataByType(type, riDataBo));
            return lines;
        }
    }
}
