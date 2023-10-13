CREATE OR ALTER PROCEDURE [dbo].[ValidatePerLifeDetail](@PerLifeAggregationDetailId INT, @Result VARCHAR(MAX) OUTPUT)

AS

SET NOCOUNT ON;

-- Delete Previous Data for re-processing
-- Delete MonthlyRetroData
DELETE FROM PerLifeAggregationMonthlyRetroData 
WHERE PerLifeAggregationMonthlyDataId IN 
(SELECT Id FROM PerLifeAggregationMonthlyData WHERE PerLifeAggregationDetailDataId IN 
(SELECT Id FROM PerLifeAggregationDetailData WHERE PerLifeAggregationDetailTreatyId IN 
(SELECT Id FROM PerLifeAggregationDetailTreaties WHERE PerLifeAggregationDetailId = @PerLifeAggregationDetailId)));

-- Delete MonthlyData
DELETE FROM PerLifeAggregationMonthlyData
WHERE PerLifeAggregationDetailDataId IN 
(SELECT Id FROM PerLifeAggregationDetailData WHERE PerLifeAggregationDetailTreatyId IN 
(SELECT Id FROM PerLifeAggregationDetailTreaties WHERE PerLifeAggregationDetailId = @PerLifeAggregationDetailId));

-- Constant
DECLARE
	@TypeInsuredGenderCode INT = 30,
	@TypeTerritoryOfIssueCode INT = 103,
	@TypeFundsAccountingType INT = 24,
	@TypeReinsBasisCode INT = 9,
	@TypeTransactionTypeCode INT = 11

-- Variable
DECLARE
	@IsProceedToAggregate BIT = 0,					-- Set Back to Table?
	@ExpectedGenderCode VARCHAR(MAX),				-- Set Back to Table
	@RetroBenefitCode VARCHAR(MAX),					-- Set Back to Table
	@ExpectedTerritoryOfIssueCode VARCHAR(MAX),		-- Set Back to Table
	@FlagCode INT,									-- Set Back to Table
	@ExceptionType INT,								-- Set Back to Table
	@ExceptionErrorType INT,						-- Set Back to Table
	@IsException BIT,								-- Set Back to Table
	@ProceedStatus INT,								-- Set Back to Table
	@IsToAggregate BIT,								-- Set Back to Table
	@CurrentTreatyCode VARCHAR(MAX),
	@IsInclusion BIT = 0,
	@IsCheckReinsBasisCode BIT = 0

-- Table
DECLARE
	-- PerLifeAggregationDetailData
	@PerLifeAggregationDetailDataId INT,
	@PerLifeAggregationDetailTreatyId INT,
	@RiDataWarehouseHistoryId INT,
	-- RiDataWarehouseHistory
	@TreatyCode VARCHAR(MAX),
	@InsuredName VARCHAR(MAX),
	@InsuredDateOfBirth VARCHAR(MAX),
	@PolicyNumber VARCHAR(MAX),
	@InsuredGenderCode VARCHAR(MAX),
	@TerritoryOfIssueCode VARCHAR(MAX),
	@FundsAccountingTypeCode VARCHAR(MAX),
	@CurrencyCode VARCHAR(MAX),
	@Aar VARCHAR(MAX),
	@NetPremium VARCHAR(MAX),
	@MlreBenefitCode VARCHAR(MAX),
	@ReinsEffDatePol VARCHAR(MAX),
	@UnderwriterRating VARCHAR(MAX),
	@CedingTreatyCode VARCHAR(MAX),
	@TransactionTypeCode VARCHAR(MAX),
	@EffectiveDate VARCHAR(MAX),
	@CedingPlanCode VARCHAR(MAX),
	@CedingBenefitRiskCode VARCHAR(MAX),
	@CedingBenefitTypeCode VARCHAR(MAX),
	@ReinsBasisCode VARCHAR(MAX),
	@RiskDate VARCHAR(MAX),
	@TreatyType VARCHAR(MAX)

-- Count
DECLARE
	@TotalRecords INT = 0,
	@SuccessRecords INT = 0,
	@FailedRecords INT = 0,
	@RiskQuarter VARCHAR(MAX)

