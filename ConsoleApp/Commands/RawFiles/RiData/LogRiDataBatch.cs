using BusinessObject.RiDatas;
using Shared;
using System;
using System.Collections.Generic;

namespace ConsoleApp.Commands.RawFiles.RiData
{
    public class LogRiDataBatch
    {
        public RiDataBatchBo RiDataBatchBo { get; set; }

        public List<LogRiDataFile> ProcessRiDataFiles { get; set; } = new List<LogRiDataFile> { };

        public int ProcessDataRow { get; set; } = 0;

        public int RiDataCount { get; set; } = 0;

        public int MappingErrorCount { get; set; } = 0;

        public int PreComputation1ErrorCount { get; set; } = 0;
        public int PreComputation2ErrorCount { get; set; } = 0;

        public int FormulaErrorCount { get; set; } = 0;

        public int BenefitCodeMappingErrorCount { get; set; } = 0;

        public int TreatyCodeMappingErrorCount { get; set; } = 0;
        public int TreatyNumberMappingErrorCount { get; set; } = 0;
        public int TreatyTypeMappingErrorCount { get; set; } = 0;

        public int FeatureProfitCommMappingErrorCount { get; set; } = 0;
        public int FeatureMaxAgeAtExpiryMappingErrorCount { get; set; } = 0;
        public int FeatureMinIssueAgeMappingErrorCount { get; set; } = 0;
        public int FeatureMaxIssueAgeMappingErrorCount { get; set; } = 0;
        public int FeatureMaxUwRatingMappingErrorCount { get; set; } = 0;
        public int FeatureApLoadingMappingErrorCount { get; set; } = 0;
        public int FeatureMinAarMappingErrorCount { get; set; } = 0;
        public int FeatureMaxAarMappingErrorCount { get; set; } = 0;
        public int FeatureAblAmountMappingErrorCount { get; set; } = 0;
        public int FeatureRetentionShareMappingErrorCount { get; set; } = 0;
        public int FeatureRetentionCapMappingErrorCount { get; set; } = 0;
        public int FeatureRiShareMappingErrorCount { get; set; } = 0;
        public int FeatureRiShareCapMappingErrorCount { get; set; } = 0;
        public int FeatureRiShare2MappingErrorCount { get; set; } = 0;
        public int FeatureRiShareCap2MappingErrorCount { get; set; } = 0;
        public int FeatureServiceFeeMappingErrorCount { get; set; } = 0;
        public int FeatureWakalahFeeMappingErrorCount { get; set; } = 0;
        public int FeatureEffectiveDateMappingErrorCount { get; set; } = 0;

        public int CellBasicRiderMappingErrorCount { get; set; } = 0;
        public int CellNameMappingErrorCount { get; set; } = 0;
        public int CellMfrs17TreatyCodeMappingErrorCount { get; set; } = 0;
        public int CellLoaCodeMappingErrorCount { get; set; } = 0;

        public int RateTableCodeMappingErrorCount { get; set; } = 0;
        public int RateTableRatePerBasisMappingErrorCount { get; set; } = 0;
        public int RateTableRateByPreviousAgeMappingErrorCount { get; set; } = 0;
        public int RateTableRateByCurrentAgeMappingErrorCount { get; set; } = 0;
        public int RateTableRiDiscountMappingErrorCount { get; set; } = 0;
        public int RateTableLargeDiscountMappingErrorCount { get; set; } = 0;
        public int RateTableGroupDiscountMappingErrorCount { get; set; } = 0;

        public int AnnuityFactorMappingErrorCount { get; set; } = 0;
        public int RiDataLookupErrorCount { get; set; } = 0;

        public int RiskDateOption1StartDateErrorCount { get; set; } = 0;
        public int RiskDateOption1EndDateErrorCount { get; set; } = 0;
        public int RiskDateOption2StartDateErrorCount { get; set; } = 0;
        public int RiskDateOption2EndDateErrorCount { get; set; } = 0;

