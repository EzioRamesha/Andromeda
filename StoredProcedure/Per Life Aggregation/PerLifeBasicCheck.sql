CREATE OR ALTER PROCEDURE [dbo].[PerLifeBasicCheck](
	@FundsAccountingTypeCode VARCHAR(MAX),
	@InsuredGenderCodePickListDetailId INT, 
	@TerritoryOfIssueCodePickListDetailId INT, 
	@ReinsEffDatePol VARCHAR(Max), 
	@UnderwriterRating VARCHAR(Max), 
	@CedingTreatyCode VARCHAR(Max), 
	@TreatyCode VARCHAR(Max),
	-- @PolicyNumber VARCHAR(Max), 
	-- @InsuredDateOfBirth VARCHAR(Max),

	@ExceptionErrorType INT OUTPUT)

AS

DECLARE @FlagCode INT;

IF @FundsAccountingTypeCode = dbo.GetConstant('FundsAccountingTypeCodeIndividual')
	BEGIN
		IF (dbo.IsNull(@InsuredGenderCodePickListDetailId) = 1)
			BEGIN
				SET @FlagCode = dbo.GetConstantInt('FlagCodeQ2d2');
				SET @ExceptionErrorType = dbo.GetConstantInt('ExceptionErrorTypeGenderNullOrNotPermitted');
				RETURN @FlagCode;
			END

		IF NOT EXISTS (SELECT * FROM PerLifeRetroGenders WHERE InsuredGenderCodePickListDetailId = @InsuredGenderCodePickListDetailId)
			BEGIN
				SET @FlagCode = dbo.GetConstantInt('FlagCodeQ2d4');
				SET @ExceptionErrorType = dbo.GetConstantInt('ExceptionErrorTypeGenderNullOrNotPermitted');
				RETURN @FlagCode;
			END
	END

IF @FundsAccountingTypeCode = dbo.GetConstant('FundsAccountingTypeCodeIndividual') OR 
	@FundsAccountingTypeCode = dbo.GetConstant('FundsAccountingTypeCodeGroup')
	BEGIN
		IF (dbo.IsNull(@TerritoryOfIssueCodePickListDetailId) = 1)
			BEGIN
				SET @FlagCode = dbo.GetConstantInt('FlagCodeQ2d2');
				SET @ExceptionErrorType = dbo.GetConstantInt('ExceptionErrorTypeTerritoryCodeNullOrNotPermitted');
				RETURN @FlagCode;
			END

		IF (dbo.IsNull(@ReinsEffDatePol) = 1)
			BEGIN
				SET @FlagCode = dbo.GetConstantInt('FlagCodeQ2d2');
				SET @ExceptionErrorType = dbo.GetConstantInt('ExceptionErrorTypeReinsEffDateNullOrGreaterThanSystem');
				RETURN @FlagCode;
			END

		IF (dbo.IsNull(@UnderwriterRating) = 1)
			BEGIN
				SET @FlagCode = dbo.GetConstantInt('FlagCodeQ2d2');
				SET @ExceptionErrorType = dbo.GetConstantInt('ExceptionErrorTypeUwNullOrLessThanZero');
				RETURN @FlagCode;
			END

		-- IF (dbo.IsNull(@CedingTreatyCode) = 1)
		-- 	BEGIN
		-- 		SET @FlagCode = dbo.GetConstantInt('FlagCodeQ2d2');
		-- 		SET @ExceptionErrorType = dbo.GetConstantInt('ExceptionErrorTypeCedingTreatyCodeNull');
		-- 		RETURN @FlagCode;
		-- 	END

		IF (dbo.IsNull(@TreatyCode) = 1)
			BEGIN
				SET @FlagCode = dbo.GetConstantInt('FlagCodeQ2d2');
				SET @ExceptionErrorType = dbo.GetConstantInt('ExceptionErrorTypeTreatyCodeNull');
				RETURN @FlagCode;
			END

		IF NOT EXISTS (SELECT * FROM PerLifeRetroCountries WHERE TerritoryOfIssueCodePickListDetailId = @TerritoryOfIssueCodePickListDetailId)
		BEGIN
			SET @FlagCode = dbo.GetConstantInt('FlagCodeQ2d4');
			SET @ExceptionErrorType = dbo.GetConstantInt('ExceptionErrorTypeTerritoryCodeNullOrNotPermitted');
			RETURN @FlagCode;
		END

		IF (@ReinsEffDatePol > CONVERT(VARCHAR(8), GETDATE(), 112))
		BEGIN
			SET @FlagCode = dbo.GetConstantInt('FlagCodeQ2d4');
			SET @ExceptionErrorType = dbo.GetConstantInt('ExceptionErrorTypeReinsEffDateNullOrGreaterThanSystem');
			RETURN @FlagCode;
		END

		IF (@UnderwriterRating < '100') -- < 0 in excel but < 100 in FSD
		BEGIN
			SET @FlagCode = dbo.GetConstantInt('FlagCodeQ2d4');
			SET @ExceptionErrorType = dbo.GetConstantInt('ExceptionErrorTypeUwNullOrLessThanZero');
			RETURN @FlagCode;
		END
	END

RETURN 0;
