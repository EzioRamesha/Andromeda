CREATE OR ALTER PROCEDURE [dbo].[PerLifeDataValidation](@PerLifeAggregationId INT, @Result VARCHAR(MAX) OUTPUT)

AS

SET NOCOUNT ON;

DECLARE
	@FundsAccountingTypePickListDetailId INT,
	@FundsAccountingTypeCode VARCHAR(MAX),
	@SoaQuarter VARCHAR(MAX),
	@CutOffId INT,
	@SoaQuarterStartMonth INT,
	@SoaQuarterEndMonth INT,
	@SoaQuarterYear INT

-- Table for PerLifeRetroConfigurationTreaties TreatyCode
DECLARE @RetroTreatyCodes TABLE(Code VARCHAR(MAX))

-- Table for Detail
DECLARE @DetailTable TABLE(Id INT, RiskQuarter VARCHAR(MAX))

-- Table for Detail Treaties
DECLARE @DetailTreatyTable TABLE(Id INT, PerLifeAggregationDetailId INT, TreatyCode VARCHAR(MAX))

-- Find PerLifeAggregation by Id
SELECT TOP(1)
	@FundsAccountingTypePickListDetailId = FundsAccountingTypePickListDetailId,
	@SoaQuarter = SoaQuarter,
	@CutOffId = CutOffId
FROM 
	PerLifeAggregations
WHERE 
	Id = @PerLifeAggregationId;

-- Find FundAccountingType by @FundsAccountingTypePickListDetailId
SELECT TOP(1)
	@FundsAccountingTypeCode = Code
FROM 
	PickListDetails
WHERE 
	Id = @FundsAccountingTypePickListDetailId;

-- Process SoaQuarter
EXEC SplitQuarter @SoaQuarter, @SoaQuarterYear OUTPUT, @SoaQuarterStartMonth OUTPUT, @SoaQuarterEndMonth OUTPUT;

-- Delete Previous Data for re-processing
-- Delete MonthlyRetroData
DELETE FROM PerLifeAggregationMonthlyRetroData 
WHERE PerLifeAggregationMonthlyDataId IN 
(SELECT Id FROM PerLifeAggregationMonthlyData WHERE PerLifeAggregationDetailDataId IN 
(SELECT Id FROM PerLifeAggregationDetailData WHERE PerLifeAggregationDetailTreatyId IN 
(SELECT Id FROM PerLifeAggregationDetailTreaties WHERE PerLifeAggregationDetailId IN
(SELECT Id FROM PerLifeAggregationDetails WHERE PerLifeAggregationId = @PerLifeAggregationId))));

-- Delete MonthlyData
DELETE FROM PerLifeAggregationMonthlyData
WHERE PerLifeAggregationDetailDataId IN 
(SELECT Id FROM PerLifeAggregationDetailData WHERE PerLifeAggregationDetailTreatyId IN 
(SELECT Id FROM PerLifeAggregationDetailTreaties WHERE PerLifeAggregationDetailId IN 
(SELECT Id FROM PerLifeAggregationDetails WHERE PerLifeAggregationId = @PerLifeAggregationId)));

-- Delete DetailData
DELETE FROM PerLifeAggregationDetailData
WHERE PerLifeAggregationDetailTreatyId IN 
(SELECT Id FROM PerLifeAggregationDetailTreaties WHERE PerLifeAggregationDetailId IN 
(SELECT Id FROM PerLifeAggregationDetails WHERE PerLifeAggregationId = @PerLifeAggregationId));

-- Delete DetailTreaty
DELETE FROM PerLifeAggregationDetailTreaties	
WHERE PerLifeAggregationDetailId IN (SELECT Id FROM PerLifeAggregationDetails WHERE PerLifeAggregationId = @PerLifeAggregationId);

-- Delete Detail
DELETE FROM PerLifeAggregationDetails
WHERE PerLifeAggregationId = @PerLifeAggregationId;

-- Insert TreatyCode from PerLifeRetroConfigurationTreaties
INSERT INTO @RetroTreatyCodes 
SELECT TC.Code 
FROM PerLifeRetroConfigurationTreaties AS PLRCT
LEFT JOIN  TreatyCodes AS TC
ON PLRCT.TreatyCodeId = TC.Id;

DECLARE
	@PerLifeAggregationDetailId INT,
	@PerLifeAggregationDetailTreatyId INT,
	@RiDataWarehouseId INT,
	@RiskPeriodMonth INT,
	@RiskPeriodYear INT,
	@TreatyCode VARCHAR(MAX),
	@RiskQuarter VARCHAR(MAX),
	@TotalRiskQuarter INT

DECLARE RiDataWarehouseHistoryCursor CURSOR FOR
SELECT
	[Id],
	[RiskPeriodMonth],
	[RiskPeriodYear],
	[TreatyCode]
FROM 
	RiDataWarehouseHistories
WHERE 
	CutOffId = @CutOffId 
	AND TreatyCode IN (SELECT Code FROM @RetroTreatyCodes)
	AND TransactionTypeCode IN ('NB', 'RN')
	AND PolicyStatusCode = 'IF'
	AND ((RiskPeriodYear = @SoaQuarterYear AND RiskPeriodMonth <= @SoaQuarterEndMonth) OR (RiskPeriodYear < @SoaQuarterYear))
	AND FundsAccountingTypeCode = @FundsAccountingTypeCode
