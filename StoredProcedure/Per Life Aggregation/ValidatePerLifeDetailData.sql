CREATE OR ALTER PROCEDURE [dbo].[ValidatePerLifeDetailData](
	@PerLifeAggregationDetailId INT,
	@IsInclusion BIT,
	@IsCheckReinsBasisCode BIT,
	-- PARAMS
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
	@TreatyType VARCHAR(MAX),
	-- OUTPUTS
	@IsProceedToAggregate BIT OUTPUT,
	@ExpectedGenderCode VARCHAR(15) OUTPUT,
	@ExpectedTerritoryOfIssueCode VARCHAR(50) OUTPUT,
	@RetroBenefitCode VARCHAR(30) OUTPUT,
	@FlagCode INT OUTPUT,
	@ExceptionErrorType INT OUTPUT,
	@IsToAggregate BIT OUTPUT
)

AS

-- Variable
DECLARE
	@TreatyCodeId INT,
	@InsuredGenderCodePickListDetailId INT,
	@TerritoryOfIssueCodePickListDetailId INT,
	@ExpectedGenderCodeId INT,
	@ExpectedTerritoryOfIssueCodeId INT,
	@MlreBenefitCodeId INT,
	@RetroBenefitCodeId INT,
	@MinRetentionLimit VARCHAR(MAX),
	@FundsAccountingTypePickListDetailId INT,
	@ReinsBasisCodePickListDetailId INT,
	@TransactionTypePickListDetailId INT,
	@RetentionLimitType INT,
	@ExceptionType INT

-- Find Per Life Retro Configuration Treaty
SELECT TOP(1) 
	@IsToAggregate = IsToAggregate
FROM 
	PerLifeRetroConfigurationTreaties AS CT
LEFT JOIN
	TreatyCodes AS TC ON TC.Id = CT.TreatyCodeId
LEFT JOIN
	PickListDetails AS TT ON TT.Id = CT.TreatyTypePickListDetailId
LEFT JOIN
	PickListDetails AS FAT ON FAT.Id = CT.FundsAccountingTypePickListDetailId
WHERE 
	TC.Code = @TreatyCode AND
	TT.Code = @TreatyType AND
	FAT.Code = @FundsAccountingTypeCode AND
	@ReinsEffDatePol BETWEEN ReinsEffectiveStartDate AND ReinsEffectiveEndDate AND
	@RiskDate BETWEEN RiskQuarterStartDate AND RiskQuarterEndDate

IF (@IsToAggregate IS NULL)
	SET @IsToAggregate = 0;

IF (@IsToAggregate = 0)
	BEGIN
		SET @FlagCode = NULL;
		RETURN 0;
	END

-- SET Retention Limit Type
SET @RetentionLimitType = CASE (@FundsAccountingTypeCode)
	WHEN dbo.GetConstant('FundsAccountingTypeCodeGroup') THEN dbo.GetConstantInt('RetentionLimitTypeGroup')
	ELSE dbo.GetConstantInt('RetentionLimitTypeIndividual') END

-- Retro Benefit Code Mapping
SELECT @MlreBenefitCodeId = Id FROM Benefits WHERE Code = @MlreBenefitCode;

SELECT TOP(1)
	@RetroBenefitCodeId = RBC.Id,
	@RetroBenefitCode = RBC.Code
FROM
	RetroBenefitCodeMappingDetails AS RBCMD
LEFT JOIN
	RetroBenefitCodes AS RBC ON RBCMD.RetroBenefitCodeId = RBC.Id
WHERE
	RetroBenefitCodeMappingId = (SELECT TOP(1) Id FROM RetroBenefitCodeMappings WHERE BenefitId = @MlreBenefitCodeId)

IF (dbo.IsNull(@RetroBenefitCode) = 1)
	BEGIN
		SET @ExceptionType =  dbo.GetConstantInt('ExceptionTypeRetroBenefitCodeMapping');
		SET @ExceptionErrorType = dbo.GetConstantInt('ExceptionErrorTypeRetroBenefitCodeMapping');
		RETURN @ExceptionType;
	END

-- Get MinRetentionLimit by RetroBenefitCode
SELECT TOP(1) @MinRetentionLimit = MinRetentionLimit 
FROM RetroBenefitRetentionLimits 
WHERE RetroBenefitCodeId = @RetroBenefitCodeId AND Type = @RetentionLimitType;

IF (dbo.IsNull(@MinRetentionLimit) = 1)
	BEGIN
		SET @ExceptionType =  dbo.GetConstantInt('ExceptionTypeRetroBenefitRetentionLimit');
		SET @ExceptionErrorType = dbo.GetConstantInt('ExceptionErrorTypeRetroBenefitRetentionLimit');
		RETURN @ExceptionType;
	END

