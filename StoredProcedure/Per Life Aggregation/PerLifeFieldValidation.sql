CREATE OR ALTER PROCEDURE [dbo].[PerLifeFieldValidation](
	@FundsAccountingTypeCode VARCHAR(MAX), 
	@InsuredName VARCHAR(MAX), 
	@InsuredDateOfBirth VARCHAR(Max), 
	@InsuredGenderCode VARCHAR(Max), 
	@CurrencyCode VARCHAR(Max), 
	@Aar VARCHAR(Max), 
	@NetPremium VARCHAR(Max), 
	@MinRetentionLimit VARCHAR(Max),

	@ExceptionErrorType INT OUTPUT)

AS

DECLARE @FlagCode INT;

IF @FundsAccountingTypeCode = dbo.GetConstant('FundsAccountingTypeCodeIndividual')
	BEGIN
		IF (dbo.IsNull(@InsuredName) = 1 OR dbo.IsNull(@InsuredDateOfBirth) = 1 OR
			dbo.IsNull(@InsuredGenderCode) = 1 OR dbo.IsNull(@CurrencyCode) = 1)
			BEGIN
				SET @FlagCode = dbo.GetConstantInt('FlagCodeBad');
				SET @ExceptionErrorType = dbo.GetConstantInt('ExceptionErrorTypeMissingInfo');
				RETURN @FlagCode;
			END

		IF (dbo.IsNull(@Aar) = 1)
			BEGIN
				SET @FlagCode = dbo.GetConstantInt('FlagCodeBad');
				SET @ExceptionErrorType = dbo.GetConstantInt('ExceptionErrorTypeAarNull');
				RETURN @FlagCode;
			END

		IF (dbo.IsNull(@NetPremium) = 1)
			BEGIN
				SET @FlagCode = dbo.GetConstantInt('FlagCodeBad');
				SET @ExceptionErrorType = dbo.GetConstantInt('ExceptionErrorTypeNetPremiumNull');
				RETURN @FlagCode;
			END

		IF (@Aar < '0') -- <= 0 in excel but < 0 in FSD
			BEGIN
				SET @FlagCode = dbo.GetConstantInt('FlagCodeBad');
				SET @ExceptionErrorType = dbo.GetConstantInt('ExceptionErrorTypeAarLessThanZero');
				RETURN @FlagCode;
			END

		IF (@NetPremium < '0') -- <= 0 in excel but < 0 in FSD
			BEGIN
				SET @FlagCode = dbo.GetConstantInt('FlagCodeBad');
				SET @ExceptionErrorType = dbo.GetConstantInt('ExceptionErrorTypeNetPremiumLessThanZero');
				RETURN @FlagCode;
			END
	END

IF @FundsAccountingTypeCode = dbo.GetConstant('FundsAccountingTypeCodeIndividual') OR 
	@FundsAccountingTypeCode = dbo.GetConstant('FundsAccountingTypeCodeGroup')
	BEGIN
		IF (@NetPremium = '0' AND @Aar < @MinRetentionLimit)
			BEGIN
				SET @FlagCode =  dbo.GetConstantInt('FlagCodeQ1');
				SET @ExceptionErrorType = dbo.GetConstantInt('ExceptionErrorTypeMissingNetPremiumZeroAarLessThanMinRetentionAmount');
				RETURN @FlagCode;
			END

		IF (@NetPremium = '0' AND @Aar >= @MinRetentionLimit)
			BEGIN
				SET @FlagCode =  dbo.GetConstantInt('FlagCodeQ2d1');
				SET @ExceptionErrorType = dbo.GetConstantInt('ExceptionErrorTypeMissingNetPremiumZeroAarMoreOrEqualMinRetentionAmount');
				RETURN @FlagCode;
			END
	END

RETURN 0;
