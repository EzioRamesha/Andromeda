
/*
SELECT COUNT(*) as 'Total'
FROM [RiData];
*/

/*
SELECT [FinaliseStatus], COUNT(*) as 'Total'
FROM [RiData]
WHERE [RiDataBatchId] = 1415
GROUP BY [FinaliseStatus];
*/

/*
SELECT COUNT(*) as 'Total'
FROM [RiData]
WHERE [RiDataBatchId] IN (1159);
*/

/*
SELECT [RiDataBatchId], COUNT(*) as 'Total'
FROM [RiData]
GROUP BY [RiDataBatchId];
*/

/*
SELECT [RiDataBatchId], COUNT(*) as 'Total'
FROM [RiData]
WHERE [RiDataBatchId] IN (1340)
GROUP BY [RiDataBatchId];
*/

/*
SELECT
	[RiDataFileId], COUNT(*) as 'Total'
FROM [RiData]
GROUP BY [RiDataFileId];
*/

/* to find those batch status is finalised but the ri data finalise status is pending */
/*
SELECT [RiData].[RiDataBatchId], COUNT([RiData].[Id]) as 'Total'
FROM [RiData]
LEFT JOIN [RiDataBatches] ON [RiDataBatches].[Id] = [RiData].[RiDataBatchId]
WHERE [RiDataBatches].[Status] = 8
AND [RiData].[FinaliseStatus] = 1
GROUP BY [RiDataBatchId];
*/