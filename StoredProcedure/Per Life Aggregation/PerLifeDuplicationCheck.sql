CREATE OR ALTER PROCEDURE [dbo].[PerLifeDuplicationCheck](
	@PerLifeAggregationDetailId INT,
	@IsCheckReinsBasisCode BIT,
	@InsuredName VARCHAR(MAX),
	@PolicyNumber VARCHAR(MAX),
	@MlreBenefitCode VARCHAR(MAX), 
	@InsuredDateOfBirth VARCHAR(MAX), 
	@TransactionTypeCode VARCHAR(MAX),  
	@CedingPlanCode VARCHAR(MAX),  
	@TreatyCode VARCHAR(MAX),  
	@InsuredGenderCode VARCHAR(MAX),
	@EffectiveDate VARCHAR(MAX),
	@ReinsEffDatePol VARCHAR(MAX),
	@TreatyCodeId INT,
	@InsuredGenderCodePickListDetailId INT,
	@MlreBenefitCodeId INT,
	@CedingBenefitRiskCode VARCHAR(MAX),
	@CedingBenefitTypeCode VARCHAR(MAX),
	@ReinsBasisCode VARCHAR(MAX),
	@FundsAccountingTypePickListDetailId INT,
	@ReinsBasisCodePickListDetailId INT,
	@TransactionTypePickListDetailId INT,
	@ExceptionErrorType INT OUTPUT)

AS

-- Variable
DECLARE
	@PerLifeDataCorrectionId INT = 0,
	@DuplicationCount INT = 0,
	@FlagCode INT

	IF (@IsCheckReinsBasisCode = 1)
		BEGIN
			SELECT 
				@DuplicationCount = COUNT(*)
			FROM 
				PerLifeAggregationDetailData AS PLADD
			LEFT JOIN
				RiDataWarehouseHistories AS RDWH
			ON
				PLADD.RiDataWarehouseHistoryId = RDWH.Id
			WHERE 
				PLADD.PerLifeAggregationDetailTreatyId IN
				(SELECT Id FROM PerLifeAggregationDetailTreaties WHERE PerLifeAggregationDetailId = @PerLifeAggregationDetailId) AND
				RDWH.InsuredName = @InsuredName AND
				RDWH.PolicyNumber = @PolicyNumber AND
				RDWH.MlreBenefitCode = @MlreBenefitCode AND
				RDWH.InsuredDateOfBirth = @InsuredDateOfBirth AND
				RDWH.TransactionTypeCode = @TransactionTypeCode AND
				RDWH.CedingPlanCode = @CedingPlanCode AND
				RDWH.TreatyCode = @TreatyCode AND
				RDWH.InsuredGenderCode = @InsuredGenderCode AND
				ISNULL(RDWH.EffectiveDate, '') = ISNULL(@EffectiveDate, '') AND
				RDWH.ReinsEffDatePol = @ReinsEffDatePol AND
				RDWH.ReinsBasisCode = @ReinsBasisCode
		END
	ELSE
		BEGIN
			SELECT 
				@DuplicationCount = COUNT(*)
			FROM 
				PerLifeAggregationDetailData AS PLADD
			LEFT JOIN
				RiDataWarehouseHistories AS RDWH
			ON
				PLADD.RiDataWarehouseHistoryId = RDWH.Id
			WHERE 
				PLADD.PerLifeAggregationDetailTreatyId IN
				(SELECT Id FROM PerLifeAggregationDetailTreaties WHERE PerLifeAggregationDetailId = @PerLifeAggregationDetailId) AND
				RDWH.InsuredName = @InsuredName AND
				RDWH.PolicyNumber = @PolicyNumber AND
				RDWH.MlreBenefitCode = @MlreBenefitCode AND
				RDWH.InsuredDateOfBirth = @InsuredDateOfBirth AND
				RDWH.TransactionTypeCode = @TransactionTypeCode AND
				RDWH.CedingPlanCode = @CedingPlanCode AND
				RDWH.TreatyCode = @TreatyCode AND
				RDWH.InsuredGenderCode = @InsuredGenderCode AND
				ISNULL(RDWH.EffectiveDate, '') = ISNULL(@EffectiveDate, '') AND
				RDWH.ReinsEffDatePol = @ReinsEffDatePol
		END

			

	IF @DuplicationCount > 1
		BEGIN
			IF NOT EXISTS (
				SELECT 1 FROM 
					ValidDuplicationLists 
				WHERE 
					TreatyCodeId = @TreatyCodeId AND
					CedantPlanCode = @CedingPlanCode AND
					InsuredName = @InsuredName AND
					InsuredDateOfBirth = @InsuredDateOfBirth AND
					InsuredGenderCodePickListDetailId = @InsuredGenderCodePickListDetailId AND
					MLReBenefitCodeId = @MlreBenefitCodeId AND
					CedingBenefitRiskCode = @CedingBenefitRiskCode AND
					CedingBenefitTypeCode = @CedingBenefitTypeCode AND
					ReinsuranceEffectiveDate = @ReinsEffDatePol AND
					FundsAccountingTypePickListDetailId = @FundsAccountingTypePickListDetailId AND
					ReinsBasisCodePickListDetailId = @ReinsBasisCodePickListDetailId AND
					TransactionTypePickListDetailId = @TransactionTypePickListDetailId
			)
			BEGIN
				SET @FlagCode =  dbo.GetConstantInt('FlagCodeQ2dA');
				SET @ExceptionErrorType = dbo.GetConstantInt('ExceptionErrorTypeDuplicationRecord');
				RETURN @FlagCode;
			END
		END

RETURN 0;