        public int FacEwarpNumberErrorCount { get; set; } = 0;
        public int FacEwarpActionCodeErrorCount { get; set; } = 0;
        public int FacOfferLetterSentDateErrorCount { get; set; } = 0;
        public int FacSumAssuredOfferedErrorCount { get; set; } = 0;
        public int FacUwRatingOfferedErrorCount { get; set; } = 0;
        public int FacFlatExtraAmountOfferedErrorCount { get; set; } = 0;
        public int FacFlatExtraDurationErrorCount { get; set; } = 0;

        public int PreValidationErrorCount { get; set; } = 0;
        public int PostComputationErrorCount { get; set; } = 0;
        public int PostValidationErrorCount { get; set; } = 0;

        public TimeSpan TsRead { get; set; } = new TimeSpan();
        public TimeSpan TsMapping { get; set; } = new TimeSpan();
        public TimeSpan TsProcess { get; set; } = new TimeSpan();

        public TimeSpan TsMappingDetail { get; set; } = new TimeSpan();
        public TimeSpan TsMappingDetail1 { get; set; } = new TimeSpan();
        public TimeSpan TsMappingDetail2 { get; set; } = new TimeSpan();
        public TimeSpan TsMappingDetail3 { get; set; } = new TimeSpan();
        public TimeSpan TsFixedValue { get; set; } = new TimeSpan();
        public TimeSpan TsOverrideProperties { get; set; } = new TimeSpan();
        public TimeSpan TsDataCorrection { get; set; } = new TimeSpan();
        public TimeSpan TsPreComputation1 { get; set; } = new TimeSpan();
        public TimeSpan TsPreComputation2 { get; set; } = new TimeSpan();
        public TimeSpan TsTreatyMapping { get; set; } = new TimeSpan();
        public TimeSpan TsTreatyNumberTreatyTypeMapping { get; set; } = new TimeSpan();
        public TimeSpan TsBenefitMapping { get; set; } = new TimeSpan();
        public TimeSpan TsProductFeatureMapping { get; set; } = new TimeSpan();
        public TimeSpan TsCellMapping { get; set; } = new TimeSpan();
        public TimeSpan TsRateTableMapping { get; set; } = new TimeSpan();
        public TimeSpan TsAnnuityFactorMapping { get; set; } = new TimeSpan();
        public TimeSpan TsRiDataLookup { get; set; } = new TimeSpan();
        public TimeSpan TsRiskDate { get; set; } = new TimeSpan();
        public TimeSpan TsFacMapping { get; set; } = new TimeSpan();
        public TimeSpan TsPreValidation { get; set; } = new TimeSpan();
        public TimeSpan TsPostComputation { get; set; } = new TimeSpan();
        public TimeSpan TsPostValidation { get; set; } = new TimeSpan();
        public TimeSpan TsSave { get; set; } = new TimeSpan();
        public TimeSpan TsAllFiles { get; set; } = new TimeSpan();

        public LogRiDataBatch(RiDataBatchBo batch)
        {
            RiDataBatchBo = batch;
        }

        public static int GetWidth(int w = 45)
        {
            return w;
        }

        public static int GetLineWidth(int w = 80)
        {
            return w;
        }

