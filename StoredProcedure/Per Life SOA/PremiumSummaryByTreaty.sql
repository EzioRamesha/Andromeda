CREATE OR ALTER PROCEDURE [dbo].[PremiumSummaryByTreaty](@PerLifeSoaId INT, @SoaQuarter NVARCHAR(30))

AS

SET NOCOUNT ON;

DECLARE
	@RiskYear INT = CAST(LEFT(@SoaQuarter,4) AS INT),
	@RiskMonth1 INT = (CASE RIGHT(@SoaQuarter,1) WHEN '1' THEN 1 WHEN '2' THEN 4 WHEN '3' THEN 7 WHEN '4' THEN 10 END),
	@RiskMonth2 INT = (CASE RIGHT(@SoaQuarter,1) WHEN '1' THEN 2 WHEN '2' THEN 5 WHEN '3' THEN 8 WHEN '4' THEN 11 END),
	@RiskMonth3 INT = (CASE RIGHT(@SoaQuarter,1) WHEN '1' THEN 3 WHEN '2' THEN 6 WHEN '3' THEN 9 WHEN '4' THEN 12 END)

INSERT INTO PerLifeSoaSummariesByTreaty (PerLifeSoaId, TreatyCode, ProcessingPeriod, TotalRetroAmount, TotalGrossPremium, TotalNetPremium,
TotalDiscount, TotalPolicyCount, CreatedAt, UpdatedAt, CreatedById, UpdatedById)
SELECT
	@PerLifeSoaId AS PerLifeSoaId,
	PLADT.[TreatyCode] AS TreatyCode,
	@SoaQuarter AS ProcessingPeriod,
	SUM(ISNULL(PLAMD.[RetroAmount],0)) AS TotalRetroAmount,
	SUM(ISNULL(PLAMRD.[RetroGrossPremium],0)) AS TotalGrossPremium,
	SUM(ISNULL(PLAMRD.[RetroNetPremium],0)) AS TotalNetPremium,
	SUM(ISNULL(PLAMRD.[RetroDiscount],0)) AS TotalDiscount,

	(SELECT COUNT(*) FROM PerLifeAggregationMonthlyData WHERE TreatyCode = PLADT.[TreatyCode] AND RiskYear = @RiskYear
	AND RiskMonth IN (@RiskMonth1,@RiskMonth2,@RiskMonth3)) AS TotalPolicyCount,

	GETDATE() AS CreatedAt,
	GETDATE() AS UpdatedAt,
	1 AS CreatedById,
	1 AS UpdatedById
FROM
	PerLifeAggregationMonthlyRetroData PLAMRD
LEFT JOIN
	PerLifeAggregationMonthlyData PLAMD
	ON PLAMD.Id = PLAMRD.PerLifeAggregationMonthlyDataId
LEFT JOIN 
	PerLifeAggregationDetailData PLADD
	ON PLADD.Id = PLAMD.PerLifeAggregationDetailDataId
LEFT JOIN 
	PerLifeSoaData PLSD
	ON PLSD.PerLifeAggregationDetailDataId = PLADD.Id
LEFT JOIN 
	PerLifeAggregationDetailTreaties PLADT
	ON PLADT.Id = PLADD.PerLifeAggregationDetailTreatyId
LEFT JOIN
	TreatyCodes AS TC
	ON TC.Code = PLADT.TreatyCode
LEFT JOIN
	Treaties AS T
	ON T.Id = TC.TreatyId
WHERE
	PLSD.PerLifeSoaId = @PerLifeSoaId
	AND PLSD.PerLifeAggregationDetailDataId IS NOT NULL
	AND PLAMD.RetroIndicator = 1
GROUP BY PLADT.[TreatyCode]