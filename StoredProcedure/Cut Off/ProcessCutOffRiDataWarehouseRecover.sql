CREATE OR ALTER PROCEDURE [dbo].[ProcessCutOffRiDataWarehouseRecover](@CutOffId INT, @RecoverFrom INT, @Interval INT, @Result VARCHAR(MAX) OUTPUT)

AS

SET NOCOUNT ON;

DECLARE
	@MinId INT,
	@MaxId INT,
	@RiDataWarehouseCount INT,
	@Start INT = @RecoverFrom + 1,
	@StartPlusInterval INT

SELECT
	@MinId = MIN(Id),
	@MaxId = MAX(Id),
	@RiDataWarehouseCount = COUNT(*)
FROM
	RiDataWarehouse

WHILE (@Start <= @MaxId)
BEGIN
	SET @StartPlusInterval = @Start + @Interval

	INSERT INTO RiDataWarehouseHistories (
		[CutOffId],
		[RiDataWarehouseId],
		[EndingPolicyStatus],
		[RecordType],
		[Quarter],
		[TreatyCode],
		[ReinsBasisCode],
		[FundsAccountingTypeCode],
		[PremiumFrequencyCode],
		[ReportPeriodMonth],
		[ReportPeriodYear],
		[RiskPeriodMonth],
		[RiskPeriodYear],
		[TransactionTypeCode],
		[PolicyNumber],
		[IssueDatePol],
		[IssueDateBen],
		[ReinsEffDatePol],
		[ReinsEffDateBen],
		[CedingPlanCode],
		[CedingBenefitTypeCode],
		[CedingBenefitRiskCode],
		[MlreBenefitCode],
		[OriSumAssured],
		[CurrSumAssured],
		[AmountCededB4MlreShare],
		[RetentionAmount],
		[AarOri],
		[Aar],
		[AarSpecial1],
		[AarSpecial2],
		[AarSpecial3],
		[InsuredName],
		[InsuredGenderCode],
		[InsuredTobaccoUse],
		[InsuredDateOfBirth],
		[InsuredOccupationCode],
		[InsuredRegisterNo],
		[InsuredAttainedAge],
		[InsuredNewIcNumber],
		[InsuredOldIcNumber],
		[InsuredName2nd],
		[InsuredGenderCode2nd],
		[InsuredTobaccoUse2nd],
		[InsuredDateOfBirth2nd],
		[InsuredAttainedAge2nd],
		[InsuredNewIcNumber2nd],
		[InsuredOldIcNumber2nd],
		[ReinsuranceIssueAge],
		[ReinsuranceIssueAge2nd],
		[PolicyTerm],
		[PolicyExpiryDate],
		[DurationYear],
		[DurationDay],
		[DurationMonth],
		[PremiumCalType],
		[CedantRiRate],
		[RateTable],
		[AgeRatedUp],
		[DiscountRate],
		[LoadingType],
		[UnderwriterRating],
		[UnderwriterRatingUnit],
		[UnderwriterRatingTerm],
		[UnderwriterRating2],
		[UnderwriterRatingUnit2],
		[UnderwriterRatingTerm2],
		[UnderwriterRating3],
		[UnderwriterRatingUnit3],
		[UnderwriterRatingTerm3],
		[FlatExtraAmount],
		[FlatExtraUnit],
		[FlatExtraTerm],
		[FlatExtraAmount2],
		[FlatExtraTerm2],
		[StandardPremium],
		[SubstandardPremium],
		[FlatExtraPremium],
		[GrossPremium],
		[StandardDiscount],
		[SubstandardDiscount],
		[VitalityDiscount],
		[TotalDiscount],
		[NetPremium],
		[AnnualRiPrem],
		[RiCovPeriod],
		[AdjBeginDate],
		[AdjEndDate],
		[PolicyNumberOld],
		[PolicyStatusCode],
		[PolicyGrossPremium],
		[PolicyStandardPremium],
		[PolicySubstandardPremium],
		[PolicyTermRemain],
		[PolicyAmountDeath],
		[PolicyReserve],
		[PolicyPaymentMethod],
		[PolicyLifeNumber],
		[FundCode],
		[LineOfBusiness],
		[ApLoading],
		[LoanInterestRate],
		[DefermentPeriod],
		[RiderNumber],
		[CampaignCode],
		[Nationality],
		[TerritoryOfIssueCode],
		[CurrencyCode],
		[StaffPlanIndicator],
		[CedingTreatyCode],
		[CedingPlanCodeOld],
		[CedingBasicPlanCode],
		[CedantSar],
		[CedantReinsurerCode],
		[AmountCededB4MlreShare2],
		[CessionCode],
		[CedantRemark],
		[GroupPolicyNumber],
		[GroupPolicyName],
		[NoOfEmployee],
		[PolicyTotalLive],
		[GroupSubsidiaryName],
		[GroupSubsidiaryNo],
		[GroupEmployeeBasicSalary],
		[GroupEmployeeJobType],
		[GroupEmployeeJobCode],
		[GroupEmployeeBasicSalaryRevise],
		[GroupEmployeeBasicSalaryMultiplier],
		[CedingPlanCode2],
		[DependantIndicator],
		[GhsRoomBoard],
		[PolicyAmountSubstandard],
		[Layer1RiShare],
		[Layer1InsuredAttainedAge],
		[Layer1InsuredAttainedAge2nd],
		[Layer1StandardPremium],
		[Layer1SubstandardPremium],
		[Layer1GrossPremium],
		[Layer1StandardDiscount],
		[Layer1SubstandardDiscount],
		[Layer1TotalDiscount],
		[Layer1NetPremium],
		[Layer1GrossPremiumAlt],
		[Layer1TotalDiscountAlt],
		[Layer1NetPremiumAlt],
		[SpecialIndicator1],
		[SpecialIndicator2],
		[SpecialIndicator3],
		[IndicatorJointLife],
		[TaxAmount],
		[GstIndicator],
		[GstGrossPremium],
		[GstTotalDiscount],
		[GstVitality],
		[GstAmount],
		[Mfrs17BasicRider],
		[Mfrs17CellName],
		[Mfrs17TreatyCode],
		[LoaCode],
		[CurrencyRate],
		[NoClaimBonus],
		[SurrenderValue],
		[DatabaseCommision],
		[GrossPremiumAlt],
		[NetPremiumAlt],
		[Layer1FlatExtraPremium],
		[TransactionPremium],
		[OriginalPremium],
		[TransactionDiscount],
		[OriginalDiscount],
		[BrokerageFee],
		[MaxUwRating],
		[RetentionCap],
		[AarCap],
		[RiRate],
		[RiRate2],
		[AnnuityFactor],
		[SumAssuredOffered],
		[UwRatingOffered],
		[FlatExtraAmountOffered],
		[FlatExtraDuration],
		[EffectiveDate],
		[OfferLetterSentDate],
		[RiskPeriodStartDate],
		[RiskPeriodEndDate],
		[Mfrs17AnnualCohort],
		[MaxExpiryAge],
		[MinIssueAge],
		[MaxIssueAge],
		[MinAar],
		[MaxAar],
		[CorridorLimit],
		[Abl],
		[RatePerBasisUnit],
		[RiDiscountRate],
		[LargeSaDiscount],
		[GroupSizeDiscount],
		[EwarpNumber],
		[EwarpActionCode],
		[RetentionShare],
		[AarShare],
		[ProfitComm],
		[TotalDirectRetroAar],
		[TotalDirectRetroGrossPremium],
		[TotalDirectRetroDiscount],
		[TotalDirectRetroNetPremium],
		[TotalDirectRetroNoClaimBonus],
		[TotalDirectRetroDatabaseCommission],
		[TreatyType],
		[LastUpdatedDate],
		[RetroParty1],
		[RetroParty2],
		[RetroParty3],
		[RetroShare1],
		[RetroShare2],
		[RetroShare3],
		[RetroPremiumSpread1],
		[RetroPremiumSpread2],
		[RetroPremiumSpread3],
		[RetroAar1],
		[RetroAar2],
		[RetroAar3],
		[RetroReinsurancePremium1],
		[RetroReinsurancePremium2],
		[RetroReinsurancePremium3],
		[RetroDiscount1],
		[RetroDiscount2],
		[RetroDiscount3],
		[RetroNetPremium1],
		[RetroNetPremium2],
		[RetroNetPremium3],
		[RetroNoClaimBonus1],
		[RetroNoClaimBonus2],
		[RetroNoClaimBonus3],
		[RetroDatabaseCommission1],
		[RetroDatabaseCommission2],
		[RetroDatabaseCommission3],
		[MaxApLoading],
		[MlreInsuredAttainedAgeAtCurrentMonth],
		[MlreInsuredAttainedAgeAtPreviousMonth],
		[InsuredAttainedAgeCheck],
		[MaxExpiryAgeCheck],
		[MlrePolicyIssueAge],
		[PolicyIssueAgeCheck],
		[MinIssueAgeCheck],
		[MaxIssueAgeCheck],
		[MaxUwRatingCheck],
		[ApLoadingCheck],
		[EffectiveDateCheck],
		[MinAarCheck],
		[MaxAarCheck],
		[CorridorLimitCheck],
		[AblCheck],
		[RetentionCheck],
		[AarCheck],
		[MlreStandardPremium],
		[MlreSubstandardPremium],
		[MlreFlatExtraPremium],
		[MlreGrossPremium],
		[MlreStandardDiscount],
		[MlreSubstandardDiscount],
		[MlreLargeSaDiscount],
		[MlreGroupSizeDiscount],
		[MlreVitalityDiscount],
		[MlreTotalDiscount],
		[MlreNetPremium],
		[NetPremiumCheck],
		[ServiceFeePercentage],
		[ServiceFee],
		[MlreBrokerageFee],
		[MlreDatabaseCommission],
		[ValidityDayCheck],
		[SumAssuredOfferedCheck],
		[UwRatingCheck],
		[FlatExtraAmountCheck],
		[FlatExtraDurationCheck],
		[AarShare2],
		[AarCap2],
		[WakalahFeePercentage],
		[TreatyNumber],
		[ConflictType],
		[CreatedAt],
		[UpdatedAt],
		[CreatedById],
		[UpdatedById]
	)
	SELECT
		@CutOffId,
		[Id],
		[EndingPolicyStatus],
		[RecordType],
		[Quarter],
		[TreatyCode],
		[ReinsBasisCode],
		[FundsAccountingTypeCode],
		[PremiumFrequencyCode],
		[ReportPeriodMonth],
		[ReportPeriodYear],
		[RiskPeriodMonth],
		[RiskPeriodYear],
		[TransactionTypeCode],
		[PolicyNumber],
		[IssueDatePol],
		[IssueDateBen],
		[ReinsEffDatePol],
		[ReinsEffDateBen],
		[CedingPlanCode],
		[CedingBenefitTypeCode],
		[CedingBenefitRiskCode],
		[MlreBenefitCode],
		[OriSumAssured],
		[CurrSumAssured],
		[AmountCededB4MlreShare],
		[RetentionAmount],
		[AarOri],
		[Aar],
		[AarSpecial1],
		[AarSpecial2],
		[AarSpecial3],
		[InsuredName],
		[InsuredGenderCode],
		[InsuredTobaccoUse],
		[InsuredDateOfBirth],
		[InsuredOccupationCode],
		[InsuredRegisterNo],
		[InsuredAttainedAge],
		[InsuredNewIcNumber],
		[InsuredOldIcNumber],
		[InsuredName2nd],
		[InsuredGenderCode2nd],
		[InsuredTobaccoUse2nd],
		[InsuredDateOfBirth2nd],
		[InsuredAttainedAge2nd],
		[InsuredNewIcNumber2nd],
		[InsuredOldIcNumber2nd],
		[ReinsuranceIssueAge],
		[ReinsuranceIssueAge2nd],
		[PolicyTerm],
		[PolicyExpiryDate],
		[DurationYear],
		[DurationDay],
		[DurationMonth],
		[PremiumCalType],
		[CedantRiRate],
		[RateTable],
		[AgeRatedUp],
		[DiscountRate],
		[LoadingType],
		[UnderwriterRating],
		[UnderwriterRatingUnit],
		[UnderwriterRatingTerm],
		[UnderwriterRating2],
		[UnderwriterRatingUnit2],
		[UnderwriterRatingTerm2],
		[UnderwriterRating3],
		[UnderwriterRatingUnit3],
		[UnderwriterRatingTerm3],
		[FlatExtraAmount],
		[FlatExtraUnit],
		[FlatExtraTerm],
		[FlatExtraAmount2],
		[FlatExtraTerm2],
		[StandardPremium],
		[SubstandardPremium],
		[FlatExtraPremium],
		[GrossPremium],
		[StandardDiscount],
		[SubstandardDiscount],
		[VitalityDiscount],
		[TotalDiscount],
		[NetPremium],
		[AnnualRiPrem],
		[RiCovPeriod],
		[AdjBeginDate],
		[AdjEndDate],
		[PolicyNumberOld],
		[PolicyStatusCode],
		[PolicyGrossPremium],
		[PolicyStandardPremium],
		[PolicySubstandardPremium],
		[PolicyTermRemain],
		[PolicyAmountDeath],
		[PolicyReserve],
		[PolicyPaymentMethod],
		[PolicyLifeNumber],
		[FundCode],
		[LineOfBusiness],
		[ApLoading],
		[LoanInterestRate],
		[DefermentPeriod],
		[RiderNumber],
		[CampaignCode],
		[Nationality],
		[TerritoryOfIssueCode],
		[CurrencyCode],
		[StaffPlanIndicator],
		[CedingTreatyCode],
		[CedingPlanCodeOld],
		[CedingBasicPlanCode],
		[CedantSar],
		[CedantReinsurerCode],
		[AmountCededB4MlreShare2],
		[CessionCode],
		[CedantRemark],
		[GroupPolicyNumber],
		[GroupPolicyName],
		[NoOfEmployee],
		[PolicyTotalLive],
		[GroupSubsidiaryName],
		[GroupSubsidiaryNo],
		[GroupEmployeeBasicSalary],
		[GroupEmployeeJobType],
		[GroupEmployeeJobCode],
		[GroupEmployeeBasicSalaryRevise],
		[GroupEmployeeBasicSalaryMultiplier],
		[CedingPlanCode2],
		[DependantIndicator],
		[GhsRoomBoard],
		[PolicyAmountSubstandard],
		[Layer1RiShare],
		[Layer1InsuredAttainedAge],
		[Layer1InsuredAttainedAge2nd],
		[Layer1StandardPremium],
		[Layer1SubstandardPremium],
		[Layer1GrossPremium],
		[Layer1StandardDiscount],
		[Layer1SubstandardDiscount],
		[Layer1TotalDiscount],
		[Layer1NetPremium],
		[Layer1GrossPremiumAlt],
		[Layer1TotalDiscountAlt],
		[Layer1NetPremiumAlt],
		[SpecialIndicator1],
		[SpecialIndicator2],
		[SpecialIndicator3],
		[IndicatorJointLife],
		[TaxAmount],
		[GstIndicator],
		[GstGrossPremium],
		[GstTotalDiscount],
		[GstVitality],
		[GstAmount],
		[Mfrs17BasicRider],
		[Mfrs17CellName],
		[Mfrs17TreatyCode],
		[LoaCode],
		[CurrencyRate],
		[NoClaimBonus],
		[SurrenderValue],
		[DatabaseCommision],
		[GrossPremiumAlt],
		[NetPremiumAlt],
		[Layer1FlatExtraPremium],
		[TransactionPremium],
		[OriginalPremium],
		[TransactionDiscount],
		[OriginalDiscount],
		[BrokerageFee],
		[MaxUwRating],
		[RetentionCap],
		[AarCap],
		[RiRate],
		[RiRate2],
		[AnnuityFactor],
		[SumAssuredOffered],
		[UwRatingOffered],
		[FlatExtraAmountOffered],
		[FlatExtraDuration],
		[EffectiveDate],
		[OfferLetterSentDate],
		[RiskPeriodStartDate],
		[RiskPeriodEndDate],
		[Mfrs17AnnualCohort],
		[MaxExpiryAge],
		[MinIssueAge],
		[MaxIssueAge],
		[MinAar],
		[MaxAar],
		[CorridorLimit],
		[Abl],
		[RatePerBasisUnit],
		[RiDiscountRate],
		[LargeSaDiscount],
		[GroupSizeDiscount],
		[EwarpNumber],
		[EwarpActionCode],
		[RetentionShare],
		[AarShare],
		[ProfitComm],
		[TotalDirectRetroAar],
		[TotalDirectRetroGrossPremium],
		[TotalDirectRetroDiscount],
		[TotalDirectRetroNetPremium],
		[TotalDirectRetroNoClaimBonus],
		[TotalDirectRetroDatabaseCommission],
		[TreatyType],
		[LastUpdatedDate],
		[RetroParty1],
		[RetroParty2],
		[RetroParty3],
		[RetroShare1],
		[RetroShare2],
		[RetroShare3],
		[RetroPremiumSpread1],
		[RetroPremiumSpread2],
		[RetroPremiumSpread3],
		[RetroAar1],
		[RetroAar2],
		[RetroAar3],
		[RetroReinsurancePremium1],
		[RetroReinsurancePremium2],
		[RetroReinsurancePremium3],
		[RetroDiscount1],
		[RetroDiscount2],
		[RetroDiscount3],
		[RetroNetPremium1],
		[RetroNetPremium2],
		[RetroNetPremium3],
		[RetroNoClaimBonus1],
		[RetroNoClaimBonus2],
		[RetroNoClaimBonus3],
		[RetroDatabaseCommission1],
		[RetroDatabaseCommission2],
		[RetroDatabaseCommission3],
		[MaxApLoading],
		[MlreInsuredAttainedAgeAtCurrentMonth],
		[MlreInsuredAttainedAgeAtPreviousMonth],
		[InsuredAttainedAgeCheck],
		[MaxExpiryAgeCheck],
		[MlrePolicyIssueAge],
		[PolicyIssueAgeCheck],
		[MinIssueAgeCheck],
		[MaxIssueAgeCheck],
		[MaxUwRatingCheck],
		[ApLoadingCheck],
		[EffectiveDateCheck],
		[MinAarCheck],
		[MaxAarCheck],
		[CorridorLimitCheck],
		[AblCheck],
		[RetentionCheck],
		[AarCheck],
		[MlreStandardPremium],
		[MlreSubstandardPremium],
		[MlreFlatExtraPremium],
		[MlreGrossPremium],
		[MlreStandardDiscount],
		[MlreSubstandardDiscount],
		[MlreLargeSaDiscount],
		[MlreGroupSizeDiscount],
		[MlreVitalityDiscount],
		[MlreTotalDiscount],
		[MlreNetPremium],
		[NetPremiumCheck],
		[ServiceFeePercentage],
		[ServiceFee],
		[MlreBrokerageFee],
		[MlreDatabaseCommission],
		[ValidityDayCheck],
		[SumAssuredOfferedCheck],
		[UwRatingCheck],
		[FlatExtraAmountCheck],
		[FlatExtraDurationCheck],
		[AarShare2],
		[AarCap2],
		[WakalahFeePercentage],
		[TreatyNumber],
		[ConflictType],
		[CreatedAt],
		[UpdatedAt],
		[CreatedById],
		[UpdatedById]
	FROM
		 RiDataWarehouse
	WHERE
		[Id] >= @Start AND [Id] < @StartPlusInterval

	SET @Start = @StartPlusInterval
END