-- Field Validation
EXEC @FlagCode = PerLifeFieldValidation @FundsAccountingTypeCode, @InsuredName, @InsuredDateOfBirth, 
	@InsuredGenderCode, @CurrencyCode, @Aar, @NetPremium, @MinRetentionLimit,
	@ExceptionErrorType=@ExceptionErrorType OUTPUT

IF (@FlagCode != 0)
	BEGIN
		SET @ExceptionType = dbo.GetConstantInt('ExceptionTypeFieldValidation');
		RETURN @ExceptionType;
	END

SET @InsuredGenderCodePickListDetailId = dbo.GetPickListDetailId(@InsuredGenderCode, 13);
SET @TerritoryOfIssueCodePickListDetailId = dbo.GetPickListDetailId(@TerritoryOfIssueCode, 31);

-- Basic Check
EXEC @FlagCode = PerLifeBasicCheck @FundsAccountingTypeCode, @InsuredGenderCodePickListDetailId, 
	@TerritoryOfIssueCodePickListDetailId, @ReinsEffDatePol, @UnderwriterRating, @CedingTreatyCode, @TreatyCode,
	@ExceptionErrorType=@ExceptionErrorType OUTPUT

IF (@FlagCode != 0)
	BEGIN
		SET @ExceptionType =  dbo.GetConstantInt('ExceptionTypeBasicCheck');
		RETURN @ExceptionType;
	END

-- Conflict Check
SELECT @TreatyCodeId = Id FROM TreatyCodes WHERE Code = @TreatyCode;

EXEC @FlagCode = PerLifeConflictCheck @InsuredName, @InsuredDateOfBirth, @MlreBenefitCode, @TransactionTypeCode, 
	@PolicyNumber, @InsuredGenderCodePickListDetailId, @TerritoryOfIssueCodePickListDetailId, 
	@TreatyCodeId, @InsuredGenderCode, @TerritoryOfIssueCode,
	@IsProceedToAggregate=@IsProceedToAggregate OUTPUT,
	@ExpectedGenderCodeId=@ExpectedGenderCodeId OUTPUT,
	@ExpectedTerritoryOfIssueCodeId=@ExpectedTerritoryOfIssueCodeId OUTPUT,
	@ExceptionErrorType=@ExceptionErrorType OUTPUT

SELECT @ExpectedGenderCode = PLD.Code
FROM PerLifeRetroGenders AS G
JOIN PickListDetails AS PLD ON G.InsuredGenderCodePickListDetailId = PLD.Id
WHERE G.Id = @ExpectedGenderCodeId

SELECT @ExpectedTerritoryOfIssueCode = PLD.Code
FROM PerLifeRetroCountries AS C
JOIN PickListDetails AS PLD ON C.TerritoryOfIssueCodePickListDetailId = PLD.Id
WHERE C.Id = @ExpectedTerritoryOfIssueCodeId

IF (@FlagCode != 0)
	BEGIN
		SET @ExceptionType =  dbo.GetConstantInt('ExceptionTypeConflictCheck');
		RETURN @ExceptionType;
	END
	
-- Duplicate Check
IF (@IsInclusion = 1) 
	BEGIN
		SET @FundsAccountingTypePickListDetailId = dbo.GetPickListDetailId(@FundsAccountingTypeCode, 24);
		SET @ReinsBasisCodePickListDetailId = dbo.GetPickListDetailId(@ReinsBasisCode, 9);
		SET @TransactionTypePickListDetailId = dbo.GetPickListDetailId(@TransactionTypeCode, 11);

		EXEC @FlagCode = PerLifeDuplicationCheck @PerLifeAggregationDetailId, @IsCheckReinsBasisCode, @InsuredName, @PolicyNumber, 
			@MlreBenefitCode, @InsuredDateOfBirth, @TransactionTypeCode, @CedingPlanCode, @TreatyCode,  
			@InsuredGenderCode, @EffectiveDate, @ReinsEffDatePol, @TreatyCodeId, @InsuredGenderCodePickListDetailId, 
			@MlreBenefitCodeId, @CedingBenefitRiskCode, @CedingBenefitTypeCode, @ReinsBasisCode,
			@FundsAccountingTypePickListDetailId, @ReinsBasisCodePickListDetailId, @TransactionTypePickListDetailId,
			@ExceptionErrorType=@ExceptionErrorType OUTPUT

		IF (@FlagCode != 0)
			BEGIN
				SET @ExceptionType =  dbo.GetConstantInt('ExceptionTypeDuplicationCheck');
				RETURN @ExceptionType;
			END
	END

SET @FlagCode = NULL;
RETURN 0;
