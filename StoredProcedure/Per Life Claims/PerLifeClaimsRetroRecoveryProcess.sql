CREATE OR ALTER PROCEDURE [dbo].[PerLifeClaimsRetroRecoveryProcess](@PerLifeClaimId INT, @Result VARCHAR(MAX) OUTPUT)

AS

SET NOCOUNT ON;

--Constants
DECLARE
	@StatusReported INT = 1,
	@StatusProcessing INT = 2,
	@StatusSuccess INT = 3,
	@StatusFailed INT = 4,
	@StatusPendingClarification INT = 5,
	@StatusClosed INT = 6,
	@StatusSuspectedDuplication INT = 7,
	@StatusRegistered INT = 8,
	@StatusPostUnderwritingReview INT = 9,
	@StatusApprovalByLimit INT = 10,
	@StatusPendingCeoApproval INT = 11,
	@StatusApprovedByCeo INT = 12,
	@StatusApproved INT = 13,
	@StatusApprovedReferralClaim INT = 14,
	@StatusDeclinedReferralClaim INT = 15,
	@StatusClosedReferralClaim INT = 16,
	@StatusDeclinedByClaim INT = 17,
	@StatusPendingCeoSignOff INT = 18,      
	@StatusDeclined INT = 19

DECLARE
	@OffsetStatusPending INT = 1,
	@OffsetStatusOffset INT = 2,
	@OffsetStatusPendingInvoicing INT = 3,
	@OffsetStatusNotRequired INT = 4

DECLARE
	@ClaimCategoryPaidClaim INT = 1,
	@ClaimCategoryPendingClaim INT = 2,
	@ClaimCategoryReversed INT = 3,
	@ClaimCategoryRetainClaim INT = 4

--Variables
DECLARE
	@Id INT,
	@PerLifeAggregationDetailDataId INT,
	@ClaimRegisterHistoryId INT,
	@ClaimStatus INT,
	@OffsetStatus INT,
	@ClaimTransactionType NVARCHAR(255),
	@IsExcludePerformClaimRecovery INT,
	@ClaimCategory INT,
	@PerLifeAggregationMonthlyDataId INT,
	@RetroIndicator BIT,
	@RetroAmount FLOAT,
	@AAR FLOAT,
	@RetentionLimit FLOAT,
	@MlreShare FLOAT,
	--@RetroClaimRecoveryAmount FLOAT,
	@ChildResult VARCHAR(MAX)

DECLARE PerLifeClaimDataCursor CURSOR FOR 
	SELECT
		PLCD.Id,
		PLCD.PerLifeAggregationDetailDataId,
		CR.ClaimStatus,
		CR.OffsetStatus,
		CR.ClaimTransactionType,
		PLCD.ClaimRegisterHistoryId
	FROM 
		PerLifeClaimData AS PLCD
	JOIN
		ClaimRegisterHistories AS CRH
		ON CRH.Id = PLCD.ClaimRegisterHistoryId
	JOIN
		ClaimRegister AS CR
		ON CR.Id = CRH.ClaimRegisterId
	WHERE 
		PLCD.PerLifeClaimId = @PerLifeClaimId
	GROUP BY
		PLCD.Id,
		PLCD.PerLifeAggregationDetailDataId,
		CR.ClaimStatus,
		CR.OffsetStatus,
		CR.ClaimTransactionType,
		PLCD.ClaimRegisterHistoryId

OPEN PerLifeClaimDataCursor

FETCH NEXT FROM PerLifeClaimDataCursor INTO @Id, @PerLifeAggregationDetailDataId, @ClaimStatus, @OffsetStatus, @ClaimTransactionType, @ClaimRegisterHistoryId

