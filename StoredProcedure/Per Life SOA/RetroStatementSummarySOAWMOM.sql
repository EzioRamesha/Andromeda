CREATE OR ALTER PROCEDURE [dbo].[RetroStatementSummarySOAWMOM](@PerLifeSoaId INT)

AS

SET NOCOUNT ON;

--Claim section
DECLARE
	@BusinessOrigin NVARCHAR(30),
	@ReinsBasisCode NVARCHAR(30),
	@TransactionTypeCode NVARCHAR(30),
	@RetroAmount FLOAT,

	@WMClaimAuto FLOAT = 0,
	@WMClaimFacultative FLOAT = 0,
	@WMClaimAdvantage FLOAT = 0,
	@WMClaimTotal FLOAT = 0,

	@OMClaimAuto FLOAT = 0,
	@OMClaimFacultative FLOAT = 0,
	@OMClaimAdvantage FLOAT = 0,
	@OMClaimTotal FLOAT = 0

DECLARE ClaimsCursor CURSOR FOR 
	SELECT
		PLD.[Code] AS BusinessOrigin,
		CR.[ReinsBasisCode] AS ReinsBasisCode,
		ISNULL(PLCRD.[RetroClaimRecoveryAmount], 0) AS RetroClaimRecoveryAmount
	FROM
		PerLifeSoaData PLSD
	LEFT JOIN
		PerLifeClaimData AS PLCD
		ON PLCD.Id = PLSD.PerLifeClaimDataId
	LEFT JOIN
		PerLifeClaimRetroData AS PLCRD
		ON PLCRD.PerLifeClaimDataId = PLCD.Id
	LEFT JOIN
		ClaimRegisterHistories AS CRH
		ON CRH.Id = PLCD.ClaimRegisterHistoryId
	LEFT JOIN
		ClaimRegister AS CR
		ON CR.Id = CRH.ClaimRegisterId
	LEFT JOIN
		TreatyCodes AS TC
		ON TC.Code = CR.TreatyCode
	LEFT JOIN
		Treaties AS T
		ON T.Id = TC.TreatyId
	LEFT JOIN
		PickListDetails AS PLD
		ON PLD.Id = T.BusinessOriginPickListDetailId
	LEFT JOIN
		PickListDetails AS PLD1
		ON PLD1.Id = T.LineOfBusinessPickListDetailId
	WHERE
		PLSD.PerLifeSoaId = @PerLifeSoaId
		AND PLSD.PerLifeClaimDataId IS NOT NULL

OPEN ClaimsCursor

FETCH NEXT FROM ClaimsCursor INTO @BusinessOrigin, @ReinsBasisCode, @RetroAmount

WHILE @@FETCH_STATUS = 0
BEGIN
	IF @BusinessOrigin = 'WM'
	BEGIN
		--SET @WMClaimTotal = @WMClaimTotal + @RetroAmount

		IF @ReinsBasisCode = 'AUTO'
			SET @WMClaimAuto = @WMClaimAuto + @RetroAmount
		IF @ReinsBasisCode = 'FAC'
			SET @WMClaimFacultative = @WMClaimFacultative + @RetroAmount
		IF @ReinsBasisCode = 'AP'
			SET @WMClaimAdvantage = @WMClaimAdvantage + @RetroAmount
	END
	ELSE
	BEGIN
		--SET @OMClaimTotal = @OMClaimTotal + @RetroAmount

		IF @ReinsBasisCode = 'AUTO'
			SET @OMClaimAuto = @OMClaimAuto + @RetroAmount
		IF @ReinsBasisCode = 'FAC'
			SET @OMClaimFacultative = @OMClaimFacultative + @RetroAmount
		IF @ReinsBasisCode = 'AP'
			SET @OMClaimAdvantage = @OMClaimAdvantage + @RetroAmount
	END

	FETCH NEXT FROM ClaimsCursor INTO @BusinessOrigin, @ReinsBasisCode, @RetroAmount
END

CLOSE ClaimsCursor

DEALLOCATE ClaimsCursor

--Premiums section
DECLARE
	--@BusinessOrigin
	--@ReinsBasisCode
	--@RetroAmount

	--NB
	@WMNBPremiumAuto FLOAT = 0,
	@WMNBPremiumFacultative FLOAT = 0,
	@WMNBPremiumAdvantage FLOAT = 0,
	@WMNBPremiumTotal FLOAT = 0,

	@OMNBPremiumAuto FLOAT = 0,
	@OMNBPremiumFacultative FLOAT = 0,
	@OMNBPremiumAdvantage FLOAT = 0,
	@OMNBPremiumTotal FLOAT = 0,

	--RN
	@WMRNPremiumAuto FLOAT = 0,
	@WMRNPremiumFacultative FLOAT = 0,
	@WMRNPremiumAdvantage FLOAT = 0,
	@WMRNPremiumTotal FLOAT = 0,

	@OMRNPremiumAuto FLOAT = 0,
	@OMRNPremiumFacultative FLOAT = 0,
	@OMRNPremiumAdvantage FLOAT = 0,
	@OMRNPremiumTotal FLOAT = 0

