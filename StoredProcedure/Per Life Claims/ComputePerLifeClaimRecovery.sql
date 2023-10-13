CREATE OR ALTER PROCEDURE [dbo].[ComputePerLifeClaimRecovery](
	@PerLifeClaimDataId INT,
	@RetroAmount FLOAT,
	@AAR FLOAT,
	@RetentionLimit FLOAT,
	@PerLifeAggregationMonthlyDataId INT,
	@ClaimRegisterHistoryId INT,
	@ClaimCategory INT,
	@Result VARCHAR(MAX) OUTPUT)

AS

SET NOCOUNT ON;

DECLARE
	@RetroClaimRecoveryAmount FLOAT,
	@LateInterest FLOAT,
	@ExGratia FLOAT,
	@PerLifeAggregationMonthlyRetroDataId INT,
	@MlreShare FLOAT

DECLARE PerLifeAggregationMonthlyRetroDataCursor CURSOR FOR 
	SELECT
		PLAMRD.Id,
		ISNULL(PLAMRD.MlreShare,100) AS MlreShare
	FROM 
		PerLifeAggregationMonthlyRetroData AS PLAMRD
	WHERE 
		PLAMRD.PerLifeAggregationMonthlyDataId = @PerLifeAggregationMonthlyDataId

OPEN PerLifeAggregationMonthlyRetroDataCursor

FETCH NEXT FROM PerLifeAggregationMonthlyRetroDataCursor INTO @PerLifeAggregationMonthlyRetroDataId, @MlreShare

WHILE @@FETCH_STATUS = 0
BEGIN
	SELECT TOP 1 @LateInterest = ISNULL(LateInterest,0), @ExGratia = ISNULL(ExGratia,0)
	FROM ClaimRegisterHistories
	WHERE Id = @ClaimRegisterHistoryId
	
	SET @RetroClaimRecoveryAmount = @RetroAmount / (@AAR * @MlreShare) * @RetentionLimit
	SET @LateInterest = @RetroAmount / (@AAR * @MlreShare) * @LateInterest
	SET @ExGratia = @RetroAmount / (@AAR * @MlreShare) * @ExGratia

	INSERT INTO PerLifeClaimRetroData (
		PerLifeClaimDataId,
		ClaimCategory,
		MlreShare,
		RetroClaimRecoveryAmount,
		LateInterest,
		ExGratia,
		ComputedRetroRecoveryAmount,
		ComputedRetroLateInterest,
		ComputedRetroExGratia,
		ComputedClaimCategory,
		CreatedById,
		UpdatedById,
		CreatedAt,
		UpdatedAt)
	VALUES (
		@PerLifeClaimDataId,
		@ClaimCategory,
		@MlreShare,
		@RetroClaimRecoveryAmount,
		@LateInterest,
		@ExGratia,
		@RetroClaimRecoveryAmount,
		@LateInterest,
		@ExGratia,
		@ClaimCategory,
		1,
		1,
		GETDATE(),
		GETDATE())

	FETCH NEXT FROM PerLifeAggregationMonthlyRetroDataCursor INTO @PerLifeAggregationMonthlyRetroDataId, @MlreShare
END

CLOSE PerLifeAggregationMonthlyRetroDataCursor

DEALLOCATE PerLifeAggregationMonthlyRetroDataCursor