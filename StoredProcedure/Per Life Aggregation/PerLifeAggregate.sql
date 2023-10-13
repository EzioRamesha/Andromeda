CREATE OR ALTER PROCEDURE [dbo].[PerLifeAggregate](@PerLifeAggregationDetailId INT, @Result VARCHAR(MAX) OUTPUT)

AS

SET NOCOUNT ON;

DELETE FROM PerLifeAggregationMonthlyRetroData 
WHERE PerLifeAggregationMonthlyDataId IN (
	SELECT Id FROM PerLifeAggregationMonthlyData WHERE PerLifeAggregationDetailDataId IN (
		SELECT Id FROM PerLifeAggregationDetailData WHERE PerLifeAggregationDetailTreatyId IN (
			SELECT Id FROM PerLifeAggregationDetailTreaties WHERE PerLifeAggregationDetailId = @PerLifeAggregationDetailId
		)
	)
)

DECLARE 
	-- To differentiate assured
	@InsuredName VARCHAR(MAX),
	@InsuredDateOfBirth VARCHAR(MAX),
	@InsuredGenderCode VARCHAR(MAX),
	@TerritoryOfIssueCode VARCHAR(MAX),
	@ReinsEffDatePol VARCHAR(MAX),
	@RetroBenefitCode VARCHAR(MAX),
	@RiskMonth INT,
	@RecordNo INT,
	-- Values
	@Aar FLOAT,
	-- Retention Limit 
	@RetentionLimit FLOAT,
	@MinReinsAmount FLOAT,
	@PrevAccumulativeRetain FLOAT,
	@AccumulativeRetainAmount FLOAT,
	-- Monthly Data
	@DataMonthlyId INT, -- PerLifeAggregationMonthlyDataId
	@AccumulativeAar FLOAT,
	@DistributedRetentionLimit FLOAT,
	@DistributedRetroAmount FLOAT,
	@RetroIndicator BIT,
	-- Sum Data
	@SumOfNetPremium FLOAT,
	@SumOfAar FLOAT,
	@RetroAmount FLOAT

DECLARE @RetentionAmounts TABLE(ReinsEffDatePol VARCHAR(MAX), RetentionLimit FLOAT, MinReinsAmount FLOAT)
DECLARE @RetentionLimits TABLE(InsuredName VARCHAR(MAX), InsuredDateOfBirth VARCHAR(MAX), InsuredGenderCode VARCHAR(MAX), 
	TerritoryOfIssueCode VARCHAR(MAX), RetroBenefitCode VARCHAR(MAX), RetentionLimit FLOAT, MinReinsAmount FLOAT)

DECLARE MonthlyDataCursor CURSOR FOR 
	SELECT
		M.Id,
		W.InsuredName, 
		W.InsuredDateOfBirth, 
		W.InsuredGenderCode, 
		W.TerritoryOfIssueCode, 
		W.ReinsEffDatePol,
		D.RetroBenefitCode,
		M.Aar,
		M.RiskMonth,
		RANK() OVER (PARTITION BY M.RiskMonth,W.InsuredName,W.InsuredDateOfBirth,W.InsuredGenderCode,
			W.TerritoryOfIssueCode,D.RetroBenefitCode ORDER BY M.Id) RecordNo
	FROM 
		PerLifeAggregationMonthlyData AS M
	JOIN
		PerLifeAggregationDetailData AS D
		ON D.Id = M.PerLifeAggregationDetailDataId
	JOIN
		PerLifeAggregationDetailTreaties AS T
		ON T.Id = PerLifeAggregationDetailTreatyId
	JOIN 
		RiDataWarehouseHistories AS W
		ON W.Id = D.RiDataWarehouseHistoryId
	WHERE 
		T.PerLifeAggregationDetailId = @PerLifeAggregationDetailId AND
		D.IsToAggregate = 1

OPEN MonthlyDataCursor

FETCH NEXT FROM MonthlyDataCursor INTO @DataMonthlyId, @InsuredName, @InsuredDateOfBirth, @InsuredGenderCode, @TerritoryOfIssueCode, 
	@ReinsEffDatePol, @RetroBenefitCode, @Aar, @RiskMonth, @RecordNo

