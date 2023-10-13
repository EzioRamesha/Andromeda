CREATE OR ALTER PROCEDURE [dbo].[RiDataMigrateWarehouse](@RiDataBatchId INT, @Result VARCHAR(MAX) OUTPUT) 

AS

SET NOCOUNT ON;

DECLARE 
	@PolicyNumber VARCHAR(MAX), 
	@CedingPlanCode VARCHAR(MAX), 
	@RiskPeriodMonth VARCHAR(MAX),
	@RiskPeriodYear VARCHAR(MAX),
	@MlreBenefitCode VARCHAR(MAX), 
	@TreatyCode VARCHAR(MAX),
	@RiderNumber VARCHAR(MAX),
	@InsuredName VARCHAR(MAX),
	@CedingBenefitTypeCode VARCHAR(MAX),
	@CedingBenefitRiskCode VARCHAR(MAX),
	@CedingPlanCode2 VARCHAR(MAX),
	@CessionCode VARCHAR(MAX),

	@RiDataWarehouseId VARCHAR(MAX),
	@RiDataId VARCHAR(MAX),
	@PolicyStatusCode VARCHAR(MAX),
	@HasError BIT,
	@Total INT,
	@TotalSuccess INT,
	@TotalFailed INT,
	@EndingPolicyStatusId INT

SET @Total =  0;
SET @TotalSuccess =  0;
SET @TotalFailed =  0;

DECLARE RiDataCursor CURSOR FOR 
	SELECT 
		Id,
		PolicyNumber,
		CedingPlanCode,
		RiskPeriodMonth,
		RiskPeriodYear,
		MlreBenefitCode,
		TreatyCode,
		RiderNumber,
		PolicyStatusCode,
		InsuredName,
		CedingBenefitTypeCode,
		CedingBenefitRiskCode,
		CedingPlanCode2,
		CessionCode
	FROM 
		RiData
	WHERE
		RiDataBatchId = @RiDataBatchId AND
		(ProcessWarehouseStatus = dbo.GetConstantInt('ProcessWarehouseStatusPending') OR ProcessWarehouseStatus = dbo.GetConstantInt('ProcessWarehouseStatusFailed'));

OPEN RiDataCursor;

FETCH NEXT FROM RiDataCursor INTO 
	@RiDataId, @PolicyNumber, @CedingPlanCode, @RiskPeriodMonth, @RiskPeriodYear, @MlreBenefitCode, @TreatyCode, @RiderNumber, @PolicyStatusCode, @InsuredName, @CedingBenefitTypeCode, @CedingBenefitRiskCode, @CedingPlanCode2, @CessionCode;

WHILE @@FETCH_STATUS = 0
	BEGIN
		SET @RiDataWarehouseId = NULL;

		SELECT 
			@RiDataWarehouseId = Id 
		FROM 
			RiDataWarehouse
		WHERE 
			PolicyNumber = @PolicyNumber AND
			ISNULL(CedingPlanCode, '') = ISNULL(@CedingPlanCode, '') AND
			RiskPeriodMonth = @RiskPeriodMonth AND
			RiskPeriodYear = @RiskPeriodYear AND
			MlreBenefitCode = @MlreBenefitCode AND
			TreatyCode = @TreatyCode AND
			ISNULL(RiderNumber, '') = ISNULL(@RiderNumber, '') AND
			ISNULL(InsuredName, '') = ISNULL(@InsuredName, '') AND
			ISNULL(CedingBenefitTypeCode, '') = ISNULL(@CedingBenefitTypeCode, '') AND
			ISNULL(CedingBenefitRiskCode, '') = ISNULL(@CedingBenefitRiskCode, '') AND
			ISNULL(CedingPlanCode2, '') = ISNULL(@CedingPlanCode2, '') AND
			ISNULL(CessionCode, '') = ISNULL(@CessionCode, '');

		IF @RiDataWarehouseId IS NULL
			BEGIN
				EXEC @RiDataWarehouseId = RiDataWarehouseInsert @RiDataId=@RiDataId, @HasError=@HasError OUTPUT;
				IF (@HasError = 1)
					BEGIN
						SET @TotalFailed = @TotalFailed + 1;
					END
				ELSE 
					BEGIN
						SET @TotalSuccess = @TotalSuccess + 1;
					END
			END
		ELSE
			BEGIN
				EXEC RiDataWarehouseUpdate @RiDataId=@RidataId, @RiDataWarehouseId=@RiDataWarehouseId;
				SET @TotalSuccess = @TotalSuccess + 1;
			END;

		IF @RiDataWarehouseId IS NOT NULL AND @RiDataWarehouseId != 0
			BEGIN
				UPDATE 
					RiDataWarehouse 
				SET 
					Quarter = dbo.FormatQuarter(RiskPeriodYear, RiskPeriodMonth)
				WHERE 
					Id = @RiDataWarehouseId;

				UPDATE RiData SET ProcessWarehouseStatus = dbo.GetConstantInt('ProcessWarehouseStatusSuccess') WHERE Id = @RiDataId;

				SET @EndingPolicyStatusId = dbo.GetPickListDetailId(@PolicyStatusCode, 18);
				UPDATE 
					RiDataWarehouse
				SET 
					EndingPolicyStatus = @EndingPolicyStatusId,
					LastUpdatedDate = Convert(date, GETDATE()),
					UpdatedAt = GETDATE(),
					UpdatedById = 1
				WHERE 
					PolicyNumber = @PolicyNumber AND
					ISNULL(CedingPlanCode, '') = ISNULL(@CedingPlanCode, '') AND
					MlreBenefitCode = @MlreBenefitCode AND
					Id != @RiDataWarehouseId
			END
		ELSE
			BEGIN
				UPDATE RiData SET ProcessWarehouseStatus = dbo.GetConstantInt('ProcessWarehouseStatusFailed') WHERE Id = @RiDataId;
			END
	
		SET @Total = @Total + 1;

		FETCH NEXT FROM RiDataCursor INTO 
			@RiDataId, @PolicyNumber, @CedingPlanCode, @RiskPeriodMonth, @RiskPeriodYear, @MlreBenefitCode, @TreatyCode, @RiderNumber, @PolicyStatusCode, @InsuredName, @CedingBenefitTypeCode, @CedingBenefitRiskCode, @CedingPlanCode2, @CessionCode;

	END

CLOSE RiDataCursor;

DEALLOCATE RiDataCursor;

SET @Result = '{"Total":'+ CAST(@Total as varchar(10)) + 
			', "Success": ' + CAST(@TotalSuccess as varchar(10)) + 
			', "Failed": ' + CAST(@TotalFailed as varchar(10)) + '}';
