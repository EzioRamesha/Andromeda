using BusinessObject.Identity;
using Shared;

namespace BusinessObject.TreatyPricing
{
    public class TreatyPricingCampaignVersionBo
    {
        public int Id { get; set; }

        public int TreatyPricingCampaignId { get; set; }
        public virtual TreatyPricingCampaignBo TreatyPricingCampaignBo { get; set; }

        public int Version { get; set; }
        public string VersionStr { get; set; }

        public int PersonInChargeId { get; set; }
        public virtual UserBo PersonInChargeBo { get; set; }

        public bool IsPerBenefit { get; set; } = true;
        public string BenefitRemark { get; set; }

        public bool IsPerCedantRetention { get; set; } = true;
        public string CedantRetention { get; set; }

        public bool IsPerMlreShare { get; set; } = true;
        public string MlreShare { get; set; }

        public bool IsPerDistributionChannel { get; set; } = true;
        public string DistributionChannel { get; set; }

        public bool IsPerAgeBasis { get; set; } = true;
        public int? AgeBasisPickListDetailId { get; set; }
        public virtual PickListDetailBo AgeBasisPickListDetailBo { get; set; }

        public bool IsPerMinEntryAge { get; set; } = true;
        public string MinEntryAge { get; set; }

        public bool IsPerMaxEntryAge { get; set; } = true;
        public string MaxEntryAge { get; set; }

        public bool IsPerMaxExpiryAge { get; set; } = true;
        public string MaxExpiryAge { get; set; }

        public bool IsPerMinSumAssured { get; set; } = true;
        public string MinSumAssured { get; set; }

        public bool IsPerMaxSumAssured { get; set; } = true;
        public string MaxSumAssured { get; set; }

        public bool IsPerReinsuranceRate { get; set; } = true;
        public int? ReinsRateTreatyPricingRateTableId { get; set; }
        public virtual TreatyPricingRateTableBo ReinsRateTreatyPricingRateTableBo { get; set; }
        public int? ReinsRateTreatyPricingRateTableVersionId { get; set; }
        public TreatyPricingRateTableVersionBo ReinsRateTreatyPricingRateTableVersionBo { get; set; }
        public string ReinsRateTreatyPricingRateTableSelect { get; set; }
        public string ReinsRateTreatyPricingRateTableSelectStr { get; set; }
        public string ReinsRateNote { get; set; }

        public bool IsPerReinsuranceDiscount { get; set; } = true;
        public int? ReinsDiscountTreatyPricingRateTableId { get; set; }
        public virtual TreatyPricingRateTableBo ReinsDiscountTreatyPricingRateTableBo { get; set; }
        public int? ReinsDiscountTreatyPricingRateTableVersionId { get; set; }
        public TreatyPricingRateTableVersionBo ReinsDiscountTreatyPricingRateTableVersionBo { get; set; }
        public string ReinsDiscountTreatyPricingRateTableSelect { get; set; }
        public string ReinsDiscountTreatyPricingRateTableSelectStr { get; set; }
        public string ReinsDiscountNote { get; set; }

        public bool IsPerProfitComm { get; set; } = true;
        public int? TreatyPricingProfitCommissionId { get; set; }
        public virtual TreatyPricingProfitCommissionBo TreatyPricingProfitCommissionBo { get; set; }
        public int? TreatyPricingProfitCommissionVersionId { get; set; }
        public TreatyPricingProfitCommissionVersionBo TreatyPricingProfitCommissionVersionBo { get; set; }
        public string TreatyPricingProfitCommissionSelect { get; set; }
        public string TreatyPricingProfitCommissionSelectStr { get; set; }
        public string ProfitCommNote { get; set; }

        public string CampaignFundByMlre { get; set; }

        public string ComplimentarySumAssured { get; set; }


        public bool IsPerUnderwritingMethod { get; set; } = true;
        public string UnderwritingMethod { get; set; }

        public bool IsPerUnderwritingQuestion { get; set; } = true;
        public int? TreatyPricingUwQuestionnaireId { get; set; }
        public virtual TreatyPricingUwQuestionnaireBo TreatyPricingUwQuestionnaireBo { get; set; }
        public int? TreatyPricingUwQuestionnaireVersionId { get; set; }
        public TreatyPricingUwQuestionnaireVersionBo TreatyPricingUwQuestionnaireVersionBo { get; set; }
        public string TreatyPricingUwQuestionnaireSelect { get; set; }
        public string TreatyPricingUwQuestionnaireSelectStr { get; set; }
        public string UnderwritingQuestionNote { get; set; }

        public bool IsPerMedicalTable { get; set; } = true;
        public int? TreatyPricingMedicalTableId { get; set; }
        public virtual TreatyPricingMedicalTableBo TreatyPricingMedicalTableBo { get; set; }
        public int? TreatyPricingMedicalTableVersionId { get; set; }
        public TreatyPricingMedicalTableVersionBo TreatyPricingMedicalTableVersionBo { get; set; }
        public string TreatyPricingMedicalTableSelect { get; set; }
        public string TreatyPricingMedicalTableSelectStr { get; set; }
        public string MedicalTableNote { get; set; }

