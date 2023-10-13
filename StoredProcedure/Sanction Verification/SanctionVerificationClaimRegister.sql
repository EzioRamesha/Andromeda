CREATE OR ALTER PROCEDURE [dbo].[SanctionVerificationClaimRegister](@SanctionIds IdTable READONLY, @SanctionVerificationId INT)

AS

DECLARE
	@ModuleId INT,
	@ClaimRegisterId INT,
	@FundsAccountingTypeCode VARCHAR(30),
	@InsuredDateOfBirth VARCHAR(64),
	@InsuredName VARCHAR(128),

	@CedingCompany VARCHAR(30),
	@TreatyCode VARCHAR(35),
	@CedingPlanCode VARCHAR(30),
	@PolicyNumber VARCHAR(150),
	@SoaQuarter VARCHAR(30),
	@SumReins VARCHAR(16),
	@ClaimAmount VARCHAR(16),
	@BatchId INT,
	@LineOfBusiness VARCHAR(5),
	@PolicyCommencementDate VARCHAR(64),
	@PolicyStatusCode VARCHAR(20),
	@RiskCoverageEndDate VARCHAR(64),
	@GrossPremium VARCHAR(16),
	@RiDataWarehouseId INT,
	@Status INT,

	@Count INT,
	@Rule INT,
	@TotalFound INT,
	@MatchedSanctionIds IdTable,
	@ResultSanctionIds VARCHAR(MAX)
	-- @CurrentPolicyNumber VARCHAR(MAX),
	-- @CurrentInsuredName VARCHAR(MAX)

DECLARE @SplitNames TABLE(Name VARCHAR(MAX))

SELECT @ModuleId = Id FROM Modules Where Controller = 'ClaimRegister';
SET @TotalFound = 0;
SET @Count = 0;

DECLARE ClaimRegisterCursor CURSOR FOR 
	WITH ClaimRegisterGroup AS (
		SELECT *, RANK() OVER (PARTITION BY PolicyNumber, InsuredName ORDER BY Id Desc, SoaQuarter DESC) RecordNo
		FROM ClaimRegister
	)
	SELECT 
		[Id],
		[FundsAccountingTypeCode],
		[InsuredDateOfBirth],
		[InsuredName],
		[CedingCompany],
		[TreatyCode],
		[CedingPlanCode],
		[PolicyNumber],
		[SoaQuarter],
		[AarPayable],
		[ClaimRecoveryAmt],
		[ClaimDataBatchId],
		[RiDataWarehouseId],
		[ClaimStatus],
		[SignOffDate]
    FROM 
        ClaimRegisterGroup
    WHERE 
    	PolicyNumber IS NOT NULL AND 
    	InsuredName IS NOT NULL AND 
    	RecordNo = 1

-- DECLARE ClaimRegisterCursor CURSOR FOR 
-- 	SELECT 
-- 		[Id],
-- 		[InsuredDateOfBirth],
-- 		[InsuredName],
-- 		[CedingCompany],
-- 		[TreatyCode],
-- 		[CedingPlanCode],
-- 		[PolicyNumber],
-- 		[SoaQuarter],
-- 		[Layer1SumRein],
-- 		[ClaimRecoveryAmt],
-- 		[ClaimDataBatchId],
-- 		[RiDataWarehouseId],
-- 		[ClaimStatus],
-- 		[SignOffDate]
--     FROM 
--         ClaimRegister
--     WHERE 
--     	PolicyNumber IS NOT NULL AND 
--     	InsuredName IS NOT NULL
--     ORDER BY 
--     	PolicyNumber, InsuredName, SoaQuarter DESC

OPEN ClaimRegisterCursor

FETCH NEXT FROM ClaimRegisterCursor INTO @ClaimRegisterId, @FundsAccountingTypeCode, @InsuredDateOfBirth, @InsuredName, @CedingCompany, 
	@TreatyCode, @CedingPlanCode, @PolicyNumber, @SoaQuarter, @SumReins, @ClaimAmount, @BatchId, @RiDataWarehouseId, @Status, 
	@RiskCoverageEndDate

