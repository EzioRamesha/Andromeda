CREATE OR ALTER PROCEDURE [dbo].[ProcessCutOffRiDataWarehouse](@CutOffQuarter VARCHAR(30), @Result VARCHAR(MAX) OUTPUT)

AS

SET NOCOUNT ON;

DECLARE
	@TableName VARCHAR(MAX),
	@Query VARCHAR(MAX)

SET @TableName = REPLACE('RiDataWarehouseHistories_%1', '%1', @CutOffQuarter)

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE' AND TABLE_NAME=@TableName)
BEGIN
EXEC('DROP TABLE ' + @TableName)
END

SET @Query = 'SELECT * INTO ' + @TableName + ' FROM [dbo].[RiDataWarehouse]';

EXEC(@Query)

--- Create Primary Key ---
EXEC('ALTER TABLE ' + @TableName + ' ADD CONSTRAINT PK_' + @TableName + ' PRIMARY KEY NONCLUSTERED (Id)')

--- Create Indexes ---
/****** Object:  Index [IX_CedingBenefitRiskCode] ******/
EXEC('CREATE NONCLUSTERED INDEX [IX_CedingBenefitRiskCode] ON ' + @TableName + ' ([CedingBenefitRiskCode] ASC)')
/****** Object:  Index [IX_CedingBenefitTypeCode] ******/
EXEC('CREATE NONCLUSTERED INDEX [IX_CedingBenefitTypeCode] ON ' + @TableName + ' ([CedingBenefitTypeCode] ASC)')
/****** Object:  Index [IX_CedingPlanCode] ******/
EXEC('CREATE NONCLUSTERED INDEX [IX_CedingPlanCode] ON ' + @TableName + ' ([CedingPlanCode] ASC)')
/****** Object:  Index [IX_ConflictType] ******/
EXEC('CREATE NONCLUSTERED INDEX [IX_ConflictType] ON ' + @TableName + ' ([ConflictType] ASC)')
/****** Object:  Index [IX_CreatedById] ******/
EXEC('CREATE NONCLUSTERED INDEX [IX_CreatedById] ON ' + @TableName + ' ([CreatedById] ASC)')
/****** Object:  Index [IX_EndingPolicyStatus] ******/
EXEC('CREATE NONCLUSTERED INDEX [IX_EndingPolicyStatus] ON ' + @TableName + ' ([EndingPolicyStatus] ASC)')
/****** Object:  Index [IX_InsuredName] ******/
EXEC('CREATE NONCLUSTERED INDEX [IX_InsuredName] ON ' + @TableName + ' ([InsuredName] ASC)')
/****** Object:  Index [IX_PolicyNumber] ******/
EXEC('CREATE NONCLUSTERED INDEX [IX_PolicyNumber] ON ' + @TableName + ' ([PolicyNumber] ASC)')
/****** Object:  Index [IX_PremiumFrequencyCode] ******/
EXEC('CREATE NONCLUSTERED INDEX [IX_PremiumFrequencyCode] ON ' + @TableName + ' ([PremiumFrequencyCode] ASC)')
/****** Object:  Index [IX_Quarter] ******/
EXEC('CREATE NONCLUSTERED INDEX [IX_Quarter] ON ' + @TableName + ' ([Quarter] ASC)')
/****** Object:  Index [IX_RecordType] ******/
EXEC('CREATE NONCLUSTERED INDEX [IX_RecordType] ON ' + @TableName + ' ([RecordType] ASC)')
/****** Object:  Index [IX_RetroParty1] ******/
EXEC('CREATE NONCLUSTERED INDEX [IX_RetroParty1] ON ' + @TableName + ' ([RetroParty1] ASC)')
/****** Object:  Index [IX_RetroParty2] ******/
EXEC('CREATE NONCLUSTERED INDEX [IX_RetroParty2] ON ' + @TableName + ' ([RetroParty2] ASC)')
/****** Object:  Index [IX_RetroParty3] ******/
EXEC('CREATE NONCLUSTERED INDEX [IX_RetroParty3] ON ' + @TableName + ' ([RetroParty3] ASC)')
/****** Object:  Index [IX_RiskPeriodMonth] ******/
EXEC('CREATE NONCLUSTERED INDEX [IX_RiskPeriodMonth] ON ' + @TableName + ' ([RiskPeriodMonth] ASC)')
/****** Object:  Index [IX_RiskPeriodYear] ******/
EXEC('CREATE NONCLUSTERED INDEX [IX_RiskPeriodYear] ON ' + @TableName + ' ([RiskPeriodYear] ASC)')
/****** Object:  Index [IX_TransactionTypeCode] ******/
EXEC('CREATE NONCLUSTERED INDEX [IX_TransactionTypeCode] ON ' + @TableName + ' ([TransactionTypeCode] ASC)')
/****** Object:  Index [IX_TreatyCode] ******/
EXEC('CREATE NONCLUSTERED INDEX [IX_TreatyCode] ON ' + @TableName + ' ([TreatyCode] ASC)')
/****** Object:  Index [IX_UpdatedById] ******/
EXEC('CREATE NONCLUSTERED INDEX [IX_UpdatedById] ON ' + @TableName + ' ([UpdatedById] ASC)')

--- Create View ---
DECLARE
	@QuarterCount INT,
	@CutOffId INT,
	@Quarter VARCHAR(30),
	@ViewQuery VARCHAR(MAX),
	@UnionQuery VARCHAR(MAX)

SELECT
	@QuarterCount = COUNT(*)
FROM
	CutOff
WHERE
	Status = 4

SET @ViewQuery = 'CREATE OR ALTER VIEW [dbo].[RiDataWarehouseHistories] AS ';

DECLARE CutOffCursor CURSOR FOR 
	SELECT 
		[Id],
		[Quarter]
    FROM 
        CutOff
    WHERE 
    	Status = 4

IF (@QuarterCount > 0)
BEGIN
	OPEN CutOffCursor

	FETCH NEXT FROM CutOffCursor INTO @CutOffId, @Quarter

	WHILE @@FETCH_STATUS = 0
	BEGIN
		IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE' AND TABLE_NAME='RiDataWarehouseHistories_' + REPLACE(@Quarter, ' ', ''))
		BEGIN
			SET @UnionQuery = COALESCE(@UnionQuery, '(') + ' SELECT ' + CAST(@CutOffId as varchar(10))  + ' AS CutOffId, Id AS RiDataWarehouseId, * FROM RiDataWarehouseHistories_' + REPLACE(@Quarter, ' ', '') + ' UNION';
		END
		FETCH NEXT FROM CutOffCursor INTO @CutOffId, @Quarter
	END

	CLOSE CutOffCursor

	DEALLOCATE CutOffCursor
END

IF (@UnionQuery IS NULL)
BEGIN
	SET @ViewQuery = @ViewQuery + 'SELECT 1 AS CutOffId, Id AS RiDataWarehouseId, * FROM RiDataWarehouse WHERE Id = 0';
END
ELSE
BEGIN
	SET @ViewQuery = @ViewQuery + @UnionQuery + ')';
	SET @ViewQuery = REPLACE(@ViewQuery, 'UNION)', ')');
END

EXEC(@ViewQuery);

--- Create Partitioning ---
EXEC dbo.[AddRiDataWarehouseHistoryCutOffPartition] @CutOffQuarter