        public bool IsPerFinancialTable { get; set; } = true;
        public int? TreatyPricingFinancialTableId { get; set; }
        public virtual TreatyPricingFinancialTableBo TreatyPricingFinancialTableBo { get; set; }
        public int? TreatyPricingFinancialTableVersionId { get; set; }
        public TreatyPricingFinancialTableVersionBo TreatyPricingFinancialTableVersionBo { get; set; }
        public string TreatyPricingFinancialTableSelect { get; set; }
        public string TreatyPricingFinancialTableSelectStr { get; set; }
        public string FinancialTableNote { get; set; }

        public bool IsPerAggregationNotes { get; set; } = true;
        public string AggregationNotes { get; set; }

        public bool IsPerAdvantageProgram { get; set; } = true;
        public int? TreatyPricingAdvantageProgramId { get; set; }
        public virtual TreatyPricingAdvantageProgramBo TreatyPricingAdvantageProgramBo { get; set; }
        public int? TreatyPricingAdvantageProgramVersionId { get; set; }
        public TreatyPricingAdvantageProgramVersionBo TreatyPricingAdvantageProgramVersionBo { get; set; }
        public string TreatyPricingAdvantageProgramSelect { get; set; }
        public string TreatyPricingAdvantageProgramSelectStr { get; set; }
        public string AdvantageProgramNote { get; set; }


        public bool IsPerWaitingPeriod { get; set; } = true;
        public string WaitingPeriod { get; set; }

        public bool IsPerSurvivalPeriod { get; set; } = true;
        public string SurvivalPeriod { get; set; }

        public string OtherCampaignCriteria { get; set; }

        public string AdditionalRemark { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }


        // Campaign Comparison only
        public string IsPerBenefitStr { get; set; }
        public string IsPerCedantRetentionStr { get; set; }
        public string IsPerMlreShareStr { get; set; }
        public string IsPerDistributionChannelStr { get; set; }
        public string IsPerAgeBasisStr { get; set; }
        public string IsPerMinEntryAgeStr { get; set; }
        public string IsPerMaxEntryAgeStr { get; set; }
        public string IsPerMaxExpiryAgeStr { get; set; }
        public string IsPerMinSumAssuredStr { get; set; }
        public string IsPerMaxSumAssuredStr { get; set; }
        public string IsPerReinsuranceRateStr { get; set; }
        public string IsPerReinsuranceDiscountStr { get; set; }
        public string IsPerProfitCommStr { get; set; }
        public string IsPerUnderwritingMethodStr { get; set; }
        public string IsPerUnderwritingQuestionStr { get; set; }
        public string IsPerMedicalTableStr { get; set; }
        public string IsPerFinancialTableStr { get; set; }
        public string IsPerAggregationNotesStr { get; set; }
        public string IsPerAdvantageProgramStr { get; set; }
        public string IsPerWaitingPeriodStr { get; set; }
        public string IsPerSurvivalPeriodStr { get; set; }


        public TreatyPricingCampaignVersionBo() { }