WHILE @@FETCH_STATUS = 0
	BEGIN
		DELETE @MatchedSanctionIds

		IF (@Count % 1000 = 0 AND @Count != 0)
			RAISERROR('Processed: %d, Found: %d', 10, 1, @Count, @TotalFound) WITH NOWAIT

		-- IF (@CurrentPolicyNumber = @PolicyNumber AND @CurrentInsuredName = @InsuredName)
		-- 	BEGIN
		-- 		FETCH NEXT FROM ClaimRegisterCursor INTO @ClaimRegisterId, @InsuredDateOfBirth, @InsuredName, @CedingCompany, @TreatyCode, @CedingPlanCode, 
		-- 			@PolicyNumber, @SoaQuarter, @SumReins, @ClaimAmount, @BatchId, @RiDataWarehouseId, @Status, @RiskCoverageEndDate
		-- 		CONTINUE;
		-- 	END

		-- SET @CurrentPolicyNumber = @PolicyNumber;
		-- SET @CurrentInsuredName = @InsuredName;

		EXEC @Rule = SanctionVerificationSearch @SanctionIds, @Category=@FundsAccountingTypeCode, @InsuredName=@InsuredName, 
			@InsuredDateOfBirth=@InsuredDateOfBirth, @ResultSanctionIds=@ResultSanctionIds OUTPUT

		IF (@Rule != 0)
			BEGIN
				SET @TotalFound += 1;

				INSERT INTO @MatchedSanctionIds SELECT * FROM STRING_SPLIT(@ResultSanctionIds, ',');

				IF (@RiDataWarehouseId IS NOT NULL)
					BEGIN
						SELECT
							@LineOfBusiness = LineOfBusiness,
							@PolicyCommencementDate = IssueDatePol,
							@GrossPremium = GrossPremium
						FROM
							RiDataWarehouse
						WHERE
							Id = @RiDataWarehouseId;
					END

				IF (@Status = dbo.GetConstantInt('ClaimStatusApproved')) 
					BEGIN
						SET @PolicyStatusCode = 'CLOSED';
					END
				ELSE 
					BEGIN
						SET @RiskCoverageEndDate = NULL;
						SET @PolicyStatusCode = 'ACTIVE';
					END

				EXEC AddSanctionVerificationDetail     
					@SanctionVerificationId = @SanctionVerificationId,
					@MatchedSanctionIds = @MatchedSanctionIds,
					@ModuleId = @ModuleId,
					@ObjectId = @ClaimRegisterId,
					@BatchId = @BatchId,
					@Rule = @Rule,
					@TreatyCode = @TreatyCode,
					@CedingPlanCode = @CedingPlanCode,
					@PolicyNumber = @PolicyNumber,
					@InsuredName = @InsuredName,
					@InsuredDateOfBirth = @InsuredDateOfBirth,
					@CedingCompany = @CedingCompany,
					@SoaQuarter = @SoaQuarter,
					@SumReins = @SumReins,
					@ClaimAmount = @ClaimAmount,
					@Category = @FundsAccountingTypeCode,
					@LineOfBusiness = @LineOfBusiness,
					@PolicyCommencementDate = @PolicyCommencementDate,
					@PolicyStatusCode = @PolicyStatusCode,
					@RiskCoverageEndDate = @RiskCoverageEndDate,
					@GrossPremium = @GrossPremium
			END

		SET @Count += 1;

		FETCH NEXT FROM ClaimRegisterCursor INTO @ClaimRegisterId, @FundsAccountingTypeCode, @InsuredDateOfBirth, @InsuredName, @CedingCompany, 
			@TreatyCode, @CedingPlanCode, @PolicyNumber, @SoaQuarter, @SumReins, @ClaimAmount, @BatchId, @RiDataWarehouseId, @Status, 
			@RiskCoverageEndDate
	END

RAISERROR('Processed: %d, Found: %d', 10, 1, @Count, @TotalFound) WITH NOWAIT
RAISERROR('', 10, 1) WITH NOWAIT

CLOSE ClaimRegisterCursor

DEALLOCATE ClaimRegisterCursor	

RETURN @TotalFound