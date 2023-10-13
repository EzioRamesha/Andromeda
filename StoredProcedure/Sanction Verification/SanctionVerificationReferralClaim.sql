CREATE OR ALTER PROCEDURE [dbo].[SanctionVerificationReferralClaim](@SanctionIds IdTable READONLY, @SanctionVerificationId INT)

AS

DECLARE
	@ModuleId INT,
	@ReferralClaimId INT,
	@InsuredIcNumber VARCHAR(15),
	@InsuredDateOfBirth VARCHAR(64),
	@InsuredName VARCHAR(128),

	@CedingCompany VARCHAR(30),
	@TreatyCode VARCHAR(35),
	@CedingPlanCode VARCHAR(30),
	@PolicyNumber VARCHAR(150),
	@SumReins VARCHAR(16),
	@ClaimAmount VARCHAR(16),
	@PolicyCommencementDate VARCHAR(64),
	@PolicyStatusCode VARCHAR(20),
	@RiskCoverageEndDate VARCHAR(64),

	@Count INT,
	@Rule INT,
	@TotalFound INT,
	@MatchedSanctionIds IdTable,
	@ResultSanctionIds VARCHAR(MAX)

SELECT @ModuleId = Id FROM Modules Where Controller = 'ReferralClaim';
SET @TotalFound = 0;
SET @Count = 0;

DECLARE ReferralClaimCursor CURSOR FOR 
	WITH ReferralClaimGroup AS (
		SELECT *, RANK() OVER (PARTITION BY PolicyNumber, InsuredName ORDER BY Id DESC) RecordNo
		FROM ReferralClaims
	)
	SELECT 
		[Id],
		[InsuredIcNumber],
		[InsuredDateOfBirth],
		[InsuredName],
		[CedingCompany],
		[TreatyCode],
		[CedingPlanCode],
		[PolicyNumber],
		[SumReinsured],
		[ClaimRecoveryAmount],
		[DateOfCommencement]
    FROM 
        ReferralClaimGroup
    WHERE 
    	PolicyNumber IS NOT NULL AND 
    	InsuredName IS NOT NULL AND 
    	RecordNo = 1

OPEN ReferralClaimCursor

FETCH NEXT FROM ReferralClaimCursor INTO @ReferralClaimId, @InsuredIcNumber, @InsuredDateOfBirth, @InsuredName, @CedingCompany, @TreatyCode, @CedingPlanCode, 
	@PolicyNumber, @SumReins, @ClaimAmount, @PolicyCommencementDate

WHILE @@FETCH_STATUS = 0
	BEGIN
		DELETE @MatchedSanctionIds

		IF (@Count % 1000 = 0 AND @Count != 0)
			RAISERROR('Processed: %d, Found: %d', 10, 1, @Count, @TotalFound) WITH NOWAIT

		EXEC @Rule = SanctionVerificationSearch @SanctionIds, @InsuredIcNumber=@InsuredIcNumber, @InsuredName=@InsuredName, 
			@InsuredDateOfBirth=@InsuredDateOfBirth, @ResultSanctionIds=@ResultSanctionIds OUTPUT

		IF (@Rule != 0)
			BEGIN
				SET @TotalFound += 1;
				INSERT INTO @MatchedSanctionIds SELECT * FROM STRING_SPLIT(@ResultSanctionIds, ',');

				EXEC AddSanctionVerificationDetail
					@SanctionVerificationId = @SanctionVerificationId,
					@MatchedSanctionIds = @MatchedSanctionIds,
					@ModuleId = @ModuleId,
					@ObjectId = @ReferralClaimId,
					@Rule = @Rule,
					@TreatyCode = @TreatyCode,
					@CedingPlanCode = @CedingPlanCode,
					@PolicyNumber = @PolicyNumber,
					@InsuredName = @InsuredName,
					@InsuredDateOfBirth = @InsuredDateOfBirth,
					@CedingCompany = @CedingCompany,
					@InsuredIcNumber = @InsuredIcNumber,
					@SumReins = @SumReins,
					@ClaimAmount = @ClaimAmount,
					@PolicyCommencementDate = @PolicyCommencementDate
			END

		SET @Count += 1;

		FETCH NEXT FROM ReferralClaimCursor INTO @ReferralClaimId, @InsuredIcNumber, @InsuredDateOfBirth, @InsuredName, @CedingCompany, @TreatyCode, @CedingPlanCode, 
			@PolicyNumber, @SumReins, @ClaimAmount, @PolicyCommencementDate
	END

RAISERROR('Processed: %d, Found: %d', 10, 1, @Count, @TotalFound) WITH NOWAIT
RAISERROR('', 10, 1) WITH NOWAIT

CLOSE ReferralClaimCursor

DEALLOCATE ReferralClaimCursor		

RETURN @TotalFound