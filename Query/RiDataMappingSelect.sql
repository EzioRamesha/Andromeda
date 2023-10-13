
SELECT TOP (1000) [Id]
    ,[RiDataConfigId]
    ,[StandardOutputId]
    ,[SortIndex]
    ,[Row]
    ,[RawColumnName]
    ,[Length]
    ,[TransformFormula]
    ,[DefaultValue]
    ,[DefaultObjectId]
FROM [RiDataMappings]
WHERE [RiDataConfigId] = 13
AND [StandardOutputId] = 2;
