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
    @PartitionFunctionName VARCHAR(50),
    @PartitionSchemeName VARCHAR(50),
    @FileCount INT,
    @WarehousePartitionFunctionName VARCHAR(50),
    @WarehousePartitionSchemeName VARCHAR(50),
    @WarehouseFileGroupName VARCHAR(MAX)

SET @DatabaseName = 'Andromeda2a'
SET @FilePath = 'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\'
SET @PartitionFunctionName = 'TreatyCodeRiDataFunction'
SET @PartitionSchemeName = 'TreatyCodeRiDataScheme'
SET @WarehousePartitionFunctionName = 'TreatyCodeRiDataWarehouseFunction'
SET @WarehousePartitionSchemeName = 'TreatyCodeRiDataWarehouseScheme'
SET @FileCount = 3

-- RiData
IF NOT EXISTS (SELECT 1 FROM sys.partition_functions WHERE name = @PartitionFunctionName)
BEGIN
    SET @Query = 'CREATE PARTITION FUNCTION ' + @PartitionFunctionName + '(nvarchar(35)) AS RANGE RIGHT FOR VALUES ()'
    EXEC(@Query)
END

IF NOT EXISTS (SELECT 1 FROM sys.partition_schemes WHERE name = @PartitionSchemeName)
BEGIN
    SET @Query = 'CREATE PARTITION SCHEME ' + @PartitionSchemeName + ' 
        AS PARTITION ' + @PartitionFunctionName + '
        TO ([primary]);'
    EXEC(@Query)