DECLARE PerLifeAggregationDetailDataCursor CURSOR FOR
SELECT
	-- PerLifeAggregationDetailData
	PLADD.[Id] AS Id,
	PLADD.[PerLifeAggregationDetailTreatyId] AS PerLifeAggregationDetailTreatyId,
	PLADD.[RiDataWarehouseHistoryId] AS RiDataWarehouseHistoryId,
	-- RiDataWarehouseHistory
	RDWH.[TreatyCode] AS TreatyCode,
	RDWH.[InsuredName] AS InsuredName,
	RDWH.[InsuredDateOfBirth] AS InsuredDateOfBirth,
	RDWH.[PolicyNumber] AS PolicyNumber,
	RDWH.[InsuredGenderCode] AS InsuredGenderCode,
	RDWH.[TerritoryOfIssueCode] AS TerritoryOfIssueCode,
	RDWH.[FundsAccountingTypeCode] AS FundsAccountingTypeCode,
	RDWH.[CurrencyCode] AS CurrencyCode,
	RDWH.[Aar] AS Aar,
	RDWH.[NetPremium] AS NetPremium,
	RDWH.[MlreBenefitCode] AS MlreBenefitCode,
	RDWH.[ReinsEffDatePol] AS ReinsEffDatePol,
	RDWH.[UnderwriterRating] AS UnderwriterRating,
	RDWH.[CedingTreatyCode] AS CedingTreatyCode,
	RDWH.[TransactionTypeCode] AS TransactionTypeCode,
	RDWH.[EffectiveDate] AS EffectiveDate,
	RDWH.[CedingPlanCode] AS CedingPlanCode,
	RDWH.[CedingBenefitRiskCode] AS CedingBenefitRiskCode,
	RDWH.[CedingBenefitTypeCode] AS CedingBenefitTypeCode,
	RDWH.[ReinsBasisCode] AS ReinsBasisCode,
	CAST(RDWH.RiskPeriodYear AS VARCHAR(4)) + '-' + CAST(RDWH.RiskPeriodMonth AS VARCHAR(2)) + '-01' AS RiskDate,
	RDWH.[TreatyType] AS TreatyType
FROM 
	PerLifeAggregationDetailData AS PLADD
LEFT JOIN 
	RiDataWarehouseHistories AS RDWH ON PLADD.RiDataWarehouseHistoryId = RDWH.Id
WHERE 
	PerLifeAggregationDetailTreatyId IN (SELECT Id FROM PerLifeAggregationDetailTreaties WHERE PerLifeAggregationDetailId = @PerLifeAggregationDetailId)
ORDER BY PLADD.Id

OPEN PerLifeAggregationDetailDataCursor

FETCH NEXT FROM PerLifeAggregationDetailDataCursor INTO 
	-- PerLifeAggregationDetailData
	@PerLifeAggregationDetailDataId,
	@PerLifeAggregationDetailTreatyId,
	@RiDataWarehouseHistoryId,
	-- RiDataWarehouseHistory
	@TreatyCode,
	@InsuredName,
	@InsuredDateOfBirth,
	@PolicyNumber,
	@InsuredGenderCode,
	@TerritoryOfIssueCode,
	@FundsAccountingTypeCode,
	@CurrencyCode,
	@Aar,
	@NetPremium,
	@MlreBenefitCode,
	@ReinsEffDatePol,
	@UnderwriterRating,
	@CedingTreatyCode,
	@TransactionTypeCode,
	@EffectiveDate,
	@CedingPlanCode,
	@CedingBenefitRiskCode,
	@CedingBenefitTypeCode,
	@ReinsBasisCode,
	@RiskDate,
	@TreatyType

WHILE @@FETCH_STATUS = 0