WHILE @@FETCH_STATUS = 0
	BEGIN
		IF (@RecordNo = 1)
			BEGIN 
				DELETE @RetentionAmounts;
				SET @AccumulativeAar = 0;
				SET @AccumulativeRetainAmount = 0;

				INSERT INTO @RetentionAmounts (ReinsEffDatePol)
				SELECT W.ReinsEffDatePol
				FROM PerLifeAggregationMonthlyData AS M
				JOIN PerLifeAggregationDetailData AS D ON D.Id = M.PerLifeAggregationDetailDataId
				JOIN RiDataWarehouseHistories AS W ON W.Id = D.RiDataWarehouseHistoryId
				WHERE W.InsuredName = @InsuredName AND W.InsuredDateOfBirth = @InsuredDateOfBirth AND
					W.InsuredGenderCode = @InsuredGenderCode AND W.TerritoryOfIssueCode = @TerritoryOfIssueCode AND
					D.RetroBenefitCode = @RetroBenefitCode AND M.RiskMonth = @RiskMonth

				-- UPDATE RA SET RetentionLimit = (SELECT MlreRetentionAmount 
				-- 	FROM RetroBenefitRetentionLimitDetails AS LD
				-- 	JOIN RetroBenefitRetentionLimits AS L ON L.Id = LD.RetroBenefitRetentionLimitId
				-- 	JOIN RetroBenefitCodes AS C ON C.Id = L.RetroBenefitCodeId
				-- 	WHERE C.Code = @RetroBenefitCode AND 
				-- 		RA.ReinsEffDatePol BETWEEN LD.ReinsEffStartDate AND LD.ReinsEffEndDate) 
				-- FROM @RetentionAmounts AS RA

				UPDATE RA SET RetentionLimit = LD.MlreRetentionAmount, MinReinsAmount = LD.MinReinsAmount
				FROM @RetentionAmounts AS RA
				INNER JOIN RetroBenefitRetentionLimitDetails AS LD ON RA.ReinsEffDatePol BETWEEN LD.ReinsEffStartDate AND LD.ReinsEffEndDate
				INNER JOIN RetroBenefitRetentionLimits AS L ON L.Id = LD.RetroBenefitRetentionLimitId
				INNER JOIN RetroBenefitCodes AS C ON C.Id = L.RetroBenefitCodeId
				WHERE C.Code = @RetroBenefitCode

				SELECT 
					@SumOfNetPremium = SUM(M.NetPremium),
					@SumOfAar = SUM(M.Aar)
				FROM PerLifeAggregationDetailData AS D
				JOIN PerLifeAggregationMonthlyData AS M ON M.PerLifeAggregationDetailDataId = D.Id
				JOIN RiDataWarehouseHistories AS W ON W.Id = RiDataWarehouseHistoryId
				WHERE W.InsuredName = @InsuredName AND W.InsuredDateOfBirth = @InsuredDateOfBirth AND
					W.InsuredGenderCode = @InsuredGenderCode AND W.TerritoryOfIssueCode = @TerritoryOfIssueCode AND
					D.RetroBenefitCode = @RetroBenefitCode AND M.RiskMonth = @RiskMonth 
				Group BY M.RiskMonth,W.InsuredName,W.InsuredDateOfBirth,W.InsuredGenderCode,W.TerritoryOfIssueCode,D.RetroBenefitCode

				-- SELECT @RetentionLimit = MAX(RetentionLimit) FROM @RetentionAmounts;

				SELECT TOP(1) @RetentionLimit = RetentionLimit, @MinReinsAmount = MinReinsAmount 
				FROM @RetentionAmounts
				ORDER BY RetentionLimit DESC

				SET @RetroAmount = @SumOfAar - @RetentionLimit;
			END

		SET @RetroIndicator = 1;
		SET @PrevAccumulativeRetain = @AccumulativeRetainAmount;

		SET @AccumulativeAar += @Aar;
		SET @AccumulativeRetainAmount = IIF(@RetentionLimit < @AccumulativeAar, @RetentionLimit, @AccumulativeAar);
		SET @DistributedRetentionLimit = IIF(@AccumulativeRetainAmount < @RetentionLimit, @Aar, @RetentionLimit - @PrevAccumulativeRetain);
		SET @DistributedRetroAmount = @Aar - @DistributedRetentionLimit;

		IF (@DistributedRetroAmount <= @MinReinsAmount)
			SET @RetroIndicator = 0;

		UPDATE 
			PerLifeAggregationMonthlyData 
		SET 
			SumOfAar = @SumOfAar,
			SumOfNetPremium = @SumOfNetPremium,
			RetentionLimit = @RetentionLimit,
			DistributedRetentionLimit = @DistributedRetentionLimit,
			RetroAmount = @RetroAmount,
			DistributedRetroAmount = @DistributedRetroAmount,
			AccumulativeRetainAmount = @AccumulativeRetainAmount,
			RetroIndicator = @RetroIndicator
		WHERE 
			Id = @DataMonthlyId

		FETCH NEXT FROM MonthlyDataCursor INTO @DataMonthlyId, @InsuredName, @InsuredDateOfBirth, @InsuredGenderCode, 
			@TerritoryOfIssueCode, @ReinsEffDatePol, @RetroBenefitCode, @Aar, @RiskMonth, @RecordNo
	END

CLOSE MonthlyDataCursor

DEALLOCATE MonthlyDataCursor	



