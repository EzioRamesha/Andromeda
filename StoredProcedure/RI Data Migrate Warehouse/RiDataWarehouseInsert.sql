CREATE OR ALTER PROCEDURE [dbo].[RiDataWarehouseInsert] (@RiDataId INT, @HasError BIT OUTPUT)

AS

SET @HasError = 0;

--IF EXISTS (SELECT * FROM RiData WHERE Id = @RiDataId AND RecordType = dbo.GetConstantInt('RiDataRecordTypeAdj'))
--	BEGIN
--	 UPDATE RiData SET Errors = '{"ProcessWarehouseError":"Record not found in RI Data Warehouse for RI Data with Record Type: Adjustment"}' WHERE id = @RiDataId;
--	 SET @HasError = 1;
--	 RETURN;
--	END

--IF EXISTS (SELECT * FROM RiData WHERE Id = @RiDataId AND TransactionTypeCode = dbo.GetConstant('TransactionTypeCodeAlteration'))
--	BEGIN
--		UPDATE RiData SET Errors = '{"ProcessWarehouseError":"Record not found in RI Data Warehouse for RI Data with Transaction Type Code: AL"}' WHERE id = @RiDataId;
--		SET @HasError = 1;
--		RETURN;
--	END

INSERT INTO RiDataWarehouse (
	[EndingPolicyStatus], [Quarter], [TreatyCode], [ReinsBasisCode], [FundsAccountingTypeCode], [PremiumFrequencyCode], [ReportPeriodMonth],
	[ReportPeriodYear], [RiskPeriodMonth], [RiskPeriodYear], [TransactionTypeCode], [PolicyNumber], [IssueDatePol], [IssueDateBen],
	[ReinsEffDatePol], [ReinsEffDateBen], [CedingPlanCode], [CedingBenefitTypeCode], [CedingBenefitRiskCode], [MlreBenefitCode], [OriSumAssured],
	[CurrSumAssured], [AmountCededB4MlreShare], [RetentionAmount], [AarOri], [Aar], [AarSpecial1], [AarSpecial2], [AarSpecial3], [InsuredName],
	[InsuredGenderCode], [InsuredTobaccoUse], [InsuredDateOfBirth], [InsuredOccupationCode], [InsuredRegisterNo], [InsuredAttainedAge],
	[InsuredNewIcNumber], [InsuredOldIcNumber], [InsuredName2nd], [InsuredGenderCode2nd], [InsuredTobaccoUse2nd], [InsuredDateOfBirth2nd],
	[InsuredAttainedAge2nd], [InsuredNewIcNumber2nd], [InsuredOldIcNumber2nd], [ReinsuranceIssueAge], [ReinsuranceIssueAge2nd], [PolicyTerm],
	[PolicyExpiryDate], [DurationYear], [DurationDay], [DurationMonth], [PremiumCalType], [CedantRiRate], [RateTable], [AgeRatedUp], [DiscountRate],
	[LoadingType], [UnderwriterRating], [UnderwriterRatingUnit], [UnderwriterRatingTerm], [UnderwriterRating2], [UnderwriterRatingUnit2], 
	[UnderwriterRatingTerm2], [UnderwriterRating3], [UnderwriterRatingUnit3], [UnderwriterRatingTerm3], [FlatExtraAmount], [FlatExtraUnit],
	[FlatExtraTerm], [FlatExtraAmount2], [FlatExtraTerm2], [StandardPremium], [SubstandardPremium], [FlatExtraPremium], [GrossPremium],
	[StandardDiscount], [SubstandardDiscount], [VitalityDiscount], [TotalDiscount], [NetPremium], [AnnualRiPrem], [RiCovPeriod], [AdjBeginDate],
	[AdjEndDate], [PolicyNumberOld], [PolicyStatusCode], [PolicyGrossPremium], [PolicyStandardPremium], [PolicySubstandardPremium], [PolicyTermRemain],
	[PolicyAmountDeath], [PolicyReserve], [PolicyPaymentMethod], [PolicyLifeNumber], [FundCode], [LineOfBusiness], [ApLoading], [LoanInterestRate],
	[DefermentPeriod], [RiderNumber], [CampaignCode], [Nationality], [TerritoryOfIssueCode], [CurrencyCode], [StaffPlanIndicator], [CedingTreatyCode],
	[CedingPlanCodeOld], [CedingBasicPlanCode], [CedantSar], [CedantReinsurerCode], [AmountCededB4MlreShare2], [CessionCode], [CedantRemark],
	[GroupPolicyNumber], [GroupPolicyName], [NoOfEmployee], [PolicyTotalLive], [GroupSubsidiaryName], [GroupSubsidiaryNo], [GroupEmployeeBasicSalary],
	[GroupEmployeeJobType], [GroupEmployeeJobCode], [GroupEmployeeBasicSalaryRevise], [GroupEmployeeBasicSalaryMultiplier], [CedingPlanCode2],
	[DependantIndicator], [GhsRoomBoard], [PolicyAmountSubstandard], [Layer1RiShare], [Layer1InsuredAttainedAge], [Layer1InsuredAttainedAge2nd],
	[Layer1StandardPremium], [Layer1SubstandardPremium], [Layer1GrossPremium], [Layer1StandardDiscount], [Layer1SubstandardDiscount], [Layer1TotalDiscount],
	[Layer1NetPremium], [Layer1GrossPremiumAlt], [Layer1TotalDiscountAlt], [Layer1NetPremiumAlt], [SpecialIndicator1], [SpecialIndicator2], [SpecialIndicator3],
	[IndicatorJointLife], [TaxAmount], [GstIndicator], [GstGrossPremium], [GstTotalDiscount], [GstVitality], [GstAmount], [Mfrs17BasicRider], [Mfrs17CellName],
	[Mfrs17TreatyCode], [LoaCode], [CurrencyRate], [NoClaimBonus], [SurrenderValue], [DatabaseCommision], [GrossPremiumAlt], [NetPremiumAlt],
	[Layer1FlatExtraPremium], [TransactionPremium], [OriginalPremium], [TransactionDiscount], [OriginalDiscount], [BrokerageFee], [MaxUwRating],
	[RetentionCap], [AarCap], [RiRate], [RiRate2], [AnnuityFactor], [SumAssuredOffered], [UwRatingOffered], [FlatExtraAmountOffered], [FlatExtraDuration],
	[EffectiveDate], [OfferLetterSentDate], [RiskPeriodStartDate], [RiskPeriodEndDate], [Mfrs17AnnualCohort], [MaxExpiryAge], [MinIssueAge], [MaxIssueAge],
	[MinAar], [MaxAar], [CorridorLimit], [Abl], [RatePerBasisUnit], [RiDiscountRate], [LargeSaDiscount], [GroupSizeDiscount], [EwarpNumber], [EwarpActionCode],
	[RetentionShare], [AarShare], [ProfitComm], [TotalDirectRetroAar], [TotalDirectRetroGrossPremium], [TotalDirectRetroDiscount], [TotalDirectRetroNetPremium],
	[TreatyType], [LastUpdatedDate], [CreatedAt], [UpdatedAt], [CreatedById], [UpdatedById], [RetroParty1], [RetroParty2], [RetroParty3], [RetroShare1],
	[RetroShare2], [RetroShare3], [RetroAar1], [RetroAar2], [RetroAar3], [RetroReinsurancePremium1], [RetroReinsurancePremium2], [RetroReinsurancePremium3],
	[RetroDiscount1], [RetroDiscount2], [RetroDiscount3], [RetroNetPremium1], [RetroNetPremium2], [RetroNetPremium3], [RecordType],
	[MlreInsuredAttainedAgeAtCurrentMonth],[MlrePolicyIssueAge],[MlreStandardPremium],[MlreSubstandardPremium],[MlreFlatExtraPremium],
	[MlreGrossPremium],[MlreTotalDiscount],[MlreNetPremium],[NetPremiumCheck],[AarShare2],[AarCap2],[WakalahFeePercentage],[MaxApLoading],[MlreInsuredAttainedAgeAtPreviousMonth],
	[MlreStandardDiscount],[MlreSubstandardDiscount],[MlreLargeSaDiscount],[MlreGroupSizeDiscount],[MlreVitalityDiscount],[ServiceFeePercentage],[ServiceFee],[MlreBrokerageFee],
	[MlreDatabaseCommission],[TreatyNumber],[TotalDirectRetroNoClaimBonus],[TotalDirectRetroDatabaseCommission],[RetroPremiumSpread1],[RetroPremiumSpread2],[RetroPremiumSpread3],
	[RetroNoClaimBonus1],[RetroNoClaimBonus2],[RetroNoClaimBonus3],[RetroDatabaseCommission1],[RetroDatabaseCommission2],[RetroDatabaseCommission3],[InsuredAttainedAgeCheck],[MaxExpiryAgeCheck],
	[PolicyIssueAgeCheck],[MinIssueAgeCheck],[MaxIssueAgeCheck],[MaxUwRatingCheck],[ApLoadingCheck],[EffectiveDateCheck],[MinAarCheck],[MaxAarCheck],[CorridorLimitCheck],[AblCheck],
	[RetentionCheck],[AarCheck],[ValidityDayCheck],[SumAssuredOfferedCheck],[UwRatingCheck],[FlatExtraAmountCheck],[FlatExtraDurationCheck],[ConflictType]
) 
SELECT 
	dbo.GetPickListDetailId(PolicyStatusCode, 18),
	(Select rdb.Quarter FROM RiDataBatches rdb WHERE RiData.RiDataBatchId = rdb.Id),
	[TreatyCode], [ReinsBasisCode], [FundsAccountingTypeCode], [PremiumFrequencyCode], [ReportPeriodMonth], [ReportPeriodYear], [RiskPeriodMonth],
	[RiskPeriodYear], [TransactionTypeCode], [PolicyNumber], [IssueDatePol], [IssueDateBen], [ReinsEffDatePol], [ReinsEffDateBen], [CedingPlanCode],
	[CedingBenefitTypeCode], [CedingBenefitRiskCode], [MlreBenefitCode], [OriSumAssured], [CurrSumAssured], [AmountCededB4MlreShare], [RetentionAmount],
	[AarOri], [Aar], [AarSpecial1], [AarSpecial2], [AarSpecial3], [InsuredName], [InsuredGenderCode], [InsuredTobaccoUse], [InsuredDateOfBirth],
	[InsuredOccupationCode], [InsuredRegisterNo], [InsuredAttainedAge], [InsuredNewIcNumber], [InsuredOldIcNumber], [InsuredName2nd], [InsuredGenderCode2nd],
	[InsuredTobaccoUse2nd], [InsuredDateOfBirth2nd], [InsuredAttainedAge2nd], [InsuredNewIcNumber2nd], [InsuredOldIcNumber2nd], [ReinsuranceIssueAge],
	[ReinsuranceIssueAge2nd], [PolicyTerm], [PolicyExpiryDate], [DurationYear], [DurationDay], [DurationMonth], [PremiumCalType], [CedantRiRate], [RateTable],
	[AgeRatedUp], [DiscountRate], [LoadingType], [UnderwriterRating], [UnderwriterRatingUnit], [UnderwriterRatingTerm], [UnderwriterRating2],
	[UnderwriterRatingUnit2], [UnderwriterRatingTerm2], [UnderwriterRating3], [UnderwriterRatingUnit3], [UnderwriterRatingTerm3], [FlatExtraAmount],
	[FlatExtraUnit], [FlatExtraTerm], [FlatExtraAmount2], [FlatExtraTerm2], [StandardPremium], [SubstandardPremium], [FlatExtraPremium], [GrossPremium],
	[StandardDiscount], [SubstandardDiscount], [VitalityDiscount], [TotalDiscount], [NetPremium], [AnnualRiPrem], [RiCovPeriod], [AdjBeginDate], [AdjEndDate],
	[PolicyNumberOld], [PolicyStatusCode], [PolicyGrossPremium], [PolicyStandardPremium], [PolicySubstandardPremium], [PolicyTermRemain], [PolicyAmountDeath],
	[PolicyReserve], [PolicyPaymentMethod], [PolicyLifeNumber], [FundCode], [LineOfBusiness], [ApLoading], [LoanInterestRate], [DefermentPeriod], [RiderNumber],
	[CampaignCode], [Nationality], [TerritoryOfIssueCode], [CurrencyCode], [StaffPlanIndicator], [CedingTreatyCode], [CedingPlanCodeOld], [CedingBasicPlanCode],
	[CedantSar], [CedantReinsurerCode], [AmountCededB4MlreShare2], [CessionCode], [CedantRemark], [GroupPolicyNumber], [GroupPolicyName], [NoOfEmployee],
	[PolicyTotalLive], [GroupSubsidiaryName], [GroupSubsidiaryNo], [GroupEmployeeBasicSalary], [GroupEmployeeJobType], [GroupEmployeeJobCode],
	[GroupEmployeeBasicSalaryRevise], [GroupEmployeeBasicSalaryMultiplier], [CedingPlanCode2], [DependantIndicator], [GhsRoomBoard], [PolicyAmountSubstandard],
	[Layer1RiShare], [Layer1InsuredAttainedAge], [Layer1InsuredAttainedAge2nd], [Layer1StandardPremium], [Layer1SubstandardPremium], [Layer1GrossPremium],
	[Layer1StandardDiscount], [Layer1SubstandardDiscount], [Layer1TotalDiscount], [Layer1NetPremium], [Layer1GrossPremiumAlt], [Layer1TotalDiscountAlt],
	[Layer1NetPremiumAlt], [SpecialIndicator1], [SpecialIndicator2], [SpecialIndicator3], [IndicatorJointLife], [TaxAmount], [GstIndicator], [GstGrossPremium],
	[GstTotalDiscount], [GstVitality], [GstAmount], [Mfrs17BasicRider], [Mfrs17CellName], [Mfrs17TreatyCode], [LoaCode], [CurrencyRate], [NoClaimBonus],
	[SurrenderValue], [DatabaseCommision], [GrossPremiumAlt], [NetPremiumAlt], [Layer1FlatExtraPremium], [TransactionPremium], [OriginalPremium],
	[TransactionDiscount], [OriginalDiscount], [BrokerageFee], [MaxUwRating], [RetentionCap], [AarCap], [RiRate], [RiRate2], [AnnuityFactor], [SumAssuredOffered],
	[UwRatingOffered], [FlatExtraAmountOffered], [FlatExtraDuration], [EffectiveDate], [OfferLetterSentDate], [RiskPeriodStartDate], [RiskPeriodEndDate],
	[Mfrs17AnnualCohort], [MaxExpiryAge], [MinIssueAge], [MaxIssueAge], [MinAar], [MaxAar], [CorridorLimit], [Abl], [RatePerBasisUnit], [RiDiscountRate],
	[LargeSaDiscount], [GroupSizeDiscount], [EwarpNumber], [EwarpActionCode], [RetentionShare], [AarShare], [ProfitComm], [TotalDirectRetroAar],
	[TotalDirectRetroGrossPremium], [TotalDirectRetroDiscount], [TotalDirectRetroNetPremium], [TreatyType], Convert(date, GETDATE()), GETDATE(), GETDATE(),
	dbo.GetConstantInt('AuthUserId'), dbo.GetConstantInt('AuthUserId'), [RetroParty1], [RetroParty2], [RetroParty3], [RetroShare1], [RetroShare2], [RetroShare3], 
	[RetroAar1], [RetroAar2], [RetroAar3], [RetroReinsurancePremium1], [RetroReinsurancePremium2], [RetroReinsurancePremium3], [RetroDiscount1], 
	[RetroDiscount2], [RetroDiscount3], [RetroNetPremium1], [RetroNetPremium2], [RetroNetPremium3], [RecordType],
	[MlreInsuredAttainedAgeAtCurrentMonth], [MlrePolicyIssueAge], [MlreStandardPremium], [MlreSubstandardPremium], [MlreFlatExtraPremium],
	[MlreGrossPremium], [MlreTotalDiscount], [MlreNetPremium], [NetPremiumCheck], [AarShare2], [AarCap2], [WakalahFeePercentage], [MaxApLoading], [MlreInsuredAttainedAgeAtPreviousMonth],
	[MlreStandardDiscount], [MlreSubstandardDiscount], [MlreLargeSaDiscount], [MlreGroupSizeDiscount], [MlreVitalityDiscount], [ServiceFeePercentage], [ServiceFee], [MlreBrokerageFee],
	[MlreDatabaseCommission], [TreatyNumber], [TotalDirectRetroNoClaimBonus], [TotalDirectRetroDatabaseCommission], [RetroPremiumSpread1], [RetroPremiumSpread2], [RetroPremiumSpread3],
	[RetroNoClaimBonus1], [RetroNoClaimBonus2], [RetroNoClaimBonus3], [RetroDatabaseCommission1], [RetroDatabaseCommission2], [RetroDatabaseCommission3], [InsuredAttainedAgeCheck], [MaxExpiryAgeCheck], 
	[PolicyIssueAgeCheck], [MinIssueAgeCheck], [MaxIssueAgeCheck], [MaxUwRatingCheck], [ApLoadingCheck], [EffectiveDateCheck], [MinAarCheck], [MaxAarCheck], [CorridorLimitCheck], [AblCheck], 
	[RetentionCheck], [AarCheck], [ValidityDayCheck], [SumAssuredOfferedCheck], [UwRatingCheck], [FlatExtraAmountCheck], [FlatExtraDurationCheck], [ConflictType]
FROM 
	RiData 
WHERE 
	RiData.Id = @RiDataId;

RETURN SCOPE_IDENTITY()

