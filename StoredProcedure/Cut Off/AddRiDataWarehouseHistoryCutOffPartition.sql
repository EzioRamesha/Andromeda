--replace proper DB names
--replace file path

CREATE OR ALTER PROCEDURE [dbo].[AddRiDataWarehouseHistoryCutOffPartition](@CutOffQuarter VARCHAR(30))

AS

SET NOCOUNT ON;

DECLARE								
@CedantCode VARCHAR(MAX),								
@Query NVARCHAR(MAX),								
@DatabaseName VARCHAR(MAX),								
@FilePath VARCHAR(MAX),								
@FileGroupName VARCHAR(MAX),								
@PartName VARCHAR(MAX),								
@FileName VARCHAR(MAX),								
@TreatyCode VARCHAR(MAX),								
@DashPosition INT,								
@FormattedTreatyCode VARCHAR(50),								
@FileCount INT,								
@WarehousePartitionFunctionName VARCHAR(50),								
@WarehousePartitionSchemeName VARCHAR(50),								
@WarehouseFileGroupName VARCHAR(MAX),
@TableName VARCHAR(MAX)
								
SET @DatabaseName = 'Andromeda_Phase2a-MigTesting'								
SET @FilePath = 'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\'								
SET @WarehousePartitionFunctionName = 'TreatyCodeRiDataWarehouseHistoryFunction'								
SET @WarehousePartitionSchemeName = 'TreatyCodeRiDataWarehouseHistoryScheme'								
SET @FileCount = 3	
SET @TableName = REPLACE('RiDataWarehouseHistories_%1', '%1', @CutOffQuarter)
								
-- RiDataWarehouse								
IF NOT EXISTS (SELECT 1 FROM sys.partition_functions WHERE name = @WarehousePartitionFunctionName)								
BEGIN								
SET @Query = 'CREATE PARTITION FUNCTION ' + @WarehousePartitionFunctionName + '(nvarchar(35)) AS RANGE RIGHT FOR VALUES ()'								
EXEC(@Query)								
END								
								
IF NOT EXISTS (SELECT 1 FROM sys.partition_schemes WHERE name = @WarehousePartitionSchemeName)								
BEGIN								
SET @Query = 'CREATE PARTITION SCHEME ' + @WarehousePartitionSchemeName + '								
AS PARTITION ' + @WarehousePartitionFunctionName + '								
TO ([primary]);'								
EXEC(@Query)								
END								
								
DECLARE CedantCursor CURSOR FOR								
SELECT								
DISTINCT Code								
FROM Cedants								
								
OPEN CedantCursor								
								
FETCH NEXT FROM CedantCursor INTO @CedantCode								
								
WHILE @@FETCH_STATUS = 0								
								
BEGIN								
-- RiDataWarehouse								
SET @WarehouseFileGroupName = 'RiDataWH' + @CedantCode								
								
IF NOT EXISTS (								
SELECT 1 FROM sys.filegroups WHERE name = @WarehouseFileGroupName								
)								
BEGIN								
SET @Query = 'ALTER DATABASE [' + @DatabaseName + '] ADD FILEGROUP ' + @WarehouseFileGroupName;								
EXEC (@Query);								
END								
								
DECLARE @Counter INT								
SET @Counter=1								
WHILE ( @Counter <= @FileCount)								
BEGIN								
-- RiDataWarehouse								
SET @PartName = @WarehouseFileGroupName + 'File' + CAST(@Counter AS VARCHAR(MAX))								
SET @FileName = @FilePath + @PartName +'.ndf'								
								
IF NOT EXISTS (								
SELECT 1 FROM sys.database_files WHERE name = @PartName								
)								
BEGIN								
SET @Query = 'ALTER DATABASE [' + @DatabaseName + ']								
ADD FILE (NAME = [' + @PartName + '],								
FILENAME = ''' + @FileName + ''',								
SIZE = 64 MB,								
MAXSIZE = 1 TB,								
FILEGROWTH = 64 MB								
) TO FILEGROUP [' + @WarehouseFileGroupName + ']'								
EXEC (@Query);								
END								
								
SET @Counter  = @Counter  + 1								
END								
								
DECLARE TreatyCodeCursor CURSOR FOR								
SELECT								
DISTINCT tc.Code								
FROM TreatyCodes AS tc								
LEFT JOIN Treaties AS t ON								
t.Id = tc.TreatyId								
LEFT JOIN Cedants AS c ON								
c.Id = t.CedantId								
WHERE c.Code = @CedantCode								
								
OPEN TreatyCodeCursor								
								
FETCH NEXT FROM TreatyCodeCursor INTO @TreatyCode								
								
WHILE @@FETCH_STATUS = 0								
								
BEGIN								
SELECT @DashPosition = CHARINDEX('-', @TreatyCode);								
SELECT @FormattedTreatyCode = SUBSTRING(@TreatyCode, 1, @DashPosition - 1);								
								
IF NOT EXISTS (SELECT 1 FROM sys.partition_range_values AS prv								
LEFT JOIN sys.partition_functions AS pf ON								
prv.function_id = pf.function_id								
WHERE pf.name = @WarehousePartitionFunctionName AND								
prv.value = @FormattedTreatyCode								
)								
BEGIN								
SET @Query = 'ALTER PARTITION SCHEME ' + @WarehousePartitionSchemeName + ' NEXT USED ' + @WarehouseFileGroupName;								
EXEC(@Query)								
								
SET @Query = 'ALTER PARTITION FUNCTION ' + @WarehousePartitionFunctionName + '() SPLIT RANGE (''' + @FormattedTreatyCode + ''')';								
EXEC(@Query)								
END								
								
FETCH NEXT FROM TreatyCodeCursor INTO @TreatyCode								
END								
								
CLOSE TreatyCodeCursor								
								
DEALLOCATE TreatyCodeCursor								
								
FETCH NEXT FROM CedantCursor INTO @CedantCode								
END								
								
CLOSE CedantCursor								
								
DEALLOCATE CedantCursor								
								
								
-- RiDataWarehouse								
IF NOT EXISTS (SELECT 1 FROM sys.partitions p								
JOIN sys.objects o ON o.object_id = p.object_id								
JOIN sys.indexes i ON p.object_id = i.object_id and p.index_id = i.index_id								
JOIN sys.data_spaces ds ON i.data_space_id = ds.data_space_id								
JOIN sys.partition_schemes ps ON ds.data_space_id = ps.data_space_id								
JOIN sys.partition_functions pf ON ps.function_id = pf.function_id								
WHERE o.name = @TableName								
AND ps.name = @WarehousePartitionSchemeName								
AND pf.name = @WarehousePartitionFunctionName								
)								
BEGIN								
BEGIN TRANSACTION								
								
SET ANSI_PADDING ON								

EXEC('CREATE CLUSTERED INDEX [ClusteredIndex_on_TreatyCodeRiDataWarehouseHistoryScheme_638054914144358106] ON ' + @TableName + ' ([TreatyCode])WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [TreatyCodeRiDataWarehouseHistoryScheme]([TreatyCode])	')
	
EXEC('DROP INDEX [ClusteredIndex_on_TreatyCodeRiDataWarehouseHistoryScheme_638054914144358106] ON ' + @TableName)
							
								
COMMIT TRANSACTION								
END								