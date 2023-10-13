CREATE OR ALTER PROCEDURE [dbo].[PerLifeConflictCheck](
	@InsuredName VARCHAR(MAX),
	@InsuredDateOfBirth VARCHAR(MAX), 
	@MlreBenefitCode VARCHAR(MAX), 
	@TransactionTypeCode VARCHAR(MAX),  
	@PolicyNumber VARCHAR(MAX),  
	@InsuredGenderCodePickListDetailId VARCHAR(MAX),  
	@TerritoryOfIssueCodePickListDetailId VARCHAR(MAX),
	@TreatyCodeId INT,
	@InsuredGenderCode VARCHAR(MAX),
	@TerritoryOfIssueCode VARCHAR(MAX),

	@IsProceedToAggregate BIT = 0 OUTPUT,
	@ExpectedGenderCodeId INT OUTPUT,
	@ExpectedTerritoryOfIssueCodeId INT OUTPUT,
	@ExceptionErrorType INT OUTPUT)

AS

-- Variable
DECLARE
	@PerLifeDataCorrectionId INT = 0,
	@ConflictCount INT = 0,
	@FlagCode INT

	SELECT 
		@ConflictCount = COUNT(*)
	FROM 
		PerLifeAggregationDetailData AS PLADD
	LEFT JOIN
		RiDataWarehouseHistories AS RDWH
	ON
		PLADD.RiDataWarehouseHistoryId = RDWH.Id
	WHERE 
		RDWH.InsuredName = @InsuredName AND
		RDWH.InsuredDateOfBirth = @InsuredDateOfBirth AND
		RDWH.MlreBenefitCode = @MlreBenefitCode AND
		RDWH.TransactionTypeCode = @TransactionTypeCode AND
		RDWH.InsuredGenderCode != @InsuredGenderCode AND
		RDWH.TerritoryOfIssueCode != @TerritoryOfIssueCode;
		
	SELECT
		@PerLifeDataCorrectionId = Id,
		@IsProceedToAggregate = IsProceedToAggregate,
		@ExpectedGenderCodeId = PerLifeRetroGenderId,
		@ExpectedTerritoryOfIssueCodeId = PerLifeRetroCountryId
	FROM
		PerLifeDataCorrections
	WHERE
		TreatyCodeId = @TreatyCodeId AND
		InsuredName = @InsuredName AND
		InsuredDateOfBirth = @InsuredDateOfBirth AND
		PolicyNumber = @PolicyNumber AND
		InsuredGenderCodePickListDetailId = @InsuredGenderCodePickListDetailId AND
		TerritoryOfIssueCodePickListDetailId = @TerritoryOfIssueCodePickListDetailId;

	IF (@ConflictCount = 0 AND @PerLifeDataCorrectionId > 0 AND @IsProceedToAggregate = 0) OR
		(@ConflictCount >= 1 AND (@PerLifeDataCorrectionId = 0 OR (@PerLifeDataCorrectionId > 0 AND @IsProceedToAggregate = 0)))
		BEGIN
			SET @FlagCode =  dbo.GetConstantInt('FlagCodeQ2d3');
			SET @ExceptionErrorType = dbo.GetConstantInt('ExceptionErrorTypeSameLifeAssureConflictInGenderOrTerrritoryCode');
			RETURN @FlagCode;
		END

RETURN 0;