        public List<string> GetDetails()
        {
            int w = GetWidth();
            int line = GetLineWidth();
            var lines = new List<string>
            {
                "".PadLeft(line, '='),
                "SUMMARY",
                "",
                string.Format("{0}: {1}", "RI Data Batch ID".PadRight(w, ' '), RiDataBatchBo.Id),
                string.Format("{0}: {1}", "Total processed row".PadRight(w, ' '), ProcessDataRow),
                string.Format("{0}: {1}", "Total number of RI Data".PadRight(w, ' '), RiDataCount),
                string.Format("{0}: {1}", "Total Mapping error".PadRight(w, ' '), MappingErrorCount),
                "",
                string.Format("{0}: {1}", "Total Pre-Computation 1 Error".PadRight(w, ' '), PreComputation1ErrorCount),
                string.Format("{0}: {1}", "Total Pre-Computation 2 Error".PadRight(w, ' '), PreComputation2ErrorCount),
                "",
                string.Format("{0}: {1}", "  Total Formula error".PadRight(w, ' '), FormulaErrorCount),
                string.Format("{0}: {1}", "  Total Benefit Code mapping error".PadRight(w, ' '), BenefitCodeMappingErrorCount),
                string.Format("{0}: {1}", "  Total Treaty Code mapping error".PadRight(w, ' '), TreatyCodeMappingErrorCount),
                string.Format("{0}: {1}", "  Total Treaty Number mapping error".PadRight(w, ' '), TreatyNumberMappingErrorCount),
                string.Format("{0}: {1}", "  Total Treaty Type mapping error".PadRight(w, ' '), TreatyTypeMappingErrorCount),
                "",
                string.Format("{0}: {1}", "  Total Feature Profit Commission mapping error".PadRight(w, ' '), FeatureProfitCommMappingErrorCount),
                string.Format("{0}: {1}", "  Total Feature Max Age at Expiry mapping error".PadRight(w, ' '), FeatureMaxAgeAtExpiryMappingErrorCount),
                string.Format("{0}: {1}", "  Total Feature Min Issue Age mapping error".PadRight(w, ' '), FeatureMinIssueAgeMappingErrorCount),
                string.Format("{0}: {1}", "  Total Feature Max Issue Age mapping error".PadRight(w, ' '), FeatureMaxIssueAgeMappingErrorCount),
                string.Format("{0}: {1}", "  Total Feature Max UW Rating mapping error".PadRight(w, ' '), FeatureMaxUwRatingMappingErrorCount),
                string.Format("{0}: {1}", "  Total Feature AP Loading mapping error".PadRight(w, ' '), FeatureApLoadingMappingErrorCount),
                string.Format("{0}: {1}", "  Total Feature Min AAR mapping error".PadRight(w, ' '), FeatureMinAarMappingErrorCount),
                string.Format("{0}: {1}", "  Total Feature Max AAR mapping error".PadRight(w, ' '), FeatureMaxAarMappingErrorCount),
                string.Format("{0}: {1}", "  Total Feature ABL Amount mapping error".PadRight(w, ' '), FeatureAblAmountMappingErrorCount),
                string.Format("{0}: {1}", "  Total Feature Retention Share mapping error".PadRight(w, ' '), FeatureRetentionShareMappingErrorCount),
                string.Format("{0}: {1}", "  Total Feature Retention Cap mapping error".PadRight(w, ' '), FeatureRetentionCapMappingErrorCount),
                string.Format("{0}: {1}", "  Total Feature RI Share mapping error".PadRight(w, ' '), FeatureRiShareMappingErrorCount),
                string.Format("{0}: {1}", "  Total Feature RI Share Cap mapping error".PadRight(w, ' '), FeatureRiShareCapMappingErrorCount),
                string.Format("{0}: {1}", "  Total Feature RI Share 2 mapping error".PadRight(w, ' '), FeatureRiShare2MappingErrorCount),
                string.Format("{0}: {1}", "  Total Feature RI Share Cap 2 mapping error".PadRight(w, ' '), FeatureRiShareCap2MappingErrorCount),
                string.Format("{0}: {1}", "  Total Feature Service Fee mapping error".PadRight(w, ' '), FeatureServiceFeeMappingErrorCount),
                string.Format("{0}: {1}", "  Total Feature Wakalah Fee mapping error".PadRight(w, ' '), FeatureWakalahFeeMappingErrorCount),
                string.Format("{0}: {1}", "  Total Feature Effective Date mapping error".PadRight(w, ' '), FeatureEffectiveDateMappingErrorCount),
                "",
                string.Format("{0}: {1}", "  Total Cell Basic Rider mapping error".PadRight(w, ' '), CellBasicRiderMappingErrorCount),
                string.Format("{0}: {1}", "  Total Cell Name mapping error".PadRight(w, ' '), CellNameMappingErrorCount),
                string.Format("{0}: {1}", "  Total MFRS17 Treaty Code mapping error".PadRight(w, ' '), CellMfrs17TreatyCodeMappingErrorCount),
                string.Format("{0}: {1}", "  Total LOA Code mapping error".PadRight(w, ' '), CellLoaCodeMappingErrorCount),
                "",
                string.Format("{0}: {1}", "  Total Rate Table Code mapping error".PadRight(w, ' '), RateTableCodeMappingErrorCount),
                string.Format("{0}: {1}", "  Total Rate Table Rate Per Basis mapping error".PadRight(w, ' '), RateTableRatePerBasisMappingErrorCount),
                string.Format("{0}: {1}", "  Total Rate Table Rate (Previous Age) mapping error".PadRight(w, ' '), RateTableRateByPreviousAgeMappingErrorCount),
                string.Format("{0}: {1}", "  Total Rate Table Rate (Current Age) mapping error".PadRight(w, ' '), RateTableRateByCurrentAgeMappingErrorCount),
                string.Format("{0}: {1}", "  Total Rate Table RI Discount mapping error".PadRight(w, ' '), RateTableRiDiscountMappingErrorCount),
                string.Format("{0}: {1}", "  Total Rate Table Large Discount mapping error".PadRight(w, ' '), RateTableLargeDiscountMappingErrorCount),
                string.Format("{0}: {1}", "  Total Rate Table Group Discount mapping error".PadRight(w, ' '), RateTableGroupDiscountMappingErrorCount),
                "",
                string.Format("{0}: {1}", "  Total Annuity Factor mapping error".PadRight(w, ' '), AnnuityFactorMappingErrorCount),
                string.Format("{0}: {1}", "  Total RI data lookup error".PadRight(w, ' '), RiDataLookupErrorCount),
                string.Format("{0}: {1}", "  Total Risk Date Option 1 Start Date error".PadRight(w, ' '), RiskDateOption1StartDateErrorCount),
                string.Format("{0}: {1}", "  Total Risk Date Option 1 End Date error".PadRight(w, ' '), RiskDateOption1EndDateErrorCount),
                string.Format("{0}: {1}", "  Total Risk Date Option 2 Start Date error".PadRight(w, ' '), RiskDateOption2StartDateErrorCount),
                string.Format("{0}: {1}", "  Total Risk Date Option 2 End Date error".PadRight(w, ' '), RiskDateOption2EndDateErrorCount),
                string.Format("{0}: {1}", "  Total FAC eWarp Number error".PadRight(w, ' '), FacEwarpNumberErrorCount),
                string.Format("{0}: {1}", "  Total FAC eWarp Action Code error".PadRight(w, ' '), FacEwarpActionCodeErrorCount),
                string.Format("{0}: {1}", "  Total FAC Offer Letter Sent Date error".PadRight(w, ' '), FacOfferLetterSentDateErrorCount),
                string.Format("{0}: {1}", "  Total FAC Sum Assured Offered error".PadRight(w, ' '), FacSumAssuredOfferedErrorCount),
                string.Format("{0}: {1}", "  Total FAC UW Rating Offered error".PadRight(w, ' '), FacUwRatingOfferedErrorCount),
                string.Format("{0}: {1}", "  Total FAC Flat Extra Amount Offered error".PadRight(w, ' '), FacFlatExtraAmountOfferedErrorCount),
                string.Format("{0}: {1}", "  Total FAC Flat Extra Duration error".PadRight(w, ' '), FacFlatExtraDurationErrorCount),
                "",
                string.Format("{0}: {1}", "Total Pre-Validation error".PadRight(w, ' '), PreValidationErrorCount),
                string.Format("{0}: {1}", "Total Post-Computation error".PadRight(w, ' '), PostComputationErrorCount),
                string.Format("{0}: {1}", "Total Post-Validation error".PadRight(w, ' '), PostValidationErrorCount),
            };

            lines.AddRange(GetElapsedTime());

            return lines;
        }

