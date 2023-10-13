CREATE OR ALTER PROCEDURE [dbo].[PerLifeSplitData](@PerLifeAggregationDetailId INT, @Result VARCHAR(MAX) OUTPUT)

AS

SET NOCOUNT ON;

DECLARE 
	@RiskQuarter VARCHAR(MAX),
	@RiskYear INT,
	@RiskMonth INT,
	@RiskStartMonth INT,
	@RiskEndMonth INT,
	@RiskDate DATETIME,
	@AuthUserId INT = dbo.GetConstantInt('AuthUserId')

SELECT TOP(1)
	@RiskQuarter = RiskQuarter
FROM
	PerLifeAggregationDetails
WHERE
	Id = @PerLifeAggregationDetailId;

EXEC SplitQuarter @RiskQuarter, @RiskYear OUTPUT, @RiskStartMonth OUTPUT, @RiskEndMonth OUTPUT

SET @RiskMonth = @RiskStartMonth;
WHILE (@RiskMonth <= @RiskEndMonth)
	BEGIN
		SET @RiskDate =  CONVERT(DATETIME, '01/' + CONVERT(VARCHAR, @RiskMonth) + '/' + CONVERT(VARCHAR, @RiskYear));
		INSERT INTO PerLifeAggregationMonthlyData 
		(
			PerLifeAggregationDetailDataId, RiskYear, RiskMonth, UniqueKeyPerLife, RetroPremFreq, Aar, NetPremium, RetroRatio,
			CreatedAt, UpdatedAt, CreatedById, UpdatedById 
		)
		SELECT 
			D.Id, @RiskYear, @RiskMonth,
			W.InsuredName + FORMAT(W.InsuredDateOfBirth,'ddMMyyyy') + D.RetroBenefitCode, 'M',
			IIF(R.Id IS NOT NULL, ISNULL(W.Aar, 0) * R.RuleValue * R.MlreRetainRatio, ISNULL(W.Aar, 0)),
			IIF(R.Id IS NOT NULL, ISNULL(W.NetPremium, 0) * R.MlreRetainRatio, ISNULL(W.NetPremium, 0)),
			ISNULL(R.RetroRatio, 100),
			GETDATE(), GETDATE(), @AuthUserId, @AuthUserId
		FROM
			PerLifeAggregationDetailData AS D
		JOIN
			PerLifeAggregationDetailTreaties AS T
			ON T.Id = PerLifeAggregationDetailTreatyId
		JOIN 
			RiDataWarehouseHistories AS W
			ON W.Id = RiDataWarehouseHistoryId
		LEFT JOIN 
			PerLifeRetroConfigurationRatios AS R
			ON R.TreatyCodeId IN (SELECT Id FROM TreatyCodes WHERE Code = W.TreatyCode) AND
				R.RuleCeaseDate > GETDATE() AND @RiskDate BETWEEN R.RiskQuarterStartDate AND R.RiskQuarterEndDate AND
				W.ReinsEffDatePol BETWEEN R.ReinsEffectiveStartDate AND R.ReinsEffectiveEndDate
		WHERE 
			T.PerLifeAggregationDetailId = @PerLifeAggregationDetailId AND
			(D.IsException = 0 OR D.ProceedStatus = 1)
		ORDER BY
			W.InsuredName, W.InsuredDateOfBirth, W.InsuredGenderCode, W.TerritoryOfIssueCode, D.RetroBenefitCode,
			W.ReinsEffDatePol, W.PolicyNumber, W.MlreBenefitCode

		SET @RiskMonth += 1;
	END

