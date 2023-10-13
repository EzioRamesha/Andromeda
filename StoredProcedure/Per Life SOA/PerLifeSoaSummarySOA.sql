CREATE OR ALTER PROCEDURE [dbo].[PerLifeSoaSummarySOA](@PerLifeSoaId INT, @SoaQuarter NVARCHAR(30))

AS

SET NOCOUNT ON;

DECLARE @Temp1 TABLE (
	RiskQuarter NVARCHAR(30),
	RetroAmount FLOAT,
	FundsAccountingType NVARCHAR(30)
)

DECLARE @Temp2 TABLE (
	RiskQuarter NVARCHAR(30)
)

INSERT INTO @Temp1
SELECT
	CAST(PLAMD.[RiskYear] AS NVARCHAR(4)) + ' Q' +
	CASE PLAMD.[RiskMonth] WHEN 1 THEN '1' WHEN 2 THEN '1' WHEN 3 THEN '1'
	WHEN 4 THEN '2' WHEN 5 THEN '2' WHEN 6 THEN '2'
	WHEN 7 THEN '3' WHEN 8 THEN '3' WHEN 9 THEN '3'
	WHEN 10 THEN '4' WHEN 11 THEN '4' WHEN 12 THEN '4' END AS [RiskQuarter],
	PLAMD.[RetroAmount],
	PLD.[Description] AS [FundsAccountingType]
FROM
	PerLifeAggregationMonthlyData PLAMD
LEFT JOIN 
	PerLifeAggregationDetailData PLADD
	ON PLADD.Id = PLAMD.PerLifeAggregationDetailDataId
LEFT JOIN 
	PerLifeAggregationDetailTreaties PLADT
	ON PLADT.Id = PLADD.PerLifeAggregationDetailTreatyId
LEFT JOIN
	PerLifeAggregationDetails PLAD
	ON PLAD.Id = PLADT.PerLifeAggregationDetailId
LEFT JOIN
	PerLifeAggregations PLA
	ON PLA.Id = PLAD.PerLifeAggregationId
LEFT JOIN
	PickListDetails PLD
	ON PLD.Id = PLA.FundsAccountingTypePickListDetailId
WHERE PLA.SoaQuarter = @SoaQuarter AND PLAMD.RetroIndicator = 1

INSERT INTO @Temp2
SELECT DISTINCT RiskQuarter FROM @Temp1 ORDER BY RiskQuarter

--Insert premiums into PerLifeSoaSummariesSoa
INSERT INTO PerLifeSoaSummariesSoa (PerLifeSoaId, PremiumClaim, RowLabel, Individual, [Group], CreatedAt, UpdatedAt, CreatedById, UpdatedById)
SELECT
	@PerLifeSoaId AS PerLifeSoaId,
	1 AS PremiumClaim,
	T2.RiskQuarter AS RowLabel,

	(SELECT SUM(T1.RetroAmount) FROM @Temp1 T1 WHERE T1.RiskQuarter = T2.RiskQuarter AND T1.FundsAccountingType LIKE 'Ind%') AS Individual,
	(SELECT SUM(T1.RetroAmount) FROM @Temp1 T1 WHERE T1.RiskQuarter = T2.RiskQuarter AND T1.FundsAccountingType NOT LIKE 'Ind%') AS [Group],

	GETDATE() AS CreatedAt,
	GETDATE() AS UpdatedAt,
	1 AS CreatedById,
	1 AS UpdatedById
FROM @Temp2 T2

--Insert claims into PerLifeSoaSummariesSoa
INSERT INTO PerLifeSoaSummariesSoa (PerLifeSoaId, PremiumClaim, RowLabel, Individual, CreatedAt, UpdatedAt, CreatedById, UpdatedById)
SELECT
	@PerLifeSoaId AS PerLifeSoaId,
	2 AS PremiumClaim,
	CR.ClaimId AS RowLabel,

	SUM(PLCRD.RetroClaimRecoveryAmount) AS Individual,

	GETDATE() AS CreatedAt,
	GETDATE() AS UpdatedAt,
	1 AS CreatedById,
	1 AS UpdatedById
FROM PerLifeSoaData PLSD
LEFT JOIN 
	PerLifeClaimData PLCD
	ON PLCD.Id = PLSD.PerLifeClaimDataId
LEFT JOIN
	PerLifeClaimRetroData PLCRD
	ON PLCRD.PerLifeClaimDataId = PLCD.Id
LEFT JOIN
	ClaimRegisterHistories CRH
	ON CRH.Id = PLCD.ClaimRegisterHistoryId
LEFT JOIN
	ClaimRegister CR
	ON CR.Id = CRH.ClaimRegisterId
WHERE PLSD.PerLifeSoaId = @PerLifeSoaId AND PLSD.PerLifeClaimDataId IS NOT NULL
GROUP BY CR.ClaimId

--Insert profit comm into PerLifeSoaSummariesSoa
INSERT INTO PerLifeSoaSummariesSoa (PerLifeSoaId, PremiumClaim, RowLabel, Individual, CreatedAt, UpdatedAt, CreatedById, UpdatedById)
SELECT
	@PerLifeSoaId AS PerLifeSoaId,
	3 AS PremiumClaim,
	'PC' AS RowLabel,

	SUM(PLRS.ProfitComm) AS Individual,

	GETDATE() AS CreatedAt,
	GETDATE() AS UpdatedAt,
	1 AS CreatedById,
	1 AS UpdatedById
FROM PerLifeRetroStatements PLRS
WHERE PLRS.PerLifeSoaId = @PerLifeSoaId