        public List<string> GetElapsedTime()
        {
            int w = GetWidth();
            int line = GetLineWidth();
            return new List<string>
            {
                "Elapsed Time".PadBoth(line, '*'),
                string.Format("{0}: {1}", "Read Lines".PadRight(w, ' '), TsRead.ToString()),
                string.Format("{0}: {1}", "Mapping Lines".PadRight(w, ' '), TsMapping.ToString()),
                string.Format("{0}: {1}", "Process Lines".PadRight(w, ' '), TsProcess.ToString()),
                //string.Format("{0}: {1}", "Mapping Detail".PadRight(w, ' '), TsMappingDetail.ToString()),
                //string.Format("{0}: {1}", "  Mapping Detail 1".PadRight(w-2, ' '), TsMappingDetail1.ToString()),
                //string.Format("{0}: {1}", "  Mapping Detail 2".PadRight(w-2, ' '), TsMappingDetail2.ToString()),
                //string.Format("{0}: {1}", "  Mapping Detail 3".PadRight(w-2, ' '), TsMappingDetail3.ToString()),
                string.Format("{0}: {1}", "Fixed Value".PadRight(w, ' '), TsFixedValue.ToString()),
                string.Format("{0}: {1}", "Override Properties".PadRight(w, ' '), TsOverrideProperties.ToString()),
                string.Format("{0}: {1}", "Data Correction".PadRight(w, ' '), TsDataCorrection.ToString()),
                string.Format("{0}: {1}", "Pre-Computation 1".PadRight(w, ' '), TsPreComputation1.ToString()),
                string.Format("{0}: {1}", "Pre-Computation 2".PadRight(w, ' '), TsPreComputation2.ToString()),
                string.Format("{0}: {1}", "Treaty Code Mapping".PadRight(w, ' '), TsTreatyMapping.ToString()),
                string.Format("{0}: {1}", "Treaty Number Treaty Type Mapping".PadRight(w, ' '), TsTreatyNumberTreatyTypeMapping.ToString()),
                string.Format("{0}: {1}", "Benefit Code Mapping".PadRight(w, ' '), TsBenefitMapping.ToString()),
                string.Format("{0}: {1}", "Product Feature Mapping".PadRight(w, ' '), TsProductFeatureMapping.ToString()),
                string.Format("{0}: {1}", "Cell Mapping".PadRight(w, ' '), TsCellMapping.ToString()),
                string.Format("{0}: {1}", "Rate Table Mapping".PadRight(w, ' '), TsRateTableMapping.ToString()),
                string.Format("{0}: {1}", "Annuity Factor Mapping".PadRight(w, ' '), TsAnnuityFactorMapping.ToString()),
                string.Format("{0}: {1}", "Risk Date".PadRight(w, ' '), TsRiskDate.ToString()),
                string.Format("{0}: {1}", "RI Data Lookup".PadRight(w, ' '), TsRiDataLookup.ToString()),
                string.Format("{0}: {1}", "FAC Mapping".PadRight(w, ' '), TsFacMapping.ToString()),
                string.Format("{0}: {1}", "Pre-Validation".PadRight(w, ' '), TsPreValidation.ToString()),
                string.Format("{0}: {1}", "Post-Computation".PadRight(w, ' '), TsPostComputation.ToString()),
                string.Format("{0}: {1}", "Post-Validation".PadRight(w, ' '), TsPostValidation.ToString()),
                string.Format("{0}: {1}", "Save".PadRight(w, ' '), TsSave.ToString()),
                string.Format("{0}: {1}", "All Files".PadRight(w, ' '), TsAllFiles.ToString()),
            };
        }