END

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
        -- RiData
        SET @FileGroupName = 'RiData' + @CedantCode

        IF NOT EXISTS (
        SELECT 1 FROM sys.filegroups WHERE name = @FileGroupName
        )
        BEGIN
            SET @Query = 'ALTER DATABASE ' + @DatabaseName + ' ADD FILEGROUP ' + @FileGroupName;
            EXEC (@Query);
        END

        -- RiDataWarehouse
        SET @WarehouseFileGroupName = 'RiDataWarehouse' + @CedantCode

        IF NOT EXISTS (
        SELECT 1 FROM sys.filegroups WHERE name = @WarehouseFileGroupName
        )
        BEGIN
            SET @Query = 'ALTER DATABASE ' + @DatabaseName + ' ADD FILEGROUP ' + @WarehouseFileGroupName;
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
                SET @Query = 'ALTER DATABASE ' + @DatabaseName + ' 
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
                    WHERE pf.name = @PartitionFunctionName AND
                    prv.value = @FormattedTreatyCode
                )
                BEGIN
                    SET @Query = 'ALTER PARTITION SCHEME ' + @PartitionSchemeName + ' NEXT USED ' + @FileGroupName;
                    EXEC(@Query)

                    SET @Query = 'ALTER PARTITION FUNCTION ' + @PartitionFunctionName + '() SPLIT RANGE (''' + @FormattedTreatyCode + ''')';
                    EXEC(@Query)
                END

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

-- RiData
IF NOT EXISTS (SELECT 1 FROM sys.partitions p
    JOIN sys.objects o ON o.object_id = p.object_id
    JOIN sys.indexes i ON p.object_id = i.object_id and p.index_id = i.index_id
    JOIN sys.data_spaces ds ON i.data_space_id = ds.data_space_id
    JOIN sys.partition_schemes ps ON ds.data_space_id = ps.data_space_id
    JOIN sys.partition_functions pf ON ps.function_id = pf.function_id
    WHERE o.name = 'RiData'
    AND ps.name = @PartitionSchemeName 
    AND pf.name = @PartitionFunctionName
)
BEGIN
    BEGIN TRANSACTION

    ALTER TABLE [dbo].[RiData] DROP CONSTRAINT [PK_dbo.RiData] WITH ( ONLINE = OFF )


    ALTER TABLE [dbo].[RiData] ADD  CONSTRAINT [PK_dbo.RiData] PRIMARY KEY NONCLUSTERED 
    (
        [Id] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]


    SET ANSI_PADDING ON

    CREATE CLUSTERED INDEX [ClusteredIndex_on_TreatyCodeRiDataScheme_637617764906209264] ON [dbo].[RiData]
    (
        [TreatyCode]
    )WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [TreatyCodeRiDataScheme]([TreatyCode])


    DROP INDEX [ClusteredIndex_on_TreatyCodeRiDataScheme_637617764906209264] ON [dbo].[RiData]

    COMMIT TRANSACTION
END

-- RiDataWarehouse
IF NOT EXISTS (SELECT 1 FROM sys.partitions p
    JOIN sys.objects o ON o.object_id = p.object_id
    JOIN sys.indexes i ON p.object_id = i.object_id and p.index_id = i.index_id
    JOIN sys.data_spaces ds ON i.data_space_id = ds.data_space_id
    JOIN sys.partition_schemes ps ON ds.data_space_id = ps.data_space_id
    JOIN sys.partition_functions pf ON ps.function_id = pf.function_id
    WHERE o.name = 'RiDataWarehouse'
    AND ps.name = @WarehousePartitionSchemeName 
    AND pf.name = @WarehousePartitionFunctionName
)
BEGIN
    BEGIN TRANSACTION

    ALTER TABLE [dbo].[ClaimRegister] DROP CONSTRAINT [FK_dbo.ClaimRegister_dbo.RiDataWarehouse_RiDataWarehouseId]


    ALTER TABLE [dbo].[ClaimRegisterHistories] DROP CONSTRAINT [FK_dbo.ClaimRegisterHistories_dbo.RiDataWarehouse_RiDataWarehouseId]


    ALTER TABLE [dbo].[ReferralClaims] DROP CONSTRAINT [FK_dbo.ReferralClaims_dbo.RiDataWarehouse_RiDataWarehouseId]


    ALTER TABLE [dbo].[RiDataWarehouseHistories] DROP CONSTRAINT [FK_dbo.RiDataWarehouseHistories_dbo.RiDataWarehouse_RiDataWarehouseId]

    ALTER TABLE [dbo].[RiDataWarehouse] DROP CONSTRAINT [PK_dbo.RiDataWarehouse] WITH ( ONLINE = OFF )

    ALTER TABLE [dbo].[RiDataWarehouse] ADD  CONSTRAINT [PK_dbo.RiDataWarehouse] PRIMARY KEY NONCLUSTERED 
    (
        [Id] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]

    SET ANSI_PADDING ON

    CREATE CLUSTERED INDEX [ClusteredIndex_on_TreatyCodeRiDataWarehouseScheme_637722284477101793] ON [dbo].[RiDataWarehouse]
    (
        [TreatyCode]
    )WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [TreatyCodeRiDataWarehouseScheme]([TreatyCode])

    DROP INDEX [ClusteredIndex_on_TreatyCodeRiDataWarehouseScheme_637722284477101793] ON [dbo].[RiDataWarehouse]

    ALTER TABLE [dbo].[ClaimRegister]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ClaimRegister_dbo.RiDataWarehouse_RiDataWarehouseId] FOREIGN KEY([RiDataWarehouseId])
    REFERENCES [dbo].[RiDataWarehouse] ([Id])
    ALTER TABLE [dbo].[ClaimRegister] CHECK CONSTRAINT [FK_dbo.ClaimRegister_dbo.RiDataWarehouse_RiDataWarehouseId]

    ALTER TABLE [dbo].[ClaimRegisterHistories]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ClaimRegisterHistories_dbo.RiDataWarehouse_RiDataWarehouseId] FOREIGN KEY([RiDataWarehouseId])
    REFERENCES [dbo].[RiDataWarehouse] ([Id])
    ALTER TABLE [dbo].[ClaimRegisterHistories] CHECK CONSTRAINT [FK_dbo.ClaimRegisterHistories_dbo.RiDataWarehouse_RiDataWarehouseId]

    ALTER TABLE [dbo].[ReferralClaims]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ReferralClaims_dbo.RiDataWarehouse_RiDataWarehouseId] FOREIGN KEY([RiDataWarehouseId])
    REFERENCES [dbo].[RiDataWarehouse] ([Id])
    ALTER TABLE [dbo].[ReferralClaims] CHECK CONSTRAINT [FK_dbo.ReferralClaims_dbo.RiDataWarehouse_RiDataWarehouseId]

    ALTER TABLE [dbo].[RiDataWarehouseHistories]  WITH CHECK ADD  CONSTRAINT [FK_dbo.RiDataWarehouseHistories_dbo.RiDataWarehouse_RiDataWarehouseId] FOREIGN KEY([RiDataWarehouseId])
    REFERENCES [dbo].[RiDataWarehouse] ([Id])
    ALTER TABLE [dbo].[RiDataWarehouseHistories] CHECK CONSTRAINT [FK_dbo.RiDataWarehouseHistories_dbo.RiDataWarehouse_RiDataWarehouseId]

    COMMIT TRANSACTION
END