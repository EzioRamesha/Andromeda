using BusinessObject;
using BusinessObject.Claims;
using Newtonsoft.Json.Linq;
using Services;
using Services.Claims;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using Row = Shared.ProcessFile.Row;

namespace ConsoleApp.Commands.RawFiles.ClaimData
{
    public class ProcessRowClaimData : Command
    {
        public ProcessClaimDataBatch ProcessClaimDataBatch { get; set; }
        public Row Row { get; set; }
        public List<ClaimDataBo> ClaimDataBos { get; set; }
        public LogClaimDataFile LogClaimDataFile { get; set; }
        public List<string> OutputLines { get; set; }
        public bool AllowAddOutputLine { get; set; } = false;
        public int TitleWidth { get; set; } = 90;
        public int DetailWidth { get; set; } = 30;
        public int CedantId { get; set; }

        public ProcessRowClaimData(ProcessClaimDataBatch batch, MappingClaimData mappingClaimData)
        {
            ProcessClaimDataBatch = batch;
            CedantId = batch.ClaimDataBatchBo.CedantId;
            Row = mappingClaimData.Row;
            ClaimDataBos = mappingClaimData.ClaimDataBos;
            OutputLines = new List<string> { };
        }

        public void Process()
        {
            LogClaimDataFile = new LogClaimDataFile(ProcessClaimDataBatch.ClaimDataFileBo);
            if (ClaimDataBos.IsNullOrEmpty())
                return;

            LogClaimDataFile.ProcessDataRow++;
            LogClaimDataFile.ClaimDataCount += ClaimDataBos.Count;

            foreach (var claimDataBo in ClaimDataBos)
            {
                if (claimDataBo.MappingValidate)
                    claimDataBo.MappingStatus = ClaimDataBo.MappingStatusSuccess;
                else
                    claimDataBo.MappingStatus = ClaimDataBo.MappingStatusFailed;
            }

            ProcessFixedValue();
            ProcessOverrideProperties();
            ProcessRemoveSalutation();
            ProcessPreComputation();
            ProcessPreValidation();

            Save();
        }

        public void ProcessFixedValue()
        {
            LogClaimDataFile.SwFixedValue.Start();

            var fixedValues = ProcessClaimDataBatch.ClaimDataMappingBos.Where(q => q.TransformFormula == ClaimDataMappingBo.TransformFormulaFixedValue).ToList();
            foreach (var mapping in fixedValues)
            {
                if (mapping.Row == 0 || mapping.Row == null)
                {
                    // All rows
                    foreach (var claimDataBo in ClaimDataBos)
                    {
                        bool success = claimDataBo.ProcessClaimData(mapping, mapping.DefaultValue);
                        if (!success)
                        {
                            LogClaimDataFile.MappingErrorCount++;
                        }
                    }
                }
                else if (ClaimDataBos.Count > mapping.Row.Value - 1)
                {
                    bool success = ClaimDataBos[mapping.Row.Value - 1].ProcessClaimData(mapping, mapping.DefaultValue);
                    if (!success)
                    {
                        LogClaimDataFile.MappingErrorCount++;
                    }
                }
            }

            LogClaimDataFile.SwFixedValue.Stop();
        }

