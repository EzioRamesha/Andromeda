
/*
SELECT
	[RiDataFiles].[Id] as 'RiDataFileId'
	,[RiDataFiles].[RiDataBatchId]
	,[RawFiles].[FileName]
	,[RawFiles].[HashFileName]
	,[RiDataFiles].[Status]
	,[RiDataFiles].[RawFileId]
	,[RiDataFiles].[TreatyCodeId]
	,[RiDataFiles].[RiDataConfigId]
	,[RiDataFiles].[Configs]
	,[RiDataFiles].[OverrideProperties]
	,[RiDataFiles].[Mode]
	,[RiDataFiles].[TreatyId]
	,[RiDataFiles].[Errors]
FROM [RiDataFiles]
LEFT JOIN [RawFiles] ON [RawFiles].[Id] = [RiDataFiles].[RawFileId]
WHERE [RiDataFiles].[RiDataBatchId] IN (38);
*/

SELECT
	[RiDataFiles].[Id] as 'RiDataFileId'
	,[RiDataFiles].[RiDataBatchId]
	,[RiDataFiles].[RiDataConfigId]
	,[RawFiles].[FileName]
	,[RawFiles].[HashFileName]
	,[RiDataFiles].[Status]
	,[RiDataFiles].[RawFileId]
	,[RiDataFiles].[TreatyCodeId]
	,[RiDataFiles].[RiDataConfigId]
	,[RiDataFiles].[Configs]
	,[RiDataFiles].[OverrideProperties]
	,[RiDataFiles].[Mode]
	,[RiDataFiles].[TreatyId]
	,[RiDataFiles].[Errors]
FROM [RiDataFiles]
LEFT JOIN [RawFiles] ON [RawFiles].[Id] = [RiDataFiles].[RawFileId]
WHERE [RiDataFiles].[Id] IN (1264);
