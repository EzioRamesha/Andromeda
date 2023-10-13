DECLARE
    @Query NVARCHAR(MAX),
    @DatabaseName VARCHAR(MAX),
    @FilePath VARCHAR(MAX),
    @FileGroupName VARCHAR(MAX),
    @PartName VARCHAR(MAX),
    @FileName VARCHAR(MAX),
    @CutOffId INT,
    @PartitionFunctionName VARCHAR(50),
    @PartitionSchemeName VARCHAR(50),
    @FileCount INT

SET @DatabaseName = 'Andromeda2a'
SET @FilePath = 'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\'
SET @PartitionFunctionName = 'CutOffRiDataWarehouseHistoryFunction'
SET @PartitionSchemeName = 'CutOffRiDataWarehouseHistoryScheme'
SET @FileCount = 8

IF NOT EXISTS (SELECT 1 FROM sys.partition_functions WHERE name = @PartitionFunctionName)
BEGIN
    SET @Query = 'CREATE PARTITION FUNCTION ' + @PartitionFunctionName + '(INT) AS RANGE RIGHT FOR VALUES ()'
    EXEC(@Query)
END

IF NOT EXISTS (SELECT 1 FROM sys.partition_schemes WHERE name = @PartitionSchemeName)
BEGIN
    SET @Query = 'CREATE PARTITION SCHEME ' + @PartitionSchemeName + ' 
        AS PARTITION ' + @PartitionFunctionName + '
        TO ([primary]);'
    EXEC(@Query)
END

DECLARE CutOffCursor CURSOR FOR 
    SELECT
        Id
    FROM CutOff

OPEN CutOffCursor

FETCH NEXT FROM CutOffCursor INTO @CutOffId

WHILE @@FETCH_STATUS = 0

    BEGIN
        SET @FileGroupName = 'RiDataWH' + CAST(@CutOffId AS VARCHAR(MAX))

        IF NOT EXISTS (
        SELECT 1 FROM sys.filegroups WHERE name = @FileGroupName
        )
        BEGIN
            SET @Query = 'ALTER DATABASE ' + @DatabaseName + ' ADD FILEGROUP ' + @FileGroupName;
            EXEC (@Query);
        END

        DECLARE @Counter INT 
        SET @Counter=1
        WHILE ( @Counter <= @FileCount)
        BEGIN
            -- RiData
            SET @PartName = @FileGroupName + 'File' + CAST(@Counter AS VARCHAR(MAX))
            SET @FileName = @FilePath + @PartName +'.ndf'

            IF NOT EXISTS (
                SELECT 1 FROM sys.database_files WHERE name = @PartName
            )
            BEGIN
                SET @Query = 'ALTER DATABASE ' + @DatabaseName + ' 
                    ADD FILE (NAME = [' + @PartName + '], 
                                FILENAME = ''' + @FileName + ''', 
                                SIZE = 64 MB, 
                                MAXSIZE = 1 TB, 
                                FILEGROWTH = 64 MB
                            ) TO FILEGROUP [' + @FileGroupName + ']'
                EXEC (@Query);
            END

            SET @Counter  = @Counter  + 1
        END

        IF NOT EXISTS (SELECT 1 FROM sys.partition_range_values AS prv 
            LEFT JOIN sys.partition_functions AS pf ON
            prv.function_id = pf.function_id
            WHERE pf.name = @PartitionFunctionName AND
            prv.value = @CutOffId
        )
        BEGIN
            SET @Query = 'ALTER PARTITION SCHEME ' + @PartitionSchemeName + ' NEXT USED ' + @FileGroupName;
            EXEC(@Query)

            SET @Query = 'ALTER PARTITION FUNCTION ' + @PartitionFunctionName + '() SPLIT RANGE (' + CAST(@CutOffId AS VARCHAR(MAX)) + ')';
            EXEC(@Query)
        END

        FETCH NEXT FROM CutOffCursor INTO @CutOffId
    END

CLOSE CutOffCursor

DEALLOCATE CutOffCursor

IF NOT EXISTS (SELECT 1 FROM sys.partitions p
    JOIN sys.objects o ON o.object_id = p.object_id
    JOIN sys.indexes i ON p.object_id = i.object_id and p.index_id = i.index_id
    JOIN sys.data_spaces ds ON i.data_space_id = ds.data_space_id
    JOIN sys.partition_schemes ps ON ds.data_space_id = ps.data_space_id
    JOIN sys.partition_functions pf ON ps.function_id = pf.function_id
    WHERE o.name = 'RiDataWarehouseHistories'
    AND ps.name = @PartitionSchemeName 
    AND pf.name = @PartitionFunctionName
)
BEGIN
    BEGIN TRANSACTION
    ALTER TABLE [dbo].[Mfrs17ReportingDetailRiDatas] DROP CONSTRAINT [FK_dbo.Mfrs17ReportingDetailRiDatas_dbo.RiDataWarehouseHistories_RiDataWarehouseHistoryId]

    ALTER TABLE [dbo].[PerLifeAggregationDetailData] DROP CONSTRAINT [FK_dbo.PerLifeAggregationDetailData_dbo.RiDataWarehouseHistories_RiDataWarehouseHistoryId]

    ALTER TABLE [dbo].[RiDataWarehouseHistories] DROP CONSTRAINT [PK_dbo.RiDataWarehouseHistories] WITH ( ONLINE = OFF )

    ALTER TABLE [dbo].[RiDataWarehouseHistories] ADD  CONSTRAINT [PK_dbo.RiDataWarehouseHistories] PRIMARY KEY NONCLUSTERED 
    (
        [Id] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]

    CREATE CLUSTERED INDEX [ClusteredIndex_on_CutOffRiDataWarehouseHistoryScheme_637729331631253015] ON [dbo].[RiDataWarehouseHistories]
    (
        [CutOffId]
    )WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [CutOffRiDataWarehouseHistoryScheme]([CutOffId])

    DROP INDEX [ClusteredIndex_on_CutOffRiDataWarehouseHistoryScheme_637729331631253015] ON [dbo].[RiDataWarehouseHistories]

    ALTER TABLE [dbo].[Mfrs17ReportingDetailRiDatas]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Mfrs17ReportingDetailRiDatas_dbo.RiDataWarehouseHistories_RiDataWarehouseHistoryId] FOREIGN KEY([RiDataWarehouseHistoryId])
    REFERENCES [dbo].[RiDataWarehouseHistories] ([Id])
    ALTER TABLE [dbo].[Mfrs17ReportingDetailRiDatas] CHECK CONSTRAINT [FK_dbo.Mfrs17ReportingDetailRiDatas_dbo.RiDataWarehouseHistories_RiDataWarehouseHistoryId]

    ALTER TABLE [dbo].[PerLifeAggregationDetailData]  WITH CHECK ADD  CONSTRAINT [FK_dbo.PerLifeAggregationDetailData_dbo.RiDataWarehouseHistories_RiDataWarehouseHistoryId] FOREIGN KEY([RiDataWarehouseHistoryId])
    REFERENCES [dbo].[RiDataWarehouseHistories] ([Id])
    ALTER TABLE [dbo].[PerLifeAggregationDetailData] CHECK CONSTRAINT [FK_dbo.PerLifeAggregationDetailData_dbo.RiDataWarehouseHistories_RiDataWarehouseHistoryId]

    COMMIT TRANSACTION
END