        public void Add(LogRiDataFile file)
        {
            ProcessRiDataFiles.Add(file);

            ProcessDataRow += file.ProcessDataRow;
            RiDataCount += file.RiDataCount;
            MappingErrorCount += file.MappingErrorCount;
            PreComputation1ErrorCount += file.PreComputation1ErrorCount;
            PreComputation2ErrorCount += file.PreComputation2ErrorCount;
            FormulaErrorCount += file.FormulaErrorCount;
            BenefitCodeMappingErrorCount += file.BenefitCodeMappingErrorCount;
            TreatyCodeMappingErrorCount += file.TreatyCodeMappingErrorCount;
            TreatyNumberMappingErrorCount += file.TreatyNumberMappingErrorCount;
            TreatyTypeMappingErrorCount += file.TreatyTypeMappingErrorCount;
            FeatureProfitCommMappingErrorCount += file.FeatureProfitCommMappingErrorCount;
            FeatureMaxAgeAtExpiryMappingErrorCount += file.FeatureMaxAgeAtExpiryMappingErrorCount;
            FeatureMinIssueAgeMappingErrorCount += file.FeatureMinIssueAgeMappingErrorCount;
            FeatureMaxIssueAgeMappingErrorCount += file.FeatureMaxIssueAgeMappingErrorCount;
            FeatureMaxUwRatingMappingErrorCount += file.FeatureMaxUwRatingMappingErrorCount;
            FeatureApLoadingMappingErrorCount += file.FeatureApLoadingMappingErrorCount;
            FeatureMinAarMappingErrorCount += file.FeatureMinAarMappingErrorCount;
            FeatureMaxAarMappingErrorCount += file.FeatureMaxAarMappingErrorCount;
            FeatureAblAmountMappingErrorCount += file.FeatureAblAmountMappingErrorCount;
            FeatureRetentionShareMappingErrorCount += file.FeatureRetentionShareMappingErrorCount;
            FeatureRetentionCapMappingErrorCount += file.FeatureRetentionCapMappingErrorCount;
            FeatureRiShareMappingErrorCount += file.FeatureRiShareMappingErrorCount;
            FeatureRiShareCapMappingErrorCount += file.FeatureRiShareCapMappingErrorCount;
            FeatureRiShare2MappingErrorCount += file.FeatureRiShare2MappingErrorCount;
            FeatureRiShareCap2MappingErrorCount += file.FeatureRiShareCap2MappingErrorCount;
            FeatureServiceFeeMappingErrorCount += file.FeatureServiceFeeMappingErrorCount;
            FeatureWakalahFeeMappingErrorCount += file.FeatureWakalahFeeMappingErrorCount;
            FeatureEffectiveDateMappingErrorCount += file.FeatureEffectiveDateMappingErrorCount;
            CellBasicRiderMappingErrorCount += file.CellBasicRiderMappingErrorCount;
            CellNameMappingErrorCount += file.CellNameMappingErrorCount;
            CellMfrs17TreatyCodeMappingErrorCount += file.CellMfrs17TreatyCodeMappingErrorCount;
            CellLoaCodeMappingErrorCount += file.CellLoaCodeMappingErrorCount;
            RateTableCodeMappingErrorCount += file.RateTableCodeMappingErrorCount;
            RateTableRatePerBasisMappingErrorCount += file.RateTableRatePerBasisMappingErrorCount;
            RateTableRateByPreviousAgeMappingErrorCount += file.RateTableRateByPreviousAgeMappingErrorCount;
            RateTableRateByCurrentAgeMappingErrorCount += file.RateTableRateByCurrentAgeMappingErrorCount;
            RateTableRiDiscountMappingErrorCount += file.RateTableRiDiscountMappingErrorCount;
            RateTableLargeDiscountMappingErrorCount += file.RateTableLargeDiscountMappingErrorCount;
            RateTableGroupDiscountMappingErrorCount += file.RateTableGroupDiscountMappingErrorCount;
            AnnuityFactorMappingErrorCount += file.AnnuityFactorMappingErrorCount;
            RiDataLookupErrorCount += file.RiDataLookupErrorCount;
            RiskDateOption1StartDateErrorCount += file.RiskDateOption1StartDateErrorCount;
            RiskDateOption1EndDateErrorCount += file.RiskDateOption1EndDateErrorCount;
            RiskDateOption2StartDateErrorCount += file.RiskDateOption2StartDateErrorCount;
            RiskDateOption2EndDateErrorCount += file.RiskDateOption2EndDateErrorCount;
            FacEwarpNumberErrorCount += file.FacEwarpNumberErrorCount;
            FacEwarpActionCodeErrorCount += file.FacEwarpActionCodeErrorCount;
            FacOfferLetterSentDateErrorCount += file.FacOfferLetterSentDateErrorCount;
            FacSumAssuredOfferedErrorCount += file.FacSumAssuredOfferedErrorCount;
            FacUwRatingOfferedErrorCount += file.FacUwRatingOfferedErrorCount;
            FacFlatExtraAmountOfferedErrorCount += file.FacFlatExtraAmountOfferedErrorCount;
            FacFlatExtraDurationErrorCount += file.FacFlatExtraDurationErrorCount;
            PreValidationErrorCount += file.PreValidationErrorCount;
            PostComputationErrorCount += file.PostComputationErrorCount;
            PostValidationErrorCount += file.PostValidationErrorCount;

            TsRead = file.SwRead.Elapsed.Add(TsRead);
            TsMapping = file.SwMapping.Elapsed.Add(TsMapping);
            TsProcess = file.SwProcess.Elapsed.Add(TsProcess);

            TsMappingDetail = file.SwMappingDetail.Elapsed.Add(TsMappingDetail);
            TsMappingDetail1 = file.SwMappingDetail1.Elapsed.Add(TsMappingDetail1);
            TsMappingDetail2 = file.SwMappingDetail2.Elapsed.Add(TsMappingDetail2);
            TsMappingDetail3 = file.SwMappingDetail3.Elapsed.Add(TsMappingDetail3);
            TsFixedValue = file.SwFixedValue.Elapsed.Add(TsFixedValue);
            TsOverrideProperties = file.SwOverrideProperties.Elapsed.Add(TsOverrideProperties);
            TsDataCorrection = file.SwDataCorrection.Elapsed.Add(TsDataCorrection);
            TsPreComputation1 = file.SwPreComputation1.Elapsed.Add(TsPreComputation1);
            TsPreComputation2 = file.SwPreComputation2.Elapsed.Add(TsPreComputation2);
            TsTreatyMapping = file.SwTreatyMapping.Elapsed.Add(TsTreatyMapping);
            TsTreatyNumberTreatyTypeMapping = file.SwTreatyNumberTreatyTypeMapping.Elapsed.Add(TsTreatyNumberTreatyTypeMapping);
            TsBenefitMapping = file.SwBenefitMapping.Elapsed.Add(TsBenefitMapping);
            TsProductFeatureMapping = file.SwProductFeatureMapping.Elapsed.Add(TsProductFeatureMapping);
            TsCellMapping = file.SwCellMapping.Elapsed.Add(TsCellMapping);
            TsRateTableMapping = file.SwRateTableMapping.Elapsed.Add(TsRateTableMapping);
            TsAnnuityFactorMapping = file.SwAnnuityFactorMapping.Elapsed.Add(TsAnnuityFactorMapping);
            TsRiDataLookup = file.SwRiDataLookup.Elapsed.Add(TsRiDataLookup);
            TsRiskDate = file.SwRiskDate.Elapsed.Add(TsRiskDate);
            TsFacMapping = file.SwFacMapping.Elapsed.Add(TsFacMapping);
            TsPreValidation = file.SwPreValidation.Elapsed.Add(TsPreValidation);
            TsPostComputation = file.SwPostComputation.Elapsed.Add(TsPostComputation);
            TsPostValidation = file.SwPostValidation.Elapsed.Add(TsPostValidation);
            TsSave = file.SwSave.Elapsed.Add(TsSave);
            TsAllFiles = file.SwFile.Elapsed.Add(TsAllFiles);
        }
    }
}