WHILE @@FETCH_STATUS = 0
BEGIN
	SET @IsExcludePerformClaimRecovery = 1
	SET @PerLifeAggregationMonthlyDataId = 0
	SET @ClaimCategory = @ClaimCategoryRetainClaim

	IF ISNULL(@PerLifeAggregationDetailDataId,0) <> 0
	BEGIN
		SET @PerLifeAggregationMonthlyDataId = ISNULL((SELECT TOP 1 Id FROM PerLifeAggregationMonthlyData
			WHERE PerLifeAggregationDetailDataId = @PerLifeAggregationDetailDataId
			ORDER BY RiskMonth DESC),0)

		IF @PerLifeAggregationMonthlyDataId > 0
		BEGIN
			SELECT
				@RetroIndicator = PLAMD.RetroIndicator,
				@RetroAmount = PLAMD.RetroAmount,
				@AAR = PLAMD.SumOfAar,
				@RetentionLimit = (SELECT TOP 1 RBRL.MinRetentionLimit FROM PerLifeAggregationDetailData PLADD
					LEFT JOIN RetroBenefitCodes RBC ON RBC.Code = PLADD.RetroBenefitCode
					LEFT JOIN RetroBenefitRetentionLimits RBRL ON RBRL.RetroBenefitCodeId = RBC.Id
					WHERE PLADD.Id = PLAMD.PerLifeAggregationDetailDataId)
			FROM PerLifeAggregationMonthlyData PLAMD
			WHERE PLAMD.Id = @PerLifeAggregationMonthlyDataId

			IF @RetroAmount > 0 --Matched and retro amount > 0
			BEGIN
				SET @IsExcludePerformClaimRecovery = 0

				IF @OffsetStatus = @OffsetStatusOffset AND (@ClaimStatus = @StatusApproved OR @ClaimStatus = @StatusApprovedByCeo) AND @ClaimTransactionType = 'New' AND @RetroIndicator = 1
					SET @ClaimCategory = @ClaimCategoryPaidClaim

				ELSE IF @OffsetStatus = @OffsetStatusOffset AND (@ClaimStatus = @StatusApproved OR @ClaimStatus = @StatusApprovedByCeo) AND (@ClaimTransactionType = 'ADJ' OR @ClaimTransactionType = 'Adjustment') AND @RetroIndicator = 1
					SET @ClaimCategory = @ClaimCategoryReversed

				ELSE IF @OffsetStatus = @OffsetStatusOffset AND @ClaimStatus = @StatusPendingCeoApproval AND @RetroIndicator = 1
					SET @ClaimCategory = @ClaimCategoryPendingClaim

				ELSE IF @OffsetStatus = @OffsetStatusOffset AND @ClaimStatus = @StatusDeclined AND @ClaimTransactionType = 'New' AND @RetroIndicator = 1
					SET @ClaimCategory = @ClaimCategoryPendingClaim

				ELSE IF @OffsetStatus = @OffsetStatusOffset AND @ClaimStatus = @StatusDeclined AND (@ClaimTransactionType = 'ADJ' OR @ClaimTransactionType = 'Adjustment') AND @RetroIndicator = 1
					SET @ClaimCategory = @ClaimCategoryReversed

				ELSE IF @OffsetStatus <> @OffsetStatusOffset AND (@ClaimStatus = @StatusApproved OR @ClaimStatus = @StatusApprovedByCeo) AND @RetroIndicator = 1
					SET @ClaimCategory = @ClaimCategoryPendingClaim

				ELSE IF @OffsetStatus <> @OffsetStatusOffset AND @ClaimStatus = @StatusPendingCeoApproval AND @RetroIndicator = 1
					SET @ClaimCategory = @ClaimCategoryPendingClaim

				ELSE IF @OffsetStatus <> @OffsetStatusOffset AND @ClaimStatus = @StatusDeclined AND @RetroIndicator = 1
					SET @ClaimCategory = @ClaimCategoryPendingClaim

				ELSE
					SET @IsExcludePerformClaimRecovery = 1
			END
		END

		IF @IsExcludePerformClaimRecovery = 0 --Process matched
		BEGIN
			--SET @RetroClaimRecoveryAmount = @RetroAmount / (@AAR * @MlreShare) * @RetentionLimit
			EXEC dbo.ComputePerLifeClaimRecovery
				@Id,
				@RetroAmount,
				@AAR,
				@RetentionLimit,
				@PerLifeAggregationMonthlyDataId,
				@ClaimRegisterHistoryId,
				@ClaimCategory,
				@ChildResult
		END
	END
	ELSE --Not Matched
	BEGIN
		SET @IsExcludePerformClaimRecovery = 1
	END

	FETCH NEXT FROM PerLifeClaimDataCursor INTO @Id, @PerLifeAggregationDetailDataId, @ClaimStatus, @OffsetStatus, @ClaimTransactionType, @ClaimRegisterHistoryId
END

CLOSE PerLifeClaimDataCursor

DEALLOCATE PerLifeClaimDataCursor