DECLARE PremiumsCursor CURSOR FOR 
	SELECT
		PLD.[Code] AS BusinessOrigin,
		RDWH.[ReinsBasisCode] AS ReinsBasisCode,
		RDWH.[TransactionTypeCode],
		ISNULL(PLAMD.[RetroAmount], 0) AS RetroAmount
	FROM
		PerLifeSoaData PLSD
	LEFT JOIN 
		PerLifeAggregationDetailData PLADD
		ON PLADD.Id = PLSD.PerLifeAggregationDetailDataId
	LEFT JOIN 
		PerLifeAggregationDetailTreaties PLADT
		ON PLADT.Id = PLADD.PerLifeAggregationDetailTreatyId
	LEFT JOIN
		RiDataWarehouseHistories RDWH
		ON RDWH.Id = PLADD.RiDataWarehouseHistoryId
	LEFT JOIN
		PerLifeAggregationMonthlyData PLAMD
		ON PLAMD.PerLifeAggregationDetailDataId = PLADD.Id
	LEFT JOIN
		TreatyCodes AS TC
		ON TC.Code = PLADT.TreatyCode
	LEFT JOIN
		Treaties AS T
		ON T.Id = TC.TreatyId
	LEFT JOIN
		PickListDetails AS PLD
		ON PLD.Id = T.BusinessOriginPickListDetailId
	LEFT JOIN
		PickListDetails AS PLD1
		ON PLD1.Id = T.LineOfBusinessPickListDetailId
	WHERE
		PLSD.PerLifeSoaId = @PerLifeSoaId
		AND PLSD.PerLifeAggregationDetailDataId IS NOT NULL
		AND PLAMD.RetroIndicator = 1

OPEN PremiumsCursor

FETCH NEXT FROM PremiumsCursor INTO @BusinessOrigin, @ReinsBasisCode, @TransactionTypeCode, @RetroAmount

WHILE @@FETCH_STATUS = 0
BEGIN
	IF @BusinessOrigin = 'WM'
	BEGIN
		IF @TransactionTypeCode = 'NB'
		BEGIN
			--SET @WMNBPremiumTotal = @WMNBPremiumTotal + @RetroAmount

			IF @ReinsBasisCode = 'AUTO'
				SET @WMNBPremiumAuto = @WMNBPremiumAuto + @RetroAmount
			IF @ReinsBasisCode = 'FAC'
				SET @WMNBPremiumFacultative = @WMNBPremiumFacultative + @RetroAmount
			IF @ReinsBasisCode = 'AP'
				SET @WMNBPremiumAdvantage = @WMNBPremiumAdvantage + @RetroAmount
		END

		IF @TransactionTypeCode = 'RN'
		BEGIN
			--SET @WMRNPremiumTotal = @WMRNPremiumTotal + @RetroAmount

			IF @ReinsBasisCode = 'AUTO'
				SET @WMRNPremiumAuto = @WMRNPremiumAuto + @RetroAmount
			IF @ReinsBasisCode = 'FAC'
				SET @WMRNPremiumFacultative = @WMRNPremiumFacultative + @RetroAmount
			IF @ReinsBasisCode = 'AP'
				SET @WMRNPremiumAdvantage = @WMRNPremiumAdvantage + @RetroAmount
		END
	END
	ELSE
	BEGIN
		IF @TransactionTypeCode = 'NB'
		BEGIN
			--SET @OMNBPremiumTotal = @OMNBPremiumTotal + @RetroAmount

			IF @ReinsBasisCode = 'AUTO'
				SET @OMNBPremiumAuto = @OMNBPremiumAuto + @RetroAmount
			IF @ReinsBasisCode = 'FAC'
				SET @OMNBPremiumFacultative = @OMNBPremiumFacultative + @RetroAmount
			IF @ReinsBasisCode = 'AP'
				SET @OMNBPremiumAdvantage = @OMNBPremiumAdvantage + @RetroAmount
		END

		IF @TransactionTypeCode = 'RN'
		BEGIN
			--SET @OMRNPremiumTotal = @OMRNPremiumTotal + @RetroAmount

			IF @ReinsBasisCode = 'AUTO'
				SET @OMRNPremiumAuto = @OMRNPremiumAuto + @RetroAmount
			IF @ReinsBasisCode = 'FAC'
				SET @OMRNPremiumFacultative = @OMRNPremiumFacultative + @RetroAmount
			IF @ReinsBasisCode = 'AP'
				SET @OMRNPremiumAdvantage = @OMRNPremiumAdvantage + @RetroAmount
		END
	END

	FETCH NEXT FROM PremiumsCursor INTO @BusinessOrigin, @ReinsBasisCode, @TransactionTypeCode, @RetroAmount