        public void ProcessOverrideProperties()
        {
            LogClaimDataFile.SwOverrideProperties.Start();

            AllowAddOutputLine = false;
            AddOutputTitle("ProcessOverrideProperties");

            var obj = JObject.Parse(ProcessClaimDataBatch.ClaimDataFileBo.OverrideProperties);
            foreach (JProperty prop in obj.Properties())
            {
                var type = int.Parse(prop.Name);
                var property = StandardClaimDataOutputBo.GetPropertyNameByType(type);
                var value = prop.Value.ToString();
                var datatype = StandardClaimDataOutputBo.GetDataTypeByType(type);

                foreach (var claimDataBo in ClaimDataBos)
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(value))
                        {
                            AddOutputLine();
                            AddOutputDetail("Property", property);
                            AddOutputDetail("BEFORE", claimDataBo.GetPropertyValue(property));
                            claimDataBo.SetClaimData(datatype, property, value);
                            AddOutputDetail("AFTER", claimDataBo.GetPropertyValue(property));
                        }
                    }
                    catch (Exception e)
                    {
                        claimDataBo.SetError(property, string.Format("Mapping Error: {0}", e.Message));
                    }
                }
            }

            LogClaimDataFile.SwOverrideProperties.Stop();
        }

        public void ProcessRemoveSalutation()
        {
            if (ProcessClaimDataBatch == null)
                return;
            if (ProcessClaimDataBatch.CacheService == null)
                return;
            if (ProcessClaimDataBatch.CacheService.SalutationBos.IsNullOrEmpty())
                return;

            AllowAddOutputLine = false;

            bool removed = false;
            int index = 0;
            foreach (var claimDataBo in ClaimDataBos)
            {
                index++;
                if (string.IsNullOrEmpty(claimDataBo.InsuredName))
                    continue;

                // Development testing purpose
                //claimDataBo.InsuredName = "Ms. " + "TEST NAME";

                foreach (var salutation in ProcessClaimDataBatch.CacheService.SalutationBos)
                {
                    string formattedSalutationWithSpace = salutation.Name.ToLower() + ' ';
                    string formattedSalutationWithDot = salutation.Name.ToLower().Last() == '.' ? salutation.Name.ToLower() : salutation.Name.ToLower() + '.';
                    if (claimDataBo.InsuredName.ToLower().StartsWith(formattedSalutationWithSpace) || claimDataBo.InsuredName.ToLower().StartsWith(formattedSalutationWithDot))
                    {
                        AddOutputPosition("ProcessRemoveSalutation", index);
                        AddOutputDetail("Salutation", salutation.Name);
                        AddOutputDetail("BEFORE", claimDataBo.InsuredName);
                        claimDataBo.InsuredName = claimDataBo.InsuredName.Remove(0, salutation.Name.Length).Trim();

                        if (claimDataBo.InsuredName.StartsWith("."))
                            claimDataBo.InsuredName = claimDataBo.InsuredName.Remove(0, 1).Trim();

                        AddOutputDetail("AFTER", claimDataBo.InsuredName);
                        AddOutputLine();
                        removed = true;
                    }
                }
            }

            if (!removed)
                AddOutputLine("No record was removed salutation");
        }

        public void ProcessPreComputation()
        {
            if (ProcessClaimDataBatch.ClaimDataComputationBos.IsNullOrEmpty())
            {
                foreach (var claimData in ClaimDataBos)
                {
                    claimData.PreComputationStatus = ClaimDataBo.PreComputationStatusSuccess;
                }
                return;
            }

            var formulas = ProcessClaimDataBatch.ClaimDataComputationBos.Where(q => q.Mode == ClaimDataComputationBo.ModeFormula).ToList();
            var tables = ProcessClaimDataBatch.ClaimDataComputationBos.Where(q => q.Mode == ClaimDataComputationBo.ModeTable).ToList();

            // Product Feature Mapping - Treaty
            var treaties = tables.Where(q => q.CalculationFormula == ClaimDataComputationBo.TableTreaty.ToString()).ToList();

            LogClaimDataFile.SwPreComputation.Start();

            int index = 0;
            foreach (var claimDataBo in ClaimDataBos)
            {
                index++;
                ProcessPreComputationFormula(claimDataBo, formulas, index);
                ProcessTreatyMapping(treaties, claimDataBo);
                ProcessRiskQuarter(claimDataBo);

                if (claimDataBo.FormulaValidate && claimDataBo.TreatyCodeMappingValidate)
                    claimDataBo.PreComputationStatus = ClaimDataBo.PreComputationStatusSuccess;
                else
                    claimDataBo.PreComputationStatus = ClaimDataBo.PreComputationStatusFailed;
            }

            LogClaimDataFile.SwPreComputation.Stop();
        }

        public void ProcessPreComputationFormula(ClaimDataBo claimDataBo, IList<ClaimDataComputationBo> formulas, int index)
        {
            AllowAddOutputLine = false;
            AddOutputPosition("ProcessPreComputationFormula", index);

            if (formulas.Count == 0)
                return;

            foreach (var computation in formulas)
            {
                string property = StandardClaimDataOutputBo.GetPropertyNameByType(computation.StandardClaimDataOutputBo.Type);
                var soe = new StandardClaimDataOutputEval
                {
                    Condition = computation.Condition,
                    Formula = computation.CalculationFormula,
                    ClaimDataBo = claimDataBo,
                    Quarter = ProcessClaimDataBatch.ClaimDataBatchBo.Quarter
                };

                bool success = true;
                bool condition = soe.EvalCondition();

                AddOutputPosition("COMPUTATION FORMULA", index);
                AddOutputDetail("PROPERTY", property);
                AddOutputDetail("CONDITION", soe.Condition);
                AddOutputDetail("FORMATTED CONDITION", soe.FormattedCondition);
                AddOutputDetail("CONDITION RESULT", condition ? "SUCCESS" : "FAILED");

                if (condition)
                {
                    var value = soe.EvalFormula();

                    AddOutputDetail("FORMULA", soe.Formula);
                    AddOutputDetail("FORMATTED FORMULA", soe.FormattedFormula);
                    AddOutputDetail("FORMULA RESULT", value ?? Util.Null);

                    if (value == null)
                    {
                        // the computation result should not be null
                        success = false;

                        var msg = string.Format("The computation result is null, FORMATTED FORMULA: {0}", soe.FormattedFormula);
                        claimDataBo.SetError(property, "Formula Error: " + msg);
                        AddOutputDetail("Formula Error", msg);
                    }
                    else
                    {
                        AddOutputLine("");
                        AddOutputLine(" -> SUCCESS");
                        AddOutputDetail("BEFORE Value", claimDataBo.GetPropertyValue(property));

                        try
                        {
                            claimDataBo.SetClaimData(computation.StandardClaimDataOutputBo.DataType, property, value);
                        }
                        catch (Exception e)
                        {
                            success = false;

                            var msg = e.Message;
                            claimDataBo.SetError(property, "Formula Error: " + msg);
                            AddOutputDetail("Formula Error", msg);
                        }

                        AddOutputDetail("AFTER Value", claimDataBo.GetPropertyValue(property));
                    }
                }

                if (!success)
                {
                    AddOutputLine("");
                    AddOutputLine(" -> FAILED");
                    AddOutputDetail("Value", claimDataBo.GetPropertyValue(property));

                    claimDataBo.FormulaValidate = false;

                    LogClaimDataFile.PreComputationErrorCount++;
                    LogClaimDataFile.FormulaErrorCount++;
                }

                AddOutputLine("");
            }
        }

        public void ProcessTreatyMapping(IList<ClaimDataComputationBo> computations, ClaimDataBo claimDataBo)
        {
            if (computations.IsNullOrEmpty())
            {
                return;
            }

            LogClaimDataFile.SwTreatyMapping.Start();

            // NOTED: THIS IS FOR TESTING PURPOSE AND DO NOT UNCOMMENT IT
            //riDataBo.CedingPlanCode = "T21";
            //riDataBo.CedingBenefitTypeCode = "CLR";
            //riDataBo.CedingBenefitRiskCode = "CBRC1";
            //riDataBo.CedingTreatyCode = "CTC1";
            //riDataBo.CampaignCode = "CC1";
            //riDataBo.ReinsEffDatePol = new DateTime(2013, 3, 31);
            //riDataBo.ReinsBasisCode = "AP";

            TreatyBenefitCodeMappingDetailBo detailBo = null;
            var errors = claimDataBo.ValidateTreatyMapping();
            if (errors.IsNullOrEmpty())
            {
                var types = new List<int>
                {
                    StandardClaimDataOutputBo.TypeCedingBenefitTypeCode,
                    StandardClaimDataOutputBo.TypeReinsBasisCode,
                };
                var validateErrors = ClaimDataService.ValidateDropDownCodes(ClaimDataBo.TreatyCodeMappingTitle, types, claimDataBo);
                errors.AddRange(validateErrors);

                if (errors.IsNullOrEmpty())
                {
                    // Search by params
                    var count = TreatyBenefitCodeMappingDetailService.CountByTreatyParamsForClaim(CedantId, claimDataBo, ProcessClaimDataBatch.CacheService, true); // groupById
                    if (count > 0)
                    {
                        detailBo = TreatyBenefitCodeMappingDetailService.FindByTreatyParamsForClaim(CedantId, claimDataBo, ProcessClaimDataBatch.CacheService, true);
                        // Ignore multiple record found checking (Use only first record to map)
                        //if (count == 1)
                        //{
                        //    detailBo = TreatyBenefitCodeMappingDetailService.FindByTreatyParamsForClaim(claimDataBo, ProcessClaimDataBatch.CacheService, true); // groupById
                        //}
                        //else
                        //{
                        //    errors.Add(claimDataBo.FormatTreatyMappingError(MessageBag.MultipleMappingRecordsFound));
                        //    //riDataBo.Log.Add("--Matched Parameters Records");
                        //    //var detailBos = TreatyBenefitCodeMappingDetailService.GetByTreatyParams(riDataBo, ProcessRiDataBatch.CacheService);
                        //    //foreach (var d in detailBos)
                        //    //    riDataBo.Log.Add(string.Format(MessageBag.MappingDetailCombination, d.TreatyBenefitCodeMappingId, d.Id, d.Combination));
                        //    //riDataBo.Log.Add(string.Format(MessageBag.TotalMappingDetailCombination, "TreatyBenefitCodeMapping", detailBos.Select(d => d.TreatyBenefitCodeMappingId).Distinct().Count()));
                        //    //riDataBo.Log.Add();
                        //}
                    }
                    else
                    {
                        errors.Add(claimDataBo.FormatTreatyMappingError(MessageBag.NoRecordMatchParams));
                    }
                }
            }

            string treatyCode = null;
            TreatyBenefitCodeMappingBo found = null;
            if (detailBo != null)
                if (detailBo.TreatyBenefitCodeMappingBo == null)
                    errors.Add(claimDataBo.FormatTreatyMappingError("TreatyBenefitCodeMapping", MessageBag.NoRecordFound));
                else if (detailBo.TreatyBenefitCodeMappingBo.TreatyCodeBo == null)
                    errors.Add(claimDataBo.FormatTreatyMappingError("TreatyCode", MessageBag.NoRecordFound));
                else
                    found = detailBo.TreatyBenefitCodeMappingBo;

            if (found != null)
                treatyCode = found.TreatyCodeBo.Code;

            if (errors.IsNullOrEmpty())
            {
                if (treatyCode != null)
                {
                    foreach (var computation in computations)
                    {
                        var so = computation.GetStandardClaimDataOutputBo();
                        claimDataBo.SetPropertyValue(so.Property, treatyCode);
                    }
                }
            }
            else
            {
                foreach (var computation in computations)
                {
                    claimDataBo.TreatyCodeMappingValidate = false;

                    var so = computation.GetStandardClaimDataOutputBo();
                    claimDataBo.SetError(so.Property, errors);

                    LogClaimDataFile.PreComputationErrorCount++;
                    LogClaimDataFile.TreatyCodeMappingErrorCount++;
                }
            }

            LogClaimDataFile.SwTreatyMapping.Stop();
        }

        public void ProcessRiskQuarter(ClaimDataBo claimDataBo)
        {
            if (claimDataBo.RiskPeriodYear.HasValue && claimDataBo.RiskPeriodMonth.HasValue)
            {
                claimDataBo.RiskQuarter = Util.MonthYearToQuarter(claimDataBo.RiskPeriodYear.Value, claimDataBo.RiskPeriodMonth.Value);
            }
        }

        public void ProcessPreValidation()
        {
            if (ProcessClaimDataBatch.ClaimDataPreValidationBos.IsNullOrEmpty())
            {
                foreach (var claimData in ClaimDataBos)
                {
                    claimData.PreValidationStatus = ClaimDataBo.PreValidationStatusSuccess;
                }
                return;
            }

            AllowAddOutputLine = false;
            LogClaimDataFile.SwPreValidation.Start();
            int index = 0;
            foreach (var claimDataBo in ClaimDataBos)
            {
                index++;
                AddOutputPosition("ProcessPreValidation", index);
                var soe = new StandardClaimDataOutputEval
                {
                    ClaimDataBo = claimDataBo,
                    Quarter = ProcessClaimDataBatch.ClaimDataBatchBo.Quarter,
                };
                foreach (var preValidation in ProcessClaimDataBatch.ClaimDataPreValidationBos)
                {
                    soe.Condition = preValidation.Condition;
                    bool valid = soe.EvalCondition();

                    AddOutputLine("-----");
                    AddOutputDetail("Condition", soe.Condition);
                    AddOutputDetail("FormattedCondition", soe.FormattedCondition);
                    AddOutputDetail("#Item", preValidation.SortIndex);
                    AddOutputDetail("#Description", preValidation.Description);
                    AddOutputLine();

                    if (valid)
                    {
                        // if condition true that's means failed
                        AddOutputLine("**STOPPED**");
                        AddOutputLine();

                        claimDataBo.PreValidationStatus = ClaimDataBo.PreValidationStatusFailed;
                        claimDataBo.SetError("PreValidationError", string.Format("#{0} {1}", preValidation.SortIndex, preValidation.Description));
                        LogClaimDataFile.PreValidationErrorCount++;
                        break;
                    }
                }

                // If looping all pre-validations done means success
                if (claimDataBo.PreValidationStatus != ClaimDataBo.PreValidationStatusFailed)
                    claimDataBo.PreValidationStatus = ClaimDataBo.PreValidationStatusSuccess;
            }

            LogClaimDataFile.SwPreValidation.Stop();
        }

        public void Save()
        {
            if (ProcessClaimDataBatch.Test)
                return;
            if (ClaimDataBos.IsNullOrEmpty())
                return;

            foreach (var claimDataBo in ClaimDataBos)
            {
                var claimData = claimDataBo;
                claimData.ClaimDataBatchId = ProcessClaimDataBatch.ClaimDataFileBo.ClaimDataBatchId;
                claimData.ClaimDataFileId = ProcessClaimDataBatch.ClaimDataFileBo.Id;
                claimData.CedingCompany = ProcessClaimDataBatch.ClaimDataBatchBo.CedantBo.Code;
                claimData.SoaQuarter = ProcessClaimDataBatch.ClaimDataBatchBo.Quarter;
                claimData.ClaimTransactionType = ProcessClaimDataBatch.ClaimDataBatchBo.ClaimTransactionTypePickListDetailBo?.Code;
                claimData.CreatedById = ProcessClaimDataBatch.ClaimDataFileBo.CreatedById;
                claimData.UpdatedById = ProcessClaimDataBatch.ClaimDataFileBo.UpdatedById;
                ClaimDataService.Create(ref claimData);
            }
        }

        public void ResetOutputLines()
        {
            OutputLines = new List<string> { };
        }

        public void AddOutputTitle(string title)
        {
            if (!AllowAddOutputLine)
                return;

            AddOutputLine();
            AddOutputLine("".PadRight(TitleWidth, '-'));
            AddOutputLine(title);
            AddOutputLine();
        }

        public void AddOutputPosition(string title = "", int? index = null)
        {
            if (!AllowAddOutputLine)
                return;

            int? row = Row.RowIndex;
            string output;
            if (index != null)
            {
                if (title.Length > 0)
                    output = string.Format("{0} ROW {1} | #{2}", title, row, index);
                else
                    output = string.Format("ROW {0} | #{1}", row, index);
            }
            else
            {
                if (title.Length > 0)
                    output = string.Format("{0} ROW {1}", title, row);
                else
                    output = string.Format("ROW {0}", row);
            }

            AddOutputLine();
            AddOutputLine("".PadRight(TitleWidth, '-'));
            AddOutputLine(output);
            AddOutputLine();
        }

        public void AddOutputDetail(string name, object value, int? totalWidth = null)
        {
            if (!AllowAddOutputLine)
                return;
            if (totalWidth == null)
                totalWidth = DetailWidth;
            if (value == null)
                value = Util.Null;

            AddOutputLine(Util.FormatDetail(name, value, "", totalWidth.Value));
        }

        public void AddOutputLine(object value = null)
        {
            if (value == null)
                value = "";
            if (!AllowAddOutputLine)
                return;

            OutputLines.Add(value.ToString());
        }
    }
}
