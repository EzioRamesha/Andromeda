CREATE OR ALTER PROCEDURE [dbo].[PerLifeClaimsProcessing](@PerLifeClaimId INT, @Result VARCHAR(MAX) OUTPUT)

AS

SET NOCOUNT ON;

DECLARE
	@CutOffId INT,
	@FundsAccountingTypePickListDetailId INT,
	@SoaQuarter NVARCHAR(30),
	@PerLifeSoaId INT

SELECT 
	@CutOffId = CutOffId,
	@FundsAccountingTypePickListDetailId = FundsAccountingTypePickListDetailId,
	@SoaQuarter = SoaQuarter
FROM PerLifeClaims
WHERE Id = @PerLifeClaimId

--Clear Per Life SOA tables
DECLARE PerLifeSoaCursor CURSOR FOR 
	SELECT
		PLSD.PerLifeSoaId
	FROM 
		PerLifeClaims AS PLC
	LEFT JOIN PerLifeClaimData PLCD
		ON PLCD.PerLifeClaimId = PLC.Id
	LEFT JOIN PerLifeSoaData PLSD
		ON PLSD.PerLifeClaimDataId = PLCD.Id
	WHERE 
		PLC.Id = @PerLifeClaimId

OPEN PerLifeSoaCursor

FETCH NEXT FROM PerLifeSoaCursor INTO @PerLifeSoaId

WHILE @@FETCH_STATUS = 0
BEGIN
	DELETE FROM PerLifeSoaData WHERE PerLifeSoaId = @PerLifeSoaId
	DELETE FROM PerLifeSoaSummaries WHERE PerLifeSoaId = @PerLifeSoaId
	DELETE FROM PerLifeRetroStatements WHERE PerLifeSoaId = @PerLifeSoaId
	DELETE FROM PerLifeSoaSummariesByTreaty WHERE PerLifeSoaId = @PerLifeSoaId
	DELETE FROM PerLifeSoaSummariesSoa WHERE PerLifeSoaId = @PerLifeSoaId

	FETCH NEXT FROM PerLifeSoaCursor INTO @PerLifeSoaId
END

CLOSE PerLifeSoaCursor

DEALLOCATE PerLifeSoaCursor

--Clear Per Life Claim tables
DELETE FROM PerLifeClaimRetroData 
WHERE PerLifeClaimDataId IN (
	SELECT Id FROM PerLifeClaimData WHERE PerLifeClaimId = @PerLifeClaimId
)

DELETE FROM PerLifeClaimData WHERE PerLifeClaimId = @PerLifeClaimId

--Insert
INSERT INTO PerLifeClaimData (
	PerLifeClaimId,
	ClaimRegisterHistoryId,
	PerLifeAggregationDetailDataId,
	IsException,
	IsExcludePerformClaimRecovery,
	ClaimRecoveryStatus,
	IsLateInterestShare,
	IsExGratiaShare,
	CreatedById,
	UpdatedById,
	CreatedAt,
	UpdatedAt)
SELECT
	@PerLifeClaimId AS PerLifeClaimId,
	CRH.Id AS ClaimRegisterHistoryId,
	PLADD.Id AS PerLifeAggregationDetailDataId,
	0 AS IsException,
	0 AS IsExcludePerformClaimRecovery,
	1 AS ClaimRecoveryStatus,
	1 AS IsLateInterestShare,
	1 AS IsExGratiaShare,
	1 AS CreatedById,
	1 AS UpdatedById,
	GETDATE() AS CreatedAt,
	GETDATE() AS UpdatedAt
FROM ClaimRegister CR
LEFT JOIN ClaimRegisterHistories CRH ON CRH.ClaimRegisterId = CR.Id
LEFT JOIN PickListDetails PLD ON PLD.Code = CR.FundsAccountingTypeCode AND PLD.PickListId = 24
LEFT JOIN TreatyCodes TC ON TC.Code = CR.TreatyCode
LEFT JOIN RiDataWarehouseHistories AS RDWH ON RDWH.RiDataWarehouseId = CR.RiDataWarehouseId
LEFT JOIN PerLifeAggregationDetailData AS PLADD ON PLADD.RiDataWarehouseHistoryId = RDWH.Id
WHERE CR.ClaimTransactionType IN ('New', 'ADJ', 'Adjustment')
AND EXISTS (SELECT * FROM PerLifeRetroConfigurationTreaties WHERE TreatyCodeId = TC.Id)
AND CRH.CutOffId = @CutOffId AND PLD.Id = @FundsAccountingTypePickListDetailId AND CR.SoaQuarter <= @SoaQuarter

UPDATE PerLifeClaims SET ProcessingDate = GETDATE() WHERE Id = @PerLifeClaimId