END

CLOSE PremiumsCursor

DEALLOCATE PremiumsCursor


--Insert into table (WM)
INSERT INTO PerLifeSoaSummaries ([PerLifeSoaId], [WMOM], [RowLabel], [Automatic], [Facultative], [Advantage], [CreatedAt], [UpdatedAt], [CreatedById], [UpdatedById])
VALUES (@PerLifeSoaId, 1, 'NEW BUSINESS', @WMNBPremiumAuto, @WMNBPremiumFacultative, @WMNBPremiumAdvantage, GETDATE(), GETDATE(), 1, 1)

INSERT INTO PerLifeSoaSummaries ([PerLifeSoaId], [WMOM], [RowLabel], [Automatic], [Facultative], [Advantage], [CreatedAt], [UpdatedAt], [CreatedById], [UpdatedById])
VALUES (@PerLifeSoaId, 1, 'RENEWAL', @WMRNPremiumAuto, @WMRNPremiumFacultative, @WMRNPremiumAdvantage, GETDATE(), GETDATE(), 1, 1)

INSERT INTO PerLifeSoaSummaries ([PerLifeSoaId], [WMOM], [RowLabel], [Automatic], [Facultative], [Advantage], [CreatedAt], [UpdatedAt], [CreatedById], [UpdatedById])
VALUES (@PerLifeSoaId, 1, 'Total Premium', @WMNBPremiumAuto + @WMRNPremiumAuto, @WMNBPremiumFacultative + @WMRNPremiumFacultative, @WMNBPremiumAdvantage + @WMRNPremiumAdvantage, GETDATE(), GETDATE(), 1, 1)

INSERT INTO PerLifeSoaSummaries ([PerLifeSoaId], [WMOM], [RowLabel], [Automatic], [Facultative], [Advantage], [CreatedAt], [UpdatedAt], [CreatedById], [UpdatedById])
VALUES (@PerLifeSoaId, 1, 'Total Claim', @WMClaimAuto, @WMClaimFacultative, @WMClaimAdvantage, GETDATE(), GETDATE(), 1, 1)


--Insert into table (OM)
INSERT INTO PerLifeSoaSummaries ([PerLifeSoaId], [WMOM], [RowLabel], [Automatic], [Facultative], [Advantage], [CreatedAt], [UpdatedAt], [CreatedById], [UpdatedById])
VALUES (@PerLifeSoaId, 1, 'NEW BUSINESS', @OMNBPremiumAuto, @OMNBPremiumFacultative, @OMNBPremiumAdvantage, GETDATE(), GETDATE(), 1, 1)

INSERT INTO PerLifeSoaSummaries ([PerLifeSoaId], [WMOM], [RowLabel], [Automatic], [Facultative], [Advantage], [CreatedAt], [UpdatedAt], [CreatedById], [UpdatedById])
VALUES (@PerLifeSoaId, 1, 'RENEWAL', @OMRNPremiumAuto, @OMRNPremiumFacultative, @OMRNPremiumAdvantage, GETDATE(), GETDATE(), 1, 1)

INSERT INTO PerLifeSoaSummaries ([PerLifeSoaId], [WMOM], [RowLabel], [Automatic], [Facultative], [Advantage], [CreatedAt], [UpdatedAt], [CreatedById], [UpdatedById])
VALUES (@PerLifeSoaId, 1, 'Total Premium', @OMNBPremiumAuto + @OMRNPremiumAuto, @OMNBPremiumFacultative + @OMRNPremiumFacultative, @OMNBPremiumAdvantage + @OMRNPremiumAdvantage, GETDATE(), GETDATE(), 1, 1)

INSERT INTO PerLifeSoaSummaries ([PerLifeSoaId], [WMOM], [RowLabel], [Automatic], [Facultative], [Advantage], [CreatedAt], [UpdatedAt], [CreatedById], [UpdatedById])
VALUES (@PerLifeSoaId, 2, 'Total Claim', @OMClaimAuto, @OMClaimFacultative, @OMClaimAdvantage, GETDATE(), GETDATE(), 1, 1)