CREATE OR ALTER PROCEDURE [dbo].[PerLifeSoaProcessing](@PerLifeSoaId INT, @Result VARCHAR(MAX) OUTPUT)

AS

SET NOCOUNT ON;

DECLARE
	@RetroPartyId INT,
	@RetroTreatyId INT,
	@SoaQuarter NVARCHAR(30)

SELECT 
	@RetroPartyId = RetroPartyId,
	@RetroTreatyId = RetroTreatyId,
	@SoaQuarter = SoaQuarter
FROM PerLifeSoa
WHERE Id = @PerLifeSoaId

DELETE FROM PerLifeSoaData WHERE PerLifeSoaId = @PerLifeSoaId
DELETE FROM PerLifeSoaSummaries WHERE PerLifeSoaId = @PerLifeSoaId
DELETE FROM PerLifeRetroStatements WHERE PerLifeSoaId = @PerLifeSoaId
DELETE FROM PerLifeSoaSummariesByTreaty WHERE PerLifeSoaId = @PerLifeSoaId
DELETE FROM PerLifeSoaSummariesSoa WHERE PerLifeSoaId = @PerLifeSoaId

INSERT INTO PerLifeSoaData (
	PerLifeSoaId,
	PerLifeClaimDataId,
	CreatedById,
	UpdatedById,
	CreatedAt,
	UpdatedAt)
SELECT
	@PerLifeSoaId AS PerLifeSoaId,
	PLCD.Id AS PerLifeClaimDataId,
	1 AS CreatedById,
	1 AS UpdatedById,
	GETDATE() AS CreatedAt,
	GETDATE() AS UpdatedAt
FROM PerLifeClaimData PLCD
LEFT JOIN PerLifeClaims PLC ON PLCD.PerLifeClaimId = PLC.Id
WHERE PLC.SoaQuarter = @SoaQuarter

INSERT INTO PerLifeSoaData (
	PerLifeSoaId,
	PerLifeAggregationDetailDataId,
	CreatedById,
	UpdatedById,
	CreatedAt,
	UpdatedAt)
SELECT
	@PerLifeSoaId AS PerLifeSoaId,
	PLADD.Id AS PerLifeAggregationDetailDataId,
	1 AS CreatedById,
	1 AS UpdatedById,
	GETDATE() AS CreatedAt,
	GETDATE() AS UpdatedAt
FROM PerLifeAggregationDetailData PLADD
LEFT JOIN PerLifeAggregationDetailTreaties PLADT ON PLADT.Id = PLADD.PerLifeAggregationDetailTreatyId
LEFT JOIN PerLifeAggregationDetails PLD ON PLD.Id = PLADT.PerLifeAggregationDetailId
LEFT JOIN PerLifeAggregations PLA ON PLA.Id = PLD.PerLifeAggregationId
WHERE PLA.SoaQuarter = @SoaQuarter

--Update PerLifeAggregationId in PerLifeSoa table
DECLARE @PerLifeAggregationId INT, @PerLifeAggregationDetailDataId INT

SET @PerLifeAggregationDetailDataId = (SELECT TOP 1 PerLifeAggregationDetailDataId FROM PerLifeSoaData
	WHERE PerLifeAggregationDetailDataId IS NOT NULL)

IF @PerLifeAggregationDetailDataId IS NOT NULL
BEGIN
	SET @PerLifeAggregationId = (SELECT TOP 1 PLD.PerLifeAggregationId FROM PerLifeAggregationDetailData PLADD
	LEFT JOIN PerLifeAggregationDetailTreaties PLADT ON PLADT.Id = PLADD.PerLifeAggregationDetailTreatyId
	LEFT JOIN PerLifeAggregationDetails PLD ON PLD.Id = PLADT.PerLifeAggregationDetailId)

	UPDATE PerLifeSoa SET PerLifeAggregationId = @PerLifeAggregationId WHERE Id = @PerLifeSoaId
END

--Retro Statement Summary - SOA WMOM
EXEC dbo.RetroStatementSummarySOAWMOM @PerLifeSoaId

--Premium (Summary by Treaty)
EXEC dbo.PremiumSummaryByTreaty @PerLifeSoaId, @SoaQuarter

--Summary SOA
EXEC dbo.PerLifeSoaSummarySOA @PerLifeSoaId, @SoaQuarter