        public TreatyPricingCampaignVersionBo(TreatyPricingCampaignVersionBo bo)
        {
            TreatyPricingCampaignId = bo.TreatyPricingCampaignId;
            Version = bo.Version;
            PersonInChargeId = bo.PersonInChargeId;
            IsPerBenefit = bo.IsPerBenefit;
            BenefitRemark = bo.BenefitRemark;
            IsPerCedantRetention = bo.IsPerCedantRetention;
            CedantRetention = bo.CedantRetention;
            IsPerMlreShare = bo.IsPerMlreShare;
            MlreShare = bo.MlreShare;
            IsPerDistributionChannel = bo.IsPerDistributionChannel;
            DistributionChannel = bo.DistributionChannel;
            IsPerAgeBasis = bo.IsPerAgeBasis;
            AgeBasisPickListDetailId = bo.AgeBasisPickListDetailId;
            IsPerMinEntryAge = bo.IsPerMinEntryAge;
            MinEntryAge = bo.MinEntryAge;
            IsPerMaxEntryAge = bo.IsPerMaxEntryAge;
            MaxEntryAge = bo.MaxEntryAge;
            IsPerMaxExpiryAge = bo.IsPerMaxExpiryAge;
            MaxExpiryAge = bo.MaxExpiryAge;
            IsPerMinSumAssured = bo.IsPerMinSumAssured;
            MinSumAssured = bo.MinSumAssured;
            IsPerMaxSumAssured = bo.IsPerMaxSumAssured;
            MaxSumAssured = bo.MaxSumAssured;
            IsPerReinsuranceRate = bo.IsPerReinsuranceRate;
            ReinsRateTreatyPricingRateTableId = bo.ReinsRateTreatyPricingRateTableId;
            ReinsRateTreatyPricingRateTableVersionId = bo.ReinsRateTreatyPricingRateTableVersionId;
            ReinsRateTreatyPricingRateTableSelect = bo.ReinsRateTreatyPricingRateTableSelect;
            ReinsRateNote = bo.ReinsRateNote;
            IsPerReinsuranceDiscount = bo.IsPerReinsuranceDiscount;
            ReinsDiscountTreatyPricingRateTableId = bo.ReinsDiscountTreatyPricingRateTableId;
            ReinsDiscountTreatyPricingRateTableVersionId = bo.ReinsDiscountTreatyPricingRateTableVersionId;
            ReinsDiscountTreatyPricingRateTableSelect = bo.ReinsDiscountTreatyPricingRateTableSelect;
            ReinsDiscountNote = bo.ReinsDiscountNote;
            IsPerProfitComm = bo.IsPerProfitComm;
            TreatyPricingProfitCommissionId = bo.TreatyPricingProfitCommissionId;
            TreatyPricingProfitCommissionVersionId = bo.TreatyPricingProfitCommissionVersionId;
            TreatyPricingProfitCommissionSelect = bo.TreatyPricingProfitCommissionSelect;
            ProfitCommNote = bo.ProfitCommNote;
            CampaignFundByMlre = bo.CampaignFundByMlre;
            ComplimentarySumAssured = bo.ComplimentarySumAssured;
            IsPerUnderwritingMethod = bo.IsPerUnderwritingMethod;
            UnderwritingMethod = bo.UnderwritingMethod;
            IsPerUnderwritingQuestion = bo.IsPerUnderwritingQuestion;
            TreatyPricingUwQuestionnaireId = bo.TreatyPricingUwQuestionnaireId;
            TreatyPricingUwQuestionnaireVersionId = bo.TreatyPricingUwQuestionnaireVersionId;
            TreatyPricingUwQuestionnaireSelect = bo.TreatyPricingUwQuestionnaireSelect;
            UnderwritingQuestionNote = bo.UnderwritingQuestionNote;
            IsPerMedicalTable = bo.IsPerMedicalTable;
            TreatyPricingMedicalTableId = bo.TreatyPricingMedicalTableId;
            TreatyPricingMedicalTableVersionId = bo.TreatyPricingMedicalTableVersionId;
            TreatyPricingMedicalTableSelect = bo.TreatyPricingMedicalTableSelect;
            MedicalTableNote = bo.MedicalTableNote;
            IsPerFinancialTable = bo.IsPerFinancialTable;
            TreatyPricingFinancialTableId = bo.TreatyPricingFinancialTableId;
            TreatyPricingFinancialTableVersionId = bo.TreatyPricingFinancialTableVersionId;
            TreatyPricingFinancialTableSelect = bo.TreatyPricingFinancialTableSelect;
            FinancialTableNote = bo.FinancialTableNote;
            IsPerAggregationNotes = bo.IsPerAggregationNotes;
            AggregationNotes = bo.AggregationNotes;
            IsPerAdvantageProgram = bo.IsPerAdvantageProgram;
            TreatyPricingAdvantageProgramId = bo.TreatyPricingAdvantageProgramId;
            TreatyPricingAdvantageProgramVersionId = bo.TreatyPricingAdvantageProgramVersionId;
            TreatyPricingAdvantageProgramSelect = bo.TreatyPricingAdvantageProgramSelect;
            AdvantageProgramNote = bo.AdvantageProgramNote;
            IsPerWaitingPeriod = bo.IsPerWaitingPeriod;
            WaitingPeriod = bo.WaitingPeriod;
            IsPerSurvivalPeriod = bo.IsPerSurvivalPeriod;
            SurvivalPeriod = bo.SurvivalPeriod;
            OtherCampaignCriteria = bo.OtherCampaignCriteria;
            AdditionalRemark = bo.AdditionalRemark;
        }

        public void SetSelectValues()
        {
            SetSelectValue("TreatyPricingMedicalTable");
            SetSelectValue("TreatyPricingFinancialTable");
            SetSelectValue("TreatyPricingUwQuestionnaire");
            SetSelectValue("TreatyPricingAdvantageProgram");
            SetSelectValue("TreatyPricingProfitCommission");
            SetSelectValue("ReinsRateTreatyPricingRateTable");
            SetSelectValue("ReinsDiscountTreatyPricingRateTable");
        }

        public void SetSelectValue(string property)
        {
            string objectProperty = property + "Id";
            string versionProperty = property + "VersionId";
            string selectProperty = property + "Select";

            this.SetPropertyValue(objectProperty, null);
            this.SetPropertyValue(versionProperty, null);

            object value = this.GetPropertyValue(selectProperty);
            if (value == null)
                return;

            string[] values = value.ToString().Split('|');
            if (values.Length == 2)
            {
                int? versionId = Util.GetParseInt(values[0]);
                int? objectId = Util.GetParseInt(values[1]);

                if (versionId.HasValue && objectId.HasValue)
                {
                    this.SetPropertyValue(objectProperty, objectId);
                    this.SetPropertyValue(versionProperty, versionId);
                }
            }
        }
    }
}