ORDER BY Id

OPEN RiDataWarehouseHistoryCursor

FETCH NEXT FROM RiDataWarehouseHistoryCursor INTO @RiDataWarehouseId, @RiskPeriodMonth, @RiskPeriodYear, @TreatyCode

WHILE @@FETCH_STATUS = 0
BEGIN
	SET @RiskQuarter = dbo.FormatQuarter(@RiskPeriodYear, @RiskPeriodMonth)
	
	IF EXISTS (SELECT * FROM @DetailTable WHERE RiskQuarter = @RiskQuarter)
		BEGIN
	        SELECT @PerLifeAggregationDetailId = Id FROM @DetailTable WHERE RiskQuarter = @RiskQuarter
	    END
	ELSE
	    BEGIN
	        INSERT INTO PerLifeAggregationDetails (PerLifeAggregationId, RiskQuarter, Status, CreatedAt, UpdatedAt, CreatedById, UpdatedById) 
            VALUES (@PerLifeAggregationId, @RiskQuarter, dbo.GetConstantInt('AggregationDetailStatusPending'), GETDATE(), GETDATE(), dbo.GetConstantInt('AuthUserId'), dbo.GetConstantInt('AuthUserId'))

            SET @PerLifeAggregationDetailId = SCOPE_IDENTITY()

            INSERT INTO @DetailTable (Id, RiskQuarter)
            VALUES (@PerLifeAggregationDetailId, @RiskQuarter)
	    END


  	IF EXISTS (SELECT * FROM @DetailTreatyTable WHERE PerLifeAggregationDetailId = @PerLifeAggregationDetailId AND TreatyCode = @TreatyCode)
  		BEGIN
	        SELECT @PerLifeAggregationDetailTreatyId = Id FROM @DetailTreatyTable WHERE PerLifeAggregationDetailId = @PerLifeAggregationDetailId AND TreatyCode = @TreatyCode
	    END
	ELSE
	    BEGIN
	        INSERT INTO PerLifeAggregationDetailTreaties (PerLifeAggregationDetailId, TreatyCode, CreatedAt, UpdatedAt, CreatedById, UpdatedById) 
            VALUES (@PerLifeAggregationDetailId, @TreatyCode, GETDATE(), GETDATE(), dbo.GetConstantInt('AuthUserId'), dbo.GetConstantInt('AuthUserId'))

            SET @PerLifeAggregationDetailTreatyId = SCOPE_IDENTITY()

            INSERT INTO @DetailTreatyTable (Id, PerLifeAggregationDetailId, TreatyCode)
            VALUES (@PerLifeAggregationDetailTreatyId, @PerLifeAggregationDetailId, @TreatyCode)
	    END

    INSERT INTO PerLifeAggregationDetailData (PerLifeAggregationDetailTreatyId, RiDataWarehouseHistoryId, IsException, CreatedAt, UpdatedAt, CreatedById, UpdatedById)
    VALUES (@PerLifeAggregationDetailTreatyId, @RiDataWarehouseId, 0, GETDATE(), GETDATE(), dbo.GetConstantInt('AuthUserId'), dbo.GetConstantInt('AuthUserId'))

	FETCH NEXT FROM RiDataWarehouseHistoryCursor INTO @RiDataWarehouseId, @RiskPeriodMonth, @RiskPeriodYear, @TreatyCode
END

CLOSE RiDataWarehouseHistoryCursor;

DEALLOCATE RiDataWarehouseHistoryCursor;

SELECT @TotalRiskQuarter=COUNT(*) FROM @DetailTable;
SET @Result = '{"Total Risk Quarter": ' + CAST(@TotalRiskQuarter AS VARCHAR(10));

-- Proceed to next step Process by Risk Quarter
DECLARE @ChildResult VARCHAR(MAX)

DECLARE PerLifeAggregationDetailCursor CURSOR FOR
SELECT
	[Id]
FROM 
	PerLifeAggregationDetails
WHERE 
	PerLifeAggregationId = @PerLifeAggregationId
ORDER BY Id

OPEN PerLifeAggregationDetailCursor

FETCH NEXT FROM PerLifeAggregationDetailCursor INTO @PerLifeAggregationDetailId

WHILE @@FETCH_STATUS = 0
BEGIN
	EXEC ValidatePerLifeDetail @PerLifeAggregationDetailId, @Result=@ChildResult OUTPUT

	UPDATE PerLifeAggregationDetails SET Status = dbo.GetConstantInt('AggregationDetailStatusValidationSuccess') 
	WHERE Id = @PerLifeAggregationDetailId

	SET @Result += ', ' + @ChildResult;
	SET @ChildResult = NULL;

	FETCH NEXT FROM PerLifeAggregationDetailCursor INTO @PerLifeAggregationDetailId
END

CLOSE PerLifeAggregationDetailCursor;

DEALLOCATE PerLifeAggregationDetailCursor;

-- Set Result
SET @Result += '}';

-- Declare @Result Varchar(Max) Exec PerLifeAggregation 1, @Result Output

-- Print @Result