BEGIN
	-- RESET Value
	SET @IsProceedToAggregate = 0;
	SET @IsException = 0;
	SET @ProceedStatus = dbo.GetConstantInt('ProceedStatusProceed');
	SET @IsToAggregate = 0;
	-- SET @RetroBenefitCodeId = 0;
	SET @RetroBenefitCode = NULL;

	IF (dbo.IsNull(@CurrentTreatyCode) = 1) OR @CurrentTreatyCode != @TreatyCode
		BEGIN
			SET @CurrentTreatyCode = @TreatyCode;

			SET @IsInclusion = 0; 
			SET @IsCheckReinsBasisCode = 0; 
			SELECT TOP(1) @IsInclusion = Inclusion, @IsCheckReinsBasisCode = EnableReinsuranceBasisCodeCheck
			FROM PerLifeDuplicationCheckDetails AS DCD
			JOIN  PerLifeDuplicationChecks AS DC ON DC.Id = DCD.PerLifeDuplicationCheckId
			WHERE DCD.TreatyCode = @CurrentTreatyCode
		END

	EXEC @ExceptionType = ValidatePerLifeDetailData @PerLifeAggregationDetailId, @IsInclusion, @IsCheckReinsBasisCode, @TreatyCode, @InsuredName,
		@InsuredDateOfBirth, @PolicyNumber, @InsuredGenderCode, @TerritoryOfIssueCode, @FundsAccountingTypeCode, @CurrencyCode, @Aar, 
		@NetPremium, @MlreBenefitCode, @ReinsEffDatePol, @UnderwriterRating, @CedingTreatyCode, @TransactionTypeCode, @EffectiveDate,
		@CedingPlanCode, @CedingBenefitRiskCode, @CedingBenefitTypeCode, @ReinsBasisCode, @RiskDate, @TreatyType, @IsProceedToAggregate OUTPUT, 
		@ExpectedGenderCode OUTPUT, @ExpectedTerritoryOfIssueCode OUTPUT, @RetroBenefitCode OUTPUT, @FlagCode OUTPUT, @ExceptionErrorType OUTPUT, 
		@IsToAggregate OUTPUT

	SET @TotalRecords += 1;
	IF (@ExceptionType = 0)
		BEGIN
			SET @SuccessRecords += 1;
			SET @FlagCode = dbo.GetConstantInt('FlagCodeGood1');
		END
	ELSE
		BEGIN
			SET @FailedRecords += 1;
			SET @IsException = 1;
			SET @ProceedStatus = dbo.GetConstantInt('ProceedStatusNotProceed');
		END
		
	
	-- Unable to find Retro Benefit Code
	-- IF (dbo.IsNull(@RetroBenefitCode) = 1)
	-- 	BEGIN
	-- 		UPDATE PerLifeAggregationDetailData
	-- 		SET IsException = 1, ExceptionType = dbo.GetConstantInt('ExceptionTypeRetroBenefitCodeMapping')
	-- 		WHERE Id = @PerLifeAggregationDetailDataId;

	-- 		FETCH NEXT FROM PerLifeAggregationDetailDataCursor INTO 
	-- 			-- PerLifeAggregationDetailData
	-- 			@PerLifeAggregationDetailDataId,
	-- 			@PerLifeAggregationDetailTreatyId,
	-- 			@RiDataWarehouseHistoryId,
	-- 			-- RiDataWarehouseHistory
	-- 			@TreatyCode,
	-- 			@InsuredName,
	-- 			@InsuredDateOfBirth,
	-- 			@PolicyNumber,
	-- 			@InsuredGenderCode,
	-- 			@TerritoryOfIssueCode,
	-- 			@FundsAccountingTypeCode,
	-- 			@CurrencyCode,
	-- 			@Aar,
	-- 			@NetPremium,
	-- 			@MlreBenefitCode,
	-- 			@ReinsEffDatePol,
	-- 			@UnderwriterRating,
	-- 			@CedingTreatyCode,
	-- 			@TransactionTypeCode,
	-- 			@EffectiveDate,
	-- 			@CedingPlanCode,
	-- 			@CedingBenefitRiskCode,
	-- 			@CedingBenefitTypeCode,
	-- 			@ReinsBasisCode
				
	-- 		CONTINUE;
	-- 	END

	
	UPDATE 
		PerLifeAggregationDetailData
	SET 
		IsException = @IsException,
		ExceptionType = @ExceptionType,
		ExceptionErrorType = @ExceptionErrorType,
		FlagCode = @FlagCode,
		ExpectedGenderCode = @ExpectedGenderCode,
		RetroBenefitCode = @RetroBenefitCode, 
		ExpectedTerritoryOfIssueCode = @ExpectedTerritoryOfIssueCode,
		ProceedStatus = @ProceedStatus,
		IsToAggregate = @IsToAggregate
	WHERE 
		Id = @PerLifeAggregationDetailDataId;

	FETCH NEXT FROM PerLifeAggregationDetailDataCursor INTO 
		-- PerLifeAggregationDetailData
		@PerLifeAggregationDetailDataId,
		@PerLifeAggregationDetailTreatyId,
		@RiDataWarehouseHistoryId,
		-- RiDataWarehouseHistory
		@TreatyCode,
		@InsuredName,
		@InsuredDateOfBirth,
		@PolicyNumber,
		@InsuredGenderCode,
		@TerritoryOfIssueCode,
		@FundsAccountingTypeCode,
		@CurrencyCode,
		@Aar,
		@NetPremium,
		@MlreBenefitCode,
		@ReinsEffDatePol,
		@UnderwriterRating,
		@CedingTreatyCode,
		@TransactionTypeCode,
		@EffectiveDate,
		@CedingPlanCode,
		@CedingBenefitRiskCode,
		@CedingBenefitTypeCode,
		@ReinsBasisCode,
		@RiskDate,
		@TreatyType
			
END

CLOSE PerLifeAggregationDetailDataCursor;

DEALLOCATE PerLifeAggregationDetailDataCursor;

SELECT @RiskQuarter = RiskQuarter FROM PerLifeAggregationDetails WHERE Id = @PerLifeAggregationDetailId;

-- Set Result
SET @Result = 
	'"' + @RiskQuarter + ' (Total)": ' + CAST(@TotalRecords AS VARCHAR(10)) + ', ' +
	'"' + @RiskQuarter + ' (Success)": ' + CAST(@SuccessRecords AS VARCHAR(10)) + ', ' +
	'"' + @RiskQuarter + ' (Failed)": ' + CAST(@FailedRecords AS VARCHAR(10));