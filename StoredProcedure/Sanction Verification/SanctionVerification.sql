CREATE OR ALTER PROCEDURE [dbo].[SanctionVerification](@SanctionVerificationId INT, @Result VARCHAR(MAX) OUTPUT)

AS

SET NOCOUNT ON;

DECLARE
	@SourceId INT,
	@IsRiData BIT,
	@IsClaimRegister BIT,
	@IsReferralClaim BIT,

	@InsuredIcNumber VARCHAR(MAX),
	@InsuredDateOfBirth VARCHAR(MAX),
	@InsuredName VARCHAR(MAX),
	@FormattedName VARCHAR(MAX),

	@SanctionIds IdTable,

	@TotalFound INT,
	@TotalRiDataFound INT,
	@TotalRiDataGroupFound INT,
	@TotalClaimRegisterFound INT,
	@TotalReferralClaimFound INT,

	@StopWatch DATETIME,
	@TimeTakenRiData INT,
	@TimeTakenRiDataGroup INT,
	@TimeTakenClaimRegister INT,
	@TimeTakenReferralClaim INT

SET @TotalFound = 0;
SET @TotalRiDataFound = 0;
SET @TotalClaimRegisterFound = 0;
SET @TotalReferralClaimFound = 0;

SELECT 
	@SourceId = SourceId, 
	@IsRiData = IsRiData,
	@IsClaimRegister = IsClaimRegister,
	@IsReferralClaim = IsReferralClaim
FROM 
	SanctionVerifications
WHERE 
	Id = @SanctionVerificationId

INSERT INTO 
	@SanctionIds (Id)
SELECT 
	Sanctions.Id
FROM 
	Sanctions
JOIN 
	SanctionBatches ON SanctionBatches.Id = Sanctions.SanctionBatchId
WHERE 
	SourceId = @SourceId AND
	Status = dbo.GetConstantInt('SanctionBatchStatusSuccess');

SET @Stopwatch = GETDATE();
IF (@IsRiData = 1)
	BEGIN
		RAISERROR('Process RI Data', 10, 1) WITH NOWAIT
		EXEC @TotalRiDataFound = SanctionVerificationRiData @SanctionIds = @SanctionIds, @SanctionVerificationId = @SanctionVerificationId
	END
SET @TimeTakenRiData = DateDiff(ms, @Stopwatch, GETDATE());

SET @Stopwatch = GETDATE();
IF (@IsRiData = 1)
	BEGIN
		RAISERROR('Process RI Data(Group)', 10, 1) WITH NOWAIT
		EXEC @TotalRiDataGroupFound = SanctionVerificationRiDataGroup @SanctionIds = @SanctionIds, @SanctionVerificationId = @SanctionVerificationId
	END
SET @TimeTakenRiDataGroup = DateDiff(ms, @Stopwatch, GETDATE());

SET @Stopwatch = GETDATE();
IF (@IsClaimRegister = 1)
	BEGIN
		RAISERROR('Process Claim Register', 10, 1) WITH NOWAIT
		EXEC @TotalClaimRegisterFound = SanctionVerificationClaimRegister @SanctionIds = @SanctionIds, @SanctionVerificationId = @SanctionVerificationId
	END
SET @TimeTakenClaimRegister = DateDiff(ms, @Stopwatch, GETDATE());

SET @Stopwatch = GETDATE();
IF (@IsReferralClaim = 1)
	BEGIN
		RAISERROR('Process Referral Claim', 10, 1) WITH NOWAIT
		EXEC @TotalReferralClaimFound = SanctionVerificationReferralClaim @SanctionIds = @SanctionIds, @SanctionVerificationId = @SanctionVerificationId
	END
SET @TimeTakenReferralClaim = DateDiff(ms, @Stopwatch, GETDATE());

DELETE @SanctionIds

SET @TotalFound = @TotalRiDataFound + @TotalClaimRegisterFound + @TotalReferralClaimFound;
SET @Result = '{"Total":'+ CAST(@TotalFound as VARCHAR(10)) + 
      ', "RI Data": ' + CAST(@TotalRiDataFound as VARCHAR(10)) + 
      ', "RI Data Time Taken": ' + CAST(@TimeTakenRiData as VARCHAR(128)) + 
      ', "Claim Register": ' + CAST(@TotalClaimRegisterFound as VARCHAR(10)) +
      ', "Claim Register Time Taken": ' + CAST(@TimeTakenClaimRegister as VARCHAR(128)) + 
      ', "Referral Claim": ' + CAST(@TotalReferralClaimFound as VARCHAR(10)) + 
      ', "Referral Claim Time Taken": ' + CAST(@TimeTakenReferralClaim as VARCHAR(128)) + '}';