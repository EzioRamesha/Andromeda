CREATE OR ALTER PROCEDURE [dbo].[RemoveRiDataPartition](@TreatyCode VARCHAR(MAX), @Result VARCHAR(MAX) OUTPUT)

AS

DECLARE
    @DashPosition INT,
    @FormattedTreatyCode VARCHAR(50),
    @PartitionFunctionName VARCHAR(50),
    @PartitionSchemeName VARCHAR(50),
    @WarehousePartitionFunctionName VARCHAR(50),
    @WarehousePartitionSchemeName VARCHAR(50),
    @Query NVARCHAR(MAX)

-- RiData
SET @PartitionFunctionName = 'TreatyCodeRiDataFunction'
SET @PartitionSchemeName = 'TreatyCodeRiDataScheme'

IF NOT EXISTS (SELECT 1 FROM sys.partition_functions WHERE name = @PartitionFunctionName)
BEGIN
	RETURN 0;
END

IF NOT EXISTS (SELECT 1 FROM sys.partition_schemes WHERE name = @PartitionSchemeName)
BEGIN
	RETURN 0;
END

SELECT @DashPosition = CHARINDEX('-', @TreatyCode);
SELECT @FormattedTreatyCode = SUBSTRING(@TreatyCode, 1, @DashPosition - 1);

IF EXISTS (SELECT 1 FROM TreatyCodes WHERE Code LIKE @FormattedTreatyCode + '-%')
BEGIN
	RETURN 0;
END

IF EXISTS (SELECT 1 FROM RiData WHERE TreatyCode LIKE @FormattedTreatyCode + '-%')
BEGIN
    RETURN 0;
END

IF NOT EXISTS (SELECT 1 FROM sys.partition_range_values AS prv 
    LEFT JOIN sys.partition_functions AS pf ON
    prv.function_id = pf.function_id
    WHERE pf.name = @PartitionFunctionName AND
    prv.value = @FormattedTreatyCode
)
BEGIN
	RETURN 0;
END

SET @Query = 'ALTER PARTITION FUNCTION ' + @PartitionFunctionName + '() MERGE RANGE (''' + @FormattedTreatyCode +''')';
EXEC(@Query);

-- RiDataWarehouse
SET @WarehousePartitionFunctionName = 'TreatyCodeRiDataWarehouseFunction'
SET @WarehousePartitionSchemeName = 'TreatyCodeRiDataWarehouseScheme'

IF NOT EXISTS (SELECT 1 FROM sys.partition_functions WHERE name = @WarehousePartitionFunctionName)
BEGIN
    RETURN 0;
END

IF NOT EXISTS (SELECT 1 FROM sys.partition_schemes WHERE name = @WarehousePartitionSchemeName)
BEGIN
    RETURN 0;
END

-- SELECT @DashPosition = CHARINDEX('-', @TreatyCode);
-- SELECT @FormattedTreatyCode = SUBSTRING(@TreatyCode, 1, @DashPosition - 1);

-- IF EXISTS (SELECT 1 FROM TreatyCodes WHERE Code LIKE @FormattedTreatyCode + '-%')
-- BEGIN
--     RETURN 0;
-- END

-- IF EXISTS (SELECT 1 FROM RiData WHERE TreatyCode LIKE @FormattedTreatyCode + '-%')
-- BEGIN
--     RETURN 0;
-- END

IF EXISTS (SELECT 1 FROM RiDataWarehouse WHERE TreatyCode LIKE @FormattedTreatyCode + '-%')
BEGIN
    RETURN 0;
END

IF NOT EXISTS (SELECT 1 FROM sys.partition_range_values AS prv 
    LEFT JOIN sys.partition_functions AS pf ON
    prv.function_id = pf.function_id
    WHERE pf.name = @WarehousePartitionFunctionName AND
    prv.value = @WarehousePartitionSchemeName
)
BEGIN
    RETURN 0;
END

SET @Query = 'ALTER PARTITION FUNCTION ' + @WarehousePartitionFunctionName + '() MERGE RANGE (''' + @FormattedTreatyCode +''')';
EXEC(@Query);
