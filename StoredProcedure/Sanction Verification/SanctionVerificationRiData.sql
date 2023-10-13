CREATE OR ALTER PROCEDURE [dbo].[SanctionVerificationRiData](@SanctionIds IdTable READONLY, @SanctionVerificationId INT)

AS

DECLARE
	@ModuleId INT,
	@RiDataId INT,
	@FundsAccountingTypeCode VARCHAR(30),
	@InsuredIcNumber VARCHAR(15),
	@InsuredDateOfBirth VARCHAR(64),
	@InsuredName VARCHAR(128),

	@CedingCompany VARCHAR(30),
	@TreatyCode VARCHAR(35),
	@CedingPlanCode VARCHAR(30),
	@PolicyNumber VARCHAR(150),
	@SoaQuarter VARCHAR(64),
	@BatchId INT,
	@LineOfBusiness VARCHAR(5),
	@PolicyCommencementDate VARCHAR(64),
	@PolicyStatusCode VARCHAR(20),
	@RiskCoverageEndDate VARCHAR(64),
	@GrossPremium VARCHAR(16),

	@Count INT,
	@Rule INT,
	@TotalFound INT,
	@MatchedSanctionIds IdTable,
	@ResultSanctionIds VARCHAR(MAX)

SELECT @ModuleId = Id FROM Modules Where Controller = 'RiData';
SET @TotalFound = 0;
SET @Count = 0;

DECLARE RiDataCursor CURSOR FOR
	WITH RiDataGroup AS (
		SELECT RiData.*, Cedants.Code, RANK() OVER (PARTITION BY PolicyNumber, InsuredName ORDER BY RiData.Id DESC, ReportPeriodYear DESC, ReportPeriodMonth DESC) RecordNo
		FROM RiData
		LEFT OUTER JOIN RiDataBatches ON RiDataBatches.Id = RiData.RiDataBatchId
    	LEFT OUTER JOIN Cedants ON Cedants.Id = RiDataBatches.CedantId
		WHERE FundsAccountingTypeCode IS NULL OR FundsAccountingTypeCode = 'INDIVIDUAL'
	) 
	SELECT 
		Id,
		[FundsAccountingTypeCode],
		[InsuredNewIcNumber],
		[InsuredDateOfBirth],
		[InsuredName],
		Code,
		[TreatyCode],
		[CedingPlanCode],
		[PolicyNumber],
		dbo.FormatQuarter(ReportPeriodYear, ReportPeriodMonth) AS SoaQuarter,
		[RiDataBatchId],
		[LineOfBusiness],
		[IssueDatePol],
		[PolicyStatusCode],
		[RiskPeriodEndDate],
		[GrossPremium]
    FROM 
        RiDataGroup
    WHERE 
    	PolicyNumber IS NOT NULL AND 
    	InsuredName IS NOT NULL AND 
    	RecordNo = 1
    

OPEN RiDataCursor

FETCH NEXT FROM RiDataCursor INTO @RiDataId, @FundsAccountingTypeCode, @InsuredIcNumber, @InsuredDateOfBirth, @InsuredName, @CedingCompany, 
	@TreatyCode, @CedingPlanCode, @PolicyNumber, @SoaQuarter, @BatchId, @LineOfBusiness, @PolicyCommencementDate, @PolicyStatusCode, 
	@RiskCoverageEndDate, @GrossPremium

WHILE @@FETCH_STATUS = 0
	BEGIN
		DELETE @MatchedSanctionIds
		
		IF (@Count % 1000 = 0 AND @Count != 0)
			RAISERROR('Processed: %d, Found: %d', 10, 1, @Count, @TotalFound) WITH NOWAIT

		EXEC @Rule = SanctionVerificationSearch @SanctionIds, @Category=@FundsAccountingTypeCode, @InsuredIcNumber=@InsuredIcNumber, 
			@InsuredName=@InsuredName, @InsuredDateOfBirth=@InsuredDateOfBirth, @ResultSanctionIds=@ResultSanctionIds OUTPUT

		IF (@Rule != 0)
			BEGIN
				SET @TotalFound += 1;
				INSERT INTO @MatchedSanctionIds SELECT * FROM STRING_SPLIT(@ResultSanctionIds, ',');

				EXEC AddSanctionVerificationDetail
					@SanctionVerificationId = @SanctionVerificationId,
					@MatchedSanctionIds = @MatchedSanctionIds,
					@ModuleId = @ModuleId,
					@ObjectId = @RiDataId,
					@BatchId = @BatchId,
					@Rule = @Rule,
					@TreatyCode = @TreatyCode,
					@CedingPlanCode = @CedingPlanCode,
					@PolicyNumber = @PolicyNumber,
					@InsuredName = @InsuredName,
					@InsuredDateOfBirth = @InsuredDateOfBirth,
					@CedingCompany = @CedingCompany,
					@InsuredIcNumber = @InsuredIcNumber,
					@Category = @FundsAccountingTypeCode,
					@SoaQuarter = @SoaQuarter,
					@LineOfBusiness = @LineOfBusiness,
					@PolicyCommencementDate = @PolicyCommencementDate,
					@PolicyStatusCode = @PolicyStatusCode,
					@RiskCoverageEndDate = @RiskCoverageEndDate,
					@GrossPremium = @GrossPremium
			END

		SET @Count += 1;

		FETCH NEXT FROM RiDataCursor INTO @RiDataId, @FundsAccountingTypeCode, @InsuredIcNumber, @InsuredDateOfBirth, @InsuredName, @CedingCompany, 
			@TreatyCode, @CedingPlanCode, @PolicyNumber, @SoaQuarter, @BatchId, @LineOfBusiness, @PolicyCommencementDate, @PolicyStatusCode, 
			@RiskCoverageEndDate, @GrossPremium
	END

RAISERROR('Processed: %d, Found: %d', 10, 1, @Count, @TotalFound) WITH NOWAIT
RAISERROR('', 10, 1) WITH NOWAIT

CLOSE RiDataCursor

DEALLOCATE RiDataCursor		

RETURN @TotalFound